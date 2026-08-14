using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace movie.Models
{
    public class movieModel
    {
        public int Movie_ID {  get; set; }
        public string Movie_Name { get; set; }
        public string Release_Date { get; set; }
        public int Cat_ID { get; set; }
        public int rate { get; set; }
    }
}