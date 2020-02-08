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
