using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Message
{
    public int ObjectCategory { get; set; }
    public long Id { get; set; }
    public UserAccount UserAccount { get; set; }
    public Guid ObjectGuid { get; set; }
    public long UserIDCreate { get; set; }
    public int MessageType { get; set; }
    public string MessageContent { get; set; }
    public bool IsDelete { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime LastUpdate { get; set; }
    public bool IsMessagePrivate { get; set; }

    public static string GetPM(long IdUser,long IdUserFriend,out List<Message> outLtMessage)
    {
        return DBM.GetList("usp_Message_GetPM", new { IdUser, IdUserFriend }, out outLtMessage);
    }
}

public class ShowUserAcountShow
{
    public long IdUser { get; set; }
    public long ObjectGuid { get; set; }
    public string Image { get; set; }
    public string Name { get; set; }
}
