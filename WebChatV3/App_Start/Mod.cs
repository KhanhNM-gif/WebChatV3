using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Mod : Member
{
    public int IdRoleGroup { get; set; }
    public bool IsDelegacyDelete { get; set; }
    public DateTime DelegacyDate { get; set; }

    public static string GetOne (long IdMember,long IdServer,out Mod outMod)
    {
        return DBM.GetOne("usp_Mod_GetOne", new { IdMember, IdServer }, out outMod);
    }
}
