using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;

public partial class ViewApplicantTestScores : PASSIS.LIB.Utility.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        if (!IsPostBack)
        {
            AdmissionConfiguration getConfigureAdminssionMode = context.AdmissionConfigurations.FirstOrDefault(s => s.SchoolId == logonUser.SchoolId);
            if (getConfigureAdminssionMode != null)
            {
                if (getConfigureAdminssionMode.AdmissionMode == "Form Only")
                {
                    lblErrorMsg.Text = "No Test in your Configuration";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                }
                else
                {

                    gdvList.DataSource = from admissionList in context.AdmissionApplicationLists
                                         where admissionList.SchoolId == logonUser.SchoolId &&
                                         admissionList.ProcessingLevel == 2 &&
                                         admissionList.TestScore != null &&
                                         admissionList.TestInvitationStatus == 1
                                         select admissionList;
                    gdvList.DataBind();
                }

            }
            else
            {
                lblErrorMsg.Text = "You have not done the Admission Configuration";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
            }
        }
    }

    protected void gdvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvList.PageIndex = e.NewPageIndex;
        gdvList.DataBind();
    }
}