using System;
using App.Model;

namespace App.ViewModel

{ 
     //the language procesor
     class  Langue
     {
        private int _lang;
        
        public string Translation(int terme)
        {

            //return the message in the right language
            return Message.MessageTable[terme, _lang];
        }

        //look in the setting file and take the language
        public  Langue()
        {
            //difined the language
            switch (Config.Read().Langue)
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
            }
 
        }
    }
}