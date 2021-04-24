using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace WebChatV3.Controllers
{
    public class ApiFriendshipController : Authentication
    {
        /// <summary>
        /// API
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Result GetListFriends()
        {
            if (!ResultCheckToken.isOk) return ResultCheckToken;

            string msg = UserAccount.GetListFriendById(UserToken.UserID, out List<UserAccount> outLtUserAccount);
            if (msg.Length > 0) return Log.ProcessError(msg).ToResultError();

            return outLtUserAccount.ToResultOk();
        }

        /// <summary>
        /// API
        /// </summary>
        /// <param name="idUserReceive"></param>
        /// <returns></returns>
        [HttpPost]
        public Result SendFriendInvitations(long idUserReceive)
        {
            if (!ResultCheckToken.isOk) return ResultCheckToken;

            DBM dbm = new DBM();
            dbm.BeginTransac();

            string msg = DoSendFriendInvitations(dbm, idUserReceive);
            if (msg.Length > 0) return Log.ProcessError(msg).ToResultError();

            dbm.CommitTransac();

            return msg.ToResultOk();
        }

        private string DoSendFriendInvitations(DBM dbm, long idUserReceive)
        {
            FriendshipRegisterHistory friendshipRegisterHistory = new FriendshipRegisterHistory()
            {
                IDUserSend = UserToken.ID,
                IDUserReceive = idUserReceive
            };

            string msg = friendshipRegisterHistory.InsertOrUpdate(dbm, out friendshipRegisterHistory);
            if (msg.Length > 0) return msg;

            return Log.WriteHistoryLog(dbm, "Gửi Lời Mời Kết Bạn", friendshipRegisterHistory.ObjectGuid);
        }
        /// <summary>
        /// API
        /// </summary>
        /// <param name="objectGuid"></param>
        /// <returns></returns>
        [HttpPost]
        public Result AgreeFriendInvitations(Guid objectGuid)
        {
            if (!ResultCheckToken.isOk) return ResultCheckToken;

            string msg = DoAgreeFriendInvitations(objectGuid, UserToken.ID);
            if (msg.Length > 0) return Log.ProcessError(msg).ToResultError();

            return msg.ToResultOk();
        }

        private string DoAgreeFriendInvitations(Guid objectGuid, long IdUser)
        {

            string msg = DoAgreeFriendInvitations_Validate(objectGuid, IdUser);
            if (msg.Length > 0) return msg;

            DBM dbm = new DBM();
            dbm.BeginTransac();

            msg = DoAgreeFriendInvitations_ObjectToDB(dbm, objectGuid, out FriendShip friendShip);
            if (msg.Length > 0) { dbm.RollBackTransac(); return msg; }

            dbm.CommitTransac();

            return msg;
        }

        private string DoAgreeFriendInvitations_Validate(Guid objectGuid, long IdUser)
        {

            string msg = FriendshipRegisterHistory.GetOneByObjectGuid(objectGuid, out FriendshipRegisterHistory outFriendshipRegisterHistory);
            if (msg.Length > 0) return msg;

            if (outFriendshipRegisterHistory != null) return "Lời mời kết bạn không tồn tại".ToMessageForUser();

            if (outFriendshipRegisterHistory.IDUserReceive != IdUser) return "Bạn không thể đồng ý lời mời kết bạn vì nó không rành cho bạn".ToMessageForUser();

            if (outFriendshipRegisterHistory.isDelete) return "Lời mời kết bạn đã bị xóa".ToMessageForUser();

            return msg;
        }

        private string DoAgreeFriendInvitations_ObjectToDB(DBM dbm, Guid objectGuid, out FriendShip outFriendShip)
        {
            string msg = FriendShip.InsertByObjectGuidRegister(dbm, objectGuid, out outFriendShip);
            if (msg.Length > 0) return msg;

            return Log.WriteHistoryLog(dbm, "Đồng ý lời mời kết bạn", outFriendShip.ObjectGuid);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectGuid"></param>
        /// <returns></returns>
        [HttpPost]
        public Result Delete(long idFriend)
        {
            if (!ResultCheckToken.isOk) return ResultCheckToken;

            string msg=DoDelete(idFriend, UserToken.ID);
            if (msg.Length > 0) return Log.ProcessError(msg).ToResultError();

            return msg.ToResultOk();
        }

        private string DoDelete(long idFriend, long IdUser)
        {
            string msg = DoDelete_Validate(idFriend, IdUser);
            if (msg.Length > 0) return msg;

            FriendShip friendship = new FriendShip()
            {
                IdUser1 = IdUser,
                IdUser2 = idFriend
            };

            DBM dbm = new DBM();
            dbm.BeginTransac();

            msg = DoDelete_ObjectToDB(dbm, friendship, out FriendShip friendShip);
            if (msg.Length > 0) { dbm.RollBackTransac(); return msg; }

            dbm.CommitTransac();

            return msg;
        }

        private string DoDelete_Validate(long idFriend, long IdUser)
        {
            string msg = FriendShip.GetOne(idFriend, IdUser, out FriendShip outFriendship);
            if (msg.Length > 0) return msg;

            if (outFriendship == null) return "Không tồn tại quan hệ bạn bè ".ToMessageForUser();

            if (outFriendship.isDelete) return "Bạn đã không còn là bạn với người này".ToMessageForUser();

            return msg;
        }

        private string DoDelete_ObjectToDB(DBM dbm, FriendShip friendShip, out FriendShip outFriendShip)
        {
            string msg = friendShip.Delete(dbm, out outFriendShip);
            if (msg.Length > 0) return msg;

            return Log.WriteHistoryLog(dbm, "Xóa quan hệ bạn bè", outFriendShip.ObjectGuid);
        }

    }
}
