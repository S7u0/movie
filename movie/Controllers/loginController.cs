using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using movie.Models;

namespace movie.Controllers
{
    public class loginController : Controller
    {
        // GET: login
        public ActionResult Index()
        {
            if (Session["Email"].ToString() != null)
            {
                ViewBag.LoginSuccess = "Welcome " + Session["Email"];
            }
            else
            {
                ViewBag.LoginFailed = "Please Try Again Login Failed";
            }
            return View();
        }

        // GET: login/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: login/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: login/Create
        [HttpPost]
        public ActionResult Create(Login lmodel)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("Login_User", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                connection.Open();
                cmd.Parameters.AddWithValue("@Email_id", lmodel.Email_ID);
                cmd.Parameters.AddWithValue("@User_password", lmodel.User_Password);
                int result = (int)cmd.ExecuteScalar();
                if (result > 0)
                {
                    Session["Email"] = lmodel.Email_ID;
                    return RedirectToAction("Details", "booking");
                    
                }
                else
                {
                    ViewBag.Error = "Email or Password Invalid.";
                    return View(lmodel);
                }
            }
            catch
            {
                return View(lmodel);
            }
        }

        // GET: login/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: login/Edit/5
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

        // GET: login/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: login/Delete/5
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
