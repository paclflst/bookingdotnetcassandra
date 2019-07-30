using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingService.Models
{
    public partial class LoginByUsernamePassword_Result
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}