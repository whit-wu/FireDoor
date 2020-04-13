using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenHardwareMonitor.Hardware;

namespace FireDoor.Services
{
    public class CpuTempService
    {
        private readonly Process proc;

        private float? coreTemp;

        private List<float?> coreTemps = new List<float?>();

        private int maxTemp;

        private string coreName;

        private TempWriterService writerService = new TempWriterService();

        private Stopwatch appUpTime = new Stopwatch(); 

        public CpuTempService(ProcessStartInfo appToTest)
        {
            maxTemp = PromptForMaxTemp();
            proc = Process.Start(appToTest);
            appUpTime.Start();
        }

        /// <summary>
        /// Public method that calls IsCPUTooHot until a CPU core temp
        /// exceeds the max temp specified by the user OR the user termiates
        /// the process tied to the test app.
        /// </summary>
        /// <returns>
        /// Returns a tuple with the following data (in order)
        /// termReason = Termination reason
        /// coreName = Name of core that trigged termination
        /// coreTemp = Temp of core that triggered termination (value is zero if program was terminated by user)
        /// appRunTime = The total time the test app ran before termination
        /// </returns>
        public (string termReason, string coreName, float? coreTemp, Stopwatch appRunTime) MeasureTemperature()
        {
            bool firstIteration = true;
            Console.WriteLine($"FireDoor is now running, and will terminate upon exit or when CPU reaches {maxTemp} degress Celcius.");
            while (!IsCPUTooHot())
            {
                Process[] pname = Process.GetProcessesByName(proc.ProcessName);
                if (pname.Length == 0)
                {
                    Console.Clear();
                    writerService.ReadCoreData();
                    appUpTime.Stop();
                    return ("Process terminated by user", null, 0, appUpTime);
                }
                
                // The original intention was to have this loop
                // run once every second, but this causes some
                // apps to crash.  The reason for this is that
                // when testing some apps, GetProcessByName 
                // will see our process on the first iteration, 
                // but fail to see it afterwards.  This issue
                // was discovered when attempting to get DOOM
                // to run, and the fix was to have FireDoor
                // sleep for ten seconds for the first iteration.
                // After that, the process is able to be found
                // at 1 second iterations.
                if (firstIteration)
                {
                    // sleep once for 10 seconds
                    System.Threading.Thread.Sleep(10000);
                    firstIteration = false;
                }
                else
                {
                    // sleep for 1 second
                    System.Threading.Thread.Sleep(1000);
                }
            }
            KillOCTestApp();
            writerService.ReadCoreData();
            appUpTime.Stop();
            return ("Core temp reached max threshold", coreName, coreTemp, appUpTime);
        }
        
        /// <summary>
        /// Checks if the CPU core temps exceed the max temp specified by the user.
        /// </summary>
        /// <returns> TRUE if the temp exceeds the max value.  FALSE if the temp is lower than the max value</returns>
        private bool IsCPUTooHot()
        {
            // The updateVisitor and computer objects are
            // what allow us to get our tempratures from 
            // OpenHardwareMonitor.  
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            computer.CPUEnabled = true;
            computer.Accept(updateVisitor);

            for (int i = 0; i < computer.Hardware.Length; i++)
            {
                // Seek out the CPU
                if (computer.Hardware[i].HardwareType == HardwareType.CPU)
                {
                    for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                    {
                        coreTemp = computer.Hardware[i].Sensors[j].Value;
                        
                        // If we are measuring temps for a CPU core...
                        if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature && computer.Hardware[i].Sensors[j].Name.ToLower().Contains("core"))
                        {
                            //_coreTempInt = Convert.ToInt32(_coreTemp);
                            coreName = computer.Hardware[i].Sensors[j].Name;
                            coreTemps.Add(coreTemp);

                            if (coreTemp > maxTemp)
                            {
                                Console.Clear();
                                Console.WriteLine($"Core temp reached {coreTemp}.  Max temp allowed is {maxTemp}.");
                                return true;
                            }
                        }
                    }
                    writerService.WriteTempData(coreTemps);
                    coreTemps.Clear();
                    return false;
                }
            }

            computer.Close();
            return false;
        }

        /// <summary>
        /// Kills the process tied to the app the user is testing.
        /// </summary>
        public void KillOCTestApp()
        {
            proc.Kill();
        }

        /// <summary>
        /// Method that asks user to input the maximum temp they wish a CPU core to reach.
        /// </summary>
        /// <returns>Max temp of CPU core</returns>
        public static int PromptForMaxTemp()
        {
            bool confirmed = false;
            string Key;
            do
            {
                Console.WriteLine("Please enter the max temp (in C) that you are willing your CPU to reach while testing and hit enter.");
                Console.WriteLine("Or press enter without entering a value to use the default max tem (60 degrees C).");
                Console.Write("Max temp: ");

                Key = Console.ReadLine();
                Console.Clear();
                var isNumeric = int.TryParse(Key, out int n);

                if (!isNumeric)
                {
                    Console.WriteLine("Error: Value entered is not a number.");
                    Console.WriteLine("Please press any key to try again.");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                // the assigned value is a placeholder as ConsoleKey types can never be null
                ConsoleKey response = ConsoleKey.UpArrow;
                if (n > 60)
                {
                    do
                    {
                        Console.WriteLine($"Inputted temp {n.ToString()} C greater than reccomended temp of 60 C?");
                        Console.Write("Are you sure you wish to move forward with testing at this temp? [y/n] ");
                        response = Console.ReadKey(false).Key;   // true is intercept key (dont show), false is show
                        if (response != ConsoleKey.Enter)
                            Console.WriteLine();
                    }
                    while (response != ConsoleKey.Y && response != ConsoleKey.N);

                    if (response == ConsoleKey.N)
                    {
                        Console.Clear();
                        continue;
                    }
                    return n;
                }
                else
                {
                    return n;
                }

            } while (!confirmed);
            Console.ReadLine();
            return 0;
        }
    }
}
