using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChatV3.Models
{
    public class UserRegister:User
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}