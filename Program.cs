using System;
using System.IO;
using System.Globalization;
using System.Data;
using CsvHelper;
using System.Collections.Generic;
using System.Linq;

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


            var deliveries = ReadDeliveries();

            //print data in deliveries DataTable
            foreach (DataRow dataRow in deliveries.Rows)
            {
                foreach (var item in dataRow.ItemArray)
                {
                    Console.WriteLine(item);
                }
            }

            // Result, Total number of matches played in all seasons
            MatchesPlayedPerSeason(matches);

        }

        //Matches Played Per Seasons
        static void MatchesPlayedPerSeason(DataTable matches)
        {
            
           var matchesIEnumerable = matches.AsEnumerable();
            var matchesPerSeason = from match in matchesIEnumerable 

                          group match by match.Field<string>("season") into matchesEachSeason
                          orderby matchesEachSeason.Key
                          select new
                          {
                              season = matchesEachSeason.Key,
                              matches = matchesEachSeason.Count()
                          };
            foreach (var item in matchesPerSeason)
            {
                Console.WriteLine($"{item.season}, {item.matches}");
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
                    deliveriesTable.Columns.Add("wide_runs", typeof(int));
                    deliveriesTable.Columns.Add("noball_runs", typeof(int));
                    deliveriesTable.Columns.Add("extra_runs", typeof(int));
                    deliveriesTable.Columns.Add("total_runs", typeof(int));
                    deliveriesTable.Load(dr);
                    return deliveriesTable;
                }
            }
        }
    }
}
