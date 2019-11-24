using System;
using System.Linq;
using System.IO;
using System.Xml.Linq;

namespace GligerFlightBook.Flight
{
    public class Flight : IFlight
    {
        public Flight(string FlightFileName)
        {

            assignFlightID();
            this.FlightFilename = FlightFileName;
            ImportFromKMLFile();

            
        }
        public static string _WorkingFolderPath = @"WorkingDirectoryForTest/";
        private string _FlightFilename;
        private uint _FlightID;
        private double[] _FlightLatitudes;
        private double[] _FlightLongitudes;
        private double[] _FlightHeights;
        private DateTime[] _FlightTraceTimestamp;
        private static TimeSpan _KmlTimeSpan = new TimeSpan(0,0,0,0,200);
        private static double _EarthRadius = 6371000;
        public string FlightFilename
        {
            get
            {
                Console.WriteLine("Getting FlightFileName through accessor");
                return _FlightFilename;
            }
            set
            {
                if (File.Exists(_WorkingFolderPath+value))
                {
                    _FlightFilename = value;
                }
                else
                {
                    throw new FileNotFoundException(_WorkingFolderPath+value);
                }
            }
        }
        public uint FlightID
        {
            get
            {
                Console.WriteLine("Getting FlightID through accessor");
                return _FlightID;
            }
        }
        private void assignFlightID()
        {
            // to do: make a guid for the flightID
            _FlightID = 123;
        }
        public double FlownDistance(double AverageWindowDistance)
        {
            double[] dxyz = new double[_FlightHeights.Length-1];
            for(int i=0;i<_FlightHeights.Length-2;++i)
            {
                dxyz[i] = Math.Sqrt(Math.Pow(distanceBetweenGPSCoordinate(_FlightLatitudes[i],_FlightLongitudes[i],_FlightLatitudes[i+1],_FlightLongitudes[i+1],_FlightHeights[i]),2)
                +Math.Pow(_FlightHeights[i+1]-_FlightHeights[i],2));
                if (Double.IsNaN(dxyz[i]))
                    Console.Write("Nan when computing distance");
            }
            return dxyz.Sum();
        }
        public double XCDistance()
                {
            // to be done
            return 0;
        }
        public double FAITriangleDistance()
                {
            // to be done
            return 0;
        }
        public double FlatTriangleDistance()
                {
            // to be done
            return 0;
        }
        public double FreeDistance()
                {
            // to be done
            return 0;
        }
        public double MaximumHeight()
                {
            // to be done 
            return _FlightHeights.Max();
        }
        public double MaximumRiseRate(double AverageWindowDistance)
        {
            // to be done
            return 0;
        }
        public double MaximumSinkRate(double AverageWindowDistance)
        {
            // to be done
            return 0;
        }
        public double StartHeight()
        {
            return _FlightHeights[0];
        }
        public double LandingHeight()
        {
            return _FlightHeights[_FlightHeights.Length-1];
        }
        public double AltitudeDifference()
                {
            // to be done
            return 0;
        }
    
        private void ImportFromKMLFile()
        {
            XDocument KMLData = XDocument.Load(_WorkingFolderPath+FlightFilename);
            XNamespace ns = KMLData.Root.Name.Namespace;
            //kml/Placemark/MultiGeometry/LineString/coordinates
            var KMLLineStrings = KMLData.Element(ns + "kml")
                                        .Element(ns + "Placemark")
                                        .Element(ns + "MultiGeometry")
                                        .Elements(ns + "LineString");
            string RawCoordinate = "";
            foreach (XElement PartialGeo in KMLLineStrings)
            {
                RawCoordinate = RawCoordinate + PartialGeo.Element(ns + "coordinates").Value;
            }
            var RawStartTime = KMLData.Element(ns + "kml").Element(ns + "Placemark").Element(ns + "name").Value.Split(" ")[0];
            
            string[] RawCoordinateLines = RawCoordinate.Split("\n");
            int emptyLine = 0;
            foreach(string line in RawCoordinateLines)
            {
                if (line=="")
                    ++emptyLine;
            }
            string[] coordinates;
            _FlightLatitudes = new double[RawCoordinateLines.Length - emptyLine];
            _FlightLongitudes = new double[RawCoordinateLines.Length - emptyLine];
            _FlightHeights = new double[RawCoordinateLines.Length - emptyLine];
            _FlightTraceTimestamp = new DateTime[RawCoordinateLines.Length - emptyLine];
            _FlightTraceTimestamp[0] = DateTime.Parse(RawStartTime);
            int i = 0;
            foreach(string line in RawCoordinateLines)
            {
                if (line !="")
                {
                    coordinates = line.Split(",");
                    if (i!=0)
                        _FlightTraceTimestamp[i]=_FlightTraceTimestamp[i-1]+_KmlTimeSpan;
                
                    _FlightLatitudes[i] = double.Parse(coordinates[0],
                    System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.NumberFormatInfo.InvariantInfo);

                    _FlightLongitudes[i] = double.Parse(coordinates[1],
                    System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.NumberFormatInfo.InvariantInfo);

                    _FlightHeights[i] = double.Parse(coordinates[2],
                    System.Globalization.NumberStyles.AllowDecimalPoint,
                    System.Globalization.NumberFormatInfo.InvariantInfo);
                    ++i;
                }
                

            }
        }
        private double distanceBetweenGPSCoordinate(double latitude1, double longitude1, 
        double latitude2, double longitude2, double altitude)// from DraftKMLParser.py
        {
            double R = _EarthRadius +altitude;
            double deltaLat = (latitude2-latitude1)/180*Math.PI;
            double deltaLong = (longitude2-longitude1)/180*Math.PI;
            double long1 = longitude1/180*Math.PI;
            double long2 = longitude2/180*Math.PI;
            double a = Math.Pow(Math.Sin(deltaLong/2),2) + Math.Pow(Math.Sin(deltaLat/2),2) * Math.Cos(long1) * Math.Cos(long2);
            double c = 2 * Math.Atan2(Math.Sqrt(a),Math.Sqrt(1-a));
            return R * c;
        }
    }
}