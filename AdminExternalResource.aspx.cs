using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using System.Data.SqlClient;
using PASSIS.LIB;

public partial class AdminExternalResource : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Bindgrid();
        }
        catch (Exception ex)
        {
        }
    }

    private void Bindgrid()
    {
        var getResource = from list in context.ExternalResourceCenters
                          where list.CampusId == logonUser.SchoolCampusId
                          select list;
        gdvList.DataSource = getResource.ToList();
        gdvList.DataBind();
    }
}