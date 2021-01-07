using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using PASSIS.LIB;
using System.Collections.Generic;

public partial class ScoreGradeConfig : PASSIS.LIB.Utility.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
            if (!IsPostBack)
            {
                long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
                long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
                ddlClass.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
                ddlClass.DataBind();

                BindScore();
            }
        }

    public void BindScore()
    {
        gvScoreGrades.DataSource = new SchoolLIB().GetScoreGradeConfiguration((long)logonUser.SchoolId);
        gvScoreGrades.DataBind();
    }

    protected void gvScoreGrades_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lblErrorMsg.Visible = false;
        gvScoreGrades.EditIndex = -1;
        BindScore();
    }
   
    public IList<ScoreGradeConfiguration> GetScoreGradeConfiguration(Int64 schoolId, Int64 classId, string grade, string remark)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        var getConfig = from r in context.ScoreGradeConfigurations
                        where r.SchoolId == schoolId && r.ClassId == classId && r.Grade == grade && r.Remark == remark
                        select r;
        return getConfig.ToList<ScoreGradeConfiguration>();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int lowestRange = Convert.ToInt32(txtLowestRange.Text.Trim());
            int highestRange = Convert.ToInt32(txtHighestRange.Text.Trim());
            string grade = txtGrade.Text.Trim().ToUpper();
            string remark = txtRemark.Text.Trim().ToUpper();

            IList<ScoreGradeConfiguration> getScore = new ScoreGradeConfig().GetScoreGradeConfiguration((long)logonUser.SchoolId, Convert.ToInt64(ddlClass.SelectedValue), grade, remark); //work on the ddlyear as the last parameter inside getconfiguration
            if (getScore.Count > 0)
            {
                lblErrorMsg.Text = "Configuration already exist, kindly use panel below to edit";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            else
            {
                ScoreGradeConfiguration newConfig = new ScoreGradeConfiguration();
                //newConfig.CampusId = logonUser.SchoolCampusId;
                newConfig.SchoolId = (long)logonUser.SchoolId;
                newConfig.LowestRange = Convert.ToInt64(lowestRange);
                newConfig.HighestRange = Convert.ToInt64(highestRange);
                newConfig.Grade = grade;
                newConfig.Remark = remark;
                newConfig.ClassId = Convert.ToInt64(ddlClass.SelectedValue);
                new SchoolLIB().SaveScoreGradeConfiguration(newConfig);
                lblErrorMsg.Text = "Configuration saved successfully";
                lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                lblErrorMsg.Visible = true;
                txtLowestRange.Text = "";
                txtHighestRange.Text = "";
                txtGrade.Text = "";
                txtRemark.Text = "";
                gvScoreGrades.DataSource = new SchoolLIB().GetScoreGradeConfiguration((long)logonUser.SchoolId);
                gvScoreGrades.DataBind();
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
    public string getClassName(object ClassId)
    {
        ClassGradeLIB cgl = new ClassGradeLIB();
        string theName = cgl.RetrieveClass_Grade(Convert.ToInt32(ClassId)).Name.ToString();
        return theName;
    }
    protected void gvScoreGrades_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvScoreGrades.PageIndex = e.NewPageIndex;
        BindScore();
    }
    protected void gvScoreGrades_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvScoreGrades.EditIndex = e.NewEditIndex;
        BindScore();
        txtHighestRange.Text = "0";
        txtLowestRange.Text = "0";
        txtGrade.Text = "A";
        txtRemark.Text = "Good";
    }
    protected void gvScoreGrades_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            lblErrorMsg.Visible = false;
            Label lblId = (Label)gvScoreGrades.Rows[e.RowIndex].FindControl("lblId");
            TextBox txtGvLowestRange = (TextBox)gvScoreGrades.Rows[e.RowIndex].FindControl("txtLowestRange");
            TextBox txtGvHighestRange = (TextBox)gvScoreGrades.Rows[e.RowIndex].FindControl("txtHighestRange");
            TextBox txtGvGrade = (TextBox)gvScoreGrades.Rows[e.RowIndex].FindControl("txtGrade");
            TextBox txtGvRemark = (TextBox)gvScoreGrades.Rows[e.RowIndex].FindControl("txtRemark");
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            PASSIS.LIB.ScoreGradeConfiguration objScoreGradeConfig = context.ScoreGradeConfigurations.FirstOrDefault(x => x.Id == Convert.ToInt64(lblId.Text));
            int lowestRange = Convert.ToInt32(txtGvLowestRange.Text.Trim());
            int highestRange = Convert.ToInt32(txtGvHighestRange.Text.Trim());
            string grade = txtGvGrade.Text.Trim().ToUpper();
            string remark = txtGvRemark.Text.Trim().ToUpper();

            objScoreGradeConfig.LowestRange = Convert.ToInt64(lowestRange);
            objScoreGradeConfig.HighestRange = Convert.ToInt64(highestRange);
            objScoreGradeConfig.Grade = grade;
            objScoreGradeConfig.Remark = remark;
            context.SubmitChanges();
            gvScoreGrades.EditIndex = -1;
            BindScore();
            lblMessage.Text = "Updated Successfully";
            lblMessage.ForeColor = System.Drawing.Color.Green;
            txtLowestRange.Text = "";
            txtHighestRange.Text = "";
            txtGrade.Text = "";
            txtRemark.Text = "";

        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error occurred, try again";
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }


}