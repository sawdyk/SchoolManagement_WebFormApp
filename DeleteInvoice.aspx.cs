using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO;
using PASSIS.LIB;


public partial class DeleteInvoice : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InvoiceList();
        }

    }
    protected void ddlInvoiceCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlInvoiceCode.SelectedIndex == 0)
        {
            lblList.Visible = false;
            lblSummary.Visible = false;
        }
        else
        {
            lblList.Visible = true;
            lblSummary.Visible = true;
        }
       
        getInvoiceSummary(ddlInvoiceCode.SelectedItem.Text);
        getInvoiceLists(ddlInvoiceCode.SelectedItem.Text);
    }

      
    public void InvoiceList()
    {
        var invoiceList = from s in context.PaymentPermanents where s.ParentId == logonUser.Id select s;
        ddlInvoiceCode.DataTextField = "InvoiceCode";
        ddlInvoiceCode.DataValueField = "Id";
        ddlInvoiceCode.DataSource = invoiceList;
        ddlInvoiceCode.DataBind();
    }
    protected void btnDeleteInvoice_Click(object sender, EventArgs e)
    {
        

        var checkLog = from s in context.PaymentTemporaries where s.PermanentPaymentId == Convert.ToInt64(ddlInvoiceCode.SelectedValue) select s;
        if (ddlInvoiceCode.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Select the Invoice Code";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
        if (checkLog.Count() > 0)
        {
            lblErrorMsg.Text = "This Invoice can't be deleted, there is an existing transaction on it";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }

        //        long parentId = logonUser.Id;
        //        long schoolId = (long)logonUser.SchoolId;
        //        long campusId = logonUser.SchoolCampusId;
        //        var NumofChildren = new UsersLIB().RetrieveParentsChildren(logonUser.Id);

        //        var paymentExists = from x in context.PaymentPermanents
        //                            where x.ParentId == parentId
        //                            && x.SchoolId == schoolId
        //                            && x.CampusId == campusId
        //                            select x;

        //        foreach (var paymentExist in paymentExists)

        //{

        //   Response.Write( paymentExists.Count());

        //}



        var invoiceList = from s in context.PaymentInvoiceLists where s.PermanentPaymentId == Convert.ToInt64(ddlInvoiceCode.SelectedValue) select s;
        context.PaymentInvoiceLists.DeleteAllOnSubmit(invoiceList);
        context.SubmitChanges();

        PaymentPermanent invoice = context.PaymentPermanents.FirstOrDefault(x => x.Id == Convert.ToInt64(ddlInvoiceCode.SelectedValue));
        if (invoice != null)
        {
            context.PaymentPermanents.DeleteOnSubmit(invoice);
            context.SubmitChanges();
            Response.Redirect("~/DeleteInvoice.aspx");
            //lblErrorMsg.Text = "Invoice Deleted Successfully";
            //lblErrorMsg.ForeColor = System.Drawing.Color.Green;
            //lblErrorMsg.Visible = true;
            //return;
        }

    }


    public void getInvoiceSummary(string invoiceCode)
        {

        var getOutStandSummary = from s in context.PaymentPermanents
                                 where s.SchoolId == logonUser.SchoolId && s.CampusId == logonUser.SchoolCampusId
                                 && s.InvoiceCode == invoiceCode

                                 select new
                                 {
                                     s.User.AdmissionNumber,
                                     s.InvoiceCode,
                                     s.User.FirstName,
                                     AmountGenerated = s.OriginalAmountGenerated,
                                     AfterDiscount = s.AmountGenerated,
                                     s.AmountPaid,
                                     s.Balance,
                                     s.Discount,
                                     s.DateCreated
                                 };

        gvOutStandPaySummary.DataSource = getOutStandSummary;
        gvOutStandPaySummary.DataBind();

    }

    public void getInvoiceLists(string invoiceCode)
    {

        var getOutStandPayList = from s in context.PaymentInvoiceLists
                                 where s.SchoolId == logonUser.SchoolId && s.CampusId == logonUser.SchoolCampusId
                                 && s.InvoiceCode == invoiceCode
                                 select new
                                 {

                                     s.User.AdmissionNumber,
                                     s.InvoiceCode,
                                     s.User.FirstName,
                                     Class = s.Class_Grade.Name,
                                     Grade = s.Grade.GradeName,
                                     Session = s.AcademicSessionName.SessionName,
                                     Term = s.AcademicTerm1.AcademicTermName,
                                     Fee = s.PaymentFee.PaymentFeeType.FeeName,
                                     s.Amount

                                 };
        gvOutStandPayList.DataSource = getOutStandPayList;
        gvOutStandPayList.DataBind();

    }

    protected void gvOutStandPayList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvOutStandPayList.PageIndex = e.NewPageIndex;
        gvOutStandPayList.DataBind();
    }


    protected void gvOutStandPaySummary_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvOutStandPaySummary.PageIndex = e.NewPageIndex;
        gvOutStandPaySummary.DataBind();
    }
}