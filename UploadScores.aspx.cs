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

public partial class UploadScores : PASSIS.LIB.Utility.BasePage
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

            ddlSession.DataSource = new UploadScores().schSession().Distinct();
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
                long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
                long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
                ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
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
        var availableGrades = new ClassGradeLIB().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, yearId);
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
        lblresltt.Visible = true;
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

        try
        {
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
            if (ddlCategory.SelectedItem.Text == "Behavioral")
            {
                Int64 yearId = 0L, gradeId = 0L, schoolId = 0L, campusId = 0L;
                yearId = Convert.ToInt64(ddlYear.SelectedValue);
                gradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                schoolId = (long)logonUser.SchoolId;
                campusId = logonUser.SchoolCampusId;
                //btnExportToexcel.Visible = true;
                gvBehavioural.DataSource = new PASSIS.LIB.ClassGradeLIB().RetrieveGradeStudents(schoolId, campusId, yearId, gradeId, curSessionId);
                gvBehavioural.DataBind();
                MultiView1.SetActiveView(ViewBehavioural);
                lblErrorMsg.Text = "Kindly insert the scores in the table below";
                lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                lblErrorMsg.Visible = true;
            }
            if (ddlCategory.SelectedItem.Text == "Extra Curricular")
            {
                Int64 yearId = 0L, gradeId = 0L, schoolId = 0L, campusId = 0L;
                yearId = Convert.ToInt64(ddlYear.SelectedValue);
                gradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                schoolId = (long)logonUser.SchoolId;
                campusId = logonUser.SchoolCampusId;
                //btnExportToexcel.Visible = true;
                gvExtraCurricular.DataSource = new PASSIS.LIB.ClassGradeLIB().RetrieveGradeStudents(schoolId, campusId, yearId, gradeId, curSessionId);
                gvExtraCurricular.DataBind();
                MultiView1.SetActiveView(ViewExtraCurricular);
                lblErrorMsg.Text = "Kindly insert the scores in the table below";
                lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                lblErrorMsg.Visible = true;
            }
            if (ddlCategory.SelectedItem.Text == "CA")
            {
                Int64 yearId = 0L, gradeId = 0L, schoolId = 0L, campusId = 0L;
                yearId = Convert.ToInt64(ddlYear.SelectedValue);
                gradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                schoolId = (long)logonUser.SchoolId;
                campusId = logonUser.SchoolCampusId;
                //btnExportToexcel.Visible = true;
                gvTestAssigment.DataSource = new PASSIS.LIB.ClassGradeLIB().RetrieveGradeStudents(schoolId, campusId, yearId, gradeId, curSessionId);
                gvTestAssigment.DataBind();
                MultiView1.SetActiveView(ViewTestAssignment);
                lblErrorMsg.Text = "Kindly insert the scores in the table below";
                lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                lblErrorMsg.Visible = true;
            }
            else if (ddlCategory.SelectedItem.Text == "Exam")
            {
                Int64 yearId = 0L, gradeId = 0L, schoolId = 0L, campusId = 0L;
                yearId = Convert.ToInt64(ddlYear.SelectedValue);
                gradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                schoolId = (long)logonUser.SchoolId;
                campusId = logonUser.SchoolCampusId;
                //btnExportToexcel.Visible = true;
                gvExam.DataSource = new PASSIS.LIB.ClassGradeLIB().RetrieveGradeStudents(schoolId, campusId, yearId, gradeId, curSessionId);
                gvExam.DataBind();
                MultiView1.SetActiveView(ViewExam);
                lblErrorMsg.Text = "Kindly insert the scores in the table below";
                lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                lblErrorMsg.Visible = true;
            }

            btnSaveScore.Visible = true;
        }
        catch (Exception ex)
        {

        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
    }

    protected void btnSaveScore_Click(object sender, EventArgs e)
    {
        try
        {
            Int64 yearId = Convert.ToInt64(ddlYear.SelectedValue);
            Int64 gradeId = Convert.ToInt64(ddlGrade.SelectedValue);
            Int64 subjectId = Convert.ToInt64(ddlClassSubject.SelectedValue);
            Int64 TeacherId = logonUser.Id;
            //Int64 departmentId = 0;
            Int64 sessionId = Convert.ToInt64(ddlSession.SelectedValue);
            Int64 termId = Convert.ToInt64(ddlTerm.SelectedValue);


       PASSIS.LIB.AcademicSession academicSession = context.AcademicSessions.FirstOrDefault(x => x.AcademicSessionId == Convert.ToInt64(ddlSession.SelectedValue) && x.AcademicTermId == Convert.ToInt64(ddlTerm.SelectedValue)
       && x.SchoolId == logonUser.SchoolId);


            if (academicSession != null && academicSession.IsClosed == true)
            {
                lblErrorMsg.Text = "";
                lblErrorMsg.Text = "This term has been closed for this session, Kindly contact Administrator!";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (academicSession != null && academicSession.IsLocked == true)
            {
                lblErrorMsg.Text = "";
                lblErrorMsg.Text = "This term has been locked for this session, Kindly contact Administrator!";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                return;
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

            if (ddlCategory.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select category";
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
            if (ddlSession.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select session";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            if (txtTotalMark.Text.Trim() == "")
            {
                lblErrorMsg.Text = "Mark Obtainable is Missing";
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
            if (ddlClassSubject.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select Subject";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            ScoreCategoryConfiguration scoreCategory = context.ScoreCategoryConfigurations.FirstOrDefault(x => x.ClassId == Convert.ToInt64(ddlYear.SelectedValue) && x.Category == ddlCategory.SelectedItem.Text && x.SessionId == sessionId && x.TermId == termId && x.SchoolId == logonUser.SchoolId && x.CampusId == logonUser.SchoolCampusId);
            if (scoreCategory == null)
            {
                lblErrorMsg.Text = "Kindly set the score category";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            PASSIS.LIB.SubjectTeacher subTeacher = context.SubjectTeachers.FirstOrDefault(x => x.SubjectId == Convert.ToInt64(ddlClassSubject.SelectedValue));
            //class id is equal to grade id


            PASSIS.LIB.Subject subject = context.Subjects.FirstOrDefault(x => x.Id == subjectId);
            PASSIS.LIB.SubjectsInSchool subSch = context.SubjectsInSchools.FirstOrDefault(x => x.SubjectId == subject.Id && x.SchoolId == logonUser.SchoolId);
            //if (subject != null)
            //{
            //    departmentId = Convert.ToInt64(subject.DepartmentId);
            //}
            if (ddlCategory.SelectedItem.Text == "Exam")
            {
                ScoreSubCategoryConfiguration subCategory = context.ScoreSubCategoryConfigurations.FirstOrDefault(x => x.Id == Convert.ToInt64(ddlSubCategory.SelectedValue));
                if (ddlClassSubject.SelectedIndex == 0)
                {
                    lblErrorMsg.Text = "Kindly select subject";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                    return;
                }
                if (txtCode.Text == "")
                {
                    lblErrorMsg.Text = "Kindly enter the code";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                    return;
                }
                string templateCode = yearId.ToString() + gradeId.ToString() + subjectId.ToString() + TeacherId.ToString() + sessionId.ToString() + termId.ToString() + scoreCategory.Id.ToString() + subCategory.Id;
                TestAssignmentScoreTemplate TestList = context.TestAssignmentScoreTemplates.FirstOrDefault(x => x.TemplateCode == templateCode && x.HasSubmitted == true);

                //if (TestList != null)
                //{
                //    lblErrorMsg.Text = "Kindly confirm you are uploading against submitted scores";
                //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //    lblErrorMsg.Visible = true;
                //}
                //else
                //{
                TestAssignmentScoreTemplate theTempObj = context.TestAssignmentScoreTemplates.FirstOrDefault(x => x.TemplateCode == templateCode);
                if (theTempObj == null)
                {
                    TestAssignmentScoreTemplate newTempObj = new TestAssignmentScoreTemplate();
                    newTempObj.ClassId = gradeId;
                    newTempObj.DateGenerated = DateTime.Now;
                    newTempObj.TemplateCode = templateCode;
                    newTempObj.SubjectId = subjectId;
                    newTempObj.TeacherId = TeacherId;
                    newTempObj.SchoolId = logonUser.SchoolId;
                    newTempObj.CampusId = logonUser.SchoolCampusId;
                    newTempObj.HasSubmitted = false;
                    new ScoresheetLIB().SaveTestAssignmentTemplate(newTempObj);
                }
                IList<PASSIS.LIB.StudentScore> scoreList = new List<PASSIS.LIB.StudentScore>();
                foreach (GridViewRow row in gvExam.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        PASSIS.LIB.StudentScore score = new PASSIS.LIB.StudentScore();
                        TestAssignmentScoreTemplate tempObject = context.TestAssignmentScoreTemplates.FirstOrDefault(x => x.TemplateCode == templateCode);

                        Label lblAdmNo = (Label)gvExam.Rows[row.RowIndex].FindControl("lblAdmNo");
                        Label lblId = (Label)gvExam.Rows[row.RowIndex].FindControl("lblId");
                        TextBox txtExamScore = (TextBox)gvExam.Rows[row.RowIndex].FindControl("txtExamScore");
                        TextBox txtRemark = (TextBox)gvExam.Rows[row.RowIndex].FindControl("txtRemark");
                        string examScore = txtExamScore.Text.Trim();
                        long? studentId = Convert.ToInt64(lblId.Text.ToString().Trim());
                        if (examScore != "")
                        {
                            StudentScoreTransaction trans = context.StudentScoreTransactions.FirstOrDefault(x => x.TemplateId == tempObject.ID && x.Code == Convert.ToInt16(txtCode.Text) && x.StudentId == studentId);
                            if (Convert.ToInt16(txtCode.Text) < 1)
                            {
                                lblErrorMsg.Text = "Code should not be lesser than 1";
                                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                                lblErrorMsg.Visible = true;
                                return;
                            }

                            if (trans != null)
                            {
                                lblErrorMsg.Text = "Code has been used";
                                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                                lblErrorMsg.Visible = true;
                                continue;
                            }
                            else
                            {
                                StudentScoreTransaction transs = context.StudentScoreTransactions.FirstOrDefault(x => x.TemplateId == tempObject.ID && x.Code == Convert.ToInt16(txtCode.Text) - 1 && x.StudentId == studentId);
                                if (transs == null && Convert.ToInt16(txtCode.Text) - 1 != 0)
                                {
                                    lblErrorMsg.Text = "Enter lesser number for the code";
                                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                                    lblErrorMsg.Visible = true;
                                    return;
                                }
                            }

                            decimal scores = Convert.ToDecimal(examScore);

                            StudentScoreTransaction scoreTransaction = new StudentScoreTransaction();
                            scoreTransaction.AdmissionNumber = lblAdmNo.Text;
                            scoreTransaction.StudentId = Convert.ToInt64(lblId.Text.ToString().Trim());
                            scoreTransaction.ExamScoreObtainable = Convert.ToInt16(txtTotalMark.Text.Trim());
                            scoreTransaction.ExamScore = Convert.ToDecimal(scores);
                            scoreTransaction.TermId = termId;
                            scoreTransaction.AcademicSessionID = sessionId;
                            scoreTransaction.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
                            scoreTransaction.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                            scoreTransaction.SubjectId = Convert.ToInt16(subjectId);
                            if (subSch.DepartmentId != null)
                            {
                                score.DepartmentId = Convert.ToInt64(subSch.DepartmentId);
                            }
                            //score.Percentage = subCategory.Percentage;
                            //score.PercentageScore = percentageScore;
                            //score.ExamPercentage = scoreCategory.Percentage;
                            //score.ExamPercentageScore = ExamPercentageScore;
                            scoreTransaction.CategoryId = scoreCategory.Id;
                            scoreTransaction.SubCategoryId = subCategory.Id;
                            //score.DepartmentId = departmentId;
                            scoreTransaction.CampusId = logonUser.SchoolCampusId;
                            scoreTransaction.SchoolId = logonUser.SchoolId;
                            scoreTransaction.SubjectTeacherId = Convert.ToInt64(subTeacher.TeacherId);
                            scoreTransaction.UploadedById = logonUser.Id;
                            scoreTransaction.TemplateId = tempObject.ID;
                            scoreTransaction.Remark = txtRemark.Text.Trim();
                            scoreTransaction.Description = txtDescription.Text.Trim();
                            scoreTransaction.Date = DateTime.Now;
                            scoreTransaction.Code = Convert.ToInt16(txtCode.Text);
                            scoreTransaction.StatusCode = "I";
                            scoreTransaction.IsCancelled = false;
                            context.StudentScoreTransactions.InsertOnSubmit(scoreTransaction);
                            context.SubmitChanges();



                            PASSIS.LIB.StudentScore getScoreObj = context.StudentScores.FirstOrDefault(x => x.AdmissionNumber == lblAdmNo.Text.Trim() && x.StudentId == Convert.ToInt64(lblId.Text.ToString().Trim()) && x.TemplateId == tempObject.ID);
                            if (getScoreObj == null)
                            {

                                decimal totalScore = Convert.ToDecimal(txtTotalMark.Text.Trim());
                                decimal exScore = scores / totalScore;
                                int examPercentage = Convert.ToInt16(subCategory.Percentage);
                                //int examPercentage = Convert.ToInt16(scoreCategory.Percentage);
                                decimal percentageScore = exScore * examPercentage;
                                decimal ExamPercentageScore = Convert.ToDecimal((percentageScore / 100) * scoreCategory.Percentage);
                                decimal? finalScore = (ExamPercentageScore * subSch.MaximumScore) / 100;

                                score.AdmissionNumber = lblAdmNo.Text;
                                score.StudentId = Convert.ToInt64(lblId.Text.ToString().Trim());
                                score.ExamScoreObtainable = Convert.ToInt16(totalScore);
                                score.ExamScore = Convert.ToDecimal(scores);
                                score.TermId = termId;
                                score.AcademicSessionID = sessionId;
                                score.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
                                score.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                                score.SubjectId = Convert.ToInt16(subjectId);
                                if (subSch.DepartmentId != null)
                                {
                                    score.DepartmentId = Convert.ToInt64(subSch.DepartmentId);
                                }
                                score.Percentage = subCategory.Percentage;
                                score.PercentageScore = percentageScore;
                                score.ExamPercentage = scoreCategory.Percentage;
                                score.ExamPercentageScore = ExamPercentageScore;
                                score.SubjectMaxScore = subSch.MaximumScore;
                                score.FinalScore = finalScore;
                                score.CategoryId = scoreCategory.Id;
                                score.SubCategoryId = subCategory.Id;
                                //score.DepartmentId = departmentId;
                                score.CampusId = logonUser.SchoolCampusId;
                                score.SchoolId = logonUser.SchoolId;
                                score.SubjectTeacherId = Convert.ToInt64(subTeacher.TeacherId);
                                score.UploadedById = logonUser.Id;
                                score.TemplateId = tempObject.ID;
                                score.Remark = txtRemark.Text.Trim();
                                score.Count = 1;
                                //score.Description = txtDescription.Text.Trim();
                                score.Date = DateTime.Now;
                                context.StudentScores.InsertOnSubmit(score);
                                context.SubmitChanges();
                                //scoreList.Add(score);
                                //new ScoresheetLIB().SaveStudentScoreDetail(scoreList);
                            }
                            else
                            {
                                decimal totalScore = Convert.ToDecimal(txtTotalMark.Text.Trim()) + Convert.ToDecimal(getScoreObj.ExamScoreObtainable);
                                decimal newScore = scores + Convert.ToDecimal(getScoreObj.ExamScore);
                                decimal tsScore = newScore / totalScore;
                                int examPercentage = Convert.ToInt16(subCategory.Percentage);
                                decimal percentageScore = tsScore * examPercentage;
                                decimal ExamPercentageScore = Convert.ToDecimal((percentageScore / 100) * scoreCategory.Percentage);
                                decimal? finalScore = (ExamPercentageScore * subSch.MaximumScore) / 100;

                                getScoreObj.AdmissionNumber = lblAdmNo.Text;
                                getScoreObj.StudentId = Convert.ToInt64(lblId.Text.ToString().Trim());
                                getScoreObj.ExamScoreObtainable = Convert.ToInt16(totalScore);
                                getScoreObj.ExamScore = Convert.ToDecimal(newScore);
                                getScoreObj.TermId = termId;
                                getScoreObj.AcademicSessionID = sessionId;
                                getScoreObj.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
                                getScoreObj.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                                getScoreObj.SubjectId = Convert.ToInt16(subjectId);
                                getScoreObj.Percentage = subCategory.Percentage;
                                getScoreObj.PercentageScore = percentageScore;
                                getScoreObj.ExamPercentage = scoreCategory.Percentage;
                                getScoreObj.ExamPercentageScore = ExamPercentageScore;
                                getScoreObj.SubjectMaxScore = subSch.MaximumScore;
                                getScoreObj.FinalScore = finalScore;
                                getScoreObj.CategoryId = scoreCategory.Id;
                                getScoreObj.SubCategoryId = subCategory.Id;
                                if (subSch.DepartmentId != null)
                                {
                                    score.DepartmentId = Convert.ToInt64(subSch.DepartmentId);
                                }
                                getScoreObj.CampusId = logonUser.SchoolCampusId;
                                getScoreObj.SchoolId = logonUser.SchoolId;
                                getScoreObj.SubjectTeacherId = Convert.ToInt64(subTeacher.TeacherId);
                                getScoreObj.UploadedById = logonUser.Id;
                                getScoreObj.TemplateId = tempObject.ID;
                                getScoreObj.Remark = txtRemark.Text.Trim();
                                getScoreObj.Count = getScoreObj.Count + 1;
                                //getScoreObj.Description = txtDescription.Text.Trim();
                                getScoreObj.Date = DateTime.Now;
                                context.SubmitChanges();
                                //scoreList.Add(getScoreObj);

                            }
                            scoreTransaction.StatusCode = "C";
                            context.SubmitChanges();
                        }
                    }
                }

                TestAssignmentScoreTemplate tempObj = context.TestAssignmentScoreTemplates.FirstOrDefault(x => x.TemplateCode == templateCode);
                tempObj.HasSubmitted = true;
                context.SubmitChanges();
                //lblErrorMsg.Text = "Uploaded Successfully, copy your Score Code for Reference: " + templateCode;
                lblErrorMsg.Text = "Uploaded Successfully";
                lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                lblErrorMsg.Visible = true;
                //}
            }
            if (ddlCategory.SelectedItem.Text == "CA")
            {
                ScoreSubCategoryConfiguration subCategory = context.ScoreSubCategoryConfigurations.FirstOrDefault(x => x.Id == Convert.ToInt64(ddlSubCategory.SelectedValue));

                if (ddlClassSubject.SelectedIndex == 0)
                {
                    lblErrorMsg.Text = "Kindly select subject";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                    return;
                }
                if (txtCode.Text == "")
                {
                    lblErrorMsg.Text = "Kindly enter the code";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                    return;
                }

                string templateCode = yearId.ToString() + gradeId.ToString() + subjectId.ToString() + TeacherId.ToString() + sessionId.ToString() + termId.ToString() + scoreCategory.Id.ToString() + subCategory.Id;
                TestAssignmentScoreTemplate TestList = context.TestAssignmentScoreTemplates.FirstOrDefault(x => x.TemplateCode == templateCode && x.HasSubmitted == true);
                //TestAssignmentScoreTemplate TestList = new ScoresheetLIB().GetTemplateList(classId, subjectId, txtCode.Text.Trim());

                //if (TestList != null)
                //{
                //    lblErrorMsg.Text = "Kindly confirm you are uploading against submitted scores ";
                //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //    lblErrorMsg.Visible = true;
                //}
                //else
                //{
                TestAssignmentScoreTemplate theTempObj = context.TestAssignmentScoreTemplates.FirstOrDefault(x => x.TemplateCode == templateCode);
                if (theTempObj == null)
                {
                    TestAssignmentScoreTemplate newTempObj = new TestAssignmentScoreTemplate();
                    newTempObj.ClassId = gradeId;
                    newTempObj.DateGenerated = DateTime.Now;
                    newTempObj.TemplateCode = templateCode;
                    newTempObj.SubjectId = subjectId;
                    newTempObj.TeacherId = TeacherId;
                    newTempObj.SchoolId = logonUser.SchoolId;
                    newTempObj.CampusId = logonUser.SchoolCampusId;
                    newTempObj.HasSubmitted = false;
                    new ScoresheetLIB().SaveTestAssignmentTemplate(newTempObj);
                }


                if (subCategory == null)
                {
                    lblErrorMsg.Text = "Kindly set the score sub category";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                    return;
                }
                IList<StudentScoreRepository> scoreList = new List<StudentScoreRepository>();
                foreach (GridViewRow row in gvTestAssigment.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        StudentScoreRepository score = new StudentScoreRepository();
                        TestAssignmentScoreTemplate tempObject = context.TestAssignmentScoreTemplates.FirstOrDefault(x => x.TemplateCode == templateCode);

                        Label lblAdmNo = (Label)gvTestAssigment.Rows[row.RowIndex].FindControl("lblAdmNo");
                        Label lblId = (Label)gvTestAssigment.Rows[row.RowIndex].FindControl("lblId");
                        //decimal testScore = Convert.ToDecimal(row.Cells[3].FindControl("txtTestScore").Text.Trim());
                        TextBox txtTestScore = (TextBox)gvTestAssigment.Rows[row.RowIndex].FindControl("txtTestScore");
                        TextBox txtRemark = (TextBox)gvTestAssigment.Rows[row.RowIndex].FindControl("txtRemark");
                        string testScore = txtTestScore.Text.Trim();
                        long? studentId = Convert.ToInt64(lblId.Text.ToString().Trim());
                        string admissionNumber = lblAdmNo.Text.Trim();
                        long? templateID = tempObject.ID;

                        if (testScore != "")
                        {
                            StudentScoreRepositoryTransaction trans = context.StudentScoreRepositoryTransactions.FirstOrDefault(x => x.TemplateId == templateID && x.Code == Convert.ToInt16(txtCode.Text) && x.StudentId == studentId);
                            if (Convert.ToInt16(txtCode.Text) < 1)
                            {
                                lblErrorMsg.Text = "Code should not be lesser than 1";
                                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                                lblErrorMsg.Visible = true;
                                return;
                            }

                            if (trans != null)
                            {
                                lblErrorMsg.Text = "Code has been used";
                                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                                lblErrorMsg.Visible = true;
                                continue;
                            }
                            else
                            {
                                StudentScoreRepositoryTransaction transs = context.StudentScoreRepositoryTransactions.FirstOrDefault(x => x.TemplateId == templateID && x.Code == Convert.ToInt16(txtCode.Text) - 1 && x.StudentId == studentId);
                                if (transs == null && Convert.ToInt16(txtCode.Text) - 1 != 0)
                                {
                                    lblErrorMsg.Text = "Enter lesser number for the code";
                                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                                    lblErrorMsg.Visible = true;
                                    return;
                                }
                            }

                            decimal scores = Convert.ToDecimal(txtTestScore.Text.Trim());

                            StudentScoreRepositoryTransaction newTrnsactionObj = new StudentScoreRepositoryTransaction();
                            newTrnsactionObj.AdmissionNo = lblAdmNo.Text;
                            newTrnsactionObj.StudentId = Convert.ToInt64(lblId.Text.ToString().Trim());
                            newTrnsactionObj.MarkObtainable = Convert.ToInt16(txtTotalMark.Text.Trim());
                            newTrnsactionObj.MarkObtained = Convert.ToDecimal(scores);
                            newTrnsactionObj.TermId = termId;
                            newTrnsactionObj.SessionId = sessionId;
                            newTrnsactionObj.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
                            newTrnsactionObj.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                            newTrnsactionObj.SubjectId = Convert.ToInt16(subjectId);
                            if (subSch.DepartmentId != null)
                            {
                                score.DepartmentId = Convert.ToInt64(subSch.DepartmentId);
                            }
                            newTrnsactionObj.CategoryId = scoreCategory.Id;
                            newTrnsactionObj.SubCategoryId = subCategory.Id;
                            //score.DepartmentId = departmentId;
                            newTrnsactionObj.CampusId = logonUser.SchoolCampusId;
                            newTrnsactionObj.SchoolId = logonUser.SchoolId; 
                            newTrnsactionObj.SubjectTeacherId = Convert.ToInt64(subTeacher.TeacherId);
                            newTrnsactionObj.UploadedById = logonUser.Id;
                            newTrnsactionObj.TemplateId = tempObject.ID;
                            newTrnsactionObj.Remark = txtRemark.Text.Trim();
                            newTrnsactionObj.Description = txtDescription.Text.Trim();
                            newTrnsactionObj.Date = DateTime.Now;
                            newTrnsactionObj.Code = Convert.ToInt16(txtCode.Text);
                            newTrnsactionObj.StatusCode = "I";
                            newTrnsactionObj.IsCancelled = false;
                            //score.TestAssigenmentBroadSheetTemplateID = TestAssigenmentBroadSheetTemplateID;
                            context.StudentScoreRepositoryTransactions.InsertOnSubmit(newTrnsactionObj);
                            context.SubmitChanges();



                            StudentScoreRepository getScoreObj = context.StudentScoreRepositories.FirstOrDefault(x => x.AdmissionNo == admissionNumber && x.StudentId == studentId && x.TemplateId == templateID);
                            if (getScoreObj == null)
                            {
                                //Calculating the percentage for the first entry

                                decimal totalScore = Convert.ToDecimal(txtTotalMark.Text.Trim());
                                decimal tsScore = scores / totalScore;
                                int testPercentage = Convert.ToInt16(subCategory.Percentage);
                                decimal percentageScore = tsScore * testPercentage;
                                decimal CAPercentageScore = Convert.ToDecimal((percentageScore / 100) * scoreCategory.Percentage);
                                decimal? finalScore = (CAPercentageScore * subSch.MaximumScore) / 100;

                                score.AdmissionNo = lblAdmNo.Text;
                                score.StudentId = Convert.ToInt64(lblId.Text.ToString().Trim());
                                score.MarkObtainable = Convert.ToInt16(txtTotalMark.Text.Trim());
                                score.MarkObtained = Convert.ToDecimal(scores);
                                score.TermId = termId;
                                score.SessionId = sessionId;
                                score.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
                                score.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                                score.SubjectId = Convert.ToInt16(subjectId);
                                if (subSch.DepartmentId != null)
                                {
                                    score.DepartmentId = Convert.ToInt64(subSch.DepartmentId);
                                }
                                score.Percentage = subCategory.Percentage;
                                score.PercentageScore = percentageScore;
                                score.CAPercentage = scoreCategory.Percentage;
                                score.CAPercentageScore = CAPercentageScore;
                                score.SubjectMaxScore = subSch.MaximumScore;
                                score.FinalScore = finalScore;
                                score.CategoryId = scoreCategory.Id;
                                score.SubCategoryId = subCategory.Id;
                                //score.DepartmentId = departmentId;
                                score.CampusId = logonUser.SchoolCampusId;
                                score.SchoolId = logonUser.SchoolId;
                                score.SubjectTeacherId = Convert.ToInt64(subTeacher.TeacherId);
                                score.UploadedById = logonUser.Id;
                                score.TemplateId = tempObject.ID;
                                score.Remark = txtRemark.Text.Trim();
                                score.Count = 1;
                                //score.Description = txtDescription.Text.Trim();
                                score.Date = DateTime.Now;
                                //score.TestAssigenmentBroadSheetTemplateID = TestAssigenmentBroadSheetTemplateID;
                                context.StudentScoreRepositories.InsertOnSubmit(score);
                                context.SubmitChanges();
                                //scoreList.Add(score);
                                //new ScoresheetLIB().SaveStudentTestAssignmentScore(scoreList);
                            }
                            else
                            {
                                //decimal totalScore = Convert.ToDecimal(txtTotalMark.Text.Trim());
                                decimal totalScore = Convert.ToDecimal(txtTotalMark.Text.Trim()) + Convert.ToDecimal(getScoreObj.MarkObtainable);
                                decimal newScore = scores + Convert.ToDecimal(getScoreObj.MarkObtained);
                                decimal tsScore = newScore / totalScore;
                                int testPercentage = Convert.ToInt16(subCategory.Percentage);
                                decimal percentageScore = tsScore * testPercentage;
                                decimal CAPercentageScore = Convert.ToDecimal((percentageScore / 100) * scoreCategory.Percentage);
                                decimal? finalScore = (CAPercentageScore * subSch.MaximumScore) / 100;

                                getScoreObj.AdmissionNo = lblAdmNo.Text;
                                getScoreObj.StudentId = Convert.ToInt64(lblId.Text.ToString().Trim());
                                getScoreObj.MarkObtainable = Convert.ToInt16(totalScore);
                                getScoreObj.MarkObtained = Convert.ToDecimal(newScore);
                                getScoreObj.TermId = termId;
                                getScoreObj.SessionId = sessionId;
                                getScoreObj.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
                                getScoreObj.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                                getScoreObj.SubjectId = Convert.ToInt16(subjectId);
                                if (subSch.DepartmentId != null)
                                {
                                    getScoreObj.DepartmentId = Convert.ToInt64(subSch.DepartmentId);
                                }
                                getScoreObj.Percentage = subCategory.Percentage;
                                getScoreObj.PercentageScore = percentageScore;
                                getScoreObj.CAPercentage = scoreCategory.Percentage;
                                getScoreObj.CAPercentageScore = CAPercentageScore;
                                getScoreObj.SubjectMaxScore = subSch.MaximumScore;
                                getScoreObj.FinalScore = finalScore;
                                getScoreObj.CategoryId = scoreCategory.Id;
                                getScoreObj.SubCategoryId = subCategory.Id;
                                getScoreObj.CampusId = logonUser.SchoolCampusId;
                                getScoreObj.SchoolId = logonUser.SchoolId;
                                getScoreObj.SubjectTeacherId = Convert.ToInt64(subTeacher.TeacherId);
                                getScoreObj.UploadedById = logonUser.Id;
                                getScoreObj.TemplateId = tempObject.ID;
                                getScoreObj.Remark = txtRemark.Text.Trim();
                                //getScoreObj.Description = txtDescription.Text.Trim();
                                getScoreObj.Count = getScoreObj.Count + 1;
                                getScoreObj.Date = DateTime.Now;
                                //score.TestAssigenmentBroadSheetTemplateID = TestAssigenmentBroadSheetTemplateID;
                                //scoreList.Add(getScoreObj);
                                context.SubmitChanges();
                            }
                            newTrnsactionObj.StatusCode = "C";
                            context.SubmitChanges();
                        }
                    }
                }

                TestAssignmentScoreTemplate tempObj = context.TestAssignmentScoreTemplates.FirstOrDefault(x => x.TemplateCode == templateCode);
                tempObj.HasSubmitted = true;
                context.SubmitChanges();
                //new ScoresheetLIB().SaveTestAssignmentTemplate(newTempObj);
                //lblErrorMsg.Text = "Uploaded Successfully, copy your Score Code for Reference: " + templateCode;
                lblErrorMsg.Text = "Uploaded Successfully";
                lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                lblErrorMsg.Visible = true;
                //}
            }
            else if (ddlCategory.SelectedItem.Text == "Behavioral")
            {
                ScoreSubCategoryConfiguration subCategory = context.ScoreSubCategoryConfigurations.FirstOrDefault(x => x.Id == Convert.ToInt64(ddlSubCategory.SelectedValue));

                string templateCode = yearId.ToString() + gradeId.ToString() + subjectId.ToString() + TeacherId.ToString() + sessionId.ToString() + termId.ToString() + scoreCategory.Id.ToString() + subCategory.Id.ToString();
                TestAssignmentScoreTemplate TestList = context.TestAssignmentScoreTemplates.FirstOrDefault(x => x.TemplateCode == templateCode && x.HasSubmitted == true);

                //if (TestList != null)
                //{
                //    lblErrorMsg.Text = "Kindly confirm you are uploading against submitted scores";
                //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //    lblErrorMsg.Visible = true;
                //}
                //else
                //{
                TestAssignmentScoreTemplate theTempObj = context.TestAssignmentScoreTemplates.FirstOrDefault(x => x.TemplateCode == templateCode);
                if (theTempObj == null)
                {
                    TestAssignmentScoreTemplate newTempObj = new TestAssignmentScoreTemplate();
                    newTempObj.ClassId = gradeId;
                    newTempObj.DateGenerated = DateTime.Now;
                    newTempObj.TemplateCode = templateCode;
                    newTempObj.SubjectId = subjectId;
                    newTempObj.TeacherId = TeacherId;
                    newTempObj.SchoolId = logonUser.SchoolId;
                    newTempObj.CampusId = logonUser.SchoolCampusId;
                    newTempObj.HasSubmitted = false;
                    new ScoresheetLIB().SaveTestAssignmentTemplate(newTempObj);
                }
                IList<PASSIS.LIB.StudentScoreBehavioral> scoreList = new List<PASSIS.LIB.StudentScoreBehavioral>();
                foreach (GridViewRow row in gvBehavioural.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        PASSIS.LIB.StudentScoreBehavioral score = new PASSIS.LIB.StudentScoreBehavioral();
                        TestAssignmentScoreTemplate tempObject = context.TestAssignmentScoreTemplates.FirstOrDefault(x => x.TemplateCode == templateCode);

                        Label lblAdmNo = (Label)gvBehavioural.Rows[row.RowIndex].FindControl("lblAdmNo");
                        Label lblId = (Label)gvBehavioural.Rows[row.RowIndex].FindControl("lblId");
                        TextBox txtScore = (TextBox)gvBehavioural.Rows[row.RowIndex].FindControl("txtScore");
                        string Score = txtScore.Text.Trim();
                        if (Convert.ToDecimal(txtTotalMark.Text.Trim()) > scoreCategory.Range || Convert.ToDecimal(txtTotalMark.Text.Trim()) < 0)
                        {
                            lblErrorMsg.Text = "Mark obtainable is greater or lesser than the configured range";
                            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                            lblErrorMsg.Visible = true;
                            return;
                        }
                        if (Score != "")
                        {

                            if (Convert.ToDecimal(txtScore.Text.Trim()) > scoreCategory.Range || Convert.ToDecimal(txtScore.Text.Trim()) < 0)
                            {
                                lblErrorMsg.Text = "Behavioral Score is greater or lesser than the configured range";
                                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                                lblErrorMsg.Visible = true;
                                return;
                            }

                            decimal scores = Convert.ToDecimal(txtScore.Text.Trim()); ;
                            decimal totalScore = Convert.ToDecimal(txtTotalMark.Text.Trim());
                            decimal tsScore = scores / totalScore;
                            int testPercentage = Convert.ToInt16(subCategory.Percentage);
                            decimal percentageScore = tsScore * testPercentage;
                            decimal CAPercentageScore = Convert.ToDecimal((percentageScore / 100) * scoreCategory.Percentage);
                            PASSIS.LIB.StudentScoreBehavioral getScoreObj = context.StudentScoreBehaviorals.FirstOrDefault(x => x.AdmissionNo == lblAdmNo.Text.Trim() && x.StudentId == Convert.ToInt64(lblId.Text.ToString().Trim()) && x.TemplateId == tempObject.ID);
                            if (getScoreObj == null)
                            {
                                score.AdmissionNo = lblAdmNo.Text;
                                score.StudentId = Convert.ToInt64(lblId.Text.ToString().Trim());
                                score.MarkObtainable = Convert.ToInt16(txtTotalMark.Text.Trim());
                                score.MarkObtained = Convert.ToDecimal(scores);
                                score.TermId = termId;
                                score.SessionId = sessionId;
                                score.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
                                score.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                                //score.SubjectId = Convert.ToInt16(subjectId);
                                score.Percentage = subCategory.Percentage;
                                score.PercentageScore = percentageScore;
                                score.CAPercentage = scoreCategory.Percentage;
                                score.CAPercentageScore = CAPercentageScore;
                                score.CategoryId = scoreCategory.Id;
                                score.SubCategoryId = subCategory.Id;
                                //score.DepartmentId = departmentId;
                                score.CampusId = logonUser.SchoolCampusId;
                                score.SchoolId = logonUser.SchoolId;
                                //score.SubjectTeacherId = Convert.ToInt64(subTeacher.TeacherId);
                                score.UploadedById = logonUser.Id;
                                score.TemplateId = tempObject.ID;
                                score.Description = txtDescription.Text.Trim();
                                score.Date = DateTime.Now;
                                //score.TestAssigenmentBroadSheetTemplateID = TestAssigenmentBroadSheetTemplateID;
                                context.StudentScoreBehaviorals.InsertOnSubmit(score);
                                context.SubmitChanges();
                            }
                            else
                            {
                                getScoreObj.AdmissionNo = lblAdmNo.Text;
                                getScoreObj.StudentId = Convert.ToInt64(lblId.Text.ToString().Trim());
                                getScoreObj.MarkObtainable = Convert.ToInt16(txtTotalMark.Text.Trim());
                                getScoreObj.MarkObtained = Convert.ToDecimal(scores);
                                getScoreObj.TermId = termId;
                                getScoreObj.SessionId = sessionId;
                                getScoreObj.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
                                getScoreObj.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                                //score.SubjectId = Convert.ToInt16(subjectId);
                                getScoreObj.Percentage = subCategory.Percentage;
                                getScoreObj.PercentageScore = percentageScore;
                                getScoreObj.CAPercentage = scoreCategory.Percentage;
                                getScoreObj.CAPercentageScore = CAPercentageScore;
                                getScoreObj.CategoryId = scoreCategory.Id;
                                getScoreObj.SubCategoryId = subCategory.Id;
                                //score.DepartmentId = departmentId;
                                getScoreObj.CampusId = logonUser.SchoolCampusId;
                                getScoreObj.SchoolId = logonUser.SchoolId;
                                //score.SubjectTeacherId = Convert.ToInt64(subTeacher.TeacherId);
                                getScoreObj.UploadedById = logonUser.Id;
                                getScoreObj.TemplateId = tempObject.ID;
                                getScoreObj.Description = txtDescription.Text.Trim();
                                getScoreObj.Date = DateTime.Now;
                                context.SubmitChanges();
                            }
                        }
                    }
                }
                //new ScoresheetLIB().SaveStudentScoreDetail(scoreList);
                TestAssignmentScoreTemplate tempObj = context.TestAssignmentScoreTemplates.FirstOrDefault(x => x.TemplateCode == templateCode);
                tempObj.HasSubmitted = true;
                context.SubmitChanges();
                //new ScoresheetLIB().SaveTestAssignmentTemplate(newTempObj);
                lblErrorMsg.Text = "Uploaded Successfully";
                lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                lblErrorMsg.Visible = true;
                //}
            }
            else if (ddlCategory.SelectedItem.Text == "Extra Curricular")
            {
                ScoreSubCategoryConfiguration subCategory = context.ScoreSubCategoryConfigurations.FirstOrDefault(x => x.Id == Convert.ToInt64(ddlSubCategory.SelectedValue));

                string templateCode = yearId.ToString() + gradeId.ToString() + subjectId.ToString() + TeacherId.ToString() + sessionId.ToString() + termId.ToString() + scoreCategory.Id.ToString() + subCategory.Id.ToString();
                TestAssignmentScoreTemplate TestList = context.TestAssignmentScoreTemplates.FirstOrDefault(x => x.TemplateCode == templateCode && x.HasSubmitted == true);

                //if (TestList != null)
                //{
                //    lblErrorMsg.Text = "Kindly confirm you are uploading against submitted scores";
                //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //    lblErrorMsg.Visible = true;
                //}
                //else
                //{
                TestAssignmentScoreTemplate theTempObj = context.TestAssignmentScoreTemplates.FirstOrDefault(x => x.TemplateCode == templateCode);
                if (theTempObj == null)
                {
                    TestAssignmentScoreTemplate newTempObj = new TestAssignmentScoreTemplate();
                    newTempObj.ClassId = gradeId;
                    newTempObj.DateGenerated = DateTime.Now;
                    newTempObj.TemplateCode = templateCode;
                    newTempObj.SubjectId = subjectId;
                    newTempObj.TeacherId = TeacherId;
                    newTempObj.SchoolId = logonUser.SchoolId;
                    newTempObj.CampusId = logonUser.SchoolCampusId;
                    newTempObj.HasSubmitted = false;
                    new ScoresheetLIB().SaveTestAssignmentTemplate(newTempObj);
                }
                IList<PASSIS.LIB.StudentScoreExtraCurricular> scoreList = new List<PASSIS.LIB.StudentScoreExtraCurricular>();
                foreach (GridViewRow row in gvExtraCurricular.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        PASSIS.LIB.StudentScoreExtraCurricular score = new PASSIS.LIB.StudentScoreExtraCurricular();
                        TestAssignmentScoreTemplate tempObject = context.TestAssignmentScoreTemplates.FirstOrDefault(x => x.TemplateCode == templateCode);

                        Label lblAdmNo = (Label)gvExtraCurricular.Rows[row.RowIndex].FindControl("lblAdmNo");
                        Label lblId = (Label)gvExtraCurricular.Rows[row.RowIndex].FindControl("lblId");
                        TextBox txtScore = (TextBox)gvExtraCurricular.Rows[row.RowIndex].FindControl("txtScore");
                        string Score = txtScore.Text.Trim();
                        if (Convert.ToDecimal(txtTotalMark.Text.Trim()) > scoreCategory.Range || Convert.ToDecimal(txtTotalMark.Text.Trim()) < 0)
                        {
                            lblErrorMsg.Text = "Mark obtainable is greater or lesser than the configured range";
                            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                            lblErrorMsg.Visible = true;
                            return;
                        }
                        if (Score != "")
                        {

                            if (Convert.ToDecimal(txtScore.Text.Trim()) > scoreCategory.Range || Convert.ToDecimal(txtScore.Text.Trim()) < 0)
                            {
                                lblErrorMsg.Text = "Extra Curricular Score is greater or lesser than the configured range";
                                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                                lblErrorMsg.Visible = true;
                                return;
                            }

                            decimal scores = Convert.ToDecimal(txtScore.Text.Trim()); ;
                            decimal totalScore = Convert.ToDecimal(txtTotalMark.Text.Trim());
                            decimal tsScore = scores / totalScore;
                            int testPercentage = Convert.ToInt16(subCategory.Percentage);
                            decimal percentageScore = tsScore * testPercentage;
                            decimal CAPercentageScore = Convert.ToDecimal((percentageScore / 100) * scoreCategory.Percentage);
                            PASSIS.LIB.StudentScoreExtraCurricular getScoreObj = context.StudentScoreExtraCurriculars.FirstOrDefault(x => x.AdmissionNo == lblAdmNo.Text.Trim() && x.StudentId == Convert.ToInt64(lblId.Text.ToString().Trim()) && x.TemplateId == tempObject.ID);
                            if (getScoreObj == null)
                            {
                                score.AdmissionNo = lblAdmNo.Text;
                                score.StudentId = Convert.ToInt64(lblId.Text.ToString().Trim());
                                score.MarkObtainable = Convert.ToInt16(txtTotalMark.Text.Trim());
                                score.MarkObtained = Convert.ToDecimal(scores);
                                score.TermId = termId;
                                score.SessionId = sessionId;
                                score.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
                                score.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                                //score.SubjectId = Convert.ToInt16(subjectId);
                                score.Percentage = subCategory.Percentage;
                                score.PercentageScore = percentageScore;
                                score.CAPercentage = scoreCategory.Percentage;
                                score.CAPercentageScore = CAPercentageScore;
                                score.CategoryId = scoreCategory.Id;
                                score.SubCategoryId = subCategory.Id;
                                //score.DepartmentId = departmentId;
                                score.CampusId = logonUser.SchoolCampusId;
                                score.SchoolId = logonUser.SchoolId;
                                //score.SubjectTeacherId = Convert.ToInt64(subTeacher.TeacherId);
                                score.UploadedById = logonUser.Id;
                                score.TemplateId = tempObject.ID;
                                score.Description = txtDescription.Text.Trim();
                                score.Date = DateTime.Now;
                                //score.TestAssigenmentBroadSheetTemplateID = TestAssigenmentBroadSheetTemplateID;
                                context.StudentScoreExtraCurriculars.InsertOnSubmit(score);
                                context.SubmitChanges();
                            }
                            else
                            {
                                getScoreObj.AdmissionNo = lblAdmNo.Text;
                                getScoreObj.StudentId = Convert.ToInt64(lblId.Text.ToString().Trim());
                                getScoreObj.MarkObtainable = Convert.ToInt16(txtTotalMark.Text.Trim());
                                getScoreObj.MarkObtained = Convert.ToDecimal(scores);
                                getScoreObj.TermId = termId;
                                getScoreObj.SessionId = sessionId;
                                getScoreObj.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
                                getScoreObj.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                                //score.SubjectId = Convert.ToInt16(subjectId);
                                getScoreObj.Percentage = subCategory.Percentage;
                                getScoreObj.PercentageScore = percentageScore;
                                getScoreObj.CAPercentage = scoreCategory.Percentage;
                                getScoreObj.CAPercentageScore = CAPercentageScore;
                                getScoreObj.CategoryId = scoreCategory.Id;
                                getScoreObj.SubCategoryId = subCategory.Id;
                                //score.DepartmentId = departmentId;
                                getScoreObj.CampusId = logonUser.SchoolCampusId;
                                getScoreObj.SchoolId = logonUser.SchoolId;
                                //score.SubjectTeacherId = Convert.ToInt64(subTeacher.TeacherId);
                                getScoreObj.UploadedById = logonUser.Id;
                                getScoreObj.TemplateId = tempObject.ID;
                                getScoreObj.Description = txtDescription.Text.Trim();
                                getScoreObj.Date = DateTime.Now;
                                context.SubmitChanges();
                            }
                        }
                    }
                }
                //new ScoresheetLIB().SaveStudentScoreDetail(scoreList);
                TestAssignmentScoreTemplate tempObj = context.TestAssignmentScoreTemplates.FirstOrDefault(x => x.TemplateCode == templateCode);
                tempObj.HasSubmitted = true;
                context.SubmitChanges();
                //new ScoresheetLIB().SaveTestAssignmentTemplate(newTempObj);
                lblErrorMsg.Text = "Uploaded Successfully";
                lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                lblErrorMsg.Visible = true;
                //}
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
}