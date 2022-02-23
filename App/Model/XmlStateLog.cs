using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace App.Model
{
    //writing 
    public static class XmlStateLog
    {

        public static ItemLogClass[] Read()
        {

            // Create a TextReader to read the file.
            FileStream fs = new FileStream(Settings.PathLog + DateTime.Now.ToString("MMMM dd, yyyy") + ".Xml", FileMode.OpenOrCreate);
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
                catch (Exception e)
                {
                    reader.Close();
                    return null;
                }

        }

        public static void Write(ItemLogClass itemToAdd)
        {
            try
            {
                var itemsRead = Read();
                
                
                int length;
                if (itemsRead?.Length == null)
                {
                    length = 0;
                }
                else
                {
                    length = itemsRead.Length;
                }

                ItemLogClass[] itemToWrite = new ItemLogClass[length+1] ;
                for (var i = 0; i < itemsRead?.Length; i++)
                {
                    itemToWrite[i]= itemsRead[i];
                }
                itemToWrite[length] = itemToAdd;

                TextWriter textWriter = new StreamWriter(Settings.PathLog + itemToAdd.Date + ".Xml");
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