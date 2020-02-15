using System;

namespace GligerFlightBook
{
    class Program
    {
        
        static void Main(string[] args)
        {
            
            Flight.Flight._WorkingFolderPath = @"/home/noar/Documents/parapente/Traces/";
            string[] KmlFileName = {
                "2020-01-25-XTR-9DC0890216A9-01.KML",
                "2020-02-08-XTR-9DC0890216A9-01.KML",
                "2020-02-15-XTR-9DC0890216A9-01.KML"
                };
            string[] GliderName = {
                "Mescal4",
                "Epsilon 9",
                "Epsilon 9"
            };
            string[] TakeOffSiteName = {
                "Montoz Sud",
                "Boujean Est",
                "Boujean Est"
            };
            string[] LandingSiteName = {
                "La Heute",
                "Boujean",
                "Boujean"
            };
            string[] FlightType = {
                "HF",
                "HF",
                "HF"
            };
            string[] Comment = {
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
            ;
        }
    }
}
