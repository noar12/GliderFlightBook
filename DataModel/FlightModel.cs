namespace GliderFlightBook
{
    public class FlightModel
    {
        public FlightModel(int FlightID, int SiteID, int GliderID, string Date,
        int XContestType, double XContestPoint,double XContestDistance, 
        double FlownDistance, double FlightDuration, double CumulatedElevation,
        double MaxAltitude, string File, int FlightType, string Comment)
        {
            this.FlightID = FlightID;
            this.SiteID = SiteID;
            this.GliderID = GliderID;
            this.Date = Date;
            this.XContestType = XContestType;
            this.XContestPoint = XContestPoint;
            this.XContestDistance = XContestDistance;
            this.FlownDistance = FlownDistance;
            this.FlightDuration = FlightDuration;
            this.CumulatedElevation = CumulatedElevation;
            this.MaxAltitude = MaxAltitude;
            this.File = File;
            this.FlightType = FlightType;
            this.Comment = Comment;
        }
        public int FlightID;
        public int SiteID;
        public int GliderID;
        public string Date;
        public int XContestType;
        public double XContestPoint;
        public double XContestDistance;
        public double FlownDistance;
        public double FlightDuration;
        public double CumulatedElevation;
        public double MaxAltitude;
        public string File;
        public int FlightType;
        public string Comment;
    }
}