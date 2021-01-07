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

public partial class SubjectScoreSheet : PASSIS.LIB.Utility.BasePage
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

            PASSIS.LIB.School objUser = context.Schools.FirstOrDefault(x => x.Id == logonUser.SchoolId);
            SchoolName = objUser.Name.ToString();
            if (objUser.Logo.ToString().Equals(""))
            {
                SchoolLogo = "~/Images/Schools_Logo/passis_logoB.png";
            }
            else
            {
                SchoolLogo = objUser.Logo.ToString();
            }
            SchoolAddress = objUser.Address.ToString();
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
        ddlSubject.Items.Clear();
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

    private iTextSharp.text.Image getBackgroundImage()
    {
        string imagepath = Server.MapPath(SchoolLogo); // "\\images\\";
        iTextSharp.text.Image backgroundLogo = iTextSharp.text.Image.GetInstance(imagepath);// + "/newhalllogo.png");
        backgroundLogo.ScaleToFit(70, 70);
        backgroundLogo.SetAbsolutePosition(255, 755);
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
            document.SetPageSize(iTextSharp.text.PageSize.A4);
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
            Paragraph SessionDetails = new Paragraph(string.Format("Scoresheet for {0} {1}", ddlYear.SelectedItem.Text, new PASSIS.LIB.AcademicSessionLIB().GetCurrentSession((long)logonUser.SchoolId)), blackFntB);
            SessionDetails.Alignment = Element.ALIGN_CENTER;
            document.Add(SessionDetails);
            Paragraph subject = new Paragraph(string.Format("Subject: {0}", ddlSubject.SelectedItem.Text));
            subject.Alignment = Element.ALIGN_CENTER;
            document.Add(subject);
            //document.Add(new Phrase(Environment.NewLine));

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

        #region
        Int64 sessionId = Convert.ToInt64(ddlAcademicSession.SelectedValue);
        Int64 TermId = Convert.ToInt64(ddlAcademicTerm.SelectedValue);

        Dictionary<Int64, IList<PASSIS.LIB.CustomClasses.ResultProperties>> resultProp_Dic = new Dictionary<Int64, IList<PASSIS.LIB.CustomClasses.ResultProperties>>();
        IList<PASSIS.LIB.CustomClasses.ResultProperties> resList;// = new List<ResultProperties>();
        Dictionary<Int64, ResultSummaryStatistics> resultStat_Dic = new Dictionary<Int64, ResultSummaryStatistics>();

        IList<PASSIS.LIB.Subject> getId = new SubjectTeachersLIB().getAllSubjectsInSchool(Convert.ToInt64(ddlYear.SelectedValue));
        int rows = 11;
        int totalAttainable = getId.Count * 100;
        PdfPTable resTable = new PdfPTable(rows);
        PdfPCell numberHdr = new PdfPCell(); numberHdr.AddElement(new Phrase(" S/N "));
        PdfPCell nameHdr = new PdfPCell(); nameHdr.Colspan = 5; nameHdr.AddElement(new Phrase("  Name  "));
        PdfPCell admissionNumberHdr = new PdfPCell(); admissionNumberHdr.AddElement(new Phrase("Admission Number")); admissionNumberHdr.Colspan = 2;
        PdfPCell CAHdr = new PdfPCell(); CAHdr.AddElement(new Phrase("C.A"));
        PdfPCell examHdr = new PdfPCell(); examHdr.AddElement(new Phrase("Exam"));
        PdfPCell totalScoreHdr = new PdfPCell(); totalScoreHdr.AddElement(new Phrase("Total"));
        resTable.AddCell(numberHdr);
        resTable.AddCell(nameHdr);
        resTable.AddCell(admissionNumberHdr);
        resTable.AddCell(CAHdr);
        resTable.AddCell(examHdr);
        resTable.AddCell(totalScoreHdr);
        Int64 stdCounter = 0;
        int totalMarkObtained = 0;
        int totalMarkObtainable = 0;
        decimal testScoreConfiguration = 0;
        decimal aggregateTestScore = 0;
        decimal totalScore = 0;
        decimal aggregateTotalScore = 0;
        foreach (PASSIS.LIB.GradeStudent std in new ClassGradeLIB().RetrieveGradeStudents(schoolId, campusId, yearId, gradeId, sessionId))
        {
            try
            {
                ++stdCounter;
                resTable.AddCell(new Phrase(stdCounter.ToString()));
                PdfPCell name = new PdfPCell(new Phrase(std.User.StudentFullName)); name.Colspan = 5;
                resTable.AddCell(name);
                PdfPCell admissionnumber = new PdfPCell(new Phrase(std.User.AdmissionNumber)); admissionnumber.Colspan = 2;
                resTable.AddCell(admissionnumber);
                PASSIS.LIB.StudentScore scor = new ScoresheetLIB().RetrieveStudentScoreBulkUploadTest(Convert.ToInt64(ddlSubject.SelectedValue), std.User.AdmissionNumber, sessionId, TermId);
                if (scor != null)
                {
                    try
                    {
                        IList<StudentScoreRepository> scoreRepository = new ScoresheetLIB().getScorelist(sessionId, TermId, Convert.ToInt64(ddlSubject.SelectedValue), std.User.AdmissionNumber, logonUser.SchoolCampusId);
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
                        decimal examScore = Convert.ToDecimal(scor.ExamScore);
                        totalScore = Math.Round(Convert.ToDecimal(aggregateTestScore) + examScore, 0);
                        resTable.AddCell(Math.Round(Convert.ToDecimal(aggregateTestScore), 0).ToString());
                        resTable.AddCell(Math.Round(examScore, 0).ToString());
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
                    resTable.AddCell("");
                }

                PdfPCell aggregateTotalScoreCell = new PdfPCell(new Phrase(aggregateTotalScore.ToString()));
                resTable.AddCell(aggregateTotalScoreCell);
                totalScore = 0;
                aggregateTotalScore = 0;
            }
            catch (Exception x) { throw x; }
        }
        #endregion

        document.Add(new Phrase(Environment.NewLine));
        document.Add(new Phrase(Environment.NewLine));
        //Paragraph schoolAddress = new Paragraph("Class Teacher Signature");
        //schoolAddress.Alignment = Element.ALIGN_CENTER;
        //document.Add(schoolAddress);

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
    protected void ddlGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        IList<SubjectsInSchool> getId = new SubjectTeachersLIB().getAllSubjectsForClass((long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue));
        foreach (SubjectsInSchool subjId in getId)
        {
            PASSIS.LIB.Subject reqSubject = context.Subjects.FirstOrDefault(c => c.Id == subjId.SubjectId);
            ddlSubject.Items.Add(new System.Web.UI.WebControls.ListItem(reqSubject.Name, reqSubject.Id.ToString()));
        }
    }
}