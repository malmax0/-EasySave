namespace WpfApp.Model
{
    public class ItemLogClass
    {
        public string Name { get; set; }
        public string FileSource { get; set; }
        public string FileTarget { get; set; }
        public string FileSize { get; set; }
        public string FileTransferTime { get; set; }
        public string Date { get; set; }
        public bool Encrypt { get; set; }
        public int EncryptTime { get; set; }
    }
}
