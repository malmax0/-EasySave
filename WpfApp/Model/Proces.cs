﻿using System.Diagnostics;
namespace WpfApp.Model
{
    static class Proces
    {
        public static bool IsExist(string proces)
        {
            Process[] liste = Process.GetProcessesByName(proces);
            return liste.Length != 0;
        }
        public static void close()
        {
            Process[] liste = Process.GetProcessesByName("WpfApp");
            foreach(Process item in liste)
            {
                item.Kill();
            }
        }
    }
}
