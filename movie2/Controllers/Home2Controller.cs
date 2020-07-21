using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using movie2.Models;
using System.Configuration;


namespace movie2.Controllers
{
    public class Home2Controller : Controller
    {
        string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        static List<ProductionCompany> list1 = new List<ProductionCompany>();
        public ActionResult ProductionCompany()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ProductionCompany(ProductionCompany pc)
        {
            SqlConnection con = new SqlConnection(CS);
            con.Open();
            SqlCommand cmd = new SqlCommand("spAdd1", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductionCompany_name", pc.ProductionCompany_name);
            cmd.Parameters.AddWithValue("@Year_OF_Establish", pc.Year_Of_Establish);
            cmd.Parameters.AddWithValue("@Chairman", pc.ChairMan);
            cmd.Parameters.AddWithValue("@Founder", pc.Founder);
            cmd.ExecuteNonQuery();
            ViewBag.Msg2 = "Added successfully!";
            con.Close();
            return View();
        }
        public ActionResult ProductionCompany1()
        {
            list1.Clear();
            SqlConnection con = new SqlConnection(CS);
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("spProductionCompanyList", con);
            DataTable dt = new DataTable();

            sda.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list1.Add(new ProductionCompany
                    {
                        ProductionCompany_id = Convert.ToInt32(dr["ProductionCompany_id"]),
                        ProductionCompany_name = dr["ProductionCompany_name"].ToString(),
                        Year_Of_Establish = Convert.ToInt32(dr["Year_Of_Establish"]),
                        ChairMan = dr["ChairMan"].ToString(),
                        Founder = dr["Founder"].ToString(),
                        
                    }); ;
                }

            }
            con.Close();
            return View(list1);


        }
        /* public ActionResult Edit(int? ProductionCompany_id)
         {
             var Edit = list1.Where(x => x.ProductionCompany_id == ProductionCompany_id).FirstOrDefault();
             return View(Edit);
         }
         [HttpPost]
         public ActionResult Edit(ProductionCompany pc)
         {
             SqlConnection con = new SqlConnection(CS);
             con.Open();
             SqlCommand cmd = new SqlCommand("spUpdate1", con);
             cmd.CommandType = System.Data.CommandType.StoredProcedure;
             cmd.Parameters.AddWithValue("@ProductionCompany_name", pc.ProductionCompany_name);
             cmd.Parameters.AddWithValue("@Year_Of_Establish", pc.Year_Of_Establish);
             cmd.Parameters.AddWithValue("@ChairMan", pc.ChairMan);
             cmd.Parameters.AddWithValue("@Founder", pc.Founder);
             cmd.ExecuteNonQuery();
             ViewBag.Msg2 = "Updated successfully!";
             con.Close();

             return View(pc);
         }*/
        public ActionResult Edit(int id)
        {
            try
            {
                ProductionCompany pc = new ProductionCompany();

                SqlConnection con = new SqlConnection(CS);
                SqlCommand cmd = new SqlCommand("select * from ProductionCompany where ProductionCompany_id=" + id, con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        pc.ProductionCompany_id = Convert.ToInt32(dr["ProductionCompany_id"]);
                        pc.ProductionCompany_name = dr["ProductionCompany_name"].ToString();
                        pc.Year_Of_Establish = Convert.ToInt32(dr["Year_Of_Establish"]);
                        pc.ChairMan = dr["ChairMan"].ToString();
                        pc.Founder = dr["Founder"].ToString();
                    }
                }
                con.Close();
                //var Edit = list4.Where(x => x.Movie_id == Movie_id).FirstOrDefault();
                return View(pc);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Edit(ProductionCompany pc, int id)
        {
            SqlConnection con = new SqlConnection(CS);
            con.Open();
            SqlCommand cmd = new SqlCommand("spUpdate1", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductionCompany_id", id);
            cmd.Parameters.AddWithValue("@ProductionCompany_name", pc.ProductionCompany_name);
            cmd.Parameters.AddWithValue("@Year_Of_Establish", pc.Year_Of_Establish);
            cmd.Parameters.AddWithValue("@ChairMan", pc.ChairMan);
            cmd.Parameters.AddWithValue("@Founder", pc.Founder);
            cmd.ExecuteNonQuery();
            ViewBag.Msg2 = "Updated successfully!";
            con.Close();

            return View(pc);

            
        }

        public ActionResult Delete(int id)
        {
            int ProductionCompanyId = Convert.ToInt32(id);
            SqlConnection con = new SqlConnection(CS);
            con.Open();
            SqlCommand cmd = new SqlCommand("spDelete1", con);
            cmd.Parameters.AddWithValue("@ProductionCompany_id", ProductionCompanyId);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            ViewBag.Msg2 = "Deleted successfully!";
            con.Close();

            return View();

        }

    }
}