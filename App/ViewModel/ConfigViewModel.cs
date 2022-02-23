using App.Model;

namespace App.ViewModel
{ 
    //class for setting interaction 
    public static  class ConfigViewModel
    {
        //change setting
	    public static void PutInConfig(params string[] a)
	    {
            switch (a[0])
            {
                case "1":
                    Config.Write("EN");
                    break;
                case "2":
                    Config.Write("FR");
                    break;
                default:
                    Config.Write("EN");
                    break;
            }
        }     
	}   
}