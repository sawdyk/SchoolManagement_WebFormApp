using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO;
using PASSIS.DAO.Utility;
using System.IO;
using PASSIS.LIB;
using System.Data.SqlClient;
using System.Data;

public partial class StudentGradeBook : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
   
    protected void Page_Load(object sender, EventArgs e)
    {
        Int64 userId = logonUser.Id;
        Int64 StudentGradeId = 0L;
        Int64 StudentClassId = 0L;
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

        StudentGradeId = new ClassGradeLIB().RetrieveStudentGrade(userId, curSessionId).GradeId;
        StudentClassId = new ClassGradeLIB().RetrieveStudentGrade(userId, curSessionId).ClassId;

        
        if (!IsPostBack)
        {
            PopulateallSubject();
            StudentGradeId = new ClassGradeLIB().RetrieveStudentGrade(userId, curSessionId).GradeId;
           StudentClassId = new ClassGradeLIB().RetrieveStudentGrade(userId, curSessionId).ClassId;

          
            ddlCategory.Items.Add(new ListItem("CA", "0"));
            ddlCategory.Items.Add(new ListItem("Exam", "1"));
        }

    }

    protected void PopulateallSubject()
    {
        Int64 userId = logonUser.Id;
        Int64 StudentGradeId = 0L;
        Int64 StudentClassId = 0L;
                long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

        StudentGradeId = new ClassGradeLIB().RetrieveStudentGrade(userId, curSessionId).GradeId;
        StudentClassId = new ClassGradeLIB().RetrieveStudentGrade(userId, curSessionId).ClassId;

        PASSIS.LIB.Class_Grade classgrade = context.Class_Grades.FirstOrDefault(x => x.Id == StudentClassId);
        //gdvAllSubj.DataSource = new TeacherLIB().GetAllSubject(Convert.ToInt32(classgrade.CurriculumId), classgrade.Id);
        gdvAllSubj.DataSource = new TeacherLIB().GetAllSubjectInClass(Convert.ToInt32(classgrade.CurriculumId), classgrade.Id,(long)logonUser.SchoolId);

        gdvAllSubj.DataBind();

    }

    protected void selectedSubjects()
    {
        IList<PASSIS.LIB.StudentScore> studScore = new List<PASSIS.LIB.StudentScore>();

        Int64 userId = logonUser.Id;
        Int64? schoolId = logonUser.SchoolId;
        Int64 campusId = logonUser.SchoolCampusId;
        Int64 StudentGradeId = 0L;
        Int64 StudentClassId = 0L;
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

        StudentGradeId = new ClassGradeLIB().RetrieveStudentGrade(userId, curSessionId).GradeId;
        StudentClassId = new ClassGradeLIB().RetrieveStudentGrade(userId, curSessionId).ClassId;
        Int64 sessionId = new AcademicSessionLIB().GetCurrentSessionId(schoolId);
        Int64? termId = new AcademicSessionLIB().GetCurrentTermId(schoolId);

        //DataTable dt = new DataTable();

        //dt.Columns.AddRange(new DataColumn[8] { new DataColumn("StudentFullName"), new DataColumn("AdmissionNumber"),
        //    new DataColumn("Subject"), new DataColumn("MarkObtainable"), new DataColumn("MarkObtained"),
        //    new DataColumn("Percentage Score"), new DataColumn("Exam Percentage Score"), new DataColumn("Final Score") });

        IList<PASSIS.LIB.StudentScore> getStudExamScore = new List<PASSIS.LIB.StudentScore>();
        IList<PASSIS.LIB.StudentScoreRepository> getStudCAScore = new List<PASSIS.LIB.StudentScoreRepository>();

        foreach (GridViewRow row in gdvAllSubj.Rows)
        {
            //int viewIndex = Convert.ToInt32(ViewState["Index"]) + 1;

            if (row.RowType == DataControlRowType.DataRow)
            {
                bool isChecked = row.Cells[2].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                if (isChecked)
                {
                    string SubjectName = row.Cells[1].Controls.OfType<Label>().FirstOrDefault().Text;
                    PASSIS.LIB.Grade gradetech = context.Grades.FirstOrDefault(x => x.Id == Convert.ToInt64(StudentGradeId));
                    PASSIS.LIB.Subject sub = context.Subjects.FirstOrDefault(x => x.Name == SubjectName && x.ClassId == gradetech.ClassId);

                    //PASSIS.LIB.StudentScore getScoress = context.StudentScores.FirstOrDefault(x => x.SubjectId == sub.Id && x.AcademicSessionID == sessionId && x.TermId == termId
                    //&& x.StudentId == userId && x.SchoolId == schoolId && x.CampusId == campusId && x.ClassId == StudentClassId && x.GradeId == StudentGradeId);
                    if (ddlCategory.SelectedItem.Text == "Exam") {
                        PASSIS.LIB.StudentScore getScores = context.StudentScores.FirstOrDefault(x => x.SubjectId == sub.Id && x.AcademicSessionID == sessionId && x.TermId == termId
                        && x.StudentId == userId && x.SchoolId == schoolId && x.CampusId == campusId && x.ClassId == StudentClassId && x.GradeId == StudentGradeId);
                     
                        if (getScores != null)
                        {
                            getStudExamScore.Add(getScores);
                        }
                       
                        gdvExamScores.DataSource = getStudExamScore;
                        gdvExamScores.DataBind();
                        MultiView1.SetActiveView(ViewExam);
                    }
                    else if (ddlCategory.SelectedItem.Text == "CA") {
                        PASSIS.LIB.StudentScoreRepository getScores = context.StudentScoreRepositories.FirstOrDefault(x => x.SubjectId == sub.Id && x.SessionId == sessionId && x.TermId == termId
                        && x.StudentId == userId && x.SchoolId == schoolId && x.CampusId == campusId && x.ClassId == StudentClassId && x.GradeId == StudentGradeId);


                        if (getScores != null)
                        {
                            getStudCAScore.Add(getScores);
                        }

                        gdvTestScores.DataSource = getStudCAScore;
                        gdvTestScores.DataBind();
                        MultiView1.SetActiveView(ViewTestAssignment);
                    }
                   
                }
            }
        }
    }


    protected void btnViewScores_Click(object sender, EventArgs e)
    {
        selectedSubjects();
        PASSIS.LIB.User StudentFullName = context.Users.FirstOrDefault(x=>x.Id == logonUser.Id);
        
        if (ddlCategory.SelectedItem.Text == "Exam")
        {
            gdvExamScores.Visible = true;
            lblUploaded.Text = StudentFullName.StudentFullName +" EXAM SCORES";
            lblUploaded.Visible = true;
        }
        else if (ddlCategory.SelectedItem.Text == "CA")
        {
            gdvTestScores.Visible = true;
            lblUploaded.Text = StudentFullName.StudentFullName  +" CA SCORES";
            lblUploaded.Visible = true;

        }

    }

    public string SubjectName(long SubjectId)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        PASSIS.LIB.Subject subjectName = context.Subjects.FirstOrDefault(x => x.Id == SubjectId);
        return subjectName.Name;
        //if (SubjectId != 0) { return subjectName.Name; }
        //else { return ""; }

    }
    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chk = (sender as CheckBox);
            if (chk.ID == "chkAll")
            {
                foreach (GridViewRow row in gdvAllSubj.Rows)
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

    protected void gdvAllSubject_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }
}