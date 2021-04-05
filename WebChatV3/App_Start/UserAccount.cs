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

    static public string Login(string Username, out UserAccount outUserAccount)
    {
        return DBM.GetOne("usp_User_Login", new { Username }, out outUserAccount);
    }
}

