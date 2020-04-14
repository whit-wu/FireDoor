using System;
using System.Diagnostics;
using System.IO;

namespace FireDoor.Services
{
    public class TestAppService
    {
        private string testApp;

        private bool validPath = false;

        // Array of Digital Download launchers (have only confirmed steam works).
        private readonly string[] launchers = { "gog", "steam", "epic", "uplay" };

        /// <summary>
        /// Prompts the user to enter the path to the app they wish to test.
        /// </summary>
        /// <returns>A ProcessStartInfo object that contains information about the test app</returns>
        public ProcessStartInfo GetTestApp()
        {
            ProcessStartInfo processInfo = new ProcessStartInfo(); ;
            while (!validPath)
            {
                Console.WriteLine("Please enter the full path to the exe you wish to run for testing (with exe file included).");
                testApp = Console.ReadLine();
                Console.Clear();

                if (testApp.ToLower().Contains(".exe") && File.Exists(testApp))
                {
                    bool hasLauncher = Array.Exists(launchers, element => testApp.ToLower().Contains(element.ToLower()));
                    
                    if (hasLauncher)
                    {
                        Console.Clear();
                        Console.WriteLine("It appears the game you wish to test uses a launcher from a digital games provider to startup before running.");
                        Console.WriteLine("Firedoor needs the test app to run before it can read your CPU temp.");
                        Console.WriteLine("To do this, your launcher must be running first.");
                        Console.WriteLine("Please sign into or startup your lancher now, then press enter once it is running.");
                        Console.ReadLine();
                        Console.Clear();
                    }

                    processInfo.FileName = testApp;
                    processInfo.ErrorDialog = true;
                    processInfo.UseShellExecute = false;
                    processInfo.RedirectStandardOutput = true;
                    processInfo.RedirectStandardError = true;
                    processInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(testApp);

                    validPath = true;
                }
                else
                {
                    Console.WriteLine("Entry is not a valid path or does not specify exe.  Please verify your path and try again.");
                }
            }
            return processInfo;
        }
    }
}
