using System;

namespace GliderFlightBook
{
    public class SiteModel
    {
        public SiteModel(int SiteID, string SiteName, double Latitude, double Longitude, double Altitude, double Radius)
        {
            this.SiteID = SiteID;
            this.SiteName = SiteName;
            this.Latitude = Latitude;
            this.Longitude = Longitude;
            this.Altitude = Altitude;
            this.Radius = Radius;
        }
        public int SiteID;
        public string SiteName;
        public double Latitude;
        public double Longitude;
        public double Altitude;
        public double Radius;

    }
}