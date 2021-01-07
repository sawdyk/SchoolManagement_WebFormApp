
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

public partial class PrintResult : PASSIS.LIB.Utility.BasePage
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
    public static string SchoolName = "";
    public static string SchoolLogo = "";
    public static string SchoolAddress = "";
    public static string SchoolCurriculumId = "";
    public static string SchoolUrl = "";
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    IList<PASSIS.LIB.User> students;
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (logonUser.SchoolId != 119 && logonUser.SchoolId !=162)
        //{
        Response.Redirect("PrintResultt-" + logonUser.SchoolId + ".aspx");
        //}
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

            //string querySession = "SELECT DISTINCT AcademicSessionId FROM AcademicSession WHERE SchoolId=" + logonUser.SchoolId;
            //SqlDataReader readerSession = mdb.fetch(querySession);
            //while (readerSession.Read())
            //{
            //    ddlAcademicSession.DataSource = from s in context.AcademicSessionNames
            //                                    where s.ID == Convert.ToInt64(readerSession["AcademicSessionId"].ToString())
            //                                    select s;
            //    ddlAcademicSession.DataBind();
            //}

            //reader.Close();
            //readerSession.Close();
            //mdb.closeConnct();

            var academicSessions = (from c in context.AcademicSessions

                                    where c.SchoolId == logonUser.SchoolId

                                    orderby c.IsCurrent descending

                                    select new
                                    {


                                        // ACInstitutions  = GetInstitution(c.InstitutionName)
                                        c.AcademicSessionName.SessionName

                                    }).Distinct();

            ddlAcademicSession.DataSource = new PrintResult().schSession().Distinct();
            ddlAcademicSession.DataTextField = "SessionName";
            ddlAcademicSession.DataValueField = "ID";
            ddlAcademicSession.DataBind();
            ddlAcademicSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Session--", "0", true));

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
            }
            else if (curriculumId == (long)CurriculumType.Nigerian)
            {
                ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
            }
            ddlYear.DataBind();

            //ddlAcademicTerm.DataSource = new AcademicTermLIB().RetrieveAcademicTerm();
            //ddlAcademicTerm.DataBind();



            var academicTermCurrent = (from c in context.AcademicSessions
                                       where c.IsCurrent == true
                                       select new
                                       {


                                           // ACInstitutions  = GetInstitution(c.InstitutionName)
                                           c.AcademicTerm1

                                       }).Distinct();


            ddlAcademicTerm.DataSource = new PrintResult().schTerm().Distinct();
            ddlAcademicTerm.DataTextField = "AcademicTermName";
            ddlAcademicTerm.DataValueField = "Id";
            ddlAcademicTerm.DataBind();
            ddlAcademicTerm.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Term--", "0", true));



            //var academicTermlastTerm = (from c in context.AcademicSessions

            //                            where c.IsCurrent == false

            //                            select new
            //                            {


            //                                // ACInstitutions  = GetInstitution(c.InstitutionName)
            //                                c.AcademicTerm1.AcademicTermName

            //                            }).Distinct();

            //ddlAcademicTerm.DataSource = academicTermlastTerm;
            //ddlAcademicTerm.DataTextField = "AcademicTermName";
            //ddlAcademicTerm.DataBind();

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
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

        var scoreList = new ClassGradeLIB().RetrieveGradeStudents(schoolId, campusId, yearId, gradeId, curSessionId);

        if (logonUser.SchoolId == 1)
        {
            gdvList.DataSource = RetrieveStudentFromCA(termId, sessionId, schoolId, campusId, yearId, gradeId);
            gdvList.DataBind();
        }
        else if (logonUser.SchoolId == 119 || logonUser.SchoolId == 162)
        {
            students = new PASSIS.LIB.UsersLIB().RetrieveStudents(schoolId, campusId, yearId, gradeId);
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

        bulkPrinting(RetrieveStudentFromReport(termId, sessionId, schoolId, campusId, yearID, gradeId));
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
        var students = from s in context.StudentScoreCAs
                       where s.TermId == termId && s.AcademicSessionID == sessionId && s.SchoolId == schoolId &&
                           s.CampusId == campusId && s.GradeId == gradeId && s.ClassId == yearId
                       select s;
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
    private Font darkRedFnt = FontFactory.GetFont(BaseFont.HELVETICA, 12, Font.NORMAL, new BaseColor(169, 34, 82));
    private Font darkRedFnt11 = FontFactory.GetFont(BaseFont.HELVETICA, 10, Font.BOLD, new BaseColor(170, 38, 98));
    private Font darkerGrnFnt12 = FontFactory.GetFont(BaseFont.HELVETICA, 12, Font.BOLD, new BaseColor(26, 74, 17));
    private Font darkerGrnFnt9 = FontFactory.GetFont(BaseFont.HELVETICA, 10, Font.BOLD, new BaseColor(26, 74, 17));
    private Font grnFnt = FontFactory.GetFont(BaseFont.HELVETICA, 14, new BaseColor(26, 74, 17));
    private Font blackFntB = FontFactory.GetFont(BaseFont.HELVETICA, 14, Font.BOLD, new BaseColor(0, 0, 0));
    private Font blackFnt = FontFactory.GetFont(BaseFont.HELVETICA, 14, new BaseColor(0, 0, 0));
    private Font resultRedFnt = FontFactory.GetFont(BaseFont.HELVETICA, 12, new BaseColor(161, 13, 76));
    private Font resultRedFnt_b = FontFactory.GetFont(BaseFont.HELVETICA, 12, Font.BOLD, new BaseColor(161, 13, 76));
    private Font resultGrnFnt = FontFactory.GetFont(BaseFont.HELVETICA, 12, new BaseColor(170, 38, 98));
    private Font resultGrnFnt_b = FontFactory.GetFont(BaseFont.HELVETICA, 12, Font.BOLD, new BaseColor(170, 38, 98));
    private Font blackFnt12 = FontFactory.GetFont(BaseFont.HELVETICA, 12, new BaseColor(0, 0, 0));
    private Font resultTitleRedFnt10 = FontFactory.GetFont(BaseFont.HELVETICA, 10, Font.BOLD, new BaseColor(161, 13, 76));
    private Font resultTitleRedFnt9 = FontFactory.GetFont(BaseFont.HELVETICA, 9, Font.BOLD, new BaseColor(161, 13, 76));
    private Font resultTitleRedFnt8 = FontFactory.GetFont(BaseFont.HELVETICA, 8, Font.BOLD, new BaseColor(161, 13, 76));
    private iTextSharp.text.Image getBackgroundImage()
    {
        string imagepath = Server.MapPath(SchoolLogo);// +"\\images\\";
        iTextSharp.text.Image backgroundLogo = iTextSharp.text.Image.GetInstance(imagepath);
        backgroundLogo.ScaleToFit(70, 70);
        backgroundLogo.Alignment = iTextSharp.text.Image.UNDERLYING;
        backgroundLogo.SetAbsolutePosition(250, 750);
        return backgroundLogo;
    }
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

        if (logonUser.SchoolId == 119)
        {
            addResultSummaryPageCrownMidTerm(document, student, usrDal);
        }
        else
        {
            addResultSummaryPage(document, student, usrDal);
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
        document.SetMargins(0f, 10f, 30f, 0f);
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

    protected void addResultSummaryPageCrownMidTerm(Document document, PASSIS.LIB.User student, UsersLIB usrDal)
    {
        PdfPTable table = new PdfPTable(2);
        PdfPCell cell = new PdfPCell();
        Paragraph c = new Paragraph();
        c.Add(new Phrase("Test "));
        c.Add(new Chunk(getBackgroundImage(), 0, 0));
        c.Add(new Phrase(" more text "));
        c.Add(new Chunk(getBackgroundImage(), 0, 0));
        c.Add(new Chunk(getBackgroundImage(), 0, 0));
        c.Add(new Phrase(" end."));
        cell.AddElement(c);
        table.AddCell(cell);
        table.AddCell(new PdfPCell(new Phrase("test 2")));
        document.Add(table);
        document.Add(getBackgroundImage());
        document.Add(new Phrase(Environment.NewLine));
        document.Add(new Phrase(Environment.NewLine));
        document.Add(new Phrase(Environment.NewLine));
        document.Add(new Phrase(Environment.NewLine));
        document.Add(new Phrase(Environment.NewLine));
        Paragraph schoolname = new Paragraph(string.Format("{0}", SchoolName), darkerRedFnt);
        schoolname.Alignment = Element.ALIGN_CENTER;
        document.Add(schoolname);
        Paragraph schoolAddress = new Paragraph(string.Format("{0}", SchoolAddress), darkerRedFnt);
        schoolAddress.Alignment = Element.ALIGN_CENTER;
        document.Add(schoolAddress);
        Paragraph p = new Paragraph("Report Card For", darkerRedFnt);
        p.Alignment = Element.ALIGN_CENTER;
        document.Add(p);
        Paragraph SessionDetails = new Paragraph(string.Format("{0} {1} {2} Session", ddlAcademicTerm.SelectedItem.Text, ",", ddlAcademicSession.SelectedItem.Text), darkerRedFnt);
        SessionDetails.Alignment = Element.ALIGN_CENTER;
        document.Add(SessionDetails);

        document.Add(new Phrase(Environment.NewLine));
        document.Add(new Phrase(Environment.NewLine));

        long session = Convert.ToInt64(ddlAcademicSession.SelectedValue);
        long term = Convert.ToInt64(ddlAcademicTerm.SelectedValue);
        long? schoolId = logonUser.SchoolId;
        long yearId = Convert.ToInt64(ddlYear.SelectedValue);
        long gradeIdd = Convert.ToInt64(ddlGrade.SelectedValue);

        long? schId = logonUser.SchoolId;
        long gradeId = new PrintResult().theGradeId(student.Id).GradeId;
        string noInClass = noOfStudentInClass(schId, gradeId).Count.ToString();
        string classPosition = "";
        var posInClass = getClassPosition(student.AdmissionNumber, term, session, schoolId, yearId, gradeIdd);
        if (posInClass != null)
        {
            classPosition = posInClass.Position.ToString();
        }


        PdfPTable maintbl = new PdfPTable(8);
        PdfPCell cell1 = new PdfPCell(new Phrase("NAME")); cell1.Colspan = 1; cell1.HorizontalAlignment = Element.ALIGN_LEFT; //cell1.Border = 0;
        PdfPCell cell2 = new PdfPCell(new Phrase(student.StudentFullName)); cell2.Colspan = 3; cell1.HorizontalAlignment = Element.ALIGN_LEFT; //cell2.Border = 0;
        PdfPCell cell3 = new PdfPCell(new Phrase("SEX")); cell3.Colspan = 1; cell3.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell cell4 = new PdfPCell(new Phrase(getGender(student.Gender))); cell4.Colspan = 1; cell4.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell cell5 = new PdfPCell(new Phrase("HOUSE")); cell5.Colspan = 1; cell5.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell cell6 = new PdfPCell(new Phrase(student.SchoolHouse)); cell6.Colspan = 1; cell6.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;

        PdfPCell row2cell1 = new PdfPCell(new Phrase("CLASS")); row2cell1.HorizontalAlignment = Element.ALIGN_LEFT; row2cell1.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell row2cell2 = new PdfPCell(new Phrase(usrDal.getStudentsHomeRoom(student.Id))); row2cell2.Colspan = 1; row2cell2.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        PdfPCell row2cell3 = new PdfPCell(new Phrase("AGE")); row2cell3.HorizontalAlignment = Element.ALIGN_LEFT; row2cell3.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell row2cell4 = new PdfPCell(new Phrase(GetAge(Convert.ToDateTime(student.DateOfBirth)))); row2cell4.Colspan = 1; row2cell4.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        PdfPCell row2cell5 = new PdfPCell(new Phrase("SESSION")); row2cell5.HorizontalAlignment = Element.ALIGN_LEFT; row2cell5.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell row2cell6 = new PdfPCell(new Phrase(ddlAcademicSession.SelectedItem.Text)); row2cell6.Colspan = 1; row2cell6.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        PdfPCell row2cell7 = new PdfPCell(new Phrase("TERM")); row2cell1.HorizontalAlignment = Element.ALIGN_LEFT; row2cell7.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell row2cell8 = new PdfPCell(new Phrase(ddlAcademicTerm.SelectedItem.Text)); row2cell8.Colspan = 1; row2cell8.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;

        PdfPTable Attendance = new PdfPTable(8);
        PdfPCell row3cell1 = new PdfPCell(new Phrase("ATTENDANCE")); row3cell1.HorizontalAlignment = Element.ALIGN_LEFT; row3cell1.Colspan = 2; //row3cell1.Border = 0;
        PdfPCell row3cell2 = new PdfPCell(new Phrase("")); row3cell2.HorizontalAlignment = Element.ALIGN_LEFT; row3cell2.Colspan = 2; //row3cell2.Border = 0;
        PdfPCell row3cell3 = new PdfPCell(new Phrase("NUMBER OF TIMES")); row3cell3.HorizontalAlignment = Element.ALIGN_LEFT; row3cell3.Colspan = 2; //row3cell1.Border = 0;
        PdfPCell row3cell4 = new PdfPCell(new Phrase("PERCENTAGE PRESENT")); row3cell4.HorizontalAlignment = Element.ALIGN_LEFT; row3cell4.Colspan = 2; //row3cell2.Border = 0;

        PdfPCell row4cell1 = new PdfPCell(new Phrase("")); row4cell1.Colspan = 2; row4cell1.HorizontalAlignment = Element.ALIGN_LEFT; //row4cell1.Border = 0;
        PdfPCell row4cell2 = new PdfPCell(new Phrase("SCHOOL OPENED: " + " ")); row4cell2.HorizontalAlignment = Element.ALIGN_LEFT; row4cell2.Colspan = 2; //row4cell2.Border = 0;
        PdfPCell row4cell3 = new PdfPCell(new Phrase("PRESENT: " + " ")); row4cell1.Colspan = 1; row4cell1.HorizontalAlignment = Element.ALIGN_LEFT; //row4cell1.Border = 0;
        PdfPCell row4cell4 = new PdfPCell(new Phrase("ABSENT: " + " ")); row4cell2.HorizontalAlignment = Element.ALIGN_LEFT; row4cell2.Colspan = 1; //row4cell2.Border = 0;
        PdfPCell row4cell5 = new PdfPCell(new Phrase("PERCENTAGE: " + " ")); row4cell2.HorizontalAlignment = Element.ALIGN_LEFT; row4cell2.Colspan = 2; //row4cell2.Border = 0;

        Paragraph domain = new Paragraph(string.Format("{0}", "COGNITIVE DOMAIN"), darkerRedFnt);
        domain.Alignment = Element.ALIGN_CENTER;
        document.Add(domain);

        PdfPCell row5cell1 = new PdfPCell(new Phrase("NO IN CLASS")); row5cell1.HorizontalAlignment = Element.ALIGN_LEFT; row5cell1.Colspan = 3; //row5cell1.Border = 0;
        PdfPCell row5cell2 = new PdfPCell(new Phrase(noInClass)); row5cell2.HorizontalAlignment = Element.ALIGN_LEFT; row5cell2.Colspan = 1; //row5cell2.Border = 0;
        PdfPCell row5cell3 = new PdfPCell(new Phrase("POSITION IN CLASS")); row5cell3.HorizontalAlignment = Element.ALIGN_LEFT; row5cell3.Colspan = 3; //row5cell1.Border = 0;
        PdfPCell row5cell4 = new PdfPCell(new Phrase(classPosition + ToOrdinal(Convert.ToInt16(Convert.ToInt64(1))))); row5cell4.HorizontalAlignment = Element.ALIGN_LEFT; row5cell4.Colspan = 1; //row5cell2.Border = 0;

        PdfPCell row6cell1 = new PdfPCell(new Phrase("NO OF SUBJECT TAKEN")); row6cell1.Colspan = 3; row6cell1.HorizontalAlignment = Element.ALIGN_LEFT; //row6cell1.Border = 0;
        //PdfPCell row6cell2 = new PdfPCell(new Phrase(subjectCounter.ToString())); row6cell2.HorizontalAlignment = Element.ALIGN_LEFT; row6cell2.Colspan = 1; //row6cell2.Border = 0;
        PdfPCell row6cell3 = new PdfPCell(new Phrase("NO OF SUBJECT PASSED")); row6cell3.HorizontalAlignment = Element.ALIGN_LEFT; row6cell3.Colspan = 3; //row6cell3.Border = 0;
        PdfPCell row6cell4 = new PdfPCell(new Phrase("...")); row6cell4.HorizontalAlignment = Element.ALIGN_LEFT; row6cell4.Colspan = 1; //row6cell4.Border = 0;

        //PdfPCell row4cell1 = new PdfPCell(new Phrase("Name")); row4cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell row4cell2 = new PdfPCell(new Phrase(student.StudentFullName)); row4cell2.Colspan = 3; row4cell2.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell row4cell3 = new PdfPCell(new Phrase("Times Present")); row4cell3.Colspan = 2; row4cell3.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell row4cell4 = new PdfPCell(new Phrase("16")); row4cell4.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell row4cell5 = new PdfPCell(new Phrase("63.99-55")); row4cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
        //PdfPCell row4cell6 = new PdfPCell(new Phrase("C")); row4cell6.HorizontalAlignment = Element.ALIGN_LEFT;

        //PdfPCell row5cell1 = new PdfPCell(new Phrase("Adm. No")); row5cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell row5cell2 = new PdfPCell(new Phrase(student.AdmissionNumber)); row5cell2.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell row5cell3 = new PdfPCell(new Phrase("Gender")); row5cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
        //PdfPCell row5cell4 = new PdfPCell(new Phrase("Male")); row5cell4.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell row5cell5 = new PdfPCell(new Phrase("Times Puntual")); row5cell5.Colspan = 2; row5cell5.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell row5cell6 = new PdfPCell(new Phrase("16")); row5cell6.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell row5cell7 = new PdfPCell(new Phrase("54.99-45")); row5cell7.HorizontalAlignment = Element.ALIGN_RIGHT;
        //PdfPCell row5cell8 = new PdfPCell(new Phrase("D")); row5cell6.HorizontalAlignment = Element.ALIGN_LEFT;

        //PdfPCell row6cell1 = new PdfPCell(new Phrase("")); row6cell1.Colspan = 4;
        //PdfPCell row6cell2 = new PdfPCell(new Phrase("Times Absent")); row6cell2.Colspan = 2; row6cell2.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell row6cell3 = new PdfPCell(new Phrase("2")); row6cell3.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell row6cell4 = new PdfPCell(new Phrase("44.99-0")); row6cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
        //PdfPCell row6cell5 = new PdfPCell(new Phrase("F")); row6cell5.Colspan = 2; row6cell5.HorizontalAlignment = Element.ALIGN_LEFT;


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

        maintbl.AddCell(row5cell1);
        maintbl.AddCell(row5cell2);
        maintbl.AddCell(row5cell3);
        maintbl.AddCell(row5cell4);

        //maintbl.AddCell(row6cell1);
        //maintbl.AddCell(row6cell2);
        //maintbl.AddCell(row6cell3);
        //maintbl.AddCell(row6cell4);

        //document.Add(maintbl);
        //document.Add(new Phrase(Environment.NewLine));

        PdfPTable amaintbl = new PdfPTable(8);
        PdfPCell acell1 = new PdfPCell(new Phrase("END OF")); acell1.Colspan = 1; acell1.HorizontalAlignment = Element.ALIGN_LEFT; //cell1.Border = 0;
        PdfPCell acell2 = new PdfPCell(new Phrase(ddlAcademicTerm.SelectedItem.Text)); acell2.Colspan = 5; acell1.HorizontalAlignment = Element.ALIGN_LEFT; //cell2.Border = 0;
        PdfPCell acell3 = new PdfPCell(new Phrase("ENDING")); acell3.Colspan = 1; acell3.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell acell4 = new PdfPCell(new Phrase("...")); acell4.Colspan = 1; acell4.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;

        PdfPCell arow2cell1 = new PdfPCell(new Phrase("SESSION")); arow2cell1.HorizontalAlignment = Element.ALIGN_LEFT; arow2cell1.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell arow2cell2 = new PdfPCell(new Phrase(ddlAcademicSession.SelectedItem.Text)); arow2cell2.Colspan = 7; arow2cell2.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;

        PdfPCell arow3cell1 = new PdfPCell(new Phrase("NO OF TIMES SCHOOL OPENED")); arow3cell1.HorizontalAlignment = Element.ALIGN_LEFT; arow3cell1.Colspan = 4; //row3cell1.Border = 0;
        PdfPCell arow3cell2 = new PdfPCell(new Phrase("...")); arow3cell2.HorizontalAlignment = Element.ALIGN_LEFT; arow3cell2.Colspan = 4; //row3cell2.Border = 0;

        PdfPCell arow4cell1 = new PdfPCell(new Phrase("NO OF TIMES ABSENT")); arow4cell1.Colspan = 3; arow4cell1.HorizontalAlignment = Element.ALIGN_LEFT; //row4cell1.Border = 0;
        PdfPCell arow4cell2 = new PdfPCell(new Phrase("...")); arow4cell2.HorizontalAlignment = Element.ALIGN_LEFT; arow4cell2.Colspan = 5; //row4cell2.Border = 0;


        amaintbl.AddCell(acell1);
        amaintbl.AddCell(acell2);
        amaintbl.AddCell(acell3);
        amaintbl.AddCell(acell4);

        amaintbl.AddCell(arow2cell1);
        amaintbl.AddCell(arow2cell2);

        amaintbl.AddCell(arow3cell1);
        amaintbl.AddCell(arow3cell2);

        amaintbl.AddCell(arow4cell1);
        amaintbl.AddCell(arow4cell2);


        //document.Add(amaintbl);
        //PdfPTable outer = new PdfPTable(2);
        //outer.AddCell(maintbl);
        //outer.AddCell(amaintbl);
        //document.Add(outer);

        //document.Add(new Phrase(Environment.NewLine));



        ///PdfPTable resultTable = getResultTable(student);/// dont add the table yet so as to retrieve the gpa viewstate value needed in the gpa summ ary slot 
        /// 
        Int64 sessionId = Convert.ToInt64(ddlAcademicSession.SelectedIndex);
        Int64 TermId = Convert.ToInt64(ddlAcademicTerm.SelectedIndex);
        PdfPTable resTable = new PdfPTable(13);
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
        PdfPCell gradeHdr = new PdfPCell(); gradeHdr.Colspan = 1; gradeHdr.Rotation = 90; gradeHdr.HorizontalAlignment = Element.ALIGN_LEFT; gradeHdr.VerticalAlignment = Element.ALIGN_TOP; gradeHdr.AddElement(new Phrase("GRADE", resultTitleRedFnt8)); resTable.AddCell(gradeHdr);
        PdfPCell commentHdr = new PdfPCell(); commentHdr.Colspan = 2; commentHdr.Rotation = 90; commentHdr.HorizontalAlignment = Element.ALIGN_LEFT; commentHdr.VerticalAlignment = Element.ALIGN_TOP; commentHdr.AddElement(new Phrase("COMMENT", resultTitleRedFnt8)); resTable.AddCell(commentHdr);

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
                    deptName = dept.DepartmentName;
                    subjectCounter++;
                    subjName = d.Subject.Name;
                    testScore = Convert.ToDecimal(d.Test);
                    cw = Convert.ToDecimal(d.ClassWork);
                    hw = Convert.ToDecimal(d.HomeWorkProject);
                    note = Convert.ToDecimal(d.AttendanceNote);
                    //examScore = Convert.ToDecimal(d.ExamScore);
                    totalScore = cw + hw + note + testScore;
                    //scorePosition = Convert.ToDecimal(d.Position);

                    PdfPCell deptCell = new PdfPCell(new Phrase(subjName, darkerGrnFnt9)); deptCell.Colspan = 2;
                    resTable.AddCell(deptCell);
                    PdfPCell subjCell = new PdfPCell(new Phrase(subjName, darkerGrnFnt9)); subjCell.Colspan = 2;
                    resTable.AddCell(subjCell);
                    resTable.AddCell(cw.ToString());
                    resTable.AddCell(hw.ToString());
                    resTable.AddCell(note.ToString());
                    resTable.AddCell(totalScore.ToString());
                    resTable.AddCell(PASSIS.LIB.Utility.Utili.getExamGradeLetters(totalScore, (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)));
                    PdfPCell tRemark = new PdfPCell(new Phrase(PASSIS.LIB.Utility.Utili.getExamGradeRemarks(totalScore, (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)))); tRemark.Colspan = 2;
                    resTable.AddCell(tRemark);
                    //if (d.TermId == 3)
                    //{
                    //    var firstTermScore = getSubjectScorePosition2(student.AdmissionNumber, session, 1, schoolId, yearId, gradeIdd, (long)d.SubjectId);
                    //    if (firstTermScore == null)
                    //    {
                    //        resTable.AddCell(new Phrase("..."));
                    //    }
                    //    else
                    //    {
                    //        resTable.AddCell(new Phrase(firstTermScore.TotalScore.ToString()));
                    //    }
                    //    var secondTermScore = getSubjectScorePosition2(student.AdmissionNumber, session, 2, schoolId, yearId, gradeIdd, (long)d.SubjectId);
                    //    if (secondTermScore == null)
                    //    {
                    //        resTable.AddCell(new Phrase("..."));
                    //    }
                    //    else
                    //    {
                    //        resTable.AddCell(new Phrase(secondTermScore.TotalScore.ToString()));
                    //    }
                    //    var annualScore = getSubjectAnnualScore(student.AdmissionNumber, session, term, schoolId, yearId, gradeIdd, (long)d.SubjectId);
                    //    resTable.AddCell(new Phrase(annualScore.AverageScore.ToString()));
                    //    resTable.AddCell(new Phrase(annualScore.AveragePosition.ToString() + ToOrdinal(Convert.ToInt16(annualScore.AveragePosition))));
                    //    resTable.AddCell(PASSIS.LIB.Utility.Utili.getExamGradeLetters((long)annualScore.AverageScore, (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)));
                    //    PdfPCell tRemark = new PdfPCell(new Phrase(PASSIS.LIB.Utility.Utili.getExamGradeRemarks((long)annualScore.AverageScore, (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)))); tRemark.Colspan = 2;
                    //    resTable.AddCell(tRemark);
                    //    resTable.AddCell(new Phrase(""));
                    //    totalScoreAverage = Convert.ToDecimal(annualScore.AverageScore);
                    //    aggregateTotalScore += totalScoreAverage;

                    //}
                    //else
                    //{
                    //    resTable.AddCell(new Phrase("..."));
                    //    resTable.AddCell(new Phrase("..."));
                    //    resTable.AddCell(new Phrase("..."));
                    //    resTable.AddCell(scorePosition.ToString() + ToOrdinal(Convert.ToInt16(scorePosition)));

                    //    resTable.AddCell(PASSIS.LIB.Utility.Utili.getExamGradeLetters(totalScore, (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)));
                    //    PdfPCell tRemark = new PdfPCell(new Phrase(PASSIS.LIB.Utility.Utili.getExamGradeRemarks(totalScore, (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)))); tRemark.Colspan = 2;
                    //    resTable.AddCell(tRemark);
                    //    resTable.AddCell(new Phrase(""));
                    //    aggregateTotalScore += totalScore;
                    //}
                    //resTable.AddCell(new Phrase("..."));

                    //++noOfSubjectWithScore;
                    totalMarkObtainable = 0;
                    totalMarkObtained = 0;
                    testScore = 0;
                }
            }
        }

        //if (gs != null)
        //{
        //    yearGPASum_VS = 0m;
        //    int noOfSubjectWithScore = 0;
        //    //IList<PASSIS.LIB.Subject> getId = new SubjectTeachersLIB().getAllSubjectsInSchool(Convert.ToInt64(ddlYear.SelectedValue));
        //    IList<PASSIS.LIB.Subject> getId = new ScoresheetLIB().ReportCardSubject(Convert.ToInt64(ddlAcademicSession.SelectedValue), TermId, student.AdmissionNumber);
        //    foreach (PASSIS.LIB.Subject subj in getId)
        //    {
        //        subjectCounter++;
        //        try
        //        {
        //            PASSIS.LIB.StudentScore scor = null;
        //            //if (PASSIS.LIB.Utility.ConfigHelper.IsLive)
        //            scor = new ScoresheetLIB().RetrieveStudentScoreBulkUploadTest(Convert.ToInt64(subj.Id), student.AdmissionNumber, Convert.ToInt64(ddlAcademicSession.SelectedValue), TermId);// live version //sessionId
        //            //else
        //            //    scor = new ScoresheetLIB().RetrieveStudentScoreBulkUploadTest(subj.Id, student.AdmissionNumber);

        //            if (scor != null)
        //            {
        //                PdfPCell subjCell = new PdfPCell(new Phrase(subj.Name, darkerGrnFnt9)); subjCell.Colspan = 2;
        //                resTable.AddCell(subjCell);

        //                IList<StudentScoreRepository> scoreRepository = new ScoresheetLIB().getScorelist(Convert.ToInt64(ddlAcademicSession.SelectedValue), TermId, Convert.ToInt64(subj.Id), student.AdmissionNumber, logonUser.SchoolCampusId);
        //                foreach (StudentScoreRepository scoreRepo in scoreRepository)
        //                {
        //                    totalMarkObtained += Convert.ToInt32(scoreRepo.MarkObtained);
        //                    totalMarkObtainable += Convert.ToInt32(scoreRepo.MarkObtainable);
        //                }
        //                if (scoreRepository.Count > 0)
        //                {
        //                    ScoreConfiguration getScore = new ScoresheetLIB().getScoreConfiguration((long)logonUser.SchoolId, logonUser.SchoolCampusId);
        //                    testScoreConfiguration = Convert.ToDecimal(getScore.TestScore);
        //                    aggregateTestScore = Convert.ToDecimal(totalMarkObtained) / Convert.ToDecimal(totalMarkObtainable) * testScoreConfiguration;
        //                }
        //                resTable.AddCell(Math.Round(Convert.ToDecimal(aggregateTestScore), 0).ToString());
        //                decimal.TryParse(scor.ExamScore.ToString(), out examScore);
        //                totalScore = Math.Round(Convert.ToDecimal(aggregateTestScore) + examScore, 0);
        //                resTable.AddCell(examScore.ToString());
        //                resTable.AddCell(totalScore.ToString());
        //                resTable.AddCell(new Phrase("..."));
        //                resTable.AddCell(new Phrase("..."));
        //                resTable.AddCell(new Phrase("..."));
        //                resTable.AddCell(new Phrase("..."));
        //                //resTable.AddCell(PASSIS.LIB.Utility.Utili.GetSubjectPosition());
        //                resTable.AddCell(PASSIS.LIB.Utility.Utili.getExamGradeLetters(totalScore, (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)));
        //                PdfPCell tRemark = new PdfPCell(new Phrase(PASSIS.LIB.Utility.Utili.getExamGradeRemarks(totalScore, (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)))); tRemark.Colspan = 2;
        //                resTable.AddCell(tRemark);
        //                //resTable.AddCell(getExamGradeLetters(totalScore, (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)));
        //                //PdfPCell tRemark = new PdfPCell(new Phrase(getExamGradeRemarks(totalScore, (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)))); tRemark.Colspan = 2;
        //                //resTable.AddCell(tRemark);
        //                resTable.AddCell(new Phrase("..."));
        //                aggregateTotalScore += totalScore;
        //                ++noOfSubjectWithScore;
        //                totalMarkObtainable = 0;
        //                totalMarkObtained = 0;
        //                aggregateTestScore = 0;
        //            }
        //            else
        //            {
        //                PdfPCell emptyCell = new PdfPCell(); emptyCell.Colspan = 2;
        //                PdfPCell subjectCell = new PdfPCell(new Phrase(subj.Name, darkerGrnFnt9)); subjectCell.Colspan = 2;
        //                resTable.AddCell(subjectCell);
        //                resTable.AddCell("");
        //                resTable.AddCell("");
        //                resTable.AddCell("");
        //                resTable.AddCell("");
        //                resTable.AddCell("");
        //                resTable.AddCell("");
        //                resTable.AddCell("");
        //                resTable.AddCell("");
        //                resTable.AddCell(emptyCell);
        //                resTable.AddCell("");
        //            }
        //        }
        //        catch (Exception ex) { }

        //    }
        //}
        /*
         get the list of result of the student year level, iterate over          
         * 
         */
        //document.Add(resTable);
        //document.Add(new Phrase(Environment.NewLine));
        CheckBox box = new CheckBox();
        PdfPTable bmaintbl = new PdfPTable(8);
        PdfPCell bcell1 = new PdfPCell(new Phrase("BEHAVIOUR")); bcell1.BorderWidthBottom = 0; bcell1.Colspan = 3; bcell1.VerticalAlignment = Element.ALIGN_BOTTOM; bcell1.HorizontalAlignment = Element.ALIGN_BOTTOM; //cell1.Border = 0;
        PdfPCell bcell2 = new PdfPCell(new Phrase("5")); bcell2.Colspan = 1; bcell1.HorizontalAlignment = Element.ALIGN_LEFT; //cell2.Border = 0;
        PdfPCell bcell3 = new PdfPCell(new Phrase("4")); bcell3.Colspan = 1; bcell3.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell bcell4 = new PdfPCell(new Phrase("3")); bcell4.Colspan = 1; bcell4.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell bcell5 = new PdfPCell(new Phrase("2")); bcell5.Colspan = 1; bcell5.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell bcell6 = new PdfPCell(new Phrase("1")); bcell6.Colspan = 1; bcell6.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;

        PdfPCell brow2cell1 = new PdfPCell(new Phrase("")); brow2cell1.BorderWidthTop = 0; brow2cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow2cell1.Colspan = 3; //row2cell1.Border = 0;
        PdfPCell brow2cell2 = new PdfPCell(new Phrase("Excellent", resultTitleRedFnt8)); brow2cell2.Rotation = 90; brow2cell2.Colspan = 1; brow2cell2.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        PdfPCell brow2cell3 = new PdfPCell(new Phrase("Good", resultTitleRedFnt8)); brow2cell3.Rotation = 90; brow2cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow2cell3.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell brow2cell4 = new PdfPCell(new Phrase("Average", resultTitleRedFnt8)); brow2cell4.Rotation = 90; brow2cell4.Colspan = 1; brow2cell4.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        PdfPCell brow2cell5 = new PdfPCell(new Phrase("Fair", resultTitleRedFnt8)); brow2cell5.Rotation = 90; brow2cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow2cell5.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell brow2cell6 = new PdfPCell(new Phrase("Poor", resultTitleRedFnt8)); brow2cell6.Rotation = 90; brow2cell6.Colspan = 1; brow2cell6.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;

        PdfPCell brow3cell1 = new PdfPCell(new Phrase("1. Punctuality")); brow3cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow3cell1.Colspan = 3; //row3cell1.Border = 0;
        PdfPCell brow3cell2 = new PdfPCell(new Phrase("")); brow3cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow3cell2.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow3cell3 = new PdfPCell(new Phrase("")); brow3cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow3cell3.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow3cell4 = new PdfPCell(new Phrase("")); brow3cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow3cell4.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow3cell5 = new PdfPCell(new Phrase("")); brow3cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow3cell5.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow3cell6 = new PdfPCell(new Phrase("")); brow3cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow3cell6.Colspan = 1; //row3cell2.Border = 0;

        PdfPCell brow4cell1 = new PdfPCell(new Phrase("2. Neatness")); brow4cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow4cell1.Colspan = 3; //row3cell1.Border = 0;
        PdfPCell brow4cell2 = new PdfPCell(new Phrase("")); brow4cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow4cell2.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow4cell3 = new PdfPCell(new Phrase("")); brow4cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow4cell3.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow4cell4 = new PdfPCell(new Phrase("")); brow4cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow4cell4.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow4cell5 = new PdfPCell(new Phrase("")); brow4cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow4cell5.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow4cell6 = new PdfPCell(new Phrase("")); brow4cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow4cell6.Colspan = 1; //row3cell2.Border = 0;

        PdfPCell brow5cell1 = new PdfPCell(new Phrase("3. Relationship with Students")); brow5cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow5cell1.Colspan = 3; //row3cell1.Border = 0;
        PdfPCell brow5cell2 = new PdfPCell(new Phrase("")); brow5cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow5cell2.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow5cell3 = new PdfPCell(new Phrase("")); brow5cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow5cell3.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow5cell4 = new PdfPCell(new Phrase("")); brow5cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow5cell4.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow5cell5 = new PdfPCell(new Phrase("")); brow5cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow5cell5.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow5cell6 = new PdfPCell(new Phrase("")); brow5cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow5cell6.Colspan = 1; //row3cell2.Border = 0;

        PdfPCell brow6cell1 = new PdfPCell(new Phrase("4. Relationship with Staffs")); brow6cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow6cell1.Colspan = 3; //row3cell1.Border = 0;
        PdfPCell brow6cell2 = new PdfPCell(new Phrase("")); brow6cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow6cell2.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow6cell3 = new PdfPCell(new Phrase("")); brow6cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow6cell3.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow6cell4 = new PdfPCell(new Phrase("")); brow6cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow6cell4.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow6cell5 = new PdfPCell(new Phrase("")); brow6cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow6cell5.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow6cell6 = new PdfPCell(new Phrase("")); brow6cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow6cell6.Colspan = 1; //row3cell2.Border = 0;

        PdfPCell brow7cell1 = new PdfPCell(new Phrase("5. Leadership Traits")); brow7cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow7cell1.Colspan = 3; //row3cell1.Border = 0;
        PdfPCell brow7cell2 = new PdfPCell(new Phrase("")); brow7cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow7cell2.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow7cell3 = new PdfPCell(new Phrase("")); brow7cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow7cell3.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow7cell4 = new PdfPCell(new Phrase("")); brow7cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow7cell4.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow7cell5 = new PdfPCell(new Phrase("")); brow7cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow7cell5.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow7cell6 = new PdfPCell(new Phrase("")); brow7cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow7cell6.Colspan = 1; //row3cell2.Border = 0;

        PdfPCell brow8cell1 = new PdfPCell(new Phrase("6. Honesty")); brow8cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow8cell1.Colspan = 3; //row3cell1.Border = 0;
        PdfPCell brow8cell2 = new PdfPCell(new Phrase("")); brow8cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow8cell2.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow8cell3 = new PdfPCell(new Phrase("")); brow8cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow8cell3.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow8cell4 = new PdfPCell(new Phrase("")); brow8cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow8cell4.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow8cell5 = new PdfPCell(new Phrase("")); brow8cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow8cell5.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow8cell6 = new PdfPCell(new Phrase("")); brow8cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow8cell6.Colspan = 1; //row3cell2.Border = 0;

        PdfPCell brow9cell1 = new PdfPCell(new Phrase("7. Attitude to Work")); brow9cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow9cell1.Colspan = 3; //row3cell1.Border = 0;
        PdfPCell brow9cell2 = new PdfPCell(new Phrase("")); brow9cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow9cell2.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow9cell3 = new PdfPCell(new Phrase("")); brow9cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow9cell3.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow9cell4 = new PdfPCell(new Phrase("")); brow9cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow9cell4.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow9cell5 = new PdfPCell(new Phrase("")); brow9cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow9cell5.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow9cell6 = new PdfPCell(new Phrase("")); brow9cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow9cell6.Colspan = 1; //row3cell2.Border = 0;

        PdfPCell brow10cell1 = new PdfPCell(new Phrase("8. Politeness")); brow10cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow10cell1.Colspan = 3; //row3cell1.Border = 0;
        PdfPCell brow10cell2 = new PdfPCell(new Phrase("")); brow10cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow10cell2.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow10cell3 = new PdfPCell(new Phrase("")); brow10cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow10cell3.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow10cell4 = new PdfPCell(new Phrase("")); brow10cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow10cell4.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow10cell5 = new PdfPCell(new Phrase("")); brow10cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow10cell5.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow10cell6 = new PdfPCell(new Phrase("")); brow10cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow10cell6.Colspan = 1; //row3cell2.Border = 0;

        PdfPCell brow11cell1 = new PdfPCell(new Phrase("9. Initiative")); brow11cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow11cell1.Colspan = 3; //row3cell1.Border = 0;
        PdfPCell brow11cell2 = new PdfPCell(new Phrase("")); brow11cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow11cell2.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow11cell3 = new PdfPCell(new Phrase("")); brow11cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow11cell3.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow11cell4 = new PdfPCell(new Phrase("")); brow11cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow11cell4.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow11cell5 = new PdfPCell(new Phrase("")); brow11cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow11cell5.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow11cell6 = new PdfPCell(new Phrase("")); brow11cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow11cell6.Colspan = 1; //row3cell2.Border = 0;

        PdfPCell brow12cell1 = new PdfPCell(new Phrase("10. Self-Control")); brow12cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow12cell1.Colspan = 3; //row3cell1.Border = 0;
        PdfPCell brow12cell2 = new PdfPCell(new Phrase("")); brow12cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow12cell2.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow12cell3 = new PdfPCell(new Phrase("")); brow12cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow12cell3.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow12cell4 = new PdfPCell(new Phrase("")); brow12cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow12cell4.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow12cell5 = new PdfPCell(new Phrase("")); brow12cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow12cell5.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow12cell6 = new PdfPCell(new Phrase("")); brow12cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow12cell6.Colspan = 1; //row3cell2.Border = 0;

        PdfPCell brow13cell1 = new PdfPCell(new Phrase("11. Perseverance")); brow13cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow13cell1.Colspan = 3; //row3cell1.Border = 0;
        PdfPCell brow13cell2 = new PdfPCell(new Phrase("")); brow13cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow13cell2.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow13cell3 = new PdfPCell(new Phrase("")); brow13cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow13cell3.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow13cell4 = new PdfPCell(new Phrase("")); brow13cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow13cell4.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow13cell5 = new PdfPCell(new Phrase("")); brow13cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow13cell5.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow13cell6 = new PdfPCell(new Phrase("")); brow13cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow13cell6.Colspan = 1; //row3cell2.Border = 0;

        PdfPCell brow14cell1 = new PdfPCell(new Phrase("12. Attentiveness in Class")); brow14cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow14cell1.Colspan = 3; //row3cell1.Border = 0;
        PdfPCell brow14cell2 = new PdfPCell(new Phrase("")); brow14cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow14cell2.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow14cell3 = new PdfPCell(new Phrase("")); brow14cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow14cell3.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow14cell4 = new PdfPCell(new Phrase("")); brow14cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow14cell4.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow14cell5 = new PdfPCell(new Phrase("")); brow14cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow14cell5.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow14cell6 = new PdfPCell(new Phrase("")); brow14cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow14cell6.Colspan = 1; //row3cell2.Border = 0;

        PdfPCell brow15cell1 = new PdfPCell(new Phrase("13. Spirit of Co-operation")); brow15cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow15cell1.Colspan = 3; //row3cell1.Border = 0;
        PdfPCell brow15cell2 = new PdfPCell(new Phrase("")); brow15cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow15cell2.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow15cell3 = new PdfPCell(new Phrase("")); brow15cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow15cell3.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow15cell4 = new PdfPCell(new Phrase("")); brow15cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow15cell4.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow15cell5 = new PdfPCell(new Phrase("")); brow15cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow15cell5.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow15cell6 = new PdfPCell(new Phrase("")); brow15cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow15cell6.Colspan = 1; //row3cell2.Border = 0;

        //PdfPCell brow16cell1 = new PdfPCell(new Phrase("SKILLS", resultTitleRedFnt10)); brow16cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow16cell1.Colspan = 3; //row3cell1.Border = 0;
        //PdfPCell brow16cell2 = new PdfPCell(new Phrase("")); brow16cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow16cell2.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow16cell3 = new PdfPCell(new Phrase("")); brow16cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow16cell3.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow16cell4 = new PdfPCell(new Phrase("")); brow16cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow16cell4.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow16cell5 = new PdfPCell(new Phrase("")); brow16cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow16cell5.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow16cell6 = new PdfPCell(new Phrase("")); brow16cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow16cell6.Colspan = 1; //row3cell2.Border = 0;

        //PdfPCell brow17cell1 = new PdfPCell(new Phrase("1. Handling")); brow17cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow17cell1.Colspan = 3; //row3cell1.Border = 0;
        //PdfPCell brow17cell2 = new PdfPCell(new Phrase("")); brow17cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow17cell2.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow17cell3 = new PdfPCell(new Phrase("")); brow17cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow17cell3.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow17cell4 = new PdfPCell(new Phrase("")); brow17cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow17cell4.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow17cell5 = new PdfPCell(new Phrase("")); brow17cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow17cell5.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow17cell6 = new PdfPCell(new Phrase("")); brow17cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow17cell6.Colspan = 1; //row3cell2.Border = 0;

        //PdfPCell brow18cell1 = new PdfPCell(new Phrase("2. Handling Tools and Equipment")); brow18cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow18cell1.Colspan = 3; //row3cell1.Border = 0;
        //PdfPCell brow18cell2 = new PdfPCell(new Phrase("")); brow18cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow18cell2.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow18cell3 = new PdfPCell(new Phrase("")); brow18cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow18cell3.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow18cell4 = new PdfPCell(new Phrase("")); brow18cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow18cell4.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow18cell5 = new PdfPCell(new Phrase("")); brow18cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow18cell5.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow18cell6 = new PdfPCell(new Phrase("")); brow18cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow18cell6.Colspan = 1; //row3cell2.Border = 0;

        //PdfPCell brow19cell1 = new PdfPCell(new Phrase("3. Musical Skills")); brow19cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow19cell1.Colspan = 3; //row3cell1.Border = 0;
        //PdfPCell brow19cell2 = new PdfPCell(new Phrase("")); brow19cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow19cell2.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow19cell3 = new PdfPCell(new Phrase("")); brow19cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow19cell3.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow19cell4 = new PdfPCell(new Phrase("")); brow19cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow19cell4.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow19cell5 = new PdfPCell(new Phrase("")); brow19cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow19cell5.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow19cell6 = new PdfPCell(new Phrase("")); brow19cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow19cell6.Colspan = 1; //row3cell2.Border = 0;

        //PdfPCell brow20cell1 = new PdfPCell(new Phrase("4. Drawing & Painting")); brow20cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow20cell1.Colspan = 3; //row3cell1.Border = 0;
        //PdfPCell brow20cell2 = new PdfPCell(new Phrase("")); brow20cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow20cell2.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow20cell3 = new PdfPCell(new Phrase("")); brow20cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow20cell3.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow20cell4 = new PdfPCell(new Phrase("")); brow20cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow20cell4.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow20cell5 = new PdfPCell(new Phrase("")); brow20cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow20cell5.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow20cell6 = new PdfPCell(new Phrase("")); brow20cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow20cell6.Colspan = 1; //row3cell2.Border = 0;

        //PdfPCell brow21cell1 = new PdfPCell(new Phrase("5. Verbal Communication")); brow21cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow21cell1.Colspan = 3; //row3cell1.Border = 0;
        //PdfPCell brow21cell2 = new PdfPCell(new Phrase("")); brow21cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow21cell2.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow21cell3 = new PdfPCell(new Phrase("")); brow21cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow21cell3.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow21cell4 = new PdfPCell(new Phrase("")); brow21cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow21cell4.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow21cell5 = new PdfPCell(new Phrase("")); brow21cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow21cell5.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow21cell6 = new PdfPCell(new Phrase("")); brow21cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow21cell6.Colspan = 1; //row3cell2.Border = 0;

        //PdfPCell brow22cell1 = new PdfPCell(new Phrase("6. Sport Activities")); brow22cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow22cell1.Colspan = 3; //row3cell1.Border = 0;
        //PdfPCell brow22cell2 = new PdfPCell(new Phrase("")); brow22cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow22cell2.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow22cell3 = new PdfPCell(new Phrase("")); brow22cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow22cell3.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow22cell4 = new PdfPCell(new Phrase("")); brow22cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow22cell4.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow22cell5 = new PdfPCell(new Phrase("")); brow22cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow22cell5.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow22cell6 = new PdfPCell(new Phrase("")); brow22cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow22cell6.Colspan = 1; //row3cell2.Border = 0;

        bmaintbl.AddCell(bcell1);
        bmaintbl.AddCell(bcell2);
        bmaintbl.AddCell(bcell3);
        bmaintbl.AddCell(bcell4);
        bmaintbl.AddCell(bcell5);
        bmaintbl.AddCell(bcell6);

        bmaintbl.AddCell(brow2cell1);
        bmaintbl.AddCell(brow2cell2);
        bmaintbl.AddCell(brow2cell3);
        bmaintbl.AddCell(brow2cell4);
        bmaintbl.AddCell(brow2cell5);
        bmaintbl.AddCell(brow2cell6);

        bmaintbl.AddCell(brow3cell1);
        bmaintbl.AddCell(brow3cell2);
        bmaintbl.AddCell(brow3cell3);
        bmaintbl.AddCell(brow3cell4);
        bmaintbl.AddCell(brow3cell5);
        bmaintbl.AddCell(brow3cell6);

        bmaintbl.AddCell(brow4cell1);
        bmaintbl.AddCell(brow4cell2);
        bmaintbl.AddCell(brow4cell3);
        bmaintbl.AddCell(brow4cell4);
        bmaintbl.AddCell(brow4cell5);
        bmaintbl.AddCell(brow4cell6);

        bmaintbl.AddCell(brow5cell1);
        bmaintbl.AddCell(brow5cell2);
        bmaintbl.AddCell(brow5cell3);
        bmaintbl.AddCell(brow5cell4);
        bmaintbl.AddCell(brow5cell5);
        bmaintbl.AddCell(brow5cell6);

        bmaintbl.AddCell(brow6cell1);
        bmaintbl.AddCell(brow6cell2);
        bmaintbl.AddCell(brow6cell3);
        bmaintbl.AddCell(brow6cell4);
        bmaintbl.AddCell(brow6cell5);
        bmaintbl.AddCell(brow6cell6);

        bmaintbl.AddCell(brow7cell1);
        bmaintbl.AddCell(brow7cell2);
        bmaintbl.AddCell(brow7cell3);
        bmaintbl.AddCell(brow7cell4);
        bmaintbl.AddCell(brow7cell5);
        bmaintbl.AddCell(brow7cell6);

        bmaintbl.AddCell(brow8cell1);
        bmaintbl.AddCell(brow8cell2);
        bmaintbl.AddCell(brow8cell3);
        bmaintbl.AddCell(brow8cell4);
        bmaintbl.AddCell(brow8cell5);
        bmaintbl.AddCell(brow8cell6);

        bmaintbl.AddCell(brow9cell1);
        bmaintbl.AddCell(brow9cell2);
        bmaintbl.AddCell(brow9cell3);
        bmaintbl.AddCell(brow9cell4);
        bmaintbl.AddCell(brow9cell5);
        bmaintbl.AddCell(brow9cell6);

        bmaintbl.AddCell(brow10cell1);
        bmaintbl.AddCell(brow10cell2);
        bmaintbl.AddCell(brow10cell3);
        bmaintbl.AddCell(brow10cell4);
        bmaintbl.AddCell(brow10cell5);
        bmaintbl.AddCell(brow10cell6);

        bmaintbl.AddCell(brow11cell1);
        bmaintbl.AddCell(brow11cell2);
        bmaintbl.AddCell(brow11cell3);
        bmaintbl.AddCell(brow11cell4);
        bmaintbl.AddCell(brow11cell5);
        bmaintbl.AddCell(brow11cell6);

        bmaintbl.AddCell(brow12cell1);
        bmaintbl.AddCell(brow12cell2);
        bmaintbl.AddCell(brow12cell3);
        bmaintbl.AddCell(brow12cell4);
        bmaintbl.AddCell(brow12cell5);
        bmaintbl.AddCell(brow12cell6);

        bmaintbl.AddCell(brow13cell1);
        bmaintbl.AddCell(brow13cell2);
        bmaintbl.AddCell(brow13cell3);
        bmaintbl.AddCell(brow13cell4);
        bmaintbl.AddCell(brow13cell5);
        bmaintbl.AddCell(brow13cell6);

        bmaintbl.AddCell(brow14cell1);
        bmaintbl.AddCell(brow14cell2);
        bmaintbl.AddCell(brow14cell3);
        bmaintbl.AddCell(brow14cell4);
        bmaintbl.AddCell(brow14cell5);
        bmaintbl.AddCell(brow14cell6);

        bmaintbl.AddCell(brow15cell1);
        bmaintbl.AddCell(brow15cell2);
        bmaintbl.AddCell(brow15cell3);
        bmaintbl.AddCell(brow15cell4);
        bmaintbl.AddCell(brow15cell5);
        bmaintbl.AddCell(brow15cell6);

        //bmaintbl.AddCell(brow16cell1);
        //bmaintbl.AddCell(brow16cell2);
        //bmaintbl.AddCell(brow16cell3);
        //bmaintbl.AddCell(brow16cell4);
        //bmaintbl.AddCell(brow16cell5);
        //bmaintbl.AddCell(brow16cell6);

        //bmaintbl.AddCell(brow17cell1);
        //bmaintbl.AddCell(brow17cell2);
        //bmaintbl.AddCell(brow17cell3);
        //bmaintbl.AddCell(brow17cell4);
        //bmaintbl.AddCell(brow17cell5);
        //bmaintbl.AddCell(brow17cell6);

        //bmaintbl.AddCell(brow18cell1);
        //bmaintbl.AddCell(brow18cell2);
        //bmaintbl.AddCell(brow18cell3);
        //bmaintbl.AddCell(brow18cell4);
        //bmaintbl.AddCell(brow18cell5);
        //bmaintbl.AddCell(brow18cell6);

        //bmaintbl.AddCell(brow19cell1);
        //bmaintbl.AddCell(brow19cell2);
        //bmaintbl.AddCell(brow19cell3);
        //bmaintbl.AddCell(brow19cell4);
        //bmaintbl.AddCell(brow19cell5);
        //bmaintbl.AddCell(brow19cell6);

        //bmaintbl.AddCell(brow20cell1);
        //bmaintbl.AddCell(brow20cell2);
        //bmaintbl.AddCell(brow20cell3);
        //bmaintbl.AddCell(brow20cell4);
        //bmaintbl.AddCell(brow20cell5);
        //bmaintbl.AddCell(brow20cell6);

        //bmaintbl.AddCell(brow21cell1);
        //bmaintbl.AddCell(brow21cell2);
        //bmaintbl.AddCell(brow21cell3);
        //bmaintbl.AddCell(brow21cell4);
        //bmaintbl.AddCell(brow21cell5);
        //bmaintbl.AddCell(brow21cell6);

        //bmaintbl.AddCell(brow22cell1);
        //bmaintbl.AddCell(brow22cell2);
        //bmaintbl.AddCell(brow22cell3);
        //bmaintbl.AddCell(brow22cell4);
        //bmaintbl.AddCell(brow22cell5);
        //bmaintbl.AddCell(brow22cell6);


        //document.Add(bmaintbl);
        //PdfPTable outer = new PdfPTable(2);
        //outer.AddCell(maintbl);
        //outer.AddCell(amaintbl);
        //document.Add(outer);

        //document.Add(new Phrase(Environment.NewLine));
        if (subjectCounter != 0)
        {
            percentage = (aggregateTotalScore / (subjectCounter * 100)) * 100;
        }
        PdfPTable summaryTable = new PdfPTable(10);
        iTextSharp.text.pdf.draw.LineSeparator line = new iTextSharp.text.pdf.draw.LineSeparator(0f, 100f, BaseColor.BLACK, Element.ALIGN_LEFT, 1);
        PdfPCell Summarycell1 = new PdfPCell(new Phrase("Total Obtainable:")); Summarycell1.Colspan = 3; Summarycell1.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell1.Border = 0;
        PdfPCell Summarycell2 = new PdfPCell(new Phrase((subjectCounter * 100).ToString())); Summarycell2.Colspan = 1; Summarycell2.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell2.Border = 0;
        PdfPCell Summarycell3 = new PdfPCell(new Phrase("Total Obtained:")); Summarycell3.Colspan = 2; Summarycell3.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell3.Border = 0;
        PdfPCell Summarycell4 = new PdfPCell(new Phrase(Math.Round(aggregateTotalScore, 0).ToString())); Summarycell4.Colspan = 1; Summarycell4.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell4.Border = 0;
        PdfPCell Summarycell5 = new PdfPCell(new Phrase("Percentage(%):")); Summarycell5.Colspan = 2; Summarycell5.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell5.Border = 0;
        PdfPCell Summarycell6 = new PdfPCell(new Phrase(Math.Round(percentage, 0).ToString())); Summarycell6.Colspan = 1; Summarycell6.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell6.Border = 0;

        PdfPCell summaryRow2Cell1 = new PdfPCell(new Phrase("Class Teacher's Comment")); summaryRow2Cell1.Colspan = 4; summaryRow2Cell1.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow2Cell1.Border = 0;
        PdfPCell summaryRow2Cell2 = new PdfPCell(new Phrase("")); summaryRow2Cell2.Colspan = 6; summaryRow2Cell2.Border = 0;

        PdfPCell summaryRow3Cell1 = new PdfPCell(); summaryRow3Cell1.Colspan = 6; summaryRow3Cell1.AddElement(new Chunk(line)); summaryRow3Cell1.Border = 0;
        PdfPCell SummaryRow3cell2 = new PdfPCell(new Phrase("Signature:")); SummaryRow3cell2.Colspan = 2; SummaryRow3cell2.HorizontalAlignment = Element.ALIGN_LEFT; SummaryRow3cell2.Border = 0;
        PdfPCell SummaryRow3cell3 = new PdfPCell(); SummaryRow3cell3.Colspan = 2; SummaryRow3cell3.AddElement(new Chunk(line)); SummaryRow3cell3.HorizontalAlignment = Element.ALIGN_LEFT; SummaryRow3cell3.Border = 0;

        PdfPCell summaryRow4Cell1 = new PdfPCell(new Phrase("Principal's Comment")); summaryRow4Cell1.Colspan = 3; summaryRow4Cell1.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow4Cell1.Border = 0;
        PdfPCell summaryRow4Cell2 = new PdfPCell(new Phrase("")); summaryRow4Cell2.Colspan = 7; summaryRow4Cell2.Border = 0;

        PdfPCell summaryRow5Cell1 = new PdfPCell(); summaryRow5Cell1.Colspan = 6; summaryRow5Cell1.AddElement(new Chunk(line)); summaryRow5Cell1.Border = 0;
        PdfPCell SummaryRow5cell2 = new PdfPCell(new Phrase("Signature:")); SummaryRow5cell2.Colspan = 2; SummaryRow5cell2.HorizontalAlignment = Element.ALIGN_LEFT; SummaryRow5cell2.Border = 0;
        PdfPCell SummaryRow5cell3 = new PdfPCell(); SummaryRow5cell3.Colspan = 2; SummaryRow5cell3.AddElement(new Chunk(line)); SummaryRow5cell3.HorizontalAlignment = Element.ALIGN_LEFT; SummaryRow5cell3.Border = 0;

        summaryTable.AddCell(Summarycell1); summaryTable.AddCell(Summarycell2); summaryTable.AddCell(Summarycell3);
        summaryTable.AddCell(Summarycell4); summaryTable.AddCell(Summarycell5); summaryTable.AddCell(Summarycell6);
        summaryTable.AddCell(summaryRow2Cell1); summaryTable.AddCell(summaryRow2Cell2);
        summaryTable.AddCell(summaryRow3Cell1); summaryTable.AddCell(SummaryRow3cell2); summaryTable.AddCell(SummaryRow3cell3);
        summaryTable.AddCell(summaryRow4Cell1); summaryTable.AddCell(summaryRow4Cell2);
        summaryTable.AddCell(summaryRow5Cell1); summaryTable.AddCell(SummaryRow5cell2); summaryTable.AddCell(SummaryRow5cell3);
        // The row6cell2 was placed here because we need the calculation to present the total number of subject after counting the subject
        PdfPCell row6cell2 = new PdfPCell(new Phrase(subjectCounter.ToString())); row6cell2.HorizontalAlignment = Element.ALIGN_LEFT; row6cell2.Colspan = 1; //row6cell2.Border = 0;
        maintbl.AddCell(row6cell1);
        maintbl.AddCell(row6cell2);
        maintbl.AddCell(row6cell3);
        maintbl.AddCell(row6cell4);
        document.Add(maintbl);
        document.Add(domain);
        document.Add(amaintbl);
        document.Add(resTable);
        document.Add(bmaintbl);
        document.Add(summaryTable);
    }
    protected void addResultSummaryPage(Document document, PASSIS.LIB.User student, UsersLIB usrDal)
    {

        document.Add(getBackgroundImage());
        document.Add(new Phrase(Environment.NewLine));
        document.Add(new Phrase(Environment.NewLine));
        document.Add(new Phrase(Environment.NewLine));
        document.Add(new Phrase(Environment.NewLine));
        document.Add(new Phrase(Environment.NewLine));
        Paragraph schoolname = new Paragraph(string.Format("{0}", SchoolName), darkerRedFnt);
        schoolname.Alignment = Element.ALIGN_CENTER;
        document.Add(schoolname);
        Paragraph schoolAddress = new Paragraph(string.Format("{0}", SchoolAddress), darkerRedFnt);
        schoolAddress.Alignment = Element.ALIGN_CENTER;
        document.Add(schoolAddress);
        Paragraph p = new Paragraph("Report Card For", darkerRedFnt);
        p.Alignment = Element.ALIGN_CENTER;
        document.Add(p);
        Paragraph SessionDetails = new Paragraph(string.Format("{0} {1} {2} Session", ddlAcademicTerm.SelectedItem.Text, ",", ddlAcademicSession.SelectedItem.Text), darkerRedFnt);
        SessionDetails.Alignment = Element.ALIGN_CENTER;
        document.Add(SessionDetails);

        document.Add(new Phrase(Environment.NewLine));
        document.Add(new Phrase(Environment.NewLine));

        long session = Convert.ToInt64(ddlAcademicSession.SelectedValue);
        long term = Convert.ToInt64(ddlAcademicTerm.SelectedValue);
        long? schoolId = logonUser.SchoolId;
        long yearId = Convert.ToInt64(ddlYear.SelectedValue);
        long gradeIdd = Convert.ToInt64(ddlGrade.SelectedValue);

        long? schId = logonUser.SchoolId;
        long gradeId = new PrintResult().theGradeId(student.Id).GradeId;
        string noInClass = noOfStudentInClass(schId, gradeId).Count.ToString();
        string classPosition = "";
        var posInClass = getClassPosition(student.AdmissionNumber, term, session, schoolId, yearId, gradeIdd);
        if (posInClass != null)
        {
            classPosition = posInClass.Position.ToString();
        }


        PdfPTable maintbl = new PdfPTable(8);
        PdfPCell cell1 = new PdfPCell(new Phrase("NAME")); cell1.Colspan = 1; cell1.HorizontalAlignment = Element.ALIGN_LEFT; //cell1.Border = 0;
        PdfPCell cell2 = new PdfPCell(new Phrase(student.StudentFullName)); cell2.Colspan = 5; cell1.HorizontalAlignment = Element.ALIGN_LEFT; //cell2.Border = 0;
        PdfPCell cell3 = new PdfPCell(new Phrase("SEX")); cell3.Colspan = 1; cell3.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell cell4 = new PdfPCell(new Phrase(getGender(student.Gender))); cell4.Colspan = 1; cell4.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;

        PdfPCell row2cell1 = new PdfPCell(new Phrase("CLASS")); row2cell1.HorizontalAlignment = Element.ALIGN_LEFT; row2cell1.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell row2cell2 = new PdfPCell(new Phrase(usrDal.getStudentsHomeRoom(student.Id))); row2cell2.Colspan = 7; row2cell2.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;

        PdfPCell row3cell1 = new PdfPCell(new Phrase("DATE OF BIRTH")); row3cell1.HorizontalAlignment = Element.ALIGN_LEFT; row3cell1.Colspan = 2; //row3cell1.Border = 0;
        PdfPCell row3cell2 = new PdfPCell(new Phrase(student.DateOfBirth.ToString())); row3cell2.HorizontalAlignment = Element.ALIGN_LEFT; row3cell2.Colspan = 6; //row3cell2.Border = 0;

        PdfPCell row4cell1 = new PdfPCell(new Phrase("ADMISSION NUMBER")); row4cell1.Colspan = 3; row4cell1.HorizontalAlignment = Element.ALIGN_LEFT; //row4cell1.Border = 0;
        PdfPCell row4cell2 = new PdfPCell(new Phrase(student.AdmissionNumber)); row4cell2.HorizontalAlignment = Element.ALIGN_LEFT; row4cell2.Colspan = 5; //row4cell2.Border = 0;

        PdfPCell row5cell1 = new PdfPCell(new Phrase("NO IN CLASS")); row5cell1.HorizontalAlignment = Element.ALIGN_LEFT; row5cell1.Colspan = 3; //row5cell1.Border = 0;
        PdfPCell row5cell2 = new PdfPCell(new Phrase(noInClass)); row5cell2.HorizontalAlignment = Element.ALIGN_LEFT; row5cell2.Colspan = 1; //row5cell2.Border = 0;
        PdfPCell row5cell3 = new PdfPCell(new Phrase("POSITION IN CLASS")); row5cell3.HorizontalAlignment = Element.ALIGN_LEFT; row5cell3.Colspan = 3; //row5cell1.Border = 0;
        PdfPCell row5cell4 = new PdfPCell(new Phrase(classPosition + ToOrdinal(Convert.ToInt16(posInClass.Position)))); row5cell4.HorizontalAlignment = Element.ALIGN_LEFT; row5cell4.Colspan = 1; //row5cell2.Border = 0;

        PdfPCell row6cell1 = new PdfPCell(new Phrase("NO OF SUBJECT TAKEN")); row6cell1.Colspan = 3; row6cell1.HorizontalAlignment = Element.ALIGN_LEFT; //row6cell1.Border = 0;
        //PdfPCell row6cell2 = new PdfPCell(new Phrase(subjectCounter.ToString())); row6cell2.HorizontalAlignment = Element.ALIGN_LEFT; row6cell2.Colspan = 1; //row6cell2.Border = 0;
        PdfPCell row6cell3 = new PdfPCell(new Phrase("NO OF SUBJECT PASSED")); row6cell3.HorizontalAlignment = Element.ALIGN_LEFT; row6cell3.Colspan = 3; //row6cell3.Border = 0;
        PdfPCell row6cell4 = new PdfPCell(new Phrase("...")); row6cell4.HorizontalAlignment = Element.ALIGN_LEFT; row6cell4.Colspan = 1; //row6cell4.Border = 0;

        //PdfPCell row4cell1 = new PdfPCell(new Phrase("Name")); row4cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell row4cell2 = new PdfPCell(new Phrase(student.StudentFullName)); row4cell2.Colspan = 3; row4cell2.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell row4cell3 = new PdfPCell(new Phrase("Times Present")); row4cell3.Colspan = 2; row4cell3.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell row4cell4 = new PdfPCell(new Phrase("16")); row4cell4.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell row4cell5 = new PdfPCell(new Phrase("63.99-55")); row4cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
        //PdfPCell row4cell6 = new PdfPCell(new Phrase("C")); row4cell6.HorizontalAlignment = Element.ALIGN_LEFT;

        //PdfPCell row5cell1 = new PdfPCell(new Phrase("Adm. No")); row5cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell row5cell2 = new PdfPCell(new Phrase(student.AdmissionNumber)); row5cell2.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell row5cell3 = new PdfPCell(new Phrase("Gender")); row5cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
        //PdfPCell row5cell4 = new PdfPCell(new Phrase("Male")); row5cell4.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell row5cell5 = new PdfPCell(new Phrase("Times Puntual")); row5cell5.Colspan = 2; row5cell5.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell row5cell6 = new PdfPCell(new Phrase("16")); row5cell6.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell row5cell7 = new PdfPCell(new Phrase("54.99-45")); row5cell7.HorizontalAlignment = Element.ALIGN_RIGHT;
        //PdfPCell row5cell8 = new PdfPCell(new Phrase("D")); row5cell6.HorizontalAlignment = Element.ALIGN_LEFT;

        //PdfPCell row6cell1 = new PdfPCell(new Phrase("")); row6cell1.Colspan = 4;
        //PdfPCell row6cell2 = new PdfPCell(new Phrase("Times Absent")); row6cell2.Colspan = 2; row6cell2.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell row6cell3 = new PdfPCell(new Phrase("2")); row6cell3.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell row6cell4 = new PdfPCell(new Phrase("44.99-0")); row6cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
        //PdfPCell row6cell5 = new PdfPCell(new Phrase("F")); row6cell5.Colspan = 2; row6cell5.HorizontalAlignment = Element.ALIGN_LEFT;


        maintbl.AddCell(cell1);
        maintbl.AddCell(cell2);
        maintbl.AddCell(cell3);
        maintbl.AddCell(cell4);

        maintbl.AddCell(row2cell1);
        maintbl.AddCell(row2cell2);

        maintbl.AddCell(row3cell1);
        maintbl.AddCell(row3cell2);

        maintbl.AddCell(row4cell1);
        maintbl.AddCell(row4cell2);

        maintbl.AddCell(row5cell1);
        maintbl.AddCell(row5cell2);
        maintbl.AddCell(row5cell3);
        maintbl.AddCell(row5cell4);

        //maintbl.AddCell(row6cell1);
        //maintbl.AddCell(row6cell2);
        //maintbl.AddCell(row6cell3);
        //maintbl.AddCell(row6cell4);

        //document.Add(maintbl);
        //document.Add(new Phrase(Environment.NewLine));

        PdfPTable amaintbl = new PdfPTable(8);
        PdfPCell acell1 = new PdfPCell(new Phrase("END OF")); acell1.Colspan = 1; acell1.HorizontalAlignment = Element.ALIGN_LEFT; //cell1.Border = 0;
        PdfPCell acell2 = new PdfPCell(new Phrase(ddlAcademicTerm.SelectedItem.Text)); acell2.Colspan = 5; acell1.HorizontalAlignment = Element.ALIGN_LEFT; //cell2.Border = 0;
        PdfPCell acell3 = new PdfPCell(new Phrase("ENDING")); acell3.Colspan = 1; acell3.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell acell4 = new PdfPCell(new Phrase("...")); acell4.Colspan = 1; acell4.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;

        PdfPCell arow2cell1 = new PdfPCell(new Phrase("SESSION")); arow2cell1.HorizontalAlignment = Element.ALIGN_LEFT; arow2cell1.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell arow2cell2 = new PdfPCell(new Phrase(ddlAcademicSession.SelectedItem.Text)); arow2cell2.Colspan = 7; arow2cell2.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;

        PdfPCell arow3cell1 = new PdfPCell(new Phrase("NO OF TIMES SCHOOL OPENED")); arow3cell1.HorizontalAlignment = Element.ALIGN_LEFT; arow3cell1.Colspan = 4; //row3cell1.Border = 0;
        PdfPCell arow3cell2 = new PdfPCell(new Phrase("...")); arow3cell2.HorizontalAlignment = Element.ALIGN_LEFT; arow3cell2.Colspan = 4; //row3cell2.Border = 0;

        PdfPCell arow4cell1 = new PdfPCell(new Phrase("NO OF TIMES ABSENT")); arow4cell1.Colspan = 3; arow4cell1.HorizontalAlignment = Element.ALIGN_LEFT; //row4cell1.Border = 0;
        PdfPCell arow4cell2 = new PdfPCell(new Phrase("...")); arow4cell2.HorizontalAlignment = Element.ALIGN_LEFT; arow4cell2.Colspan = 5; //row4cell2.Border = 0;


        amaintbl.AddCell(acell1);
        amaintbl.AddCell(acell2);
        amaintbl.AddCell(acell3);
        amaintbl.AddCell(acell4);

        amaintbl.AddCell(arow2cell1);
        amaintbl.AddCell(arow2cell2);

        amaintbl.AddCell(arow3cell1);
        amaintbl.AddCell(arow3cell2);

        amaintbl.AddCell(arow4cell1);
        amaintbl.AddCell(arow4cell2);


        //document.Add(amaintbl);
        //PdfPTable outer = new PdfPTable(2);
        //outer.AddCell(maintbl);
        //outer.AddCell(amaintbl);
        //document.Add(outer);

        //document.Add(new Phrase(Environment.NewLine));



        ///PdfPTable resultTable = getResultTable(student);/// dont add the table yet so as to retrieve the gpa viewstate value needed in the gpa summ ary slot 
        /// 
        Int64 sessionId = Convert.ToInt64(ddlAcademicSession.SelectedIndex);
        Int64 TermId = Convert.ToInt64(ddlAcademicTerm.SelectedIndex);
        PdfPTable resTable = new PdfPTable(13);
        //resTable.SetWidthPercentage(new float[8] { 80f, 30f, 30f, 30f, 35f, 35f, 38f, 30f }, PageSize.A5.Rotate());
        //resTable.TotalWidth = 400f;
        //resTable.WidthPercentage = 80;
        //resTable.LockedWidth = true;
        PdfPCell subjectHdr = new PdfPCell(new Phrase("SUBJECTS", resultTitleRedFnt10)); subjectHdr.Padding = 0f;
        subjectHdr.Colspan = 2; subjectHdr.HorizontalAlignment = Element.ALIGN_CENTER; subjectHdr.VerticalAlignment = Element.ALIGN_BOTTOM;
        resTable.AddCell(subjectHdr);
        PdfPCell attHdr = new PdfPCell(new Phrase("C.A.", resultTitleRedFnt8)); attHdr.Colspan = 1; attHdr.Rotation = 90; attHdr.VerticalAlignment = Element.ALIGN_TOP; attHdr.HorizontalAlignment = Element.ALIGN_LEFT;
        resTable.AddCell(attHdr);
        PdfPCell examCel = new PdfPCell(); examCel.AddElement(new Phrase("EXAMINATION", resultTitleRedFnt8)); examCel.Colspan = 1; examCel.Rotation = 90; examCel.HorizontalAlignment = Element.ALIGN_LEFT; examCel.VerticalAlignment = Element.ALIGN_TOP;
        resTable.AddCell(examCel);
        PdfPCell totalCell = new PdfPCell(); totalCell.Colspan = 1; totalCell.Rotation = 90; totalCell.HorizontalAlignment = Element.ALIGN_LEFT; totalCell.VerticalAlignment = Element.ALIGN_TOP; totalCell.AddElement(new Phrase("TOTAL", resultTitleRedFnt8)); resTable.AddCell(totalCell);
        PdfPCell firstTermCell = new PdfPCell(); firstTermCell.Colspan = 1; firstTermCell.Rotation = 90; firstTermCell.HorizontalAlignment = Element.ALIGN_LEFT; firstTermCell.VerticalAlignment = Element.ALIGN_TOP; firstTermCell.AddElement(new Phrase("1ST TERM SCORE", resultTitleRedFnt8)); resTable.AddCell(firstTermCell);
        PdfPCell secondTermCell = new PdfPCell(); secondTermCell.Colspan = 1; secondTermCell.Rotation = 90; secondTermCell.HorizontalAlignment = Element.ALIGN_LEFT; secondTermCell.VerticalAlignment = Element.ALIGN_TOP; secondTermCell.AddElement(new Phrase("2ND TERM SCORE", resultTitleRedFnt8)); resTable.AddCell(secondTermCell);
        PdfPCell annualScoreCell = new PdfPCell(); annualScoreCell.Colspan = 1; annualScoreCell.Rotation = 90; annualScoreCell.HorizontalAlignment = Element.ALIGN_LEFT; annualScoreCell.VerticalAlignment = Element.ALIGN_TOP; annualScoreCell.AddElement(new Phrase("ANNUAL SCORE", resultTitleRedFnt8)); resTable.AddCell(annualScoreCell);
        PdfPCell positionCell = new PdfPCell(); positionCell.Colspan = 1; positionCell.Rotation = 90; positionCell.HorizontalAlignment = Element.ALIGN_LEFT; positionCell.VerticalAlignment = Element.ALIGN_TOP; positionCell.AddElement(new Phrase("POSITION", resultTitleRedFnt8)); resTable.AddCell(positionCell);
        PdfPCell gradeCell = new PdfPCell(); gradeCell.Colspan = 1; gradeCell.Rotation = 90; gradeCell.HorizontalAlignment = Element.ALIGN_LEFT; gradeCell.VerticalAlignment = Element.ALIGN_TOP; gradeCell.AddElement(new Phrase("GRADE", resultTitleRedFnt8)); resTable.AddCell(gradeCell);
        PdfPCell subjectTecherCell = new PdfPCell(); subjectTecherCell.Colspan = 2; subjectTecherCell.HorizontalAlignment = Element.ALIGN_CENTER; subjectTecherCell.VerticalAlignment = Element.ALIGN_BOTTOM; subjectTecherCell.AddElement(new Phrase("SUBJECT TEACHER'S REMARKS", resultTitleRedFnt8)); resTable.AddCell(subjectTecherCell);
        PdfPCell teacherSignatureCell = new PdfPCell(); teacherSignatureCell.Colspan = 1; teacherSignatureCell.Rotation = 90; teacherSignatureCell.HorizontalAlignment = Element.ALIGN_LEFT; teacherSignatureCell.VerticalAlignment = Element.ALIGN_TOP; teacherSignatureCell.AddElement(new Phrase("SIGNATURE", resultTitleRedFnt8)); resTable.AddCell(teacherSignatureCell);
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

        PASSIS.LIB.GradeStudent gs = new ClassGradeLIB().RetrieveStudentGrade(student.Id, session);

        //Int64 subjectCounter = 0;
        int totalMarkObtained = 0;
        int totalMarkObtainable = 0;
        decimal testScoreConfiguration = 0;
        decimal aggregateTestScore = 0;
        decimal examScore = 0m;
        decimal totalScore = 0;
        decimal totalScoreAverage = 0;
        decimal scorePosition = 0;
        decimal aggregateTotalScore = 0;
        string subjName = "";
        decimal percentage = 0m;
        long subjectId = 0;


        if (gs != null)
        {
            //IList<PASSIS.LIB.Subject> getSub = getSubject(session, term, schoolId, yearId, gradeIdd);
            //foreach (PASSIS.LIB.Subject sub in getSub) 
            //{
            //    subjectCounter++;
            //}

            IList<ReportCardData> rpCard = getSubjectScorePosition(student.AdmissionNumber, session, term, schoolId, yearId, gradeIdd);
            foreach (ReportCardData d in rpCard)
            {
                subjectCounter++;
                subjName = d.Subject.Name;
                aggregateTestScore = Convert.ToDecimal(d.TextScore);
                examScore = Convert.ToDecimal(d.ExamScore);
                totalScore = aggregateTestScore + examScore;
                scorePosition = Convert.ToDecimal(d.Position);

                PdfPCell subjCell = new PdfPCell(new Phrase(subjName, darkerGrnFnt9)); subjCell.Colspan = 2;
                resTable.AddCell(subjCell);
                resTable.AddCell(aggregateTestScore.ToString());
                resTable.AddCell(examScore.ToString());
                resTable.AddCell(totalScore.ToString());
                if (d.TermId == 3)
                {
                    var firstTermScore = getSubjectScorePosition2(student.AdmissionNumber, session, 1, schoolId, yearId, gradeIdd, (long)d.SubjectId);
                    if (firstTermScore == null)
                    {
                        resTable.AddCell(new Phrase("..."));
                    }
                    else
                    {
                        resTable.AddCell(new Phrase(firstTermScore.TotalScore.ToString()));
                    }
                    var secondTermScore = getSubjectScorePosition2(student.AdmissionNumber, session, 2, schoolId, yearId, gradeIdd, (long)d.SubjectId);
                    if (secondTermScore == null)
                    {
                        resTable.AddCell(new Phrase("..."));
                    }
                    else
                    {
                        resTable.AddCell(new Phrase(secondTermScore.TotalScore.ToString()));
                    }
                    var annualScore = getSubjectAnnualScore(student.AdmissionNumber, session, term, schoolId, yearId, gradeIdd, (long)d.SubjectId);
                    resTable.AddCell(new Phrase(annualScore.AverageScore.ToString()));
                    resTable.AddCell(new Phrase(annualScore.AveragePosition.ToString() + ToOrdinal(Convert.ToInt16(annualScore.AveragePosition))));
                    resTable.AddCell(PASSIS.LIB.Utility.Utili.getExamGradeLetters((long)annualScore.AverageScore, (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)));
                    PdfPCell tRemark = new PdfPCell(new Phrase(PASSIS.LIB.Utility.Utili.getExamGradeRemarks((long)annualScore.AverageScore, (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)))); tRemark.Colspan = 2;
                    resTable.AddCell(tRemark);
                    resTable.AddCell(new Phrase(""));
                    totalScoreAverage = Convert.ToDecimal(annualScore.AverageScore);
                    aggregateTotalScore += totalScoreAverage;

                }
                else
                {
                    resTable.AddCell(new Phrase("..."));
                    resTable.AddCell(new Phrase("..."));
                    resTable.AddCell(new Phrase("..."));
                    resTable.AddCell(scorePosition.ToString() + ToOrdinal(Convert.ToInt16(scorePosition)));

                    resTable.AddCell(PASSIS.LIB.Utility.Utili.getExamGradeLetters(totalScore, (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)));
                    PdfPCell tRemark = new PdfPCell(new Phrase(PASSIS.LIB.Utility.Utili.getExamGradeRemarks(totalScore, (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)))); tRemark.Colspan = 2;
                    resTable.AddCell(tRemark);
                    resTable.AddCell(new Phrase(""));
                    aggregateTotalScore += totalScore;
                }
                //resTable.AddCell(new Phrase("..."));

                //++noOfSubjectWithScore;
                totalMarkObtainable = 0;
                totalMarkObtained = 0;
                aggregateTestScore = 0;

            }
        }

        //if (gs != null)
        //{
        //    yearGPASum_VS = 0m;
        //    int noOfSubjectWithScore = 0;
        //    //IList<PASSIS.LIB.Subject> getId = new SubjectTeachersLIB().getAllSubjectsInSchool(Convert.ToInt64(ddlYear.SelectedValue));
        //    IList<PASSIS.LIB.Subject> getId = new ScoresheetLIB().ReportCardSubject(Convert.ToInt64(ddlAcademicSession.SelectedValue), TermId, student.AdmissionNumber);
        //    foreach (PASSIS.LIB.Subject subj in getId)
        //    {
        //        subjectCounter++;
        //        try
        //        {
        //            PASSIS.LIB.StudentScore scor = null;
        //            //if (PASSIS.LIB.Utility.ConfigHelper.IsLive)
        //            scor = new ScoresheetLIB().RetrieveStudentScoreBulkUploadTest(Convert.ToInt64(subj.Id), student.AdmissionNumber, Convert.ToInt64(ddlAcademicSession.SelectedValue), TermId);// live version //sessionId
        //            //else
        //            //    scor = new ScoresheetLIB().RetrieveStudentScoreBulkUploadTest(subj.Id, student.AdmissionNumber);

        //            if (scor != null)
        //            {
        //                PdfPCell subjCell = new PdfPCell(new Phrase(subj.Name, darkerGrnFnt9)); subjCell.Colspan = 2;
        //                resTable.AddCell(subjCell);

        //                IList<StudentScoreRepository> scoreRepository = new ScoresheetLIB().getScorelist(Convert.ToInt64(ddlAcademicSession.SelectedValue), TermId, Convert.ToInt64(subj.Id), student.AdmissionNumber, logonUser.SchoolCampusId);
        //                foreach (StudentScoreRepository scoreRepo in scoreRepository)
        //                {
        //                    totalMarkObtained += Convert.ToInt32(scoreRepo.MarkObtained);
        //                    totalMarkObtainable += Convert.ToInt32(scoreRepo.MarkObtainable);
        //                }
        //                if (scoreRepository.Count > 0)
        //                {
        //                    ScoreConfiguration getScore = new ScoresheetLIB().getScoreConfiguration((long)logonUser.SchoolId, logonUser.SchoolCampusId);
        //                    testScoreConfiguration = Convert.ToDecimal(getScore.TestScore);
        //                    aggregateTestScore = Convert.ToDecimal(totalMarkObtained) / Convert.ToDecimal(totalMarkObtainable) * testScoreConfiguration;
        //                }
        //                resTable.AddCell(Math.Round(Convert.ToDecimal(aggregateTestScore), 0).ToString());
        //                decimal.TryParse(scor.ExamScore.ToString(), out examScore);
        //                totalScore = Math.Round(Convert.ToDecimal(aggregateTestScore) + examScore, 0);
        //                resTable.AddCell(examScore.ToString());
        //                resTable.AddCell(totalScore.ToString());
        //                resTable.AddCell(new Phrase("..."));
        //                resTable.AddCell(new Phrase("..."));
        //                resTable.AddCell(new Phrase("..."));
        //                resTable.AddCell(new Phrase("..."));
        //                //resTable.AddCell(PASSIS.LIB.Utility.Utili.GetSubjectPosition());
        //                resTable.AddCell(PASSIS.LIB.Utility.Utili.getExamGradeLetters(totalScore, (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)));
        //                PdfPCell tRemark = new PdfPCell(new Phrase(PASSIS.LIB.Utility.Utili.getExamGradeRemarks(totalScore, (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)))); tRemark.Colspan = 2;
        //                resTable.AddCell(tRemark);
        //                //resTable.AddCell(getExamGradeLetters(totalScore, (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)));
        //                //PdfPCell tRemark = new PdfPCell(new Phrase(getExamGradeRemarks(totalScore, (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)))); tRemark.Colspan = 2;
        //                //resTable.AddCell(tRemark);
        //                resTable.AddCell(new Phrase("..."));
        //                aggregateTotalScore += totalScore;
        //                ++noOfSubjectWithScore;
        //                totalMarkObtainable = 0;
        //                totalMarkObtained = 0;
        //                aggregateTestScore = 0;
        //            }
        //            else
        //            {
        //                PdfPCell emptyCell = new PdfPCell(); emptyCell.Colspan = 2;
        //                PdfPCell subjectCell = new PdfPCell(new Phrase(subj.Name, darkerGrnFnt9)); subjectCell.Colspan = 2;
        //                resTable.AddCell(subjectCell);
        //                resTable.AddCell("");
        //                resTable.AddCell("");
        //                resTable.AddCell("");
        //                resTable.AddCell("");
        //                resTable.AddCell("");
        //                resTable.AddCell("");
        //                resTable.AddCell("");
        //                resTable.AddCell("");
        //                resTable.AddCell(emptyCell);
        //                resTable.AddCell("");
        //            }
        //        }
        //        catch (Exception ex) { }

        //    }
        //}
        /*
         get the list of result of the student year level, iterate over          
         * 
         */
        //document.Add(resTable);
        //document.Add(new Phrase(Environment.NewLine));
        CheckBox box = new CheckBox();
        PdfPTable bmaintbl = new PdfPTable(8);
        PdfPCell bcell1 = new PdfPCell(new Phrase("BEHAVIOUR")); bcell1.BorderWidthBottom = 0; bcell1.Colspan = 3; bcell1.VerticalAlignment = Element.ALIGN_BOTTOM; bcell1.HorizontalAlignment = Element.ALIGN_BOTTOM; //cell1.Border = 0;
        PdfPCell bcell2 = new PdfPCell(new Phrase("5")); bcell2.Colspan = 1; bcell1.HorizontalAlignment = Element.ALIGN_LEFT; //cell2.Border = 0;
        PdfPCell bcell3 = new PdfPCell(new Phrase("4")); bcell3.Colspan = 1; bcell3.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell bcell4 = new PdfPCell(new Phrase("3")); bcell4.Colspan = 1; bcell4.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell bcell5 = new PdfPCell(new Phrase("2")); bcell5.Colspan = 1; bcell5.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell bcell6 = new PdfPCell(new Phrase("1")); bcell6.Colspan = 1; bcell6.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;

        PdfPCell brow2cell1 = new PdfPCell(new Phrase("")); brow2cell1.BorderWidthTop = 0; brow2cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow2cell1.Colspan = 3; //row2cell1.Border = 0;
        PdfPCell brow2cell2 = new PdfPCell(new Phrase("Excellent", resultTitleRedFnt8)); brow2cell2.Rotation = 90; brow2cell2.Colspan = 1; brow2cell2.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        PdfPCell brow2cell3 = new PdfPCell(new Phrase("Good", resultTitleRedFnt8)); brow2cell3.Rotation = 90; brow2cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow2cell3.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell brow2cell4 = new PdfPCell(new Phrase("Average", resultTitleRedFnt8)); brow2cell4.Rotation = 90; brow2cell4.Colspan = 1; brow2cell4.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        PdfPCell brow2cell5 = new PdfPCell(new Phrase("Fair", resultTitleRedFnt8)); brow2cell5.Rotation = 90; brow2cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow2cell5.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell brow2cell6 = new PdfPCell(new Phrase("Poor", resultTitleRedFnt8)); brow2cell6.Rotation = 90; brow2cell6.Colspan = 1; brow2cell6.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;

        PdfPCell brow3cell1 = new PdfPCell(new Phrase("1. Punctuality")); brow3cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow3cell1.Colspan = 3; //row3cell1.Border = 0;
        PdfPCell brow3cell2 = new PdfPCell(new Phrase("")); brow3cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow3cell2.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow3cell3 = new PdfPCell(new Phrase("")); brow3cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow3cell3.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow3cell4 = new PdfPCell(new Phrase("")); brow3cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow3cell4.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow3cell5 = new PdfPCell(new Phrase("")); brow3cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow3cell5.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow3cell6 = new PdfPCell(new Phrase("")); brow3cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow3cell6.Colspan = 1; //row3cell2.Border = 0;

        PdfPCell brow4cell1 = new PdfPCell(new Phrase("2. Neatness")); brow4cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow4cell1.Colspan = 3; //row3cell1.Border = 0;
        PdfPCell brow4cell2 = new PdfPCell(new Phrase("")); brow4cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow4cell2.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow4cell3 = new PdfPCell(new Phrase("")); brow4cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow4cell3.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow4cell4 = new PdfPCell(new Phrase("")); brow4cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow4cell4.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow4cell5 = new PdfPCell(new Phrase("")); brow4cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow4cell5.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow4cell6 = new PdfPCell(new Phrase("")); brow4cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow4cell6.Colspan = 1; //row3cell2.Border = 0;

        PdfPCell brow5cell1 = new PdfPCell(new Phrase("3. Relationship with Students")); brow5cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow5cell1.Colspan = 3; //row3cell1.Border = 0;
        PdfPCell brow5cell2 = new PdfPCell(new Phrase("")); brow5cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow5cell2.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow5cell3 = new PdfPCell(new Phrase("")); brow5cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow5cell3.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow5cell4 = new PdfPCell(new Phrase("")); brow5cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow5cell4.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow5cell5 = new PdfPCell(new Phrase("")); brow5cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow5cell5.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow5cell6 = new PdfPCell(new Phrase("")); brow5cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow5cell6.Colspan = 1; //row3cell2.Border = 0;

        PdfPCell brow6cell1 = new PdfPCell(new Phrase("4. Relationship with Staffs")); brow6cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow6cell1.Colspan = 3; //row3cell1.Border = 0;
        PdfPCell brow6cell2 = new PdfPCell(new Phrase("")); brow6cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow6cell2.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow6cell3 = new PdfPCell(new Phrase("")); brow6cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow6cell3.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow6cell4 = new PdfPCell(new Phrase("")); brow6cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow6cell4.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow6cell5 = new PdfPCell(new Phrase("")); brow6cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow6cell5.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow6cell6 = new PdfPCell(new Phrase("")); brow6cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow6cell6.Colspan = 1; //row3cell2.Border = 0;

        PdfPCell brow7cell1 = new PdfPCell(new Phrase("5. Leadership Traits")); brow7cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow7cell1.Colspan = 3; //row3cell1.Border = 0;
        PdfPCell brow7cell2 = new PdfPCell(new Phrase("")); brow7cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow7cell2.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow7cell3 = new PdfPCell(new Phrase("")); brow7cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow7cell3.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow7cell4 = new PdfPCell(new Phrase("")); brow7cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow7cell4.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow7cell5 = new PdfPCell(new Phrase("")); brow7cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow7cell5.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow7cell6 = new PdfPCell(new Phrase("")); brow7cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow7cell6.Colspan = 1; //row3cell2.Border = 0;

        PdfPCell brow8cell1 = new PdfPCell(new Phrase("6. Honesty")); brow8cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow8cell1.Colspan = 3; //row3cell1.Border = 0;
        PdfPCell brow8cell2 = new PdfPCell(new Phrase("")); brow8cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow8cell2.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow8cell3 = new PdfPCell(new Phrase("")); brow8cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow8cell3.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow8cell4 = new PdfPCell(new Phrase("")); brow8cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow8cell4.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow8cell5 = new PdfPCell(new Phrase("")); brow8cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow8cell5.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow8cell6 = new PdfPCell(new Phrase("")); brow8cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow8cell6.Colspan = 1; //row3cell2.Border = 0;

        PdfPCell brow9cell1 = new PdfPCell(new Phrase("7. Attitude to Work")); brow9cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow9cell1.Colspan = 3; //row3cell1.Border = 0;
        PdfPCell brow9cell2 = new PdfPCell(new Phrase("")); brow9cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow9cell2.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow9cell3 = new PdfPCell(new Phrase("")); brow9cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow9cell3.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow9cell4 = new PdfPCell(new Phrase("")); brow9cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow9cell4.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow9cell5 = new PdfPCell(new Phrase("")); brow9cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow9cell5.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow9cell6 = new PdfPCell(new Phrase("")); brow9cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow9cell6.Colspan = 1; //row3cell2.Border = 0;

        PdfPCell brow10cell1 = new PdfPCell(new Phrase("8. Politeness")); brow10cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow10cell1.Colspan = 3; //row3cell1.Border = 0;
        PdfPCell brow10cell2 = new PdfPCell(new Phrase("")); brow10cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow10cell2.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow10cell3 = new PdfPCell(new Phrase("")); brow10cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow10cell3.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow10cell4 = new PdfPCell(new Phrase("")); brow10cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow10cell4.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow10cell5 = new PdfPCell(new Phrase("")); brow10cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow10cell5.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow10cell6 = new PdfPCell(new Phrase("")); brow10cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow10cell6.Colspan = 1; //row3cell2.Border = 0;

        PdfPCell brow11cell1 = new PdfPCell(new Phrase("9. Initiative")); brow11cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow11cell1.Colspan = 3; //row3cell1.Border = 0;
        PdfPCell brow11cell2 = new PdfPCell(new Phrase("")); brow11cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow11cell2.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow11cell3 = new PdfPCell(new Phrase("")); brow11cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow11cell3.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow11cell4 = new PdfPCell(new Phrase("")); brow11cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow11cell4.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow11cell5 = new PdfPCell(new Phrase("")); brow11cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow11cell5.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow11cell6 = new PdfPCell(new Phrase("")); brow11cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow11cell6.Colspan = 1; //row3cell2.Border = 0;

        PdfPCell brow12cell1 = new PdfPCell(new Phrase("10. Self-Control")); brow12cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow12cell1.Colspan = 3; //row3cell1.Border = 0;
        PdfPCell brow12cell2 = new PdfPCell(new Phrase("")); brow12cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow12cell2.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow12cell3 = new PdfPCell(new Phrase("")); brow12cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow12cell3.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow12cell4 = new PdfPCell(new Phrase("")); brow12cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow12cell4.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow12cell5 = new PdfPCell(new Phrase("")); brow12cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow12cell5.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow12cell6 = new PdfPCell(new Phrase("")); brow12cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow12cell6.Colspan = 1; //row3cell2.Border = 0;

        PdfPCell brow13cell1 = new PdfPCell(new Phrase("11. Perseverance")); brow13cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow13cell1.Colspan = 3; //row3cell1.Border = 0;
        PdfPCell brow13cell2 = new PdfPCell(new Phrase("")); brow13cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow13cell2.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow13cell3 = new PdfPCell(new Phrase("")); brow13cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow13cell3.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow13cell4 = new PdfPCell(new Phrase("")); brow13cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow13cell4.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow13cell5 = new PdfPCell(new Phrase("")); brow13cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow13cell5.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow13cell6 = new PdfPCell(new Phrase("")); brow13cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow13cell6.Colspan = 1; //row3cell2.Border = 0;

        PdfPCell brow14cell1 = new PdfPCell(new Phrase("12. Attentiveness in Class")); brow14cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow14cell1.Colspan = 3; //row3cell1.Border = 0;
        PdfPCell brow14cell2 = new PdfPCell(new Phrase("")); brow14cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow14cell2.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow14cell3 = new PdfPCell(new Phrase("")); brow14cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow14cell3.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow14cell4 = new PdfPCell(new Phrase("")); brow14cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow14cell4.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow14cell5 = new PdfPCell(new Phrase("")); brow14cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow14cell5.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow14cell6 = new PdfPCell(new Phrase("")); brow14cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow14cell6.Colspan = 1; //row3cell2.Border = 0;

        PdfPCell brow15cell1 = new PdfPCell(new Phrase("13. Spirit of Co-operation")); brow15cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow15cell1.Colspan = 3; //row3cell1.Border = 0;
        PdfPCell brow15cell2 = new PdfPCell(new Phrase("")); brow15cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow15cell2.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow15cell3 = new PdfPCell(new Phrase("")); brow15cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow15cell3.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow15cell4 = new PdfPCell(new Phrase("")); brow15cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow15cell4.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow15cell5 = new PdfPCell(new Phrase("")); brow15cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow15cell5.Colspan = 1; //row3cell2.Border = 0;
        PdfPCell brow15cell6 = new PdfPCell(new Phrase("")); brow15cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow15cell6.Colspan = 1; //row3cell2.Border = 0;

        //PdfPCell brow16cell1 = new PdfPCell(new Phrase("SKILLS", resultTitleRedFnt10)); brow16cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow16cell1.Colspan = 3; //row3cell1.Border = 0;
        //PdfPCell brow16cell2 = new PdfPCell(new Phrase("")); brow16cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow16cell2.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow16cell3 = new PdfPCell(new Phrase("")); brow16cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow16cell3.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow16cell4 = new PdfPCell(new Phrase("")); brow16cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow16cell4.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow16cell5 = new PdfPCell(new Phrase("")); brow16cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow16cell5.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow16cell6 = new PdfPCell(new Phrase("")); brow16cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow16cell6.Colspan = 1; //row3cell2.Border = 0;

        //PdfPCell brow17cell1 = new PdfPCell(new Phrase("1. Handling")); brow17cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow17cell1.Colspan = 3; //row3cell1.Border = 0;
        //PdfPCell brow17cell2 = new PdfPCell(new Phrase("")); brow17cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow17cell2.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow17cell3 = new PdfPCell(new Phrase("")); brow17cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow17cell3.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow17cell4 = new PdfPCell(new Phrase("")); brow17cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow17cell4.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow17cell5 = new PdfPCell(new Phrase("")); brow17cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow17cell5.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow17cell6 = new PdfPCell(new Phrase("")); brow17cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow17cell6.Colspan = 1; //row3cell2.Border = 0;

        //PdfPCell brow18cell1 = new PdfPCell(new Phrase("2. Handling Tools and Equipment")); brow18cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow18cell1.Colspan = 3; //row3cell1.Border = 0;
        //PdfPCell brow18cell2 = new PdfPCell(new Phrase("")); brow18cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow18cell2.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow18cell3 = new PdfPCell(new Phrase("")); brow18cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow18cell3.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow18cell4 = new PdfPCell(new Phrase("")); brow18cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow18cell4.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow18cell5 = new PdfPCell(new Phrase("")); brow18cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow18cell5.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow18cell6 = new PdfPCell(new Phrase("")); brow18cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow18cell6.Colspan = 1; //row3cell2.Border = 0;

        //PdfPCell brow19cell1 = new PdfPCell(new Phrase("3. Musical Skills")); brow19cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow19cell1.Colspan = 3; //row3cell1.Border = 0;
        //PdfPCell brow19cell2 = new PdfPCell(new Phrase("")); brow19cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow19cell2.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow19cell3 = new PdfPCell(new Phrase("")); brow19cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow19cell3.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow19cell4 = new PdfPCell(new Phrase("")); brow19cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow19cell4.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow19cell5 = new PdfPCell(new Phrase("")); brow19cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow19cell5.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow19cell6 = new PdfPCell(new Phrase("")); brow19cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow19cell6.Colspan = 1; //row3cell2.Border = 0;

        //PdfPCell brow20cell1 = new PdfPCell(new Phrase("4. Drawing & Painting")); brow20cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow20cell1.Colspan = 3; //row3cell1.Border = 0;
        //PdfPCell brow20cell2 = new PdfPCell(new Phrase("")); brow20cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow20cell2.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow20cell3 = new PdfPCell(new Phrase("")); brow20cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow20cell3.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow20cell4 = new PdfPCell(new Phrase("")); brow20cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow20cell4.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow20cell5 = new PdfPCell(new Phrase("")); brow20cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow20cell5.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow20cell6 = new PdfPCell(new Phrase("")); brow20cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow20cell6.Colspan = 1; //row3cell2.Border = 0;

        //PdfPCell brow21cell1 = new PdfPCell(new Phrase("5. Verbal Communication")); brow21cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow21cell1.Colspan = 3; //row3cell1.Border = 0;
        //PdfPCell brow21cell2 = new PdfPCell(new Phrase("")); brow21cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow21cell2.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow21cell3 = new PdfPCell(new Phrase("")); brow21cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow21cell3.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow21cell4 = new PdfPCell(new Phrase("")); brow21cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow21cell4.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow21cell5 = new PdfPCell(new Phrase("")); brow21cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow21cell5.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow21cell6 = new PdfPCell(new Phrase("")); brow21cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow21cell6.Colspan = 1; //row3cell2.Border = 0;

        //PdfPCell brow22cell1 = new PdfPCell(new Phrase("6. Sport Activities")); brow22cell1.HorizontalAlignment = Element.ALIGN_LEFT; brow22cell1.Colspan = 3; //row3cell1.Border = 0;
        //PdfPCell brow22cell2 = new PdfPCell(new Phrase("")); brow22cell2.HorizontalAlignment = Element.ALIGN_LEFT; brow22cell2.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow22cell3 = new PdfPCell(new Phrase("")); brow22cell3.HorizontalAlignment = Element.ALIGN_LEFT; brow22cell3.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow22cell4 = new PdfPCell(new Phrase("")); brow22cell4.HorizontalAlignment = Element.ALIGN_LEFT; brow22cell4.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow22cell5 = new PdfPCell(new Phrase("")); brow22cell5.HorizontalAlignment = Element.ALIGN_LEFT; brow22cell5.Colspan = 1; //row3cell2.Border = 0;
        //PdfPCell brow22cell6 = new PdfPCell(new Phrase("")); brow22cell6.HorizontalAlignment = Element.ALIGN_LEFT; brow22cell6.Colspan = 1; //row3cell2.Border = 0;

        bmaintbl.AddCell(bcell1);
        bmaintbl.AddCell(bcell2);
        bmaintbl.AddCell(bcell3);
        bmaintbl.AddCell(bcell4);
        bmaintbl.AddCell(bcell5);
        bmaintbl.AddCell(bcell6);

        bmaintbl.AddCell(brow2cell1);
        bmaintbl.AddCell(brow2cell2);
        bmaintbl.AddCell(brow2cell3);
        bmaintbl.AddCell(brow2cell4);
        bmaintbl.AddCell(brow2cell5);
        bmaintbl.AddCell(brow2cell6);

        bmaintbl.AddCell(brow3cell1);
        bmaintbl.AddCell(brow3cell2);
        bmaintbl.AddCell(brow3cell3);
        bmaintbl.AddCell(brow3cell4);
        bmaintbl.AddCell(brow3cell5);
        bmaintbl.AddCell(brow3cell6);

        bmaintbl.AddCell(brow4cell1);
        bmaintbl.AddCell(brow4cell2);
        bmaintbl.AddCell(brow4cell3);
        bmaintbl.AddCell(brow4cell4);
        bmaintbl.AddCell(brow4cell5);
        bmaintbl.AddCell(brow4cell6);

        bmaintbl.AddCell(brow5cell1);
        bmaintbl.AddCell(brow5cell2);
        bmaintbl.AddCell(brow5cell3);
        bmaintbl.AddCell(brow5cell4);
        bmaintbl.AddCell(brow5cell5);
        bmaintbl.AddCell(brow5cell6);

        bmaintbl.AddCell(brow6cell1);
        bmaintbl.AddCell(brow6cell2);
        bmaintbl.AddCell(brow6cell3);
        bmaintbl.AddCell(brow6cell4);
        bmaintbl.AddCell(brow6cell5);
        bmaintbl.AddCell(brow6cell6);

        bmaintbl.AddCell(brow7cell1);
        bmaintbl.AddCell(brow7cell2);
        bmaintbl.AddCell(brow7cell3);
        bmaintbl.AddCell(brow7cell4);
        bmaintbl.AddCell(brow7cell5);
        bmaintbl.AddCell(brow7cell6);

        bmaintbl.AddCell(brow8cell1);
        bmaintbl.AddCell(brow8cell2);
        bmaintbl.AddCell(brow8cell3);
        bmaintbl.AddCell(brow8cell4);
        bmaintbl.AddCell(brow8cell5);
        bmaintbl.AddCell(brow8cell6);

        bmaintbl.AddCell(brow9cell1);
        bmaintbl.AddCell(brow9cell2);
        bmaintbl.AddCell(brow9cell3);
        bmaintbl.AddCell(brow9cell4);
        bmaintbl.AddCell(brow9cell5);
        bmaintbl.AddCell(brow9cell6);

        bmaintbl.AddCell(brow10cell1);
        bmaintbl.AddCell(brow10cell2);
        bmaintbl.AddCell(brow10cell3);
        bmaintbl.AddCell(brow10cell4);
        bmaintbl.AddCell(brow10cell5);
        bmaintbl.AddCell(brow10cell6);

        bmaintbl.AddCell(brow11cell1);
        bmaintbl.AddCell(brow11cell2);
        bmaintbl.AddCell(brow11cell3);
        bmaintbl.AddCell(brow11cell4);
        bmaintbl.AddCell(brow11cell5);
        bmaintbl.AddCell(brow11cell6);

        bmaintbl.AddCell(brow12cell1);
        bmaintbl.AddCell(brow12cell2);
        bmaintbl.AddCell(brow12cell3);
        bmaintbl.AddCell(brow12cell4);
        bmaintbl.AddCell(brow12cell5);
        bmaintbl.AddCell(brow12cell6);

        bmaintbl.AddCell(brow13cell1);
        bmaintbl.AddCell(brow13cell2);
        bmaintbl.AddCell(brow13cell3);
        bmaintbl.AddCell(brow13cell4);
        bmaintbl.AddCell(brow13cell5);
        bmaintbl.AddCell(brow13cell6);

        bmaintbl.AddCell(brow14cell1);
        bmaintbl.AddCell(brow14cell2);
        bmaintbl.AddCell(brow14cell3);
        bmaintbl.AddCell(brow14cell4);
        bmaintbl.AddCell(brow14cell5);
        bmaintbl.AddCell(brow14cell6);

        bmaintbl.AddCell(brow15cell1);
        bmaintbl.AddCell(brow15cell2);
        bmaintbl.AddCell(brow15cell3);
        bmaintbl.AddCell(brow15cell4);
        bmaintbl.AddCell(brow15cell5);
        bmaintbl.AddCell(brow15cell6);

        //bmaintbl.AddCell(brow16cell1);
        //bmaintbl.AddCell(brow16cell2);
        //bmaintbl.AddCell(brow16cell3);
        //bmaintbl.AddCell(brow16cell4);
        //bmaintbl.AddCell(brow16cell5);
        //bmaintbl.AddCell(brow16cell6);

        //bmaintbl.AddCell(brow17cell1);
        //bmaintbl.AddCell(brow17cell2);
        //bmaintbl.AddCell(brow17cell3);
        //bmaintbl.AddCell(brow17cell4);
        //bmaintbl.AddCell(brow17cell5);
        //bmaintbl.AddCell(brow17cell6);

        //bmaintbl.AddCell(brow18cell1);
        //bmaintbl.AddCell(brow18cell2);
        //bmaintbl.AddCell(brow18cell3);
        //bmaintbl.AddCell(brow18cell4);
        //bmaintbl.AddCell(brow18cell5);
        //bmaintbl.AddCell(brow18cell6);

        //bmaintbl.AddCell(brow19cell1);
        //bmaintbl.AddCell(brow19cell2);
        //bmaintbl.AddCell(brow19cell3);
        //bmaintbl.AddCell(brow19cell4);
        //bmaintbl.AddCell(brow19cell5);
        //bmaintbl.AddCell(brow19cell6);

        //bmaintbl.AddCell(brow20cell1);
        //bmaintbl.AddCell(brow20cell2);
        //bmaintbl.AddCell(brow20cell3);
        //bmaintbl.AddCell(brow20cell4);
        //bmaintbl.AddCell(brow20cell5);
        //bmaintbl.AddCell(brow20cell6);

        //bmaintbl.AddCell(brow21cell1);
        //bmaintbl.AddCell(brow21cell2);
        //bmaintbl.AddCell(brow21cell3);
        //bmaintbl.AddCell(brow21cell4);
        //bmaintbl.AddCell(brow21cell5);
        //bmaintbl.AddCell(brow21cell6);

        //bmaintbl.AddCell(brow22cell1);
        //bmaintbl.AddCell(brow22cell2);
        //bmaintbl.AddCell(brow22cell3);
        //bmaintbl.AddCell(brow22cell4);
        //bmaintbl.AddCell(brow22cell5);
        //bmaintbl.AddCell(brow22cell6);


        //document.Add(bmaintbl);
        //PdfPTable outer = new PdfPTable(2);
        //outer.AddCell(maintbl);
        //outer.AddCell(amaintbl);
        //document.Add(outer);

        //document.Add(new Phrase(Environment.NewLine));
        if (subjectCounter != 0)
        {
            percentage = (aggregateTotalScore / (subjectCounter * 100)) * 100;
        }
        PdfPTable summaryTable = new PdfPTable(10);
        iTextSharp.text.pdf.draw.LineSeparator line = new iTextSharp.text.pdf.draw.LineSeparator(0f, 100f, BaseColor.BLACK, Element.ALIGN_LEFT, 1);
        PdfPCell Summarycell1 = new PdfPCell(new Phrase("Total Obtainable:")); Summarycell1.Colspan = 3; Summarycell1.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell1.Border = 0;
        PdfPCell Summarycell2 = new PdfPCell(new Phrase((subjectCounter * 100).ToString())); Summarycell2.Colspan = 1; Summarycell2.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell2.Border = 0;
        PdfPCell Summarycell3 = new PdfPCell(new Phrase("Total Obtained:")); Summarycell3.Colspan = 2; Summarycell3.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell3.Border = 0;
        PdfPCell Summarycell4 = new PdfPCell(new Phrase(Math.Round(aggregateTotalScore, 0).ToString())); Summarycell4.Colspan = 1; Summarycell4.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell4.Border = 0;
        PdfPCell Summarycell5 = new PdfPCell(new Phrase("Percentage(%):")); Summarycell5.Colspan = 2; Summarycell5.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell5.Border = 0;
        PdfPCell Summarycell6 = new PdfPCell(new Phrase(Math.Round(percentage, 0).ToString())); Summarycell6.Colspan = 1; Summarycell6.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell6.Border = 0;

        PdfPCell summaryRow2Cell1 = new PdfPCell(new Phrase("Class Teacher's Comment")); summaryRow2Cell1.Colspan = 4; summaryRow2Cell1.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow2Cell1.Border = 0;
        PdfPCell summaryRow2Cell2 = new PdfPCell(new Phrase("")); summaryRow2Cell2.Colspan = 6; summaryRow2Cell2.Border = 0;

        PdfPCell summaryRow3Cell1 = new PdfPCell(); summaryRow3Cell1.Colspan = 6; summaryRow3Cell1.AddElement(new Chunk(line)); summaryRow3Cell1.Border = 0;
        PdfPCell SummaryRow3cell2 = new PdfPCell(new Phrase("Signature:")); SummaryRow3cell2.Colspan = 2; SummaryRow3cell2.HorizontalAlignment = Element.ALIGN_LEFT; SummaryRow3cell2.Border = 0;
        PdfPCell SummaryRow3cell3 = new PdfPCell(); SummaryRow3cell3.Colspan = 2; SummaryRow3cell3.AddElement(new Chunk(line)); SummaryRow3cell3.HorizontalAlignment = Element.ALIGN_LEFT; SummaryRow3cell3.Border = 0;

        PdfPCell summaryRow4Cell1 = new PdfPCell(new Phrase("Principal's Comment")); summaryRow4Cell1.Colspan = 3; summaryRow4Cell1.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow4Cell1.Border = 0;
        PdfPCell summaryRow4Cell2 = new PdfPCell(new Phrase("")); summaryRow4Cell2.Colspan = 7; summaryRow4Cell2.Border = 0;

        PdfPCell summaryRow5Cell1 = new PdfPCell(); summaryRow5Cell1.Colspan = 6; summaryRow5Cell1.AddElement(new Chunk(line)); summaryRow5Cell1.Border = 0;
        PdfPCell SummaryRow5cell2 = new PdfPCell(new Phrase("Signature:")); SummaryRow5cell2.Colspan = 2; SummaryRow5cell2.HorizontalAlignment = Element.ALIGN_LEFT; SummaryRow5cell2.Border = 0;
        PdfPCell SummaryRow5cell3 = new PdfPCell(); SummaryRow5cell3.Colspan = 2; SummaryRow5cell3.AddElement(new Chunk(line)); SummaryRow5cell3.HorizontalAlignment = Element.ALIGN_LEFT; SummaryRow5cell3.Border = 0;

        summaryTable.AddCell(Summarycell1); summaryTable.AddCell(Summarycell2); summaryTable.AddCell(Summarycell3);
        summaryTable.AddCell(Summarycell4); summaryTable.AddCell(Summarycell5); summaryTable.AddCell(Summarycell6);
        summaryTable.AddCell(summaryRow2Cell1); summaryTable.AddCell(summaryRow2Cell2);
        summaryTable.AddCell(summaryRow3Cell1); summaryTable.AddCell(SummaryRow3cell2); summaryTable.AddCell(SummaryRow3cell3);
        summaryTable.AddCell(summaryRow4Cell1); summaryTable.AddCell(summaryRow4Cell2);
        summaryTable.AddCell(summaryRow5Cell1); summaryTable.AddCell(SummaryRow5cell2); summaryTable.AddCell(SummaryRow5cell3);
        // The row6cell2 was placed here because we need the calculation to present the total number of subject after counting the subject
        PdfPCell row6cell2 = new PdfPCell(new Phrase(subjectCounter.ToString())); row6cell2.HorizontalAlignment = Element.ALIGN_LEFT; row6cell2.Colspan = 1; //row6cell2.Border = 0;
        maintbl.AddCell(row6cell1);
        maintbl.AddCell(row6cell2);
        maintbl.AddCell(row6cell3);
        maintbl.AddCell(row6cell4);
        document.Add(maintbl);
        document.Add(amaintbl);
        document.Add(resTable);
        document.Add(bmaintbl);
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
    public PASSIS.LIB.GradeStudent theGradeId(Int64 studentId)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        PASSIS.LIB.User user = new PASSIS.LIB.User();
        PASSIS.LIB.GradeStudent gradeId = context.GradeStudents.SingleOrDefault(x => x.StudentId == studentId);
        return gradeId;
    }

    protected void ddlAcademicTerm_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void ddlGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlAcademicSession.SelectedIndex == 0)
        //{
        //  lblErrorMsg.Text = "Kindly select academic session";
        //  lblErrorMsg.ForeColor = System.Drawing.Color.Red;
        // lblErrorMsg.Visible = true;
        // return;
        //}

        if (ddlAcademicTerm.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select term";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }


        BindGrid(Convert.ToInt64(ddlYear.SelectedValue), Convert.ToInt64(ddlGrade.SelectedValue));
        btnApprove.Visible = true;
        btnPrintAll.Visible = true;


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

}