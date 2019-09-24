using System;
using System.Configuration;
using System.Threading.Tasks;

namespace TCPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting server...");

                var portStr = ConfigurationManager.AppSettings["ConfiguredPorts"];

                var ports = portStr.Split(',');

                foreach (var port in ports)
                {
                    IServer server = new TcpServer(int.Parse(port));

                    Task.Factory.StartNew(() => { server.Start(); });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.Read();
        }
    }
}
