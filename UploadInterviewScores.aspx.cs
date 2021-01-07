using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;


public partial class UploadInterviewScores: PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
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
                    btnSaveInterviewScore.Visible = false;
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
        gdvList.DataSource = from appList in context.AdmissionApplicationLists
                             where appList.SchoolId == (long)logonUser.SchoolId &&
                             appList.ProcessingLevel == 3 &&
                             appList.TestScore != null &&
                             appList.InterviewScore == null &&
                             appList.InterviewInvitationStatus == 1
                             select new
                             {
                                 appList.ID,
                                 appList.ApplicantId,
                                 Fullname = appList.AdmissionApp.FirstName + " " + appList.AdmissionApp.LastName,
                                 appList.TestScore,
                                 appList.InterviewScore
                             };
        gdvList.DataBind();
    }
    protected void btnSaveInterviewScore_Click(object sender, EventArgs e)
    {
        try
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
                        objAdminList.InterviewScore = Convert.ToInt32(row.Cells[4].Controls.OfType<TextBox>().FirstOrDefault().Text);
                        objAdminList.DateInterviewTaken = DateTime.Now;
                        context.SubmitChanges();
                        row.Cells[5].Controls.OfType<CheckBox>().FirstOrDefault().Checked = false;
                        lblErrorMsg.Text = "Saved Successfully";
                        lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                        lblErrorMsg.Visible = true;
                        Bindgrid();
                    }
                }
            }

            //lblErrorMsg.Text = "Test Saved Successfully";
            //lblErrorMsg.ForeColor = System.Drawing.Color.Green;
            //lblErrorMsg.Visible = true;
            //Bindgrid();
        }



        catch (Exception ex)
        {

        }
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
                row.Cells[4].Controls.OfType<Label>().FirstOrDefault().Visible = !isChecked;
                if (row.Cells[4].Controls.OfType<TextBox>().ToList().Count > 0)
                {
                    row.Cells[4].Controls.OfType<TextBox>().FirstOrDefault().Visible = isChecked;
                }
                if (!isChecked)
                {
                    chkAll.Checked = false;
                }
            }
        }
    }
}