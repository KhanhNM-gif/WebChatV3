using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class UserRegisterOTP
{
    public long ID { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public int SubscriptionNetworkID { get; set; }    
    public string OTPCode { get; set; }
    public string IPAddress { get; set; }    
    public DateTime CreateDate { get; set; }
    public DateTime ExpriedDate { get; set; }

    public string Insert()
    {
        string msg = DBM.GetOne("usp_UserRegisterOTP_Insert", new { Email, Mobile, OTPCode, IPAddress, CreateDate, ExpriedDate }, out UserRegisterOTP oNew);
        if (msg.Length > 0) return msg;

        ID = oNew.ID;

        return msg;
    }

    static public string GetAllExpiredDate(out List<UserRegisterOTP> ltUserRegisterOTP)
    {
        return DBM.GetList("usp_UserRegisterOTP_SelectAllExpiredDate", new { ExpriedDate=DateTime.Now }, out ltUserRegisterOTP);
    }

    static public string GetByPhoneNumber(string Email,string Mobile, string OTPCode, out UserRegisterOTP o)
    {
        return DBM.GetOne("usp_UserRegisterOTP_SelectByMobile", new { Email, Mobile, OTPCode }, out o);
    }

    static public string GetCountByPhoneNumber(string Mobile, out int count)
    {
        count = 0;

        string msg = DBM.GetList("usp_UserRegisterOTP_SelectCountByMoblie", new { Mobile }, out List<UserRegisterOTP> lt);
        if (msg.Length > 0) return msg;

        count = lt.Count;
        return msg;
    }
    public static string ConfirmOTPEmailOrMoble(string Email, string Mobile, out UserRegisterOTP userRegisterOTP)
    {
        return DBM.GetOne("usp_UserRegisterOTP_CheckEmailOrMobile", new { Email, Mobile }, out userRegisterOTP);
    }
}