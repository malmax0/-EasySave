using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.ViewModel;
using App.Model;
using System.IO;


namespace App.View
{
    class Program
    {

        static private void _ChangeSettings(Langue lang)
        {

            Console.Write(lang.Translation(1));
           string Langue=Console.ReadLine();

            Console.Write(lang.Translation(34));
            string extension = Console.ReadLine();
            ConfigViewModel.PutInConfig(Langue, extension);
        }

        static void Main(string[] args)
        {

            Langue lang = new Langue();
            Console.Clear();

            while (true)
            {
                Console.Clear();
                Console.Write(lang.Translation(2));

                string input = Console.ReadLine();

                switch (input)
                {
                    case "0":
                        // leave the application 

                        Environment.Exit(0);
                        break;

                    case "1":
                        // function add a backup :

                        Console.Clear();
                        Console.Write(lang.Translation(3));

                        Console.Write(lang.Translation(10));
                        string name = Console.ReadLine();

                        Console.Write(lang.Translation(11));
                        string srcPath = Console.ReadLine();

                        Console.Write(lang.Translation(12));
                        string destPath = Console.ReadLine();

                        Console.Write(lang.Translation(6));
                        string backupType = Console.ReadLine();


                        Console.WriteLine(States.AddTask(name, srcPath, destPath, backupType));
                        Console.ReadKey();
                        break;

                    case "2":
                        // function delete a backup

                        //Console.Clear();
                        Console.Write(lang.Translation(21));
                        string deleteNb = Console.ReadLine();
                        if (deleteNb != "0")
                        {
                            Console.Write(lang.Translation(22));
                            string confirmationD = Console.ReadLine();
                            if (confirmationD == "1")
                            {
                                Console.Write(States.DeleteStatus(Int16.Parse(deleteNb)));
                                Console.ReadKey();
                            }
                        }

                        break;

                    case "3":
                        // function show a backup

                        Console.Clear();

                        Console.Write(States.ReadStatus());
                        Console.ReadKey();

                        break;

                    case "4":
                        // function launch a backup

                        Console.Clear();
                        Console.Write(lang.Translation(24));

                        string launchNb = Console.ReadLine();

                        if (launchNb != "0")
                        {
                            var save = new Save();
                            var observer = new ProgressObserver();
                            save.Attach(observer);
                            Console.Write(lang.Translation(25));
                            string confirmation = Console.ReadLine();
                            if (confirmation == "1")
                            {
                                Console.Write(save.MakeASave(launchNb));
                                save.Detach(observer);
                                Console.ReadKey();
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            Console.Write(lang.Translation(27));
                        };

                        break;

                    case "5":
                        //function change langage
                        _ChangeSettings(lang);
                        lang = new Langue();
                        break;



                    default:
                        break;
                }
            }
        }
    }
}
