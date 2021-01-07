using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using System.Data;
using System.Data.OleDb;
using System.IO;
using PASSIS.LIB;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Text;


public partial class StudentDuplicate : PASSIS.LIB.Utility.BasePage
{
    //protected void Page_PreInit(object sender, EventArgs e)
    //{
    //    base.Page_PreInit(sender, e);
    //    this.MasterPageFile = "~/SchlAdminStds.master";
    //}
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    PASSIS.LIB.Utility.BasePage log = new PASSIS.LIB.Utility.BasePage();
    PASSISDataContext db = new PASSISDataContext();
    PASSIS.LIB.ParentDetail parentDetail = new PASSIS.LIB.ParentDetail();
    PASSIS.LIB.User ExistingUsrs = new PASSIS.LIB.User();
    string schoolCode = "";
    int skipDataWithissue = 0;
    string skippedReason = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            clsMyDB mdb = new clsMyDB();
            mdb.connct();
            string query = "SELECT DISTINCT AcademicSessionId FROM AcademicSession WHERE SchoolId=" + logonUser.SchoolId;
            SqlDataReader reader = mdb.fetch(query);
            while (reader.Read())
            {
                ddlSession.DataSource = from s in context.AcademicSessionNames
                                        where s.ID == Convert.ToInt64(reader["AcademicSessionId"].ToString())
                                        select s;
                ddlSession.DataBind();
            }
            ddlSession.Items.Insert(0, new ListItem("--Select--", "0", true));
            reader.Close();
            mdb.closeConnct();

            lblResult.Visible = false;
            PASSIS.LIB.User currentUser = logonUser;
            ddlCampus.DataSource = new AcademicSessionDAL().GetAllSchoolCampus((long)currentUser.SchoolId);
            ddlCampus.DataBind();
            //ddlCampus.SelectedValue = currentUser.SchoolCampusId.ToString();
            ddlCampus.Items.Insert(0, new ListItem("--Select--", "0", true));

            var schlTypeId = context.Schools.FirstOrDefault(x => x.Id == (long)logonUser.SchoolId).SchoolTypeId;
            ddlYear.DataTextField = "Name";
            ddlYear.DataValueField = "Id";
            ddlYear.DataSource = new PASSIS.LIB.ClassGradeLIB().getClassGrade(2, (long)schlTypeId);
            ddlYear.DataBind();
            ddlYear.Items.Insert(0, new ListItem(" --Select-- ", "0", true));


            //ddlCampus.Enabled = false;
            //bp.Visible = false;

            long schoolId = (long)logonUser.SchoolId;
            PASSIS.LIB.SchoolLIB schLib = new PASSIS.LIB.SchoolLIB();
            schoolCode = schLib.RetrieveSchool(schoolId).Code;
        }

    }
    private DataTable createUnsuccessfulDataTableTemplate()
    {
        DataTable table = new DataTable("Table Title");

        DataColumn col1 = new DataColumn("New Name");
        col1.DataType = System.Type.GetType("System.String");

        DataColumn col2 = new DataColumn("Existing Name(s)");
        col1.DataType = System.Type.GetType("System.String");

        DataColumn col3 = new DataColumn("Reason");
        col3.DataType = System.Type.GetType("System.String");


        table.Columns.Add(col1);
        table.Columns.Add(col2);
        table.Columns.Add(col3);
        return table;
    }
    private DataTable createsuccessfulDataTableTemplate()
    {
        DataTable table = new DataTable("Table Title");

        DataColumn col1 = new DataColumn("New Name");
        col1.DataType = System.Type.GetType("System.String");

        DataColumn col2 = new DataColumn("Existing Name(s)");
        col1.DataType = System.Type.GetType("System.String");

        DataColumn col3 = new DataColumn("Remark");
        col3.DataType = System.Type.GetType("System.String");


        table.Columns.Add(col1);
        table.Columns.Add(col2);
        table.Columns.Add(col3);
        return table;
    }
    protected void ddlCampus_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlStudent.Items.Clear();
        var dupList = from s in context.StudentNameDuplicateExceptions
                      where s.SchoolId == log.logonUser.SchoolId
                          && s.CampusId == Convert.ToInt64(ddlCampus.SelectedValue)
                      select s;
        IList<StudentNameDuplicateException> listOfDupStudent = dupList.ToList<StudentNameDuplicateException>();
        ddlStudent.DataSource = listOfDupStudent;
        ddlStudent.DataBind();
        ddlStudent.Items.Insert(0, new ListItem("--Select--", "0", true));
    }
    protected void ddlStudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblStudents.Visible = true;
        //StudentNameDuplicateException duplicate = context.StudentNameDuplicateExceptions.FirstOrDefault(x=>x.Id==Convert.ToInt64(ddlStudent.SelectedValue));
        //if (duplicate != null)
        //{
        var existingStudents = from s in context.StudentNameDuplicateExceptions
                               where s.NewStudent == ddlStudent.SelectedValue
                                   && s.SchoolId == log.logonUser.SchoolId && s.CampusId == log.logonUser.SchoolCampusId
                               select s;
        IList<StudentNameDuplicateException> stdList = existingStudents.ToList<StudentNameDuplicateException>();
        GridView1.DataSource = stdList;
        GridView1.DataBind();
        //}
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (ddlYear.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Select the new class";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
        if (ddlGrade.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Select the new grade";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
        if (ddlSession.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Select the new session";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
        int count = 0;
        foreach (GridViewRow row in GridView1.Rows)
        {

            if (row.RowType == DataControlRowType.DataRow)
            {
                bool isChecked = row.Cells[7].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                if (isChecked)
                {
                    count++;
                    if (count == 1)
                    {
                        long studentId = Convert.ToInt64(row.Cells[6].Controls.OfType<Label>().FirstOrDefault().Text.ToString().Trim());
                        PASSIS.LIB.Grade getGradeTeacherObj = context.Grades.FirstOrDefault(x => x.Id == Convert.ToInt64(ddlGrade.SelectedValue));
                        long? gradeTeacherId = getGradeTeacherObj.GradeTeacherId;

                        PASSIS.LIB.User stdObj = context.Users.FirstOrDefault(x => x.Id == studentId);
                        if (stdObj != null)
                        {
                            stdObj.StudentStatus = true;

                            PASSIS.LIB.GradeStudent grdObj = context.GradeStudents.FirstOrDefault(x => x.StudentId == studentId);
                            if (grdObj != null)
                            {
                                grdObj.StudentStatusId = 1;
                                grdObj.GradeTeacherId = gradeTeacherId;
                                grdObj.SessionId = Convert.ToInt64(ddlSession.SelectedValue);
                                grdObj.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
                                grdObj.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                                grdObj.HasGraduated = false;
                            }
                            context.SubmitChanges();

                            var exepObj = from s in context.StudentNameDuplicateExceptions where s.NewStudent == ddlStudent.SelectedValue select s;
                            IList<StudentNameDuplicateException> exepObjList = exepObj.ToList<StudentNameDuplicateException>();
                            foreach (StudentNameDuplicateException s in exepObjList)
                            {
                                StudentNameDuplicateException duplicate = context.StudentNameDuplicateExceptions.FirstOrDefault(x => x.ExistingStudentId == s.ExistingStudentId
                                    && x.SchoolId == s.SchoolId
                                    && x.CampusId == s.CampusId);
                                if (duplicate != null)
                                {
                                    context.StudentNameDuplicateExceptions.DeleteOnSubmit(duplicate);
                                    context.SubmitChanges();
                                }
                            }

                            GridView1.DataSource = null;
                            GridView1.DataBind();
                            lblErrorMsg.Text = "The student is now active";
                            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                            lblErrorMsg.Visible = true;
                        }
                    }
                    else
                    {

                        lblErrorMsg.Text = "You selected more than one and the operation has been performed on the first one";
                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                        lblErrorMsg.Visible = true;
                    }
                }
            }
        }
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int64 selectedYearId = 0L;
        selectedYearId = Convert.ToInt64(ddlYear.SelectedValue);
        ddlGrade.Items.Clear();
        var availableGrades = new ClassGradeDAL().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, selectedYearId);
        ddlGrade.DataSource = availableGrades;
        ddlGrade.DataBind();
        ddlGrade.Items.Insert(0, new ListItem("--Select--", "0", true));
    }
}