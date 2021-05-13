using BSS;
using BSS.DataValidator;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class RoleGroup
{
    public int RoleGroupID { get; set; }
    public Guid ObjectGuid { get; set; }
    public string RoleGroupName { get; set; }
    public long QuyenServer { get; set; }
    public long QuyenThanhVien { get; set; }
    public long QuyenChannel { get; set; }
    public long QuyenChiaSe { get; set; }
    [JsonIgnore]
    public int UserIDCreate { get; set; }
    public List<Role> ListRole = new List<Role>();
    public bool IsDelete { get; set; }

    public const int ADMIN = 1, USER = 2, ADMIN_DEPT = 3;

    static public string GetOne(int RoleGroupID, out RoleGroup RoleGroup)
    {
        RoleGroup = null;

        string msg = DBM.GetOne("usp_RoleGroup_GetOne", new { RoleGroupID }, out RoleGroup);
        if (msg.Length > 0) return msg;

        if (RoleGroup == null) RoleGroup = new RoleGroup();
        return "";
    }
    public string InsertUpdate(out RoleGroup r)
    {
        return DBM.GetOne("usp_RoleGroup_InsertOrUpdate", new
        {
            RoleGroupID,
            RoleGroupName,
            QuyenServer,
            QuyenThanhVien,
            QuyenChannel

        }, out r);
    }

    public static string GetAll(out List<RoleGroup> lt)
    {
        return DBM.GetList("usp_RoleGroup_SelectAll", new { }, out lt);
    }
    public static string GetList(string RoleGroupName, int StatusID, out List<RoleGroup> lt)
    {
        return DBM.GetList("usp_RoleGroup_SelectSearch", new { RoleGroupName, StatusID }, out lt);
    }
    public static string GetByRoleGroupName(string RoleGroupName, out List<RoleGroup> ltTag)
    {
        return DBM.GetList("usp_RoleGroup_SelectByRoleGroupName", new { RoleGroupName }, out ltTag);
    }

    public static string UpdateIsDelete(int RoleGroupID, bool IsDelete)
    {
        return DBM.ExecStore("usp_RoleGroup_UpdateIsDelete", new { RoleGroupID, IsDelete });
    }

    public static string ValidateRoleGroup(RoleGroup RoleGroup)
    {
        string msg = "";

        if (RoleGroup.RoleGroupName.Trim().Length == 0) return "Tên Nhóm quyền không được để trống";

        msg = DataValidator.Validate(RoleGroup).ToErrorMessage();
        if (msg.Length > 0) return msg.ToMessageForUser();

        List<RoleGroup> ltRoleGroup;
        msg = RoleGroup.GetByRoleGroupName(RoleGroup.RoleGroupName.Trim(), out ltRoleGroup);
        if (msg.Length > 0) return msg;
        if (ltRoleGroup.Where(v => !v.IsDelete && v.RoleGroupID != RoleGroup.RoleGroupID).Count() > 0) return ("Đã tồn tại Tên Nhóm quyền '" + RoleGroup.RoleGroupName.Trim() + "'").ToMessageForUser();

        return msg;
    }

    public static string GetByUserID(long UserID, long IdServer, out RoleGroup rg)
    {
        rg = null;

        Mod mod;
        string msg = Mod.GetOne(UserID, IdServer, out mod);
        if (msg.Length > 0) return msg;

        int RoleGroupID;
        if (mod == null) RoleGroupID = RoleGroup.USER;
        else RoleGroupID = mod.IdRoleGroup;

        msg = RoleGroup.GetOne(RoleGroupID, out rg);
        if (msg.Length > 0) return msg;

        if (rg == null || rg.IsDelete)
        {
            RoleGroupID = RoleGroup.USER;
            msg = RoleGroup.GetOne(RoleGroupID, out rg);
            if (msg.Length > 0) return msg;
        }

        return msg;
    }
}
public class Role
{
    public long RoleValue { get; set; }
    public string RoleName { get; set; }
    public int TabID { get; set; }
    public int OrderID { get; set; }
    public bool IsRole { get; set; }

    public Role()
    {
        RoleValue = 0;
        RoleName = "";
        TabID = 0;
        IsRole = false;
        OrderID = 0;
    }

    public const long QUYEN_SERVER_ROINHOM = 1;
    public const long QUYEN_SERVER_MOITHAMGIA = 2;
    public const long QUYEN_SERVER_XEMTHANHVIEN = 4;
    public const long QUYEN_SERVER_THEMKENH = 8;
    public const long QUYEN_SERVER_XOAKENH = 16;



    public static List<Role> GetListRole()
    {
        return new List<Role>
        {
             new Role { RoleName="Rời Server", RoleValue = QUYEN_SERVER_ROINHOM, TabID = Constants.TabID.RoleServer,OrderID=1},
             new Role { RoleName="Rời Server", RoleValue = QUYEN_SERVER_MOITHAMGIA, TabID = Constants.TabID.RoleServer,OrderID=1},
             new Role { RoleName="Xem Thành Viên", RoleValue = QUYEN_SERVER_XEMTHANHVIEN, TabID = Constants.TabID.RoleServer,OrderID=1},
             new Role { RoleName="Thêm Channel", RoleValue = QUYEN_SERVER_THEMKENH, TabID = Constants.TabID.RoleServer,OrderID=1},
             new Role { RoleName="Xóa Kênh", RoleValue = QUYEN_SERVER_XOAKENH, TabID = Constants.TabID.RoleServer,OrderID=1},
        };
    }

    public static string Check(long UserID,long ServerID, int TabID)
    {
        bool IsRole;
        string msg = Check(UserID,ServerID, TabID, -1, out IsRole);
        if (msg.Length > 0) return msg;

        if (!IsRole) return "Bạn không có quyền thực hiện chức năng này".ToMessageForUser();
        else return "";
    }
    public static string Check(long UserID, long ServerID, int TabID, long RoleValue)
    {
        bool IsRole;
        string msg = Check(UserID, ServerID, TabID, RoleValue, out IsRole);
        if (msg.Length > 0) return msg;

        if (!IsRole) return "Bạn không có quyền thực hiện chức năng này".ToMessageForUser();
        else return "";
    }
    public static string Check(long UserID, long ServerID, int TabID, long RoleValue, out bool IsRole)
    {
        IsRole = false;

        RoleGroup rg;
        string msg = RoleGroup.GetByUserID(UserID, ServerID, out rg);
        if (msg.Length > 0) return msg;

        return Check(rg, TabID, RoleValue, out IsRole);
    }
    public static string Check(RoleGroup rg, int TabID, long RoleValue, out bool IsRole)
    {
        IsRole = false;

        //if (TabID == Constants.TabID.NHOMQUYEN || TabID == Constants.TabID.NGUOIDUNG || TabID == Constants.TabID.DONVI || TabID == Constants.TabID.CHUCVU || TabID == Constants.TabID.HETHONG)
        //{
        //    if (rg.RoleGroupID == RoleGroup.ADMIN) IsRole = true;
        //    else IsRole = false;
        //}
        //else
        //{
        string msg = GetRoleGroupCol(TabID, rg, out long RoleGroupCol);
        if (msg.Length > 0) return msg;

        if (RoleGroupCol > 0) IsRole = ((RoleGroupCol & RoleValue) == RoleValue);
        else IsRole = false;
        // }

        return "";
    }
    public static string GetRoleGroupCol(int tabID, RoleGroup rg, out long RoleGroupCol)
    {
        RoleGroupCol = 0;
        switch (tabID)
        {
            case Constants.TabID.RoleServer:
                RoleGroupCol = rg.QuyenServer;
                break;
            case Constants.TabID.RoleChannel:
                RoleGroupCol = rg.QuyenChannel;
                break;
            case Constants.TabID.RoleMember:
                RoleGroupCol = rg.QuyenThanhVien;
                break;
            default:
                break;
        }
        return "";
    }
}
