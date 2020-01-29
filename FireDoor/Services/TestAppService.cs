using FireDoor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireDoor.Services
{
    public class TestAppService
    {
        private List<TestApp> testApps;

        public TestAppService()
        {

        }


        /// <summary>
        /// Looks for GOG and Steam directories on C for .exe files
        /// and saves them to a text file.
        /// Optioal paramter exists so user can input a custom path.
        /// If exe found in custom path, let user enter path.
        /// Inform user that any GOG and Steam apps installed in
        /// custom path will need to use the custom path flag.
        /// </summary>
        /// <returns>List<TestApp></returns>
        public List<TestApp> GetTestApps(bool isCustomPath = false, string customPath = null)
        {
           

            string steamApps = @"C:\Program Files (x86)\Steam\steamapps\common";

            List<string> files = Directory.GetFiles(steamApps, "*.exe",
                                    SearchOption.AllDirectories).ToList();


            //TODO: Look through GOG dirs.  Also, check if isCustomPath is true.
            //  If it is, look at user provided path.  Save all entries to txt file.
            return testApps;
        }
    }
}
