using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;
using System.IO;
using System.Text;
using System.Data.Linq;
public partial class AdmissionStatus : PASSIS.LIB.Utility.BasePage
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
        AdmissionApplicationList checkProcessingStatus = context.AdmissionApplicationLists.FirstOrDefault(A => A.ProcessingLevel == 4 && A.SchoolId == (long)logonUser.SchoolId);
        AdmissionApplicationList checkProcessingStatusLevel2 = context.AdmissionApplicationLists.FirstOrDefault(A => A.ProcessingLevel == 5 && A.SchoolId == (long)logonUser.SchoolId);
        if (checkProcessingStatus != null)
        {
            gdvList.DataSource = from appSuccessfulList in context.AdmissionApplicationLists
                                 where appSuccessfulList.SchoolId == (long)logonUser.SchoolId && appSuccessfulList.ProcessingLevel == 4
                                 select new
                                 {
                                     appSuccessfulList.ID,
                                     appSuccessfulList.ApplicantId,
                                     Fullname = appSuccessfulList.AdmissionApp.FirstName + " " + appSuccessfulList.AdmissionApp.LastName,
                                     appSuccessfulList.TestScore,
                                     appSuccessfulList.InterviewScore
                                 };
            gdvList.DataBind();
            btnSendAdmissionNotification.Visible = true;
        }
        else
        {
            lblErrorMsg.Text = "Successful Applicant Doesn't Exist";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
        }
        //if (checkProcessingStatusLevel2 != null)
        //{
        //    lblErrorMsg.Text = "Applicant(s) have been admitted";
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
                //Label2.Visible = true;
                //bulkUploadFile.Visible = true;
                //chkAll.Checked = false;
                //row.Cells[3].Controls.OfType<Label>().FirstOrDefault().Visible = !isChecked;
                //if (row.Cells[3].Controls.OfType<TextBox>().ToList().Count > 0)
                //{
                //    row.Cells[3].Controls.OfType<TextBox>().FirstOrDefault().Visible = isChecked;
                //}
                if (!isChecked)
                {
                    chkAll.Checked = false;
                    //Label2.Visible = false;
                    //bulkUploadFile.Visible = false;
                }
            }
        }
    }
    public static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPEG", ".BMP", ".GIF", ".PNG" };

    protected void btnSendAdmissionNotification_Click(object sender, EventArgs e)
    {
        try
        {
            AdmissionApplicationList checkProcessingStatus = context.AdmissionApplicationLists.FirstOrDefault(A => A.ProcessingLevel == 3 && A.SchoolId == (long)logonUser.SchoolId);
            //AdmissionPayment checkPaymentStat = context.AdmissionPayments.FirstOrDefault(P => P.ApplicantId == checkProcessingStatus.ApplicantId && P.SchoolId == (long)logonUser.SchoolId && P.ApplicationListId == checkProcessingStatus.ID);

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
                            objAdminList.ProcessingLevel = 5;
                            objAdminList.Admissionstatus = "Successful";
                            //objAdminList.DateInterviewTaken = DateTime.Now;
                            context.SubmitChanges();
                            row.Cells[5].Controls.OfType<CheckBox>().FirstOrDefault().Checked = false;
                            row.Visible = false;

                            lblErrorMsg.Text = "Admission Notification Sent Successfully";
                            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                            lblErrorMsg.Visible = true;
                        }
                    }
                }

            }
            else
            {
                lblErrorMsg.Text = "Applicant has been Admitted";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
            }


            //else
            //{
            //    lblErrorMsg.Text = "Payment have not been Made!";
            //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            //    lblErrorMsg.Visible = true;
            //}

        }
        catch (Exception ex)
        {

        }


    }
}