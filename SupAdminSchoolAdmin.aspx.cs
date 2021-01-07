using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO;
using PASSIS.LIB;
using PASSIS.DAO.Utility;
using System.Data.SqlTypes;

public partial class SupAdminSchoolAdmin : PASSIS.LIB.Utility.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        BindGrid();
       
    }
  
    protected void BindGrid()
    {
        PASSISDataContext context = new PASSISDataContext();


        var allRoles = from rol in context.Roles
                       where rol.ParentRoleId == 1 && rol.Id != 5 && rol.Id != 6 && rol.Id != 14 && rol.Id != 15 && rol.Id != 18
                       select rol.Id;
        IList<Int64> IRoleIds = allRoles.ToList<Int64>();


        long[] roleIds = IRoleIds.ToArray();
        //if (campusId > 0)
        var allUserIds = from usrRol in context.UserRoles where roleIds.Contains((long)usrRol.RoleId) select (long)usrRol.UserId;

        //var allUser = (System.Linq.IQueryable<PASSIS.LIB.User>)null;
        //if (schoolId == 0)
        var allUser = from usr in context.Users where allUserIds.ToList<Int64>().Contains(usr.Id) select usr;
        //else
        allUser = from usr in context.Users
                  where allUserIds.ToList<Int64>().Contains(usr.Id)
                  select usr;

        gdvList.DataSource = allUser;
        gdvList.DataBind();
    }

    protected void gdvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvList.PageIndex = e.NewPageIndex;
        BindGrid();
    }
}