using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;

public partial class UploadAdmissionTestScores: PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                var getConfigureAdminssionMode = context.AdmissionConfigurations.FirstOrDefault(s => s.SchoolId == logonUser.SchoolId).AdmissionMode;
                if (getConfigureAdminssionMode == "Form Only")
                {
                    btnSaveTestScore.Visible = false;
                    lblErrorMsg.Text = "No Test in your Configuration";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                }
                else
                {
                    Bindgrid();
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
                             where appList.SchoolId == (long)logonUser.SchoolId && appList.TestScore == null
                             && appList.TestInvitationStatus == 1
                             && appList.ProcessingLevel == 2
                             select new
                             {
                                 appList.ID,
                                 appList.ApplicantId,
                                 Fullname = appList.AdmissionApp.FirstName + " " + appList.AdmissionApp.LastName,
                                 appList.TestScore
                             };
        gdvList.DataBind();
    }

    protected void btnSaveTestScore_Click(object sender, EventArgs e)
    {
        try
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
                        objAdminList.TestScore = Convert.ToInt32(row.Cells[3].Controls.OfType<TextBox>().FirstOrDefault().Text);
                        objAdminList.DateTestTaken = DateTime.Now;
                        context.SubmitChanges();
                        row.Cells[4].Controls.OfType<CheckBox>().FirstOrDefault().Checked = false;
                    }
                }
            }

            lblErrorMsg.Text = "Saved Successfully";
            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
            lblErrorMsg.Visible = true;
            Bindgrid();
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
                row.Cells[3].Controls.OfType<Label>().FirstOrDefault().Visible = !isChecked;
                if (row.Cells[3].Controls.OfType<TextBox>().ToList().Count > 0)
                {
                    row.Cells[3].Controls.OfType<TextBox>().FirstOrDefault().Visible = isChecked;
                }
                if (!isChecked)
                {
                    chkAll.Checked = false;
                }
            }
        }
    }
}