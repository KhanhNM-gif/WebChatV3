using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class Server
{
    public long Id;
    public Guid ObjectGuid;
    public long IdUserCreate;
    public string Name;
    public long ServerCode;
    public bool isDelete;

    static public string GetListByIdUser(long IdUser, out List<Server> outLtServer)
    {
        return DBM.GetOne("usp_Server_GetListByUserID", new { IdUser }, out outLtServer);
    }
}
