using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Message
{
    private const bool Private_Messge = true;
    private const bool Public_Messge = false;

    public int ObjectCategory { get; set; }
    public long Id { get; set; }
    public UserAccount UserAccount { get; set; }
    public Guid RoomGuid { get; set; }
    public Guid ObjectGuid { get; set; }
    public long UserIDCreate { get; set; }
    public int MessageType { get; set; }
    public string MessageContent { get; set; }
    public bool IsDelete { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime LastUpdate { get; set; }
    public bool IsMessagePrivate { get; set; }

    public static string GetPrivateMessage(Guid RoomGuid, out List<Message> outLtMessage)
    {
        bool IsMessagePrivate = true;
        return DBM.GetList("usp_Message_GetMessages", new { RoomGuid, IsMessagePrivate }, out outLtMessage);
    }
    public static string GetPublicMessage(Guid RoomGuid, out List<Message> outLtMessage)
    {
        bool IsMessagePrivate = false;
        return DBM.GetList("usp_Message_GetMessages", new { RoomGuid, IsMessagePrivate }, out outLtMessage);
    }

    public string InsertOrUpdate(DBM dbm,out Message outMessage)
    {
        outMessage = null;

        string msg = dbm.SetStoreNameAndParams("usp_Message_InsertOrUpdate",
            new
            {
                Id,
                RoomGuid,
                UserIDCreate,
                MessageType,
                MessageContent,
                IsMessagePrivate
            });
        if (msg.Length > 0) return msg;

        return dbm.GetOne(out outMessage);
    }
}

public class ShowUserAcountShow
{
    public long IdUser { get; set; }
    public long ObjectGuid { get; set; }
    public string Image { get; set; }
    public string Name { get; set; }
}
