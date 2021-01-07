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


public partial class ViewScoresExtended : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!isUserClassTeacher)
            {
                PASSIS.LIB.Grade grd = getLogonTeacherGrade;
               
                ddlSession.DataSource = new ViewScoresExtended().schSession().Distinct();
                ddlSession.DataTextField = "SessionName";
                ddlSession.DataValueField = "ID";
                ddlSession.DataBind();
                ddlSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Session--", "0", true));

                ddlCategory.Items.Add(new ListItem("CA", "0"));
                ddlCategory.Items.Add(new ListItem("Exam", "1"));

                long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
                long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
                ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
                ddlYear.DataBind();

                ddlTerm.DataSource = new AcademicTermLIB().RetrieveAcademicTerm();
                ddlTerm.DataBind();
                ddlTerm.Items.Insert(0, new ListItem("--Select--", "0", true));
            }
            else
            {
               
                ddlSession.DataSource = new ViewScoresExtended().schSession().Distinct();
                ddlSession.DataTextField = "SessionName";
                ddlSession.DataValueField = "ID";
                ddlSession.DataBind();
                ddlSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Session--", "0", true));

                //ddlYear.DataSource = new PASSIS.LIB.TeacherLIB().getTeacherAllClass_Grade(logonUser.Id);
                //ddlYear.DataBind();
                long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
                long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
                ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
                ddlYear.DataBind();

                ddlCategory.Items.Add(new ListItem("CA", "0"));
                ddlCategory.Items.Add(new ListItem("Exam", "1"));

                ddlTerm.DataSource = new AcademicTermLIB().RetrieveAcademicTerm();
                ddlTerm.DataBind();
                ddlTerm.Items.Insert(0, new ListItem("--Select--", "0", true));


                ////send a message that the logon user will not be able to view certain section of this page bcos he's not a class teacher 
            }
        }
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

    PASSISLIBDataContext db = new PASSISLIBDataContext();
    protected void GetDataSource()
    {
        DataTable dt = new DataTable();
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

            PASSIS.LIB.GradeStudent gstu = db.GradeStudents.FirstOrDefault(x => x.SchoolId == schoolId && x.SchoolCampusId == campusId && x.ClassId == yearId && x.GradeId == gradeId);
            PASSIS.LIB.GradeStudent Allstudents = db.GradeStudents.FirstOrDefault(x => x.StudentId == grdstud.StudentId);
            dt.Rows.Add(Allstudents.User.StudentFullName, Allstudents.User.AdmissionNumber);
        }

        gdvViewExtendedScores.DataSource = dt;
        gdvViewExtendedScores.DataBind();
        IList<PASSIS.LIB.SubjectTeacher> getId = new TeacherLIB().getTeacherSubjects(logonUser.Id, Convert.ToInt32(ddlGrade.SelectedValue));
        PASSISLIBDataContext context = new PASSISLIBDataContext();

        // var subjects = db.ExecuteQuery<string>("select Name from Subject where ClassId = '" + Convert.ToInt64(ddlYear.SelectedValue) + "'").ToList();

        // Header implementation
        int count = 0;
        //  var subject in subjects
        ViewState["Index"] = 2;
        foreach (GridViewRow row in gdvAllSubject.Rows)
        {
            int viewIndex = Convert.ToInt32(ViewState["Index"]) + 1;
            if (row.RowType == DataControlRowType.DataRow)
            {
                bool isChecked = row.Cells[2].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                if (isChecked)
                {
                    string SubjectName = row.Cells[1].Controls.OfType<Label>().FirstOrDefault().Text;
                    DataColumn dc = new DataColumn(SubjectName.ToString());
                    dt.Columns.Add(dc);
                    gdvViewExtendedScores.DataSource = dt;
                    gdvViewExtendedScores.DataBind();
                }
            }
            count++;
        }


        foreach (GridViewRow row in gdvAllSubject.Rows)
        {
            int viewIndex = Convert.ToInt32(ViewState["Index"]) + 1;
            if (row.RowType == DataControlRowType.DataRow)
            {
                bool isChecked = row.Cells[2].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                if (isChecked)
                {
                    string SubjectName = row.Cells[1].Controls.OfType<Label>().FirstOrDefault().Text;
                    PASSIS.LIB.Grade gradetech = db.Grades.FirstOrDefault(x => x.Id == Convert.ToInt64(ddlGrade.SelectedValue));
                    PASSIS.LIB.Subject sub = db.Subjects.FirstOrDefault(x => x.Name == SubjectName && x.ClassId == gradetech.ClassId);

                    //PASSIS.LIB.StudentScoreRepository checkgetScores = db.StudentScoreRepositories.FirstOrDefault(x => x.SubjectId == sub.Id && x.SessionId == Convert.ToInt64(ddlSession.SelectedValue) && x.TermId == Convert.ToInt64(ddlTerm.SelectedValue) && x.SubjectTeacherId == logonUser.Id);
                    //if (checkgetScores != null)
                    //{
                    foreach (GridViewRow rowScores in gdvViewExtendedScores.Rows)
                    {
                        if (ddlCategory.SelectedItem.Text == "Exam")
                        {
                            PASSIS.LIB.StudentScore getScores = db.StudentScores.FirstOrDefault(x => x.SubjectId == sub.Id && x.AcademicSessionID == Convert.ToInt64(ddlSession.SelectedValue) && x.TermId == Convert.ToInt64(ddlTerm.SelectedValue) && x.AdmissionNumber == rowScores.Cells[2].Text);

                            if (getScores != null)
                            {
                                rowScores.Cells[viewIndex].Text = getScores.ExamScore.ToString();
                            }
                        }
                        if (ddlCategory.SelectedItem.Text == "CA")
                        {

                            PASSIS.LIB.StudentScoreRepository getCAscores = db.StudentScoreRepositories.FirstOrDefault(x => x.SubjectId == sub.Id && x.SessionId == Convert.ToInt64(ddlSession.SelectedValue) && x.TermId == Convert.ToInt64(ddlTerm.SelectedValue) && x.AdmissionNo == rowScores.Cells[2].Text);

                            if (getCAscores != null)
                            {
                                rowScores.Cells[viewIndex].Text = getCAscores.MarkObtained.ToString();
                            }
                        }
                    }
                    //}
                    ViewState["Index"] = viewIndex.ToString();
                }
            }
        }




        //foreach (PASSIS.LIB.SubjectTeacher subjId in getId)
        //{
        //    PASSIS.LIB.Subject reqSubject = context.Subjects.FirstOrDefault(c => c.Id == subjId.SubjectId);
        //    DataColumn dc = new DataColumn(reqSubject.Name.ToString());
        //    dt.Columns.Add(dc);
        //    count++;
        //}

        // Rows implementation here

    }
    protected void ddlGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateallSubject();
        gdvAllSubject.Visible = true;
    }

    protected void PopulateallSubject()
    {
        //ddlStudents.Items.Clear();
        //ddlStudents.DataSource = new ClassGradeLIB().RetrieveUnallocatedStudents((long)logonUser.SchoolId);
        PASSIS.LIB.Class_Grade classgrade = db.Class_Grades.FirstOrDefault(x => x.Name == ddlYear.SelectedItem.Text);
        gdvAllSubject.DataSource = new TeacherLIB().GetAllSubject(Convert.ToInt32(classgrade.CurriculumId), classgrade.Id);
        // gdvUnassignedStudents.DataSource = new ClassGradeLIB().RetrieveGradeStudents((long)logonUser.SchoolId, logonUser.SchoolCampusId, 0, 0);
        ////ddlStudents.DataSource = RetrieveUnallocatedStudents((long)logonUser.SchoolId);
        //ddlStudents.DataBind();
        gdvAllSubject.DataBind();

    }

    public void BindScore()
    {
        try
        {

            Int64 campusId = logonUser.SchoolCampusId;
            Int64 TeacherId = logonUser.Id;
            Int64 termId = Convert.ToInt64(ddlTerm.SelectedValue);
            Int64 sessionId = Convert.ToInt64(ddlSession.SelectedValue);

            GetDataSource();
        }
        catch (Exception ex)
        {
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
            lblErrorMsg.Text = "Error occurred, kindly contact your administrator";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
        }

    }
    protected void btnViewExtendedScores_Click(object sender, EventArgs e)
    {
        Label1.Visible = true;
        lblStudents.Visible = true;
        BindScore();
        btnExportToexcel.Visible = true;

        if (ddlCategory.SelectedItem.Text == "Exam")
        {
            btnExportToexcel.Text = "Export Exam Scores";
        }
        else { btnExportToexcel.Text = "Export CA Scores"; }

    }
    protected void gdvViewExtendedScores_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            Label lblId = (Label)gdvViewExtendedScores.Rows[e.RowIndex].FindControl("lblId");

            PASSISLIBDataContext context = new PASSISLIBDataContext();

            PASSIS.LIB.StudentScore _studentScore = context.StudentScores.FirstOrDefault(x => x.Id == Convert.ToInt64(lblId.Text));

            if (_studentScore != null)
            {
                // int SubjectId = Convert.ToInt32(ddlClassSubject.SelectedValue);
                Int64 schoolId = Convert.ToInt64(logonUser.SchoolId);
                Int64 CampusId = Convert.ToInt64(logonUser.SchoolCampusId);
                Int64 termId = Convert.ToInt64(ddlTerm.SelectedValue);
                Int64 sessionId = Convert.ToInt64(ddlSession.SelectedValue);
                Int64 loginUserID = Convert.ToInt64(logonUser.Id);

                TextBox txtExamScore = (TextBox)gdvViewExtendedScores.Rows[e.RowIndex].FindControl("txtExamScore");
                _studentScore.SchoolId = schoolId;
                _studentScore.CampusId = CampusId;
                _studentScore.TermId = termId;
                _studentScore.AcademicSessionID = sessionId;
                //_studentScore.SubjectId = SubjectId;
                _studentScore.SubjectTeacherId = loginUserID;

                _studentScore.ExamScore = Convert.ToInt64(txtExamScore.Text);
                context.SubmitChanges();
                gdvViewExtendedScores.EditIndex = -1;
                BindScore();
                lblErrorMsg.Text = "Updated Successfully";
                lblErrorMsg.ForeColor = System.Drawing.Color.Green;

            }
            else
            {


            }
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = "Error occurred, try again";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
        }
    }
    protected void gdvViewExtendedScores_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gdvViewExtendedScores.EditIndex = -1;

        BindScore();
    }
    protected void gdvViewExtendedScores_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gdvViewExtendedScores.EditIndex = e.NewEditIndex;
        BindScore();
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
    protected void gdvAllSubject_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void btnExportToexcel_Click(object sender, EventArgs e)
    {

        try
        {
            string FileName1 = ddlYear.SelectedItem.Text + ddlGrade.SelectedItem.Text + ddlSession.SelectedItem.Text + ddlTerm.SelectedItem.Text + "Exam_Score" + ".xls";
            string FileName = FileName1.Replace(" ", "_");

            Response.ClearContent();
            Response.AppendHeader("content-disposition", "attachment; filename=" + FileName);
            Response.ContentType = "application/vnd.ms-excel";
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(stringWriter);
            gdvViewExtendedScores.RenderControl(htw);
            Response.Write(stringWriter.ToString());
            Response.End();
        }
        catch (Exception ex)
        {

        }

    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }
}