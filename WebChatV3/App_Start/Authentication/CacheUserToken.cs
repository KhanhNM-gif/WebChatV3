using BSS;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for Cache
/// </summary>
public static class CacheUserToken
{
    private static List<UserToken> LtUser_Token = null;
    const int HOUR_TIMEOUT_TOKEN = 4;
    const int HOUR_TIMEOUT_TOKEN_REMEMBER = 24 * 7;

    private static string GetAllToken()
    {
        string msg = UserToken.GetAllExpiredDate(out LtUser_Token);
        if (msg.Length > 0) return msg;

        foreach (var item in LtUser_Token) item.TimeUpdateExpiredDateToDB = DateTime.Now;

        return msg;
    }
    public static string CreateToken(UserAccount accountUser, bool IsRememberPassword, out UserToken UserToken)
    {
        UserToken = null;
        string msg = "";

        if (LtUser_Token == null)
        {
            msg = GetAllToken();
            if (msg.Length > 0) return msg;
        }

        UserToken = new UserToken
        {
            UserID = accountUser.Id,
            IsRememberPassword = IsRememberPassword,
            Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
            ExpiredDate = DateTime.Now.AddHours(IsRememberPassword ? HOUR_TIMEOUT_TOKEN_REMEMBER : HOUR_TIMEOUT_TOKEN),
            CreateDate = DateTime.Now
        };

        msg = UserToken.Insert();
        if (msg.Length > 0) return msg;

        LtUser_Token.Add(UserToken);

        return msg;
    }
    public static Result GetResultUserToken(out UserToken UserToken)
    {
        string msg = GetUserToken(out UserToken);
        if (msg.Length > 0) return Log.ProcessError(msg).ToResult(-1);
        return Result.ResultOk;
    }
    public static bool GetResultUserToken(string Token)
    {
        string msg = GetUserToken(Token,out UserToken user);
        if (msg.Length > 0) return false;

        return true;
    }
    public static string GetUserToken(out UserToken UserToken)
    {
        UserToken = null;

        HttpContext context = HttpContext.Current;
        if (context == null) return "HttpContext.Current == null".ToMessageForUser();

        HttpRequest request = context.Request;
        if (request == null) return "request == null".ToMessageForUser();

        string token = "";
        if (request.Headers["Authorization"] != null)
            token = request.Headers["Authorization"];
        else
            return ("Header không chứa key Authorization (có value là Token đăng nhập)").ToMessageForUser();

        return GetUserToken(token, out UserToken);
    }

    public static string GetUserToken(string Token, out UserToken UserToken)
    {
        string msg = "";

        UserToken = null;

        if (LtUser_Token == null)
        {
            msg = GetAllToken();
            if (msg.Length > 0) return msg;
        }

        if (LtUser_Token == null) return "LtUser_Token == null";

        var vDB_Token = LtUser_Token.Where(v => v != null && v.Token == Token).ToList();
        if (vDB_Token == null || vDB_Token.Count() == 0) return "Không tồn tại token".ToMessageForUser();

        UserToken = vDB_Token.First();
        if (UserToken.ExpiredDate < DateTime.Now) return "Token đã hết hạn".ToMessageForUser();

        UserToken.ExpiredDate = DateTime.Now.AddHours(UserToken.IsRememberPassword ? HOUR_TIMEOUT_TOKEN_REMEMBER : HOUR_TIMEOUT_TOKEN);

        if ((DateTime.Now - UserToken.TimeUpdateExpiredDateToDB).TotalMinutes > 3)
        {
            UserToken.TimeUpdateExpiredDateToDB = DateTime.Now;
            msg = UserToken.UpdateExpiredDate(UserToken.ID, UserToken.ExpiredDate);
            if (msg.Length > 0) return msg;
        }

        return "";
    }
    public static string Logout()
    {
        string msg = "";
        UserToken UserToken;
        msg = GetUserToken(out UserToken);
        if (msg.Length > 0) return msg;

        bool isRemove = LtUser_Token.Remove(UserToken);
        if (!isRemove) return "Xóa token lỗi";

        msg = UserToken.Delete(UserToken.ID);
        if (msg.Length > 0) return msg;

        return msg;
    }
}