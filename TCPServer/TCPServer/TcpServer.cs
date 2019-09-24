using System;
using System.Net;
using System.Net.Sockets;

namespace TCPServer
{
    public class TcpServer : IServer
    {
        private readonly int port = 0;
        private readonly TcpListener listener;

        public TcpServer(int port)
        {
            this.port = port;

            var localAdd = IPAddress.Parse("127.0.0.1");
            listener = new TcpListener(localAdd, port);
           
        }

        public void Start()
        {
            try
            {
                listener.Start();

                Console.WriteLine($"Server started [Port:{port}]");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void Stop()
        {
            try
            {
                listener.Stop();

                Console.WriteLine($"Server stopped [Port:{port}]");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
