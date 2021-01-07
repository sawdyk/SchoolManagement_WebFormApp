using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO;
using PASSIS.DAO.Utility;
using PASSIS.LIB;


public partial class ChangePassword : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            var confirmCurrentPassword = from userList in context.Users
                                         where userList.Id == logonUser.Id && userList.Password == txtOldPassword.Text.Trim()
                                         select userList;
            if (confirmCurrentPassword.Count() > 0)
            {
                if (txtConNewPassword.Text.Trim().Length < 6)
                {
                    lblMessage.Text = "The password cannot be less than six characters";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    PASSIS.LIB.User objUser = context.Users.FirstOrDefault(x => x.Id == logonUser.Id);
                    objUser.Password = txtConNewPassword.Text.Trim();
                    context.SubmitChanges();
                    lblMessage.Text = "Password changed successfully";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    txtConNewPassword.Text = "";
                    txtNewPassword.Text = "";
                    txtOldPassword.Text = "";
                }
            }
            else
            {
                lblMessage.Text = "The supply current password is incorrect";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
        catch (Exception ex)
        {

        }
    }
}