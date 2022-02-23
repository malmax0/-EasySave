using System.IO;
using System.Text.Json;

namespace WpfApp.Model
{ 
    //setting class
    public static class Config
    {
        static string _Path="./Setting.json";


        public static void Write(ItemSettings param)
        { 
            //Pass the filepath and filename to the StreamWriter Constructor
            StreamWriter file = new StreamWriter(_Path);
            //Write a line of text

            JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };
            file.WriteLine(JsonSerializer.Serialize(param, options));

            //Close the file
            file.Close();
         }

		public static ItemSettings Read()
        {
            //Pass the file path and file name to the StreamReader constructor
            StreamReader file = new StreamReader(_Path);
            //return settings
            string data = file.ReadToEnd();
            file.Close();
            return JsonSerializer.Deserialize<ItemSettings>(data);
        }
    }
	    
}
