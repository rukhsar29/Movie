using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace movie2.Models
{
    public class ProductionCompany
    {
        public int ProductionCompany_id { get; set; }
        public string ProductionCompany_name { get; set; }
        public int Year_Of_Establish { get; set; }
        public string ChairMan { get; set; }
        public string Founder { get; set; }
    }
}