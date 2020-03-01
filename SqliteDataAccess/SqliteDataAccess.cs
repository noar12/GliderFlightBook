using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;


namespace GliderFlightBook
{
    public class SqliteDataAccess
    {
        static string DBDirectory = ""; 
        public static List<GliderModel> LoadGliders()
        {
            string SqliteCmd = "select GliderID, Brand, Model, EnCertification  from Glider";

            using var connection = new SQLiteConnection(LoadConnectionString());
            connection.Open();
            using var cmd = new SQLiteCommand(SqliteCmd, connection);
            using SQLiteDataReader reader = cmd.ExecuteReader();
            List<GliderModel> Gliders = new List<GliderModel>();
            while (reader.Read())
            {

                Gliders.Add(new GliderModel(reader.GetInt32(0),
                                                reader.GetString(1),
                                                reader.GetString(2),
                                                reader.GetString(3)));
            }
            return Gliders;


        }

        public static void SaveGlider(GliderModel glider)
        {
            string SqliteCmd = "insert into Glider (Brand, Model, EnCertification) values('"
            + glider.Brand + "' , '"
            + glider.Model + "' , '"
            + glider.EnCertification + "')";

            using var connection = new SQLiteConnection(LoadConnectionString());
            connection.Open();
            using var cmd = new SQLiteCommand(SqliteCmd, connection);
            cmd.ExecuteNonQuery();



        }

        public static List<SiteModel> LoadSites()
        {
            string SqliteCmd = "select SiteID, SiteName, Latitude, Longitude, Altitude, Radius from Site";
            
            using var connection = new SQLiteConnection(LoadConnectionString());
            connection.Open();
            using var cmd = new SQLiteCommand(SqliteCmd, connection);
            using SQLiteDataReader reader = cmd.ExecuteReader();
            List<SiteModel> Sites = new List<SiteModel>();
            while(reader.Read())
            {
                Sites.Add(new SiteModel(reader.GetInt32(0),
                                        reader.GetString(1),
                                        reader.GetFloat(2),
                                        reader.GetFloat(3),
                                        reader.GetFloat(4),
                                        reader.GetFloat(5)));
            }
            return Sites;
        }
        public static void SaveSite(SiteModel site)
        {
            CultureInfo CH = new CultureInfo("en-EN");
            string SqliteCmd = "insert into Site (SiteName, Latitude, Longitude, Altitude, Radius) values('"
            + site.SiteName + "' , "
            + site.Latitude.ToString(CH) + " , "
            + site.Longitude.ToString(CH) + " ,"
            + site.Altitude.ToString(CH) + " ,"
            + site.Radius.ToString(CH) + ")";

            using var connection = new SQLiteConnection(LoadConnectionString());
            connection.Open();
            using var cmd = new SQLiteCommand(SqliteCmd, connection);
            cmd.ExecuteNonQuery();



        }

        public static List<FlightModel> LoadFlights()
        {
            string SqliteCmd = "select FlightID, SiteID, GliderID, Date, XContestType, XContestPoint, XContestDistance, FlownDistance, FlightDuration, CumulatedElevation, MaxAltitude, File, FlightType, Comment from Flight";

            using var connection = new SQLiteConnection(LoadConnectionString());
            connection.Open();
            using var cmd = new SQLiteCommand(SqliteCmd, connection);
            using SQLiteDataReader reader = cmd.ExecuteReader();
            List<FlightModel> Flights = new List<FlightModel>();
            while(reader.Read())
            {
                Flights.Add(new FlightModel(reader.GetInt32(0),
                                            reader.GetInt32(1),
                                            reader.GetInt32(2),
                                            reader.GetString(3),
                                            reader.GetInt32(4),
                                            reader.GetDouble(5),
                                            reader.GetDouble(6),
                                            reader.GetDouble(7),
                                            reader.GetDouble(8),
                                            reader.GetDouble(9),
                                            reader.GetDouble(10),
                                            reader.GetString(11),
                                            reader.GetInt32(12),
                                            reader.GetString(13)));
            }
            return Flights;
        }

        public static void SaveFlights(FlightModel flight)
        {
            CultureInfo CH = new CultureInfo("en-EN");

            string SqliteCmd = "insert into Flight (SiteID, GliderID, Date, XContestType, XContestPoint, XContestDistance, FlownDistance, FlightDuration, CumulatedElevation, MaxAltitude, File, FlightType, Comment) values("
            + flight.SiteID + ", "
            + flight.GliderID + ", '"
            + flight.Date + "', "
            + flight.XContestType + ", "
            + flight.XContestPoint.ToString(CH) + ", "
            + flight.XContestDistance.ToString(CH) + ", "
            + flight.FlownDistance.ToString(CH) + ", "
            + flight.FlightDuration.ToString(CH) + ", "
            + flight.CumulatedElevation.ToString(CH) + ", "
            + flight.MaxAltitude.ToString(CH) + ", '"
            + flight.File + "', "
            + flight.FlightType + ", '"
            + flight.Comment + "')";

            using var connection = new SQLiteConnection(LoadConnectionString());
            connection.Open();
            using var cmd = new SQLiteCommand(SqliteCmd, connection);
            cmd.ExecuteNonQuery();
        }

        public static List<TraceSampleModel> LoadTraceByFlightID(int FlightID)
        {
            string SqliteCmd = "select SampleID, FlightID, Latitude, Longitude, Altitude from TraceSample where FlightID = " + FlightID;
            
            using var connection = new SQLiteConnection(LoadConnectionString());
            connection.Open();
            using var cmd = new SQLiteCommand(SqliteCmd, connection);
            using SQLiteDataReader reader = cmd.ExecuteReader();
            List<TraceSampleModel> TraceSamples = new List<TraceSampleModel>();
            while(reader.Read())
            {
                TraceSamples.Add(new TraceSampleModel(reader.GetInt32(0),
                                                        reader.GetInt32(1),
                                                        reader.GetDouble(2),
                                                        reader.GetDouble(3),
                                                        reader.GetDouble(4)));
            }
            return TraceSamples;
        }

        public static void SaveTraceSample(TraceSampleModel TraceSample)
        {
            CultureInfo CH = new CultureInfo("en-EN");

            string SqliteCmd ="INSERT INTO TraceSample (FlightID, Latitude, Longitude, Altitude) VALUES ("
                + TraceSample.FlightID + ", "
                + TraceSample.Latitude.ToString(CH) + ", "
                + TraceSample.Longitude.ToString(CH) + ", "
                + TraceSample.Altitude.ToString(CH) + ")";
            using var connection = new SQLiteConnection(LoadConnectionString());
            connection.Open();
            using var cmd = new SQLiteCommand(SqliteCmd, connection);
            cmd.ExecuteNonQuery();
        }
        private static string LoadConnectionString()
        {
            // "Data Source=./<relativePathToSqliteDataBase;Version=3;"
            return "Data Source=" + DBDirectory + "/GliderFlightBookDatabase.db;Version=3;";
        }
        public static void SetDBPath(string PathToDBDirectory)
        {
            if (File.Exists(PathToDBDirectory + "/GliderFlightBookDatabase.db"))
            {
                DBDirectory = PathToDBDirectory;
            }
            else
            {
                throw new FileNotFoundException(PathToDBDirectory + "/GliderFlightBookDatabase.db");
            }
        }
    }
}