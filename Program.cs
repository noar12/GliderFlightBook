using System;

namespace GligerFlightBook
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running some test on Flight class");
            Console.WriteLine("Construct a flight");
            Flight.IFlight TestFlight = new Flight.Flight("2019-07-13-XTR-9DC0890216A9-01.KML");
            Console.WriteLine("Flight ID is " + TestFlight.FlightID + " and the associated filename is " + TestFlight.FlightFilename);
            Console.WriteLine("---------------------");
            Console.WriteLine("Start altitude is:" + TestFlight.StartHeight() + " [m]");
            Console.WriteLine("Landing altitude is:" + TestFlight.LandingHeight() + " [m]");
            Console.WriteLine("The maximum height reached is : " + TestFlight.MaximumHeight() + "[m]");
            Console.WriteLine("The total flown distance is : " + TestFlight.FlownDistance(0)/1000 + " [km]");
        }
    }
}
