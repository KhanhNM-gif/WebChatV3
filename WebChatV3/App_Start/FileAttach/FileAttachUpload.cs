using BSS;
using BSS.DataValidator;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

public class FileAttachUpload
{
    const string EXTENSION_ALLOW = "aif,cda,mid,mp3,aac,mpa,ogg,wav,avi,flv,mp4,mov,mpg,mpeg,swf,wma,m4v,vob,wmv,wpl,7z,rar,gz,z,zip,csv,tar,xml,log,ai,bmp,gif,ico,jpeg,jpg,png,psd,svg,tif,tiff,pps,ppt,pptx,ods,xls,xlsm,xlsx,xlsb,doc,docx,odt,pdf,rtf,tex,txt,wpd";

    public const string AddMission = "AddMission",
                        ReportMission = "ReportMission",
                        ExtendMission = "ExtendMission";// FunctionID: AddMission - Thêm nhiệm vụ; ReportMission - Báo cáo nhiệm vụ; ExtendMission - Gia hạn nhiệm vụ

    public static string Upload(long UserID, string FunctionID, out List<FileAttach> ltFileAttach)
    {
        return Upload(UserID, Guid.Empty, out ltFileAttach);
    }
    public static string Upload(long UserID, Guid ObjectGUID, out List<FileAttach> ltFileAttach)
    {
        ltFileAttach = new List<FileAttach>();

        string msg = GetListFileAttachInfo_FromRequest(out List<FileAttachInfo> ltFileAttachInfo);
        if (msg.Length > 0) return msg;

        return Upload(UserID, ltFileAttachInfo, ObjectGUID, false, out ltFileAttach);
    }
    private static string GetListFileAttachInfo_FromRequest(out List<FileAttachInfo> ltFileAttachInfo)
    {
        ltFileAttachInfo = new List<FileAttachInfo>();

        try
        {
            var httpContext = HttpContext.Current;
            if (httpContext == null) return "httpContext == null";
            if (httpContext.Request == null) return "httpContext.Request == null";
            if (httpContext.Request.Files.Count == 0) return "Không có File đính kèm nào".ToMessageForUser();

            for (int i = 0; i < httpContext.Request.Files.Count; i++)
            {
                HttpPostedFile httpPostedFile = httpContext.Request.Files[i];

                Stream fs = httpPostedFile.InputStream;
                BinaryReader br = new BinaryReader(fs);
                byte[] fileContent = br.ReadBytes((Int32)fs.Length);

                FileAttachInfo fa = new FileAttachInfo
                {
                    FileName = httpPostedFile.FileName,
                    FileContent = fileContent,
                    FileSize = httpPostedFile.ContentLength
                };
                ltFileAttachInfo.Add(fa);
            }
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }

        return "";
    }

    public static string Upload(long UserID, string FunctionID, List<FileAttachInfo> ltFileAttachInfo, bool IsCreateFromAPI, out List<FileAttach> ltFileAttach)
    {
        return Upload(UserID, ltFileAttachInfo, Guid.Empty, IsCreateFromAPI, out ltFileAttach);
    }
    public static string Upload(long UserID, List<FileAttachInfo> ltFileAttachInfo, Guid ObjectGUID, bool IsCreateFromAPI, out List<FileAttach> ltFileAttach)
    {
        ltFileAttach = null;
        string msg = "";

        msg = Upload_SaveAndGetList(UserID, ltFileAttachInfo, ObjectGUID, out ltFileAttach);
        if (msg.Length > 0) return msg;

        msg = Upload_Validate(UserID, ObjectGUID, ltFileAttach);
        if (msg.Length > 0) return msg.ToMessageForUser();

        DBM dbm = new DBM();
        dbm.BeginTransac();

        msg = Upload_ObjectToDB(dbm, ltFileAttach);
        if (msg.Length > 0) { dbm.RollBackTransac(); return msg; }

        dbm.CommitTransac();

        return msg;
    }
    private static string Upload_Validate(long UserIDCreate, Guid ObjectGUID, List<FileAttach> ltFileAttach)
    {
        string msg = "";

        string[] arrExtemsionAllow = EXTENSION_ALLOW.Split(',');
        foreach (FileAttach fa in ltFileAttach)
        {
            msg = DataValidator.Validate(new { fa.FileName, fa.FileSize }).ToErrorMessage();
            if (msg.Length > 0) return ("File đính kem " + fa.FileName + " không hợp lệ: " + msg).ToMessageForUser();

            if (arrExtemsionAllow.Count(v => "." + v == fa.FileExttension.ToLower()) == 0) return ("Hệ thống không cho phép upload file đính kèm có đuôi " + fa.FileExttension).ToMessageForUser();
        }

        return msg;
    }
    private static string Upload_SaveAndGetList(long UserIDCreate, List<FileAttachInfo> ltFileAttachInfo, Guid ObjectGUID, out List<FileAttach> ltFileAttach)
    {
        ltFileAttach = new List<FileAttach>();

        try
        {
            foreach (var item in ltFileAttachInfo)
            {
                Guid FileAttachGUID = Guid.NewGuid();
                string msg = BSS.Common.GetSetting("FolderFileUpload", out string FileUpload);
                if (msg.Length > 0) return msg;

                string folderFileUpload = HttpContext.Current.Server.MapPath(FileUpload);

                //if (!Directory.Exists(folderFileUpload)) folderFileUpload = HttpContext.Current.Server.MapPath(folderFileUpload);

                folderFileUpload = folderFileUpload + "/" + FileAttachGUID;
                if (!Directory.Exists(folderFileUpload)) Directory.CreateDirectory(folderFileUpload);

                File.WriteAllBytes(folderFileUpload + "/" + item.FileName, item.FileContent);

                FileAttach fa = new FileAttach
                {
                    FileAttachGUID = FileAttachGUID,
                    UserIDCreate = UserIDCreate,
                    ObjectGUID = ObjectGUID,
                    FileName = item.FileName,
                    FileSize = item.FileSize,
                };
                ltFileAttach.Add(fa);
            }
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }

        return "";
    }
    private static string Upload_ObjectToDB(DBM dbm, List<FileAttach> ltFileAttach)
    {
        string msg = "";

        foreach (FileAttach fa in ltFileAttach)
        {
            FileAttach faNew;
            msg = fa.Insert(dbm, out faNew);
            if (msg.Length > 0) return msg;
            fa.FileAttachGUID = faNew.FileAttachGUID;
        }

        return msg;
    }


}