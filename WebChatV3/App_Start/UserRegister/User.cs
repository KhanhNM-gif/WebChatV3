using BSS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class User
{
    public long UserID { get; set; }
    public Guid ObjectGuid { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public bool IsActive { get; set; }
    public bool IsDelete { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime LastUpdate { get; set; }

    public string InsertUpdate(DBM dbm,out User userRegister)
    {
        userRegister = null;
        string msg = dbm.SetStoreNameAndParams("usp_UserRegister_InsertUpdate",
           new
           {
               UserID,
               Email,
               Mobile,
               PasswordHash,
               PasswordSalt,
               IsActive,
           });
        if (msg.Length > 0) return msg;

        return dbm.GetOne(out userRegister);
    }
    public static string GetOneByEmailOrMoble(string Email, string Mobile, out User userRegister)
    {
        return DBM.GetOne("usp_UserRegister_CheckEmailOrMoblie", new { Email, Mobile }, out userRegister);
    }
    public static string GetOneByObjectGuid(Guid ObjectGuid, out User userRegister)
    {
        return DBM.GetOne("usp_UserRegister_GetOneByObjectGuid", new { ObjectGuid }, out userRegister);
    }
    public static string GetOneByUserID(long UserID, out User userRegister)
    {
        return DBM.GetOne("usp_UserRegister_GetOneByUserID", new { UserID }, out userRegister);
    }
    public static string Login(string Username, string CountriesCode, out User userRegister)
    {
        return DBM.GetOne("usp_UserRegister_Login", new { Username, CountriesCode }, out userRegister);
    }
    public static string GetUserByTextSearch(string TextSearch, string AreaCode, out User userRegister)
    {
        return DBM.GetOne("usp_UserRegister_GetOneByTextSearch", new { TextSearch, AreaCode }, out userRegister);
    }
}