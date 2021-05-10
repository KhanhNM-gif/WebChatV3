using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebChatV3.Controllers
{
    public class ApiChannelController : Authentication
    {
        [HttpGet]
        public Result GetList(Guid GuidServer)
        {
            if (!ResultCheckToken.isOk) return ResultCheckToken;

            string msg = DoGetList(GuidServer, out List<Channel> outLtChannel);
            if (msg.Length > 0) return Log.ProcessError(msg).ToResultError();

            return outLtChannel.ToResultOk();
        }
        public string DoGetList(Guid GuidServer,out List<Channel> outLtChannel)
        {
            outLtChannel = null;

            string msg = Server.GetOneByObjectGuid(GuidServer, out Server outServer);
            if (msg.Length > 0) return msg;
            if (outServer == null) return "Server chat không tồn tại".ToMessageForUser();
            if (outServer.isDelete) return "Server chat đã bị xóa".ToMessageForUser();

            msg = Channel.GetList(outServer.Id, out outLtChannel);
            if (msg.Length > 0) return msg;

            return msg;
        }

        public Result GetOne(Guid GuidServer, Guid GuidChannel)
        {
            if (!ResultCheckToken.isOk) return ResultCheckToken;

            string msg = DoGetOne(GuidServer, GuidChannel, out Channel outChannel);
            if (msg.Length > 0) return Log.ProcessError(msg).ToResultError();

            return outChannel.ToResultOk();
        }
        public string DoGetOne(Guid GuidServer, Guid GuidChannel, out Channel outChannel)
        {
            outChannel = null;

            string msg = Server.GetOneByObjectGuid(GuidServer, out Server outServer);
            if (msg.Length > 0) return msg;
            if (outServer == null) return "Server chat không tồn tại".ToMessageForUser();
            if (outServer.isDelete) return "Server chat đã bị xóa".ToMessageForUser();

            msg = Channel.GetOneByObjectGuidChannelAndServer(GuidServer, GuidChannel, out outChannel);
            if (msg.Length > 0) return msg;
            if (outChannel == null) return "Kênh chat không tồn tại".ToMessageForUser();
            if (outChannel.IsDelete) return "Kênh chat đã bị xóa".ToMessageForUser();

            return msg;
        }
    }
}
