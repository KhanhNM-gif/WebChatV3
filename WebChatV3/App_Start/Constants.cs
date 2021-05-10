using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Constants
/// </summary>
public class Constants
{
 

    public class TabID
    {
        public const int RoleServer = 1,
                         RoleMember = 2,
                         RoleChannel = 3;
    }
    public class SexType
    {
        public const int MALE = 1,
                        FEMALE = 2;
    }
    public class CategoryUser
    {
        public const int UserDelivery = 1,
                         UserPerform = 2,
                         UserCooperation = 3;
    }
    public class ActivityGUID
    {
        public static Guid CreateMission_API = Guid.Parse("4379ac92-ca0d-4866-8c3b-12282659b6c3");
    }

    public class SourceTypeID
    {
        public const int VB = 1, KPI_BI = 2, BaoCao_eForm = 3, DiemTin = 4, Khac = 0;
    }
    public class SourceCategoryID
    {
        public const int VB = 1, Khac = 2, VB_EGOV = 3, DB_IOC = 4;
    }

    public class SourceType
    {
        public const int VanBanDen = 0, VanBanDi = 1;
    }
}
