using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireDoor.Models;
using OpenHardwareMonitor.Hardware;

namespace FireDoor.Services
{
    public class CpuTempService
    {

        public CpuTempService(Process proc)
        {
            this._proc = proc;
        }


        private Process _proc;
        
        // TODO: instead of returning bool, return tuple with last temps and reason
        // for termination
        public bool MeasureTemperature()
        {
            while (!IsCPUTooHot())
            {
                Process[] pname = Process.GetProcessesByName(_proc.ProcessName);
                if (pname.Length == 0)
                {
                    return false;
                }
                System.Threading.Thread.Sleep(10000);
                Console.Clear();
            }
            KillOCTestApp();
            return false;
        }
        
        private bool IsCPUTooHot()
        {
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            computer.CPUEnabled = true;
            computer.Accept(updateVisitor);
            for (int i = 0; i < computer.Hardware.Length; i++)
            {
                if (computer.Hardware[i].HardwareType == HardwareType.CPU)
                {
                    for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                    {
                        var coreTemp = computer.Hardware[i].Sensors[j].Value;
                        if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature && computer.Hardware[i].Sensors[j].Name.ToLower().Contains("core"))
                        {
                            Console.WriteLine(computer.Hardware[i].Sensors[j].Name + ":" + coreTemp.ToString() + "\r");
                            if (coreTemp > 60)
                            {
                                Console.WriteLine($"Core temp reached {Convert.ToInt32(coreTemp)}.  Max temp allowed is 80.");
                                return true;
                            }
                            Console.WriteLine();
                        }

                    }
                    return false;
                }
            }
            computer.Close();
            return false;
        }

        public void KillOCTestApp()
        {
            Console.WriteLine("Killing process");
            _proc.Kill();
            Console.WriteLine("Test complete");
            Console.ReadLine();
        }
    }
}
