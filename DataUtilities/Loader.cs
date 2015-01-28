using System.IO;
using DataStructures.Matrices;

namespace DataUtilities
{
    public class MachineLearningDataSet
    {
        public Vector<double>[] InputData { get; set; }
        public string[] ClassLabels { get; set; }
    }

    public class Loader
    {

        public static MachineLearningDataSet LoadFromFile(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            var labels = new string[lines.Length];
            var data = new Vector<double>[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                string[] split = line.Split(',');
                int dataLength = split.Length - 1;
                var dataRow = new Vector<double>(dataLength);
                for (int j = 0; j < split.Length; j++)
                {
                    var str = split[j];
                    if (j < dataLength)
                    {
                        double number;
                        dataRow[j] = (double.TryParse(str, out number)) ? number : double.NaN;
                    }
                    else
                    {
                        labels[i] = str;
                    }
                }
                data[i] = dataRow;
            }
            return new MachineLearningDataSet { ClassLabels = labels, InputData = data };
        }

    }
}
