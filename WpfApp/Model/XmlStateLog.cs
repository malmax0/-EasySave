using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Windows;
using System.Windows.Threading;
namespace WpfApp.Model
{
    //writing 
    public static class XmlStateLog
    {

        public static ItemLogClass[] Read(string path)
        {
            // Create a TextReader to read the file.

            FileStream fs = new FileStream(path + DateTime.Now.ToString("MMMM dd, yyyy") + ".Xml", FileMode.OpenOrCreate);
            TextReader reader = new StreamReader(fs);
            try
                {

                    ItemLogClass[] items;
                    // Create an instance of the XmlSerializer specifying type.
                    XmlSerializer serializer = new XmlSerializer(typeof(ItemLogClass[]));
                   
                    // Use the Deserialize method to restore the object's state.
                    items = (ItemLogClass[])serializer.Deserialize(reader);
                    reader.Close();

                    return items;
                }
                catch (Exception)
                {
                    reader.Close();
                    return null;
                }

        }

        public static void Write(ItemLogClass itemToAdd, Setting setting,params string[] task)
        {
            string path;
             if (task.Length > 0)
            {
                 path = setting.PathStates()+task[0];
            }
            else
            {
                 path = setting.PathStates() ;
            }
            try
            {
                //Read the file content
                var itemsRead = Read(path);
                
                //Identify the number of logs in the file
                int length;
                if (itemsRead?.Length == null)
                {
                    length = 0;
                }
                else
                {
                    length = itemsRead.Length;
                }

                //Create the collection of log to write
                ItemLogClass[] itemToWrite = new ItemLogClass[length+1] ;
                for (var i = 0; i < itemsRead?.Length; i++)
                {
                    itemToWrite[i]= itemsRead[i];
                }
                //Add the new log in the collection
                itemToWrite[length] = itemToAdd;
                //Write all the log
                TextWriter textWriter = new StreamWriter(path + itemToAdd.Date + ".Xml");
                XmlSerializer serializer = new XmlSerializer(typeof(ItemLogClass[]));
                serializer.Serialize(textWriter, itemToWrite);
                textWriter.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

    }
}