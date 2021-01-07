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
using System.Data.SqlClient;
using PASSIS.LIB;

public partial class CbtResult : PASSIS.LIB.Utility.BasePage
{
    //protected void Page_PreInit(object sender, EventArgs e)
    //{
    //    base.Page_PreInit(sender, e);
    //    this.MasterPageFile = "~/SchlAdminRepCard.master";
    //}

    #region viewstate
    protected decimal yearGPASum_VS
    {
        get
        {
            if (ViewState["::yearGPASum_VS::"] == null)
                ViewState["::yearGPASum_VS::"] = 0m;
            return Convert.ToDecimal(ViewState["::yearGPASum_VS::"]);
        }
        set
        {
            ViewState["::yearGPASum_VS::"] = value;
        }
    }

    #endregion
    Int64 subjectCounter = 0;
    Int64 distinctionCounter = 0;
    Int64 creditCounter = 0;
    Int64 passesCounter = 0;
    Int64 failureCounter = 0;
    public static string SchoolName = "";
    public static string SchoolLogo = "";
    public static string Passport = "";
    public static string SchoolAddress = "";
    public static string SchoolCurriculumId = "";
    public static string SchoolUrl = "";
    IList<PASSIS.LIB.User> students;
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {

        Response.Redirect("CbtResult-" + logonUser.SchoolId + ".aspx");

        //mainview.Visible = true;
        if (!IsPostBack)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            //User currentUser = logonUser;
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
            if (SchoolCurriculumId == "") SchoolCurriculumId = "0";


            var academicSessions = (from c in context.AcademicSessions
                                    where c.SchoolId == logonUser.SchoolId
                                    orderby c.IsCurrent == true/*&& c.IsCurrent == true*/
                                    select new
                                    {
                                        c.AcademicSessionName.SessionName

                                    }).Distinct();

            ddlAcademicSession.DataSource = new CbtResult().schSession().Distinct();
            ddlAcademicSession.DataTextField = "SessionName";
            ddlAcademicSession.DataValueField = "ID";
            ddlAcademicSession.DataBind();
            ddlAcademicSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Session--", "0", true));

            ddlType.DataSource = new CbtResult().cbtType();
            ddlType.DataTextField = "Type";
            ddlType.DataValueField = "Id";
            ddlType.DataBind();
            ddlType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Type--", "0", true));

            ddlCategory.DataSource = new CbtResult().cbtCategory();
            ddlCategory.DataTextField = "Category";
            ddlCategory.DataValueField = "Id";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Category--", "0", true));

            //ddlCurrentSession.DataSource = new PrintResultt_119().schSession().Distinct();
            //ddlCurrentSession.DataTextField = "SessionName";
            //ddlCurrentSession.DataValueField = "ID";
            //ddlCurrentSession.DataBind();
            //ddlCurrentSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Session--", "0", true));
            //ddlReportType.Items.Add(new System.Web.UI.WebControls.ListItem("Mid Term", "1"));
            //ddlReportType.Items.Add(new System.Web.UI.WebControls.ListItem("End of the Term", "2"));

            //Olayemi
            //var sessions = from c in context.AcademicSessions

            //                where c.SchoolId == logonUser.SchoolId

            //                orderby c.IsCurrent descending

            //                select c.AcademicSessionName;

            //ddlAcademicSession.DataSource = sessions;
            //ddlAcademicSession.DataTextField = "SessionName";
            //ddlAcademicSession.DataBind();
            //ddlAcademicSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Session--", "0", true));

            //ddlYear.DataSource = new PASSIS.DAO.SchoolConfigDAL().getAllClass_Grade();
            //ddlYear.DataBind();
            //dsClass.SelectCommand = "SELECT * FROM Class_Grade WHERE CurriculumId=" + SchoolCurriculumId;
            //BindGrid();

            PASSIS.LIB.User currentUser = logonUser;
            long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
            long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
            if (curriculumId == (long)CurriculumType.British)
            {
                ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, 0);
                //ddlCurrentYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, 0);
            }
            else if (curriculumId == (long)CurriculumType.Nigerian)
            {
                ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
                //ddlCurrentYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);

            }
            ddlYear.DataBind();
            //ddlCurrentYear.DataBind();

            //ddlAcademicTerm.DataSource = new AcademicTermLIB().RetrieveAcademicTerm();
            //ddlAcademicTerm.DataBind();



            var academicTermCurrent = (from c in context.AcademicSessions
                                       where c.IsCurrent == true
                                       select new
                                       {
                                           c.AcademicTerm1

                                       }).Distinct();


            ddlAcademicTerm.DataSource = new AcademicTermLIB().RetrieveAcademicTerm();
            ddlAcademicTerm.DataTextField = "AcademicTermName";
            ddlAcademicTerm.DataValueField = "Id";
            ddlAcademicTerm.DataBind();
            ddlAcademicTerm.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Term--", "0", true));

        }
    }

    public enum CurriculumType
    {
        British = 1,
        Nigerian = 2
    }
    public string getGender(int gender)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        var Gender = context.Genders.SingleOrDefault(x => x.Id == gender).Name;
        return Gender.ToString();
    }
    protected void ddlYear_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Int64 selectedYearId = 0L;
        selectedYearId = Convert.ToInt64(ddlYear.SelectedValue);
        ddlGrade.Items.Clear();
        //var availableGrades = new ClassGradeDAL().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, selectedYearId);
        var availableGrades = new ClassGradeLIB().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, selectedYearId);
        ddlGrade.DataSource = availableGrades;
        ddlGrade.DataBind();
        ddlGrade.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Grade--", "0", true));
    }
    protected void BindGrid(long yearId, long gradeId)
    {
        long campusId, schoolId;
        campusId = logonUser.SchoolCampusId;
        schoolId = (long)logonUser.SchoolId;
        long termId = Convert.ToInt64(ddlAcademicTerm.SelectedValue);
        long sessionId = Convert.ToInt64(ddlAcademicSession.SelectedValue);
        //Int64.TryParse(ddlSubject.SelectedValue, out subjectId);
        //Int64.TryParse(ddlYear.SelectedValue, out yearID);
        //Int64.TryParse(ddlGrade.SelectedValue, out gradeId);
        //var scoreList = new ClassGradeLIB().RetrieveGradeStudents(schoolId, campusId, yearId, gradeId, sessionID);
        //var scoreList = new ClassGradeLIB().RetrieveGradeStudents(schoolId, campusId, yearId, gradeId, Convert.ToInt64(ddlAcademicSession.SelectedValue));


        if (logonUser.SchoolId == 119)
        {
            students = new PASSIS.LIB.UsersLIB().RetrieveStudents(schoolId, campusId, yearId, gradeId, sessionId);
            //gdvList.DataSource = RetrieveStudentFromCA(termId, sessionId, schoolId, campusId, yearId, gradeId);
            gdvList.DataSource = students;
            gdvList.DataBind();
        }
        else
        {
            gdvList.DataSource = RetrieveStudentFromReport(termId, sessionId, schoolId, campusId, yearId, gradeId);
            gdvList.DataBind();
        }
        //RePopulateCheckBoxes();
    }
    public const string SELECTED_CUSTOMERS_INDEX = "SelectedCustomersIndex";
    private List<Int32> SelectedCustomersIndex
    {
        get
        {
            if (ViewState[SELECTED_CUSTOMERS_INDEX] == null)
            {
                ViewState[SELECTED_CUSTOMERS_INDEX] = new List<Int32>();
            }

            return (List<Int32>)ViewState[SELECTED_CUSTOMERS_INDEX];
        }
    }
    private void RePopulateCheckBoxes()
    {
        foreach (GridViewRow row in gdvList.Rows)
        {
            var chkBox = row.FindControl("chkSelect") as CheckBox;

            IDataItemContainer container = (IDataItemContainer)chkBox.NamingContainer;

            if (SelectedCustomersIndex != null)
            {
                if (SelectedCustomersIndex.Exists(i => i == container.DataItemIndex))
                {
                    chkBox.Checked = true;
                }
            }
        }
    }
    private void RemoveRowIndex(int index)
    {
        SelectedCustomersIndex.Remove(index);
    }
    private void PersistRowIndex(int index)
    {
        if (!SelectedCustomersIndex.Exists(i => i == index))
        {
            SelectedCustomersIndex.Add(index);
        }
    }
    protected void gdvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        foreach (GridViewRow row in gdvList.Rows)
        {
            var chkBox = row.FindControl("chkSelect") as CheckBox;

            IDataItemContainer container = (IDataItemContainer)chkBox.NamingContainer;

            if (chkBox.Checked)
            {
                PersistRowIndex(container.DataItemIndex);
            }
            else
            {
                RemoveRowIndex(container.DataItemIndex);
            }
        }
        gdvList.PageIndex = e.NewPageIndex;
        BindGrid(Convert.ToInt64(ddlYear.SelectedValue), Convert.ToInt64(ddlGrade.SelectedValue));
    }
    protected void btnPrintAll_OnClick(object sender, EventArgs e)
    {
        long campusId, schoolId, subjectId, yearID, gradeId, termId, sessionId = 0;
        campusId = logonUser.SchoolCampusId;
        schoolId = (long)logonUser.SchoolId;
        termId = Convert.ToInt64(ddlAcademicTerm.SelectedValue);
        sessionId = Convert.ToInt64(ddlAcademicSession.SelectedValue);
        //Int64.TryParse(ddlSubject.SelectedValue, out subjectId);
        Int64.TryParse(ddlYear.SelectedValue, out yearID);
        Int64.TryParse(ddlGrade.SelectedValue, out gradeId);
        //var scoreList = new ClassGradeLIB().RetrieveGradeStudents(schoolId, campusId, yearID, gradeId);
        //var selectedUsers = from u in scoreList select u.User;
        //bulkPrinting(selectedUsers.ToList<PASSIS.LIB.User>());

        //bulkPrinting(RetrieveStudentFromReport(termId, sessionId, schoolId, campusId, yearID, gradeId));
        //bulkPrinting(students);

        try
        {
            IList<PASSIS.LIB.User> selectedUsers = new List<PASSIS.LIB.User>();
            UsersLIB userLib = new UsersLIB();
            foreach (GridViewRow row in gdvList.Rows)
            {
                Label lblStudentId = row.FindControl("lblStudentId") as Label;
                Int64 Id = Convert.ToInt64(lblStudentId.Text.Trim());

                selectedUsers.Add(userLib.RetrieveUser(Id));

            }
            bulkPrinting(selectedUsers);
        }
        catch (Exception ex)
        {

            //throw ex;
        }
    }

    public IList<PASSIS.LIB.User> RetrieveStudentFromReport(long termId, long sessionId, long schoolId, long campusId, long yearId, long gradeId)
    {
        IList<PASSIS.LIB.User> stdUserList = new List<PASSIS.LIB.User>();
        var students = from s in context.ReportCardPositions
                       where s.TermId == termId && s.SessionId == sessionId && s.SchoolId == schoolId &&
                           s.CampusId == campusId && s.GradeId == gradeId && s.YearId == yearId
                       select s;
        IList<ReportCardPosition> studentsList = students.ToList<ReportCardPosition>();

        foreach (ReportCardPosition pos in studentsList)
        {

            PASSIS.LIB.User std = context.Users.FirstOrDefault(x => x.AdmissionNumber == pos.AdmissionNumber && x.Id == pos.StudentId);
            stdUserList.Add(std);
        }

        return stdUserList;
    }
    public IList<PASSIS.LIB.User> RetrieveStudentFromCA(long termId, long sessionId, long schoolId, long campusId, long yearId, long gradeId)
    {
        IList<PASSIS.LIB.User> stdUserList = new List<PASSIS.LIB.User>();
        var students = (from s in context.StudentScoreCAs
                        where s.TermId == termId && s.AcademicSessionID == sessionId && s.SchoolId == schoolId &&
                            s.CampusId == campusId && s.GradeId == gradeId && s.ClassId == yearId
                        select s).Distinct();
        IList<StudentScoreCA> studentsList = students.ToList<StudentScoreCA>();

        foreach (StudentScoreCA pos in studentsList)
        {

            PASSIS.LIB.User std = context.Users.FirstOrDefault(x => x.AdmissionNumber == pos.AdmissionNumber && x.Id == pos.StudentId);
            stdUserList.Add(std);
        }

        return stdUserList;
    }

    protected void btnPrintSelection_OnClick(object sender, EventArgs e)
    {

        try
        {
            IList<PASSIS.LIB.User> selectedUsers = new List<PASSIS.LIB.User>();
            UsersLIB userLib = new UsersLIB();
            foreach (GridViewRow row in gdvList.Rows)
            {
                CheckBox chkSelectStudent = row.FindControl("chkSelect") as CheckBox;
                Label lblStudentId = row.FindControl("lblStudentId") as Label;
                Int64 Id = Convert.ToInt64(lblStudentId.Text.Trim());

                if (chkSelectStudent.Checked)
                {
                    selectedUsers.Add(userLib.RetrieveUser(Id));
                }

            }
            bulkPrinting(selectedUsers);
        }
        catch (Exception ex)
        {

            //throw ex;
        }
    }
    protected void gdvList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        long rowItemId;
        long.TryParse(e.CommandArgument.ToString(), out rowItemId);

        switch (e.CommandName)
        {

            case "report":
                IList<PASSIS.LIB.User> list = new List<PASSIS.LIB.User>();
                list.Add(new UsersLIB().RetrieveUser(rowItemId));
                bulkPrinting(list);
                //Download();
                break;
            case "ViewReport":
                //PASSISLIBDataContext context = new PASSISLIBDataContext();
                //mainview.Visible = false;
                //reportview.Visible = true;
                //long termId = Convert.ToInt64(ddlAcademicTerm.SelectedValue);
                //long academicSessionId = Convert.ToInt64(ddlAcademicSession.SelectedValue);
                //string admNumber = e.CommandArgument.ToString();
                //long gradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                //lblAdmissionNumber.Text = admNumber.ToString();
                //lblClass.Text = new ClassGradeLIB().RetrieveGrade(gradeId).Class_Grade.Name;
                //lblGrade.Text = new ClassGradeLIB().RetrieveGrade(gradeId).GradeName;
                //lblSession.Text = ddlAcademicSession.SelectedItem.Text;
                //lblTerm.Text = ddlAcademicTerm.SelectedItem.Text;
                //PASSIS.LIB.User userObj = new PASSIS.LIB.User();
                //userObj = context.Users.FirstOrDefault(x => x.AdmissionNumber == admNumber);
                //lblFullname.Text = userObj.StudentFullName;
                //gdvViewResult.DataSource = new ScoresheetLIB().RetrieveStudentScores(termId, academicSessionId, admNumber, gradeId);
                //gdvViewResult.DataBind();
                break;
        }
    }
    //new Font(Font.HELVETICA, 8f, Font.NORMAL, Color.YELLOW)));
    //Font grnFnt = FontFactory.GetFont("Verdana",14,BaseColor.GREEN.Darker());
    private Font darkerGrnFnt = FontFactory.GetFont(BaseFont.HELVETICA, 14, Font.BOLD, new BaseColor(26, 74, 17));
    //Font darkerRedFnt = FontFactory.GetFont("Verdana", 12, Font.BOLD, BaseColor.RED);
    private Font darkerRedFnt = FontFactory.GetFont(BaseFont.HELVETICA, 12, Font.BOLD, new BaseColor(169, 34, 82));
    private Font darkerRedFnt16 = FontFactory.GetFont(BaseFont.HELVETICA, 16, Font.BOLD, new BaseColor(169, 34, 82));
    private Font darkRedFnt = FontFactory.GetFont(BaseFont.HELVETICA, 12, Font.NORMAL, new BaseColor(169, 34, 82));
    private Font darkRedFnt11 = FontFactory.GetFont(BaseFont.HELVETICA, 10, Font.BOLD, new BaseColor(170, 38, 98));
    private Font darkerGrnFnt12 = FontFactory.GetFont(BaseFont.HELVETICA, 12, Font.BOLD, new BaseColor(26, 74, 17));
    private Font darkerGrnFnt8 = FontFactory.GetFont(BaseFont.HELVETICA, 8, Font.BOLD, new BaseColor(26, 74, 17));
    private Font grnFnt = FontFactory.GetFont(BaseFont.HELVETICA, 14, new BaseColor(26, 74, 17));
    private Font blackFntB = FontFactory.GetFont(BaseFont.HELVETICA, 14, Font.BOLD, new BaseColor(0, 0, 0));
    private Font blackFnt8 = FontFactory.GetFont(BaseFont.HELVETICA, 8, new BaseColor(0, 0, 0));
    private Font resultRedFnt = FontFactory.GetFont(BaseFont.HELVETICA, 12, new BaseColor(161, 13, 76));
    private Font resultRedFnt_b = FontFactory.GetFont(BaseFont.HELVETICA, 12, Font.BOLD, new BaseColor(161, 13, 76));
    private Font resultGrnFnt = FontFactory.GetFont(BaseFont.HELVETICA, 12, new BaseColor(170, 38, 98));
    private Font resultGrnFnt_b = FontFactory.GetFont(BaseFont.HELVETICA, 12, Font.BOLD, new BaseColor(170, 38, 98));
    private Font blackFnt12 = FontFactory.GetFont(BaseFont.HELVETICA, 12, new BaseColor(0, 0, 0));
    private Font resultTitleRedFnt10 = FontFactory.GetFont(BaseFont.HELVETICA, 10, Font.BOLD, new BaseColor(161, 13, 76));
    private Font resultTitleRedFnt9 = FontFactory.GetFont(BaseFont.HELVETICA, 9, Font.BOLD, new BaseColor(161, 13, 76));
    private Font resultTitleRedFnt8 = FontFactory.GetFont(BaseFont.HELVETICA, 8, Font.BOLD, new BaseColor(161, 13, 76));
    private Font white = FontFactory.GetFont(BaseFont.TIMES_ROMAN, Font.NORMAL, new BaseColor(255, 255, 255));
    //private iTextSharp.text.Image getBackgroundImage()
    //{
    //    string imagepath = Server.MapPath(SchoolLogo);// +"\\images\\";
    //    iTextSharp.text.Image backgroundLogo = iTextSharp.text.Image.GetInstance(imagepath);
    //    backgroundLogo.ScaleToFit(70, 70);
    //    backgroundLogo.Alignment = iTextSharp.text.Image.UNDERLYING;
    //    backgroundLogo.SetAbsolutePosition(250, 750);
    //    return backgroundLogo;
    //}
    void Download(PASSIS.LIB.User student, Document document, UsersLIB usrDal)
    {
        //try
        //{
        //PASSIS.DAO.User student = usrDal.RetrieveByAdmissionNumber(admNumber);
        var scoresinEachSubject = new ScoresheetLIB().ReportCard_SubjectScore(student.AdmissionNumber);

        BaseColor bcFaintRed = new BaseColor(169, 34, 82);
        string imagepath = Server.MapPath(SchoolLogo); // "\\images\\";
        //iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(imagepath + "/greenLogo.png");
        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(imagepath);
        iTextSharp.text.Image backgroundLogo = iTextSharp.text.Image.GetInstance(imagepath);
        //Resize image depend upon your need
        //For give the size to image //backgroundLogo.ScaleToFit(150, 375);

        addResultSummaryPageUnilag(document, student, usrDal);
        //}
        //catch (Exception ex)
        //{
        //    PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
        //}
    }
    public void bulkPrinting(IList<PASSIS.LIB.User> selectedUsers)
    {
        Document document = new Document();    // instantiate a iTextSharp.text.pdf.Document
        document.SetMargins(0f, 20f, 30f, 0f);
        MemoryStream mem = new MemoryStream(); // PDF data will be written here
        PdfWriter.GetInstance(document, mem);  // tie a PdfWriter instance to the stream
        document.SetPageSize(iTextSharp.text.PageSize.A4_LANDSCAPE);
        document.Open();
        UsersLIB usrdal = new UsersLIB();
        foreach (PASSIS.LIB.User stdnt in selectedUsers)
        {
            Download(stdnt, document, usrdal);
            document.NewPage();
        }
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

    protected void addResultSummaryPageUnilag(Document document, PASSIS.LIB.User student, UsersLIB usrDal)
    {

        iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(Server.MapPath(SchoolLogo));
        jpg.ScaleToFit(70, 70);
        jpg.Alignment = iTextSharp.text.Image.UNDERLYING;
        jpg.SetAbsolutePosition(250, 750);

        //Passport = student.PassportFileName;
        //if (Passport == null) { Passport = "~/Images/student3.PNG"; }
        //iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(Server.MapPath(Passport));
        //jpg1.ScaleToFit(70, 70);
        //jpg1.Alignment = iTextSharp.text.Image.UNDERLYING;
        //jpg1.SetAbsolutePosition(250, 750);
        PdfPTable innerTable = new PdfPTable(9);
        PdfPCell innerCell1 = new PdfPCell(new Phrase("LIGHT HOUSE SCHOOL", darkerRedFnt16)); innerCell1.Colspan = 9; innerCell1.Border = 0; innerCell1.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell innerCell2 = new PdfPCell(new Phrase("www.lighthouseschool.com", darkerGrnFnt8)); innerCell2.Colspan = 9; innerCell2.Border = 0; innerCell2.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell innerCell3 = new PdfPCell(new Phrase("Address: Plot 242, Kofo Abayomi, VI, Lagos ", darkerGrnFnt8)); innerCell3.Colspan = 9; innerCell3.Border = 0; innerCell3.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell innerCell4 = new PdfPCell(new Phrase("Email: lighthouseschool@gmail.com", blackFnt8)); innerCell4.Colspan = 9; innerCell4.Border = 0; innerCell4.HorizontalAlignment = Element.ALIGN_CENTER;
        innerTable.AddCell(innerCell1);
        innerTable.AddCell(innerCell2);
        innerTable.AddCell(innerCell3);
        innerTable.AddCell(innerCell4);
        document.Add(innerTable);
        Paragraph transcript = new Paragraph(string.Format("{0}", "ACADEMIC TRANSCRIPT"), darkerRedFnt);
        transcript.Alignment = Element.ALIGN_CENTER;
        document.Add(transcript);
        PdfPTable head = new PdfPTable(15);
        PdfPCell head1 = new PdfPCell(jpg); head1.Colspan = 3; head1.Border = 0;
        PdfPCell head2 = new PdfPCell(innerTable); head2.Colspan = 9; head2.Border = 0;
        //PdfPCell head3 = new PdfPCell(jpg1); head3.Colspan = 3; head3.HorizontalAlignment = Element.ALIGN_RIGHT; head3.Border = 0;


        head.AddCell(head1);
        head.AddCell(head2);
        //head.AddCell(head3);
        //document.Add(head);
        //document.Add(getBackgroundImage());
        //document.Add(new Phrase(Environment.NewLine));
        //document.Add(new Phrase(Environment.NewLine));
        //document.Add(new Phrase(Environment.NewLine));
        //document.Add(new Phrase(Environment.NewLine));
        //document.Add(new Phrase(Environment.NewLine));
        Paragraph schoolname = new Paragraph(string.Format("{0}", SchoolName), darkerRedFnt);
        schoolname.Alignment = Element.ALIGN_CENTER;
        //document.Add(schoolname);
        Paragraph schoolAddress = new Paragraph(string.Format("{0}", SchoolAddress), darkerRedFnt);
        schoolAddress.Alignment = Element.ALIGN_CENTER;
        //document.Add(schoolAddress);
        Paragraph p = new Paragraph("Report Card For", darkerRedFnt);
        p.Alignment = Element.ALIGN_CENTER;
        //document.Add(p);
        Paragraph SessionDetails = new Paragraph(string.Format("{0} {1} {2} Session", ddlAcademicTerm.SelectedItem.Text, ",", ddlAcademicSession.SelectedItem.Text), darkerRedFnt);
        SessionDetails.Alignment = Element.ALIGN_CENTER;
        //document.Add(SessionDetails);


        long session = Convert.ToInt64(ddlAcademicSession.SelectedValue);
        long term = Convert.ToInt64(ddlAcademicTerm.SelectedValue);
        long? schoolId = logonUser.SchoolId;
        long yearId = Convert.ToInt64(ddlYear.SelectedValue);
        long gradeIdd = Convert.ToInt64(ddlGrade.SelectedValue);

        long typeId = Convert.ToInt64(ddlType.SelectedValue);

        long? schId = logonUser.SchoolId;
        long gradeId = new CbtResult().theGradeId(student.Id, session).GradeId;
        string noInClass = noOfStudentInClass(schId, gradeId).Count.ToString();
        string classPosition = "";
        var posInClass = getClassPosition(student.AdmissionNumber, term, session, schoolId, yearId, gradeIdd);
        if (posInClass != null)
        {
            classPosition = posInClass.Position.ToString();
        }
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);
        string totalNoInClass = new ClassGradeLIB().RetrieveGradeStudents(Convert.ToInt64(schId), Convert.ToInt64(logonUser.SchoolCampusId), Convert.ToInt64(yearId), gradeIdd, session).Count().ToString();

        PdfPTable maintbl = new PdfPTable(9);
        PdfPCell cell1 = new PdfPCell(new Phrase("Name", blackFnt8)); cell1.Colspan = 1; cell1.HorizontalAlignment = Element.ALIGN_LEFT; //cell1.Border = 0;
        PdfPCell cell2 = new PdfPCell(new Phrase(student.StudentFullName, blackFnt8)); cell2.Colspan = 2; cell1.HorizontalAlignment = Element.ALIGN_LEFT; //cell2.Border = 0;
        PdfPCell cell3 = new PdfPCell(new Phrase("Class", blackFnt8)); cell3.Colspan = 1; cell3.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell cell4 = new PdfPCell(new Phrase(usrDal.getStudentsHomeRoom(student.Id), blackFnt8)); cell4.Colspan = 1; cell4.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell cell5 = new PdfPCell(new Phrase("Age", blackFnt8)); cell5.Colspan = 1; cell5.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell cell6 = new PdfPCell(new Phrase(GetAge(Convert.ToDateTime(student.DateOfBirth)).ToString(), blackFnt8)); cell6.Colspan = 1; cell6.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell cell7 = new PdfPCell(new Phrase("Sex", blackFnt8)); cell5.Colspan = 1; cell5.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell cell8 = new PdfPCell(new Phrase(getGender(student.Gender), blackFnt8)); cell6.Colspan = 1; cell6.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;

        PdfPCell row2cell1 = new PdfPCell(new Phrase("SESSION", blackFnt8)); row2cell1.HorizontalAlignment = Element.ALIGN_LEFT; row2cell1.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell row2cell2 = new PdfPCell(new Phrase(ddlAcademicSession.SelectedItem.Text, blackFnt8)); row2cell2.Colspan = 2; row2cell2.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        PdfPCell row2cell3 = new PdfPCell(new Phrase("TERM", blackFnt8)); row2cell3.HorizontalAlignment = Element.ALIGN_LEFT; row2cell3.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell row2cell4 = new PdfPCell(new Phrase(ddlAcademicTerm.SelectedItem.Text, blackFnt8)); row2cell4.Colspan = 1; row2cell4.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        PdfPCell row2cell5 = new PdfPCell(new Phrase("No. in Class", blackFnt8)); row2cell5.HorizontalAlignment = Element.ALIGN_LEFT; row2cell5.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell row2cell6 = new PdfPCell(new Phrase(totalNoInClass, blackFnt8)); row2cell6.Colspan = 1; row2cell6.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        PdfPCell row2cell7 = new PdfPCell(new Phrase("Position", blackFnt8)); row2cell1.HorizontalAlignment = Element.ALIGN_LEFT; row2cell7.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell row2cell8 = new PdfPCell(new Phrase("", blackFnt8)); row2cell8.Colspan = 1; row2cell8.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;

       
        Paragraph domain = new Paragraph(string.Format("{0}", "CBT " + ddlType.SelectedItem.Text.ToUpper() + " RESULT"), darkerRedFnt);
        domain.Alignment = Element.ALIGN_CENTER;
        Paragraph domain2 = new Paragraph(string.Format("{0}", ""), darkerRedFnt);
        domain2.Alignment = Element.ALIGN_CENTER;




        maintbl.AddCell(cell1);
        maintbl.AddCell(cell2);
        maintbl.AddCell(cell3);
        maintbl.AddCell(cell4);
        maintbl.AddCell(cell5);
        maintbl.AddCell(cell6);
        maintbl.AddCell(cell7);
        maintbl.AddCell(cell8);

        maintbl.AddCell(row2cell1);
        maintbl.AddCell(row2cell2);
        maintbl.AddCell(row2cell3);
        maintbl.AddCell(row2cell4);
        maintbl.AddCell(row2cell5);
        maintbl.AddCell(row2cell6);
        maintbl.AddCell(row2cell7);
        maintbl.AddCell(row2cell8);

        //maintbl.AddCell(row3cell1);
        //maintbl.AddCell(row3cell2);
        //maintbl.AddCell(row3cell3);
        //maintbl.AddCell(row3cell4);

        //maintbl.AddCell(row4cell1);
        //maintbl.AddCell(row4cell2);
        //maintbl.AddCell(row4cell3);
        //maintbl.AddCell(row4cell4);
        //maintbl.AddCell(row4cell5);




        ///PdfPTable resultTable = getResultTable(student);/// dont add the table yet so as to retrieve the gpa viewstate value needed in the gpa summ ary slot 
        /// 
        Int64 sessionId = Convert.ToInt64(ddlAcademicSession.SelectedIndex);
        Int64 TermId = Convert.ToInt64(ddlAcademicTerm.SelectedIndex);
        PdfPTable resTable = new PdfPTable(12);
        //resTable.SetWidthPercentage(new float[8] { 80f, 30f, 30f, 30f, 35f, 35f, 38f, 30f }, PageSize.A5.Rotate());
        //resTable.TotalWidth = 400f;
        //resTable.WidthPercentage = 80;
        //resTable.LockedWidth = true;
        //PdfPCell deptHdr = new PdfPCell(new Phrase("DEPARTMENT", resultTitleRedFnt10)); deptHdr.Padding = 0f;
        //deptHdr.Colspan = 2; deptHdr.HorizontalAlignment = Element.ALIGN_CENTER; deptHdr.VerticalAlignment = Element.ALIGN_BOTTOM;
        //resTable.AddCell(deptHdr);
        PdfPCell subjectHead = new PdfPCell(new Phrase("SUBJECTS", resultTitleRedFnt10)); subjectHead.Padding = 0f;
        subjectHead.Colspan = 3; subjectHead.HorizontalAlignment = Element.ALIGN_CENTER; subjectHead.VerticalAlignment = Element.ALIGN_BOTTOM;
        resTable.AddCell(subjectHead);
        PdfPCell testHead1 = new PdfPCell(new Phrase("ASSESSMENT NAME", resultTitleRedFnt8)); testHead1.Colspan = 2; testHead1.VerticalAlignment = Element.ALIGN_BOTTOM; testHead1.HorizontalAlignment = Element.ALIGN_LEFT;
        resTable.AddCell(testHead1);
        //PdfPCell testHead2 = new PdfPCell(new Phrase("DURATION", resultTitleRedFnt8)); testHead2.Colspan = 2; testHead2.VerticalAlignment = Element.ALIGN_BOTTOM; testHead2.HorizontalAlignment = Element.ALIGN_LEFT;
        //resTable.AddCell(testHead2);
        PdfPCell examHead = new PdfPCell(); examHead.Colspan = 2; examHead.HorizontalAlignment = Element.ALIGN_LEFT; examHead.VerticalAlignment = Element.ALIGN_BOTTOM; examHead.AddElement(new Phrase("PASSMARK", resultTitleRedFnt8)); resTable.AddCell(examHead);
        PdfPCell totalHead = new PdfPCell(); totalHead.Colspan = 2; totalHead.HorizontalAlignment = Element.ALIGN_LEFT; totalHead.VerticalAlignment = Element.ALIGN_BOTTOM; totalHead.AddElement(new Phrase("SCORE", resultTitleRedFnt8)); resTable.AddCell(totalHead);
        PdfPCell gradeHead = new PdfPCell(); gradeHead.Colspan = 1; gradeHead.HorizontalAlignment = Element.ALIGN_CENTER; gradeHead.VerticalAlignment = Element.ALIGN_BOTTOM; gradeHead.AddElement(new Phrase("SCORE%", resultTitleRedFnt8)); resTable.AddCell(gradeHead);
        PdfPCell commentHdr = new PdfPCell(); commentHdr.Colspan = 2; commentHdr.HorizontalAlignment = Element.ALIGN_CENTER; commentHdr.VerticalAlignment = Element.ALIGN_BOTTOM; commentHdr.AddElement(new Phrase("REMARK", resultTitleRedFnt8)); resTable.AddCell(commentHdr);
        //PdfPCell space = new PdfPCell(); space.Colspan = 3;
        PdfPCell space2 = new PdfPCell(); space2.Colspan = 2;
        PdfPCell space1 = new PdfPCell(); space1.Colspan = 1;
        //PdfPCell testMax1 = new PdfPCell(new Phrase("20")); testMax1.Colspan = 2;
        //PdfPCell testMax2 = new PdfPCell(new Phrase("20")); testMax2.Colspan = 2;
        //PdfPCell examMax = new PdfPCell(new Phrase("60")); examMax.Colspan = 2;
        //PdfPCell total = new PdfPCell(new Phrase("100")); total.Colspan = 2;
        //resTable.AddCell(space);
        //resTable.AddCell(testMax1);
        //resTable.AddCell(testMax2);
        //resTable.AddCell(examMax);
        //resTable.AddCell(total);
        //resTable.AddCell(space1);
        //resTable.AddCell(space2);

        PASSIS.LIB.GradeStudent gs = new ClassGradeLIB().RetrieveStudentGrade(student.Id, session);

        //Int64 subjectCounter = 0;
        int? curricullumId = 0;
        int totalMarkObtained = 0;
        int totalMarkObtainable = 0;
        decimal testScoreConfiguration = 0;
        decimal cw = 0;
        decimal hw = 0;
        decimal note = 0;
        decimal testScore = 0;
        decimal? examScore = 0m;
        decimal totalScore = 0;

        decimal cbtFinalScore = 0;


        decimal examScoreObtained = 0;
        long examCatPercentage = 0;
        decimal examPercentageScore = 0;
        decimal testScoreObtained = 0;
        long testCatPercentage = 0;
        decimal testPercentageScore = 0;

        decimal totalScoreAverage = 0;
        decimal scorePosition = 0;
        decimal? aggregateTotalScore = 0;
        string subjName = "";
        string deptName = "";
        decimal? percentage = 0m;
        long subjectId = 0;
        Dictionary<long, string> dictionary = new Dictionary<long, string>();
        PASSIS.LIB.Class_Grade classGrade = context.Class_Grades.FirstOrDefault(x => x.Id == Convert.ToInt64(ddlYear.SelectedValue));
        if (classGrade != null)
        {
            curricullumId = classGrade.CurriculumId;
        }

        if (gs != null)
        {
            //IList<PASSIS.LIB.Subject> getSub = getSubject(session, term, schoolId, yearId, gradeIdd);
            //foreach (PASSIS.LIB.Subject sub in getSub) 
            //{
            //    subjectCounter++;
            //}
            var deptList = from s in context.SubjectDepartments where s.SchoolId == logonUser.SchoolId select s;
            decimal? totalTest = -1;
            decimal? tScore = -1;
            TeacherLIB techLib = new TeacherLIB();
            IList<PASSIS.LIB.Subject> subjectInClass = techLib.GetAllSubjectInClass((int)curricullumId, yearId, (long)logonUser.SchoolId);

            IList<PASSIS.LIB.Subject> AllSubject = new List<PASSIS.LIB.Subject>();
            IList<SubjectsInSchool> getSubjectInClass = (from s in context.SubjectsInSchools where s.SchoolId == schoolId select s).ToList();
            //IList<SubjectsInSchool> sortedTScore = getSubjectInClass.OrderBy(c => c.ReportCardOrder).ToList();


            foreach (SubjectsInSchool subjects in getSubjectInClass)
            {
                PASSIS.LIB.Subject subject = context.Subjects.FirstOrDefault(x =>
                                  x.CurriculumId == (int)curricullumId &&
                                  x.ClassId == yearId && x.Id == subjects.SubjectId);
                if (subject != null)
                {
                    AllSubject.Add(subject);


                    IList<PASSIS.LIB.StudentScoreRepository> scoreRepo = getSubjectScoreCategoryCA(student.AdmissionNumber, student.Id, session, term, schoolId, yearId, gradeId, subject.Id);
                    IList<PASSIS.LIB.StudentScoreRepository> sortedScoreRepoByCategory = scoreRepo.OrderBy(x => x.SubCategoryId).ToList();

                    PASSIS.LIB.CbtAssessmentRecord cbtScores = getCbtScores(student.Id, session, term, schoolId, yearId, gradeId, subject.Id, typeId);


                    PdfPCell subjCell = new PdfPCell(new Phrase(subject.Name, darkerGrnFnt8)); subjCell.Colspan = 3;
                    resTable.AddCell(subjCell);

                    if (cbtScores != null)
                    {
                        PdfPCell assessmentNameCell = new PdfPCell(new Phrase(cbtScores.CbtExamination.Name, darkerGrnFnt8)); assessmentNameCell.Colspan = 2;
                        resTable.AddCell(assessmentNameCell);

                        PdfPCell passMarkCell = new PdfPCell(new Phrase(cbtScores.CbtExamination.Passmark.ToString(), darkerGrnFnt8)); passMarkCell.Colspan = 2;
                        resTable.AddCell(passMarkCell);

                        //Calculates the student score from the percentage score and passmark
                        cbtFinalScore = Convert.ToDecimal((cbtScores.ScorePercentage / 100) * cbtScores.CbtExamination.Passmark);

                        PdfPCell scoreCell = new PdfPCell(new Phrase(Math.Round(cbtFinalScore, 0).ToString(), darkerGrnFnt8)); scoreCell.Colspan = 2;
                        resTable.AddCell(scoreCell);

                        PdfPCell scorePercentCell = new PdfPCell(new Phrase(Math.Round(Convert.ToDecimal(cbtScores.ScorePercentage), 0).ToString()+ "%", blackFnt8)); scorePercentCell.Colspan = 1;
                        resTable.AddCell(scorePercentCell);

                        PdfPCell statusCell = new PdfPCell(new Phrase(cbtScores.Status, blackFnt8)); statusCell.Colspan = 2;
                        resTable.AddCell(statusCell);
                    }
                    else
                    {
                        PdfPCell assessmentNameCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); assessmentNameCell.Colspan = 2;
                        resTable.AddCell(assessmentNameCell);

                        PdfPCell passMarkCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); passMarkCell.Colspan = 2;
                        resTable.AddCell(passMarkCell);

                        PdfPCell scoreCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); scoreCell.Colspan = 2;
                        resTable.AddCell(scoreCell);

                        PdfPCell scorePercentCell = new PdfPCell(new Phrase("", blackFnt8)); scorePercentCell.Colspan = 1;
                        resTable.AddCell(scorePercentCell);

                        PdfPCell statusCell = new PdfPCell(new Phrase("", blackFnt8)); statusCell.Colspan = 2;
                        resTable.AddCell(statusCell);
                    }

                                      
                    if (tScore > 0)
                    {
                        subjectCounter++;
                    }
                    if (tScore == -1) { tScore = 0; }
                    aggregateTotalScore += tScore;
                    totalTest = -1;
                    tScore = -1;
                }

            }

            if (subjectCounter != 0)
            {
                percentage = Math.Round(Convert.ToDecimal((aggregateTotalScore / (subjectCounter * 100)) * 100), 2);
            }

        }

        document.Add(maintbl);
        document.Add(domain);
        document.Add(domain2);
        document.Add(resTable);


    }



    protected void lnkReturn_Click(object sender, EventArgs e)
    {
        //mainview.Visible = true;
        //reportview.Visible = false;
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {

    }

    public List<PASSIS.LIB.GradeStudent> noOfStudentInClass(long? schoolId, Int64 classId)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        //PASSIS.LIB.User user = new PASSIS.LIB.User();
        //var grdStudents = user.GradeStudents.FirstOrDefault(x => x.SchoolId == schoolId && x.GradeId == classId);
        var grdStudents = from gs in context.GradeStudents where gs.SchoolId == schoolId && gs.GradeId == classId select gs;
        return grdStudents.ToList<PASSIS.LIB.GradeStudent>();
    }
    public PASSIS.LIB.GradeStudent theGradeId(Int64 studentId, Int64 sessionId)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        PASSIS.LIB.User user = new PASSIS.LIB.User();
        PASSIS.LIB.GradeStudent gradeId = context.GradeStudents.SingleOrDefault(x => x.StudentId == studentId && x.AcademicSessionId == sessionId);
        return gradeId;
    }

    protected void ddlAcademicTerm_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void ddlGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAcademicSession.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select academic session";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }

        if (ddlAcademicTerm.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select term";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }


        BindGrid(Convert.ToInt64(ddlYear.SelectedValue), Convert.ToInt64(ddlGrade.SelectedValue));
        lblMsgDisplay.Text = "LIST OF STUDENTS FOR " + " " + ddlAcademicSession.SelectedItem.Text + " " + " ACADEMIC SESSION";
        lblMsgDisplay.Visible = true;
        btnApprove.Visible = true;
        btnPrintAll.Visible = true;
    }

    protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAcademicTerm.SelectedIndex > 0 && ddlYear.SelectedIndex > 0 && ddlGrade.SelectedIndex > 0)
        {
            BindGrid(Convert.ToInt64(ddlYear.SelectedValue), Convert.ToInt64(ddlGrade.SelectedValue));
            lblMsgDisplay.Text = "LIST OF STUDENTS FOR " + " " + ddlAcademicSession.SelectedItem.Text + " " + " ACADEMIC SESSION";
            lblMsgDisplay.Visible = true;
            btnApprove.Visible = true;
            btnPrintAll.Visible = true;
        }
    }
    public IList<AcademicSessionName> schSession()
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        var session = from c in context.AcademicSessions
                      where c.SchoolId == logonUser.SchoolId
                      orderby c.IsCurrent descending
                      select c.AcademicSessionName;
        return session.ToList<AcademicSessionName>();
    }

    public IList<CbtType> cbtType()
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        var getCbtType = from c in context.CbtTypes select c;
        return getCbtType.ToList();
    }

    public IList<CbtCategory> cbtCategory()
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        var getCbtCategories = from c in context.CbtCategories select c;
        return getCbtCategories.ToList();
    }

    public IList<AcademicTerm1> schTerm()
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        var session = from c in context.AcademicSessions
                      where c.SchoolId == logonUser.SchoolId
                      //orderby c.IsCurrent descending
                      select c.AcademicTerm1;
        return session.ToList<AcademicTerm1>();
    }



    public static CbtAssessmentRecord getCbtScores(long studentId, long sessionId, long termId, long? schId, long yearId, long gradeId, long subId, long typeId)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        CbtAssessmentRecord score = context.CbtAssessmentRecords.FirstOrDefault(s =>
                                                              s.StudentId == studentId &&
                                                              s.SessionId == sessionId &&
                                                              s.TermId == termId &&
                                                              s.SchoolId == schId &&
                                                              s.ClassId == yearId &&
                                                              s.GradeId == gradeId &&
                                                              s.CbtExamination.SubjectId == subId &&
                                                              s.TypeId == typeId);
        return score;
    }

    public static IList<StudentScoreCA> getSubjectScoreCA(string admNo, long studentId, long sessionId, long termId, long? schId, long yearId, long gradeId, long deptId)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        IList<StudentScoreCA> score = (from s in context.StudentScoreCAs
                                       where s.AdmissionNumber == admNo &&
                                           s.StudentId == studentId &&
                                           s.AcademicSessionID == sessionId &&
                                           s.TermId == termId &&
                                           s.SchoolId == schId &&
                                           s.ClassId == yearId &&
                                           s.GradeId == gradeId &&
                                           s.DepartmentId == deptId
                                       select s).ToList();
        return score.ToList<StudentScoreCA>();
    }
    public static IList<PASSIS.LIB.StudentScoreRepository> getSubjectScoreCategoryCA(string admNo, long studentId, long sessionId, long termId, long? schId, long yearId, long gradeId, long subId)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        IList<PASSIS.LIB.StudentScoreRepository> score = (from s in context.StudentScoreRepositories
                                                          where s.AdmissionNo == admNo &&
                                                              s.StudentId == studentId &&
                                                              s.SessionId == sessionId &&
                                                              s.TermId == termId &&
                                                              s.SchoolId == schId &&
                                                              s.ClassId == yearId &&
                                                              s.GradeId == gradeId &&
                                                              s.SubjectId == subId
                                                          select s).ToList();
        return score.ToList<PASSIS.LIB.StudentScoreRepository>();
    }
    public static PASSIS.LIB.StudentScore getSubjectScoreCategoryExam(string admNo, long studentId, long sessionId, long termId, long? schId, long yearId, long gradeId, long subId)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        PASSIS.LIB.StudentScore score = context.StudentScores.FirstOrDefault(s =>
                                                          s.AdmissionNumber == admNo &&
                                                              s.StudentId == studentId &&
                                                              s.AcademicSessionID == sessionId &&
                                                              s.TermId == termId &&
                                                              s.SchoolId == schId &&
                                                              s.ClassId == yearId &&
                                                              s.GradeId == gradeId &&
                                                              s.SubjectId == subId);
        return score;
    }

    public static IList<ReportCardData> getSubjectScorePosition(string admNo, long sessionId, long termId, long? schId, long yearId, long gradeId)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        IList<ReportCardData> score = (from s in context.ReportCardDatas
                                       where s.AdmissionNumber == admNo &&
                                           s.SessionId == sessionId &&
                                           s.TermId == termId &&
                                           s.SchoolId == schId &&
                                           s.YearId == yearId &&
                                           s.GradeId == gradeId
                                       select s).ToList();
        return score.ToList<ReportCardData>();
    }

    public ReportCardData getSubjectScorePosition2(string admNo, long sessionId, long termId, long? schId, long yearId, long gradeId, long subjectId)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        ReportCardData score = new ReportCardData();
        score = context.ReportCardDatas.FirstOrDefault(s =>
                                          s.AdmissionNumber == admNo &&
                                          s.SessionId == sessionId &&
                                          s.TermId == termId &&
                                          s.SchoolId == schId &&
                                          s.YearId == yearId &&
                                          s.GradeId == gradeId &&
                                          s.SubjectId == subjectId);
        //if (score == null)
        //{
        //    return 0;
        //}
        //else
        //{
        return score;
        //}
    }

    public ReportCardPosition getClassPosition(string admNo, long termId, long sessionId, long? schId, long yearId, long gradeId)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        ReportCardPosition getPosition = context.ReportCardPositions.FirstOrDefault(s => s.AdmissionNumber == admNo &&
            s.TermId == termId && s.SessionId == sessionId && s.SchoolId == schId && s.YearId == yearId && s.GradeId == gradeId);

        return getPosition;
    }

    public ReportCardData getSubjectAnnualScore(string admNo, long sessionId, long termId, long? schId, long yearId, long gradeId, long subjectId)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        ReportCardData score = context.ReportCardDatas.FirstOrDefault(s =>
                                           s.AdmissionNumber == admNo &&
                                           s.SessionId == sessionId &&
                                           s.TermId == termId &&
                                           s.SchoolId == schId &&
                                           s.YearId == yearId &&
                                           s.GradeId == gradeId &&
                                           s.SubjectId == subjectId);
        return score;
    }

    public static IList<PASSIS.LIB.Subject> getSubject(long sessionId, long termId, long? schId, long yearId, long gradeId)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        IList<PASSIS.LIB.Subject> score = (from s in context.ReportCardDatas
                                           where
                                               s.SessionId == sessionId &&
                                               s.TermId == termId &&
                                               s.SchoolId == schId &&
                                               s.YearId == yearId &&
                                               s.GradeId == gradeId
                                           select s.Subject).ToList();
        return score.ToList<PASSIS.LIB.Subject>();
    }

    public static string getExamGradeRemarks(decimal sc, long schId, long classId)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();

        var score = context.ScoreGradeConfigurations.FirstOrDefault(s => s.LowestRange <= sc &&
                        s.HighestRange >= sc &&
                        s.SchoolId == schId &&
                        s.ClassId == classId).Remark;


        //var scoreRemark = from getGrade in context.ScoreGradeConfigurations
        //                 where sc >= getGrade.LowestRange &&
        //                     sc <= getGrade.HighestRange &&
        //                     getGrade.SchoolId == schId &&
        //                     getGrade.ClassId == classId
        //                 select getGrade.Remark;
        return score.ToString();
    }

    public string ToOrdinal(int value)
    {
        // Start with the most common extension.
        string extension = "th";

        // Examine the last 2 digits.
        int last_digits = value % 100;

        // If the last digits are 11, 12, or 13, use th. Otherwise:
        if (last_digits < 11 || last_digits > 13)
        {
            // Check the last digit.
            switch (last_digits % 10)
            {
                case 1:
                    extension = "st";
                    break;
                case 2:
                    extension = "nd";
                    break;
                case 3:
                    extension = "rd";
                    break;
            }
        }

        return extension;
    }

    public int GetAge(DateTime Dob)
    {
        DateTime now = DateTime.Today;
        int age = now.Year - Dob.Year;
        if (Dob > now.AddYears(-age)) age--;

        return age;
    }

    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chk = (sender as CheckBox);
            if (chk.ID == "chkAll")
            {
                foreach (GridViewRow row in gdvList.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        row.Cells[1].Controls.OfType<CheckBox>().FirstOrDefault().Checked = chk.Checked;
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

    //protected void ddlCurrentYear_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Int64 selectedYearId = 0L;
    //    selectedYearId = Convert.ToInt64(ddlCurrentYear.SelectedValue);
    //    ddlCurrentGrade.Items.Clear();
    //    //var availableGrades = new ClassGradeDAL().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, selectedYearId);
    //    var availableGrades = new ClassGradeLIB().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, selectedYearId);
    //    ddlCurrentGrade.DataSource = availableGrades;
    //    ddlCurrentGrade.DataBind();
    //    ddlCurrentGrade.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Grade--", "0", true));
    //}
    //protected void ddlCurrentGrade_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (ddlAcademicTerm.SelectedIndex == 0)
    //    {
    //        lblErrorMsg.Text = "Kindly select term";
    //        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
    //        lblErrorMsg.Visible = true;
    //        return;
    //    }


    //    BindGrid(Convert.ToInt64(ddlCurrentYear.SelectedValue), Convert.ToInt64(ddlCurrentGrade.SelectedValue));
    //    btnApprove.Visible = true;
    //    btnPrintAll.Visible = true;
    //}
}