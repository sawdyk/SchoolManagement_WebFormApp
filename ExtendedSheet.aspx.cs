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
using System.Data;
using System.Text;

public partial class ExtendedSheet : PASSIS.LIB.Utility.BasePage
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
            if (!isUserClassTeacher)
            {
                PASSIS.LIB.Grade grd = getLogonTeacherGrade;
                //if (grd != null)
                //{
                //BindDropDown(grd.Class_Grade.ClassSubjectIds);
                ddlAllSingle.Items.Add(new ListItem("All Student", "0"));
                ddlAllSingle.Items.Add(new ListItem("Single Student", "1"));

                ddlTemplate.Items.Add(new ListItem("Test or Assignment", "0"));
                ddlTemplate.Items.Add(new ListItem("Exam", "1"));
                long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
                long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
                ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
                ddlYear.DataBind();
                //}
            }
            else
            {
                long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
                long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
                ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
                //ddlYear.DataSource = new PASSIS.LIB.TeacherLIB().getTeacherAllClass_Grade(logonUser.Id);
                ddlYear.DataBind();
                ddlAllSingle.Items.Add(new ListItem("All Student", "0"));
                ddlAllSingle.Items.Add(new ListItem("Single Student", "1"));

                ddlTemplate.Items.Add(new ListItem("Test or Assignment", "0"));
                ddlTemplate.Items.Add(new ListItem("Exam", "1"));
                ////send a message that the logon user will not be able to view certain section of this page bcos he's not a class teacher 
            }

        }

    }

    private void BindDropDown(string subjectIds)
    {
        if (!string.IsNullOrEmpty(subjectIds))
        {
            ////IList<Int64> subjIds = PASSIS.DAO.Utility.Util.GetIdListFromString(subjectIds);
            ////ddlClassSubject.DataTextField = "SubjectName";
            ////ddlClassSubject.DataValueField = "Id";
            ////ddlClassSubject.DataSource = new PASSIS.DAO.SubjectTeachersDAL().getAllSubjects((subjIds));
            ////ddlClassSubject.DataBind();
            //PASSISLIBDataContext context = new PASSISLIBDataContext();
            //IList<PASSIS.LIB.SubjectTeacher> getId = new TeacherLIB().getTeacherSubjects(logonUser.Id, Convert.ToInt32(ddlGrade.SelectedValue));
            //ddlClassSubject.Items.Clear();
            //foreach (PASSIS.LIB.SubjectTeacher subjId in getId)
            //{
            //    PASSIS.LIB.Subject reqSubject = context.Subjects.FirstOrDefault(c => c.Id == subjId.SubjectId);
            //    ddlClassSubject.Items.Add(new ListItem(reqSubject.Name, reqSubject.Id.ToString()));
            //}
        }
    }
    protected void ddlTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    PASSISLIBDataContext context = new PASSISLIBDataContext();

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {

            if (ddlTemplate.SelectedIndex == 0)
            {
                if (ddlAllSingle.SelectedIndex == 1)
                {
                    if (ddlStudent.SelectedIndex == 0)
                    {
                        lblErrorMsg.Text = "Kindly Select the Student";
                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                        lblErrorMsg.Visible = true;
                        return;
                    }

                    lblresltt.Visible = true;
                    btnExportToexcel.Visible = true;
                    gvTestAssigment.DataSource = GetDataSourceSingleStudent();
                    gvTestAssigment.DataBind();
                    MultiView1.SetActiveView(ViewTestAssignment);

                }
                else
                {

                    lblresltt.Visible = true;
                    btnExportToexcel.Visible = true;
                    gvTestAssigment.DataSource = GetDataSource();
                    gvTestAssigment.DataBind();
                    MultiView1.SetActiveView(ViewTestAssignment);
                }

            }
            else if (ddlTemplate.SelectedIndex == 1)
            {
                if (ddlAllSingle.SelectedIndex == 1)
                {
                    if (ddlStudent.SelectedIndex == 0)
                    {
                        lblErrorMsg.Text = "Kindly Select the Student";
                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                        lblErrorMsg.Visible = true;
                        return;
                    }

                    lblresltt.Visible = true;
                    btnExportToexcel.Visible = true;
                    gvExam.DataSource = GetDataSourceSingleStudent();
                    gvExam.DataBind();
                    MultiView1.SetActiveView(ViewExam);

                }
                else
                {
                    lblresltt.Visible = true;
                    btnExportToexcel.Visible = true;
                    gvExam.DataSource = GetDataSource();
                    gvExam.DataBind();
                    MultiView1.SetActiveView(ViewExam);
                }
            }

        }
        catch (Exception ex)
        {
            //throw ex;
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Source, ex.StackTrace);
            lblErrorMsg.Text = "Error occurred";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;

        }


    }

    PASSISLIBDataContext db = new PASSISLIBDataContext();
    protected DataTable GetDataSource()
    {
        DataTable dt = new DataTable();
        try
        {

            Int64 yearId = 0L, gradeId = 0L, schoolId = 0L, campusId = 0L;
            yearId = Convert.ToInt64(ddlYear.SelectedValue);
            gradeId = Convert.ToInt64(ddlGrade.SelectedValue);
            schoolId = (long)logonUser.SchoolId;
            campusId = logonUser.SchoolCampusId;
            long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

            IList<PASSIS.LIB.GradeStudent> gradestudent = new PASSIS.LIB.ClassGradeLIB().RetrieveGradeStudents(schoolId, campusId, yearId, gradeId, curSessionId);
            var allUsers = from grd in db.GradeStudents
                           where grd.SchoolId == schoolId && grd.SchoolCampusId == campusId && grd.ClassId == yearId && grd.GradeId == gradeId
                           select grd;

            dt.Columns.AddRange(new DataColumn[2] { new DataColumn("StudentFullName"), new DataColumn("AdmissionNumber") });
            foreach (PASSIS.LIB.GradeStudent grdstud in gradestudent)
            {

                PASSIS.LIB.GradeStudent gstu = db.GradeStudents.FirstOrDefault(x => x.SchoolId == schoolId && x.SchoolCampusId == campusId && x.ClassId == yearId && x.GradeId == gradeId && x.HasGraduated == null);
                PASSIS.LIB.GradeStudent Allstudents = db.GradeStudents.FirstOrDefault(x => x.StudentId == grdstud.StudentId && (x.HasGraduated == null || x.HasGraduated == false));
                if (Allstudents != null)
                {
                    dt.Rows.Add(Allstudents.User.StudentFullName, Allstudents.User.AdmissionNumber);
                }
            }
            IList<PASSIS.LIB.SubjectTeacher> getId = new TeacherLIB().getTeacherSubjects(logonUser.Id, Convert.ToInt32(ddlGrade.SelectedValue));
            PASSISLIBDataContext context = new PASSISLIBDataContext();

            // var subjects = db.ExecuteQuery<string>("select Name from Subject where ClassId = '" + Convert.ToInt64(ddlYear.SelectedValue) + "'").ToList();

            // Header implementation
            int count = 0;
            //  var subject in subjects

            foreach (GridViewRow row in gdvAllSubject.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    bool isChecked = row.Cells[2].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    if (isChecked)
                    {
                        string SubjectName = row.Cells[1].Controls.OfType<Label>().FirstOrDefault().Text;
                        DataColumn dc = new DataColumn(SubjectName.ToString());
                        dt.Columns.Add(dc);
                    }
                }
                count++;
            }
            //foreach (PASSIS.LIB.SubjectTeacher subjId in getId)
            //{
            //    PASSIS.LIB.Subject reqSubject = context.Subjects.FirstOrDefault(c => c.Id == subjId.SubjectId);
            //    DataColumn dc = new DataColumn(reqSubject.Name.ToString());
            //    dt.Columns.Add(dc);
            //    count++;
            //}

            // Rows implementation here
            //DataRow rows = dt.NewRow();

            //dt.Rows.Add(rows);


        }
        catch (Exception ex)
        {
            //throw ex;
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Source, ex.StackTrace);
            lblErrorMsg.Text = "Error occurred";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;

        }
        return dt;
    }


    protected DataTable GetDataSourceSingleStudent()
    {
        DataTable dt = new DataTable();
        try
        {

            Int64 yearId = 0L, gradeId = 0L, schoolId = 0L, campusId = 0L;
            yearId = Convert.ToInt64(ddlYear.SelectedValue);
            gradeId = Convert.ToInt64(ddlGrade.SelectedValue);
            schoolId = (long)logonUser.SchoolId;
            campusId = logonUser.SchoolCampusId;
            long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

            IList<PASSIS.LIB.GradeStudent> gradestudent = new PASSIS.LIB.ClassGradeLIB().RetrieveGradeStudents(schoolId, campusId, yearId, gradeId, curSessionId);
            //var allUsers = from grd in db.GradeStudents
            //               where grd.SchoolId == schoolId && grd.SchoolCampusId == campusId && grd.ClassId == yearId && grd.GradeId == gradeId
            //               select grd;

            dt.Columns.AddRange(new DataColumn[2] { new DataColumn("StudentFullName"), new DataColumn("AdmissionNumber") });
            //foreach (PASSIS.LIB.GradeStudent grdstud in gradestudent)
            //{

               // PASSIS.LIB.GradeStudent gstu = db.GradeStudents.FirstOrDefault(x => x.SchoolId == schoolId && x.SchoolCampusId == campusId && x.ClassId == yearId && x.GradeId == gradeId && x.HasGraduated == null);
                PASSIS.LIB.GradeStudent student = db.GradeStudents.FirstOrDefault(x => x.StudentId == Convert.ToInt64(ddlStudent.SelectedValue) && (x.HasGraduated == null || x.HasGraduated == false));
                if (student != null)
                {
                    dt.Rows.Add(student.User.StudentFullName, student.User.AdmissionNumber);
                }
            //}
            IList<PASSIS.LIB.SubjectTeacher> getId = new TeacherLIB().getTeacherSubjects(logonUser.Id, Convert.ToInt32(ddlGrade.SelectedValue));
            PASSISLIBDataContext context = new PASSISLIBDataContext();

            // var subjects = db.ExecuteQuery<string>("select Name from Subject where ClassId = '" + Convert.ToInt64(ddlYear.SelectedValue) + "'").ToList();

            // Header implementation
            int count = 0;
            //  var subject in subjects

            foreach (GridViewRow row in gdvAllSubject.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    bool isChecked = row.Cells[2].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    if (isChecked)
                    {
                        string SubjectName = row.Cells[1].Controls.OfType<Label>().FirstOrDefault().Text;
                        DataColumn dc = new DataColumn(SubjectName.ToString());
                        dt.Columns.Add(dc);
                    }
                }
                count++;
            }
        
        }
        catch (Exception ex)
        {
            //throw ex;
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Source, ex.StackTrace);
            lblErrorMsg.Text = "Error occurred";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;

        }
        return dt;
    }



    protected void btnExportToexcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlAllSingle.SelectedIndex == 1)
            {
                if (ddlStudent.SelectedIndex == 0)
                {
                    lblErrorMsg.Text = "Kindly Select the Student";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                    return;
                }
            }

            if (ddlYear.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select year";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            if (ddlGrade.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select class";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            if (txtDescription.Text == "")
            {
                lblErrorMsg.Text = "Kindly supply a description for these Template";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }


            Int64 classId = Convert.ToInt64(ddlGrade.SelectedValue);
            IList<PASSIS.LIB.SubjectTeacher> getId = new TeacherLIB().getTeacherSubjects(logonUser.Id, Convert.ToInt32(ddlGrade.SelectedValue));
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            StringBuilder sb = new StringBuilder();
            long count = 0;
            long ifcheck = 0;
            foreach (GridViewRow row in gdvAllSubject.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    bool isChecked = row.Cells[2].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    if (isChecked)
                    {
                        string lblId = row.Cells[3].Controls.OfType<Label>().FirstOrDefault().Text;
                        long studentId = Convert.ToInt64(lblId);
                        sb.Append(studentId.ToString() + ",");
                        ifcheck++;
                    }
                }
                count++;
            }
            //foreach (PASSIS.LIB.SubjectTeacher subjId in getId)
            //{
            //    PASSIS.LIB.Subject reqSubject = context.Subjects.FirstOrDefault(c => c.Id == subjId.SubjectId);
            //    sb.Append(reqSubject.Id.ToString() + ",");
            //    count++;
            //}
            string AllSubjectId = sb.ToString().Remove(sb.ToString().Length - 1);

            Int64 TeacherId = logonUser.Id;
            string FileName1 = ddlGrade.SelectedItem.Text + AllSubjectId + txtDescription.Text + ".xls";
            string FileName = FileName1.Replace(",", "_");


            if (ddlTemplate.SelectedIndex == 0)
            {
                BroadSheetDescriptionCode broadsheet = context.BroadSheetDescriptionCodes.FirstOrDefault(x => x.DescriptionName == txtDescription.Text);
                if (broadsheet != null)
                {
                    lblErrorMsg.Text = "A template with this description has previously been generated, Use another description!";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                }


                else
                {



                    using (PASSISLIBDataContext dbContext = new PASSISLIBDataContext())
                    {
                        BroadSheetDescriptionCode DescriptionCode = new BroadSheetDescriptionCode
                        {
                            DescriptionName = txtDescription.Text

                        };
                        dbContext.BroadSheetDescriptionCodes.InsertOnSubmit(DescriptionCode);
                        dbContext.SubmitChanges();
                    }


                    BroadSheetDescriptionCode TestList = db.BroadSheetDescriptionCodes.FirstOrDefault(x => x.DescriptionName == txtDescription.Text);

                    TestAssigenmentBroadSheetTemplate newTempObj = new TestAssigenmentBroadSheetTemplate();
                    newTempObj.ClassId = classId;
                    newTempObj.DateGenerated = DateTime.Now;
                    newTempObj.DescriptionId = TestList.Id;
                    newTempObj.SubjectId = AllSubjectId;
                    newTempObj.TotalNumberofSubjectInserted = ifcheck;
                    newTempObj.TeacherId = TeacherId;
                    newTempObj.SchoolId = logonUser.SchoolId;
                    newTempObj.CampusId = logonUser.SchoolCampusId;
                    newTempObj.HasSubmitted = false;
                    new ScoresheetLIB().SaveTestAssignmentTemplateBroad(newTempObj);
                    Response.ClearContent();
                    Response.AppendHeader("content-disposition", "attachment; filename=" + FileName);
                    Response.ContentType = "application/vnd.ms-excel";
                    System.IO.StringWriter stringWriter = new System.IO.StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(stringWriter);
                    gvTestAssigment.RenderControl(htw);
                    Response.Write(stringWriter.ToString());
                    Response.End();
                }
            }

            else if (ddlTemplate.SelectedIndex == 1)
            {
                BroadSheetDescriptionCode TestList = db.BroadSheetDescriptionCodes.FirstOrDefault(x => x.DescriptionName == txtDescription.Text);
                //  TestAssigenmentBroadSheetTemplate TestList = new ScoresheetLIB().GetTemplateList(classId,);

                if (TestList != null)
                {
                    lblErrorMsg.Text = "This template has previously been generated";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                }
                else
                {
                    using (PASSISLIBDataContext dbContext = new PASSISLIBDataContext())
                    {
                        BroadSheetDescriptionCode DescriptionCode = new BroadSheetDescriptionCode
                        {
                            DescriptionName = txtDescription.Text

                        };
                        dbContext.BroadSheetDescriptionCodes.InsertOnSubmit(DescriptionCode);
                        dbContext.SubmitChanges();
                    }

                    BroadSheetDescriptionCode broadsheet = db.BroadSheetDescriptionCodes.FirstOrDefault(x => x.DescriptionName == txtDescription.Text);
                    TestAssigenmentBroadSheetTemplate newTempObj = new TestAssigenmentBroadSheetTemplate();
                    newTempObj.ClassId = classId;
                    newTempObj.DateGenerated = DateTime.Now;
                    newTempObj.DescriptionId = broadsheet.Id;
                    newTempObj.SubjectId = AllSubjectId;
                    newTempObj.TotalNumberofSubjectInserted = ifcheck;
                    newTempObj.TeacherId = TeacherId;
                    newTempObj.SchoolId = logonUser.SchoolId;
                    newTempObj.CampusId = logonUser.SchoolCampusId;
                    newTempObj.HasSubmitted = false;
                    new ScoresheetLIB().SaveTestAssignmentTemplateBroad(newTempObj);
                    Response.ClearContent();
                    Response.AppendHeader("content-disposition", "attachment; filename=" + FileName);
                    Response.ContentType = "application/vnd.ms-excel";
                    System.IO.StringWriter stringWriter = new System.IO.StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(stringWriter);
                    gvExam.RenderControl(htw);
                    Response.Write(stringWriter.ToString());
                    Response.End();
                }
            }
        }

        //        }
        //    }
        //}
        catch (Exception ex)
        {

        }

    }

    protected void BindGrid(long yearId, long gradeId)
    {
        long campusId, schoolId;
        campusId = logonUser.SchoolCampusId;
        schoolId = (long)logonUser.SchoolId;
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

        IList<PASSIS.LIB.GradeStudent> classList = new ClassGradeLIB().RetrieveGradeStudents(schoolId, campusId, yearId, gradeId, curSessionId);
        ddlStudent.Items.Add(new ListItem("--Select Student--", "0"));
        foreach (PASSIS.LIB.GradeStudent studentList in classList)
        {
            PASSIS.LIB.User stdList = db.Users.FirstOrDefault(x => x.Id == studentList.StudentId);
            ddlStudent.Items.Add(new ListItem(stdList.StudentFullName, stdList.Id.ToString()));
        }
    }


    public override void VerifyRenderingInServerForm(Control control)
    {
    }

    protected void ddlAllSingle_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAllSingle.SelectedIndex == 0) //if All student is selected displays
        { 
            ddlStudent.Visible = false;
            lblStudentName.Visible = false;
        }
        else
        {
            ddlStudent.Visible = true;
            lblStudentName.Visible = true;
        }

    }

    protected void ddlGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateallSubject();
        gdvAllSubject.Visible = true;
        lblsubjectResp.Visible = true;
        ddlStudent.Items.Clear();
        BindGrid(Convert.ToInt64(ddlYear.SelectedValue), Convert.ToInt64(ddlGrade.SelectedValue));

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

    protected void PopulateallSubject()
    {
        //PASSIS.LIB.Class_Grade classgrade = db.Class_Grades.FirstOrDefault(x => x.Name == ddlYear.SelectedItem.Text);
        //gdvAllSubject.DataSource = new TeacherLIB().GetAllSubject(Convert.ToInt32(classgrade.CurriculumId),classgrade.Id);
        //gdvAllSubject.DataBind();

        IList<PASSIS.LIB.Subject> subjects = new List<PASSIS.LIB.Subject>();
        IList<PASSIS.LIB.SubjectsInSchool> test = new PASSIS.LIB.SubjectTeachersLIB().getAllSubjects((long)logonUser.SchoolId);
        foreach (PASSIS.LIB.SubjectsInSchool subjId in test)
        {
            PASSIS.LIB.Subject reqSubjects = context.Subjects.FirstOrDefault(c => c.Id == subjId.SubjectId && c.ClassId == Convert.ToInt64(ddlYear.SelectedValue));
            if (reqSubjects != null)
            {
                subjects.Add(reqSubjects);
            }
        }
        gdvAllSubject.DataSource = subjects;
        gdvAllSubject.DataBind();
    }
    protected void gdvAllSubject_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void gdvAllSubject_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvAllSubject.PageIndex = e.NewPageIndex;
    }
    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chk = (sender as CheckBox);
            if (chk.ID == "chkAll")
            {
                foreach (GridViewRow row in gdvAllSubject.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        row.Cells[2].Controls.OfType<CheckBox>().FirstOrDefault().Checked = chk.Checked;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
            lblErrorMsg.Text = "Error occured, kindly contact your administrator";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
        }
    }
}