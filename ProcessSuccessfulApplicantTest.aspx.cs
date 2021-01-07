using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;


public partial class ProcessSuccessfulApplicantTest : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
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
                    Bindgrid();
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
    private void Bindgrid()
    {
        AdmissionConfiguration checkminimiumScoreRequired = context.AdmissionConfigurations.FirstOrDefault(Adm => Adm.SchoolId == logonUser.SchoolId);
        AdmissionApplicationList checkProcessingStatus = context.AdmissionApplicationLists.FirstOrDefault(A => A.ProcessingLevel == 2 && A.SchoolId == (long)logonUser.SchoolId);
        AdmissionApplicationList checkProcessingStatusLevel2 = context.AdmissionApplicationLists.FirstOrDefault(A => A.ProcessingLevel == 3 && A.TestScore >= int.Parse(checkminimiumScoreRequired.MinimiumTestScore) && A.SchoolId == (long)logonUser.SchoolId);
        if (checkProcessingStatus != null)
        {
            gdvList.DataSource = from appSuccessfulList in context.AdmissionApplicationLists
                                 where appSuccessfulList.SchoolId == (long)logonUser.SchoolId &&
                                 appSuccessfulList.ProcessingLevel == 2 &&
                                 appSuccessfulList.TestScore != null &&
                                 appSuccessfulList.TestInvitationStatus == 1 &&
                                 appSuccessfulList.TestScore >= Convert.ToInt64(checkminimiumScoreRequired.MinimiumTestScore)
                                 select new
                                 {
                                     appSuccessfulList.ID,
                                     appSuccessfulList.ApplicantId,
                                     Fullname = appSuccessfulList.AdmissionApp.FirstName + " " + appSuccessfulList.AdmissionApp.LastName,
                                     appSuccessfulList.TestScore
                                 };
            gdvList.DataBind();
            btnMoveApplicants.Visible = true;
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
                    row.Cells[4].Controls.OfType<CheckBox>().FirstOrDefault().Checked = chk.Checked;
                }
            }
        }
        CheckBox chkAll = (gdvList.HeaderRow.FindControl("chkAll") as CheckBox);
        chkAll.Checked = true;
        foreach (GridViewRow row in gdvList.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                bool isChecked = row.Cells[4].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
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
    protected void btnMoveApplicants_Click(object sender, EventArgs e)
    {

        try
        {
            AdmissionApplicationList checkProcessingStatus = context.AdmissionApplicationLists.FirstOrDefault(A => A.ProcessingLevel == 1 && A.SchoolId == (long)logonUser.SchoolId);
            var getConfigureAdminssionMode = context.AdmissionConfigurations.FirstOrDefault(s => s.SchoolId == logonUser.SchoolId).AdmissionMode;
            if (checkProcessingStatus != null)
            {
                foreach (GridViewRow row in gdvList.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        bool isChecked = row.Cells[4].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                        if (isChecked)
                        {

                            string ApplicantId = row.Cells[1].Controls.OfType<Label>().FirstOrDefault().Text;
                            AdmissionApplicationList objAdminList = context.AdmissionApplicationLists.First(x => x.ApplicantId == ApplicantId && x.SchoolId == (long)logonUser.SchoolId);
                            if (getConfigureAdminssionMode == "Form/Test/Interview")
                            {
                                objAdminList.ProcessingLevel = 3;
                            }
                            else { objAdminList.ProcessingLevel = 4; }
                            objAdminList.DateTestTaken = DateTime.Now;
                            context.SubmitChanges();
                            row.Cells[4].Controls.OfType<CheckBox>().FirstOrDefault().Checked = false;
                            row.Visible = false;
                            lblErrorMsg.Text = "Applicant(s) Moved to Another Stage Successfully";
                            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                            lblErrorMsg.Visible = true;
                        }
                    }
                }

                //lblErrorMsg.Text = "Successfully Applicant Move to Another Stage";
                //lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                //lblErrorMsg.Visible = true;


            }
            else
            {
                lblErrorMsg.Text = "Applicants have been Moved to Another Stage";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
            }

        }
        catch (Exception ex)
        {

        }
    }
}