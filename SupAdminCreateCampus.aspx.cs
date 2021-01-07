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

public partial class SupAdminCreateCampus : PASSIS.LIB.Utility.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //txtSchoolId.Text = "0";
        if (!IsPostBack)
        {
            ddlSchool.DataSource = new PASSIS.LIB.SchoolLIB().getAllSchools();
            ddlSchool.DataBind();
            ddlSchool.Items.Insert(0, new ListItem("--Select School--", "0", true));
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtCampusName.Text != "" && txtCampusCode.Text != "" && txtCampusAddress.Text != "")
        {
            if (ddlSchool.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select school";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
            }
            else
            {
                clsMyDB db = new clsMyDB();
                string cond = "Name='" + txtCampusName.Text + "' AND Code='" + txtCampusCode.Text + "' AND SchoolId=" + logonUser.SchoolId;
                if (db.exist("SchoolCampus", cond))
                {
                    //report
                    lblErrorMsg.Text = "Campus already exist for your school, kindly try another.";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                }
                else
                {
                    //string query = "INSERT INTO SchoolCampus (Name, Code, CampusAddress, SchoolId) VALUES('" + txtCampusName.Text + "', '" + txtCampusCode.Text + "', '" + txtCampusAddress.Text + "', " + logonUser.SchoolId + ")";
                    try
                    {
                        PASSIS.LIB.SchoolCampus newCampus = new PASSIS.LIB.SchoolCampus();
                        newCampus.Name = txtCampusName.Text;
                        newCampus.CampusAddress = txtCampusAddress.Text;
                        newCampus.Code = txtCampusCode.Text;
                        newCampus.SchoolId = Convert.ToInt32(ddlSchool.SelectedValue);

                        new PASSIS.LIB.CampusLIB().SaveCampus(newCampus);
                        txtCampusName.Text = "";
                        txtCampusCode.Text = "";
                        txtCampusAddress.Text = "";
                        //bindgrid();
                        lblErrorMsg.Text = "Saved Successfully";
                        lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                        lblErrorMsg.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        lblErrorMsg.Text = "Error! Campus not added, review your input data or contact your system administrator.";
                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                        lblErrorMsg.Visible = true;
                    }
                }
            }
        }
        else
        {
            //report
            lblErrorMsg.Text = "Error! Some fields are empty, kindly review your input data.";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
        }
    }
    private void bindgrid()
    {
        //gvCampus.DataSource = new CampusLIB().GetAllCampus(logonUser.SchoolId.ToString());
        //gvCampus.DataBind();
    }
    
   
}