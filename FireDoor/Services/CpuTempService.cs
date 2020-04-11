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
        private readonly Process _proc;

        private int _coreTempInt;

        private List<int> _coreTemps = new List<int>();

        private int _maxTemp;

        private string _coreName;

        private ReportWriterService writerService = new ReportWriterService();

        public CpuTempService(ProcessStartInfo appToTest, int maxTemp = 60)
        {
            _proc = Process.Start(appToTest);
            _maxTemp = maxTemp;
        }
        
        public (string, string, int) MeasureTemperature()
        {
            while (!IsCPUTooHot())
            {
                Process[] pname = Process.GetProcessesByName(_proc.ProcessName);
                if (pname.Length == 0)
                {
                    return ("Process terminated by user", null, 0);
                }
                Console.WriteLine("----");
                System.Threading.Thread.Sleep(10000);
            }
            KillOCTestApp();
            return ("Core temp reached max threshold", _coreName, _coreTempInt);
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
                        var _coreTemp = computer.Hardware[i].Sensors[j].Value;
                        if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature && computer.Hardware[i].Sensors[j].Name.ToLower().Contains("core"))
                        {
                            _coreTempInt = Convert.ToInt32(_coreTemp);
                            _coreName = computer.Hardware[i].Sensors[j].Name;
                            _coreTemps.Add(_coreTempInt);

                            Console.Write(_coreTemp);
                            if (_coreTemp > _maxTemp)
                            {
                                Console.WriteLine($"Core temp reached {_coreTempInt}.  Max temp allowed is {_maxTemp}.");
                                return true;
                            }
                            Console.WriteLine();
                        }
                    }
                    writerService.WriteTempData(_coreTemps);
                    _coreTemps.Clear();
                    return false;
                }
            }

            computer.Close();
            return false;
        }

        public void KillOCTestApp()
        {
            _proc.Kill();
        }
    }
}
