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
            CpuTempService cpuTempService = new CpuTempService();
            cpuTempService.MeasureTemperature();
            
        }
    }
}