using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using user.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace movie.Controllers
{
    public class userController : Controller
    {
        // GET: user
        public ActionResult Index()
        {
            return View();
        }

        // GET: user/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: user/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: user/Create
        [HttpPost]
        public ActionResult Create(userModel umodel)
        {

            try
            {
                if (ModelState.IsValid)
                {

                    string constring = ConfigurationManager.ConnectionStrings["DBconnection"].ToString();
                    SqlConnection con = new SqlConnection(constring);
                    SqlCommand cmd = new SqlCommand("add_User", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@User_name", umodel.User_name);
                    cmd.Parameters.AddWithValue("@Email_ID", umodel.Email_ID);
                    cmd.Parameters.AddWithValue("@User_Password", umodel.User_Password);
                    cmd.Parameters.AddWithValue("@PhoneNo", umodel.PhoneNo);
                    cmd.Parameters.AddWithValue("@City", umodel.City);
                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    con.Close();

                    if (i >= 0)
                    {
                        return View(umodel);
                    }
                }
                return View(umodel);
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }

        // GET: user/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: user/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: user/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: user/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
