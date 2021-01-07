using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO;
using PASSIS.LIB;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;

public partial class PaymentInvoice : PASSIS.LIB.Utility.BasePage
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
            BindDropDown();
            btnGenerateInvoice.Visible = false;
            btnReset.Visible = false;
            btnTotal.Visible = false;
            lblUpdatedTotalSelectedAmount.Visible = false;
            lbltotalAmtAfterDiscount.Visible = false;
            lbltotalDiscountCalculated.Visible = false;
            lblTotalDiscount.Visible = false;

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
    protected void ddlWard_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlClass.Items.Clear();
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);
        PASSIS.LIB.GradeStudent stdGrade = context.GradeStudents.FirstOrDefault(x => x.StudentId == Convert.ToInt64(ddlWard.SelectedValue) && x.AcademicSessionId == curSessionId);
        if (stdGrade != null)
        {
            var stdClass = from s in context.Class_Grades where s.Id == stdGrade.ClassId select s;
            ddlClass.DataSource = stdClass;
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0", true));
        }
    }

    protected void BindDropDown()
    {
        ddlWard.DataSource = new UsersLIB().RetrieveParentsChildren(logonUser.Id);
        ddlWard.DataBind();

        //var schlTypeId = context.Schools.FirstOrDefault(x => x.Id == (long)logonUser.SchoolId).SchoolTypeId;
        //ddlClass.DataSource = new PASSIS.LIB.ClassGradeLIB().getClassGrade(2, (long)schlTypeId);
        //ddlClass.DataBind();

        clsMyDB mdb = new clsMyDB();
        mdb.connct();

        string querySession = "SELECT DISTINCT AcademicSessionId FROM AcademicSession WHERE SchoolId=" + logonUser.SchoolId;
        SqlDataReader readerSession = mdb.fetch(querySession);
        while (readerSession.Read())
        {
            ddlSession.DataSource = from s in context.AcademicSessionNames
                                    where s.ID == Convert.ToInt64(readerSession["AcademicSessionId"].ToString())
                                    select s;
            ddlSession.DataBind();
        }

        readerSession.Close();
        mdb.closeConnct();

        ddlTerm.DataSource = new AcademicTermLIB().RetrieveAcademicTerm();
        ddlTerm.DataBind();
    }
    protected void btnGenerateInvoice_Click(object sender, EventArgs e)
    {
        //try
        //{
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

        PASSIS.LIB.GradeStudent grdStudent = context.GradeStudents.FirstOrDefault(x => x.StudentId == Convert.ToInt64(ddlWard.SelectedValue) && x.AcademicSessionId == curSessionId);
        if (grdStudent != null)
        {
            long stdGrade = grdStudent.GradeId;
        }
        else
        {
            lblErrorMsg.Text = "Your Child has not been assigned to any class";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
        if (lblTotalInvoicePayable.Text == "")
        {
            lblErrorMsg.Text = "Total amount is empty";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
        string invoiceCode = GetInvoiceNumber().ToString();
        long studentId = Convert.ToInt64(ddlWard.SelectedValue);
        long parentId = logonUser.Id;
        long classId = Convert.ToInt64(ddlClass.SelectedValue);
        long gradeId = grdStudent.GradeId;
        long? schoolId = logonUser.SchoolId;
        long campusId = logonUser.SchoolCampusId;
        long sessionId = Convert.ToInt64(ddlSession.SelectedValue);
        long termId = Convert.ToInt64(ddlTerm.SelectedValue);
        var NumofChildren = new UsersLIB().RetrieveParentsChildren(logonUser.Id);



        //code to fetch each child a parent has for calculating mandatory fees for each child
        var eachChild = new UsersLIB().RetrieveParentsChildrenUserIds(logonUser.Id);
        int countChild = 0;
        foreach (var eachChilds in eachChild)
        {
            countChild++;
            PASSIS.LIB.User getChild = context.Users.FirstOrDefault(x => x.Id == eachChilds);

            PASSIS.LIB.GradeStudent getChildGrade = context.GradeStudents.FirstOrDefault(x => x.StudentId == getChild.Id && x.AcademicSessionId == curSessionId);


            long classid = getChildGrade.ClassId;
            long? studschoolId = getChildGrade.SchoolId;
            long? studcampusId = getChildGrade.SchoolCampusId;


            var getAllChildMandatoryFee = (from x in context.PaymentFees
                                          where x.ClassId == classid && x.SchoolId == studschoolId
                                          && x.CampusId == studcampusId && x.TermId == termId
                                          && x.SessionId == sessionId && x.Mandatory == 1
                                          select x).Sum(x=>x.Amount);
            //decimal? getChildTotalFee = 0;
            //foreach (var getAllChildMandatoryFees in getAllChildMandatoryFee)
            //{

            //    getChildTotalFee = getChildTotalFee + getAllChildMandatoryFees.Amount;

            //}

            decimal? childMandatoryFee = Convert.ToDecimal(lblTotalInvoicePayable.Text);

            PaymentPermanent paymentExists = context.PaymentPermanents.FirstOrDefault(x => x.ParentId == parentId
                          //&& x.GradeId == gradeId
                          //&& x.ClassId == classId
                          && x.SchoolId == schoolId
                          && x.CampusId == campusId
                          && x.SessionId == sessionId
                          && x.TermId == termId
                          && x.IsPaymentCompleted == null);

            FinanceDiscount checkDiscExist = context.FinanceDiscounts.FirstOrDefault(x => x.ClassId == classId
                            && x.SchoolId == schoolId
                            && x.CampusId == campusId
                            && x.SessionId == sessionId
                            && x.TermId == termId);

            // decimal discountValue = 0.00M;
            if (paymentExists == null)
            {

                if (checkDiscExist != null && childMandatoryFee < getAllChildMandatoryFee && countChild > 1) // code to check and enforce the child that has the highest mandatory fee
                {

                    lblErrorMsg.Text = "You have " + NumofChildren.Count + " Child(ren), and you cant generate Invoice for a child with a low Mandatory Fees First!";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                    return;

                }
            }
        }

        //code to check if an invoice has been generated for a student for a term, class and session
        PaymentPermanent getPayment = context.PaymentPermanents.FirstOrDefault(x => x.StudentId == studentId
        && x.ParentId == parentId
        && x.GradeId == gradeId
        && x.ClassId == classId
        && x.SchoolId == schoolId
        && x.CampusId == campusId
        && x.SessionId == sessionId
        && x.TermId == termId);

        if (getPayment != null && getPayment.IsPaymentCompleted != null)
        {

            lblErrorMsg.Text = "An Invoice has been generated for this Child and Payments has been made completely for this Term and session";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;

        }

        if (getPayment != null && getPayment.AmountPaid == null)

        {

            lblErrorMsg.Text = "An Invoice Exists For this Student, and payment has not been made. You may Kindly delete an existing Invoice before generating a new Invoice!";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;

        }

        else if (getPayment != null && getPayment.AmountPaid != null && getPayment.IsPaymentCompleted == null)
        {

            lblErrorMsg.Text = "An Invoice Exists For this Student, and Payment has been made but not completed. Kindly Check the Invoice to balance Payment before generating a new Invoice!";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;

        }

        //Code to Count the total Payments Made by a Parent for a session and term
        var getPaymentsCount = from x in context.PaymentPermanents
                               where x.ParentId == parentId
                               && x.SchoolId == schoolId
                               && x.CampusId == campusId
                               && x.SessionId == sessionId
                               && x.TermId == termId
                               && x.IsPaymentCompleted == null
                               select x;

        long countPaymentPermanents = getPaymentsCount.ToList().Count + 1; //This code adds one to the number of payments in the PaymentPermanents Table (it hepls in determining the child the discount will be applied for)        

        //This code saves the payment in the permanent table
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

        //saves the discont of the original amount to be paid if the number of child is greater than or equals 1
        if (NumofChildren.Count >= 1 && NumofChildren.Count == countPaymentPermanents)
        {

            paymentPermanent.OriginalAmountGenerated = Convert.ToDecimal(lblTotalInvoicePayable.Text);
            paymentPermanent.AmountGenerated = Convert.ToDecimal(lblTotalAmountAfterDiscount.Text);
            paymentPermanent.Balance = Convert.ToDecimal(lblTotalAmountAfterDiscount.Text);
            paymentPermanent.Discount = Convert.ToDecimal(lbltotalDiscountCalculated.Text);

            // checks the the permanent payments table if discount has been applied to a child. if yes, 
            //sets the Isdiscount to false else true
            foreach (var getPaymentsCounts in getPaymentsCount)
            {

                if (getPaymentsCounts.IsDiscount == true)
                {
                    paymentPermanent.IsDiscount = false;
                }

                else
                {
                    paymentPermanent.IsDiscount = true;
                }

            }

        }

        else
        {
            paymentPermanent.OriginalAmountGenerated = Convert.ToDecimal(lblTotalInvoicePayable.Text);
            paymentPermanent.AmountGenerated = Convert.ToDecimal(lblTotalInvoicePayable.Text);
            paymentPermanent.Balance = Convert.ToDecimal(lblTotalInvoicePayable.Text);
            paymentPermanent.Discount = Convert.ToDecimal(lbltotalDiscountCalculated.Text);
            paymentPermanent.IsDiscount = false;
        }
        paymentPermanent.DateCreated = DateTime.Now;
        context.PaymentPermanents.InsertOnSubmit(paymentPermanent);
        context.SubmitChanges();


        foreach (GridViewRow row in gdvList.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                //CheckBox chkPayInvoice = (CheckBox)row.FindControl("CheckBox1");

                bool isChecked = row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                Label lblMandatory = row.FindControl("lblMandatory") as Label;
                Label lblFeeId = row.FindControl("lblFeeId") as Label;

                Decimal amount = Convert.ToDecimal(row.Cells[4].Text.ToString().Trim());
                long feeId = Convert.ToInt64(lblFeeId.Text.ToString().Trim());
                int mandatory = Convert.ToInt16(lblMandatory.Text);


                if (isChecked)
                {
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
                    paymentInvoiceList.PermanentPaymentId = paymentPermanent.Id;

                    context.PaymentInvoiceLists.InsertOnSubmit(paymentInvoiceList);
                    context.SubmitChanges();
                }
            }
        }

        PASSIS.LIB.User studentUser = context.Users.FirstOrDefault(x => x.Id == Convert.ToInt64(ddlWard.SelectedValue));

        if (NumofChildren.Count >= 1)  //Prints the Invoice discount of the original amount to be paid if the number of child is greater than or equals to 1
        {
            InvoicePrinting(studentUser, invoiceCode, Convert.ToDecimal(lblTotalAmountAfterDiscount.Text));
        }
        else //Prints the Invoice original amount to be paid if the number of child is less than 1
        {
            InvoicePrinting(studentUser, invoiceCode, Convert.ToDecimal(lblTotalInvoicePayable.Text));
        }
        //}
        //catch (Exception ex)
        //{
        //    PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Source, ex.StackTrace);
        //    lblErrorMsg.Text = "Error occurred";
        //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
        //    lblErrorMsg.Visible = true;
        //}
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
                        row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked = chk.Checked;
                        CheckBox chkPayInvoice = (CheckBox)row.FindControl("CheckBox1");
                        Label regid = row.FindControl("lblMandatory") as Label;
                        if (regid != null)
                        {
                            //mandatory
                            if (Convert.ToInt32(regid.Text) == 1)
                            {
                                chkPayInvoice.Checked = true;
                                chkPayInvoice.Enabled = false;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Source, ex.StackTrace);
            lblErrorMsg.Text = "Error occurred";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
        }
    }
    protected void gdvList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        CheckBox chkPayInvoice = (CheckBox)e.Row.Cells[0].FindControl("CheckBox1");
        Label regid = (e.Row.FindControl("lblMandatory") as Label);
        if (regid != null)
        {
            //mandatory
            if (Convert.ToInt32(regid.Text) == 1)
            {
                chkPayInvoice.Checked = true;
                chkPayInvoice.Enabled = false;
            }
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


    protected void btnViewPaymentList_Click(object sender, EventArgs e)
    {
        lblLists.Text = "LIST OF FEES FOR " + ddlClass.SelectedItem.Text + " " + ddlTerm.SelectedItem.Text + " " + ddlSession.SelectedItem.Text;
        lblLists.Visible = true;
        try
        {
            if (ddlWard.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select the ward";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }

            if (ddlClass.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select class";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }

            if (ddlSession.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select the session";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }

            if (ddlTerm.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select the term";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }

            PASSIS.LIB.GradeStudent grdStudent = context.GradeStudents.FirstOrDefault(x => x.StudentId == Convert.ToInt64(ddlWard.SelectedValue));
            if (grdStudent != null)
            {
                long stdGrade = grdStudent.GradeId;
            }
            else
            {
                lblErrorMsg.Text = "Your Child has not been assigned to any class";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }

            long parentId = logonUser.Id;
            long studentId = Convert.ToInt64(ddlWard.SelectedValue);
            long classId = Convert.ToInt64(ddlClass.SelectedValue);
            //long gradeId = grdStudent.GradeId;
            long sessionId = Convert.ToInt64(ddlSession.SelectedValue);
            long termId = Convert.ToInt64(ddlTerm.SelectedValue);
            long schoolId = (long)logonUser.SchoolId;
            long campusId = logonUser.SchoolCampusId;


            ////code to check if an invoice has been generated for a student and payment is complete
            //PaymentPermanent checkPayment = context.PaymentPermanents.FirstOrDefault(x => x.StudentId == studentId
            //&& x.ParentId == parentId
            //&& x.ClassId == classId
            //&& x.SchoolId == schoolId
            //&& x.CampusId == campusId
            //&& x.SessionId == sessionId
            //&& x.TermId == termId
            //&& x.IsPaymentCompleted == null);//incomplete or No payment

            //if (checkPayment != null)
            //{

            //    var invoiceListGen = from x in context.PaymentFees
            //                         where
            //                         x.ClassId == classId
            //                         && x.SchoolId == schoolId
            //                         && x.CampusId == campusId
            //                         && x.SessionId == sessionId
            //                         && x.TermId == termId
            //                         select x;
            //    foreach (var invoiceListGenerate in invoiceListGen) // code to populate the fees that wasnt previously selected by Users
            //    {

            //        var paymentfeeLists = from s in context.PaymentInvoiceLists
            //                              where s.FeeId == invoiceListGenerate.Id && s.ClassId == classId && s.SessionId == sessionId && s.TermId == termId
            //                             && s.SchoolId == logonUser.SchoolId && s.CampusId == logonUser.SchoolCampusId && s.Mandatory == 0 
            //                              select s;

            //        foreach (var paymentfeeListss in paymentfeeLists)
            //        {

            //            var paymentList = from s in context.PaymentFees
            //                              where s.ClassId == classId && s.SessionId == sessionId && s.TermId == termId
            //                                  && s.SchoolId == logonUser.SchoolId && s.CampusId == logonUser.SchoolCampusId
            //                              && s.Mandatory == 0  && s.Id != paymentfeeListss.FeeId
            //                              select new
            //                              {
            //                                  s.Id,
            //                                  s.PaymentFeeType.FeeName,
            //                                  s.FeeTypeId,
            //                                  s.Mandatory,
            //                                  s.Amount
            //                              };

            //            gdvList.DataSource = paymentList;
            //            gdvList.DataBind();

            //            if (paymentList.Count() > 0)
            //            {
            //                btnGenerateInvoice.Visible = true;
            //                btnReset.Visible = true;
            //                btnTotal.Visible = true;
            //                lblUpdatedTotalSelectedAmount.Visible = true;
            //                lbltotalAmtAfterDiscount.Visible = true;
            //                ddlWard.Enabled = false;
            //                ddlClass.Enabled = false;
            //                ddlSession.Enabled = false;
            //                ddlTerm.Enabled = false;
            //                lblErrorMsg.Visible = false;
            //                lbltotalDiscountCalculated.Visible = true;
            //                lblTotalDiscount.Visible = true;
            //            }
            //        }
            //    }
            //}

            //else 
            //   {

            var paymentList = from s in context.PaymentFees
                              where s.ClassId == classId && s.SessionId == sessionId && s.TermId == termId
                                  && s.SchoolId == logonUser.SchoolId && s.CampusId == logonUser.SchoolCampusId
                              select new
                              {
                                  s.Id,
                                  s.PaymentFeeType.FeeName,
                                  s.FeeTypeId,
                                  s.Mandatory,
                                  s.Amount
                              };
            gdvList.DataSource = paymentList;
            gdvList.DataBind();

            if (paymentList.Count() > 0)
            {
                btnGenerateInvoice.Visible = true;
                btnReset.Visible = true;
                btnTotal.Visible = true;
                lblUpdatedTotalSelectedAmount.Visible = true;
                lbltotalAmtAfterDiscount.Visible = true;
                ddlWard.Enabled = false;
                ddlClass.Enabled = false;
                ddlSession.Enabled = false;
                ddlTerm.Enabled = false;
                lblErrorMsg.Visible = false;
                lbltotalDiscountCalculated.Visible = true;
                lblTotalDiscount.Visible = true;
            }

        }

        //}
        catch (Exception ex)
        {
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Source, ex.StackTrace);
            lblErrorMsg.Text = "An Error occurred";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ddlClass.Enabled = true;
        ddlSession.Enabled = true;
        ddlTerm.Enabled = true;
        ddlWard.Enabled = true;
        btnGenerateInvoice.Visible = false;
        btnTotal.Visible = false;
        lblTotalInvoicePayable.Text = "";
        lbltotalDiscountCalculated.Text = "";
        lblTotalAmountAfterDiscount.Text = "";
        lblErrorMsg.Text = "";
        btnReset.Visible = false;
        gdvList.DataSource = null;
        gdvList.DataBind();
    }
    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        lblTotalInvoicePayable.Text = "";
        lbltotalDiscountCalculated.Text = "";
        lblTotalAmountAfterDiscount.Text = "";
    }



    protected void btnTotal_Click(object sender, EventArgs e)
    {

        decimal totalAmount = 0;
        decimal totalDiscountAmount = 0;
        decimal TotDiscount = 0;

        //get the number of Child using the logonuserId
        var NumofChildren = new UsersLIB().RetrieveParentsChildren(logonUser.Id);

        long parentId = logonUser.Id;
        long wardId = Convert.ToInt64(ddlWard.SelectedValue);
        long classId = Convert.ToInt64(ddlClass.SelectedValue);
        long sessionId = Convert.ToInt64(ddlSession.SelectedValue);
        long termId = Convert.ToInt64(ddlTerm.SelectedValue);
        long schoolId = (long)logonUser.SchoolId;
        long campusId = logonUser.SchoolCampusId;

        //Count the total Payments Made by a Parent for a session and term
        var paymentExists = from x in context.PaymentPermanents
                            where x.ParentId == parentId
                            && x.SchoolId == schoolId
                            && x.CampusId == campusId
                            && x.SessionId == sessionId
                            && x.TermId == termId
                            && x.IsPaymentCompleted == null
                            select x;

        long countPaymentPermanents = paymentExists.ToList().Count + 1; //This code adds one to the number of payments in the PaymentPermanents Table (it hepls in determining the child the discount will be applied for)        

        //get the status of the child Parent (Staff/Non-Staff/Scholarship) using the (logonuser Id)
        PASSIS.LIB.User getStatus = context.Users.FirstOrDefault(x => x.Id == logonUser.Id);

        foreach (GridViewRow row in gdvList.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                bool isChecked = row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                if (isChecked)
                {

                    // Label lblFeeTypeId = row.FindControl("lblFeeTypeId") as Label;

                    Label lblFeeId = row.FindControl("lblFeeId") as Label;
                    long feeTypeId = Convert.ToInt64(lblFeeId.Text.ToString().Trim());

                    var getMaxNumChildInDiscount = (from x in context.FinanceDiscounts //gets the discount of the maximum of child
                                                    where x.FeeId == feeTypeId
                                             && x.GradeId == (Convert.ToInt64(ddlClass.SelectedValue))
                                             && x.SchoolId == (Convert.ToInt64(logonUser.SchoolId))
                                             && x.CampusId == (Convert.ToInt64(logonUser.SchoolCampusId))
                                             && x.SessionId == (Convert.ToInt64(ddlSession.SelectedValue))
                                             && x.TermId == (Convert.ToInt64(ddlTerm.SelectedValue))
                                             && x.Status == (getStatus.Status)
                                                    select x).Max(x => x.NumChild);



                    if (NumofChildren.Count >= 1 && NumofChildren.Count == countPaymentPermanents) // && NumofChildren.Count == countPaymentPermanents  Discount applies if parent has one or more child
                    {
                        //get fee Discount for Staff, N/staff and Scholarship
                        PASSIS.LIB.FinanceDiscount getDiscount = context.FinanceDiscounts.FirstOrDefault(x => x.FeeId == feeTypeId
                        && x.GradeId == (Convert.ToInt64(ddlClass.SelectedValue))
                        && x.SchoolId == (Convert.ToInt64(logonUser.SchoolId))
                        && x.CampusId == (Convert.ToInt64(logonUser.SchoolCampusId))
                        && x.SessionId == (Convert.ToInt64(ddlSession.SelectedValue))
                        && x.TermId == (Convert.ToInt64(ddlTerm.SelectedValue))
                        && x.Status == (getStatus.Status)
                        && x.NumChild == (NumofChildren.Count));

                        //get fee Discount specified for ALL status
                        PASSIS.LIB.FinanceDiscount getDiscountAllStatus = context.FinanceDiscounts.FirstOrDefault(x => x.FeeId == feeTypeId
                        && x.GradeId == (Convert.ToInt64(ddlClass.SelectedValue))
                        && x.SchoolId == (Convert.ToInt64(logonUser.SchoolId))
                        && x.CampusId == (Convert.ToInt64(logonUser.SchoolCampusId))
                        && x.SessionId == (Convert.ToInt64(ddlSession.SelectedValue))
                        && x.TermId == (Convert.ToInt64(ddlTerm.SelectedValue))
                        && x.Status == (4) //  All Status
                        && x.NumChild == (NumofChildren.Count));


                        if (getDiscount != null)
                        {

                            Decimal discountPercent;
                            Decimal discount = (Decimal)getDiscount.Discount; //to get the discount of each of the fees in the financeDiscount table
                                                                              //Response.Write(discount);

                            Decimal FeeAmount = Convert.ToDecimal(row.Cells[4].Text.ToString().Trim());//original fee amount

                            discountPercent = ((discount / 100) * FeeAmount); //calculate the discount of each fees by 100%
                            totalDiscountAmount += discountPercent; //sum of the total discount of each fees applicable ( totalDiscountAmount=52,000)
                                                                    ////Response.Write(discountPercent);//36,000 and 16,000

                            //to subtract the discount of each fees from the original fee amount (90,000-36,000=54,000 and 20,000-1600=4,000)
                            Decimal dddAmount = (FeeAmount - discountPercent);//(54,000 and 4,000)
                            TotDiscount += dddAmount; //sum of the total discount after subtration( TotDiscount=58,000)

                        }

                        else if (getDiscountAllStatus != null) //This code applies if discount is for ALL status
                        {

                            Decimal discountPercent;
                            Decimal discount = (Decimal)getDiscountAllStatus.Discount; //to get the discount of each of the fees in the financeDiscount table

                            Decimal FeeAmount = Convert.ToDecimal(row.Cells[4].Text.ToString().Trim());//original fee amount

                            discountPercent = ((discount / 100) * FeeAmount); //calculate the discount of each fees by 100%
                            totalDiscountAmount += discountPercent; //sum of the total discount of each fees applicable ( totalDiscountAmount=52,000)
                                                                    //Response.Write(discountPercent);//36,000 and 16,000

                            //to subtract the discount of each fees from the original fee amount (90,000-36,000=54,000 and 20,000-1600=4,000)
                            Decimal dddAmount = (FeeAmount - discountPercent);//(54,000 and 4,000)
                            TotDiscount += dddAmount; //sum of the total discount after subtration( TotDiscount=58,000)

                        }

                        else { }
                    }

                    ////////////////////////////////////////// Applies discount of the highest number of child in the finance discount table if the parents children is more than the number of child specified in the table

                    else if (NumofChildren.Count >= 1 && NumofChildren.Count == countPaymentPermanents && NumofChildren.Count >= getMaxNumChildInDiscount) // && NumofChildren.Count == countPaymentPermanents  Discount applies if parent has one or more child
                    {

                        //get fee Discount for Staff, N/staff and Scholarship
                        PASSIS.LIB.FinanceDiscount getDiscount = context.FinanceDiscounts.FirstOrDefault(x => x.FeeId == feeTypeId
                        && x.GradeId == (Convert.ToInt64(ddlClass.SelectedValue))
                        && x.SchoolId == (Convert.ToInt64(logonUser.SchoolId))
                        && x.CampusId == (Convert.ToInt64(logonUser.SchoolCampusId))
                        && x.SessionId == (Convert.ToInt64(ddlSession.SelectedValue))
                        && x.TermId == (Convert.ToInt64(ddlTerm.SelectedValue))
                        && x.Status == (getStatus.Status)
                        && x.NumChild == (getMaxNumChildInDiscount));

                        //get fee Discount specified for ALL status
                        PASSIS.LIB.FinanceDiscount getDiscountAllStatus = context.FinanceDiscounts.FirstOrDefault(x => x.FeeId == feeTypeId
                        && x.GradeId == (Convert.ToInt64(ddlClass.SelectedValue))
                        && x.SchoolId == (Convert.ToInt64(logonUser.SchoolId))
                        && x.CampusId == (Convert.ToInt64(logonUser.SchoolCampusId))
                        && x.SessionId == (Convert.ToInt64(ddlSession.SelectedValue))
                        && x.TermId == (Convert.ToInt64(ddlTerm.SelectedValue))
                        && x.Status == (4) //  All Status
                        && x.NumChild == (getMaxNumChildInDiscount));


                        if (getDiscount != null) //This code applies if discount is for a specific status
                        {

                            Decimal discountPercent;
                            Decimal discount = (Decimal)getDiscount.Discount; //to get the discount of each of the fees in the financeDiscount table

                            Decimal FeeAmount = Convert.ToDecimal(row.Cells[4].Text.ToString().Trim());//original fee amount

                            discountPercent = ((discount / 100) * FeeAmount); //calculate the discount of each fees by 100%
                            totalDiscountAmount += discountPercent; //sum of the total discount of each fees applicable ( totalDiscountAmount=52,000)
                                                                    //Response.Write(discountPercent);//36,000 and 16,000

                            //to subtract the discount of each fees from the original fee amount (90,000-36,000=54,000 and 20,000-1600=4,000)
                            Decimal dddAmount = (FeeAmount - discountPercent);//(54,000 and 4,000)
                            TotDiscount += dddAmount; //sum of the total discount after subtration( TotDiscount=58,000)

                        }

                        else if (getDiscountAllStatus != null) //This code applies if discount is for ALL status
                        {

                            Decimal discountPercent;
                            Decimal discount = (Decimal)getDiscountAllStatus.Discount; //to get the discount of each of the fees in the financeDiscount table

                            Decimal FeeAmount = Convert.ToDecimal(row.Cells[4].Text.ToString().Trim());//original fee amount

                            discountPercent = ((discount / 100) * FeeAmount); //calculate the discount of each fees by 100%
                            totalDiscountAmount += discountPercent; //sum of the total discount of each fees applicable ( totalDiscountAmount=52,000)
                                                                    //Response.Write(discountPercent);//36,000 and 16,000

                            //to subtract the discount of each fees from the original fee amount (90,000-36,000=54,000 and 20,000-1600=4,000)
                            Decimal dddAmount = (FeeAmount - discountPercent);//(54,000 and 4,000)
                            TotDiscount += dddAmount; //sum of the total discount after subtration( TotDiscount=58,000)

                        }

                        else { }
                    }





                    //////////////////////// Revert the discount if new payments are to be made for a new term and session, setting the counts to 0;
                    else if (NumofChildren.Count >= 1 && NumofChildren.Count > countPaymentPermanents - countPaymentPermanents) // && NumofChildren.Count == countPaymentPermanents  Discount applies if parent has one or more child
                    { }
                    else
                    {
                        //get fee Discount for Staff, N/staff and Scholarship
                        PASSIS.LIB.FinanceDiscount getDiscount = context.FinanceDiscounts.FirstOrDefault(x => x.FeeId == feeTypeId
                        && x.GradeId == (Convert.ToInt64(ddlClass.SelectedValue))
                        && x.SchoolId == (Convert.ToInt64(logonUser.SchoolId))
                        && x.CampusId == (Convert.ToInt64(logonUser.SchoolCampusId))
                        && x.SessionId == (Convert.ToInt64(ddlSession.SelectedValue))
                        && x.TermId == (Convert.ToInt64(ddlTerm.SelectedValue))
                        && x.Status == (getStatus.Status)
                        && x.NumChild == (NumofChildren.Count));

                        //get fee Discount specified for ALL status
                        PASSIS.LIB.FinanceDiscount getDiscountAllStatus = context.FinanceDiscounts.FirstOrDefault(x => x.FeeId == feeTypeId
                        && x.GradeId == (Convert.ToInt64(ddlClass.SelectedValue))
                        && x.SchoolId == (Convert.ToInt64(logonUser.SchoolId))
                        && x.CampusId == (Convert.ToInt64(logonUser.SchoolCampusId))
                        && x.SessionId == (Convert.ToInt64(ddlSession.SelectedValue))
                        && x.TermId == (Convert.ToInt64(ddlTerm.SelectedValue))
                        && x.Status == (4) //  All Status
                        && x.NumChild == (NumofChildren.Count));


                        if (getDiscount != null)
                        {

                            Decimal discountPercent;
                            Decimal discount = (Decimal)getDiscount.Discount; //to get the discount of each of the fees in the financeDiscount table

                            Decimal FeeAmount = Convert.ToDecimal(row.Cells[4].Text.ToString().Trim());//original fee amount

                            discountPercent = ((discount / 100) * FeeAmount); //calculate the discount of each fees by 100%
                            totalDiscountAmount += discountPercent; //sum of the total discount of each fees applicable ( totalDiscountAmount=52,000)
                                                                    //Response.Write(discountPercent);//36,000 and 16,000

                            //to subtract the discount of each fees from the original fee amount (90,000-36,000=54,000 and 20,000-1600=4,000)
                            Decimal dddAmount = (FeeAmount - discountPercent);//(54,000 and 4,000)
                            TotDiscount += dddAmount; //sum of the total discount after subtration( TotDiscount=58,000)

                        }

                        else if (getDiscountAllStatus != null) //This code applies if discount is for ALL status
                        {

                            Decimal discountPercent;
                            Decimal discount = (Decimal)getDiscountAllStatus.Discount; //to get the discount of each of the fees in the financeDiscount table

                            Decimal FeeAmount = Convert.ToDecimal(row.Cells[4].Text.ToString().Trim());//original fee amount

                            discountPercent = ((discount / 100) * FeeAmount); //calculate the discount of each fees by 100%
                            totalDiscountAmount += discountPercent; //sum of the total discount of each fees applicable ( totalDiscountAmount=52,000)
                                                                    //Response.Write(discountPercent);//36,000 and 16,000

                            //to subtract the discount of each fees from the original fee amount (90,000-36,000=54,000 and 20,000-1600=4,000)
                            Decimal dddAmount = (FeeAmount - discountPercent);//(54,000 and 4,000)
                            TotDiscount += dddAmount; //sum of the total discount after subtration( TotDiscount=58,000)

                        }

                        else { }

                    }//No Discount is addedd to the Fees for parent

                    //}

                    //}

                    Decimal Amount = Convert.ToDecimal(row.Cells[4].Text.ToString().Trim());
                    totalAmount += Amount;

                }
            }
        }

        //var paymentExt = from x in context.PaymentPermanents
        //                           where x.ParentId == parentId
        //                           && x.SchoolId == schoolId
        //                           && x.CampusId == campusId
        //                           && x.SessionId == sessionId
        //                           && x.TermId == termId
        //                           //&& x.IsDiscount == true
        //                           && x.IsPaymentCompleted == null
        //                           select x;

        //foreach (var paymentExts in paymentExt)
        //{

        PaymentPermanent paymentExt = context.PaymentPermanents.FirstOrDefault(x => x.ParentId == parentId
         && x.SchoolId == schoolId
         && x.CampusId == campusId
         && x.SessionId == sessionId
         && x.TermId == termId
         && x.IsPaymentCompleted == null);

        if (paymentExt != null && paymentExt.IsDiscount == true) //check if discount has been applied for a parent's student. if true, discount doesnt applies else discount applies
        {

            //decimal payAfterDiscount = 0;
            //payAfterDiscount = totalAmount - TotDiscount; //to subtract the total amount after discount (58,000) from sum of original total amount (134,500)

            lblTotalInvoicePayable.Text = totalAmount.ToString("N");
            lblTotalAmountAfterDiscount.Text = totalAmount.ToString("N");
            lbltotalDiscountCalculated.Text = "0.00";

            //Response.Write(totalAmount);//134,500
            //Response.Write(",");
            //Response.Write(TotDiscount);//58,000
            //Response.Write(",");
            //Response.Write(payAfterDiscount);//76,500

        }


        else
        {
            decimal payAfterDiscount = 0;
            payAfterDiscount = totalAmount - TotDiscount; //to subtract the total amount after discount (58,000) from sum of original total amount (134,500)

            lblTotalInvoicePayable.Text = totalAmount.ToString("N");
            lblTotalAmountAfterDiscount.Text = payAfterDiscount.ToString("N");
            lbltotalDiscountCalculated.Text = TotDiscount.ToString("N");

            //Response.Write(totalAmount);//134,500
            //Response.Write(",");
            //Response.Write(TotDiscount);//58,000
            //Response.Write(",");
            //Response.Write(payAfterDiscount);//76,500
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

    void Download(PASSIS.LIB.User student, Document document, UsersLIB usrDal, string invoiceCode, decimal total)
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


            addResultSummaryPage(document, student, usrDal, invoiceCode, total);

        }
        catch (Exception ex)
        {
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
        }
    }

    protected void addResultSummaryPage(Document document, PASSIS.LIB.User student, UsersLIB usrDal, string invoiceCode, decimal Total)
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
        Paragraph p = new Paragraph("Invoice", darkerRedFnt);
        p.Alignment = Element.ALIGN_CENTER;
        document.Add(p);
        Paragraph SessionDetails = new Paragraph(string.Format("{0} {1} {2} Session", ddlTerm.SelectedItem.Text, ",", ddlSession.SelectedItem.Text), darkerRedFnt);
        SessionDetails.Alignment = Element.ALIGN_CENTER;
        document.Add(SessionDetails);

        //document.Add(new Phrase(Environment.NewLine));
        document.Add(new Phrase(Environment.NewLine));

        PdfPTable infoTable = new PdfPTable(7);
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


        var invoiceList = from s in context.PaymentInvoiceLists where s.InvoiceCode == invoiceCode select s;

        PdfPTable invoiceTable = new PdfPTable(7);

        PdfPCell feeTypeHeader = new PdfPCell(new Phrase("Fee Type", darkerGrnFnt9)); //feeTypeHeader.Padding = 0f;
        feeTypeHeader.Colspan = 4; //feeTypeHeader.HorizontalAlignment = Element.ALIGN_LEFT; feeTypeHeader.VerticalAlignment = Element.ALIGN_BOTTOM;

        PdfPCell amountHeader = new PdfPCell(new Phrase("Amount(N)", darkerGrnFnt9)); //amountHeader.Padding = 0f;
        amountHeader.Colspan = 3; //amountHeader.HorizontalAlignment = Element.ALIGN_LEFT; amountHeader.VerticalAlignment = Element.ALIGN_BOTTOM;

        invoiceTable.AddCell(feeTypeHeader);
        invoiceTable.AddCell(amountHeader);
        //invoiceTable.AddCell(DiscountHeader);


        foreach (PaymentInvoiceList list in invoiceList)
        {

            //PASSIS.LIB.User getStatus = context.Users.FirstOrDefault(x => x.Id == logonUser.Id);   
            //long? feeId =  list.FeeId;
            //var NumofChildren = new UsersLIB().RetrieveParentsChildren(logonUser.Id);

            // long parentId = logonUser.Id;
            // long wardId = Convert.ToInt64(ddlWard.SelectedValue);
            // long classId = Convert.ToInt64(ddlClass.SelectedValue);
            // long sessionId = Convert.ToInt64(ddlSession.SelectedValue);
            // long termId = Convert.ToInt64(ddlTerm.SelectedValue);
            // long schoolId = (long)logonUser.SchoolId;
            // long campusId = logonUser.SchoolCampusId;

            // var paymentExists = from x in context.PaymentPermanents
            //                     where x.ParentId == parentId
            //                         //&& x.GradeId == gradeId
            //                         //&& x.ClassId == classId
            //                     && x.SchoolId == schoolId
            //                     && x.CampusId == campusId
            //                     && x.SessionId == sessionId
            //                     && x.TermId == termId
            //                     && x.IsPaymentCompleted == null
            //                     select x;
            // long countPaymentPermanents = paymentExists.ToList().Count; //Add one to the number of payments in the PaymentPermanents Table

            //     FinanceDiscount getFeeDiscount = context.FinanceDiscounts.FirstOrDefault
            //     (x => x.FeeId == (feeId)
            //     && x.SchoolId == (schoolId)
            //     && x.CampusId == (campusId)
            //     && x.SessionId == (sessionId)
            //     && x.TermId == (termId)
            //     && x.GradeId == (classId)
            //     && x.Status == (getStatus.Status)
            //     && x.NumChild == (NumofChildren.Count));

            //     //get fee Discount specified for ALL status 
            //     FinanceDiscount getDiscountAllStatus = context.FinanceDiscounts.FirstOrDefault
            //     (x => x.FeeId == (feeId)
            //     && x.SchoolId == (schoolId)
            //     && x.CampusId == (campusId)
            //     && x.SessionId == (sessionId)
            //     && x.TermId == (termId)
            //     && x.GradeId == (classId)
            //     && x.Status == (4)
            //     && x.NumChild == (NumofChildren.Count));

            PdfPCell feeType = new PdfPCell(new Phrase(list.PaymentFee.PaymentFeeType.FeeName, resultTitleRedFnt10)); //feeType.Padding = 0f;
            feeType.Colspan = 4;

            PdfPCell amount = new PdfPCell(new Phrase(list.Amount.ToString(), resultTitleRedFnt10)); //amount.Padding = 0f;
            amount.Colspan = 3;

            invoiceTable.AddCell(feeType);
            invoiceTable.AddCell(amount);

            //    if (getFeeDiscount != null && NumofChildren.Count == countPaymentPermanents)
            //    {
            //        PdfPCell Discount = new PdfPCell(new Phrase(getFeeDiscount.Discount.ToString(), resultTitleRedFnt10)); //amount.Padding = 0f;
            //        Discount.Colspan = 3;

            //        invoiceTable.AddCell(feeType);
            //        invoiceTable.AddCell(amount);
            //        invoiceTable.AddCell(Discount);
            //    }

            //    else if (getDiscountAllStatus != null && NumofChildren.Count == countPaymentPermanents)
            //    {
            //        PdfPCell Discount = new PdfPCell(new Phrase(getDiscountAllStatus.Discount.ToString(), resultTitleRedFnt10)); //amount.Padding = 0f;
            //        Discount.Colspan = 3;

            //        invoiceTable.AddCell(feeType);
            //        invoiceTable.AddCell(amount);
            //        invoiceTable.AddCell(Discount);
            //    }
            //    else
            //    {

            //        PdfPCell Discount = new PdfPCell(new Phrase("", resultTitleRedFnt10)); //amount.Padding = 0f;
            //        Discount.Colspan = 3;

            //        invoiceTable.AddCell(feeType);
            //        invoiceTable.AddCell(amount);
            //        invoiceTable.AddCell(Discount);

            //}         

        }

        PdfPCell totalCell1 = new PdfPCell(new Phrase("Total Amount After Discount(N):")); totalCell1.Colspan = 3; totalCell1.HorizontalAlignment = Element.ALIGN_LEFT; totalCell1.Border = 0;
        PdfPCell totalCell2 = new PdfPCell(new Phrase(Total.ToString())); totalCell2.Colspan = 7; totalCell2.HorizontalAlignment = Element.ALIGN_LEFT; totalCell2.Border = 0;

        PdfPCell totalCell3 = new PdfPCell(new Phrase("Total Amount in words:")); totalCell3.Colspan = 3; totalCell3.HorizontalAlignment = Element.ALIGN_LEFT; totalCell3.Border = 0;
        PdfPCell totalCell4 = new PdfPCell(new Phrase(NumberToWords(Convert.ToInt64(Total)) + " naira only")); totalCell4.Colspan = 7; totalCell4.HorizontalAlignment = Element.ALIGN_LEFT; totalCell4.Border = 0;
        invoiceTable.AddCell(totalCell1);
        invoiceTable.AddCell(totalCell2);

        invoiceTable.AddCell(totalCell3);
        invoiceTable.AddCell(totalCell4);

        document.Add(invoiceTable);
    }

    public void InvoicePrinting(PASSIS.LIB.User selectedUsers, string invoiceCode, decimal total)
    {
        Document document = new Document();    // instantiate a iTextSharp.text.pdf.Document
        document.SetMargins(0f, 10f, 30f, 0f);
        MemoryStream mem = new MemoryStream(); // PDF data will be written here
        PdfWriter.GetInstance(document, mem);  // tie a PdfWriter instance to the stream
        document.SetPageSize(iTextSharp.text.PageSize.A4_LANDSCAPE);
        document.Open();
        UsersLIB usrdal = new UsersLIB();
        Download(selectedUsers, document, usrdal, invoiceCode, total);
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
                words += "and";

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