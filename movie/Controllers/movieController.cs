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
    public class movieController : Controller
    {
        // GET: movie
        public ActionResult Index()
        {
            return View();
        }

        // GET: movie/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: movie/Create
        public ActionResult Create()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                List<SelectListItem> list = new List<SelectListItem>();
                SqlCommand cmd = new SqlCommand("Bind_Category", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new SelectListItem
                    {
                        Value = reader["Cat_ID"].ToString(),
                        Text = reader["Cat_Type"].ToString()
                    });
                }
                connection.Close();
                ViewBag.CategoryList = list;
            }
            return View(new movieModel());
        }

        // POST: movie/Create
        [HttpPost]
        public ActionResult Create(movieModel mmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
                    SqlConnection connection = new SqlConnection(connectionString);
                    SqlCommand cmd = new SqlCommand("add_Movie", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    cmd.Parameters.AddWithValue("@Movie_name", mmodel.Movie_Name);
                    cmd.Parameters.AddWithValue("@Release_date", mmodel.Release_Date);
                    cmd.Parameters.AddWithValue("@Cat_ID", mmodel.Cat_ID);
                    cmd.Parameters.AddWithValue("@Rate", mmodel.rate);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    ModelState.Clear();
                    ViewBag.Message = "Movie Insert Successflly!";
                    return View(new movieModel());
                }

                return View(mmodel);

            }
            catch(Exception e)
            {
                ViewBag.error = e.Message;
                return View();
            }
        }

        // GET: movie/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: movie/Edit/5
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

        // GET: movie/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: movie/Delete/5
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
