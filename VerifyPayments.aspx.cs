using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PASSIS.DAO;
using PASSIS.LIB;

public partial class VerifyPayments : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    public static string SchoolName = "";
    public static string SchoolLogo = "";
    public static string SchoolAddress = "";
    public static string SchoolCurriculumId = "";
    public static string SchoolUrl = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //LoadPayment();
            var schlTypeId = context.Schools.FirstOrDefault(x => x.Id == (long)logonUser.SchoolId).SchoolTypeId;
            ddlYear.DataTextField = "Name";
            ddlYear.DataValueField = "Id";
            ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" ---Select--- ", "0", true));
            ddlYear.DataSource = new PASSIS.LIB.ClassGradeLIB().getClassGrade(2, (long)schlTypeId);
            ddlYear.DataBind();

            ddlGrade.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0", true));

            var session = from c in context.AcademicSessions
                          where c.SchoolId == logonUser.SchoolId && c.IsCurrent == true
                          orderby c.IsCurrent descending
                          select c.AcademicSessionName;

            ddlSession.DataSource = session;
            ddlSession.DataTextField = "SessionName";
            ddlSession.DataValueField = "ID";
            ddlSession.DataBind();
            ddlSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0", true));

            var term = from c in context.AcademicTerm1s
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
    protected void gvPayment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPayment.PageIndex = e.NewPageIndex;
        LoadPayment();
    }

    public void LoadPayment()
    {
        var paymentList = from s in context.PaymentTemporaries
                          select new
                          {
                              s.Id,
                              s.InvoiceCode,
                              s.ReferenceCode,
                              s.Amount,
                              s.Date,
                              s.DepositorAccountName,
                              s.BankName,
                              s.ApprovalStatusId,
                              s.ClassId,
                              s.GradeId,
                              s.SessionId,
                              s.TermId,
                              s.ApprovalStatus.Status
                          };
        long classId = Convert.ToInt64(ddlYear.SelectedValue.ToString());
        if (classId > 0)
        {
            paymentList = paymentList.Where(x => x.ClassId == Convert.ToInt64(ddlYear.SelectedValue));
        }
        if (Convert.ToInt64(ddlGrade.SelectedValue) > 0)
        {
            paymentList = paymentList.Where(x => x.GradeId == Convert.ToInt64(ddlGrade.SelectedValue));
        }
        if (Convert.ToInt64(ddlSession.SelectedValue) > 0)
        {
            paymentList = paymentList.Where(x => x.SessionId == Convert.ToInt64(ddlSession.SelectedValue));
        }
        if (Convert.ToInt64(ddlTerm.SelectedValue) > 0)
        {
            paymentList = paymentList.Where(x => x.TermId == Convert.ToInt64(ddlTerm.SelectedValue));
        }
        gvPayment.DataSource = paymentList;
        gvPayment.DataBind();
    }
    protected void gvPayment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        long rowItemId;
        if (e.CommandName == "approve")
        {
            long.TryParse(e.CommandArgument.ToString(), out rowItemId);
            foreach (GridViewRow r in gvPayment.Rows)
            {
                if (r.RowType == DataControlRowType.DataRow)
                {
                    Label lblId = (Label)gvPayment.Rows[r.RowIndex].FindControl("lblId");
                    LinkButton approve = (LinkButton)gvPayment.Rows[r.RowIndex].FindControl("lnkApprove");
                    long ID = Convert.ToInt64(lblId.Text);

                    if (ID == rowItemId)
                    {
                        approve.Enabled = false;
                    }
                }
            }
            Int64 Id = Convert.ToInt64(e.CommandArgument);
            PaymentTemporary payment = context.PaymentTemporaries.FirstOrDefault(s => s.Id == Id);
            PaymentPermanent permPayment = context.PaymentPermanents.FirstOrDefault(x => x.InvoiceCode == payment.InvoiceCode);
            payment.ApprovalStatusId = 2;
            payment.ApprovedById = logonUser.Id;
            if (permPayment.AmountPaid == null) { permPayment.AmountPaid = 0; }
            if (permPayment.Discount == null) { permPayment.Discount = 0; }
            decimal? amountPaid = permPayment.AmountPaid + payment.Amount;
            decimal? totalAmount = amountPaid + permPayment.Discount;
            decimal? balance = permPayment.Balance - payment.Amount;
            if (totalAmount > permPayment.AmountGenerated)
            {
                lblErrorMsg.Text = "The total amount can't be greater than invoice generated amount";
                lblErrorMsg.Visible = true;
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }
            permPayment.Balance = balance;
            permPayment.AmountPaid = amountPaid;
            permPayment.Total = totalAmount;
            if (totalAmount == permPayment.AmountGenerated) { permPayment.IsPaymentCompleted = true; }
            context.SubmitChanges();
            lblErrorMsg.Text = "Payment approved successfully";
            lblErrorMsg.Visible = true;
            lblErrorMsg.ForeColor = System.Drawing.Color.Green;

            LoadPayment();

            PASSIS.LIB.User studentUser = context.Users.FirstOrDefault(x => x.Id == Convert.ToInt64(payment.StudentId));
            ReceiptPrinting(studentUser, payment.InvoiceCode, payment.Id, Convert.ToDecimal(permPayment.Total));

            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }
        if (e.CommandName == "decline")
        {
            Int64 Id = Convert.ToInt64(e.CommandArgument);
            PaymentTemporary payment = context.PaymentTemporaries.FirstOrDefault(s => s.Id == Id);
            payment.ApprovalStatusId = 3;
            context.SubmitChanges();
            lblErrorMsg.Text = "Payment declined successfully";
            lblErrorMsg.Visible = true;
            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
            LoadPayment();
        }
    }
    protected void gvPayment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton approve = e.Row.FindControl("lnkApprove") as LinkButton;
            LinkButton decline = e.Row.FindControl("lnkDecline") as LinkButton;
            Label status = e.Row.FindControl("lblStatus") as Label;
            if (status.Text == "Pending")
            {
                approve.Visible = true;
                decline.Visible = true;
            }
            else
            {
                approve.Visible = false;
                decline.Visible = false;
            }
        }
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        //populate the grade list,
        Int64 selectedYearId = 0L;
        selectedYearId = Convert.ToInt64(ddlYear.SelectedValue);
        ddlGrade.Items.Clear();
        var availableGrades = new ClassGradeDAL().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, selectedYearId);
        ddlGrade.DataSource = availableGrades;
        ddlGrade.DataBind();
        ddlGrade.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0", true));
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblList.Visible = true;
        LoadPayment();
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
        Paragraph p = new Paragraph("Cash Payment Receipt", darkerRedFnt);
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
        PaymentTemporary paymentTemporary = context.PaymentTemporaries.FirstOrDefault(x => x.Id == paymentId);

        PdfPTable receiptTable = new PdfPTable(12);

        PdfPCell invoiceHeader = new PdfPCell(new Phrase("Payment Code", darkerGrnFnt9)); //feeTypeHeader.Padding = 0f;
        invoiceHeader.Colspan = 3; //feeTypeHeader.HorizontalAlignment = Element.ALIGN_LEFT; feeTypeHeader.VerticalAlignment = Element.ALIGN_BOTTOM;

        PdfPCell amountGenHeader = new PdfPCell(new Phrase("Amount Generated(N)", darkerGrnFnt9)); //feeTypeHeader.Padding = 0f;
        amountGenHeader.Colspan = 3;

        PdfPCell amountHeader = new PdfPCell(new Phrase("Amount Paid(N)", darkerGrnFnt9)); //amountHeader.Padding = 0f;
        amountHeader.Colspan = 3; //amountHeader.HorizontalAlignment = Element.ALIGN_LEFT; amountHeader.VerticalAlignment = Element.ALIGN_BOTTOM;

        PdfPCell balanceHeader = new PdfPCell(new Phrase("Balance(N)", darkerGrnFnt9)); //feeTypeHeader.Padding = 0f;
        balanceHeader.Colspan = 3;

        receiptTable.AddCell(invoiceHeader);
        receiptTable.AddCell(amountGenHeader);
        receiptTable.AddCell(amountHeader);
        receiptTable.AddCell(balanceHeader);

        //foreach (PaymentInvoiceList list in invoiceList)
        //{
        PdfPCell invoice = new PdfPCell(new Phrase(paymentTemporary.InvoiceCode, resultTitleRedFnt10)); //feeType.Padding = 0f;
        invoice.Colspan = 3;

        PdfPCell amountGen = new PdfPCell(new Phrase(paymentTemporary.PaymentPermanent.AmountGenerated.ToString(), resultTitleRedFnt10)); //feeType.Padding = 0f;
        amountGen.Colspan = 3;

        PdfPCell amount = new PdfPCell(new Phrase(paymentTemporary.Amount.ToString(), resultTitleRedFnt10)); //amount.Padding = 0f;
        amount.Colspan = 3;

        PdfPCell balance = new PdfPCell(new Phrase(paymentTemporary.PaymentPermanent.Balance.ToString(), resultTitleRedFnt10)); //amount.Padding = 0f;
        balance.Colspan = 3;

        receiptTable.AddCell(invoice);
        receiptTable.AddCell(amountGen);
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