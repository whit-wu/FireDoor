using System;
using System.Diagnostics;
using FireDoor.Services;


namespace FireDoor { 
    class Program
    {
        static void Main(string[] args)
        {
            TestAppService test = new TestAppService();
            ProcessStartInfo appToTest = test.GetTestApp();
            CpuTempService cpuTempService = new CpuTempService(appToTest);
            var results = cpuTempService.MeasureTemperature();

            Console.WriteLine($"Termination result: {results.termReason}");

            if (results.coreName != null)
            {
              
                Console.WriteLine($"Core that passed threshold: {results.coreName}");
                Console.WriteLine($"Temp of {results.coreName}: {results.coreTemp} degress C.");
                
            }

            Console.WriteLine($"Total uptime {results.appRunTime.Elapsed.ToString(@"hh\:mm\:ss\:fff")}");

            Console.ReadLine();
        }
    }
}