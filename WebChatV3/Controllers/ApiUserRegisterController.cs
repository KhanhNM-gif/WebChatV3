using BSS;
using BSS.DataValidator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using WebChatV3.Models;

namespace WebChatV3.Controllers
{
    public class ApiUserRegisterController : Authentication
    {
        [HttpPost]
        public Result RegisterUser([FromBody] UserRegister InputuserRegister)
        {
            try
            {
                string msg = DoRegisterUser(InputuserRegister, out User userRegister);
                if (msg.Length > 0) return Log.ProcessError(msg).ToResultError();
                return userRegister.ToResultOk();
            }
            catch (Exception ex)
            {
                return Log.ProcessError(ex.ToString(), InputuserRegister).ToResultError();
            }
        }


        public static string DoRegisterUser(UserRegister InputuserRegister, out User userRegister)
        {
            string msg = "";
            userRegister = null;

            if (msg.Length > 0) return msg;

            msg = DoRegisterUser_Validate(InputuserRegister);
            if (msg.Length > 0) return msg;

            msg = DoRegisterUser_ObjectToDB(InputuserRegister, out userRegister);
            if (msg.Length > 0) return msg;

            return "";
        }
        private static string DoRegisterUser_Validate(UserRegister InputuserRegister)
        {
            string msg = "";

            if (string.IsNullOrEmpty(InputuserRegister.Email) && string.IsNullOrEmpty(InputuserRegister.Mobile))
                if (msg.Length > 0) return ("Bạn phải chọn đăng ký bằng email hoặc số điện thoại").ToMessageForUser();

            if (string.IsNullOrEmpty(InputuserRegister.Email) && !string.IsNullOrEmpty(InputuserRegister.Mobile))
            {
                msg = ValidateMobile(InputuserRegister.Mobile);
                if (msg.Length > 0) return msg;
            }

            if (!string.IsNullOrEmpty(InputuserRegister.Email) && string.IsNullOrEmpty(InputuserRegister.Mobile))
            {
                msg = ValidateEmail(InputuserRegister.Email);
                if (msg.Length > 0) return msg;
            }

            if (InputuserRegister.UserID == 0)
            {
                msg = UserRegister.GetOneByEmailOrMoble(InputuserRegister.Email, InputuserRegister.Mobile, out User userRegisterOut);
                if (msg.Length > 0) return msg;

                if (userRegisterOut != null) return ("Email và số điện thoại đã tồn tại trong hệ thống").ToMessageForUser();
            }

            if (InputuserRegister.Password != InputuserRegister.ConfirmPassword) return ("Mật khẩu đang không trùng nhau").ToMessageForUser();

            InputuserRegister.PasswordSalt = Common.GenerateRandomBytes(16);
            InputuserRegister.PasswordHash = Common.GetInputPasswordHash(InputuserRegister.Password, InputuserRegister.PasswordSalt);

            return msg;
        }
        private static string DoRegisterUser_ObjectToDB(UserRegister inputUserRegister, out User ur)
        {
            string msg = inputUserRegister.InsertUpdate(new DBM(), out ur);
            if (msg.Length > 0) return msg;

            return msg;
        }
        private static string ValidateMobile(string PhoneNumber)
        {
            string msg = DataValidator.Validate(new { Mobile = PhoneNumber }).ToErrorMessage();
            if (msg.Length > 0) return msg.ToMessageForUser();
            return msg;
        }
        private static string ValidateEmail(string value)
        {
            bool isEmail = Regex.IsMatch(value, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (!isEmail) return ("Email không đúng định dạng").ToMessageForUser();

            return "";
        }
        [HttpPost]
        public Result Login(UserLogin userLogin)
        {
            string msg = "";

            msg = DoLogin(userLogin, out UserToken userToken, out UserAccount userAccount);
            if (msg.Length > 0) { return Log.ProcessError(msg).ToResultError(); }

            return new
            {
                userToken = userToken,
                userAccount = userAccount
            }.ToResultOk();
        }

        private string DoLogin(UserLogin userLogin, out UserToken userToken, out UserAccount userAccount)
        {
            string msg = "";
            userToken = null;
            userAccount = null;

            DoValidateLogin(userLogin);
            if (msg.Length > 0) { Log.WriteErrorLog(msg); return msg.ToMessageForUser(); ; }

            msg = UserAccount.Login(userLogin.UserName, out userAccount);
            if (msg.Length > 0) return msg;
            if (userAccount == null) return "Bạn nhập sai tên tài khoản hoặc mật khẩu".ToMessageForUser(); ;

            byte[] PasswordHash = Common.GetInputPasswordHash(userLogin.Password, userAccount.PasswordSalt);
            if (!userAccount.PasswordHash.SequenceEqual(PasswordHash)) return "Bạn nhập sai tên tài khoản hoặc mật khẩu".ToMessageForUser(); ;

            if (!userAccount.IsActive) return ("Tài khoản của bạn đã bị khóa. Xin vui lòng liên hệ với CSKH").ToMessageForUser();

            msg = CacheUserToken.CreateToken(userAccount, userLogin.IsRememberPassword, out userToken);
            if (msg.Length > 0) return msg;

            return msg;
        }

        private string DoValidateLogin(UserLogin userLogin)
        {
            string msg = "";

            if (userLogin.UserName.Length == 0) msg = "Chưa nhập tên đăng nhập".ToMessageForUser(); ;
            if (msg.Length > 0) { return msg; }
            if (userLogin.Password.Length == 0) msg = "Chưa nhập password".ToMessageForUser(); ;
            if (msg.Length > 0) { return msg; }

            return msg;
        }
    }

}