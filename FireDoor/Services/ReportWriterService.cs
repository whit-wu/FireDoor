using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FireDoor.Services
{
    public class ReportWriterService
    {


        private string AppDirectory;
        
        public ReportWriterService()
        {
            Environment.CurrentDirectory = "../..";
            AppDirectory = Environment.CurrentDirectory;

        }
        
        // method to write data to csv
        public void WriteTempData(List<int> coreTemps)
        {

            string coreTempFile =  Path.Combine(AppDirectory, "coreTemplist.csv");
            

            
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
