using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;

public partial class SubCategoryConfig : PASSIS.LIB.Utility.BasePage
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
        gvSubCategory.DataSource = new SchoolLIB().GetScoreSubCategoryConfiguration((long)logonUser.SchoolId, logonUser.SchoolCampusId);
        gvSubCategory.DataBind();
    }

    protected void gvSubCategory_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lblErrorMsg.Visible = false;
        gvSubCategory.EditIndex = -1;
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
            string categoryName = txtSubCategoryName.Text.Trim();
            int percentage = Convert.ToInt32(txtPercentage.Text.Trim());
            long classId = Convert.ToInt64(ddlYear.SelectedValue);
            long sessionId = Convert.ToInt64(ddlSession.SelectedValue);
            long termId = Convert.ToInt64(ddlTerm.SelectedValue);

            IList<ScoreSubCategoryConfiguration> getCategory = new SchoolLIB().GetScoreSubCategoryConfiguration((long)logonUser.SchoolId, (long)logonUser.SchoolCampusId, classId, sessionId, termId, Convert.ToInt64(ddlCategory.SelectedValue), categoryName); //work on the ddlyear as the last parameter inside getconfiguration
            if (getCategory != null && getCategory.Count > 0)
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
                    int? totalPercent = (from s in context.ScoreSubCategoryConfigurations where s.ScoreCategoryConfiguration.SchoolId == logonUser.SchoolId && s.ScoreCategoryConfiguration.CampusId == logonUser.SchoolCampusId && s.ScoreCategoryConfiguration.ClassId == classId && s.SessionId == sessionId && s.TermId == termId && s.CategoryId == Convert.ToInt64(ddlCategory.SelectedValue) select s).Sum(x => x.Percentage);
                    if (totalPercent < 100 || totalPercent == null || percentage == 0)
                    {
                        ScoreSubCategoryConfiguration newConfig = new ScoreSubCategoryConfiguration();
                        newConfig.CategoryId = Convert.ToInt64(ddlCategory.SelectedValue);
                        newConfig.SubCategory = txtSubCategoryName.Text;
                        newConfig.Percentage = Convert.ToInt16(txtPercentage.Text);
                        newConfig.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
                        newConfig.SessionId = Convert.ToInt64(ddlSession.SelectedValue);
                        newConfig.TermId = Convert.ToInt64(ddlTerm.SelectedValue);
                        newConfig.Date = DateTime.Now;
                        if (totalPercent != null)
                        {
                            if (totalPercent + newConfig.Percentage <= 100)
                            {
                                new SchoolLIB().SaveScoreSubCategoryConfiguration(newConfig);
                                lblErrorMsg.Text = "Configuration saved successfully";
                                lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                                lblErrorMsg.Visible = true;
                                txtSubCategoryName.Text = "";
                                txtPercentage.Text = "";
                                ddlCategory.SelectedIndex = 0;
                                ddlYear.SelectedIndex = 0;
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
                            new SchoolLIB().SaveScoreSubCategoryConfiguration(newConfig);
                            lblErrorMsg.Text = "Configuration saved successfully";
                            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                            lblErrorMsg.Visible = true;
                            txtSubCategoryName.Text = "";
                            txtPercentage.Text = "";
                            ddlCategory.SelectedIndex = 0;
                            ddlYear.SelectedIndex = 0;
                            BindScore();
                        }
                    }
                    else
                    {
                        lblErrorMsg.Text = "You can't have more than 100% for a class";
                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                        lblErrorMsg.Visible = true;
                        return;
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
        ddlCategory.Items.Clear();
        long yearId = Convert.ToInt64(ddlYear.SelectedValue);
        var categoryList = from s in context.ScoreCategoryConfigurations where s.ClassId == yearId && s.SchoolId == logonUser.SchoolId && s.CampusId == logonUser.SchoolCampusId && s.SessionId == Convert.ToInt64(ddlSession.SelectedValue) && s.TermId == Convert.ToInt64(ddlTerm.SelectedValue) select s;
        ddlCategory.DataSource = categoryList;
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, new ListItem("--Select--", "0", true));
    }

    public string getClassName(object ClassId)
    {
        ClassGradeLIB cgl = new ClassGradeLIB();
        string theName = cgl.RetrieveClass_Grade(Convert.ToInt32(ClassId)).Name.ToString();
        return theName;
    }
    protected void gvSubCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSubCategory.PageIndex = e.NewPageIndex;
        BindScore();
    }
    protected void gvSubCategory_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvSubCategory.EditIndex = e.NewEditIndex;
        BindScore();
        txtSubCategoryName.Text = "CA";
        txtPercentage.Text = "0";
       
    }
    protected void gvSubCategory_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            lblErrorMsg.Visible = false;
            Label lblId = (Label)gvSubCategory.Rows[e.RowIndex].FindControl("lblId");
            Label CategoryId = (Label)gvSubCategory.Rows[e.RowIndex].FindControl("CategoryId");
            TextBox txtGvClass = (TextBox)gvSubCategory.Rows[e.RowIndex].FindControl("txtClass");
            TextBox txtGvSession = (TextBox)gvSubCategory.Rows[e.RowIndex].FindControl("txtSession");
            TextBox txtGvTerm = (TextBox)gvSubCategory.Rows[e.RowIndex].FindControl("txtTerm");
            TextBox txtGvSubCategory = (TextBox)gvSubCategory.Rows[e.RowIndex].FindControl("txtSubCategory");
            TextBox txtGvPercentage = (TextBox)gvSubCategory.Rows[e.RowIndex].FindControl("txtPercentage");
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            PASSIS.LIB.Class_Grade classGrade = context.Class_Grades.FirstOrDefault(x => x.Name == txtGvClass.Text.Trim());
            PASSIS.LIB.AcademicSessionName session = context.AcademicSessionNames.FirstOrDefault(x => x.SessionName == txtGvSession.Text.Trim());
            PASSIS.LIB.AcademicTerm1 term = context.AcademicTerm1s.FirstOrDefault(x => x.AcademicTermName == txtGvTerm.Text.Trim());
            PASSIS.LIB.ScoreSubCategoryConfiguration objScoreSubCategoryConfig = context.ScoreSubCategoryConfigurations.FirstOrDefault(x => x.Id == Convert.ToInt64(lblId.Text));
            int? totalPercent = (from s in context.ScoreSubCategoryConfigurations where s.ScoreCategoryConfiguration.SchoolId == logonUser.SchoolId && s.ScoreCategoryConfiguration.CampusId == logonUser.SchoolCampusId && s.ScoreCategoryConfiguration.ClassId == classGrade.Id && s.SessionId == session.ID && s.TermId == term.Id && s.CategoryId == Convert.ToInt64(CategoryId.Text) select s).Sum(x => x.Percentage);
            string category = txtGvSubCategory.Text.Trim();
            string percentage = txtGvPercentage.Text.Trim();

            int? oldPercentage = objScoreSubCategoryConfig.Percentage;
            int? presentTotalPercentage = totalPercent - oldPercentage;
            //int availableTotalPercentage = Convert.ToInt16(totalPercent) - Convert.ToInt16(presentTotalPercentage);
            int availablePercentage = 100 - Convert.ToInt16(presentTotalPercentage);

            objScoreSubCategoryConfig.SubCategory = category;
            objScoreSubCategoryConfig.Percentage = Convert.ToInt16(percentage);
            if (objScoreSubCategoryConfig.Percentage <= availablePercentage)
            {
                context.SubmitChanges();
                gvSubCategory.EditIndex = -1;
                BindScore();
                lblMessage.Text = "Updated Successfully";
                lblMessage.ForeColor = System.Drawing.Color.Green;
                txtGvSubCategory.Text = "";
                txtGvPercentage.Text = "";
                txtSubCategoryName.Text = "";
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

    public string GetCategory(long CategoryId)
    {
        string categoryName = "";
        ScoreCategoryConfiguration SCC = context.ScoreCategoryConfigurations.FirstOrDefault(x => x.Id == CategoryId);
        if (SCC != null)
        {
            categoryName = SCC.Category;
        }
        return categoryName;
    }
    public IList<AcademicSessionName> schSession()
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        var session = from c in context.AcademicSessions
                      where c.SchoolId == logonUser.SchoolId
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