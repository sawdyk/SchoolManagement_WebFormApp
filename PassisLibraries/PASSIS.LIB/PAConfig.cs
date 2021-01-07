using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PASSIS.LIB.Utility;
using System.Web;

namespace PASSIS.LIB
{
    public class PAConfig : BasePage
    {
        public PAConfig()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public enum AuditAction
        {
            Login = 1,
            Add = 2,
            Edit = 3,
            Delete = 4,
        }

        public static void LogAudit(PAConfig.AuditAction action, string webpage, string remark, long userId)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();

            PASSIS.LIB.User currentuser = context.Users.FirstOrDefault(x => x.Id == userId);

            PAAuditTrial paaudittrial = new PAAuditTrial();

            string ipAddress = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDER_FOR"] != null)
            {
                ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDER_FOR"].ToString();
            }
            else
            {
                ipAddress = HttpContext.Current.Request.UserHostAddress;
            }

            paaudittrial.ActionDate = DateTime.Now;
            paaudittrial.ActionID = (int)action;
            paaudittrial.IP_Address = ipAddress;
            paaudittrial.AffectedWebPage = webpage;
            paaudittrial.Remark = remark;
            paaudittrial.UserID = currentuser.Id;
            paaudittrial.SchoolId = currentuser.SchoolId;

            context.PAAuditTrials.InsertOnSubmit(paaudittrial);
            context.SubmitChanges();

        }
    }


}
