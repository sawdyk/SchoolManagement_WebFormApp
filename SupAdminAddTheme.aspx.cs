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
using System.IO;

public partial class SupAdminAddTheme : PASSIS.LIB.Utility.BasePage
{
    //protected void Page_PreInit(object sender, EventArgs e)
    //{
    //    base.Page_PreInit(sender, e);
    //    this.MasterPageFile = "~/SupAdminCurriculum.master";
    //}

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        //loads
        if (!IsPostBack)
        {
            ddlCurriculum.DataSource = new PASSIS.LIB.SchoolLIB().SchoolCurriculum();
            ddlCurriculum.DataBind();
            ddlCurriculum.Items.Insert(0, new ListItem("--Select Curriculum--", "0", true));
            //this.schoolType.Visible = false;
            ddlSchoolType.DataSource = new PASSIS.LIB.SchoolLIB().SchoolType();
            ddlSchoolType.DataBind();
            ddlSchoolType.Items.Insert(0, new ListItem("--Select School Type--", "0", true));
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ddlSubject.SelectedValue != "" && txtTitle.Text != "")
        {
            clsMyDB db = new clsMyDB();
            string cond = "Title='" + txtTitle.Text + "' AND SubjectId=" + ddlSubject.SelectedValue + " AND ClassId=" + ddlClass.SelectedValue;
            if (db.exist("Subject_Theme", cond))
            {
                //report
                lblReport.Text = "Theme already exist, kindly try another.";
                lblReport.ForeColor = System.Drawing.Color.Red;
                lblReport.Visible = true;
            }
            else
            {
                try
                {
                    PASSIS.LIB.PASSISLIBDataContext context = new PASSIS.LIB.PASSISLIBDataContext();
                    PASSIS.LIB.Subject_Theme themeObj = new PASSIS.LIB.Subject_Theme();
                    themeObj.SubjectId = Convert.ToInt64(ddlSubject.SelectedValue);
                    themeObj.Title = txtTitle.Text.Trim();
                    themeObj.ClassId = Convert.ToInt64(ddlClass.SelectedValue);
                    context.Subject_Themes.InsertOnSubmit(themeObj);
                    context.SubmitChanges();
                    //report
                    lblReport.Text = "Theme added successfuly";
                    lblReport.ForeColor = System.Drawing.Color.Green;
                    lblReport.Visible = true;

                }
                catch (Exception ex)
                {
                    throw ex;
                    //report
                    //lblReport.Text = "Error! Theme not added, review your input data or contact your system administrator";
                    //lblReport.ForeColor = System.Drawing.Color.Red;
                    //lblReport.Visible = true;
                }
            }
        }
        else
        {
            //report
            lblReport.Text = "Error! Some fields are empty, kindly review your input data.";
            lblReport.ForeColor = System.Drawing.Color.Red;
            lblReport.Visible = true;
        }
    }
    protected void ddlCurriculum_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCurriculum.SelectedValue == "2")
        {
            ddlClass.Items.Clear();
            //schoolType.Visible = true;
        }
        else
        {
            ddlClass.Items.Clear();
            ddlClass.DataSource = new PASSIS.LIB.ClassGradeLIB().getClassGrade(Convert.ToInt64(ddlCurriculum.SelectedValue));
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("--Select Class--", "0", true));
            //schoolType.Visible = false;
        }
    }
    protected void ddlSchoolType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSchoolType.SelectedIndex == 1)
        {
            ddlClass.DataSource = new PASSIS.LIB.ClassGradeLIB().getClassGrade(Convert.ToInt64(ddlCurriculum.SelectedValue), Convert.ToInt64(ddlSchoolType.SelectedValue));
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("--Select Class--", "0", true));
        }
        else if (ddlSchoolType.SelectedIndex == 2)
        {
            ddlClass.DataSource = new PASSIS.LIB.ClassGradeLIB().getClassGrade(Convert.ToInt64(ddlCurriculum.SelectedValue), Convert.ToInt64(ddlSchoolType.SelectedValue));
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("--Select Class--", "0", true));
        }
        else if (ddlSchoolType.SelectedIndex == 3)
        {
            ddlClass.DataSource = new PASSIS.LIB.ClassGradeLIB().getClassGrade(Convert.ToInt64(ddlCurriculum.SelectedValue), Convert.ToInt64(ddlSchoolType.SelectedValue));
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("--Select Class--", "0", true));
        }
        else
        {
            ddlClass.Items.Clear();
        }
    }
    protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCurriculum.SelectedIndex == 1)
        {
            ddlSubject.DataSource = new PASSIS.LIB.SchoolLIB().GetClassSubjectsBritish(Convert.ToInt64(ddlCurriculum.SelectedValue), Convert.ToInt64(ddlClass.SelectedValue));
            ddlSubject.DataBind();
        }
        else if (ddlCurriculum.SelectedIndex == 2)
        {
            ddlSubject.DataSource = new PASSIS.LIB.SchoolLIB().GetClassSubjectsNigeria(Convert.ToInt64(ddlCurriculum.SelectedValue), Convert.ToInt64(ddlSchoolType.SelectedValue), Convert.ToInt64(ddlClass.SelectedValue));
            ddlSubject.DataBind();
        }
    }
}