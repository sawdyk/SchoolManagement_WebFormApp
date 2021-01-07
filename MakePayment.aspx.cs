using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO;
using PASSIS.LIB;

public partial class MakePayment : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblLists.Visible = true;
            InvoiceList();
            LoadPayment();
        }
    }
    protected void ddlPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPaymentMethod.SelectedIndex == 0)
        {
            lblBankName.Visible = false;
            lblInvoiceRef.Visible = false;
            lblBankName.Visible = false;
            lblBankRef.Visible = false;
            lblPaymentDate.Visible = false;

            lblAmounttoPay.Visible = false;
            txtAmounttoPay.Visible = false;
            lblInvoiceRef.Visible = false;
            txtRefNo.Visible = false;
            lblBankName.Visible = false;
            txtBankName.Visible = false;
            lblBankRef.Visible = false;
            txtDepAcctName.Visible = false;
            lblPaymentDate.Visible = false;
            txtBankPaymentDate.Visible = false;
            //rowName.Visible = false;
            //rowRefNo.Visible = false;
            //rowBankName.Visible = false;
            //rowPaymentDate.Visible = false;
            //trAmount.Visible = false;
        }
        else if (ddlPaymentMethod.SelectedIndex == 1)
        {
            lblBankName.Visible = false;
            lblInvoiceRef.Visible = false;
            lblBankName.Visible = false;
            lblBankRef.Visible = false;
            lblPaymentDate.Visible = false;
            lblAmounttoPay.Visible = true;
            txtAmounttoPay.Visible = true;
            lblInvoiceRef.Visible = false;
            txtRefNo.Visible = false;
            lblBankName.Visible = false;
            txtBankName.Visible = false;
            lblBankRef.Visible = false;
            txtDepAcctName.Visible = false;
            lblPaymentDate.Visible = false;
            txtBankPaymentDate.Visible = false;

            //rowName.Visible = false;
            //rowRefNo.Visible = false;
            //rowBankName.Visible = false;
            //rowPaymentDate.Visible = false;
            //trAmount.Visible = true;
            lblErrorMsg.Text = "";
        }
        else if (ddlPaymentMethod.SelectedIndex == 2)
        {
            lblBankName.Visible = true;
            lblInvoiceRef.Visible = true;
            lblBankName.Visible = true;
            lblBankRef.Visible = true;
            lblPaymentDate.Visible = true;
            lblAmounttoPay.Visible = true;
            txtAmounttoPay.Visible = true;
            lblInvoiceRef.Visible = true;
            txtRefNo.Visible = true;
            lblBankName.Visible = true;
            txtBankName.Visible = true;
            lblBankRef.Visible = true;
            txtDepAcctName.Visible = true;
            lblPaymentDate.Visible = true;
            txtBankPaymentDate.Visible = true;

            //rowName.Visible = true;
            //rowRefNo.Visible = true;
            //rowBankName.Visible = true;
            //rowPaymentDate.Visible = true;
            //trAmount.Visible = true;
            lblErrorMsg.Text = "";
        }
        else if (ddlPaymentMethod.SelectedIndex == 3)
        {
            lblBankName.Visible = true;
            lblInvoiceRef.Visible = true;
            lblBankName.Visible = true;
            lblBankRef.Visible = true;
            lblPaymentDate.Visible = true;
            lblAmounttoPay.Visible = true;
            txtAmounttoPay.Visible = true;
            lblInvoiceRef.Visible = true;
            txtRefNo.Visible = true;
            lblBankName.Visible = true;
            txtBankName.Visible = true;
            lblBankRef.Visible = true;
            txtDepAcctName.Visible = true;
            lblPaymentDate.Visible = true;
            txtBankPaymentDate.Visible = true;

            //rowName.Visible = true;
            //rowRefNo.Visible = true;
            //rowBankName.Visible = true;
            //rowPaymentDate.Visible = true;
            //trAmount.Visible = true;
            lblErrorMsg.Text = "";
        }
    }

    public void InvoiceList()
    {
        var invoiceList = from s in context.PaymentPermanents where s.ParentId == logonUser.Id select s;
        ddlInvoiceCode.DataTextField = "InvoiceCode";
        ddlInvoiceCode.DataValueField = "Id";
        ddlInvoiceCode.DataSource = invoiceList;
        ddlInvoiceCode.DataBind();
    }

    protected void btnMakePayment_Click(object sender, EventArgs e)
    {
        if (ddlInvoiceCode.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Select the Invoice Code";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
        if (ddlPaymentMethod.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Select Payment Method";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
        if (ddlPaymentMethod.SelectedIndex > 1)
        {
            if (txtAmounttoPay.Text == "")
            {
                lblErrorMsg.Text = "Amount Paid is required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            if (txtRefNo.Text == "")
            {
                lblErrorMsg.Text = "Payment Reference Number is Empty";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            if (txtBankName.Text == "")
            {
                lblErrorMsg.Text = "Bank Name is required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            if (txtDepAcctName.Text == "")
            {
                lblErrorMsg.Text = "Depositor/Account Name is required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            if (txtBankPaymentDate.Text == "")
            {
                lblErrorMsg.Text = "Payment Date is required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
        }
        //DateTime paymentDate = DateTime.ParseExact(this.txtBankPaymentDate.Text, "MM-dd-yyyy", null);

        PaymentPermanent objPayPermanent = context.PaymentPermanents.FirstOrDefault(x => x.Id == Convert.ToInt64(ddlInvoiceCode.SelectedValue));
        PaymentTemporary objTemporary = new PaymentTemporary();
        objTemporary.InvoiceCode = objPayPermanent.InvoiceCode;
        objTemporary.StudentId = objPayPermanent.StudentId;
        objTemporary.ParentId = objPayPermanent.ParentId;
        objTemporary.ClassId = objPayPermanent.ClassId;
        objTemporary.GradeId = objPayPermanent.GradeId;
        objTemporary.SchoolId = objPayPermanent.SchoolId;
        objTemporary.CampusId = objPayPermanent.CampusId;
        objTemporary.SessionId = objPayPermanent.SessionId;
        objTemporary.TermId = objPayPermanent.TermId;
        objTemporary.Amount = Convert.ToDecimal(txtAmounttoPay.Text);
        objTemporary.Date = DateTime.Now;
        objTemporary.PermanentPaymentId = objPayPermanent.Id;
        objTemporary.BankName = txtBankName.Text;
        objTemporary.DepositorAccountName = txtDepAcctName.Text;
        objTemporary.ReferenceCode = txtRefNo.Text;
        objTemporary.ApprovalStatusId = 1;
        context.PaymentTemporaries.InsertOnSubmit(objTemporary);
        context.SubmitChanges();

        lblSuccessMsg.Text = "Payment Submited Successfully";
        lblSuccessMsg.ForeColor = System.Drawing.Color.Green;
        lblSuccessMsg.Visible = true;

        txtAmounttoPay.Text = "";
        txtBankName.Text = "";
        txtDepAcctName.Text = "";
        txtRefNo.Text = "";
        txtBankPaymentDate.Text = "";

        LoadPayment();
    }

    public void LoadPayment()
    {
        var paymentList = from s in context.PaymentTemporaries
                          where s.ParentId == logonUser.Id
                          select new
                          {
                              s.Id,
                              s.InvoiceCode,
                              s.ReferenceCode,
                              s.Amount,
                              s.Date,
                              s.DepositorAccountName,
                              s.BankName,
                              s.ApprovalStatusId,
                              s.ApprovalStatus.Status
                          };
        gvPayment.DataSource = paymentList;
        gvPayment.DataBind();
    }
    protected void gvPayment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPayment.PageIndex = e.NewPageIndex;
        LoadPayment();
    }
}