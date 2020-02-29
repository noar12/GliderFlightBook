using System;
using System.Collections.Generic;

namespace GliderFlightBook
{
    class Program
    {

        static void Main(string[] args)
        {


            Flight.Flight._WorkingFolderPath = @"/home/noar/Documents/parapente/Traces/";
            string[] KmlFileName = {
                "2020-02-24-XTR-9DC0890216A9-01.KML",
                "2020-02-24-XTR-9DC0890216A9-02.KML",
                "2020-02-24-XTR-9DC0890216A9-03.KML",
                "2020-02-24-XTR-9DC0890216A9-04.KML",
                "2020-02-24-XTR-9DC0890216A9-05.KML",
                "2020-02-24-XTR-9DC0890216A9-06.KML"
                };
            string[] GliderName = {
                "Epsilon 9",
                "Epsilon 9",
                "Epsilon 9",
                "Epsilon 9",
                "Epsilon 9",
                "Epsilon 9"
            };
            string[] TakeOffSiteName = {
                "Oeschinen",
                "Oeschinen",
                "Oeschinen",
                "Oeschinen",
                "Oeschinen",
                "Oeschinen"
            };
            string[] LandingSiteName = {
                "Kandersteg",
                "Kandersteg",
                "Kandersteg",
                "Kandersteg",
                "Kandersteg",
                "Kandersteg"
            };
            string[] FlightType = {
                "Local",
                "Local",
                "Local",
                "Local",
                "Local",
                "Local"
            };
            string[] Comment = {
                "",
                "",
                "",
                "",
                "",
                ""
            };






            int index = 0;
            Flight.IFlight[] MyFlights = new Flight.Flight[KmlFileName.Length];
            foreach (string KmlFileNameItem in KmlFileName)
            {
                Flight.IFlight ThisFlight = new Flight.Flight(KmlFileNameItem);
                Console.WriteLine(ThisFlight.GetCSVExportLine(GliderName[index],
                                                                TakeOffSiteName[index],
                                                                LandingSiteName[index],
                                                                FlightType[index],
                                                                Comment[index]));
                ++index;
            }

            Console.WriteLine("Test sqlite database reading:");
            List<GliderModel> MyGliderCollection = SqliteDataAccess.LoadGliders();
            Console.WriteLine("to be done");

        }
    }
}
