using System;
using System.IO;
using System.Text.Json;

namespace WpfApp.Model
{
    //writing 
    public static class JsonStateLog
    {

        public static ItemStateClass[] Read()
        {
            Setting setting = new Setting();
            string jsonString = File.ReadAllText(setting.PathStates());
            var read = JsonSerializer.Deserialize<ItemStateClass[]>(jsonString);

            return read;
        }

        public static void Write( params ItemStateClass[] itemToWrite)
        {
            Setting setting = new Setting();
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(itemToWrite, options);
                File.WriteAllText(setting.PathStates(), jsonString);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
        public static void Write(ItemLogClass itemToWrite)
        {
            Setting setting = new Setting();
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(itemToWrite, options) + "\n";
                File.AppendAllText(setting.PathLog() + itemToWrite.Date + ".Json", jsonString);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        public static bool IsFileExist()
        {
            Setting setting = new Setting();
            return File.Exists(setting.PathStates());
        }
    }
}