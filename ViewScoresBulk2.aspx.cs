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
using System.Web.UI.HtmlControls;

public partial class ViewScoresBulk2 : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
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
            int SessionId = 0;
            clsMyDB mdb = new clsMyDB();
            mdb.connct();
            string query = "SELECT DISTINCT AcademicSessionId FROM AcademicSession WHERE SchoolId=" + logonUser.SchoolId;
            SqlDataReader reader = mdb.fetch(query);
            //while (reader.Read())
            //{
            //    ddlSession.DataSource = from s in context.AcademicSessionNames
            //                            where s.ID == Convert.ToInt64(reader["AcademicSessionId"].ToString())
            //                            select s;
            //    ddlSession.DataBind();
            //    ddlSession.Items.Insert(0, new ListItem("--Select--", "0", true));
            //}
            //reader.Close();
            //mdb.closeConnct();

            ddlSession.DataSource = new ViewScoresBulk2().schSession().Distinct();
            ddlSession.DataTextField = "SessionName";
            ddlSession.DataValueField = "ID";
            ddlSession.DataBind();
            ddlSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Session--", "0", true));

            ddlTerm.DataSource = new AcademicTermLIB().RetrieveAcademicTerm();
            ddlTerm.DataBind();
            ddlTerm.Items.Insert(0, new ListItem("--Select--", "0", true));

            if (!isUserClassTeacher)
            {
                PASSIS.LIB.Grade grd = getLogonTeacherGrade;
                if (grd != null)
                {

                    IList<PASSIS.LIB.SubjectsInSchool> test = new PASSIS.LIB.SubjectTeachersLIB().getAllSubjects((long)logonUser.SchoolId);
                    foreach (PASSIS.LIB.SubjectsInSchool subjId in test)
                    {
                        PASSIS.LIB.Subject reqSubjects = context.Subjects.FirstOrDefault(c => c.Id == subjId.SubjectId);
                        ddlClassSubject.Items.Add(new ListItem(reqSubjects.Name, reqSubjects.Id.ToString()));
                    }
                    ddlClassSubject.Items.Insert(0, new ListItem("--Select--", "0", true));

                    BindDropDown(grd.Class_Grade.ClassSubjectIds);
                }
                else
                {
                    //IList<PASSIS.LIB.SubjectsInSchool> test = new PASSIS.LIB.SubjectTeachersLIB().getAllSubjects((long)logonUser.SchoolId);
                    //IList<PASSIS.LIB.SubjectTeacher> test = new TeacherLIB().getTeacherSubjects(logonUser.Id);
                    //foreach (PASSIS.LIB.SubjectTeacher subjId in test)
                    //{
                    //    PASSIS.LIB.Subject reqSubjects = context.Subjects.FirstOrDefault(c => c.Id == subjId.SubjectId);
                    //    ddlClassSubject.Items.Add(new ListItem(reqSubjects.Name, reqSubjects.Id.ToString()));
                    //}
                    //ddlClassSubject.Items.Insert(0, new ListItem("--Select--", "0", true));

                    long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
                    long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
                    ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
                    ddlYear.DataBind();

                }
            }
            else
            {
                ddlYear.DataSource = new PASSIS.LIB.TeacherLIB().getTeacherAllClass_Grade(logonUser.Id);
                ddlYear.DataBind();
                ////send a message that the logon user will not be able to view certain section of this page bcos he's not a class teacher 
            }
            //BindGrid(new AssignmentDAL().RetrieveAssignmentSubmitted(new ClassGradeDAL().RetrieveTeacherGrade(logonUser.Id).GradeId));

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
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        //populate the grade list,
        long yearId = Convert.ToInt64(ddlYear.SelectedValue);
        long sessionId = Convert.ToInt64(ddlSession.SelectedValue);
        long termId = Convert.ToInt64(ddlTerm.SelectedValue);
        ddlGrade.Items.Clear();
        var availableGrades = new ClassGradeDAL().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, yearId);
        ddlGrade.DataSource = availableGrades;
        ddlGrade.DataBind();
        ddlGrade.Items.Insert(0, new ListItem("--Select--", "0", true));
        //Populate Category
        ddlCategory.Items.Clear();
        //var categoryList = from s in context.ScoreCategoryConfigurations where s.ClassId == yearId && s.SchoolId == logonUser.SchoolId && s.CampusId == logonUser.SchoolCampusId select s;
        var categoryList = from s in context.ScoreCategoryConfigurations where s.ClassId == yearId && s.SessionId == sessionId && s.TermId == termId && s.SchoolId == logonUser.SchoolId && s.CampusId == logonUser.SchoolCampusId select s;
        ddlCategory.DataSource = categoryList;
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, new ListItem("--Select--", "0", true));
    }
    protected void ddlGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        if (!isUserClassTeacher)
        {
            IList<PASSIS.LIB.SubjectTeacher> getId = new TeacherLIB().getTeacherSubjects(logonUser.Id, Convert.ToInt32(ddlGrade.SelectedValue));
            //PASSISLIBDataContext context = new PASSISLIBDataContext();
            ddlClassSubject.Items.Clear();
            foreach (PASSIS.LIB.SubjectTeacher subjId in getId)
            {
                PASSIS.LIB.Subject reqSubject = context.Subjects.FirstOrDefault(c => c.Id == subjId.SubjectId);
                ddlClassSubject.Items.Add(new ListItem(reqSubject.Name, reqSubject.Id.ToString()));
            }
            ddlClassSubject.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
        else
        {
            ddlClassSubject.Items.Clear();
            IList<PASSIS.LIB.SubjectsInSchool> test = new SubjectTeachersLIB().getAllSubjectsForClass((long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue));
            foreach (PASSIS.LIB.SubjectsInSchool subjId in test)
            {
                PASSIS.LIB.Subject reqSubjects = context.Subjects.FirstOrDefault(c => c.Id == subjId.SubjectId);
                ddlClassSubject.Items.Add(new ListItem(reqSubjects.Name, reqSubjects.Id.ToString()));
            }
            ddlClassSubject.Items.Insert(0, new ListItem("--Select--", "0", true));
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Label1.Visible = true;
        try
        {
            Int64 yearId = 0L, gradeId = 0L, schoolId = 0L, campusId = 0L, sessionId = 0, termId = 0, categoryId = 0, subCategoryId = 0, subjectId = 0;
            yearId = Convert.ToInt64(ddlYear.SelectedValue);
            gradeId = Convert.ToInt64(ddlGrade.SelectedValue);
            sessionId = Convert.ToInt64(ddlSession.SelectedValue);
            termId = Convert.ToInt64(ddlTerm.SelectedValue);
            categoryId = Convert.ToInt64(ddlCategory.SelectedValue);
            subCategoryId = Convert.ToInt64(ddlSubCategory.SelectedValue);
            subjectId = Convert.ToInt64(ddlClassSubject.SelectedValue);
            schoolId = (long)logonUser.SchoolId;
            campusId = logonUser.SchoolCampusId;
            if (ddlYear.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select year";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            if (ddlGrade.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select grade";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            if (ddlSession.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select Session";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            if (ddlTerm.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select term";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            if (ddlCategory.SelectedItem.Text == "Behavioral")
            {
                if (ddlSubCategory.SelectedIndex == 0)
                {
                    lblErrorMsg.Text = "Kindly select sub category";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                    return;
                }
                //Int64 yearId = 0L, gradeId = 0L, schoolId = 0L, campusId = 0L;
                //yearId = Convert.ToInt64(ddlYear.SelectedValue);
                //gradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                //schoolId = (long)logonUser.SchoolId;
                //campusId = logonUser.SchoolCampusId;
                //btnExportToexcel.Visible = true;
                GetBehavioralScores(yearId, gradeId, sessionId, termId, subjectId, categoryId, subCategoryId, schoolId, campusId);
                MultiView1.SetActiveView(ViewBehavioural);
            }
            if (ddlCategory.SelectedItem.Text == "Extra Curricular")
            {
                if (ddlSubCategory.SelectedIndex == 0)
                {
                    lblErrorMsg.Text = "Kindly select sub category";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                    return;
                }
                //Int64 yearId = 0L, gradeId = 0L, schoolId = 0L, campusId = 0L;
                //yearId = Convert.ToInt64(ddlYear.SelectedValue);
                //gradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                //schoolId = (long)logonUser.SchoolId;
                //campusId = logonUser.SchoolCampusId;
                //btnExportToexcel.Visible = true;
                GetExtraScores(yearId, gradeId, sessionId, termId, subjectId, categoryId, subCategoryId, schoolId, campusId);
                MultiView1.SetActiveView(ViewExtraCurricular);
            }
            if (ddlCategory.SelectedItem.Text == "CA")
            {
                if (ddlClassSubject.SelectedIndex == 0)
                {
                    lblErrorMsg.Text = "Kindly select subject";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                    return;
                }
                if (ddlSubCategory.SelectedIndex == 0)
                {
                    lblErrorMsg.Text = "Kindly select sub category";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                    return;
                }
                //Int64 yearId = 0L, gradeId = 0L, schoolId = 0L, campusId = 0L, sessionId = 0, termId =0, categoryId = 0, subCategoryId = 0, subjectId=0;
                //yearId = Convert.ToInt64(ddlYear.SelectedValue);
                //gradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                //sessionId = Convert.ToInt64(ddlSession.SelectedValue);
                //termId = Convert.ToInt64(ddlTerm.SelectedValue);
                //categoryId = Convert.ToInt64(ddlCategory.SelectedValue);
                //subCategoryId = Convert.ToInt64(ddlSubCategory.SelectedValue);
                //subjectId = Convert.ToInt64(ddlClassSubject.SelectedValue);
                //schoolId = (long)logonUser.SchoolId;
                //campusId = logonUser.SchoolCampusId;
                //btnExportToexcel.Visible = true;
                GetCAScores(yearId, gradeId, sessionId, termId, subjectId, categoryId, subCategoryId, schoolId, campusId);
                MultiView1.SetActiveView(ViewTestAssignment);
            }
            else if (ddlCategory.SelectedItem.Text == "Exam")
            {
                if (ddlClassSubject.SelectedIndex == 0)
                {
                    lblErrorMsg.Text = "Kindly select subject";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                    return;
                }
                //Int64 yearId = 0L, gradeId = 0L, schoolId = 0L, campusId = 0L;
                //yearId = Convert.ToInt64(ddlYear.SelectedValue);
                //gradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                //schoolId = (long)logonUser.SchoolId;
                //campusId = logonUser.SchoolCampusId;
                //btnExportToexcel.Visible = true;
                GetExamScores(yearId, gradeId, sessionId, termId, subjectId, categoryId, subCategoryId, schoolId, campusId);
                MultiView1.SetActiveView(ViewExam);
            }

            //btnSaveScore.Visible = true;
        }
        catch (Exception ex)
        {

        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
    }

    
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Populate Sub Category
        ddlSubCategory.Items.Clear();
        var categoryList = from s in context.ScoreSubCategoryConfigurations where s.CategoryId == Convert.ToInt64(ddlCategory.SelectedValue) select s;
        ddlSubCategory.DataSource = categoryList;
        ddlSubCategory.DataBind();
        ddlSubCategory.Items.Insert(0, new ListItem("--Select--", "0", true));

        //if (ddlCategory.SelectedItem.Text == "Exam") 
        //{
        //    lblMark.Visible = false;
        //    txtTotalMark.Visible = false;
        //}
    }
    public IList<AcademicSessionName> schSession()
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        var session = from c in context.AcademicSessions
                      where c.SchoolId == logonUser.SchoolId && c.IsCurrent == true
                      orderby c.IsCurrent descending
                      select c.AcademicSessionName;
        return session.ToList<AcademicSessionName>();
    }

    public void GetCAScores(long yearId, long gradeId, long sessionId, long termId, long subjectId, long categoryId, long subCategoryId, long schoolId, long campusId)
    {
        var scores = from s in context.StudentScoreRepositories
                     where s.ClassId == yearId && s.GradeId == gradeId && s.SessionId == sessionId && s.TermId == termId && s.SubjectId == subjectId && s.CategoryId == categoryId && s.SubCategoryId == subCategoryId && s.SchoolId == schoolId && s.CampusId == campusId
                     select new
                     {
                         s.User.StudentFullName,
                         s.User.AdmissionNumber,
                         s.Subject.Name,
                         s.MarkObtainable,
                         s.MarkObtained,
                         s.PercentageScore,
                         s.CAPercentageScore,
                         s.FinalScore,
                         s.Remark
                     };
        gvTestAssigment.DataSource = scores.ToList();
        gvTestAssigment.DataBind();
    }
    public void GetExamScores(long yearId, long gradeId, long sessionId, long termId, long subjectId, long categoryId, long subCategoryId, long schoolId, long campusId)
    {
        var scores = from s in context.StudentScores
                     where s.ClassId == yearId && s.GradeId == gradeId && s.AcademicSessionID == sessionId && s.TermId == termId && s.SubjectId == subjectId && s.CategoryId == categoryId && (s.SubCategoryId == subCategoryId || s.SubCategoryId == null) && s.SchoolId == schoolId && s.CampusId == campusId
                     select new
                     {
                         s.User.StudentFullName,
                         s.User.AdmissionNumber,
                         s.Subject.Name,
                         s.ExamScoreObtainable,
                         s.ExamScore,
                         s.PercentageScore,
                         s.ExamPercentageScore,
                         s.FinalScore,
                         s.Remark
                     };
        gvExam.DataSource = scores.ToList();
        gvExam.DataBind();
    }
    public void GetBehavioralScores(long yearId, long gradeId, long sessionId, long termId, long subjectId, long categoryId, long subCategoryId, long schoolId, long campusId)
    {
        var scores = from s in context.StudentScoreBehaviorals
                     where s.ClassId == yearId && s.GradeId == gradeId && s.SessionId == sessionId && s.TermId == termId && s.CategoryId == categoryId && s.SubCategoryId == subCategoryId && s.SchoolId == schoolId && s.CampusId == campusId
                     select new
                     {
                         s.User.StudentFullName,
                         s.User.AdmissionNumber,
                         s.Subject.Name,
                         s.MarkObtainable,
                         s.MarkObtained,
                         s.PercentageScore,
                         s.CAPercentageScore,
                         s.Remark
                     };
        gvBehavioural.DataSource = scores.ToList();
        gvBehavioural.DataBind();
    }
    public void GetExtraScores(long yearId, long gradeId, long sessionId, long termId, long subjectId, long categoryId, long subCategoryId, long schoolId, long campusId)
    {
        var scores = from s in context.StudentScoreExtraCurriculars
                     where s.ClassId == yearId && s.GradeId == gradeId && s.SessionId == sessionId && s.TermId == termId && s.CategoryId == categoryId && s.SubCategoryId == subCategoryId && s.SchoolId == schoolId && s.CampusId == campusId
                     select new
                     {
                         s.User.StudentFullName,
                         s.User.AdmissionNumber,
                         s.Subject.Name,
                         s.MarkObtainable,
                         s.MarkObtained,
                         s.PercentageScore,
                         s.CAPercentageScore,
                         s.Remark
                     };
        gvExtraCurricular.DataSource = scores.ToList();
        gvExtraCurricular.DataBind();
    }
}