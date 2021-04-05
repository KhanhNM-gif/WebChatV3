using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebChatV3.Models
{
    public class UserLogin
    {
        
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsRememberPassword { get; set; }
    }
}