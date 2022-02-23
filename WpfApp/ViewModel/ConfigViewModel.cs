using WpfApp.Model;

namespace WpfApp.ViewModel
{ 
    //class for setting interaction 
    public static  class ConfigViewModel
    {
        //change setting
	    public static void PutInConfig(params string[] a)
	    {
            ItemSettings param = new ItemSettings();
           
            param.Language=a[0].Split(' ')[1];
            param.PathStates = a[1];
            param.PathLog = a[2];
            param.LogExtension = a[3].Split(' ')[1];
            param.PathCrypt = a[4];
            param.CyptoExtension = a[5].Split(',');
            param.buisnessoft = a[6];
            Config.Write(param);




        }
        public static ItemSettings getparam()
        {
            Setting param = new Setting();
            return param.GetSettings();
        }
	}   
}