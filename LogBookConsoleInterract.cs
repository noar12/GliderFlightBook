using System;
using System.IO;
using System.Collections.Generic;

namespace GliderFlightBook
{
    public class LogBookConsoleInterract
    {

        public static string ChooseFileToImport()
        {
            Console.WriteLine("Available Kml file are :");
            DirectoryInfo di = new DirectoryInfo(Flight.Flight._WorkingFolderPath);
            FileInfo[] fileInfo = di.GetFiles("*.KML");

            // propose file to import for the user
            for (int i = 0; i < fileInfo.Length; i++)
            {
                Console.WriteLine(i + ": " + fileInfo[i].Name);
            }
            Console.WriteLine("What file do you want to import? [select by index]");
            string answer = Console.ReadLine();
            int FileIndexToImport;
            while (!Int32.TryParse(answer, out FileIndexToImport))
            {
                Console.WriteLine("index must be an integer in the precedent list");
                answer = Console.ReadLine();
            }
            string FilenameToImport = fileInfo[FileIndexToImport].Name;
            return FilenameToImport;
        }

        public static int ChooseTakeOffSite()
        {
            List<SiteModel> MySiteCollection = SqliteDataAccess.LoadSites();
            foreach (SiteModel Site in MySiteCollection)
            {
                Console.WriteLine(Site.SiteID + ": " + Site.SiteName);
            }
            Console.WriteLine("Where did you take off? [select by index]");
            string answer = Console.ReadLine();
            int TakeOffSiteID;
            while (!Int32.TryParse(answer, out TakeOffSiteID))
            {
                Console.WriteLine("index must be an integer in the precedent list");
                answer = Console.ReadLine();
            }
            return TakeOffSiteID;
        }
        public static int ChooseLandingSite()
        {
            List<SiteModel> MySiteCollection = SqliteDataAccess.LoadSites();
            foreach (SiteModel Site in MySiteCollection)
            {
                Console.WriteLine(Site.SiteID + ": " + Site.SiteName);
            }
            Console.WriteLine("Where did you land? [select by index]");
            string answer = Console.ReadLine();
            int LandingSiteID;
            while (!Int32.TryParse(answer, out LandingSiteID))
            {
                Console.WriteLine("index must be an integer in the precedent list");
                answer = Console.ReadLine();
            }
            return LandingSiteID;
        }
        public static int ChooseGlider()
        {
            List<GliderModel> MyGliderCollection = SqliteDataAccess.LoadGliders();
            foreach (GliderModel Glider in MyGliderCollection)
            {
                Console.WriteLine(Glider.GliderID + ": " + Glider.LongGliderName);
            }
            Console.WriteLine("Which Glider did you use? [select by index]");
            string answer = Console.ReadLine();
            int UsedGliderID;
            while (!Int32.TryParse(answer, out UsedGliderID))
            {
                Console.WriteLine("index must be an integer in the precedent list");
                answer = Console.ReadLine();
            }
            return UsedGliderID;
        }
        public static int ChooseFlightType()
        {
            Console.WriteLine("Was this flight a Local Flight [0], a Hike and Fly[1] or a Cross Country [2]?");
            string answer = Console.ReadLine();
            int FlightType;
            while (!Int32.TryParse(answer, out FlightType))
            {
                Console.WriteLine("index must be an integer in the precedent list");
                answer = Console.ReadLine();
            }
            return FlightType;
        }
        public static void WriteFlightInDB(Flight.IFlight Flight, string Comment)
        {
            Console.WriteLine("Do you want to write these data in the database? [y/n]");
                    string DataWriteDecision = Console.ReadLine();
                    if (DataWriteDecision == "y")
                    {
                        FlightModel Myflight = new FlightModel(0,Flight.TakeOffSiteID, Flight.LandingSiteID, Flight.GliderID,Flight.GetTakeOffDateString(),
                        Flight.GetFlownDistance(0), Flight.GetDuration().TotalSeconds,Flight.GetCumulativeElevation(),
                        Flight.GetMaximumHeight(), Flight.FlightFilename,Flight.FlightType,Comment);
                        SqliteDataAccess.SaveFlights(Myflight);

                        int FlightID = SqliteDataAccess.LoadLastSavedFlight().FlightID;
                        List<TraceSampleModel> FlightTrace = new List<TraceSampleModel>();
                        for(int i=0;i<Flight.FlightHeights.Length;++i)
                        {
                            FlightTrace.Add(new TraceSampleModel(0,FlightID,Flight.FlightLatitudes[i],Flight.FlightLongitudes[i],Flight.FlightHeights[i]));
                        }
                        SqliteDataAccess.SaveFlightTraceSamples(FlightTrace);
                        Console.WriteLine("Data written");
                        
                    }
                    else
                    {
                        Console.WriteLine("Data writing aborted");
                    }
        }
    }
}