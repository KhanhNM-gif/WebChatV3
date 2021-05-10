using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Member:UserAccount
{
    public long IdServer { get; set; }
    public Guid ObjectMemberGuid { get; set; }
    public DateTime JoinDate { get; set; }
    public bool IsLeave { get; set; }

    public static string GetOneByIDUserAndGuidServer(long IdUser,Guid GuidServer,out Member outMember)
    {
        return DBM.GetOne("usp_Member_GetOneByIDUserAndGuidServer", new { IdUser, GuidServer }, out outMember);
    }
    public static string GetList(Guid ServerGuid, out List<Member> outMember)
    {
        return DBM.GetList("usp_Member_GetListByServerGuid", new { ServerGuid }, out outMember);
    }
}
