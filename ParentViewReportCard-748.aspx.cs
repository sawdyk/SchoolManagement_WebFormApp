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

public partial class ParentViewReportCard_748 : PASSIS.LIB.Utility.BasePage
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

            ddlAcademicSession.DataSource = new ParentViewReportCard_748().schSession().Distinct();
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


            ddlAcademicTerm.DataSource = new ParentViewReportCard_748().schTerm().Distinct();
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
                var scoreList = new ClassGradeLIB().RetrieveSingleGradeStudent(schoolId, campusId, yearID, gradeId, Convert.ToInt64(ddlWard.SelectedValue));
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
    private Font darkergrnFnt = FontFactory.GetFont(BaseFont.HELVETICA, 14, BaseColor.GREEN.Darker());
    private Font darkergrayFnt = FontFactory.GetFont(BaseFont.HELVETICA, 14, BaseColor.GRAY);
    private Font darkerGrnFnt = FontFactory.GetFont(BaseFont.HELVETICA, 14, Font.BOLD, new BaseColor(26, 74, 17));
    //Font darkerRedFnt = FontFactory.GetFont("Verdana", 12, Font.BOLD, BaseColor.RED);
    private Font whiteFnt8 = FontFactory.GetFont(BaseFont.HELVETICA, 8, Font.BOLD, BaseColor.WHITE);
    private Font whiteFnt7 = FontFactory.GetFont(BaseFont.HELVETICA, 7, Font.BOLD, BaseColor.WHITE);
    private Font whiteFnt6 = FontFactory.GetFont(BaseFont.HELVETICA, 7, Font.BOLD, BaseColor.WHITE);
    private Font brighterGrnFnt = FontFactory.GetFont(BaseFont.HELVETICA, 12, Font.BOLD, BaseColor.GREEN.Brighter());
    private Font darkerRedFnt = FontFactory.GetFont(BaseFont.HELVETICA, 12, Font.BOLD, new BaseColor(169, 34, 82));
    private Font darkerRedFnt16 = FontFactory.GetFont(BaseFont.HELVETICA, 14, Font.BOLD, new BaseColor(169, 34, 82));
    private Font darkRedFnt = FontFactory.GetFont(BaseFont.HELVETICA, 12, Font.NORMAL, new BaseColor(169, 34, 82));
    private Font darkRedFnt11 = FontFactory.GetFont(BaseFont.HELVETICA, 10, Font.BOLD, new BaseColor(170, 38, 98));
    private Font darkerGrnFnt12 = FontFactory.GetFont(BaseFont.HELVETICA, 12, Font.BOLD, new BaseColor(26, 74, 17));
    private Font darkerGrnFnt8 = FontFactory.GetFont(BaseFont.HELVETICA, 8, Font.BOLD, new BaseColor(26, 74, 17));
    private Font darkerGrnFnt16 = FontFactory.GetFont(BaseFont.HELVETICA, 16, Font.BOLD, new BaseColor(26, 74, 17));
    private Font grnFnt = FontFactory.GetFont(BaseFont.HELVETICA, 14, new BaseColor(26, 74, 17));
    private Font blackFntB = FontFactory.GetFont(BaseFont.HELVETICA, 14, Font.BOLD, new BaseColor(0, 0, 0));
    private Font blackFnt8 = FontFactory.GetFont(BaseFont.HELVETICA, 8, new BaseColor(0, 0, 0));
    private Font blackFnt6 = FontFactory.GetFont(BaseFont.HELVETICA, 6, new BaseColor(0, 0, 0));
    private Font blackFnt10 = FontFactory.GetFont(BaseFont.HELVETICA, 10, new BaseColor(0, 0, 0));
    private Font resultRedFnt = FontFactory.GetFont(BaseFont.HELVETICA, 12, new BaseColor(161, 13, 76));
    private Font resultRedFnt_b = FontFactory.GetFont(BaseFont.HELVETICA, 12, Font.BOLD, new BaseColor(161, 13, 76));
    private Font resultGrnFnt = FontFactory.GetFont(BaseFont.HELVETICA, 12, new BaseColor(170, 38, 98));
    private Font resultGrnFnt_b = FontFactory.GetFont(BaseFont.HELVETICA, 12, Font.BOLD, new BaseColor(170, 38, 98));
    private Font blackFnt12 = FontFactory.GetFont(BaseFont.HELVETICA, 12, new BaseColor(0, 0, 0));
    private Font resultTitleRedFnt10 = FontFactory.GetFont(BaseFont.HELVETICA, 10, Font.BOLD, new BaseColor(161, 13, 76));
    private Font resultTitleRedFnt9 = FontFactory.GetFont(BaseFont.HELVETICA, 9, Font.BOLD, new BaseColor(161, 13, 76));
    private Font resultTitleRedFnt8 = FontFactory.GetFont(BaseFont.HELVETICA, 8, Font.BOLD, new BaseColor(161, 13, 76));
    private Font white = FontFactory.GetFont(BaseFont.TIMES_ROMAN, Font.NORMAL, new BaseColor(255, 255, 255));

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


        if (ddlAcademicTerm.SelectedValue == "1" || ddlAcademicTerm.SelectedValue == "2")
        {
            addResultSummaryPageTaiSolarinSecondaryFirst(document, student, usrDal);
        }
        else if (ddlAcademicTerm.SelectedValue == "3")
        {
            addResultSummaryPageTaiSolarinSecondaryThird(document, student, usrDal);
        }

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
        document.SetMargins(0f, 10f, 10f, 0f);
        MemoryStream mem = new MemoryStream(); // PDF data will be written here
        PdfWriter.GetInstance(document, mem);  // tie a PdfWriter instance to the stream

        if (ddlAcademicTerm.SelectedValue == "1" || ddlAcademicTerm.SelectedValue == "2")
        {
            document.SetPageSize(iTextSharp.text.PageSize.A4_LANDSCAPE);
        }
        else if (ddlAcademicTerm.SelectedValue == "3")
        {
            document.SetPageSize(iTextSharp.text.PageSize.A4_LANDSCAPE.Rotate());
        }
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

    protected void addResultSummaryPageTaiSolarinSecondaryFirst(Document document, PASSIS.LIB.User student, UsersLIB usrDal)
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
        PdfPCell innerCell1 = new PdfPCell(new Phrase("TAI SOLARIN MEMORIAL SECONDARY SCHOOL", darkerRedFnt16)); innerCell1.Colspan = 9; innerCell1.Border = 0; innerCell1.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell innerCell2 = new PdfPCell(new Phrase("PLOT 772 NEW TOWN ROAD AMUWO ODOFIN LAGOS STATE", blackFnt8)); innerCell2.Colspan = 9; innerCell2.Border = 0; innerCell2.HorizontalAlignment = Element.ALIGN_CENTER;
        //PdfPCell innerCell3 = new PdfPCell(new Phrase("Tel: 08164584442, 08085687700", resultTitleRedFnt8)); innerCell3.Colspan = 9; innerCell3.Border = 0; innerCell3.HorizontalAlignment = Element.ALIGN_CENTER;
        //PdfPCell innerCell4 = new PdfPCell(new Phrase("MOTTO: Work Hard, Pray Hard and Keep Straight", blackFnt8)); innerCell4.Colspan = 9; innerCell4.Border = 0; innerCell4.HorizontalAlignment = Element.ALIGN_CENTER;
        innerTable.AddCell(innerCell1);
        innerTable.AddCell(innerCell2);
        //innerTable.AddCell(innerCell3);
        //innerTable.AddCell(innerCell4);


        PdfPTable head = new PdfPTable(15);
        PdfPCell head1 = new PdfPCell(jpg); head1.Colspan = 2; head1.Border = 0;
        PdfPCell head2 = new PdfPCell(innerTable); head2.Colspan = 13; head2.Border = 0; head2.HorizontalAlignment = Element.ALIGN_CENTER;
        //PdfPCell head3 = new PdfPCell(jpg1); head3.Colspan = 2; head3.HorizontalAlignment = Element.ALIGN_RIGHT; head3.Border = 0;


        head.AddCell(head1);
        head.AddCell(head2);
        //head.AddCell(head3);
        document.Add(head);



        //if (Convert.ToInt16(ddlYear.SelectedValue) > 32 && Convert.ToInt16(ddlYear.SelectedValue) < 36)
        //{
        //    Paragraph transcript = new Paragraph(string.Format("{0}", "SENIOR SECONDARY SCHOOL TERMINAL REPORT SHEET"), darkerRedFnt);
        //    transcript.Alignment = Element.ALIGN_CENTER;
        //    document.Add(transcript);
        //}
        //else if (Convert.ToInt16(ddlYear.SelectedValue) > 29 && Convert.ToInt16(ddlYear.SelectedValue) < 33)
        //{
        //    Paragraph transcript = new Paragraph(string.Format("{0}", "JUNIOR SECONDARY SCHOOL TERMINAL REPORT SHEET"), darkerRedFnt);
        //    transcript.Alignment = Element.ALIGN_CENTER;
        //    document.Add(transcript);
        //}

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

        string totalNoInClass = new ClassGradeLIB().RetrieveGradeStudents(Convert.ToInt64(schId), Convert.ToInt64(logonUser.SchoolCampusId), Convert.ToInt64(yearId), gradeIdd, session).Count().ToString();
        ReportCardNextTermBegin date = context.ReportCardNextTermBegins.OrderByDescending(x => x.SchoolID == logonUser.SchoolId && x.CampusID == logonUser.SchoolCampusId).FirstOrDefault();

        PdfPTable maintbl = new PdfPTable(8);
        PdfPCell cell1 = new PdfPCell(new Phrase("FIRST NAME", blackFnt8)); cell1.Colspan = 2; cell1.HorizontalAlignment = Element.ALIGN_LEFT; //cell1.Border = 0;
        PdfPCell cell2 = new PdfPCell(new Phrase(student.FirstName.ToUpper(), blackFnt8)); cell2.Colspan = 3; cell1.HorizontalAlignment = Element.ALIGN_LEFT; //cell2.Border = 0;
        PdfPCell cell3 = new PdfPCell(new Phrase("TERM", blackFnt8)); cell3.Colspan = 2; cell3.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell cell4 = new PdfPCell(new Phrase(ddlAcademicTerm.SelectedItem.ToString(), blackFnt8)); cell4.Colspan = 1; cell4.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;

        PdfPCell cell5 = new PdfPCell(new Phrase("SURNAME", blackFnt8)); cell5.Colspan = 2; cell5.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell cell6 = new PdfPCell(new Phrase(student.LastName.ToUpper(), blackFnt8)); cell6.Colspan = 3; cell6.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell cell7 = new PdfPCell(new Phrase("SESSION", blackFnt8)); cell7.Colspan = 2; cell7.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell cell8 = new PdfPCell(new Phrase(ddlAcademicSession.SelectedItem.ToString(), blackFnt8)); cell8.Colspan = 1; cell8.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;

        PdfPCell cell9 = new PdfPCell(new Phrase("GENDER", blackFnt8)); cell9.Colspan = 2; cell9.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell cell10 = new PdfPCell(new Phrase(getGender(student.Gender).ToString(), blackFnt8)); cell10.Colspan = 3; cell10.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell cell11 = new PdfPCell(new Phrase("NUMBER IN CLASS", blackFnt8)); cell11.Colspan = 2; cell11.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell cell12 = new PdfPCell(new Phrase(totalNoInClass.ToString(), blackFnt8)); cell12.Colspan = 1; cell12.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;

        PdfPCell cell13 = new PdfPCell(new Phrase("CLASS", blackFnt8)); cell13.Colspan = 2; cell13.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell cell14 = new PdfPCell(new Phrase(ddlYear.SelectedItem.ToString(), blackFnt8)); cell14.Colspan = 3; cell14.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell cell15 = new PdfPCell(new Phrase("POSITION IN CLASS", blackFnt8)); cell15.Colspan = 2; cell15.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell cell16 = new PdfPCell(new Phrase(classPosition.ToString() + "" + ToOrdinal(Convert.ToInt16(classPosition)), blackFnt8)); cell16.Colspan = 1; cell16.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;

        PdfPCell cell17 = new PdfPCell(new Phrase("", blackFnt8)); cell17.Colspan = 5; cell17.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        //PdfPCell cell18 = new PdfPCell(new Phrase("", blackFnt8)); cell18.Colspan = 3; cell18.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell cell19 = new PdfPCell(new Phrase("NEXT TERM BEGINS", blackFnt8)); cell19.Colspan = 2; cell19.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;



        maintbl.AddCell(cell1);
        maintbl.AddCell(cell2);
        maintbl.AddCell(cell3);
        maintbl.AddCell(cell4);
        maintbl.AddCell(cell5);
        maintbl.AddCell(cell6);
        maintbl.AddCell(cell7);
        maintbl.AddCell(cell8);
        maintbl.AddCell(cell9);
        maintbl.AddCell(cell10);
        maintbl.AddCell(cell11);
        maintbl.AddCell(cell12);
        maintbl.AddCell(cell13);
        maintbl.AddCell(cell14);
        maintbl.AddCell(cell15);
        maintbl.AddCell(cell16);
        maintbl.AddCell(cell17);
        //maintbl.AddCell(cell18);
        maintbl.AddCell(cell19);
        if (date != null)
        {
            PdfPCell cell20 = new PdfPCell(new Phrase(Convert.ToDateTime(date.NextTermBegins.ToString()).ToString("dd/M/yyyy"), blackFnt8)); cell20.Colspan = 1; cell20.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
            maintbl.AddCell(cell20);
        }
        else
        {
            PdfPCell cell20 = new PdfPCell(new Phrase("", blackFnt8)); cell20.Colspan = 1; cell20.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
            maintbl.AddCell(cell20);
        }




        //maintbl.AddCell(row2cell1);
        //maintbl.AddCell(row2cell2);
        //maintbl.AddCell(row2cell3);
        //maintbl.AddCell(row2cell4);
        //maintbl.AddCell(row2cell5);
        //maintbl.AddCell(row2cell6);
        //maintbl.AddCell(row2cell7);
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

        PdfPTable headd = new PdfPTable(15);
        PdfPCell headd1 = new PdfPCell(maintbl); headd1.Colspan = 13;
        PdfPCell headd2 = new PdfPCell(jpg1); headd2.Colspan = 2; headd2.Border = 0; headd2.HorizontalAlignment = Element.ALIGN_CENTER; headd2.Border = 0;
        //PdfPCell head3 = new PdfPCell(jpg1); head3.Colspan = 2; head3.HorizontalAlignment = Element.ALIGN_RIGHT; head3.Border = 0;


        headd.AddCell(headd1);
        headd.AddCell(headd2);
        document.Add(headd);



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
        PdfPTable resTable = new PdfPTable(9);
        //PdfPCell thirdRow1Cell1 = new PdfPCell(new Phrase("3RD TERM RESULT", blackFnt10)); thirdRow1Cell1.Colspan = 8; thirdRow1Cell1.HorizontalAlignment = Element.ALIGN_CENTER;

        PdfPCell thirdRow2Cell1 = new PdfPCell(new Phrase("SUBJECT", whiteFnt7)); thirdRow2Cell1.Colspan = 3; thirdRow2Cell1.HorizontalAlignment = Element.ALIGN_CENTER; thirdRow2Cell1.BackgroundColor = darkergrayFnt.Color;
        PdfPCell thirdRow2Cell2 = new PdfPCell(new Phrase("CA 40%", whiteFnt7)); thirdRow2Cell2.Colspan = 1; thirdRow2Cell2.HorizontalAlignment = Element.ALIGN_CENTER; thirdRow2Cell2.BackgroundColor = darkergrayFnt.Color;
        PdfPCell thirdRow2Cell3 = new PdfPCell(new Phrase("EXAM 60%", whiteFnt7)); thirdRow2Cell3.Colspan = 1; thirdRow2Cell3.HorizontalAlignment = Element.ALIGN_CENTER; thirdRow2Cell3.BackgroundColor = darkergrayFnt.Color;
        PdfPCell thirdRow2Cell4 = new PdfPCell(new Phrase("TOTAL 100%", whiteFnt7)); thirdRow2Cell4.Colspan = 1; thirdRow2Cell4.HorizontalAlignment = Element.ALIGN_CENTER; thirdRow2Cell4.BackgroundColor = darkergrayFnt.Color;
        PdfPCell thirdRow2Cell5 = new PdfPCell(new Phrase("GRADE", whiteFnt7)); thirdRow2Cell5.Colspan = 1; thirdRow2Cell5.HorizontalAlignment = Element.ALIGN_CENTER; thirdRow2Cell5.BackgroundColor = darkergrayFnt.Color;
        //PdfPCell thirdRow2Cell6 = new PdfPCell(new Phrase("POSITION", whiteFnt7)); thirdRow2Cell6.Colspan = 1; thirdRow2Cell6.HorizontalAlignment = Element.ALIGN_CENTER; thirdRow2Cell6.BackgroundColor = darkergrnFnt.Color;
        PdfPCell thirdRow2Cell7 = new PdfPCell(new Phrase("REMARK", whiteFnt7)); thirdRow2Cell7.Colspan = 2; thirdRow2Cell7.HorizontalAlignment = Element.ALIGN_CENTER; thirdRow2Cell7.BackgroundColor = darkergrayFnt.Color;
        //PdfPCell thirdRow2Cell8 = new PdfPCell(new Phrase("Annual Score Grade", blackFnt8)); thirdRow2Cell8.Colspan = 1; thirdRow2Cell8.HorizontalAlignment = Element.ALIGN_CENTER;
        Paragraph domain = new Paragraph(string.Format("{0}", " "), darkerRedFnt);
        domain.Alignment = Element.ALIGN_CENTER;

        Paragraph domain1 = new Paragraph(string.Format("{0}", " "), darkerRedFnt);
        domain1.Alignment = Element.ALIGN_CENTER;

        //thirdTerm.AddCell(thirdRow1Cell1);

        resTable.AddCell(thirdRow2Cell1);
        resTable.AddCell(thirdRow2Cell2);
        resTable.AddCell(thirdRow2Cell3);
        resTable.AddCell(thirdRow2Cell4);
        resTable.AddCell(thirdRow2Cell5);
        //resTable.AddCell(thirdRow2Cell6);
        resTable.AddCell(thirdRow2Cell7);
        //resTable.AddCell(thirdRow2Cell8);
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

        long examCatPercentage = 0;
        decimal examPercentageScore = 0;
        decimal testScoreObtained = 0;
        long testCatPercentage = 0;
        decimal testPercentageScore = 0;

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

                    //if (ddlAcademicTerm.SelectedValue == "1")
                    //{

                    ReportCardPrintConfig checkExist = context.ReportCardPrintConfigs.FirstOrDefault(x => x.SchoolId == logonUser.SchoolId && x.CampusId == logonUser.SchoolCampusId
                        && x.ClassId == Convert.ToInt64(ddlYear.SelectedValue) && x.GradeId == Convert.ToInt64(ddlGrade.SelectedValue) && x.SessionId == Convert.ToInt64(ddlAcademicSession.SelectedValue)
                        && x.TermId == Convert.ToInt64(ddlAcademicTerm.SelectedValue) && x.SubjectId == subjects.SubjectId);

                    if (checkExist != null && checkExist.Exam == true && checkExist.CA == false) //Only Exam
                    {
                        //decimal examScoreObtained = 0;
                        //long examCatPercentage = 0;
                        //decimal examPercentageScore = 0;
                        //long maxScore = Convert.ToInt64(subjects.MaximumScore);

                        ScoreCategoryConfiguration scoreCatConfig = context.ScoreCategoryConfigurations.FirstOrDefault(x => x.SchoolId == schoolId && x.CampusId == logonUser.SchoolCampusId && x.ClassId == yearId && x.SessionId == session && x.TermId == term && x.Category.Trim() == "Exam");
                        if (scoreCatConfig != null)
                        {
                            examCatPercentage = Convert.ToInt64(scoreCatConfig.Percentage);
                        }

                        PdfPCell subjCell = new PdfPCell(new Phrase(subject.Name, darkerGrnFnt8)); subjCell.Colspan = 3;
                        resTable.AddCell(subjCell);
                        PdfPCell CACell = new PdfPCell(new Phrase("", darkerGrnFnt8)); CACell.Colspan = 1;
                        resTable.AddCell(CACell);
                        PdfPCell ExamCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); ExamCell.Colspan = 1;
                        resTable.AddCell(ExamCell);

                        PASSIS.LIB.StudentScore stdScore = getSubjectScoreCategoryExam(student.AdmissionNumber, student.Id, session, term, schoolId, yearId, gradeId, subject.Id);
                        //IList<PASSIS.LIB.StudentScore> sortedScoreRepoByCategory = stdScore.OrderBy(x => x.SubCategoryId).ToList(); //fetch all Exam Scores 

                        if (stdScore != null)
                        {
                                examPercentageScore = Math.Round((decimal)stdScore.ExamScore, 0);

                            if (examPercentageScore > 0 && examCatPercentage > 0)
                            {
                                totalScore = (examPercentageScore / examCatPercentage) * (decimal)subjects.MaximumScore;
                            }
                            else
                            {
                                totalScore = 0;
                            }

                            if (totalScore > 0)
                            {
                                subjectCounter++;
                                aggregateTotalScore += Math.Round(totalScore, 0);

                                PdfPCell TotalFirstCell = new PdfPCell(new Phrase(Math.Round(totalScore, 0).ToString(), darkerGrnFnt8)); TotalFirstCell.Colspan = 1;
                                resTable.AddCell(TotalFirstCell);
                                PdfPCell gradeCell = new PdfPCell(new Phrase((PASSIS.LIB.Utility.Utili.getExamGradeLetters(Math.Round(totalScore), (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue))), darkerGrnFnt8)); gradeCell.Colspan = 1;
                                resTable.AddCell(gradeCell);
                                PdfPCell tRemark = new PdfPCell(new Phrase(PASSIS.LIB.Utility.Utili.getExamGradeRemarks(Math.Round(totalScore), (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)), darkerGrnFnt8)); tRemark.Colspan = 2;
                                resTable.AddCell(tRemark);
                            }
                            else
                            {
                                PdfPCell TotalFirstCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); TotalFirstCell.Colspan = 1;
                                resTable.AddCell(TotalFirstCell);
                                PdfPCell gradeCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); gradeCell.Colspan = 1;
                                resTable.AddCell(gradeCell);
                                PdfPCell tRemark = new PdfPCell(new Phrase("", darkerGrnFnt8)); tRemark.Colspan = 2;
                                resTable.AddCell(tRemark);
                            }
                        }
                        else
                        {
                            PdfPCell TotalFirstCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); TotalFirstCell.Colspan = 1;
                            resTable.AddCell(TotalFirstCell);
                            PdfPCell gradeCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); gradeCell.Colspan = 1;
                            resTable.AddCell(gradeCell);
                            PdfPCell tRemark = new PdfPCell(new Phrase("", darkerGrnFnt8)); tRemark.Colspan = 2;
                            resTable.AddCell(tRemark);
                        }

                        //if (totalScore > 0)
                        //{
                        //    subjectCounter++;
                        //}

                    }


                    else if (checkExist != null && checkExist.Exam == false && checkExist.CA == true) //Only CA
                    {
                        //decimal testScoreObtained = 0;
                        //long testCatPercentage = 0;
                        //decimal testPercentageScore = 0;
                        //long maxScore = Convert.ToInt64(subjects.MaximumScore);

                        ScoreCategoryConfiguration scoreCatConfig = context.ScoreCategoryConfigurations.FirstOrDefault(x => x.SchoolId == schoolId && x.CampusId == logonUser.SchoolCampusId && x.ClassId == yearId && x.SessionId == session && x.TermId == term && x.Category.Trim() == "CA");
                        if (scoreCatConfig != null)
                        {
                            testCatPercentage = Convert.ToInt64(scoreCatConfig.Percentage);
                        }

                        PdfPCell subjCell = new PdfPCell(new Phrase(subject.Name, darkerGrnFnt8)); subjCell.Colspan = 3;
                        resTable.AddCell(subjCell);
                        PdfPCell CACell = new PdfPCell(new Phrase("", darkerGrnFnt8)); CACell.Colspan = 1;
                        resTable.AddCell(CACell);
                        PdfPCell ExamCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); ExamCell.Colspan = 1;
                        resTable.AddCell(ExamCell);

                        IList<PASSIS.LIB.StudentScoreRepository> scoreRepo = getSubjectScoreCategoryCA(student.AdmissionNumber, student.Id, session, term, schoolId, yearId, gradeId, subject.Id);
                        //IList<PASSIS.LIB.StudentScoreRepository> sortedScoreRepoByCategory = scoreRepo.OrderBy(x => x.SubCategoryId).ToList(); //fetch all continuous assessment (CA) scores 
                        if (scoreRepo.Count > 0)
                        {
                            foreach (PASSIS.LIB.StudentScoreRepository s in scoreRepo)
                            {
                                testPercentageScore = (decimal)s.MarkObtained;
                            }

                            if (testPercentageScore > 0 && testCatPercentage > 0)
                            {
                                totalScore = (testPercentageScore / testCatPercentage) * (decimal)subjects.MaximumScore;
                            }
                            else
                            {
                                totalScore = 0;
                            }

                            if (totalScore > 0)
                            {
                                subjectCounter++;
                                aggregateTotalScore += Math.Round(totalScore, 0);

                                PdfPCell TotalFirstCell = new PdfPCell(new Phrase(Math.Round(totalScore, 0).ToString(), darkerGrnFnt8)); TotalFirstCell.Colspan = 1;
                                resTable.AddCell(TotalFirstCell);
                                PdfPCell gradeCell = new PdfPCell(new Phrase((PASSIS.LIB.Utility.Utili.getExamGradeLetters(Math.Round(totalScore), (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue))), darkerGrnFnt8)); gradeCell.Colspan = 1;
                                resTable.AddCell(gradeCell);
                                PdfPCell tRemark = new PdfPCell(new Phrase(PASSIS.LIB.Utility.Utili.getExamGradeRemarks(Math.Round(totalScore), (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)), darkerGrnFnt8)); tRemark.Colspan = 2;
                                resTable.AddCell(tRemark);
                            }
                            else
                            {
                                PdfPCell TotalFirstCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); TotalFirstCell.Colspan = 1;
                                resTable.AddCell(TotalFirstCell);
                                PdfPCell gradeCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); gradeCell.Colspan = 1;
                                resTable.AddCell(gradeCell);
                                PdfPCell tRemark = new PdfPCell(new Phrase("", darkerGrnFnt8)); tRemark.Colspan = 2;
                                resTable.AddCell(tRemark);
                            }
                        }
                        else
                        {
                            PdfPCell TotalFirstCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); TotalFirstCell.Colspan = 1;
                            resTable.AddCell(TotalFirstCell);
                            PdfPCell gradeCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); gradeCell.Colspan = 1;
                            resTable.AddCell(gradeCell);
                            PdfPCell tRemark = new PdfPCell(new Phrase("", darkerGrnFnt8)); tRemark.Colspan = 2;
                            resTable.AddCell(tRemark);
                        }

                        //if (totalScore > 0)
                        //{
                        //    subjectCounter++;
                        //}

                    }

                    else //CA AND EXAM
                    {
                        IList<PASSIS.LIB.StudentScoreRepository> scoreRepoFirstTerm = getSubjectScoreCategoryCA(student.AdmissionNumber, student.Id, session, term, schoolId, yearId, gradeId, subject.Id);
                        PdfPCell subjCell = new PdfPCell(new Phrase(subject.Name, darkerGrnFnt8)); subjCell.Colspan = 3;
                        resTable.AddCell(subjCell);
                        if (scoreRepoFirstTerm.Count > 0)
                        {
                            foreach (PASSIS.LIB.StudentScoreRepository fs in scoreRepoFirstTerm)
                            {
                                testScore = Convert.ToDecimal(fs.MarkObtained);
                                PdfPCell CACell = new PdfPCell(new Phrase(Math.Round(testScore, 0).ToString(), darkerGrnFnt8)); CACell.Colspan = 1;
                                resTable.AddCell(CACell);
                                break;
                            }
                        }
                        else
                        {
                            testScore = 0;
                            PdfPCell CACell = new PdfPCell(new Phrase("", darkerGrnFnt8)); CACell.Colspan = 1;
                            resTable.AddCell(CACell);
                        }

                        PASSIS.LIB.StudentScore scoreRepoExamFirstTerm = getSubjectScoreCategoryExam(student.AdmissionNumber, student.Id, session, term, schoolId, yearId, gradeId, subject.Id);
                        if (scoreRepoExamFirstTerm != null)
                        {
                                examScore = Convert.ToDecimal(scoreRepoExamFirstTerm.ExamScore);
                                PdfPCell ExamCell = new PdfPCell(new Phrase(Math.Round(examScore, 0).ToString(), darkerGrnFnt8)); ExamCell.Colspan = 1;
                                resTable.AddCell(ExamCell);
                        }
                        else
                        {
                            examScore = 0;
                            PdfPCell ExamCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); ExamCell.Colspan = 1;
                            resTable.AddCell(ExamCell);
                        }

                        totalScore = testScore + examScore;
                        aggregateTotalScore += Math.Round(totalScore, 0);

                        if (totalScore > 0)
                        {
                            subjectCounter++;
                        }

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

                        if (totalScore > 0)
                        {
                            PdfPCell gradeCell = new PdfPCell(new Phrase((PASSIS.LIB.Utility.Utili.getExamGradeLetters(Math.Round(totalScore), (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue))), darkerGrnFnt8)); gradeCell.Colspan = 1;
                            resTable.AddCell(gradeCell);

                            //resTable.AddCell(space1);

                            PdfPCell tRemark = new PdfPCell(new Phrase(PASSIS.LIB.Utility.Utili.getExamGradeRemarks(Math.Round(totalScore), (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)), darkerGrnFnt8)); tRemark.Colspan = 2;
                            resTable.AddCell(tRemark);
                        }
                        else
                        {
                            PdfPCell gradeCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); gradeCell.Colspan = 1;
                            resTable.AddCell(gradeCell);

                            //resTable.AddCell(space1);

                            PdfPCell tRemark = new PdfPCell(new Phrase("", darkerGrnFnt8)); tRemark.Colspan = 2;
                            resTable.AddCell(tRemark);
                        }
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
                percentage = Math.Round(Convert.ToDecimal((aggregateTotalScore / (subjectCounter * 100)) * 100), 1);
            }

            subjectCounter = 0;
        }

        PdfPTable psychomotorTable = new PdfPTable(4);
        PdfPCell row1col1 = new PdfPCell(new Phrase("PSYCHOMOTOR SKILLS", blackFnt6)); row1col1.Colspan = 3; row1col1.HorizontalAlignment = Element.ALIGN_LEFT; /*row1col1.BackgroundColor = darkergrnFnt.Color;*/
        PdfPCell row1col2 = new PdfPCell(new Phrase("RATES", blackFnt6)); row1col2.Colspan = 1; /*row1col2.BackgroundColor = darkergrnFnt.Color;*/
        //PdfPCell row2col1 = new PdfPCell(new Phrase("HAND WRITING", blackFnt6)); row2col1.Colspan = 3; row2col1.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell row2col2 = new PdfPCell(new Phrase("2", blackFnt6)); row2col2.Colspan = 1; 

        psychomotorTable.AddCell(row1col1);
        psychomotorTable.AddCell(row1col2);
        //psychomotorTable.AddCell(row2col1);
        //psychomotorTable.AddCell(row2col2);



        PdfPTable affectiveTable = new PdfPTable(4);
        PdfPCell affectiverow1col1 = new PdfPCell(new Phrase("AFFECTIVE AREAS", blackFnt6)); affectiverow1col1.Colspan = 3; affectiverow1col1.HorizontalAlignment = Element.ALIGN_LEFT; /*affectiverow1col1.BackgroundColor = darkergrnFnt.Color;*/
        PdfPCell affectiverow2col1 = new PdfPCell(new Phrase("RATES", blackFnt6)); affectiverow2col1.Colspan = 1; /*affectiverow2col1.BackgroundColor = darkergrnFnt.Color;*/
        //PdfPCell affectiverow1col2 = new PdfPCell(new Phrase("PUCTUALITY", blackFnt6)); affectiverow1col2.Colspan = 3; affectiverow1col2.HorizontalAlignment = Element.ALIGN_LEFT; 
        //PdfPCell affectiverow2col2 = new PdfPCell(new Phrase("5", blackFnt6)); affectiverow2col2.Colspan = 1;

        affectiveTable.AddCell(affectiverow1col1);
        affectiveTable.AddCell(affectiverow2col1);
        //affectiveTable.AddCell(affectiverow1col2);
        //affectiveTable.AddCell(affectiverow2col2);

        PdfPTable scaleTable = new PdfPTable(3);
        PdfPCell scalerow0col1 = new PdfPCell(new Phrase("KEY TO RATING", blackFnt6)); scalerow0col1.Colspan = 2; scalerow0col1.HorizontalAlignment = Element.ALIGN_CENTER; /*scalerow0col1.BackgroundColor = darkergrnFnt.Color;*/
        PdfPCell scalerow1col1 = new PdfPCell(new Phrase("RATES", blackFnt6)); scalerow1col1.Colspan = 1; /*scalerow1col1.Border = 0;*/
        PdfPCell scalerow2col1 = new PdfPCell(new Phrase("EXCELLENT LEVEL", blackFnt6)); scalerow2col1.Colspan = 2; /*scalerow2col1.Border = 0;*/
        PdfPCell scalerow3col1 = new PdfPCell(new Phrase("5", blackFnt6)); scalerow3col1.Colspan = 1; /*scalerow3col1.Border = 0;*/
        PdfPCell scalerow4col1 = new PdfPCell(new Phrase("HIGH LEVEL", blackFnt6)); scalerow4col1.Colspan = 2; /*scalerow4col1.Border = 0;*/
        PdfPCell scalerow5col1 = new PdfPCell(new Phrase("4", blackFnt6)); scalerow5col1.Colspan = 1; /*scalerow5col1.Border = 0;*/
        PdfPCell scalerow6col1 = new PdfPCell(new Phrase("ACCEPTABLE LEVEL", blackFnt6)); scalerow6col1.Colspan = 2; /*scalerow6col1.Border = 0;*/
        PdfPCell scalerow7col1 = new PdfPCell(new Phrase("3", blackFnt6)); scalerow7col1.Colspan = 1;/* scalerow7col1.Border = 0;*/
        PdfPCell scalerow8col1 = new PdfPCell(new Phrase("MINIMAL LEVEL", blackFnt6)); scalerow8col1.Colspan = 2; /*scalerow8col1.Border = 0;*/
        PdfPCell scalerow9col1 = new PdfPCell(new Phrase("2", blackFnt6)); scalerow9col1.Colspan = 1; /*scalerow9col1.Border = 0;*/
        PdfPCell scalerow10col1 = new PdfPCell(new Phrase("LOW LEVEL", blackFnt6)); scalerow10col1.Colspan = 2; /*scalerow10col1.Border = 0;*/
        PdfPCell scalerow11col1 = new PdfPCell(new Phrase("1", blackFnt6)); scalerow11col1.Colspan = 1; /*scalerow5col1.Border = 0;*/


        scaleTable.AddCell(scalerow0col1);
        scaleTable.AddCell(scalerow1col1);
        scaleTable.AddCell(scalerow2col1);
        scaleTable.AddCell(scalerow3col1);
        scaleTable.AddCell(scalerow4col1);
        scaleTable.AddCell(scalerow5col1);
        scaleTable.AddCell(scalerow6col1);
        scaleTable.AddCell(scalerow7col1);
        scaleTable.AddCell(scalerow8col1);
        scaleTable.AddCell(scalerow9col1);
        scaleTable.AddCell(scalerow10col1);
        scaleTable.AddCell(scalerow11col1);


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
            psychomotorTable.AddCell(psychoCells1);
            if (ssb != null)
            {
                PdfPCell psychoCells2 = new PdfPCell(new Phrase(Convert.ToInt16(ssb.MarkObtained).ToString(), blackFnt8)); psychoCells2.Colspan = 1;
                psychomotorTable.AddCell(psychoCells2);
            }
            else if (ssb == null)
            {
                PdfPCell psychoCells2 = new PdfPCell(new Phrase("", blackFnt8)); psychoCells2.Colspan = 1;
                psychomotorTable.AddCell(psychoCells2);
            }
        }

        foreach (ScoreSubCategoryConfiguration s in behavioral)
        {
            StudentScoreBehavioral ssb = context.StudentScoreBehaviorals.FirstOrDefault(x => x.SubCategoryId == s.Id && x.StudentId == student.Id && x.AdmissionNo == student.AdmissionNumber && x.SessionId == session && x.TermId == term && x.SchoolId == schoolId && x.ClassId == yearId && x.GradeId == gradeId);
            PdfPCell affectiveCells1 = new PdfPCell(new Phrase(s.SubCategory, blackFnt8)); affectiveCells1.Colspan = 3;
            affectiveTable.AddCell(affectiveCells1);
            if (ssb != null)
            {
                PdfPCell affectiveCells2 = new PdfPCell(new Phrase(Convert.ToInt16(ssb.MarkObtained).ToString(), blackFnt8)); affectiveCells2.Colspan = 1;
                affectiveTable.AddCell(affectiveCells2);
            }
            else if (ssb == null)
            {
                PdfPCell affectiveCells2 = new PdfPCell(new Phrase("", blackFnt8)); affectiveCells2.Colspan = 1;
                affectiveTable.AddCell(affectiveCells2);
            }
        }


        PdfPTable baseTbl = new PdfPTable(11);
        PdfPCell psych = new PdfPCell(psychomotorTable); psych.Colspan = 4;
        PdfPCell scale = new PdfPCell(scaleTable); scale.Colspan = 3;
        PdfPCell affectiveAreas = new PdfPCell(affectiveTable); affectiveAreas.Colspan = 4;

        baseTbl.AddCell(psych);
        baseTbl.AddCell(affectiveAreas);
        baseTbl.AddCell(scale);




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
        //ReportCardNextTermBegin date = context.ReportCardNextTermBegins.OrderByDescending(x => x.SchoolID == logonUser.SchoolId && x.CampusID == logonUser.SchoolCampusId).FirstOrDefault();
        //PdfPTable lastTable = new PdfPTable(10);
        //PdfPCell EmptyCell = new PdfPCell(new Phrase(" ", blackFnt8)); EmptyCell.Colspan = 10; EmptyCell.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell1Row1.Border = 0;
        //PdfPCell lastCell1Row1 = new PdfPCell(new Phrase("Next Term Begins", blackFnt8)); lastCell1Row1.Colspan = 2; lastCell1Row1.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell1Row1.Border = 0;
        //lastTable.AddCell(lastCell1Row1);
        //if (date != null)
        //{
        //    PdfPCell lastCell2Row1 = new PdfPCell(new Phrase(date.NextTermBegins.ToString(), blackFnt8)); lastCell2Row1.Colspan = 8; lastCell2Row1.HorizontalAlignment = Element.ALIGN_LEFT;// lastCell2Row1.Border = 0;
        //    lastTable.AddCell(lastCell2Row1);
        //}
        //else
        //{
        //    PdfPCell lastCell2Row1 = new PdfPCell(new Phrase("", blackFnt8)); lastCell2Row1.Colspan = 8; lastCell2Row1.HorizontalAlignment = Element.ALIGN_LEFT;// lastCell2Row1.Border = 0;
        //    lastTable.AddCell(lastCell2Row1);
        //}



        PdfPTable commentsTable = new PdfPTable(7);

        PdfPCell commentsRow1Cell1 = new PdfPCell(new Phrase("TOTAL SCORES", blackFnt6)); commentsRow1Cell1.Colspan = 2; commentsRow1Cell1.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow1Cell1.Border = 0;*/
        PdfPCell commentsRow1Cell2 = new PdfPCell(new Phrase((aggregateTotalScore).ToString(), blackFnt6)); commentsRow1Cell2.Colspan = 5; commentsRow1Cell2.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow1Cell2.Border = 0;*/

        PdfPCell commentsRow2Cell1 = new PdfPCell(new Phrase("PERCENTAGE", blackFnt6)); commentsRow2Cell1.Colspan = 2; commentsRow2Cell1.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow2Cell1.Border = 0;*/
        PdfPCell commentsRow2Cell2 = new PdfPCell(new Phrase(percentage.ToString()+"%", blackFnt6)); commentsRow2Cell2.Colspan = 5; commentsRow2Cell2.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow2Cell2.Border = 0;*/

        PdfPCell commentsRow3Cell1 = new PdfPCell(new Phrase("NO OF TIMES PRESENT", blackFnt6)); commentsRow3Cell1.Colspan = 2; commentsRow3Cell1.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow3Cell1.Border = 0;*/
        PdfPCell commentsRow3Cell2 = new PdfPCell(new Phrase("", blackFnt6)); commentsRow3Cell2.Colspan = 5; commentsRow3Cell2.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow3Cell2.Border = 0;*/

        PdfPCell commentsRow4Cell1 = new PdfPCell(new Phrase("CLASS TEACHERS REMARK", blackFnt6)); commentsRow4Cell1.Colspan = 2; commentsRow4Cell1.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow4Cell1.Border = 0;*/
        PdfPCell commentsRow4Cell2 = new PdfPCell(new Phrase(classTeacherComment, blackFnt6)); commentsRow4Cell2.Colspan = 5; commentsRow4Cell2.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow4Cell2.Border = 0;*/

        PdfPCell commentsRow5Cell1 = new PdfPCell(new Phrase("PRINICIPAL REMARK", blackFnt6)); commentsRow5Cell1.Colspan = 2; commentsRow5Cell1.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow5Cell1.Border = 0;*/
        PdfPCell commentsRow5Cell2 = new PdfPCell(new Phrase(headTeacherComment, blackFnt6)); commentsRow5Cell2.Colspan = 5; commentsRow5Cell2.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow5Cell2.Border = 0;*/

        PdfPCell commentsRow6Cell1 = new PdfPCell(new Phrase("PRINICIPAL SIGNATURE", blackFnt6)); commentsRow6Cell1.Colspan = 2; commentsRow6Cell1.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow6Cell1.Border = 0;*/
        PdfPCell commentsRow6Cell2 = new PdfPCell(sign); commentsRow6Cell2.Colspan = 5; commentsRow6Cell2.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow6Cell2.Border = 0;*/

        PdfPCell commentsRow7Cell1 = new PdfPCell(new Phrase("DATE", blackFnt6)); commentsRow7Cell1.Colspan = 2; commentsRow7Cell1.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow6Cell1.Border = 0;*/
        PdfPCell commentsRow7Cell2 = new PdfPCell(new Phrase(DateTime.Now.ToString("M/dd/yyyy"), blackFnt6)); commentsRow7Cell2.Colspan = 5; commentsRow7Cell2.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow6Cell2.Border = 0;*/

        commentsTable.AddCell(commentsRow1Cell1);
        commentsTable.AddCell(commentsRow1Cell2);

        commentsTable.AddCell(commentsRow2Cell1);
        commentsTable.AddCell(commentsRow2Cell2);

        commentsTable.AddCell(commentsRow3Cell1);
        commentsTable.AddCell(commentsRow3Cell2);

        commentsTable.AddCell(commentsRow4Cell1);
        commentsTable.AddCell(commentsRow4Cell2);

        commentsTable.AddCell(commentsRow5Cell1);
        commentsTable.AddCell(commentsRow5Cell2);

        commentsTable.AddCell(commentsRow6Cell1);
        commentsTable.AddCell(commentsRow6Cell2);

        commentsTable.AddCell(commentsRow7Cell1);
        commentsTable.AddCell(commentsRow7Cell2);


        //PdfPTable gradeTable = new PdfPTable(4);
        //PdfPCell gradeRow1Cell1 = new PdfPCell(new Phrase("SCORE", blackFnt6)); gradeRow1Cell1.Colspan = 1; gradeRow1Cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell gradeRow1Cell2 = new PdfPCell(new Phrase("GRADE", blackFnt6)); gradeRow1Cell2.Colspan = 1; gradeRow1Cell2.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell gradeRow1Cell3 = new PdfPCell(new Phrase("REMARK", blackFnt6)); gradeRow1Cell3.Colspan = 2; gradeRow1Cell3.HorizontalAlignment = Element.ALIGN_LEFT;

        //PdfPCell gradeRow2Cell1 = new PdfPCell(new Phrase("50", blackFnt6)); gradeRow2Cell1.Colspan = 1; gradeRow2Cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell gradeRow2Cell2 = new PdfPCell(new Phrase("D", blackFnt6)); gradeRow2Cell2.Colspan = 1; gradeRow2Cell2.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell gradeRow2Cell3 = new PdfPCell(new Phrase("AVERAGE", blackFnt6)); gradeRow2Cell3.Colspan = 2; gradeRow2Cell3.HorizontalAlignment = Element.ALIGN_LEFT;

        //PdfPCell gradeRow3Cell1 = new PdfPCell(new Phrase("60", blackFnt6)); gradeRow3Cell1.Colspan = 1; gradeRow3Cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell gradeRow3Cell2 = new PdfPCell(new Phrase("C", blackFnt6)); gradeRow3Cell2.Colspan = 1; gradeRow3Cell2.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell gradeRow3Cell3 = new PdfPCell(new Phrase("GOOD", blackFnt6)); gradeRow3Cell3.Colspan = 2; gradeRow3Cell3.HorizontalAlignment = Element.ALIGN_LEFT;

        //PdfPCell gradeRow4Cell1 = new PdfPCell(new Phrase("70", blackFnt6)); gradeRow4Cell1.Colspan = 1; gradeRow4Cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell gradeRow4Cell2 = new PdfPCell(new Phrase("B", blackFnt6)); gradeRow4Cell2.Colspan = 1; gradeRow4Cell2.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell gradeRow4Cell3 = new PdfPCell(new Phrase("VERY GOOD", blackFnt6)); gradeRow4Cell3.Colspan = 2; gradeRow4Cell3.HorizontalAlignment = Element.ALIGN_LEFT;

        //PdfPCell gradeRow5Cell1 = new PdfPCell(new Phrase("80", blackFnt6)); gradeRow5Cell1.Colspan = 1; gradeRow5Cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell gradeRow5Cell2 = new PdfPCell(new Phrase("A", blackFnt6)); gradeRow5Cell2.Colspan = 1; gradeRow5Cell2.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell gradeRow5Cell3 = new PdfPCell(new Phrase("EXCELLENT", blackFnt6)); gradeRow5Cell3.Colspan = 2; gradeRow5Cell3.HorizontalAlignment = Element.ALIGN_LEFT;
        //gradeTable.AddCell(gradeRow1Cell1);
        //gradeTable.AddCell(gradeRow1Cell2);
        //gradeTable.AddCell(gradeRow1Cell3);

        //gradeTable.AddCell(gradeRow2Cell1);
        //gradeTable.AddCell(gradeRow2Cell2);
        //gradeTable.AddCell(gradeRow2Cell3);

        //gradeTable.AddCell(gradeRow3Cell1);
        //gradeTable.AddCell(gradeRow3Cell2);
        //gradeTable.AddCell(gradeRow3Cell3);

        //gradeTable.AddCell(gradeRow4Cell1);
        //gradeTable.AddCell(gradeRow4Cell2);
        //gradeTable.AddCell(gradeRow4Cell3);

        //gradeTable.AddCell(gradeRow5Cell1);
        //gradeTable.AddCell(gradeRow5Cell2);
        //gradeTable.AddCell(gradeRow5Cell3);


        PdfPTable finalTable = new PdfPTable(11);
        PdfPCell commentsTbl = new PdfPCell(commentsTable); commentsTbl.Colspan = 11;
        //PdfPCell gradeTbl = new PdfPCell(gradeTable); gradeTbl.Colspan = 2;
        finalTable.AddCell(commentsTbl);
        //finalTable.AddCell(gradeTbl);



        document.Add(domain);
        document.Add(resTable);
        document.Add(domain);
        document.Add(baseTbl);
        document.Add(finalTable);


    }

    protected void addResultSummaryPageTaiSolarinSecondaryThird(Document document, PASSIS.LIB.User student, UsersLIB usrDal)
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
        PdfPCell innerCell1 = new PdfPCell(new Phrase("TAI SOLARIN MEMORIAL SECONDARY SCHOOL", darkerRedFnt16)); innerCell1.Colspan = 9; innerCell1.Border = 0; innerCell1.HorizontalAlignment = Element.ALIGN_CENTER;
        PdfPCell innerCell2 = new PdfPCell(new Phrase("PLOT 772 NEW TOWN ROAD AMUWO ODOFIN LAGOS STATE", blackFnt8)); innerCell2.Colspan = 9; innerCell2.Border = 0; innerCell2.HorizontalAlignment = Element.ALIGN_CENTER;
        //PdfPCell innerCell3 = new PdfPCell(new Phrase("Tel: 08164584442, 08085687700", resultTitleRedFnt8)); innerCell3.Colspan = 9; innerCell3.Border = 0; innerCell3.HorizontalAlignment = Element.ALIGN_CENTER;
        //PdfPCell innerCell4 = new PdfPCell(new Phrase("MOTTO: Work Hard, Pray Hard and Keep Straight", blackFnt8)); innerCell4.Colspan = 9; innerCell4.Border = 0; innerCell4.HorizontalAlignment = Element.ALIGN_CENTER;
        innerTable.AddCell(innerCell1);
        innerTable.AddCell(innerCell2);
        //innerTable.AddCell(innerCell3);
        //innerTable.AddCell(innerCell4);


        PdfPTable head = new PdfPTable(15);
        PdfPCell head1 = new PdfPCell(jpg); head1.Colspan = 2; head1.Border = 0;
        PdfPCell head2 = new PdfPCell(innerTable); head2.Colspan = 13; head2.Border = 0; head2.HorizontalAlignment = Element.ALIGN_CENTER;
        //PdfPCell head3 = new PdfPCell(jpg1); head3.Colspan = 2; head3.HorizontalAlignment = Element.ALIGN_RIGHT; head3.Border = 0;


        head.AddCell(head1);
        head.AddCell(head2);
        //head.AddCell(head3);
        document.Add(head);



        //if (Convert.ToInt16(ddlYear.SelectedValue) > 32 && Convert.ToInt16(ddlYear.SelectedValue) < 36)
        //{
        //    Paragraph transcript = new Paragraph(string.Format("{0}", "SENIOR SECONDARY SCHOOL TERMINAL REPORT SHEET"), darkerRedFnt);
        //    transcript.Alignment = Element.ALIGN_CENTER;
        //    document.Add(transcript);
        //}
        //else if (Convert.ToInt16(ddlYear.SelectedValue) > 29 && Convert.ToInt16(ddlYear.SelectedValue) < 33)
        //{
        //    Paragraph transcript = new Paragraph(string.Format("{0}", "JUNIOR SECONDARY SCHOOL TERMINAL REPORT SHEET"), darkerRedFnt);
        //    transcript.Alignment = Element.ALIGN_CENTER;
        //    document.Add(transcript);
        //}

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

        string totalNoInClass = new ClassGradeLIB().RetrieveGradeStudents(Convert.ToInt64(schId), Convert.ToInt64(logonUser.SchoolCampusId), Convert.ToInt64(yearId), gradeIdd, session).Count().ToString();
        ReportCardNextTermBegin date = context.ReportCardNextTermBegins.OrderByDescending(x => x.SchoolID == logonUser.SchoolId && x.CampusID == logonUser.SchoolCampusId).FirstOrDefault();

        PdfPTable maintbl = new PdfPTable(8);
        PdfPCell cell1 = new PdfPCell(new Phrase("SURNAME", blackFnt8)); cell1.Colspan = 2; cell1.HorizontalAlignment = Element.ALIGN_LEFT; //cell1.Border = 0;
        PdfPCell cell2 = new PdfPCell(new Phrase(student.LastName.ToUpper(), blackFnt8)); cell2.Colspan = 3; cell1.HorizontalAlignment = Element.ALIGN_LEFT; //cell2.Border = 0;
        PdfPCell cell3 = new PdfPCell(new Phrase("SEX", blackFnt8)); cell3.Colspan = 1; cell3.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell cell4 = new PdfPCell(new Phrase(getGender(student.Gender).ToString(), blackFnt8)); cell4.Colspan = 2; cell4.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;

        PdfPCell cell5 = new PdfPCell(new Phrase("FIRST NAME", blackFnt8)); cell5.Colspan = 2; cell5.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell cell6 = new PdfPCell(new Phrase(student.FirstName.ToUpper(), blackFnt8)); cell6.Colspan = 3; cell6.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell cell7 = new PdfPCell(new Phrase("HOUSE", blackFnt8)); cell7.Colspan = 1; cell7.HorizontalAlignment = Element.ALIGN_LEFT; //cell3.Border = 0;
        PdfPCell cell8 = new PdfPCell(new Phrase("", blackFnt8)); cell8.Colspan = 2; cell8.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;

        PdfPCell cell9 = new PdfPCell(new Phrase("OVERALL POSITION", blackFnt8)); cell9.Colspan = 2; cell9.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell cell10 = new PdfPCell(new Phrase("", blackFnt8)); cell10.Colspan = 3; cell10.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell cell11 = new PdfPCell(new Phrase("TERM", blackFnt8)); cell11.Colspan = 1; cell11.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell cell12 = new PdfPCell(new Phrase(ddlAcademicTerm.SelectedItem.ToString(), blackFnt8)); cell12.Colspan = 2; cell12.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;

        PdfPCell cell13 = new PdfPCell(new Phrase("POSITION IN CLASS", blackFnt8)); cell13.Colspan = 2; cell13.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell cell14 = new PdfPCell(new Phrase(classPosition.ToString() + "" + ToOrdinal(Convert.ToInt16(classPosition)), blackFnt8)); cell14.Colspan = 3; cell14.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell cell15 = new PdfPCell(new Phrase("SESSION", blackFnt8)); cell15.Colspan = 1; cell15.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell cell16 = new PdfPCell(new Phrase(ddlAcademicSession.SelectedItem.ToString(), blackFnt8)); cell16.Colspan = 2; cell16.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;

        PdfPCell cell17 = new PdfPCell(new Phrase("NEXT TERM BEGINS", blackFnt8)); cell17.Colspan = 2; cell17.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell cell18 = new PdfPCell(new Phrase(Convert.ToDateTime(date.NextTermBegins.ToString()).ToString("dd/M/yyyy"), blackFnt8)); cell18.Colspan = 3; cell18.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell cell19 = new PdfPCell(new Phrase("CLASS", blackFnt8)); cell19.Colspan = 1; cell19.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell cell21 = new PdfPCell(new Phrase(ddlYear.SelectedItem.ToString(), blackFnt8)); cell21.Colspan = 2; cell21.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;
        PdfPCell spaceCol3 = new PdfPCell(new Phrase("", blackFnt8)); spaceCol3.Colspan = 3; spaceCol3.HorizontalAlignment = Element.ALIGN_LEFT; //cell4.Border = 0;



        maintbl.AddCell(cell1);
        maintbl.AddCell(cell2);
        maintbl.AddCell(cell3);
        maintbl.AddCell(cell4);
        maintbl.AddCell(cell5);
        maintbl.AddCell(cell6);
        maintbl.AddCell(cell7);
        maintbl.AddCell(cell8);
        maintbl.AddCell(cell9);
        maintbl.AddCell(cell10);
        maintbl.AddCell(cell11);
        maintbl.AddCell(cell12);
        maintbl.AddCell(cell13);
        maintbl.AddCell(cell14);
        maintbl.AddCell(cell15);
        maintbl.AddCell(cell16);
        maintbl.AddCell(cell17);
        if (date != null)
        {
            maintbl.AddCell(cell18);
        }
        else
        {
            maintbl.AddCell(spaceCol3);
        }
        maintbl.AddCell(cell19);
        maintbl.AddCell(cell21);



        //maintbl.AddCell(row2cell1);
        //maintbl.AddCell(row2cell2);
        //maintbl.AddCell(row2cell3);
        //maintbl.AddCell(row2cell4);
        //maintbl.AddCell(row2cell5);
        //maintbl.AddCell(row2cell6);
        //maintbl.AddCell(row2cell7);
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

        PdfPTable headd = new PdfPTable(15);
        PdfPCell headd1 = new PdfPCell(maintbl); headd1.Colspan = 13;
        PdfPCell headd2 = new PdfPCell(jpg1); headd2.Colspan = 2; headd2.Border = 0; headd2.HorizontalAlignment = Element.ALIGN_CENTER; headd2.Border = 0;
        //PdfPCell head3 = new PdfPCell(jpg1); head3.Colspan = 2; head3.HorizontalAlignment = Element.ALIGN_RIGHT; head3.Border = 0;


        headd.AddCell(headd1);
        headd.AddCell(headd2);
        document.Add(headd);



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

        PdfPCell thirdRow2Cell1 = new PdfPCell(new Phrase("SUBJECT", whiteFnt7)); thirdRow2Cell1.Colspan = 3; thirdRow2Cell1.HorizontalAlignment = Element.ALIGN_CENTER; thirdRow2Cell1.BackgroundColor = darkergrayFnt.Color;
        PdfPCell thirdRow2Cell2 = new PdfPCell(new Phrase("CA 40%", whiteFnt7)); thirdRow2Cell2.Colspan = 1; thirdRow2Cell2.HorizontalAlignment = Element.ALIGN_CENTER; thirdRow2Cell2.BackgroundColor = darkergrayFnt.Color;
        PdfPCell thirdRow2Cell3 = new PdfPCell(new Phrase("EXAM 60%", whiteFnt7)); thirdRow2Cell3.Colspan = 1; thirdRow2Cell3.HorizontalAlignment = Element.ALIGN_CENTER; thirdRow2Cell3.BackgroundColor = darkergrayFnt.Color;
        PdfPCell thirdRow2Cell4 = new PdfPCell(new Phrase("TOTAL 100%", whiteFnt7)); thirdRow2Cell4.Colspan = 1; thirdRow2Cell4.HorizontalAlignment = Element.ALIGN_CENTER; thirdRow2Cell4.BackgroundColor = darkergrayFnt.Color;
        PdfPCell thirdRow2Cell5 = new PdfPCell(new Phrase("1ST TERM", whiteFnt7)); thirdRow2Cell5.Colspan = 1; thirdRow2Cell5.HorizontalAlignment = Element.ALIGN_CENTER; thirdRow2Cell5.BackgroundColor = darkergrayFnt.Color;
        PdfPCell thirdRow2Cell6 = new PdfPCell(new Phrase("2ND TERM", whiteFnt7)); thirdRow2Cell6.Colspan = 1; thirdRow2Cell6.HorizontalAlignment = Element.ALIGN_CENTER; thirdRow2Cell6.BackgroundColor = darkergrayFnt.Color;
        PdfPCell thirdRow2Cell7 = new PdfPCell(new Phrase("AVERAGE", whiteFnt7)); thirdRow2Cell7.Colspan = 1; thirdRow2Cell7.HorizontalAlignment = Element.ALIGN_CENTER; thirdRow2Cell7.BackgroundColor = darkergrayFnt.Color;
        PdfPCell thirdRow2Cell8 = new PdfPCell(new Phrase("GRADE", whiteFnt7)); thirdRow2Cell8.Colspan = 1; thirdRow2Cell8.HorizontalAlignment = Element.ALIGN_CENTER; thirdRow2Cell8.BackgroundColor = darkergrayFnt.Color;
        //PdfPCell thirdRow2Cell9 = new PdfPCell(new Phrase("POSITION", whiteFnt7)); thirdRow2Cell9.Colspan = 1; thirdRow2Cell9.HorizontalAlignment = Element.ALIGN_CENTER; thirdRow2Cell9.BackgroundColor = darkergrnFnt.Color;
        PdfPCell thirdRow2Cell10 = new PdfPCell(new Phrase("REMARK", whiteFnt7)); thirdRow2Cell10.Colspan = 2; thirdRow2Cell10.HorizontalAlignment = Element.ALIGN_CENTER; thirdRow2Cell10.BackgroundColor = darkergrayFnt.Color;

        Paragraph domain = new Paragraph(string.Format("{0}", " "), darkerRedFnt);
        domain.Alignment = Element.ALIGN_CENTER;

        Paragraph domain1 = new Paragraph(string.Format("{0}", " "), darkerRedFnt);
        domain1.Alignment = Element.ALIGN_CENTER;

        //thirdTerm.AddCell(thirdRow1Cell1);

        resTable.AddCell(thirdRow2Cell1);
        resTable.AddCell(thirdRow2Cell2);
        resTable.AddCell(thirdRow2Cell3);
        resTable.AddCell(thirdRow2Cell4);
        resTable.AddCell(thirdRow2Cell5);
        resTable.AddCell(thirdRow2Cell6);
        resTable.AddCell(thirdRow2Cell7);
        resTable.AddCell(thirdRow2Cell8);
        //resTable.AddCell(thirdRow2Cell9);
        resTable.AddCell(thirdRow2Cell10);
        //resTable.AddCell(thirdRow2Cell8);
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
        decimal firstTermCA = 0;
        decimal firstTermExam = 0;
        decimal firstTermTotal = 0;
        decimal secondTermCA = 0;
        decimal secondTermExam = 0;
        decimal secondTermTotal = 0;
        decimal averageScore = 0;

        //decimal examScoreObtained = 0;
        long examCatPercentage = 0;
        decimal examPercentageScore = 0;
        decimal testScoreObtained = 0;
        long testCatPercentage = 0;
        decimal testPercentageScore = 0;

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

                    //if (ddlAcademicTerm.SelectedValue == "3")
                    //{


                    ReportCardPrintConfig checkExist = context.ReportCardPrintConfigs.FirstOrDefault(x => x.SchoolId == logonUser.SchoolId && x.CampusId == logonUser.SchoolCampusId
                       && x.ClassId == Convert.ToInt64(ddlYear.SelectedValue) && x.GradeId == Convert.ToInt64(ddlGrade.SelectedValue) && x.SessionId == Convert.ToInt64(ddlAcademicSession.SelectedValue)
                       && x.TermId == Convert.ToInt64(ddlAcademicTerm.SelectedValue) && x.SubjectId == subjects.SubjectId);

                    if (checkExist != null && checkExist.Exam == true && checkExist.CA == false) //Only Exam
                    {
                        ////decimal examScoreObtained = 0;
                        //long examCatPercentage = 0;
                        //decimal examPercentageScore = 0;
                        //long maxScore = Convert.ToInt64(subjects.MaximumScore);

                        ScoreCategoryConfiguration scoreCatConfig = context.ScoreCategoryConfigurations.FirstOrDefault(x => x.SchoolId == schoolId && x.CampusId == logonUser.SchoolCampusId && x.ClassId == yearId && x.SessionId == session && x.TermId == term && x.Category.Trim() == "Exam");
                        if (scoreCatConfig != null)
                        {
                            examCatPercentage = Convert.ToInt64(scoreCatConfig.Percentage);
                        }

                        PdfPCell subjCell = new PdfPCell(new Phrase(subject.Name, darkerGrnFnt8)); subjCell.Colspan = 3;
                        resTable.AddCell(subjCell);
                        PdfPCell CACell = new PdfPCell(new Phrase("", darkerGrnFnt8)); CACell.Colspan = 1;
                        resTable.AddCell(CACell);
                        PdfPCell ExamCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); ExamCell.Colspan = 1;
                        resTable.AddCell(ExamCell);

                        PASSIS.LIB.StudentScore stdScore = getSubjectScoreCategoryExam(student.AdmissionNumber, student.Id, session, term, schoolId, yearId, gradeId, subject.Id);
                        //IList<PASSIS.LIB.StudentScore> sortedScoreRepoByCategory = stdScore.OrderBy(x => x.SubCategoryId).ToList(); //fetch all Exam Scores 

                        if (stdScore != null)
                        {
                                examPercentageScore = Math.Round((decimal)stdScore.ExamScore, 0);

                            if (examPercentageScore > 0 && examCatPercentage > 0)
                            {
                                totalScore = (examPercentageScore / examCatPercentage) * (decimal)subjects.MaximumScore;
                            }
                            else
                            {
                                totalScore = 0;
                                examPercentageScore = 0;
                                examCatPercentage = 0;
                            }

                            if (totalScore > 0)
                            {
                                aggregateTotalScore += Math.Round(totalScore, 0);
                                subjectCounter++;

                                PdfPCell TotalFirstCell = new PdfPCell(new Phrase(Math.Round(totalScore, 0).ToString(), darkerGrnFnt8)); TotalFirstCell.Colspan = 1;
                                resTable.AddCell(TotalFirstCell);
                            }
                            else
                            {
                                PdfPCell TotalFirstCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); TotalFirstCell.Colspan = 1;
                                resTable.AddCell(TotalFirstCell);
                            }
                        }
                        else
                        {
                            PdfPCell TotalFirstCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); TotalFirstCell.Colspan = 1;
                            resTable.AddCell(TotalFirstCell);
                        }

                        //aggregateTotalScore += Math.Round(totalScore, 0);

                        //if (totalScore > 0)
                        //{
                        //    subjectCounter++;
                        //}

                    }

                    //else if (subjects.CA == true && subjects.Exam == false) //Only CA
                    //{
                    else if (checkExist != null && checkExist.CA == true && checkExist.Exam == false)
                    {
                        //decimal testScoreObtained = 0;
                        //long testCatPercentage = 0;
                        //decimal testPercentageScore = 0;
                        //long maxScore = Convert.ToInt64(subjects.MaximumScore);

                        ScoreCategoryConfiguration scoreCatConfig = context.ScoreCategoryConfigurations.FirstOrDefault(x => x.SchoolId == schoolId && x.CampusId == logonUser.SchoolCampusId && x.ClassId == yearId && x.SessionId == session && x.TermId == term && x.Category.Trim() == "CA");
                        if (scoreCatConfig != null)
                        {
                            testCatPercentage = Convert.ToInt64(scoreCatConfig.Percentage);
                        }

                        PdfPCell subjCell = new PdfPCell(new Phrase(subject.Name, darkerGrnFnt8)); subjCell.Colspan = 3;
                        resTable.AddCell(subjCell);
                        PdfPCell CACell = new PdfPCell(new Phrase("", darkerGrnFnt8)); CACell.Colspan = 1;
                        resTable.AddCell(CACell);
                        PdfPCell ExamCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); ExamCell.Colspan = 1;
                        resTable.AddCell(ExamCell);

                        IList<PASSIS.LIB.StudentScoreRepository> scoreRepo = getSubjectScoreCategoryCA(student.AdmissionNumber, student.Id, session, term, schoolId, yearId, gradeId, subject.Id);
                        //IList<PASSIS.LIB.StudentScoreRepository> sortedScoreRepoByCategory = scoreRepo.OrderBy(x => x.SubCategoryId).ToList(); //fetch all continuous assessment (CA) scores 
                        if (scoreRepo.Count > 0)
                        {
                            foreach (PASSIS.LIB.StudentScoreRepository s in scoreRepo)
                            {
                                testPercentageScore = (decimal)s.MarkObtained;
                            }

                            if (testPercentageScore > 0 && testCatPercentage > 0)
                            {
                                totalScore = (testPercentageScore / testCatPercentage) * (decimal)subjects.MaximumScore;
                            }
                            else
                            {
                                totalScore = 0;
                                examPercentageScore = 0;
                                examCatPercentage = 0;
                            }

                            if (totalScore > 0)
                            {
                                aggregateTotalScore += Math.Round(totalScore, 0);
                                subjectCounter++;

                                PdfPCell TotalFirstCell = new PdfPCell(new Phrase(Math.Round(totalScore, 0).ToString(), darkerGrnFnt8)); TotalFirstCell.Colspan = 1;
                                resTable.AddCell(TotalFirstCell);

                            }
                            else
                            {
                                PdfPCell TotalFirstCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); TotalFirstCell.Colspan = 1;
                                resTable.AddCell(TotalFirstCell);
                            }
                        }
                        else
                        {
                            PdfPCell TotalFirstCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); TotalFirstCell.Colspan = 1;
                            resTable.AddCell(TotalFirstCell);
                        }

                        //aggregateTotalScore += Math.Round(totalScore, 0);
                        //if (totalScore > 0)
                        //{
                        //    subjectCounter++;
                        //}

                    }


                    else //EXAM AND CA
                    {

                        PdfPCell subjCell = new PdfPCell(new Phrase(subject.Name, darkerGrnFnt8)); subjCell.Colspan = 3;
                        resTable.AddCell(subjCell);
                        IList<PASSIS.LIB.StudentScoreRepository> scoreRepoFirstTerm = getSubjectScoreCategoryCA(student.AdmissionNumber, student.Id, session, term, schoolId, yearId, gradeId, subject.Id);
                        if (scoreRepoFirstTerm.Count > 0)
                        {
                            foreach (PASSIS.LIB.StudentScoreRepository fs in scoreRepoFirstTerm)
                            {

                                testScore = Convert.ToDecimal(fs.MarkObtained);
                                PdfPCell CACell = new PdfPCell(new Phrase(Math.Round(testScore, 0).ToString(), darkerGrnFnt8)); CACell.Colspan = 1;
                                resTable.AddCell(CACell);
                                break;
                            }
                        }
                        else
                        {
                            testScore = 0;
                            PdfPCell CACell = new PdfPCell(new Phrase("", darkerGrnFnt8)); CACell.Colspan = 1;
                            resTable.AddCell(CACell);
                        }

                        PASSIS.LIB.StudentScore scoreRepoExamFirstTerm = getSubjectScoreCategoryExam(student.AdmissionNumber, student.Id, session, term, schoolId, yearId, gradeId, subject.Id);
                        if (scoreRepoExamFirstTerm != null)
                        {
                                examScore = Convert.ToDecimal(scoreRepoExamFirstTerm.ExamScore);
                                PdfPCell ExamCell = new PdfPCell(new Phrase(Math.Round(examScore, 0).ToString(), darkerGrnFnt8)); ExamCell.Colspan = 1;
                                resTable.AddCell(ExamCell);
                        }
                        else
                        {
                            examScore = 0;
                            PdfPCell ExamCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); ExamCell.Colspan = 1;
                            resTable.AddCell(ExamCell);
                        }

                        totalScore = testScore + examScore;
                        aggregateTotalScore += Math.Round(totalScore, 0);

                        if (totalScore > 0)
                        {
                            subjectCounter++;
                        }

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
                    }


                    ///////...............................................FIRST TERM...................................................................................... //
                    ReportCardPrintConfig checkExistt = context.ReportCardPrintConfigs.FirstOrDefault(x => x.SchoolId == logonUser.SchoolId && x.CampusId == logonUser.SchoolCampusId
                                          && x.ClassId == Convert.ToInt64(ddlYear.SelectedValue) && x.GradeId == Convert.ToInt64(ddlGrade.SelectedValue) && x.SessionId == Convert.ToInt64(ddlAcademicSession.SelectedValue)
                                          && x.TermId == 1 && x.SubjectId == subjects.SubjectId);

                    if (checkExistt != null && checkExistt.Exam == true && checkExistt.CA == false) //Only Exam
                    {
                        ScoreCategoryConfiguration scoreCatConfigg = context.ScoreCategoryConfigurations.FirstOrDefault(x => x.SchoolId == schoolId && x.CampusId == logonUser.SchoolCampusId && x.ClassId == yearId && x.SessionId == session && x.TermId == 1 && x.Category.Trim() == "Exam");
                        if (scoreCatConfigg != null)
                        {
                            examCatPercentage = Convert.ToInt64(scoreCatConfigg.Percentage);
                        }
                        PASSIS.LIB.StudentScore stdScoree = getSubjectScoreCategoryExam(student.AdmissionNumber, student.Id, session, 1, schoolId, yearId, gradeId, subject.Id);

                        if (stdScoree != null)
                        {
                                examPercentageScore = Math.Round((decimal)stdScoree.ExamScore, 0);

                            if (examPercentageScore > 0 && examCatPercentage > 0)
                            {
                                firstTermExam = (examPercentageScore / examCatPercentage) * (decimal)subjects.MaximumScore;
                            }
                            else
                            {
                                firstTermExam = 0;
                                examPercentageScore = 0;
                                examCatPercentage = 0;
                            }
                        }
                        else
                        {
                            firstTermExam = 0;
                            examPercentageScore = 0;
                            examCatPercentage = 0;
                        }

                        firstTermTotal = firstTermExam;
                    }

                    else if (checkExistt != null && checkExistt.Exam == false && checkExistt.CA == true) //Only CA
                    {
                        ScoreCategoryConfiguration scoreCatConfig = context.ScoreCategoryConfigurations.FirstOrDefault(x => x.SchoolId == schoolId && x.CampusId == logonUser.SchoolCampusId && x.ClassId == yearId && x.SessionId == session && x.TermId == 1 && x.Category.Trim() == "CA");
                        if (scoreCatConfig != null)
                        {
                            testCatPercentage = Convert.ToInt64(scoreCatConfig.Percentage);
                        }

                        IList<PASSIS.LIB.StudentScoreRepository> scoreRepo = getSubjectScoreCategoryCA(student.AdmissionNumber, student.Id, session, 1, schoolId, yearId, gradeId, subject.Id);
                        //IList<PASSIS.LIB.StudentScoreRepository> sortedScoreRepoByCategory = scoreRepo.OrderBy(x => x.SubCategoryId).ToList(); //fetch all continuous assessment (CA) scores 
                        if (scoreRepo.Count > 0)
                        {
                            foreach (PASSIS.LIB.StudentScoreRepository s in scoreRepo)
                            {
                                testPercentageScore = (decimal)s.MarkObtained;
                                break;
                            }

                            if (testPercentageScore > 0 && testCatPercentage > 0)
                            {
                                firstTermCA = (testPercentageScore / testCatPercentage) * (decimal)subjects.MaximumScore;
                            }
                            else
                            {
                                firstTermCA = 0;
                                testPercentageScore = 0;
                                testCatPercentage = 0;
                            }
                        }
                        else
                        {
                            firstTermCA = 0;
                            testPercentageScore = 0;
                            testCatPercentage = 0;
                        }

                        firstTermTotal = firstTermCA;
                    }

                    else //EXAM AND CA
                    {

                        IList<PASSIS.LIB.StudentScoreRepository> scoreRepoFirstTerm1 = getSubjectScoreCategoryCA(student.AdmissionNumber, student.Id, session, 1, schoolId, yearId, gradeId, subject.Id);
                        if (scoreRepoFirstTerm1.Count > 0)
                        {
                            foreach (PASSIS.LIB.StudentScoreRepository fs in scoreRepoFirstTerm1)
                            {
                                firstTermCA = Convert.ToDecimal(fs.MarkObtained);
                                break;
                            }
                        }
                        else
                        {
                            firstTermCA = 0;
                        }

                        PASSIS.LIB.StudentScore scoreRepoExamFirstTerm1 = getSubjectScoreCategoryExam(student.AdmissionNumber, student.Id, session, 1, schoolId, yearId, gradeId, subject.Id);
                        if (scoreRepoExamFirstTerm1 != null)
                        {
                                firstTermExam = Convert.ToDecimal(scoreRepoExamFirstTerm1.ExamScore);
                                break;
                        }
                        else
                        {
                            firstTermExam = 0;
                        }

                        firstTermTotal = firstTermCA + firstTermExam;
                    }

                    if (firstTermTotal > 0)
                    {
                        PdfPCell TotalFirstTermCell = new PdfPCell(new Phrase(Math.Round(firstTermTotal, 0).ToString(), darkerGrnFnt8)); TotalFirstTermCell.Colspan = 1;
                        resTable.AddCell(TotalFirstTermCell);
                    }
                    else
                    {
                        PdfPCell TotalFirstTermCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); TotalFirstTermCell.Colspan = 1;
                        resTable.AddCell(TotalFirstTermCell);
                    }


                    /////...........................SECOND TERM................................................'....................................... //
                    ReportCardPrintConfig checkExisttt = context.ReportCardPrintConfigs.FirstOrDefault(x => x.SchoolId == logonUser.SchoolId && x.CampusId == logonUser.SchoolCampusId
                                                            && x.ClassId == Convert.ToInt64(ddlYear.SelectedValue) && x.GradeId == Convert.ToInt64(ddlGrade.SelectedValue) && x.SessionId == Convert.ToInt64(ddlAcademicSession.SelectedValue)
                                                            && x.TermId == 2 && x.SubjectId == subjects.SubjectId);

                    if (checkExisttt != null && checkExisttt.Exam == true && checkExisttt.CA == false) //Only Exam
                    {
                        ScoreCategoryConfiguration scoreCatConfigg = context.ScoreCategoryConfigurations.FirstOrDefault(x => x.SchoolId == schoolId && x.CampusId == logonUser.SchoolCampusId && x.ClassId == yearId && x.SessionId == session && x.TermId == 2 && x.Category.Trim() == "Exam");
                        if (scoreCatConfigg != null)
                        {
                            examCatPercentage = Convert.ToInt64(scoreCatConfigg.Percentage);
                        }
                        PASSIS.LIB.StudentScore  stdScoree = getSubjectScoreCategoryExam(student.AdmissionNumber, student.Id, session, 2, schoolId, yearId, gradeId, subject.Id);

                        if (stdScoree != null)
                        {
                                examPercentageScore = Math.Round((decimal)stdScoree.ExamScore, 0);

                            if (examPercentageScore > 0 && examCatPercentage > 0)
                            {
                                secondTermExam = (examPercentageScore / examCatPercentage) * (decimal)subjects.MaximumScore;
                            }
                            else
                            {
                                secondTermExam = 0;
                                examPercentageScore = 0;
                                examCatPercentage = 0;
                            }
                        }
                        else
                        {
                            secondTermExam = 0;
                            examPercentageScore = 0;
                            examCatPercentage = 0;
                        }

                        secondTermTotal = secondTermExam;
                    }

                    else if (checkExisttt != null && checkExisttt.Exam == false && checkExisttt.CA == true) //Only CA
                    {
                        ScoreCategoryConfiguration scoreCatConfig = context.ScoreCategoryConfigurations.FirstOrDefault(x => x.SchoolId == schoolId && x.CampusId == logonUser.SchoolCampusId && x.ClassId == yearId && x.SessionId == session && x.TermId == 2 && x.Category.Trim() == "CA");
                        if (scoreCatConfig != null)
                        {
                            testCatPercentage = Convert.ToInt64(scoreCatConfig.Percentage);
                        }

                        IList<PASSIS.LIB.StudentScoreRepository> scoreRepo = getSubjectScoreCategoryCA(student.AdmissionNumber, student.Id, session, 2, schoolId, yearId, gradeId, subject.Id);
                        //IList<PASSIS.LIB.StudentScoreRepository> sortedScoreRepoByCategory = scoreRepo.OrderBy(x => x.SubCategoryId).ToList(); //fetch all continuous assessment (CA) scores 
                        if (scoreRepo.Count > 0)
                        {
                            foreach (PASSIS.LIB.StudentScoreRepository s in scoreRepo)
                            {
                                testPercentageScore = (decimal)s.MarkObtained;
                                break;
                            }

                            if (testPercentageScore > 0 && testCatPercentage > 0)
                            {
                                secondTermCA = (testPercentageScore / testCatPercentage) * (decimal)subjects.MaximumScore;
                            }
                            else
                            {
                                secondTermCA = 0;
                                testPercentageScore = 0;
                                testCatPercentage = 0;
                            }
                        }
                        else
                        {
                            secondTermCA = 0;
                            testPercentageScore = 0;
                            testCatPercentage = 0;
                        }

                        secondTermTotal = secondTermCA;
                    }

                    else //EXAM AND CA
                    {
                        IList<PASSIS.LIB.StudentScoreRepository> scoreRepoSecondTerm1 = getSubjectScoreCategoryCA(student.AdmissionNumber, student.Id, session, 2, schoolId, yearId, gradeId, subject.Id);
                        if (scoreRepoSecondTerm1.Count > 0)
                        {
                            foreach (PASSIS.LIB.StudentScoreRepository fs in scoreRepoSecondTerm1)
                            {
                                secondTermCA = Convert.ToDecimal(fs.MarkObtained);
                                break;
                            }
                        }
                        else
                        {
                            secondTermCA = 0;
                        }

                        PASSIS.LIB.StudentScore scoreRepoExamSecondTerm1 = getSubjectScoreCategoryExam(student.AdmissionNumber, student.Id, session, 2, schoolId, yearId, gradeId, subject.Id);
                        if (scoreRepoExamSecondTerm1 != null)
                        {
                                secondTermExam = Convert.ToDecimal(scoreRepoExamSecondTerm1.ExamScore);
                                break;
                        }
                        else
                        {
                            secondTermExam = 0;
                        }

                        secondTermTotal = secondTermCA + secondTermExam;
                    }

                    if (secondTermTotal > 0)
                    {
                        PdfPCell TotalSecondTermCell = new PdfPCell(new Phrase(Math.Round(secondTermTotal, 0).ToString(), darkerGrnFnt8)); TotalSecondTermCell.Colspan = 1;
                        resTable.AddCell(TotalSecondTermCell);
                    }
                    else
                    {
                        PdfPCell TotalSecondTermCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); TotalSecondTermCell.Colspan = 1;
                        resTable.AddCell(TotalSecondTermCell);
                    }
                    //////.............................................................. average score...................................................

                    averageScore = (totalScore + firstTermTotal + secondTermTotal) / 3;


                    if (averageScore > 0)
                    {
                        PdfPCell AverageCell = new PdfPCell(new Phrase(Math.Round(averageScore, 1).ToString(), darkerGrnFnt8)); AverageCell.Colspan = 1;
                        resTable.AddCell(AverageCell);
                    }
                    else
                    {
                        PdfPCell AverageCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); AverageCell.Colspan = 1;
                        resTable.AddCell(AverageCell);
                    }

                    if (averageScore > 0)
                    {
                        PdfPCell gradeCell = new PdfPCell(new Phrase((PASSIS.LIB.Utility.Utili.getExamGradeLetters(Math.Round(averageScore, 1), (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue))), darkerGrnFnt8)); gradeCell.Colspan = 1;
                        resTable.AddCell(gradeCell);

                        //resTable.AddCell(space1);

                        PdfPCell tRemark = new PdfPCell(new Phrase(PASSIS.LIB.Utility.Utili.getExamGradeRemarks(Math.Round(averageScore, 1), (long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue)), darkerGrnFnt8)); tRemark.Colspan = 2;
                        resTable.AddCell(tRemark);
                    }
                    else
                    {
                        PdfPCell gradeCell = new PdfPCell(new Phrase("", darkerGrnFnt8)); gradeCell.Colspan = 1;
                        resTable.AddCell(gradeCell);

                        //resTable.AddCell(space1);

                        PdfPCell tRemark = new PdfPCell(new Phrase("", darkerGrnFnt8)); tRemark.Colspan = 2;
                        resTable.AddCell(tRemark);
                    }

                    //}

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


            subjectCounter = 0;

        }

        PdfPTable psychomotorTable = new PdfPTable(5);
        PdfPCell row1col1 = new PdfPCell(new Phrase("PSYCHOMETER REPORTS", blackFnt6)); row1col1.Colspan = 4; row1col1.HorizontalAlignment = Element.ALIGN_LEFT;/* row1col1.BackgroundColor = darkergrnFnt.Color;*/
        PdfPCell row1col2 = new PdfPCell(new Phrase("RATES", whiteFnt6)); row1col2.Colspan = 1; /*row1col2.BackgroundColor = darkergrnFnt.Color;*/
        //PdfPCell row2col1 = new PdfPCell(new Phrase("HAND WRITING", blackFnt6)); row2col1.Colspan = 4; row2col1.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell row2col2 = new PdfPCell(new Phrase("2", blackFnt6)); row2col2.Colspan = 1;

        psychomotorTable.AddCell(row1col1);
        psychomotorTable.AddCell(row1col2);
        //psychomotorTable.AddCell(row2col1);
        //psychomotorTable.AddCell(row2col2);

        PdfPTable scaleTable = new PdfPTable(3);
        PdfPCell scalerow0col1 = new PdfPCell(new Phrase("KEY TO RATING", blackFnt6)); scalerow0col1.Colspan = 2; scalerow0col1.HorizontalAlignment = Element.ALIGN_CENTER; /*scalerow0col1.BackgroundColor = darkergrnFnt.Color;*/
        PdfPCell scalerow1col1 = new PdfPCell(new Phrase("RATES", blackFnt6)); scalerow1col1.Colspan = 1; /*scalerow1col1.Border = 0;*/
        PdfPCell scalerow2col1 = new PdfPCell(new Phrase("EXCELLENT LEVEL", blackFnt6)); scalerow2col1.Colspan = 2; /*scalerow2col1.Border = 0;*/
        PdfPCell scalerow3col1 = new PdfPCell(new Phrase("5", blackFnt6)); scalerow3col1.Colspan = 1; /*scalerow3col1.Border = 0;*/
        PdfPCell scalerow4col1 = new PdfPCell(new Phrase("HIGH LEVEL", blackFnt6)); scalerow4col1.Colspan = 2; /*scalerow4col1.Border = 0;*/
        PdfPCell scalerow5col1 = new PdfPCell(new Phrase("4", blackFnt6)); scalerow5col1.Colspan = 1; /*scalerow5col1.Border = 0;*/
        PdfPCell scalerow6col1 = new PdfPCell(new Phrase("ACCEPTABLE LEVEL", blackFnt6)); scalerow6col1.Colspan = 2; /*scalerow6col1.Border = 0;*/
        PdfPCell scalerow7col1 = new PdfPCell(new Phrase("3", blackFnt6)); scalerow7col1.Colspan = 1;/* scalerow7col1.Border = 0;*/
        PdfPCell scalerow8col1 = new PdfPCell(new Phrase("MINIMAL LEVEL", blackFnt6)); scalerow8col1.Colspan = 2; /*scalerow8col1.Border = 0;*/
        PdfPCell scalerow9col1 = new PdfPCell(new Phrase("2", blackFnt6)); scalerow9col1.Colspan = 1; /*scalerow9col1.Border = 0;*/
        PdfPCell scalerow10col1 = new PdfPCell(new Phrase("LOW LEVEL", blackFnt6)); scalerow10col1.Colspan = 2; /*scalerow10col1.Border = 0;*/
        PdfPCell scalerow11col1 = new PdfPCell(new Phrase("1", blackFnt6)); scalerow11col1.Colspan = 1; /*scalerow5col1.Border = 0;*/


        scaleTable.AddCell(scalerow0col1);
        scaleTable.AddCell(scalerow1col1);
        scaleTable.AddCell(scalerow2col1);
        scaleTable.AddCell(scalerow3col1);
        scaleTable.AddCell(scalerow4col1);
        scaleTable.AddCell(scalerow5col1);
        scaleTable.AddCell(scalerow6col1);
        scaleTable.AddCell(scalerow7col1);
        scaleTable.AddCell(scalerow8col1);
        scaleTable.AddCell(scalerow9col1);
        scaleTable.AddCell(scalerow10col1);
        scaleTable.AddCell(scalerow11col1);



        PdfPTable affectiveTable = new PdfPTable(5);
        PdfPCell affectiverow1col1 = new PdfPCell(new Phrase("AFFECTIVE REPORTS", blackFnt6)); affectiverow1col1.Colspan = 4; affectiverow1col1.HorizontalAlignment = Element.ALIGN_LEFT; /*affectiverow1col1.BackgroundColor = darkergrnFnt.Color;*/
        PdfPCell affectiverow2col1 = new PdfPCell(new Phrase("RATES", blackFnt6)); affectiverow2col1.Colspan = 1; /*affectiverow2col1.BackgroundColor = darkergrnFnt.Color;*/
        //PdfPCell affectiverow1col2 = new PdfPCell(new Phrase("PUCTUALITY", blackFnt6)); affectiverow1col2.Colspan = 4; affectiverow1col2.HorizontalAlignment = Element.ALIGN_LEFT;
        //PdfPCell affectiverow2col2 = new PdfPCell(new Phrase("5", blackFnt6)); affectiverow2col2.Colspan = 1;

        affectiveTable.AddCell(affectiverow1col1);
        affectiveTable.AddCell(affectiverow2col1);
        //affectiveTable.AddCell(affectiverow1col2);
        //affectiveTable.AddCell(affectiverow2col2);

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
            PdfPCell psychoCells1 = new PdfPCell(new Phrase(s.SubCategory, blackFnt8)); psychoCells1.Colspan = 4;
            psychomotorTable.AddCell(psychoCells1);
            if (ssb != null)
            {
                PdfPCell psychoCells2 = new PdfPCell(new Phrase(Convert.ToInt16(ssb.MarkObtained).ToString(), blackFnt8)); psychoCells2.Colspan = 1;
                psychomotorTable.AddCell(psychoCells2);
            }
            else if (ssb == null)
            {
                PdfPCell psychoCells2 = new PdfPCell(new Phrase("", blackFnt8)); psychoCells2.Colspan = 1;
                psychomotorTable.AddCell(psychoCells2);
            }
        }

        foreach (ScoreSubCategoryConfiguration s in behavioral)
        {
            StudentScoreBehavioral ssb = context.StudentScoreBehaviorals.FirstOrDefault(x => x.SubCategoryId == s.Id && x.StudentId == student.Id && x.AdmissionNo == student.AdmissionNumber && x.SessionId == session && x.TermId == term && x.SchoolId == schoolId && x.ClassId == yearId && x.GradeId == gradeId);
            PdfPCell affectiveCells1 = new PdfPCell(new Phrase(s.SubCategory, blackFnt8)); affectiveCells1.Colspan = 4;
            affectiveTable.AddCell(affectiveCells1);
            if (ssb != null)
            {
                PdfPCell affectiveCells2 = new PdfPCell(new Phrase(Convert.ToInt16(ssb.MarkObtained).ToString(), blackFnt8)); affectiveCells2.Colspan = 1;
                affectiveTable.AddCell(affectiveCells2);
            }
            else if (ssb == null)
            {
                PdfPCell affectiveCells2 = new PdfPCell(new Phrase("", blackFnt8)); affectiveCells2.Colspan = 1;
                affectiveTable.AddCell(affectiveCells2);
            }
        }



        PdfPTable baseTbl = new PdfPTable(12);
        PdfPCell psych = new PdfPCell(psychomotorTable); psych.Colspan = 5;
        PdfPCell scale = new PdfPCell(scaleTable); scale.Colspan = 2;
        PdfPCell affectiveAreas = new PdfPCell(affectiveTable); affectiveAreas.Colspan = 5;


        baseTbl.AddCell(psych);
        baseTbl.AddCell(affectiveAreas);
        baseTbl.AddCell(scale);


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
        //ReportCardNextTermBegin date = context.ReportCardNextTermBegins.OrderByDescending(x => x.SchoolID == logonUser.SchoolId && x.CampusID == logonUser.SchoolCampusId).FirstOrDefault();
        //PdfPTable lastTable = new PdfPTable(10);
        //PdfPCell EmptyCell = new PdfPCell(new Phrase(" ", blackFnt8)); EmptyCell.Colspan = 10; EmptyCell.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell1Row1.Border = 0;
        //PdfPCell lastCell1Row1 = new PdfPCell(new Phrase("Next Term Begins", blackFnt8)); lastCell1Row1.Colspan = 2; lastCell1Row1.HorizontalAlignment = Element.ALIGN_LEFT; //lastCell1Row1.Border = 0;
        //lastTable.AddCell(lastCell1Row1);
        //if (date != null)
        //{
        //    PdfPCell lastCell2Row1 = new PdfPCell(new Phrase(date.NextTermBegins.ToString(), blackFnt8)); lastCell2Row1.Colspan = 8; lastCell2Row1.HorizontalAlignment = Element.ALIGN_LEFT;// lastCell2Row1.Border = 0;
        //    lastTable.AddCell(lastCell2Row1);
        //}
        //else
        //{
        //    PdfPCell lastCell2Row1 = new PdfPCell(new Phrase("", blackFnt8)); lastCell2Row1.Colspan = 8; lastCell2Row1.HorizontalAlignment = Element.ALIGN_LEFT;// lastCell2Row1.Border = 0;
        //    lastTable.AddCell(lastCell2Row1);
        //}




        PdfPTable commentsTable = new PdfPTable(7);

        PdfPCell commentsRow1Cell1 = new PdfPCell(new Phrase("TOTAL SCORE", blackFnt6)); commentsRow1Cell1.Colspan = 2; commentsRow1Cell1.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow1Cell1.Border = 0;*/
        PdfPCell commentsRow1Cell2 = new PdfPCell(new Phrase(aggregateTotalScore.ToString(), blackFnt6)); commentsRow1Cell2.Colspan = 1; commentsRow1Cell2.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow1Cell2.Border = 0;*/
        PdfPCell commentsRow1Cell3 = new PdfPCell(new Phrase("PERCENTAGE", blackFnt6)); commentsRow1Cell3.Colspan = 1; commentsRow1Cell3.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow1Cell2.Border = 0;*/
        PdfPCell commentsRow1Cell4 = new PdfPCell(new Phrase(percentage.ToString()+"%", blackFnt6)); commentsRow1Cell4.Colspan = 3; commentsRow1Cell4.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow1Cell2.Border = 0;*/

        PdfPCell commentsRow2Cell1 = new PdfPCell(new Phrase("TIMES SCHOOL OPENED", blackFnt6)); commentsRow2Cell1.Colspan = 2; commentsRow2Cell1.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow2Cell1.Border = 0;*/
        PdfPCell commentsRow2Cell2 = new PdfPCell(new Phrase("", blackFnt6)); commentsRow2Cell2.Colspan = 1; commentsRow2Cell2.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow2Cell2.Border = 0;*/
        PdfPCell commentsRow2Cell3 = new PdfPCell(new Phrase("ATTENDANCE", blackFnt6)); commentsRow2Cell3.Colspan = 1; commentsRow2Cell3.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow2Cell2.Border = 0;*/
        PdfPCell commentsRow2Cell4 = new PdfPCell(new Phrase("", blackFnt6)); commentsRow2Cell4.Colspan = 3; commentsRow2Cell4.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow2Cell2.Border = 0;*/

        PdfPCell commentsRow3Cell1 = new PdfPCell(new Phrase("CLASS TEACHER REMARKS", blackFnt6)); commentsRow3Cell1.Colspan = 2; commentsRow3Cell1.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow3Cell1.Border = 0;*/
        PdfPCell commentsRow3Cell2 = new PdfPCell(new Phrase(classTeacherComment, blackFnt6)); commentsRow3Cell2.Colspan = 5; commentsRow3Cell2.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow3Cell2.Border = 0;*/

        PdfPCell commentsRow4Cell1 = new PdfPCell(new Phrase("PRINCIPALS REMARK", blackFnt6)); commentsRow4Cell1.Colspan = 2; commentsRow4Cell1.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow4Cell1.Border = 0;*/
        PdfPCell commentsRow4Cell2 = new PdfPCell(new Phrase(headTeacherComment, blackFnt6)); commentsRow4Cell2.Colspan = 5; commentsRow4Cell2.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow4Cell2.Border = 0;*/

        PdfPCell commentsRow5Cell1 = new PdfPCell(new Phrase("PRINICIPALS SIGNATURE", blackFnt6)); commentsRow5Cell1.Colspan = 2; commentsRow5Cell1.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow5Cell1.Border = 0;*/
        PdfPCell commentsRow5Cell2 = new PdfPCell(sign); commentsRow5Cell2.Colspan = 1; commentsRow5Cell2.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow5Cell2.Border = 0;*/
        PdfPCell commentsRow6Cell1 = new PdfPCell(new Phrase("DATE", blackFnt6)); commentsRow6Cell1.Colspan = 1; commentsRow6Cell1.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow6Cell1.Border = 0;*/
        PdfPCell commentsRow6Cell2 = new PdfPCell(new Phrase(DateTime.Now.ToString("M/dd/yyyy"), blackFnt6)); commentsRow6Cell2.Colspan = 3; commentsRow6Cell2.HorizontalAlignment = Element.ALIGN_LEFT; /*commentsRow6Cell2.Border = 0;*/

        commentsTable.AddCell(commentsRow1Cell1);
        commentsTable.AddCell(commentsRow1Cell2);
        commentsTable.AddCell(commentsRow1Cell3);
        commentsTable.AddCell(commentsRow1Cell4);

        commentsTable.AddCell(commentsRow2Cell1);
        commentsTable.AddCell(commentsRow2Cell2);
        commentsTable.AddCell(commentsRow2Cell3);
        commentsTable.AddCell(commentsRow2Cell4);

        commentsTable.AddCell(commentsRow3Cell1);
        commentsTable.AddCell(commentsRow3Cell2);

        commentsTable.AddCell(commentsRow4Cell1);
        commentsTable.AddCell(commentsRow4Cell2);

        commentsTable.AddCell(commentsRow5Cell1);
        commentsTable.AddCell(commentsRow5Cell2);

        commentsTable.AddCell(commentsRow6Cell1);
        commentsTable.AddCell(commentsRow6Cell2);


        PdfPTable gradeTable = new PdfPTable(4);
        PdfPCell gradeRow1Cell1 = new PdfPCell(new Phrase("SCORE", blackFnt6)); gradeRow1Cell1.Colspan = 1; gradeRow1Cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        PdfPCell gradeRow1Cell2 = new PdfPCell(new Phrase("GRADE", blackFnt6)); gradeRow1Cell2.Colspan = 1; gradeRow1Cell2.HorizontalAlignment = Element.ALIGN_LEFT;
        PdfPCell gradeRow1Cell3 = new PdfPCell(new Phrase("REMARK", blackFnt6)); gradeRow1Cell3.Colspan = 2; gradeRow1Cell3.HorizontalAlignment = Element.ALIGN_LEFT;

        PdfPCell gradeRow2Cell1 = new PdfPCell(new Phrase("50", blackFnt6)); gradeRow2Cell1.Colspan = 1; gradeRow2Cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        PdfPCell gradeRow2Cell2 = new PdfPCell(new Phrase("D", blackFnt6)); gradeRow2Cell2.Colspan = 1; gradeRow2Cell2.HorizontalAlignment = Element.ALIGN_LEFT;
        PdfPCell gradeRow2Cell3 = new PdfPCell(new Phrase("AVERAGE", blackFnt6)); gradeRow2Cell3.Colspan = 2; gradeRow2Cell3.HorizontalAlignment = Element.ALIGN_LEFT;

        PdfPCell gradeRow3Cell1 = new PdfPCell(new Phrase("60", blackFnt6)); gradeRow3Cell1.Colspan = 1; gradeRow3Cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        PdfPCell gradeRow3Cell2 = new PdfPCell(new Phrase("C", blackFnt6)); gradeRow3Cell2.Colspan = 1; gradeRow3Cell2.HorizontalAlignment = Element.ALIGN_LEFT;
        PdfPCell gradeRow3Cell3 = new PdfPCell(new Phrase("GOOD", blackFnt6)); gradeRow3Cell3.Colspan = 2; gradeRow3Cell3.HorizontalAlignment = Element.ALIGN_LEFT;

        PdfPCell gradeRow4Cell1 = new PdfPCell(new Phrase("70", blackFnt6)); gradeRow4Cell1.Colspan = 1; gradeRow4Cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        PdfPCell gradeRow4Cell2 = new PdfPCell(new Phrase("B", blackFnt6)); gradeRow4Cell2.Colspan = 1; gradeRow4Cell2.HorizontalAlignment = Element.ALIGN_LEFT;
        PdfPCell gradeRow4Cell3 = new PdfPCell(new Phrase("VERY GOOD", blackFnt6)); gradeRow4Cell3.Colspan = 2; gradeRow4Cell3.HorizontalAlignment = Element.ALIGN_LEFT;

        PdfPCell gradeRow5Cell1 = new PdfPCell(new Phrase("80", blackFnt6)); gradeRow5Cell1.Colspan = 1; gradeRow5Cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        PdfPCell gradeRow5Cell2 = new PdfPCell(new Phrase("A", blackFnt6)); gradeRow5Cell2.Colspan = 1; gradeRow5Cell2.HorizontalAlignment = Element.ALIGN_LEFT;
        PdfPCell gradeRow5Cell3 = new PdfPCell(new Phrase("EXCELLENT", blackFnt6)); gradeRow5Cell3.Colspan = 2; gradeRow5Cell3.HorizontalAlignment = Element.ALIGN_LEFT;
        gradeTable.AddCell(gradeRow1Cell1);
        gradeTable.AddCell(gradeRow1Cell2);
        gradeTable.AddCell(gradeRow1Cell3);

        gradeTable.AddCell(gradeRow2Cell1);
        gradeTable.AddCell(gradeRow2Cell2);
        gradeTable.AddCell(gradeRow2Cell3);

        gradeTable.AddCell(gradeRow3Cell1);
        gradeTable.AddCell(gradeRow3Cell2);
        gradeTable.AddCell(gradeRow3Cell3);

        gradeTable.AddCell(gradeRow4Cell1);
        gradeTable.AddCell(gradeRow4Cell2);
        gradeTable.AddCell(gradeRow4Cell3);

        gradeTable.AddCell(gradeRow5Cell1);
        gradeTable.AddCell(gradeRow5Cell2);
        gradeTable.AddCell(gradeRow5Cell3);





        PdfPTable finalTable = new PdfPTable(11);
        PdfPCell commentsTbl = new PdfPCell(commentsTable); commentsTbl.Colspan = 11;
        //PdfPCell gradeTbl = new PdfPCell(gradeTable); gradeTbl.Colspan = 2;
        finalTable.AddCell(commentsTbl);
        //finalTable.AddCell(gradeTbl);



        document.Add(domain);
        document.Add(resTable);
        document.Add(domain);
        document.Add(baseTbl);
        document.Add(finalTable);

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

    public int GetAge(DateTime Dob)
    {
        DateTime now = DateTime.Today;
        int age = now.Year - Dob.Year;
        if (Dob > now.AddYears(-age)) age--;

        return age;
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

    public static IList<StudentScoreRepository> getSubjectScorePerSubcategory(string admNo, long studentId, long sessionId, long termId, long? schId, long yearId, long gradeId, long subId, long catId, long subCatId)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        IList<PASSIS.LIB.StudentScoreRepository> scoreRepo = (from s in context.StudentScoreRepositories
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
        ddlAcademicSession.DataSource = new ParentViewReportCard_748().schSession().Distinct();
        ddlAcademicSession.DataTextField = "SessionName";
        ddlAcademicSession.DataValueField = "ID";
        ddlAcademicSession.DataBind();
        ddlAcademicSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Session--", "0", true));

        ddlAcademicTerm.Items.Clear();
        ddlAcademicTerm.DataSource = new ParentViewReportCard_748().schTerm().Distinct();
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
    public string RetrieveGradeClassTeacher(Int64 Id)
    {
        string fullName = "";
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        PASSIS.LIB.Grade grade = context.Grades.FirstOrDefault(s => s.Id == Id);
        if (grade != null)
        {
            PASSIS.LIB.User usr = context.Users.FirstOrDefault(s => s.Id == grade.GradeTeacherId);
            if (usr != null)
            {
                fullName = usr.FirstName + " " + usr.LastName;
            }
        }
        return fullName;
    }
}