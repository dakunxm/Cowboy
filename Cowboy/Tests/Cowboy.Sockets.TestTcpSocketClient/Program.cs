﻿using System;
using System.Net;
using System.Text;
using Logrila.Logging.NLogIntegration;

namespace Cowboy.Sockets.TestTcpSocketClient
{
    class Program
    {
        static TcpSocketClient _client;

        static void Main(string[] args)
        {
            NLogLogger.Use();

            ConnectToServer();

            Console.WriteLine("TCP client has connected to server.");
            Console.WriteLine("Type something to send to server...");
            while (true)
            {
                try
                {
                    string text = Console.ReadLine();
                    if (text == "quit")
                    {
                        break;
                    }
                    else if (text == "many")
                    {
                        text = new string('x', 8192);
                        for (int i = 0; i < 1000000; i++)
                        {
                            _client.Send(Encoding.UTF8.GetBytes(text));
                        }
                    }
                    else if (text == "big1")
                    {
                        text = new string('x', 1024 * 1024 * 1);
                        _client.Send(Encoding.UTF8.GetBytes(text));
                    }
                    else if (text == "big2")
                    {
                        text = new string('x', 1024 * 1024 * 2);
                        _client.Send(Encoding.UTF8.GetBytes(text));
                    }
                    else if (text == "big5")
                    {
                        text = new string('x', 1024 * 1024 * 5);
                        _client.Send(Encoding.UTF8.GetBytes(text));
                    }
                    else if (text == "big10")
                    {
                        text = new string('x', 1024 * 1024 * 10);
                        _client.Send(Encoding.UTF8.GetBytes(text));
                    }
                    else if (text == "big20")
                    {
                        text = new string('x', 1024 * 1024 * 20);
                        _client.Send(Encoding.UTF8.GetBytes(text));
                    }
                    else if (text == "big50")
                    {
                        text = new string('x', 1024 * 1024 * 50);
                        _client.Send(Encoding.UTF8.GetBytes(text));
                    }
                    else if (text == "big100")
                    {
                        text = new string('x', 1024 * 1024 * 100);
                        _client.Send(Encoding.UTF8.GetBytes(text));
                    }
                    else if (text == "big1000")
                    {
                        text = new string('x', 1024 * 1024 * 1000);
                        _client.Send(Encoding.UTF8.GetBytes(text));
                    }
                    else
                    {
                        _client.Send(Encoding.UTF8.GetBytes(text));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            _client.Close();
            Console.WriteLine("TCP client has disconnected from server.");

            Console.ReadKey();
        }

        private static void ConnectToServer()
        {
            var config = new TcpSocketClientConfiguration();
            //config.UseSsl = true;
            //config.SslTargetHost = "Cowboy";
            //config.SslClientCertificates.Add(new System.Security.Cryptography.X509Certificates.X509Certificate2(@"D:\\Cowboy.cer"));
            //config.SslPolicyErrorsBypassed = false;
            //config.SendTimeout = TimeSpan.FromSeconds(2);

            //config.FrameBuilder = new FixedLengthFrameBuilder(20000);
            //config.FrameBuilder = new RawBufferFrameBuilder();
            //config.FrameBuilder = new LineBasedFrameBuilder();
            //config.FrameBuilder = new LengthPrefixedFrameBuilder();
            //config.FrameBuilder = new LengthFieldBasedFrameBuilder();

            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 22222);

            _client = new TcpSocketClient(remoteEP, config);
            _client.ServerConnected += client_ServerConnected;
            _client.ServerDisconnected += client_ServerDisconnected;
            _client.ServerDataReceived += client_ServerDataReceived;
            _client.Connect();
        }

        static void client_ServerConnected(object sender, TcpServerConnectedEventArgs e)
        {
            Console.WriteLine(string.Format("TCP server {0} has connected.", e.RemoteEndPoint));
        }

        static void client_ServerDisconnected(object sender, TcpServerDisconnectedEventArgs e)
        {
            Console.WriteLine(string.Format("TCP server {0} has disconnected.", e.RemoteEndPoint));
        }

        static void client_ServerDataReceived(object sender, TcpServerDataReceivedEventArgs e)
        {
            var text = Encoding.UTF8.GetString(e.Data, e.DataOffset, e.DataLength);
            Console.Write(string.Format("Server : {0} --> ", e.Client.RemoteEndPoint));
            if (e.DataLength < 1024 * 1024 * 1)
            {
                Console.WriteLine(text);
            }
            else
            {
                Console.WriteLine("{0} Bytes", e.DataLength);
            }
        }
    }
}
