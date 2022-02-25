namespace WpfApp.Model
{
    public class Setting
    {
        private ItemSettings Parametre;

        public Setting()
        {
            Parametre = Config.Read();
        }
        public void Refresh()
        {
            Parametre = Config.Read();
        }

        public string PathCrypt()
        {
            return Parametre.PathCrypt;
        }
        public string PathLog()
        {
            return Parametre.PathLog;
        }
        public string LogExtension()
        {
            return Parametre.LogExtension;
        }
        public string PathStates()
        {
            return Parametre.PathStates;
        }
        public string[] CyptoExtension()
        {
            return Parametre.CyptoExtension;
        }
        public string Buisnessoft()
        {
            return Parametre.Buisnessoft;
        }
        public string[] FilesPrio()
        {
            return Parametre.FilesPrio;
        }
        public string LimitSize()
        {
            return Parametre.LimitSize;
        }
        public string LimitThread()
        {
            return Parametre.LimitThread;
        }
        public ItemSettings GetSettings()
        {
            return Parametre;
        }
    }
}
