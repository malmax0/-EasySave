namespace App.Model
{

    //all the settings information
     public class Settings
    {
        public string Langue { set; get; }
        public string LogExtension { set; get; }
        public static string PathLog = "./";
        public static string PathStates ="./State.json";
    }

}
