using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;

public partial class InterviewInvitationStatus : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    string IVStatus = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                var getConfigureAdminssionMode = context.AdmissionConfigurations.FirstOrDefault(s => s.SchoolId == logonUser.SchoolId).AdmissionMode;
                if (getConfigureAdminssionMode == "Form/Test/Interview")
                {
                    Bindgrid();
                }
                else
                {
                    lblErrorMsg.Text = "No Interview in your Configuration";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                }
            }
            catch (Exception ex)
            {
            }
        }
    }

    private void Bindgrid()
    {
        AdmissionConfiguration con = context.AdmissionConfigurations.FirstOrDefault(x => x.SchoolId == logonUser.SchoolId);
        //if(con.SelectFormFee == "NO")
        gdvList.DataSource = from appList in context.AdmissionApplicationLists
                             where appList.SchoolId == (long)logonUser.SchoolId
                             && appList.TestScore != null
                             && appList.TestInvitation != null
                             && appList.TestInvitationStatus == 1
                             && appList.InterviewScore == null
                             && appList.InterviewInvitation != null
                             && appList.InterviewInvitationStatus != null
                             && appList.ProcessingLevel == 3
                             select new
                             {
                                 appList.ID,
                                 appList.ApplicantId,
                                 Fullname = appList.AdmissionApp.FirstName + " " + appList.AdmissionApp.LastName,
                                 appList.InterviewInvitationStatus
                             };
        gdvList.DataBind();
    }

    public string getLNStatus(long status)
    {
        if (status == 0)
        {
            IVStatus = "Awaiting Approval";
        }
        else if (status == 1)
        {
            IVStatus = "Approved";
        }
        else if (status == 2)
        {
            IVStatus = "Declined";
        }
        return IVStatus;
    }


}