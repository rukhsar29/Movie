using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace movie2.Models
{
 
    public class AddMovie
    {
        
        public int Movie_id { get; set; }
       
        public string Movie_name { get; set; }
        
        public int MovieType_id { get; set; }
        public string Director { get; set; }
        public string About { get; set; }
        public DateTime DateOFRelease { get; set; }
        public string ProductionCompany_name1 { get; set; }
     
        public List<ProductionCompany_list> productionCompany_list { get; set; }
        public class ProductionCompany_list
        {
            public string ProductionCompany_name { get; set; }
            public int id { get; set; }
        }
       public string IsAvailable { get; set; }
      



    }
}