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
            bool inGame = false;
            try
            {
                TcpClient tcpclnt = new TcpClient();
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
                Console.WriteLine("Waiting for a game (either start one or wait to be asked).....");

                String line;

                while (true)
                {
                    line = Console.ReadLine();
                    if (line.Length > 10 && line.Substring(0, 10).Equals("wanna_play", StringComparison.Ordinal))
                    {
                        Console.WriteLine("Trying to connect to " + line.Substring(11, line.Length - 11));

                        tcpclnt.Connect(line.Substring(11, line.Length - 11), port);

                        //String str = Console.ReadLine();

                        ASCIIEncoding asen = new ASCIIEncoding();
                        byte[] ba = asen.GetBytes("wanna_play");
                        Console.WriteLine("Transmitting.....");

                        Stream stm = tcpclnt.GetStream();
                        stm.Write(ba, 0, ba.Length);

                        //Console.WriteLine("Connected");
                        //Console.Write("Enter the string to be transmitted : ");
                        //byte[] bb = new byte[100];

                        //while (true)
                        //{
                        //    bb = new byte[100];
                        //    String str = Console.ReadLine();
                        //    Stream stm = tcpclnt.GetStream();

                        //    ASCIIEncoding asen = new ASCIIEncoding();
                        //    byte[] ba = asen.GetBytes(str);
                        //    Console.WriteLine("Transmitting.....");

                        //    stm.Write(ba, 0, ba.Length);


                        //    int k = stm.Read(bb, 0, 100);
                        //    string msg = System.Text.Encoding.Default.GetString(bb);
                        //    //for (int i = 0; i < k; i++)
                        //    //    Console.Write(Convert.ToChar(bb[i]));
                        //    Console.WriteLine(msg);
                        //}
                        
                    }

                    if (tcpListener.Pending())
                    {
                        //if (tcpclnt.Connected)
                        //{
                        //    Socket s = tcpListener.AcceptSocket();
                        //    ASCIIEncoding asen = new ASCIIEncoding();
                        //    s.Send(asen.GetBytes("already in a game"));
                        //}
                        //else
                        //{
                            Socket s = tcpListener.AcceptSocket();
                            Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);

                            byte[] b = new byte[100];
                            //while (true)
                            //{
                            b = new byte[100];
                            int k = s.Receive(b);
                            Console.WriteLine("Recieved...");
                            string msg = System.Text.Encoding.Default.GetString(b);
                            //for (int i = 0; i < k; i++)
                            //    Console.Write(Convert.ToChar(b[i]));
                            Console.WriteLine(msg);
                            if (!inGame && msg.Substring(0, 10).Equals("wanna_play", StringComparison.Ordinal))
                            {
                                inGame = true;
                                ASCIIEncoding asen = new ASCIIEncoding();
                                s.Send(asen.GetBytes("Let's play!"));
                            }
                            else if (inGame && msg.Substring(0, 10).Equals("wanna_play", StringComparison.Ordinal))
                            {
                                ASCIIEncoding asen = new ASCIIEncoding();
                                s.Send(asen.GetBytes("Already in a game!"));
                            }

                            ASCIIEncoding asen2 = new ASCIIEncoding();
                            s.Send(asen2.GetBytes("The string was recieved by the server."));
                            Console.WriteLine("\nSent Acknowledgement");

                            if (msg.Substring(0, 4).Equals("exit", StringComparison.Ordinal))
                            {
                                Console.WriteLine("connection terminated");
                                s.Close();
                                break;
                            }
                            //}
                        //}
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
