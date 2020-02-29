using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;


namespace GliderFlightBook
{
    public class SqliteDataAccess
    {
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

        private static string LoadConnectionString()
        {
            // "Data Source=./<relativePathToSqliteDataBase;Version=3;"
            return "Data Source=./WorkingDirectoryForTest/GliderFlightBookDatabase.db;Version=3;";
        }
    }
}