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

public partial class ParentsViewSubjects : PASSIS.LIB.Utility.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            BindDropDown();
        }
    }

    protected void PopulateallSubject()
    {
        gdvListSubject.DataSource = new SubjectTeachersLIB().getAllSubjectsForClass((long)logonUser.SchoolId, Convert.ToInt64(ddlClass.SelectedValue));
        gdvListSubject.DataBind();
    }

    protected void BindDropDown()
    {
        ddlWard.DataSource = new UsersLIB().RetrieveParentsChildren(logonUser.Id);
        ddlWard.DataBind();
    }

    protected void ddlWard_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlClass.Items.Clear();
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        long currentSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);
        PASSIS.LIB.GradeStudent stdGrade = context.GradeStudents.FirstOrDefault(x => x.StudentId == Convert.ToInt64(ddlWard.SelectedValue) && x.AcademicSessionId == currentSessionId);
        if (stdGrade != null)
        {
            var stdClass = from s in context.Class_Grades where s.Id == stdGrade.ClassId select s;
            ddlClass.DataSource = stdClass;
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0", true));
        }
    }

    protected void ddlClass_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        PopulateallSubject();
        Label1.Visible = true;
        //lblCampusSelected.Text = ddlYear.SelectedItem.Text;
        //lblCampusSelected.Visible = true;
        //lblSelected.Visible = true;


    }

    protected void gdvListSubject_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvListSubject.PageIndex = e.NewPageIndex;
        PopulateallSubject(); 
    }
}