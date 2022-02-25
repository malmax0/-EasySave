using System.IO;
using System.Text.Json;

namespace WpfApp.Model
{
    internal static class JsonTempoLog
    {
        public static ItemCryptClass[] Read()
        {
            Setting setting = new Setting();
            string jsonString = File.ReadAllText(setting.PathCrypt());
            ItemCryptClass[] read = JsonSerializer.Deserialize<ItemCryptClass[]>(jsonString);

            return read;
        }
    }
}
