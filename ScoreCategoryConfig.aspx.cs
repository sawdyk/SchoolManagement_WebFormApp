using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;

public partial class ScoreCategoryConfig : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
            long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));

            ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
            ddlYear.DataBind();
            ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Year--", "0", true));
            LoadCategory();
            BindScore();

            ddlSession.DataSource = schSession().Distinct();
            ddlSession.DataTextField = "SessionName";
            ddlSession.DataValueField = "ID";
            ddlSession.DataBind();
            ddlSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Session--", "0", true));

            ddlTerm.DataSource = schTerm().Distinct();
            ddlTerm.DataTextField = "AcademicTermName";
            ddlTerm.DataValueField = "Id";
            ddlTerm.DataBind();
            ddlTerm.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Term--", "0", true));
        }
    }

    public void BindScore()
    {
        gvCategory.DataSource = new SchoolLIB().GetScoreCategoryConfiguration((long)logonUser.SchoolId, logonUser.SchoolCampusId);
        gvCategory.DataBind();
    }

    protected void gvScoreGrades_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lblErrorMsg.Visible = false;
        gvCategory.EditIndex = -1;
        BindScore();
    }
   
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlYear.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly Select the class";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            if (ddlSession.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly Select the session";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            if (ddlTerm.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly Select the term";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            if (ddlCategory.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly Select the category";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            if (ddlCategory.SelectedItem.Text == "Behavioral" || ddlCategory.SelectedItem.Text == "Extra Curricular")
            {
                if (ddlRange.SelectedIndex == 0)
                {
                    lblErrorMsg.Text = "Kindly Select the range";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                    return;
                }
            }
            string categoryName = ddlCategory.SelectedItem.Text.Trim();
            int percentage = Convert.ToInt32(txtPercentage.Text.Trim());
            long classId = Convert.ToInt64(ddlYear.SelectedValue);
            long sessionId = Convert.ToInt64(ddlSession.SelectedValue);
            long termId = Convert.ToInt64(ddlTerm.SelectedValue);

            IList<ScoreCategoryConfiguration> getCategory = new SchoolLIB().GetScoreCategoryConfiguration((long)logonUser.SchoolId, (long)logonUser.SchoolCampusId, classId, sessionId, termId, categoryName); //work on the ddlyear as the last parameter inside getconfiguration
            if (getCategory.Count > 0)
            {
                lblErrorMsg.Text = "Configuration already exist, kindly use panel below to edit";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            else
            {
                if (percentage <= 100 && percentage >= 0)
                {
                    int? totalPercent = (from s in context.ScoreCategoryConfigurations where s.SchoolId == logonUser.SchoolId && s.CampusId == logonUser.SchoolCampusId && s.ClassId == classId && s.SessionId == sessionId && s.TermId == termId select s).Sum(x => x.Percentage);
                    if (totalPercent < 100 || totalPercent == null || percentage == 0)
                    {
                        ScoreCategoryConfiguration newConfig = new ScoreCategoryConfiguration();
                        //newConfig.CampusId = logonUser.SchoolCampusId;
                        newConfig.SchoolId = (long)logonUser.SchoolId;
                        newConfig.CampusId = logonUser.SchoolCampusId;
                        newConfig.Category = ddlCategory.SelectedItem.Text.Trim();
                        newConfig.Percentage = Convert.ToInt16(txtPercentage.Text);
                        if (ddlCategory.SelectedItem.Text == "Behavioral" || ddlCategory.SelectedItem.Text == "Extra Curricular")
                        {
                            newConfig.Range = Convert.ToInt16(ddlRange.SelectedItem.Text);
                        }
                        newConfig.Date = DateTime.Now;
                        newConfig.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
                        newConfig.SessionId = Convert.ToInt64(ddlSession.SelectedValue);
                        newConfig.TermId = Convert.ToInt64(ddlTerm.SelectedValue);
                        if (totalPercent != null)
                        {
                            if (totalPercent + newConfig.Percentage <= 100)
                            {
                                new SchoolLIB().SaveScoreCategoryConfiguration(newConfig);
                                lblErrorMsg.Text = "Configuration saved successfully";
                                lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                                lblErrorMsg.Visible = true;
                                ddlCategory.SelectedIndex = 0;
                                txtPercentage.Text = "";
                                BindScore();
                            }
                            else
                            {
                                lblErrorMsg.Text = "You can't have more than 100% for a class";
                                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                                lblErrorMsg.Visible = true;
                            }
                        }
                        else
                        {
                            new SchoolLIB().SaveScoreCategoryConfiguration(newConfig);
                            lblErrorMsg.Text = "Configuration saved successfully";
                            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                            lblErrorMsg.Visible = true;
                            ddlCategory.SelectedIndex = 0;
                            txtPercentage.Text = "";
                            ddlYear.SelectedIndex = 0;
                            BindScore();
                        }
                    }
                    else
                    {
                        lblErrorMsg.Text = "You can't have more than 100% for a class";
                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                        lblErrorMsg.Visible = true;
                    }
                }
                else
                {
                    lblErrorMsg.Text = "The range should be 0% to 100%";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
            lblErrorMsg.Text = "Error occurred, kindly contact your administrator";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
        }

    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    public string getClassName(object ClassId)
    {
        ClassGradeLIB cgl = new ClassGradeLIB();
        string theName = cgl.RetrieveClass_Grade(Convert.ToInt32(ClassId)).Name.ToString();
        return theName;
    }
    protected void gvScoreGrades_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCategory.PageIndex = e.NewPageIndex;
        BindScore();
    }
    protected void gvScoreGrades_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvCategory.EditIndex = e.NewEditIndex;
        BindScore();
        txtPercentage.Text = "0";
    }
    protected void gvScoreGrades_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            lblErrorMsg.Visible = false;
            Label lblId = (Label)gvCategory.Rows[e.RowIndex].FindControl("lblId");
            TextBox txtGvClass = (TextBox)gvCategory.Rows[e.RowIndex].FindControl("txtClass");
            TextBox txtGvSession = (TextBox)gvCategory.Rows[e.RowIndex].FindControl("txtSession");
            TextBox txtGvTerm = (TextBox)gvCategory.Rows[e.RowIndex].FindControl("txtTerm");
            TextBox txtGvCategory = (TextBox)gvCategory.Rows[e.RowIndex].FindControl("txtCategory");
            TextBox txtGvPercentage = (TextBox)gvCategory.Rows[e.RowIndex].FindControl("txtPercentage");
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            PASSIS.LIB.Class_Grade classGrade = context.Class_Grades.FirstOrDefault(x => x.Name == txtGvClass.Text.Trim());
            PASSIS.LIB.AcademicSessionName session = context.AcademicSessionNames.FirstOrDefault(x => x.SessionName == txtGvSession.Text.Trim());
            PASSIS.LIB.AcademicTerm1 term = context.AcademicTerm1s.FirstOrDefault(x => x.AcademicTermName == txtGvTerm.Text.Trim());
            PASSIS.LIB.ScoreCategoryConfiguration objScoreCategoryConfig = context.ScoreCategoryConfigurations.FirstOrDefault(x => x.Id == Convert.ToInt64(lblId.Text));
            int? totalPercent = (from s in context.ScoreCategoryConfigurations where s.SchoolId == logonUser.SchoolId && s.CampusId == logonUser.SchoolCampusId && s.ClassId == classGrade.Id && s.SessionId == session.ID && s.TermId == term.Id select s).Sum(x => x.Percentage);
            string category = txtGvCategory.Text.Trim();
            string percentage = txtGvPercentage.Text.Trim();

            int? oldPecentage = objScoreCategoryConfig.Percentage;
            int? availableTotalPercentage = totalPercent - oldPecentage;
            objScoreCategoryConfig.Category = category;
            objScoreCategoryConfig.Percentage = Convert.ToInt16(percentage);
            if (availableTotalPercentage + objScoreCategoryConfig.Percentage <= 100)
            {
                context.SubmitChanges();
                gvCategory.EditIndex = -1;
                BindScore();
                lblMessage.Text = "Updated Successfully";
                lblMessage.ForeColor = System.Drawing.Color.Green;
                txtGvCategory.Text = "";
                txtGvPercentage.Text = "";
                txtPercentage.Text = "";
            }
            else
            {
                lblMessage.Text = "You can't have more than 100% for a class";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error occurred, try again";
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }

    private void LoadCategory()
    {
        var categoryList = from s in context.ScoreCategorySystems select s;
        ddlCategory.DataSource = categoryList;
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, new ListItem("--Select--", "0", true));
    }
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlRange.Items.Clear();
        if (ddlCategory.SelectedItem.Text.Trim() == "Behavioral" || ddlCategory.SelectedItem.Text == "Extra Curricular")
        {
            ddlRange.Items.Add(new ListItem("4", "1"));
            ddlRange.Items.Add(new ListItem("5", "2"));
            ddlRange.Items.Insert(0, new ListItem("--Select--", "0", true));

            ddlRange.Visible = true;
            Label4.Visible = true;
        }
        else
        {
            ddlRange.Visible = false;
            Label4.Visible = false;
        }

    }
    public IList<AcademicSessionName> schSession()
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        var session = from c in context.AcademicSessions
                      where c.SchoolId == logonUser.SchoolId    
                      //&& c.IsCurrent == true
                      orderby c.IsCurrent descending
                      select c.AcademicSessionName;
        return session.ToList<AcademicSessionName>();
    }
    public IList<AcademicTerm1> schTerm()
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        var session = from c in context.AcademicSessions
                      where c.SchoolId == logonUser.SchoolId
                      orderby c.IsCurrent descending
                      select c.AcademicTerm1;
        return session.ToList<AcademicTerm1>();
    }
}