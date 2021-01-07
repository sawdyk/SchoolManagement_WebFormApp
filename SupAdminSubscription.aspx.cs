using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO;
using PASSIS.DAO.Utility;
using PASSIS.LIB;
using System.Data.SqlClient;

public partial class SupAdminSubscription : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlSchool.DataSource = new PASSIS.LIB.SchoolLIB().getAllSchools();
            ddlSchool.DataBind();
            ddlSchool.Items.Insert(0, new ListItem("--Select School--", "0", true));

            ddlTerm.DataSource = new AcademicTermLIB().RetrieveAcademicTerm();
            ddlTerm.DataBind();
        }
    }
    protected void btnSaveSubscriptionFees_Click(object sender, EventArgs e)
    {
        try
        {
            SubscriptionPaymentDetail newSubObj = new SubscriptionPaymentDetail();
            newSubObj.Amount = Convert.ToInt64(txtAmount.Text.Trim());
            newSubObj.CampusId = Convert.ToInt64(ddlCampus.SelectedValue);
            newSubObj.PaymentDate = txtPaymentDate.Text.Trim();
            newSubObj.SessionId = Convert.ToInt64(ddlSession.SelectedValue);
            newSubObj.TermId = Convert.ToInt64(ddlTerm.SelectedValue);
            newSubObj.AmountUsed = 0;
            newSubObj.SchoolId = Convert.ToInt64(ddlSchool.SelectedValue);
            newSubObj.HasFullyUsedForSubscription = false;
            context.SubscriptionPaymentDetails.InsertOnSubmit(newSubObj);
            context.SubmitChanges();
            lblReport.Text = "Subscription saved successfully";
            lblReport.ForeColor = System.Drawing.Color.Green;
        }
        catch (Exception ex)
        {

        }
    }
    protected void ddlSchool_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlCampus.DataSource = new CampusLIB().GetAllCampus(ddlSchool.SelectedValue);
        ddlCampus.DataBind();

        ddlSession.Items.Clear();
        clsMyDB mdb = new clsMyDB();
        mdb.connct();
        string query = "SELECT DISTINCT AcademicSessionId FROM AcademicSession WHERE SchoolId=" + ddlSchool.SelectedValue;
        SqlDataReader reader = mdb.fetch(query);
        while (reader.Read())
        {
            ddlSession.DataSource = from s in context.AcademicSessionNames
                                    where s.ID == Convert.ToInt64(reader["AcademicSessionId"].ToString())
                                    select s;
            ddlSession.DataBind();
        }
        reader.Close();
        mdb.closeConnct();

    }
}