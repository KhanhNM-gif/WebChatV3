using BSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace WebUpLoadFile.Controllers
{
    public class ApiUploadFileController : Authentication
    {
        [HttpPost]
        public Result UploadFile()
        {
            if (!ResultCheckToken.isOk) return ResultCheckToken;

            List<FileAttach> ltFileAttach;
            string msg = DoUploadFile(out ltFileAttach);
            if (msg.Length > 0) return Log.ProcessError(msg).ToResultError();

            return ltFileAttach.ToResultOk();
        }
        private string DoUploadFile(out List<FileAttach> ltFileAttach)
        {
            ltFileAttach = null;
            string msg = "";

            try
            {
                Guid ObjectGUID = Guid.Empty;

                msg = FileAttachUpload.Upload(UserToken.UserID, ObjectGUID, out ltFileAttach);
                return msg;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        [HttpGet]
        public Result GetList()
        {
            long IdUser = 1;
            List<FileAttach> ltFileAttach;
            string msg = FileAttach.GetByUserIDCreate(IdUser, out ltFileAttach);
            if (msg.Length > 0) return Log.ProcessError(msg).ToResultError();

            return ltFileAttach.ToResultOk();
        }
        [HttpPost]
        public Result Delete([FromBody] FileAttach FileAttach)
        {
            bool isDelete = true;
            string msg = FileAttach.UpdateIsDelete(new DBM(), FileAttach.FileAttachGUID, isDelete);
            if (msg.Length > 0) return Log.ProcessError(msg).ToResultError();

            return msg.ToResultOk();
        }

    }
}
