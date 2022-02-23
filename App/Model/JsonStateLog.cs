using System;
using System.IO;
using System.Text.Json;

namespace App.Model
{
    //writing 
    public static class JsonStateLog
    {
        public static ItemStateClass[] Read()
        {
            string jsonString = File.ReadAllText(Settings.PathStates);
            var read = JsonSerializer.Deserialize<ItemStateClass[]>(jsonString);

            return read;
        }

        public static void Write( params ItemStateClass[] itemToWrite)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(itemToWrite, options);
                File.WriteAllText(Settings.PathStates, jsonString);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
        public static void Write(ItemLogClass itemToWrite)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(itemToWrite, options) + "\n";
                File.AppendAllText(Settings.PathLog + itemToWrite.Date + ".Json", jsonString);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        public static bool IsFileExist()
        {
            return File.Exists(Settings.PathStates);
        }
    }
}