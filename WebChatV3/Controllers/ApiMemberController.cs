using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebChatV3.Controllers
{
    public class ApiMemberController : Authentication
    {
        [HttpGet]
        public Result GetList(Guid GuidServer)
        {
            if (!ResultCheckToken.isOk) return ResultCheckToken;

            string msg = DoGetList(GuidServer, out List<Member> outLtOnlineMember, out List<Member> outLtOfflineMember);
            if (msg.Length > 0) return Log.ProcessError(msg).ToResultError();

            return new { outLtOnlineMember, outLtOfflineMember }.ToResultOk();
        }
        private string DoGetList(Guid GuidServer, out List<Member> outLtOnlineMember, out List<Member> outLtOfflineMember)
        {
            outLtOnlineMember = outLtOfflineMember = null;

            string msg = Server.GetOneByObjectGuid(GuidServer, out Server outServer);
            if (msg.Length > 0) return msg;
            if (outServer == null) return "Server chat không tồn tại".ToMessageForUser();
            if (outServer.isDelete) return "Server chat đã bị xóa".ToMessageForUser();

            msg = Member.GetOneByIDUserAndGuidServer(UserToken.UserID, GuidServer, out Member outMember);
            if (msg.Length > 0) return msg;
            if (outMember == null) return "Bạn không ở trong Server Chat. Vui lòng vào nhóm trước khi thực hiện".ToMessageForUser();

            msg =ListGroupHub.GetConnectMembers(GuidServer, out outLtOnlineMember, out outLtOfflineMember);
            if (msg.Length > 0) return msg;

            return msg;
        }
    }
}
