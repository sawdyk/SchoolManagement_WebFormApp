using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;

public partial class ViewApplicantInterviewScores : PASSIS.LIB.Utility.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            AdmissionConfiguration getConfigureAdminssionMode = context.AdmissionConfigurations.FirstOrDefault(s => s.SchoolId == logonUser.SchoolId);
            if (getConfigureAdminssionMode != null)
            {
                if (getConfigureAdminssionMode.AdmissionMode == "Form/Test/Interview")
                {
                    gdvList.DataSource = from admissionList in context.AdmissionApplicationLists
                                         where admissionList.SchoolId == logonUser.SchoolId &&
                                         admissionList.InterviewScore != null &&
                                         admissionList.ProcessingLevel == 3
                                         select admissionList;
                    gdvList.DataBind();

                }
                else
                {
                    lblErrorMsg.Text = "No Interview in your Configuration";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
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