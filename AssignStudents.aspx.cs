using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using pdc = PASSIS.LIB.CustomClasses;
using System.Data.SqlClient;
using PASSIS.LIB;
using System.Data;

public partial class AssignStudents : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            PASSIS.LIB.User currentUser = logonUser;
            ddlCampus.DataSource = ddlCampus.DataSource = new AcademicSessionLIB().GetAllSchoolCampus((long)currentUser.SchoolId);
            ddlCampus.DataBind();
            ddlCampus.DataBind();
            ddlCampus.SelectedValue = currentUser.SchoolCampusId.ToString();
            //ddlCampus.Enabled = ddlCampus.Enabled = false;

            long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
            long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));

            if (curriculumId == (long)CurriculumType.British)
            {
                ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, 0);
            }
            else if (curriculumId == (long)CurriculumType.Nigerian)
            {

               ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);

            }
            ddlYear.DataBind();
            ddlGrade.Items.Insert(0, new ListItem("--- Select Grade(Arm) ---", "0", true));
            ddlYear.Items.Insert(0, new ListItem("--- Select Year ---", "0", true));
            PopulateUnallocatedStudents();

        }

    }
    protected void PopulateUnallocatedStudents()
    {
        
        gdvUnassignedStudents.DataSource = new ClassGradeLIB().RetrieveUnallocatedStudentsByCampusID((long)logonUser.SchoolId, logonUser.SchoolCampusId);
        gdvUnassignedStudents.DataBind();

    }
    public enum CurriculumType
    {
        British = 1,
        Nigerian = 2
    }
    protected void ddlYear_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //populate the grade list,
        Int64 selectedYearId = 0L;
        selectedYearId = Convert.ToInt64(ddlYear.SelectedValue);
        ddlGrade.Items.Clear();
        var availableGrades = new ClassGradeLIB().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, selectedYearId);
        ddlGrade.DataSource = availableGrades;
        ddlGrade.DataBind();

    }

    protected void btnAssign_OnClick(object sender, EventArgs e)
    {
        if (ddlYear.SelectedValue == "0")
        {
            lblErrorMsg.Text = string.Format("Select a valid Year!");
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
        if (ddlGrade.SelectedValue == "0")
        {
            lblErrorMsg.Text = string.Format("Select a Grade!");
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
        if (ddlGrade.Items.Count == 0)
        {
            lblErrorMsg.Text = string.Format("Select a valid Grade(Arm)!");
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }

        PASSIS.LIB.AcademicSession checkSession = context.AcademicSessions.FirstOrDefault(x => x.SchoolId == logonUser.SchoolId && x.IsCurrent == true);
        if (checkSession == null)
        {
            lblErrorMsg.Text = "Kindly set the current Session!";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
        try
        {
            PASSIS.LIB.AcademicSession getSession = context.AcademicSessions.FirstOrDefault(x => x.SchoolId == logonUser.SchoolId && x.IsCurrent == true);

            foreach (GridViewRow row in gdvUnassignedStudents.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {

                    bool isChecked = row.Cells[2].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    if (isChecked)
                    {
                        string lblId = row.Cells[3].Controls.OfType<Label>().FirstOrDefault().Text;
                        long studentId = Convert.ToInt64(lblId);

                        PASSIS.LIB.GradeStudent studentExists = context.GradeStudents.FirstOrDefault(x => x.StudentId == studentId && x.AcademicSessionId == getSession.AcademicSessionId);

                        if (studentExists != null)
                        {
                            lblErrorMsg.Text = "One or more student(s) selection exists for this session!";
                            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                            lblErrorMsg.Visible = true;
                            return;
                        }
                        PASSIS.LIB.GradeStudent gs = new PASSIS.LIB.GradeStudent();
                        gs.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
                        gs.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                        gs.StudentId = studentId;
                        gs.DateCreated = DateTime.Now;
                        gs.AcademicSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);
                        gs.SchoolId = logonUser.SchoolId;
                        gs.SchoolCampusId = Convert.ToInt32(ddlCampus.SelectedValue);
                        try
                        {
                            gs.GradeTeacherId = new AcademicSessionLIB().GetGradeTeacherId(gs.GradeId);
                        }
                        catch
                        {
                            gs.GradeTeacherId = 0;
                        }
                        new ClassGradeLIB().SaveGradeStudent(gs);
                        row.Visible = false;
                       
                    }
                }
            }

            lblErrorMsg.Text = "Student assigned successfully";
            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
            lblErrorMsg.Visible = true;

        }
        catch (Exception ex)
        {

        }
    }

    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chk = (sender as CheckBox);
            if (chk.ID == "chkAll")
            {
                foreach (GridViewRow row in gdvUnassignedStudents.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        row.Cells[2].Controls.OfType<CheckBox>().FirstOrDefault().Checked = chk.Checked;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
            lblErrorMsg.Text = "Error occured, kindly contact your administrator";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
        }
    }
}

