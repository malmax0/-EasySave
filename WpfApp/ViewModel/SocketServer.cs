using System;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text.Json;


namespace WpfApp.ViewModel
{
    public class SocketServer
    {
        private Socket client;
        private readonly Socket server;


        //creation of the connexion
        public SocketServer()
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            int port = 11000;
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            try
            {
                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(localEndPoint);
                listener.Listen(3);
                server = listener;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        //accept connexion
        public void AccepterConnection()
        {
            bool isconect = false;
            while (!isconect)
            {
                try
                {
                    client = server.Accept();
                    isconect = true;
                }
                catch (Exception)
                {
                }
            }

        }

        //Send the datagrid to the remote console
        public void EnvoiStatus( )
        {
            string message = JsonSerializer.Serialize(States.GetSaveList());
            if(client!=null)
            { 
            Console.WriteLine(client.LocalEndPoint.ToString());
            byte[] msg = Encoding.UTF8.GetBytes(message);
            client.Send(msg);
            }
        }

        //disconect the server
        public static void Deconnecter(Socket s)
        {
            s.Shutdown(SocketShutdown.Both);
            s.Close();
        }

        //while loop to know which button is selected by the client
        public bool Look(MainWindow MW)
        {
            byte[] bytes = new byte[1024];
           
                int bytesRec;
                string e;
            while(true)
            { 
                bytesRec = client.Receive(bytes);
                e = Encoding.UTF8.GetString(bytes, 0, bytesRec);

                if (e == "refresh")
                {
                    EnvoiStatus();
                }
                else if (e.StartsWith("taskNumbers"))
                {
                    Tasklunch(MW, e);




                }
                else if (e.StartsWith("Pause"))
                {
                    MW.Pause_Click(null, null);
                    byte[] msg = Encoding.UTF8.GetBytes("pause");
                    client.Send(msg);
                }
                else if (e.StartsWith("resume"))
                {
                    MW.Resume_Click(null, null);
                    byte[] msg = Encoding.UTF8.GetBytes("resume");
                    client.Send(msg);
                }
                else if (e.StartsWith("kill"))
                {
                    MW.Stop_Click(null, null);
                    byte[] msg = Encoding.UTF8.GetBytes("kill");
                    client.Send(msg);
                }
                else if (e.StartsWith("trash"))
                {
                    MW.Stop_Click(null, null);
                    e = e.Split('?')[1];
                    int id = int.Parse(e);
                    States.DeleteStatus(id);
                    MW.LoadGridView();                  
                }
            }
        }
        public async void Tasklunch(MainWindow MW,string e)
        {
            MW.Animate("StartLaunch");
            byte[] msg = Encoding.UTF8.GetBytes("play");
            client.Send(msg);
            e = e.Split('?')[1];

            Task task = Task.Run(() =>
            {
                MW.save.MakeASave(e, MW.progre);
            });
            await task;
            Thread.Sleep(1000);

            MW.Animate("EndLaunch");
            msg = Encoding.UTF8.GetBytes("fini");
            client.Send(msg);            
        }
        public void Requete(string e)
        {
            byte[] msg = Encoding.UTF8.GetBytes(e);
            if(client!=null)
            { 
                client.Send(msg);
            }
        }

        public void Launch(MainWindow MW)
        {
            bool etat = true;
            //EcouterReseau(client,"");
            while (etat)
            {
                AccepterConnection();
                try
                {
                    etat=Look(MW);
                }
                catch(ThreadAbortException)
                {
                    break;
                }
                catch(Exception)
                {
                    client.Disconnect(false);
                }
            }
        } 
    }
}
