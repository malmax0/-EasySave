using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace WpfApp.Model
{
    static class Proces
    {
        public static bool isexist(string proces)
        {
            Process[] liste = Process.GetProcessesByName(proces);
            return liste.Length != 0;
        }
    }
}
