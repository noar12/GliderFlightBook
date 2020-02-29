using System;


namespace GliderFlightBook
{
    public class GliderModel
    {
        public GliderModel(int GliderID, string Brand, string Model, string EnCertification)
        {
            this.GliderID = GliderID;
            this.Brand = Brand;
            this.Model = Model;
            this.EnCertification = EnCertification;
        }
        public int GliderID;

        public string Brand;
        public string Model;
        public string EnCertification;
        public string LongGliderName
        {
            get
            {
                return Brand + " " + Model;
            }
        }
    }
}