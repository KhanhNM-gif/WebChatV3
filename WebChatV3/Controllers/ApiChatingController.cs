using BSS;
using BSS.DataValidator;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebChatV3.Models;

namespace WebChatV3.Controllers
{
    public class ApiChatingController : Authentication
    {
        IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

        [HttpPost]
        public Result getListServer()
        {
            if (!ResultCheckToken.isOk) return ResultCheckToken;

            string msg = UserAccount.GetListFriendById(UserToken.ID, out List<UserAccount> outLtUserAccount);
            if (msg.Length > 0) return Log.ProcessError(msg).ToResultError();

            return outLtUserAccount.ToResultOk();
        }
        [HttpPost]
        public async Task<Result> SendMessageToServer(InputSendMessage inputSendMessage)
        {
            if (!ResultCheckToken.isOk) return ResultCheckToken;

            string msg = await DoSendMessageToServer(inputSendMessage);
            if (msg.Length > 0) return Log.ProcessError(msg).ToResultError();

            return msg.ToResultOk();
        }

        private async Task<string> DoSendMessageToServer(InputSendMessage inputSendMessage)
        {
            inputSendMessage.UserIDCreate = UserToken.UserID;

            string msg = DoSendMessageToServer_Validate(inputSendMessage);
            if (msg.Length > 0) return msg;

            DBM dbm = new DBM();
            dbm.BeginTransac();

            msg = DoSendMessageToServer_ObjectToGuid(dbm, inputSendMessage, out Message outMessage);
            if (msg.Length > 0) { dbm.RollBackTransac(); return msg; }
            else dbm.CommitTransac();

            GroupHub groupHub = ChatHub.listGroup.GetGroupHub(inputSendMessage.RoomGuid.ToString(), inputSendMessage.IsMessagePrivate);

            string NameGroup = (inputSendMessage.IsMessagePrivate ? "private " : "public ") + inputSendMessage.RoomGuid;
            if (groupHub==null)
            {
                if (inputSendMessage.IsMessagePrivate) ChatHub.listGroup.CreatePrivateGroupHub(inputSendMessage.RoomGuid, NameGroup, hubContext);
                else ChatHub.listGroup.CreatePublicGroupHub(inputSendMessage.GuidServer, NameGroup, hubContext);
            }
            

            await hubContext.Groups.Add(inputSendMessage.ConnectionID, NameGroup);

            List<Message> messages = new List<Message>();

            UserAccount.GetOneByIdUser(UserToken.UserID, out UserAccount outUserAccount);
            outMessage.UserAccount = outUserAccount;

            messages.Add(outMessage);

            hubContext.Clients.Group(NameGroup).addNewMessageToPage(messages);

            return msg;

        }

        private string DoSendMessageToServer_Validate(InputSendMessage inputMessage)
        {
            // string msg = DataValidator.Validate(new { inputMessage.MessageContent, inputMessage.IsMessagePrivate, inputMessage.RoomGuid, inputMessage.GuidServer, inputMessage.MessageType }).ToErrorMessage();
            string msg = "";
            if (Guid.Empty != inputMessage.GuidServer) msg = DoSendMessageToGroupServer__Validate(inputMessage);
            else DoSendMessageToPrivateServer__Validate(inputMessage);

            return msg;

        }
        private string DoSendMessageToGroupServer__Validate(InputSendMessage inputMessage)
        {
            string msg = Server.GetOneByObjectGuid(inputMessage.GuidServer, out Server outServer);
            if (msg.Length > 0) return msg;
            if (outServer == null) return "Server không tồn tại".ToMessageForUser();

            msg = Channel.GetOneByObjectGuidChannelAndServer(inputMessage.GuidServer, inputMessage.RoomGuid, out Channel outChannel);
            if (msg.Length > 0) return msg;
            if (outServer == null) return "Kênh này không tồn tại trong Server".ToMessageForUser();

            msg = Member.GetOneByIDUserAndGuidServer(inputMessage.UserIDCreate, inputMessage.GuidServer, out Member outMember);
            if (msg.Length > 0) return msg;
            if (outMember == null) return "Bạn không ở trong Server Chat. Vui lòng vào nhóm trước khi thực hiện".ToMessageForUser();

            return msg;
        }
        private string DoSendMessageToPrivateServer__Validate(InputSendMessage inputMessage)
        {

            string msg = UserAccount.GetOnByObjectGuid(inputMessage.RoomGuid, out UserAccount outUserAccount);
            if (msg.Length > 0) return msg;
            if (outUserAccount == null) return "Người bạn không tồn tại tồn tại".ToMessageForUser();

            msg = FriendShip.GetOne(outUserAccount.Id, inputMessage.UserIDCreate, out FriendShip outFriendShip);
            if (msg.Length > 0) return msg;
            if (outUserAccount == null) return "Bạn chưa kết bạn với người này".ToMessageForUser();

            inputMessage.RoomGuid = outFriendShip.ObjectGuid;

            return msg;
        }

        private string DoSendMessageToServer_ObjectToGuid(DBM dbm, InputSendMessage inputSendMessage, out Message outMessage)
        {
            string msg = inputSendMessage.InsertOrUpdate(dbm, out outMessage);
            if (msg.Length > 0) return msg;

            msg = Log.WriteHistoryLog(dbm, "Thêm tin nhắn", outMessage.ObjectGuid);
            if (msg.Length > 0) return msg;

            return msg;
        }
    }
}