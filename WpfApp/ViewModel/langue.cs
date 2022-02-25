using WpfApp.Model;

namespace WpfApp.ViewModel

{ 
     //the language procesor
     public class Langue
     {
        private readonly int _lang;
        
        public string Translation(int terme)
        //Get a prefinished message in message.cs
        {
            return Message.MessageTable[terme, _lang];
            //Return the message in the right language
        }

        public  Langue()
        //Look in the setting file and take the language
        {

            switch (Config.Read().Language)
            {
                case "FR":
                    {
                        _lang = 1;
                        break;
                    }

                case "EN":
                    {
                        _lang = 0;
                        break;
                    }

                default:
                    break;
            }
        }
    }
}