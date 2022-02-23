namespace App.Model
{
    public class ItemStateClass
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string FileSource { get; set; }
        public string FileTarget { get; set; }
        public string State { get; set; }
        public string BackupType { get; set; }
        public string FileSize { get; set; }
        public string TotalFilesToCopy { get; set; }
        public string TotalFileSize { get; set; }
        public string NbFilesLeftToDo { get; set; }
        public string Progression { get; set; }
    }
}
