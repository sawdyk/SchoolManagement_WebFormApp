using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using PASSIS.DAO.CustomClasses;
using System.Data.SqlClient;
using PASSIS.LIB;

public partial class ComputeResult : PASSIS.LIB.Utility.BasePage
{
    public static string SchoolName = "";
    public static string SchoolLogo = "";
    public static string SchoolAddress = "";
    public static string SchoolCurriculumId = "";
    public static string SchoolUrl = "";
    //public static string subjectId = "";
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
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

            PASSIS.LIB.School objUser = context.Schools.FirstOrDefault(x => x.Id == logonUser.SchoolId);
            SchoolName = objUser.Name.ToString();
            //if (objUser.Logo.ToString().Equals(""))
            //{
            //    SchoolLogo = "~/Images/Schools_Logo/passis_logoB.png";
            //}
            //else 
            //{
            //    SchoolLogo = objUser.Logo.ToString();
            //}
            //SchoolAddress = objUser.Address.ToString(); 
            ddlAcademicTerm.DataSource = new AcademicTermLIB().RetrieveAcademicTerm();
            ddlAcademicTerm.DataBind();

            clsMyDB mdb = new clsMyDB();
            mdb.connct();

            string querySession = "SELECT DISTINCT AcademicSessionId FROM AcademicSession WHERE SchoolId=" + logonUser.SchoolId;
            SqlDataReader readerSession = mdb.fetch(querySession);
            while (readerSession.Read())
            {
                ddlAcademicSession.DataSource = from s in context.AcademicSessionNames
                                                where s.ID == Convert.ToInt64(readerSession["AcademicSessionId"].ToString())
                                                select s;
                ddlAcademicSession.DataBind();
            }

            readerSession.Close();
            mdb.closeConnct();


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
        ////removes the grades that are already on the grid 
        //var d = from s in availableGrades where !GridGradeIds_VS.Contains(s.Id) select s;

        ddlGrade.DataSource = availableGrades;
        ddlGrade.DataBind();
        //ddlGrade.Items.Add(new ListItem("--Select All--", "0", true));
        ddlGrade.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select All--", "0", true));
        //BindGrid();
        //ddlSubject.Items.Clear();
    }
    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        //GridView gdvAllSubject = (GridView)FindControl("gdvAllSubject");
        //Label lblSubjectSelectionError = (Label)FindControl("lblSubjectSelectionError");
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

    protected void btnCompute_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlYear.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select year before you proceed";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }

            if (ddlGrade.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select class before you proceed";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }

            if (ddlAcademicSession.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select session before you proceed";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }

            if (ddlAcademicTerm.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select term before you proceed";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }

            lblErrorMsg.Visible = false;
            Int64 yearId = 0L, gradeId = 0L, schoolId = 0L, campusId = 0L;
            yearId = Convert.ToInt64(ddlYear.SelectedValue);
            gradeId = Convert.ToInt64(ddlGrade.SelectedValue);
            schoolId = (long)logonUser.SchoolId;
            campusId = logonUser.SchoolCampusId;
            printBroadSheet(yearId, gradeId, schoolId, campusId);
        }
        catch (Exception ex)
        {
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Source, ex.StackTrace);
            lblErrorMsg.Text = "Error occurred";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
        }
    }
    void printBroadSheet(Int64 yearId, Int64 gradeId, Int64 schoolId, Int64 campusId)
    {
        try
        {
            UsersLIB usrDal = new UsersLIB();




            addResultSummaryPage(yearId, schoolId, gradeId, campusId);

        }
        catch (Exception ex) { throw ex; }
    }
    protected void addResultSummaryPage(Int64 yearId, Int64 schId, Int64 gradeId, Int64 campusId)
    {
        AddResultTableToDocument(yearId, gradeId, schId, campusId);
    }
    // By Prof
    //private long? GetPreviousScore()
    //{
    //    IList<ReportCardData> tScores = (from t in context.ReportCardDatas
    //                                     where t.SubjectId == Convert.ToInt64(ddlSubject.SelectedValue) &&
    //                                         t.TermId == Convert.ToInt64(ddlAcademicTerm.SelectedValue) &&
    //                                         t.SessionId == Convert.ToInt64(ddlAcademicSession.SelectedValue) &&
    //                                         t.SchoolId == logonUser.SchoolId
    //                                     select t).ToList();
    //    IList<ReportCardData> sortedTScore2 = tScores.Where(d => d.Position != null).OrderByDescending(c => c.TotalScore).ToList();
    //    long? score = 0;
    //    foreach (var s in sortedTScore2)
    //    {
    //        score = s.TotalScore;
    //    }
    //    return score;
    //}
    protected void AddResultTableToDocument(Int64 yearId, Int64 gradeId, Int64 schoolId, Int64 campusId)
    {

        #region
        Int64 sessionId = Convert.ToInt64(ddlAcademicSession.SelectedValue);
        Int64 TermId = Convert.ToInt64(ddlAcademicTerm.SelectedValue);


        Dictionary<Int64, IList<PASSIS.LIB.CustomClasses.ResultProperties>> resultProp_Dic = new Dictionary<Int64, IList<PASSIS.LIB.CustomClasses.ResultProperties>>();
        IList<PASSIS.LIB.CustomClasses.ResultProperties> resList;// = new List<ResultProperties>();
        Dictionary<Int64, ResultSummaryStatistics> resultStat_Dic = new Dictionary<Int64, ResultSummaryStatistics>();

        IList<PASSIS.LIB.Subject> getId = new SubjectTeachersLIB().getAllSubjectsInSchool(Convert.ToInt64(ddlYear.SelectedValue));
        int rows = 11;
        int totalAttainable = getId.Count * 100;

        Int64 stdCounter = 0;
        int totalMarkObtained = 0;
        int totalMarkObtainable = 0;
        decimal testScoreConfiguration = 0;
        decimal aggregateTestScore = 0;
        decimal totalScore = 0;
        decimal examScore = 0;
        decimal aggregateTotalScore = 0;
        PASSIS.LIB.ReportCardData report = new ReportCardData();
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        foreach (GridViewRow row in gdvAllSubject.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                bool isChecked = row.Cells[2].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                if (isChecked)
                {
                    string subjectId = row.Cells[3].Controls.OfType<Label>().FirstOrDefault().Text;
                   
                    IList<PASSIS.LIB.GradeStudent> gradeStudent = new ClassGradeLIB().RetrieveGradeStudents(schoolId, campusId, yearId, gradeId, sessionId);
                    foreach (PASSIS.LIB.GradeStudent std in gradeStudent)
                    {
                        try
                        {
                            ++stdCounter;

                            decimal totalExamScore = 0;
                            decimal totalExam = 0;
                            IList<PASSIS.LIB.StudentScore> scor = (from g in context.StudentScores
                                                                   where g.SubjectId == Convert.ToInt64(subjectId) && g.AdmissionNumber == std.User.AdmissionNumber &&
                                                                   g.AcademicSessionID == sessionId && g.TermId == TermId && g.SchoolId == schoolId && g.CampusId == campusId && g.ClassId == yearId && g.GradeId == gradeId
                                                                   select g).ToList();

                            if (scor.Count > 1)
                            {
                                foreach (PASSIS.LIB.StudentScore scrs in scor)
                                {
                                    totalExam = Convert.ToDecimal(scrs.ExamScore);
                                    totalExamScore += totalExam;

                                }
                            }
                            else if (scor.Count == 1)
                            {
                                foreach (PASSIS.LIB.StudentScore scrs in scor)
                                {
                                    totalExamScore = Convert.ToDecimal(scrs.ExamScore);
                                }
                            }
                            else
                            {
                                totalExamScore = 0;
                            }

                            //PASSIS.LIB.StudentScore scor = new ScoresheetLIB().RetrieveStudentScoreBulkUploadTest(Convert.ToInt64(subjectId), std.User.AdmissionNumber, sessionId, TermId);
                            //if (scor != null)
                            //{
                            //    //try
                            //{



                            decimal totalTestScore = 0;
                            decimal totalTest = 0;
                            IList<StudentScoreRepository> scoreRepository = (from score in context.StudentScoreRepositories
                                                                             where score.SessionId == sessionId && score.TermId == TermId && score.ClassId == yearId && score.GradeId == gradeId &&
                                                                               score.SubjectId == Convert.ToInt64(subjectId) && score.AdmissionNo == std.User.AdmissionNumber && score.CampusId == logonUser.SchoolCampusId
                                                                            && score.SchoolId == logonUser.SchoolId
                                                                             select score).ToList();

                            //StudentScoreRepository scoreRepository = new ScoresheetLIB().getTestScore(sessionId, TermId, Convert.ToInt64(subjectId), std.User.AdmissionNumber, logonUser.SchoolCampusId);
                            if (scoreRepository.Count > 1)
                            {
                                foreach (PASSIS.LIB.StudentScoreRepository scRep in scoreRepository)
                                {
                                    totalTest = Convert.ToDecimal(scRep.MarkObtained);
                                    totalTestScore += totalTest;
                                }
                            }
                            else if (scoreRepository.Count == 1)
                            {
                                foreach (PASSIS.LIB.StudentScoreRepository scRep in scoreRepository)
                                {
                                    totalTestScore = Convert.ToDecimal(scRep.MarkObtained);
                                }
                            }
                            else
                            {
                                totalTestScore = 0;
                            }

                            totalScore = totalTestScore + totalExamScore;
                            //totalMarkObtained += Convert.ToInt32(scoreRepository.MarkObtained);
                            //            totalMarkObtainable += Convert.ToInt32(scoreRepository.MarkObtainable);

                            //ScoreConfiguration getScore = new ScoresheetLIB().getScoreConfiguration((long)logonUser.SchoolId, logonUser.SchoolCampusId, Convert.ToInt64(ddlYear.SelectedValue));
                            //testScoreConfiguration = Convert.ToDecimal(getScore.TestScore);

                            //aggregateTestScore = Convert.ToDecimal(totalMarkObtained) / Convert.ToDecimal(totalMarkObtainable) * testScoreConfiguration;
                            //examScore = Convert.ToDecimal(scor.ExamScore);
                            //totalScore = Math.Round(Convert.ToDecimal(aggregateTestScore) + examScore, 0);

                            //aggregateTotalScore += totalScore;

                            ReportCardData getScoreIfExist = context.ReportCardDatas.FirstOrDefault(s =>
                                                   s.TextScore == totalTestScore &&
                                                   s.ExamScore == totalExamScore &&
                                                   s.AdmissionNumber == std.User.AdmissionNumber &&
                                                   s.StudentId == std.User.Id &&
                                                   s.TotalScore == totalScore &&
                                                   s.YearId == Convert.ToInt64(ddlYear.SelectedValue) &&
                                                   s.GradeId == Convert.ToInt64(ddlGrade.SelectedValue) &&
                                                   s.SubjectId == Convert.ToInt64(subjectId) &&
                                                   s.TermId == Convert.ToInt64(ddlAcademicTerm.SelectedValue) &&
                                                   s.SessionId == Convert.ToInt64(ddlAcademicSession.SelectedValue) &&
                                                   s.SchoolId == logonUser.SchoolId &&
                                                   s.CampusId == logonUser.SchoolCampusId);


                            if (getScoreIfExist != null) //update the record if it exists
                            {
                                getScoreIfExist.ExamScore = Convert.ToInt64(totalExamScore);
                                getScoreIfExist.TextScore = Convert.ToInt64(totalTestScore);
                                getScoreIfExist.TotalScore = Convert.ToInt64(totalScore);
                                getScoreIfExist.AdmissionNumber = std.User.AdmissionNumber;
                                getScoreIfExist.StudentId = std.User.Id;
                                getScoreIfExist.SubjectId = int.Parse(subjectId);
                                getScoreIfExist.SchoolId = logonUser.SchoolId;
                                getScoreIfExist.CampusId = logonUser.SchoolCampusId;
                                getScoreIfExist.YearId = Convert.ToInt64(ddlYear.SelectedValue);
                                getScoreIfExist.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                                getScoreIfExist.TermId = Convert.ToInt64(ddlAcademicTerm.SelectedValue);
                                getScoreIfExist.SessionId = Convert.ToInt64(ddlAcademicSession.SelectedValue);
                                getScoreIfExist.Date = DateTime.Today;
                            }
                            else //Save a new record if it doesnt exist
                            {
                                report = new ReportCardData();
                                report.ExamScore = Convert.ToInt64(totalExamScore);
                                report.TextScore = Convert.ToInt64(totalTestScore);
                                report.TotalScore = Convert.ToInt64(totalScore);
                                report.AdmissionNumber = std.User.AdmissionNumber;
                                report.StudentId = std.User.Id;
                                report.SubjectId = int.Parse(subjectId);
                                report.SchoolId = logonUser.SchoolId;
                                report.CampusId = logonUser.SchoolCampusId;
                                report.YearId = Convert.ToInt64(ddlYear.SelectedValue);
                                report.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                                report.TermId = Convert.ToInt64(ddlAcademicTerm.SelectedValue);
                                report.SessionId = Convert.ToInt64(ddlAcademicSession.SelectedValue);
                                report.Date = DateTime.Today;
                                context.ReportCardDatas.InsertOnSubmit(report);

                            }


                            //PASSISLIBDataContext context = new PASSISLIBDataContext();



                            totalMarkObtainable = 0;
                            totalMarkObtained = 0;
                            aggregateTestScore = 0;
                            //}
                            //else if (scoreRepository == null)
                            //{

                            //    examScore = Convert.ToDecimal(scor.ExamScore);
                            //    totalScore = Math.Round(Convert.ToDecimal(0) + examScore, 0);
                            //    aggregateTotalScore += totalScore;

                            //    ReportCardData getScoreIfExist = context.ReportCardDatas.FirstOrDefault(s =>
                            //               s.TextScore == aggregateTestScore &&
                            //               s.ExamScore == examScore &&
                            //               s.AdmissionNumber == std.User.AdmissionNumber &&
                            //               s.StudentId == std.User.Id &&
                            //               s.TotalScore == totalScore &&
                            //               s.YearId == Convert.ToInt64(ddlYear.SelectedValue) &&
                            //               s.GradeId == Convert.ToInt64(ddlGrade.SelectedValue) &&
                            //               s.SubjectId == Convert.ToInt64(subjectId) &&
                            //               s.TermId == Convert.ToInt64(ddlAcademicTerm.SelectedValue) &&
                            //               s.SessionId == Convert.ToInt64(ddlAcademicSession.SelectedValue) &&
                            //               s.SchoolId == logonUser.SchoolId &&
                            //               s.CampusId == logonUser.SchoolCampusId);


                            //    if (getScoreIfExist != null)
                            //    {
                            //        getScoreIfExist.ExamScore = Convert.ToInt64(examScore);
                            //        getScoreIfExist.TextScore = Convert.ToInt64(aggregateTestScore);
                            //        getScoreIfExist.TotalScore = Convert.ToInt64(totalScore);
                            //        getScoreIfExist.AdmissionNumber = std.User.AdmissionNumber;
                            //        getScoreIfExist.StudentId = std.User.Id;
                            //        getScoreIfExist.SubjectId = int.Parse(subjectId);
                            //        getScoreIfExist.SchoolId = logonUser.SchoolId;
                            //        getScoreIfExist.CampusId = logonUser.SchoolCampusId;
                            //        getScoreIfExist.YearId = Convert.ToInt64(ddlYear.SelectedValue);
                            //        getScoreIfExist.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                            //        getScoreIfExist.TermId = Convert.ToInt64(ddlAcademicTerm.SelectedValue);
                            //        getScoreIfExist.SessionId = Convert.ToInt64(ddlAcademicSession.SelectedValue);
                            //        getScoreIfExist.Date = DateTime.Today;
                            //    }
                            //    else
                            //    {
                            //        report = new ReportCardData();
                            //        report.ExamScore = Convert.ToInt64(examScore);
                            //        report.TextScore = Convert.ToInt64(aggregateTestScore);
                            //        report.TotalScore = Convert.ToInt64(totalScore);
                            //        report.AdmissionNumber = std.User.AdmissionNumber;
                            //        report.StudentId = std.User.Id;
                            //        report.SubjectId = int.Parse(subjectId);
                            //        report.SchoolId = logonUser.SchoolId;
                            //        report.CampusId = logonUser.SchoolCampusId;
                            //        report.YearId = Convert.ToInt64(ddlYear.SelectedValue);
                            //        report.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                            //        report.TermId = Convert.ToInt64(ddlAcademicTerm.SelectedValue);
                            //        report.SessionId = Convert.ToInt64(ddlAcademicSession.SelectedValue);
                            //        report.Date = DateTime.Today;
                            //        context.ReportCardDatas.InsertOnSubmit(report);

                            //    }


                            //PASSISLIBDataContext context = new PASSISLIBDataContext();



                            totalMarkObtainable = 0;
                            totalMarkObtained = 0;
                            aggregateTestScore = 0;
                        }
                        //}
                        catch (Exception e)
                        {
                            //resTable.AddCell("");
                        }
                        //}
                        //else
                        //{
                        //    lblErrorMsg.Text = "No Record Found!";
                        //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                        //    lblErrorMsg.Visible = true;
                        //    // If Exam is null
                        //    //resTable.AddCell("");
                        //    //resTable.AddCell("");
                        //}
                    }

                            //    catch (Exception x) { throw x; }
                            //}

                    //context.ReportCardDatas.InsertOnSubmit(report);
                    context.SubmitChanges();

                    //var tScore = context.ReportCardDatas.OrderByDescending(c => c.TotalScore);

                    //Update ReportCardData object with computed position for each subjects

                    IList<ReportCardData> tScores = (from t in context.ReportCardDatas
                                                     where t.SubjectId == Convert.ToInt64(subjectId) &&
                                                         t.TermId == Convert.ToInt64(ddlAcademicTerm.SelectedValue) &&
                                                         t.SessionId == Convert.ToInt64(ddlAcademicSession.SelectedValue) &&
                                                         t.SchoolId == logonUser.SchoolId &&
                                                         t.CampusId == logonUser.SchoolCampusId &&
                                                         t.YearId == Convert.ToInt64(ddlYear.SelectedValue) &&
                                                         t.GradeId == Convert.ToInt64(ddlGrade.SelectedValue)
                                                     select t).ToList();
                    // check if we have data to compute
                    //if(tScores.Count()>0){
                    //var sortedTScore = tScore.OrderByDescending(c=>c).ToList();
                    IList<ReportCardData> sortedTScore = tScores.OrderByDescending(c => c.TotalScore).ToList();

                    int pos = 1;
                    long? ScorePrev = 0; int countscore = 0; int countRec = 0;
                    foreach (ReportCardData score in sortedTScore) //
                    {

                        countRec++;
                        PASSIS.LIB.ReportCardData posDataa = context.ReportCardDatas.FirstOrDefault(x => x.TotalScore == score.TotalScore &&
                            x.Id == score.Id &&
                            x.SubjectId == Convert.ToInt64(subjectId) &&
                            x.TermId == Convert.ToInt64(ddlAcademicTerm.SelectedValue) &&
                            x.SessionId == Convert.ToInt64(ddlAcademicSession.SelectedValue) &&
                            x.SchoolId == logonUser.SchoolId &&
                            x.CampusId == logonUser.SchoolCampusId &&
                            x.YearId == Convert.ToInt64(ddlYear.SelectedValue) &&
                            x.GradeId == Convert.ToInt64(ddlGrade.SelectedValue)
                            );
                        if (countRec > 1)
                        {
                            var data = sortedTScore.Where(d => d.Position != null).OrderByDescending(d => d.TotalScore);
                            foreach (var s in data)
                            {

                                ScorePrev = s.TotalScore;
                            }
                            if (ScorePrev == score.TotalScore)
                            {
                                countscore++;
                                posDataa.Position = pos;
                            }
                            else
                            {
                                if (countscore > 0)
                                {
                                    pos = pos + countscore;
                                    countscore = 0;
                                }
                                pos++;
                                posDataa.Position = pos;
                            }
                        }
                        else
                        {
                            posDataa.Position = pos;
                        }
                    }
                    context.SubmitChanges();

                    // Annual Subject Score & Position
                    //if (TermId == 3)
                    //{
                    //    IList<PASSIS.LIB.GradeStudent> getGradeStudents = new ClassGradeLIB().RetrieveGradeStudents(schoolId, campusId, yearId, gradeId, sessionId);
                    //    foreach (PASSIS.LIB.GradeStudent gstd in getGradeStudents)
                    //    {
                    //        IList<ReportCardData> getAllTermScores = (from s in context.ReportCardDatas
                    //                                                  where s.AdmissionNumber == gstd.User.AdmissionNumber &&
                    //                                                      s.StudentId == gstd.User.Id &&
                    //                                                      s.SchoolId == schoolId &&
                    //                                                      s.CampusId == campusId &&
                    //                                                      s.SessionId == sessionId &&
                    //                                                      s.SubjectId == int.Parse(subjectId) &&
                    //                                                      s.YearId == Convert.ToInt64(ddlYear.SelectedValue) &&
                    //                                                      s.GradeId == Convert.ToInt64(ddlGrade.SelectedValue)
                    //                                                  select s).ToList();

                    //        long? annualScoresPerSubject = 0;
                    //        int scoreCounter = 0;
                    //        long? averageAnnualScore = 0;
                    //        foreach (ReportCardData termScores in getAllTermScores)
                    //        {
                    //            scoreCounter++;
                    //            annualScoresPerSubject += termScores.TotalScore;
                    //        }
                    //        if (scoreCounter != 0)
                    //        {
                    //            averageAnnualScore = annualScoresPerSubject / scoreCounter;
                    //        }

                    //        ReportCardData getData = context.ReportCardDatas.FirstOrDefault(s => s.SchoolId == schoolId &&
                    //                                          s.CampusId == campusId &&
                    //                                          s.AdmissionNumber == gstd.User.AdmissionNumber &&
                    //                                          s.StudentId == gstd.User.Id &&
                    //                                          s.SessionId == sessionId &&
                    //                                          s.TermId == Convert.ToInt64(ddlAcademicTerm.SelectedValue) &&
                    //                                          s.SubjectId == int.Parse(subjectId) &&
                    //                                          s.YearId == Convert.ToInt64(ddlYear.SelectedValue) &&
                    //                                          s.GradeId == Convert.ToInt64(ddlGrade.SelectedValue));
                    //        if (getData == null)
                    //        {
                    //            //ReportCardData data = new ReportCardData();
                    //            //data.SchoolId = schoolId;
                    //            //data.SessionId = sessionId;
                    //            //data.TermId = TermId;
                    //            //data.SubjectId = int.Parse(ddlSubject.SelectedValue);
                    //            //data.YearId = Convert.ToInt64(ddlYear.SelectedValue);
                    //            //data.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                    //            //data.TextScore = Convert.ToInt64(aggregateTestScore);
                    //            //data.ExamScore = Convert.ToInt64(examScore);
                    //            //context.ReportCardDatas.InsertOnSubmit(data);
                    //            //context.SubmitChanges();
                    //        }
                    //        else
                    //        {
                    //            //ReportCardData data = new ReportCardData();
                    //            //data.SchoolId = schoolId;
                    //            //data.SessionId = sessionId;
                    //            //data.TermId = TermId;
                    //            //data.SubjectId = int.Parse(subjectId);
                    //            //data.YearId = Convert.ToInt64(ddlYear.SelectedValue);
                    //            //data.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                    //            //data.TextScore = Convert.ToInt64(aggregateTestScore);
                    //            //data.ExamScore = Convert.ToInt64(examScore);
                    //            getData.AverageScore = averageAnnualScore;

                    //        }

                    //    }
                    //    context.SubmitChanges();
                    //    IList<ReportCardData> getAnnnualTotalScore = (from s in context.ReportCardDatas
                    //                                                  where
                    //                                                      s.SubjectId == Convert.ToInt64(subjectId) &&
                    //                                                      s.SchoolId == schoolId &&
                    //                                                      s.CampusId == campusId &&
                    //                                                      s.SessionId == sessionId &&
                    //                                                      s.TermId == TermId &&
                    //                                                      s.YearId == Convert.ToInt64(ddlYear.SelectedValue) &&
                    //                                                      s.GradeId == Convert.ToInt64(ddlGrade.SelectedValue)
                    //                                                  select s).ToList();

                    //    IList<ReportCardData> sortedAnnualScore = getAnnnualTotalScore.OrderByDescending(s => s.AverageScore).ToList();
                    //    int averagePosition = 0; int countAverageScore = 0;
                    //    IDictionary<long?, long> dic = new Dictionary<long?, long>();

                    //    foreach (ReportCardData score in sortedAnnualScore)
                    //    {
                    //        ReportCardData reportData = context.ReportCardDatas.FirstOrDefault(s =>
                    //            s.AdmissionNumber == score.AdmissionNumber &&
                    //            s.StudentId == score.StudentId &&
                    //            s.TotalScore == score.TotalScore &&
                    //            s.Position == score.Position &&
                    //            s.AverageScore == score.AverageScore &&
                    //            s.SchoolId == score.SchoolId &&
                    //            s.CampusId == score.CampusId &&
                    //            s.SessionId == score.SessionId &&
                    //            s.TermId == score.TermId &&
                    //            s.SubjectId == score.SubjectId &&
                    //            s.YearId == score.YearId &&
                    //            s.GradeId == score.GradeId);

                    //        if (dic.ContainsKey(score.AverageScore))
                    //        {
                    //            countAverageScore++;
                    //            reportData.AveragePosition = averagePosition;
                    //        }
                    //        else
                    //        {
                    //            if (countAverageScore > 0)
                    //            {
                    //                averagePosition = averagePosition + countAverageScore;
                    //                countAverageScore = 0;
                    //            }
                    //            averagePosition++;
                    //            reportData.AveragePosition = averagePosition;
                    //            dic.Add(score.AverageScore, averagePosition);
                    //        }
                    //    }
                    //    context.SubmitChanges();
                    //    dic.Clear();


                    //}
                    //// Class Position
                    //if (TermId == 3)
                    //{

                    //    foreach (PASSIS.LIB.GradeStudent gstd in gradeStudent)
                    //    {
                    //        var stdTotalAverageScore = from s in context.ReportCardDatas
                    //                                   where s.AdmissionNumber == gstd.User.AdmissionNumber &&
                    //                                       s.StudentId == gstd.User.Id &&
                    //                                       s.TermId == TermId &&
                    //                                       s.SessionId == sessionId
                    //                                   select s.AverageScore;

                    //        List<long?> totalAverageScore = stdTotalAverageScore.ToList();
                    //        if (totalAverageScore.Count() > 0)
                    //        {
                    //            long? aggAverageScore = 0;
                    //            long subjectCount = 0;
                    //            foreach (var avSore in totalAverageScore)
                    //            {
                    //                aggAverageScore += avSore;
                    //                subjectCount++;
                    //            }
                    //            //ReportCardPosition rcPos = new ReportCardPosition();
                    //            ReportCardPosition rcPosition = context.ReportCardPositions.Where(s => s.AdmissionNumber == gstd.User.AdmissionNumber &&
                    //                s.StudentId == gstd.User.Id &&
                    //                s.YearId == Convert.ToInt64(ddlYear.SelectedValue) &&
                    //                s.GradeId == Convert.ToInt64(ddlGrade.SelectedValue) &&
                    //                s.TermId == Convert.ToInt64(ddlAcademicTerm.SelectedValue) &&
                    //                s.SessionId == Convert.ToInt64(ddlAcademicSession.SelectedValue)).FirstOrDefault();
                    //            if (rcPosition == null)
                    //            {
                    //                ReportCardPosition rcPos = new ReportCardPosition();
                    //                rcPos.AdmissionNumber = gstd.User.AdmissionNumber;
                    //                rcPos.StudentId = gstd.User.Id;
                    //                rcPos.TotalScore = aggAverageScore;
                    //                rcPos.SchoolId = schoolId;
                    //                rcPos.CampusId = campusId;
                    //                rcPos.YearId = Convert.ToInt64(ddlYear.SelectedValue);
                    //                rcPos.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                    //                rcPos.SessionId = Convert.ToInt64(ddlAcademicSession.SelectedValue);
                    //                rcPos.TermId = Convert.ToInt64(ddlAcademicTerm.SelectedValue);
                    //                rcPos.Date = DateTime.Today;
                    //                rcPos.SubjectComputed = subjectCount;
                    //                context.ReportCardPositions.InsertOnSubmit(rcPos);
                    //                context.SubmitChanges();
                    //            }
                    //            else
                    //            {
                    //                rcPosition.AdmissionNumber = gstd.User.AdmissionNumber;
                    //                rcPosition.StudentId = gstd.User.Id;
                    //                rcPosition.TotalScore = aggAverageScore;
                    //                rcPosition.SchoolId = schoolId;
                    //                rcPosition.CampusId = campusId;
                    //                rcPosition.YearId = Convert.ToInt64(ddlYear.SelectedValue);
                    //                rcPosition.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                    //                rcPosition.SessionId = Convert.ToInt64(ddlAcademicSession.SelectedValue);
                    //                rcPosition.TermId = Convert.ToInt64(ddlAcademicTerm.SelectedValue);
                    //                rcPosition.Date = DateTime.Today;
                    //                rcPosition.SubjectComputed = subjectCount;
                    //                //context.ReportCardPositions.InsertOnSubmit(rcPosition);
                    //                context.SubmitChanges();
                    //            }
                    //        }

                    //    }

                    //    IList<ReportCardPosition> classPos = (from s in context.ReportCardPositions
                    //                                          where
                    //                                              s.GradeId == Convert.ToInt64(ddlGrade.SelectedValue) &&
                    //                                              s.YearId == Convert.ToInt64(ddlYear.SelectedValue) &&
                    //                                              s.SchoolId == schoolId &&
                    //                                              s.CampusId == campusId &&
                    //                                              s.SessionId == Convert.ToInt64(ddlAcademicSession.SelectedValue) &&
                    //                                              s.TermId == Convert.ToInt64(ddlAcademicTerm.SelectedValue)
                    //                                          select s).ToList();
                    //    if (classPos.Count() > 0)
                    //    {
                    //        IList<ReportCardPosition> sortedClassPos = classPos.OrderByDescending(s => s.TotalScore).ToList();

                    //        int classPosition = 0; int countClassScore = 0;
                    //        IDictionary<long?, long> map = new Dictionary<long?, long>();
                    //        foreach (ReportCardPosition classPoss in sortedClassPos)
                    //        {
                    //            //classPosition++;
                    //            //countClassRec++;

                    //            ReportCardPosition posObj = context.ReportCardPositions.FirstOrDefault(s => s.Id == classPoss.Id &&
                    //                s.TotalScore == classPoss.TotalScore && s.SchoolId == classPoss.SchoolId &&
                    //                s.GradeId == classPoss.GradeId && s.YearId == classPoss.YearId &&
                    //                s.TermId == classPoss.TermId && s.SessionId == classPoss.SessionId);
                    //            //posObj.Position = classPosition;


                    //            if (map.ContainsKey(classPoss.TotalScore) == true)
                    //            {
                    //                countClassScore++;
                    //                posObj.Position = classPosition;
                    //            }
                    //            else
                    //            {
                    //                if (countClassScore > 0)
                    //                {
                    //                    classPosition = classPosition + countClassScore;
                    //                    countClassScore = 0;
                    //                }
                    //                classPosition++;
                    //                posObj.Position = classPosition;
                    //                map.Add(classPoss.TotalScore, classPosition);
                    //            }

                    //        }
                    //        context.SubmitChanges();
                    //        map.Clear();
                    //        lblErrorMsg.Text = "Result Computed Successfully";
                    //        lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                    //        lblErrorMsg.Visible = true;
                    //    }
                    //}
                    //else 
                    //{

                        IList<PASSIS.LIB.GradeStudent> gradeStudents = new ClassGradeLIB().RetrieveGradeStudents(schoolId, campusId, yearId, gradeId, sessionId);
                        foreach (PASSIS.LIB.GradeStudent gstd in gradeStudent)
                        {
                            var stdTotalScore = from s in context.ReportCardDatas
                                                where s.AdmissionNumber == gstd.User.AdmissionNumber &&
                                                    s.StudentId == gstd.User.Id &&
                                                    s.TermId == TermId &&
                                                    s.SessionId == sessionId &&
                                                    s.YearId == Convert.ToInt64(ddlYear.SelectedValue) &&
                                                    s.GradeId == Convert.ToInt64(ddlGrade.SelectedValue)
                                                select s.TotalScore;

                            List<long?> totalScoreStd = stdTotalScore.ToList();
                            if (totalScoreStd.Count() > 0)
                            {
                                long? aggScore = 0;
                                long subjectCount = 0;

                                foreach (var oScore in totalScoreStd)
                                {
                                    aggScore += oScore; //addup all the totalscore
                                    subjectCount++; // count the number of subjects
                                }

                                ReportCardPosition rpPos = new ReportCardPosition();
                                var rpPoss = context.ReportCardPositions.Where(s => s.AdmissionNumber == gstd.User.AdmissionNumber &&
                                    s.StudentId == gstd.User.Id &&
                                    s.YearId == Convert.ToInt64(ddlYear.SelectedValue) &&
                                    s.GradeId == Convert.ToInt64(ddlGrade.SelectedValue) &&
                                    s.TermId == Convert.ToInt64(ddlAcademicTerm.SelectedValue) &&
                                    s.SessionId == Convert.ToInt64(ddlAcademicSession.SelectedValue)).FirstOrDefault();
                                if (rpPoss == null) //add new data
                                {
                                    rpPos.AdmissionNumber = gstd.User.AdmissionNumber;
                                    rpPos.StudentId = gstd.User.Id;
                                    rpPos.TotalScore = aggScore;
                                    rpPos.SchoolId = schoolId;
                                    rpPos.CampusId = campusId;
                                    rpPos.YearId = Convert.ToInt64(ddlYear.SelectedValue);
                                    rpPos.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                                    rpPos.SessionId = Convert.ToInt64(ddlAcademicSession.SelectedValue);
                                    rpPos.TermId = Convert.ToInt64(ddlAcademicTerm.SelectedValue);
                                    rpPos.Date = DateTime.Today;
                                    rpPos.SubjectComputed = subjectCount;
                                    context.ReportCardPositions.InsertOnSubmit(rpPos);
                                    context.SubmitChanges();
                                }
                                else //update data
                                {
                                    rpPoss.AdmissionNumber = gstd.User.AdmissionNumber;
                                    rpPoss.StudentId = gstd.User.Id;
                                    rpPoss.TotalScore = aggScore;
                                    rpPoss.SchoolId = schoolId;
                                    rpPoss.CampusId = campusId;
                                    rpPoss.YearId = Convert.ToInt64(ddlYear.SelectedValue);
                                    rpPoss.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                                    rpPoss.SessionId = Convert.ToInt64(ddlAcademicSession.SelectedValue);
                                    rpPoss.TermId = Convert.ToInt64(ddlAcademicTerm.SelectedValue);
                                    rpPoss.Date = DateTime.Today;
                                    rpPoss.SubjectComputed = subjectCount;
                                    //context.ReportCardPositions.InsertOnSubmit(rpPoss);
                                    context.SubmitChanges();
                                }
                            }
                        }

                        //compute students class position 
                        IList<ReportCardPosition> classPos = (from s in context.ReportCardPositions
                                                              where
                                                                  s.GradeId == Convert.ToInt64(ddlGrade.SelectedValue) &&
                                                                  s.YearId == Convert.ToInt64(ddlYear.SelectedValue) &&
                                                                  s.SchoolId == schoolId &&
                                                                  s.CampusId == campusId &&
                                                                  s.SessionId == Convert.ToInt64(ddlAcademicSession.SelectedValue) &&
                                                                  s.TermId == Convert.ToInt64(ddlAcademicTerm.SelectedValue)
                                                              select s).ToList();
                        if (classPos.Count() > 0)
                        {
                            IList<ReportCardPosition> sortedClassPos = classPos.OrderByDescending(s => s.TotalScore).ToList(); //arrange the scores in descending order

                            int classPosition = 0; int countClassRec = 0; long? scorePrevClass = 0; int countClassScore = 0;
                            IDictionary<long?, long> map = new Dictionary<long?, long>();
                            foreach (ReportCardPosition classPoss in sortedClassPos) //itereate through the scores arranged in descending order
                            {
                                //classPosition++;
                                //countClassRec++;

                                    ReportCardPosition posObj = context.ReportCardPositions.FirstOrDefault(s => s.Id == classPoss.Id &&
                                    s.TotalScore == classPoss.TotalScore && s.SchoolId == classPoss.SchoolId && s.CampusId == campusId &&
                                    s.GradeId == classPoss.GradeId && s.YearId == classPoss.YearId &&
                                    s.TermId == classPoss.TermId && s.SessionId == classPoss.SessionId);
                            //posObj.Position = classPosition;


                            if (map.ContainsKey(classPoss.TotalScore) == true) // if the dictionary contains same totalScore increment countClassScore
                            {
                                countClassScore++;      //increment countClassScore
                                posObj.Position = classPosition;
                            }
                            else
                            {
                                if (countClassScore > 0)
                                {
                                    classPosition = classPosition + countClassScore;
                                    countClassScore = 0;
                                }
                                classPosition++;
                                posObj.Position = classPosition;
                                map.Add(classPoss.TotalScore, classPosition);
                            }


                            }
                            context.SubmitChanges();
                            map.Clear();
                            lblErrorMsg.Text = "Result Computed Successfully";
                            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                            lblErrorMsg.Visible = true;
                        }
                   // }
                    //lblErrorMsg.Text = "Result Computed Successfully";
                    //lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                    //lblErrorMsg.Visible = true;
                    //return;

                    //}else
                    //    {
                    //        lblErrorMsg.Text = "No data to be computed(test or exam score is missing)";
                    //        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    //        lblErrorMsg.Visible = true;
                    //    }
                }
            }
        }
    }
    protected void ddlGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddlSubject.Items.Clear();
        lblMsgDisplay.Visible = true;
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        IList<SubjectsInSchool> getId = new SubjectTeachersLIB().getAllSubjectsForClass((long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue));
        foreach (SubjectsInSchool subjId in getId)
        {
            PASSIS.LIB.Subject reqSubject = context.Subjects.FirstOrDefault(c => c.Id == subjId.SubjectId);
            //ddlSubject.Items.Add(new System.Web.UI.WebControls.ListItem(reqSubject.Name, reqSubject.Id.ToString()));

        }
        gdvAllSubject.DataSource = getId;
        gdvAllSubject.DataBind();
        gdvAllSubject.Visible = true;
    }

    public string subjectName(int subjectId)
    {
        PASSIS.LIB.Subject reqSubject = context.Subjects.FirstOrDefault(c => c.Id == subjectId);
        return reqSubject.Name;
    }


}
#endregion