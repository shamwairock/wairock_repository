using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelloWorld_NETStandard;

namespace HelloWorld_NETFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            DeviceSimulation deviceSimulation = new DeviceSimulation(); 
            Console.Write(deviceSimulation.GetStatus());

            Console.Read();
        }
    }
}
