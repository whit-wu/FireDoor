using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;

namespace FireDoor.Services
{
    public class TempWriterService
    {
        private string appDirectory;
        private string currentDateTime;
        private List<string[]> csvCoreTemps = new List<string[]>();
        private List<decimal> averageTemps = new List<decimal>();
        
        // The constructor is used to change our directory 
        // to the home directory of the project.  This is
        // where coreTempList.csv will live.  If the directory
        // is not changed, we would have to specfiy a full 
        // qualifed path.  
        public TempWriterService()
        {
            Environment.CurrentDirectory = "../..";
            appDirectory = Environment.CurrentDirectory;
            currentDateTime = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
        }

        /// <summary>
        /// method to write data to csv
        /// </summary>
        /// <param name="coreTemps"></param>
        public void WriteTempData(List<float?> coreTemps)
        {
            string coreTempFile =  Path.Combine(appDirectory, $"coreTempList_{currentDateTime}.csv");
            
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

        /// <summary>
        /// Reads data from CSV and stores it in memory for processing
        /// </summary>
        public void ReadCoreData()
        {
            if (File.Exists($"coreTempList_{ currentDateTime}.csv"))
            {
                using (TextFieldParser parser = new TextFieldParser($"coreTempList_{currentDateTime}.csv"))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    while (!parser.EndOfData)
                    {
                        //Processing row
                        string[] fields = parser.ReadFields();
                        csvCoreTemps.Add(fields);
                    }

                    int _numberOfRows = csvCoreTemps.Count;
                    int _numberOfColumns = csvCoreTemps[0].Length;

                    // loop through each column
                    for (int i = 0; i < _numberOfColumns; i++)
                    {
                        List<float?> coreTemps = new List<float?>();


                        // now pick up the temps from each entry in the
                        // column we are in
                        for (int j = 0; j < _numberOfRows; j++)
                        {
                            //var coreTemp = (_csvCoreTemps[j][i]);
                            var coreTemp = float.Parse(csvCoreTemps[j][i], CultureInfo.InvariantCulture.NumberFormat);
                            coreTemps.Add(coreTemp);
                        }

                        // now calculate the average temp of the core and
                        // put it in the _average temps arraylist
                        averageTemps.Add(CalcAvgTempOfEachCore(coreTemps));
                    }
                }
                WriteAvgTemps();
            }
            else
            {
                Console.WriteLine("Unable to generate report.  The CPU core temp thresehold was exceeded before any meaningful data could be recorded.");
            }
        }

        /// <summary>
        /// calculates average temp for each core
        /// </summary>
        /// <param name="coreTemps">A list of tempratures recorded for a specific core</param>
        /// <returns>Average temp of a specific core</returns>
        private decimal CalcAvgTempOfEachCore(List<float?> coreTemps)
        {
            int sumOfTemps = (int)coreTemps.Sum();

            decimal avgTempForCore = (decimal)sumOfTemps / coreTemps.Count;

            return avgTempForCore;
        }

        /// <summary>
        /// Writes out the average temp of each core
        /// </summary>
        private void WriteAvgTemps()
        {
            int coreCounter = 1;
            foreach (var temp in averageTemps)
            {
                Console.WriteLine($"Average temp for core {coreCounter.ToString()}: { Math.Round(temp, 2)}");
                coreCounter++;
            }
        }

    }
}
