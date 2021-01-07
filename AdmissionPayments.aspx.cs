using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.LIB;
using PASSIS.DAO.Utility;

public partial class AdmissionPayments : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        //var getConfigureFormFee = context.AdmissionConfigurations.FirstOrDefault(s => s.SchoolId == logonUser.SchoolId).SelectFormFee;
        var getConfigureFormFee = context.AdmissionConfigurations.FirstOrDefault(s => s.SchoolId == logonUser.SchoolId);

        if (getConfigureFormFee != null)
        {
            if (getConfigureFormFee.SelectFormFee == "NO")
            {
                btnContinue.Visible = false;
                Label1.Visible = false;
                txtApplicationNo.Visible = false;
                lblResponse.Text = "The form is free";
                lblResponse.ForeColor = System.Drawing.Color.Red;
                lblResponse.Visible = true;

            }
        }
        else
        {
            btnContinue.Visible = false;
            Label1.Visible = false;
            txtApplicationNo.Visible = false;
            lblResponse.Text = "No Admission Configuration has been set for this School!";
            lblResponse.ForeColor = System.Drawing.Color.Red;
            lblResponse.Visible = true;
        }
       

    }

    protected void btnContinue_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtApplicationNo.Text.Equals(""))
            {
                lblResponse.Text = "Application No is required";
                lblResponse.ForeColor = System.Drawing.Color.Red;
                lblResponse.Visible = true;
            }
            else
            {
                var getDetails = from appList in context.AdmissionApplicationLists
                                 where appList.ApplicantId == txtApplicationNo.Text.Trim()
                                 select appList;


                if (getDetails.Count() > 0)
                {
                    Session["ApplicationNo"] = txtApplicationNo.Text.Trim();
                    Response.Redirect("~/AdmissionFormPayment.aspx");
                }
                else
                {
                    lblResponse.Text = "This Application No doesn't exist";
                    lblResponse.ForeColor = System.Drawing.Color.Red;
                    lblResponse.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

}