using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows.Threading;
using System.Text.Json;
using System.Threading;
using System.Windows;
namespace EasySaveRemote
{
    class SocketClient
    {
        public Socket client;
        Thread ecoute;
        MainWindow MW;

        //create client socket
        public SocketClient(MainWindow _MW)
        {
            MW = _MW;
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            int port = 11000;
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

             sender.Connect(remoteEP);
            client = sender;
            ecoute = new Thread(() => EcouterReseau());
            ecoute.Start();
            Refresh();
        }

        //collect socket name "client"
        public Socket getclient()
        {
            return client;
        }

        //display datagrid and dispay the animation
        public void EcouterReseau()
        {
            int bytesRec;
            byte[] bytes = new byte[1024];
            string e;
            
            while (true)
            {
                string a = "a";
                while (a != "")
                {
                    try
                    {
                        bytesRec = client.Receive(bytes);
                        e = Encoding.UTF8.GetString(bytes, 0, bytesRec);

                        if (e.StartsWith("["))
                        {

                            SaveList[] read = JsonSerializer.Deserialize<SaveList[]>(e);
                            List<SaveList> p = new List<SaveList>();
                            foreach (SaveList f in read)
                            {
                                p.Add(f);
                            }
                            while (a != "")
                            {
                                MW.actualise(p);
                                a = "";
                            }
                        }
                        else if(e=="play")
                        {
                            MW.Animate("StartLaunch");                           
                        }
                        else if (e == "pause")
                        {
                            MW.pause();
                        }
                        else if (e == "fini")
                        {
                            MW.Animate("EndLaunch");                                                        
                        }
                        else if (e == "resume")
                        {
                            MW.resume();
                        }
                        else if (e == "kill")
                        {
                            MW.kill();
                        }
                        else if (e.StartsWith("av"))
                        {
                            MW.avancement(Int32.Parse(e.Split('?')[1]));
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }

        //disconnect client socket
        public void Deconnecter(Socket socket)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        //send info to the server
        public void requete(string message)
        {
            byte[] bytes = new byte[1024];
            byte[] msg = Encoding.UTF8.GetBytes(message);
            client.Send(msg);
        }

        //launch a backup
        public void Launch(string a)
        {
            requete("taskNumbers?" + a);
        }
        //delete a backup
        public void trash(string a)
        {
            requete("trash?" + a);
        }
        //refresh the view
        public void Refresh()
        {
            requete("refresh");
            Thread.Sleep(1000);                     
        }
    }
}
