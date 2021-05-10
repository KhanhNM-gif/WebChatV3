using BSS.DataValidator;
using System;
using System.Globalization;

/// <summary>
/// Summary description for IVANValidator
/// </summary>
static public class WebChatValidate
{
    static WebChatValidate()
    {
        DataValidator.AddRules("Username", new LengthInRangeValidationRule(3, 50));
        DataValidator.AddRules("Password", new LengthInRangeValidationRule(1, 30));

        DataValidator.AddRules(new string[] { "FullName_BMM" }, new NotEmptyValidationRule(),
                                                                new LengthInRangeValidationRule(3, 128));
        DataValidator.AddRules("UrlAvatar", new EmptyOrLengthInRangeValidationRule(0, 100));
        DataValidator.AddRules("MobileUser", new EmptyOrIsAMobileNumberValidationRule());
        DataValidator.AddRules("BirthDate", new EmptyOrLengthInRangeValidationRule(5, 50));
        DataValidator.AddRules("UserDeptName", new EmptyOrLengthInRangeValidationRule(0, 200));
        DataValidator.AddRules("SexName", new EmptyOrLengthInRangeValidationRule(0, 5));
        DataValidator.AddRules("Sex", new EmptyOrLengthInRangeValidationRule(0, 2));
        DataValidator.AddRules("StatusName", new EmptyOrLengthInRangeValidationRule(0, 20));

        DataValidator.AddRules("DeptID", new InRangeValidationRule(0, 10000));
        DataValidator.AddRules("DeptIDParent", new InRangeValidationRule(0, 10000));
        DataValidator.AddRules("Name", new EmptyOrLengthInRangeValidationRule(0, 255));
        DataValidator.AddRules("DeptShortName", new EmptyOrLengthInRangeValidationRule(0, 255));
        DataValidator.AddRules(new string[] { "DeptFullName", "DeptFullNameParent", "DeptGroupName" }, new EmptyOrLengthInRangeValidationRule(0, 500));

        DataValidator.AddRules("Id", new InRangeValidationRule(0, 10000));


        DataValidator.AddRules("PositionID", new InRangeValidationRule(0, 10000));
        DataValidator.AddRules("PositionIDParent", new InRangeValidationRule(0, 10000));
        DataValidator.AddRules("PositionName", new EmptyOrLengthInRangeValidationRule(0, 255));


        DataValidator.AddRules(new string[] { "MissionID", "CheckNumber", "ID", "ProfileID", "TotalMission", "CategorySort" }, new InRangeValidationRule(0, 10000000));
        DataValidator.AddRules(new string[] { "CategorySearch", "StatusID", "StatusEndDateID", "TabID", "Order" }, new InRangeValidationRule(0, 50));
        DataValidator.AddRules(new string[] { "TextEasySearch", "TextSearch" }, new EmptyOrLengthInRangeValidationRule(0, 1000));
        DataValidator.AddRules(new string[] { "BeginDateCategory", "EndDateCategory", "LastUpdateCategory", "FinishDateCategory" }, new InRangeValidationRule(0, 200));

        DataValidator.AddRules(new string[] { "IsDelete", "IsNotifyEmail", "IsNotifySMS", "IsOpen", "IsFocus", "IsViewAll" }, new BoolValidationRule());

        DataValidator.AddRules(new string[] { "MissionGroupID" }, new InRangeValidationRule(-1, 10000000));

        DataValidator.AddRules(new string[] { "UserIDCreate", "DeptIDDelivery", "UserIDDelivery", "DeptIDPerform", "UserIDPerform", "DeptIDCooperation", "UserIDCooperation", "UserIDPerform", "TagID", "MissionGroupIDParent", "DelegacyID", "UserID", "SourceTypeID" }, new InRangeValidationRule(0, 1000000));
        DataValidator.AddRules(new string[] { "MissionContent", "Note", "MissionGroupName", "TagName", "Reason", "ReasonNotApproval", "Result", "Proof", "LeadingIdeas", "ReportContent", "LastContentProgress" }, new EmptyOrLengthInRangeValidationRule(0, 4000));
        DataValidator.AddRules(new string[] { "CommentProcess", "TagIDs" }, new EmptyOrLengthInRangeValidationRule(0, 200));
        DataValidator.AddRules(new string[] { "CreateDate", "LastUpdate", "LastUpdateFrom", "BeginDateFrom", "BeginDateTo", "EndDateFrom", "EndDateTo", "LastUpdateTo", "FinishDateFrom", "FinishDateTo", "BeginDate", "EndDate", "NewBeginDate", "NewEndDate", "ExtendDate", "FinishDate" }, new NullOrDatetimeValidationRule());
        DataValidator.AddRules(new string[] { "ObjectGuid", "ObjectGuidDocument" }, new CheckGUIDValidationRule());

        DataValidator.AddRules(new string[] { "ListDeptIDDelivery", "ListDeptIDPerform", "ListDeptIDCooperation" }, new EmptyOrLengthInRangeValidationRule(0, 4000));

        DataValidator.AddRules(new string[] { "UserIDDelegacy", "UserIDDelegacyed" }, new InRangeValidationRule(0, 10000));

        DataValidator.AddRules(new string[] { "SourceID" }, new InRangeValidationRule(0, 100000));
        DataValidator.AddRules(new string[] { "SourceName" }, new EmptyOrLengthInRangeValidationRule(0, 500));

        DataValidator.AddRules(new string[] { "SourceObjectID" }, new InRangeValidationRule(0, 50000));
        DataValidator.AddRules(new string[] { "DataJson", "SourceJsonData" }, new EmptyOrLengthInRangeValidationRule(0, 10000));

        DataValidator.AddRules(new string[] { "SourceCategoryID" }, new InRangeValidationRule(0, 50));
        DataValidator.AddRules(new string[] { "SourceSystemObjectID" }, new EmptyOrLengthInRangeValidationRule(0, 50));
        DataValidator.AddRules(new string[] { "SourceCategoryName" }, new EmptyOrLengthInRangeValidationRule(0, 500));


        DataValidator.AddRules(new string[] { "RoleGroupID", "NVCTXL", "NVG", "NVPHXL", "NNV", "NPHONGBAN", "Tag", "UQXL", "NGUONNV", "NHOMDONVI", "UserIDCreate", "NVTD", "NGUOIDUNG", "DONVI", "CHUCVU", "HETHONG", "NHOMQUYEN", "SUB_TAB_TVB", "SUB_TAB_TNK", "SUB_TAB_TBD" }, new InRangeValidationRule(0, 10000));
        DataValidator.AddRules(new string[] { "RoleGroupName" }, new EmptyOrLengthInRangeValidationRule(0, 500));

        DataValidator.AddRules(new string[] { "ListRoleDescription", "LtSourceObject" }, new EmptyOrLengthInRangeValidationRule(0, 500));

        DataValidator.AddRules("FileName", new LengthInRangeValidationRule(1, 100));
        DataValidator.AddRules("FileExttension", new LengthInRangeValidationRule(1, 20));
        DataValidator.AddRules("FileSize", new InRangeValidationRule(1, 200000000));

        DataValidator.AddRules("PageSize", new InRangeValidationRule(0, 1000));
        DataValidator.AddRules("CurrentPage", new InRangeValidationRule(0, 10000));
    }
    static public void Init() { }

    public class NullOrInRangeValidationRule : ValidationRule
    {
        public int min { get; set; }
        public int max { get; set; }

        public NullOrInRangeValidationRule(int min, int max, int ruleType = RuleTypeCB1)
            : base(ruleType, "Giá trị không nằm trong khoảng: " + min + " - " + max + ".")
        {
            this.min = min;
            this.max = max;
        }
        override public BSS.Result Validate(object o)
        {
            if (o == null) return BSS.Result.ResultOk;

            if (string.IsNullOrEmpty(o.ToString())) return BSS.Result.ResultOk;

            int value = int.Parse(o.ToString());
            if (value < min || value > max)
                return BSS.Result.GetResultError(this);

            return BSS.Result.ResultOk;
        }
    }
    public class EmptyOrLengthInRangeValidationRule : ValidationRule
    {
        public long min { get; set; }
        public long max { get; set; }

        public EmptyOrLengthInRangeValidationRule(int min, int max, int ruleType = RuleTypeCB1)
            : base(ruleType, "Chiều dài xâu không nằm trong khoảng: " + min + " - " + max + ".")
        {
            this.min = min;
            this.max = max;
        }
        override public BSS.Result Validate(object o)
        {
            if (o == null) return BSS.Result.ResultOk;

            if (string.IsNullOrEmpty(o.ToString())) return BSS.Result.ResultOk;

            int len = o.ToString().Length;
            if (len < min || len > max)
                return BSS.Result.GetResultError(this);

            return BSS.Result.ResultOk;
        }
    }
    public class AllCharacterIsNumberValidationRule : ValidationRule
    {
        string check = "0123456789";
        public AllCharacterIsNumberValidationRule(int ruleType = RuleTypeCB1) : base(ruleType, "Chứa ký tự không phải là số") { }
        override public BSS.Result Validate(object o)
        {
            if (o == null) return BSS.Result.GetResultError("object is null");

            if (!DataValidator.AllChar(o.ToString(), check)) return BSS.Result.GetResultError(this);

            return BSS.Result.ResultOk;
        }
    }
    public class EmptyOrLengthValidationRule : ValidationRule
    {
        public long length { get; set; }
        public long max { get; set; }

        public EmptyOrLengthValidationRule(int length, int ruleType = RuleTypeCB1)
            : base(ruleType, "Chiều dài xâu không bằng " + length)
        {
            this.length = length;
        }
        override public BSS.Result Validate(object o)
        {
            if (o == null) return BSS.Result.ResultOk;

            if (string.IsNullOrEmpty(o.ToString())) return BSS.Result.ResultOk;

            int len = o.ToString().Length;
            if (len != length)
                return BSS.Result.GetResultError(this);

            return BSS.Result.ResultOk;
        }
    }
    public class BoolValidationRule : ValidationRule
    {
        public BoolValidationRule(int ruleType = RuleTypeCB1) : base(ruleType, "Không phải là bool") { }
        override public BSS.Result Validate(object o)
        {
            if (o == null) return BSS.Result.GetResultError("object is null");

            bool b;
            if (!bool.TryParse(o.ToString(), out b)) return BSS.Result.GetResultError(string.Format("\"{0}\" không phải là bool", o));

            return BSS.Result.ResultOk;
        }
    }
    public class MonthYearValidationRule : ValidationRule
    {
        public DateTime DateTimeMin { set; get; }
        public DateTime DateTimeMax { set; get; }
        public MonthYearValidationRule(DateTime DateTimeMin, DateTime DateTimeMax, int ruleType = RuleTypeCB1) : base(ruleType, "Không phải là tháng năm (Ví dụ: 01/2019)")
        {
            this.DateTimeMin = DateTimeMin;
            this.DateTimeMax = DateTimeMax;
        }
        override public BSS.Result Validate(object o)
        {
            if (o == null) return BSS.Result.GetResultError("object is null");
            if (o.ToString() == "") return BSS.Result.ResultOk;

            DateTime dt;
            if (!DateTime.TryParseExact("01/" + o.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt)) return BSS.Result.GetResultError("Không đúng định dạng tháng/năm (Ví dụ: 01/2019)");

            if (dt < DateTimeMin || dt > DateTimeMax) return BSS.Result.GetResultError(string.Format("\"{0}\" không nằm trong dải từ {1} đến {2}", o, DateTimeMin.ToString("MM/yyyy"), DateTimeMax.ToString("MM/yyyy")));

            return BSS.Result.ResultOk;
        }
    }
    public class EmptyOrIsAMobileNumberValidationRule : ValidationRule
    {
        string check = "0123456789";

        public EmptyOrIsAMobileNumberValidationRule(int ruleType = RuleTypeCB1) : base(ruleType, "Không phải số điện thoại di động") { }
        override public BSS.Result Validate(object o)
        {
            if (o == null) return BSS.Result.ResultOk;

            string s = o.ToString();

            if (string.IsNullOrEmpty(s)) return BSS.Result.ResultOk;

            if (DataValidator.AllChar(s, check) &&
                (((s.StartsWith("01") || s.StartsWith("028") || s.StartsWith("023") || s.StartsWith("02")) && (s.Length == 11)) || ((s.StartsWith("03") || s.StartsWith("05") || s.StartsWith("07") || s.StartsWith("08") || s.StartsWith("09")) && (s.Length == 10)))) return BSS.Result.ResultOk;

            return BSS.Result.GetResultError(this);
        }
    }
    public class CheckDatetimeValidationRule : ValidationRule
    {
        public CheckDatetimeValidationRule(int ruleType = RuleTypeCB1) : base(ruleType, "Không đúng định dạng Datetime") { }
        override public BSS.Result Validate(object o)
        {
            if (o == null) return BSS.Result.GetResultError("object is null");

            string s = o.ToString();
            DateTime dt = DateTime.Now;
            if (DateTime.TryParse(s, out dt))
                if (dt >= new DateTime(1900, 01, 01) && dt < new DateTime(2086, 12, 31)) return BSS.Result.ResultOk;
                else return BSS.Result.GetResultError("Không nằm trong khoảng từ 01/01/1900 đến 31/12/2086");

            return BSS.Result.GetResultError(this);
        }
    }
    public class NullOrDatetimeValidationRule : ValidationRule
    {
        public NullOrDatetimeValidationRule(int ruleType = RuleTypeCB1) : base(ruleType, "Không đúng định dạng Datetime") { }
        override public BSS.Result Validate(object o)
        {
            if (o == null) return BSS.Result.ResultOk;

            string s = o.ToString();
            DateTime dt = DateTime.Now;
            if (DateTime.TryParse(s, out dt))
                if (dt == new DateTime(0001, 01, 01) || (dt >= new DateTime(1900, 01, 01) && dt < new DateTime(2086, 12, 31))) return BSS.Result.ResultOk;
                else return BSS.Result.GetResultError("Không nằm trong khoảng từ 01/01/1900 đến 31/12/2086");

            return BSS.Result.GetResultError(this);
        }
    }
    public class CheckGUIDValidationRule_IVAN : ValidationRule
    {
        public long min { get; set; }
        public long max { get; set; }

        public CheckGUIDValidationRule_IVAN(int ruleType = RuleTypeCB1) : base(ruleType, "Không phải GUID") { }
        override public BSS.Result Validate(object o)
        {
            if (o == null) return BSS.Result.GetResultError("object is null");
            if (string.IsNullOrWhiteSpace(o.ToString())) return BSS.Result.ResultOk;

            try
            {
                Guid.Parse(o.ToString());
            }
            catch (Exception)
            {
                return BSS.Result.GetResultError(this);
            }

            return BSS.Result.ResultOk;
        }
    }
}