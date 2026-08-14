using movie.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace movie.Models
{

    public class MovieDBHandle
    {
        private SqlConnection con;
        private void Connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["DBconnection"].ToString();
            con = new SqlConnection(constring);
        }

        public bool AddStudent(movieModel mmodel)
        {
            Connection();
            SqlCommand cmd = new SqlCommand("add_Movie", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Movie_Name", mmodel.Movie_Name);
            cmd.Parameters.AddWithValue("@Release_Date", mmodel.Release_Date);
            cmd.Parameters.AddWithValue("@Cat_ID", mmodel.Cat_ID);
            cmd.Parameters.AddWithValue("@Release_Date", mmodel.rate);
         
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public List<movieModel> add_Movie()
        {
            Connection();

            List<movieModel> movielist = new List<movieModel>();

            SqlCommand cmd = new SqlCommand("add_Movie", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            con.Open();

            SqlDataReader sdr = cmd.ExecuteReader();

            while (sdr.Read())
            {
                movieModel mmodel = new movieModel();

                mmodel.Movie_ID = Convert.ToInt32(sdr["Movie_ID"]);
                mmodel.Movie_Name = sdr["Movie_Name"].ToString();
                mmodel.Release_Date = sdr["Release_Date"].ToString();
                mmodel.Cat_ID = Convert.ToInt32(sdr["Cat_ID"]);
                mmodel.rate = Convert.ToInt32(sdr["rate"]);
                movielist.Add(mmodel);
            }

            con.Close();

            return movielist;
        }

    }
}