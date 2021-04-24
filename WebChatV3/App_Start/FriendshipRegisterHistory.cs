using BSS;
using System;

public class FriendshipRegisterHistory
{
    public long IDUserSend { get; set; }
    public Guid ObjectGuid { get; set; }
    public long IDUserReceive { get; set; }
    public bool isDelete { get; set; }
    public bool isAgree { get; set; }
    public DateTime CreateDate { get; set; }

    public static string GetOneByObjectGuid(Guid ObjectGuid, out FriendshipRegisterHistory outFriendshipRegisterHistory)
    {
        return DBM.GetOne("usp_FriendshipRegisterHistory_GetOneByGuidID", new { ObjectGuid }, out outFriendshipRegisterHistory);
    }
    public string InsertOrUpdate(DBM dbm, out FriendshipRegisterHistory friendshipRegisterHistory)
    {
        friendshipRegisterHistory = null;

        string msg = dbm.SetStoreNameAndParams("usp_FriendshipRegisterHistory_Insert", new
        {
            IDUserSend,
            IDUserReceive,
            isDelete,
            isAgree
        });
        if (msg.Length > 0) return msg;

        return dbm.GetOne(out friendshipRegisterHistory);
    }
}
