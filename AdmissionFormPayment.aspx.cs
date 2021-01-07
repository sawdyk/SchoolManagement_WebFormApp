using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.LIB;
using PASSIS.DAO.Utility;

public partial class AdmissionFormPayment : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["ApplicationNo"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                lblApplicationNo.Text = Session["ApplicationNo"].ToString();
                lblApplicationNo.ForeColor = System.Drawing.Color.Green;
                ddlPaymentChannel.Items.Add(new ListItem("Select Payment Method", "0"));
                ddlPaymentChannel.Items.Add(new ListItem("Online Card", "1"));
                ddlPaymentChannel.Items.Add(new ListItem("Online Transfer", "2"));
                ddlPaymentChannel.Items.Add(new ListItem("Bank deposit", "3"));
                AdmissionConfiguration objConfig = context.AdmissionConfigurations.First(x => x.SchoolId == (long)logonUser.SchoolId);
                lblFormFee.Text = objConfig.FormFee;
                txtBankRef.Text = "";
                txtPaymentDate.Text = "";
            }
        }

    }
    protected void btnPay_Click(object sender, EventArgs e)
    {
        if (ddlPaymentChannel.SelectedIndex == 0)
        {
            lblReport.Text = "Kindly select method of payment";
            lblReport.ForeColor = System.Drawing.Color.Red;
        }
        else if (ddlPaymentChannel.SelectedIndex == 1)
        {
            //Do online payment
            lblReport.Text = "Payment Channel Not yet Available";
            lblReport.ForeColor = System.Drawing.Color.Red;
        }
        else if (ddlPaymentChannel.SelectedIndex == 2 || ddlPaymentChannel.SelectedIndex == 3)
        {
            if (txtBankRef.Text == "" || txtPaymentDate.Text == "")
            {
                lblReport.Text = "All fields are required";
                lblReport.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                AdmissionPayment objPayment = new AdmissionPayment();
                AdmissionPayment objPaymentNotApproved = context.AdmissionPayments.FirstOrDefault(x => x.ApplicantId == Session["ApplicationNo"].ToString() && x.SchoolId == (long)logonUser.SchoolId && x.HasPaid == false);
                AdmissionPayment objPaymentApproved = context.AdmissionPayments.FirstOrDefault(x => x.ApplicantId == Session["ApplicationNo"].ToString() && x.SchoolId == (long)logonUser.SchoolId && x.HasPaid == true);
                AdmissionApplicationList objApplList = context.AdmissionApplicationLists.First(x => x.ApplicantId == Session["ApplicationNo"].ToString());
                if (objPaymentNotApproved != null)
                {
                    lblReport.Text = "Payment Made, Awaiting Approval";
                    lblReport.ForeColor = System.Drawing.Color.Red;
                }
                else if (objPaymentApproved != null)
                {
                    lblReport.Text = "Payment Made and Approved";
                    lblReport.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    objPayment.SchoolId = (long)logonUser.SchoolId;
                    objPayment.ApplicantId = Session["ApplicationNo"].ToString();
                    objPayment.AmountPaid = lblFormFee.Text.Trim();
                    objPayment.BankReferenceNo = txtBankRef.Text.Trim();
                    objPayment.PaymentChannel = ddlPaymentChannel.SelectedItem.Text;
                    objPayment.PaymentDate = txtPaymentDate.Text.Trim();
                    objPayment.HasPaid = false;
                    objPayment.ApplicationListId = objApplList.ID;
                    context.AdmissionPayments.InsertOnSubmit(objPayment);
                    context.SubmitChanges();
                    lblReport.Text = "Submitted successfully";
                    txtBankRef.Text = "";
                    txtPaymentDate.Text = "";
                    lblReport.ForeColor = System.Drawing.Color.Green;
                }
            }
        }

    }
    protected void ddlPaymentChannel_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPaymentChannel.SelectedIndex == 1)
        {
            lblRefNo.Visible = false;
            txtBankRef.Visible = false;
            lblPayDate.Visible = false;
            txtPaymentDate.Visible = false;
        }
        else if (ddlPaymentChannel.SelectedIndex == 2 || ddlPaymentChannel.SelectedIndex == 3)
        {
            lblRefNo.Visible = true;
            txtBankRef.Visible = true;
            lblPayDate.Visible = true;
            txtPaymentDate.Visible = true;
            btnPay.Text = "Submit";
        }
    }
}