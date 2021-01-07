using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO;
using PASSIS.DAO.Utility;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using PASSIS.LIB;


public partial class SupAdminViewCampus : PASSIS.LIB.Utility.BasePage
{
  
    protected void Page_Load(object sender, EventArgs e)
    {
        //txtSchoolId.Text = "0";
        if (!IsPostBack)
        {
            ddlSchoolList.DataSource = new PASSIS.LIB.SchoolLIB().getAllSchools();
            ddlSchoolList.DataBind();
            ddlSchoolList.Items.Insert(0, new ListItem("--Select School--", "0", true));
        }
    }
   
    protected void ddlSchoolList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSchoolList.SelectedIndex == 0) { lblSchool.Visible = false; }
        else { lblSchool.Visible = true; }
        BindCampus(ddlSchoolList.SelectedValue);
    }
    protected void gvCampus_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvCampus.EditIndex = e.NewEditIndex;
        BindCampus(ddlSchoolList.SelectedValue);
    }
    public void BindCampus(string schoolId)
    {
        gvCampus.DataSource = new CampusLIB().GetAllCampus(schoolId);
        gvCampus.DataBind();
    }
    protected void gvCampus_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvCampus.EditIndex = -1;
        BindCampus(ddlSchoolList.SelectedValue);
    }
    protected void gvCampus_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            Label lblId = (Label)gvCampus.Rows[e.RowIndex].FindControl("lblId");
            TextBox campus_name = (TextBox)gvCampus.Rows[e.RowIndex].FindControl("txtCampusName");
            TextBox campus_address = (TextBox)gvCampus.Rows[e.RowIndex].FindControl("txtCampusAddress");
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            PASSIS.LIB.SchoolCampus objSchoolCampus = context.SchoolCampus.FirstOrDefault(x => x.Id == Convert.ToInt64(lblId.Text));
            objSchoolCampus.Name = campus_name.Text;
            objSchoolCampus.CampusAddress = campus_address.Text;
            context.SubmitChanges();
            gvCampus.EditIndex = -1;
            BindCampus(ddlSchoolList.SelectedValue);
            lblErrorMsg.Text = "Changes Saved Successfully";
            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = "Error occurred, try again";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
        }
    }
}