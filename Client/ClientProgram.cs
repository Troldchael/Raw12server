using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Client
{
    class ClientProgram
    {
        static void Main(string[] args)
        {
            using var client = new TcpClient();
            client.Connect(IPAddress.Loopback, 5000);

            var request1 = new
            {
                // fill values in object
                method = "create",
                path = "/test",
                dateTime = UnixTimestamp()
            };

            Console.WriteLine("REQUESTOBJECT TEST: " + request1);

            // convert request1 object to JSON
            string requestAsJson = JsonSerializer.Serialize(request1);

            var stream = client.GetStream();

            //convert JSON to bytes utf8 encoding
            var data = Encoding.UTF8.GetBytes(requestAsJson);

            stream.Write(data);

            data = new byte[client.ReceiveBufferSize];

            var cnt = stream.Read(data);

            var msg = Encoding.UTF8.GetString(data, 0, cnt);

            Console.WriteLine($"Message from the server: {msg}");
        }

        private static string UnixTimestamp()
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
        }
    }
}
