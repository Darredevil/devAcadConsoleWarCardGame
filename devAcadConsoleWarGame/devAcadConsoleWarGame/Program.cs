using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;


namespace devAcadConsoleWarGame
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IPAddress ipAd = IPAddress.Parse("127.0.0.1");
                // use local m/c IP address, and 
                // use the same in the client

                int port = 8081;

                /* Initializes the Listener */
                TcpListener tcpListener = new TcpListener(ipAd, port);

                /* Start Listeneting at the specified port */
                tcpListener.Start();

                Console.WriteLine("The program is running at port " + port + "...");
                Console.WriteLine("The local End point is  :" +
                                  tcpListener.LocalEndpoint);
                Console.WriteLine("Waiting for a connection.....");

                while (true)
                {

                    if (tcpListener.Pending())
                    {
                        Socket s = tcpListener.AcceptSocket();
                        Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);

                        byte[] b = new byte[100];
                        while (true)
                        {
                            b = new byte[100];
                            int k = s.Receive(b);
                            Console.WriteLine("Recieved...");
                            string msg = System.Text.Encoding.Default.GetString(b);
                            //for (int i = 0; i < k; i++)
                            //    Console.Write(Convert.ToChar(b[i]));
                            Console.WriteLine(msg);

                            ASCIIEncoding asen = new ASCIIEncoding();
                            s.Send(asen.GetBytes("The string was recieved by the server."));
                            Console.WriteLine("\nSent Acknowledgement");

                            if (msg.Substring(0, 4).Equals("exit", StringComparison.Ordinal))
                            {
                                Console.WriteLine("connection terminated");
                                s.Close();
                                break;
                            }
                        }
                    }
                }
                /* clean up */
                Console.Read();
                
                tcpListener.Stop();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
        }
    }
}
