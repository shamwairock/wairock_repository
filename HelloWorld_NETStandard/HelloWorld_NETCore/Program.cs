using System;
using HelloWorld_NETStandard;

namespace HelloWorld_NETCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            DeviceSimulation deviceSimulation = new DeviceSimulation();
            Console.WriteLine(deviceSimulation.GetStatus());
            Console.Read();
        }
    }
}
