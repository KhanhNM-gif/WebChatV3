using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;

public static class Common
{
    public static string GetClientIpAddress(this HttpRequestMessage request)
    {
        if (request.Properties.ContainsKey("MS_HttpContext"))
        {
            dynamic ctx = request.Properties["MS_HttpContext"];
            if (ctx != null)
            {
                return ctx.Request.UserHostAddress;
            }
        }

        if (request.Properties.ContainsKey("System.ServiceModel.Channels.RemoteEndpointMessageProperty"))
        {
            dynamic remoteEndpoint = request.Properties["System.ServiceModel.Channels.RemoteEndpointMessageProperty"];
            if (remoteEndpoint != null)
            {
                return remoteEndpoint.Address;
            }
        }

        return null;
    }

    public static string GetIPAddress(this HttpRequest request)
    {
        string ipAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (string.IsNullOrEmpty(ipAddress)) ipAddress = request.ServerVariables["REMOTE_ADDR"];

        return ipAddress;
    }
    public static string GetIPAddress()
    {
        HttpContext context = HttpContext.Current;
        if (context == null) return "";

        HttpRequest request = context.Request;
        if (request == null) return "";

        string ipAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (string.IsNullOrEmpty(ipAddress)) ipAddress = request.ServerVariables["REMOTE_ADDR"];

        return ipAddress;
    }

    public static string ToObject<T>(this JObject data, string key, out T t)
    {
        t = default(T);
        try
        {
            if (string.IsNullOrWhiteSpace(key)) return "key is null or empty @JObjectToOject";
            if (data[key] == null) return string.Format("data[key] is null (key: {0})", key);

            t = data[key].ToObject<T>();
            return "";
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }
    public static string ToString(this JObject data, string key, out string str)
    {
        str = "";
        try
        {
            if (string.IsNullOrWhiteSpace(key)) return "key is null or empty @JObjectToString";
            if (data[key] == null) return string.Format("data[key] is null (key: {0})", key);

            str = data[key].ToString();
            return "";
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }
    public static string ToNumber(this JObject data, string key, out int number)
    {
        number = 0;
        try
        {
            string msg = data.ToString(key, out string str);
            if (msg.Length > 0) return msg;

            number = int.Parse(str);
            return "";
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }

    public static string ToGuid(this JObject data, string key, out Guid guid)
    {
        guid = Guid.Empty;
        try
        {
            string msg = data.ToString(key, out string str);
            if (msg.Length > 0) return msg;

            guid = Guid.Parse(str);
            return "";
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }
    public static string ToDateTime(this JObject data, string key, out DateTime date)
    {
        date = DateTime.Now;
        try
        {
            string msg = data.ToString(key, out string str);
            if (msg.Length > 0) return msg;

            date = DateTime.Parse(str);
            return "";
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }
    public static bool IsValidDateTime(string value)
    {
        DateTime dateTime;
        if (DateTime.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out dateTime))
        {
            return true;
        }
        return false;
    }
    public static string ToBool(this JObject data, string key, out bool b)
    {
        b = true;
        try
        {
            string msg = data.ToString(key, out string str);
            if (msg.Length > 0) return msg;

            b = bool.Parse(str);
            return "";
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }

    public static byte[] GenerateRandomBytes(int length)
    {
        var result = new byte[length];
        RandomNumberGenerator.Create().GetBytes(result);
        return result;
    }
    public static byte[] GetInputPasswordHash(string pwd, byte[] salt)
    {
        int passwordDerivationIteration = 1000;
        int passwordBytesLength = 64;
        var inputPwdBytes = Encoding.UTF8.GetBytes(pwd);
        var inputPwdHasher = new Rfc2898DeriveBytes(inputPwdBytes, salt, passwordDerivationIteration);
        return inputPwdHasher.GetBytes(passwordBytesLength);
    }
}