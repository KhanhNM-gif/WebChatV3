using BSS;
using BSS.DataValidator;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

public class FileAttach
{
    public Guid FileAttachGUID { get; set; }
    public long UserIDCreate { get; set; }
    public Guid ObjectGUID { get; set; }
    public string FileName { get; set; }
    public string FileExttension
    {
        get
        {
            return Path.GetExtension(FileName);
        }       
    }   
    public int FileSize { get; set; }
    public bool IsDelete { get; set; }
    public string FileUrl
    {
        get
        {
            return "/File/FileUpload/" + FileAttachGUID + "/" + FileName;
        }       
    }   
    public DateTime CreateDate { get; set; }

    public static string GetByFileAttachGUID(Guid FileAttachGUID, out FileAttach fa)
    {
        return DBM.GetOne("usp_FileAttach_SelectByFileAttachGUID", new { FileAttachGUID }, out fa);
    }
    public static string GetByObjectGUID(Guid ObjectGUID, out List<FileAttach> lt)
    {
        return DBM.GetList("usp_FileAttach_GetByObjectGUID", new { ObjectGUID }, out lt);
    }
    public static string GetByUserIDCreate(long UserIDCreate, out List<FileAttach> lt)
    {
        return DBM.GetList("usp_FileAttach_GetByUserIDCreate", new {UserIDCreate }, out lt);
    }

    public string Insert(DBM dbm, out FileAttach fa)
    {
        fa = null;

        string msg = dbm.SetStoreNameAndParams("usp_FileAttach_Insert",
           new
           {
               FileAttachGUID,
               UserIDCreate,
               FileUrl,
               FileExttension,
               FileName,
               FileSize,
           });
        if (msg.Length > 0) return msg;

        return dbm.GetOne(out fa);
    }
    public static string UpdateIsDelete(DBM dbm, Guid FileAttachGUID, bool IsDelete)
    {
        string msg = dbm.SetStoreNameAndParams("usp_FileAttach_UpdateIsDelete",
           new
           {
               FileAttachGUID,
               IsDelete
           });
        if (msg.Length > 0) return msg;

        return dbm.ExecStore();
    }

    //public static string Update(DBM dbm, Guid FileAttachGUID, Guid ObjectGUID, bool IsDelete)
    //{
    //    string msg = dbm.SetStoreNameAndParams("usp_FileAttach_Update",
    //       new
    //       {
    //           FileAttachGUID,
    //           ObjectGUID,
    //           IsDelete
    //       });
    //    if (msg.Length > 0) return msg;

    //    return dbm.ExecStore();
    //}    
}
public class FileAttachInfo
{
    public string FileName { get; set; }
    public byte[] FileContent { get; set; }    
    public int FileSize { get; set; }
}