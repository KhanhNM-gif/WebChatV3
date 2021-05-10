using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChatV3.Models
{
    public class InputSendMessage : Message
    {
        public string ConnectionID { get; set; }
        public Guid GuidServer { get; set; }
    }
}