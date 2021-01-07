using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;

public partial class ProcessSuccessfulApplicantInterview : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bindgrid();
        }
    }

    private void Bindgrid()
    {
        AdmissionConfiguration checkminimiumScoreRequired = context.AdmissionConfigurations.FirstOrDefault(Adm => Adm.SchoolId == logonUser.SchoolId);
        if (checkminimiumScoreRequired != null)
        {
            AdmissionApplicationList checkProcessingStatus = context.AdmissionApplicationLists.FirstOrDefault(A => A.ProcessingLevel == 3 && A.SchoolId == (long)logonUser.SchoolId);
            AdmissionApplicationList checkProcessingStatusLevel2 = context.AdmissionApplicationLists.FirstOrDefault(A => A.ProcessingLevel == 3 && A.TestScore >= Convert.ToInt64(checkminimiumScoreRequired.MinimiumInterviewScore) && A.SchoolId == (long)logonUser.SchoolId);
            if (checkProcessingStatus != null)
            {
                gdvList.DataSource = from appSuccessfulList in context.AdmissionApplicationLists
                                     where appSuccessfulList.SchoolId == (long)logonUser.SchoolId &&
                                     appSuccessfulList.ProcessingLevel == 3 &&
                                     appSuccessfulList.TestScore != null &&
                                     appSuccessfulList.InterviewInvitationStatus == 1 &&
                                     appSuccessfulList.InterviewScore != null &&
                                     appSuccessfulList.InterviewScore >= Convert.ToInt64(checkminimiumScoreRequired.MinimiumInterviewScore)
                                     select new
                                     {
                                         appSuccessfulList.ID,
                                         appSuccessfulList.ApplicantId,
                                         Fullname = appSuccessfulList.AdmissionApp.FirstName + " " + appSuccessfulList.AdmissionApp.LastName,
                                         appSuccessfulList.TestScore,
                                         appSuccessfulList.InterviewScore
                                     };
                gdvList.DataBind();
                btnSaveInterviewScore.Visible = true;
            }
        }
        else
        {
            lblErrorMsg.Text = "You have not done the Admission Configuration";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
        }
        //if (checkProcessingStatusLevel2 != null)
        //{
        //    lblErrorMsg.Text = "Applicant have been Move to Another Stage";
        //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
        //    lblErrorMsg.Visible = true;
        //}

    }

    protected void OnCheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (sender as CheckBox);
        if (chk.ID == "chkAll")
        {
            foreach (GridViewRow row in gdvList.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    row.Cells[5].Controls.OfType<CheckBox>().FirstOrDefault().Checked = chk.Checked;
                }
            }
        }
        CheckBox chkAll = (gdvList.HeaderRow.FindControl("chkAll") as CheckBox);
        chkAll.Checked = true;
        foreach (GridViewRow row in gdvList.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                bool isChecked = row.Cells[5].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                //row.Cells[3].Controls.OfType<Label>().FirstOrDefault().Visible = !isChecked;
                //if (row.Cells[3].Controls.OfType<TextBox>().ToList().Count > 0)
                //{
                //    row.Cells[3].Controls.OfType<TextBox>().FirstOrDefault().Visible = isChecked;
                //}
                if (!isChecked)
                {
                    chkAll.Checked = false;
                }
            }
        }
    }
    protected void btnSaveInterviewScore_Click(object sender, EventArgs e)
    {

        try
        {
            AdmissionApplicationList checkProcessingStatus = context.AdmissionApplicationLists.FirstOrDefault(A => A.ProcessingLevel == 2 && A.SchoolId == (long)logonUser.SchoolId);

            if (checkProcessingStatus != null)
            {
                foreach (GridViewRow row in gdvList.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        bool isChecked = row.Cells[5].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                        if (isChecked)
                        {
                            string ApplicantId = row.Cells[1].Controls.OfType<Label>().FirstOrDefault().Text;
                            AdmissionApplicationList objAdminList = context.AdmissionApplicationLists.First(x => x.ApplicantId == ApplicantId && x.SchoolId == (long)logonUser.SchoolId);
                            objAdminList.ProcessingLevel = 4;
                            objAdminList.DateInterviewTaken = DateTime.Now;
                            context.SubmitChanges();
                            row.Cells[5].Controls.OfType<CheckBox>().FirstOrDefault().Checked = false;
                            row.Visible = false;
                            lblErrorMsg.Text = "Applicant Moved to Another Stage Successfully";
                            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                            lblErrorMsg.Visible = true;
                            Bindgrid();
                        }
                    }
                }

                //lblErrorMsg.Text = "Successfully Applicant Move to Another Stage";
                //lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                //lblErrorMsg.Visible = true;


            }
            else
            {
                lblErrorMsg.Text = "Applicant has been Moved to Another Stage";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
            }

        }
        catch (Exception ex)
        {

        }


    }
}