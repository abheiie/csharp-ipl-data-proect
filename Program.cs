using System;
using System.IO;
using System.Globalization;
using System.Data;
using CsvHelper;

namespace IPLDataProject
{
    class IPLAnalysis
    {
        static void Main(string[] args)
        {
            var matches = ReadMatches();

            //print data in matches data table
            foreach (DataRow dataRow in matches.Rows)
            {
                foreach (var item in dataRow.ItemArray)
                {
                    Console.WriteLine(item);
                }
            }
        }

        static DataTable ReadMatches()
        {
            string filePath = Path.GetFullPath(@"IPLDataset/matches.csv");
            using (var reader = new StreamReader(filePath))
            using (var csvObject = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                using (var dr = new CsvDataReader(csvObject))
                {
                    var matchesataTable = new DataTable();
                    matchesataTable.Load(dr);
                    return matchesataTable;
                }
            }
        }
    }
}
