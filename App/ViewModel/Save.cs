using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using App.Model;
using App.Interfaces;

namespace App.ViewModel
{

    public class Save : ISubject
    {
        // Subject for progress
        public int Progress { get; set; } = 0;

        private readonly Langue _lang = new Langue();

        private readonly List<IObserver> _observers = new List<IObserver>();

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        // Trigger an update in each subscriber.
        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update(this);
            }
        }

        public string MakeASave(string taskNumbers)
        {
            
            var listTaskNumbers = taskNumbers.Split(';');
            foreach (string taskNumber in listTaskNumbers)
            {
                int idTaskNumber = Int32.Parse(taskNumber);

                var r = JsonStateLog.Read();

                string source = "";
                string target = "";
                string BackupType = "";
                string idToSave = "";
                string nbFileLeftToDo = "";
                string saveName = "";


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
                        break;
                    }
                    else if (id == r.Length)
                    {
                        return _lang.Translation(26);
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
                string[] whiteList = {".txt",".json",".cs",".html",".php",".css" };
                foreach (string SourceFilePath in SourcesFilesPath)
                {
                    if (BackupType == "Differential")
                    {
                         if (whiteList.ToList().Contains(SourceFilePath.Split('\\').Last()) && !(File.Exists(SourceFilePath.Replace(source, target))) && File.GetLastWriteTime(SourceFilePath)> File.GetLastWriteTime(SourceFilePath.Replace(source, target))) 
                         {
                             if (_diffentialFileCompare(SourceFilePath, SourceFilePath.Replace(source, target)) == true)
                             {
                                continue;
                             }
                         }
                         else if (!(File.Exists(SourceFilePath.Replace(source, target))) && File.GetLastWriteTime(SourceFilePath) <= File.GetLastWriteTime(SourceFilePath.Replace(source, target)))
                        {
                            continue;
                        }
                    }
                    count = _copy(SourceFilePath, SourcesFilesPath, source, target, count, idToSave, nbFileLeftToDo, saveName);
                }
            }
            return "\n"+_lang.Translation(31);
        }
        private int _copy(string SourceFilePath,string[] SourcesFilesPath, string source, string target,int  count,string idToSave,string nbFileLeftToDo,string saveName)
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
            _writeLog(saveName, SourceFilePath, SourceFilePath.Replace(source, target), SourceFilePath.Length, elapsedTime);
            Notify();
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

        private void _writeLog(string name, string fileSource, string fileTarget, int fileSize, string transferTime)
        {
            var newLog = new ItemLogClass()
            {
                Name = name,
                FileSource = fileSource,
                FileTarget = fileTarget,
                FileSize = fileSize.ToString(),
                FileTransferTime = transferTime,
                Date = DateTime.Now.ToString("MMMM dd, yyyy")
            };
            JsonStateLog.Write(newLog);
        }
    }
}

