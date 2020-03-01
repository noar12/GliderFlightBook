using System;

namespace GliderFlightBook.Flight
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
         string GetTakeOffDateString();
         double GetFlownDistance(double AverageWindowDistance);
         double GetXCDistance();
         double GetFAITriangleDistance();
         double GetFlatTriangleDistance();
         double GetFreeDistance();
         double GetMaximumHeight();
         double GetMaximumRiseRate(double AverageWindowDistance);
         double GetMaximumSinkRate(double AverageWindowDistance);
         double GetStartHeight();
         double GetLandingHeight();
         TimeSpan GetDuration();
         double GetCumulativeElevation();
         double GetAltitudeDifference();
         string GetCSVExportLine(string GliderName, string TakeOffSite, string LandingSite,
         string FlightType, string Comment);

    }
}