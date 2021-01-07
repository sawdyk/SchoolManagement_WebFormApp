using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.LIB;
public partial class TeacherViewStudents : PASSIS.LIB.Utility.BasePage
{
    //protected void Page_PreInit(object sender, EventArgs e)
    //{
    //    base.Page_PreInit(sender, e);
    //    this.MasterPageFile = "~/TeachersStds.master";
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid();
        }


    }

    protected void BindGrid()
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
       long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);
        //gdvLists.DataSource = new PASSIS.DAO.ClassGradeDAL().getAllGradeStudents(logonUser.Id);
        gdvLists.DataSource = new PASSIS.LIB.ClassGradeLIB().getAllGradeStudents(logonUser.Id, (long)logonUser.SchoolId, logonUser.SchoolCampusId, curSessionId);
        gdvLists.DataBind();
    }

    //protected void gdvList_PageIndexChan(object sender, GridViewPageEventArgs e)
    //{
    //    gdvList.PageIndex = e.NewPageIndex;
    //    BindGrid();
    //}
    protected void gdvLists_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvLists.PageIndex = e.NewPageIndex;
        BindGrid();
    }
}