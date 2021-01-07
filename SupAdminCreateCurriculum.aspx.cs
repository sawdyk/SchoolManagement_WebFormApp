using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO;
using PASSIS.DAO.Utility;
using PASSIS.LIB;
public partial class SupAdminCreateCurriculum : PASSIS.LIB.Utility.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        MultiView1.SetActiveView(ViewSetupCurriculum);
        if (!IsPostBack)
        {
            ddlSchool.DataSource = new PASSIS.LIB.SchoolLIB().getAllSchools();
            ddlSchool.DataBind();
            ddlSchool.Items.Insert(0, new ListItem("--Select School--", "0", true));
            ddlCurriculum.DataSource = new SchoolLIB().SchoolCurriculum();
            ddlCurriculum.DataBind();
            ddlCurriculum.Items.Insert(0, new ListItem("--Select Curriculum--", "0", true));
            ddlOptionalSubjects.Items.Insert(0, new ListItem("--Select--", "-1", true));
            ddlOptionalSubjects.Items.Insert(1, new ListItem("YES", "1", true));
            ddlOptionalSubjects.Items.Insert(2, new ListItem("NO", "0", true));
        }
    }

    protected void ddlCurriculum_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblList.Visible = true;
        if (ddlCurriculum.SelectedValue == "1")
        {
            long schoolTypeId = new SchoolLIB().GetSchoolTypeId(Convert.ToInt64(ddlSchool.SelectedValue));
            gdvCurriulumSubjectsList.DataSource = new SchoolLIB().MasterSubjects(Convert.ToInt64(ddlCurriculum.SelectedValue), schoolTypeId);
            gdvCurriulumSubjectsList.DataBind();
            btnSaveSchoolSubjects.Visible = true;
        }
        else if (ddlCurriculum.SelectedValue == "2")
        {
            long schoolTypeId = new SchoolLIB().GetSchoolTypeId(Convert.ToInt64(ddlSchool.SelectedValue));
            gdvCurriulumSubjectsList.DataSource = new SchoolLIB().MasterSubjects(Convert.ToInt64(ddlCurriculum.SelectedValue), schoolTypeId);
            gdvCurriulumSubjectsList.DataBind();
            btnSaveSchoolSubjects.Visible = true;
        }
    }
    protected void btnSaveSchoolSubjects_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gdvCurriulumSubjectsList.Rows)
        {
            CheckBox chkSelectSubject = row.FindControl("SubjectCheckbox") as CheckBox;
            Label lblSubjectId = row.FindControl("lblId") as Label;
            Int64 Id = Convert.ToInt64(lblSubjectId.Text.Trim());

            if (chkSelectSubject.Checked)
            {
                long id = Id;
                if (new SchoolLIB().subjectInSchoolExist(id, Convert.ToInt64(ddlSchool.SelectedValue)))
                {
                    //Do nothing because the subject all aready exist
                }
                else
                {
                    SubjectsInSchool newSubject = new SubjectsInSchool();
                    newSubject.SubjectId = Convert.ToInt32(Id);
                    newSubject.SchoolId = Convert.ToInt32(ddlSchool.SelectedValue);
                    newSubject.SchoolPickedSubject = 1; //One is use to denote all the subjects under the selected curriculum a user picked
                    if (new SchoolLIB().subjectInSchoolExist(Convert.ToInt32(Id), Convert.ToInt32(ddlSchool.SelectedValue)))
                    {
                        //Subject exist, don't save
                    }
                    else
                    {
                        new SchoolLIB().SaveSchoolCurriculum(newSubject);
                        chkSelectSubject.Checked = false;
                    }
                }
            }

        }

        PASSISLIBDataContext context = new PASSISLIBDataContext();
        PASSIS.LIB.School objSchool = context.Schools.FirstOrDefault(x => x.Id == Convert.ToInt64(ddlSchool.SelectedValue));
        objSchool.CurriculumId = Convert.ToInt32(ddlCurriculum.SelectedValue);
        context.SubmitChanges();

        MultiView1.SetActiveView(ViewOptionalCurriculum);
        if (ddlCurriculum.SelectedIndex == 1)
        {
            lblOptionalSubjects.Text = "Are you offering part of Nigeria curriculum?";
        }
        else if (ddlCurriculum.SelectedIndex == 2)
        {
            lblOptionalSubjects.Text = "Are you offering part of British curriculum?";
        }
    }
    protected void ddlOptionalSubjects_SelectedIndexChanged(object sender, EventArgs e)
    {
        long schoolTypeId = new SchoolLIB().GetSchoolTypeId(Convert.ToInt64(ddlSchool.SelectedValue));
        if (ddlOptionalSubjects.SelectedIndex == 1)
        {
            if (ddlCurriculum.SelectedIndex == 1)
            {
                gdvOptionalSubjects.DataSource = new SchoolLIB().OptionalSubjects(Convert.ToInt64(2), schoolTypeId);
                gdvOptionalSubjects.DataBind();
                gdvOptionalSubjects.Visible = true;
                btnSaveOptionalSubjects.Visible = true;
                MultiView1.SetActiveView(ViewOptionalCurriculum);

            }
            else if (ddlCurriculum.SelectedIndex == 2)
            {
                gdvOptionalSubjects.DataSource = new SchoolLIB().OptionalSubjects(Convert.ToInt64(1), schoolTypeId);
                gdvOptionalSubjects.DataBind();
                gdvOptionalSubjects.Visible = true;
                btnSaveOptionalSubjects.Visible = true;
                MultiView1.SetActiveView(ViewOptionalCurriculum);

            }
        }
        else
        {
            MultiView1.SetActiveView(ViewSummary);
            lblMessage.Text = "Curriculum setup completed successfully";
            lblMessage.ForeColor = System.Drawing.Color.Green;
        }
    }
    protected void btnSaveOptionalSubjects_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gdvOptionalSubjects.Rows)
        {
            CheckBox chkSelectSubject = row.FindControl("SubjectCheckboxOptional") as CheckBox;
            Label lblSubjectId = row.FindControl("lblIdOptional") as Label;
            Int64 Id = Convert.ToInt64(lblSubjectId.Text.Trim());

            if (chkSelectSubject.Checked)
            {
                long id = Id;
                SubjectsInSchool newSubject = new SubjectsInSchool();
                newSubject.SubjectId = Convert.ToInt32(Id);
                newSubject.SchoolId = Convert.ToInt32(ddlSchool.SelectedValue);
                newSubject.SchoolPickedSubject = 0; //Zero is use to denote optional subjects in the database
                new SchoolLIB().SaveSchoolCurriculum(newSubject);
                chkSelectSubject.Checked = false;
            }

        }

        MultiView1.SetActiveView(ViewSummary);
        lblMessage.Text = "Setup completed successfully";
        lblMessage.ForeColor = System.Drawing.Color.Green;

    }

    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chk = (sender as CheckBox);
            if (chk.ID == "chkAll")
            {
                foreach (GridViewRow row in gdvCurriulumSubjectsList.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        row.Cells[4].Controls.OfType<CheckBox>().FirstOrDefault().Checked = chk.Checked;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
            lblMessage.Text = "Error occured, kindly contact your administrator";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Visible = true;
        }
    }
}