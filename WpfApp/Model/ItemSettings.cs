namespace WpfApp.Model
{

    //all the settings information
     public class ItemSettings
     {
        public string Language { set; get; }
        public  string PathCrypt { set; get; }
        public  string PathLog { set; get; }
        public string LogExtension { set; get; }
        public  string PathStates { set; get; }
        public string[] CyptoExtension { set; get; }
        public string Buisnessoft { set; get; }
        public string FilesPrio { get; set; }
        public string LimitSize { get; set; }
     }

}
