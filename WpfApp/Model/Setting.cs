using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace WpfApp.Model
{
    class Setting
    {
        private ItemSettings Parametre;


        public Setting()
        {
            Parametre = Config.Read();
        }
        public void refresh()
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
        public string buisnessoft()
        {
            return Parametre.buisnessoft;
        }
        public ItemSettings GetSettings()
        {
            return Parametre;
        }


    }
}
