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
using PASSIS.LIB;

public partial class SupAdminAddThemeTopic : PASSIS.LIB.Utility.BasePage
{
    //protected void Page_PreInit(object sender, EventArgs e)
    //{
    //    base.Page_PreInit(sender, e);
    //    this.MasterPageFile = "~/SupAdminCurriculum.master";
    //}

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        //loads
        if (!IsPostBack)
        {
            ddlCurriculum.DataSource = new PASSIS.LIB.SchoolLIB().SchoolCurriculum();
            ddlCurriculum.DataBind();
            ddlCurriculum.Items.Insert(0, new ListItem("--Select Curriculum--", "0", true));
            ddlSchoolType.DataSource = new PASSIS.LIB.SchoolLIB().SchoolType();
            ddlSchoolType.DataBind();
            ddlSchoolType.Items.Insert(0, new ListItem("--Select School Type--", "0", true));
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ddlThemeTitle.SelectedValue != "" && txaTopic.Text != "")
        {
            clsMyDB db = new clsMyDB();
            string cond1 = "Topic='" + txaTopic.Text.ToString() + "' AND ThemeID=" + ddlThemeTitle.SelectedValue;
            string cond2 = "Id=" + ddlThemeTitle.SelectedValue + " AND SubjectId=" + ddlSubject.SelectedValue + " AND ClassId=" + ddlClass.SelectedValue;
            if (db.exist("SubjectThemeTopic", cond1) && db.exist("Subject_Theme", cond2))
            {
                //report
                lblReport.Text = "Theme Topic already exist, kindly try another.";
                lblReport.ForeColor = System.Drawing.Color.Red;
                lblReport.Visible = true;
            }
            else
            {
                string query = "INSERT INTO SubjectThemeTopic (Topic, ThemeID) VALUES('" + txaTopic.Text.ToString() + "', " + ddlThemeTitle.SelectedValue + ")";
                //string query = "INSERT INTO SubjectThemeTopic (Topic, ThemeID) VALUES(@topic, @theme)";
                SqlCommand cmd = new SqlCommand(query, db.Conn);
                //cmd.Parameters.AddWithValue("@topic", txaTopic.Text.ToString());
                //cmd.Parameters.AddWithValue("@theme", ddlThemeTitle.SelectedValue);
                try
                {
                    db.excQuery(query);
                    //db.excQuery(cmd);
                    //report
                    lblReport.Text = "Theme Topic added successfuly";
                    lblReport.ForeColor = System.Drawing.Color.Green;
                    lblReport.Visible = true;

                    txaTopic.Text = "";
                }
                catch (Exception ex)
                {
                    throw ex;
                    //report
                    //lblReport.Text = "Error! Theme Topic not added, review your input data or contact your system administrator";
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
        if (ddlCurriculum.SelectedIndex == 2)
        {
            ddlClass.Items.Clear();
            lblSchoolType.Visible = true;
            ddlSchoolType.Visible = true;
            //SchoolType.Visible = true;
        }
        else
        {
            ddlClass.Items.Clear();
            ddlClass.DataSource = new PASSIS.LIB.ClassGradeLIB().getClassGrade(Convert.ToInt64(ddlCurriculum.SelectedValue));
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("--Select Class--", "0", true));
            lblSchoolType.Visible = true;
            ddlSchoolType.Visible = true;
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
            ddlSubject.Items.Insert(0, new ListItem("--Select Subject--", "0", true));
        }
        else if (ddlCurriculum.SelectedIndex == 2)
        {
            ddlSubject.DataSource = new PASSIS.LIB.SchoolLIB().GetClassSubjectsNigeria(Convert.ToInt64(ddlCurriculum.SelectedValue), Convert.ToInt64(ddlSchoolType.SelectedValue), Convert.ToInt64(ddlClass.SelectedValue));
            ddlSubject.DataBind();
            ddlSubject.Items.Insert(0, new ListItem("--Select Subject--", "0", true));
        }
    }
    protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        var getAllTheme = from theme in context.Subject_Themes
                          where theme.SubjectId == Convert.ToInt64(ddlSubject.SelectedValue)
                          select theme;
        ddlThemeTitle.DataSource = getAllTheme;
        ddlThemeTitle.DataTextField = "Title";
        ddlThemeTitle.DataValueField = "Id";
        ddlThemeTitle.DataBind();
        ddlThemeTitle.Items.Insert(0, new ListItem("--Select Theme--", "0", true));
    }
    protected void ddlThemeTitle_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}