﻿using System;
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

public partial class BroadSheet : PASSIS.LIB.Utility.BasePage
{
    public static string SchoolName = "";
    public static string SchoolLogo = "";
    public static string SchoolAddress = "";
    public static string SchoolCurriculumId = "";
    public static string SchoolUrl = "";
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

           
            ddlAcademicTerm.DataSource = new AcademicTermLIB().RetrieveAcademicTerm();
            ddlAcademicTerm.DataBind();

            clsMyDB mdb = new clsMyDB();
            mdb.connct();
            string query = "SELECT * FROM Schools WHERE Id=" + logonUser.SchoolId;
            SqlDataReader reader = mdb.fetch(query);
            while (reader.Read())
            {
                SchoolName = reader[1].ToString();
                SchoolLogo = reader[5].ToString();
                SchoolAddress = reader[4].ToString();
                SchoolCurriculumId = reader[7].ToString();
                SchoolUrl = reader[6].ToString();
            }
            if (SchoolLogo == "") SchoolLogo = "~/Images/Schools_Logo/passis_logoB.png";


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
    }
    protected void btnPrintBroadsheet_OnClick(object sender, EventArgs e)
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
            throw ex;
        }
    }
    private Font darkerGrnFnt = FontFactory.GetFont(BaseFont.HELVETICA, 14, Font.BOLD, new BaseColor(26, 74, 17));
    private Font darkerRedFnt = FontFactory.GetFont(BaseFont.HELVETICA, 12, Font.BOLD, new BaseColor(169, 34, 82));
    private Font darkRedFnt = FontFactory.GetFont(BaseFont.HELVETICA, 12, Font.NORMAL, new BaseColor(169, 34, 82));
    private Font darkRedFnt11 = FontFactory.GetFont(BaseFont.HELVETICA, 10, Font.BOLD, new BaseColor(170, 38, 98));
    private Font darkerGrnFnt12 = FontFactory.GetFont(BaseFont.HELVETICA, 12, Font.BOLD, new BaseColor(26, 74, 17));
    private Font darkerGrnFnt9 = FontFactory.GetFont(BaseFont.HELVETICA, 10, Font.BOLD, new BaseColor(26, 74, 17));
    private Font grnFnt = FontFactory.GetFont(BaseFont.HELVETICA, 14, new BaseColor(26, 74, 17));
    private Font blackFnt = FontFactory.GetFont(BaseFont.HELVETICA, 14, new BaseColor(0, 0, 0));
    private Font blackFntB = FontFactory.GetFont(BaseFont.HELVETICA, 14, Font.BOLD, new BaseColor(0, 0, 0));
    private Font resultRedFnt = FontFactory.GetFont(BaseFont.HELVETICA, 12, new BaseColor(161, 13, 76));
    private Font resultRedFnt_b = FontFactory.GetFont(BaseFont.HELVETICA, 12, Font.BOLD, new BaseColor(161, 13, 76));
    private Font resultGrnFnt = FontFactory.GetFont(BaseFont.HELVETICA, 12, new BaseColor(170, 38, 98));
    private Font resultGrnFnt_b = FontFactory.GetFont(BaseFont.HELVETICA, 12, Font.BOLD, new BaseColor(170, 38, 98));
    private Font blackFnt12 = FontFactory.GetFont(BaseFont.HELVETICA, 12, new BaseColor(0, 0, 0));
    private Font blackFnt10 = FontFactory.GetFont(BaseFont.HELVETICA, 10, new BaseColor(0, 0, 0));
    private Font resultTitleRedFnt10 = FontFactory.GetFont(BaseFont.HELVETICA, 10, Font.BOLD, new BaseColor(161, 13, 76));
    private Font resultTitleRedFnt9 = FontFactory.GetFont(BaseFont.HELVETICA, 9, Font.BOLD, new BaseColor(161, 13, 76));
    private Font resultTitleRedFnt8 = FontFactory.GetFont(BaseFont.HELVETICA, 8, Font.BOLD, new BaseColor(161, 13, 76));
    private Font blackFnt8 = FontFactory.GetFont(BaseFont.HELVETICA, 8, new BaseColor(0, 0, 0));

    private iTextSharp.text.Image getBackgroundImage()
    {
        string imagepath = Server.MapPath(SchoolLogo); // "\\images\\";
        iTextSharp.text.Image backgroundLogo = iTextSharp.text.Image.GetInstance(imagepath);// + "/newhalllogo.png");
        backgroundLogo.ScaleToFit(70, 70);
        backgroundLogo.SetAbsolutePosition(370, 510);
        return backgroundLogo;
    }
    void printBroadSheet(Int64 yearId, Int64 gradeId, Int64 schoolId, Int64 campusId)
    {
        try
        {
            UsersLIB usrDal = new UsersLIB();

            Document document = new Document();    // instantiate a iTextSharp.text.pdf.Document
            document.SetMargins(0, 0, 30f, 0f);
            MemoryStream mem = new MemoryStream(); // PDF data will be written here
            PdfWriter.GetInstance(document, mem);  // tie a PdfWriter instance to the stream
            //document.SetPageSize(iTextSharp.text.PageSize.A3.Rotate());
            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            document.Open();
            BaseColor bcFaintRed = new BaseColor(169, 34, 82);
            //string imagepath = Server.MapPath("~");// +"\\images\\";
            ////iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(imagepath + "/greenLogo.png");
            //iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(imagepath + SchoolLogo);// + "/newhalllogo.png");
            //iTextSharp.text.Image backgroundLogo = iTextSharp.text.Image.GetInstance(imagepath + SchoolLogo);// + "/newhalllogo.png");
            //backgroundLogo.Alignment = iTextSharp.text.Image.UNDERLYING;
            ////If you want to give absolute/specified fix position to image.
            //backgroundLogo.SetAbsolutePosition(150, 5);
            //backgroundLogo.ScaleToFit(100, 100);
            //logo.SetAbsolutePosition(document.PageSize.Width - 36f - 72f, document.PageSize.Height - 36f - 100f);

            document.Add(getBackgroundImage());
            document.Add(new Phrase(Environment.NewLine));
            document.Add(new Phrase(Environment.NewLine));
            document.Add(new Phrase(Environment.NewLine));
            document.Add(new Phrase(Environment.NewLine));
            Paragraph schoolname = new Paragraph(string.Format("{0}", SchoolName), blackFntB);
            schoolname.Alignment = Element.ALIGN_CENTER;
            document.Add(schoolname);
            Paragraph schoolAddress = new Paragraph(string.Format("{0}", SchoolAddress));
            schoolAddress.Alignment = Element.ALIGN_CENTER;
            document.Add(schoolAddress);
            //Paragraph SessionDetails = new Paragraph(string.Format("Academic Broadsheet for {0} {1}",ddlYear.SelectedItem.Text, new PASSIS.LIB.AcademicSessionLIB().GetCurrentSession((long)logonUser.SchoolId)), blackFntB);
            Paragraph SessionDetails = new Paragraph(string.Format("Academic Broadsheet for {0} {1}", ddlYear.SelectedItem.Text, ddlAcademicTerm.SelectedItem.Text + " " + ddlAcademicSession.SelectedItem.Text + " Session"), blackFntB);
            SessionDetails.Alignment = Element.ALIGN_CENTER;
            document.Add(SessionDetails);
            document.Add(new Phrase(Environment.NewLine));

            addResultSummaryPage(document, yearId, schoolId, gradeId, campusId);


            document.Close();   // automatically closes the attached MemoryStream

            byte[] docData = mem.GetBuffer(); // get the generated PDF as raw data

            // write the document data to response stream and set appropriate headers:
            string filename = string.Format("{0}.pdf", DateTime.Now.Millisecond);
            //Response.AppendHeader("Content-Disposition", "attachment; filename=testdoc.pdf");
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(docData);
            Response.End();
        }
        catch (Exception ex) { throw ex; }
    }
    protected void addResultSummaryPage(Document document, Int64 yearId, Int64 schId, Int64 gradeId, Int64 campusId)
    {
        AddResultTableToDocument(document, yearId, gradeId, schId, campusId);
    }
    protected void AddResultTableToDocument(Document document, Int64 yearId, Int64 gradeId, Int64 schoolId, Int64 campusId)
    {
        try
        {
            #region
            Int64 sessionId = Convert.ToInt64(ddlAcademicSession.SelectedValue);
            Int64 TermId = Convert.ToInt64(ddlAcademicTerm.SelectedValue);

            Dictionary<Int64, IList<PASSIS.LIB.CustomClasses.ResultProperties>> resultProp_Dic = new Dictionary<Int64, IList<PASSIS.LIB.CustomClasses.ResultProperties>>();
            IList<PASSIS.LIB.CustomClasses.ResultProperties> resList;// = new List<ResultProperties>();
            Dictionary<Int64, ResultSummaryStatistics> resultStat_Dic = new Dictionary<Int64, ResultSummaryStatistics>();

            IList<PASSIS.LIB.Subject> getId = new SubjectTeachersLIB().getAllSubjectsInSchool(Convert.ToInt64(ddlYear.SelectedValue));
            int rows = (getId.Count) + 13;
            int totalAttainable = getId.Count * 100;
            PdfPTable resTable = new PdfPTable(rows);
            PdfPCell numberHdr = new PdfPCell(); numberHdr.AddElement(new Phrase("S/N", resultTitleRedFnt8)); numberHdr.Rotation = 90; numberHdr.Colspan = 2;
            PdfPCell nameHdr = new PdfPCell(); nameHdr.Colspan = 5; nameHdr.AddElement(new Phrase("Name", resultTitleRedFnt8)); nameHdr.Rotation = 90;
            PdfPCell admissionNumberHdr = new PdfPCell(); admissionNumberHdr.Colspan = 2; admissionNumberHdr.AddElement(new Phrase("Admission Number", resultTitleRedFnt8)); admissionNumberHdr.Rotation = 90;
            PdfPCell totalHdr = new PdfPCell(); totalHdr.AddElement(new Phrase("Total Score", resultTitleRedFnt8)); totalHdr.Rotation = 90; totalHdr.Colspan = 1;
            PdfPCell totalAttainableHdr = new PdfPCell(); totalAttainableHdr.Colspan = 2; totalAttainableHdr.AddElement(new Phrase("Total Attainable", resultTitleRedFnt8)); totalAttainableHdr.Rotation = 90;
            PdfPCell percentageHdr = new PdfPCell(); percentageHdr.AddElement(new Phrase("Percentage (%)", resultTitleRedFnt8)); percentageHdr.Rotation = 90; percentageHdr.Colspan = 1;
            resTable.AddCell(numberHdr);
            resTable.AddCell(nameHdr);
            resTable.AddCell(admissionNumberHdr);
            foreach (PASSIS.LIB.Subject sub in getId)
            {
                PASSIS.LIB.Subject reqSubject = context.Subjects.FirstOrDefault(c => c.Id == sub.Id);
                PdfPCell subjectHdr = new PdfPCell(); subjectHdr.Colspan = 1; //subjectHdr.Rotation = 90;
                string str = reqSubject.Name.Substring(0, 3);
                subjectHdr.AddElement(new Phrase(str, resultTitleRedFnt8));
                resTable.AddCell(subjectHdr);
            }
            resTable.AddCell(totalHdr);
            resTable.AddCell(totalAttainableHdr);
            resTable.AddCell(percentageHdr);
            Int64 stdCounter = 0;
            int totalMarkObtained = 0;
            int totalMarkObtainable = 0;
            decimal testScoreConfiguration = 0;
            decimal aggregateTestScore = 0;
            long totalScore = 0;
            long aggregateTotalScore = 0;
            decimal averageScore = 0;
            decimal percentageScore = 0;
            foreach (PASSIS.LIB.GradeStudent std in new ClassGradeLIB().RetrieveGradeStudents(schoolId, campusId, yearId, gradeId, sessionId))
            {
                try
                {
                    ++stdCounter;
                    PdfPCell NumberCell = new PdfPCell(); NumberCell.AddElement(new Phrase(stdCounter.ToString(), blackFnt8)); NumberCell.Colspan = 2;
                    resTable.AddCell(NumberCell);
                    PdfPCell name = new PdfPCell(new Phrase(std.User.StudentFullName, blackFnt8)); name.Colspan = 5;
                    resTable.AddCell(name);
                    PdfPCell admissionnumber = new PdfPCell(new Phrase(std.User.AdmissionNumber, blackFnt8)); admissionnumber.Colspan = 2;
                    resTable.AddCell(admissionnumber);
                    foreach (PASSIS.LIB.Subject sub in getId)
                    {
                        PASSIS.LIB.StudentScore scor = new ScoresheetLIB().RetrieveStudentScoreBulkUploadTest(Convert.ToInt64(sub.Id), std.User.AdmissionNumber, sessionId, TermId);
                        if (scor != null)
                        {
                            try
                            {
                                IList<StudentScoreRepository> scoreRepository = new ScoresheetLIB().getScorelist(sessionId, TermId, Convert.ToInt64(sub.Id), std.User.AdmissionNumber, logonUser.SchoolCampusId);
                                foreach (StudentScoreRepository scoreRepo in scoreRepository)
                                {
                                    totalMarkObtained += Convert.ToInt32(scoreRepo.MarkObtained);
                                    totalMarkObtainable += Convert.ToInt32(scoreRepo.MarkObtainable);
                                }
                                if (scoreRepository.Count > 0)
                                {
                                    ScoreConfiguration getScore = new ScoresheetLIB().getScoreConfiguration((long)logonUser.SchoolId, logonUser.SchoolCampusId, Convert.ToInt64(ddlYear.SelectedValue));
                                    testScoreConfiguration = Convert.ToDecimal(getScore.TestScore);
                                    aggregateTestScore = Convert.ToDecimal(totalMarkObtained) / Convert.ToDecimal(totalMarkObtainable) * testScoreConfiguration;
                                }
                                decimal? examScore = scor.ExamScore;
                                totalScore = Convert.ToInt64(Math.Round(Convert.ToDecimal(aggregateTestScore) + Convert.ToDecimal(examScore), 0));
                                PdfPCell totalCell = new PdfPCell(); totalCell.AddElement(new Phrase(totalScore.ToString(), blackFnt8)); totalCell.Colspan = 1;
                                resTable.AddCell(totalCell);
                                aggregateTotalScore += totalScore;
                                totalMarkObtainable = 0;
                                totalMarkObtained = 0;
                                aggregateTestScore = 0;
                            }
                            catch (Exception e) { resTable.AddCell(""); }
                        }
                        else
                        {
                            resTable.AddCell("");
                        }
                    }

                    PdfPCell aggregateTotalScoreCell = new PdfPCell(new Phrase(aggregateTotalScore.ToString(), blackFnt8)); aggregateTotalScoreCell.Colspan = 1;
                    resTable.AddCell(aggregateTotalScoreCell);
                    PdfPCell totalAttainableCell = new PdfPCell(new Phrase(totalAttainable.ToString(), blackFnt8)); totalAttainableCell.Colspan = 2; totalAttainableCell.VerticalAlignment = Element.ALIGN_CENTER;
                    resTable.AddCell(totalAttainableCell);
                    //averageScore = Math.Round(Convert.ToDecimal(aggregateTotalScore / getId.Count),0);
                    //PdfPCell averageScoreCell = new PdfPCell(new Phrase(averageScore.ToString()));
                    //resTable.AddCell(averageScoreCell);
                    percentageScore = Math.Round(Convert.ToDecimal(aggregateTotalScore) / Convert.ToDecimal(totalAttainable) * 100, 0);
                    PdfPCell percentageScoreCell = new PdfPCell(new Phrase(percentageScore.ToString(), blackFnt8)); percentageScoreCell.Colspan = 1;
                    resTable.AddCell(percentageScoreCell);
                    totalScore = 0;
                    aggregateTotalScore = 0;
                }
                catch (Exception x) { throw x; }
            }



            #endregion

            #region  result statistics table
            document.Add(resTable);
            PdfPTable summaryTable = new PdfPTable(10);
            iTextSharp.text.pdf.draw.LineSeparator line = new iTextSharp.text.pdf.draw.LineSeparator(0f, 100f, BaseColor.BLACK, Element.ALIGN_LEFT, 1);
            PdfPCell summaryRow2Cell1 = new PdfPCell(new Phrase("Class Teacher's Comment")); summaryRow2Cell1.Colspan = 4; summaryRow2Cell1.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow2Cell1.Border = 0;
            PdfPCell summaryRow2Cell2 = new PdfPCell(); summaryRow2Cell2.Colspan = 6; summaryRow2Cell2.AddElement(new Chunk(line)); summaryRow2Cell2.Border = 0;

            PdfPCell summaryRow3Cell1 = new PdfPCell(); summaryRow3Cell1.Colspan = 6; summaryRow3Cell1.AddElement(new Chunk(line)); summaryRow3Cell1.Border = 0;
            PdfPCell SummaryRow3cell2 = new PdfPCell(new Phrase("Signature:")); SummaryRow3cell2.Colspan = 2; SummaryRow3cell2.HorizontalAlignment = Element.ALIGN_LEFT; SummaryRow3cell2.Border = 0;
            PdfPCell SummaryRow3cell3 = new PdfPCell(); SummaryRow3cell3.Colspan = 2; SummaryRow3cell3.AddElement(new Chunk(line)); SummaryRow3cell3.HorizontalAlignment = Element.ALIGN_LEFT; SummaryRow3cell3.Border = 0;

            PdfPCell summaryRow4Cell1 = new PdfPCell(new Phrase("Proprietor's Comment")); summaryRow4Cell1.Colspan = 3; summaryRow4Cell1.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow4Cell1.Border = 0;
            PdfPCell summaryRow4Cell2 = new PdfPCell(); summaryRow4Cell2.Colspan = 7; summaryRow4Cell2.AddElement(new Chunk(line)); summaryRow4Cell2.Border = 0;

            PdfPCell summaryRow5Cell1 = new PdfPCell(); summaryRow5Cell1.Colspan = 6; summaryRow5Cell1.AddElement(new Chunk(line)); summaryRow5Cell1.Border = 0;
            PdfPCell SummaryRow5cell2 = new PdfPCell(new Phrase("Signature:")); SummaryRow5cell2.Colspan = 2; SummaryRow5cell2.HorizontalAlignment = Element.ALIGN_LEFT; SummaryRow5cell2.Border = 0;
            PdfPCell SummaryRow5cell3 = new PdfPCell(); SummaryRow5cell3.Colspan = 2; SummaryRow5cell3.AddElement(new Chunk(line)); SummaryRow5cell3.HorizontalAlignment = Element.ALIGN_LEFT; SummaryRow5cell3.Border = 0;

            summaryTable.AddCell(summaryRow2Cell1); summaryTable.AddCell(summaryRow2Cell2);
            summaryTable.AddCell(summaryRow3Cell1); summaryTable.AddCell(SummaryRow3cell2); summaryTable.AddCell(SummaryRow3cell3);
            summaryTable.AddCell(summaryRow4Cell1); summaryTable.AddCell(summaryRow4Cell2);
            summaryTable.AddCell(summaryRow5Cell1); summaryTable.AddCell(SummaryRow5cell2); summaryTable.AddCell(SummaryRow5cell3);
            document.Add(summaryTable);


            //pple wt A        
            document.Add(new Phrase(Environment.NewLine));
            #endregion



            //return resTable;

        }
        catch (Exception ex)
        {
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Source, ex.StackTrace);
            lblErrorMsg.Text = "Error occurred";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            throw ex;
        }
    }

}