using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;


public partial class SupAdminCreateClassGrade : PASSIS.LIB.Utility.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindGrid();
        if (!IsPostBack)
        {
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            ddlSchoolType.DataSource = from sc in context.SchoolTypes select sc;
            ddlSchoolType.DataTextField = "SchoolTypeName";
            ddlSchoolType.DataValueField = "Id";
            ddlSchoolType.DataBind();

            ddlCurriculum.DataSource = from sc in context.Curriculums select sc;
            ddlCurriculum.DataTextField = "CurriculumName";
            ddlCurriculum.DataValueField = "Id";
            ddlCurriculum.DataBind();
        }
    }

    protected void BindGrid()
    {
        gdvList.DataSource = new PASSIS.LIB.SchoolConfigLIB().getAllClass_Grade();
        gdvList.DataBind();
    }
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        //validate the value and save
        try
        {
            string name = string.Empty;
            string code = string.Empty;
            string address = string.Empty;
            name = txtName.Text.Trim();
            code = txtCode.Text.Trim();
            //address = txtAddress.Text.Trim(); 
            ClassGradeLIB clsDAL = new ClassGradeLIB();
            if (clsDAL.RetrieveClass_GradeByCode((long)logonUser.SchoolId, code) != null)
            {
                lblErrorMsg.Text = string.Format("Code already exist! Try again.");
                lblErrorMsg.Visible = true;
                return;
            }
            if (clsDAL.RetrieveClass_GradeByName((long)logonUser.SchoolId, name) != null)
            {
                lblErrorMsg.Text = string.Format("Name already exist! Try again.");
                lblErrorMsg.Visible = true;
                return;
            }
            PASSIS.LIB.Class_Grade sch = new PASSIS.LIB.Class_Grade();
            sch.Code = code;
            sch.Name = name;
            //PASSIS.LIB.School schl = new PASSIS.LIB.School();
            //schl = logonUser.School;
            sch.DateCreated = DateTime.Now;
            sch.School = (long)logonUser.SchoolId;
            //sch.Address = address;
            sch.UserId = logonUser.Id;
            sch.CurriculumId = Convert.ToInt32(ddlCurriculum.SelectedValue);
            sch.SchoolTypeId = Convert.ToInt32(ddlSchoolType.SelectedValue);
            clsDAL.SaveClassGrade(sch);

            //bind the data to a grid 
            BindGrid();
            txtCode.Text = txtName.Text = string.Empty;
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