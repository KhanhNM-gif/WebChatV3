using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChatV3.Models
{
    public class InputSendMessage
    {
        public string connectionID { get; set; }
        public Guid guidSend { get; set; }
        public bool isPrivate { get; set; }
        public string ContentMessage { get; set; }
    }
}