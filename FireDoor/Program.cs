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
            //TODO: Make project of unit tests so we don't need to 
            // have this junk in the main method.
            
            TestAppService test = new TestAppService();
            Process appToTest = test.GetTestApp();

            CpuTempService cpuTempService = new CpuTempService();
            cpuTempService.MeasureTemperature();

        }
    }
}