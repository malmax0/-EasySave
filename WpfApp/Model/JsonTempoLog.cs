using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace WpfApp.Model
{
    internal class JsonTempoLog
    {
        public static ItemCryptClass[] Read()
        {
            Setting setting = new Setting();
            string jsonString = File.ReadAllText(setting.PathCrypt());
            var read = JsonSerializer.Deserialize<ItemCryptClass[]>(jsonString);

            return read;
        }
    }
}
