﻿using BSS;
using BSS.DataValidator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebChatV3.Controllers
{
    public class ApiServerController : Authentication
    {
        [HttpGet]
        public Result GetListServer()
        {
            if (!ResultCheckToken.isOk) return ResultCheckToken;

            string msg = Server.GetListByIdUser(UserToken.UserID, out List<Server> outLtServer);
            if (msg.Length > 0) return Log.ProcessError(msg).ToResultError();

            return outLtServer.ToResultOk();
        }
        [HttpGet]
        public Result GetOne(Guid GuidServer)
        {
            if (!ResultCheckToken.isOk) return ResultCheckToken;

            string msg = DoGetOne(GuidServer, out Server outServer);
            if (msg.Length > 0) return Log.ProcessError(msg).ToResultError();

            return outServer.ToResultOk();
        }
        private string DoGetOne(Guid GuidServer, out Server outServer)
        {
            outServer = null;

            string msg = Member.GetOneByIDUserAndGuidServer(UserToken.UserID, GuidServer, out Member outMember);
            if (msg.Length > 0) return msg;
            if (outMember == null) return "Bạn bạn không phải là thành viên trong Server chat".ToMessageForUser();
            if (outMember.IsLeave) return "Bạn đã đã rời Server chat".ToMessageForUser();

            msg = Server.GetOneByObjectGuid(GuidServer, out outServer);
            if (msg.Length > 0) return msg;
            if (outServer == null) return "Server chat không tồn tại".ToMessageForUser();
            if (outServer.isDelete) return "Server chat đã bị xóa".ToMessageForUser();

            return msg;
        }

        [HttpPost]
        public Result InsertOrUpdate([FromBody] Server server)
        {
            string msg = DoInsertOrUpdate(server, out Server outServer);
            if (msg.Length > 0) return Log.ProcessError(msg).ToResultError();

            return outServer.ToResultOk();
        }

        private string DoInsertOrUpdate(Server server, out Server outServer)
        {
            outServer = null;

            server.IdUserCreate = UserToken.UserID;
            server.ServerCode = Common.AutoGeneratedServerCode(server.IdUserCreate);

            string msg = DoValidateInsertOrUpdate(server);
            if (msg.Length > 0) return msg;

            DBM dbm = new DBM();
            dbm.BeginTransac();

            msg = DoObjectToDb(dbm, server, out outServer);
            if (msg.Length > 0) { dbm.RollBackTransac(); return msg; }
            else dbm.CommitTransac();

            return msg;
        }

        private string DoValidateInsertOrUpdate(Server server)
        {
            if (string.IsNullOrEmpty(server.Name)) return "Tên Server chat không được để trống".ToMessageForUser();

            if (string.IsNullOrEmpty(server.Image)) return "Chưa chọn ảnh đại diện Server chat được để trống".ToMessageForUser();

            string msg = DataValidator.Validate(new { server.Id, server.Name, server.ObjectGuid }).ToErrorMessage();
            if (msg.Length > 0) return msg.ToMessageForUser();

            return msg;
        }

        private string DoObjectToDb(DBM dbm, Server server, out Server outServer)
        {
            string msg = server.InsertOrUpdate(dbm, out outServer);
            if (msg.Length > 0) return msg.ToMessageForUser();

            return Log.WriteHistoryLog(dbm, outServer.Id != 0 ? "Sửa thông tin Server chat" : "Thêm mới Server chat", outServer.ObjectGuid);
        }
    }
}