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
public partial class CashPayment : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext db = new PASSISLIBDataContext();

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
            ddlWard.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Student--", "0"));
            ddlClass.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Class--", "0"));

            //code to fetch the payment categories
            var category = from s in db.PaymentCategories select s;
            ddlfeeType.DataSource = category;
            ddlfeeType.DataTextField = "CategoryName";
            ddlfeeType.DataValueField = "Id";
            ddlfeeType.DataBind();
            ddlfeeType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0", true));

            //code to fetch the Academic Sessions
            var session = (from c in db.AcademicSessions
                           where c.SchoolId == logonUser.SchoolId && c.IsCurrent == true
                           orderby c.IsCurrent descending
                           select c.AcademicSessionName).Distinct();

            ddlsession.DataSource = session;
            ddlsession.DataTextField = "SessionName";
            ddlsession.DataValueField = "ID";
            ddlsession.DataBind();
            ddlsession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0", true));

            //code to fetch the Academic Terms
            var term = from c in db.AcademicTerm1s
                       select c;
            ddlterm.DataSource = term;
            ddlterm.DataTextField = "AcademicTermName";
            ddlterm.DataValueField = "Id";
            ddlterm.DataBind();
            ddlterm.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0", true));

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

    protected void ddlfeeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int64 selectedFeeTypeId = 0L;
        Int64 selectedSessionId = 0L;
        Int64 selectedTermId = 0L;

        selectedFeeTypeId = Convert.ToInt64(ddlfeeType.SelectedValue);
        selectedSessionId = Convert.ToInt64(ddlsession.SelectedValue);
        selectedTermId = Convert.ToInt64(ddlterm.SelectedValue);

        ddlCashCategoryType.Items.Clear();
        //ddlAmount.Items.Clear();


        var getCashType = from s in db.PaymentFeeTypes
                          where s.SchoolId == logonUser.SchoolId && s.CampusId == logonUser.SchoolCampusId && Convert.ToInt64(s.CategoryId) == (Convert.ToInt64(ddlfeeType.SelectedValue))
                          select s;


        ddlCashCategoryType.DataSource = getCashType.ToList();
        ddlCashCategoryType.DataTextField = "FeeName";
        ddlCashCategoryType.DataValueField = "Id";
        ddlCashCategoryType.DataBind();
        ddlCashCategoryType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
    }



    protected void ddlCashCategoryType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int64 selectedFeeTypeId = 0L;
        Int64 selectedSessionId = 0L;
        Int64 selectedTermId = 0L;
        string selectedCashCategory;

        selectedFeeTypeId = Convert.ToInt64(ddlfeeType.SelectedValue);
        selectedSessionId = Convert.ToInt64(ddlsession.SelectedValue);
        selectedTermId = Convert.ToInt64(ddlterm.SelectedValue);
        selectedCashCategory = ddlCashCategoryType.SelectedItem.ToString();

        ddlAmount.Items.Clear();

        var getCashType = from s in db.PaymentFees
                          where s.SchoolId == logonUser.SchoolId && s.CampusId == logonUser.SchoolCampusId && Convert.ToInt64(s.TermId) == (Convert.ToInt64(ddlterm.SelectedValue))
                          && Convert.ToInt64(s.SessionId) == (Convert.ToInt64(ddlsession.SelectedValue))
                          && Convert.ToInt64(s.ClassId) == (Convert.ToInt64(ddlYear.SelectedValue))
                          && (Convert.ToInt64(s.FeeTypeId) == Convert.ToInt64(ddlCashCategoryType.SelectedValue))
                          select s;

        ddlAmount.DataSource = getCashType.ToList();
        ddlAmount.DataTextField = "Amount";
        ddlAmount.DataValueField = "Id";
        ddlAmount.DataBind();
        //  ddlAmount.Items.Insert(0, new ListItem("--Select The Amount--", "0"));



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
        ddlWard.Items.Clear();
        BindGrid(Convert.ToInt64(ddlYear.SelectedValue), Convert.ToInt64(ddlClass.SelectedValue));

    }

    protected void BindGrid(long yearId, long gradeId)
    {
        long campusId, schoolId;
        campusId = logonUser.SchoolCampusId;
        schoolId = (long)logonUser.SchoolId;
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

        IList<PASSIS.LIB.GradeStudent> classList = new ClassGradeLIB().RetrieveGradeStudents(schoolId, campusId, yearId, gradeId, curSessionId);
        ddlWard.Items.Add(new System.Web.UI.WebControls.ListItem("--Select Student--", "0"));
        foreach (PASSIS.LIB.GradeStudent studentList in classList)
        {
            PASSIS.LIB.User stdList = db.Users.FirstOrDefault(x => x.Id == studentList.StudentId);
            ddlWard.Items.Add(new System.Web.UI.WebControls.ListItem(stdList.StudentFullName, stdList.Id.ToString()));
        }
    }


    public class getAll
    {
        public int Id { get; set; }
        public string InvoiceCode { get; set; }
        public string Fee_Name { get; set; }
        public Decimal Fee_Amount { get; set; }

    }

    public static IList<getAll> listCompare = new List<getAll>();

    protected void gvdAddCashPayments_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvdAddCashPayments.PageIndex = e.NewPageIndex;

    }


    protected void btnAdd_OnClick(object sender, EventArgs e)
    {

        lblFee.Visible = true;
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
            if (ddlWard.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Student Name is required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;
            }

            if (ddlsession.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Session is required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;
            }

            if (ddlterm.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Select the Term";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;

            }

            if (ddlfeeType.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Select the Fee Category";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;

            }


            if (ddlCashCategoryType.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Select the Fee Type";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;

            }

            if (ddlAmount.SelectedItem == null)
            {
                lblErrorMsg.Text = "Amount is Required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;
            }


            string Fee_Name = ddlCashCategoryType.SelectedItem.ToString();
            string Fee_Amount = ddlAmount.SelectedItem.Text;
            int Fee_Id = Convert.ToInt32(ddlAmount.SelectedValue);
            string invoiceCode = GetInvoiceNumber().ToString();
            int Session_Id = Convert.ToInt32(ddlsession.SelectedValue);
            int Term_Id = Convert.ToInt32(ddlterm.SelectedValue);


            getAll get = new getAll();
            get.Fee_Amount = Convert.ToDecimal(Fee_Amount);
            get.Fee_Name = Fee_Name;
            get.Id = Fee_Id;
            get.InvoiceCode = invoiceCode;

            bool exists = listCompare.Any(x => x.Fee_Name == Fee_Name);

            if (exists)
            {

                lblErrorMsg.Text = "Fee Type already selected!";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                //trErrorMsg.Visible = true;

            }
            else
            {
                listCompare.Add(get);
                lblErrorMsg.Visible = false;
                //trErrorMsg.Visible = false;
            }




            gvdAddCashPayments.DataSource = listCompare.ToList();
            gvdAddCashPayments.DataBind();


            if (gvdAddCashPayments.Rows.Count > 0)
            {
                btnSave.Visible = true;
                btnClear.Visible = true;
                lblAmtToPay.Visible = true;
                txtAmountToPay.Visible = true;
                lblAmount.Visible = true;


            }
            else { btnSave.Visible = false; }

            decimal totalAmount = 0;
            foreach (GridViewRow row in gvdAddCashPayments.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    //TextBox txt = (TextBox)GridViewFRM.row.Cells[4].FindControl("Fee_Amount");
                    //string Ques = txt.Text;

                    Label txtAmount = (Label)row.Cells[4].FindControl("Fee_Amount");
                    Decimal Amount = Convert.ToDecimal(txtAmount.Text);

                    //Decimal Amount = Convert.ToDecimal(row.Cells[1].Text.ToString().Trim());
                    totalAmount += Amount;

                }
            }

            lblAmountGen.Text = totalAmount.ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public long GetInvoiceNumber()
    {
        long NewInvoiceRef = 200000;
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        var checkIsTableEmpty = from payment in context.PaymentPermanents
                                where payment.SchoolId == logonUser.SchoolId
                                select payment;
        if (checkIsTableEmpty.Count() > 0)
        {
            var maxValue = context.PaymentPermanents.Where(x => x.SchoolId == logonUser.SchoolId).Max(x => x.InvoiceCode);
            NewInvoiceRef = Convert.ToInt64(maxValue) + 1;
        }
        else
        {
            long newInvoice;
            newInvoice = NewInvoiceRef;
        }

        return NewInvoiceRef;
    }


    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        btnClear.Visible = false;
        btnSave.Visible = false;
        listCompare.Clear();
        gvdAddCashPayments.DataSource = null;
        Response.Redirect("CashPayment.aspx");



    }



    protected void btnSave_OnClick(object sender, EventArgs e)
    {

        try
        {

            if (txtAmountToPay.Text == " ")
            {
                lblErrorMsg.Text = "Amount to Pay is Required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;

            }

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
            if (ddlWard.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Student Name is required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;
            }

            if (ddlsession.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Session is required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;
            }

            if (ddlterm.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Select the Term";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;

            }

            if (ddlCashCategoryType.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Select the Fee Category";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;

            }


            if (ddlfeeType.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Select the Fee Type";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;

            }

            if (ddlAmount.SelectedItem == null)
            {
                lblErrorMsg.Text = "Fee Selected has no Amount, Amount is Required!";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //trErrorMsg.Visible = true;
                lblErrorMsg.Visible = true;
                return;

            }

            if (Convert.ToDecimal(lblAmountGen.Text.ToString()) < Convert.ToDecimal(txtAmountToPay.Text.ToString()))
            {

                lblErrorMsg.Text = "Amount To Pay Must not be greater than Total Amount generated";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                //trErrorMsg.Visible = true;

            }

            else
            {
                Decimal balanceToPay = Convert.ToDecimal(lblAmountGen.Text.ToString()) - Convert.ToDecimal(txtAmountToPay.Text.ToString());
                lblErrorMsg.Visible = false;
                //trErrorMsg.Visible = false;


                PASSISLIBDataContext context = new PASSISLIBDataContext();

                PASSIS.LIB.GradeStudent grdStudent = context.GradeStudents.FirstOrDefault(x => x.StudentId == Convert.ToInt64(ddlWard.SelectedValue));
                if (grdStudent != null)
                {
                    long stdGrade = grdStudent.GradeId;
                }

                //var getStudentParentId = new PASSIS.LIB.ParentStudentMapLIB().RetrieveStudentParent(Convert.ToInt64(ddlWard.SelectedValue));

                //code to fetch Student's Parent Id
                PASSIS.LIB.ParentStudentMap getStudentParentId = context.ParentStudentMaps.FirstOrDefault(x => x.StudentId == Convert.ToInt64(ddlWard.SelectedValue));

                // logonUser.Id;
                //Response.Write(ddlWard.SelectedValue);
                string invoiceCode = GetInvoiceNumber().ToString();
                long studentId = Convert.ToInt64(ddlWard.SelectedValue);
                long parentId = (long)getStudentParentId.ParentUserId;
                long classId = Convert.ToInt64(ddlYear.SelectedValue);
                long gradeId = grdStudent.GradeId;
                long? schoolId = logonUser.SchoolId;
                long campusId = logonUser.SchoolCampusId;
                long approvedById = logonUser.Id;
                long sessionId = Convert.ToInt64(ddlsession.SelectedValue);
                long termId = Convert.ToInt64(ddlterm.SelectedValue);
                string PaymentType = "Cash";
                int ApprovalStatusId = 2;

                PaymentPermanent paymentPermanent = new PaymentPermanent();
                paymentPermanent.InvoiceCode = invoiceCode;
                paymentPermanent.StudentId = studentId;
                paymentPermanent.ParentId = parentId;
                paymentPermanent.ClassId = classId;
                paymentPermanent.GradeId = gradeId;
                paymentPermanent.SchoolId = schoolId;
                paymentPermanent.CampusId = campusId;
                paymentPermanent.SessionId = sessionId;
                paymentPermanent.TermId = termId;
                paymentPermanent.AmountGenerated = Convert.ToDecimal(lblAmountGen.Text);
                paymentPermanent.AmountPaid = Convert.ToDecimal(txtAmountToPay.Text);
                paymentPermanent.Balance = Convert.ToDecimal(balanceToPay);
                paymentPermanent.Total = Convert.ToDecimal(lblAmountGen.Text);
                //paymentPermanent.ApprovedById = approvedById;

                if (Convert.ToDecimal(txtAmountToPay.Text) == Convert.ToDecimal(lblAmountGen.Text))
                {
                    paymentPermanent.IsPaymentCompleted = true;
                }
                paymentPermanent.DateCreated = DateTime.Now;
                context.PaymentPermanents.InsertOnSubmit(paymentPermanent);
                context.SubmitChanges();

                //Code to save  record into the PaymentTemporary Table
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
                paymentTemporary.Amount = Convert.ToDecimal(txtAmountToPay.Text);
                paymentTemporary.Date = DateTime.Now;
                paymentTemporary.PermanentPaymentId = paymentPermanent.Id;
                paymentTemporary.ApprovalStatusId = ApprovalStatusId;
                paymentTemporary.ApprovedById = logonUser.Id;
                context.PaymentTemporaries.InsertOnSubmit(paymentTemporary);
                context.SubmitChanges();


                //Code to save each record into the InvoiceList Table

                foreach (GridViewRow row in gvdAddCashPayments.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {


                        // Decimal amount = Convert.ToDecimal(row.Cells[4].Text.ToString().Trim());
                        // int feeId = Convert.ToInt32(row.Cells[1].Text.ToString().Trim());

                        Label txtamount = (Label)row.Cells[4].FindControl("Fee_Amount");
                        Decimal amount = Convert.ToDecimal(txtamount.Text);

                        Label txtfeeId = (Label)row.Cells[1].FindControl("Fee_Id");
                        int feeId = Convert.ToInt32(txtfeeId.Text);


                        int mandatory = 1;

                        PaymentInvoiceList paymentInvoiceList = new PaymentInvoiceList();
                        paymentInvoiceList.InvoiceCode = invoiceCode;
                        paymentInvoiceList.StudentId = studentId;
                        paymentInvoiceList.ParentId = parentId;
                        paymentInvoiceList.ClassId = classId;
                        paymentInvoiceList.GradeId = gradeId;
                        paymentInvoiceList.SchoolId = schoolId;
                        paymentInvoiceList.CampusId = campusId;
                        paymentInvoiceList.SessionId = sessionId;
                        paymentInvoiceList.TermId = termId;
                        paymentInvoiceList.Amount = amount;
                        paymentInvoiceList.FeeId = feeId;
                        paymentInvoiceList.Mandatory = mandatory;
                        paymentInvoiceList.ApprovedById = logonUser.Id;
                        paymentInvoiceList.PermanentPaymentId = paymentPermanent.Id;

                        context.PaymentInvoiceLists.InsertOnSubmit(paymentInvoiceList);
                        context.SubmitChanges();


                        lblErrorMsg.Text = "Payment Made successfully (Clear entries to make new Payments)";
                        lblMessage.Text = " ";
                        lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                        lblErrorMsg.Visible = true;
                        //trErrorMsg.Visible = true;
                        listCompare.Clear();
                        gvdAddCashPayments.DataSource = null;

                    }
                }
                PASSIS.LIB.User studentUser = context.Users.FirstOrDefault(x => x.Id == Convert.ToInt64(ddlWard.SelectedValue));
                ReceiptPrinting(studentUser, invoiceCode, paymentTemporary.Id, Convert.ToDecimal(paymentPermanent.Total));

            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "An Error occurred, Make Sure All Fields are Selected or Filled!";
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }


    protected void gvdAddCashPayments_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        listCompare.RemoveAt(e.RowIndex);
        gvdAddCashPayments.DataSource = listCompare;

        gvdAddCashPayments.DataBind();

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
        Paragraph SessionDetails = new Paragraph(string.Format("{0} {1} {2} Session", ddlterm.SelectedItem.Text, ",", ddlsession.SelectedItem.Text), darkerRedFnt);
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
        PaymentTemporary paymentTemporary = db.PaymentTemporaries.FirstOrDefault(x => x.Id == paymentId);

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

//saving the grid to database working OKAY
//for (int i = 0; i < GridView1.Rows.Count; i++)
//{
//    PASSIS.LIB.AddCashCategoryPayment tryy = new AddCashCategoryPayment();
//    tryy.CashPaymentCategory = GridView1.Rows[i].Cells[1].Text;

//    db.AddCashCategoryPayments.InsertOnSubmit(tryy);
//    db.SubmitChanges();

//}//if (Convert.ToDecimal(txtAmountToPay.Text.ToString()) > Convert.ToDecimal(lblAmountGen.Text.ToString()))
//{
//    lblErrorMsg.Text = "Amount to pay must not be greater than the amount generated!";
//    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
//    lblErrorMsg.Visible = true;
//    trErrorMsg.Visible = true;

//}
//string alph = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz";
//if (txtAmountToPay.Text.Contains(alph))
//{
//    lblErrorMsg.Text = "Input Numbers Only";
//    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
//    lblErrorMsg.Visible = true;
//    trErrorMsg.Visible = true;
//}
