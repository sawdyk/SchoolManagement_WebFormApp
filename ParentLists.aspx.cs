using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO;
using PASSIS.DAO.Utility;
using PASSIS.LIB;
using System.Data.SqlTypes;


public partial class ParentLists : PASSIS.LIB.Utility.BasePage
{
    //protected void Page_PreInit(object sender, EventArgs e)
    //{
    //    base.Page_PreInit(sender, e);
    //    this.MasterPageFile = "~/SchlAdminStaffs.master";
    //}
    protected void Page_Load(object sender, EventArgs e)
    {
        BindGrid();
        if (!IsPostBack)
        {
            if (logonUserRole.Id == (long)roles.teacher)
            {
                pnlCreateUser.Visible = false;
            }
        }
    }
    protected void objRole_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["parentId"] = 2;
    }
    protected void btnSearchStaff_OnClick(object sender, EventArgs e)
    {
        BindGrid(txtUsername.Text.Trim());
        BindGridFirstname(txtUsername.Text.Trim());
    }
    protected void BindGrid()
    {
        BindGrid(string.Empty);
    }

    protected void BindGridFirstname(string firstname)
    {
        if (!string.IsNullOrEmpty(firstname))
        {
            gdvList.DataSource = RetrieveUsersInRoleByCampusFirstname(5, (long)logonUser.SchoolId, (long)logonUser.SchoolCampusId, firstname);
            gdvList.DataBind();
        }
            
    }

    // Code to fetch parent by firstname
    public IList<PASSIS.LIB.User> RetrieveUsersInRoleByCampusFirstname(Int64 roleId, Int64 schoolId, Int64 CampusId, string firstname)
    {
        IList<PASSIS.LIB.User> results = new List<PASSIS.LIB.User>();
        PASSISLIBDataContext context = new PASSISLIBDataContext();

        var allUsers = from usrRol in context.UserRoles where usrRol.RoleId == roleId && (long)usrRol.User.SchoolId == schoolId && (long)usrRol.User.SchoolCampusId == CampusId
                       && usrRol.User.FirstName.Trim().ToLower().Contains(firstname.ToLower())
                       select usrRol.User;
        return allUsers.ToList<PASSIS.LIB.User>();
    }

    protected void BindGrid(string username)
    {
        PASSIS.LIB.User user = logonUser;
        if (logonUserRole.Id == (long)roles.teacher)
        {
            gdvList.DataSource = new PASSIS.DAO.UsersDAL().RetrieveSingleUserList(user.Id);
        }
        else
        {
            if (string.IsNullOrEmpty(username))
                gdvList.DataSource = new PASSIS.LIB.UsersLIB().RetrieveUsersInRoleByCampus(5, (long)user.SchoolId, (long)user.SchoolCampusId);
            //gdvList.DataSource = new PASSIS.LIB.UsersLIB().RetrieveUsersBelowRole

            else

                gdvList.DataSource = new PASSIS.LIB.UsersLIB().RetrieveUsersByName(username);

        }
        //gdvList.DataSource = new PASSIS.DAO.UsersDAL().getAllUsers();
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
            try
            {
                //roleName = new PASSIS.DAO.RoleDAL().RetrieveRole((long)new PASSIS.DAO.UsersDAL().RetrieveUserRole(userID).RoleId).RoleName;
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
            schName = new PASSIS.DAO.AcademicSessionDAL().RetrieveSchoolCampus(schId).Name;

        }
        return schName;

    }
    protected void gdvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvList.PageIndex = e.NewPageIndex;
        BindGrid();
    }
 
}