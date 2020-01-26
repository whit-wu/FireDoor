using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenHardwareMonitor.Hardware;

namespace FireDoor { 
    class Program
    {
        public class UpdateVisitor : IVisitor
        {
            public void VisitComputer(IComputer computer)
            {
                computer.Traverse(this);
            }
            public void VisitHardware(IHardware hardware)
            {
                hardware.Update();
                foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
            }
            public void VisitSensor(ISensor sensor) { }
            public void VisitParameter(IParameter parameter) { }
        }

        public static bool IsCPUTooHot()
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
                            if (coreTemp > 20)
                            {
                                Console.WriteLine($"Core temp reached {Convert.ToInt32(coreTemp)}.  Max temp allowed is 80.");
                                return true;
                            }
                            Console.WriteLine();
                            return false;
                        }
                           
                    }
                }
            }
            computer.Close();
            return false;
        }

        static void KillOCTestApp()
        {
            Console.WriteLine("Killing process");
            foreach (var process in Process.GetProcessesByName("chrome"))
            {
                process.Kill();
            }
        }

        static void Main(string[] args)
        {
            bool IsTooHot = IsCPUTooHot();

            while (!IsTooHot)
            {
                IsTooHot = IsCPUTooHot();
                System.Threading.Thread.Sleep(10000);
                Console.Clear();
            }

            KillOCTestApp();
            
        }
    }
}