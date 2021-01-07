using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.LIB;
using PASSIS.DAO.Utility;
using PASSIS.DAO;


public partial class CreateStaff :PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
                PASSIS.LIB.User curUser = logonUser;
                ddlCampus.DataTextField = "Name";
                ddlCampus.DataValueField = "Id";
                ddlCampus.DataSource = new AcademicSessionLIB().GetAllSchoolCampus((long)curUser.SchoolId);
                ddlCampus.DataBind();
                ddlCampus.SelectedValue = curUser.SchoolCampusId.ToString();

                ddlRole.DataSource = new RoleLIB().getAllRolesSpecial();
                ddlRole.DataTextField = "RoleName";
                ddlRole.DataValueField = "Id";
                ddlRole.DataBind();
                ddlRole.Items.Insert(0, new ListItem("--Select Role--", "0"));
                ddlRole.SelectedIndex = 0;
            }
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        //validate the value and save
        try
        {
            string username = txtUsername.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(username))
            {
                lblErrorMsg.Text = string.Format("Username field cannot be Empty");
                lblErrorMsg.Visible = true;
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }
            if (new UsersLIB().RetrieveUser(username) != null)
            {
                lblErrorMsg.Text = string.Format("Username already exist. Try a more unique username");
                lblErrorMsg.Visible = true;
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }
            if (ddlRole.SelectedValue == "0")
            {
                lblErrorMsg.Text = string.Format("Select a role for the new user. ");
                lblErrorMsg.Visible = true;
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }

            
            Int32 selectedRole = 0;
            selectedRole = Convert.ToInt32(ddlRole.SelectedValue);
            UsersLIB usrDal = new UsersLIB();

            PASSIS.LIB.User usr = new PASSIS.LIB.User();
            usr.EmailAddress = txtEmailAddress.Text.Trim();
            usr.FirstName = txtFirstname.Text.Trim();
            usr.LastLoginDate = DateTime.Now;
            usr.LastName = txtLastname.Text.Trim();
            usr.DateOfBirth = (DateTime)SqlDateTime.MinValue;
            usr.Password = "password";
            usr.PhoneNumber = txtPhoneNumber.Text.Trim();
            usr.UserStatus = (Int32)UserStatus.Active;
            usr.SchoolId = logonUser.SchoolId;
            usr.Username = username;
            usr.SchoolCampusId = Convert.ToInt64(ddlCampus.SelectedValue);
            usr.Gender = Convert.ToInt32(ddlGender.SelectedValue);
            usr.DateCreated = DateTime.Now;

            usrDal.SaveUser(usr);

        //save user role
            PASSIS.LIB.UserRole usrRl = new PASSIS.LIB.UserRole();
            usrRl.RoleId = selectedRole;
            usrRl.UserId = usr.Id;
            usrDal.SaveUserRole(usrRl);

            //report
            lblErrorMsg.Text = "Successful! Please login with the following details;" + Environment.NewLine + "USERNAME: " + usr.Username + Environment.NewLine + "PASSWORD: " + usr.Password;
            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
            lblErrorMsg.Visible = true;

            //bind the data to a grid 
            //BindGrid();
            txtEmailAddress.Text = txtFirstname.Text = txtLastname.Text = txtPhoneNumber.Text = txtUsername.Text = string.Empty;

        //Response.Redirect("~/ViewStaff.aspx");
    }

        catch (Exception ex)
        {
            lblErrorMsg.Text = ex.Message;
            lblErrorMsg.Visible = true;
            throw ex;
        }
    }

   
}


