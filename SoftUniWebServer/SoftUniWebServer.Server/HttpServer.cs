﻿using SoftUniWebServer.Server.HTTP;
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

        private readonly RoutingTable _routingTable;

        public HttpServer(string ipAddress, int port, Action<IRoutingTable> routingTableConfiguration)
        {
            _ipAddress = IPAddress.Parse(ipAddress);
            _port = port;

            _serverListener = new TcpListener(_ipAddress, _port);

            routingTableConfiguration(_routingTable = new RoutingTable());
        }

        public HttpServer(int port, Action<IRoutingTable> routingTable)
            : this("127.0.0.1", port, routingTable)
        {
        }

        public HttpServer(Action<IRoutingTable> routingTable)
            : this(8080, routingTable)
        {
        }

        public async Task Start()
        {
            _serverListener.Start();

            Console.WriteLine($"Server started on port {_port}...");
            Console.WriteLine("Listening for requests...");

            while (true)
            {
                var connection = await _serverListener.AcceptTcpClientAsync();

                _ = Task.Run(async () =>
                {
                    var networkStream = connection.GetStream();

                    var requestText = await this.ReadRequest(networkStream);
                    Console.WriteLine(requestText);

                    var request = Request.Parse(requestText);

                    var response = _routingTable.MatchRequest(request);

                    //Execute pre-render action for the response
                    if (response.PreRenderAction != null)
                    {
                        response.PreRenderAction(request, response);
                    }

                    await WriteResponse(networkStream, response);

                    connection.Close();
                });
            }
        }

        private async Task WriteResponse(NetworkStream networkStream, Response response)
        {
            var responseBytes = Encoding.UTF8.GetBytes(response.ToString());

            await networkStream.WriteAsync(responseBytes);
        }

        private async Task<string> ReadRequest(NetworkStream networkStream)
        {
            var bufferLength = 1024;
            var buffer = new byte[bufferLength];

            var totalBytes = 0;

            var requestBuilder = new StringBuilder();

            do
            {
                var bytesRead = await networkStream.ReadAsync(buffer, 0, bufferLength);

                totalBytes += bytesRead;

                if (totalBytes > 10 * 1024)
                {
                    throw new InvalidOperationException("Request is too large.");
                }

                requestBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            } 
            while (networkStream.DataAvailable);

            return requestBuilder.ToString();
        }
    }
}
