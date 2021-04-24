using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Member
{
    public long IdUser { get; set; }
    public long IdServer { get; set; }
    public Guid ObjectGuid { get; set; }
    public DateTime CreateDate { get; set; }
    public bool IsDelete { get; set; }

    public static string GetListByIdUser(long IdUser,List<Member> outListMember)
    {
        return DBM.GetList("usp_User_GetListByServerID", new { IdUser }, out outListMember);
    }
}
