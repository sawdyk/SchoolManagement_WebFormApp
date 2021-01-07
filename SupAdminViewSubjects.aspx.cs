using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO;
using PASSIS.DAO.Utility;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using PASSIS.LIB;

public partial class SupAdminViewSubjects : PASSIS.LIB.Utility.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindGrid();
        if (!IsPostBack)
        {
            ddlCurriculum.DataSource = new PASSIS.LIB.SchoolLIB().SchoolCurriculum();
            ddlCurriculum.DataBind();
            ddlCurriculum.Items.Insert(0, new ListItem("--Select Curriculum--", "0", true));
            //this.schoolType.Visible = false;
            ddlSchoolType.DataSource = new PASSIS.LIB.SchoolLIB().SchoolType();
            ddlSchoolType.DataBind();
            ddlSchoolType.Items.Insert(0, new ListItem("--Select School Type--", "0", true));
        }
    }
    protected void ddlCurriculum_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCurriculum.SelectedValue == "2")
        {
            ddlClassGrade.Items.Clear();
            //schoolType.Visible = true;
        }
        else
        {
            ddlClassGrade.Items.Clear();
            ddlClassGrade.DataSource = new PASSIS.LIB.ClassGradeLIB().getClassGrade(Convert.ToInt64(ddlCurriculum.SelectedValue));
            ddlClassGrade.DataBind();
            //schoolType.Visible = false;
        }
    }
    protected void ddlSchoolType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSchoolType.SelectedIndex == 1)
        {
            ddlClassGrade.DataSource = new PASSIS.LIB.ClassGradeLIB().getClassGrade(Convert.ToInt64(ddlCurriculum.SelectedValue), Convert.ToInt64(ddlSchoolType.SelectedValue));
            ddlClassGrade.DataBind();
        }
        else if (ddlSchoolType.SelectedIndex == 2)
        {
            ddlClassGrade.DataSource = new PASSIS.LIB.ClassGradeLIB().getClassGrade(Convert.ToInt64(ddlCurriculum.SelectedValue), Convert.ToInt64(ddlSchoolType.SelectedValue));
            ddlClassGrade.DataBind();
        }
        else if (ddlSchoolType.SelectedIndex == 3)
        {
            ddlClassGrade.DataSource = new PASSIS.LIB.ClassGradeLIB().getClassGrade(Convert.ToInt64(ddlCurriculum.SelectedValue), Convert.ToInt64(ddlSchoolType.SelectedValue));
            ddlClassGrade.DataBind();


        }
        else
        {
            ddlClassGrade.Items.Clear();
        }
    }
    public void BindGrid()
    {
        gvSubjects.DataSource = new SchoolLIB().GetMasterSubjects();
        gvSubjects.DataBind();
    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        if (ddlCurriculum.SelectedIndex == 1)
        {
            gvSubjects.DataSource = new PASSIS.LIB.SchoolLIB().GetClassSubjectsBritish(Convert.ToInt64(ddlCurriculum.SelectedValue), Convert.ToInt64(ddlClassGrade.SelectedValue));
            gvSubjects.DataBind();
        }
        else if (ddlCurriculum.SelectedIndex == 2)
        {
            gvSubjects.DataSource = new PASSIS.LIB.SchoolLIB().GetClassSubjectsNigeria(Convert.ToInt64(ddlCurriculum.SelectedValue), Convert.ToInt64(ddlSchoolType.SelectedValue), Convert.ToInt64(ddlClassGrade.SelectedValue));
            gvSubjects.DataBind();
        }
    }

    protected void gvSubjects_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSubjects.PageIndex = e.NewPageIndex;
        BindGrid();
    }
}