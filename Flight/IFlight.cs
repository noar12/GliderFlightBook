namespace GligerFlightBook.Flight
{
    public interface IFlight
    {
         string FlightFilename
         {
             get;
             set;
         }
         uint FlightID
         {
             get;
         }
         double FlownDistance(double AverageWindowDistance);
         double XCDistance();
         double FAITriangleDistance();
         double FlatTriangleDistance();
         double FreeDistance();
         double MaximumHeight();
         double MaximumRiseRate(double AverageWindowDistance);
         double MaximumSinkRate(double AverageWindowDistance);
         double StartHeight();
         double LandingHeight();
         double AltitudeDifference();

    }
}