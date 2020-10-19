using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Client
{
    class ClientProgram
    {
        // request class
        class Request
        {
            public string method;
            public string path;
            public DateTime dateTime;
        } 

        static void Main(string[] args)
        {
            using var client = new TcpClient();
            client.Connect(IPAddress.Loopback, 5000);

            // request1 object
            Request request1 = new Request();

            // fill values in object
			request1.method = "create";
            request1.path = "/test";
            request1.dateTime =  DateTime.Now;

            string requestAsJson = JsonSerializer.Serialize<Request1>(request1);

            var stream = client.GetStream();

            //var data = Encoding.UTF8.GetBytes(request1);

            stream.Write(request1);

            data = new byte[client.ReceiveBufferSize];

            var cnt = stream.Read(data);

            var msg = Encoding.UTF8.GetString(data, 0, cnt);

            Console.WriteLine($"Message from the server: {msg}");
        }

    }

    class Request1
    {
    }
}
