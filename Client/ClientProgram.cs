﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class ClientProgram
    {
        // request object class
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

		// requets object attempt1
            Request request1 = new Request()
			{
				method = "create";
				path = "dinmor";
				dateTime = "dinmortid";
			}
            
            var stream = client.GetStream();

            var data = Encoding.UTF8.GetBytes("Hello");

            stream.Write(data);

            data = new byte[client.ReceiveBufferSize];

            var cnt = stream.Read(data);

            var msg = Encoding.UTF8.GetString(data, 0, cnt);

            Console.WriteLine($"Message from the server: {msg}");
        }
    }
}
