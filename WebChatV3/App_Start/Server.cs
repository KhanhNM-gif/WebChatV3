using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class Server
{
    public long Id { get; set; }
    public Guid ObjectGuid { get; set; }
    public long IdUserCreate { get; set; }
    public string Name { get; set; }
    public string ServerCode { get; set; }
    public bool isDelete { get; set; }
    public string Abbreviation { get; set; }
    public string ChannelDefault{ get; set; }
    public string Image{ get; set; }
    public string InsertOrUpdate(DBM dbm, out Server outServer)
    {
        outServer = null;
        string msg = dbm.SetStoreNameAndParams("usp_Server_InsertOrUpdate",
            new
            {
                Id,
                ObjectGuid,
                IdUserCreate,
                ServerCode,
                Name,
                isDelete,
                Image
            });
        if (msg.Length > 0) return msg;

        return dbm.GetOne(out outServer);
    }
    static public string GetListByIdUser(long IdUser, out List<Server> outLtServer)
    {
        return DBM.GetList("usp_Server_GetListByUserID", new { IdUser }, out outLtServer);
    }

    static public string GetOneByObjectGuid(Guid ObjectGuid,out Server outServer)
    {
        return DBM.GetOne("usp_Server_GetByObjectGuid", new { ObjectGuid }, out outServer);
    }
}
