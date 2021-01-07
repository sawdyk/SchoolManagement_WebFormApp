using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using System.IO;
using PASSIS.DAO.CustomClasses;
using System.Data.SqlClient;
using PASSIS.LIB;

public partial class BroadSheet : PASSIS.LIB.Utility.BasePage
{
    public static string SchoolName = "";
    public static string SchoolLogo = "";
    public static string SchoolAddress = "";
    public static string SchoolCurriculumId = "";
    public static string SchoolUrl = "";
    public static string SchoolTypeId = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            clsMyDB mdb = new clsMyDB();
            mdb.connct();
            string query = "SELECT * FROM Schools WHERE Id=" + logonUser.SchoolId;
            SqlDataReader reader = mdb.fetch(query);
            while (reader.Read())
            {
                SchoolName = reader[1].ToString();
                SchoolLogo = reader[5].ToString();
                SchoolAddress = reader[4].ToString();
                SchoolCurriculumId = reader[7].ToString();
                SchoolUrl = reader[6].ToString();
                SchoolTypeId = reader[8].ToString();
            }
            if (SchoolLogo == "") SchoolLogo = "/Images/Schools_Logo/passis_logoB.png";
            if (SchoolCurriculumId == "") SchoolCurriculumId = "0";
            reader.Close();
            mdb.closeConnct();

            long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
            long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
            if (curriculumId == (long)CurriculumType.British)
            {
                ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, 0);

            }
            else if (curriculumId == (long)CurriculumType.Nigerian)
            {
                ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);

            }
            ddlYear.DataBind();
        }

    }
    public enum CurriculumType
    {
        British = 1,
        Nigerian = 2
    }
    protected void ddlYear_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //populate the grade list,
        Int64 selectedYearId = 0L;
        selectedYearId = Convert.ToInt64(ddlYear.SelectedValue);
        ddlGrade.Items.Clear();
        var availableGrades = new ClassGradeDAL().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, selectedYearId);

        ddlGrade.DataSource = availableGrades;
        ddlGrade.DataBind();
        ddlGrade.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select All--", "0", true));
       
    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        lblAcademic.Visible = true;
        Int64 yearId = 0L, gradeId = 0L, schoolId = 0L, campusId = 0L;
        yearId = Convert.ToInt64(ddlYear.SelectedValue);
        gradeId = Convert.ToInt64(ddlGrade.SelectedValue);
        schoolId = (long)logonUser.SchoolId;
        campusId = logonUser.SchoolCampusId;

        btnExportToexcel.Visible = true;
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

        GridView1.DataSource = new PASSIS.LIB.ClassGradeLIB().RetrieveGradeStudents(schoolId, campusId, yearId, gradeId, curSessionId);
        GridView1.DataBind();
    }
    protected void btnExportToexcel_Click(object sender, EventArgs e) 
    {
        Response.ClearContent();
        Response.AppendHeader("content-disposition", "attachment; filename=" + ddlGrade.SelectedItem.Text + ".xls");
        Response.ContentType = "application/vnd.ms-excel";
        System.IO.StringWriter stringWriter = new System.IO.StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(stringWriter);
        GridView1.RenderControl(htw);
        Response.Write(stringWriter.ToString());
        Response.End();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
    }
}