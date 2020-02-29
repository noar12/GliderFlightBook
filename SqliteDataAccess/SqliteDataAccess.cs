using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GliderFlightBook
{
    public class SqliteDataAccess
    {
        public static List<GliderModel> LoadGliders()
        {
                string SqliteCmd = "select * from Glider";
                
                using var connection =new SQLiteConnection(LoadConnectionString());
                connection.Open();
                using var cmd = new SQLiteCommand(SqliteCmd, connection);
                using SQLiteDataReader reader = cmd.ExecuteReader();
                List<GliderModel> Gliders = new List<GliderModel>();
                while(reader.Read())
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
/*             using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Execute("insert into Glider (GliderID, Brand, Model, EnCertification) values (@GliderID, @Brand, @Model, @EnCertification)",glider);
            } */
        }

        private static string LoadConnectionString()
        {
            // "Data Source=./<relativePathToSqliteDataBase;Version=3;"
            return "Data Source=./WorkingDirectoryForTest/GliderFlightBookDatabase.db;Version=3;";
        }
    }
}