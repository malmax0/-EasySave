using WpfApp.Model;

namespace WpfApp.ViewModel
{ 
    //Class for settings interactions
    public static  class ConfigViewModel
    {
        //Functions which aim to change settings
	    public static void PutInConfig(params string[] a)
	    {
            ItemSettings param = new ItemSettings
            {
                Language = a[0].Split(' ')[1],
                PathStates = a[1],
                PathLog = a[2],
                LogExtension = a[3].Split(' ')[1],
                PathCrypt = a[4],
                CyptoExtension = a[5].Split(','),
                Buisnessoft = a[6],
                FilesPrio = a[7].Split(','),
                LimitSize = a[8],
                LimitThread = a[9]
            };
            Config.Write(param);

        }
        public static ItemSettings Getparam()
        {
            Setting param = new Setting();
            return param.GetSettings();
        }
	}   
}