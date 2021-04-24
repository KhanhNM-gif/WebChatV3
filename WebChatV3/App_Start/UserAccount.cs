using BSS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

public class UserAccount
{
    public long Id { get; set; }
    public Guid ObjectGuid { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string Name { get; set; }
    public bool Sex { get; set; }
    public DateTime BirthDate { get; set; }
    public bool IsDelete { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime LastUpdate { get; set; }
    public string Image { get; set; }

    static public string Login(string Username, out UserAccount outUserAccount)
    {
        return DBM.GetOne("usp_User_Login", new { Username }, out outUserAccount);
    }

    static public string GetListFriendById(long IdUser, out List<UserAccount> outLtUserAccount)
    {
        return DBM.GetList("usp_User_GetListFriendByUserID", new { IdUser }, out outLtUserAccount);
    }

    static public string GetListByIdServer(long IdUser, out List<UserAccount> outLtUserAccount)
    {
        return DBM.GetList("usp_User_GetListByServerID", new { IdUser }, out outLtUserAccount);
    }
    static public string GetListByGuidServer(Guid objectGuid, out List<UserAccount> outLtUserAccount)
    {
        return DBM.GetList("usp_User_GetListByGuidServer", new { objectGuid }, out outLtUserAccount);
    }
    static public string GetOnByObjectGuid(Guid objectGuid, out UserAccount outUserAccount)
    {
        return DBM.GetOne("usp_User_GetOnByObjectGuid", new { objectGuid }, out outUserAccount);
    }
    static public string GetListByIdUsers(string IdUsers, out List<UserAccount> outLtUserAccount)
    {
        return DBM.GetList("usp_User_GetListByIdUsers", new { IdUsers }, out outLtUserAccount);
    }
}

