using movie2.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using movie2.Models;
using System.Security.Cryptography;
using static movie2.Models.AddMovie;
using WebGrease.Css.Ast.Selectors;

namespace movie2.Controllers
{
    public class Home1Controller : Controller
    {

        string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        static List<AddMovie> list4 = new List<AddMovie>();
        //Don't make list globally.Woh value usme store rhta h jitne request jayega.Otherwise do list.clear when you want to use it as fresh.
        static List<DashBoard> list1 = new List<DashBoard>();
        static List<DashBoard> _list = new List<DashBoard>();
        public ActionResult AdminLogin()
        {

            return View();
        }
        [HttpPost]
        public ActionResult AdminLogin(AdminLogin al)
        {
            SqlConnection con = new SqlConnection(CS);
            string Query = "";
            Query += "select * from AdminLogin where E_MailId='" + al.E_MailId + "' and Password='" + al.Password + "'";
            SqlDataAdapter cmd = new SqlDataAdapter(Query, con);
            DataTable dt = new DataTable();
            cmd.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                Session["E_MailId"] = dt.Rows[0]["E_MailId"];
                return RedirectToAction("DashBoard", "Home1");

            }
            else
            {
                ViewBag.msg = "Invalid UserName and Password";
            }
            return View();
        }
        public ActionResult DashBoard()
        {
            return View();
        }

        public ActionResult AddMovie()
        {
            AddMovie ad = new AddMovie
            {
                productionCompany_list = GetProductionCompanylist()
            };
            return View(ad);
        }
        public List<ProductionCompany_list> GetProductionCompanylist()
        {
            SqlConnection con = new SqlConnection(CS);
            SqlCommand cmd = new SqlCommand("select ProductionCompany_name,ProductionCompany_id from ProductionCompany", con);
            con.Open();
            SqlDataReader idr = cmd.ExecuteReader();
            List<ProductionCompany_list> ProductionCompanies = new List<ProductionCompany_list>();
            if (idr.HasRows)
            {
                while (idr.Read())
                {

                    ProductionCompanies.Add(new ProductionCompany_list
                    {

                        ProductionCompany_name = idr["ProductionCompany_name"].ToString(),
                        id = Convert.ToInt16(idr["ProductionCompany_id"].ToString()),

                    });
                }
            }

            con.Close();

            return ProductionCompanies;
        }
        [HttpPost]
        public ActionResult AddMovie(AddMovie ad)
        {
            ad.productionCompany_list = GetProductionCompanylist();
            SqlConnection con = new SqlConnection(CS);
            con.Open();
            SqlCommand cmd = new SqlCommand("spAdd2", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Movie_name", ad.Movie_name);
            cmd.Parameters.AddWithValue("@MovieType_id", ad.MovieType_id);
            cmd.Parameters.AddWithValue("@Date_Of_Release", ad.DateOFRelease);
            cmd.Parameters.AddWithValue("@Director", ad.Director);
            cmd.Parameters.AddWithValue("@About", ad.About);
            cmd.Parameters.AddWithValue("@ProductionCompany_name1", ad.ProductionCompany_name1);
            cmd.Parameters.AddWithValue("@IsAvailable", ad.IsAvailable);
            cmd.ExecuteNonQuery();
            ViewBag.Msg1 = "Added successfully!";
            con.Close();
            ModelState.Clear();
            return View(ad);
        }

        public ActionResult Movie()
        {
            try
            {
                list4.Clear();
                SqlConnection con = new SqlConnection(CS);
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("spMovieList", con);
                DataTable dt = new DataTable();

                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        list4.Add(new AddMovie
                        {
                            Movie_id = Convert.ToInt32(dr["Movie_id"]),
                            Movie_name = dr["Movie_name"].ToString(),
                            MovieType_id = Convert.ToInt32(dr["MovieType_id"]),
                            Director = dr["Director"].ToString(),
                            About = dr["About"].ToString(),
                            DateOFRelease = Convert.ToDateTime(dr["Date_Of_Release"]),
                            ProductionCompany_name1 = dr["ProductionCompany_name1"].ToString(),
                            IsAvailable = dr["IsAvailable"].ToString()
                        }); ;
                    }

                }
                con.Close();
                return View(list4);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return null;
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                AddMovie ad = new AddMovie();
                
                SqlConnection con = new SqlConnection(CS);
                SqlCommand cmd = new SqlCommand("select * from AddMovie where Movie_id=" + id, con);
                con.Open();
                SqlDataReader idr = cmd.ExecuteReader();
                List<ProductionCompany_list> ProductionCompanies = new List<ProductionCompany_list>();
                if (idr.HasRows)
                {
                    while (idr.Read())
                    {
                        ad.Movie_id = Convert.ToInt32(idr["Movie_id"]);
                        ad.Movie_name = idr["Movie_name"].ToString();
                        ad.MovieType_id = Convert.ToInt32(idr["MovieType_id"]);
                        ad.Director = idr["Director"].ToString();
                        ad.About = idr["About"].ToString();
                        ad.DateOFRelease = Convert.ToDateTime(idr["Date_Of_Release"]);
                        ad.ProductionCompany_name1 = idr["ProductionCompany_name1"].ToString();
                        ad.IsAvailable = idr["IsAvailable"].ToString();
                    }
                }
                con.Close();
                //var Edit = list4.Where(x => x.Movie_id == Movie_id).FirstOrDefault();
                return View(ad);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Edit(AddMovie ad,int id)
        {

            SqlConnection con = new SqlConnection(CS);
            con.Open();
            SqlCommand cmd = new SqlCommand("spUpdate", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Movie_id", id);
            cmd.Parameters.AddWithValue("@Movie_name", ad.Movie_name);
            cmd.Parameters.AddWithValue("@MovieType_id", ad.MovieType_id);
            cmd.Parameters.AddWithValue("@Director", ad.Director);
            cmd.Parameters.AddWithValue("@Date_Of_Release", ad.DateOFRelease);
            cmd.Parameters.AddWithValue("@About", ad.About);
            cmd.Parameters.AddWithValue("@ProductionCompany_name1", ad.ProductionCompany_name1);
            cmd.Parameters.AddWithValue("@IsAvailable", ad.IsAvailable);
            cmd.ExecuteNonQuery();
            ViewBag.Msg2 = "Updated successfully!";
            con.Close();

            return View(ad);
        }
        public ActionResult Delete(int id)
        {
            int movieId = Convert.ToInt32(id);
            SqlConnection con = new SqlConnection(CS);
            con.Open();
            SqlCommand cmd = new SqlCommand("spDelete", con);
            cmd.Parameters.AddWithValue("@Movie_id", movieId);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            ViewBag.Msg2 = "Deleted successfully!";
            con.Close();

            return View();
        }

        public ActionResult Horror()
        {
            List<AddMovie> list2 = new List<AddMovie>();
            SqlConnection con = new SqlConnection(CS);
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("spHorror", con);
            DataTable dt = new DataTable();

            sda.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list2.Add(new AddMovie
                    {

                        Movie_name = dr["Movie_name"].ToString(),
                        DateOFRelease = Convert.ToDateTime(dr["Date_Of_Release"]),
                        Director = dr["Director"].ToString(),
                        About = dr["About"].ToString(),
                        IsAvailable = dr["IsAvailable"].ToString()

                    }); ;
                }
            }
            return View(list2);
        }
        public ActionResult Comedy()
        {
            List<AddMovie> list3 = new List<AddMovie>();
            SqlConnection con = new SqlConnection(CS);
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("spComedy", con);
            DataTable dt = new DataTable();

            sda.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list3.Add(new AddMovie
                    {

                        Movie_name = dr["Movie_name"].ToString(),
                        DateOFRelease = Convert.ToDateTime(dr["Date_Of_Release"]),
                        Director = dr["Director"].ToString(),
                        About = dr["About"].ToString(),
                        IsAvailable = dr["IsAvailable"].ToString()

                    }); ;
                }
            }
            return View(list3);
        }
        public ActionResult Romance()
        {
            List<AddMovie> list3 = new List<AddMovie>();
            SqlConnection con = new SqlConnection(CS);
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("spRomance", con);
            DataTable dt = new DataTable();

            sda.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list3.Add(new AddMovie
                    {

                        Movie_name = dr["Movie_name"].ToString(),
                        DateOFRelease = Convert.ToDateTime(dr["Date_Of_Release"]),
                        Director = dr["Director"].ToString(),
                        About = dr["About"].ToString(),
                        IsAvailable = dr["IsAvailable"].ToString()

                    }); ;
                }
            }
            return View(list3);
        }
        public ActionResult Motivational()
        {
            List<AddMovie> list3 = new List<AddMovie>();
            SqlConnection con = new SqlConnection(CS);
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("spMotivational", con);
            DataTable dt = new DataTable();

            sda.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list3.Add(new AddMovie
                    {

                        Movie_name = dr["Movie_name"].ToString(),
                        DateOFRelease = Convert.ToDateTime(dr["Date_Of_Release"]),
                        Director = dr["Director"].ToString(),
                        About = dr["About"].ToString(),
                        IsAvailable = dr["IsAvailable"].ToString()

                    }); ;
                }
            }
            return View(list3);
        }
        public ActionResult MainDashboard(DashBoard d)
        {

            SqlConnection con = new SqlConnection(CS);
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("spMainDashBoard", con);
            DataTable dt = new DataTable();

            sda.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list1.Add(new DashBoard
                    {
                        Movie_id = Convert.ToInt16(dr["Movie_id"]),
                        Movie_name = dr["Movie_name"].ToString(),
                        DateOFRelease = Convert.ToDateTime(dr["Date_Of_Release"]),
                        Director = dr["Director"].ToString(),
                        About = dr["About"].ToString(),
                        IsAvailable = dr["IsAvailable"].ToString(),
                        ProductionCompany_name = dr["ProductionCompany_name"].ToString(),


                    }); ;
                }

            }
            con.Close();
            return View(list1);

        }


    }
}