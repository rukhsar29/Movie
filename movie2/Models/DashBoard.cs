using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace movie2.Models
{
    public class DashBoard
    {
        public AddMovie AddMovie { get; set; }
        public int Movie_id { get; set; }
        public string Movie_name { get; set; }
        public DateTime DateOFRelease { get; set; }
        public string Director { get; set; }
        public string About { get; set; }
        public string IsAvailable { get; set; }
        public string ProductionCompany_name { get; set; }
    }
}