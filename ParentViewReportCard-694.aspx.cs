using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB.Utility;
using PASSIS.LIB;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using System.Data.SqlClient;
using PASSIS.LIB;

public partial class ParentViewReportCard_694 : PASSIS.LIB.Utility.BasePage
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
    Int64 subjectPassedCounter = 0;
    public static string SchoolName = "";
    public static string SchoolLogo = "";
    public static string Passport = "";
    public static string SchoolSignature = "";
    public static string SchoolAddress = "";
    public static string SchoolCurriculumId = "";
    public static string SchoolUrl = "";
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        //mainview.Visible = true;
        if (!IsPostBack)
        {

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
                SchoolCurriculumId = reader[8].ToString();
                SchoolSignature = reader[6].ToString();
                SchoolUrl = reader[7].ToString();
            }
            if (SchoolLogo == "") SchoolLogo = "~/Images/Schools_Logo/passis_logoB.png";
            if (SchoolSignature == "") SchoolSignature = "~/Images/SchoolSignature/Signature.PNG";
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

            ddlAcademicSession.DataSource = new ParentViewReportCard_694().schSession().Distinct();
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
            //if (curriculumId == (long)CurriculumType.British)
            //{
            //    ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, 0);
            //}
            //else if (curriculumId == (long)CurriculumType.Nigerian)
            //{
            //    ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
            //}
            //ddlYear.DataBind();

            //ddlAcademicTerm.DataSource = new AcademicTermLIB().RetrieveAcademicTerm();
            //ddlAcademicTerm.DataBind();



            var academicTermCurrent = (from c in context.AcademicSessions
                                       where c.IsCurrent == true
                                       select new
                                       {


                                           // ACInstitutions  = GetInstitution(c.InstitutionName)
                                           c.AcademicTerm1

                                       }).Distinct();


            ddlAcademicTerm.DataSource = new ParentViewReportCard_694().schTerm().Distinct();
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
            BindDropDown();

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
        //Int64 selectedYearId = 0L;
        //selectedYearId = Convert.ToInt64(ddlYear.SelectedValue);
        //ddlGrade.Items.Clear();
        //var availableGrades = new ClassGradeDAL().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, selectedYearId);
        //ddlGrade.DataSource = availableGrades;
        //ddlGrade.DataBind();
        //ddlGrade.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Grade--", "0", true));
    }
    //protected void BindGrid(long yearId, long gradeId)
    //{
    //    long campusId, schoolId;
    //    campusId = logonUser.SchoolCampusId;
    //    schoolId = (long)logonUser.SchoolId;
    //    var scoreList = new ClassGradeLIB().RetrieveGradeStudents(schoolId, campusId, yearId, gradeId);
    //    gdvList.DataSource = scoreList;
    //    gdvList.DataBind();
    //    //RePopulateCheckBoxes();
    //}
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
    //private void RePopulateCheckBoxes()
    //{
    //    foreach (GridViewRow row in gdvList.Rows)
    //    {
    //        var chkBox = row.FindControl("chkSelect") as CheckBox;

    //        IDataItemContainer container = (IDataItemContainer)chkBox.NamingContainer;

    //        if (SelectedCustomersIndex != null)
    //        {
    //            if (SelectedCustomersIndex.Exists(i => i == container.DataItemIndex))
    //            {
    //                chkBox.Checked = true;
    //            }
    //        }
    //    }
    //}
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
    //protected void gdvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    foreach (GridViewRow row in gdvList.Rows)
    //    {
    //        var chkBox = row.FindControl("chkSelect") as CheckBox;

    //        IDataItemContainer container = (IDataItemContainer)chkBox.NamingContainer;

    //        if (chkBox.Checked)
    //        {
    //            PersistRowIndex(container.DataItemIndex);
    //        }
    //        else
    //        {
    //            RemoveRowIndex(container.DataItemIndex);
    //        }
    //    }
    //    gdvList.PageIndex = e.NewPageIndex;
    //    BindGrid(Convert.ToInt64(ddlYear.SelectedValue), Convert.ToInt64(ddlGrade.SelectedValue));
    //}

    protected void BindDropDown()
    {
        //ddlWard.DataSource = new UsersDAL().RetrieveParentsChildren(logonUser.Id);
        ddlWard.DataSource = new UsersLIB().RetrieveParentsChildren(logonUser.Id);
        ddlWard.DataBind();
        ddlWard.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Ward--", "0", true));
    }

    protected void btnPrintAll_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (ddlAcademicSession.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select session";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
            }
            else if (ddlAcademicTerm.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select term";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
            }
            //else if (ddlYear.SelectedIndex == 0)
            //{
            //    lblErrorMsg.Text = "Kindly select year";
            //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            //    lblErrorMsg.Visible = true;
            //}
            //else if (ddlGrade.SelectedIndex == 0)
            //{
            //    lblErrorMsg.Text = "Kindly select grade";
            //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            //    lblErrorMsg.Visible = true;
            //}
            else if (ddlWard.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select ward";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
            }
            else
            {
                long campusId, schoolId, subjectId, yearID, gradeId = 0;
                campusId = logonUser.SchoolCampusId;
                schoolId = (long)logonUser.SchoolId;
                //Int64.TryParse(ddlSubject.SelectedValue, out subjectId);
                Int64.TryParse(ddlYear.SelectedValue, out yearID);
                Int64.TryParse(ddlGrade.SelectedValue, out gradeId);
                var scoreList = new ClassGradeLIB().RetrieveSingleGradeStudent(schoolId, campusId, Convert.ToInt64(ddlWard.SelectedValue));
                var selectedUsers = from u in scoreList select u.User;
                bulkPrinting(selectedUsers.ToList<PASSIS.LIB.User>());
            }
        }
        catch (Exception ex)
        {
        }
    }
    //protected void btnPrintAll_OnClick(object sender, EventArgs e)
    //{
    //    long campusId, schoolId, subjectId, yearID, gradeId = 0;
    //    campusId = logonUser.SchoolCampusId;
    //    schoolId = (long)logonUser.SchoolId;
    //    //Int64.TryParse(ddlSubject.SelectedValue, out subjectId);
    //    Int64.TryParse(ddlYear.SelectedValue, out yearID);
    //    Int64.TryParse(ddlGrade.SelectedValue, out gradeId);
    //    var scoreList = new ClassGradeLIB().RetrieveGradeStudents(schoolId, campusId, yearID, gradeId);
    //    var selectedUsers = from u in scoreList select u.User;
    //    bulkPrinting(selectedUsers.ToList<PASSIS.LIB.User>());

    //}
    //protected void btnPrintSelection_OnClick(object sender, EventArgs e)
    //{

    //    try
    //    {
    //        IList<PASSIS.LIB.User> selectedUsers = new List<PASSIS.LIB.User>();
    //        UsersLIB userLib = new UsersLIB();
    //        foreach (GridViewRow row in gdvList.Rows)
    //        {
    //            CheckBox chkSelectStudent = row.FindControl("chkSelect") as CheckBox;
    //            Label lblStudentId = row.FindControl("lblStudentId") as Label;
    //            Int64 Id = Convert.ToInt64(lblStudentId.Text.Trim());

    //            if (chkSelectStudent.Checked)
    //            {
    //                selectedUsers.Add(userLib.RetrieveUser(Id));
    //            }

    //        }
    //        bulkPrinting(selectedUsers);
    //    }
    //    catch (Exception ex)
    //    {

    //        //throw ex;
    //    }
    //}
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
    private Font darkerGrnFnt8 = FontFactory.GetFont(BaseFont.HELVETICA, 8, Font.BOLD, new BaseColor(26, 74, 17));
    private Font darkerGrnFnt9 = FontFactory.GetFont(BaseFont.HELVETICA, 10, Font.BOLD, new BaseColor(26, 74, 17));
    private Font darkerGrnFnt16 = FontFactory.GetFont(BaseFont.HELVETICA, 16, Font.BOLD, new BaseColor(26, 74, 17));
    private Font grnFnt = FontFactory.GetFont(BaseFont.HELVETICA, 14, new BaseColor(26, 74, 17));
    private Font blackFntB = FontFactory.GetFont(BaseFont.HELVETICA, 14, Font.BOLD, new BaseColor(0, 0, 0));
    private Font blackFnt8 = FontFactory.GetFont(BaseFont.HELVETICA, 8, new BaseColor(0, 0, 0));
    private Font blackFnt10 = FontFactory.GetFont(BaseFont.HELVETICA, 10, new BaseColor(0, 0, 0));
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


        addResultSummaryPageJNJ(document, student, usrDal);

        //}
        //catch (Exception ex)
        //{
        //    PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
        //}
    }

    void BehaviorDownload(PASSIS.LIB.User student, Document document, UsersLIB usrDal)
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


        BehavioralPage(document, student, usrDal);

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

    public void bulkPrintingBehavior(IList<PASSIS.LIB.User> selectedUsers)
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
            BehaviorDownload(stdnt, document, usrdal);
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
    protected void addResultSummaryPageJNJ(Document document, PASSIS.LIB.User student, UsersLIB usrDal)
    {

        iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(Server.MapPath(SchoolLogo));
        jpg.ScaleToFit(70, 70);
        jpg.Alignment = iTextSharp.text.Image.UNDERLYING;
        jpg.SetAbsolutePosition(250, 750);

        iTextSharp.text.Image sign = iTextSharp.text.Image.GetInstance(Server.MapPath(SchoolSignature));
        sign.ScaleToFit(70, 20);
        sign.Alignment = iTextSharp.text.Image.UNDERLYING;
        sign.SetAbsolutePosition(250, 750);

        Passport = student.PassportFileName;
        if (Passport == null) { Passport = "~/Images/student3.PNG"; }
        iTextSharp.text.Image jpg1 = iTextSharp.text.Image.GetInstance(Server.MapPath(Passport));
        jpg1.ScaleToFit(70, 70);
        jpg1.Alignment = iTextSharp.text.Image.UNDERLYING;
        jpg1.SetAbsolutePosition(250, 750);
        PdfPTable innerTable = new PdfPTable(9);
        PdfPCell innerCell1 = new PdfPCell(new Phrase("JANET 'N' JOHN COLLEGE", darkerGrnFnt16)); innerCell1.Colspan = 9; innerCell1.Border = 0; innerCell1.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell innerCell2 = new PdfPCell(new Phrase("1, ESTAPORT AVENUE, SOLUYI-GBAGADA, LAGOS", blackFnt8)); innerCell2.Colspan = 9; innerCell2.Border = 0; innerCell2.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell innerCell3 = new PdfPCell(new Phrase("Tel: 08164584442, 08085687700", resultTitleRedFnt8)); innerCell3.Colspan = 9; innerCell3.Border = 0; innerCell3.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell innerCell4 = new PdfPCell(new Phrase("MOTTO: Work Hard, Pray Hard and Keep Straight", blackFnt8)); innerCell4.Colspan = 9; innerCell4.Border = 0; innerCell4.HorizontalAlignment = Element.ALIGN_CENTER;
        innerTable.AddCell(innerCell1);
        innerTable.AddCell(innerCell2);
        innerTable.AddCell(innerCell3);
        innerTable.AddCell(innerCell4);


        PdfPTable head = new PdfPTable(15);
        PdfPCell head1 = new PdfPCell(jpg); head1.Colspan = 2; head1.Border = 0;
        PdfPCell head2 = new PdfPCell(innerTable); head2.Colspan = 11; head2.Border = 0; head2.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell head3 = new PdfPCell(jpg1); head3.Colspan = 2; head3.HorizontalAlignment = Element.ALIGN_RIGHT; head3.Border = 0;


        head.AddCell(head1);
        head.AddCell(head2);
        head.AddCell(head3);
        document.Add(head);
        if (Convert.ToInt16(ddlYear.SelectedValue) > 32 && Convert.ToInt16(ddlYear.SelectedValue) < 36)
        {
            Paragraph transcript = new Paragraph(string.Format("{0}", "SENIOR SECONDARY SCHOOL TERMINAL REPORT SHEET"), darkerRedFnt);
            transcript.Alignment = Element.ALIGN_CENTER;
            document.Add(transcript);
        }
        else if (Convert.ToInt16(ddlYear.SelectedValue) > 29 && Convert.ToInt16(ddlYear.SelectedValue) < 33)
        {
            Paragraph transcript = new Paragraph(string.Format("{0}", "JUNIOR SECONDARY SCHOOL TERMINAL REPORT SHEET"), darkerRedFnt);
            transcript.Alignment = Element.ALIGN_CENTER;
        }

        //document.Add(getBackgroundImage());
        document.Add(new Phrase(Environment.NewLine));
        Paragraph sessionName = new Paragraph(string.Format("SESSION: {0} TERM: {1}", ddlAcademicSession.SelectedItem.Text, ddlAcademicTerm.SelectedItem.Text), blackFnt8);
        sessionName.IndentationLeft = 100;
        //sessionName.Alignment = Element.ALIGN_CENTER;
        //document.Add(sessionName);
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
        //long gradeId = new print_report_card_jnj().theGradeId(student.Id).GradeId;
        long gradeId = Convert.ToInt64(ddlGrade.SelectedValue);
        string noInClass = noOfStudentInClass(schId, gradeId).Count.ToString();
        string classPosition = "";
        decimal? aggregateTotalScore = 0;
        decimal? totalAverageScore = 0;
        var posInClass = getClassPosition(student.AdmissionNumber, term, session, schoolId, yearId, gradeIdd);
        if (posInClass != null)
        {
            classPosition = posInClass.Position.ToString();
        }

        string totalNoInClass = new ClassGradeLIB().RetrieveGradeStudents(Convert.ToInt64(schId), Convert.ToInt64(logonUser.SchoolCampusId), Convert.ToInt64(yearId), gradeIdd, Convert.ToInt64(ddlAcademicSession.SelectedValue)).Count().ToString();

        PdfPTable maintbl = new PdfPTable(8);
        PdfPCell cell1 = new PdfPCell(new Phrase("Session:", blackFnt8)); cell1.Colspan = 1; cell1.HorizontalAlignment = Element.ALIGN_LEFT; //cell1.Border = 0;
        PdfPCell cell2 = new PdfPCell(new Phrase(ddlAcademicSession.SelectedItem.Text, blackFnt8)); cell2.Colspan = 1; cell1.HorizontalAlignment = Element.ALIGN_LEFT; //cell2.Border = 0;
        PdfPCell cell3 = new PdfPCell(new Phrase("Term:", blackFnt8)); cell3.Colspan = 1; cell3.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell cell4 = new PdfPCell(new Phrase(ddlAcademicTerm.SelectedItem.Text, blackFnt8)); cell4.Colspan = 1; cell4.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell cell5 = new PdfPCell(new Phrase("No of times School Opened:", blackFnt8)); cell5.Colspan = 3; cell5.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell cell6 = new PdfPCell(new Phrase("", blackFnt8)); cell6.Colspan = 1; cell6.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        //PdfPCell cell7 = new PdfPCell(new Phrase("SCHOOL OPENED:", blackFnt8)); cell7.Colspan = 2; cell7.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        //PdfPCell cell8 = new PdfPCell(new Phrase("", blackFnt8)); cell8.Colspan = 2; cell8.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;

        PdfPCell row2cell1 = new PdfPCell(new Phrase("Student's Name:", blackFnt8)); row2cell1.HorizontalAlignment = Element.ALIGN_LEFT; row2cell1.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell row2cell2 = new PdfPCell(new Phrase(student.StudentFullName, blackFnt8)); row2cell2.Colspan = 3; row2cell2.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        PdfPCell row2cell3 = new PdfPCell(new Phrase("No of times Present:", blackFnt8)); row2cell3.HorizontalAlignment = Element.ALIGN_LEFT; row2cell3.Colspan = 2; //row2cell1.Border = 0;
        PdfPCell row2cell4 = new PdfPCell(new Phrase("", blackFnt8)); row2cell4.Colspan = 2; row2cell4.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        PdfPCell row2cell5 = new PdfPCell(new Phrase("Class:", blackFnt8)); row2cell5.HorizontalAlignment = Element.ALIGN_LEFT; row2cell5.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell row2cell6 = new PdfPCell(new Phrase(ddlGrade.SelectedItem.Text, blackFnt8)); row2cell6.Colspan = 1; row2cell6.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        //PdfPCell row2cell6 = new PdfPCell(new Phrase(usrDal.getStudentsHomeRoom(student.Id), blackFnt8)); row2cell6.Colspan = 1; row2cell6.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        PdfPCell row2cell7 = new PdfPCell(new Phrase("No in Class", blackFnt8)); row2cell7.HorizontalAlignment = Element.ALIGN_LEFT; row2cell7.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell row2cell8 = new PdfPCell(new Phrase(totalNoInClass, blackFnt8)); row2cell8.Colspan = 2; row2cell8.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        PdfPCell row2cell9 = new PdfPCell(new Phrase("Average Mark", blackFnt8)); row2cell9.HorizontalAlignment = Element.ALIGN_LEFT; row2cell9.Colspan = 2; //row2cell1.Border = 0;
        //PdfPCell row2cell10 = new PdfPCell(new Phrase("", blackFnt8)); row2cell10.Colspan = 2; row2cell10.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;




        maintbl.AddCell(cell1);
        maintbl.AddCell(cell2);
        maintbl.AddCell(cell3);
        maintbl.AddCell(cell4);
        maintbl.AddCell(cell5);
        maintbl.AddCell(cell6);
        //maintbl.AddCell(cell7);
        //maintbl.AddCell(cell8);

        maintbl.AddCell(row2cell1);
        maintbl.AddCell(row2cell2);
        maintbl.AddCell(row2cell3);
        maintbl.AddCell(row2cell4);
        maintbl.AddCell(row2cell5);
        maintbl.AddCell(row2cell6);
        maintbl.AddCell(row2cell7);
        //maintbl.AddCell(row2cell8);
        //maintbl.AddCell(row2cell9);
        //maintbl.AddCell(row2cell10);

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

        //resTable.SetWidthPercentage(new float[8] { 80f, 30f, 30f, 30f, 35f, 35f, 38f, 30f }, PageSize.A5.Rotate());
        //resTable.TotalWidth = 400f;
        //resTable.WidthPercentage = 80;
        //resTable.LockedWidth = true;
        //PdfPCell deptHdr = new PdfPCell(new Phrase("DEPARTMENT", resultTitleRedFnt10)); deptHdr.Padding = 0f;
        //deptHdr.Colspan = 2; deptHdr.HorizontalAlignment = Element.ALIGN_CENTER; deptHdr.VerticalAlignment = Element.ALIGN_BOTTOM;
        //resTable.AddCell(deptHdr);

        PdfPTable thirdTerm = new PdfPTable(12);
        PdfPTable resTable = new PdfPTable(12);
        //PdfPCell thirdRow1Cell1 = new PdfPCell(new Phrase("3RD TERM RESULT", blackFnt10)); thirdRow1Cell1.Colspan = 8; thirdRow1Cell1.HorizontalAlignment = Element.ALIGN_CENTER;

        PdfPCell thirdRow2Cell1 = new PdfPCell(new Phrase("Subject", blackFnt8)); thirdRow2Cell1.Colspan = 3; thirdRow2Cell1.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell thirdRow2Cell2 = new PdfPCell(new Phrase("Continuous Assesment Score", blackFnt8)); thirdRow2Cell2.Colspan = 2; thirdRow2Cell2.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell thirdRow2Cell3 = new PdfPCell(new Phrase("Examination Score", blackFnt8)); thirdRow2Cell3.Colspan = 2; thirdRow2Cell3.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell thirdRow2Cell4 = new PdfPCell(new Phrase("1st Term Score", blackFnt8)); thirdRow2Cell4.Colspan = 1; thirdRow2Cell4.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell thirdRow2Cell5 = new PdfPCell(new Phrase("2nd Term Score", blackFnt8)); thirdRow2Cell5.Colspan = 1; thirdRow2Cell5.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell thirdRow2Cell6 = new PdfPCell(new Phrase("3rd Term Score", blackFnt8)); thirdRow2Cell6.Colspan = 1; thirdRow2Cell6.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell thirdRow2Cell7 = new PdfPCell(new Phrase("Annual Score", blackFnt8)); thirdRow2Cell7.Colspan = 1; thirdRow2Cell7.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell thirdRow2Cell8 = new PdfPCell(new Phrase("Annual Score Grade", blackFnt8)); thirdRow2Cell8.Colspan = 1; thirdRow2Cell8.HorizontalAlignment = Element.ALIGN_CENTER;


        //thirdTerm.AddCell(thirdRow1Cell1);

        resTable.AddCell(thirdRow2Cell1);
        resTable.AddCell(thirdRow2Cell2);
        resTable.AddCell(thirdRow2Cell3);
        resTable.AddCell(thirdRow2Cell4);
        resTable.AddCell(thirdRow2Cell5);
        resTable.AddCell(thirdRow2Cell6);
        resTable.AddCell(thirdRow2Cell7);
        resTable.AddCell(thirdRow2Cell8);
        //document.Add(thirdTerm);

        PdfPTable remarkHead = new PdfPTable(2);
        PdfPCell remarkHeadCell = new PdfPCell(new Phrase("REMARK", blackFnt10)); remarkHeadCell.Colspan = 2; remarkHeadCell.Rotation = 90;
        remarkHead.AddCell(remarkHeadCell);

        PdfPTable ressTable = new PdfPTable(5);

        //PdfPCell subjectHdr = new PdfPCell(new Phrase("SUBJECTS", blackFnt10)); subjectHdr.Padding = 0f;
        //subjectHdr.Colspan = 3; subjectHdr.HorizontalAlignment = Element.ALIGN_CENTER; //subjectHdr.VerticalAlignment = Element.ALIGN_BOTTOM;
        //ressTable.AddCell(subjectHdr);
        //PdfPCell ts1Hdr = new PdfPCell(new Phrase("1ST TERM", blackFnt10)); ts1Hdr.Colspan = 1; ts1Hdr.Rotation = 90; ts1Hdr.VerticalAlignment = Element.ALIGN_TOP; //ts1Hdr.HorizontalAlignment = Element.ALIGN_LEFT;
        //ressTable.AddCell(ts1Hdr);
        //PdfPCell ts2Hdr = new PdfPCell(new Phrase("2ND TERM", blackFnt10)); ts2Hdr.Colspan = 1; ts2Hdr.Rotation = 90; ts2Hdr.VerticalAlignment = Element.ALIGN_TOP; //ts2Hdr.HorizontalAlignment = Element.ALIGN_LEFT; 
        //ressTable.AddCell(ts2Hdr);
        //PdfPTable groupHead = new PdfPTable(15);
        //PdfPCell headCell1 = new PdfPCell(ressTable); headCell1.Colspan = 5;
        //PdfPCell headCell2 = new PdfPCell(thirdTerm); headCell2.Colspan = 8;
        //PdfPCell headCell3 = new PdfPCell(remarkHead); headCell3.Colspan = 2;
        //groupHead.AddCell(headCell1);
        //groupHead.AddCell(headCell2);
        //groupHead.AddCell(headCell3);



        //PdfPCell ts3Hdr = new PdfPCell(); ts3Hdr.Colspan = 1; ts3Hdr.Rotation = 90; ts3Hdr.HorizontalAlignment = Element.ALIGN_LEFT; ts3Hdr.VerticalAlignment = Element.ALIGN_TOP; ts3Hdr.AddElement(new Phrase("TEST 3", resultTitleRedFnt8)); resTable.AddCell(ts3Hdr);
        //PdfPCell ts4Hdr = new PdfPCell(); ts4Hdr.Colspan = 1; ts4Hdr.Rotation = 90; ts4Hdr.HorizontalAlignment = Element.ALIGN_LEFT; ts4Hdr.VerticalAlignment = Element.ALIGN_TOP; ts4Hdr.AddElement(new Phrase("TEST 4", resultTitleRedFnt8)); resTable.AddCell(ts4Hdr);
        //PdfPCell tsTotalHdr = new PdfPCell(); tsTotalHdr.Colspan = 1; tsTotalHdr.Rotation = 90; tsTotalHdr.HorizontalAlignment = Element.ALIGN_LEFT; tsTotalHdr.VerticalAlignment = Element.ALIGN_TOP; tsTotalHdr.AddElement(new Phrase("TEST TOTAL", resultTitleRedFnt8)); resTable.AddCell(tsTotalHdr);
        //PdfPCell examHdr = new PdfPCell(); examHdr.Colspan = 1; examHdr.Rotation = 90; examHdr.HorizontalAlignment = Element.ALIGN_LEFT; examHdr.VerticalAlignment = Element.ALIGN_TOP; examHdr.AddElement(new Phrase("EXAM", resultTitleRedFnt8)); resTable.AddCell(examHdr);
        //PdfPCell totalHdr = new PdfPCell(); totalHdr.Colspan = 1; totalHdr.Rotation = 90; totalHdr.HorizontalAlignment = Element.ALIGN_LEFT; totalHdr.VerticalAlignment = Element.ALIGN_TOP; totalHdr.AddElement(new Phrase("TERM TOTAL", resultTitleRedFnt8)); resTable.AddCell(totalHdr);
        //PdfPCell commentHdr = new PdfPCell(); commentHdr.Colspan = 2; commentHdr.HorizontalAlignment = Element.ALIGN_CENTER; commentHdr.VerticalAlignment = Element.ALIGN_BOTTOM; commentHdr.AddElement(new Phrase("COMMENT", resultTitleRedFnt8)); resTable.AddCell(commentHdr);
        PdfPCell space3 = new PdfPCell(); space3.Colspan = 3;
        PdfPCell space2 = new PdfPCell(); space2.Colspan = 2;
        PdfPCell space1 = new PdfPCell(); space1.Colspan = 1;
        PdfPCell space10 = new PdfPCell(); space10.Colspan = 10;
        //resTable.AddCell(space);
        ////resTable.AddCell(space);
        //resTable.AddCell("10");
        //resTable.AddCell("10");
        //resTable.AddCell("10");
        //resTable.AddCell("10");
        //resTable.AddCell("40");
        //resTable.AddCell("60");
        //resTable.AddCell("100");
        //resTable.AddCell(space2);
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

        PASSIS.LIB.GradeStudent gs = new ClassGradeLIB().RetrieveStudentGrade(student.Id, session);

        //Int64 subjectCounter = 0;
        int? curricullumId = 0;
        int totalMarkObtained = 0;
        int totalMarkObtainable = 0;
        decimal testScoreConfiguration = 0;
        decimal? ftt = 0;
        decimal? stt = 0;
        decimal? fte = 0;
        decimal? ste = 0;
        decimal? ftte = 0;
        decimal? stte = 0;
        decimal? totalBroughtForward = 0;
        decimal testScore = 0;
        decimal examScore = -1;
        decimal totalScore = 0;
        decimal? totalScoreAverage = 0;
        decimal? totalScoreAverages = 0;
        decimal scorePosition = 0;

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
            //IList<SubjectsInSchool> getSubjectInClass = (from s in context.SubjectsInSchools where s.SchoolId == schoolId && s.ReportCardOrder != null select s).ToList();
            IList<SubjectsInSchool> getSubjectInClass = (from s in context.SubjectsInSchools where s.SchoolId == schoolId select s).ToList();
            IList<SubjectsInSchool> sortedTScore = getSubjectInClass.OrderBy(c => c.ReportCardOrder).ToList();
            foreach (SubjectsInSchool subjects in getSubjectInClass)
            {
                PASSIS.LIB.Subject subject = context.Subjects.FirstOrDefault(x =>
                                  x.CurriculumId == (int)curricullumId &&
                                  x.ClassId == yearId && x.Id == subjects.SubjectId);
                if (subject != null)
                {
                    AllSubject.Add(subject);
                    //}
                    //}
                    //return AllSubject.ToList<Subject>();

                    //foreach (PASSIS.LIB.Subject sub in AllSubject)
                    //{
                    //Get first and second term scores for test and exam

                    if (ddlAcademicTerm.SelectedValue == "1")
                    {
                        PdfPCell subjCell = new PdfPCell(new Phrase(subject.Name, darkerGrnFnt8)); subjCell.Colspan = 3;
                        resTable.AddCell(subjCell);
                        IList<PASSIS.LIB.StudentScoreRepository> scoreRepoFirstTerm = getSubjectScoreCategoryCA(student.AdmissionNumber, student.Id, session, 1, schoolId, yearId, gradeId, subject.Id);
                        if (scoreRepoFirstTerm.Count > 0)
                        {
                            foreach (PASSIS.LIB.StudentScoreRepository fs in scoreRepoFirstTerm)
                            {
                                //if (fs != null)
                                //{
                                testScore = Convert.ToDecimal(fs.CAPercentageScore);
                                PdfPCell CACell = new PdfPCell(new Phrase(Math.Round(testScore, 0).ToString(), darkerGrnFnt8)); CACell.Colspan = 2;
                                resTable.AddCell(CACell);
                                //}
                                //else
                                //{

                                //}

                                //PdfPCell secondCell = new PdfPCell(new Phrase(stte.ToString(), darkerGrnFnt8)); secondCell.Colspan = 1;


                                //resTable.AddCell(secondCell);
                                break;
                            }
                        }
                        else
                        {
                            testScore = 0;
                            PdfPCell CACell = new PdfPCell(new Phrase("", darkerGrnFnt8)); CACell.Colspan = 2;
                            resTable.AddCell(CACell);
                        }
                        IList<PASSIS.LIB.StudentScore> scoreRepoExamFirstTerm = getSubjectScoreCategoryExam(student.AdmissionNumber, student.Id, session, 1, schoolId, yearId, gradeId, subject.Id);
                        if (scoreRepoExamFirstTerm.Count > 0)
                        {
                            foreach (PASSIS.LIB.StudentScore fs in scoreRepoExamFirstTerm)
                            {
                                //if (fs != null)
                                //{
                                examScore = Convert.ToDecimal(fs.ExamPercentageScore);
                                PdfPCell ExamCell = new PdfPCell(new Phrase(Math.Round(examScore, 0).ToString(), darkerGrnFnt8)); ExamCell.Colspan = 2;
                                resTable.AddCell(ExamCell);

                                //}
                                //else
                                //{

                                //}
                                break;
                            }
                        }
                        else
                        {
                            examScore = 0;
                            PdfPCell ExamCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); ExamCell.Colspan = 2;
                            resTable.AddCell(ExamCell);
                        }
                        totalScore = testScore + examScore;
                        aggregateTotalScore += Math.Round(totalScore, 0);
                        if (totalScore == 0)
                        {
                            PdfPCell TotalFirstCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); TotalFirstCell.Colspan = 1;
                            resTable.AddCell(TotalFirstCell);
                        }
                        else
                        {
                            PdfPCell TotalFirstCell = new PdfPCell(new Phrase(Math.Round(totalScore, 0).ToString(), darkerGrnFnt8)); TotalFirstCell.Colspan = 1;
                            resTable.AddCell(TotalFirstCell);
                        }
                        resTable.AddCell(space1);
                        resTable.AddCell(space1);
                        resTable.AddCell(space1);
                        resTable.AddCell(space1);
                    }
                    else if (ddlAcademicTerm.SelectedValue == "2")
                    {
                        PdfPCell subjCell = new PdfPCell(new Phrase(subject.Name, darkerGrnFnt8)); subjCell.Colspan = 3;
                        resTable.AddCell(subjCell);
                        IList<PASSIS.LIB.StudentScoreRepository> scoreRepoSecondTerm = getSubjectScoreCategoryCA(student.AdmissionNumber, student.Id, session, 2, schoolId, yearId, gradeId, subject.Id);
                        if (scoreRepoSecondTerm.Count > 0)
                        {
                            foreach (PASSIS.LIB.StudentScoreRepository fs in scoreRepoSecondTerm)
                            {
                                //if (fs != null)
                                //{
                                testScore = Convert.ToDecimal(fs.CAPercentageScore);
                                PdfPCell CACell = new PdfPCell(new Phrase(Math.Round(testScore, 0).ToString(), darkerGrnFnt8)); CACell.Colspan = 2;
                                resTable.AddCell(CACell);
                                //}
                                //else
                                //{

                                //}

                                //PdfPCell secondCell = new PdfPCell(new Phrase(stte.ToString(), darkerGrnFnt8)); secondCell.Colspan = 1;


                                //resTable.AddCell(secondCell);
                                break;
                            }
                        }
                        else
                        {
                            testScore = 0;
                            PdfPCell CACell = new PdfPCell(new Phrase("", darkerGrnFnt8)); CACell.Colspan = 2;
                            resTable.AddCell(CACell);
                        }
                        IList<PASSIS.LIB.StudentScore> scoreRepoExamSecondTerm = getSubjectScoreCategoryExam(student.AdmissionNumber, student.Id, session, 2, schoolId, yearId, gradeId, subject.Id);
                        if (scoreRepoExamSecondTerm.Count > 0)
                        {
                            foreach (PASSIS.LIB.StudentScore fs in scoreRepoExamSecondTerm)
                            {
                                //if (fs != null)
                                //{
                                examScore = Convert.ToDecimal(fs.ExamPercentageScore);
                                PdfPCell ExamCell = new PdfPCell(new Phrase(Math.Round(examScore, 0).ToString(), darkerGrnFnt8)); ExamCell.Colspan = 2;
                                resTable.AddCell(ExamCell);
                                //}
                                //else
                                //{

                                //}
                                break;
                            }
                        }
                        else
                        {
                            examScore = 0;
                            PdfPCell ExamCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); ExamCell.Colspan = 2;
                            resTable.AddCell(ExamCell);
                        }
                        resTable.AddCell(space1);
                        totalScore = testScore + examScore;
                        aggregateTotalScore += Math.Round(totalScore, 0);
                        if (totalScore == 0)
                        {
                            PdfPCell TotalFirstCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); TotalFirstCell.Colspan = 1;
                            resTable.AddCell(TotalFirstCell);
                        }
                        else
                        {
                            PdfPCell TotalFirstCell = new PdfPCell(new Phrase(Math.Round(totalScore, 0).ToString(), darkerGrnFnt8)); TotalFirstCell.Colspan = 1;
                            resTable.AddCell(TotalFirstCell);
                        }
                        resTable.AddCell(space1);
                        resTable.AddCell(space1);
                        resTable.AddCell(space1);

                    }
                    else if (ddlAcademicTerm.SelectedValue == "3")
                    {
                        PdfPCell subjCell = new PdfPCell(new Phrase(subject.Name, darkerGrnFnt8)); subjCell.Colspan = 3;
                        resTable.AddCell(subjCell);
                        IList<PASSIS.LIB.StudentScoreRepository> scoreRepoThirdTerm = getSubjectScoreCategoryCA(student.AdmissionNumber, student.Id, session, 3, schoolId, yearId, gradeId, subject.Id);
                        if (scoreRepoThirdTerm.Count > 0)
                        {
                            foreach (PASSIS.LIB.StudentScoreRepository fs in scoreRepoThirdTerm)
                            {
                                //if (fs != null)
                                //{
                                testScore = Convert.ToDecimal(fs.CAPercentageScore);
                                PdfPCell CACell = new PdfPCell(new Phrase(Math.Round(testScore, 0).ToString(), darkerGrnFnt8)); CACell.Colspan = 2;
                                resTable.AddCell(CACell);
                                //}
                                //else
                                //{

                                //}

                                //PdfPCell secondCell = new PdfPCell(new Phrase(stte.ToString(), darkerGrnFnt8)); secondCell.Colspan = 1;


                                //resTable.AddCell(secondCell);
                                break;
                            }
                        }
                        else
                        {
                            testScore = 0;
                            PdfPCell CACell = new PdfPCell(new Phrase("", darkerGrnFnt8)); CACell.Colspan = 2;
                            resTable.AddCell(CACell);
                        }
                        IList<PASSIS.LIB.StudentScore> scoreRepoExamThirdTerm = getSubjectScoreCategoryExam(student.AdmissionNumber, student.Id, session, 3, schoolId, yearId, gradeId, subject.Id);
                        if (scoreRepoExamThirdTerm.Count > 0)
                        {
                            foreach (PASSIS.LIB.StudentScore fs in scoreRepoExamThirdTerm)
                            {
                                //if (fs != null)
                                //{
                                examScore = Convert.ToDecimal(fs.ExamPercentageScore);
                                PdfPCell ExamCell = new PdfPCell(new Phrase(Math.Round(examScore, 0).ToString(), darkerGrnFnt8)); ExamCell.Colspan = 2;
                                resTable.AddCell(ExamCell);
                                //}
                                //else
                                //{

                                //}
                                break;
                            }
                        }
                        else
                        {
                            examScore = 0;
                            PdfPCell ExamCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); ExamCell.Colspan = 2;
                            resTable.AddCell(ExamCell);
                        }
                        IList<PASSIS.LIB.StudentScoreRepository> scoreRepoFirstTerm = getSubjectScoreCategoryCA(student.AdmissionNumber, student.Id, session, 1, schoolId, yearId, gradeId, subject.Id);
                        if (scoreRepoFirstTerm.Count > 0)
                        {
                            foreach (StudentScoreRepository s in scoreRepoFirstTerm)
                                ftt = s.CAPercentageScore;
                        }
                        else
                        {
                            ftt = 0;
                        }
                        IList<PASSIS.LIB.StudentScore> scoreRepoExamFirstTerm = getSubjectScoreCategoryExam(student.AdmissionNumber, student.Id, session, 1, schoolId, yearId, gradeId, subject.Id);
                        if (scoreRepoExamFirstTerm.Count > 0)
                        {
                            foreach (PASSIS.LIB.StudentScore fs in scoreRepoExamFirstTerm)
                            {
                                fte = Convert.ToDecimal(fs.ExamPercentageScore);
                                break;
                            }
                        }
                        else
                        {
                            fte = 0;
                        }

                        IList<PASSIS.LIB.StudentScoreRepository> scoreRepoSecondTerm = getSubjectScoreCategoryCA(student.AdmissionNumber, student.Id, session, 2, schoolId, yearId, gradeId, subject.Id);
                        if (scoreRepoSecondTerm.Count > 0)
                        {
                            foreach (StudentScoreRepository s in scoreRepoSecondTerm)
                                stt = s.CAPercentageScore;
                        }
                        else
                        {
                            stt = 0;
                        }
                        IList<PASSIS.LIB.StudentScore> scoreRepoExamSecondTerm = getSubjectScoreCategoryExam(student.AdmissionNumber, student.Id, session, 2, schoolId, yearId, gradeId, subject.Id);
                        if (scoreRepoExamSecondTerm.Count > 0)
                        {
                            foreach (PASSIS.LIB.StudentScore fs in scoreRepoExamSecondTerm)
                            {
                                ste = Convert.ToDecimal(fs.ExamPercentageScore);
                                break;
                            }
                        }
                        else
                        {
                            ste = 0;
                        }
                        ftte = ftt + fte;
                        stte = stt + ste;
                        if (ftte == 0)
                        {
                            PdfPCell TotalFirstTermCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); TotalFirstTermCell.Colspan = 1;
                            resTable.AddCell(TotalFirstTermCell);
                        }
                        else
                        {
                            PdfPCell TotalFirstTermCell = new PdfPCell(new Phrase(Math.Round(Convert.ToDecimal(ftte), 0).ToString(), darkerGrnFnt8)); TotalFirstTermCell.Colspan = 1;
                            resTable.AddCell(TotalFirstTermCell);
                        }
                        if (stte == 0)
                        {
                            PdfPCell TotalSecondTermCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); TotalSecondTermCell.Colspan = 1;
                            resTable.AddCell(TotalSecondTermCell);
                        }
                        else
                        {
                            PdfPCell TotalSecondTermCell = new PdfPCell(new Phrase(Math.Round(Convert.ToDecimal(stte), 0).ToString(), darkerGrnFnt8)); TotalSecondTermCell.Colspan = 1;
                            resTable.AddCell(TotalSecondTermCell);
                        }
                        totalScore = Convert.ToDecimal(testScore + examScore);
                        if (totalScore == 0)
                        {
                            PdfPCell TotalThirdTermCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); TotalThirdTermCell.Colspan = 1;
                            resTable.AddCell(TotalThirdTermCell);
                        }
                        else
                        {
                            PdfPCell TotalThirdTermCell = new PdfPCell(new Phrase(Math.Round(totalScore, 0).ToString(), darkerGrnFnt8)); TotalThirdTermCell.Colspan = 1;
                            resTable.AddCell(TotalThirdTermCell);
                        }
                        totalScoreAverage = Math.Round(Convert.ToDecimal(ftte + stte + totalScore), 0);
                        if (ftte == 0 && stte == 0 && totalScore > 0)
                        {
                            totalAverageScore = totalScoreAverage;
                        }
                        else if (ftte == 0 && stte > 0 && totalScore > 0)
                        {
                            totalAverageScore = totalScoreAverage / 2;
                        }
                        else if (ftte == 0 && stte > 0 && totalScore == 0)
                        {
                            totalAverageScore = totalScoreAverage / 1;
                        }
                        //else if (ftte == 0 && stte == 0 && totalScore == 0)
                        //{
                        //    totalAverageScore = null;
                        //}
                        else if (ftte > 0 && stte > 0 && totalScore > 0)
                        {
                            totalAverageScore = totalScoreAverage / 3;
                        }
                        else if (ftte > 0 && stte > 0 && totalScore == 0)
                        {
                            totalAverageScore = totalScoreAverage / 2;
                        }
                        else if (ftte > 0 && stte == 0 && totalScore == 0)
                        {
                            totalAverageScore = totalScoreAverage / 1;
                        }
                        else if (ftte > 0 && stte == 0 && totalScore > 0)
                        {
                            totalAverageScore = totalScoreAverage / 2;
                        }
                        if (totalAverageScore == 0 || totalAverageScore == null)
                        {
                            PdfPCell TotalAverageCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); TotalAverageCell.Colspan = 1;
                            resTable.AddCell(TotalAverageCell);
                            resTable.AddCell(space1);
                        }
                        else
                        {
                            PdfPCell TotalAverageCell = new PdfPCell(new Phrase(Math.Round(Convert.ToDecimal(totalAverageScore), 0).ToString(), darkerGrnFnt8)); TotalAverageCell.Colspan = 1;
                            resTable.AddCell(TotalAverageCell);
                            PdfPCell tGrade = new PdfPCell(new Phrase(PASSIS.LIB.Utility.Utili.getExamGradeLetters(Math.Round(Convert.ToDecimal(totalAverageScore), 0), (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)), blackFnt8)); tGrade.Colspan = 1;
                            resTable.AddCell(tGrade);
                        }

                        aggregateTotalScore += Math.Round(Convert.ToDecimal(totalAverageScore), 0);
                    }

                    examScore = 0;
                    testScore = 0;
                    totalScore = 0;
                    ftte = 0;
                    stte = 0;
                    totalAverageScore = 0;
                    totalScoreAverage = 0;
                }
            }
            if (subjectCounter != 0)
            {
                percentage = Math.Round(Convert.ToDecimal((aggregateTotalScore / (subjectCounter * 100)) * 100), 0);
            }
            //PdfPCell totalScoreCell = new PdfPCell(new Phrase("TOTAL SCORED", darkerGrnFnt8)); totalScoreCell.Colspan = 3;
            //resTable.AddCell(totalScoreCell);
            //resTable.AddCell("");
            //resTable.AddCell("");
            //resTable.AddCell("");
            //resTable.AddCell("");
            //resTable.AddCell("");
            //resTable.AddCell("");
            //resTable.AddCell(aggregateTotalScore.ToString());
            //resTable.AddCell(space2);
            //PdfPCell percentageCell = new PdfPCell(new Phrase("PERCENTAGE %", darkerGrnFnt8)); percentageCell.Colspan = 3;
            //resTable.AddCell(percentageCell);
            //resTable.AddCell("");
            //resTable.AddCell("");
            //resTable.AddCell("");
            //resTable.AddCell("");
            //resTable.AddCell("");
            //resTable.AddCell("");
            //resTable.AddCell(percentage.ToString());
            //resTable.AddCell(space2);
            subjectCounter = 0;
            //    foreach (SubjectDepartment dept in deptList)
            //    {
            //        IList<StudentScoreCA> rpCard = getSubjectScoreCA(student.AdmissionNumber, student.Id, session, term, schoolId, yearId, gradeIdd, dept.Id);
            //        foreach (StudentScoreCA d in rpCard)
            //        {

            //            if (dictionary.ContainsKey(dept.Id))
            //            {
            //                deptName = "";
            //                PdfPCell deptCell = new PdfPCell(new Phrase(deptName, darkerGrnFnt9)); deptCell.Colspan = 2; deptCell.BorderWidthTop = 0; deptCell.BorderColorTop = new BaseColor(255,255,255);
            //                resTable.AddCell(deptCell);
            //            }
            //            else 
            //            {
            //                deptName = dept.DepartmentName;
            //                dictionary.Add(dept.Id, dept.DepartmentName);
            //                PdfPCell deptCell = new PdfPCell(new Phrase(deptName, darkerGrnFnt9)); deptCell.Colspan = 2;
            //                resTable.AddCell(deptCell);
            //            }

            //            subjectCounter++;
            //            aggregateTotalScore += Convert.ToDecimal(d.Total);
            //            subjName = d.Subject.Name;
            //            testScore = Convert.ToDecimal(d.Test);
            //            cw = Convert.ToDecimal(d.ClassWork);
            //            hw = Convert.ToDecimal(d.HomeWorkProject);
            //            note = Convert.ToDecimal(d.AttendanceNote);
            //            //examScore = Convert.ToDecimal(d.ExamScore);
            //            //totalScore = Convert.ToDecimal(d.Total);
            //            totalScore = cw + hw + note + testScore;
            //            //scorePosition = Convert.ToDecimal(d.Position);

            //            string grade = PASSIS.LIB.Utility.Utili.getExamGradeLetters(Convert.ToDecimal(d.Total), (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue));
            //            if (grade.Trim() == "A") { distinctionCounter++; }
            //            else if (grade.Trim() == "B" || grade.Trim() == "C") { creditCounter++; }
            //            else if (grade.Trim() == "D") { passesCounter++; }
            //            else if (grade.Trim() == "E") { failureCounter++; }


            //            PdfPCell subjCell = new PdfPCell(new Phrase(d.Subject.Name, darkerGrnFnt9)); subjCell.Colspan = 2;
            //            resTable.AddCell(subjCell);
            //            PdfPCell cwCell = new PdfPCell(new Phrase(d.ClassWork.ToString(), blackFnt)); cwCell.Colspan = 1;
            //            resTable.AddCell(cwCell);
            //            PdfPCell hwCell = new PdfPCell(new Phrase(d.HomeWorkProject.ToString(), blackFnt)); hwCell.Colspan = 1;
            //            resTable.AddCell(hwCell);
            //            PdfPCell noteCell = new PdfPCell(new Phrase(d.AttendanceNote.ToString(), blackFnt)); noteCell.Colspan = 1;
            //            resTable.AddCell(noteCell);
            //            PdfPCell testCell = new PdfPCell(new Phrase(d.Test.ToString(), blackFnt)); testCell.Colspan = 1;
            //            resTable.AddCell(testCell);
            //            PdfPCell totalCell = new PdfPCell(new Phrase(totalScore.ToString(), blackFnt)); totalCell.Colspan = 1;
            //            resTable.AddCell(totalCell);
            //            PdfPCell gradeCell = new PdfPCell(new Phrase((PASSIS.LIB.Utility.Utili.getExamGradeLetters(Convert.ToDecimal(d.Total), (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue))), blackFnt)); gradeCell.Colspan = 1;
            //            resTable.AddCell(gradeCell);
            //            PdfPCell tRemark = new PdfPCell(new Phrase(PASSIS.LIB.Utility.Utili.getExamGradeRemarks(Convert.ToDecimal(d.Total), (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)), blackFnt)); tRemark.Colspan = 2;
            //            resTable.AddCell(tRemark);


            //            //++noOfSubjectWithScore;
            //            totalMarkObtainable = 0;
            //            totalMarkObtained = 0;
            //            testScore = 0;
            //        }
            //    }
        }

        PdfPTable subjectHeadTable = new PdfPTable(5);
        //PdfPCell subjectHeadCell1 = new PdfPCell(groupHead); subjectHeadCell1.Colspan = 5;
        //PdfPCell subjectHeadCell2 = new PdfPCell(resTable); subjectHeadCell2.Colspan = 5;
        //subjectHeadTable.AddCell(subjectHeadCell1);
        //subjectHeadTable.AddCell(subjectHeadCell1);
        //subjectHeadTable.AddCell(subjectHeadCell1);
        //subjectHeadTable.AddCell(subjectHeadCell2);

        Paragraph affective = new Paragraph(string.Format("{0}", "AFFECTIVE AREAS"), darkerRedFnt);
        affective.Alignment = Element.ALIGN_CENTER;


        PdfPTable socialTable = new PdfPTable(6);
        PdfPCell socialHead = new PdfPCell(new Phrase("SOCIAL BEHAVIOR AND MANIPULATIVE SKILLS", blackFnt10)); socialHead.Colspan = 6;
        PdfPCell socialCell1 = new PdfPCell(new Phrase("WORK HABITS", blackFnt10)); socialCell1.Colspan = 3;
        socialTable.AddCell(socialCell1);
        PdfPCell socialCell2 = new PdfPCell(new Phrase("RATINGS", blackFnt10)); socialCell2.Colspan = 3;
        socialTable.AddCell(socialCell2);

        PdfPTable behaviorTable = new PdfPTable(6);
        PdfPCell behaviorCell1 = new PdfPCell(new Phrase("BEHAVIOUR", blackFnt10)); behaviorCell1.Colspan = 3;
        behaviorTable.AddCell(behaviorCell1);
        PdfPCell behaviorCell2 = new PdfPCell(new Phrase("RATINGS", blackFnt10)); behaviorCell2.Colspan = 3;
        behaviorTable.AddCell(behaviorCell2);

        PdfPTable gradingTable = new PdfPTable(4);
        //PdfPCell gradingHead = new PdfPCell(new Phrase("KEY TO GRADING", blackFnt10)); gradingHead.Colspan = 6;
        //PdfPCell gradingCell1 = new PdfPCell(new Phrase("GRADE", blackFnt10)); gradingCell1.Colspan = 2;
        //gradingTable.AddCell(gradingCell1);
        //PdfPCell gradingCell2 = new PdfPCell(new Phrase("RATINGS", blackFnt10)); gradingCell2.Colspan = 2;
        //gradingTable.AddCell(gradingCell2);
        //PdfPCell gradingCell3 = new PdfPCell(new Phrase("EFFORT", blackFnt10)); gradingCell3.Colspan = 2;
        //gradingTable.AddCell(gradingCell3);

        PdfPCell gradingRow0Cell0 = new PdfPCell(new Phrase("GRADES", blackFnt10)); gradingRow0Cell0.Colspan = 4;
        gradingTable.AddCell(gradingRow0Cell0);
        PdfPCell gradingRow1Cell1 = new PdfPCell(new Phrase("00 - 39", blackFnt8)); gradingRow1Cell1.Colspan = 2;
        gradingTable.AddCell(gradingRow1Cell1);
        PdfPCell gradingRow1Cell2 = new PdfPCell(new Phrase("F9", blackFnt8)); gradingRow1Cell2.Colspan = 2;
        gradingTable.AddCell(gradingRow1Cell2);
        //PdfPCell gradingRow1Cell3 = new PdfPCell(new Phrase("DISTINCTION", blackFnt8)); gradingRow1Cell3.Colspan = 2;
        //gradingTable.AddCell(gradingRow1Cell3);

        PdfPCell gradingRow2Cell1 = new PdfPCell(new Phrase("40 - 44", blackFnt8)); gradingRow2Cell1.Colspan = 2;
        gradingTable.AddCell(gradingRow2Cell1);
        PdfPCell gradingRow2Cell2 = new PdfPCell(new Phrase("E8", blackFnt8)); gradingRow2Cell2.Colspan = 2;
        gradingTable.AddCell(gradingRow2Cell2);
        //PdfPCell gradingRow2Cell3 = new PdfPCell(new Phrase("EXCELLENT", blackFnt8)); gradingRow2Cell3.Colspan = 2;
        //gradingTable.AddCell(gradingRow2Cell3);

        PdfPCell gradingRow3Cell1 = new PdfPCell(new Phrase("45 - 49", blackFnt8)); gradingRow3Cell1.Colspan = 2;
        gradingTable.AddCell(gradingRow3Cell1);
        PdfPCell gradingRow3Cell2 = new PdfPCell(new Phrase("D7", blackFnt8)); gradingRow3Cell2.Colspan = 2;
        gradingTable.AddCell(gradingRow3Cell2);
        //PdfPCell gradingRow3Cell3 = new PdfPCell(new Phrase("VERY GOOD", blackFnt8)); gradingRow3Cell3.Colspan = 2;
        //gradingTable.AddCell(gradingRow3Cell3);

        PdfPCell gradingRow4Cell1 = new PdfPCell(new Phrase("50 - 54", blackFnt8)); gradingRow4Cell1.Colspan = 2;
        gradingTable.AddCell(gradingRow4Cell1);
        PdfPCell gradingRow4Cell2 = new PdfPCell(new Phrase("C6", blackFnt8)); gradingRow4Cell2.Colspan = 2;
        gradingTable.AddCell(gradingRow4Cell2);
        //PdfPCell gradingRow4Cell3 = new PdfPCell(new Phrase("GOOD", blackFnt8)); gradingRow4Cell3.Colspan = 2;
        //gradingTable.AddCell(gradingRow4Cell3);

        PdfPCell gradingRow5Cell1 = new PdfPCell(new Phrase("55 - 59", blackFnt8)); gradingRow5Cell1.Colspan = 2;
        gradingTable.AddCell(gradingRow5Cell1);
        PdfPCell gradingRow5Cell2 = new PdfPCell(new Phrase("C5", blackFnt8)); gradingRow5Cell2.Colspan = 2;
        gradingTable.AddCell(gradingRow5Cell2);
        //PdfPCell gradingRow5Cell3 = new PdfPCell(new Phrase("CREDIT", blackFnt8)); gradingRow5Cell3.Colspan = 2;
        //gradingTable.AddCell(gradingRow5Cell3);

        PdfPCell gradingRow6Cell1 = new PdfPCell(new Phrase("60 - 64", blackFnt8)); gradingRow6Cell1.Colspan = 2;
        gradingTable.AddCell(gradingRow6Cell1);
        PdfPCell gradingRow6Cell2 = new PdfPCell(new Phrase("C4", blackFnt8)); gradingRow6Cell2.Colspan = 2;
        gradingTable.AddCell(gradingRow6Cell2);
        //PdfPCell gradingRow6Cell3 = new PdfPCell(new Phrase("FAIR", blackFnt8)); gradingRow6Cell3.Colspan = 2;
        //gradingTable.AddCell(gradingRow6Cell3);

        PdfPCell gradingRow7Cell1 = new PdfPCell(new Phrase("65 - 69", blackFnt8)); gradingRow7Cell1.Colspan = 2;
        gradingTable.AddCell(gradingRow7Cell1);
        PdfPCell gradingRow7Cell2 = new PdfPCell(new Phrase("B3", blackFnt8)); gradingRow7Cell2.Colspan = 2;
        gradingTable.AddCell(gradingRow7Cell2);
        //PdfPCell gradingRow7Cell3 = new PdfPCell(new Phrase("POOR", blackFnt8)); gradingRow7Cell3.Colspan = 2;
        //gradingTable.AddCell(gradingRow7Cell3);

        PdfPCell gradingRow8Cell1 = new PdfPCell(new Phrase("70 - 74", blackFnt8)); gradingRow8Cell1.Colspan = 2;
        gradingTable.AddCell(gradingRow8Cell1);
        PdfPCell gradingRow8Cell2 = new PdfPCell(new Phrase("B2", blackFnt8)); gradingRow8Cell2.Colspan = 2;
        gradingTable.AddCell(gradingRow8Cell2);

        PdfPCell gradingRow9Cell1 = new PdfPCell(new Phrase("75 - 100", blackFnt8)); gradingRow9Cell1.Colspan = 2;
        gradingTable.AddCell(gradingRow9Cell1);
        PdfPCell gradingRow9Cell2 = new PdfPCell(new Phrase("A1", blackFnt8)); gradingRow9Cell2.Colspan = 2;
        gradingTable.AddCell(gradingRow9Cell2);

        PdfPTable gradingTableJunior = new PdfPTable(4);

        PdfPCell gradingRow0Cell0Junior = new PdfPCell(new Phrase("SCORE GRADING", blackFnt10)); gradingRow0Cell0Junior.Colspan = 4;
        gradingTableJunior.AddCell(gradingRow0Cell0Junior);

        PdfPCell gradingRow1Cell1Junior = new PdfPCell(new Phrase("0 - 44", blackFnt8)); gradingRow1Cell1Junior.Colspan = 2;
        gradingTableJunior.AddCell(gradingRow1Cell1Junior);
        PdfPCell gradingRow1Cell2Junior = new PdfPCell(new Phrase("F", blackFnt8)); gradingRow1Cell2Junior.Colspan = 2;
        gradingTableJunior.AddCell(gradingRow1Cell2Junior);

        PdfPCell gradingRow2Cell1Junior = new PdfPCell(new Phrase("45 - 54", blackFnt8)); gradingRow2Cell1Junior.Colspan = 2;
        gradingTableJunior.AddCell(gradingRow2Cell1Junior);
        PdfPCell gradingRow2Cell2Junior = new PdfPCell(new Phrase("P", blackFnt8)); gradingRow2Cell2Junior.Colspan = 2;
        gradingTableJunior.AddCell(gradingRow2Cell2Junior);

        PdfPCell gradingRow3Cell1Junior = new PdfPCell(new Phrase("55 - 74", blackFnt8)); gradingRow3Cell1Junior.Colspan = 2;
        gradingTableJunior.AddCell(gradingRow3Cell1Junior);
        PdfPCell gradingRow3Cell2Junior = new PdfPCell(new Phrase("C", blackFnt8)); gradingRow3Cell2Junior.Colspan = 2;
        gradingTableJunior.AddCell(gradingRow3Cell2Junior);

        PdfPCell gradingRow4Cell1Junior = new PdfPCell(new Phrase("75 - 100", blackFnt8)); gradingRow4Cell1Junior.Colspan = 2;
        gradingTableJunior.AddCell(gradingRow4Cell1Junior);
        PdfPCell gradingRow4Cell2Junior = new PdfPCell(new Phrase("A", blackFnt8)); gradingRow4Cell2Junior.Colspan = 2;
        gradingTableJunior.AddCell(gradingRow4Cell2Junior);

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

        foreach (ScoreSubCategoryConfiguration s in extra)
        {
            StudentScoreExtraCurricular ssb = context.StudentScoreExtraCurriculars.FirstOrDefault(x => x.SubCategoryId == s.Id && x.StudentId == student.Id && x.AdmissionNo == student.AdmissionNumber && x.SessionId == session && x.TermId == term && x.SchoolId == schoolId && x.ClassId == yearId && x.GradeId == gradeId);
            PdfPCell psychoCells1 = new PdfPCell(new Phrase(s.SubCategory, blackFnt8)); psychoCells1.Colspan = 3;
            socialTable.AddCell(psychoCells1);
            if (ssb != null)
            {
                PdfPCell psychoCells2 = new PdfPCell(new Phrase(Convert.ToInt16(ssb.MarkObtained).ToString(), blackFnt8)); psychoCells2.Colspan = 3;
                socialTable.AddCell(psychoCells2);
            }
            else if (ssb == null)
            {
                PdfPCell psychoCells2 = new PdfPCell(new Phrase("", blackFnt8)); psychoCells2.Colspan = 3;
                socialTable.AddCell(psychoCells2);
            }
        }

        foreach (ScoreSubCategoryConfiguration s in behavioral)
        {
            StudentScoreBehavioral ssb = context.StudentScoreBehaviorals.FirstOrDefault(x => x.SubCategoryId == s.Id && x.StudentId == student.Id && x.AdmissionNo == student.AdmissionNumber && x.SessionId == session && x.TermId == term && x.SchoolId == schoolId && x.ClassId == yearId && x.GradeId == gradeId);
            PdfPCell affectiveCells1 = new PdfPCell(new Phrase(s.SubCategory, blackFnt8)); affectiveCells1.Colspan = 3;
            behaviorTable.AddCell(affectiveCells1);
            if (ssb != null)
            {
                PdfPCell affectiveCells2 = new PdfPCell(new Phrase(Convert.ToInt16(ssb.MarkObtained).ToString(), blackFnt8)); affectiveCells2.Colspan = 3;
                behaviorTable.AddCell(affectiveCells2);
            }
            else if (ssb == null)
            {
                PdfPCell affectiveCells2 = new PdfPCell(new Phrase("", blackFnt8)); affectiveCells2.Colspan = 3;
                behaviorTable.AddCell(affectiveCells2);
            }
        }

        PdfPTable rightTable = new PdfPTable(3);
        PdfPCell rightRow1 = new PdfPCell(socialTable); rightRow1.Colspan = 3;
        PdfPCell rightRow2 = new PdfPCell(gradingTable); rightRow2.Colspan = 3;
        PdfPCell rightRow3 = new PdfPCell(behaviorTable); rightRow3.Colspan = 3;
        rightTable.AddCell(rightRow1);
        rightTable.AddCell(rightRow2);
        rightTable.AddCell(rightRow3);

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
        PASSIS.LIB.Grade objGrade = context.Grades.FirstOrDefault(x => x.Id == gradeId);
        if (objGrade != null)
        {
            PASSIS.LIB.User objUser = context.Users.FirstOrDefault(x => x.Id == objGrade.GradeTeacherId);
            if (objUser != null)
            {
                classTeacherName = objUser.FirstName + " " + objUser.LastName;
            }
        }
        ReportCardNextTermBegin date = context.ReportCardNextTermBegins.OrderByDescending(x => x.SchoolID == logonUser.SchoolId && x.CampusID == logonUser.SchoolCampusId).FirstOrDefault();
        PdfPTable lastTable = new PdfPTable(10);
        PdfPCell EmptyCell = new PdfPCell(new Phrase(" ", blackFnt8)); EmptyCell.Colspan = 10; EmptyCell.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell1Row1.Border = 0;
        PdfPCell lastCell1Row1 = new PdfPCell(new Phrase("Next Term Begins", blackFnt8)); lastCell1Row1.Colspan = 2; lastCell1Row1.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell1Row1.Border = 0;
        lastTable.AddCell(lastCell1Row1);
        if (date != null)
        {
            PdfPCell lastCell2Row1 = new PdfPCell(new Phrase(date.NextTermBegins.ToString(), blackFnt8)); lastCell2Row1.Colspan = 8; lastCell2Row1.HorizontalAlignment = Element.ALIGN_LEFT;// lastCell2Row1.Border = 0;
            lastTable.AddCell(lastCell2Row1);
        }
        else
        {
            PdfPCell lastCell2Row1 = new PdfPCell(new Phrase("", blackFnt8)); lastCell2Row1.Colspan = 8; lastCell2Row1.HorizontalAlignment = Element.ALIGN_LEFT;// lastCell2Row1.Border = 0;
            lastTable.AddCell(lastCell2Row1);
        }

        PdfPCell lastCell1Row2 = new PdfPCell(new Phrase("Form Master's Comment", blackFnt8)); lastCell1Row2.Colspan = 2; lastCell1Row2.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell1Row4.Border = 0;
        PdfPCell lastCell2Row2 = new PdfPCell(new Phrase(classTeacherComment, blackFnt8)); lastCell2Row2.Colspan = 8; lastCell2Row2.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell2Row4.Border = 0;

        PdfPCell lastCell1Row3 = new PdfPCell(new Phrase("Principal's Comment", blackFnt8)); lastCell1Row3.Colspan = 2; lastCell1Row3.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell1Row6.Border = 0;
        PdfPCell lastCell2Row3 = new PdfPCell(new Phrase(headTeacherComment, blackFnt8)); lastCell2Row3.Colspan = 8; lastCell2Row3.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell2Row6.Border = 0;

        PdfPCell lastCell1Row4 = new PdfPCell(new Phrase("Signature", blackFnt8)); lastCell1Row4.Colspan = 2; lastCell1Row4.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell1Row6.Border = 0;
        PdfPCell lastCell2Row4 = new PdfPCell(sign); lastCell2Row4.Colspan = 8; lastCell2Row4.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell2Row6.Border = 0;

        lastTable.AddCell(lastCell1Row2);
        lastTable.AddCell(lastCell2Row2);
        lastTable.AddCell(EmptyCell);
        lastTable.AddCell(lastCell1Row3);
        lastTable.AddCell(lastCell2Row3);
        lastTable.AddCell(EmptyCell);
        lastTable.AddCell(lastCell1Row4);
        lastTable.AddCell(lastCell2Row4);

        PdfPTable baseTable = new PdfPTable(10);
        PdfPCell base1 = new PdfPCell(lastTable); base1.Colspan = 6;
        PdfPCell base2 = new PdfPCell(); base2.Colspan = 2; base2.Border = 0;
        baseTable.AddCell(base1);
        baseTable.AddCell(base2);
        if (Convert.ToInt16(ddlYear.SelectedValue) > 32 && Convert.ToInt16(ddlYear.SelectedValue) < 36)
        {
            PdfPCell base3 = new PdfPCell(gradingTable); base3.Colspan = 2; base3.Border = 0;
            baseTable.AddCell(base3);
        }
        else if (Convert.ToInt16(ddlYear.SelectedValue) > 29 && Convert.ToInt16(ddlYear.SelectedValue) < 33)
        {
            PdfPCell base3 = new PdfPCell(gradingTableJunior); base3.Colspan = 2; base3.Border = 0;
            baseTable.AddCell(base3);
        }
        ////document.Add(new Phrase(Environment.NewLine));
        //if (subjectCounter != 0)
        //{
        //    percentage = (aggregateTotalScore / (subjectCounter * 100)) * 100;
        //}
        //string classTeacherComment = txtClassTeacherComment.Text.Trim();
        //string proprietorComment = txtProprietorComment.Text.Trim();
        //PdfPTable summaryTable = new PdfPTable(10);
        //iTextSharp.text.pdf.draw.LineSeparator line = new iTextSharp.text.pdf.draw.LineSeparator(0f, 100f, BaseColor.BLACK, Element.ALIGN_LEFT, 1);
        //PdfPCell Summarycell1 = new PdfPCell(new Phrase("Total Score Obtained:", blackFnt)); Summarycell1.Colspan = 3; Summarycell1.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell1.Border = 0;
        //PdfPCell Summarycell2 = new PdfPCell(new Phrase(Math.Round(aggregateTotalScore, 0).ToString(), blackFnt)); Summarycell2.Colspan = 1; Summarycell2.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell2.Border = 0;
        //PdfPCell Summarycell3 = new PdfPCell(new Phrase("Total Score Obtainable:", blackFnt)); Summarycell3.Colspan = 2; Summarycell3.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell3.Border = 0;
        //PdfPCell Summarycell4 = new PdfPCell(new Phrase((subjectCounter * 100).ToString(), blackFnt)); Summarycell4.Colspan = 1; Summarycell4.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell4.Border = 0;
        //PdfPCell Summarycell5 = new PdfPCell(new Phrase("Percentage(%):", blackFnt)); Summarycell5.Colspan = 2; Summarycell5.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell5.Border = 0;
        //PdfPCell Summarycell6 = new PdfPCell(new Phrase(Math.Round(percentage, 0).ToString(), blackFnt)); Summarycell6.Colspan = 1; Summarycell6.HorizontalAlignment = Element.ALIGN_LEFT; Summarycell6.Border = 0;

        //PdfPCell summaryRow5Cell1 = new PdfPCell(new Phrase("Number of Subject Offered:", blackFnt)); summaryRow5Cell1.Colspan = 4; summaryRow5Cell1.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow5Cell1.Border = 0;
        //PdfPCell summaryRow5Cell2 = new PdfPCell(new Phrase(subjectCounter.ToString(), blackFnt)); summaryRow5Cell2.Colspan = 6; summaryRow5Cell2.Border = 0;
        //PdfPCell summaryRow5Cell3 = new PdfPCell(new Phrase("Number of Distinctions:", blackFnt)); summaryRow5Cell3.Colspan = 4; summaryRow5Cell3.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow5Cell3.Border = 0;
        //PdfPCell summaryRow5Cell4 = new PdfPCell(new Phrase(distinctionCounter.ToString(), blackFnt)); summaryRow5Cell4.Colspan = 6; summaryRow5Cell4.Border = 0;
        //PdfPCell summaryRow5Cell5 = new PdfPCell(new Phrase("Number of Credits:", blackFnt)); summaryRow5Cell5.Colspan = 4; summaryRow5Cell5.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow5Cell5.Border = 0;
        //PdfPCell summaryRow5Cell6 = new PdfPCell(new Phrase(creditCounter.ToString(), blackFnt)); summaryRow5Cell6.Colspan = 6; summaryRow5Cell6.Border = 0;
        //PdfPCell summaryRow5Cell7 = new PdfPCell(new Phrase("Number of Passes:", blackFnt)); summaryRow5Cell7.Colspan = 4; summaryRow5Cell7.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow5Cell7.Border = 0;
        //PdfPCell summaryRow5Cell8 = new PdfPCell(new Phrase(passesCounter.ToString(), blackFnt)); summaryRow5Cell8.Colspan = 6; summaryRow5Cell8.Border = 0;
        //PdfPCell summaryRow5Cell9 = new PdfPCell(new Phrase("Number of Failure:", blackFnt)); summaryRow5Cell9.Colspan = 4; summaryRow5Cell9.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow5Cell9.Border = 0;
        //PdfPCell summaryRow5Cell10 = new PdfPCell(new Phrase(failureCounter.ToString(), blackFnt)); summaryRow5Cell10.Colspan = 6; summaryRow5Cell10.Border = 0;


        //PdfPCell summaryRow2Cell1 = new PdfPCell(new Phrase("Class Teacher's Comment:", blackFnt)); summaryRow2Cell1.Colspan = 4; summaryRow2Cell1.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow2Cell1.Border = 0;
        //PdfPCell summaryRow2Cell2 = new PdfPCell(new Phrase(classTeacherComment, blackFnt)); summaryRow2Cell2.Colspan = 6; summaryRow2Cell2.Border = 0;

        //PdfPCell summaryRow4Cell1 = new PdfPCell(new Phrase("Principal's Comment:", blackFnt)); summaryRow4Cell1.Colspan = 3; summaryRow4Cell1.HorizontalAlignment = Element.ALIGN_LEFT; summaryRow4Cell1.Border = 0;
        //PdfPCell summaryRow4Cell2 = new PdfPCell(new Phrase(proprietorComment, blackFnt)); summaryRow4Cell2.Colspan = 7; summaryRow4Cell2.Border = 0;

        //summaryTable.AddCell(Summarycell1); summaryTable.AddCell(Summarycell2); summaryTable.AddCell(Summarycell3);
        //summaryTable.AddCell(Summarycell4); summaryTable.AddCell(Summarycell5); summaryTable.AddCell(Summarycell6);
        //summaryTable.AddCell(summaryRow5Cell1); summaryTable.AddCell(summaryRow5Cell2);
        //summaryTable.AddCell(summaryRow5Cell3); summaryTable.AddCell(summaryRow5Cell4);
        //summaryTable.AddCell(summaryRow5Cell5); summaryTable.AddCell(summaryRow5Cell6);
        //summaryTable.AddCell(summaryRow5Cell7); summaryTable.AddCell(summaryRow5Cell8);
        //summaryTable.AddCell(summaryRow5Cell9); summaryTable.AddCell(summaryRow5Cell10);

        //summaryTable.AddCell(summaryRow2Cell1); summaryTable.AddCell(summaryRow2Cell2);
        //summaryTable.AddCell(summaryRow4Cell1); summaryTable.AddCell(summaryRow4Cell2);
        //// The row6cell2 was placed here because we need the calculation to present the total number of subject after counting the subject
        //PdfPCell row6cell2 = new PdfPCell(new Phrase(subjectCounter.ToString())); row6cell2.HorizontalAlignment = Element.ALIGN_LEFT; row6cell2.Colspan = 1; //row6cell2.Border = 0;
        ////maintbl.AddCell(row6cell1);
        ////maintbl.AddCell(row6cell2);
        ////maintbl.AddCell(row6cell3);
        ////maintbl.AddCell(row6cell4);
        PdfPCell row2cell10 = new PdfPCell(new Phrase(Math.Round(Convert.ToDecimal(aggregateTotalScore), 0).ToString(), blackFnt8)); row2cell8.Colspan = 2; row2cell8.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;
        maintbl.AddCell(row2cell8);
        maintbl.AddCell(row2cell9);
        maintbl.AddCell(row2cell10);
        document.Add(maintbl);
        document.Add(new Phrase(Environment.NewLine));
        //document.Add(thirdTerm);
        document.Add(resTable);
        //document.Add(new Phrase(Environment.NewLine));
        document.Add(baseTable);
        //document.Add(lastTable);


    }

    protected void BehavioralPage(Document document, PASSIS.LIB.User student, UsersLIB usrDal)
    {
        document.Add(getBackgroundImage());
        //document.Add(new Phrase(Environment.NewLine));
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
        Paragraph p = new Paragraph("Report For", darkerRedFnt);
        p.Alignment = Element.ALIGN_CENTER;
        document.Add(p);
        Paragraph SessionDetails = new Paragraph(string.Format("{0} {1} {2} Session", ddlAcademicTerm.SelectedItem.Text, ",", ddlAcademicSession.SelectedItem.Text), darkerRedFnt);
        SessionDetails.Alignment = Element.ALIGN_CENTER;
        document.Add(SessionDetails);

        //document.Add(new Phrase(Environment.NewLine));
        document.Add(new Phrase(Environment.NewLine));

        string[] date = student.DateOfBirth.ToString().Split(' ');

        PdfPTable maintbl = new PdfPTable(8);
        PdfPCell cell1 = new PdfPCell(new Phrase("NAME")); cell1.Colspan = 1; cell1.HorizontalAlignment = Element.ALIGN_LEFT; //cell1.Border = 0;
        PdfPCell cell2 = new PdfPCell(new Phrase(student.StudentFullName)); cell2.Colspan = 5; cell1.HorizontalAlignment = Element.ALIGN_LEFT; //cell2.Border = 0;
        PdfPCell cell3 = new PdfPCell(new Phrase("SEX")); cell3.Colspan = 1; cell3.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell cell4 = new PdfPCell(new Phrase(getGender(student.Gender))); cell4.Colspan = 1; cell4.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;

        PdfPCell row2cell1 = new PdfPCell(new Phrase("CLASS")); row2cell1.HorizontalAlignment = Element.ALIGN_LEFT; row2cell1.Colspan = 1; //row2cell1.Border = 0;
        PdfPCell row2cell2 = new PdfPCell(new Phrase(usrDal.getStudentsHomeRoom(student.Id))); row2cell2.Colspan = 7; row2cell2.HorizontalAlignment = Element.ALIGN_LEFT; //row2cell2.Border = 0;

        PdfPCell row3cell1 = new PdfPCell(new Phrase("DATE OF BIRTH")); row3cell1.HorizontalAlignment = Element.ALIGN_LEFT; row3cell1.Colspan = 2; //row3cell1.Border = 0;
        PdfPCell row3cell2 = new PdfPCell(new Phrase(date[0])); row3cell2.HorizontalAlignment = Element.ALIGN_LEFT; row3cell2.Colspan = 6; //row3cell2.Border = 0;

        PdfPCell row4cell1 = new PdfPCell(new Phrase("ADMISSION NUMBER")); row4cell1.Colspan = 3; row4cell1.HorizontalAlignment = Element.ALIGN_LEFT; //row4cell1.Border = 0;
        PdfPCell row4cell2 = new PdfPCell(new Phrase(student.AdmissionNumber)); row4cell2.HorizontalAlignment = Element.ALIGN_LEFT; row4cell2.Colspan = 5; //row4cell2.Border = 0;

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
        document.Add(maintbl);

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

        document.Add(bmaintbl);
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
        ddlYear.Items.Clear();
        ddlGrade.Items.Clear();
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        long studentId = Convert.ToInt64(ddlWard.SelectedValue);
        long sessionId = Convert.ToInt64(ddlAcademicSession.SelectedValue);
        long termId = Convert.ToInt64(ddlAcademicTerm.SelectedValue);

        PASSIS.LIB.AcademicSession academicSession = context.AcademicSessions.FirstOrDefault(x => x.AcademicSessionId == Convert.ToInt64(ddlAcademicSession.SelectedValue) && x.AcademicTermId == Convert.ToInt64(ddlAcademicTerm.SelectedValue)
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

        var getStudentAdmNo = context.Users.FirstOrDefault(x => x.Id == studentId).AdmissionNumber;

        //ReportCardData reportObj = context.ReportCardDatas.FirstOrDefault(x => x.AdmissionNumber == getStudentAdmNo && x.SessionId == sessionId && x.TermId == termId);
        PASSIS.LIB.StudentScore reportObj = context.StudentScores.FirstOrDefault(x => x.AdmissionNumber == getStudentAdmNo && x.AcademicSessionID == sessionId && x.TermId == termId);
        if (reportObj != null)
        {
            long? classId = reportObj.ClassId;
            long? gradeId = reportObj.GradeId;


            var classObj = from s in context.Class_Grades where s.Id == classId select s;
            ddlYear.DataSource = classObj.ToList<PASSIS.LIB.Class_Grade>();
            ddlYear.DataBind();

            var gradeObj = from s in context.Grades where s.Id == gradeId select s;
            ddlGrade.DataSource = gradeObj.ToList<PASSIS.LIB.Grade>();
            ddlGrade.DataBind();

            lblErrorMsg.Visible = false;
            return;
        }
        else
        {
            lblErrorMsg.Text = "No Data for the selected Session & Term";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }

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

        //if (ddlAcademicTerm.SelectedIndex == 0)
        //{
        //    lblErrorMsg.Text = "Kindly select term";
        //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
        //    lblErrorMsg.Visible = true;
        //    return;
        //}


        //BindGrid(Convert.ToInt64(ddlYear.SelectedValue), Convert.ToInt64(ddlGrade.SelectedValue));
        //btnApprove.Visible = true;
        //btnPrintAll.Visible = true;


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

    public ReportCardPosition getClassPosition(string admNo, long termId, long sessionId, long? schId, long yearId, long gradeId)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        ReportCardPosition getPosition = context.ReportCardPositions.FirstOrDefault(s => s.AdmissionNumber == admNo &&
            s.TermId == termId && s.SessionId == sessionId && s.SchoolId == schId && s.YearId == yearId && s.GradeId == gradeId);

        return getPosition;
    }

    protected void ddlAcademicSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlYear.Items.Clear();
        ddlGrade.Items.Clear();
    }
    protected void ddlWard_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlAcademicSession.Items.Clear();
        ddlAcademicSession.DataSource = new ParentViewReportCard_694().schSession().Distinct();
        ddlAcademicSession.DataTextField = "SessionName";
        ddlAcademicSession.DataValueField = "ID";
        ddlAcademicSession.DataBind();
        ddlAcademicSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Session--", "0", true));

        ddlAcademicTerm.Items.Clear();
        ddlAcademicTerm.DataSource = new ParentViewReportCard_694().schTerm().Distinct();
        ddlAcademicTerm.DataTextField = "AcademicTermName";
        ddlAcademicTerm.DataValueField = "Id";
        ddlAcademicTerm.DataBind();
        ddlAcademicTerm.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Term--", "0", true));

        ddlYear.Items.Clear();
        ddlGrade.Items.Clear();
    }

    // Return the int's ordinal extension.
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
    protected void btnPrintBehavior_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlAcademicSession.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select session";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
            }
            else if (ddlAcademicTerm.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select term";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
            }
            //else if (ddlYear.SelectedIndex == 0)
            //{
            //    lblErrorMsg.Text = "Kindly select year";
            //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            //    lblErrorMsg.Visible = true;
            //}
            //else if (ddlGrade.SelectedIndex == 0)
            //{
            //    lblErrorMsg.Text = "Kindly select grade";
            //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            //    lblErrorMsg.Visible = true;
            //}
            else if (ddlWard.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select ward";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
            }
            else
            {
                long campusId, schoolId, subjectId, yearID, gradeId = 0;
                campusId = logonUser.SchoolCampusId;
                schoolId = (long)logonUser.SchoolId;
                //Int64.TryParse(ddlSubject.SelectedValue, out subjectId);
                Int64.TryParse(ddlYear.SelectedValue, out yearID);
                Int64.TryParse(ddlGrade.SelectedValue, out gradeId);
                var scoreList = new ClassGradeLIB().RetrieveSingleGradeStudent(schoolId, campusId, Convert.ToInt64(ddlWard.SelectedValue));
                var selectedUsers = from u in scoreList select u.User;
                bulkPrintingBehavior(selectedUsers.ToList<PASSIS.LIB.User>());
            }
        }
        catch (Exception ex)
        {
        }
    }
}