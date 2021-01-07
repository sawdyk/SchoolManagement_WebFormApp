using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.LIB;
using PASSIS.DAO.Utility;

public partial class ApproveAdmissionPayments : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindList();
        }
    }

    protected void gdvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvList.PageIndex = e.NewPageIndex;
        gdvList.DataBind();
    }
    protected void gdvList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Approve":
                    AdmissionPayment objApprovePayment = new AdmissionPayment();
                    long id = Convert.ToInt64(e.CommandArgument);
                    objApprovePayment = context.AdmissionPayments.First(x => x.ID == id);
                    objApprovePayment.HasPaid = true;
                    context.SubmitChanges();
                    lblReport.Text = "Approved Successfully";
                    lblReport.ForeColor = System.Drawing.Color.Green;
                    AdmissionApplicationList admList = context.AdmissionApplicationLists.FirstOrDefault(x => x.ApplicantId == objApprovePayment.ApplicantId);
                    var getConfigureAdminssionMode = context.AdmissionConfigurations.FirstOrDefault(s => s.SchoolId == logonUser.SchoolId).AdmissionMode;
                    if (getConfigureAdminssionMode == "Form Only")
                    {
                        admList.ProcessingLevel = 4;
                        context.SubmitChanges();
                    }
                    else
                    {
                        admList.ProcessingLevel = 2;
                        context.SubmitChanges();
                    }
                    BindList();
                    break;
            }
        }
        catch (Exception ex)
        {
            lblReport.Text = "No configuration available for this school";
            lblReport.ForeColor = System.Drawing.Color.Red;
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
        }
    }

    public void BindList()
    {
        gdvList.DataSource = from paymentList in context.AdmissionPayments
                             where paymentList.SchoolId == logonUser.SchoolId && paymentList.HasPaid == false
                             select new
                             {
                                 paymentList.ID,
                                 Fullname = paymentList.AdmissionApp.FirstName + " " + paymentList.AdmissionApp.LastName,
                                 paymentList.ApplicantId,
                                 paymentList.BankReferenceNo,
                                 paymentList.AmountPaid,
                                 paymentList.PaymentChannel,
                                 paymentList.PaymentDate,
                                 PaymentStatus = Convert.ToBoolean(paymentList.HasPaid) ? "Paid" : "Pending"
                             };
        gdvList.DataBind();
    }
}