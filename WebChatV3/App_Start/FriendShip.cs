using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class FriendShip
{
    public long IdUser1 { get; set; }
    public long IdUser2 { get; set; }
    public Guid ObjectGuid { get; set; }
    public bool isDelete { get; set; }
    public DateTime CreateDate { get; set; }

    public static string GetOne(long idUser1,long idUser2, out FriendShip outFriendShip)
    {
        return DBM.GetOne("usp_Friendship_GetOneBy2IDUser", new { idUser1, idUser2 }, out outFriendShip);
    }

    public static string InsertByObjectGuidRegister(DBM dbm, Guid ObjectGuid, out FriendShip outFriendShip)
    {
        outFriendShip = null;

        string msg = dbm.SetStoreNameAndParams("usp_Friendship_InsertByObjectGuidRegister", new { ObjectGuid });
        if (msg.Length > 0) return msg;

        return dbm.GetOne(out outFriendShip);
    }
    public string Delete(DBM dbm,out FriendShip outFriendShip)
    {
        outFriendShip = null;

        string msg = dbm.SetStoreNameAndParams("usp_Friendship_Delete", new 
        {
            IdUser1,
            IdUser2
        });
        if (msg.Length > 0) return msg;

        return dbm.GetOne(out outFriendShip);
    }
}
