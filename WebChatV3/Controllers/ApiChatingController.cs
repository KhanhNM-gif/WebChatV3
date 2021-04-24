using BSS;
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

            string GuidGroupSend;
            FriendShip outFriendship = null;

            if (inputSendMessage.isPrivate)
            {
                UserAccount.GetOnByObjectGuid(inputSendMessage.guidSend,out UserAccount outUserAccount);
                FriendShip.GetOne(UserToken.UserID, outUserAccount.Id, out outFriendship);
                GuidGroupSend = outFriendship.ObjectGuid.ToString();
            }
            else
            {
                GuidGroupSend = inputSendMessage.guidSend.ToString();
            }

            GroupHub groupHub = ChatHub.listGroup.GetGroupHub(GuidGroupSend, inputSendMessage.isPrivate);

            string NameGroup;
            
            if (groupHub == null)
            {
                if (inputSendMessage.isPrivate) ChatHub.listGroup.CreatePrivateGroupHub(outFriendship, out NameGroup);
                else ChatHub.listGroup.CreatePublicGroupHub(inputSendMessage.guidSend, out NameGroup);
            }
            else
            {
                NameGroup = (inputSendMessage.isPrivate ? "private " : "public ") + GuidGroupSend;
            }
            await hubContext.Groups.Add(inputSendMessage.connectionID, NameGroup);

            hubContext.Clients.Group(NameGroup).addNewMessageToPage(UserToken.ID, inputSendMessage.ContentMessage);

            return "".ToResultOk();
        }

    }
}