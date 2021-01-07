using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO;
using PASSIS.LIB;
using PASSIS.DAO.Utility;

public partial class ViewStaff : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid();


        }

        //Response.Write(logonUser.SchoolId);
    }

    protected void btnSearchStaff_OnClick(object sender, EventArgs e)
    {
        BindGrid(txtUsername.Text.Trim());
    }
    protected void objRole_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["parentId"] = 2;
    }
    public void BindGrid(string username)
        {

        PASSIS.LIB.User user = logonUser;
        if (string.IsNullOrEmpty(username))
        {
            gdvList.DataSource = new PASSIS.LIB.UsersLIB().RetrieveUsersBelowRole(2, (long)user.SchoolId, 5, user.SchoolCampusId);
        }
        else
        {
            gdvList.DataSource = new PASSIS.LIB.UsersLIB().RetrieveUsersByName(username);

        }
        gdvList.DataBind();

        }
    protected void BindGrid()
    {
        BindGrid(string.Empty);
    }
    protected void gdvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvList.PageIndex = e.NewPageIndex;
            BindGrid();
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
            try
            {
                roleName = new PASSIS.LIB.RoleLIB().RetrieveRole((long)new PASSIS.LIB.UsersLIB().RetrieveUserRole(userID).RoleId).RoleName;
            }
            catch { }
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
            schName = new PASSIS.DAO.SchoolConfigDAL().RetrieveSchool(schId).Name;
        }
        return schName;

    }
    public string getCampusName(object objSchlId)
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
            schName = new AcademicSessionLIB().RetrieveSchoolCampus(schId).Name;
        }
        return schName;

    }

    protected void gdvList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "remove")
            {
                Int64 Id = Convert.ToInt64(e.CommandArgument);
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                PASSIS.LIB.UserRole usrl = context.UserRoles.FirstOrDefault(s => s.UserId == Id);
                context.UserRoles.DeleteOnSubmit(usrl);

                context.SubmitChanges();
                BindGrid();
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "Staff deleted successfully";
                lblErrorMsg.ForeColor = System.Drawing.Color.Green;
            }

        }

        catch (Exception ex)
        {
            lblMessage.Text = "Error occurred, try again";
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }
}
