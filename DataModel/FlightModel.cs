namespace GliderFlightBook
{
    public class FlightModel
    {
        public FlightModel(int FlightID, int TakeOffSiteID, int LandingSiteID, int GliderID, string Date,
        double FlownDistance, double FlightDuration, double CumulatedElevation,
        double MaxAltitude, string File, int FlightType, string Comment)
        {
            this.FlightID = FlightID;
            this.TakeOffSiteID = TakeOffSiteID;
            this.LandingSiteID = LandingSiteID;
            this.GliderID = GliderID;
            this.Date = Date;
            this.FlownDistance = FlownDistance;
            this.FlightDuration = FlightDuration;
            this.CumulatedElevation = CumulatedElevation;
            this.MaxAltitude = MaxAltitude;
            this.File = File;
            this.FlightType = FlightType;
            this.Comment = Comment;
        }
        public int FlightID;
        public int TakeOffSiteID;
        public int LandingSiteID;
        public int GliderID;
        public string Date;
        public double FlownDistance;
        public double FlightDuration;
        public double CumulatedElevation;
        public double MaxAltitude;
        public string File;
        public int FlightType;
        public string Comment;
    }
}