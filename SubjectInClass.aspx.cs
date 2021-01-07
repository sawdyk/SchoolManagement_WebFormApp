using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;
using System.Data.Linq;

public partial class SubjectInClass : PASSIS.LIB.Utility.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
            long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
            ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
            ddlYear.DataBind();
        }
    }

    protected void PopulateallSubject()
    {
        gdvListSubject.DataSource = new SubjectTeachersLIB().getAllSubjectsForClass((long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue));
        gdvListSubject.DataBind();
    }
    protected void ddlYear_OnSelectedIndexChanged(object sender, EventArgs e)
    {
      
        PopulateallSubject();
        lblCampusSelected.Text = ddlYear.SelectedItem.Text;
        lblCampusSelected.Visible = true;
        lblSelected.Visible = true;


    }

    protected void gdvListSubject_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvListSubject.PageIndex = e.NewPageIndex;
        PopulateallSubject();
    }
}