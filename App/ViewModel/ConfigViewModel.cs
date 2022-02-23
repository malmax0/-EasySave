using App.Model;

namespace App.ViewModel
{ 
    //class for setting interaction 
    public static  class ConfigViewModel
    {
        //change setting
	    public static void PutInConfig(params string[] a)
	    {
            string[] data=new string[2];
            switch (a[0])
            {
                case "1":
                    data[0]="EN";
                    break;
                case "2":
                    data[0] = "FR";
                    break;
                default:
                    data[0]="EN";
                    break;
            }

            switch (a[1])
            {
                case "1":
                    data[1]=".json";
                    break;
                case "2":
                    data[1] = ".xml";
                    break;
                default:
                    data[1] = ".json";
                    break;
            }
            Config.Write(data);
        }     
	}   
}