using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using App.Model;

namespace App.ViewModel

{
    public class States
	{
		private static readonly Langue _Langue = new Langue();
		//AddTask écrit dans le fichier 
		public static string AddTask(string name, string sourcePath, string destinationPath, string backupType)
		{
			// Check there are less than five tasks
			if (!_LessThanFive())
            {
				return _Langue.Translation(4);
            }
			// Check paths in parameters are good
			string c = _CheckParametersPath(sourcePath, destinationPath);
			if(c != "OK")
            {
				return c;
            }
			// Get TotalFileSize
			var files = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);
			var totalFileSize = 0;
			foreach(string file in files)
            {
				totalFileSize += file.Length;
            }
			string id = _AddId();
			var newTask = new ItemStateClass()
			{
				Name = name,
				Id = id,
				FileSource = sourcePath,
				FileTarget = destinationPath,
				State = "Inactive",
				BackupType = backupType,
				FileSize = sourcePath.Length.ToString(),
				TotalFilesToCopy = files.Length.ToString(),
				TotalFileSize = totalFileSize.ToString(),
				NbFilesLeftToDo = files.Length.ToString(),
				Progression = "0"
			};
			var tableauObjets = JsonStateLog.Read();
			tableauObjets = new List<ItemStateClass>(tableauObjets) { newTask }.ToArray();
			JsonStateLog.Write(tableauObjets);
			return _Langue.Translation(28);
		}

		public static string ReadStatus()
		{
			if (!JsonStateLog.IsFileExist())
            {
				JsonStateLog.Write();
				return _Langue.Translation(27);
            }
			var r = JsonStateLog.Read();
			if (r.Length == 0)
            {
				return _Langue.Translation(27);
            }
			var readFormat = "";
			foreach(ItemStateClass element in r)
            {
				readFormat += $"{_Langue.Translation(29)}{element.Id}" + "\n"
					+ $"{_Langue.Translation(10)}{element.Name}" + "\n"
					+ $"{_Langue.Translation(11)}{element.FileSource}" + "\n"
					+ $"{_Langue.Translation(12)}{element.FileTarget}" + "\n"
					+ $"{_Langue.Translation(13)}{element.BackupType}" + "\n"
					+ $"{_Langue.Translation(14)}{element.State}" + "\n"
					+ $"{_Langue.Translation(15)}{element.TotalFilesToCopy}" + "\n"
					+ $"{_Langue.Translation(16)}{element.NbFilesLeftToDo}" + "\n";
				readFormat += "----------------" + "\n";
			}
			return readFormat;
		}

		// DeleteStatus supprime la save si elle existe et met à jour les id
		public static string DeleteStatus(int saveId)
		{
			if(_isExist(saveId)){
				var r = JsonStateLog.Read();
				List<ItemStateClass> list = new List<ItemStateClass>(r);
				list.RemoveAll(element => element.Id == saveId.ToString());
				var newTab = list.ToArray();
				foreach(ItemStateClass e in newTab)
                {
					var id = Int32.Parse(e.Id);
					if(id > saveId)
                    {
						var newId = id - 1;
						e.Id = newId.ToString();
                    }
                }
				JsonStateLog.Write(list.ToArray());
				return _Langue.Translation(30);
			}
			return _Langue.Translation(23);
		}

		// Update status of taks during save
		public static void UpdateStatus(string id, int progression, int nbFilesLeft, string state)
		{
			var r = JsonStateLog.Read();
			foreach(ItemStateClass e in r)
            {
				if(e.Id == id)
                {
					e.Progression = progression.ToString();
					e.NbFilesLeftToDo = nbFilesLeft.ToString();
					e.State = state;
                }
            }
			JsonStateLog.Write(r);
		}

		// isExists renvoie vrai si la save existe
		private static bool _isExist(int saveId)
		{
			var id = saveId.ToString();
			var r = JsonStateLog.Read();
			foreach(ItemStateClass element in r)
            {
				if (element.Id == id)
                {
					return true;
                }
            }
			return false;
		}

		// Check that there are less than 5 tasks
		private static bool _LessThanFive()
        {
			return JsonStateLog.Read().Length < 5;
		}

		// Check sourcePath and destinationPath are valid
		private static string _CheckParametersPath(string sourcePath, string destinationPath)
        {
			if (!Directory.Exists(sourcePath) && !File.Exists(sourcePath))
            {
				return _Langue.Translation(7);
            }
			if (!Directory.Exists(destinationPath) && !File.Exists(destinationPath))
            {
				return _Langue.Translation(8);
            }
			return "OK";
        }

		// Add an id when create a task
		private static string _AddId()
        {
			var r = JsonStateLog.Read();

			var newId = 1;
			if (!(r.Length == 0))
            {
				var lastId = r.Last().Id;

				newId = Int32.Parse(lastId) + 1;
			}

			return newId.ToString();
        }
	}
}

