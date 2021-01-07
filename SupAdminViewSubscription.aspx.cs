using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.LIB;
using PASSIS.DAO.Utility;

public partial class SupAdminViewSubscription : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Bindgrid();
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void Bindgrid()
    {
        gdvSubList.DataSource = from subList in context.SubscriptionPaymentDetails
                                select new
                                {
                                    subList.ID,
                                    CampusName = subList.SchoolCampus.Name,
                                    SchoolName = subList.School.Name,
                                    SessionName = subList.AcademicSessionName.SessionName,
                                    TermName = subList.AcademicTerm1.AcademicTermName,
                                    subList.Amount,
                                    subList.AmountUsed,
                                    subList.PaymentDate,
                                    FullyUtilized = Convert.ToBoolean(subList.HasFullyUsedForSubscription) ? "YES" : "NO",
                                };
        gdvSubList.DataBind();
    }
    protected void gdvSubList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvSubList.PageIndex = e.NewPageIndex;
        gdvSubList.DataBind();
    }
}