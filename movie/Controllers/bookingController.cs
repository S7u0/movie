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
    public class bookingController : Controller
    {
        public List<SelectListItem> Bind_Movie(int cat_id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
            List<SelectListItem> list = new List<SelectListItem>();
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("Bind_Movie", connection);
            cmd.Parameters.AddWithValue("@Cat_ID", cat_id);
            cmd.CommandType = CommandType.StoredProcedure;
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new SelectListItem { Value = reader["Movie_ID"].ToString(), Text = reader[" Movie_name"].ToString() + "\t\t|\t\tPrice : " + reader["rate"].ToString() });
            }
            ViewBag.MovieList = list;
            return list;
        }
        public int Calculate_Price(int Movie_ID, int no_of_Tickets)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("Get_Rate", connection);
            cmd.Parameters.AddWithValue("@Movie_ID", Movie_ID);
            cmd.CommandType = CommandType.StoredProcedure;
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int price = 0;
            if (reader.Read())
            {
                price = Convert.ToInt32(reader["rate"]);
            }
            int total_price = price * no_of_Tickets;
            ViewBag.TotalPrice = total_price;
            return total_price;
        }
        public int Get_User_id()
        {
            int id = 0;
            if (Session["Email"] == null)
            {
              RedirectToAction("Create", "login");
                return 0;
            }
                string connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("Get_User", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                connection.Open();
                cmd.Parameters.AddWithValue("@Email_ID", Session["Email"]);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    id = Convert.ToInt32(reader["User_ID"]);
                }
            return id;
        }
            // GET: booking
            public ActionResult Index()
        {
            return View();
        }

        // GET: booking/Details/5
        public ActionResult Details()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("Get_Booking", connection);
            List<Booking> bookList = new List<Booking>();
            cmd.CommandType = CommandType.StoredProcedure;
            connection.Open();
            cmd.Parameters.AddWithValue("@User_id", Get_User_id());
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Booking book = new Booking();
                book.booking_ID = Convert.ToInt32(reader["Booking_ID"]);
                book.Cat_Type = reader["Cat_type"].ToString();
                book.Movie_name = reader[" Movie_name"].ToString();
                book.no_of_Tickets = Convert.ToInt32(reader["No_of_Tickets"]);
                book.amount = Convert.ToInt32(reader["amount"]);
                bookList.Add(book);
            }
            reader.Close();
            connection.Close();
            return View(bookList);
        }

    // GET: booking/Create
    public ActionResult Create(int? Cat_ID, int? Movie_ID, int? no_of_Tickets)
        {
        string connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
        List<SelectListItem> list = new List<SelectListItem>();
        SqlConnection connection = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand("Bind_Category", connection);
        cmd.CommandType = CommandType.StoredProcedure;
        connection.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new SelectListItem { Value = reader["Cat_ID"].ToString(), Text = reader["Cat_Type"].ToString() });
        }
        ViewBag.CategoryList = list;
        ViewBag.MovieList = new List<SelectListItem>();
        // Bind Movie according to selected Category
        if (Cat_ID != null)
        {
            ViewBag.MovieList = Bind_Movie(Cat_ID.Value);
        }
        else
        {
            ViewBag.MovieList = new List<SelectListItem>();
        }

        Booking book = new Booking();

        if (Cat_ID != null)
        {
            book.Cat_ID = Cat_ID.Value;
        }

        if (Movie_ID != null)
        {
            book.Movie_ID = Movie_ID.Value;
        }

        if (no_of_Tickets != null)
        {
            book.no_of_Tickets = no_of_Tickets.Value;
        }

        if (Movie_ID != null && no_of_Tickets != null)
        {
            book.amount = Calculate_Price(Movie_ID.Value, no_of_Tickets.Value);
            ViewBag.TotalPrice = book.amount;
        }
        return View(book);
    }

    // POST: booking/Create
    [HttpPost]
        public ActionResult Create(Booking bmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
                    SqlConnection connection = new SqlConnection(connectionString);
                    SqlCommand cmd = new SqlCommand("Insert_Booking", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    cmd.Parameters.AddWithValue("@User_ID", Get_User_id());
                    cmd.Parameters.AddWithValue("@Cat_ID", bmodel.Cat_ID);
                    cmd.Parameters.AddWithValue("@Movie_ID", bmodel.Movie_ID);
                    cmd.Parameters.AddWithValue("@no_of_Tickets", bmodel.no_of_Tickets);
                    cmd.Parameters.AddWithValue("@amount", bmodel.amount);
                    int i = cmd.ExecuteNonQuery();
                    connection.Close();
                    if (i > 0)
                    {
                        ViewBag.Message = "Booking Insert Successfully";
                        return View(bmodel);
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex + " Insertion Failed";
                return View(bmodel);
            }
        }

        // GET: booking/Edit/5
        //public ActionResult Edit(int Booking_ID, int? Cat_ID, int? Movie_ID, int? no_of_Tickets)
        //{
        //    Booking book = new Booking();
        //    string connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();

        //    SqlConnection connection = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand("Get_Booking_ById", connection);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@Booking_ID", Booking_ID);

        //    connection.Open();
        //    SqlDataReader reader = cmd.ExecuteReader();

        //    if (reader.Read())
        //    {
        //        book.booking_ID = Convert.ToInt32(reader["Booking_ID"]);
        //        book.User_ID = Convert.ToInt32(reader["User_ID"]);
        //        book.Cat_ID = Convert.ToInt32(reader["Cat_ID"]);
        //        book.Movie_ID = Convert.ToInt32(reader["Movie_ID"]);
        //        book.no_of_Tickets = Convert.ToInt32(reader["no_of_Tickets"]);
        //        book.amount = Convert.ToInt32(reader["amount"]);
        //    }

        //    reader.Close();
        //    connection.Close();

        //    List<SelectListItem> list = new List<SelectListItem>();

        //    connection = new SqlConnection(connectionString);
        //    cmd = new SqlCommand("Bind_Category", connection);
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    connection.Open();
        //    reader = cmd.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        list.Add(new SelectListItem
        //        {
        //            Value = reader["Cat_ID"].ToString(),
        //            Text = reader["Cat_Type"].ToString()
        //        });
        //    }

        //    reader.Close();
        //    connection.Close();

        //    ViewBag.CategoryList = list;

        //    if (Cat_ID != null)
        //    {
        //        book.Cat_ID = Cat_ID.Value;
        //    }

        //    if (Movie_ID != null)
        //    {
        //        book.Cat_ID = Cat_ID.Value;
        //    }

        //    if (no_of_Tickets != null)
        //    {
        //        book.no_of_Tickets = no_of_Tickets.Value;
        //    }

        //    ViewBag.MovieList = new List<SelectListItem>();

        //    if (book.Cat_ID != 0)
        //    {
        //        ViewBag.MovieList = Bind_Movie(book.Cat_ID);
        //    }

        //    if (Movie_ID != null && no_of_Tickets != null)
        //    {
        //        book.amount = Calculate_Price(Movie_ID.Value, no_of_Tickets.Value);
        //        ViewBag.TotalPrice = book.amount;
        //    }
        //    else
        //    {
        //        ViewBag.TotalPrice = book.amount;
        //    }

        //    return View(book);
        //}

        //// POST: booking/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: booking/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: booking/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
