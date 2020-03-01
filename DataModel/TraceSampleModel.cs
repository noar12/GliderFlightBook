namespace GliderFlightBook
{
    public class TraceSampleModel
    {
        public TraceSampleModel(int SampleID, int FlightID, double Latitude, double Longitude, double Altitude)
        {
            this.SampleID = SampleID;
            this.FlightID = FlightID;
            this.Latitude = Latitude;
            this.Longitude = Longitude;
            this.Altitude = Altitude;
        }
        public int SampleID;
        public int FlightID;
        public double Latitude;
        public double Longitude;
        public double Altitude;
        
    }
}