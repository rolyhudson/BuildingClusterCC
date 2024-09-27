using BH.oM.Data.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingClusterCC
{
    public static partial class Modify
    {
        public static void CombineJsonL()
        {
            using (StreamWriter sw = new StreamWriter(@"C:\Users\rhudson\source\repos\BuildingClusterCC\bin\Debug\trainingDatasets\allTraining.jsonl", false, encoding: Encoding.UTF8))
            {
                foreach (var file in Directory.GetFiles(@"C:\Users\rhudson\source\repos\BuildingClusterCC\bin\Debug\trainingDatasets"))
                {
                    if (file.Contains("allTraining.jsonl"))
                        continue;
                    // Read all lines from the file  
                    string[] lines = File.ReadAllLines(file);

                    // Write all lines to the output file  
                    foreach (string line in lines)
                    {
                        sw.WriteLine(line);
                    }
                }
            }
            
        }
    }
    

}
