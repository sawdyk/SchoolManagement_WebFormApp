using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.LIB.Utility;
using PASSIS.LIB;


public partial class SupAdminAddSession : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        BindGrid();
    }

    public void BindGrid()
    {
        gdvList.DataSource = new SupAdminAddSession().getallSession();
        gdvList.DataBind();
    }

    public IList<AcademicSessionName> getallSession()
    {

        var allacademicSession = from academicSession in context.AcademicSessionNames
                                 select academicSession;
        return allacademicSession.ToList<AcademicSessionName>();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {

            if (txtSessionName.Text == "")
            {
                lblErrorMsg.Text = "Kindly Enter The Session Name";
                lblErrorMsg.Visible = true;
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }

            AcademicSessionName academicSessionName = new AcademicSessionName();
            academicSessionName.SessionName = txtSessionName.Text;
            context.AcademicSessionNames.InsertOnSubmit(academicSessionName);
            context.SubmitChanges();
            lblErrorMsg.Text = "Session Sucessfully Added";
            lblErrorMsg.Visible = true;
            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
            BindGrid();
            txtSessionName.Text = string.Empty;
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = ex.Message;
            lblErrorMsg.Visible = true;
            throw ex;
        }
    }
    protected void gdvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvList.PageIndex = e.NewPageIndex;
        BindGrid();
    }
}