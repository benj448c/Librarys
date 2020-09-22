using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Librarys.TCPServer
{
    public abstract class AbstractTCPServer
    {
        public void Start()
        {
            TcpListener server = new TcpListener(IPAddress.Loopback, 7007);
            server.Start();

            while (true)
            {
                TcpClient socket = server.AcceptTcpClient(); // venter på client

                // starter ny tråd
                Task.Run(
                    // indsætter en metode (delegate)
                    () =>
                    {
                        TcpClient tmpsocket = socket;
                        DoClient(tmpsocket);
                    }
                );

            }


        }

        private void DoClient(TcpClient socket)
        {
            using (StreamReader sr = new StreamReader(socket.GetStream()))
            using (StreamWriter sw = new StreamWriter(socket.GetStream()))
            {
                TcpServerWork(sr,sw);
                //string str = sr.ReadLine();
                //sw.WriteLine(str);
                sw.Flush();
            }

            socket?.Close();
        }

        protected abstract void TcpServerWork(StreamReader sr, StreamWriter sw);
    
    }
}
