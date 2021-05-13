using BSS;
using BSS.DataValidator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebChatV3.Controllers
{
    public class ApiGroupRoleController : Authentication
    {
        public Result GetOne(int RoleGroupID, Guid ServerGuid)
        {
            //if (!ResultCheckToken.isOk) return ResultCheckToken;

            string msg = DoGetOne(RoleGroupID, ServerGuid, out List<Role> roles);
            if (msg.Length > 0) return Log.ProcessError(msg).ToResultError();

            return roles.ToResultOk();
        }

        private string DoGetOne(int RoleGroupID, Guid ServerGuid,out List<Role> roles)
        {
            roles = null;
            string msg = DataValidator.Validate(new { RoleGroupID }).ToErrorMessage();
            if (msg.Length > 0) return msg;

            msg = Member.GetOneByIDUserAndGuidServer(3, ServerGuid, out Member outMember);
            if (msg.Length > 0) return msg;
            if (outMember == null) return "Bạn không ở trong Server Chat. Vui lòng vào nhóm trước khi thực hiện".ToMessageForUser();

            roles = Role.GetListRole().Where(x => x.TabID == RoleGroupID && Role.Check(3, outMember.IdServer, RoleGroupID, x.RoleValue) == "").ToList();

            return msg;
        }
    }
}
