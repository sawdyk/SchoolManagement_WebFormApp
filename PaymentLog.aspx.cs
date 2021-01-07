using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;


public partial class PaymentLog : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext dbcontext = new PASSISLIBDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        lblSummary.Visible = false;

        if (!IsPostBack)
        {
            BindDropDown();
            //code to fetch the Class based on their curriculum
            long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
            long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));

                        //code to fetch the Academic Sessions
            var session = (from c in dbcontext.AcademicSessions
                           where c.SchoolId == logonUser.SchoolId
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

        }
    }

    protected void btnViewPaymentLog_Click(object sender, EventArgs e)
    {
        lblSummary.Visible = true;
        try
        {
            var getOutStandSummary = from s in dbcontext.PaymentPermanents
                                     where s.SchoolId == logonUser.SchoolId && s.CampusId == logonUser.SchoolCampusId
                                     && Convert.ToInt64(s.StudentId) == (Convert.ToInt64(ddlWard.SelectedValue))
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
                                         s.DateCreated
                                     };

            gvOutStandPaySummary.DataSource = getOutStandSummary;
            gvOutStandPaySummary.DataBind();
        }
        catch (Exception ex)
        {

        }
    }

    protected void gvOutStandPaySummary_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvOutStandPaySummary.PageIndex = e.NewPageIndex;
        gvOutStandPaySummary.DataBind();
    }
    protected void BindDropDown()
    {
        ddlWard.DataSource = new UsersDAL().RetrieveParentsChildren(logonUser.Id);
        ddlWard.DataBind();
    }
}