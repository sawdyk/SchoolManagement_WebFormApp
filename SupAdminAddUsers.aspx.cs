using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO;
using PASSIS.DAO.Utility;
using System.Data.SqlTypes;
using PASSIS.LIB;

public partial class SupAdminAddUsers : PASSIS.LIB.Utility.BasePage
{
    //protected void Page_PreInit(object sender, EventArgs e)
    //{
    //    base.Page_PreInit(sender, e);
    //    this.MasterPageFile = "~/SupAdminUsers.master";
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void objRole_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["parentId"] = 1;
    }
    protected void BindGrid()
    {
        gdvList.DataSource = new PASSIS.LIB.UsersLIB().getAllUsers();
        gdvList.DataBind();
    }
    public string getRoleFromUserRole(object objUsrId)
    {
        Int64 userID = 0;
        string roleName = "";
        try
        {
            userID = Convert.ToInt32(objUsrId);
        }
        catch { }
        if (userID != 0)
        {
            //roleName = new PASSIS.DAO.RoleDAL().RetrieveRole(new PASSIS.DAO.UsersDAL().RetrieveUserRole(userID).RoleId).RoleName;
        }
        return roleName;

    }
    public string getSchoolName(object objSchlId)
    {
        Int64 schId = 0;
        string schName = "";
        try
        {
            schId = Convert.ToInt32(objSchlId);
        }
        catch { }
        if (schId != 0)
        {
            schName = new PASSIS.LIB.SchoolConfigLIB().RetrieveSchool(schId).Name;
        }
        return schName;

    }
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        if (txtUsername.Text.Trim().Contains(" "))
        {
            lblUsernameError.Text = string.Format("Spaces not allowed!");
            lblUsernameError.Visible = true;
            return;
        }
        if (txtFirstname.Text.Trim() == "" || txtLastname.Text.Trim() == "" || txtUsername.Text.Trim() == "" || txtEmailAddress.Text.Trim() == "")
        {
            //report
            lblErrorMsg.Text = "Some fields are empty, kindly review your input data.";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            lblErrorMsg.Focus();
        }
        else
        {
            //validate the value and save
            if (new UsersLIB().usernameExist(txtUsername.Text))
            {
                //report 
                lblUsernameError.Text = "Username already exist, kindly try a different one.";
                lblUsernameError.ForeColor = System.Drawing.Color.Red;
                lblUsernameError.Visible = true;
                txtUsername.Focus();
            }
            else
            {
                try
                {
                    Int32 selectedRole = 0;

                    selectedRole = Convert.ToInt32(ddlRole.SelectedValue);
                    UsersLIB usrDal = new UsersLIB();

                    PASSIS.LIB.User usr = new PASSIS.LIB.User();
                    usr.EmailAddress = txtEmailAddress.Text.Trim();
                    usr.FirstName = txtFirstname.Text.Trim();
                    //string dt = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                    //usr.LastLoginDate = DateTime.Parse(dt, new CultureInfo("fr-FR"));
                    usr.LastLoginDate = (DateTime)SqlDateTime.MinValue;
                    usr.LastName = txtLastname.Text.Trim();
                    usr.DateOfBirth = (DateTime)SqlDateTime.MinValue;
                    usr.Password = "password";
                    usr.PhoneNumber = txtPhoneNumber.Text.Trim();
                    usr.SchoolId = Convert.ToInt64(ddlSchool.SelectedValue.Trim());
                    usr.SchoolCampusId = Convert.ToInt64(ddlCampus.SelectedValue.Trim());
                    usr.Username = txtUsername.Text.Trim();
                    usrDal.SaveUser(usr);

                    //save user role
                    PASSIS.LIB.UserRole usrRl = new PASSIS.LIB.UserRole();
                    usrRl.RoleId = selectedRole;
                    usrRl.UserId = usr.Id;
                    usrDal.SaveUserRole(usrRl);


                    //report
                    lblErrorMsg.Text = "Success! Please login with the following details;" + Environment.NewLine + "USERNAME: " + usr.Username + Environment.NewLine + "PASSWORD: " + usr.Password;
                    lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                    lblErrorMsg.Visible = true;

                    //bind the data to a grid 
                    BindGrid();
                    ddlRole.SelectedValue = "0";
                    txtEmailAddress.Text = txtFirstname.Text = txtLastname.Text = txtPhoneNumber.Text = txtUsername.Text = string.Empty;
                }
                catch (Exception ex)
                {
                    lblErrorMsg.Text = ex.Message;
                    lblErrorMsg.Visible = true;
                    throw ex;
                }
            }
        }
    }

    protected void gdvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvList.PageIndex = e.NewPageIndex;
        BindGrid();
    }
    protected void ddlSchool_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlCampus.Items.Clear();
        ddlCampus.Items.Insert(0, new ListItem("--Select Campus--", "0", true));
        ddlCampus.DataSource = new CampusLIB().GetAllCampus(ddlSchool.SelectedValue);
        ddlCampus.DataBind();
    }
}