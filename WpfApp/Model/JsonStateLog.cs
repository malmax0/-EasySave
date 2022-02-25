using System;
using System.IO;
using System.Text.Json;

namespace WpfApp.Model
{
    //writing 
    public static class JsonStateLog
    {

        public static ItemStateClass[] Read(string path)
        {
            ItemStateClass[] read = {new ItemStateClass()};
             string etat="d";
             while(etat!="")
            {
                try
                {
                                etat = "";
                    string jsonString = File.ReadAllText(path);
                        read = JsonSerializer.Deserialize<ItemStateClass[]>(jsonString);
                    
                }
                    catch (System.IO.IOException e)
                {
                      
                      etat = e.ToString();

                }
                catch(Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                }

            }
            return read;


        }

        public static void Write( Setting setting, params ItemStateClass[] itemToWrite)
        {
            string etat = "d";
            while (etat != "")
            {
                try
                {
                    etat = "";
                    JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };
                    string jsonString = JsonSerializer.Serialize(itemToWrite, options);
                    File.WriteAllText(setting.PathStates(), jsonString);

                }
                catch (System.IO.IOException e)
                {

                    etat = e.ToString();

                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                }
            }
            
        }
        public static void Write(Setting setting, ItemLogClass itemToWrite,params string[] task)
        {
            
            string etat = "d";

            while (etat != "")
            {
                try
                {
                    etat = "";
                    JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };
                    string jsonString = JsonSerializer.Serialize(itemToWrite, options) + "\n";
                    File.AppendAllText(setting.PathLog() + itemToWrite.Date + ".Json", jsonString);
                }
                catch (System.IO.IOException e)
                {

                    etat = e.ToString();

                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                }
            }

        }

        public static bool IsFileExist()
        {
            Setting setting = new Setting();
            return File.Exists(setting.PathStates());
        }
    }
}