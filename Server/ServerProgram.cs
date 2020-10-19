using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using Microsoft.VisualBasic;


namespace Server
{
    class ServerProgram
    {
        static void Main(string[] args)
        {
            var server = new TcpListener(IPAddress.Loopback, 5000);
            server.Start();
            Console.WriteLine("Server is running!");

            while (true)
            {
                var client = server.AcceptTcpClient();

                    Console.WriteLine("Accepted client!");
                    if (!client.GetStream().CanRead) continue;
                    var stream = client.GetStream();
                    var msg = Read(client, stream);


                    Console.WriteLine($"Message from client {msg}");
                    var data = Encoding.UTF8.GetBytes(msg.ToUpper());

                    stream.Write(data);

            } // While loop ends
        } // Main ends 

        private static string Read(TcpClient client, NetworkStream stream)
        {
            byte[] data = new byte[client.ReceiveBufferSize];
         
            var cnt = stream.Read(data);

            var msg = Encoding.UTF8.GetString(data, 0, cnt);
           
            return msg;
        } // Read ends

        private static long unixTime(TcpClient client, NetworkStream stream, DateTime foo)
        {
            byte[] data = new byte[client.ReceiveBufferSize];
            var cnt = stream.Read(data);

            foo = DateTime.UtcNow;
            long unixTime = ((DateTimeOffset)foo).ToUnixTimeSeconds();
            return unixTime;
        }
        public static void AcceptClient(TcpListener server)
        {
            Console.WriteLine("Waiting for client to connect..");
            server.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpClient), server);

        }
        public static void DoAcceptTcpClient(IAsyncResult ar)
        {
            // Get the listener that handles the client request.
            TcpListener server = (TcpListener)ar.AsyncState;

            // End the operation and display the received data on
            // the console.
            TcpClient client = server.EndAcceptTcpClient(ar);

            // Process the connection here. (Add the client to a
            // server table, read data, etc.)
            Console.WriteLine("Client has connected");

            // Signal the calling thread to continue.
            clientConnected.Set();
        }
        public static ManualResetEvent clientConnected =
            new ManualResetEvent(false);

    } // Server class ends
}
