using System;
using System.Collections.Generic;
using System.IO;

namespace GliderFlightBook
{
    class Program
    {

        static void Main(string[] args)
        {

            if (args.Length != 1)
            {
                Console.WriteLine("You must enter a path to the working directory; program will run statically");
                Flight.Flight._WorkingFolderPath = @"/home/noar/Documents/parapente/Traces";
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
            }
            else
            {
                Flight.Flight._WorkingFolderPath = args[0];
                SqliteDataAccess.SetDBPath(args[0]);

                bool OneMore = true;
                while (OneMore)
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

                    // User can choose a take off site from the database
                    // To be done -> Add a new site
                    List<SiteModel> MySiteCollection = SqliteDataAccess.LoadSites();
                    foreach (SiteModel Site in MySiteCollection)
                    {
                        Console.WriteLine(Site.SiteID + ": " + Site.SiteName);
                    }
                    Console.WriteLine("Where did you take off? [select by index]");
                    answer = Console.ReadLine();
                    int TakeOffSiteID;
                    while (!Int32.TryParse(answer, out TakeOffSiteID))
                    {
                        Console.WriteLine("index must be an integer in the precedent list");
                        answer = Console.ReadLine();
                    }
                    
                    // User can choose a Glider from the database
                    // to be done -> Add new glider
                    List<GliderModel> MyGliderCollection = SqliteDataAccess.LoadGliders();
                    foreach (GliderModel Glider in MyGliderCollection)
                    {
                        Console.WriteLine(Glider.GliderID + ": " + Glider.LongGliderName);
                    }
                    Console.WriteLine("Which Glider did you use? [select by index]");
                    answer = Console.ReadLine();
                    int UsedGliderID;
                    while(!Int32.TryParse(answer,out UsedGliderID))
                    {
                        Console.WriteLine("index must be an integer in the precedent list");
                        answer = Console.ReadLine();
                    }
                    
                    // User can choose a type of flight between Local:0 XC:1 or HF:1
                    Console.WriteLine("Was this flight a Local Flight [0], a Hike and Fly[1] or a Cross Country [2]?");
                    answer = Console.ReadLine();
                    int FlightType;
                     while(!Int32.TryParse(answer,out FlightType))
                    {
                        Console.WriteLine("index must be an integer in the precedent list");
                        answer = Console.ReadLine();
                    }

                    //Instance a new Flight based on kml file
                    Flight.IFlight SelectedFlight = new Flight.Flight(fileInfo[FileIndexToImport].Name);
                    
                    // User can now verify the imported data
                    Console.WriteLine("The data are the following:");
                    Console.WriteLine("Take off date/Time (local): " + SelectedFlight.GetTakeOffDateString());
                    Console.WriteLine("Take off site: " + MySiteCollection[TakeOffSiteID].SiteName); // to be done better. use a method to get site and glider by id directly from the data base
                    Console.WriteLine("Glider used: " + MyGliderCollection[UsedGliderID].LongGliderName);
                    Console.WriteLine("Flown distance [km]: " + (SelectedFlight.GetFlownDistance(0)/1000).ToString("F3"));
                    Console.WriteLine("Flight duration: " + SelectedFlight.GetDuration().ToString("c"));
                    Console.WriteLine("Cumulated elevation during flight [m]: " + SelectedFlight.GetCumulativeElevation());
                    Console.WriteLine("Maximal altitude reached during flight [m]: " + SelectedFlight.GetMaximumHeight().ToString("F3"));
                    Console.WriteLine("The declared flight type is [0: Local, 1: HF, 2: XC]: " + FlightType);
                    Console.WriteLine("You can now write an optional comment: ");
                    string comment = Console.ReadLine();
                    
                    //Last confirmation from the user before writing in the db
                    Console.WriteLine("Do you want to write these data in the database? [y/n]");
                    string DataWriteDecision = Console.ReadLine();
                    if (DataWriteDecision == "y")
                    {
                        FlightModel Myflight = new FlightModel(0,TakeOffSiteID,UsedGliderID,SelectedFlight.GetTakeOffDateString(),0,0,0,
                        SelectedFlight.GetFlownDistance(0), SelectedFlight.GetDuration().TotalSeconds,SelectedFlight.GetCumulativeElevation(),
                        SelectedFlight.GetMaximumHeight(), fileInfo[FileIndexToImport].Name,FlightType,comment);
                        SqliteDataAccess.SaveFlights(Myflight);
                        Console.WriteLine("Data written");
                        
                    }
                    else
                    {
                        Console.WriteLine("Data writing aborted");
                    }





                    Console.WriteLine(SelectedFlight.GetCSVExportLine("", "", "", "", ""));

                    Console.WriteLine("Continue? [y/n]");
                    string userAnswer = Console.ReadLine();
                    if (userAnswer != "y")
                    {
                        OneMore = false;
                    }
                }


            }


        }
    }
}
