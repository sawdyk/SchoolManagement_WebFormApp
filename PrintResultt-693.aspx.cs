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

public partial class PrintResultt_693 : PASSIS.LIB.Utility.BasePage
{
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

                                    where c.SchoolId == logonUser.SchoolId  orderby c.IsCurrent == true /*&& c.IsCurrent == true*/

                                    orderby c.IsCurrent descending

                                    select new
                                    {
                                        c.AcademicSessionName.SessionName

                                    }).Distinct();

            ddlAcademicSession.DataSource = new PrintResultt_693().schSession().Distinct();
            ddlAcademicSession.DataTextField = "SessionName";
            ddlAcademicSession.DataValueField = "ID";
            ddlAcademicSession.DataBind();
            ddlAcademicSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Session--", "0", true));

            //ddlCurrentSession.DataSource = new PrintResultt_693().schSession().Distinct();
            //ddlCurrentSession.DataTextField = "SessionName";
            //ddlCurrentSession.DataValueField = "ID";
            //ddlCurrentSession.DataBind();
            //ddlCurrentSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Session--", "0", true));
            //ddlReportType.Items.Add(new System.Web.UI.WebControls.ListItem("Mid Term", "1"));
            //ddlReportType.Items.Add(new System.Web.UI.WebControls.ListItem("End of the Term", "2"));



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

            var academicTermCurrent = (from c in context.AcademicSessions
                                       where c.IsCurrent == true
                                       select new
                                       {
                                           c.AcademicTerm1

                                       }).Distinct();


            ddlAcademicTerm.DataSource = new PrintResultt_693().schTerm().Distinct();
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
        var availableGrades = new ClassGradeDAL().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, selectedYearId);
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

       // var scoreList = new ClassGradeLIB().RetrieveGradeStudents(schoolId, campusId, yearId, gradeId, sessionId);

        if (logonUser.SchoolId == 693)
        {
            students = new PASSIS.LIB.UsersLIB().RetrieveStudents(schoolId, campusId, yearId, gradeId, sessionId);
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

    //protected void BindGrid(long yearId, long gradeId)
    //{
    //    long campusId, schoolId;
    //    campusId = logonUser.SchoolCampusId;
    //    schoolId = (long)logonUser.SchoolId;
    //    long termId = Convert.ToInt64(ddlAcademicTerm.SelectedValue);
    //    long sessionId = Convert.ToInt64(ddlAcademicSession.SelectedValue);
    //    //Int64.TryParse(ddlSubject.SelectedValue, out subjectId);
    //    //Int64.TryParse(ddlYear.SelectedValue, out yearID);
    //    //Int64.TryParse(ddlGrade.SelectedValue, out gradeId);
    //    //var scoreList = new ClassGradeLIB().RetrieveGradeStudents(schoolId, campusId, yearId, gradeId, Convert.ToInt64(ddlCurrentSession.SelectedValue));

    //    if (logonUser.SchoolId == 693)
    //    //if (logonUser.SchoolId == 119)
    //    {
    //        students = new PASSIS.LIB.UsersLIB().RetrieveStudents(schoolId, campusId, yearId, gradeId);
    //        //gdvList.DataSource = RetrieveStudentFromCA(termId, sessionId, schoolId, campusId, yearId, gradeId);
    //        gdvList.DataSource = students;
    //        gdvList.DataBind();
    //    }
    //    else
    //    {
    //        gdvList.DataSource = RetrieveStudentFromReport(termId, sessionId, schoolId, campusId, yearId, gradeId);
    //        gdvList.DataBind();
    //    }
    //    //RePopulateCheckBoxes();
    //}




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
    private Font darkerGrnFnt6 = FontFactory.GetFont(BaseFont.HELVETICA, 6, Font.BOLD, new BaseColor(26, 74, 17));
    private Font darkerRedFnt16 = FontFactory.GetFont(BaseFont.HELVETICA, 16, Font.BOLD, new BaseColor(169, 34, 82));
    private Font darkRedFnt = FontFactory.GetFont(BaseFont.HELVETICA, 12, Font.NORMAL, new BaseColor(169, 34, 82));
    private Font darkRedFnt11 = FontFactory.GetFont(BaseFont.HELVETICA, 10, Font.BOLD, new BaseColor(170, 38, 98));
    private Font darkerGrnFnt12 = FontFactory.GetFont(BaseFont.HELVETICA, 12, Font.BOLD, new BaseColor(26, 74, 17));
    private Font darkerGrnFnt9 = FontFactory.GetFont(BaseFont.HELVETICA, 10, Font.BOLD, new BaseColor(26, 74, 17));
    private Font grnFnt = FontFactory.GetFont(BaseFont.HELVETICA, 14, new BaseColor(26, 74, 17));
    private Font blackFnt8 = FontFactory.GetFont(BaseFont.HELVETICA, 6, new BaseColor(0, 0, 0));
    private Font blackFntB = FontFactory.GetFont(BaseFont.HELVETICA, 14, Font.BOLD, new BaseColor(0, 0, 0));
    private Font blackFnt = FontFactory.GetFont(BaseFont.HELVETICA, 10, new BaseColor(0, 0, 0));
    private Font resultRedFnt = FontFactory.GetFont(BaseFont.HELVETICA, 12, new BaseColor(161, 13, 76));
    private Font resultRedFnt_b = FontFactory.GetFont(BaseFont.HELVETICA, 12, Font.BOLD, new BaseColor(161, 13, 76));
    private Font resultGrnFnt = FontFactory.GetFont(BaseFont.HELVETICA, 12, new BaseColor(170, 38, 98));
    private Font resultGrnFnt_b = FontFactory.GetFont(BaseFont.HELVETICA, 12, Font.BOLD, new BaseColor(170, 38, 98));
    private Font blackFnt12 = FontFactory.GetFont(BaseFont.HELVETICA, 12, new BaseColor(0, 0, 0));
    private Font resultTitleRedFnt10 = FontFactory.GetFont(BaseFont.HELVETICA, 10, Font.BOLD, new BaseColor(161, 13, 76));
    private Font resultTitleRedFnt9 = FontFactory.GetFont(BaseFont.HELVETICA, 9, Font.BOLD, new BaseColor(161, 13, 76));
    private Font resultTitleRedFnt8 = FontFactory.GetFont(BaseFont.HELVETICA, 8, Font.BOLD, new BaseColor(161, 13, 76));
    private Font blackFnt6 = FontFactory.GetFont(BaseFont.HELVETICA, 6, new BaseColor(0, 0, 0));
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
        //if (ddlReportType.SelectedIndex == 0)
        //{
        //    lblErrorMsg.Text = "Kindly select the report type";
        //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
        //    lblErrorMsg.Visible = true;
        //    return;
        //}
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

        if (ddlAcademicTerm.SelectedValue == "1" || ddlAcademicTerm.SelectedValue == "2")
        {
            addResultSummaryPageCrownEndFirstAndSecondTerm(document, student, usrDal);
        }
        else if (ddlAcademicTerm.SelectedValue == "3")
        {
            addResultSummaryPageCrownEndTermNew(document, student, usrDal);
        }



        //}
        //catch (Exception ex)
        //{
        //    PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
        //}
    }
    public void bulkPrinting(IList<PASSIS.LIB.User> selectedUsers)
    {
        Document document = new Document();    // instantiate a iTextSharp.text.pdf.Document
        document.SetMargins(0f, 10f, 10f, 0f);
        MemoryStream mem = new MemoryStream(); // PDF data will be written here
        PdfWriter.GetInstance(document, mem);  // tie a PdfWriter instance to the stream
        document.SetPageSize(iTextSharp.text.PageSize.A4_LANDSCAPE.Rotate());
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

    protected void addResultSummaryPageCrownMidTerm(Document document, PASSIS.LIB.User student, UsersLIB usrDal)
    {

        iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(Server.MapPath(SchoolLogo));
        jpg.ScaleToFit(70, 70);
        jpg.Alignment = iTextSharp.text.Image.UNDERLYING;
        jpg.SetAbsolutePosition(250, 750);

        Passport = student.PassportFileName;
        if (Passport == null) { Passport = "~/Images/student3.PNG"; }
        iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(Server.MapPath(Passport));
        jpg1.ScaleToFit(70, 70);
        jpg1.Alignment = iTextSharp.text.Image.UNDERLYING;
        jpg1.SetAbsolutePosition(250, 750);
        PdfPTable innerTable = new PdfPTable(9);
        PdfPCell innerCell1 = new PdfPCell(new Phrase("CROWN JEWEL COLLEGE", darkerRedFnt16)); innerCell1.Colspan = 9; innerCell1.Border = 0; innerCell1.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell innerCell2 = new PdfPCell(new Phrase("9, Bakky Street, Off Bello Folawiyo Street, Ikosi, Ketu, Lagos ", darkerGrnFnt9)); innerCell2.Colspan = 9; innerCell2.Border = 0; innerCell2.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell innerCell3 = new PdfPCell(new Phrase("Phone: 08067864385, 08023070027, 08038796369", blackFnt)); innerCell3.Colspan = 9; innerCell3.Border = 0; innerCell3.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell innerCell4 = new PdfPCell(new Phrase("Email: crownjewelschools@yahoo.com, FB: @crownjewelcolleg", blackFnt)); innerCell4.Colspan = 9; innerCell4.Border = 0; innerCell4.HorizontalAlignment = Element.ALIGN_CENTER;
        innerTable.AddCell(innerCell1);
        innerTable.AddCell(innerCell2);
        innerTable.AddCell(innerCell3);
        innerTable.AddCell(innerCell4);
        PdfPTable head = new PdfPTable(15);
        PdfPCell head1 = new PdfPCell(jpg); head1.Colspan = 3; head1.Border = 0;
        PdfPCell head2 = new PdfPCell(innerTable); head2.Colspan = 9; head2.Border = 0;
        PdfPCell head3 = new PdfPCell(jpg1); head3.Colspan = 3; head3.HorizontalAlignment = Element.ALIGN_RIGHT; head3.Border = 0;


        head.AddCell(head1);
        head.AddCell(head2);
        head.AddCell(head3);
        document.Add(head);
        //document.Add(getBackgroundImage());
        //document.Add(new Phrase(Environment.NewLine));
        //document.Add(new Phrase(Environment.NewLine));
        //document.Add(new Phrase(Environment.NewLine));
        //document.Add(new Phrase(Environment.NewLine));
        document.Add(new Phrase(Environment.NewLine));
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

        long? schId = logonUser.SchoolId;
        long gradeId = new PrintResultt_693().theGradeId(student.Id, session).GradeId;
        string noInClass = noOfStudentInClass(schId, gradeId).Count.ToString();
        string classPosition = "";
        var posInClass = getClassPosition(student.AdmissionNumber, term, session, schoolId, yearId, gradeIdd);
        if (posInClass != null)
        {
            classPosition = posInClass.Position.ToString();
        }


        PdfPTable maintbl = new PdfPTable(8);
        PdfPCell cell1 = new PdfPCell(new Phrase("NAME", blackFnt)); cell1.Colspan = 1; cell1.HorizontalAlignment = Element.ALIGN_LEFT; //cell1.Border = 0;
        PdfPCell cell2 = new PdfPCell(new Phrase(student.StudentFullName, blackFnt)); cell2.Colspan = 3; cell1.HorizontalAlignment = Element.ALIGN_LEFT; //cell2.Border = 0;
        PdfPCell cell3 = new PdfPCell(new Phrase("SEX", blackFnt)); cell3.Colspan = 1; cell3.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell cell4 = new PdfPCell(new Phrase(getGender(student.Gender), blackFnt)); cell4.Colspan = 1; cell4.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell cell5 = new PdfPCell(new Phrase("HOUSE", blackFnt)); cell5.Colspan = 1; cell5.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell cell6 = new PdfPCell(new Phrase(student.SchoolHouse, blackFnt)); cell6.Colspan = 1; cell6.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;

        PdfPCell row2cell1 = new PdfPCell(new Phrase("CLASS", blackFnt)); row2cell1.HorizontalAlignment = Element.ALIGN_LEFT; row2cell1.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell row2cell2 = new PdfPCell(new Phrase(usrDal.getStudentsHomeRoom(student.Id), blackFnt)); row2cell2.Colspan = 1; row2cell2.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        PdfPCell row2cell3 = new PdfPCell(new Phrase("AGE", blackFnt)); row2cell3.HorizontalAlignment = Element.ALIGN_LEFT; row2cell3.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell row2cell4 = new PdfPCell(new Phrase(GetAge(Convert.ToDateTime(student.DateOfBirth)).ToString(), blackFnt)); row2cell4.Colspan = 1; row2cell4.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        PdfPCell row2cell5 = new PdfPCell(new Phrase("SESSION", blackFnt)); row2cell5.HorizontalAlignment = Element.ALIGN_LEFT; row2cell5.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell row2cell6 = new PdfPCell(new Phrase(ddlAcademicSession.SelectedItem.Text, blackFnt)); row2cell6.Colspan = 1; row2cell6.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        PdfPCell row2cell7 = new PdfPCell(new Phrase("TERM", blackFnt)); row2cell1.HorizontalAlignment = Element.ALIGN_LEFT; row2cell7.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell row2cell8 = new PdfPCell(new Phrase(ddlAcademicTerm.SelectedItem.Text, blackFnt)); row2cell8.Colspan = 1; row2cell8.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;

        PdfPTable Attendance = new PdfPTable(8);
        PdfPCell row3cell1 = new PdfPCell(new Phrase("ATTENDANCE", blackFnt)); row3cell1.HorizontalAlignment = Element.ALIGN_LEFT; row3cell1.Colspan = 2; //row3cell1.Border = 0;
        PdfPCell row3cell2 = new PdfPCell(new Phrase("", blackFnt)); row3cell2.HorizontalAlignment = Element.ALIGN_LEFT; row3cell2.Colspan = 2; //row3cell2.Border = 0;
        PdfPCell row3cell3 = new PdfPCell(new Phrase("NUMBER OF TIMES", blackFnt)); row3cell3.HorizontalAlignment = Element.ALIGN_LEFT; row3cell3.Colspan = 2; //row3cell1.Border = 0;
        PdfPCell row3cell4 = new PdfPCell(new Phrase("PERCENTAGE PRESENT", blackFnt)); row3cell4.HorizontalAlignment = Element.ALIGN_LEFT; row3cell4.Colspan = 2; //row3cell2.Border = 0;

        PdfPCell row4cell1 = new PdfPCell(new Phrase(" ", blackFnt)); row4cell1.Colspan = 2; row4cell1.HorizontalAlignment = Element.ALIGN_LEFT; //row4cell1.Border = 0;
        PdfPCell row4cell2 = new PdfPCell(new Phrase("SCHOOL OPENED: " + " ", blackFnt)); row4cell2.HorizontalAlignment = Element.ALIGN_LEFT; row4cell2.Colspan = 2; //row4cell2.Border = 0;
        PdfPCell row4cell3 = new PdfPCell(new Phrase("PRESENT: " + " ", blackFnt)); row4cell3.Colspan = 1; row4cell3.HorizontalAlignment = Element.ALIGN_LEFT; //row4cell1.Border = 0;
        PdfPCell row4cell4 = new PdfPCell(new Phrase("ABSENT: " + " ", blackFnt)); row4cell4.HorizontalAlignment = Element.ALIGN_LEFT; row4cell4.Colspan = 1; //row4cell2.Border = 0;
        PdfPCell row4cell5 = new PdfPCell(new Phrase("PERCENTAGE: " + " ", blackFnt)); row4cell5.HorizontalAlignment = Element.ALIGN_LEFT; row4cell5.Colspan = 2; //row4cell2.Border = 0;

        Paragraph domain = new Paragraph(string.Format("{0}", "COGNITIVE DOMAIN"), darkerRedFnt);
        domain.Alignment = Element.ALIGN_CENTER;




        maintbl.AddCell(cell1);
        maintbl.AddCell(cell2);
        maintbl.AddCell(cell3);
        maintbl.AddCell(cell4);
        maintbl.AddCell(cell5);
        maintbl.AddCell(cell6);

        maintbl.AddCell(row2cell1);
        maintbl.AddCell(row2cell2);
        maintbl.AddCell(row2cell3);
        maintbl.AddCell(row2cell4);
        maintbl.AddCell(row2cell5);
        maintbl.AddCell(row2cell6);
        maintbl.AddCell(row2cell7);
        maintbl.AddCell(row2cell8);

        maintbl.AddCell(row3cell1);
        maintbl.AddCell(row3cell2);
        maintbl.AddCell(row3cell3);
        maintbl.AddCell(row3cell4);

        maintbl.AddCell(row4cell1);
        maintbl.AddCell(row4cell2);
        maintbl.AddCell(row4cell3);
        maintbl.AddCell(row4cell4);
        maintbl.AddCell(row4cell5);




        ///PdfPTable resultTable = getResultTable(student);/// dont add the table yet so as to retrieve the gpa viewstate value needed in the gpa summ ary slot 
        /// 
        Int64 sessionId = Convert.ToInt64(ddlAcademicSession.SelectedIndex);
        Int64 TermId = Convert.ToInt64(ddlAcademicTerm.SelectedIndex);
        PdfPTable resTable = new PdfPTable(12);
        //resTable.SetWidthPercentage(new float[8] { 80f, 30f, 30f, 30f, 35f, 35f, 38f, 30f }, PageSize.A5.Rotate());
        //resTable.TotalWidth = 400f;
        //resTable.WidthPercentage = 80;
        //resTable.LockedWidth = true;
        PdfPCell deptHdr = new PdfPCell(new Phrase("DEPARTMENT", resultTitleRedFnt10)); deptHdr.Padding = 0f;
        deptHdr.Colspan = 2; deptHdr.HorizontalAlignment = Element.ALIGN_CENTER; deptHdr.VerticalAlignment = Element.ALIGN_BOTTOM;
        resTable.AddCell(deptHdr);
        PdfPCell subjectHdr = new PdfPCell(new Phrase("SUBJECTS", resultTitleRedFnt10)); subjectHdr.Padding = 0f;
        subjectHdr.Colspan = 2; subjectHdr.HorizontalAlignment = Element.ALIGN_CENTER; subjectHdr.VerticalAlignment = Element.ALIGN_BOTTOM;
        resTable.AddCell(subjectHdr);
        PdfPCell cwHdr = new PdfPCell(new Phrase("CW", resultTitleRedFnt8)); cwHdr.Colspan = 1; cwHdr.Rotation = 90; cwHdr.VerticalAlignment = Element.ALIGN_TOP; cwHdr.HorizontalAlignment = Element.ALIGN_LEFT;
        resTable.AddCell(cwHdr);
        PdfPCell hwHdr = new PdfPCell(); hwHdr.AddElement(new Phrase("HW/PRO", resultTitleRedFnt8)); hwHdr.Colspan = 1; hwHdr.Rotation = 90; hwHdr.HorizontalAlignment = Element.ALIGN_LEFT; hwHdr.VerticalAlignment = Element.ALIGN_TOP;
        resTable.AddCell(hwHdr);
        PdfPCell attHdr = new PdfPCell(); attHdr.Colspan = 1; attHdr.Rotation = 90; attHdr.HorizontalAlignment = Element.ALIGN_LEFT; attHdr.VerticalAlignment = Element.ALIGN_TOP; attHdr.AddElement(new Phrase("ATT/NOTE", resultTitleRedFnt8)); resTable.AddCell(attHdr);
        PdfPCell testHdr = new PdfPCell(); testHdr.Colspan = 1; testHdr.Rotation = 90; testHdr.HorizontalAlignment = Element.ALIGN_LEFT; testHdr.VerticalAlignment = Element.ALIGN_TOP; testHdr.AddElement(new Phrase("TEST", resultTitleRedFnt8)); resTable.AddCell(testHdr);
        PdfPCell totalHdr = new PdfPCell(); totalHdr.Colspan = 1; totalHdr.Rotation = 90; totalHdr.HorizontalAlignment = Element.ALIGN_LEFT; totalHdr.VerticalAlignment = Element.ALIGN_TOP; totalHdr.AddElement(new Phrase("TOTAL", resultTitleRedFnt8)); resTable.AddCell(totalHdr);
        PdfPCell gradeHdr = new PdfPCell(); gradeHdr.Colspan = 1; gradeHdr.Rotation = 90; gradeHdr.HorizontalAlignment = Element.ALIGN_LEFT; gradeHdr.VerticalAlignment = Element.ALIGN_TOP; gradeHdr.AddElement(new Phrase("GRADE", resultTitleRedFnt8)); resTable.AddCell(gradeHdr);
        PdfPCell commentHdr = new PdfPCell(); commentHdr.Colspan = 2; commentHdr.HorizontalAlignment = Element.ALIGN_CENTER; commentHdr.VerticalAlignment = Element.ALIGN_BOTTOM; commentHdr.AddElement(new Phrase("COMMENT", resultTitleRedFnt8)); resTable.AddCell(commentHdr);
        PdfPCell space = new PdfPCell(); space.Colspan = 2;
        resTable.AddCell(space);
        resTable.AddCell(space);
        resTable.AddCell("20");
        resTable.AddCell("20");
        resTable.AddCell("10");
        resTable.AddCell("50");
        resTable.AddCell("100");
        resTable.AddCell("");
        resTable.AddCell(space);

        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

        PASSIS.LIB.GradeStudent gs = new ClassGradeLIB().RetrieveStudentGrade(student.Id, session);

        //Int64 subjectCounter = 0;
        int totalMarkObtained = 0;
        int totalMarkObtainable = 0;
        decimal testScoreConfiguration = 0;
        decimal cw = 0;
        decimal hw = 0;
        decimal note = 0;
        decimal testScore = 0;
        decimal examScore = 0m;
        decimal totalScore = 0;
        decimal totalScoreAverage = 0;
        decimal scorePosition = 0;
        decimal aggregateTotalScore = 0;
        string subjName = "";
        string deptName = "";
        decimal percentage = 0m;
        long subjectId = 0;
        Dictionary<long, string> dictionary = new Dictionary<long, string>();


        if (gs != null)
        {
            //IList<PASSIS.LIB.Subject> getSub = getSubject(session, term, schoolId, yearId, gradeIdd);
            //foreach (PASSIS.LIB.Subject sub in getSub) 
            //{
            //    subjectCounter++;
            //}
            var deptList = from s in context.SubjectDepartments where s.SchoolId == logonUser.SchoolId select s;
            foreach (SubjectDepartment dept in deptList)
            {
                IList<StudentScoreCA> rpCard = getSubjectScoreCA(student.AdmissionNumber, student.Id, session, term, schoolId, yearId, gradeIdd, dept.Id);
                foreach (StudentScoreCA d in rpCard)
                {

                    if (dictionary.ContainsKey(dept.Id))
                    {
                        deptName = "";
                        PdfPCell deptCell = new PdfPCell(new Phrase(deptName, darkerGrnFnt9)); deptCell.Colspan = 2; deptCell.BorderWidthTop = 0; deptCell.BorderColorTop = new BaseColor(255, 255, 255);
                        resTable.AddCell(deptCell);
                    }
                    else
                    {
                        deptName = dept.DepartmentName;
                        dictionary.Add(dept.Id, dept.DepartmentName);
                        PdfPCell deptCell = new PdfPCell(new Phrase(deptName, darkerGrnFnt9)); deptCell.Colspan = 2;
                        resTable.AddCell(deptCell);
                    }

                    subjectCounter++;
                    aggregateTotalScore += Convert.ToDecimal(d.Total);
                    subjName = d.Subject.Name;
                    testScore = Convert.ToDecimal(d.Test);
                    cw = Convert.ToDecimal(d.ClassWork);
                    hw = Convert.ToDecimal(d.HomeWorkProject);
                    note = Convert.ToDecimal(d.AttendanceNote);
                    //examScore = Convert.ToDecimal(d.ExamScore);
                    //totalScore = Convert.ToDecimal(d.Total);
                    totalScore = cw + hw + note + testScore;
                    //scorePosition = Convert.ToDecimal(d.Position);

                    string grade = PASSIS.LIB.Utility.Utili.getExamGradeLetters(Convert.ToDecimal(d.Total), (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue));
                    if (grade.Trim() == "A") { distinctionCounter++; }
                    else if (grade.Trim() == "B" || grade.Trim() == "C") { creditCounter++; }
                    else if (grade.Trim() == "D") { passesCounter++; }
                    else if (grade.Trim() == "E") { failureCounter++; }


                    PdfPCell subjCell = new PdfPCell(new Phrase(d.Subject.Name, darkerGrnFnt9)); subjCell.Colspan = 2;
                    resTable.AddCell(subjCell);
                    PdfPCell cwCell = new PdfPCell(new Phrase(d.ClassWork.ToString(), blackFnt)); cwCell.Colspan = 1;
                    resTable.AddCell(cwCell);
                    PdfPCell hwCell = new PdfPCell(new Phrase(d.HomeWorkProject.ToString(), blackFnt)); hwCell.Colspan = 1;
                    resTable.AddCell(hwCell);
                    PdfPCell noteCell = new PdfPCell(new Phrase(d.AttendanceNote.ToString(), blackFnt)); noteCell.Colspan = 1;
                    resTable.AddCell(noteCell);
                    PdfPCell testCell = new PdfPCell(new Phrase(d.Test.ToString(), blackFnt)); testCell.Colspan = 1;
                    resTable.AddCell(testCell);
                    PdfPCell totalCell = new PdfPCell(new Phrase(totalScore.ToString(), blackFnt)); totalCell.Colspan = 1;
                    resTable.AddCell(totalCell);
                    PdfPCell gradeCell = new PdfPCell(new Phrase((PASSIS.LIB.Utility.Utili.getExamGradeLetters(Convert.ToDecimal(d.Total), (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue))), blackFnt)); gradeCell.Colspan = 1;
                    resTable.AddCell(gradeCell);
                    PdfPCell tRemark = new PdfPCell(new Phrase(PASSIS.LIB.Utility.Utili.getExamGradeRemarks(Convert.ToDecimal(d.Total), (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)), blackFnt)); tRemark.Colspan = 2;
                    resTable.AddCell(tRemark);


                    //++noOfSubjectWithScore;
                    totalMarkObtainable = 0;
                    totalMarkObtained = 0;
                    testScore = 0;
                }
            }
        }




        //document.Add(new Phrase(Environment.NewLine));
        if (subjectCounter != 0)
        {
            percentage = (aggregateTotalScore / (subjectCounter * 100)) * 100;
        }
        //string classTeacherComment = txtClassTeacherComment.Text.Trim();
        //string proprietorComment = txtProprietorComment.Text.Trim();
        PdfPTable summaryTable = new PdfPTable(10);
        iTextSharp.text.pdf.draw.LineSeparator line = new iTextSharp.text.pdf.draw.LineSeparator(0f, 100f, BaseColor.BLACK, Element.ALIGN_LEFT, 1);
        PdfPCell Summarycell1 = new PdfPCell(new Phrase("Total Score Obtained:", blackFnt)); Summarycell1.Colspan = 3; Summarycell1.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell1.Border = 0;
        PdfPCell Summarycell2 = new PdfPCell(new Phrase(Math.Round(aggregateTotalScore, 0).ToString(), blackFnt)); Summarycell2.Colspan = 1; Summarycell2.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell2.Border = 0;
        PdfPCell Summarycell3 = new PdfPCell(new Phrase("Total Score Obtainable:", blackFnt)); Summarycell3.Colspan = 2; Summarycell3.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell3.Border = 0;
        PdfPCell Summarycell4 = new PdfPCell(new Phrase((subjectCounter * 100).ToString(), blackFnt)); Summarycell4.Colspan = 1; Summarycell4.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell4.Border = 0;
        PdfPCell Summarycell5 = new PdfPCell(new Phrase("Percentage(%):", blackFnt)); Summarycell5.Colspan = 2; Summarycell5.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell5.Border = 0;
        PdfPCell Summarycell6 = new PdfPCell(new Phrase(Math.Round(percentage, 0).ToString(), blackFnt)); Summarycell6.Colspan = 1; Summarycell6.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell6.Border = 0;

        PdfPCell summaryRow5Cell1 = new PdfPCell(new Phrase("Number of Subject Offered:", blackFnt)); summaryRow5Cell1.Colspan = 4; summaryRow5Cell1.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow5Cell1.Border = 0;
        PdfPCell summaryRow5Cell2 = new PdfPCell(new Phrase(subjectCounter.ToString(), blackFnt)); summaryRow5Cell2.Colspan = 6; summaryRow5Cell2.Border = 0;
        PdfPCell summaryRow5Cell3 = new PdfPCell(new Phrase("Number of Distinctions:", blackFnt)); summaryRow5Cell3.Colspan = 4; summaryRow5Cell3.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow5Cell3.Border = 0;
        PdfPCell summaryRow5Cell4 = new PdfPCell(new Phrase(distinctionCounter.ToString(), blackFnt)); summaryRow5Cell4.Colspan = 6; summaryRow5Cell4.Border = 0;
        PdfPCell summaryRow5Cell5 = new PdfPCell(new Phrase("Number of Credits:", blackFnt)); summaryRow5Cell5.Colspan = 4; summaryRow5Cell5.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow5Cell5.Border = 0;
        PdfPCell summaryRow5Cell6 = new PdfPCell(new Phrase(creditCounter.ToString(), blackFnt)); summaryRow5Cell6.Colspan = 6; summaryRow5Cell6.Border = 0;
        PdfPCell summaryRow5Cell7 = new PdfPCell(new Phrase("Number of Passes:", blackFnt)); summaryRow5Cell7.Colspan = 4; summaryRow5Cell7.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow5Cell7.Border = 0;
        PdfPCell summaryRow5Cell8 = new PdfPCell(new Phrase(passesCounter.ToString(), blackFnt)); summaryRow5Cell8.Colspan = 6; summaryRow5Cell8.Border = 0;
        PdfPCell summaryRow5Cell9 = new PdfPCell(new Phrase("Number of Failure:", blackFnt)); summaryRow5Cell9.Colspan = 4; summaryRow5Cell9.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow5Cell9.Border = 0;
        PdfPCell summaryRow5Cell10 = new PdfPCell(new Phrase(failureCounter.ToString(), blackFnt)); summaryRow5Cell10.Colspan = 6; summaryRow5Cell10.Border = 0;


        PdfPCell summaryRow2Cell1 = new PdfPCell(new Phrase("Class Teacher's Comment:", blackFnt)); summaryRow2Cell1.Colspan = 4; summaryRow2Cell1.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow2Cell1.Border = 0;
        PdfPCell summaryRow2Cell2 = new PdfPCell(new Phrase("", blackFnt)); summaryRow2Cell2.Colspan = 6; summaryRow2Cell2.Border = 0;

        PdfPCell summaryRow4Cell1 = new PdfPCell(new Phrase("Principal's Comment:", blackFnt)); summaryRow4Cell1.Colspan = 3; summaryRow4Cell1.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow4Cell1.Border = 0;
        PdfPCell summaryRow4Cell2 = new PdfPCell(new Phrase("", blackFnt)); summaryRow4Cell2.Colspan = 7; summaryRow4Cell2.Border = 0;

        summaryTable.AddCell(Summarycell1); summaryTable.AddCell(Summarycell2); summaryTable.AddCell(Summarycell3);
        summaryTable.AddCell(Summarycell4); summaryTable.AddCell(Summarycell5); summaryTable.AddCell(Summarycell6);
        summaryTable.AddCell(summaryRow5Cell1); summaryTable.AddCell(summaryRow5Cell2);
        summaryTable.AddCell(summaryRow5Cell3); summaryTable.AddCell(summaryRow5Cell4);
        summaryTable.AddCell(summaryRow5Cell5); summaryTable.AddCell(summaryRow5Cell6);
        summaryTable.AddCell(summaryRow5Cell7); summaryTable.AddCell(summaryRow5Cell8);
        summaryTable.AddCell(summaryRow5Cell9); summaryTable.AddCell(summaryRow5Cell10);

        summaryTable.AddCell(summaryRow2Cell1); summaryTable.AddCell(summaryRow2Cell2);
        summaryTable.AddCell(summaryRow4Cell1); summaryTable.AddCell(summaryRow4Cell2);
        // The row6cell2 was placed here because we need the calculation to present the total number of subject after counting the subject
        PdfPCell row6cell2 = new PdfPCell(new Phrase(subjectCounter.ToString())); row6cell2.HorizontalAlignment = Element.ALIGN_LEFT; row6cell2.Colspan = 1; //row6cell2.Border = 0;
        //maintbl.AddCell(row6cell1);
        //maintbl.AddCell(row6cell2);
        //maintbl.AddCell(row6cell3);
        //maintbl.AddCell(row6cell4);
        document.Add(maintbl);
        document.Add(domain);
        //document.Add(new Phrase(Environment.NewLine));
        //document.Add(amaintbl);
        document.Add(resTable);
        //document.Add(bmaintbl);
        document.Add(summaryTable);


    }

    protected void addResultSummaryPageCrownEndTermNew(Document document, PASSIS.LIB.User student, UsersLIB usrDal)
    {

        iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(Server.MapPath(SchoolLogo));
        jpg.ScaleToFit(70, 70);
        jpg.Alignment = iTextSharp.text.Image.UNDERLYING;
        jpg.SetAbsolutePosition(250, 750);

        Passport = student.PassportFileName;
        if (Passport == null) { Passport = "~/Images/student3.PNG"; }
        iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(Server.MapPath(Passport));
        jpg1.ScaleToFit(70, 70);
        jpg1.Alignment = iTextSharp.text.Image.UNDERLYING;
        jpg1.SetAbsolutePosition(250, 750);
        PdfPTable innerTable = new PdfPTable(9);
        PdfPCell innerCell1 = new PdfPCell(new Phrase("CROWN JEWEL COLLEGE", darkerRedFnt16)); innerCell1.Colspan = 9; innerCell1.Border = 0; innerCell1.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell innerCell2 = new PdfPCell(new Phrase("9, Bakky Street, Off Bello Folawiyo Street, Ikosi, Ketu, Lagos ", darkerGrnFnt9)); innerCell2.Colspan = 9; innerCell2.Border = 0; innerCell2.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell innerCell3 = new PdfPCell(new Phrase("Phone: 08067864385, 08023070027, 08038796369", blackFnt)); innerCell3.Colspan = 9; innerCell3.Border = 0; innerCell3.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell innerCell4 = new PdfPCell(new Phrase("Email: crownjewelschools@yahoo.com, FB: @crownjewelcollege", blackFnt)); innerCell4.Colspan = 9; innerCell4.Border = 0; innerCell4.HorizontalAlignment = Element.ALIGN_CENTER;
        innerTable.AddCell(innerCell1);
        innerTable.AddCell(innerCell2);
        innerTable.AddCell(innerCell3);
        innerTable.AddCell(innerCell4);
        PdfPTable head = new PdfPTable(15);
        PdfPCell head1 = new PdfPCell(jpg); head1.Colspan = 3; head1.Border = 0;
        PdfPCell head2 = new PdfPCell(innerTable); head2.Colspan = 9; head2.Border = 0;
        PdfPCell head3 = new PdfPCell(jpg1); head3.Colspan = 3; head3.HorizontalAlignment = Element.ALIGN_RIGHT; head3.Border = 0;


        head.AddCell(head1);
        head.AddCell(head2);
        head.AddCell(head3);
        document.Add(head);
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

        long? schId = logonUser.SchoolId;
        long gradeId = new PrintResultt_693().theGradeId(student.Id, session).GradeId;
        //long gradeId = new PrintResultt_693().theGradeId(student.Id).GradeId;
        string noInClass = noOfStudentInClass(schId, gradeId).Count.ToString();
        string classPosition = "";
        var posInClass = getClassPosition(student.AdmissionNumber, term, session, schoolId, yearId, gradeIdd);
        if (posInClass != null)
        {
            classPosition = posInClass.Position.ToString();
        }

        string totalNoInClass = new ClassGradeLIB().RetrieveGradeStudents(Convert.ToInt64(schId), Convert.ToInt64(logonUser.SchoolCampusId), Convert.ToInt64(yearId), gradeIdd, session).Count().ToString();

        PdfPTable maintbl = new PdfPTable(8);
        PdfPCell cell1 = new PdfPCell(new Phrase("NAME", blackFnt6)); cell1.Colspan = 1; cell1.HorizontalAlignment = Element.ALIGN_LEFT; //cell1.Border = 0;
        PdfPCell cell2 = new PdfPCell(new Phrase(student.StudentFullName, blackFnt6)); cell2.Colspan = 3; cell2.HorizontalAlignment = Element.ALIGN_LEFT; //cell2.Border = 0;
        PdfPCell cell3 = new PdfPCell(new Phrase("CLASS", blackFnt6)); cell3.Colspan = 1; cell3.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell cell4 = new PdfPCell(new Phrase(usrDal.getStudentsHomeRoom(student.Id), blackFnt6)); cell4.Colspan = 1; cell4.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell cell5 = new PdfPCell(new Phrase("NO. IN CLASS", blackFnt6)); cell5.Colspan = 1; cell5.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell cell6 = new PdfPCell(new Phrase(totalNoInClass, blackFnt6)); cell6.Colspan = 1; cell6.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;

        PdfPCell row2cell1 = new PdfPCell(new Phrase("GENDER", blackFnt6)); row2cell1.HorizontalAlignment = Element.ALIGN_LEFT; row2cell1.Colspan = 2; //row2cell1.Border = 0;
        PdfPCell row2cell2 = new PdfPCell(new Phrase(getGender(student.Gender), blackFnt6)); row2cell2.Colspan = 2; row2cell2.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        PdfPCell row2cell3 = new PdfPCell(new Phrase("AGE", blackFnt6)); row2cell3.HorizontalAlignment = Element.ALIGN_LEFT; row2cell3.Colspan = 2; //row2cell1.Border = 0;
        PdfPCell row2cell4 = new PdfPCell(new Phrase(GetAge(Convert.ToDateTime(student.DateOfBirth)).ToString(), blackFnt6)); row2cell4.Colspan = 2; row2cell4.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        //PdfPCell row2cell5 = new PdfPCell(new Phrase("SESSION", blackFnt6)); row2cell5.HorizontalAlignment = Element.ALIGN_LEFT; row2cell5.Colspan = 1; //row2cell1.Border = 0;
        //PdfPCell row2cell6 = new PdfPCell(new Phrase(ddlAcademicSession.SelectedItem.Text, blackFnt6)); row2cell6.Colspan = 1; row2cell6.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        //PdfPCell row2cell7 = new PdfPCell(new Phrase("TERM", blackFnt6)); row2cell1.HorizontalAlignment = Element.ALIGN_LEFT; row2cell7.Colspan = 1; //row2cell1.Border = 0;
        //PdfPCell row2cell8 = new PdfPCell(new Phrase(ddlAcademicTerm.SelectedItem.Text, blackFnt6)); row2cell8.Colspan = 1; row2cell8.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;

        PdfPTable Attendance = new PdfPTable(8);
        //PdfPCell row3cell1 = new PdfPCell(new Phrase("ATTENDANCE", blackFnt6)); row3cell1.HorizontalAlignment = Element.ALIGN_LEFT; row3cell1.Colspan = 2; //row3cell1.Border = 0;
        //PdfPCell row3cell2 = new PdfPCell(new Phrase("", blackFnt6)); row3cell2.HorizontalAlignment = Element.ALIGN_LEFT; row3cell2.Colspan = 2; //row3cell2.Border = 0;
        //PdfPCell row3cell3 = new PdfPCell(new Phrase("NUMBER OF TIMES", blackFnt6)); row3cell3.HorizontalAlignment = Element.ALIGN_LEFT; row3cell3.Colspan = 2; //row3cell1.Border = 0;
        //PdfPCell row3cell4 = new PdfPCell(new Phrase("PERCENTAGE PRESENT", blackFnt6)); row3cell4.HorizontalAlignment = Element.ALIGN_LEFT; row3cell4.Colspan = 2; //row3cell2.Border = 0;

        //PdfPCell row4cell1 = new PdfPCell(new Phrase(" ", blackFnt6)); row4cell1.Colspan = 2; row4cell1.HorizontalAlignment = Element.ALIGN_LEFT; //row4cell1.Border = 0;
        PdfPCell row4cell2 = new PdfPCell(new Phrase("NO. OF TIMES SCHOOL OPENED: " + " ", blackFnt6)); row4cell2.HorizontalAlignment = Element.ALIGN_LEFT; row4cell2.Colspan = 4; //row4cell2.Border = 0;
        PdfPCell row4cell3 = new PdfPCell(new Phrase("TERM: " + ddlAcademicTerm.SelectedItem.Text, blackFnt6)); row4cell3.Colspan = 1; row4cell3.HorizontalAlignment = Element.ALIGN_LEFT; //row4cell1.Border = 0;
        PdfPCell row4cell4 = new PdfPCell(new Phrase("HOUSE: " + student.SchoolHouse, blackFnt6)); row4cell4.HorizontalAlignment = Element.ALIGN_LEFT; row4cell4.Colspan = 1; //row4cell2.Border = 0;
        PdfPCell row4cell5 = new PdfPCell(new Phrase("SESSION: " + ddlAcademicSession.SelectedItem.Text, blackFnt6)); row4cell5.HorizontalAlignment = Element.ALIGN_LEFT; row4cell5.Colspan = 2; //row4cell2.Border = 0;

        Paragraph domain = new Paragraph(string.Format("{0}", " "), darkerRedFnt);
        domain.Alignment = Element.ALIGN_CENTER;

        //PdfPTable domainandSkills = new PdfPTable(15);
        //PdfPCell row5cell1 = new PdfPCell(new Phrase("COGNITIVE SKILLS " + " ", blackFnt)); row5cell1.HorizontalAlignment = Element.ALIGN_CENTER; row5cell1.Colspan = 10; //row4cell2.Border = 0;
        //domainandSkills.AddCell(row5cell1);
        //PdfPCell affectivePsychSkills = new PdfPCell(new Phrase("AFFECTIVE & PSYCHOMOTOR" + " ", blackFnt)); affectivePsychSkills.HorizontalAlignment = Element.ALIGN_CENTER; affectivePsychSkills.Colspan = 9; //row4cell2.Border = 0;
        //domainandSkills.AddCell(affectivePsychSkills);


        maintbl.AddCell(cell1);
        maintbl.AddCell(cell2);
        maintbl.AddCell(cell3);
        maintbl.AddCell(cell4);
        maintbl.AddCell(cell5);
        maintbl.AddCell(cell6);

        maintbl.AddCell(row2cell1);
        maintbl.AddCell(row2cell2);
        maintbl.AddCell(row2cell3);
        maintbl.AddCell(row2cell4);
        //maintbl.AddCell(row2cell5);
        //maintbl.AddCell(row2cell6);
        //maintbl.AddCell(row2cell7);
        //maintbl.AddCell(row2cell8);

        //maintbl.AddCell(row3cell1);
        //maintbl.AddCell(row3cell2);
        //maintbl.AddCell(row3cell3);
        //maintbl.AddCell(row3cell4);

        //maintbl.AddCell(row4cell1);
        maintbl.AddCell(row4cell2);
        maintbl.AddCell(row4cell3);
        maintbl.AddCell(row4cell4);
        maintbl.AddCell(row4cell5);





        ///PdfPTable resultTable = getResultTable(student);/// dont add the table yet so as to retrieve the gpa viewstate value needed in the gpa summ ary slot 
        /// 
        Int64 sessionId = Convert.ToInt64(ddlAcademicSession.SelectedIndex);
        Int64 TermId = Convert.ToInt64(ddlAcademicTerm.SelectedIndex);
        PdfPTable resTable = new PdfPTable(15);
        PdfPTable affectPsychTbl = new PdfPTable(4);

        //resTable.SetWidthPercentage(new float[8] { 80f, 30f, 30f, 30f, 35f, 35f, 38f, 30f }, PageSize.A5.Rotate());
        //resTable.TotalWidth = 400f;
        //resTable.WidthPercentage = 80;
        //resTable.LockedWidth = true;
        PdfPCell row5cell1 = new PdfPCell(new Phrase("COGNITIVE SKILLS " + " ", blackFnt8)); row5cell1.HorizontalAlignment = Element.ALIGN_CENTER; row5cell1.Colspan = 15; //row4cell2.Border = 0;
        resTable.AddCell(row5cell1);
        PdfPCell deptHdr = new PdfPCell(new Phrase("DEPT", blackFnt8)); deptHdr.Padding = 0f;
        deptHdr.Colspan = 2; deptHdr.HorizontalAlignment = Element.ALIGN_CENTER; deptHdr.VerticalAlignment = Element.ALIGN_BOTTOM;
        resTable.AddCell(deptHdr);
        PdfPCell subjectHdr = new PdfPCell(new Phrase("SUBJECT", blackFnt8)); subjectHdr.Padding = 0f;
        subjectHdr.Colspan = 2; subjectHdr.HorizontalAlignment = Element.ALIGN_CENTER; subjectHdr.VerticalAlignment = Element.ALIGN_BOTTOM;
        resTable.AddCell(subjectHdr);
        PdfPCell cwHdr = new PdfPCell(new Phrase("CA", blackFnt8)); cwHdr.Colspan = 1; cwHdr.Rotation = 90; cwHdr.VerticalAlignment = Element.ALIGN_TOP; cwHdr.HorizontalAlignment = Element.ALIGN_LEFT;
        resTable.AddCell(cwHdr);
        PdfPCell hwHdr = new PdfPCell(); hwHdr.AddElement(new Phrase("Examination", blackFnt8)); hwHdr.Colspan = 1; hwHdr.Rotation = 90; hwHdr.Rotation = 90; hwHdr.HorizontalAlignment = Element.ALIGN_LEFT; hwHdr.VerticalAlignment = Element.ALIGN_TOP;
        resTable.AddCell(hwHdr);
        PdfPCell attHdr = new PdfPCell(); attHdr.Colspan = 1; attHdr.Rotation = 90; attHdr.HorizontalAlignment = Element.ALIGN_LEFT; attHdr.VerticalAlignment = Element.ALIGN_TOP; attHdr.AddElement(new Phrase("CLASS AVERAGE", blackFnt8)); resTable.AddCell(attHdr); attHdr.Rotation = 90;
         PdfPCell testHdr = new PdfPCell(); testHdr.Colspan = 1; testHdr.Rotation = 90; testHdr.HorizontalAlignment = Element.ALIGN_LEFT; testHdr.VerticalAlignment = Element.ALIGN_TOP; testHdr.AddElement(new Phrase("1ST TERM", blackFnt8)); resTable.AddCell(testHdr); testHdr.Rotation = 90;
        PdfPCell totalHdr = new PdfPCell(); totalHdr.Colspan = 1; totalHdr.Rotation = 90; totalHdr.HorizontalAlignment = Element.ALIGN_LEFT; totalHdr.VerticalAlignment = Element.ALIGN_TOP; totalHdr.AddElement(new Phrase("2ND TERM", blackFnt8)); resTable.AddCell(totalHdr); totalHdr.Rotation = 90;
        PdfPCell caHdr = new PdfPCell(); caHdr.Colspan = 1; caHdr.Rotation = 90; caHdr.HorizontalAlignment = Element.ALIGN_LEFT; caHdr.VerticalAlignment = Element.ALIGN_TOP; caHdr.AddElement(new Phrase("3RD TERM", blackFnt8)); resTable.AddCell(caHdr); caHdr.Rotation = 90;
        PdfPCell examHdr = new PdfPCell(); examHdr.Colspan = 1; examHdr.Rotation = 90; examHdr.HorizontalAlignment = Element.ALIGN_LEFT; examHdr.VerticalAlignment = Element.ALIGN_TOP; examHdr.AddElement(new Phrase("OVERALL TOTAL", blackFnt8)); resTable.AddCell(examHdr); examHdr.Rotation = 90;
        PdfPCell total2Hdr = new PdfPCell(); total2Hdr.Colspan = 1; total2Hdr.Rotation = 90; total2Hdr.HorizontalAlignment = Element.ALIGN_LEFT; total2Hdr.VerticalAlignment = Element.ALIGN_TOP; total2Hdr.AddElement(new Phrase("GRADE", blackFnt8)); resTable.AddCell(total2Hdr); total2Hdr.Rotation = 90;
        PdfPCell gradeHdr = new PdfPCell(); gradeHdr.Colspan = 1; gradeHdr.Rotation = 90; gradeHdr.HorizontalAlignment = Element.ALIGN_LEFT; gradeHdr.VerticalAlignment = Element.ALIGN_TOP; gradeHdr.AddElement(new Phrase("NO OF STUDENTS", blackFnt8)); resTable.AddCell(gradeHdr); gradeHdr.Rotation = 90;
        PdfPCell commentHdr = new PdfPCell(); commentHdr.Colspan = 2; commentHdr.HorizontalAlignment = Element.ALIGN_CENTER; commentHdr.VerticalAlignment = Element.ALIGN_BOTTOM; commentHdr.AddElement(new Phrase("COMMENT", blackFnt8)); resTable.AddCell(commentHdr); commentHdr.Rotation = 90;
        PdfPCell space = new PdfPCell(); space.Colspan = 2;
        //resTable.AddCell(space);
        //resTable.AddCell(space);
        //resTable.AddCell("40");
        //resTable.AddCell("60");
        //resTable.AddCell("");
        //resTable.AddCell("");
        //resTable.AddCell("");
        //resTable.AddCell("100");
        //resTable.AddCell("100");
        //resTable.AddCell("");
        //resTable.AddCell("");
        //resTable.AddCell(space);
        PdfPCell thirdRow2Cell0A = new PdfPCell(new Phrase("", blackFnt8)); thirdRow2Cell0A.Colspan = 2; thirdRow2Cell0A.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell thirdRow2Cell1A = new PdfPCell(new Phrase("", blackFnt8)); thirdRow2Cell1A.Colspan = 2; thirdRow2Cell1A.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell thirdRow2Cell2A = new PdfPCell(new Phrase("40", blackFnt8)); thirdRow2Cell2A.Colspan = 1; thirdRow2Cell2A.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell thirdRow2Cell3A = new PdfPCell(new Phrase("60", blackFnt8)); thirdRow2Cell3A.Colspan = 1; thirdRow2Cell3A.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell thirdRow2Cell4A = new PdfPCell(new Phrase("", blackFnt8)); thirdRow2Cell4A.Colspan = 1; thirdRow2Cell4A.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell thirdRow2Cell5A = new PdfPCell(new Phrase("", blackFnt8)); thirdRow2Cell5A.Colspan = 1; thirdRow2Cell5A.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell thirdRow2Cell6A = new PdfPCell(new Phrase("", blackFnt8)); thirdRow2Cell6A.Colspan = 1; thirdRow2Cell6A.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell thirdRow2Cell7A = new PdfPCell(new Phrase("100", blackFnt8)); thirdRow2Cell7A.Colspan = 1; thirdRow2Cell7A.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell thirdRow2Cell8A = new PdfPCell(new Phrase("100", blackFnt8)); thirdRow2Cell8A.Colspan = 1; thirdRow2Cell8A.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell thirdRow2Cell9A = new PdfPCell(new Phrase("", blackFnt8)); thirdRow2Cell9A.Colspan = 1; thirdRow2Cell9A.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell thirdRow2Cell10A = new PdfPCell(new Phrase("", blackFnt8)); thirdRow2Cell10A.Colspan = 1; thirdRow2Cell9A.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell thirdRow2Cell11A = new PdfPCell(new Phrase("", blackFnt8)); thirdRow2Cell11A.Colspan = 2; thirdRow2Cell9A.HorizontalAlignment = Element.ALIGN_CENTER;

        resTable.AddCell(thirdRow2Cell0A);
        resTable.AddCell(thirdRow2Cell1A);
        resTable.AddCell(thirdRow2Cell2A);
        resTable.AddCell(thirdRow2Cell3A);
        resTable.AddCell(thirdRow2Cell4A);
        resTable.AddCell(thirdRow2Cell5A);
        resTable.AddCell(thirdRow2Cell6A);
        resTable.AddCell(thirdRow2Cell7A);
        resTable.AddCell(thirdRow2Cell8A);
        resTable.AddCell(thirdRow2Cell9A);
        resTable.AddCell(thirdRow2Cell10A);
        resTable.AddCell(thirdRow2Cell11A);
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

        PASSIS.LIB.GradeStudent gs = new ClassGradeLIB().RetrieveStudentGrade(student.Id, session);

        //Int64 subjectCounter = 0;
        int totalOffered = 0;
        int? curricullumId = 0;
        int deptCount = 0;
        int deptCount2 = 0;
        int totalMarkObtained = 0;
        int totalMarkObtainable = 0;
        decimal totalTestObtained = 0;
        decimal totalExamObtained = 0;
        decimal testScoreConfiguration = 0;
        decimal? ftt = 0;
        decimal? stt = 0;
        decimal? fte = 0;
        decimal? ste = 0;
        decimal? ftte = 0;
        decimal? stte = 0;
        decimal? totalBroughtForward = 0;
        decimal testScoreFirstTerm = 0;
        decimal examScoreFirstTerm = 0;
        decimal testScoreSecondTerm = 0;
        decimal examScoreSecondTerm = 0;
        decimal examScore = 0;
        decimal testScore = 0;
        decimal totalScore = 0;
        decimal? totalScoreAverage = 0;
        decimal? totalScoreAverages = 0;
        decimal scorePosition = 0;
        decimal testTotal = 0;
        decimal examTotal = 0;
        decimal firstTermCW = 0;
        decimal firstTermHW = 0;
        decimal firstTermNote = 0;
        decimal firstTermTest = 0;
        decimal secondTermCW = 0;
        decimal secondTermHW = 0;
        decimal secondTermNote = 0;
        decimal secondTermTest = 0;
        decimal firstTermCA = 0;
        decimal secondTermCA = 0;
        decimal firstTermExam = 0;
        decimal secondTermExam = 0;
        decimal firstTermOverall = 0;
        decimal secondTermOverall = 0;
        int deptCounter = 0;
        int countCA = 0;
        int countExam = 0;
        string subjName = "";
        string deptName = "";
        decimal? percentage = 0m;
        long subjectId = 0;
        decimal thirdTermScore = 0;
        decimal firstTermCAScore = 0;
        decimal firstTermExamScore = 0;
        decimal aggregateTotalScore = 0;
        decimal secondTermCAScore = 0;
        decimal secondTermExamScore = 0;

        decimal totalFirstTermScore = 0;
        decimal totalSecondTermScore = 0;
        decimal overallTotal = 0;

        Dictionary<long, string> dictionary = new Dictionary<long, string>();
        IList<PASSIS.LIB.Subject> AllSubject = new List<PASSIS.LIB.Subject>();
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
            foreach (SubjectDepartment dept in deptList)
            {
                //deptCounter++;
                IList<SubjectsInSchool> getSubjectInClass = (from s in context.SubjectsInSchools where s.DepartmentId == dept.Id && s.SchoolId == dept.SchoolId select s).ToList();
                foreach (SubjectsInSchool subjects in getSubjectInClass)
                {

                    PASSIS.LIB.Subject subject = context.Subjects.FirstOrDefault(x =>
                                      x.CurriculumId == (int)curricullumId &&
                                      x.ClassId == yearId && x.Id == subjects.SubjectId);
                    if (subject != null)
                    {
                        deptCount++;
                        AllSubject.Add(subject);


                    }

                }


                foreach (SubjectsInSchool subjects in getSubjectInClass)
                {
                    PASSIS.LIB.Subject subject = context.Subjects.FirstOrDefault(x =>
                                      x.CurriculumId == (int)curricullumId &&
                                      x.ClassId == yearId && x.Id == subjects.SubjectId);
                    if (subject != null)
                    {
                        deptCount2++;
                        AllSubject.Add(subject);

                        //subjectCounter++;

                        var studOffered = (from x in context.StudentScores where x.SchoolId == schoolId && x.SubjectId == subject.Id && x.ClassId == yearId && x.TermId == 3 && x.AcademicSessionID == session select x);
                        totalOffered = studOffered.Count();

                        if (dictionary.ContainsKey(dept.Id))
                        {
                            deptName = "";
                            PdfPCell deptCell = new PdfPCell(new Phrase(deptName, darkerGrnFnt6)); deptCell.Colspan = 2; deptCell.BorderWidthTop = 0; deptCell.BorderColorTop = new BaseColor(255, 255, 255);
                            resTable.AddCell(deptCell);
                        }

                        else
                        {
                            deptName = dept.DepartmentName;
                            dictionary.Add(dept.Id, dept.DepartmentName);
                            PdfPCell deptCell = new PdfPCell(new Phrase(deptName, darkerGrnFnt6)); deptCell.Colspan = 2;
                            resTable.AddCell(deptCell);
                        }


                        PdfPCell subjCell = new PdfPCell(new Phrase(subject.Name, darkerGrnFnt6)); subjCell.Colspan = 2;
                        resTable.AddCell(subjCell);
                        IList<PASSIS.LIB.StudentScoreRepository> scoreRepoFirstTerm = getSubjectScoreCategoryCA(student.AdmissionNumber, student.Id, session, 3, schoolId, yearId, gradeId, subject.Id);
                        if (scoreRepoFirstTerm.Count > 0)
                        {
                            foreach (PASSIS.LIB.StudentScoreRepository fs in scoreRepoFirstTerm)
                            {
                                if (fs.MarkObtained != 0)
                                {
                                    testScore = Convert.ToDecimal(fs.MarkObtained);
                                    PdfPCell CACell = new PdfPCell(new Phrase(Math.Round(testScore, 2).ToString(), darkerGrnFnt6)); CACell.Colspan = 1;
                                    resTable.AddCell(CACell);
                                    break;
                                }
                                else
                                {
                                    testScore = 0;
                                    PdfPCell CACell = new PdfPCell(new Phrase("", darkerGrnFnt6)); CACell.Colspan = 1;
                                    resTable.AddCell(CACell);
                                }
                            }
                        }
                        else
                        {
                            testScore = 0;
                            PdfPCell CACell = new PdfPCell(new Phrase("", darkerGrnFnt6)); CACell.Colspan = 1;
                            resTable.AddCell(CACell);
                        }
                        IList<PASSIS.LIB.StudentScore> scoreRepoExamFirstTerm = getSubjectScoreCategoryExam(student.AdmissionNumber, student.Id, session, 3, schoolId, yearId, gradeId, subject.Id);
                        if (scoreRepoExamFirstTerm.Count > 0)
                        {
                            foreach (PASSIS.LIB.StudentScore fs in scoreRepoExamFirstTerm)
                            {
                                if (fs.ExamScore != 0)
                                {
                                    examScore = Convert.ToDecimal(fs.ExamScore);
                                    PdfPCell ExamCell = new PdfPCell(new Phrase(Math.Round(examScore, 2).ToString(), darkerGrnFnt6)); ExamCell.Colspan = 1; ExamCell.BorderWidthTop = 0; ExamCell.BorderColorTop = new BaseColor(255, 255, 255);
                                    resTable.AddCell(ExamCell);
                                    break;
                                }
                                else
                                {
                                    PdfPCell ExamCell = new PdfPCell(new Phrase("", darkerGrnFnt6)); ExamCell.Colspan = 1; ExamCell.BorderWidthTop = 0; ExamCell.BorderColorTop = new BaseColor(255, 255, 255);
                                    resTable.AddCell(ExamCell);
                                }                                //}
                            }
                        }
                        else
                        {
                            examScore = 0;
                            PdfPCell ExamCell = new PdfPCell(new Phrase("", darkerGrnFnt6)); ExamCell.Colspan = 1;
                            resTable.AddCell(ExamCell);
                        }


                        PdfPCell classAverageCell = new PdfPCell(new Phrase("", darkerGrnFnt6)); classAverageCell.Colspan = 1;
                        resTable.AddCell(classAverageCell);

                        //// ---------------------------------------------- CLASS WORK FIRST TERM -------------------------------------------------------/////
                        ///
                        ScoreSubCategoryConfiguration gtScoreCWf = context.ScoreSubCategoryConfigurations.FirstOrDefault(s => s.ClassId == yearId && s.SessionId == session && s.TermId == 1 && s.SubCategory == "CW");

                        if (gtScoreCWf != null)
                        {
                            IList<PASSIS.LIB.StudentScoreRepository> scoreRepoCW1 = getSubjectScorePerSubcategory(student.AdmissionNumber, student.Id, session, 1, schoolId, yearId, gradeId, subject.Id, (long)gtScoreCWf.CategoryId, gtScoreCWf.Id);

                            if (scoreRepoCW1.Count > 0) {
                                foreach (PASSIS.LIB.StudentScoreRepository srCW1 in scoreRepoCW1)
                                {
                                    if (srCW1 != null)
                                    {
                                        firstTermCW = Convert.ToDecimal(srCW1.MarkObtained);
                                        break;
                                    }
                                    else
                                    {
                                        firstTermCW = 0;
                                    }
                                }
                            }
                            else
                            {
                                firstTermCW = 0;
                            }
                            
                        }
                        else
                        {
                            firstTermCW = 0;
                        }
                        ///------------------------------------ ----------------HW/PR--------------------------------------------------//
                        ScoreSubCategoryConfiguration gtScoreHW1 = context.ScoreSubCategoryConfigurations.FirstOrDefault(s => s.ClassId == yearId && s.SessionId == session && s.TermId == 1 && s.SubCategory == "HW/PR");
                        if (gtScoreHW1 != null)
                        {
                            IList<PASSIS.LIB.StudentScoreRepository> scoreRepoHW1 = getSubjectScorePerSubcategory(student.AdmissionNumber, student.Id, session, 1, schoolId, yearId, gradeId, subject.Id, (long)gtScoreHW1.CategoryId, gtScoreHW1.Id);
                            if (scoreRepoHW1.Count > 0)
                            {
                                foreach (PASSIS.LIB.StudentScoreRepository srHW1 in scoreRepoHW1)
                                {
                                    if (srHW1 != null)
                                    {
                                        firstTermHW = Convert.ToDecimal(srHW1.MarkObtained);
                                        break;
                                    }
                                    else
                                    {
                                        firstTermHW = 0;
                                    }
                                }
                            }
                            else
                            {
                                firstTermHW = 0;
                            }
                        }
                        else
                        {
                            firstTermHW = 0;
                        }
                        ///----------------------------------------------------------------------- ATT/NOTE--------------------------//
                        ScoreSubCategoryConfiguration gtScoreAtt1 = context.ScoreSubCategoryConfigurations.FirstOrDefault(s => s.ClassId == yearId && s.SessionId == session && s.TermId == 1 && s.SubCategory == "ATT/NOTE");
                        if (gtScoreAtt1 != null)
                        {
                            IList<PASSIS.LIB.StudentScoreRepository> scoreRepoAtt1 = getSubjectScorePerSubcategory(student.AdmissionNumber, student.Id, session, 1, schoolId, yearId, gradeId, subject.Id, (long)gtScoreAtt1.CategoryId, gtScoreAtt1.Id);
                            if (scoreRepoAtt1.Count > 0)
                            {
                                foreach (PASSIS.LIB.StudentScoreRepository srAtt1 in scoreRepoAtt1)
                                {
                                    if (srAtt1 != null)
                                    {
                                        firstTermNote = Convert.ToDecimal(srAtt1.MarkObtained);
                                        break;
                                    }
                                    else
                                    {
                                        firstTermNote = 0;
                                    }
                                }
                            }
                            else
                            {
                                firstTermNote = 0;
                            }
                        }
                        else
                        {
                            firstTermNote = 0;
                        }
                        ///-------------------------------------------------------- TEST------------------------------------//
                        ScoreSubCategoryConfiguration gtScoreTest1 = context.ScoreSubCategoryConfigurations.FirstOrDefault(s => s.ClassId == yearId && s.SessionId == session && s.TermId == 1 && s.SubCategory == "TEST");
                        if (gtScoreTest1 != null)
                        {
                            IList<PASSIS.LIB.StudentScoreRepository> scoreRepoTest1 = getSubjectScorePerSubcategory(student.AdmissionNumber, student.Id, session, 1, schoolId, yearId, gradeId, subject.Id, (long)gtScoreTest1.CategoryId, gtScoreTest1.Id);
                            if (scoreRepoTest1.Count > 0)
                            {
                                foreach (PASSIS.LIB.StudentScoreRepository srTest in scoreRepoTest1)
                                {
                                    if (srTest != null)
                                    {
                                        firstTermTest = Convert.ToDecimal(srTest.MarkObtained);
                                        break;
                                    }
                                    else
                                    {
                                        firstTermTest = 0;
                                    }
                                }
                            }
                            else
                            {
                                firstTermTest = 0;
                            }
                        }
                        else
                        {
                            firstTermTest = 0;
                        }

                        IList<PASSIS.LIB.StudentScore> scoreRepoExamFirstTermm = getSubjectScoreCategoryExam(student.AdmissionNumber, student.Id, session, 1, schoolId, yearId, gradeId, subject.Id);
                        //countExam += scoreRepoExamFirstTerm.Count();
                        if (scoreRepoExamFirstTermm.Count > 0)
                        {
                            foreach (PASSIS.LIB.StudentScore exx in scoreRepoExamFirstTermm)
                            {
                                if (exx != null)
                                {
                                    examScoreFirstTerm = Convert.ToDecimal(exx.ExamScore);
                                    break;
                                }
                                else
                                {
                                    examScoreFirstTerm = 0;
                                }
                            }
                        }
                        else
                        {
                            examScoreFirstTerm = 0;
                        }

                        firstTermCAScore = ((firstTermCW + firstTermHW + firstTermNote + firstTermTest) / 100) * 40;
                        if (firstTermCAScore > 0)
                        {
                            firstTermOverall = firstTermCAScore + examScoreFirstTerm; //first term total score per subjects
                        }
                        else
                        {
                            firstTermOverall = 0;
                        }

                        ///////////////////////////////-------------------------END OF FIRST TERM.----------------BEGINNING OF SECOND TERM-------------------------------/////////////////////

                        if (firstTermOverall > 0)
                        {
                            PdfPCell firstTermCell = new PdfPCell(new Phrase(firstTermOverall.ToString(), darkerGrnFnt6)); firstTermCell.Colspan = 1;
                            resTable.AddCell(firstTermCell);
                        }
                        else
                        {
                            firstTermOverall = 0;
                            PdfPCell firstTermCell = new PdfPCell(new Phrase("", darkerGrnFnt6)); firstTermCell.Colspan = 1;
                            resTable.AddCell(firstTermCell);
                        }





                        //// --------------------------- CLASS WORK SECOND TERM -------------------------------------------------/////
                        ScoreSubCategoryConfiguration gtScoreCW2 = context.ScoreSubCategoryConfigurations.FirstOrDefault(s => s.ClassId == yearId && s.SessionId == session && s.TermId == 2 && s.SubCategory == "CW");
                        if (gtScoreCW2 != null)
                        {
                            IList<PASSIS.LIB.StudentScoreRepository> scoreRepoCW2 = getSubjectScorePerSubcategory(student.AdmissionNumber, student.Id, session, 2, schoolId, yearId, gradeId, subject.Id, (long)gtScoreCW2.CategoryId, gtScoreCW2.Id);
                            if (scoreRepoCW2.Count > 0)
                            {
                                foreach (PASSIS.LIB.StudentScoreRepository srCW2 in scoreRepoCW2)
                                {
                                    if (srCW2 != null)
                                    {
                                        secondTermCW = Convert.ToDecimal(srCW2.MarkObtained);
                                        break;
                                    }
                                    else
                                    {
                                        secondTermCW = 0;
                                    }
                                }
                            }
                            else
                            {
                                secondTermCW = 0;
                            }
                        }
                        else
                        {
                            secondTermCW = 0;
                        }
                        ///------------------------------------ HW/PRO--------------------------//
                        ScoreSubCategoryConfiguration gtScoreHW2 = context.ScoreSubCategoryConfigurations.FirstOrDefault(s => s.ClassId == yearId && s.SessionId == session && s.TermId == 2 && s.SubCategory == "HW/PR");
                        if (gtScoreHW2 != null)
                        {
                            IList<PASSIS.LIB.StudentScoreRepository> scoreRepoHW2 = getSubjectScorePerSubcategory(student.AdmissionNumber, student.Id, session, 2, schoolId, yearId, gradeId, subject.Id, (long)gtScoreHW2.CategoryId, gtScoreHW2.Id);
                            if (scoreRepoHW2.Count > 0)
                            {
                                foreach (PASSIS.LIB.StudentScoreRepository srHW2 in scoreRepoHW2)
                                {
                                    if (srHW2 != null)
                                    {
                                        secondTermHW = Convert.ToDecimal(srHW2.MarkObtained);
                                        break;
                                    }
                                    else
                                    {
                                        secondTermHW = 0;
                                    }
                                }
                            }
                            else
                            {
                                secondTermHW = 0;
                            }
                        }
                        else
                        {
                            secondTermHW = 0;
                        }

                        ///------------------------------------ ATT/NOTE--------------------------//
                        ScoreSubCategoryConfiguration gtScoreAtt2 = context.ScoreSubCategoryConfigurations.FirstOrDefault(s => s.ClassId == yearId && s.SessionId == session && s.TermId == 2 && s.SubCategory == "ATT/NOTE");
                        if (gtScoreAtt2 != null)
                        {
                            IList<PASSIS.LIB.StudentScoreRepository> scoreRepoAtt2 = getSubjectScorePerSubcategory(student.AdmissionNumber, student.Id, session, 2, schoolId, yearId, gradeId, subject.Id, (long)gtScoreAtt2.CategoryId, gtScoreAtt2.Id);
                            if (scoreRepoAtt2.Count > 0)
                            {
                                foreach (PASSIS.LIB.StudentScoreRepository srAtt2 in scoreRepoAtt2)
                                {
                                    if (srAtt2 != null)
                                    {
                                        secondTermNote = Convert.ToDecimal(srAtt2.MarkObtained);
                                        break;
                                    }
                                    else
                                    {
                                        secondTermNote = 0;
                                    }
                                }
                            }
                            else
                            {
                                secondTermNote = 0;
                            }
                        }
                        else
                        {
                            secondTermNote = 0;
                        }

                        ///------------------------------------ TEST--------------------------//
                        ScoreSubCategoryConfiguration gtScoreTest2 = context.ScoreSubCategoryConfigurations.FirstOrDefault(s => s.ClassId == yearId && s.SessionId == session && s.TermId == 2 && s.SubCategory == "TEST");
                        if (gtScoreTest2 != null)
                        {
                            IList<PASSIS.LIB.StudentScoreRepository> scoreRepoTest2 = getSubjectScorePerSubcategory(student.AdmissionNumber, student.Id, session, 2, schoolId, yearId, gradeId, subject.Id, (long)gtScoreTest2.CategoryId, gtScoreTest2.Id);
                            if (scoreRepoTest2.Count > 0)
                            {
                                foreach (PASSIS.LIB.StudentScoreRepository srTest2 in scoreRepoTest2)
                                {
                                    if (srTest2 != null)
                                    {
                                        secondTermTest = Convert.ToDecimal(srTest2.MarkObtained);
                                        break;
                                    }
                                    else
                                    {
                                        secondTermTest = 0;
                                    }
                                }
                            }
                            else
                            {
                                secondTermTest = 0;
                            }
                        }
                        else
                        {
                            secondTermTest = 0;
                        }

                        IList<PASSIS.LIB.StudentScore> scoreRepoExamFirstTermm2 = getSubjectScoreCategoryExam(student.AdmissionNumber, student.Id, session, 2, schoolId, yearId, gradeId, subject.Id);
                        //countExam += scoreRepoExamFirstTerm.Count();
                        if (scoreRepoExamFirstTermm2.Count > 0)
                        {
                            foreach (PASSIS.LIB.StudentScore exxx in scoreRepoExamFirstTermm2)
                            {
                                if (exxx != null)
                                {
                                    examScoreSecondTerm = Convert.ToDecimal(exxx.ExamScore);
                                    break;
                                }
                                else
                                {
                                    examScoreSecondTerm = 0;
                                }
                            }
                        }
                        else
                        {
                            examScoreSecondTerm = 0;
                        }


                        secondTermCAScore = ((secondTermCW + secondTermHW + secondTermNote + secondTermTest) / 100) * 40;
                        if (secondTermCAScore > 0)
                        {
                            secondTermOverall = secondTermCAScore + examScoreSecondTerm; //second term total score per subjects
                        }
                        else
                        {
                            secondTermOverall = 0;
                        }


                        if (secondTermOverall > 0)
                        {
                            PdfPCell secondTermCell = new PdfPCell(new Phrase(secondTermOverall.ToString(), darkerGrnFnt6)); secondTermCell.Colspan = 1;
                            resTable.AddCell(secondTermCell);
                        }

                        else
                        {
                            secondTermOverall = 0;
                            PdfPCell secondTermCell = new PdfPCell(new Phrase("", darkerGrnFnt6)); secondTermCell.Colspan = 1;
                            resTable.AddCell(secondTermCell);
                        }



                        thirdTermScore = testScore + examScore; // total score for each subject CA and EXAM
                        aggregateTotalScore += thirdTermScore;
                        overallTotal = (firstTermOverall + secondTermOverall + thirdTermScore) / 3; //First,seconnd and third term overall score for each subject

                        if (thirdTermScore > 0)
                        {
                            subjectCounter++;
                        }


                        string grade = PASSIS.LIB.Utility.Utili.getExamGradeLetters(Math.Round(overallTotal), (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue));
                        if (grade.Trim() == "A+" || grade.Trim() == "A" || grade.Trim() == "A-" || grade.Trim() == "B+") { distinctionCounter++; }
                        else if (grade.Trim() == "B" || grade.Trim() == "C+" || grade.Trim() == "C") { creditCounter++; };

                        if (thirdTermScore > 0)
                        {
                            PdfPCell thirdTermCell = new PdfPCell(new Phrase(thirdTermScore.ToString(), darkerGrnFnt6)); thirdTermCell.Colspan = 1;
                            resTable.AddCell(thirdTermCell);
                        }
                        else
                        {
                            PdfPCell thirdTermCell = new PdfPCell(new Phrase("", darkerGrnFnt6)); thirdTermCell.Colspan = 1;
                            resTable.AddCell(thirdTermCell);
                        }
                        if (overallTotal > 0)
                        {
                            PdfPCell overallCell = new PdfPCell(new Phrase(Math.Round(overallTotal, 2).ToString(), darkerGrnFnt6)); overallCell.Colspan = 1;
                            resTable.AddCell(overallCell);
                        }
                        else
                        {
                            PdfPCell overallCell = new PdfPCell(new Phrase("", darkerGrnFnt6)); overallCell.Colspan = 1;
                            resTable.AddCell(overallCell);
                        }

                        PdfPCell gradeCell = new PdfPCell(new Phrase((PASSIS.LIB.Utility.Utili.getExamGradeLetters(Math.Round(overallTotal), (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue))), darkerGrnFnt6)); gradeCell.Colspan = 1;
                        resTable.AddCell(gradeCell);
                      
                        PdfPCell noOfStudentsCell = new PdfPCell(new Phrase("", darkerGrnFnt6)); noOfStudentsCell.Colspan = 1;
                        resTable.AddCell(noOfStudentsCell);

                        PdfPCell tRemark = new PdfPCell(new Phrase(PASSIS.LIB.Utility.Utili.getExamGradeRemarks(Math.Round(overallTotal), (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)), darkerGrnFnt6)); tRemark.Colspan = 2;
                        resTable.AddCell(tRemark);


                        //++noOfSubjectWithScore;
                        //totalMarkObtainable = 0;
                        //totalMarkObtained = 0;
                        //testScore = 0;
                    }
                }

            }


            //document.Add(new Phrase(Environment.NewLine));
            if (subjectCounter != 0)
            {
                percentage = (aggregateTotalScore / (subjectCounter * 100)) * 100;
            }
            //string classTeacherComment = txtClassTeacherComment.Text.Trim();
            //string proprietorComment = txtProprietorComment.Text.Trim();
            //PdfPTable tbl = new PdfPTable(15);
            PdfPTable totalScoreObtainedTbl = new PdfPTable(15);
            PdfPTable totalScoreObtainableTbl = new PdfPTable(15);
            PdfPTable noOfSubjectsTbl = new PdfPTable(15);
            PdfPTable noOfDistinctionsTbl = new PdfPTable(15);
            PdfPTable noOfCreditsTbl = new PdfPTable(15);
            PdfPTable noOfPassesTbl = new PdfPTable(15);
            PdfPTable noOfFailureTbl = new PdfPTable(15);


            //PdfPCell emptyCell1 = new PdfPCell(new Phrase("", blackFnt)); emptyCell1.Colspan = 2; emptyCell1.HorizontalAlignment = Element.ALIGN_LEFT; 
            //PdfPCell emptyCell2 = new PdfPCell(new Phrase("", blackFnt)); emptyCell2.Colspan = 2; emptyCell2.HorizontalAlignment = Element.ALIGN_LEFT; 
            //PdfPCell emptyCell3 = new PdfPCell(new Phrase("", blackFnt)); emptyCell3.Colspan = 1; emptyCell3.HorizontalAlignment = Element.ALIGN_LEFT; emptyCell3.Border = 0;
            //PdfPCell emptyCell4 = new PdfPCell(new Phrase("", blackFnt)); emptyCell4.Colspan = 1; emptyCell4.HorizontalAlignment = Element.ALIGN_LEFT; emptyCell4.Border = 0;
            //PdfPCell emptyCell5 = new PdfPCell(new Phrase("", blackFnt)); emptyCell5.Colspan = 1; emptyCell5.HorizontalAlignment = Element.ALIGN_LEFT; emptyCell5.Border = 0;
            //PdfPCell emptyCell6 = new PdfPCell(new Phrase("", blackFnt)); emptyCell6.Colspan = 1; emptyCell6.HorizontalAlignment = Element.ALIGN_LEFT; emptyCell6.Border = 0;
            //PdfPCell emptyCell7 = new PdfPCell(new Phrase("", blackFnt)); emptyCell7.Colspan = 1; emptyCell7.HorizontalAlignment = Element.ALIGN_LEFT; emptyCell7.Border = 0;
            //PdfPCell emptyCell8 = new PdfPCell(new Phrase("", blackFnt)); emptyCell8.Colspan = 1; emptyCell8.HorizontalAlignment = Element.ALIGN_LEFT; emptyCell8.Border = 0;
            //PdfPCell emptyCell9 = new PdfPCell(new Phrase("", blackFnt)); emptyCell9.Colspan = 1; emptyCell9.HorizontalAlignment = Element.ALIGN_LEFT; emptyCell9.Border = 0;
            //PdfPCell emptyCell10 = new PdfPCell(new Phrase("", blackFnt)); emptyCell10.Colspan = 1; emptyCell10.HorizontalAlignment = Element.ALIGN_LEFT; emptyCell10.Border = 0;
            //PdfPCell emptyCell11 = new PdfPCell(new Phrase("", blackFnt)); emptyCell11.Colspan = 1; emptyCell11.HorizontalAlignment = Element.ALIGN_LEFT; emptyCell11.Border = 0;
            //PdfPCell emptyCell12 = new PdfPCell(new Phrase("", blackFnt)); emptyCell12.Colspan = 2; emptyCell12.HorizontalAlignment = Element.ALIGN_LEFT; emptyCell12.Border = 0;

            //tbl.AddCell(emptyCell1); emptyCell1.Colspan = 2;
            //tbl.AddCell(emptyCell2);emptyCell2.Colspan = 2;
            //tbl.AddCell(emptyCell3); emptyCell3.Colspan = 1;
            //tbl.AddCell(emptyCell4); emptyCell4.Colspan = 1;
            //tbl.AddCell(emptyCell5); emptyCell5.Colspan = 1;
            //tbl.AddCell(emptyCell6); emptyCell6.Colspan = 1;
            //tbl.AddCell(emptyCell7); emptyCell7.Colspan = 1;
            //tbl.AddCell(emptyCell8); emptyCell8.Colspan = 1;
            //tbl.AddCell(emptyCell9); emptyCell9.Colspan = 1;
            //tbl.AddCell(emptyCell10); emptyCell10.Colspan = 1;
            //tbl.AddCell(emptyCell11); emptyCell11.Colspan = 1;
            //tbl.AddCell(emptyCell12); emptyCell12.Colspan = 1;



            //iTextSharp.text.pdf.draw.LineSeparator line = new iTextSharp.text.pdf.draw.LineSeparator(0f, 100f, BaseColor.BLACK, Element.ALIGN_LEFT, 1);
            //PdfPCell Summarycell1 = new PdfPCell(new Phrase("Total Score Obtained:", blackFnt)); Summarycell1.Colspan = 4; Summarycell1.HorizontalAlignment = Element.ALIGN_LEFT; 
            //PdfPCell Summarycell2 = new PdfPCell(new Phrase(Math.Round(aggregateTotalScore, 0).ToString(), blackFnt)); Summarycell2.Colspan = 2; Summarycell2.HorizontalAlignment = Element.ALIGN_LEFT;
            //PdfPCell emptyCell3 = new PdfPCell(new Phrase("", blackFnt)); emptyCell3.Colspan = 1; emptyCell3.HorizontalAlignment = Element.ALIGN_LEFT; emptyCell3.Border = 0;
            //PdfPCell emptyCell4 = new PdfPCell(new Phrase("", blackFnt)); emptyCell4.Colspan = 1; emptyCell4.HorizontalAlignment = Element.ALIGN_LEFT; emptyCell4.Border = 0;
            //PdfPCell emptyCell5 = new PdfPCell(new Phrase("", blackFnt)); emptyCell5.Colspan = 1; emptyCell5.HorizontalAlignment = Element.ALIGN_LEFT; emptyCell5.Border = 0;
            //PdfPCell emptyCell6 = new PdfPCell(new Phrase("", blackFnt)); emptyCell6.Colspan = 1; emptyCell6.HorizontalAlignment = Element.ALIGN_LEFT; emptyCell6.Border = 0;
            //PdfPCell emptyCell7 = new PdfPCell(new Phrase("", blackFnt)); emptyCell7.Colspan = 1; emptyCell7.HorizontalAlignment = Element.ALIGN_LEFT; emptyCell7.Border = 0;
            //PdfPCell emptyCell8 = new PdfPCell(new Phrase("", blackFnt)); emptyCell8.Colspan = 1; emptyCell8.HorizontalAlignment = Element.ALIGN_LEFT; emptyCell8.Border = 0;
            //PdfPCell emptyCell9 = new PdfPCell(new Phrase("", blackFnt)); emptyCell9.Colspan = 1; emptyCell9.HorizontalAlignment = Element.ALIGN_LEFT; emptyCell9.Border = 0;
            //PdfPCell emptyCell10 = new PdfPCell(new Phrase("", blackFnt)); emptyCell10.Colspan = 2; emptyCell10.HorizontalAlignment = Element.ALIGN_LEFT; emptyCell10.Border = 0;
            //totalScoreObtainedTbl.AddCell(Summarycell1); Summarycell1.Colspan = 4; 
            //totalScoreObtainedTbl.AddCell(Summarycell2); Summarycell2.Colspan = 2;
            //totalScoreObtainedTbl.AddCell(emptyCell3); emptyCell3.Colspan = 1;
            //totalScoreObtainedTbl.AddCell(emptyCell4); emptyCell4.Colspan = 1;
            //totalScoreObtainedTbl.AddCell(emptyCell5); emptyCell5.Colspan = 1;
            //totalScoreObtainedTbl.AddCell(emptyCell6); emptyCell6.Colspan = 1;
            //totalScoreObtainedTbl.AddCell(emptyCell7); emptyCell7.Colspan = 1;
            //totalScoreObtainedTbl.AddCell(emptyCell8); emptyCell8.Colspan = 1;
            //totalScoreObtainedTbl.AddCell(emptyCell9); emptyCell9.Colspan = 1;
            //totalScoreObtainedTbl.AddCell(emptyCell10); emptyCell10.Colspan = 2;

            //PdfPCell Summarycell3 = new PdfPCell(new Phrase("Total Score Obtainable:", blackFnt)); Summarycell3.Colspan = 4; Summarycell3.HorizontalAlignment = Element.ALIGN_LEFT; 
            //PdfPCell Summarycell4 = new PdfPCell(new Phrase((subjectCounter * 100).ToString(), blackFnt)); Summarycell4.Colspan = 2; Summarycell4.HorizontalAlignment = Element.ALIGN_LEFT;
            //totalScoreObtainableTbl.AddCell(Summarycell3); Summarycell1.Colspan = 4;
            //totalScoreObtainableTbl.AddCell(Summarycell4); Summarycell2.Colspan = 2;
            //totalScoreObtainableTbl.AddCell(emptyCell3); emptyCell3.Colspan = 1;
            //totalScoreObtainableTbl.AddCell(emptyCell4); emptyCell4.Colspan = 1;
            //totalScoreObtainableTbl.AddCell(emptyCell5); emptyCell5.Colspan = 1;
            //totalScoreObtainableTbl.AddCell(emptyCell6); emptyCell6.Colspan = 1;
            //totalScoreObtainableTbl.AddCell(emptyCell7); emptyCell7.Colspan = 1;
            //totalScoreObtainableTbl.AddCell(emptyCell8); emptyCell8.Colspan = 1;
            //totalScoreObtainableTbl.AddCell(emptyCell9); emptyCell9.Colspan = 1;
            //totalScoreObtainableTbl.AddCell(emptyCell10); emptyCell10.Colspan = 2;

            //PdfPCell summaryRow5Cell1 = new PdfPCell(new Phrase("Number of Subject Offered:", blackFnt)); summaryRow5Cell1.Colspan = 4; summaryRow5Cell1.HorizontalAlignment = Element.ALIGN_LEFT; 
            //PdfPCell summaryRow5Cell2 = new PdfPCell(new Phrase(subjectCounter.ToString(), blackFnt)); summaryRow5Cell2.Colspan = 2;
            //noOfSubjectsTbl.AddCell(summaryRow5Cell1); summaryRow5Cell1.Colspan = 4;
            //noOfSubjectsTbl.AddCell(summaryRow5Cell2); summaryRow5Cell2.Colspan = 2;
            //noOfSubjectsTbl.AddCell(emptyCell3); emptyCell3.Colspan = 1;
            //noOfSubjectsTbl.AddCell(emptyCell4); emptyCell4.Colspan = 1;
            //noOfSubjectsTbl.AddCell(emptyCell5); emptyCell5.Colspan = 1;
            //noOfSubjectsTbl.AddCell(emptyCell6); emptyCell6.Colspan = 1;
            //noOfSubjectsTbl.AddCell(emptyCell7); emptyCell7.Colspan = 1;
            //noOfSubjectsTbl.AddCell(emptyCell8); emptyCell8.Colspan = 1;
            //noOfSubjectsTbl.AddCell(emptyCell9); emptyCell9.Colspan = 1;
            //noOfSubjectsTbl.AddCell(emptyCell10); emptyCell10.Colspan = 2;

            //PdfPCell summaryRow5Cell3 = new PdfPCell(new Phrase("Number of Distinctions:", blackFnt)); summaryRow5Cell3.Colspan = 4; summaryRow5Cell3.HorizontalAlignment = Element.ALIGN_LEFT; 
            //PdfPCell summaryRow5Cell4 = new PdfPCell(new Phrase(distinctionCounter.ToString(), blackFnt)); summaryRow5Cell4.Colspan = 2;
            //noOfDistinctionsTbl.AddCell(summaryRow5Cell3); summaryRow5Cell3.Colspan = 4;
            //noOfDistinctionsTbl.AddCell(summaryRow5Cell4); summaryRow5Cell4.Colspan = 2;
            //noOfDistinctionsTbl.AddCell(emptyCell3); emptyCell3.Colspan = 1;
            //noOfDistinctionsTbl.AddCell(emptyCell4); emptyCell4.Colspan = 1;
            //noOfDistinctionsTbl.AddCell(emptyCell5); emptyCell5.Colspan = 1;
            //noOfDistinctionsTbl.AddCell(emptyCell6); emptyCell6.Colspan = 1;
            //noOfDistinctionsTbl.AddCell(emptyCell7); emptyCell7.Colspan = 1;
            //noOfDistinctionsTbl.AddCell(emptyCell8); emptyCell8.Colspan = 1;
            //noOfDistinctionsTbl.AddCell(emptyCell9); emptyCell9.Colspan = 1;
            //noOfDistinctionsTbl.AddCell(emptyCell10); emptyCell10.Colspan = 2;

            //PdfPCell summaryRow5Cell5 = new PdfPCell(new Phrase("Number of Credits:", blackFnt)); summaryRow5Cell5.Colspan = 4; summaryRow5Cell5.HorizontalAlignment = Element.ALIGN_LEFT;
            //PdfPCell summaryRow5Cell6 = new PdfPCell(new Phrase(creditCounter.ToString(), blackFnt)); summaryRow5Cell6.Colspan = 2;
            //noOfCreditsTbl.AddCell(summaryRow5Cell5); summaryRow5Cell5.Colspan = 4;
            //noOfCreditsTbl.AddCell(summaryRow5Cell6); summaryRow5Cell6.Colspan = 2;
            //noOfCreditsTbl.AddCell(emptyCell3); emptyCell3.Colspan = 1;
            //noOfCreditsTbl.AddCell(emptyCell4); emptyCell4.Colspan = 1;
            //noOfCreditsTbl.AddCell(emptyCell5); emptyCell5.Colspan = 1;
            //noOfCreditsTbl.AddCell(emptyCell6); emptyCell6.Colspan = 1;
            //noOfCreditsTbl.AddCell(emptyCell7); emptyCell7.Colspan = 1;
            //noOfCreditsTbl.AddCell(emptyCell8); emptyCell8.Colspan = 1;
            //noOfCreditsTbl.AddCell(emptyCell9); emptyCell9.Colspan = 1;
            //noOfCreditsTbl.AddCell(emptyCell10); emptyCell10.Colspan = 2;

            //PdfPCell summaryRow5Cell7 = new PdfPCell(new Phrase("Number of Passes:", blackFnt)); summaryRow5Cell7.Colspan = 4; summaryRow5Cell7.HorizontalAlignment = Element.ALIGN_LEFT; 
            //PdfPCell summaryRow5Cell8 = new PdfPCell(new Phrase(passesCounter.ToString(), blackFnt)); summaryRow5Cell8.Colspan = 2;
            //noOfPassesTbl.AddCell(summaryRow5Cell7); summaryRow5Cell7.Colspan = 4;
            //noOfPassesTbl.AddCell(summaryRow5Cell8); summaryRow5Cell8.Colspan = 2;
            //noOfPassesTbl.AddCell(emptyCell3); emptyCell3.Colspan = 1;
            //noOfPassesTbl.AddCell(emptyCell4); emptyCell4.Colspan = 1;
            //noOfPassesTbl.AddCell(emptyCell5); emptyCell5.Colspan = 1;
            //noOfPassesTbl.AddCell(emptyCell6); emptyCell6.Colspan = 1;
            //noOfPassesTbl.AddCell(emptyCell7); emptyCell7.Colspan = 1;
            //noOfPassesTbl.AddCell(emptyCell8); emptyCell8.Colspan = 1;
            //noOfPassesTbl.AddCell(emptyCell9); emptyCell9.Colspan = 1;
            //noOfPassesTbl.AddCell(emptyCell10); emptyCell10.Colspan = 2;


            //PdfPCell summaryRow5Cell9 = new PdfPCell(new Phrase("Number of Failure:", blackFnt)); summaryRow5Cell9.Colspan = 4; summaryRow5Cell9.HorizontalAlignment = Element.ALIGN_LEFT; 
            //PdfPCell summaryRow5Cell10 = new PdfPCell(new Phrase(failureCounter.ToString(), blackFnt)); summaryRow5Cell10.Colspan = 2;
            //noOfFailureTbl.AddCell(summaryRow5Cell9); summaryRow5Cell9.Colspan = 4;
            //noOfFailureTbl.AddCell(summaryRow5Cell10); summaryRow5Cell10.Colspan = 2;
            //noOfFailureTbl.AddCell(emptyCell3); emptyCell3.Colspan = 1;
            //noOfFailureTbl.AddCell(emptyCell4); emptyCell4.Colspan = 1;
            //noOfFailureTbl.AddCell(emptyCell5); emptyCell5.Colspan = 1;
            //noOfFailureTbl.AddCell(emptyCell6); emptyCell6.Colspan = 1;
            //noOfFailureTbl.AddCell(emptyCell7); emptyCell7.Colspan = 1;
            //noOfFailureTbl.AddCell(emptyCell8); emptyCell8.Colspan = 1;
            //noOfFailureTbl.AddCell(emptyCell9); emptyCell9.Colspan = 1;
            //noOfFailureTbl.AddCell(emptyCell10); emptyCell10.Colspan = 2;

            PdfPTable behaviorTable = new PdfPTable(6);
            PdfPCell behaviorCell0 = new PdfPCell(new Phrase("AFFECTIVE AND PSYCHOMOTOR", blackFnt8)); behaviorCell0.Colspan = 6; behaviorCell0.HorizontalAlignment = Element.ALIGN_CENTER;
            behaviorTable.AddCell(behaviorCell0);
            PdfPCell behaviorCell1 = new PdfPCell(new Phrase("5", blackFnt8)); behaviorCell1.Colspan = 2; behaviorCell1.Border = 0;
            behaviorTable.AddCell(behaviorCell1);
            PdfPCell behaviorCell2 = new PdfPCell(new Phrase("Very High", blackFnt8)); behaviorCell2.Colspan = 4; behaviorCell2.Border = 0;
            behaviorTable.AddCell(behaviorCell2);
            PdfPCell behaviorCell3 = new PdfPCell(new Phrase("4", blackFnt8)); behaviorCell3.Colspan = 2; behaviorCell3.Border = 0;
            behaviorTable.AddCell(behaviorCell3);
            PdfPCell behaviorCell4 = new PdfPCell(new Phrase("High", blackFnt8)); behaviorCell4.Colspan = 4; behaviorCell4.Border = 0;
            behaviorTable.AddCell(behaviorCell4);
            PdfPCell behaviorCell5 = new PdfPCell(new Phrase("3", blackFnt8)); behaviorCell5.Colspan = 2; behaviorCell5.Border = 0;
            behaviorTable.AddCell(behaviorCell5);
            PdfPCell behaviorCell6 = new PdfPCell(new Phrase("Average", blackFnt8)); behaviorCell6.Colspan = 4; behaviorCell6.Border = 0;
            behaviorTable.AddCell(behaviorCell6);
            PdfPCell behaviorCell7 = new PdfPCell(new Phrase("2", blackFnt8)); behaviorCell7.Colspan = 2; behaviorCell7.Border = 0;
            behaviorTable.AddCell(behaviorCell7);
            PdfPCell behaviorCell8 = new PdfPCell(new Phrase("Below Average", blackFnt8)); behaviorCell8.Colspan = 4; behaviorCell8.Border = 0;
            behaviorTable.AddCell(behaviorCell8);
            PdfPCell behaviorCell9 = new PdfPCell(new Phrase("1", blackFnt8)); behaviorCell9.Colspan = 2; behaviorCell9.Border = 0;
            behaviorTable.AddCell(behaviorCell9);
            PdfPCell behaviorCell10 = new PdfPCell(new Phrase("Low", blackFnt8)); behaviorCell10.Colspan = 4; behaviorCell10.Border = 0;
            behaviorTable.AddCell(behaviorCell10);
            PdfPCell behaviorCell11 = new PdfPCell(new Phrase("", blackFnt8)); behaviorCell11.Colspan = 4; behaviorCell11.Border = 0;
            behaviorTable.AddCell(behaviorCell11);
            PdfPCell behaviorCell12 = new PdfPCell(new Phrase("RATINGS", blackFnt8)); behaviorCell12.Colspan = 2; behaviorCell12.HorizontalAlignment = Element.ALIGN_CENTER; behaviorCell12.Border = 0;
            behaviorTable.AddCell(behaviorCell12);
            //PdfPCell behaviorCell13 = new PdfPCell(new Phrase("Punctuality", blackFnt8)); behaviorCell13.Colspan = 4;
            //behaviorTable.AddCell(behaviorCell13);
            //PdfPCell behaviorCell14 = new PdfPCell(new Phrase("5", blackFnt8)); behaviorCell14.Colspan = 2;
            //behaviorTable.AddCell(behaviorCell14);
            //PdfPCell behaviorCell15 = new PdfPCell(new Phrase("", blackFnt8)); behaviorCell15.Colspan = 4;
            //behaviorTable.AddCell(behaviorCell15);
            //PdfPCell behaviorCell16 = new PdfPCell(new Phrase("", blackFnt8)); behaviorCell16.Colspan = 2;
            //behaviorTable.AddCell(behaviorCell16);


            PdfPTable rightTable = new PdfPTable(5);
            PdfPCell rightRow1 = new PdfPCell(resTable); rightRow1.Colspan = 4;
            PdfPCell rightRow3 = new PdfPCell(behaviorTable); rightRow3.Colspan = 1;
            rightTable.AddCell(rightRow1);
            rightTable.AddCell(rightRow3);



            PdfPTable lastTable = new PdfPTable(5);
            PdfPCell EmptyCell = new PdfPCell(new Phrase(" ", blackFnt8)); EmptyCell.Colspan = 6; EmptyCell.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell1Row1.Border = 0;

            PdfPCell lastCell1Row2 = new PdfPCell(new Phrase("TOTAL SCORE OBATAINED", blackFnt8)); lastCell1Row2.Colspan = 4; lastCell1Row2.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell1Row4.Border = 0;
            PdfPCell lastCell2Row2 = new PdfPCell(new Phrase(Math.Round(aggregateTotalScore, 1).ToString(), blackFnt8)); lastCell2Row2.Colspan = 1; lastCell2Row2.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell2Row4.Border = 0;
            PdfPCell lastCell1Row3 = new PdfPCell(new Phrase("TOTAL SCORE OBTAINABLE", blackFnt8)); lastCell1Row3.Colspan = 4; lastCell1Row3.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell1Row4.Border = 0;
            PdfPCell lastCell2Row4 = new PdfPCell(new Phrase((subjectCounter * 100).ToString(), blackFnt8)); lastCell2Row4.Colspan = 1; lastCell2Row4.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell2Row4.Border = 0;
            PdfPCell lastCell1Row5 = new PdfPCell(new Phrase("NUMBER OF SUBJECTS OFFERED", blackFnt8)); lastCell1Row5.Colspan = 4; lastCell1Row5.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell1Row4.Border = 0;
            PdfPCell lastCell2Row6 = new PdfPCell(new Phrase(subjectCounter.ToString(), blackFnt8)); lastCell2Row6.Colspan = 1; lastCell2Row6.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell2Row4.Border = 0;
            PdfPCell lastCell1Row7 = new PdfPCell(new Phrase("NUMBER OF DISTINCTIONS", blackFnt8)); lastCell1Row7.Colspan = 4; lastCell1Row7.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell1Row4.Border = 0;
            PdfPCell lastCell2Row8 = new PdfPCell(new Phrase(distinctionCounter.ToString(), blackFnt8)); lastCell2Row8.Colspan = 1; lastCell2Row8.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell2Row4.Border = 0;
            PdfPCell lastCell1Row9 = new PdfPCell(new Phrase("NUMBER OF CREDITS", blackFnt8)); lastCell1Row9.Colspan = 4; lastCell1Row9.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell1Row4.Border = 0;
            PdfPCell lastCell2Row10 = new PdfPCell(new Phrase(creditCounter.ToString(), blackFnt8)); lastCell2Row10.Colspan = 1; lastCell2Row10.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell2Row4.Border = 0;
            PdfPCell lastCell1Row11 = new PdfPCell(new Phrase("NUMBER OF PASSES", blackFnt8)); lastCell1Row11.Colspan = 4; lastCell1Row11.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell1Row4.Border = 0;
            PdfPCell lastCell2Row12 = new PdfPCell(new Phrase(passesCounter.ToString(), blackFnt8)); lastCell2Row12.Colspan = 1; lastCell2Row12.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell2Row4.Border = 0;
            PdfPCell lastCell1Row13 = new PdfPCell(new Phrase("NUMBER OF FAILURE", blackFnt8)); lastCell1Row13.Colspan = 4; lastCell1Row13.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell1Row4.Border = 0;
            PdfPCell lastCell2Row14 = new PdfPCell(new Phrase(failureCounter.ToString(), blackFnt8)); lastCell2Row14.Colspan = 1; lastCell2Row14.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell2Row4.Border = 0;


            lastTable.AddCell(lastCell1Row2);
            lastTable.AddCell(lastCell2Row2);
            lastTable.AddCell(lastCell1Row3);
            lastTable.AddCell(lastCell2Row4);
            lastTable.AddCell(lastCell1Row5);
            lastTable.AddCell(lastCell2Row6);
            lastTable.AddCell(lastCell1Row7);
            lastTable.AddCell(lastCell2Row8);
            lastTable.AddCell(lastCell1Row9);
            lastTable.AddCell(lastCell2Row10);
            lastTable.AddCell(lastCell1Row11);
            lastTable.AddCell(lastCell2Row12);
            lastTable.AddCell(lastCell1Row13);
            lastTable.AddCell(lastCell2Row14);
            //lastTable.AddCell(EmptyCell);
            //lastTable.AddCell(lastCell1Row3);
            //lastTable.AddCell(lastCell2Row3);
            //lastTable.AddCell(EmptyCell);
            //lastTable.AddCell(lastCell1Row4);
            //lastTable.AddCell(lastCell2Row4);







            PdfPTable gradingTable = new PdfPTable(4);

            PdfPCell gradingRow0Cell0 = new PdfPCell(new Phrase("COGNITIVE RATING", blackFnt8)); gradingRow0Cell0.Colspan = 4; gradingRow0Cell0.HorizontalAlignment = Element.ALIGN_CENTER;
            gradingTable.AddCell(gradingRow0Cell0);
            PdfPCell gradingRow1Cell1 = new PdfPCell(new Phrase("90 - Above", blackFnt8)); gradingRow1Cell1.Colspan = 2; /*gradingRow1Cell1.Border = 0;*/
             gradingTable.AddCell(gradingRow1Cell1);
            PdfPCell gradingRow1Cell2 = new PdfPCell(new Phrase("A+ (Outstanding)", blackFnt8)); gradingRow1Cell2.Colspan = 2; /*gradingRow1Cell2.Border = 0;*/
            gradingTable.AddCell(gradingRow1Cell2);
            //PdfPCell gradingRow1Cell3 = new PdfPCell(new Phrase("DISTINCTION", blackFnt8)); gradingRow1Cell3.Colspan = 2;
            //gradingTable.AddCell(gradingRow1Cell3);

            PdfPCell gradingRow2Cell1 = new PdfPCell(new Phrase("80 - 89", blackFnt8)); gradingRow2Cell1.Colspan = 2; /*gradingRow2Cell1.Border = 0;*/
            gradingTable.AddCell(gradingRow2Cell1);
            PdfPCell gradingRow2Cell2 = new PdfPCell(new Phrase("A (Excellent)", blackFnt8)); gradingRow2Cell2.Colspan = 2; /*gradingRow2Cell2.Border = 0;*/
            gradingTable.AddCell(gradingRow2Cell2);
            //PdfPCell gradingRow2Cell3 = new PdfPCell(new Phrase("EXCELLENT", blackFnt8)); gradingRow2Cell3.Colspan = 2;
            //gradingTable.AddCell(gradingRow2Cell3);

            PdfPCell gradingRow3Cell1 = new PdfPCell(new Phrase("75 - 79", blackFnt8)); gradingRow3Cell1.Colspan = 2; /*gradingRow3Cell1.Border = 0;*/
            gradingTable.AddCell(gradingRow3Cell1);
            PdfPCell gradingRow3Cell2 = new PdfPCell(new Phrase("A- (Very Good)", blackFnt8)); gradingRow3Cell2.Colspan = 2; /*gradingRow3Cell2.Border = 0;*/
            gradingTable.AddCell(gradingRow3Cell2);
            //PdfPCell gradingRow3Cell3 = new PdfPCell(new Phrase("VERY GOOD", blackFnt8)); gradingRow3Cell3.Colspan = 2;
            //gradingTable.AddCell(gradingRow3Cell3);

            PdfPCell gradingRow4Cell1 = new PdfPCell(new Phrase("70 - 74", blackFnt8)); gradingRow4Cell1.Colspan = 2; /*gradingRow4Cell1.Border = 0;*/
            gradingTable.AddCell(gradingRow4Cell1);
            PdfPCell gradingRow4Cell2 = new PdfPCell(new Phrase("B+ (Good)", blackFnt8)); gradingRow4Cell2.Colspan = 2; /*gradingRow4Cell2.Border = 0;*/
            gradingTable.AddCell(gradingRow4Cell2);
            //PdfPCell gradingRow4Cell3 = new PdfPCell(new Phrase("GOOD", blackFnt8)); gradingRow4Cell3.Colspan = 2;
            //gradingTable.AddCell(gradingRow4Cell3);

            PdfPCell gradingRow5Cell1 = new PdfPCell(new Phrase("65 - 69", blackFnt8)); gradingRow5Cell1.Colspan = 2; /*gradingRow5Cell1.Border = 0;*/
            gradingTable.AddCell(gradingRow5Cell1);
            PdfPCell gradingRow5Cell2 = new PdfPCell(new Phrase("B (Fairly Good)", blackFnt8)); gradingRow5Cell2.Colspan = 2; /*gradingRow5Cell2.Border = 0;*/
            gradingTable.AddCell(gradingRow5Cell2);
            //PdfPCell gradingRow5Cell3 = new PdfPCell(new Phrase("CREDIT", blackFnt8)); gradingRow5Cell3.Colspan = 2;
            //gradingTable.AddCell(gradingRow5Cell3);

            PdfPCell gradingRow6Cell1 = new PdfPCell(new Phrase("60 - 64", blackFnt8)); gradingRow6Cell1.Colspan = 2; /*gradingRow6Cell1.Border = 0;*/
            gradingTable.AddCell(gradingRow6Cell1);
            PdfPCell gradingRow6Cell2 = new PdfPCell(new Phrase("C+ (Fair)", blackFnt8)); gradingRow6Cell2.Colspan = 2; /*gradingRow6Cell2.Border = 0;*/
            gradingTable.AddCell(gradingRow6Cell2);
            //PdfPCell gradingRow6Cell3 = new PdfPCell(new Phrase("FAIR", blackFnt8)); gradingRow6Cell3.Colspan = 2;
            //gradingTable.AddCell(gradingRow6Cell3);

            PdfPCell gradingRow7Cell1 = new PdfPCell(new Phrase("55 - 59", blackFnt8)); gradingRow7Cell1.Colspan = 2; /*gradingRow7Cell1.Border = 0;*/
            gradingTable.AddCell(gradingRow7Cell1);
            PdfPCell gradingRow7Cell2 = new PdfPCell(new Phrase("C (Just Fair)", blackFnt8)); gradingRow7Cell2.Colspan = 2; /*gradingRow7Cell2.Border = 0;*/
            gradingTable.AddCell(gradingRow7Cell2);
            //PdfPCell gradingRow7Cell3 = new PdfPCell(new Phrase("POOR", blackFnt8)); gradingRow7Cell3.Colspan = 2;
            //gradingTable.AddCell(gradingRow7Cell3);

            PdfPCell gradingRow8Cell1 = new PdfPCell(new Phrase("50 - 54", blackFnt8)); gradingRow8Cell1.Colspan = 2; /*gradingRow8Cell1.Border = 0;*/
            gradingTable.AddCell(gradingRow8Cell1);
            PdfPCell gradingRow8Cell2 = new PdfPCell(new Phrase("C- (Average)", blackFnt8)); gradingRow8Cell2.Colspan = 2; /*gradingRow8Cell2.Border = 0;*/
            gradingTable.AddCell(gradingRow8Cell2);

            PdfPCell gradingRow9Cell1 = new PdfPCell(new Phrase("0 - 49", blackFnt8)); gradingRow9Cell1.Colspan = 2; /*gradingRow9Cell1.Border = 0;*/
            gradingTable.AddCell(gradingRow9Cell1);
            PdfPCell gradingRow9Cell2 = new PdfPCell(new Phrase("E (Work Hrader)", blackFnt8)); gradingRow9Cell2.Colspan = 2; /*gradingRow9Cell2.Border = 0;*/
            gradingTable.AddCell(gradingRow9Cell2);

            PdfPCell gradingRow10Cell1 = new PdfPCell(new Phrase("PERCENTAGE", blackFnt8)); gradingRow10Cell1.Colspan = 4; gradingRow10Cell1.HorizontalAlignment = Element.ALIGN_CENTER; /*gradingRow10Cell1.Border = 0;*/
            gradingTable.AddCell(gradingRow10Cell1);
            PdfPCell gradingRow11Cell2 = new PdfPCell(new Phrase("", blackFnt8)); gradingRow11Cell2.Colspan = 1; /*gradingRow11Cell2.BorderColor;*/
            gradingTable.AddCell(gradingRow11Cell2);
            PdfPCell gradingRow12Cell2 = new PdfPCell(new Phrase(Math.Round(Convert.ToDecimal(percentage)).ToString() + "%", blackFnt8)); gradingRow12Cell2.Colspan = 2;
            gradingTable.AddCell(gradingRow12Cell2);
            PdfPCell gradingRow13Cell2 = new PdfPCell(new Phrase("", blackFnt8)); gradingRow13Cell2.Colspan = 1; /*gradingRow13Cell2.Border = 0;*/
            gradingTable.AddCell(gradingRow13Cell2);


            PdfPTable baseTable = new PdfPTable(5);
            PdfPCell base1 = new PdfPCell(lastTable); base1.Colspan = 2;
            PdfPCell base2 = new PdfPCell(); base2.Colspan = 2; base2.Border = 0;
            PdfPCell base3 = new PdfPCell(gradingTable); base3.Colspan = 1; base3.Border = 0;
            baseTable.AddCell(base1);
            baseTable.AddCell(base2);
            baseTable.AddCell(base3);


            string examinerComment = "";
            string classTeacherComment = "";
            string headTeacherComment = "";
            string parentComment = "";
            string classTeacherName = "";
            PASSIS.LIB.ReportCardComment objExaminer = context.ReportCardComments.FirstOrDefault(x => x.CommentConfigId == 1 && x.AcademicSessionID == session && x.TermId == term && x.StudentId == student.Id);
            if (objExaminer != null)
            {
                examinerComment = objExaminer.Comment;
            }
            PASSIS.LIB.ReportCardComment objClassTeacher = context.ReportCardComments.FirstOrDefault(x => x.CommentConfigId == 2 && x.AcademicSessionID == session && x.TermId == term && x.StudentId == student.Id);
            if (objClassTeacher != null)
            {
                classTeacherComment = objClassTeacher.Comment;
            }
            PASSIS.LIB.ReportCardComment objHeadTeacher = context.ReportCardComments.FirstOrDefault(x => x.CommentConfigId == 3 && x.AcademicSessionID == session && x.TermId == term && x.StudentId == student.Id);
            if (objHeadTeacher != null)
            {
                headTeacherComment = objHeadTeacher.Comment;
            }
            PASSIS.LIB.ReportCardComment objParent = context.ReportCardComments.FirstOrDefault(x => x.CommentConfigId == 4 && x.AcademicSessionID == session && x.TermId == term && x.StudentId == student.Id);
            if (objParent != null)
            {
                parentComment = objHeadTeacher.Comment;
            }


            PdfPTable commentsTable = new PdfPTable(15);
            PdfPCell summaryRow2Cell1 = new PdfPCell(new Phrase("CLASS TEACHER'S COMMENT:", blackFnt8)); summaryRow2Cell1.Colspan = 5; summaryRow2Cell1.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow2Cell1.Border = 0;
            PdfPCell summaryRow2Cell2 = new PdfPCell(new Phrase(classTeacherComment, blackFnt8)); summaryRow2Cell2.Colspan = 10; summaryRow2Cell2.Border = 0;

            PdfPCell summaryRow4Cell1 = new PdfPCell(new Phrase("PRINCIPAL'S COMMENT:", blackFnt8)); summaryRow4Cell1.Colspan = 5; summaryRow4Cell1.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow4Cell1.Border = 0;
            PdfPCell summaryRow4Cell2 = new PdfPCell(new Phrase(headTeacherComment, blackFnt8)); summaryRow4Cell2.Colspan = 10; summaryRow4Cell2.Border = 0;

            //summaryTable.AddCell(Summarycell1); summaryTable.AddCell(Summarycell2); 
            //summaryTable.AddCell(Summarycell3); summaryTable.AddCell(Summarycell4);
            ////summaryTable.AddCell(Summarycell5); summaryTable.AddCell(Summarycell6);


            //summaryTable.AddCell(summaryRow5Cell1); summaryTable.AddCell(summaryRow5Cell2);
            //summaryTable.AddCell(summaryRow5Cell3); summaryTable.AddCell(summaryRow5Cell4);
            //summaryTable.AddCell(summaryRow5Cell5); summaryTable.AddCell(summaryRow5Cell6);
            //summaryTable.AddCell(summaryRow5Cell7); summaryTable.AddCell(summaryRow5Cell8);
            //summaryTable.AddCell(summaryRow5Cell9); summaryTable.AddCell(summaryRow5Cell10);



            //commentsTable.AddCell(summaryRow2Cell1); commentsTable.AddCell(summaryRow2Cell2);
            //commentsTable.AddCell(summaryRow4Cell1); commentsTable.AddCell(summaryRow4Cell2);
            // The row6cell2 was placed here because we need the calculation to present the total number of subject after counting the subject
            PdfPCell row6cell2 = new PdfPCell(new Phrase(subjectCounter.ToString())); row6cell2.HorizontalAlignment = Element.ALIGN_LEFT; row6cell2.Colspan = 1; //row6cell2.Border = 0;
                 var behavioral = from s in context.ScoreSubCategoryConfigurations
                             where s.ScoreCategoryConfiguration.Category == "Behavioral"
                                 && s.ScoreCategoryConfiguration.SchoolId == schoolId && s.ScoreCategoryConfiguration.CampusId == logonUser.SchoolCampusId
                                 && s.ScoreCategoryConfiguration.ClassId == yearId && s.SessionId == session && s.TermId == TermId
                             select s;
            var extra = from s in context.ScoreSubCategoryConfigurations
                        where s.ScoreCategoryConfiguration.Category == "Extra Curricular"
                                 && s.ScoreCategoryConfiguration.SchoolId == schoolId && s.ScoreCategoryConfiguration.CampusId == logonUser.SchoolCampusId
                                 && s.ScoreCategoryConfiguration.ClassId == yearId && s.SessionId == session && s.TermId == TermId
                        select s;

            //foreach (ScoreSubCategoryConfiguration s in extra)
            //{
            //    StudentScoreExtraCurricular ssb = context.StudentScoreExtraCurriculars.FirstOrDefault(x => x.SubCategoryId == s.Id && x.StudentId == student.Id && x.AdmissionNo == student.AdmissionNumber && x.SessionId == session && x.TermId == term && x.SchoolId == schoolId && x.ClassId == yearId && x.GradeId == gradeId);
            //    PdfPCell psychoCells1 = new PdfPCell(new Phrase(s.SubCategory, blackFnt8)); psychoCells1.Colspan = 3;
            //    socialTable.AddCell(psychoCells1);
            //    if (ssb != null)
            //    {
            //        PdfPCell psychoCells2 = new PdfPCell(new Phrase(Convert.ToInt16(ssb.MarkObtained).ToString(), blackFnt8)); psychoCells2.Colspan = 3;
            //        socialTable.AddCell(psychoCells2);
            //    }
            //    else if (ssb == null)
            //    {
            //        PdfPCell psychoCells2 = new PdfPCell(new Phrase("", blackFnt8)); psychoCells2.Colspan = 3;
            //        socialTable.AddCell(psychoCells2);
            //    }
            //}

            foreach (ScoreSubCategoryConfiguration s in behavioral)
            {
                StudentScoreBehavioral ssb = context.StudentScoreBehaviorals.FirstOrDefault(x => x.SubCategoryId == s.Id && x.StudentId == student.Id && x.AdmissionNo == student.AdmissionNumber && x.SessionId == session && x.TermId == term && x.SchoolId == schoolId && x.ClassId == yearId && x.GradeId == gradeId);
                PdfPCell affectiveCells1 = new PdfPCell(new Phrase(s.SubCategory, blackFnt8)); affectiveCells1.Colspan = 4;
                behaviorTable.AddCell(affectiveCells1);
                if (ssb != null)
                {
                    PdfPCell affectiveCells2 = new PdfPCell(new Phrase(Convert.ToInt16(ssb.MarkObtained).ToString(), blackFnt8)); affectiveCells2.Colspan = 2;
                    behaviorTable.AddCell(affectiveCells2);
                }
                else if (ssb == null)
                {
                    PdfPCell affectiveCells2 = new PdfPCell(new Phrase("", blackFnt8)); affectiveCells2.Colspan = 2;
                    behaviorTable.AddCell(affectiveCells2);
                }
            }

            foreach (ScoreSubCategoryConfiguration s in extra)
            {
                StudentScoreExtraCurricular ssb = context.StudentScoreExtraCurriculars.FirstOrDefault(x => x.SubCategoryId == s.Id && x.StudentId == student.Id && x.AdmissionNo == student.AdmissionNumber && x.SessionId == session && x.TermId == term && x.SchoolId == schoolId && x.ClassId == yearId && x.GradeId == gradeId);
                PdfPCell psychoCells1 = new PdfPCell(new Phrase(s.SubCategory, blackFnt8)); psychoCells1.Colspan = 4;
                behaviorTable.AddCell(psychoCells1);
                if (ssb != null)
                {
                    PdfPCell psychoCells2 = new PdfPCell(new Phrase(Convert.ToInt16(ssb.MarkObtained).ToString(), blackFnt8)); psychoCells2.Colspan = 2;
                    behaviorTable.AddCell(psychoCells2);
                }
                else if (ssb == null)
                {
                    PdfPCell psychoCells2 = new PdfPCell(new Phrase("", blackFnt8)); psychoCells2.Colspan = 2;
                    behaviorTable.AddCell(psychoCells2);
                }
            }



            commentsTable.AddCell(summaryRow2Cell1); commentsTable.AddCell(summaryRow2Cell2);
            commentsTable.AddCell(summaryRow4Cell1); commentsTable.AddCell(summaryRow4Cell2);
            // The row6cell2 was placed here because we need the calculation to present the total number of subject after counting the subject
            //PdfPCell row6cell2 = new PdfPCell(new Phrase(subjectCounter.ToString())); row6cell2.HorizontalAlignment = Element.ALIGN_LEFT; row6cell2.Colspan = 1; //row6cell2.Border = 0;
                                                                                                                                                                 //maintbl.AddCell(row6cell1);
                                                                                                                                                                 //maintbl.AddCell(row6cell2);
            document.Add(maintbl);
            document.Add(domain);
            //document.Add(domainandSkills);                                                                                                                                         //maintbl.AddCell(row6cell3);
            //document.Add(new Phrase(Environment.NewLine));
            //document.Add(thirdTerm);
            document.Add(rightTable);
            //document.Add(new Phrase(Environment.NewLine));
            document.Add(baseTable);
            document.Add(commentsTable);
                                                                                                                                                //maintbl.AddCell(row6cell1);
                                                                                                                                                                 //maintbl.AddCell(row6cell2);
                                                                                                                                                                 //maintbl.AddCell(row6cell3);
                                                                                                                                                                 //maintbl.AddCell(row6cell4);
            //document.Add(maintbl);
            //document.Add(domain);
            //document.Add(domainandSkills);
            ////document.Add(new Phrase(Environment.NewLine));
            ////document.Add(amaintbl);
            //document.Add(resTable);
            ////document.Add(tbl);
            //document.Add(totalScoreObtainedTbl);
            //document.Add(totalScoreObtainableTbl);
            ////document.Add(paraGrph);
            //document.Add(noOfSubjectsTbl);
            //document.Add(noOfDistinctionsTbl);
            //document.Add(noOfCreditsTbl);
            //document.Add(noOfPassesTbl);
            //document.Add(noOfFailureTbl);
            //document.Add(commentsTable);


        }
    }

    protected void addResultSummaryPageCrownEndFirstAndSecondTerm(Document document, PASSIS.LIB.User student, UsersLIB usrDal)
    {

        iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(Server.MapPath(SchoolLogo));
        jpg.ScaleToFit(70, 70);
        jpg.Alignment = iTextSharp.text.Image.UNDERLYING;
        jpg.SetAbsolutePosition(250, 750);

        Passport = student.PassportFileName;
        if (Passport == null) { Passport = "~/Images/student3.PNG"; }
        iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(Server.MapPath(Passport));
        jpg1.ScaleToFit(70, 70);
        jpg1.Alignment = iTextSharp.text.Image.UNDERLYING;
        jpg1.SetAbsolutePosition(250, 750);
        PdfPTable innerTable = new PdfPTable(9);
        PdfPCell innerCell1 = new PdfPCell(new Phrase("CROWN JEWEL COLLEGE", darkerRedFnt16)); innerCell1.Colspan = 9; innerCell1.Border = 0; innerCell1.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell innerCell2 = new PdfPCell(new Phrase("9, Bakky Street, Off Bello Folawiyo Street, Ikosi, Ketu, Lagos ", darkerGrnFnt9)); innerCell2.Colspan = 9; innerCell2.Border = 0; innerCell2.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell innerCell3 = new PdfPCell(new Phrase("Phone: 08067864385, 08023070027, 08038796369", blackFnt)); innerCell3.Colspan = 9; innerCell3.Border = 0; innerCell3.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell innerCell4 = new PdfPCell(new Phrase("Email: crownjewelschools@yahoo.com, FB: @crownjewelcollege", blackFnt)); innerCell4.Colspan = 9; innerCell4.Border = 0; innerCell4.HorizontalAlignment = Element.ALIGN_CENTER;
        innerTable.AddCell(innerCell1);
        innerTable.AddCell(innerCell2);
        innerTable.AddCell(innerCell3);
        innerTable.AddCell(innerCell4);
        PdfPTable head = new PdfPTable(15);
        PdfPCell head1 = new PdfPCell(jpg); head1.Colspan = 3; head1.Border = 0;
        PdfPCell head2 = new PdfPCell(innerTable); head2.Colspan = 9; head2.Border = 0;
        PdfPCell head3 = new PdfPCell(jpg1); head3.Colspan = 3; head3.HorizontalAlignment = Element.ALIGN_RIGHT; head3.Border = 0;


        head.AddCell(head1);
        head.AddCell(head2);
        head.AddCell(head3);
        document.Add(head);
        //document.Add(getBackgroundImage());
        //document.Add(new Phrase(Environment.NewLine));
        //document.Add(new Phrase(Environment.NewLine));
        //document.Add(new Phrase(Environment.NewLine));
        //document.Add(new Phrase(Environment.NewLine));
       // document.Add(new Phrase(Environment.NewLine));
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
        long? schId = logonUser.SchoolId;

        //long gradeId = 0;
        //PASSIS.LIB.GradeStudent GetgradeId = context.GradeStudents.SingleOrDefault(x => x.StudentId == student.Id && x.AcademicSessionId == session);
        //if (GetgradeId != null)
        //{
        //    gradeId = GetgradeId.GradeId;
        //}
        long gradeId = new PrintResultt_693().theGradeId(student.Id, session).GradeId;

        string noInClass = noOfStudentInClass(schId, gradeId).Count.ToString();
        string classPosition = "";
        var posInClass = getClassPosition(student.AdmissionNumber, term, session, schoolId, yearId, gradeIdd);
        if (posInClass != null)
        {
            classPosition = posInClass.Position.ToString();
        }

        string totalNoInClass = new ClassGradeLIB().RetrieveGradeStudents(Convert.ToInt64(schId), Convert.ToInt64(logonUser.SchoolCampusId), Convert.ToInt64(yearId), gradeIdd, session).Count().ToString();


        PdfPTable maintbl = new PdfPTable(8);
        PdfPCell cell1 = new PdfPCell(new Phrase("NAME", blackFnt6)); cell1.Colspan = 1; cell1.HorizontalAlignment = Element.ALIGN_LEFT; //cell1.Border = 0;
        PdfPCell cell2 = new PdfPCell(new Phrase(student.StudentFullName, blackFnt6)); cell2.Colspan = 3; cell1.HorizontalAlignment = Element.ALIGN_LEFT; //cell2.Border = 0;
        PdfPCell cell3 = new PdfPCell(new Phrase("SEX", blackFnt6)); cell3.Colspan = 1; cell3.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell cell4 = new PdfPCell(new Phrase(getGender(student.Gender), blackFnt6)); cell4.Colspan = 1; cell4.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell cell5 = new PdfPCell(new Phrase("HOUSE", blackFnt6)); cell5.Colspan = 1; cell5.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell cell6 = new PdfPCell(new Phrase(student.SchoolHouse, blackFnt6)); cell6.Colspan = 1; cell6.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;

        PdfPCell row2cell1 = new PdfPCell(new Phrase("CLASS", blackFnt6)); row2cell1.HorizontalAlignment = Element.ALIGN_LEFT; row2cell1.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell row2cell2 = new PdfPCell(new Phrase(usrDal.getStudentsHomeRoom(student.Id), blackFnt6)); row2cell2.Colspan = 1; row2cell2.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        PdfPCell row2cell3 = new PdfPCell(new Phrase("AGE", blackFnt6)); row2cell3.HorizontalAlignment = Element.ALIGN_LEFT; row2cell3.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell row2cell4 = new PdfPCell(new Phrase(GetAge(Convert.ToDateTime(student.DateOfBirth)).ToString(), blackFnt6)); row2cell4.Colspan = 1; row2cell4.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        PdfPCell row2cell5 = new PdfPCell(new Phrase("SESSION", blackFnt6)); row2cell5.HorizontalAlignment = Element.ALIGN_LEFT; row2cell5.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell row2cell6 = new PdfPCell(new Phrase(ddlAcademicSession.SelectedItem.Text, blackFnt6)); row2cell6.Colspan = 1; row2cell6.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        PdfPCell row2cell7 = new PdfPCell(new Phrase("TERM", blackFnt6)); row2cell1.HorizontalAlignment = Element.ALIGN_LEFT; row2cell7.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell row2cell8 = new PdfPCell(new Phrase(ddlAcademicTerm.SelectedItem.Text, blackFnt6)); row2cell8.Colspan = 1; row2cell8.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;

        PdfPTable Attendance = new PdfPTable(8);
        PdfPCell row3cell1 = new PdfPCell(new Phrase("ATTENDANCE", blackFnt6)); row3cell1.HorizontalAlignment = Element.ALIGN_LEFT; row3cell1.Colspan = 2; //row3cell1.Border = 0;
        PdfPCell row3cell2 = new PdfPCell(new Phrase("", blackFnt6)); row3cell2.HorizontalAlignment = Element.ALIGN_LEFT; row3cell2.Colspan = 2; //row3cell2.Border = 0;
        PdfPCell row3cell3 = new PdfPCell(new Phrase("NUMBER OF TIMES", blackFnt6)); row3cell3.HorizontalAlignment = Element.ALIGN_LEFT; row3cell3.Colspan = 2; //row3cell1.Border = 0;
        PdfPCell row3cell4 = new PdfPCell(new Phrase("PERCENTAGE PRESENT", blackFnt6)); row3cell4.HorizontalAlignment = Element.ALIGN_LEFT; row3cell4.Colspan = 2; //row3cell2.Border = 0;

        PdfPCell row4cell1 = new PdfPCell(new Phrase(" ", blackFnt6)); row4cell1.Colspan = 2; row4cell1.HorizontalAlignment = Element.ALIGN_LEFT; //row4cell1.Border = 0;
        PdfPCell row4cell2 = new PdfPCell(new Phrase("SCHOOL OPENED: " + " ", blackFnt6)); row4cell2.HorizontalAlignment = Element.ALIGN_LEFT; row4cell2.Colspan = 2; //row4cell2.Border = 0;
        PdfPCell row4cell3 = new PdfPCell(new Phrase("PRESENT: " + " ", blackFnt6)); row4cell3.Colspan = 1; row4cell3.HorizontalAlignment = Element.ALIGN_LEFT; //row4cell1.Border = 0;
        PdfPCell row4cell4 = new PdfPCell(new Phrase("ABSENT: " + " ", blackFnt6)); row4cell4.HorizontalAlignment = Element.ALIGN_LEFT; row4cell4.Colspan = 1; //row4cell2.Border = 0;
        PdfPCell row4cell5 = new PdfPCell(new Phrase("PERCENTAGE: " + " ", blackFnt6)); row4cell5.HorizontalAlignment = Element.ALIGN_LEFT; row4cell5.Colspan = 2; //row4cell2.Border = 0;

        Paragraph domain = new Paragraph(string.Format("{0}", " "), darkerRedFnt);
        domain.Alignment = Element.ALIGN_CENTER;

        //PdfPTable domainandSkills = new PdfPTable(15);
        //PdfPCell row5cell1 = new PdfPCell(new Phrase("COGNITIVE SKILLS " + " ", blackFnt)); row5cell1.HorizontalAlignment = Element.ALIGN_CENTER; row5cell1.Colspan = 10; //row4cell2.Border = 0;
        //domainandSkills.AddCell(row5cell1);
        //PdfPCell affectivePsychSkills = new PdfPCell(new Phrase("AFFECTIVE & PSYCHOMOTOR" + " ", blackFnt)); affectivePsychSkills.HorizontalAlignment = Element.ALIGN_CENTER; affectivePsychSkills.Colspan = 9; //row4cell2.Border = 0;
        //domainandSkills.AddCell(affectivePsychSkills);


        maintbl.AddCell(cell1);
        maintbl.AddCell(cell2);
        maintbl.AddCell(cell3);
        maintbl.AddCell(cell4);
        maintbl.AddCell(cell5);
        maintbl.AddCell(cell6);

        maintbl.AddCell(row2cell1);
        maintbl.AddCell(row2cell2);
        maintbl.AddCell(row2cell3);
        maintbl.AddCell(row2cell4);
        maintbl.AddCell(row2cell5);
        maintbl.AddCell(row2cell6);
        maintbl.AddCell(row2cell7);
        maintbl.AddCell(row2cell8);

        maintbl.AddCell(row3cell1);
        maintbl.AddCell(row3cell2);
        maintbl.AddCell(row3cell3);
        maintbl.AddCell(row3cell4);

        maintbl.AddCell(row4cell1);
        maintbl.AddCell(row4cell2);
        maintbl.AddCell(row4cell3);
        maintbl.AddCell(row4cell4);
        maintbl.AddCell(row4cell5);




        ///PdfPTable resultTable = getResultTable(student);/// dont add the table yet so as to retrieve the gpa viewstate value needed in the gpa summ ary slot 
        /// 
        Int64 sessionId = Convert.ToInt64(ddlAcademicSession.SelectedIndex);
        Int64 TermId = Convert.ToInt64(ddlAcademicTerm.SelectedIndex);
        PdfPTable resTable = new PdfPTable(15);
        PdfPTable affectPsychTbl = new PdfPTable(4);

        //resTable.SetWidthPercentage(new float[8] { 80f, 30f, 30f, 30f, 35f, 35f, 38f, 30f }, PageSize.A5.Rotate());
        //resTable.TotalWidth = 400f;
        //resTable.WidthPercentage = 80;
        //resTable.LockedWidth = true;
        PdfPCell row5cell1 = new PdfPCell(new Phrase("COGNITIVE SKILLS " + " ", blackFnt8)); row5cell1.HorizontalAlignment = Element.ALIGN_CENTER; row5cell1.Colspan = 15; //row4cell2.Border = 0;
        resTable.AddCell(row5cell1);
        PdfPCell deptHdr = new PdfPCell(new Phrase("DEPT", blackFnt6)); deptHdr.Padding = 0f;
        deptHdr.Colspan = 2; deptHdr.HorizontalAlignment = Element.ALIGN_CENTER; deptHdr.VerticalAlignment = Element.ALIGN_BOTTOM; 
        resTable.AddCell(deptHdr);
        PdfPCell subjectHdr = new PdfPCell(new Phrase("SUBJECTS", blackFnt6)); subjectHdr.Padding = 0f;
        subjectHdr.Colspan = 2; subjectHdr.HorizontalAlignment = Element.ALIGN_CENTER; subjectHdr.VerticalAlignment = Element.ALIGN_BOTTOM; 
        resTable.AddCell(subjectHdr);
        PdfPCell cwHdr = new PdfPCell(new Phrase("CW", blackFnt6)); cwHdr.Colspan = 1; cwHdr.Rotation = 90;  cwHdr.HorizontalAlignment = Element.ALIGN_LEFT; cwHdr.VerticalAlignment = Element.ALIGN_BOTTOM;
        resTable.AddCell(cwHdr);
        PdfPCell hwHdr = new PdfPCell(); hwHdr.AddElement(new Phrase("HW/PRO", blackFnt6)); hwHdr.Colspan = 1; hwHdr.HorizontalAlignment = Element.ALIGN_LEFT; hwHdr.VerticalAlignment = Element.ALIGN_TOP; hwHdr.Rotation = 90;
        resTable.AddCell(hwHdr);
        PdfPCell attHdr = new PdfPCell(); attHdr.Colspan = 1; attHdr.HorizontalAlignment = Element.ALIGN_LEFT; attHdr.VerticalAlignment = Element.ALIGN_TOP; attHdr.AddElement(new Phrase("ATT/NOTE", blackFnt6)); attHdr.Rotation = 90; resTable.AddCell(attHdr); 
        PdfPCell testHdr = new PdfPCell(); testHdr.Colspan = 1; testHdr.HorizontalAlignment = Element.ALIGN_LEFT; testHdr.VerticalAlignment = Element.ALIGN_TOP; testHdr.AddElement(new Phrase("TEST", blackFnt6)); testHdr.Rotation = 90; resTable.AddCell(testHdr);
        PdfPCell totalHdr = new PdfPCell(); totalHdr.Colspan = 1;  totalHdr.HorizontalAlignment = Element.ALIGN_LEFT; totalHdr.VerticalAlignment = Element.ALIGN_TOP; totalHdr.AddElement(new Phrase("TOTAL", blackFnt6)); totalHdr.Rotation = 90;  resTable.AddCell(totalHdr); 
        PdfPCell caHdr = new PdfPCell(); caHdr.Colspan = 1;  caHdr.HorizontalAlignment = Element.ALIGN_LEFT; caHdr.VerticalAlignment = Element.ALIGN_TOP; caHdr.AddElement(new Phrase("CA", blackFnt6)); caHdr.Rotation = 90; resTable.AddCell(caHdr); 
        PdfPCell examHdr = new PdfPCell(); examHdr.Colspan = 1;  examHdr.HorizontalAlignment = Element.ALIGN_LEFT; examHdr.VerticalAlignment = Element.ALIGN_TOP; examHdr.AddElement(new Phrase("EXAM", blackFnt6)); examHdr.Rotation = 90; resTable.AddCell(examHdr); 
        PdfPCell total2Hdr = new PdfPCell(); total2Hdr.Colspan = 1;  total2Hdr.HorizontalAlignment = Element.ALIGN_LEFT; total2Hdr.VerticalAlignment = Element.ALIGN_TOP; total2Hdr.AddElement(new Phrase("TOTAL", blackFnt6)); total2Hdr.Rotation = 90; resTable.AddCell(total2Hdr); /*total2Hdr.Rotation = 90;*/
        PdfPCell gradeHdr = new PdfPCell(); gradeHdr.Colspan = 1;  gradeHdr.HorizontalAlignment = Element.ALIGN_LEFT; gradeHdr.VerticalAlignment = Element.ALIGN_TOP; gradeHdr.AddElement(new Phrase("GRADE", blackFnt6)); gradeHdr.Rotation = 90;  resTable.AddCell(gradeHdr); /*hwHdr.Rotation = 90;*/
        PdfPCell commentHdr = new PdfPCell(); commentHdr.Colspan = 2; commentHdr.HorizontalAlignment = Element.ALIGN_CENTER; commentHdr.VerticalAlignment = Element.ALIGN_BOTTOM; commentHdr.AddElement(new Phrase("COMMENT", blackFnt6)); resTable.AddCell(commentHdr); /*hwHdr.Rotation = 90;*/
        //PdfPCell aafctivePsych = new PdfPCell(); aafctivePsych.Colspan = 4; aafctivePsych.HorizontalAlignment = Element.ALIGN_CENTER; aafctivePsych.VerticalAlignment = Element.ALIGN_BOTTOM; resTable.AddCell(aafctivePsych);
        PdfPCell space = new PdfPCell(); space.Colspan = 2; space.Border = 0;
         PdfPCell space1 = new PdfPCell(); space1.Colspan = 1; space1.Border = 0;
        //PdfPCell space4 = new PdfPCell(); space4.Colspan = 4;



        //PdfPCell affectivePsychSkills = new PdfPCell(new Phrase("AFFECTIVE & PSYCHOMOTOR" + " ", blackFnt)); affectivePsychSkills.HorizontalAlignment = Element.ALIGN_CENTER; affectivePsychSkills.Colspan = 9; //row4cell2.Border = 0;
        //domainandSkills.AddCell(affectivePsychSkills);

        //resTable.AddCell(space);
        //resTable.AddCell(space);
        //resTable.AddCell(space1);
        //resTable.AddCell(space1);
        //resTable.AddCell(space1);
        //resTable.AddCell(space1);
        //resTable.AddCell(space1);
        //resTable.AddCell(space1);
        //resTable.AddCell(space1);
        //resTable.AddCell(space1);
        //resTable.AddCell(space1);
        //resTable.AddCell(space);

        PdfPCell thirdRow2Cell0A = new PdfPCell(new Phrase("", blackFnt8)); thirdRow2Cell0A.Colspan = 2; thirdRow2Cell0A.HorizontalAlignment = Element.ALIGN_CENTER; /*thirdRow2Cell0A.Border = 0;*/
         PdfPCell thirdRow2Cell1A = new PdfPCell(new Phrase("", blackFnt8)); thirdRow2Cell1A.Colspan = 2; thirdRow2Cell1A.HorizontalAlignment = Element.ALIGN_CENTER; /*thirdRow2Cell1A.Border = 0;*/
        PdfPCell thirdRow2Cell2A = new PdfPCell(new Phrase("20", blackFnt8)); thirdRow2Cell2A.Colspan = 1; thirdRow2Cell2A.HorizontalAlignment = Element.ALIGN_CENTER; /*thirdRow2Cell2A.Border = 0;*/
        PdfPCell thirdRow2Cell3A = new PdfPCell(new Phrase("20", blackFnt8)); thirdRow2Cell3A.Colspan = 1; thirdRow2Cell3A.HorizontalAlignment = Element.ALIGN_CENTER; /*thirdRow2Cell3A.Border = 0;*/
        PdfPCell thirdRow2Cell4A = new PdfPCell(new Phrase("10", blackFnt8)); thirdRow2Cell4A.Colspan = 1; thirdRow2Cell4A.HorizontalAlignment = Element.ALIGN_CENTER; /*thirdRow2Cell4A.Border = 0;*/
        PdfPCell thirdRow2Cell5A = new PdfPCell(new Phrase("50", blackFnt8)); thirdRow2Cell5A.Colspan = 1; thirdRow2Cell5A.HorizontalAlignment = Element.ALIGN_CENTER; /*thirdRow2Cell5A.Border = 0;*/
        PdfPCell thirdRow2Cell6A = new PdfPCell(new Phrase("100", blackFnt8)); thirdRow2Cell6A.Colspan = 1; thirdRow2Cell6A.HorizontalAlignment = Element.ALIGN_CENTER; /*thirdRow2Cell6A.Border = 0;*/
        PdfPCell thirdRow2Cell7A = new PdfPCell(new Phrase("40", blackFnt8)); thirdRow2Cell7A.Colspan = 1; thirdRow2Cell7A.HorizontalAlignment = Element.ALIGN_CENTER; /*thirdRow2Cell7A.Border = 0;*/
        PdfPCell thirdRow2Cell8A = new PdfPCell(new Phrase("60", blackFnt8)); thirdRow2Cell8A.Colspan = 1; thirdRow2Cell8A.HorizontalAlignment = Element.ALIGN_CENTER; /*thirdRow2Cell8A.Border = 0;*/
        PdfPCell thirdRow2Cell9A = new PdfPCell(new Phrase("100", blackFnt8)); thirdRow2Cell9A.Colspan = 1; thirdRow2Cell9A.HorizontalAlignment = Element.ALIGN_CENTER; /*thirdRow2Cell9A.Border = 0;*/
        PdfPCell thirdRow2Cell10A = new PdfPCell(new Phrase("", blackFnt8)); thirdRow2Cell10A.Colspan = 1; thirdRow2Cell9A.HorizontalAlignment = Element.ALIGN_CENTER; /*thirdRow2Cell10A.Border = 0;*/
        PdfPCell thirdRow2Cell11A = new PdfPCell(new Phrase("", blackFnt8)); thirdRow2Cell11A.Colspan = 2; thirdRow2Cell9A.HorizontalAlignment = Element.ALIGN_CENTER; /*thirdRow2Cell11A.Border = 0;*/

        resTable.AddCell(thirdRow2Cell0A);
        resTable.AddCell(thirdRow2Cell1A);
        resTable.AddCell(thirdRow2Cell2A);
        resTable.AddCell(thirdRow2Cell3A);
        resTable.AddCell(thirdRow2Cell4A);
        resTable.AddCell(thirdRow2Cell5A);
        resTable.AddCell(thirdRow2Cell6A);
        resTable.AddCell(thirdRow2Cell7A);
        resTable.AddCell(thirdRow2Cell8A);
        resTable.AddCell(thirdRow2Cell9A);
        resTable.AddCell(thirdRow2Cell10A);
        resTable.AddCell(thirdRow2Cell11A);





        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);


        PASSIS.LIB.GradeStudent gs = new ClassGradeLIB().RetrieveStudentGrade(student.Id, session);

        //Int64 subjectCounter = 0;
        int totalOffered = 0;
        int? curricullumId = 0;
        int deptCount = 0;
        int deptCount2 = 0;
        decimal totalMarkObtained = 0;
        int totalMarkObtainable = 0;
        decimal totalTestObtained = 0;
        decimal totalExamObtained = 0;
        decimal testScoreConfiguration = 0;
        decimal? ftt = 0;
        decimal? stt = 0;
        decimal? fte = 0;
        decimal? ste = 0;
        decimal? ftte = 0;
        decimal? stte = 0;
        decimal? totalBroughtForward = 0;
        decimal testScoreFirstTerm = 0;
        decimal examScoreFirstTerm = 0;
        decimal testScoreSecondTerm = 0;
        decimal examScoreSecondTerm = 0;
        decimal examScore = 0;
        decimal testScore = 0;
        decimal totalScore = 0;
        decimal? totalScoreAverage = 0;
        decimal? totalScoreAverages = 0;
        decimal scorePosition = 0;
        decimal testTotal = 0;
        decimal examTotal = 0;
        int deptCounter = 0;
        int countCA = 0;
        int countExam = 0;
        string subjName = "";
        string deptName = "";
        decimal? percentage = 0m;
        long subjectId = 0;
        decimal thirdTermScore = 0;
        decimal firstTermCAScore = 0;
        decimal firstTermExamScore = 0;
        decimal aggregateTotalScore = 0;
        decimal secondTermCAScore = 0;
        decimal secondTermExamScore = 0;

        decimal totalFirstTermScore = 0;
        decimal totalSecondTermScore = 0;
        decimal overallTotal = 0;
        decimal cw = 0;
        decimal hw = 0;
        decimal note = 0;
        decimal ca = 0;
        Dictionary<long, string> dictionary = new Dictionary<long, string>();
        PASSIS.LIB.Class_Grade classGrade = context.Class_Grades.FirstOrDefault(x => x.Id == Convert.ToInt64(ddlYear.SelectedValue));
        if (classGrade != null)
        {
            curricullumId = classGrade.CurriculumId;
        }
        IList<PASSIS.LIB.Subject> AllSubject = new List<PASSIS.LIB.Subject>();

        if (gs != null)
        {
            //IList<PASSIS.LIB.Subject> getSub = getSubject(session, term, schoolId, yearId, gradeIdd);
            //foreach (PASSIS.LIB.Subject sub in getSub) 
            //{
            //    subjectCounter++;
            //}
            var deptList = from s in context.SubjectDepartments where s.SchoolId == logonUser.SchoolId select s;
            foreach (SubjectDepartment dept in deptList)
            {
                //deptCounter++;
                IList<SubjectsInSchool> getSubjectInClass = (from s in context.SubjectsInSchools where s.DepartmentId == dept.Id && s.SchoolId == dept.SchoolId select s).ToList();
                foreach (SubjectsInSchool subjects in getSubjectInClass)
                {

                    PASSIS.LIB.Subject subject = context.Subjects.FirstOrDefault(x =>
                                      x.CurriculumId == (int)curricullumId &&
                                      x.ClassId == yearId && x.Id == subjects.SubjectId);
                    if (subject != null)
                    {
                        deptCount++;
                        AllSubject.Add(subject);

                        

                          
                        
                    }

                }

                totalFirstTermScore = testScoreFirstTerm + examScoreFirstTerm; // total first term score for each subject CA and EXAM
                totalSecondTermScore = testScoreSecondTerm + examScoreSecondTerm; // total second term score for each subject CA and EXAM

                foreach (SubjectsInSchool subjects in getSubjectInClass)
                {

                    PASSIS.LIB.Subject subject = context.Subjects.FirstOrDefault(x =>
                                      x.CurriculumId == (int)curricullumId &&
                                      x.ClassId == yearId && x.Id == subjects.SubjectId);
                    if (subject != null)
                    {
                        deptCount2++;
                        AllSubject.Add(subject);

                        //subjectCounter++;

                        //var studOffered = (from x in context.StudentScores where x.SchoolId == schoolId && x.SubjectId == subject.Id && x.ClassId == yearId && x.TermId == 2 && x.AcademicSessionID == session select x);
                        //totalOffered = studOffered.Count();

                        if (dictionary.ContainsKey(dept.Id))
                        {
                            deptName = "";
                            PdfPCell deptCell = new PdfPCell(new Phrase(deptName, darkerGrnFnt6)); deptCell.Colspan = 2; deptCell.BorderWidthTop = 0; deptCell.BorderColorTop = new BaseColor(255, 255, 255);
                            resTable.AddCell(deptCell);
                        }

                        else
                        {
                            deptName = dept.DepartmentName;
                            dictionary.Add(dept.Id, dept.DepartmentName);
                            PdfPCell deptCell = new PdfPCell(new Phrase(deptName, darkerGrnFnt6)); deptCell.Colspan = 2;
                            resTable.AddCell(deptCell);
                        }

                        //PdfPCell deptCell = new PdfPCell(new Phrase(dept.DepartmentName, darkerGrnFnt8)); deptCell.Colspan = 2;
                        //resTable.AddCell(deptCell);
                        PdfPCell subjCell = new PdfPCell(new Phrase(subject.Name, darkerGrnFnt6)); subjCell.Colspan = 2;
                        resTable.AddCell(subjCell);
                        //IList<PASSIS.LIB.StudentScoreRepository> scoreRepoFirstTerm = getSubjectScoreCategoryCA(student.AdmissionNumber, student.Id, session, term, schoolId, yearId, gradeId, subject.Id);
                        //if (scoreRepoFirstTerm.Count > 0)
                        //{
                        //    foreach (PASSIS.LIB.StudentScoreRepository fs in scoreRepoFirstTerm)
                        //    {
                        //        //if (fs != null)
                        //        //{

                        //        testScore = Convert.ToDecimal(fs.CAPercentageScore);
                        //        // testTotal += testScore;
                        //        PdfPCell CACell = new PdfPCell(new Phrase(Math.Round(testScore, 2).ToString(), darkerGrnFnt6)); CACell.Colspan = 1;
                        //        resTable.AddCell(CACell);
                        //        break;
                        //    }   
                        //}
                        //else
                        //{
                        //    testScore = 0;
                        //    PdfPCell CACell = new PdfPCell(new Phrase("", darkerGrnFnt6)); CACell.Colspan = 1;
                        //    resTable.AddCell(CACell);
                        //}
                       


                        




//// --------------------------- CLASS WORK ----------------------------/////
                        ScoreSubCategoryConfiguration gtScoreCW = context.ScoreSubCategoryConfigurations.FirstOrDefault(s=>s.ClassId == yearId && s.SessionId == session  && s.TermId == term && s.SubCategory == "CW");
                        if (gtScoreCW != null)
                        {
                            IList<PASSIS.LIB.StudentScoreRepository> scoreRepoCW = getSubjectScorePerSubcategory(student.AdmissionNumber, student.Id, session, term, schoolId, yearId, gradeId, subject.Id, (long)gtScoreCW.CategoryId, gtScoreCW.Id);
                            //countCA += scoreRepoFirstTerm.Count();
                            if (scoreRepoCW.Count() > 0)
                            {
                                foreach (PASSIS.LIB.StudentScoreRepository srCW in scoreRepoCW)
                                {
                                    if (srCW != null)
                                    {
                                        cw = Convert.ToDecimal(srCW.MarkObtained);
                                        //firstTermCAScore += testScoreFirstTerm;
                                        PdfPCell cwCell = new PdfPCell(new Phrase(Math.Round(cw, 0).ToString(), darkerGrnFnt6)); cwCell.Colspan = 1;
                                        resTable.AddCell(cwCell);
                                        break;
                                    }
                                    else
                                    {
                                        cw = 0;
                                        //firstTermCAScore += 0;
                                        PdfPCell cwCell = new PdfPCell(new Phrase("", darkerGrnFnt6)); cwCell.Colspan = 1;
                                        resTable.AddCell(cwCell);
                                    }
                                }
                            }
                            else
                            {
                                cw = 0;
                                //firstTermCAScore += 0;
                                PdfPCell cwCell = new PdfPCell(new Phrase("", darkerGrnFnt6)); cwCell.Colspan = 1;
                                resTable.AddCell(cwCell);
                            }
                        }
                        else
                        {
                            //firstTermCAScore += 0;
                            PdfPCell cwCell = new PdfPCell(new Phrase("", darkerGrnFnt6)); cwCell.Colspan = 1;
                            resTable.AddCell(cwCell);
                        }

 //// --------------------------- HOME WORK ----------------------------/////
                        ScoreSubCategoryConfiguration gtScoreHW = context.ScoreSubCategoryConfigurations.FirstOrDefault(s => s.ClassId == yearId && s.SessionId == session && s.TermId == term && s.SubCategory == "HW/PR");
                        if (gtScoreHW != null)
                        {
                            IList<PASSIS.LIB.StudentScoreRepository> scoreRepoHW = getSubjectScorePerSubcategory(student.AdmissionNumber, student.Id, session, term, schoolId, yearId, gradeId, subject.Id, (long)gtScoreHW.CategoryId, gtScoreHW.Id);
                            //countCA += scoreRepoFirstTerm.Count();
                            if (scoreRepoHW.Count() > 0)
                            {
                                foreach (PASSIS.LIB.StudentScoreRepository srHW in scoreRepoHW)
                                {
                                    if (srHW != null)
                                    {
                                        hw = Convert.ToDecimal(srHW.MarkObtained);
                                        //firstTermCAScore += testScoreFirstTerm;
                                        PdfPCell hwCell = new PdfPCell(new Phrase(Math.Round(hw, 0).ToString(), darkerGrnFnt6)); hwCell.Colspan = 1;
                                        resTable.AddCell(hwCell);
                                        break;
                                    }
                                    else
                                    {
                                        hw = 0;
                                        //firstTermCAScore += 0;
                                        PdfPCell hwCell = new PdfPCell(new Phrase("", darkerGrnFnt6)); hwCell.Colspan = 1;
                                        resTable.AddCell(hwCell);
                                    }
                                }
                            }
                            else
                            {
                                hw = 0;
                                //firstTermCAScore += 0;
                                PdfPCell hwCell = new PdfPCell(new Phrase("", darkerGrnFnt6)); hwCell.Colspan = 1;
                                resTable.AddCell(hwCell);
                            }
                        }
                        else
                        {
                            //firstTermCAScore += 0;
                            PdfPCell hwCell = new PdfPCell(new Phrase("", darkerGrnFnt6)); hwCell.Colspan = 1;
                            resTable.AddCell(hwCell);
                        }


//// --------------------------- ATT/NOTE ----------------------------/////
                        ScoreSubCategoryConfiguration gtAttNote = context.ScoreSubCategoryConfigurations.FirstOrDefault(s => s.ClassId == yearId && s.SessionId == session && s.TermId == term && s.SubCategory == "ATT/NOTE");
                        if (gtAttNote != null)
                        {
                            IList<PASSIS.LIB.StudentScoreRepository> scoreRepoAttNote = getSubjectScorePerSubcategory(student.AdmissionNumber, student.Id, session, term, schoolId, yearId, gradeId, subject.Id, (long)gtAttNote.CategoryId, gtAttNote.Id);
                            //countCA += scoreRepoFirstTerm.Count();
                            if (scoreRepoAttNote.Count() > 0)
                            {
                                foreach (PASSIS.LIB.StudentScoreRepository saAtt in scoreRepoAttNote)
                                {
                                    if (saAtt != null)
                                    {
                                        note = Convert.ToDecimal(saAtt.MarkObtained);
                                        //firstTermCAScore += testScoreFirstTerm;
                                        PdfPCell noteCell = new PdfPCell(new Phrase(Math.Round(note, 0).ToString(), darkerGrnFnt6)); noteCell.Colspan = 1;
                                        resTable.AddCell(noteCell);
                                        break;
                                    }
                                    else
                                    {
                                        note = 0;
                                        //firstTermCAScore += 0;
                                        PdfPCell noteCell = new PdfPCell(new Phrase("", darkerGrnFnt6)); noteCell.Colspan = 1;
                                        resTable.AddCell(noteCell);
                                    }
                                }
                            }
                            else
                            {
                                note = 0;
                                //firstTermCAScore += 0;
                                PdfPCell noteCell = new PdfPCell(new Phrase("", darkerGrnFnt6)); noteCell.Colspan = 1;
                                resTable.AddCell(noteCell);
                            }
                        }
                        else
                        {
                            //firstTermCAScore += 0;
                            PdfPCell noteCell = new PdfPCell(new Phrase("", darkerGrnFnt6)); noteCell.Colspan = 1;
                            resTable.AddCell(noteCell);
                        }



//// --------------------------- TEST ----------------------------------------------------------------/////
                        ScoreSubCategoryConfiguration gtTest = context.ScoreSubCategoryConfigurations.FirstOrDefault(s => s.ClassId == yearId && s.SessionId == session && s.TermId == term && s.SubCategory == "TEST");
                        if (gtTest != null)
                        {
                            IList<PASSIS.LIB.StudentScoreRepository> scoreRepoTest = getSubjectScorePerSubcategory(student.AdmissionNumber, student.Id, session, term, schoolId, yearId, gradeId, subject.Id, (long)gtTest.CategoryId, gtTest.Id);
                            //countCA += scoreRepoFirstTerm.Count();
                            if (scoreRepoTest.Count() > 0)
                            {
                                foreach (PASSIS.LIB.StudentScoreRepository saTest in scoreRepoTest)
                                {
                                    if (saTest != null)
                                    {
                                        ca = Convert.ToDecimal(saTest.MarkObtained);
                                        //firstTermCAScore += testScoreFirstTerm;

                                        PdfPCell testCell = new PdfPCell(new Phrase(Math.Round(ca, 0).ToString(), darkerGrnFnt6)); testCell.Colspan = 1;
                                        resTable.AddCell(testCell);
                                        break;
                                    }
                                    else
                                    {
                                        ca = 0;
                                        //firstTermCAScore += 0;
                                        PdfPCell testCell = new PdfPCell(new Phrase("", darkerGrnFnt6)); testCell.Colspan = 1;
                                        resTable.AddCell(testCell);
                                    }
                                }
                            }
                            else
                            {
                                ca = 0;
                                //firstTermCAScore += 0;
                                PdfPCell testCell = new PdfPCell(new Phrase("", darkerGrnFnt6)); testCell.Colspan = 1;
                                resTable.AddCell(testCell);
                            }
                        }
                        else
                        {
                            //firstTermCAScore += 0;
                            PdfPCell testCell = new PdfPCell(new Phrase("", darkerGrnFnt6)); testCell.Colspan = 1;
                            resTable.AddCell(testCell);
                        }


                       
                         totalScore = cw + hw + note + ca; //addup scores for continuous assessment
                        if (totalScore > 0)
                        { 
                            totalTestObtained = (totalScore / 100) * 40; //total score for CA
                        }
                        else
                        {
                            totalTestObtained = 0;
                        }
                           
                       

                        if (totalScore > 0)
                        {
                            PdfPCell totalCell = new PdfPCell(new Phrase(Math.Round(totalScore, 0).ToString(), darkerGrnFnt6)); totalCell.Colspan = 1;
                            resTable.AddCell(totalCell);
                        }
                        else
                        {
                            PdfPCell totalCell = new PdfPCell(new Phrase("", darkerGrnFnt6)); totalCell.Colspan = 1;
                            resTable.AddCell(totalCell);
                        }
                        if (totalTestObtained > 0)
                        {
                            PdfPCell CACell2 = new PdfPCell(new Phrase(Math.Round(totalTestObtained, 1).ToString(), darkerGrnFnt6)); CACell2.Colspan = 1;
                            resTable.AddCell(CACell2);
                        }
                        else
                        {
                            PdfPCell CACell2 = new PdfPCell(new Phrase("", darkerGrnFnt6)); CACell2.Colspan = 1;
                            resTable.AddCell(CACell2);
                        }
                          

                        IList<PASSIS.LIB.StudentScore> scoreRepoExamFirstTerm = getSubjectScoreCategoryExam(student.AdmissionNumber, student.Id, session, term, schoolId, yearId, gradeId, subject.Id);
                        if (scoreRepoExamFirstTerm.Count > 0)
                        {
                            foreach (PASSIS.LIB.StudentScore fs in scoreRepoExamFirstTerm)
                            {
                                if (fs != null)
                                {
                                    examScore = Convert.ToDecimal(fs.ExamPercentageScore);
                                    if (deptCount2 > 0)
                                    {
                                        PdfPCell ExamCell = new PdfPCell(new Phrase(Math.Round(examScore, 0).ToString(), darkerGrnFnt6)); ExamCell.Colspan = 1; ExamCell.BorderWidthTop = 0; ExamCell.BorderColorTop = new BaseColor(255, 255, 255);
                                        resTable.AddCell(ExamCell);
                                        break;
                                    }
                                    else
                                    {
                                        PdfPCell ExamCell = new PdfPCell(new Phrase("", darkerGrnFnt6)); ExamCell.Colspan = 1; ExamCell.BorderWidthTop = 0; ExamCell.BorderColorTop = new BaseColor(255, 255, 255);
                                        resTable.AddCell(ExamCell);
                                    }
                                }
                                else
                                {
                                    PdfPCell ExamCell = new PdfPCell(new Phrase("", darkerGrnFnt6)); ExamCell.Colspan = 1; ExamCell.BorderWidthTop = 0; ExamCell.BorderColorTop = new BaseColor(255, 255, 255);
                                    resTable.AddCell(ExamCell);
                                }
                            }
                        }
                        else
                        {
                            examScore = 0;
                            PdfPCell ExamCell = new PdfPCell(new Phrase("", darkerGrnFnt6)); ExamCell.Colspan = 1;
                            resTable.AddCell(ExamCell);
                        }



                        overallTotal = totalTestObtained + examScore; // total of cw,hw,att/note,test plus exam scores for each subject
                        totalMarkObtained += overallTotal; // total mark obtained for all subjects offered by student

                        if (overallTotal > 0)
                        {
                            subjectCounter++;
                        }

                        string grade = PASSIS.LIB.Utility.Utili.getExamGradeLetters(Math.Round(overallTotal), (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue));
                        if (grade.Trim() == "A+" || grade.Trim() == "A" || grade.Trim() == "A-" || grade.Trim() == "B+") { distinctionCounter++; }
                        else if (grade.Trim() == "B" || grade.Trim() == "C+" || grade.Trim() == "C") { creditCounter++; };

                        if (overallTotal > 0)
                        {
                            PdfPCell totalCell2 = new PdfPCell(new Phrase(Math.Round(overallTotal, 1).ToString(), darkerGrnFnt6)); totalCell2.Colspan = 1;
                            resTable.AddCell(totalCell2);
                        }
                        else
                        {
                            PdfPCell totalCell2 = new PdfPCell(new Phrase("", darkerGrnFnt6)); totalCell2.Colspan = 1;
                            resTable.AddCell(totalCell2);
                        }
                        PdfPCell gradeCell = new PdfPCell(new Phrase((PASSIS.LIB.Utility.Utili.getExamGradeLetters(Math.Round(overallTotal, 0), (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue))), darkerGrnFnt6)); gradeCell.Colspan = 1;
                        resTable.AddCell(gradeCell);
                        PdfPCell tRemark = new PdfPCell(new Phrase(PASSIS.LIB.Utility.Utili.getExamGradeRemarks(Math.Round(overallTotal, 0), (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)), darkerGrnFnt6)); tRemark.Colspan = 2;
                        resTable.AddCell(tRemark);
                        //PdfPCell affctPsych = new PdfPCell(new Phrase("", darkerGrnFnt6)); affctPsych.Colspan = 4;
                        //resTable.AddCell(affctPsych);

                        //++noOfSubjectWithScore;
                        //totalMarkObtainable = 0;
                        //totalMarkObtained = 0;
                        //testScore = 0;
                    }
                }

            }





            //document.Add(new Phrase(Environment.NewLine));
            if (subjectCounter != 0)
            {
                percentage = (totalMarkObtained / (subjectCounter * 100)) * 100;
            }

           



            //string classTeacherComment = txtClassTeacherComment.Text.Trim();
            //string proprietorComment = txtProprietorComment.Text.Trim();
            PdfPTable summaryTable = new PdfPTable(6);
            PdfPTable totalScoreObtainedTbl = new PdfPTable(15);
            PdfPTable totalScoreObtainableTbl = new PdfPTable(15);
            PdfPTable noOfSubjectsTbl = new PdfPTable(15);
            PdfPTable noOfDistinctionsTbl = new PdfPTable(15);
            PdfPTable noOfCreditsTbl = new PdfPTable(15);
            PdfPTable noOfPassesTbl = new PdfPTable(15);
            PdfPTable noOfFailureTbl = new PdfPTable(15);
            //iTextSharp.text.pdf.draw.LineSeparator line = new iTextSharp.text.pdf.draw.LineSeparator(0f, 100f, BaseColor.BLACK, Element.ALIGN_LEFT, 1);
            //PdfPCell Summarycell5 = new PdfPCell(new Phrase("PERCENTAGE PERFORMANCE:", blackFnt)); Summarycell5.Colspan = 4; Summarycell5.HorizontalAlignment = Element.ALIGN_LEFT;
            //PdfPCell Summarycell6 = new PdfPCell(new Phrase(percentage.ToString(), blackFnt)); Summarycell6.Colspan = 2; Summarycell6.HorizontalAlignment = Element.ALIGN_LEFT;
            //PdfPCell Summarycell1 = new PdfPCell(new Phrase("TOTAL SCORE OBTAINED:", blackFnt)); Summarycell1.Colspan = 4; Summarycell1.HorizontalAlignment = Element.ALIGN_LEFT;
            //PdfPCell Summarycell2 = new PdfPCell(new Phrase(Math.Round(totalMarkObtained, 0).ToString(), blackFnt)); Summarycell2.Colspan = 2; Summarycell2.HorizontalAlignment = Element.ALIGN_LEFT;
            //PdfPCell Summarycell3 = new PdfPCell(new Phrase("TOTAL SCORE OBTAINABLE:", blackFnt)); Summarycell3.Colspan = 4; Summarycell3.HorizontalAlignment = Element.ALIGN_LEFT;
            //PdfPCell Summarycell4 = new PdfPCell(new Phrase((subjectCounter * 100).ToString(), blackFnt)); Summarycell4.Colspan = 2; Summarycell4.HorizontalAlignment = Element.ALIGN_LEFT;



            PdfPTable behaviorTable = new PdfPTable(6);
            PdfPCell behaviorCell0 = new PdfPCell(new Phrase("SCALE", blackFnt8)); behaviorCell0.Colspan = 6; behaviorCell0.HorizontalAlignment = Element.ALIGN_CENTER;
            behaviorTable.AddCell(behaviorCell0);
            PdfPCell behaviorCell1 = new PdfPCell(new Phrase("5", blackFnt8)); behaviorCell1.Colspan = 2; behaviorCell1.Border = 0;
            behaviorTable.AddCell(behaviorCell1);
            PdfPCell behaviorCell2 = new PdfPCell(new Phrase("EXCELLENT", blackFnt8)); behaviorCell2.Colspan = 4; behaviorCell2.Border = 0;
            behaviorTable.AddCell(behaviorCell2);
            PdfPCell behaviorCell3 = new PdfPCell(new Phrase("4", blackFnt8)); behaviorCell3.Colspan = 2; behaviorCell3.Border = 0;
            behaviorTable.AddCell(behaviorCell3);
            PdfPCell behaviorCell4 = new PdfPCell(new Phrase("V.GOOD", blackFnt8)); behaviorCell4.Colspan = 4; behaviorCell4.Border = 0;
            behaviorTable.AddCell(behaviorCell4);
            PdfPCell behaviorCell5 = new PdfPCell(new Phrase("3", blackFnt8)); behaviorCell5.Colspan = 2; behaviorCell5.Border = 0;
            behaviorTable.AddCell(behaviorCell5);
            PdfPCell behaviorCell6 = new PdfPCell(new Phrase("GOOD", blackFnt8)); behaviorCell6.Colspan = 4; behaviorCell6.Border = 0;
            behaviorTable.AddCell(behaviorCell6);
            PdfPCell behaviorCell7 = new PdfPCell(new Phrase("2", blackFnt8)); behaviorCell7.Colspan = 2; behaviorCell7.Border = 0;
            behaviorTable.AddCell(behaviorCell7);
            PdfPCell behaviorCell8 = new PdfPCell(new Phrase("AVERAGE", blackFnt8)); behaviorCell8.Colspan = 4; behaviorCell8.Border = 0;
            behaviorTable.AddCell(behaviorCell8);
            PdfPCell behaviorCell9 = new PdfPCell(new Phrase("1", blackFnt8)); behaviorCell9.Colspan = 2; behaviorCell9.Border = 0;
            behaviorTable.AddCell(behaviorCell9);
            PdfPCell behaviorCell10 = new PdfPCell(new Phrase("BELOW AVERAGE", blackFnt8)); behaviorCell10.Colspan = 4; behaviorCell10.Border = 0;
            behaviorTable.AddCell(behaviorCell10);
            PdfPCell behaviorCell11 = new PdfPCell(new Phrase("", blackFnt8)); behaviorCell11.Colspan = 4; behaviorCell11.Border = 0;
            behaviorTable.AddCell(behaviorCell11);
            PdfPCell behaviorCell12 = new PdfPCell(new Phrase("RATINGS", blackFnt8)); behaviorCell12.Colspan = 2; behaviorCell12.HorizontalAlignment = Element.ALIGN_CENTER; behaviorCell12.Border = 0;
            behaviorTable.AddCell(behaviorCell12);
            //PdfPCell behaviorCell13 = new PdfPCell(new Phrase("Punctuality", blackFnt8)); behaviorCell13.Colspan = 4;
            //behaviorTable.AddCell(behaviorCell13);
            //PdfPCell behaviorCell14 = new PdfPCell(new Phrase("5", blackFnt8)); behaviorCell14.Colspan = 2;
            //behaviorTable.AddCell(behaviorCell14);
            //PdfPCell behaviorCell15 = new PdfPCell(new Phrase("", blackFnt8)); behaviorCell15.Colspan = 4;
            //behaviorTable.AddCell(behaviorCell15);
            //PdfPCell behaviorCell16 = new PdfPCell(new Phrase("", blackFnt8)); behaviorCell16.Colspan = 2;
            //behaviorTable.AddCell(behaviorCell16);


            PdfPTable rightTable = new PdfPTable(5);
            PdfPCell rightRow1 = new PdfPCell(resTable); rightRow1.Colspan = 4;
            PdfPCell rightRow3 = new PdfPCell(behaviorTable); rightRow3.Colspan = 1;
            rightTable.AddCell(rightRow1);
            rightTable.AddCell(rightRow3);



            PdfPTable lastTable = new PdfPTable(5);

            PdfPCell EmptyCell = new PdfPCell(new Phrase(" ", blackFnt8)); EmptyCell.Colspan = 6; EmptyCell.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell1Row1.Border = 0;
            PdfPCell lastCell1Row22 = new PdfPCell(new Phrase("PERCENTAGE PERFORMANCE", blackFnt8)); lastCell1Row22.Colspan = 4; lastCell1Row22.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell1Row4.Border = 0;
            PdfPCell lastCell2Row23 = new PdfPCell(new Phrase(Math.Round(Convert.ToDecimal(percentage)).ToString() + "%", blackFnt8)); lastCell2Row23.Colspan = 1; lastCell2Row23.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell2Row4.Border = 0;
            PdfPCell lastCell1Row2 = new PdfPCell(new Phrase("TOTAL SCORE OBATAINED", blackFnt8)); lastCell1Row2.Colspan = 4; lastCell1Row2.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell1Row4.Border = 0;
            PdfPCell lastCell2Row2 = new PdfPCell(new Phrase(Math.Round(totalMarkObtained, 1).ToString(), blackFnt8)); lastCell2Row2.Colspan = 1; lastCell2Row2.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell2Row4.Border = 0;
            PdfPCell lastCell1Row3 = new PdfPCell(new Phrase("TOTAL SCORE OBTAINABLE", blackFnt8)); lastCell1Row3.Colspan = 4; lastCell1Row3.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell1Row4.Border = 0;
            PdfPCell lastCell2Row4 = new PdfPCell(new Phrase((subjectCounter * 100).ToString(), blackFnt8)); lastCell2Row4.Colspan = 1; lastCell2Row4.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell2Row4.Border = 0;
            PdfPCell lastCell1Row5 = new PdfPCell(new Phrase("NUMBER OF SUBJECTS OFFERED", blackFnt8)); lastCell1Row5.Colspan = 4; lastCell1Row5.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell1Row4.Border = 0;
            PdfPCell lastCell2Row6 = new PdfPCell(new Phrase(subjectCounter.ToString(), blackFnt8)); lastCell2Row6.Colspan = 1; lastCell2Row6.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell2Row4.Border = 0;
            PdfPCell lastCell1Row7 = new PdfPCell(new Phrase("NUMBER OF DISTINCTIONS", blackFnt8)); lastCell1Row7.Colspan = 4; lastCell1Row7.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell1Row4.Border = 0;
            PdfPCell lastCell2Row8 = new PdfPCell(new Phrase(distinctionCounter.ToString(), blackFnt8)); lastCell2Row8.Colspan = 1; lastCell2Row8.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell2Row4.Border = 0;
            PdfPCell lastCell1Row9 = new PdfPCell(new Phrase("NUMBER OF CREDITS", blackFnt8)); lastCell1Row9.Colspan = 4; lastCell1Row9.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell1Row4.Border = 0;
            PdfPCell lastCell2Row10 = new PdfPCell(new Phrase(creditCounter.ToString(), blackFnt8)); lastCell2Row10.Colspan = 1; lastCell2Row10.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell2Row4.Border = 0;
            PdfPCell lastCell1Row11 = new PdfPCell(new Phrase("NUMBER OF PASSES", blackFnt8)); lastCell1Row11.Colspan = 4; lastCell1Row11.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell1Row4.Border = 0;
            PdfPCell lastCell2Row12 = new PdfPCell(new Phrase(passesCounter.ToString(), blackFnt8)); lastCell2Row12.Colspan = 1; lastCell2Row12.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell2Row4.Border = 0;
            PdfPCell lastCell1Row13 = new PdfPCell(new Phrase("NUMBER OF FAILURE", blackFnt8)); lastCell1Row13.Colspan = 4; lastCell1Row13.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell1Row4.Border = 0;
            PdfPCell lastCell2Row14 = new PdfPCell(new Phrase(failureCounter.ToString(), blackFnt8)); lastCell2Row14.Colspan = 1; lastCell2Row14.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell2Row4.Border = 0;


            lastTable.AddCell(lastCell1Row22);
            lastTable.AddCell(lastCell2Row23);
            lastTable.AddCell(lastCell1Row2);
            lastTable.AddCell(lastCell2Row2);
            lastTable.AddCell(lastCell1Row3);
            lastTable.AddCell(lastCell2Row4);
            lastTable.AddCell(lastCell1Row5);
            lastTable.AddCell(lastCell2Row6);
            lastTable.AddCell(lastCell1Row7);
            lastTable.AddCell(lastCell2Row8);
            lastTable.AddCell(lastCell1Row9);
            lastTable.AddCell(lastCell2Row10);
            lastTable.AddCell(lastCell1Row11);
            lastTable.AddCell(lastCell2Row12);
            lastTable.AddCell(lastCell1Row13);  
            lastTable.AddCell(lastCell2Row14);
            //lastTable.AddCell(EmptyCell);
            //lastTable.AddCell(lastCell1Row3);
            //lastTable.AddCell(lastCell2Row3);
            //lastTable.AddCell(EmptyCell);
            //lastTable.AddCell(lastCell1Row4);
            //lastTable.AddCell(lastCell2Row4);







            PdfPTable gradingTable = new PdfPTable(4);

            PdfPCell gradingRow0Cell0 = new PdfPCell(new Phrase("GRADE KEY", blackFnt8)); gradingRow0Cell0.Colspan = 4; gradingRow0Cell0.HorizontalAlignment = Element.ALIGN_CENTER;
            gradingTable.AddCell(gradingRow0Cell0);
            PdfPCell gradingRow1Cell1 = new PdfPCell(new Phrase("70 - 100", blackFnt8)); gradingRow1Cell1.Colspan = 1; /*gradingRow1Cell1.Border = 0;*/
            gradingTable.AddCell(gradingRow1Cell1);
            PdfPCell gradingRow1Cell2 = new PdfPCell(new Phrase("EXCELLENT", blackFnt8)); gradingRow1Cell2.Colspan = 2; /*gradingRow1Cell2.Border = 0;*/
            gradingTable.AddCell(gradingRow1Cell2);
            PdfPCell gradingRow1Cell3 = new PdfPCell(new Phrase("A", blackFnt8)); gradingRow1Cell3.Colspan = 1;
            gradingTable.AddCell(gradingRow1Cell3);

            PdfPCell gradingRow2Cell1 = new PdfPCell(new Phrase("60 - 69", blackFnt8)); gradingRow2Cell1.Colspan = 1; /*gradingRow2Cell1.Border = 0;*/
            gradingTable.AddCell(gradingRow2Cell1);
            PdfPCell gradingRow2Cell2 = new PdfPCell(new Phrase("V.GOOD", blackFnt8)); gradingRow2Cell2.Colspan = 2; /*gradingRow2Cell2.Border = 0;*/
            gradingTable.AddCell(gradingRow2Cell2);
            PdfPCell gradingRow2Cell3 = new PdfPCell(new Phrase("B", blackFnt8)); gradingRow2Cell3.Colspan = 1;
            gradingTable.AddCell(gradingRow2Cell3);

            PdfPCell gradingRow3Cell1 = new PdfPCell(new Phrase("50 - 59", blackFnt8)); gradingRow3Cell1.Colspan = 1; /*gradingRow3Cell1.Border = 0;*/
            gradingTable.AddCell(gradingRow3Cell1);
            PdfPCell gradingRow3Cell2 = new PdfPCell(new Phrase("GOOD", blackFnt8)); gradingRow3Cell2.Colspan = 2; /*gradingRow3Cell2.Border = 0;*/
            gradingTable.AddCell(gradingRow3Cell2);
            PdfPCell gradingRow3Cell3 = new PdfPCell(new Phrase("C", blackFnt8)); gradingRow3Cell3.Colspan = 1;
            gradingTable.AddCell(gradingRow3Cell3);

            //PdfPCell gradingRow4Cell1 = new PdfPCell(new Phrase("70 - 74", blackFnt8)); gradingRow4Cell1.Colspan = 1; /*gradingRow4Cell1.Border = 0;*/
            //gradingTable.AddCell(gradingRow4Cell1);
            //PdfPCell gradingRow4Cell2 = new PdfPCell(new Phrase("B+ (Good)", blackFnt8)); gradingRow4Cell2.Colspan = 2; /*gradingRow4Cell2.Border = 0;*/
            //gradingTable.AddCell(gradingRow4Cell2);
            //PdfPCell gradingRow4Cell3 = new PdfPCell(new Phrase("GOOD", blackFnt8)); gradingRow4Cell3.Colspan = 1;
            //gradingTable.AddCell(gradingRow4Cell3);

            PdfPCell gradingRow5Cell1 = new PdfPCell(new Phrase("40 - 49", blackFnt8)); gradingRow5Cell1.Colspan = 1; /*gradingRow5Cell1.Border = 0;*/
            gradingTable.AddCell(gradingRow5Cell1);
            PdfPCell gradingRow5Cell2 = new PdfPCell(new Phrase("AVERAGE", blackFnt8)); gradingRow5Cell2.Colspan = 2; /*gradingRow5Cell2.Border = 0;*/
            gradingTable.AddCell(gradingRow5Cell2);
            PdfPCell gradingRow5Cell3 = new PdfPCell(new Phrase("D", blackFnt8)); gradingRow5Cell3.Colspan = 1;
            gradingTable.AddCell(gradingRow5Cell3);

            PdfPCell gradingRow6Cell1 = new PdfPCell(new Phrase("BELOW 40", blackFnt8)); gradingRow6Cell1.Colspan = 1; /*gradingRow6Cell1.Border = 0;*/
            gradingTable.AddCell(gradingRow6Cell1);
            PdfPCell gradingRow6Cell2 = new PdfPCell(new Phrase("WORK HARDER", blackFnt8)); gradingRow6Cell2.Colspan = 2; /*gradingRow6Cell2.Border = 0;*/
            gradingTable.AddCell(gradingRow6Cell2);
            PdfPCell gradingRow6Cell3 = new PdfPCell(new Phrase("E", blackFnt8)); gradingRow6Cell3.Colspan = 1;
            gradingTable.AddCell(gradingRow6Cell3);

            //PdfPCell gradingRow7Cell1 = new PdfPCell(new Phrase("55 - 59", blackFnt8)); gradingRow7Cell1.Colspan = 2; /*gradingRow7Cell1.Border = 0;*/
            //gradingTable.AddCell(gradingRow7Cell1);
            //PdfPCell gradingRow7Cell2 = new PdfPCell(new Phrase("C (Just Fair)", blackFnt8)); gradingRow7Cell2.Colspan = 2; /*gradingRow7Cell2.Border = 0;*/
            //gradingTable.AddCell(gradingRow7Cell2);
            ////PdfPCell gradingRow7Cell3 = new PdfPCell(new Phrase("POOR", blackFnt8)); gradingRow7Cell3.Colspan = 2;
            ////gradingTable.AddCell(gradingRow7Cell3);

            //PdfPCell gradingRow8Cell1 = new PdfPCell(new Phrase("50 - 54", blackFnt8)); gradingRow8Cell1.Colspan = 2; /*gradingRow8Cell1.Border = 0;*/
            //gradingTable.AddCell(gradingRow8Cell1);
            //PdfPCell gradingRow8Cell2 = new PdfPCell(new Phrase("C- (Average)", blackFnt8)); gradingRow8Cell2.Colspan = 2; /*gradingRow8Cell2.Border = 0;*/
            //gradingTable.AddCell(gradingRow8Cell2);

            //PdfPCell gradingRow9Cell1 = new PdfPCell(new Phrase("0 - 49", blackFnt8)); gradingRow9Cell1.Colspan = 2; /*gradingRow9Cell1.Border = 0;*/
            //gradingTable.AddCell(gradingRow9Cell1);
            //PdfPCell gradingRow9Cell2 = new PdfPCell(new Phrase("E (Work Hrader)", blackFnt8)); gradingRow9Cell2.Colspan = 2; /*gradingRow9Cell2.Border = 0;*/
            //gradingTable.AddCell(gradingRow9Cell2);

            //PdfPCell gradingRow10Cell1 = new PdfPCell(new Phrase("PERCENTAGE:", blackFnt8)); gradingRow10Cell1.Colspan = 4; /*gradingRow10Cell1.Border = 0;*/
            //gradingTable.AddCell(gradingRow10Cell1);
            //PdfPCell gradingRow11Cell2 = new PdfPCell(new Phrase("", blackFnt8)); gradingRow11Cell2.Colspan = 1; /*gradingRow11Cell2.Border = 0;*/
            //gradingTable.AddCell(gradingRow11Cell2);
            //PdfPCell gradingRow12Cell2 = new PdfPCell(new Phrase(Math.Round(Convert.ToDecimal(percentage)).ToString() + "%", blackFnt8)); gradingRow12Cell2.Colspan = 2;
            //gradingTable.AddCell(gradingRow12Cell2);
            //PdfPCell gradingRow13Cell2 = new PdfPCell(new Phrase("", blackFnt8)); gradingRow13Cell2.Colspan = 1; /*gradingRow13Cell2.Border = 0;*/
            //gradingTable.AddCell(gradingRow13Cell2);


            PdfPTable baseTable = new PdfPTable(5);
            PdfPCell base1 = new PdfPCell(lastTable); base1.Colspan = 2;
            PdfPCell base2 = new PdfPCell(); base2.Colspan = 2; base2.Border = 0;
            PdfPCell base3 = new PdfPCell(gradingTable); base3.Colspan = 1; base3.Border = 0;
            baseTable.AddCell(base1);
            baseTable.AddCell(base2);
            baseTable.AddCell(base3);






            //PdfPCell Summarycell1 = new PdfPCell(new Phrase("Total Score Obtained:", blackFnt)); Summarycell1.Colspan = 4; Summarycell1.HorizontalAlignment = Element.ALIGN_LEFT;
            //PdfPCell Summarycell2 = new PdfPCell(new Phrase(Math.Round(aggregateTotalScore, 0).ToString(), blackFnt)); Summarycell2.Colspan = 2; Summarycell2.HorizontalAlignment = Element.ALIGN_LEFT;
            //PdfPCell emptyCell3 = new PdfPCell(new Phrase("", blackFnt)); emptyCell3.Colspan = 1; emptyCell3.HorizontalAlignment = Element.ALIGN_LEFT; emptyCell3.Border = 0;
            //PdfPCell emptyCell4 = new PdfPCell(new Phrase("", blackFnt)); emptyCell4.Colspan = 1; emptyCell4.HorizontalAlignment = Element.ALIGN_LEFT; emptyCell4.Border = 0;
            //PdfPCell emptyCell5 = new PdfPCell(new Phrase("", blackFnt)); emptyCell5.Colspan = 1; emptyCell5.HorizontalAlignment = Element.ALIGN_LEFT; emptyCell5.Border = 0;
            //PdfPCell emptyCell6 = new PdfPCell(new Phrase("", blackFnt)); emptyCell6.Colspan = 1; emptyCell6.HorizontalAlignment = Element.ALIGN_LEFT; emptyCell6.Border = 0;
            //PdfPCell emptyCell7 = new PdfPCell(new Phrase("", blackFnt)); emptyCell7.Colspan = 1; emptyCell7.HorizontalAlignment = Element.ALIGN_LEFT; emptyCell7.Border = 0;
            //PdfPCell emptyCell8 = new PdfPCell(new Phrase("", blackFnt)); emptyCell8.Colspan = 1; emptyCell8.HorizontalAlignment = Element.ALIGN_LEFT; emptyCell8.Border = 0;
            //PdfPCell emptyCell9 = new PdfPCell(new Phrase("", blackFnt)); emptyCell9.Colspan = 1; emptyCell9.HorizontalAlignment = Element.ALIGN_LEFT; emptyCell9.Border = 0;
            //PdfPCell emptyCell10 = new PdfPCell(new Phrase("", blackFnt)); emptyCell10.Colspan = 2; emptyCell10.HorizontalAlignment = Element.ALIGN_LEFT; emptyCell10.Border = 0;
            //totalScoreObtainedTbl.AddCell(Summarycell1); Summarycell1.Colspan = 4;
            //totalScoreObtainedTbl.AddCell(Summarycell2); Summarycell2.Colspan = 2;
            //totalScoreObtainedTbl.AddCell(emptyCell3); emptyCell3.Colspan = 1;
            //totalScoreObtainedTbl.AddCell(emptyCell4); emptyCell4.Colspan = 1;
            //totalScoreObtainedTbl.AddCell(emptyCell5); emptyCell5.Colspan = 1;
            //totalScoreObtainedTbl.AddCell(emptyCell6); emptyCell6.Colspan = 1;
            //totalScoreObtainedTbl.AddCell(emptyCell7); emptyCell7.Colspan = 1;
            //totalScoreObtainedTbl.AddCell(emptyCell8); emptyCell8.Colspan = 1;
            //totalScoreObtainedTbl.AddCell(emptyCell9); emptyCell9.Colspan = 1;
            //totalScoreObtainedTbl.AddCell(emptyCell10); emptyCell10.Colspan = 2;

            //PdfPCell Summarycell3 = new PdfPCell(new Phrase("Total Score Obtainable:", blackFnt)); Summarycell3.Colspan = 4; Summarycell3.HorizontalAlignment = Element.ALIGN_LEFT;
            //PdfPCell Summarycell4 = new PdfPCell(new Phrase((subjectCounter * 100).ToString(), blackFnt)); Summarycell4.Colspan = 2; Summarycell4.HorizontalAlignment = Element.ALIGN_LEFT;
            //totalScoreObtainableTbl.AddCell(Summarycell3); Summarycell1.Colspan = 4;
            //totalScoreObtainableTbl.AddCell(Summarycell4); Summarycell2.Colspan = 2;
            //totalScoreObtainableTbl.AddCell(emptyCell3); emptyCell3.Colspan = 1;
            //totalScoreObtainableTbl.AddCell(emptyCell4); emptyCell4.Colspan = 1;
            //totalScoreObtainableTbl.AddCell(emptyCell5); emptyCell5.Colspan = 1;
            //totalScoreObtainableTbl.AddCell(emptyCell6); emptyCell6.Colspan = 1;
            //totalScoreObtainableTbl.AddCell(emptyCell7); emptyCell7.Colspan = 1;
            //totalScoreObtainableTbl.AddCell(emptyCell8); emptyCell8.Colspan = 1;
            //totalScoreObtainableTbl.AddCell(emptyCell9); emptyCell9.Colspan = 1;
            //totalScoreObtainableTbl.AddCell(emptyCell10); emptyCell10.Colspan = 2;

            //PdfPCell summaryRow5Cell1 = new PdfPCell(new Phrase("Number of Subject Offered:", blackFnt)); summaryRow5Cell1.Colspan = 4; summaryRow5Cell1.HorizontalAlignment = Element.ALIGN_LEFT;
            //PdfPCell summaryRow5Cell2 = new PdfPCell(new Phrase(subjectCounter.ToString(), blackFnt)); summaryRow5Cell2.Colspan = 2;
            //noOfSubjectsTbl.AddCell(summaryRow5Cell1); summaryRow5Cell1.Colspan = 4;
            //noOfSubjectsTbl.AddCell(summaryRow5Cell2); summaryRow5Cell2.Colspan = 2;
            //noOfSubjectsTbl.AddCell(emptyCell3); emptyCell3.Colspan = 1;
            //noOfSubjectsTbl.AddCell(emptyCell4); emptyCell4.Colspan = 1;
            //noOfSubjectsTbl.AddCell(emptyCell5); emptyCell5.Colspan = 1;
            //noOfSubjectsTbl.AddCell(emptyCell6); emptyCell6.Colspan = 1;
            //noOfSubjectsTbl.AddCell(emptyCell7); emptyCell7.Colspan = 1;
            //noOfSubjectsTbl.AddCell(emptyCell8); emptyCell8.Colspan = 1;
            //noOfSubjectsTbl.AddCell(emptyCell9); emptyCell9.Colspan = 1;
            //noOfSubjectsTbl.AddCell(emptyCell10); emptyCell10.Colspan = 2;

            //PdfPCell summaryRow5Cell3 = new PdfPCell(new Phrase("Number of Distinctions:", blackFnt)); summaryRow5Cell3.Colspan = 4; summaryRow5Cell3.HorizontalAlignment = Element.ALIGN_LEFT;
            //PdfPCell summaryRow5Cell4 = new PdfPCell(new Phrase(distinctionCounter.ToString(), blackFnt)); summaryRow5Cell4.Colspan = 2;
            //noOfDistinctionsTbl.AddCell(summaryRow5Cell3); summaryRow5Cell3.Colspan = 4;
            //noOfDistinctionsTbl.AddCell(summaryRow5Cell4); summaryRow5Cell4.Colspan = 2;
            //noOfDistinctionsTbl.AddCell(emptyCell3); emptyCell3.Colspan = 1;
            //noOfDistinctionsTbl.AddCell(emptyCell4); emptyCell4.Colspan = 1;
            //noOfDistinctionsTbl.AddCell(emptyCell5); emptyCell5.Colspan = 1;
            //noOfDistinctionsTbl.AddCell(emptyCell6); emptyCell6.Colspan = 1;
            //noOfDistinctionsTbl.AddCell(emptyCell7); emptyCell7.Colspan = 1;
            //noOfDistinctionsTbl.AddCell(emptyCell8); emptyCell8.Colspan = 1;
            //noOfDistinctionsTbl.AddCell(emptyCell9); emptyCell9.Colspan = 1;
            //noOfDistinctionsTbl.AddCell(emptyCell10); emptyCell10.Colspan = 2;

            //PdfPCell summaryRow5Cell5 = new PdfPCell(new Phrase("Number of Credits:", blackFnt)); summaryRow5Cell5.Colspan = 4; summaryRow5Cell5.HorizontalAlignment = Element.ALIGN_LEFT;
            //PdfPCell summaryRow5Cell6 = new PdfPCell(new Phrase(creditCounter.ToString(), blackFnt)); summaryRow5Cell6.Colspan = 2;
            //noOfCreditsTbl.AddCell(summaryRow5Cell5); summaryRow5Cell5.Colspan = 4;
            //noOfCreditsTbl.AddCell(summaryRow5Cell6); summaryRow5Cell6.Colspan = 2;
            //noOfCreditsTbl.AddCell(emptyCell3); emptyCell3.Colspan = 1;
            //noOfCreditsTbl.AddCell(emptyCell4); emptyCell4.Colspan = 1;
            //noOfCreditsTbl.AddCell(emptyCell5); emptyCell5.Colspan = 1;
            //noOfCreditsTbl.AddCell(emptyCell6); emptyCell6.Colspan = 1;
            //noOfCreditsTbl.AddCell(emptyCell7); emptyCell7.Colspan = 1;
            //noOfCreditsTbl.AddCell(emptyCell8); emptyCell8.Colspan = 1;
            //noOfCreditsTbl.AddCell(emptyCell9); emptyCell9.Colspan = 1;
            //noOfCreditsTbl.AddCell(emptyCell10); emptyCell10.Colspan = 2;

            //PdfPCell summaryRow5Cell7 = new PdfPCell(new Phrase("Number of Passes:", blackFnt)); summaryRow5Cell7.Colspan = 4; summaryRow5Cell7.HorizontalAlignment = Element.ALIGN_LEFT;
            //PdfPCell summaryRow5Cell8 = new PdfPCell(new Phrase(passesCounter.ToString(), blackFnt)); summaryRow5Cell8.Colspan = 2;
            //noOfPassesTbl.AddCell(summaryRow5Cell7); summaryRow5Cell7.Colspan = 4;
            //noOfPassesTbl.AddCell(summaryRow5Cell8); summaryRow5Cell8.Colspan = 2;
            //noOfPassesTbl.AddCell(emptyCell3); emptyCell3.Colspan = 1;
            //noOfPassesTbl.AddCell(emptyCell4); emptyCell4.Colspan = 1;
            //noOfPassesTbl.AddCell(emptyCell5); emptyCell5.Colspan = 1;
            //noOfPassesTbl.AddCell(emptyCell6); emptyCell6.Colspan = 1;
            //noOfPassesTbl.AddCell(emptyCell7); emptyCell7.Colspan = 1;
            //noOfPassesTbl.AddCell(emptyCell8); emptyCell8.Colspan = 1;
            //noOfPassesTbl.AddCell(emptyCell9); emptyCell9.Colspan = 1;
            //noOfPassesTbl.AddCell(emptyCell10); emptyCell10.Colspan = 2;


            //PdfPCell summaryRow5Cell9 = new PdfPCell(new Phrase("Number of Failure:", blackFnt)); summaryRow5Cell9.Colspan = 4; summaryRow5Cell9.HorizontalAlignment = Element.ALIGN_LEFT;
            //PdfPCell summaryRow5Cell10 = new PdfPCell(new Phrase(failureCounter.ToString(), blackFnt)); summaryRow5Cell10.Colspan = 2;
            //noOfFailureTbl.AddCell(summaryRow5Cell9); summaryRow5Cell9.Colspan = 4;
            //noOfFailureTbl.AddCell(summaryRow5Cell10); summaryRow5Cell10.Colspan = 2;
            //noOfFailureTbl.AddCell(emptyCell3); emptyCell3.Colspan = 1;
            //noOfFailureTbl.AddCell(emptyCell4); emptyCell4.Colspan = 1;
            //noOfFailureTbl.AddCell(emptyCell5); emptyCell5.Colspan = 1;
            //noOfFailureTbl.AddCell(emptyCell6); emptyCell6.Colspan = 1;
            //noOfFailureTbl.AddCell(emptyCell7); emptyCell7.Colspan = 1;
            //noOfFailureTbl.AddCell(emptyCell8); emptyCell8.Colspan = 1;
            //noOfFailureTbl.AddCell(emptyCell9); emptyCell9.Colspan = 1;
            //noOfFailureTbl.AddCell(emptyCell10); emptyCell10.Colspan = 2;



            string examinerComment = "";
            string classTeacherComment = "";
            string headTeacherComment = "";
            string parentComment = "";
            string classTeacherName = "";
            PASSIS.LIB.ReportCardComment objExaminer = context.ReportCardComments.FirstOrDefault(x => x.CommentConfigId == 1 && x.AcademicSessionID == session && x.TermId == term && x.StudentId == student.Id);
            if (objExaminer != null)
            {
                examinerComment = objExaminer.Comment;
            }
            PASSIS.LIB.ReportCardComment objClassTeacher = context.ReportCardComments.FirstOrDefault(x => x.CommentConfigId == 2 && x.AcademicSessionID == session && x.TermId == term && x.StudentId == student.Id);
            if (objClassTeacher != null)
            {
                classTeacherComment = objClassTeacher.Comment;
            }
            PASSIS.LIB.ReportCardComment objHeadTeacher = context.ReportCardComments.FirstOrDefault(x => x.CommentConfigId == 3 && x.AcademicSessionID == session && x.TermId == term && x.StudentId == student.Id);
            if (objHeadTeacher != null)
            {
                headTeacherComment = objHeadTeacher.Comment;
            }
            PASSIS.LIB.ReportCardComment objParent = context.ReportCardComments.FirstOrDefault(x => x.CommentConfigId == 4 && x.AcademicSessionID == session && x.TermId == term && x.StudentId == student.Id);
            if (objParent != null)
            {
                parentComment = objHeadTeacher.Comment;
            }


            PdfPTable commentsTable = new PdfPTable(15);
            PdfPCell summaryRow2Cell1 = new PdfPCell(new Phrase("CLASS TEACHER'S COMMENT:", blackFnt8)); summaryRow2Cell1.Colspan = 5; summaryRow2Cell1.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow2Cell1.Border = 0;
            PdfPCell summaryRow2Cell2 = new PdfPCell(new Phrase(classTeacherComment, blackFnt8)); summaryRow2Cell2.Colspan = 10; summaryRow2Cell2.Border = 0;

            PdfPCell summaryRow4Cell1 = new PdfPCell(new Phrase("PRINCIPAL'S COMMENT:", blackFnt8)); summaryRow4Cell1.Colspan = 5; summaryRow4Cell1.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow4Cell1.Border = 0;
            PdfPCell summaryRow4Cell2 = new PdfPCell(new Phrase(headTeacherComment, blackFnt8)); summaryRow4Cell2.Colspan = 10; summaryRow4Cell2.Border = 0;

            //summaryTable.AddCell(Summarycell5); summaryTable.AddCell(Summarycell6);
            //summaryTable.AddCell(Summarycell1); summaryTable.AddCell(Summarycell2);
            //summaryTable.AddCell(Summarycell3); summaryTable.AddCell(Summarycell4);


            //summaryTable.AddCell(summaryRow5Cell1); summaryTable.AddCell(summaryRow5Cell2);
            //summaryTable.AddCell(summaryRow5Cell3); summaryTable.AddCell(summaryRow5Cell4);
            //summaryTable.AddCell(summaryRow5Cell5); summaryTable.AddCell(summaryRow5Cell6);
            //summaryTable.AddCell(summaryRow5Cell7); summaryTable.AddCell(summaryRow5Cell8);
            //summaryTable.AddCell(summaryRow5Cell9); summaryTable.AddCell(summaryRow5Cell10);

            var behavioral = from s in context.ScoreSubCategoryConfigurations
                             where s.ScoreCategoryConfiguration.Category == "Behavioral"
                                 && s.ScoreCategoryConfiguration.SchoolId == schoolId && s.ScoreCategoryConfiguration.CampusId == logonUser.SchoolCampusId
                                 && s.ScoreCategoryConfiguration.ClassId == yearId && s.SessionId == session && s.TermId == TermId
                             select s;
            var extra = from s in context.ScoreSubCategoryConfigurations
                        where s.ScoreCategoryConfiguration.Category == "Extra Curricular"
                                 && s.ScoreCategoryConfiguration.SchoolId == schoolId && s.ScoreCategoryConfiguration.CampusId == logonUser.SchoolCampusId
                                 && s.ScoreCategoryConfiguration.ClassId == yearId && s.SessionId == session && s.TermId == TermId
                        select s;

            //foreach (ScoreSubCategoryConfiguration s in extra)
            //{
            //    StudentScoreExtraCurricular ssb = context.StudentScoreExtraCurriculars.FirstOrDefault(x => x.SubCategoryId == s.Id && x.StudentId == student.Id && x.AdmissionNo == student.AdmissionNumber && x.SessionId == session && x.TermId == term && x.SchoolId == schoolId && x.ClassId == yearId && x.GradeId == gradeId);
            //    PdfPCell psychoCells1 = new PdfPCell(new Phrase(s.SubCategory, blackFnt8)); psychoCells1.Colspan = 3;
            //    socialTable.AddCell(psychoCells1);
            //    if (ssb != null)
            //    {
            //        PdfPCell psychoCells2 = new PdfPCell(new Phrase(Convert.ToInt16(ssb.MarkObtained).ToString(), blackFnt8)); psychoCells2.Colspan = 3;
            //        socialTable.AddCell(psychoCells2);
            //    }
            //    else if (ssb == null)
            //    {
            //        PdfPCell psychoCells2 = new PdfPCell(new Phrase("", blackFnt8)); psychoCells2.Colspan = 3;
            //        socialTable.AddCell(psychoCells2);
            //    }
            //}

            foreach (ScoreSubCategoryConfiguration s in behavioral)
            {
                StudentScoreBehavioral ssb = context.StudentScoreBehaviorals.FirstOrDefault(x => x.SubCategoryId == s.Id && x.StudentId == student.Id && x.AdmissionNo == student.AdmissionNumber && x.SessionId == session && x.TermId == term && x.SchoolId == schoolId && x.ClassId == yearId && x.GradeId == gradeId);
                PdfPCell affectiveCells1 = new PdfPCell(new Phrase(s.SubCategory, blackFnt8)); affectiveCells1.Colspan = 4;
                behaviorTable.AddCell(affectiveCells1);
                if (ssb != null)
                {
                    PdfPCell affectiveCells2 = new PdfPCell(new Phrase(Convert.ToInt16(ssb.MarkObtained).ToString(), blackFnt8)); affectiveCells2.Colspan = 2;
                    behaviorTable.AddCell(affectiveCells2);
                }
                else if (ssb == null)
                {
                    PdfPCell affectiveCells2 = new PdfPCell(new Phrase("", blackFnt8)); affectiveCells2.Colspan = 2;
                    behaviorTable.AddCell(affectiveCells2);
                }
            }

            foreach (ScoreSubCategoryConfiguration s in extra)
            {
                StudentScoreExtraCurricular ssb = context.StudentScoreExtraCurriculars.FirstOrDefault(x => x.SubCategoryId == s.Id && x.StudentId == student.Id && x.AdmissionNo == student.AdmissionNumber && x.SessionId == session && x.TermId == term && x.SchoolId == schoolId && x.ClassId == yearId && x.GradeId == gradeId);
                PdfPCell psychoCells1 = new PdfPCell(new Phrase(s.SubCategory, blackFnt8)); psychoCells1.Colspan = 4;
                behaviorTable.AddCell(psychoCells1);
                if (ssb != null)
                {
                    PdfPCell psychoCells2 = new PdfPCell(new Phrase(Convert.ToInt16(ssb.MarkObtained).ToString(), blackFnt8)); psychoCells2.Colspan = 2;
                    behaviorTable.AddCell(psychoCells2);
                }
                else if (ssb == null)
                {
                    PdfPCell psychoCells2 = new PdfPCell(new Phrase("", blackFnt8)); psychoCells2.Colspan = 2;
                    behaviorTable.AddCell(psychoCells2);
                }
            }



            commentsTable.AddCell(summaryRow2Cell1); commentsTable.AddCell(summaryRow2Cell2);
            commentsTable.AddCell(summaryRow4Cell1); commentsTable.AddCell(summaryRow4Cell2);
            // The row6cell2 was placed here because we need the calculation to present the total number of subject after counting the subject
            PdfPCell row6cell2 = new PdfPCell(new Phrase(subjectCounter.ToString())); row6cell2.HorizontalAlignment = Element.ALIGN_LEFT; row6cell2.Colspan = 1; //row6cell2.Border = 0;
                                                                                                                                                                 //maintbl.AddCell(row6cell1);
                                                                                                                                                                 //maintbl.AddCell(row6cell2);
            document.Add(maintbl);
            document.Add(domain);
            //document.Add(domainandSkills);                                                                                                                                         //maintbl.AddCell(row6cell3);
            //document.Add(new Phrase(Environment.NewLine));
            //document.Add(thirdTerm);
            document.Add(rightTable);
            //document.Add(new Phrase(Environment.NewLine));
            document.Add(baseTable);
            document.Add(commentsTable);




            ////maintbl.AddCell(row6cell4);
            //document.Add(maintbl);
            //document.Add(domain);
            //document.Add(domainandSkills);
            ////document.Add(new Phrase(Environment.NewLine));
            ////document.Add(amaintbl);
            //document.Add(resTable);
            //document.Add(totalScoreObtainedTbl);
            //document.Add(totalScoreObtainableTbl);
            //document.Add(noOfSubjectsTbl);
            //document.Add(noOfDistinctionsTbl);
            //document.Add(noOfCreditsTbl);
            //document.Add(noOfPassesTbl);
            //document.Add(noOfFailureTbl);
            ////document.Add(summaryTable);
            //document.Add(commentsTable);


        }

    }



    protected void addResultSummaryPageCrownEndTerm(Document document, PASSIS.LIB.User student, UsersLIB usrDal)
    {

        iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(Server.MapPath(SchoolLogo));
        jpg.ScaleToFit(70, 70);
        jpg.Alignment = iTextSharp.text.Image.UNDERLYING;
        jpg.SetAbsolutePosition(250, 750);

        Passport = student.PassportFileName;
        if (Passport == null) { Passport = "~/Images/student3.PNG"; }
        iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(Server.MapPath(Passport));
        jpg1.ScaleToFit(70, 70);
        jpg1.Alignment = iTextSharp.text.Image.UNDERLYING;
        jpg1.SetAbsolutePosition(250, 750);
        PdfPTable innerTable = new PdfPTable(9);
        PdfPCell innerCell1 = new PdfPCell(new Phrase("CROWN JEWEL COLLEGE", darkerRedFnt16)); innerCell1.Colspan = 9; innerCell1.Border = 0; innerCell1.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell innerCell2 = new PdfPCell(new Phrase("9, Bakky Street, Off Bello Folawiyo Street, Ikosi, Ketu, Lagos ", darkerGrnFnt9)); innerCell2.Colspan = 9; innerCell2.Border = 0; innerCell2.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell innerCell3 = new PdfPCell(new Phrase("Phone: 08067864385, 08023070027, 08038796369", blackFnt)); innerCell3.Colspan = 9; innerCell3.Border = 0; innerCell3.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell innerCell4 = new PdfPCell(new Phrase("Email: crownjewelschools@yahoo.com, FB: @crownjewelcolleg", blackFnt)); innerCell4.Colspan = 9; innerCell4.Border = 0; innerCell4.HorizontalAlignment = Element.ALIGN_CENTER;
        innerTable.AddCell(innerCell1);
        innerTable.AddCell(innerCell2);
        innerTable.AddCell(innerCell3);
        innerTable.AddCell(innerCell4);
        PdfPTable head = new PdfPTable(15);
        PdfPCell head1 = new PdfPCell(jpg); head1.Colspan = 3; head1.Border = 0;
        PdfPCell head2 = new PdfPCell(innerTable); head2.Colspan = 9; head2.Border = 0;
        PdfPCell head3 = new PdfPCell(jpg1); head3.Colspan = 3; head3.HorizontalAlignment = Element.ALIGN_RIGHT; head3.Border = 0;


        head.AddCell(head1);
        head.AddCell(head2);
        head.AddCell(head3);
        document.Add(head);
        //document.Add(getBackgroundImage());
        //document.Add(new Phrase(Environment.NewLine));
        //document.Add(new Phrase(Environment.NewLine));
        //document.Add(new Phrase(Environment.NewLine));
        //document.Add(new Phrase(Environment.NewLine));
        document.Add(new Phrase(Environment.NewLine));
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

        long? schId = logonUser.SchoolId;
        long gradeId = new PrintResultt_693().theGradeId(student.Id, session).GradeId;
        string noInClass = noOfStudentInClass(schId, gradeId).Count.ToString();
        string classPosition = "";
        var posInClass = getClassPosition(student.AdmissionNumber, term, session, schoolId, yearId, gradeIdd);
        if (posInClass != null)
        {
            classPosition = posInClass.Position.ToString();
        }


        PdfPTable maintbl = new PdfPTable(8);
        PdfPCell cell1 = new PdfPCell(new Phrase("NAME", blackFnt)); cell1.Colspan = 1; cell1.HorizontalAlignment = Element.ALIGN_LEFT; //cell1.Border = 0;
        PdfPCell cell2 = new PdfPCell(new Phrase(student.StudentFullName, blackFnt)); cell2.Colspan = 3; cell1.HorizontalAlignment = Element.ALIGN_LEFT; //cell2.Border = 0;
        PdfPCell cell3 = new PdfPCell(new Phrase("SEX", blackFnt)); cell3.Colspan = 1; cell3.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell cell4 = new PdfPCell(new Phrase(getGender(student.Gender), blackFnt)); cell4.Colspan = 1; cell4.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell cell5 = new PdfPCell(new Phrase("HOUSE", blackFnt)); cell5.Colspan = 1; cell5.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell cell6 = new PdfPCell(new Phrase(student.SchoolHouse, blackFnt)); cell6.Colspan = 1; cell6.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;

        PdfPCell row2cell1 = new PdfPCell(new Phrase("CLASS", blackFnt)); row2cell1.HorizontalAlignment = Element.ALIGN_LEFT; row2cell1.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell row2cell2 = new PdfPCell(new Phrase(usrDal.getStudentsHomeRoom(student.Id), blackFnt)); row2cell2.Colspan = 1; row2cell2.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        PdfPCell row2cell3 = new PdfPCell(new Phrase("AGE", blackFnt)); row2cell3.HorizontalAlignment = Element.ALIGN_LEFT; row2cell3.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell row2cell4 = new PdfPCell(new Phrase(GetAge(Convert.ToDateTime(student.DateOfBirth)).ToString(), blackFnt)); row2cell4.Colspan = 1; row2cell4.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        PdfPCell row2cell5 = new PdfPCell(new Phrase("SESSION", blackFnt)); row2cell5.HorizontalAlignment = Element.ALIGN_LEFT; row2cell5.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell row2cell6 = new PdfPCell(new Phrase(ddlAcademicSession.SelectedItem.Text, blackFnt)); row2cell6.Colspan = 1; row2cell6.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        PdfPCell row2cell7 = new PdfPCell(new Phrase("TERM", blackFnt)); row2cell1.HorizontalAlignment = Element.ALIGN_LEFT; row2cell7.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell row2cell8 = new PdfPCell(new Phrase(ddlAcademicTerm.SelectedItem.Text, blackFnt)); row2cell8.Colspan = 1; row2cell8.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;

        PdfPTable Attendance = new PdfPTable(8);
        PdfPCell row3cell1 = new PdfPCell(new Phrase("ATTENDANCE", blackFnt)); row3cell1.HorizontalAlignment = Element.ALIGN_LEFT; row3cell1.Colspan = 2; //row3cell1.Border = 0;
        PdfPCell row3cell2 = new PdfPCell(new Phrase("", blackFnt)); row3cell2.HorizontalAlignment = Element.ALIGN_LEFT; row3cell2.Colspan = 2; //row3cell2.Border = 0;
        PdfPCell row3cell3 = new PdfPCell(new Phrase("NUMBER OF TIMES", blackFnt)); row3cell3.HorizontalAlignment = Element.ALIGN_LEFT; row3cell3.Colspan = 2; //row3cell1.Border = 0;
        PdfPCell row3cell4 = new PdfPCell(new Phrase("PERCENTAGE PRESENT", blackFnt)); row3cell4.HorizontalAlignment = Element.ALIGN_LEFT; row3cell4.Colspan = 2; //row3cell2.Border = 0;

        PdfPCell row4cell1 = new PdfPCell(new Phrase(" ", blackFnt)); row4cell1.Colspan = 2; row4cell1.HorizontalAlignment = Element.ALIGN_LEFT; //row4cell1.Border = 0;
        PdfPCell row4cell2 = new PdfPCell(new Phrase("SCHOOL OPENED: " + " ", blackFnt)); row4cell2.HorizontalAlignment = Element.ALIGN_LEFT; row4cell2.Colspan = 2; //row4cell2.Border = 0;
        PdfPCell row4cell3 = new PdfPCell(new Phrase("PRESENT: " + " ", blackFnt)); row4cell3.Colspan = 1; row4cell3.HorizontalAlignment = Element.ALIGN_LEFT; //row4cell1.Border = 0;
        PdfPCell row4cell4 = new PdfPCell(new Phrase("ABSENT: " + " ", blackFnt)); row4cell4.HorizontalAlignment = Element.ALIGN_LEFT; row4cell4.Colspan = 1; //row4cell2.Border = 0;
        PdfPCell row4cell5 = new PdfPCell(new Phrase("PERCENTAGE: " + " ", blackFnt)); row4cell5.HorizontalAlignment = Element.ALIGN_LEFT; row4cell5.Colspan = 2; //row4cell2.Border = 0;

        Paragraph domain = new Paragraph(string.Format("{0}", "COGNITIVE DOMAIN"), darkerRedFnt);
        domain.Alignment = Element.ALIGN_CENTER;




        maintbl.AddCell(cell1);
        maintbl.AddCell(cell2);
        maintbl.AddCell(cell3);
        maintbl.AddCell(cell4);
        maintbl.AddCell(cell5);
        maintbl.AddCell(cell6);

        maintbl.AddCell(row2cell1);
        maintbl.AddCell(row2cell2);
        maintbl.AddCell(row2cell3);
        maintbl.AddCell(row2cell4);
        maintbl.AddCell(row2cell5);
        maintbl.AddCell(row2cell6);
        maintbl.AddCell(row2cell7);
        maintbl.AddCell(row2cell8);

        maintbl.AddCell(row3cell1);
        maintbl.AddCell(row3cell2);
        maintbl.AddCell(row3cell3);
        maintbl.AddCell(row3cell4);

        maintbl.AddCell(row4cell1);
        maintbl.AddCell(row4cell2);
        maintbl.AddCell(row4cell3);
        maintbl.AddCell(row4cell4);
        maintbl.AddCell(row4cell5);




        ///PdfPTable resultTable = getResultTable(student);/// dont add the table yet so as to retrieve the gpa viewstate value needed in the gpa summ ary slot 
        /// 
        Int64 sessionId = Convert.ToInt64(ddlAcademicSession.SelectedIndex);
        Int64 TermId = Convert.ToInt64(ddlAcademicTerm.SelectedIndex);
        PdfPTable resTable = new PdfPTable(15);
        //resTable.SetWidthPercentage(new float[8] { 80f, 30f, 30f, 30f, 35f, 35f, 38f, 30f }, PageSize.A5.Rotate());
        //resTable.TotalWidth = 400f;
        //resTable.WidthPercentage = 80;
        //resTable.LockedWidth = true;
        PdfPCell deptHdr = new PdfPCell(new Phrase("DEPT", resultTitleRedFnt10)); deptHdr.Padding = 0f;
        deptHdr.Colspan = 2; deptHdr.HorizontalAlignment = Element.ALIGN_CENTER; deptHdr.VerticalAlignment = Element.ALIGN_BOTTOM;
        resTable.AddCell(deptHdr);
        PdfPCell subjectHdr = new PdfPCell(new Phrase("SUBJECTS", resultTitleRedFnt10)); subjectHdr.Padding = 0f;
        subjectHdr.Colspan = 2; subjectHdr.HorizontalAlignment = Element.ALIGN_CENTER; subjectHdr.VerticalAlignment = Element.ALIGN_BOTTOM;
        resTable.AddCell(subjectHdr);
        PdfPCell cwHdr = new PdfPCell(new Phrase("CW", resultTitleRedFnt8)); cwHdr.Colspan = 1; cwHdr.Rotation = 90; cwHdr.VerticalAlignment = Element.ALIGN_TOP; cwHdr.HorizontalAlignment = Element.ALIGN_LEFT;
        resTable.AddCell(cwHdr);
        PdfPCell hwHdr = new PdfPCell(); hwHdr.AddElement(new Phrase("HW/PRO", resultTitleRedFnt8)); hwHdr.Colspan = 1; hwHdr.Rotation = 90; hwHdr.HorizontalAlignment = Element.ALIGN_LEFT; hwHdr.VerticalAlignment = Element.ALIGN_TOP;
        resTable.AddCell(hwHdr);
        PdfPCell attHdr = new PdfPCell(); attHdr.Colspan = 1; attHdr.Rotation = 90; attHdr.HorizontalAlignment = Element.ALIGN_LEFT; attHdr.VerticalAlignment = Element.ALIGN_TOP; attHdr.AddElement(new Phrase("ATT/NOTE", resultTitleRedFnt8)); resTable.AddCell(attHdr);
        PdfPCell testHdr = new PdfPCell(); testHdr.Colspan = 1; testHdr.Rotation = 90; testHdr.HorizontalAlignment = Element.ALIGN_LEFT; testHdr.VerticalAlignment = Element.ALIGN_TOP; testHdr.AddElement(new Phrase("TEST", resultTitleRedFnt8)); resTable.AddCell(testHdr);
        PdfPCell totalHdr = new PdfPCell(); totalHdr.Colspan = 1; totalHdr.Rotation = 90; totalHdr.HorizontalAlignment = Element.ALIGN_LEFT; totalHdr.VerticalAlignment = Element.ALIGN_TOP; totalHdr.AddElement(new Phrase("TOTAL", resultTitleRedFnt8)); resTable.AddCell(totalHdr);
        PdfPCell caHdr = new PdfPCell(); caHdr.Colspan = 1; caHdr.Rotation = 90; caHdr.HorizontalAlignment = Element.ALIGN_LEFT; caHdr.VerticalAlignment = Element.ALIGN_TOP; caHdr.AddElement(new Phrase("CA", resultTitleRedFnt8)); resTable.AddCell(caHdr);
        PdfPCell examHdr = new PdfPCell(); examHdr.Colspan = 1; examHdr.Rotation = 90; examHdr.HorizontalAlignment = Element.ALIGN_LEFT; examHdr.VerticalAlignment = Element.ALIGN_TOP; examHdr.AddElement(new Phrase("EXAM", resultTitleRedFnt8)); resTable.AddCell(examHdr);
        PdfPCell total2Hdr = new PdfPCell(); total2Hdr.Colspan = 1; total2Hdr.Rotation = 90; total2Hdr.HorizontalAlignment = Element.ALIGN_LEFT; total2Hdr.VerticalAlignment = Element.ALIGN_TOP; total2Hdr.AddElement(new Phrase("TOTAL", resultTitleRedFnt8)); resTable.AddCell(total2Hdr);
        PdfPCell gradeHdr = new PdfPCell(); gradeHdr.Colspan = 1; gradeHdr.Rotation = 90; gradeHdr.HorizontalAlignment = Element.ALIGN_LEFT; gradeHdr.VerticalAlignment = Element.ALIGN_TOP; gradeHdr.AddElement(new Phrase("GRADE", resultTitleRedFnt8)); resTable.AddCell(gradeHdr);
        PdfPCell commentHdr = new PdfPCell(); commentHdr.Colspan = 2; commentHdr.HorizontalAlignment = Element.ALIGN_CENTER; commentHdr.VerticalAlignment = Element.ALIGN_BOTTOM; commentHdr.AddElement(new Phrase("COMMENT", resultTitleRedFnt8)); resTable.AddCell(commentHdr);
        PdfPCell space = new PdfPCell(); space.Colspan = 2;
        resTable.AddCell(space);
        resTable.AddCell(space);
        resTable.AddCell("20");
        resTable.AddCell("20");
        resTable.AddCell("10");
        resTable.AddCell("50");
        resTable.AddCell("100");
        resTable.AddCell("40");
        resTable.AddCell("60");
        resTable.AddCell("100");
        resTable.AddCell("");
        resTable.AddCell(space);

        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);
        PASSIS.LIB.GradeStudent gs = new ClassGradeLIB().RetrieveStudentGrade(student.Id, session);

        //Int64 subjectCounter = 0;
        decimal totalMarkObtained = 0;
        int totalMarkObtainable = 0;
        decimal testScoreConfiguration = 0;
        decimal cw = 0;
        decimal hw = 0;
        decimal note = 0;
        decimal testScore = 0;
        decimal ca = 0;
        decimal examScore = 0m;
        decimal totalScore = 0;
        decimal totalScoreAverage = 0;
        decimal scorePosition = 0;
        decimal aggregateTotalScore = 0;
        string subjName = "";
        string deptName = "";
        decimal percentage = 0m;
        long subjectId = 0;
        Dictionary<long, string> dictionary = new Dictionary<long, string>();


        if (gs != null)
        {
            //IList<PASSIS.LIB.Subject> getSub = getSubject(session, term, schoolId, yearId, gradeIdd);
            //foreach (PASSIS.LIB.Subject sub in getSub) 
            //{
            //    subjectCounter++;
            //}
            var deptList = from s in context.SubjectDepartments where s.SchoolId == logonUser.SchoolId select s;
            foreach (SubjectDepartment dept in deptList)
            {
                IList<StudentScoreCA> rpCard = getSubjectScoreCA(student.AdmissionNumber, student.Id, session, term, schoolId, yearId, gradeIdd, dept.Id);
                foreach (StudentScoreCA d in rpCard)
                {

                    if (dictionary.ContainsKey(dept.Id))
                    {
                        deptName = "";
                        PdfPCell deptCell = new PdfPCell(new Phrase(deptName, darkerGrnFnt9)); deptCell.Colspan = 2; deptCell.BorderWidthTop = 0; deptCell.BorderColorTop = new BaseColor(255, 255, 255);
                        resTable.AddCell(deptCell);
                    }
                    else
                    {
                        deptName = dept.DepartmentName;
                        dictionary.Add(dept.Id, dept.DepartmentName);
                        PdfPCell deptCell = new PdfPCell(new Phrase(deptName, darkerGrnFnt9)); deptCell.Colspan = 2;
                        resTable.AddCell(deptCell);
                    }

                    subjectCounter++;

                    subjName = d.Subject.Name;
                    testScore = Convert.ToDecimal(d.Test);
                    cw = Convert.ToDecimal(d.ClassWork);
                    hw = Convert.ToDecimal(d.HomeWorkProject);
                    note = Convert.ToDecimal(d.AttendanceNote);
                    totalScore = cw + hw + note + testScore;
                    ca = (totalScore / 100) * 40;
                    examScore = Convert.ToDecimal(d.Exam);
                    totalScoreAverage = ca + examScore;
                    aggregateTotalScore += totalScoreAverage;
                    //totalScore = Convert.ToDecimal(d.Total);

                    //scorePosition = Convert.ToDecimal(d.Position);

                    string grade = PASSIS.LIB.Utility.Utili.getExamGradeLetters(Convert.ToDecimal(totalScoreAverage), (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue));
                    if (grade.Trim() == "A") { distinctionCounter++; }
                    else if (grade.Trim() == "B" || grade.Trim() == "C") { creditCounter++; }
                    else if (grade.Trim() == "D") { passesCounter++; }
                    else if (grade.Trim() == "E") { failureCounter++; }


                    PdfPCell subjCell = new PdfPCell(new Phrase(d.Subject.Name, darkerGrnFnt9)); subjCell.Colspan = 2;
                    resTable.AddCell(subjCell);
                    PdfPCell cwCell = new PdfPCell(new Phrase(d.ClassWork.ToString(), blackFnt)); cwCell.Colspan = 1;
                    resTable.AddCell(cwCell);
                    PdfPCell hwCell = new PdfPCell(new Phrase(d.HomeWorkProject.ToString(), blackFnt)); hwCell.Colspan = 1;
                    resTable.AddCell(hwCell);
                    PdfPCell noteCell = new PdfPCell(new Phrase(d.AttendanceNote.ToString(), blackFnt)); noteCell.Colspan = 1;
                    resTable.AddCell(noteCell);
                    PdfPCell testCell = new PdfPCell(new Phrase(d.Test.ToString(), blackFnt)); testCell.Colspan = 1;
                    resTable.AddCell(testCell);
                    PdfPCell totalCell = new PdfPCell(new Phrase(totalScore.ToString(), blackFnt)); totalCell.Colspan = 1;
                    resTable.AddCell(totalCell);
                    PdfPCell caCell = new PdfPCell(new Phrase(ca.ToString(), blackFnt)); noteCell.Colspan = 1;
                    resTable.AddCell(caCell);
                    PdfPCell examCell = new PdfPCell(new Phrase(d.Exam.ToString(), blackFnt)); testCell.Colspan = 1;
                    resTable.AddCell(examCell);
                    PdfPCell total2Cell = new PdfPCell(new Phrase(totalScoreAverage.ToString(), blackFnt)); totalCell.Colspan = 1;
                    resTable.AddCell(total2Cell);
                    PdfPCell gradeCell = new PdfPCell(new Phrase((PASSIS.LIB.Utility.Utili.getExamGradeLetters(Convert.ToDecimal(totalScoreAverage), (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue))), blackFnt)); gradeCell.Colspan = 1;
                    resTable.AddCell(gradeCell);
                    PdfPCell tRemark = new PdfPCell(new Phrase(PASSIS.LIB.Utility.Utili.getExamGradeRemarks(Convert.ToDecimal(totalScoreAverage), (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)), blackFnt)); tRemark.Colspan = 2;
                    resTable.AddCell(tRemark);


                    //++noOfSubjectWithScore;
                    totalMarkObtainable = 0;
                    totalMarkObtained = 0;
                    testScore = 0;
                }
            }
        }




        //document.Add(new Phrase(Environment.NewLine));
        if (subjectCounter != 0)
        {
            percentage = (aggregateTotalScore / (subjectCounter * 100)) * 100;
        }
        //string classTeacherComment = txtClassTeacherComment.Text.Trim();
        //string proprietorComment = txtProprietorComment.Text.Trim();
        PdfPTable summaryTable = new PdfPTable(10);
        iTextSharp.text.pdf.draw.LineSeparator line = new iTextSharp.text.pdf.draw.LineSeparator(0f, 100f, BaseColor.BLACK, Element.ALIGN_LEFT, 1);
        PdfPCell Summarycell1 = new PdfPCell(new Phrase("Total Score Obtained:", blackFnt)); Summarycell1.Colspan = 3; Summarycell1.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell1.Border = 0;
        PdfPCell Summarycell2 = new PdfPCell(new Phrase(Math.Round(aggregateTotalScore, 0).ToString(), blackFnt)); Summarycell2.Colspan = 1; Summarycell2.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell2.Border = 0;
        PdfPCell Summarycell3 = new PdfPCell(new Phrase("Total Score Obtainable:", blackFnt)); Summarycell3.Colspan = 2; Summarycell3.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell3.Border = 0;
        PdfPCell Summarycell4 = new PdfPCell(new Phrase((subjectCounter * 100).ToString(), blackFnt)); Summarycell4.Colspan = 1; Summarycell4.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell4.Border = 0;
        PdfPCell Summarycell5 = new PdfPCell(new Phrase("Percentage(%):", blackFnt)); Summarycell5.Colspan = 2; Summarycell5.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell5.Border = 0;
        PdfPCell Summarycell6 = new PdfPCell(new Phrase(Math.Round(percentage, 0).ToString(), blackFnt)); Summarycell6.Colspan = 1; Summarycell6.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell6.Border = 0;

        PdfPCell summaryRow5Cell1 = new PdfPCell(new Phrase("Number of Subject Offered:", blackFnt)); summaryRow5Cell1.Colspan = 4; summaryRow5Cell1.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow5Cell1.Border = 0;
        PdfPCell summaryRow5Cell2 = new PdfPCell(new Phrase(subjectCounter.ToString(), blackFnt)); summaryRow5Cell2.Colspan = 6; summaryRow5Cell2.Border = 0;
        PdfPCell summaryRow5Cell3 = new PdfPCell(new Phrase("Number of Distinctions:", blackFnt)); summaryRow5Cell3.Colspan = 4; summaryRow5Cell3.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow5Cell3.Border = 0;
        PdfPCell summaryRow5Cell4 = new PdfPCell(new Phrase(distinctionCounter.ToString(), blackFnt)); summaryRow5Cell4.Colspan = 6; summaryRow5Cell4.Border = 0;
        PdfPCell summaryRow5Cell5 = new PdfPCell(new Phrase("Number of Credits:", blackFnt)); summaryRow5Cell5.Colspan = 4; summaryRow5Cell5.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow5Cell5.Border = 0;
        PdfPCell summaryRow5Cell6 = new PdfPCell(new Phrase(creditCounter.ToString(), blackFnt)); summaryRow5Cell6.Colspan = 6; summaryRow5Cell6.Border = 0;
        PdfPCell summaryRow5Cell7 = new PdfPCell(new Phrase("Number of Passes:", blackFnt)); summaryRow5Cell7.Colspan = 4; summaryRow5Cell7.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow5Cell7.Border = 0;
        PdfPCell summaryRow5Cell8 = new PdfPCell(new Phrase(passesCounter.ToString(), blackFnt)); summaryRow5Cell8.Colspan = 6; summaryRow5Cell8.Border = 0;
        PdfPCell summaryRow5Cell9 = new PdfPCell(new Phrase("Number of Failure:", blackFnt)); summaryRow5Cell9.Colspan = 4; summaryRow5Cell9.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow5Cell9.Border = 0;
        PdfPCell summaryRow5Cell10 = new PdfPCell(new Phrase(failureCounter.ToString(), blackFnt)); summaryRow5Cell10.Colspan = 6; summaryRow5Cell10.Border = 0;


        PdfPCell summaryRow2Cell1 = new PdfPCell(new Phrase("CLASS TEACHER'S COMMENT:", blackFnt)); summaryRow2Cell1.Colspan = 4; summaryRow2Cell1.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow2Cell1.Border = 0;
        PdfPCell summaryRow2Cell2 = new PdfPCell(new Phrase("", blackFnt)); summaryRow2Cell2.Colspan = 6; summaryRow2Cell2.Border = 0;

        PdfPCell summaryRow4Cell1 = new PdfPCell(new Phrase("PRINCIPAL'S COMMENT:", blackFnt)); summaryRow4Cell1.Colspan = 3; summaryRow4Cell1.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow4Cell1.Border = 0;
        PdfPCell summaryRow4Cell2 = new PdfPCell(new Phrase("", blackFnt)); summaryRow4Cell2.Colspan = 7; summaryRow4Cell2.Border = 0;

        summaryTable.AddCell(Summarycell1); summaryTable.AddCell(Summarycell2); summaryTable.AddCell(Summarycell3);
        summaryTable.AddCell(Summarycell4); summaryTable.AddCell(Summarycell5); summaryTable.AddCell(Summarycell6);
        summaryTable.AddCell(summaryRow5Cell1); summaryTable.AddCell(summaryRow5Cell2);
        summaryTable.AddCell(summaryRow5Cell3); summaryTable.AddCell(summaryRow5Cell4);
        summaryTable.AddCell(summaryRow5Cell5); summaryTable.AddCell(summaryRow5Cell6);
        summaryTable.AddCell(summaryRow5Cell7); summaryTable.AddCell(summaryRow5Cell8);
        summaryTable.AddCell(summaryRow5Cell9); summaryTable.AddCell(summaryRow5Cell10);

        summaryTable.AddCell(summaryRow2Cell1); summaryTable.AddCell(summaryRow2Cell2);
        summaryTable.AddCell(summaryRow4Cell1); summaryTable.AddCell(summaryRow4Cell2);
        // The row6cell2 was placed here because we need the calculation to present the total number of subject after counting the subject
        PdfPCell row6cell2 = new PdfPCell(new Phrase(subjectCounter.ToString())); row6cell2.HorizontalAlignment = Element.ALIGN_LEFT; row6cell2.Colspan = 1; //row6cell2.Border = 0;
        //maintbl.AddCell(row6cell1);
        //maintbl.AddCell(row6cell2);
        //maintbl.AddCell(row6cell3);
        //maintbl.AddCell(row6cell4);
        document.Add(maintbl);
        document.Add(domain);
        //document.Add(new Phrase(Environment.NewLine));
        //document.Add(amaintbl);
        document.Add(resTable);
        //document.Add(bmaintbl);
        document.Add(summaryTable);


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

    public IList<AcademicTerm1> schTerm()
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        var session = from c in context.AcademicSessions
                      where c.SchoolId == logonUser.SchoolId
                      orderby c.IsCurrent descending
                      select c.AcademicTerm1;
        return session.ToList<AcademicTerm1>();
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


    public static IList<StudentScoreRepository> getSubjectScorePerSubcategory(string admNo, long studentId, long sessionId, long termId, long? schId, long yearId, long gradeId, long subId, long catId, long subCatId)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        IList<PASSIS.LIB.StudentScoreRepository> scoreRepo= (from s in context.StudentScoreRepositories
                                                                where s.SchoolId == schId &&
                                                                s.SessionId == sessionId &&
                                                                s.TermId == termId &&
                                                                s.AdmissionNo == admNo && 
                                                                s.StudentId == studentId && 
                                                                s.ClassId == yearId &&
                                                                s.SubjectId == subId && 
                                                                s.CategoryId == catId && 
                                                                s.SubCategoryId == subCatId
                                                                select s).ToList();
        return scoreRepo.ToList<PASSIS.LIB.StudentScoreRepository>();
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
                                                              s.SubjectId == subId &&
                                                              s.CategoryId != null &&
                                                              s.SubCategoryId != null
                                                          select s).ToList();
        return score.ToList<PASSIS.LIB.StudentScoreRepository>();
    }
    public static IList<PASSIS.LIB.StudentScore> getSubjectScoreCategoryExam(string admNo, long studentId, long sessionId, long termId, long? schId, long yearId, long gradeId, long subId)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        IList<PASSIS.LIB.StudentScore> score = (from s in context.StudentScores
                                                where
                                                    s.AdmissionNumber == admNo &&
                                                    s.StudentId == studentId &&
                                                    s.AcademicSessionID == sessionId &&
                                                    s.TermId == termId &&
                                                    s.SchoolId == schId &&
                                                    s.ClassId == yearId &&
                                                    s.GradeId == gradeId &&
                                                    s.SubjectId == subId &&
                                                    s.CategoryId != null &&
                                                    s.SubCategoryId != null
                                                select s).ToList();
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