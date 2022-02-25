using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using WpfApp.Model;

namespace WpfApp.ViewModel
{
    public delegate void progresse(int subject);
    public class Save
    {
        private List<Thread> task = new List<Thread>(); 
        public List<bool> ListBool = new List<bool>();
        // Subject for progress
        public int Progress { get; set; } = 0;
        private readonly Langue _lang = new Langue();
        static Barrier barrier = new Barrier(participantCount: 0);
        public int GlobalCount = 0;
        public int GlobalFileNB = 0;

        public string MakeASave(string taskNumbers, progresse progre)
        //Handles all the logic of a backup
        {
            GlobalCount = 0;
            GlobalFileNB = 0;
            string[] listTaskNumbers = taskNumbers.Split(';');

            //Check if threr is a thread in the barrier. If yes, remove it.
            if (barrier.ParticipantCount > 0)
            {
                barrier.RemoveParticipants(barrier.ParticipantCount);
            }

            Setting param = new Setting();

            //For each task given, create a thread assign it a task
            foreach (string taskNumber in listTaskNumbers)
            {
                int idTaskNumber = int.Parse(taskNumber);
                ItemStateClass[] r = JsonStateLog.Read(param.PathStates()); //Recover the states information in the states file
                Thread temp = new Thread(() => DoASave(r, idTaskNumber, progre, param));
                temp.Start();
                task.Add(temp);
                barrier.AddParticipant();
            }
            //Check if a the buisness sofware is on
            Thread BusinessSoft = new Thread(() => PauseBusinessSoft(param));
            BusinessSoft.Start();
            while (task.Count != 0) { }
            return "\n" + _lang.Translation(31);

        }

        private void DoASave(ItemStateClass[] r, int idTaskNumber, progresse progre, Setting param)
        {
            //Set the maximum number of thread simultaneously
            ThreadPool.SetMaxThreads(int.Parse(param.LimitThread()), int.Parse(param.LimitThread()));

            //Define local variables for processing
            string source = "";
            string target = "";
            string idToSave = "";
            string BackupType = "";
            string nbFileLeftToDo = "";
            string saveName = "";
            string encrypt = "";

            //Fill ListBool of true, to give it a length equal to the number of treads
            foreach (ItemStateClass item in JsonStateLog.Read(param.PathStates()))
            {
                ListBool.Add(true);
            }

            // Verify if the task Number enter is correct and store source and target
            foreach (ItemStateClass element in r)
            {
                if (int.Parse(element.Id) == idTaskNumber)
                {//Give characteristics of a task to the locals variables
                    source = element.FileSource;
                    target = element.FileTarget;
                    BackupType = element.BackupType;
                    idToSave = element.Id;
                    nbFileLeftToDo = element.NbFilesLeftToDo;
                    saveName = element.Name;
                    encrypt = element.Encrypt;
                    Thread.CurrentThread.Name = element.Id;
                    int sizeMaxTransfert = 0;
                    //Test if limiter of n KO is empty or not
                    if (param.LimitSize() == "")
                    {
                        sizeMaxTransfert = int.Parse(element.TotalFileSize) + 1;
                    }
                    else
                    {
                        sizeMaxTransfert = int.Parse(param.LimitSize()) * 1000;
                    }

                    //The ListBool represents an array with a thread state at each position (according to its name (task ID)).
                    //If a file is larger than the limit, a false will be put in the array, at the position of the task.
                    //If a false is already present in the array, and there is a file with a size greater than the limit.
                    //Then we block this thread, until the other is free.
                    if (int.Parse(element.TotalFileSize) > sizeMaxTransfert)
                    {
                        if (ListBool.Contains(false))
                        {
                            while (true)
                            {
                            }
                        }
                        ListBool[int.Parse(element.Id) - 1] = false;
                    }
                    else
                    {
                        ListBool[int.Parse(element.Id) - 1] = true;
                    }
                    break;
                }
            }


            //Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(source, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(source, target));
            }
            //Copy all the files & Replaces any files with the same name
            string[] SourcesFilesPath = Directory.GetFiles(source, "*.*", SearchOption.AllDirectories);

            GlobalFileNB += SourcesFilesPath.Count();
            barrier.SignalAndWait();

            int count = 0;

            string[] priorityList = param.FilesPrio();
            string[] whiteList = param.CyptoExtension();

            List<string> PriorityFiles = new List<string>();
            List<string> NonPriorityFiles = new List<string>();

            foreach (string SourceFilePath in SourcesFilesPath)
            //Manage the priority files
            {
                if (SourceFilePath.Split('.').Length > 1)
                //if the file has an extension
                {
                    string extension = "." + SourceFilePath.Split('.').Last();
                    //Get it
                    if (priorityList.Contains(extension))
                    //If the extention is in the whitelist, add in PriorityFiles, or in NonPriorityFiles
                    {
                        PriorityFiles.Add(SourceFilePath);
                    }
                    else
                    {
                        NonPriorityFiles.Add(SourceFilePath);
                    }
                }
                else
                {
                    //Files with no extension are not whitelisted
                    NonPriorityFiles.Add(SourceFilePath);
                }
            }
            //Change the status to Active, in the state File
            States.UpdateStatus(idToSave, 0, int.Parse(nbFileLeftToDo), "Active");
            //Set the ProgressBar to 0
            progre(0);

            if (PriorityFiles.Count != 0)
            {
                count = DoByPriority(PriorityFiles, SourcesFilesPath, progre, whiteList, encrypt, BackupType, source, target, idToSave, nbFileLeftToDo, saveName, param, count);
            }
            barrier.SignalAndWait();
            DoByPriority(NonPriorityFiles, SourcesFilesPath, progre, whiteList, encrypt, BackupType, source, target, idToSave, nbFileLeftToDo, saveName, param, count);

            task.Remove(Thread.CurrentThread);
        }



        private int _copy(string SourceFilePath, string[] SourcesFilesPath, string source, string target, int count, string idToSave, string nbFileLeftToDo, string saveName, Setting param)
        //Copy function for full and differential task
        {
            //Define and start a Timer
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //Start the copy
            File.Copy(SourceFilePath, SourceFilePath.Replace(source, target), true);

            //Stop the timer and calcul the time elsapsed
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            //Increase the ProgressBar
            count += 1;
            Progress = (int)Math.Round((double)count / SourcesFilesPath.Length * 100);

            if (Progress == 100)
            {
                //If the last file was coppied, change the status to Inactive
                States.UpdateStatus(idToSave, Progress, 0, "Inactive");
            }
            else
            {
                States.UpdateStatus(idToSave, Progress, int.Parse(nbFileLeftToDo) - 1, "Active");
            }
            //Put a true in the ListBool for deliver a thread if there was a false in the ListBool
            ListBool[int.Parse(Thread.CurrentThread.Name) - 1] = true;

            _writeLog(saveName, SourceFilePath, SourceFilePath.Replace(source, target), SourceFilePath.Length, elapsedTime, 0, false, param);

            return count;

        }

        
        private int _encryptedCopy(string SourceFilePath, string[] SourcesFilesPath, string source, string target, int count, string idToSave, string nbFileLeftToDo, string saveName, int encryptTime, Setting parametre)
        //Copy function for full and differential task encrypted
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            char dbl = '\\';
            string[] tab = SourceFilePath.Split(dbl);

            //Start the CryptoSoft software
            ProcessStartInfo ProCrypto = new ProcessStartInfo(parametre.PathCrypt())
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = SourceFilePath
            };
            Process CryptoProcess = Process.Start(ProCrypto);
            CryptoProcess.WaitForExit();
            CryptoProcess.Close();

            //
            tab.ToList();
            StreamReader sr = new StreamReader("tempo_" + tab.Last().Split('.')[0] + ".txt");
            string[] lines = sr.ReadToEnd().Split('.');
            sr.Close();
            File.Delete("tempo_" + tab.Last().Split('.')[0] + ".txt");
            string TempEnd = lines[0];
            int TempCrypt = int.Parse(TempEnd);
            List<string> listContent = new List<string>(lines);
            listContent.Remove(TempEnd);
            string content = "";
            foreach (string item in listContent)
            {
                content += item;
            }

            //Create the new file crypted in the destination wanted
            StreamWriter sw = new StreamWriter(SourceFilePath.Replace(source, target));
            sw.Write(content);
            sw.Close();

            //calcul of the scrypting time
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            //Calcul the progress
            count += 1;
            Progress = (int)Math.Round((double)count / SourcesFilesPath.Length * 100);

            if (Progress == 100)
            //update the Progress bar
            {
                States.UpdateStatus(idToSave, Progress, 0, "Inactive");
            }
            else
            {
                States.UpdateStatus(idToSave, Progress, int.Parse(nbFileLeftToDo) - 1, "Active");
            }
            //Put a true in the ListBool, in case of there is a false
            ListBool[int.Parse(Thread.CurrentThread.Name) - 1] = true;

            //Write in the log file
            _writeLog(saveName, SourceFilePath, SourceFilePath.Replace(source, target), SourceFilePath.Length, elapsedTime, TempCrypt, true, parametre);
            return count;
        }


        private bool _diffentialFileCompare(string file1, string file2)
        //Make the differential comparaison of 2 files
        {
            int file1byte;
            int file2byte;
            FileStream fs1;
            FileStream fs2;

            // Determine if the same file was referenced two times.
            if (file1 == file2)
            {
                // Return true to indicate that the files are the same.
                return true;
            }

            // Open the two files.
            fs1 = new FileStream(file1, FileMode.Open);
            fs2 = new FileStream(file2, FileMode.Open);

            // Check the file sizes. If they are not the same, the files
            // are not the same.
            if (fs1.Length != fs2.Length)
            {
                // Close the file
                fs1.Close();
                fs2.Close();

                // Return false to indicate files are different
                return false;
            }

            // Read and compare a byte from each file until either a
            // non-matching set of bytes is found or until the end of
            // file1 is reached.
            do
            {
                // Read one byte from each file.
                file1byte = fs1.ReadByte();
                file2byte = fs2.ReadByte();
            }
            while ((file1byte == file2byte) && (file1byte != -1));

            // Close the files.
            fs1.Close();
            fs2.Close();

            // Return the success of the comparison. "file1byte" is
            // equal to "file2byte" at this point only if the files are
            // the same.
            return ((file1byte - file2byte) == 0);
        }

        private void _writeLog(string name, string fileSource, string fileTarget, int fileSize, string transferTime, int encrypte, bool iscrypt, Setting param)
        //Write log in log file
        {
            var newLog = new ItemLogClass()
            {
                Name = name,
                FileSource = fileSource,
                FileTarget = fileTarget,
                FileSize = fileSize.ToString(),
                FileTransferTime = transferTime,
                Date = DateTime.Now.ToString("MMMM dd, yyyy"),
                EncryptTime = encrypte,
                Encrypt = iscrypt

            };

            //Change extension for log file
            switch (param.LogExtension())
            {
                case "XML":

                    XmlStateLog.Write(newLog, param, Thread.CurrentThread.Name);

                    break;

                case "JSON":

                    JsonStateLog.Write(param, newLog, Thread.CurrentThread.Name);

                    break;

            }

        }
        public void Pause(string taskNumbers)
        //Pause a task
        {
            if (taskNumbers == "all")
            {
                foreach (Thread tache in task)
                {
                    try { 
                    tache.Suspend();
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                string[] listTaskNumbers = taskNumbers.Split(';');
                foreach (string taskNumber in listTaskNumbers)
                {
                    Setting param = new Setting();
                    int idTaskNumber = int.Parse(taskNumber);

                    ItemStateClass[] r = JsonStateLog.Read(param.PathStates());
                    foreach (ItemStateClass element in r)
                    {
                        int id = Int32.Parse(element.Id);
                        if (id == idTaskNumber)
                        {
                            foreach (Thread tache in task)
                            {
                                if (tache.Name == element.Id)
                                {
                                    tache.Suspend();
                                }
                            }
                            break;
                        }
                    }
                }

            }
        }
        public void Resume(string taskNumbers)
        //Resume a task
        {
            if (taskNumbers == "all")
            {
                foreach (Thread tache in task)
                {
                    try { 
                    tache.Resume();
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                string[] listTaskNumbers = taskNumbers.Split(';');
                foreach (string taskNumber in listTaskNumbers)
                {
                    Setting param = new Setting();
                    int idTaskNumber = Int32.Parse(taskNumber);


                    ItemStateClass[] r = JsonStateLog.Read(param.PathStates());
                    foreach (ItemStateClass element in r)
                    {
                        int id = Int32.Parse(element.Id);
                        if (id == idTaskNumber)
                        {
                            foreach (Thread tache in task)
                            {
                                if (tache.Name == element.Id)
                                {
                                    tache.Resume();
                                }
                            }
                            break;
                        }
                    }
                }

            }
        }

        public void Kill(string taskNumbers)
        //Kill a task
        {
            Setting setting = new Setting();
            if (taskNumbers == "all")
            {

                foreach (Thread tache in task)
                {
                    
                    ItemStateClass[] state = JsonStateLog.Read(setting.PathStates());
                    try
                    {
                        tache.Resume();
                    }
                    catch (System.Threading.ThreadStateException)
                    {

                    }
                    foreach(ItemStateClass item in state)
                    {
                        if (tache.Name == item.Id)
                        {
                            item.State = "Inactive";
                            break;
                        }
                    }
                    barrier.RemoveParticipant();
                    
                    tache.Abort();
                    
                    JsonStateLog.Write(setting, state);
                }
                task = new List<Thread>();
            }
            else
            {
                string[] listTaskNumbers = taskNumbers.Split(';');
                Setting param = new Setting();
                foreach (string taskNumber in listTaskNumbers)
                {
                   
                    int idTaskNumber = int.Parse(taskNumber);


                    ItemStateClass[] r = JsonStateLog.Read(param.PathStates());
                    foreach (ItemStateClass element in r)
                    {
                        int id = int.Parse(element.Id);
                        if (id == idTaskNumber)
                        {
                            foreach (Thread tache in task)
                            {
                                if (tache.Name == element.Id)
                                {
                                    element.State = "Inactive";
                                    tache.Abort();
                                }
                            }
                            break;
                        }
                    }
                    JsonStateLog.Write(setting, r);

                }

            }


        }


        public void PauseBusinessSoft(Setting param)
        //Stop a process if a buisness software is on
        {
            bool InPause = false;
            while (task.Count != 0)
            {

                if (Proces.IsExist(param.Buisnessoft()))
                {
                    Pause("all");
                    InPause = true;
                }
                else if (InPause)
                {
                    Resume("all");
                    InPause = false;
                }
            }

        }
        private int DoByPriority(List<string> FilesList, string[] SourcesFilesPath, progresse progre, string[] whiteList, string encrypt, string BackupType, string source, string target, string idToSave, string nbFileLeftToDo, string saveName, Setting param, int count)
        //Do a task by the priority extension
        {
            foreach (string SourceFilePath in FilesList)
            {
                string extension = "";

                if (SourceFilePath.Split('.').Length > 1)
                {
                    extension = "." + SourceFilePath.Split('.').Last();
                }

                if (encrypt == "True" && whiteList.Contains(extension))
                {
                    if (BackupType == "Differential" && (File.Exists(SourceFilePath.Replace(source, target))))
                    {
                        if (File.GetLastWriteTime(SourceFilePath) > File.GetLastWriteTime(SourceFilePath.Replace(source, target)))
                        {
                            if (_diffentialFileCompare(SourceFilePath, SourceFilePath.Replace(source, target)) == true)
                            {
                                count = _encryptedCopy(SourceFilePath, SourcesFilesPath, source, target, count, idToSave, nbFileLeftToDo, saveName, 0, param);

                            }
                            else
                            {
                                count++;
                            }
                        }
                        else
                        {
                            count++;
                        }
                    }
                    else
                    {
                        count = _encryptedCopy(SourceFilePath, SourcesFilesPath, source, target, count, idToSave, nbFileLeftToDo, saveName, 0, param);

                    }


                }
                else
                {
                    if (encrypt == "False" && BackupType == "Differential" && File.Exists(SourceFilePath.Replace(source, target)))
                    {
                        if (whiteList.ToList().Contains(extension) && File.GetLastWriteTime(SourceFilePath) > File.GetLastWriteTime(SourceFilePath.Replace(source, target)))
                        {
                            if (_diffentialFileCompare(SourceFilePath, SourceFilePath.Replace(source, target)) == true)
                            {
                                count = _copy(SourceFilePath, SourcesFilesPath, source, target, count, idToSave, nbFileLeftToDo, saveName, param);
                            }
                            else
                            {
                                count++;
                            }
                        }
                        else
                        {
                            count++;
                        }


                    }
                    else
                    {
                        count = _copy(SourceFilePath, SourcesFilesPath, source, target, count, idToSave, nbFileLeftToDo, saveName, param);
                    }
                }
                GlobalCount++;
                progre((int)Math.Round((double)GlobalCount / GlobalFileNB * 100));
            }
            return count;
        }
    }
}

