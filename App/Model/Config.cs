using System.IO;
using System.Text.Json;

namespace App.Model
{ 
    //setting class
    public static class Config
    {
        static string _Path="./Setting.json";


        public static void Write(params string[] data)
        { 
            //Pass the filepath and filename to the StreamWriter Constructor
            StreamWriter file = new StreamWriter(_Path);
            //Write a line of text
            Settings param =new Settings();
            param.Langue = data[0];
            param.LogExtension = data[1];
            JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };
            file.WriteLine(JsonSerializer.Serialize(param, options));

            //Close the file
            file.Close();
         }

		public static Settings Read()
        {
            //Pass the file path and file name to the StreamReader constructor
            StreamReader file = new StreamReader(_Path);
            //return settings
            string data = file.ReadToEnd();
            file.Close();
            return JsonSerializer.Deserialize<Settings>(data);
        }
    }
	    
}
