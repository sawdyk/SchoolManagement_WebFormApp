using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO;
using PASSIS.DAO.Utility;
public partial class SupAdminAddRoles : PASSIS.LIB.Utility.BasePage
{
    //protected void Page_PreInit(object sender, EventArgs e)
    //{
    //    base.Page_PreInit(sender, e);
    //    this.MasterPageFile = "~/SchlAdminSchlMng.master";
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //ddlRole.Items.Clear();
            //ddlRole.DataSource = new PASSIS.DAO.RoleDAL().getAllRoles();

            //ddlRole.DataTextField = "RoleName";
            //ddlRole.DataValueField = "Id";
        }
        BindGrid();
    }

    protected void BindGrid()
    {
        gdvList.DataSource = new PASSIS.DAO.RoleDAL().getAllRoles();
        gdvList.DataBind();
    }
    public string getRoleName(object parentRoleId)
    {
        Int32 pRoleID = 0;
        string roleName = "Self";
        try
        {
            pRoleID = Convert.ToInt32(parentRoleId);
        }
        catch { }
        if (pRoleID != 0)
        {
            roleName = new PASSIS.DAO.RoleDAL().RetrieveRole(pRoleID).RoleName;
        }
        return roleName;

    }
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        //validate the value and save
        try
        {
            if (txtName.Text == "")
            {
                lblErrorMsg.Text = string.Format("Enter Role Name!");
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            if (txtCode.Text == "")
            {
                lblErrorMsg.Text = string.Format("Enter Code!");
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            if (ddlRole.SelectedIndex == 0)
            {
                lblErrorMsg.Text = string.Format("Select The Parent Role!");
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            string name = string.Empty;
            string code = string.Empty;
            Int32 selectedRole = 0;
            name = txtName.Text.Trim();
            code = txtCode.Text.Trim();
            selectedRole = Convert.ToInt32(ddlRole.SelectedValue);
            RoleDAL rolDal = new RoleDAL();

            Role rol = new Role();
            rol.RoleCode = code;
            rol.RoleName = name;
            rol.ParentRoleId = selectedRole;

            rolDal.SaveRole(rol);

            //bind the data to a grid 
            BindGrid();
            ddlRole.SelectedValue = "0";
            txtCode.Text = txtName.Text = string.Empty;
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = ex.Message;
            lblErrorMsg.Visible = true;
            throw ex;
        }
    }

    protected void gdvList_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {

            }

        }
    }

    protected void gdvList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //long rowItemId = long.Parse(e.CommandArgument.ToString());

        switch (e.CommandName.ToLower())
        {
            //case "delete":
            //    new HomeRoomDAL().DeleteHomeRoom(rowItemId);
            //    BindGrid();
            //    break;

        }
    }

    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gdvList.EditIndex = -1;
        BindGrid();
    }

    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gdvList.EditIndex = e.NewEditIndex;
        BindGrid();
    }

    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }

    protected void gdvList_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Label lblId = (Label)gdvList.Rows[e.RowIndex].FindControl("lblId");
        long rowItemId = long.Parse(lblId.Text);
        //new HomeRoomDAL().DeleteHomeRoom(rowItemId);
        BindGrid();
    }
    protected void gdvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvList.PageIndex = e.NewPageIndex;
        BindGrid();
    }
}