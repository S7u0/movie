using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace movie.Models
{
    public class Booking
    {
        public int booking_ID { get; set; }
        public int User_ID { get; set; }
        public string Cat_Type { get; set; }
        public string Movie_name { get; set; }
        public int Cat_ID { get; set; }
        public int Movie_ID { get; set; }
        public int no_of_Tickets { get; set; }
        public int amount { get; set; }


    }
}