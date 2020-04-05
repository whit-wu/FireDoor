using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FireDoor.Services
{
    public class ReportWriterService
    {

        // method to write data to csv
        public void WriteTempData(List<int> coreTemps)
        {

            string coreTempFile = "c:/test1.csv";

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
    }
}
