using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using pdc = PASSIS.DAO.CustomClasses;
using PASSIS.LIB;
using PASSIS.LIB.Utility;

public partial class DeleteSubjectsInSchool : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindSchoolSubjetcs((long)logonUser.SchoolId);
        }
    }

    //protected void BindGridYear()
    //{
    //    long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
    //    long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
    //    ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
    //    ddlYear.DataBind();

    //}
    //protected void PopulateallSubject()
    //{
    //    PASSISLIBDataContext db = new PASSISLIBDataContext();
        
    //    PASSIS.LIB.Class_Grade classgrade = db.Class_Grades.FirstOrDefault(x => x.Name == ddlYear.SelectedItem.Text);
    //    gdvAllSubject.DataSource = new TeacherLIB().GetAllSubjectInClass(Convert.ToInt32(classgrade.CurriculumId), classgrade.Id, Convert.ToInt64(logonUser.SchoolId));
    //    gdvAllSubject.DataBind();
    //    gdvAllSubject.Visible = true;
    //}
    //protected void ddlYear_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
        
    //    IList<PASSIS.LIB.Grade> gradeList = new ClassGradeLIB().getAllGradesByYear(Convert.ToInt64(logonUser.SchoolId), logonUser.SchoolCampusId, Convert.ToInt64(ddlYear.SelectedValue));
        
    //    ddlGrade.DataSource = gradeList;
    //    ddlGrade.DataBind();

    //    PopulateallSubject();
    //    lblunAssigned.Visible = true;

    //}

    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
       
        try
        {
            CheckBox chk = (sender as CheckBox);
            if (chk.ID == "chkAll")
            {
                foreach (GridViewRow row in gdvAllSubject.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        row.Cells[3].Controls.OfType<CheckBox>().FirstOrDefault().Checked = chk.Checked;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
            lblSubjectSelectionError.Text = "Error occured, kindly contact your administrator";
            lblSubjectSelectionError.ForeColor = System.Drawing.Color.Red;
            lblSubjectSelectionError.Visible = true;
        }
    }


    protected void BindSchoolSubjetcs(long schoolId)
    {
        var allSchSubjects = from sub in context.SubjectsInSchools where sub.SchoolId == schoolId
                             select new
                             {
                                 sub.SubjectId,
                                 sub.Subject.Name,
                                Class = sub.Subject.Class_Grade.Name
                             };
        gdvAllSubject.DataSource = allSchSubjects.ToList();
        gdvAllSubject.DataBind();
    }

    protected void gdvAllSubject_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvAllSubject.PageIndex = e.NewPageIndex;
        BindSchoolSubjetcs((long)logonUser.SchoolId);
    }
    //protected void gdvAllSubject_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    switch (e.CommandName)
    //    {
    //        case "Remove":
    //            long subjetcId = Convert.ToInt64(e.CommandArgument.ToString());

    //            SubjectsInSchool subj = context.SubjectsInSchools.FirstOrDefault(s => s.SubjectId == subjetcId);
    //            context.SubjectsInSchools.DeleteOnSubmit(subj);
    //            context.SubmitChanges();


    //            lblSubjectSelectionError.Visible = true;
    //            lblSubjectSelectionError.ForeColor = System.Drawing.Color.Red;
    //            lblSubjectSelectionError.Text = "Subject Removed Successfully";
    //            BindSchoolSubjetcs((long)logonUser.SchoolId);
    //            break;
    //    }
    //}

    protected void btnDeleteSubject_OnClick(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow row in gdvAllSubject.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    bool isChecked = row.Cells[3].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    if (isChecked)
                    {
                        string lblId = row.Cells[4].Controls.OfType<Label>().FirstOrDefault().Text;
                        long subjectId = Convert.ToInt64(lblId);

                        SubjectsInSchool subj = context.SubjectsInSchools.FirstOrDefault(s => s.SubjectId == subjectId && s.SchoolId == logonUser.SchoolId);
                        context.SubjectsInSchools.DeleteOnSubmit(subj);
                        context.SubmitChanges();
                        BindSchoolSubjetcs((long)logonUser.SchoolId);
                    }
                }
            }
            lblErrorMsg.Text = "Subject deleted successfully";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;

        }
        catch (Exception ex)
        {

        }
    }
}