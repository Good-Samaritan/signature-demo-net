using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CsvHelper;

namespace XadesDemo.Helpers
{
    public static class CsvHelper
    {
        private static char[] Separators => new [] {';'};
        private const string Separator = ";";

        public static void WriteCsv(string outputFile, IEnumerable<Tuple<string, string>> valuesDictionary)
        {
            using (var writer = new StreamWriter(outputFile, false, Encoding.UTF8))
            {
                var serializer = new CsvSerializer(writer);
                serializer.Configuration.Delimiter = Separator;
                serializer.Configuration.Encoding = Encoding.UTF8;
                serializer.Configuration.IgnoreBlankLines = true;
                serializer.Write(valuesDictionary.Select(x => x.Item1).ToArray());
                //используется замена "переводов строк" , "табуляции" и "сдвига каретки" на "пробел" для сохранения формата csv файла
                serializer.Write(valuesDictionary.Select(x => Regex.Replace(x.Item2, "\n|\r|\t", " ")).ToArray());
            }
        }

        public static IEnumerable<Tuple<string, string>> ReadCsv(string inputFile)
        {
            if (!File.Exists(inputFile))
            {
                throw new ArgumentException("Указан несуществующий файл с параметрами");
            }

            if (string.IsNullOrEmpty(inputFile))
            {
                return Enumerable.Empty<Tuple<string, string>>();
            }

            var lines = File.ReadAllLines(inputFile);

            return lines[0].Split(Separators)
                    .Zip(lines[1].Split(Separators), (xpath, value) => new { xpath, value })
                    .Select( item => new Tuple<string, string>( item.xpath, item.value));
        }
    }
}