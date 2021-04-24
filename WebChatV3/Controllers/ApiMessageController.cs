using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebChatV3.Controllers
{
    public class ApiMessageController : Authentication
    {
        [HttpGet]
        public Result LoadMessage(Guid GuidUser)
        {
            if (!ResultCheckToken.isOk) return ResultCheckToken;

            string msg = DoLoadMessage(GuidUser, out List<Message> outLtMessage);
            if (msg.Length > 0) return Log.ProcessError(msg).ToResultError();

            return outLtMessage.ToResultOk();
        }
        private string DoLoadMessage(Guid GuidUser, out List<Message> outLtMessage)
        {
            outLtMessage = null;

            //string msg = DataValidator.Validate(new { GuidUser }).ToErrorMessage();
            //if (msg.Length > 0) return msg;

            string msg = UserAccount.GetOnByObjectGuid(GuidUser, out UserAccount outUserAccount);
            if (msg.Length > 0) return msg;
            if (outUserAccount == null) return "Không tồn tại tài khoản".ToMessageForUser();

            msg = FriendShip.GetOne(UserToken.UserID, outUserAccount.Id, out FriendShip outFriendShip);
            if (msg.Length > 0) return msg;
            if (outFriendShip == null) return "Bạn chưa kết bạn với người này".ToMessageForUser();

            msg = Message.GetPM(UserToken.UserID, outUserAccount.Id, out outLtMessage);
            if (msg.Length > 0) return msg;

            var IdUsers = string.Join(",",outLtMessage.Select(v=>v.UserIDCreate).Distinct().ToList());
            msg = UserAccount.GetListByIdUsers(IdUsers,out List<UserAccount> outLtUser);
            if (msg.Length > 0) return msg;

            foreach (var item in outLtUser) outLtMessage.Where(v => v.UserIDCreate == item.Id).ToList().ForEach(v => v.UserAccount = item);

            return msg;
        }
    }
}
