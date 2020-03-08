using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace GliderFlightBook
{
    public class LogBookConsoleInterract
    {

        public static string ChooseFileToImport()
        {
            Console.WriteLine("Available Kml file are :");
            DirectoryInfo di = new DirectoryInfo(Flight.Flight._WorkingFolderPath);
            FileSystemInfo[] filesInfo = di.GetFileSystemInfos("*.KML");
            var orderedFiles = filesInfo.OrderBy(f => f.CreationTime);
            // propose file to import for the user
            int i = 0;
            foreach(var file in orderedFiles)
            {
                Console.WriteLine(i + ": " + file.Name);
                ++i;
            }
            Console.WriteLine("What file do you want to import? [select by index]");
            string answer = Console.ReadLine();
            int FileIndexToImport;
            while (!Int32.TryParse(answer, out FileIndexToImport))
            {
                Console.WriteLine("index must be an integer in the precedent list");
                answer = Console.ReadLine();
            }
            string FilenameToImport = orderedFiles.ElementAt(FileIndexToImport).Name;
            return FilenameToImport;
        }

        public static int ChooseTakeOffSite()
        {
            List<SiteModel> MySiteCollection = SqliteDataAccess.LoadSites();
            foreach (SiteModel Site in MySiteCollection)
            {
                Console.WriteLine(Site.SiteID + ": " + Site.SiteName);
            }
            Console.WriteLine("Where did you take off? [select by index or 'n' to add a new site]");
            string answer = Console.ReadLine();
            int TakeOffSiteID;
            if (answer == "n")
            {
                TakeOffSiteID = AddNewSite();
            }
            else
            {
                while (!Int32.TryParse(answer, out TakeOffSiteID))
                {
                    Console.WriteLine("index must be an integer in the precedent list");
                    answer = Console.ReadLine();
                }
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
            Console.WriteLine("Where did you land? [select by index or 'n' to add a new site]");
            string answer = Console.ReadLine();
            int LandingSiteID;
            if (answer == "n")
            {
                LandingSiteID = AddNewSite();
            }
            else
            {
                while (!Int32.TryParse(answer, out LandingSiteID))
                {
                    Console.WriteLine("index must be an integer in the precedent list");
                    answer = Console.ReadLine();
                }
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
            Console.WriteLine("Which Glider did you use? [select by index or 'n' to add a new site]");
            string answer = Console.ReadLine();
            int UsedGliderID;
            if (answer == "n")
            {
                UsedGliderID = AddNewGlider();
            }
            else
            {
                while (!Int32.TryParse(answer, out UsedGliderID))
                {
                    Console.WriteLine("index must be an integer in the precedent list");
                    answer = Console.ReadLine();
                }
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
                FlightModel Myflight = new FlightModel(0, Flight.TakeOffSiteID, Flight.LandingSiteID, Flight.GliderID, Flight.GetTakeOffDateString(),
                Flight.GetFlownDistance(0), Flight.GetDuration().TotalSeconds, Flight.GetCumulativeElevation(),
                Flight.GetMaximumHeight(), Flight.FlightFilename, Flight.FlightType, Comment);
                SqliteDataAccess.SaveFlights(Myflight);

                int FlightID = SqliteDataAccess.LoadLastSavedFlight().FlightID;
                List<TraceSampleModel> FlightTrace = new List<TraceSampleModel>();
                for (int i = 0; i < Flight.FlightHeights.Length; ++i)
                {
                    FlightTrace.Add(new TraceSampleModel(0, FlightID, Flight.FlightLatitudes[i], Flight.FlightLongitudes[i], Flight.FlightHeights[i]));
                }
                SqliteDataAccess.SaveFlightTraceSamples(FlightTrace);
                Console.WriteLine("Data written");

            }
            else
            {
                Console.WriteLine("Data writing aborted");
            }
        }
        public static int AddNewSite()
        {
            Console.WriteLine("What is the name of the new site?");
            string answer;
            string SiteName = Console.ReadLine();
            Console.WriteLine("What is the new site latitude in [°]?");
            answer = Console.ReadLine();
            double SiteLatitude;
            while (!double.TryParse(answer, Globals.NumberStyle, Globals.CultureEN, out SiteLatitude))
            {
                Console.WriteLine("Latitude must be a real number");
                answer = Console.ReadLine();
            }
            Console.WriteLine("What is the new site longitude in [°]?");
            answer = Console.ReadLine();
            double SiteLongitude;
            while (!double.TryParse(answer, Globals.NumberStyle, Globals.CultureEN, out SiteLongitude))
            {
                Console.WriteLine("Longitude must be a real number");
                answer = Console.ReadLine();
            }
            Console.WriteLine("Waht is the new site altitude in [m]?");
            answer = Console.ReadLine();
            double SiteAltitude;
            while (!double.TryParse(answer, Globals.NumberStyle, Globals.CultureEN, out SiteAltitude))
            {
                Console.WriteLine("Altitude must be a real number");
                answer = Console.ReadLine();
            }
            Console.WriteLine("What is the new site radius in [m]? Typical value is 50[m]");
            answer = Console.ReadLine();
            double SiteRadius;
            while (!double.TryParse(answer, Globals.NumberStyle, Globals.CultureEN, out SiteRadius))
            {
                Console.WriteLine("Site radius must be a real number");
                answer = Console.ReadLine();
            }

            SiteModel NewSite = new SiteModel(0, SiteName, SiteLatitude, SiteLongitude, SiteAltitude, SiteRadius);
            SqliteDataAccess.SaveSite(NewSite);
            int NewSiteID = SqliteDataAccess.LoadLastSavedSite().SiteID;
            return NewSiteID;
        }
        public static int AddNewGlider()
        {
            Console.WriteLine("What is the brand of the new glider?");
            string Brand = Console.ReadLine();
            Console.WriteLine("What is the model of the new glider?");
            string Model  = Console.ReadLine();
            Console.WriteLine("What is the EN certifcation of the new glider? [0 = not certified, 1 = A, 2 = B, 3 = C, 4 = D or 5 = CCC]");
            string answer = Console.ReadLine();
            int EnCertification;
            while(!Int32.TryParse(answer, out EnCertification))
            {
                Console.WriteLine("EN Certification must be one of the following: [0 = not certified, 1 = A, 2 = B, 3 = C, 4 = D or 5 = CCC]");
                answer = Console.ReadLine();
            }
            GliderModel NewGlider  = new GliderModel(0, Brand, Model, EnCertification);
            SqliteDataAccess.SaveGlider(NewGlider);
            int NewGliderID = SqliteDataAccess.LoadLastSavedGlider();
            return NewGliderID;
        }
    }
}