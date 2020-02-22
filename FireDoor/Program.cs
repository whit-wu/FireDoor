using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireDoor.Services;


namespace FireDoor { 
    class Program
    {

        static void Main(string[] args)
        {
            TestAppService test = new TestAppService();
            Process appToTest = test.GetTestApp();

            CpuTempService cpuTempService = new CpuTempService(appToTest);

            (string, string, int) results = cpuTempService.MeasureTemperature();

            Console.Clear();
            Console.WriteLine($"Termination result: {results.Item1}.");

            if(results.Item2 != null)
            {
                Console.WriteLine($"Core that passed threshold: {results.Item2}");
                Console.WriteLine($"Temp of {results.Item2}: {results.Item3} degress C.");
            }

            Console.ReadLine();
        }
    }
}