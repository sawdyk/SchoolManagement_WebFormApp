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
public partial class DeleteScoresSingle : PASSIS.LIB.Utility.BasePage
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
            ddlCode2.DataBind();

            int SessionId = 0;
            //clsMyDB mdb = new clsMyDB();
            //mdb.connct();
            //string query = "SELECT DISTINCT AcademicSessionId FROM AcademicSession WHERE SchoolId=" + logonUser.SchoolId;
            //SqlDataReader reader = mdb.fetch(query);

            ddlSession.DataSource = new DeleteScoresSingle().schSession().Distinct();
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

                    //IList<PASSIS.LIB.SubjectsInSchool> test = new PASSIS.LIB.SubjectTeachersLIB().getAllSubjects((long)logonUser.SchoolId);
                    //foreach (PASSIS.LIB.SubjectsInSchool subjId in test)
                    //{
                    //    PASSIS.LIB.Subject reqSubjects = context.Subjects.FirstOrDefault(c => c.Id == subjId.SubjectId);
                    //    ddlClassSubject.Items.Add(new ListItem(reqSubjects.Name, reqSubjects.Id.ToString()));
                    //}
                    //ddlClassSubject.Items.Insert(0, new ListItem("--Select--", "0", true));

                    //BindDropDown(grd.Class_Grade.ClassSubjectIds);
                }
                else
                {
                    long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
                    long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
                    ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
                    ddlYear.DataBind();


                }
                //PASSIS.LIB.Grade grd = getLogonTeacherGrade;
                //var getTestTemplate = from t in context.TestAssigenmentBroadSheetTemplates
                //                      where t.SchoolId == logonUser.SchoolId && t.CampusId == logonUser.SchoolCampusId && t.TeacherId == logonUser.Id
                //                      select new
                //                      {
                //                          t.ID,
                //                          t.BroadSheetDescriptionCode.Id,
                //                          t.BroadSheetDescriptionCode.DescriptionName
                //                      };
                //ddlCode.DataSource = getTestTemplate;
                //ddlCode.DataBind();
                //ddlCode.Items.Insert(0, new ListItem("--Select--", "0", true));

            }
            else
            {
                //var getTestTemplate = from t in context.TestAssigenmentBroadSheetTemplates
                //                      where t.SchoolId == logonUser.SchoolId && t.CampusId == logonUser.SchoolCampusId && t.TeacherId == logonUser.Id
                //                      select new
                //                      {
                //                          t.ID,
                //                          t.BroadSheetDescriptionCode.Id,
                //                          t.BroadSheetDescriptionCode.DescriptionName
                //                      };
                //ddlCode.DataSource = getTestTemplate;
                //ddlCode.DataBind();
                //ddlCode.Items.Insert(0, new ListItem("--Select--", "0", true));

                //ddlYear.DataSource = new PASSIS.LIB.TeacherLIB().getTeacherAllClass_Grade(logonUser.Id);
                long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
                long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
                ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
                ddlYear.DataBind();



            }
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

        ddlCategory.Items.Clear();
        var categoryList = from s in context.ScoreCategoryConfigurations where s.Category != "Behavioral" && s.Category != "Extra Curricular" && s.ClassId == yearId && s.SessionId == sessionId && s.TermId == termId && s.SchoolId == logonUser.SchoolId && s.CampusId == logonUser.SchoolCampusId select s;
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
    public void BindScore()
    {
        if (ddlCategory.SelectedItem.Text == "CA") //if CA is selected
        {
            try
            {
                Int64 TeacherId = logonUser.Id;

                var getScores = from score in context.StudentScoreRepositoryTransactions
                                where score.TestAssigenmentBroadSheetTemplateID == Convert.ToInt64(ddlCode.SelectedValue) && score.Code == Convert.ToInt64((ddlCode2.SelectedItem.Text))
                                && score.IsCancelled == false && score.SessionId == Convert.ToInt64(ddlSession.SelectedValue) && score.TermId == Convert.ToInt64(ddlTerm.SelectedValue)
                                && score.ClassId == Convert.ToInt64(ddlYear.SelectedValue) && score.GradeId == Convert.ToInt64(ddlGrade.SelectedValue)
                                && score.CategoryId == Convert.ToInt64(ddlCategory.SelectedValue) && score.SubCategoryId == Convert.ToInt64(ddlSubCategory.SelectedValue) && score.SubjectId == Convert.ToInt64((ddlClassSubject.SelectedValue))
                                //&& score.SubjectId == Convert.ToInt64(ddlClassSubject.SelectedValue)
                                select score;

                gdvTestScores.DataSource = getScores;
                gdvTestScores.DataBind();
                MultiView1.SetActiveView(ViewTestAssignment);
            }
            catch (Exception ex)
            {
                PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
                lblErrorMsg.Text = "Error occurred, kindly contact your administrator";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
            }
        }
        else if (ddlCategory.SelectedItem.Text == "Exam")//Exam
        {
            try
            {
                Int64 TeacherId = logonUser.Id;

                var getScores = from score in context.StudentScoreTransactions
                                where score.BroadSheetTemplateID == Convert.ToInt64(ddlCode.SelectedValue) && score.Code == Convert.ToInt64((ddlCode2.SelectedItem.Text))
                                && score.IsCancelled == false && score.AcademicSessionID == Convert.ToInt64(ddlSession.SelectedValue) && score.TermId == Convert.ToInt64(ddlTerm.SelectedValue)
                                && score.ClassId == Convert.ToInt64(ddlYear.SelectedValue) && score.GradeId == Convert.ToInt64(ddlGrade.SelectedValue)
                                && score.CategoryId == Convert.ToInt64(ddlCategory.SelectedValue) && score.SubCategoryId == Convert.ToInt64(ddlSubCategory.SelectedValue) && score.SubjectId == Convert.ToInt64((ddlClassSubject.SelectedValue))
                                //&& score.SubjectId == Convert.ToInt64(ddlClassSubject.SelectedValue)
                                select score;
                gdvExamScores.DataSource = getScores;
                gdvExamScores.DataBind();
                MultiView1.SetActiveView(ViewExam);
            }
            catch (Exception ex)
            {
                PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
                lblErrorMsg.Text = "Error occurred, kindly contact your administrator";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
            }
        }

    }

    protected void btnViewScores_Click(object sender, EventArgs e)
    {
        lblUploaded.Visible = true;
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


        if (ddlSession.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select session";
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

        if (ddlCategory.SelectedValue == "")
        {
            lblErrorMsg.Text = "Kindly select Template";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
        if (ddlCode.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select Template Code";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
        if (ddlCode2.SelectedValue == "")
        {
            lblErrorMsg.Text = "Kindly select Code";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
        BindScore();

        //if (ddlClassSubject.SelectedIndex == 0)
        //{
        //    lblErrorMsg.Text = "Kindly select Subject";
        //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
        //    lblErrorMsg.Visible = true;
        //    return;
        //}
    }
    protected void btnDeleteScores_Click(object sender, EventArgs e)
    {

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


        if (ddlSession.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select session";
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

        if (ddlCategory.SelectedValue == "")
        {
            lblErrorMsg.Text = "Kindly select Template";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
        if (ddlCode.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select Template Code";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
        if (ddlCode2.SelectedValue == "")
        {
            lblErrorMsg.Text = "Kindly select Code";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
        //if (ddlClassSubject.SelectedIndex == 0)
        //{
        //    lblErrorMsg.Text = "Kindly select Subject";
        //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
        //    lblErrorMsg.Visible = true;
        //    return;
        //}


        if (ddlCategory.SelectedItem.Text == "CA") //if CA is selected
        {
            try
            {
                Int64 TeacherId = logonUser.Id;

                var getScores = from score in context.StudentScoreRepositoryTransactions
                                where score.TestAssigenmentBroadSheetTemplateID == Convert.ToInt64(ddlCode.SelectedValue) && score.Code == Convert.ToInt64((ddlCode2.SelectedItem.Text))
                                 && score.IsCancelled == false && score.SessionId == Convert.ToInt64(ddlSession.SelectedValue) && score.TermId == Convert.ToInt64(ddlTerm.SelectedValue)
                                 && score.ClassId == Convert.ToInt64(ddlYear.SelectedValue) && score.GradeId == Convert.ToInt64(ddlGrade.SelectedValue)
                                 && score.CategoryId == Convert.ToInt64(ddlCategory.SelectedValue) && score.SubCategoryId == Convert.ToInt64(ddlSubCategory.SelectedValue) && score.SubjectId == Convert.ToInt64((ddlClassSubject.SelectedValue))
                                //&& score.SubjectId == Convert.ToInt64(ddlClassSubject.SelectedValue)
                                select score;

                //IList<StudentScoreRepository> stdScore = getScores.ToList<StudentScoreRepository>();
                //foreach (GridViewRow row in gdvTestScores.Rows)
                //{
                //    if (row.RowType == DataControlRowType.DataRow)
                //    {
                TestAssigenmentBroadSheetTemplate tempObject = context.TestAssigenmentBroadSheetTemplates.FirstOrDefault(x => x.ID == Convert.ToInt64(ddlCode.SelectedValue));
                //Label lblId = (Label)gdvTestScores.Rows[row.RowIndex].FindControl("lblId");
                //Label lblAdmNo = (Label)gdvTestScores.Rows[row.RowIndex].FindControl("lblAdmNo");
                //Label lblExamScore = (Label)gdvTestScores.Rows[row.RowIndex].FindControl("lblExamScore");
                //string examScore = lblExamScore.Text.Trim();
                IList<StudentScoreRepositoryTransaction> stdScore = getScores.ToList<StudentScoreRepositoryTransaction>();
                foreach (StudentScoreRepositoryTransaction score in stdScore)
                {
                    PASSIS.LIB.StudentScoreRepositoryTransaction stdSc = context.StudentScoreRepositoryTransactions.FirstOrDefault(x => x.TestAssigenmentBroadSheetTemplateID == Convert.ToInt64(ddlCode.SelectedValue)
                        && x.Code == score.Code
                        && x.SessionId == score.SessionId && x.TermId == score.TermId && x.ClassId == score.ClassId && x.GradeId == score.GradeId
                        && x.SubjectId == score.SubjectId && x.AdmissionNo == score.AdmissionNo && x.StudentId == score.StudentId && x.SubjectId == score.SubjectId
                        );

                    if (stdSc != null)
                    {
                        PASSIS.LIB.StudentScoreRepository getScoreObj = context.StudentScoreRepositories.FirstOrDefault(x => x.TestAssigenmentBroadSheetTemplateID == tempObject.ID
                           && x.SessionId == score.SessionId && x.TermId == score.TermId && x.ClassId == score.ClassId && x.GradeId == score.GradeId
                        && x.SubjectId == score.SubjectId && x.AdmissionNo == score.AdmissionNo && x.StudentId == score.StudentId && x.SubjectId == score.SubjectId);

                        PASSIS.LIB.StudentScoreRepositoryTransaction getScoreTransObj = context.StudentScoreRepositoryTransactions.FirstOrDefault(x => x.TestAssigenmentBroadSheetTemplateID == tempObject.ID && x.Code == Convert.ToInt64(ddlCode2.SelectedItem.Text)
                            && x.SessionId == score.SessionId && x.TermId == score.TermId && x.ClassId == score.ClassId && x.GradeId == score.GradeId
                        && x.SubjectId == score.SubjectId && x.AdmissionNo == score.AdmissionNo && x.StudentId == score.StudentId && x.SubjectId == score.SubjectId);

                        PASSIS.LIB.ScoreSubCategoryConfiguration getScoreSubCatConfig = context.ScoreSubCategoryConfigurations.FirstOrDefault(x => x.Id == getScoreTransObj.SubCategoryId);
                        PASSIS.LIB.ScoreCategoryConfiguration getScoreCatConfig = context.ScoreCategoryConfigurations.FirstOrDefault(x => x.Id == getScoreTransObj.CategoryId);

                        PASSIS.LIB.Subject subject = context.Subjects.FirstOrDefault(x => x.Id == getScoreTransObj.SubjectId);
                        PASSIS.LIB.SubjectsInSchool subSch = context.SubjectsInSchools.FirstOrDefault(x => x.SubjectId == subject.Id && x.SchoolId == logonUser.SchoolId);


                        context.StudentScoreRepositories.DeleteOnSubmit(getScoreObj);
                        context.StudentScoreRepositoryTransactions.DeleteOnSubmit(getScoreTransObj);
                        context.SubmitChanges();

                        //decimal scores = Convert.ToDecimal(getScoreObj.MarkObtained);

                        //decimal totalScore = Convert.ToDecimal(getScoreObj.MarkObtainable) - Convert.ToDecimal(getScoreTransObj.MarkObtainable);// subtracts mark obtainable from mark obtainable that exists (totScore)
                        //decimal newScore = Convert.ToDecimal(getScoreObj.MarkObtained) - scores; //subtracts mark obtained from mark obtained that exists (newScore)
                        //decimal tsScore;

                        //if (totalScore == 0 && newScore == 0)
                        //{
                        //    tsScore = 0; //Assign 0 to tsScore when totalScore and newScore equals zero : reverts all calculations to zero in the main table when the last inserted scores is deleted from the transaction table
                        //}

                        //else
                        //{
                        //    tsScore = newScore / totalScore;
                        //}


                        //int examPercentage = Convert.ToInt16(getScoreSubCatConfig.Percentage);
                        //decimal percentageScore = tsScore * examPercentage;
                        //decimal ExamPercentageScore = Convert.ToDecimal((percentageScore / 100) * getScoreCatConfig.Percentage);
                        //decimal? finalScore = (ExamPercentageScore * subSch.MaximumScore) / 100;

                        ////getScoreObj.AdmissionNumber = lblAdmNo.Text;
                        ////getScoreObj.StudentId = Convert.ToInt64(lblId.Text.ToString().Trim());
                        //getScoreObj.MarkObtainable = Convert.ToInt16(totalScore);
                        //getScoreObj.MarkObtained = Convert.ToDecimal(newScore);
                        ////getScoreObj.TermId = termId;
                        ////getScoreObj.AcademicSessionID = sessionId;
                        ////getScoreObj.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
                        ////getScoreObj.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                        ////getScoreObj.SubjectId = Convert.ToInt16(subjectId);
                        //getScoreObj.Percentage = getScoreSubCatConfig.Percentage;
                        //getScoreObj.PercentageScore = percentageScore;
                        //getScoreObj.CAPercentage = getScoreCatConfig.Percentage;
                        //getScoreObj.CAPercentageScore = ExamPercentageScore;
                        //getScoreObj.SubjectMaxScore = subSch.MaximumScore;
                        //getScoreObj.FinalScore = finalScore;
                        //getScoreObj.CategoryId = getScoreCatConfig.Id;
                        //getScoreObj.SubCategoryId = getScoreSubCatConfig.Id;
                        ////if (subSch.DepartmentId != null)
                        ////{
                        ////    score.DepartmentId = Convert.ToInt64(subSch.DepartmentId);
                        ////}
                        //getScoreObj.CampusId = logonUser.SchoolCampusId;
                        //getScoreObj.SchoolId = logonUser.SchoolId;
                        ////getScoreObj.SubjectTeacherId = Convert.ToInt64(subTeacher.TeacherId);
                        ////getScoreObj.UploadedById = logonUser.Id;
                        ////getScoreObj.TemplateId = tempObject.ID;
                        ////getScoreObj.Remark = txtRemark.Text.Trim();
                        //if (getScoreObj.Count >= 1)
                        //{
                        //    getScoreObj.Count = getScoreObj.Count - 1;
                        //}
                        //getScoreTransObj.IsCancelled = true;
                        //getScoreTransObj.IsCancelledDate = DateTime.Now;
                        ////getScoreObj.Description = txtDescription.Text.Trim();
                        ////getScoreObj.Date = DateTime.Now;
                        //context.SubmitChanges();



                        //context.StudentScoreTransactions.DeleteOnSubmit(stdSc);
                        //context.SubmitChanges();
                        //    }
                    }
                }

                //TestAssigenmentBroadSheetTemplate objTemplate = context.TestAssigenmentBroadSheetTemplates.FirstOrDefault(x => x.ID == Convert.ToInt64(ddlCode.SelectedValue));
                //objTemplate.HasSubmitted = false;
                //context.SubmitChanges();//update result has been submitted to false

                lblErrorMsg.Text = "Deleted Successfully";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                gdvTestScores.DataSource = getScores;
                gdvTestScores.DataBind();
                MultiView1.SetActiveView(ViewTestAssignment);
            }
            catch (Exception ex)
            {
                PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
                lblErrorMsg.Text = "Error occurred, kindly contact your administrator";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
            }
        }
        else if (ddlCategory.SelectedItem.Text == "Exam") //If Exam is Selected
        {
            try
            {
                Int64 TeacherId = logonUser.Id;

                var getScores = from score in context.StudentScoreTransactions
                                where score.BroadSheetTemplateID == Convert.ToInt64(ddlCode.SelectedValue) && score.Code == Convert.ToInt64((ddlCode2.SelectedItem.Text))
                                 && score.IsCancelled == false && score.AcademicSessionID == Convert.ToInt64(ddlSession.SelectedValue) && score.TermId == Convert.ToInt64(ddlTerm.SelectedValue)
                                 && score.ClassId == Convert.ToInt64(ddlYear.SelectedValue) && score.GradeId == Convert.ToInt64(ddlGrade.SelectedValue)
                                && score.CategoryId == Convert.ToInt64(ddlCategory.SelectedValue) && score.SubCategoryId == Convert.ToInt64(ddlSubCategory.SelectedValue) && score.SubjectId == Convert.ToInt64((ddlClassSubject.SelectedValue))
                                //&& score.SubjectId == Convert.ToInt64(ddlClassSubject.SelectedValue)
                                select score;
                //IList<PASSIS.LIB.StudentScoreTransaction> stdScore = getScores.ToList<PASSIS.LIB.StudentScoreTransaction>();
                //foreach (GridViewRow row in gdvExamScores.Rows)
                //{
                //    if (row.RowType == DataControlRowType.DataRow)
                //    {
                TestAssigenmentBroadSheetTemplate tempObject = context.TestAssigenmentBroadSheetTemplates.FirstOrDefault(x => x.ID == Convert.ToInt64(ddlCode.SelectedValue));
                //Label lblId = (Label)gdvExamScores.Rows[row.RowIndex].FindControl("lblId");
                //Label lblAdmNo = (Label)gdvExamScores.Rows[row.RowIndex].FindControl("lblAdmNo");
                //Label lblExamScore = (Label)gdvExamScores.Rows[row.RowIndex].FindControl("lblExamScore");
                //string examScore = lblExamScore.Text.Trim();
                IList<StudentScoreTransaction> stdScore = getScores.ToList<StudentScoreTransaction>();
                foreach (StudentScoreTransaction score in stdScore)
                {
                    PASSIS.LIB.StudentScoreTransaction stdSc = context.StudentScoreTransactions.FirstOrDefault(x => x.BroadSheetTemplateID == Convert.ToInt64(ddlCode.SelectedValue) && x.Code == Convert.ToInt64(ddlCode2.SelectedItem.Text)
                        && x.Code == score.Code
                        && x.AcademicSessionID == score.AcademicSessionID && x.TermId == score.TermId && x.ClassId == score.ClassId && x.GradeId == score.GradeId
                        && x.SubjectId == score.SubjectId && x.AdmissionNumber == score.AdmissionNumber && x.StudentId == score.StudentId && x.SubjectId == score.SubjectId
                        );

                    if (stdSc != null)
                    {
                        PASSIS.LIB.StudentScore getScoreObj = context.StudentScores.FirstOrDefault(x => x.BroadSheetTemplateID == tempObject.ID
                            && x.AcademicSessionID == score.AcademicSessionID && x.TermId == score.TermId && x.ClassId == score.ClassId && x.GradeId == score.GradeId
                        && x.SubjectId == score.SubjectId && x.AdmissionNumber == score.AdmissionNumber && x.StudentId == score.StudentId && x.SubjectId == score.SubjectId);

                        PASSIS.LIB.StudentScoreTransaction getScoreTransObj = context.StudentScoreTransactions.FirstOrDefault(x => x.BroadSheetTemplateID == tempObject.ID && x.Code == Convert.ToInt64(ddlCode2.SelectedItem.Text)
                           && x.AcademicSessionID == score.AcademicSessionID && x.TermId == score.TermId && x.ClassId == score.ClassId && x.GradeId == score.GradeId
                        && x.SubjectId == score.SubjectId && x.AdmissionNumber == score.AdmissionNumber && x.StudentId == score.StudentId && x.SubjectId == score.SubjectId);

                        PASSIS.LIB.ScoreSubCategoryConfiguration getScoreSubCatConfig = context.ScoreSubCategoryConfigurations.FirstOrDefault(x => x.Id == getScoreTransObj.SubCategoryId);
                        PASSIS.LIB.ScoreCategoryConfiguration getScoreCatConfig = context.ScoreCategoryConfigurations.FirstOrDefault(x => x.Id == getScoreTransObj.CategoryId);

                        PASSIS.LIB.Subject subject = context.Subjects.FirstOrDefault(x => x.Id == getScoreTransObj.SubjectId);
                        PASSIS.LIB.SubjectsInSchool subSch = context.SubjectsInSchools.FirstOrDefault(x => x.SubjectId == subject.Id && x.SchoolId == logonUser.SchoolId);

                        context.StudentScores.DeleteOnSubmit(getScoreObj);
                        context.StudentScoreTransactions.DeleteOnSubmit(getScoreTransObj);
                        context.SubmitChanges();

                        //decimal scores = Convert.ToDecimal(getScoreObj.ExamScore);

                        //decimal totalScore = Convert.ToDecimal(getScoreObj.ExamScoreObtainable) - Convert.ToDecimal(getScoreTransObj.ExamScoreObtainable); // subtracts mark obtainable from mark obtainable that exists (totScore)
                        //decimal newScore = Convert.ToDecimal(getScoreObj.ExamScore) - scores;//subtracts mark obtained from mark obtained that exists (newScore)
                        //decimal tsScore;
                        //if (totalScore == 0 && newScore == 0)
                        //{
                        //    tsScore = 0; //Assign 0 to tsScore when totalScore and newScore equals zero : reverts all calculations to zero in the main table when the last inserted scores is deleted from the transaction table
                        //}

                        //else
                        //{
                        //    tsScore = newScore / totalScore;
                        //}


                        //int examPercentage = Convert.ToInt16(getScoreSubCatConfig.Percentage);
                        //decimal percentageScore = tsScore * examPercentage;
                        //decimal ExamPercentageScore = Convert.ToDecimal((percentageScore / 100) * getScoreCatConfig.Percentage);
                        //decimal? finalScore = (ExamPercentageScore * subSch.MaximumScore) / 100;

                        ////getScoreObj.AdmissionNumber = lblAdmNo.Text;
                        ////getScoreObj.StudentId = Convert.ToInt64(lblId.Text.ToString().Trim());
                        //getScoreObj.ExamScoreObtainable = Convert.ToInt16(totalScore);
                        //getScoreObj.ExamScore = Convert.ToDecimal(newScore);
                        ////getScoreObj.TermId = termId;
                        ////getScoreObj.AcademicSessionID = sessionId;
                        ////getScoreObj.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
                        ////getScoreObj.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                        ////getScoreObj.SubjectId = Convert.ToInt16(subjectId);
                        //getScoreObj.Percentage = getScoreSubCatConfig.Percentage;
                        //getScoreObj.PercentageScore = percentageScore;
                        //getScoreObj.ExamPercentage = getScoreCatConfig.Percentage;
                        //getScoreObj.ExamPercentageScore = ExamPercentageScore;
                        //getScoreObj.SubjectMaxScore = subSch.MaximumScore;
                        //getScoreObj.FinalScore = finalScore;
                        //getScoreObj.CategoryId = getScoreCatConfig.Id;
                        //getScoreObj.SubCategoryId = getScoreSubCatConfig.Id;
                        ////if (subSch.DepartmentId != null)
                        ////{
                        ////    score.DepartmentId = Convert.ToInt64(subSch.DepartmentId);
                        ////}
                        //getScoreObj.CampusId = logonUser.SchoolCampusId;
                        //getScoreObj.SchoolId = logonUser.SchoolId;
                        ////getScoreObj.SubjectTeacherId = Convert.ToInt64(subTeacher.TeacherId);
                        ////getScoreObj.UploadedById = logonUser.Id;
                        ////getScoreObj.TemplateId = tempObject.ID;
                        ////getScoreObj.Remark = txtRemark.Text.Trim();
                        //if (getScoreObj.Count >= 1)
                        //{
                        //    getScoreObj.Count = getScoreObj.Count - 1;
                        //}

                        //getScoreTransObj.IsCancelled = true;
                        //getScoreTransObj.IsCancelledDate = DateTime.Now;
                        ////getScoreObj.Description = txtDescription.Text.Trim();
                        ////getScoreObj.Date = DateTime.Now;
                        //context.SubmitChanges();



                        //context.StudentScoreTransactions.DeleteOnSubmit(stdSc);
                        //context.SubmitChanges();
                        //}
                    }
                }


                //TestAssigenmentBroadSheetTemplate objTemplate = context.TestAssigenmentBroadSheetTemplates.FirstOrDefault(x => x.ID == Convert.ToInt64(ddlCode.SelectedValue));
                //objTemplate.HasSubmitted = false;
                //context.SubmitChanges();//update result has been submitted to false

                lblErrorMsg.Text = "Deleted Successfully";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                gdvExamScores.DataSource = getScores;
                gdvExamScores.DataBind();
                MultiView1.SetActiveView(ViewExam);

            }
            catch (Exception ex)
            {
                PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
                lblErrorMsg.Text = "Error occurred, kindly contact your administrator";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
            }
        }
    }
    //protected void ddlTemplate_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //}

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Populate Sub Category
        ddlSubCategory.Items.Clear();
        var categoryList = from s in context.ScoreSubCategoryConfigurations where s.CategoryId == Convert.ToInt64(ddlCategory.SelectedValue) select s;
        ddlSubCategory.DataSource = categoryList;
        ddlSubCategory.DataBind();
        ddlSubCategory.Items.Insert(0, new ListItem("--Select--", "0", true));

        var getTestTemplate = from t in context.TestAssigenmentBroadSheetTemplates
                              where t.SchoolId == logonUser.SchoolId && t.CampusId == logonUser.SchoolCampusId && t.TeacherId == logonUser.Id
                              select new
                              {
                                  t.ID,
                                  t.BroadSheetDescriptionCode.Id,
                                  t.BroadSheetDescriptionCode.DescriptionName
                              };
        ddlCode.DataSource = getTestTemplate;
        ddlCode.DataBind();
        ddlCode.Items.Insert(0, new ListItem("--Select--", "0", true));

        //if (ddlCategory.SelectedItem.Text == "Exam") 
        //{
        //    lblMark.Visible = false;
        //    txtTotalMark.Visible = false;
        //}
    }

    protected void ddlCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCategory.SelectedItem.Text == "CA") //if CA is selected
        {
            var getDescription = from t in context.StudentScoreRepositoryTransactions
                                 where t.SchoolId == logonUser.SchoolId && t.CampusId == logonUser.SchoolCampusId && t.UploadedById == logonUser.Id && t.TestAssigenmentBroadSheetTemplateID == Convert.ToInt64(ddlCode.SelectedValue)
                                 && t.IsCancelled == false && t.Code != null
                                 select t; //add check for isCancelled and add isCancelled column to the table
            ddlCode2.DataSource = getDescription.GroupBy(o => new { o.Code }).Select(o => o.FirstOrDefault());
            ddlCode2.DataBind();
            ddlCode2.Items.Insert(0, new ListItem("--Select--", "0", true));

        }
        else if (ddlCategory.SelectedItem.Text == "Exam") // If Exam is selected
        {
            var getDescription = from t in context.StudentScoreTransactions
                                 where t.SchoolId == logonUser.SchoolId && t.CampusId == logonUser.SchoolCampusId && t.UploadedById == logonUser.Id && t.BroadSheetTemplateID == Convert.ToInt64(ddlCode.SelectedValue)
                                 && t.IsCancelled == false && t.Code != null
                                 select t; //add check for isCancelled and add isCancelled column to the table
            ddlCode2.DataSource = getDescription.GroupBy(o => new { o.Code }).Select(o => o.FirstOrDefault());
            ddlCode2.DataBind();
            ddlCode2.Items.Insert(0, new ListItem("--Select--", "0", true));

        }
        //StudentScoreTransaction scoreTrans = context.StudentScoreTransactions.FirstOrDefault(x => x.TemplateId == Convert.ToInt64(ddlTemplate.SelectedValue));
        //ddlDescription.DataSource = scoreTrans.ToString();
        //ddlDescription.DataBind();
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


    public string SubjectName(long SubjectId)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        PASSIS.LIB.Subject subjectName = context.Subjects.FirstOrDefault(x => x.Id == SubjectId);
        return subjectName.Name;
    }

    public string CategoryName(long catId)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        PASSIS.LIB.ScoreCategoryConfiguration catName = context.ScoreCategoryConfigurations.FirstOrDefault(x => x.Id == catId);
        return catName.Category;
    }
    public string SubCategoryName(long subCatId)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        PASSIS.LIB.ScoreSubCategoryConfiguration subCatName = context.ScoreSubCategoryConfigurations.FirstOrDefault(x => x.Id == subCatId);
        return subCatName.SubCategory;
    }

    protected void gdvTestScores_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvTestScores.PageIndex = e.NewPageIndex;
        BindScore();
    }

    protected void gdvExamScores_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvExamScores.PageIndex = e.NewPageIndex;
        BindScore();
    }
}