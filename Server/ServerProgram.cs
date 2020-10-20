using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Server
{
    class ServerProgram
    {
        // response class idea
        // not implemented
        public class Response
        {
            public string method { get; set; }
            public string path { get; set; }
            public string date { get; set; }
        }

        static void Main(string[] args)
        {
            var server = new TcpListener(IPAddress.Loopback, 5000);
            server.Start();
            Console.WriteLine("Server started!");

            while (true)
            {
                var client = server.AcceptTcpClient();
                Console.WriteLine("Accepted client!");

                var stream = client.GetStream();
           
                var msg = Read(client, stream);

                Console.WriteLine($"Message from client {msg}");

                // try to respond to RequestWithoutMethod_MissingMethodError
                if (msg.Contains("{}"))
                {
                    var methodError = new
                    {
                        status = "missing method",
                        path = "",
                        dateTime = UnixTimestamp(),
                        body = (String)null
                    };

                    var me = JsonSerializer.Serialize(methodError);

                    stream.Write(Encoding.UTF8.GetBytes(me));

                } else
                {
                    Console.WriteLine("doesnt contain!");
                }
                // end 

                // try to respond to RequestWithInvalidPath_StatusBadRequest
                var badreq = new
                {
                    status = "4 bad request",
                    path = "",
                    dateTime = UnixTimestamp(),
                    body = (String)null
                };

                var requestAsJson = JsonSerializer.Serialize(badreq);

                var data = Encoding.UTF8.GetBytes(requestAsJson);

                stream.Write(data);

            }
        }

        private static string Read(TcpClient client, NetworkStream stream)
        {
            byte[] data = new byte[client.ReceiveBufferSize];

            var cnt = stream.Read(data);

            var msg = Encoding.UTF8.GetString(data, 0, cnt);

            return msg;
        }

        private static string UnixTimestamp()
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
        }
    }
}
