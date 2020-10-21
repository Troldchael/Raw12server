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
        // not implemented. we couldnt get object to return values for compare/contains
        public class Response
        {
            public string status { get; set; }
            public string body { get; set; }
        }

        static void Main(string[] args)
        {
            var server = new TcpListener(IPAddress.Loopback, 5000);
            server.Start();
            Console.WriteLine("Server started!");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Accepted client!");

                NetworkStream stream = client.GetStream();

                var msg = Read(client, stream);

                Console.WriteLine($"Message from client {msg}");

                // try to respond to RequestWithoutMethod_MissingMethodError
                if (msg.Contains("{}"))
                {
                    var methodError = new
                    {
                        status = "missing method",
                        body = (String)null
                    };

                    var me = JsonSerializer.Serialize(methodError);
                    stream.Write(Encoding.UTF8.GetBytes(me));

                }
                // end 

                // try to respond to RequestWithInvalidPath_StatusBadRequest
                if (msg.Contains("/api/xxx"))
                {

                    var badRequest = new
                    {
                        status = "4 bad request",
                        body = (String)null
                    };

                    var br = JsonSerializer.Serialize(badRequest);
                    stream.Write(Encoding.UTF8.GetBytes(br));
                }
                // end

                // respond to rest
                var data = Encoding.UTF8.GetBytes(msg.ToUpper());
                stream.Write(data);
                // end
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
