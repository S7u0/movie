using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace user.Models
{
    public class userModel
    {
        public int User_ID { get; set; }
        public string User_name { get; set; }
        public string User_Password { get; set; }
        public string Email_ID { get; set; }
        public string PhoneNo { get; set; }
        public string City { get; set; }
    }
}