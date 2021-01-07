using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;
public partial class AdminViewStudents : PASSIS.LIB.Utility.BasePage
{
    
    long CampusID, yearId, lSupport = 0L;
    string studentName, fatherName, matricNo, AdminNo = string.Empty;
    long schoolId, gradeId, sessionId;

    protected void Page_Load(object sender, EventArgs e)
    {
        schoolId = (long)logonUser.SchoolId;
        CampusID = logonUser.SchoolCampusId;
       
        if (!IsPostBack)
        {
            PASSIS.LIB.PASSISLIBDataContext context = new PASSIS.LIB.PASSISLIBDataContext();
            var schlTypeId = context.Schools.FirstOrDefault(x => x.Id == (long)logonUser.SchoolId).SchoolTypeId;
            ddlYear.DataTextField = "Name";
            ddlYear.DataValueField = "Id";


            ddlYear.Items.Add(new ListItem(" All ", "0", true));
            ddlCampus.DataTextField = "Name";
            ddlCampus.DataValueField = "Id";
            ddlYear.DataSource = new PASSIS.LIB.ClassGradeLIB().getClassGrade(2, (long)schlTypeId);
            ddlYear.DataBind();

            ddlCampus.DataSource = new PASSIS.LIB.AcademicSessionLIB().RetrieveUserCampus((long)logonUser.SchoolCampusId);
            ddlCampus.DataBind();
            //Util.BindToEnum(typeof(LearningSupport), ddlLearningSupport);
            BindGrid(schoolId, CampusID, yearId, gradeId, studentName, fatherName, matricNo, AdminNo, lSupport, sessionId);
            lblResultSchoolCampus.Text = "SELECTED CAMPUS: " +  ddlCampus.SelectedItem.Text.ToUpper().Trim();

        }
    }
    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        //CampusID = Convert.ToInt64(ddlCampus.SelectedValue);
        yearId = Convert.ToInt64(ddlYear.SelectedValue);
        Int64.TryParse(ddlGrade.SelectedValue, out gradeId);
        //gradeId = Convert.ToInt64(ddlGrade.SelectedValue);
        //lSupport = Convert.ToInt64(ddlLearningSupport.SelectedValue);
        studentName = txtName.Text;
        fatherName = txtFathersName.Text;
        //matricNo = txtMatricNumber.Text;
        AdminNo = txtAdmNumber.Text;

        BindGrid(schoolId, CampusID, yearId, gradeId, studentName, fatherName, matricNo, AdminNo, lSupport, sessionId);
        lblResultSchoolCampus.Text = "SELECTED CAMPUS: " + ddlCampus.SelectedItem.Text.Trim();
        //lblTotalNumberofStudents.Text = students.Count.ToString();

        //usersFound = new UsersDAL().RetrieveStudents((long)logonUser.SchoolId, CampusID, yearId, (long)1.0);
        //usersFound = new UsersDAL().RetrieveStudents(
    }
   
    protected void gdvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvList.PageIndex = e.NewPageIndex;
        BindGrid(schoolId, CampusID, yearId, gradeId, studentName, fatherName, matricNo, AdminNo, lSupport, sessionId);
    }
    protected void BindGrid(long schoolId, long campusId, long yearId, long gradeId, string studentName, string fatherName, string matNo, string admNo, long learningSupport, long sessionId)
    {
        CampusID = Convert.ToInt64(ddlCampus.SelectedValue);
        yearId = Convert.ToInt64(ddlYear.SelectedValue);
        Int64.TryParse(ddlGrade.SelectedValue, out gradeId);

       var CurSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

        //lSupport = Convert.ToInt64(ddlLearningSupport.SelectedValue);
        studentName = txtName.Text;
        fatherName = txtFathersName.Text;
        //matricNo = txtMatricNumber.Text;
        AdminNo = txtAdmNumber.Text;

        //IList<User> students = new UsersDAL().RetrieveStudents((long)logonUser.SchoolId, logonUser.SchoolCampusId, 0, 0);
        IList<PASSIS.LIB.User> students = new PASSIS.LIB.UsersLIB().RetrieveStudents(schoolId, campusId, yearId, gradeId, studentName, fatherName, matNo, admNo, learningSupport, CurSessionId);
        gdvList.DataSource = students;
        gdvList.DataBind();
        lblTotalNumberofStudents.Text = "TOTAL NUMBER OF STUDENTS IN SELECTED YEAR AND GRADE: " + students.Count.ToString();
        IList<PASSIS.LIB.User> totalStudentsInSchool = new UsersLIB().RetrieveStudents((long)logonUser.SchoolId, logonUser.SchoolCampusId, 0, 0, CurSessionId);
        lblTotalInSchool.Text = "TOTAL NUMBER OF STUDENTS: " + totalStudentsInSchool.Count.ToString();

    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Response.ClearContent();
        Response.AppendHeader("content-disposition", "attachment; filename=" + ddlYear.SelectedItem.Text + ".xls");
        Response.ContentType = "application/vnd.ms-excel";
        System.IO.StringWriter stringWriter = new System.IO.StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(stringWriter);
        gdvList.AllowPaging = false;
        this.BindGrid(schoolId, CampusID, yearId, gradeId, studentName, fatherName, matricNo, AdminNo, lSupport, sessionId);
        gdvList.RenderControl(htw);
        Response.Write(stringWriter.ToString());
        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        //populate the grade list,
        Int64 selectedYearId = 0L;
        selectedYearId = Convert.ToInt64(ddlYear.SelectedValue);
        ddlGrade.Items.Clear();
        var availableGrades = new ClassGradeLIB().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, selectedYearId);
        ddlGrade.DataSource = availableGrades;
        ddlGrade.DataBind();
        ddlGrade.Items.Insert(0, new ListItem("--Select--", "0", true));
    }
}