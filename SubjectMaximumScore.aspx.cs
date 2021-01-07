using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO;
using PASSIS.LIB;

public partial class SubjectMaximumScore : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!isUserClassTeacher)
            {
                PASSIS.LIB.Grade grd = getLogonTeacherGrade;
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
                ////send a message that the logon user will not be able to view certain section of this page bcos he's not a class teacher 
            }
        }
    }

    private string ClassName(long? classId)
    {
        PASSIS.LIB.Class_Grade classGrade = context.Class_Grades.FirstOrDefault(x => x.Id == classId);
        if (classGrade != null)
        {
            return classGrade.Name;
        }
        else { return string.Empty; }
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindScoreSubject();
        lblCampusSelected.Text = ddlYear.SelectedItem.Text;
        lblCampusSelected.Visible = true;
        lblSelected.Visible = true;
    }
    protected void gdvLists_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lblErrorMsg.Visible = false;
        gdvLists.EditIndex = e.NewEditIndex;
        BindScoreSubject();
    }
    protected void gdvLists_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lblErrorMsg.Visible = false;
        gdvLists.EditIndex = -1;
        BindScoreSubject();
    }
    protected void gdvLists_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            lblErrorMsg.Visible = false;
            Label lblId = (Label)gdvLists.Rows[e.RowIndex].FindControl("lblId");
            TextBox txtScore = (TextBox)gdvLists.Rows[e.RowIndex].FindControl("txtScore");
            long Id = Convert.ToInt64(lblId.Text);
            int score = Convert.ToInt16(txtScore.Text);

            DropDownList ddlCA = (DropDownList)gdvLists.Rows[e.RowIndex].FindControl("ddlCA");
            DropDownList ddlExam = (DropDownList)gdvLists.Rows[e.RowIndex].FindControl("ddlExam");

            if (ddlCA.SelectedItem.Text.ToString() == "No" && ddlExam.SelectedItem.Text.ToString() == "No")
            {
                lblErrorMsg.Text = "CA and Exam cannot be set to NO, one must be applicable!";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }

            long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);
            long ?curTermId = new AcademicSessionLIB().GetCurrentTermId(logonUser.SchoolId);
     PASSIS.LIB.AcademicSession academicSession = context.AcademicSessions.FirstOrDefault(x => x.AcademicSessionId == curSessionId && x.AcademicTermId == curTermId && x.SchoolId == logonUser.SchoolId);

            if (academicSession != null && academicSession.IsClosed == true)
            {
                lblErrorMsg.Text = "You can't edit the maximum score, because this term has been closed for this session, Kindly contact Administrator!";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }

            if (academicSession != null && academicSession.IsLocked == true)
            {
                lblErrorMsg.Text = "You can't edit the maximum score, because this term has been locked for this session, Kindly contact Administrator!";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }


            SubjectsInSchool subSch = context.SubjectsInSchools.FirstOrDefault(x => x.Id == Id);
            subSch.MaximumScore = score;
            if (ddlCA.SelectedItem.Text.ToString() == "Yes") { subSch.CA = true; } else { subSch.CA = false; }
            if (ddlExam.SelectedItem.Text.ToString() == "Yes") { subSch.Exam = true; } else { subSch.Exam = false; }
            context.SubmitChanges();


            var examScore = from s in context.StudentScores where s.SchoolId == logonUser.SchoolId && s.CampusId == logonUser.SchoolCampusId && s.SubjectId == subSch.SubjectId && s.ExamPercentageScore != null select s;

            foreach (PASSIS.LIB.StudentScore s in examScore)
            {
                PASSIS.LIB.StudentScore objExamScore = context.StudentScores.FirstOrDefault(x => x.Id == s.Id);

                //decimal? finalScore = (objExamScore.ExamPercentageScore * score) / 100;
                decimal? finalScore = (objExamScore.ExamPercentageScore / score) * score;

                objExamScore.FinalScore = finalScore;
                objExamScore.SubjectMaxScore = score;
                context.SubmitChanges();

            }
            var testScore = from s in context.StudentScoreRepositories where s.SchoolId == logonUser.SchoolId && s.CampusId == logonUser.SchoolCampusId && s.SubjectId == subSch.SubjectId && s.CAPercentageScore != null select s;
            foreach (PASSIS.LIB.StudentScoreRepository s in testScore)
            {
                PASSIS.LIB.StudentScoreRepository objTestScore = context.StudentScoreRepositories.FirstOrDefault(x => x.ID == s.ID);

                //decimal? finalScore = (objTestScore.CAPercentageScore * score) / 100;
                decimal? finalScore = (objTestScore.CAPercentageScore / score) * score;

                objTestScore.FinalScore = finalScore;
                objTestScore.SubjectMaxScore = score;
                context.SubmitChanges();

            }

            gdvLists.EditIndex = -1;
            BindScoreSubject();
            lblErrorMsg.Text = "Updated Successfully";
            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
        }
        catch (Exception ex)
        {
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
            lblErrorMsg.Text = "Error occurred, kindly contact your administrator";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
        }

    }


    public string checkTorF(string val)
    {
        string status = string.Empty;
        if (val == "True")
        {
            status = "Yes";
        }
        else { status = "No"; }
        return status;
    }


    public void BindScoreSubject()
    {
        var subjectInClass = from s in context.SubjectsInSchools
                             where s.SchoolId == logonUser.SchoolId && s.Subject.ClassId == Convert.ToInt64(ddlYear.SelectedValue)
                             select new
                             {
                                 s.Id,
                                 s.Subject.Name,
                                 className = ClassName(s.Subject.ClassId),
                                 s.MaximumScore,
                                 s.CA,
                                 s.Exam
                             };

        gdvLists.DataSource = subjectInClass;
        gdvLists.DataBind();
    }
}