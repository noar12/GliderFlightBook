using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace GliderFlightBook
{

    class Program
    {


        static void Main(string[] args)
        {

            if (args.Length == 1)
            {
                Flight.Flight._WorkingFolderPath = args[0];
                SqliteDataAccess.SetDBPath(args[0]);
            }
            else if (args.Length == 2)
            {
                Flight.Flight._WorkingFolderPath = args[1];
                SqliteDataAccess.SetDBPath(args[0]);
            }
            else
            {
                Console.WriteLine("Usage: \n GliderFlightBook <pathToDb> <PathToWorkingDir> \n or \n GliderFlightBook <pathToDbAndWorkingDir>");
                System.Environment.Exit( 1 );
            }
            bool OneMore = true;
            while (OneMore)
            {

                string FilenameToImport = LogBookConsoleInterract.ChooseFileToImport();
                int TakeOffSiteID = LogBookConsoleInterract.ChooseTakeOffSite();
                int LandingSiteID = LogBookConsoleInterract.ChooseLandingSite();
                // To be done -> Add a new site
                int UsedGliderID = LogBookConsoleInterract.ChooseGlider();
                // to be done -> Add new glider


                // User can choose a type of flight between Local:0 XC:1 or HF:2
                // to be done FlightType must be an enum
                int FlightType = LogBookConsoleInterract.ChooseFlightType();

                //Instance a new Flight based on kml file
                Flight.IFlight SelectedFlight = new Flight.Flight(FilenameToImport, TakeOffSiteID, LandingSiteID, UsedGliderID, FlightType);

                // User can now verify the imported data
                SelectedFlight.Display();
                // User can write a free comment
                Console.WriteLine("You can now write an optional comment: ");
                string Comment = Console.ReadLine();

                //Last confirmation from the user before writing in the db
                LogBookConsoleInterract.WriteFlightInDB(SelectedFlight, Comment);

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

