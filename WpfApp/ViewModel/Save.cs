using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using WpfApp.Model;

namespace WpfApp.ViewModel
{
    public delegate void progresse(int subject);

    public class Save
    {
        private List<Thread> task = new List<Thread>();
        // Subject for progress
        public int Progress { get; set; } = 0;

        private readonly Langue _lang = new Langue();
        public List<bool> ListeBoule = new List<bool>();

        public string MakeASave(string taskNumbers, progresse progre)
        {

            string[] listTaskNumbers = taskNumbers.Split(';');
            Setting param = new Setting();

            foreach (string taskNumber in listTaskNumbers)
            {
                
                int idTaskNumber = Int32.Parse(taskNumber);

                ItemStateClass[] r = JsonStateLog.Read(param.PathStates());

                Thread temp = new Thread(() => DoASave(r, idTaskNumber, progre, param));

                temp.Start();
                task.Add(temp);

            }

            Thread BusinessSoft = new Thread(()=>PauseBusinessSoft(param));
            BusinessSoft.Start();
            while (task.Count != 0) { }
            return "\n" + _lang.Translation(31);

        }

        private void DoASave(ItemStateClass[] r, int idTaskNumber, progresse progre, Setting param)
        {

            string source = "";
            string target = "";
            string idToSave = "";
            string BackupType = "";
            string nbFileLeftToDo = "";
            string sizeOfTheFile = "";
            string saveName = "";
            string encrypt = "";
            int sizeMaxTransfert = Int32.Parse(param.LimitSize())*1000;
            foreach (Thread item in task)
            {
                ListeBoule.Add(true);
            }
            // Verify taskNumber enter is correct and store source and target
            foreach (ItemStateClass element in r)
            {
                int id = Int32.Parse(element.Id);
                if (id == idTaskNumber)
                {
                    source = element.FileSource;
                    target = element.FileTarget;
                    BackupType = element.BackupType;
                    idToSave = element.Id;
                    nbFileLeftToDo = element.NbFilesLeftToDo;
                    saveName = element.Name;
                    encrypt = element.Encrypt;
                    Thread.CurrentThread.Name = element.Id;
                    if (Int32.Parse(element.TotalFileSize) > sizeMaxTransfert)
                    {
                        if (ListeBoule.Contains(false))
                        { 
                            while (true)
                            {


                            }
                        
                        }

                        ListeBoule[Int32.Parse(element.Id)-1] = false;
                    }
                    else
                    {
                        ListeBoule[Int32.Parse(element.Id)-1] = true;
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
            var SourcesFilesPath = Directory.GetFiles(source, "*.*", SearchOption.AllDirectories);
            int count = 0;

            string[] whiteList = param.CyptoExtension();
            foreach (string SourceFilePath in SourcesFilesPath)
            {

                string extension = "";
                if (SourceFilePath.Split('.').Length > 1)
                {
                    extension = "." + SourceFilePath.Split('.')[1];
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
                progre((int)Math.Round((double)count / SourcesFilesPath.Length * 100));
            }
            task.Remove(Thread.CurrentThread);
        }



        private int _copy(string SourceFilePath, string[] SourcesFilesPath, string source, string target, int count, string idToSave, string nbFileLeftToDo, string saveName, Setting param)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            File.Copy(SourceFilePath, SourceFilePath.Replace(source, target), true);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            count += 1;
            Progress = (int)Math.Round((double)count / SourcesFilesPath.Length * 100);

            if (Progress == 100)
            {
                States.UpdateStatus(idToSave, Progress, 0, "Inactive");
            }
            else
            {
                States.UpdateStatus(idToSave, Progress, Int32.Parse(nbFileLeftToDo) - 1, "Active");
            }
            ListeBoule[Int32.Parse(Thread.CurrentThread.Name)-1] = true;
            _writeLog(saveName, SourceFilePath, SourceFilePath.Replace(source, target), SourceFilePath.Length, elapsedTime, 0, false, param);

            return count;

        }

        private int _encryptedCopy(string SourceFilePath, string[] SourcesFilesPath, string source, string target, int count, string idToSave, string nbFileLeftToDo, string saveName, int encryptTime, Setting parametre)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            char dbl = '\\';
            string[] tab = SourceFilePath.Split(dbl);

            int tempBeg = 0;

            //ProcessStartInfo ProCrypto = new ProcessStartInfo("D:\\user\\projet\\programation_system\\crypto\\WpfApp\\CryptoSoft\\cryptosoft.exe");
            ProcessStartInfo ProCrypto = new ProcessStartInfo(parametre.PathCrypt());
            ProCrypto.WindowStyle = ProcessWindowStyle.Hidden;
            ProCrypto.Arguments = SourceFilePath;
            //ProCrypto.Arguments = @"C:\temp\config.json";
            Process CryptoProcess = Process.Start(ProCrypto);
            CryptoProcess.WaitForExit();
            CryptoProcess.Close();

            tab.ToList();
            StreamReader sr = new StreamReader("tempo_" + tab.Last().Split('.')[0] + ".txt");
            string[] lines = sr.ReadToEnd().Split('.');
            sr.Close();
            File.Delete("tempo_" + tab.Last().Split('.')[0] + ".txt");
            string TempEnd = lines[0];
            int TempCrypt = Int32.Parse(TempEnd);
            List<string> listContent = new List<string>(lines);
            listContent.Remove(TempEnd);
            string content = "";
            foreach (string item in listContent)
            {
                content += item;
            }

            StreamWriter sw = new StreamWriter(SourceFilePath.Replace(source, target));
            sw.Write(content);
            sw.Close();

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            count += 1;
            Progress = (int)Math.Round((double)count / SourcesFilesPath.Length * 100);
            if (Progress == 100)
            {
                States.UpdateStatus(idToSave, Progress, 0, "Inactive");
            }
            else
            {
                States.UpdateStatus(idToSave, Progress, Int32.Parse(nbFileLeftToDo) - 1, "Active");
            }
            ListeBoule[Int32.Parse(Thread.CurrentThread.Name) - 1] = true;
            _writeLog(saveName, SourceFilePath, SourceFilePath.Replace(source, target), SourceFilePath.Length, elapsedTime, TempCrypt, true, parametre);
            return count;
        }


        private bool _diffentialFileCompare(string file1, string file2)
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
        public void pause(string taskNumbers) {
            if (taskNumbers == "all")
            {
                foreach (Thread tache in task)
                {
                    tache.Suspend();
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
                                    tache.Suspend();
                                }
                            }




                            break;
                        }
                    }
                }

            }
        }
        public void resume(string taskNumbers)
        {
            if (taskNumbers == "all")
            {
                foreach (Thread tache in task)
                {
                    tache.Resume();
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

        public void kill(string taskNumbers)
        {
            if (taskNumbers == "all")
            {
                
                foreach (Thread tache in task)
                {
                    Setting setting = new Setting();
                    ItemStateClass[] state= JsonStateLog.Read(setting.PathStates());
                    try
                    {
                        tache.Resume();
                    }
                    catch (System.Threading.ThreadStateException e)
                    {

                    }
                    tache.Abort();
                    JsonStateLog.Write(setting, state);
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
                                    tache.Abort();
                                }
                            }
                            break;
                        }
                    }
                }

            }


        }


        public void PauseBusinessSoft(Setting param)
        {
            bool InPause=false;
            while(task.Count!=0)
            {

                if (Proces.isexist(param.Buisnessoft()))
                {
                    pause("all");
                    InPause = true;
                }
                else if(InPause)
                {
                    resume("all");
                    InPause = false;
                }
            }

        }









    }
}

