using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SoftUniWebServer.Server
{
    public class HttpServer
    {
        private readonly IPAddress _ipAddress;
        private readonly int _port;
        private readonly TcpListener _serverListener;

        public HttpServer(string ipAddress, int port)
        {
            _ipAddress = IPAddress.Parse(ipAddress);
            _port = port;

            _serverListener = new TcpListener(_ipAddress, _port);
        }

        public void Start()
        {
            _serverListener.Start();

            Console.WriteLine($"Server started on port {_port}...");
            Console.WriteLine("Listening for requests...");

            while (true)
            {
                var connection = _serverListener.AcceptTcpClient();

                var networkStream = connection.GetStream();

                WriteResponse(networkStream, "Hello from the server!");

                connection.Close();
            }
        }

        private void WriteResponse(NetworkStream networkStream, string message)
        {
            var contentLength = Encoding.UTF8.GetByteCount(message);

            var response = $@"HTTP/1.1 200 OK
Content-Type: text/plain; charset=UTF-8
Content-Length: {contentLength}

{message}";

            var responseBytes = Encoding.UTF8.GetBytes(response);

            networkStream.Write(responseBytes);
        }
    }
}
