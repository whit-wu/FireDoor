using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FireDoor.Services
{
    public class ReportWriterService
    {
        private string _appDirectory;
        
        // The constructor is used to change our directory 
        // to the home directory of the project.  This is
        // where coreTempList.csv will live.  If the directory
        // is not changed, we would have to specfiy a full 
        // qualifed path.  
        public ReportWriterService()
        {
            Environment.CurrentDirectory = "../..";
            _appDirectory = Environment.CurrentDirectory;
        }
        
        // method to write data to csv
        public void WriteTempData(List<int> coreTemps)
        {
            string coreTempFile =  Path.Combine(_appDirectory, "coreTempList.csv");
            
            Console.WriteLine(coreTempFile);
            if (!File.Exists(coreTempFile))
            {
                File.WriteAllText(coreTempFile, string.Join(",", coreTemps));
            }
            else
            {
                File.AppendAllText(coreTempFile, string.Join(",", coreTemps));
            }

            File.AppendAllText(coreTempFile, "\r\n");
        }

        // method to read data from csv

        // method to calculate average temp

        // method to write final report that shows average temp
        // and time that testapp ran before terminating

        // method to delete coreTempList.csv when program is done running
    }
}
