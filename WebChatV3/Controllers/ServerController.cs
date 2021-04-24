using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace WebChatV3.Controllers
{
    public class ServerController : Authentication
    {
        [HttpPost]
        public Result GetListServer()
        {
            if (!ResultCheckToken.isOk) return ResultCheckToken;

            string msg = UserAccount.GetListFriendById(UserToken.ID, out List<UserAccount> outLtUserAccount);
            if (msg.Length > 0) return Log.ProcessError(msg).ToResultError();

            return outLtUserAccount.ToResultOk();
        }
    }
}
