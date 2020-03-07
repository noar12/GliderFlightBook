using System;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Globalization;

namespace GliderFlightBook.Flight
{
    public class Flight : IFlight
    {
        public Flight(string FlightFileName, int TakeOffSiteID, int LandingSiteID, int GliderID, int FlightType)
        {
            this.FlightFilename = FlightFileName;
            ImportFromKMLFile();
            this.TakeOffSiteID = TakeOffSiteID;
            this.LandingSiteID = LandingSiteID;
            this.GliderID = GliderID;
            this.FlightType = FlightType;

        }
        public static string _WorkingFolderPath = @"WorkingDirectoryForTest/";
        private string _FlightFilename;
        private double[] _FlightLatitudes;
        private double[] _FlightLongitudes;
        private double[] _FlightHeights;
        private int _TakeOffSiteID;
        private int _LandingSiteID;
        private int _GliderID;
        private int _FlightType;
        private DateTime[] _FlightTraceTimestamp;
        private static TimeSpan _KmlTimeSpan = new TimeSpan(0, 0, 0, 0, 200);
        private static double _EarthRadius = 6371000;
        public string GetTakeOffDateString()
        {
            return _FlightTraceTimestamp[0].ToString("s");
        }
        public string FlightFilename
        {
            get
            {
                return _FlightFilename;
            }
            set
            {
                if (File.Exists(_WorkingFolderPath + "/" + value))
                {
                    _FlightFilename = value;
                }
                else
                {
                    throw new FileNotFoundException(_WorkingFolderPath + value);
                }
            }
        }
        public double GetFlownDistance(double AverageWindowDistance)
        {
            double[] dxyz = new double[_FlightHeights.Length - 1];
            for (int i = 0; i < _FlightHeights.Length - 2; ++i)
            {
                dxyz[i] = Math.Sqrt(Math.Pow(distanceBetweenGPSCoordinate(_FlightLatitudes[i], _FlightLongitudes[i], _FlightLatitudes[i + 1], _FlightLongitudes[i + 1], _FlightHeights[i]), 2)
                + Math.Pow(_FlightHeights[i + 1] - _FlightHeights[i], 2));
                if (Double.IsNaN(dxyz[i]))
                    Console.Write("Nan when computing distance");
            }
            return dxyz.Sum();
        }
        public double GetCumulativeElevation()
        {
            double CumulativeElevation = 0;
            double mindHForElevation = 0.01;
            for (int i = 1; i < _FlightHeights.Length; ++i)
            {
                if (_FlightHeights[i] > _FlightHeights[i - 1] + mindHForElevation)
                {
                    CumulativeElevation = CumulativeElevation + (_FlightHeights[i] - _FlightHeights[i - 1]);
                }
            }
            return CumulativeElevation;
        }
        public double GetXCDistance()
        {
            // to be done
            return 0;
        }
        public double GetFAITriangleDistance()
        {
            // to be done
            return 0;
        }
        public double GetFlatTriangleDistance()
        {
            // to be done
            return 0;
        }
        public double GetFreeDistance()
        {
            // to be done
            return 0;
        }
        public double GetMaximumHeight()
        {
            return _FlightHeights.Max();
        }
        public double GetMaximumRiseRate(double AverageWindowDistance)
        {
            // to be done
            return 0;
        }
        public double GetMaximumSinkRate(double AverageWindowDistance)
        {
            // to be done
            return 0;
        }
        public double GetStartHeight()
        {
            return _FlightHeights[0];
        }
        public double GetLandingHeight()
        {
            return _FlightHeights[_FlightHeights.Length - 1];
        }
        public double GetAltitudeDifference()
        {

            return GetStartHeight() - GetLandingHeight();
        }
        public TimeSpan GetDuration()
        {
            return _FlightTraceTimestamp[_FlightTraceTimestamp.Length - 1] - _FlightTraceTimestamp[0];
        }
        private void ImportFromKMLFile()
        {
            XDocument KMLData = XDocument.Load(_WorkingFolderPath + "/" + FlightFilename);
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
            foreach (string line in RawCoordinateLines)
            {
                if (line == "")
                    ++emptyLine;
            }
            string[] coordinates;
            _FlightLatitudes = new double[RawCoordinateLines.Length - emptyLine];
            _FlightLongitudes = new double[RawCoordinateLines.Length - emptyLine];
            _FlightHeights = new double[RawCoordinateLines.Length - emptyLine];
            _FlightTraceTimestamp = new DateTime[RawCoordinateLines.Length - emptyLine];
            _FlightTraceTimestamp[0] = DateTime.Parse(RawStartTime);
            int i = 0;
            foreach (string line in RawCoordinateLines)
            {
                if (line != "")
                {
                    coordinates = line.Split(",");
                    if (i != 0)
                        _FlightTraceTimestamp[i] = _FlightTraceTimestamp[i - 1] + _KmlTimeSpan;

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
            double R = _EarthRadius + altitude;
            double deltaLat = (latitude2 - latitude1) / 180 * Math.PI;
            double deltaLong = (longitude2 - longitude1) / 180 * Math.PI;
            double long1 = longitude1 / 180 * Math.PI;
            double long2 = longitude2 / 180 * Math.PI;
            double a = Math.Pow(Math.Sin(deltaLong / 2), 2) + Math.Pow(Math.Sin(deltaLat / 2), 2) * Math.Cos(long1) * Math.Cos(long2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
        public string GetCSVExportLine(string GliderName, string TakeOffSite, string LandingSite,
         string FlightType, string Comment)
        {
            CultureInfo CH = new CultureInfo("en-EN");
            string delimiter = ";";
            string TakeOffDate = GetTakeOffDateString();
            string TakeOffHeight = _FlightHeights[0].ToString("F0");
            string MaximumHeight = GetMaximumHeight().ToString("F0");
            string CumulativeElevation = GetCumulativeElevation().ToString("F0");
            string FlownDistance = (GetFlownDistance(0) / 1000).ToString("F3", CH);
            string HeightDifference = GetAltitudeDifference().ToString("F0");
            string Duration = GetDuration().ToString("c");

            return TakeOffDate + delimiter
            + GliderName + delimiter
            + TakeOffSite + delimiter
            + TakeOffHeight + delimiter
            + CumulativeElevation + delimiter
            + MaximumHeight + delimiter
            + LandingSite + delimiter
            + FlownDistance + delimiter
            + HeightDifference + delimiter
            + FlightType + delimiter
            + Duration + delimiter
            + Comment + delimiter
            + _FlightFilename + delimiter;

        }
        public void Display()
        {
            Console.WriteLine("Flight data are the following:");
            Console.WriteLine("Filename: " + _FlightFilename);
            Console.WriteLine("Take off date/Time (local): " + GetTakeOffDateString());
            Console.WriteLine("Take off site: " + SqliteDataAccess.LoadSite(TakeOffSiteID).SiteName);
            Console.WriteLine("Landing site: " + SqliteDataAccess.LoadSite(LandingSiteID).SiteName); // to be done better. use a method to get site and glider by id directly from the data base
            Console.WriteLine("Glider used: " + SqliteDataAccess.LoadGlider(GliderID).LongGliderName);
            Console.WriteLine("Flown distance [km]: " + (GetFlownDistance(0) / 1000).ToString("F3"));
            Console.WriteLine("Flight duration: " + GetDuration().ToString("c"));
            Console.WriteLine("Cumulated elevation during flight [m]: " + GetCumulativeElevation());
            Console.WriteLine("Maximal altitude reached during flight [m]: " + GetMaximumHeight().ToString("F3"));
            Console.WriteLine("The declared flight type is [0: Local, 1: HF, 2: XC]: " + _FlightType);
        }
        public int TakeOffSiteID
        {
            get
            {
                return _TakeOffSiteID;
            }
            set
            {
                // to be done: some check before setting
                _TakeOffSiteID = value;
            }
        }
        public int LandingSiteID
        {
            get
            {
                return _LandingSiteID;
            }
            set
            {
                // to be done: some check before setting
                _LandingSiteID = value;
            }
        }
        public int GliderID
        {
            get
            {
                return _GliderID;
            }
            set
            {
                // to be done: some check before setting
                _GliderID = value;
            }
        }
        public int FlightType
        {
            get
            {
                return _FlightType;
            }
            set
            {
                // to be done: some check before setting
                _FlightType = value;
            }
        }
        public double[] FlightLatitudes
        {
            get
            {
                return _FlightLatitudes;
            }
        }
        public double[] FlightLongitudes
        {
            get
            {
                return _FlightLongitudes;
            }
        }
        public double[] FlightHeights
        {
            get
            {
                return _FlightHeights;
            }
        }
    }   

}