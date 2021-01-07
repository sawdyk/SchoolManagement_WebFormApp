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

public partial class SupAdminCreateSubjects : PASSIS.LIB.Utility.BasePage
{
   
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlCurriculum.DataSource = new PASSIS.LIB.SchoolLIB().SchoolCurriculum();
            ddlCurriculum.DataBind();
            ddlCurriculum.Items.Insert(0, new ListItem("--Select Curriculum--", "0", true));
            //this.schoolType.Visible = false;
            ddlSchoolType.DataSource = new PASSIS.LIB.SchoolLIB().SchoolType();
            ddlSchoolType.DataBind();
            ddlSchoolType.Items.Insert(0, new ListItem("--Select School Type--", "0", true));
            BindGrid();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ddlCurriculum.SelectedIndex != 0 && ddlClassGrade.SelectedIndex != 0 && txtSubject.Text != "" && txtSubCode.Text != "")
        {
            int schoolTypeId = 0;
            if (ddlSchoolType.SelectedValue == "" || ddlCurriculum.SelectedValue == "1")
                schoolTypeId = 0;
            else int.TryParse(ddlSchoolType.SelectedValue, out schoolTypeId);
            clsMyDB db = new clsMyDB();
            string cond = "Name='" + txtSubject.Text + "' AND CurriculumId=" + ddlCurriculum.SelectedValue + " AND SchoolTypeId=" + schoolTypeId + " AND ClassId=" + ddlClassGrade.SelectedValue;
            if (db.exist("Subject", cond))
            {
                //report
                lblReport.Text = "Subject already exist, kindly try another.";
                lblReport.ForeColor = System.Drawing.Color.Red;
                lblReport.Visible = true;
            }
            else
            {
                string query = "INSERT INTO Subject (Name, Code, CurriculumId, SchoolTypeId,ClassId) VALUES('" + txtSubject.Text + "', '" + txtSubCode.Text + "', " + ddlCurriculum.SelectedValue + ", " + ddlSchoolType.SelectedValue + "," + ddlClassGrade.SelectedValue + ")";
                try
                {
                    PASSIS.LIB.Subject subject = new PASSIS.LIB.Subject();
                    subject.Name = txtSubject.Text;
                    subject.Code = txtSubCode.Text;
                    subject.CurriculumId = Convert.ToInt16(ddlCurriculum.SelectedValue);
                    subject.SchoolTypeId = Convert.ToInt16(ddlSchoolType.SelectedValue);
                    subject.ClassId = Convert.ToInt16(ddlClassGrade.SelectedValue);
                    context.Subjects.InsertOnSubmit(subject);
                    context.SubmitChanges();
                    if (logonUser.SchoolId != 0 || logonUser.SchoolId != null)
                    {
                        PASSIS.LIB.SubjectsInSchool subjectInSchool = new SubjectsInSchool();
                        subjectInSchool.SchoolId = logonUser.SchoolId;
                        subjectInSchool.SubjectId = subject.Id;
                        subjectInSchool.SchoolPickedSubject = 1;
                        context.SubjectsInSchools.InsertOnSubmit(subjectInSchool);
                        context.SubmitChanges();
                    }

                    //db.excQuery(query);
                    //report
                    lblReport.Text = "Subject added successfuly";
                    lblReport.ForeColor = System.Drawing.Color.Green;
                    lblReport.Visible = true;

                    txtSubject.Text = "";
                    txtSubCode.Text = "";
                    gvSubjects.DataBind();
                }
                catch (Exception ex)
                {
                    //throw ex;
                    //report
                    lblReport.Text = "Error! Subject not added, review your input data or contact your system administrator.";
                    lblReport.ForeColor = System.Drawing.Color.Red;
                    lblReport.Visible = true;
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
        BindGrid();
    }
    protected void ddlCurriculum_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCurriculum.SelectedValue == "2")
        {
            ddlClassGrade.Items.Clear();
            //schoolType.Visible = true;
        }
        else
        {
            ddlClassGrade.Items.Clear();
            ddlClassGrade.DataSource = new PASSIS.LIB.ClassGradeLIB().getClassGrade(Convert.ToInt64(ddlCurriculum.SelectedValue));
            ddlClassGrade.DataBind();
            ddlClassGrade.Items.Insert(0, new ListItem("--Select Class--", "0", true));
            //schoolType.Visible = false;
        }
    }
    protected void ddlSchoolType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSchoolType.SelectedIndex == 1)
        {
            ddlClassGrade.DataSource = new PASSIS.LIB.ClassGradeLIB().getClassGrade(Convert.ToInt64(ddlCurriculum.SelectedValue), Convert.ToInt64(ddlSchoolType.SelectedValue));
            ddlClassGrade.DataBind();
            ddlClassGrade.Items.Insert(0, new ListItem("--Select Class--", "0", true));

        }
        else if (ddlSchoolType.SelectedIndex == 2)
        {
            ddlClassGrade.DataSource = new PASSIS.LIB.ClassGradeLIB().getClassGrade(Convert.ToInt64(ddlCurriculum.SelectedValue), Convert.ToInt64(ddlSchoolType.SelectedValue));
            ddlClassGrade.DataBind();
            ddlClassGrade.Items.Insert(0, new ListItem("--Select Class--", "0", true));

        }
        else if (ddlSchoolType.SelectedIndex == 3)
        {
            ddlClassGrade.DataSource = new PASSIS.LIB.ClassGradeLIB().getClassGrade(Convert.ToInt64(ddlCurriculum.SelectedValue), Convert.ToInt64(ddlSchoolType.SelectedValue));
            ddlClassGrade.DataBind();
            ddlClassGrade.Items.Insert(0, new ListItem("--Select Class--", "0", true));

        }
        else
        {
            ddlClassGrade.Items.Clear();
        }
    }
    public void BindGrid()
    {
        gvSubjects.DataSource = new SchoolLIB().GetMasterSubjects();
        gvSubjects.DataBind();
    }
    protected void gvSubjects_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSubjects.PageIndex = e.NewPageIndex;
        BindGrid();
    }
}