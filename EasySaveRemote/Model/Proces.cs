using System.Diagnostics;
namespace EasySaveRemote
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
            Process[] liste = Process.GetProcessesByName("EasySaveRemote");
            foreach(Process item in liste)
            {
                item.Kill();
            }
        }
    }
}
