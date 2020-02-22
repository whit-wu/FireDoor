using FireDoor.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireDoor.Services
{
    public class TestAppService
    {
        private string _testApp;

        private bool _validPath = false;

        private Process _proc;

        private readonly string[] _launchers = { "gog", "steam", "epic", "uplay" };

        public TestAppService()
        {

        }

        public Process GetTestApp()
        {
            while (!_validPath)
            {
                Console.WriteLine("Please enter the full path to the exe you wish to run for testing (with exe file included).");
                _testApp = Console.ReadLine();

                if (_testApp.ToLower().Contains(".exe") && File.Exists(_testApp))
                {
                    bool hasLauncher = Array.Exists(_launchers, element => _testApp.ToLower().Contains(element.ToLower()));
                    
                    if (hasLauncher)
                    {
                        //TODO: See if we can't force the launcher to start
                        // and check if it is running.
                        Console.Clear();
                        Console.WriteLine("It appears the game you wish to test requires a launcher to startup before running.");
                        Console.WriteLine("Firedoor needs the test app to run before it can read your CPU temp.");
                        Console.WriteLine("To do this, your launcher must be running first.");
                        Console.WriteLine("Please sign into or startup your lancher now, then press enter once it is running.");
                        Console.ReadLine();
                    }
                    _proc = System.Diagnostics.Process.Start(_testApp);
                    _validPath = true;
                }
                else
                {
                    Console.WriteLine("Entry is not a valid path or does not specify exe.  Please verify your path and try again.");
                }
            }
            return _proc;
        }
    }
}
