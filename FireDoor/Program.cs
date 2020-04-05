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
            int tempToPass = PromptForMaxTemp();
            ProcessStartInfo appToTest = test.GetTestApp();
            CpuTempService cpuTempService = new CpuTempService(appToTest, tempToPass);
            (string, string, int) results = cpuTempService.MeasureTemperature();
            //Console.Clear();
            Console.WriteLine($"Termination result: {results.Item1}.");

            if(results.Item2 != null)
            {
                Console.WriteLine($"Core that passed threshold: {results.Item2}");
                Console.WriteLine($"Temp of {results.Item2}: {results.Item3} degress C.");
            }

            Console.ReadLine();
        }

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
                var isNumeric = int.TryParse(Key, out int n);

                if (!isNumeric)
                {
                    Console.Clear();
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