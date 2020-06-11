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
            Console.WriteLine("Result, Total number of matches played in all seasons");
            // MatchesPlayedPerSeason(matches);


            // Result, Top Economical bowlers of 2015
            Console.WriteLine("Result, Top Economical bowlers of 2015");
            // TopEconomicBowlersOf2015(matches, deliveries);


            // Result, Extra runs conceded per team in 2016
            Console.WriteLine("Result, Extra runs conceded per team in 2016");
            ExtraRunsConcededPerTeamIn2016(matches, deliveries);


            // Result, Total number of Wins per season
            Console.WriteLine("Result, Total number of Wins per season");
            // WinsPerSeason(matches);

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

        // Wins per seasons
        static void WinsPerSeason(DataTable matches)
        {
            var result = matches.AsEnumerable()
            .GroupBy(r => new { v = r.Field<string>("season"), w = r.Field<string>("winner") })
            .Select(r => new
            {
                season = r.Key.v,
                winner = r.Key.w,
                matches = r.Count()
            });
            foreach (var item in result)
            {
                Console.WriteLine($"{item.season}, {item.winner}, {item.matches}");
            }
        }

        // top economic bowlers of 2015
        static void TopEconomicBowlersOf2015(DataTable matches, DataTable deliveries)
        {
            var result = ((from delivery in deliveries.AsEnumerable()
                           join match in matches.AsEnumerable()
                           on delivery.Field<string>("match_id") equals match.Field<string>("Iid")
                           where match.Field<string>("season") == "2015" && Equals(delivery.Field<string>("is_super_over"), "0")
                           group delivery by delivery.Field<string>("bowler") into bowler
                           select new
                           {
                               bowler = bowler.Key,
                               economy = bowler.Sum(r => r.Field<int>("total_runs")) * 6 / ((float)bowler.Count() - (float)bowler.Count(s => s.Field<int>("wide_runs") == 1 || s.Field<int>("noball_runs") == 1))
                           }).OrderBy(x => x.economy)).Take(10);
            foreach (var item in result)
            {
                Console.WriteLine($"{item.bowler}, {item.economy}");
            }
        }

        //Extra runs conceded per team
        static void ExtraRunsConcededPerTeamIn2016(DataTable matches, DataTable deliveries)
        {
            var result = from delivery in deliveries.AsEnumerable()
                         join match in matches.AsEnumerable()
                         on delivery.Field<string>("match_id") equals match.Field<string>("Iid")
                         where match.Field<string>("season") == "2016"
                         group delivery by delivery.Field<string>("bowling_team") into teams
                         select new
                         {
                             bowling_team = teams.Key,
                             extra_runs = teams.Sum((r)=> r.Field<int>("extra_runs"))
                         };
            foreach (var item in result)
            {
                Console.WriteLine($"{item.bowling_team}, {item.extra_runs}");
            }
        }



        //Reading matches data from matches.csv fie
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

        //Reading deliveries data from deliveries.csv fie
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
