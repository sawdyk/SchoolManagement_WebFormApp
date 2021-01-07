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

public partial class FinanceDiscounts : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext dbContext = new PASSISLIBDataContext();

    long CurriculumID;
    long SchoolID;
    public static string SchoolName = "";
    public static string SchoolLogo = "";
    public static string SchoolAddress = "";
    public static string SchoolCurriculumId = "";
    public static string SchoolUrl = "";

    protected void Page_Load(object sender, EventArgs e)
    {

        lblErrorMsg.Text = "";
        BindGrid();
        if (!IsPostBack)
        {

            //code to fetch the Class based on their curriculum
            long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
            long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
            ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
            ddlYear.DataBind();
            ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Year--", "0"));
            //ddlWard.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Student--", "0"));
            ddlClass.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Class--", "0"));

            //code to fetch the payment categories
            var category = from s in dbContext.PaymentCategories select s;
            ddlFeeCategory.DataSource = category;
            ddlFeeCategory.DataTextField = "CategoryName";
            ddlFeeCategory.DataValueField = "Id";
            ddlFeeCategory.DataBind();
            ddlFeeCategory.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0", true));

            //code to fetch the Academic Sessions
            var session = (from c in dbContext.AcademicSessions
                           where c.SchoolId == logonUser.SchoolId
                           orderby c.IsCurrent descending
                           select c.AcademicSessionName).Distinct();

            ddlsession.DataSource = session;
            ddlsession.DataTextField = "SessionName";
            ddlsession.DataValueField = "ID";
            ddlsession.DataBind();
            ddlsession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0", true));


            //ddlNumChild.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Input Numbers", "2"));
            //ddlNumChild.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Any", "1"));
            //ddlNumChild.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));

            var status = from c in dbContext.Status select c;

            ddlStatus.DataSource = status;
            ddlStatus.DataTextField = "StatusName";
            ddlStatus.DataValueField = "Id";
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0", true));



            //code to fetch the Academic Terms
            var term = from c in dbContext.AcademicTerm1s
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


    protected void ddlFeeCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int64 selectedFeeTypeId = 0L;
        Int64 selectedSessionId = 0L;
        Int64 selectedTermId = 0L;

        selectedFeeTypeId = Convert.ToInt64(ddlFeeCategory.SelectedValue);
        selectedSessionId = Convert.ToInt64(ddlsession.SelectedValue);
        selectedTermId = Convert.ToInt64(ddlterm.SelectedValue);

        ddlFeeTypee.Items.Clear();
        //ddlAmount.Items.Clear();


        var getCashType = from s in dbContext.PaymentFeeTypes
                          where s.SchoolId == logonUser.SchoolId && s.CampusId == logonUser.SchoolCampusId && Convert.ToInt64(s.CategoryId) == (Convert.ToInt64(ddlFeeCategory.SelectedValue))
                          select s;


        ddlFeeTypee.DataSource = getCashType.ToList();
        ddlFeeTypee.DataTextField = "FeeName";
        ddlFeeTypee.DataValueField = "Id";
        ddlFeeTypee.DataBind();
        ddlFeeTypee.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
    }



    protected void ddlFeeTypee_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int64 selectedFeeTypeId = 0L;
        Int64 selectedSessionId = 0L;
        Int64 selectedTermId = 0L;
        string selectedCashCategory;

        selectedFeeTypeId = Convert.ToInt64(ddlFeeCategory.SelectedValue);
        selectedSessionId = Convert.ToInt64(ddlsession.SelectedValue);
        selectedTermId = Convert.ToInt64(ddlterm.SelectedValue);
        selectedCashCategory = ddlFeeTypee.SelectedItem.ToString();

        ddlGetFeeAmountId.Items.Clear();

        var getCashType = from s in dbContext.PaymentFees
                          where s.SchoolId == logonUser.SchoolId && s.CampusId == logonUser.SchoolCampusId && Convert.ToInt64(s.TermId) == (Convert.ToInt64(ddlterm.SelectedValue))
                          && Convert.ToInt64(s.SessionId) == (Convert.ToInt64(ddlsession.SelectedValue))
                          && Convert.ToInt64(s.ClassId) == (Convert.ToInt64(ddlYear.SelectedValue))
                          && (Convert.ToInt64(s.FeeTypeId) == Convert.ToInt64(ddlFeeTypee.SelectedValue))
                          select s;

        ddlGetFeeAmountId.DataSource = getCashType.ToList();
        ddlGetFeeAmountId.DataTextField = "Amount";
        ddlGetFeeAmountId.DataValueField = "Id";
        ddlGetFeeAmountId.DataBind();
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

    //protected void ddlNumChild_SelectedIndexChanged (object sender, EventArgs e)
    //{

    //    if (ddlNumChild.SelectedIndex == 2)
    //    {

    //        lblNumChild.Visible = true;
    //        txtNumChild.Visible = true;
    //        return;

    //    }
    //    else 
    //    {
    //        lblNumChild.Visible = false;
    //        txtNumChild.Visible = false;
    //        return;
    //    }



    //}
    //protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    ddlWard.Items.Clear();
    //    BindGrid(Convert.ToInt64(ddlYear.SelectedValue), Convert.ToInt64(ddlClass.SelectedValue));

    //}

    //protected void BindGrid(long yearId, long gradeId)
    //{
    //    long campusId, schoolId;
    //    campusId = logonUser.SchoolCampusId;
    //    schoolId = (long)logonUser.SchoolId;
    //    IList<PASSIS.LIB.GradeStudent> classList = new ClassGradeLIB().RetrieveGradeStudents(schoolId, campusId, yearId, gradeId);
    //    ddlWard.Items.Add(new System.Web.UI.WebControls.ListItem("--Select Student--", "0"));
    //    foreach (PASSIS.LIB.GradeStudent studentList in classList)
    //    {
    //        PASSIS.LIB.User stdList = dbContext.Users.FirstOrDefault(x => x.Id == studentList.StudentId);
    //        ddlWard.Items.Add(new System.Web.UI.WebControls.ListItem(stdList.StudentFullName, stdList.Id.ToString()));
    //    }
    //}


    protected void btnAdd_OnClick(object sender, EventArgs e)
    {

        try
        {

            int parsedValue;
            if (!int.TryParse(txtDiscount.Text, out parsedValue))
            {
                lblErrorMsg.Text = "Discount Must Contain Numbers Only!";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }

            if (Convert.ToInt64(txtDiscount.Text) > 100)
            {
                lblErrorMsg.Text = "Discount Must not be Greater than 100!";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }

            int parsedValuee;
            if (!int.TryParse(txtNumChild.Text, out parsedValuee))
            {
                lblErrorMsg.Text = "Number of Child Must Contain Numbers Only!";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
            }

            if (txtDiscount.Text == "")
            {
                lblErrorMsg.Text = "Discount is Required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }

            if (txtNumChild.Text == "")
            {
                lblErrorMsg.Text = "Number Of Child is Required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }

            if (ddlStatus.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Status is required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }

            if (ddlYear.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Year is Required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }

            if (ddlClass.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Class is required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            //if (ddlWard.SelectedIndex == 0)
            //{
            //    lblErrorMsg.Text = "Student Name is required";
            //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            //    trErrorMsg.Visible = true;
            //    lblErrorMsg.Visible = true;
            //    return;
            //}

            if (ddlsession.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Session is required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }

            if (ddlterm.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Select the Term";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;

            }

            if (ddlFeeCategory.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Select the Fee Category";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;

            }


            if (ddlFeeTypee.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Select the Fee Type";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;

            }

            if (ddlGetFeeAmountId.SelectedItem == null)
            {
                lblErrorMsg.Text = "Amount is Required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }

            if (ddlStatus.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Status is required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }

            //long StudentId = Convert.ToInt64(ddlWard.SelectedValue);
            long YearId = Convert.ToInt64(ddlYear.SelectedValue);
            long ClassId = Convert.ToInt64(ddlClass.SelectedValue);
            long SchoolId = Convert.ToInt64(logonUser.SchoolId);
            long CampusId = logonUser.SchoolCampusId;
            long SessionId = Convert.ToInt64(ddlsession.SelectedValue);
            long TermId = Convert.ToInt64(ddlterm.SelectedValue);
            long GetFeeAmountId = Convert.ToInt64(ddlGetFeeAmountId.SelectedValue);
            long Status = Convert.ToInt64(ddlStatus.SelectedValue);
            long Discount = Convert.ToInt64(txtDiscount.Text);
            long NumChild = Convert.ToInt64(txtNumChild.Text);

            //Response.Write(ddlGetFeeAmountId.SelectedValue.ToString());
            //Response.Write(ddlFeeCategory.SelectedValue.ToString());


            //if (ddlStatus.SelectedIndex != 0)
            //{


            //    PASSIS.LIB.FinanceDiscount getDiscountStatusAlll = dbContext.FinanceDiscounts.FirstOrDefault
            //    (x => x.FeeTypeId == Convert.ToInt64(ddlGetFeeAmountId.SelectedValue)
            //            && x.SchoolId == (Convert.ToInt64(logonUser.SchoolId))
            //                          && x.ClassId == (Convert.ToInt64(ddlClass.SelectedValue))
            //                          && x.CampusId == (Convert.ToInt64(logonUser.SchoolCampusId))
            //                          && x.SessionId == (Convert.ToInt64(ddlsession.SelectedValue))
            //                          && x.TermId == (Convert.ToInt64(ddlterm.SelectedValue))
            //                           && x.GradeId == (Convert.ToInt64(ddlYear.SelectedValue))
            //    && x.Status == (3)
            //     && x.Discount == (Convert.ToInt64(txtDiscount.Text))
            //     && x.NumChild == (Convert.ToInt64(txtNumChild.Text)));

            //    if (getDiscountStatusAlll != null)
            //    {

            //        lblErrorMsg.Text = "You cant Add this Status (" + ddlStatus.SelectedItem.ToString() + "), Because you've defined this fee type for All Status!";
            //        lblMessage.Text = " ";
            //        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            //        lblErrorMsg.Visible = true;
            //        trErrorMsg.Visible = true;
            //    }

            //    else
            //    {
            //        PASSIS.LIB.FinanceDiscount newDiscount = new PASSIS.LIB.FinanceDiscount();
            //        newDiscount.FeeId = GetFeeAmountId;
            //        newDiscount.SchoolId = SchoolId;
            //        newDiscount.CampusId = CampusId;
            //        newDiscount.GradeId = YearId;
            //        newDiscount.ClassId = ClassId;
            //        newDiscount.SessionId = SessionId;
            //        newDiscount.TermId = TermId;
            //        newDiscount.Discount = Discount;
            //        newDiscount.NumChild = NumChild;
            //        newDiscount.Status = Status;
            //        newDiscount.Date = DateTime.Now;

            //        dbContext.FinanceDiscounts.InsertOnSubmit(newDiscount);
            //        dbContext.SubmitChanges();

            //        lblErrorMsg.Text = "New Discount Added Successfully!";
            //        lblMessage.Text = " ";
            //        lblErrorMsg.ForeColor = System.Drawing.Color.Green;
            //        lblErrorMsg.Visible = true;
            //        trErrorMsg.Visible = true;

            //    }



            //}


            if (ddlStatus.SelectedIndex == 4) //When users selects All as Status
            {


                PASSIS.LIB.FinanceDiscount getDiscountStatusAll = dbContext.FinanceDiscounts.FirstOrDefault
                (x => x.FeeId == Convert.ToInt64(ddlGetFeeAmountId.SelectedValue)
                        && x.SchoolId == (Convert.ToInt64(logonUser.SchoolId))
                                      && x.ClassId == (Convert.ToInt64(ddlClass.SelectedValue))
                                      && x.CampusId == (Convert.ToInt64(logonUser.SchoolCampusId))
                                      && x.SessionId == (Convert.ToInt64(ddlsession.SelectedValue))
                                      && x.TermId == (Convert.ToInt64(ddlterm.SelectedValue))
                                       && x.GradeId == (Convert.ToInt64(ddlYear.SelectedValue))
                 //&& x.Status == (Convert.ToInt64(ddlStatus.SelectedValue))
                 // && x.Discount == (Convert.ToInt64(txtDiscount.Text))
                 && x.NumChild == (Convert.ToInt64(txtNumChild.Text)));
                if (getDiscountStatusAll != null)
                {

                    lblErrorMsg.Text = "You cant Add this Status (All), Because you've defined this fee type for another Status!";
                    lblMessage.Text = " ";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                }

                else
                {
                    PASSIS.LIB.FinanceDiscount newDiscount = new PASSIS.LIB.FinanceDiscount();
                    newDiscount.FeeId = GetFeeAmountId;
                    newDiscount.SchoolId = SchoolId;
                    newDiscount.CampusId = CampusId;
                    newDiscount.GradeId = YearId;
                    newDiscount.ClassId = ClassId;
                    newDiscount.SessionId = SessionId;
                    newDiscount.TermId = TermId;
                    newDiscount.Discount = Discount;
                    newDiscount.NumChild = NumChild;
                    newDiscount.Status = Status;
                    newDiscount.Date = DateTime.Now;

                    dbContext.FinanceDiscounts.InsertOnSubmit(newDiscount);
                    dbContext.SubmitChanges();

                    lblErrorMsg.Text = "New Discount Added Successfully!";
                    lblMessage.Text = " ";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                    lblErrorMsg.Visible = true;

                }



            }



            else if (ddlStatus.SelectedIndex == 1 || ddlStatus.SelectedIndex == 2 || ddlStatus.SelectedIndex == 3)
            {


                PASSIS.LIB.FinanceDiscount getDiscount = dbContext.FinanceDiscounts.FirstOrDefault
                    (x => x.FeeId == Convert.ToInt64(ddlGetFeeAmountId.SelectedValue)
                        && x.SchoolId == (Convert.ToInt64(logonUser.SchoolId))
                                      && x.ClassId == (Convert.ToInt64(ddlClass.SelectedValue))
                                      && x.CampusId == (Convert.ToInt64(logonUser.SchoolCampusId))
                                      && x.SessionId == (Convert.ToInt64(ddlsession.SelectedValue))
                                      && x.TermId == (Convert.ToInt64(ddlterm.SelectedValue))
                                       && x.GradeId == (Convert.ToInt64(ddlYear.SelectedValue))
                                        && x.Status == (Convert.ToInt64(ddlStatus.SelectedValue))
                                         && x.Discount == (Convert.ToInt64(txtDiscount.Text))
                                         && x.NumChild == (Convert.ToInt64(txtNumChild.Text)));



                PASSIS.LIB.FinanceDiscount getDiscountStatusAlll = dbContext.FinanceDiscounts.FirstOrDefault
                (x => x.FeeId == Convert.ToInt64(ddlGetFeeAmountId.SelectedValue)
                        && x.SchoolId == (Convert.ToInt64(logonUser.SchoolId))
                                      && x.ClassId == (Convert.ToInt64(ddlClass.SelectedValue))
                                      && x.CampusId == (Convert.ToInt64(logonUser.SchoolCampusId))
                                      && x.SessionId == (Convert.ToInt64(ddlsession.SelectedValue))
                                      && x.TermId == (Convert.ToInt64(ddlterm.SelectedValue))
                                       && x.GradeId == (Convert.ToInt64(ddlYear.SelectedValue))
                && x.Status == (4)///////////////////////////
                 && x.Discount == (Convert.ToInt64(txtDiscount.Text))
                 && x.NumChild == (Convert.ToInt64(txtNumChild.Text)));

                if (getDiscountStatusAlll != null)
                {

                    lblErrorMsg.Text = "You cant Add this Status (" + ddlStatus.SelectedItem.ToString() + "), Because you've defined this fee type for All Status!";
                    lblMessage.Text = " ";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                }

                else if (getDiscount != null)
                {

                    lblErrorMsg.Text = "This Discount Exist!";
                    lblMessage.Text = " ";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                }

                else
                {
                    PASSIS.LIB.FinanceDiscount newDiscount = new PASSIS.LIB.FinanceDiscount();
                    newDiscount.FeeId = GetFeeAmountId;
                    newDiscount.SchoolId = SchoolId;
                    newDiscount.CampusId = CampusId;
                    newDiscount.GradeId = YearId;
                    newDiscount.ClassId = ClassId;
                    newDiscount.SessionId = SessionId;
                    newDiscount.TermId = TermId;
                    newDiscount.Discount = Discount;
                    newDiscount.NumChild = NumChild;
                    newDiscount.Status = Status;
                    newDiscount.Date = DateTime.Now;

                    dbContext.FinanceDiscounts.InsertOnSubmit(newDiscount);
                    dbContext.SubmitChanges();

                    lblErrorMsg.Text = "New Discount Added Successfully!";
                    lblMessage.Text = " ";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                    lblErrorMsg.Visible = true;

                }



            }




            else
            {

                PASSIS.LIB.FinanceDiscount getDiscount = dbContext.FinanceDiscounts.FirstOrDefault
                    (x => x.FeeId == Convert.ToInt64(ddlGetFeeAmountId.SelectedValue)
                        && x.SchoolId == (Convert.ToInt64(logonUser.SchoolId))
                                      && x.ClassId == (Convert.ToInt64(ddlClass.SelectedValue))
                                      && x.CampusId == (Convert.ToInt64(logonUser.SchoolCampusId))
                                      && x.SessionId == (Convert.ToInt64(ddlsession.SelectedValue))
                                      && x.TermId == (Convert.ToInt64(ddlterm.SelectedValue))
                                       && x.GradeId == (Convert.ToInt64(ddlYear.SelectedValue))
                                        && x.Status == (Convert.ToInt64(ddlStatus.SelectedValue))
                                         && x.Discount == (Convert.ToInt64(txtDiscount.Text))
                                         && x.NumChild == (Convert.ToInt64(txtNumChild.Text)));





                if (getDiscount != null)
                {

                    lblErrorMsg.Text = "This Discount Exist!";
                    lblMessage.Text = " ";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                }



                else
                {
                    PASSIS.LIB.FinanceDiscount newDiscount = new PASSIS.LIB.FinanceDiscount();
                    newDiscount.FeeId = GetFeeAmountId;
                    newDiscount.SchoolId = SchoolId;
                    newDiscount.CampusId = CampusId;
                    newDiscount.GradeId = YearId;
                    newDiscount.ClassId = ClassId;
                    newDiscount.SessionId = SessionId;
                    newDiscount.TermId = TermId;
                    newDiscount.Discount = Discount;
                    newDiscount.NumChild = NumChild;
                    newDiscount.Status = Status;
                    newDiscount.Date = DateTime.Now;

                    dbContext.FinanceDiscounts.InsertOnSubmit(newDiscount);
                    dbContext.SubmitChanges();

                    lblErrorMsg.Text = "New Discount Added Successfully!";
                    lblMessage.Text = " ";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                    lblErrorMsg.Visible = true;

                }

            }

        }

        catch (Exception ex)
        {
            lblErrorMsg.Text = "Make sure all Fields are Filled Appropriately and correctly!";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
        }
    }



    protected void BindGrid()

    {

        var getAllDiscount = from s in dbContext.FinanceDiscounts
                             where s.SchoolId == logonUser.SchoolId && s.CampusId == logonUser.SchoolCampusId

                             select new
                             {

                                 s.PaymentFee.PaymentFeeType.FeeName,
                                 s.Grade.GradeName,
                                 s.Class_Grade.Name,
                                 s.AcademicSessionName.SessionName,
                                 s.AcademicTerm1.AcademicTermName,
                                 s.Discount,
                                 s.NumChild,
                                 s.Status1.StatusName,
                                 s.Date

                             };

        gdvLists.DataSource = getAllDiscount;
        gdvLists.DataBind();
    }


    protected void gdvLists_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvLists.PageIndex = e.NewPageIndex;
        gdvLists.DataBind();
    }

}