using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;
using PASSIS.LIB.Utility;
using PASSIS.DAO.CustomClasses;
using System.Configuration;
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

public partial class OutstandingPayment : PASSIS.LIB.Utility.BasePage
{

    PASSISLIBDataContext dbcontext = new PASSISLIBDataContext();

    long CurriculumID;
    long SchoolID;
    public static string SchoolName = "";
    public static string SchoolLogo = "";
    public static string SchoolAddress = "";
    public static string SchoolCurriculumId = "";
    public static string SchoolUrl = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {

            //code to fetch the Class based on their curriculum
            long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
            long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
            ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
            ddlYear.DataBind();
            ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Year--", "0"));
            ddlStudent.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Student--", "0"));
            ddlClass.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Class--", "0"));

            //code to fetch the Academic Sessions
            var session = (from c in dbcontext.AcademicSessions
                           where c.SchoolId == logonUser.SchoolId && c.IsCurrent == true
                           orderby c.IsCurrent descending
                           select c.AcademicSessionName).Distinct();

            ddlSession.DataSource = session;
            ddlSession.DataTextField = "SessionName";
            ddlSession.DataValueField = "ID";
            ddlSession.DataBind();
            ddlSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0", true));

            //code to fetch the Academic Terms
            var term = from c in dbcontext.AcademicTerm1s
                       select c;
            ddlTerm.DataSource = term;
            ddlTerm.DataTextField = "AcademicTermName";
            ddlTerm.DataValueField = "Id";
            ddlTerm.DataBind();
            ddlTerm.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0", true));

            // To get the school detail
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
        }

    }


    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int64 selectedYearId = 0L;
        selectedYearId = Convert.ToInt64(ddlYear.SelectedValue);
        ddlClass.Items.Clear();
        var availableGrades = new ClassGradeDAL().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, selectedYearId);
        ddlClass.DataSource = availableGrades;
        ddlClass.DataBind();
        ddlClass.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Class--", "0"));


    }


    protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlStudent.Items.Clear();
        BindGrid(Convert.ToInt64(ddlYear.SelectedValue), Convert.ToInt64(ddlClass.SelectedValue));

    }

    protected void BindGrid(long yearId, long gradeId)
    {
        long campusId, schoolId;
        campusId = logonUser.SchoolCampusId;
        schoolId = (long)logonUser.SchoolId;
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

        IList<PASSIS.LIB.GradeStudent> classList = new ClassGradeLIB().RetrieveGradeStudents(schoolId, campusId, yearId, gradeId, curSessionId);
        ddlStudent.Items.Add(new System.Web.UI.WebControls.ListItem("--Select Student--", "0"));
        foreach (PASSIS.LIB.GradeStudent studentList in classList)
        {
            PASSIS.LIB.User stdList = dbcontext.Users.FirstOrDefault(x => x.Id == studentList.StudentId);
            ddlStudent.Items.Add(new System.Web.UI.WebControls.ListItem(stdList.StudentFullName, stdList.Id.ToString()));
        }
    }






    protected void btnView_OnClick(object sender, EventArgs e)
    {

        try
        {

            if (ddlYear.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Year is Required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;
            }

            if (ddlClass.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Class is required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;
            }
            if (ddlStudent.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Student Name is required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;
            }

            if (ddlSession.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Session is required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;
            }

            if (ddlTerm.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Select the Term";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;

            }


            //code to fetch the InvoiceCodes
            var InvoiceCode = from s in dbcontext.PaymentPermanents
                              where s.SchoolId == logonUser.SchoolId && s.CampusId == logonUser.SchoolCampusId //&& s.ApprovedById == logonUser.Id
                              && Convert.ToInt64(s.StudentId) == (Convert.ToInt64(ddlStudent.SelectedValue))
                              && Convert.ToInt64(s.ClassId) == (Convert.ToInt64(ddlYear.SelectedValue))
                              && Convert.ToInt64(s.GradeId) == (Convert.ToInt64(ddlClass.SelectedValue))
                              && Convert.ToInt64(s.SessionId) == (Convert.ToInt64(ddlSession.SelectedValue))
                              && Convert.ToInt64(s.TermId) == (Convert.ToInt64(ddlTerm.SelectedValue))
                              select s;


            ddlInvoiceCode.DataSource = InvoiceCode;
            ddlInvoiceCode.DataTextField = "InvoiceCode";
            ddlInvoiceCode.DataValueField = "Id";
            ddlInvoiceCode.DataBind();
            ddlInvoiceCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select the Invoice Code---", "0", true));


            var getOutStandSummary = from s in dbcontext.PaymentPermanents
                                     where s.SchoolId == logonUser.SchoolId && s.CampusId == logonUser.SchoolCampusId
                                     //&& s.ApprovedById == logonUser.Id 
                                     && Convert.ToInt64(s.StudentId) == (Convert.ToInt64(ddlStudent.SelectedValue))
                                    && Convert.ToInt64(s.ClassId) == (Convert.ToInt64(ddlYear.SelectedValue))
                                     && Convert.ToInt64(s.GradeId) == (Convert.ToInt64(ddlClass.SelectedValue))
                                     && Convert.ToInt64(s.SessionId) == (Convert.ToInt64(ddlSession.SelectedValue))
                                     && Convert.ToInt64(s.TermId) == (Convert.ToInt64(ddlTerm.SelectedValue))

                                     select new
                                     {
                                         s.User.AdmissionNumber,
                                         s.InvoiceCode,
                                         s.User.FirstName,
                                         s.AmountGenerated,
                                         s.AmountPaid,
                                         s.Balance,
                                         s.Discount,
                                         // s.Total,
                                         // s.IsPaymentCompleted,
                                         //s.PaymentType,
                                         s.DateCreated
                                     };

            gvOutStandPaySummary.DataSource = getOutStandSummary;
            gvOutStandPaySummary.DataBind();
            btnSaveBal.Visible = true;
            lblBalanceToPay.Visible = true;
            txtBalanceToPay.Visible = true;
            lblInvoiceCode.Visible = true;
            ddlInvoiceCode.Visible = true;
            lblSummary.Visible = true;
            lblList.Visible = true;

            var getOutStandPayList = from s in dbcontext.PaymentInvoiceLists
                                     where s.SchoolId == logonUser.SchoolId && s.CampusId == logonUser.SchoolCampusId
                                    && Convert.ToInt64(s.StudentId) == (Convert.ToInt64(ddlStudent.SelectedValue))
                                    && Convert.ToInt64(s.ClassId) == (Convert.ToInt64(ddlYear.SelectedValue))
                                     && Convert.ToInt64(s.GradeId) == (Convert.ToInt64(ddlClass.SelectedValue))
                                     && Convert.ToInt64(s.SessionId) == (Convert.ToInt64(ddlSession.SelectedValue))
                                     && Convert.ToInt64(s.TermId) == (Convert.ToInt64(ddlTerm.SelectedValue))

                                     select new
                                     {

                                         s.User.AdmissionNumber,
                                         s.InvoiceCode,
                                         s.User.FirstName,
                                         Class = s.Class_Grade.Name,
                                         Grade = s.Grade.GradeName,
                                         Session = s.AcademicSessionName.SessionName,
                                         Term = s.AcademicTerm1.AcademicTermName,
                                         Fee = s.PaymentFee.PaymentFeeType.FeeName,
                                         s.Amount


                                     };

            gvOutStandPayList.DataSource = getOutStandPayList;
            gvOutStandPayList.DataBind();
        }

        catch (Exception ex)
        {
            lblMessage.Text = "An Error occurred, Make Sure All Fields are Selected!";
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }




    }

    protected void gvOutStandPayList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvOutStandPayList.PageIndex = e.NewPageIndex;
        gvOutStandPayList.DataBind();
    }


    protected void gvOutStandPaySummary_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvOutStandPaySummary.PageIndex = e.NewPageIndex;
        gvOutStandPaySummary.DataBind();
    }







    protected void btnSaveBal_OnClick(object sender, EventArgs e)
    {
        try
        {

            if (ddlYear.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Year is Required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;
            }

            if (ddlClass.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Class is required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;
            }
            if (ddlStudent.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Student Name is required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;
            }

            if (ddlSession.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Session is required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;
            }

            if (ddlTerm.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Select the Term";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;

            }

            if (ddlInvoiceCode.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Select the Invoice Code";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;

            }

            if (txtBalanceToPay.Text == "")
            {
                lblErrorMsg.Text = "Amount to Pay is Required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;
            }

            PASSIS.LIB.GradeStudent grdStudent = dbcontext.GradeStudents.FirstOrDefault(x => x.StudentId == Convert.ToInt64(ddlStudent.SelectedValue));
            if (grdStudent != null)
            {
                long stdGrade = grdStudent.GradeId;
            }


            //code to fetch Student's Parent Id
            PASSIS.LIB.ParentStudentMap getStudentParentId = dbcontext.ParentStudentMaps.FirstOrDefault(x => x.StudentId == Convert.ToInt64(ddlStudent.SelectedValue));

            string invoiceCode = ddlInvoiceCode.SelectedItem.ToString();
            long studentId = Convert.ToInt64(ddlStudent.SelectedValue);
            long parentId = (long)getStudentParentId.ParentUserId;
            long classId = Convert.ToInt64(ddlYear.SelectedValue);
            long gradeId = grdStudent.GradeId;
            long? schoolId = logonUser.SchoolId;
            long campusId = logonUser.SchoolCampusId;
            long approvedById = logonUser.Id;
            long sessionId = Convert.ToInt64(ddlSession.SelectedValue);
            long termId = Convert.ToInt64(ddlTerm.SelectedValue);
            string PaymentType = "Cash";
            int ApprovalStatusId = 2;





            //code to fetch student payment details
            PASSIS.LIB.PaymentPermanent getBalancePayment = dbcontext.PaymentPermanents.FirstOrDefault(
            x => x.SchoolId == logonUser.SchoolId && x.CampusId == logonUser.SchoolCampusId
                //&& x.ApprovedById == logonUser.Id
                && x.StudentId == Convert.ToInt64(ddlStudent.SelectedValue) && x.ClassId == Convert.ToInt64(ddlYear.SelectedValue) && x.GradeId == Convert.ToInt64(ddlClass.SelectedValue)
                && x.SessionId == Convert.ToInt64(ddlSession.SelectedValue) && x.TermId == Convert.ToInt64(ddlTerm.SelectedValue) && x.InvoiceCode == ddlInvoiceCode.SelectedItem.ToString()

            );

            if (getBalancePayment != null)
            {


                if (Convert.ToDecimal(txtBalanceToPay.Text.ToString()) > getBalancePayment.Balance)
                {

                    lblErrorMsg.Text = "Amount is greater than the Balance or No Outstanding Payment!";
                    lblMessage.Text = " ";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                    //trErrorMsg.Visible = true;
                    return;
                }

                if (Convert.ToDecimal(txtBalanceToPay.Text.ToString()) <= 0)
                {

                    lblErrorMsg.Text = "Amount Cannot be less than or equal to Zero!";
                    lblMessage.Text = " ";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                    //trErrorMsg.Visible = true;
                    return;
                }

                PASSIS.LIB.PaymentPermanent updateBalancePayment = dbcontext.PaymentPermanents.FirstOrDefault(x => x.SchoolId == logonUser.SchoolId && x.CampusId == logonUser.SchoolCampusId
                //&& x.ApprovedById == logonUser.Id
                && x.StudentId == Convert.ToInt64(ddlStudent.SelectedValue) && x.ClassId == Convert.ToInt64(ddlYear.SelectedValue) && x.GradeId == Convert.ToInt64(ddlClass.SelectedValue)
                && x.SessionId == Convert.ToInt64(ddlSession.SelectedValue) && x.TermId == Convert.ToInt64(ddlTerm.SelectedValue) && x.Balance != null && x.InvoiceCode == ddlInvoiceCode.SelectedItem.ToString());



                if (Convert.ToDecimal(txtBalanceToPay.Text.ToString()) == Convert.ToDecimal(getBalancePayment.Balance))
                {
                    Decimal newAmountGenerated = Convert.ToDecimal(txtBalanceToPay.Text.ToString()) + Convert.ToDecimal(getBalancePayment.AmountPaid);

                    updateBalancePayment.Balance = null;
                    updateBalancePayment.AmountPaid = newAmountGenerated;
                    dbcontext.SubmitChanges();


                    PaymentTemporary paymentTemporary = new PaymentTemporary();
                    paymentTemporary.InvoiceCode = invoiceCode;
                    paymentTemporary.StudentId = studentId;
                    paymentTemporary.ParentId = parentId;
                    paymentTemporary.ClassId = classId;
                    paymentTemporary.GradeId = gradeId;
                    paymentTemporary.SchoolId = schoolId;
                    paymentTemporary.CampusId = campusId;
                    paymentTemporary.SessionId = sessionId;
                    paymentTemporary.TermId = termId;
                    paymentTemporary.Amount = Convert.ToDecimal(txtBalanceToPay.Text);
                    paymentTemporary.Date = DateTime.Now;
                    paymentTemporary.PermanentPaymentId = updateBalancePayment.Id;
                    paymentTemporary.ApprovalStatusId = ApprovalStatusId;
                    paymentTemporary.ApprovedById = logonUser.Id;
                    dbcontext.PaymentTemporaries.InsertOnSubmit(paymentTemporary);
                    dbcontext.SubmitChanges();

                    PASSIS.LIB.User studentUser = dbcontext.Users.FirstOrDefault(x => x.Id == Convert.ToInt64(ddlStudent.SelectedValue));
                    ReceiptPrinting(studentUser, invoiceCode, paymentTemporary.Id, Convert.ToDecimal(paymentTemporary.PaymentPermanent.AmountGenerated));


                    lblErrorMsg.Text = "Balance paid and Updated Sucessfully!";
                    lblMessage.Text = " ";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                    lblErrorMsg.Visible = true;
                    //trErrorMsg.Visible = true;
                    txtBalanceToPay.Text = " ";
                }
                else if (Convert.ToDecimal(txtBalanceToPay.Text.ToString()) != Convert.ToDecimal(getBalancePayment.Balance))
                {
                    //Decimal newAmountGenerated = Convert.ToDecimal(txtBalanceToPay.Text.ToString()) + Convert.ToDecimal(getBalancePayment.Balance);

                    Decimal newBalance = Convert.ToDecimal(getBalancePayment.Balance) - Convert.ToDecimal(txtBalanceToPay.Text.ToString());
                    Decimal newAmountPaid = Convert.ToDecimal(getBalancePayment.AmountPaid) + Convert.ToDecimal(txtBalanceToPay.Text.ToString());

                    updateBalancePayment.Balance = newBalance;
                    updateBalancePayment.AmountPaid = newAmountPaid;
                    dbcontext.SubmitChanges();

                    PaymentTemporary paymentTemporary = new PaymentTemporary();
                    paymentTemporary.InvoiceCode = invoiceCode;
                    paymentTemporary.StudentId = studentId;
                    paymentTemporary.ParentId = parentId;
                    paymentTemporary.ClassId = classId;
                    paymentTemporary.GradeId = gradeId;
                    paymentTemporary.SchoolId = schoolId;
                    paymentTemporary.CampusId = campusId;
                    paymentTemporary.SessionId = sessionId;
                    paymentTemporary.TermId = termId;
                    paymentTemporary.Amount = Convert.ToDecimal(txtBalanceToPay.Text);
                    paymentTemporary.Date = DateTime.Now;
                    paymentTemporary.PermanentPaymentId = updateBalancePayment.Id;
                    paymentTemporary.ApprovalStatusId = ApprovalStatusId;
                    paymentTemporary.ApprovedById = logonUser.Id;
                    dbcontext.PaymentTemporaries.InsertOnSubmit(paymentTemporary);
                    dbcontext.SubmitChanges();

                    PASSIS.LIB.User studentUser = dbcontext.Users.FirstOrDefault(x => x.Id == Convert.ToInt64(ddlStudent.SelectedValue));
                    ReceiptPrinting(studentUser, invoiceCode, paymentTemporary.Id, Convert.ToDecimal(paymentTemporary.PaymentPermanent.AmountGenerated));

                    lblErrorMsg.Text = "Balance paid and Updated Sucessfully!";
                    lblMessage.Text = " ";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                    lblErrorMsg.Visible = true;
                    //trErrorMsg.Visible = true;
                    txtBalanceToPay.Text = " ";
                }





            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "An Error Occured!";
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }

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

    void Download(PASSIS.LIB.User student, Document document, UsersLIB usrDal, string invoiceCode, long paymentId, decimal total)
    {
        try
        {
            //PASSIS.DAO.User student = usrDal.RetrieveByAdmissionNumber(admNumber);
            var scoresinEachSubject = new ScoresheetLIB().ReportCard_SubjectScore(student.AdmissionNumber);

            BaseColor bcFaintRed = new BaseColor(169, 34, 82);
            string imagepath = Server.MapPath(SchoolLogo); // "\\images\\";
            //iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(imagepath + "/greenLogo.png");
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(imagepath);
            iTextSharp.text.Image backgroundLogo = iTextSharp.text.Image.GetInstance(imagepath);
            //Resize image depend upon your need
            //For give the size to image //backgroundLogo.ScaleToFit(150, 375);


            addResultSummaryPage(document, student, usrDal, invoiceCode, paymentId, total);

        }
        catch (Exception ex)
        {
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
        }
    }

    protected void addResultSummaryPage(Document document, PASSIS.LIB.User student, UsersLIB usrDal, string invoiceCode, long paymentId, decimal Total)
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
        Paragraph p = new Paragraph("Cash Payment Receipt (Balance)", darkerRedFnt);
        p.Alignment = Element.ALIGN_CENTER;
        document.Add(p);
        Paragraph SessionDetails = new Paragraph(string.Format("{0} {1} {2} Session", ddlTerm.SelectedItem.Text, ",", ddlSession.SelectedItem.Text), darkerRedFnt);
        SessionDetails.Alignment = Element.ALIGN_CENTER;
        document.Add(SessionDetails);

        //document.Add(new Phrase(Environment.NewLine));
        document.Add(new Phrase(Environment.NewLine));

        PdfPTable infoTable = new PdfPTable(10);
        PdfPCell cell1 = new PdfPCell(new Phrase("NAME")); cell1.Colspan = 3; cell1.HorizontalAlignment = Element.ALIGN_LEFT;
        PdfPCell cell2 = new PdfPCell(new Phrase(student.StudentFullName)); cell2.Colspan = 7; cell1.HorizontalAlignment = Element.ALIGN_LEFT;

        PdfPCell row2cell1 = new PdfPCell(new Phrase("CLASS")); row2cell1.HorizontalAlignment = Element.ALIGN_LEFT; row2cell1.Colspan = 2; //row2cell1.Border = 0;
        PdfPCell row2cell2 = new PdfPCell(new Phrase(usrDal.getStudentsHomeRoom(student.Id))); row2cell2.Colspan = 8; row2cell2.HorizontalAlignment = Element.ALIGN_LEFT;

        PdfPCell row3cell1 = new PdfPCell(new Phrase("ADMISSION NUMBER")); row3cell1.Colspan = 3; row3cell1.HorizontalAlignment = Element.ALIGN_LEFT; //row4cell1.Border = 0;
        PdfPCell row3cell2 = new PdfPCell(new Phrase(student.AdmissionNumber)); row3cell2.HorizontalAlignment = Element.ALIGN_LEFT; row3cell2.Colspan = 7;

        PdfPCell row4cell1 = new PdfPCell(new Phrase("INVOICE NUMBER")); row4cell1.Colspan = 3; row4cell1.HorizontalAlignment = Element.ALIGN_LEFT; //row4cell1.Border = 0;
        PdfPCell row4cell2 = new PdfPCell(new Phrase(invoiceCode)); row4cell2.HorizontalAlignment = Element.ALIGN_LEFT; row4cell2.Colspan = 7;
        infoTable.AddCell(cell1);
        infoTable.AddCell(cell2);

        infoTable.AddCell(row2cell1);
        infoTable.AddCell(row2cell2);

        infoTable.AddCell(row3cell1);
        infoTable.AddCell(row3cell2);

        infoTable.AddCell(row4cell1);
        infoTable.AddCell(row4cell2);

        document.Add(infoTable);


        //var invoiceList = from s in context.PaymentInvoiceLists where s.InvoiceCode == invoiceCode select s;
        PaymentTemporary paymentTemporary = dbcontext.PaymentTemporaries.FirstOrDefault(x => x.Id == paymentId);

        PdfPTable receiptTable = new PdfPTable(12);

        PdfPCell invoiceHeader = new PdfPCell(new Phrase("Payment Code", darkerGrnFnt9)); //feeTypeHeader.Padding = 0f;
        invoiceHeader.Colspan = 2; //feeTypeHeader.HorizontalAlignment = Element.ALIGN_LEFT; feeTypeHeader.VerticalAlignment = Element.ALIGN_BOTTOM;

        PdfPCell amountGenHeader = new PdfPCell(new Phrase("Amount Generated(N)", darkerGrnFnt9)); //feeTypeHeader.Padding = 0f;
        amountGenHeader.Colspan = 3;

        PdfPCell totalAmountPaidHeader = new PdfPCell(new Phrase("Total Amount Paid(N)", darkerGrnFnt9)); //feeTypeHeader.Padding = 0f;
        totalAmountPaidHeader.Colspan = 3;

        PdfPCell amountHeader = new PdfPCell(new Phrase("Amount Paid(N)", darkerGrnFnt9)); //amountHeader.Padding = 0f;
        amountHeader.Colspan = 2; //amountHeader.HorizontalAlignment = Element.ALIGN_LEFT; amountHeader.VerticalAlignment = Element.ALIGN_BOTTOM;

        PdfPCell balanceHeader = new PdfPCell(new Phrase("Balance(N)", darkerGrnFnt9)); //feeTypeHeader.Padding = 0f;
        balanceHeader.Colspan = 2;
        receiptTable.AddCell(invoiceHeader);
        receiptTable.AddCell(amountGenHeader);
        receiptTable.AddCell(totalAmountPaidHeader);
        receiptTable.AddCell(amountHeader);
        receiptTable.AddCell(balanceHeader);

        //foreach (PaymentInvoiceList list in invoiceList)
        //{
        PdfPCell invoice = new PdfPCell(new Phrase(paymentTemporary.InvoiceCode, resultTitleRedFnt10)); //feeType.Padding = 0f;
        invoice.Colspan = 2;

        PdfPCell amountGen = new PdfPCell(new Phrase(paymentTemporary.PaymentPermanent.AmountGenerated.ToString(), resultTitleRedFnt10)); //feeType.Padding = 0f;
        amountGen.Colspan = 3;

        PdfPCell totalAmountPaid = new PdfPCell(new Phrase(paymentTemporary.PaymentPermanent.AmountPaid.ToString(), resultTitleRedFnt10)); //feeType.Padding = 0f;
        totalAmountPaid.Colspan = 3;

        PdfPCell amount = new PdfPCell(new Phrase(paymentTemporary.Amount.ToString(), resultTitleRedFnt10)); //amount.Padding = 0f;
        amount.Colspan = 2;

        PdfPCell balance = new PdfPCell(new Phrase(paymentTemporary.PaymentPermanent.Balance.ToString(), resultTitleRedFnt10)); //amount.Padding = 0f;
        balance.Colspan = 2;
        receiptTable.AddCell(invoice);
        receiptTable.AddCell(amountGen);
        receiptTable.AddCell(totalAmountPaid);
        receiptTable.AddCell(amount);
        receiptTable.AddCell(balance);
        //}
        PdfPCell totalCell1 = new PdfPCell(new Phrase("Amount paid(N):")); totalCell1.Colspan = 4; totalCell1.HorizontalAlignment = Element.ALIGN_LEFT; totalCell1.Border = 0;
        PdfPCell totalCell2 = new PdfPCell(new Phrase(paymentTemporary.Amount.ToString())); totalCell2.Colspan = 8; totalCell2.HorizontalAlignment = Element.ALIGN_LEFT; totalCell2.Border = 0;

        PdfPCell totalCell3 = new PdfPCell(new Phrase("Amount paid in words:")); totalCell3.Colspan = 4; totalCell3.HorizontalAlignment = Element.ALIGN_LEFT; totalCell3.Border = 0;
        PdfPCell totalCell4 = new PdfPCell(new Phrase(NumberToWords(Convert.ToInt64(paymentTemporary.Amount)) + " naira only")); totalCell4.Colspan = 8; totalCell4.HorizontalAlignment = Element.ALIGN_LEFT; totalCell4.Border = 0;
        receiptTable.AddCell(totalCell1);
        receiptTable.AddCell(totalCell2);

        receiptTable.AddCell(totalCell3);
        receiptTable.AddCell(totalCell4);

        document.Add(receiptTable);
    }

    public void ReceiptPrinting(PASSIS.LIB.User selectedUsers, string invoiceCode, long paymentId, decimal total)
    {
        Document document = new Document();    // instantiate a iTextSharp.text.pdf.Document
        document.SetMargins(0f, 10f, 30f, 0f);
        MemoryStream mem = new MemoryStream(); // PDF data will be written here
        PdfWriter.GetInstance(document, mem);  // tie a PdfWriter instance to the stream
        document.SetPageSize(iTextSharp.text.PageSize.A4_LANDSCAPE);
        document.Open();
        UsersLIB usrdal = new UsersLIB();
        Download(selectedUsers, document, usrdal, invoiceCode, paymentId, total);
        document.NewPage();

        //document.Close();   // automatically closes the attached MemoryStream

        byte[] docData = mem.GetBuffer(); // get the generated PDF as raw data

        // write the document data to response stream and set appropriate headers:
        string filename = string.Format("{0}.pdf", DateTime.Now.Millisecond);
        //Response.AppendHeader("Content-Disposition", "attachment; filename=testdoc.pdf");
        Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
        Response.ContentType = "application/pdf";
        Response.BinaryWrite(docData);
        Response.End();
    }

    public static string NumberToWords(long number)
    {
        if (number == 0)
            return "zero";

        if (number < 0)
            return "minus " + NumberToWords(Math.Abs(number));

        string words = "";

        if ((number / 1000000) > 0)
        {
            words += NumberToWords(number / 1000000) + " million ";
            number %= 1000000;
        }

        if ((number / 1000) > 0)
        {
            words += NumberToWords(number / 1000) + " thousand ";
            number %= 1000;
        }

        if ((number / 100) > 0)
        {
            words += NumberToWords(number / 100) + " hundred ";
            number %= 100;
        }

        if (number > 0)
        {
            if (words != "")
                words += "and ";

            var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += "-" + unitsMap[number % 10];
            }
        }

        return words;
    }
}