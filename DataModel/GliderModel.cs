using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
        int GliderID;
        string Brand;
        string Model;
        string EnCertification;
        string LongGliderName
        {
            get
            {
                return Brand + " " + Model;
            }
        }
    }
}