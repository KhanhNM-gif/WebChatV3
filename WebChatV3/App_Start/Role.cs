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
    public List<string> ListRoleDescription
    {
        get
        {
            List<string> lt = new List<string>();
            var vGroup = ListRole.Where(v => v.IsRole).GroupBy(v => new { v.TabID, v.TabName });
            foreach (var item in vGroup)
            {
                lt.Add(item.Key.TabName + ": " + string.Join("; ", item.Select(v => v.RoleName)));
            }

            return lt;
        }
    }
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

    public static string GetByUserID(long UserID,long IdServer, out RoleGroup rg)
    {
        rg = null;

        Mod mod;
        string msg = Mod.GetOne(UserID,IdServer, out mod);
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
    public string TabName
    {
        get
        {
            return Tab.GetTabName(TabID);
        }
    }
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
    public const long QUYEN_SERVER_MOITHAM = 4;
    public const long QUYEN_SERVER_MOITHAMGIA = 2;



    public static List<Role> GetListRoleServer()
    {
        return new List<Role>
        {
             new Role { RoleName="Xem thành viên", RoleValue = ROLE_NVCTXL_IsVisitPage, TabID = Constants.TabID.NVCTXL,OrderID=1},
             new Role { RoleName="Rời Server", RoleValue = ROLE_NVCTXL_AddMission, TabID = Constants.TabID.NVCTXL,OrderID=1},
             new Role { RoleName="Xuất file", RoleValue = ROLE_NVCTXL_ExportFile, TabID = Constants.TabID.NVCTXL,OrderID=1},
        };
    }

    public static string CheckViewAllMission(int UserID, int TabID, out bool IsViewAll)
    {
        string msg = "";
        IsViewAll = false;
        if (TabID == Constants.TabID.NVG)
        {
            msg = Role.Check(UserID, Constants.TabID.NVG, Role.ROLE_NVG_VIEWALL, out IsViewAll);
            if (msg.Length > 0) return msg;
        }

        return msg;
    }
    public static string CheckUQXLAllUser(int UserID, out bool IsUQXLAllUser)
    {
        return Role.Check(UserID, Constants.TabID.UQXL, Role.ROLE_UQXL_QT_ALL, out IsUQXLAllUser);
    }
    public static string CheckVisitPage(int UserID, int TabID)
    {
        long RoleValueVisitPage;
        string msg = Role.GetRoleValueVisitPage(TabID, out RoleValueVisitPage);
        if (msg.Length > 0) return msg;

        return Check(UserID, TabID, RoleValueVisitPage);
    }
    public static string CheckVisitPage(RoleGroup rg, int TabID, out bool IsRole)
    {
        IsRole = false;

        long RoleValueVisitPage;
        string msg = Role.GetRoleValueVisitPage(TabID, out RoleValueVisitPage);
        if (msg.Length > 0) return msg;

        return Role.Check(rg, TabID, RoleValueVisitPage, out IsRole);
    }
    public static string Check(int UserID, int TabID)
    {
        bool IsRole;
        string msg = Check(UserID, TabID, -1, out IsRole);
        if (msg.Length > 0) return msg;

        if (!IsRole) return "Bạn không có quyền thực hiện chức năng này".ToMessageForUser();
        else return "";
    }
    public static string Check(int UserID, int TabID, long RoleValue)
    {
        bool IsRole;
        string msg = Check(UserID, TabID, RoleValue, out IsRole);
        if (msg.Length > 0) return msg;

        if (!IsRole) return "Bạn không có quyền thực hiện chức năng này".ToMessageForUser();
        else return "";
    }
    public static string Check(int UserID, int TabID, long RoleValue, out bool IsRole)
    {
        IsRole = false;

        RoleGroup rg;
        string msg = RoleGroup.GetByUserID(UserID, out rg);
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
    public static string GetRoleValueVisitPage(int tabID, out long RoleValue)
    {
        RoleValue = 0;
        switch (tabID)
        {
            case Constants.TabID.NVCTXL:
                RoleValue = Role.ROLE_NVCTXL_IsVisitPage;
                break;
            case Constants.TabID.NVG:
                RoleValue = Role.ROLE_NVG_IsVisitPage;
                break;
            case Constants.TabID.NVPHXL:
                RoleValue = Role.ROLE_NVPHXL_IsVisitPage;
                break;
            case Constants.TabID.NGUONNV:
                RoleValue = Role.ROLE_NGUONNV_IsVisitPage;
                break;
            case Constants.TabID.NNV:
                RoleValue = Role.ROLE_NNV_IsVisitPage;
                break;
            case Constants.TabID.Tag:
                RoleValue = Role.ROLE_Tag_IsVisitPage;
                break;
            case Constants.TabID.UQXL:
                RoleValue = Role.ROLE_UQXL_IsVisitPage;
                break;
            case Constants.TabID.NHOMDONVI:
                RoleValue = Role.ROLE_NHOMDONVI_IsVisitPage;
                break;
            case Constants.TabID.NVTD:
                RoleValue = Role.ROLE_NVTD_IsVisitPage;
                break;
            case Constants.TabID.NGUOIDUNG:
                RoleValue = Role.ROLE_QLND_IsVisitPage;
                break;
            case Constants.TabID.DONVI:
                RoleValue = Role.ROLE_DONVI_IsVisitPage;
                break;
            case Constants.TabID.CHUCVU:
                RoleValue = Role.ROLE_CHUCVU_IsVisitPage;
                break;
            case Constants.TabID.HETHONG:
                RoleValue = Role.ROLE_HETHONG_IsVisitPage;
                break;
            case Constants.TabID.NHOMQUYEN:
                RoleValue = Role.ROLE_NHOMQUYEN_IsVisitPage;
                break;
            case Constants.TabID.SUB_TAB_TVB:
                RoleValue = Role.ROLE_SUB_TAB_TVB_IsVisitPage;
                break;
            case Constants.TabID.SUB_TAB_TNK:
                RoleValue = Role.ROLE_SUB_TAB_TNK_IsVisitPage;
                break;
            case Constants.TabID.SUB_TAB_TBD:
                RoleValue = Role.ROLE_SUB_TAB_TBD_IsVisitPage;
                break;
            default:
                break;
        }
        return "";
    }
    public static string GetRoleGroupCol(int tabID, RoleGroup rg, out long RoleGroupCol)
    {
        RoleGroupCol = 0;
        switch (tabID)
        {
            case Constants.TabID.NVCTXL:
                RoleGroupCol = rg.NVCTXL;
                break;
            case Constants.TabID.NVG:
                RoleGroupCol = rg.NVG;
                break;
            case Constants.TabID.NVPHXL:
                RoleGroupCol = rg.NVPHXL;
                break;
            case Constants.TabID.NGUONNV:
                RoleGroupCol = rg.NGUONNV;
                break;
            case Constants.TabID.NNV:
                RoleGroupCol = rg.NNV;
                break;
            case Constants.TabID.Tag:
                RoleGroupCol = rg.Tag;
                break;
            case Constants.TabID.UQXL:
                RoleGroupCol = rg.UQXL;
                break;
            case Constants.TabID.NHOMDONVI:
                RoleGroupCol = rg.NHOMDONVI;
                break;
            case Constants.TabID.NVTD:
                RoleGroupCol = rg.NVTD;
                break;
            case Constants.TabID.NGUOIDUNG:
                RoleGroupCol = rg.NGUOIDUNG;
                break;
            case Constants.TabID.DONVI:
                RoleGroupCol = rg.DONVI;
                break;
            case Constants.TabID.CHUCVU:
                RoleGroupCol = rg.CHUCVU;
                break;
            case Constants.TabID.HETHONG:
                RoleGroupCol = rg.HETHONG;
                break;
            case Constants.TabID.NHOMQUYEN:
                RoleGroupCol = rg.NHOMQUYEN;
                break;
            case Constants.TabID.SUB_TAB_TVB:
                RoleGroupCol = rg.SUB_TAB_TVB;
                break;
            case Constants.TabID.SUB_TAB_TNK:
                RoleGroupCol = rg.SUB_TAB_TNK;
                break;
            case Constants.TabID.SUB_TAB_TBD:
                RoleGroupCol = rg.SUB_TAB_TBD;
                break;
            default:
                break;
        }
        return "";
    }
}
