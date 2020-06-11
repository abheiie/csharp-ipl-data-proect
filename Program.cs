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

            //print data in matches DataTable
            foreach (DataRow dataRow in matches.Rows)
            {
                foreach (var item in dataRow.ItemArray)
                {
                    Console.WriteLine(item);
                }
            }

            Console.WriteLine("-----------------------------------=======");

            var deliveries = ReadDeliveries();

            //print data in deliveries DataTable
            foreach (DataRow dataRow in deliveries.Rows)
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

        static DataTable ReadDeliveries()
        {
            string filePath = Path.GetFullPath(@"IPLDataset/deliveries.csv");
            using (var reader = new StreamReader(filePath))
            using (var csvObject = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                using (var dr = new CsvDataReader(csvObject))
                {
                    var deliveriesTable = new DataTable();
                    // deliveries.Columns.Add("wide_runs", typeof(int));
                    // deliveries.Columns.Add("noball_runs", typeof(int));
                    // deliveries.Columns.Add("extra_runs", typeof(int));
                    // deliveries.Columns.Add("total_runs", typeof(int));
                    deliveriesTable.Load(dr);
                    return deliveriesTable;
                }
            }
        }
    }
}
