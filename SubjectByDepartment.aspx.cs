using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;

public partial class SubjectByDepartment : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetDepartment();
            var schlTypeId = context.Schools.FirstOrDefault(x => x.Id == (long)logonUser.SchoolId).SchoolTypeId;
            ddlYear.DataTextField = "Name";
            ddlYear.DataValueField = "Id";
            ddlYear.Items.Insert(0, new ListItem(" ---Select--- ", "0", true));
            ddlYear.DataSource = new PASSIS.LIB.ClassGradeLIB().getClassGrade(2, (long)schlTypeId);
            ddlYear.DataBind();
        }
    }
    protected void AddDepartment_Click(object sender, EventArgs e)
    {
        ddlDepartment.Items.Clear();
        if (txtNewDepartment.Text == "")
        {
            lblErrorMsg.Text = "Department Name is Empty";
            lblErrorMsg.Visible = true;
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            return;
        }
        SubjectDepartment subjectDepartment = context.SubjectDepartments.FirstOrDefault(x => x.DepartmentName == txtNewDepartment.Text.Trim() && x.SchoolId == logonUser.SchoolId);
        if (subjectDepartment == null)
        {
            SubjectDepartment newDepartment = new SubjectDepartment();
            newDepartment.DepartmentName = txtNewDepartment.Text;
            newDepartment.SchoolId = logonUser.SchoolId;
            context.SubjectDepartments.InsertOnSubmit(newDepartment);
            context.SubmitChanges();
            GetDepartment();
            txtNewDepartment.Text = "";
        }
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblSubjects.Visible = true;
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        IList<SubjectsInSchool> getId = new SubjectTeachersLIB().getAllSubjectsForClass((long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue));
        foreach (SubjectsInSchool subjId in getId)
        {
            PASSIS.LIB.Subject reqSubject = context.Subjects.FirstOrDefault(c => c.Id == subjId.SubjectId);
            //ddlSubject.Items.Add(new System.Web.UI.WebControls.ListItem(reqSubject.Name, reqSubject.Id.ToString()));

        }
        gdvAllSubject.DataSource = getId;
        gdvAllSubject.DataBind();
        gdvAllSubject.Visible = true;
    }

    public void GetDepartment()
    {
        var subjectList = from s in context.SubjectDepartments where s.SchoolId == logonUser.SchoolId select s;
        ddlDepartment.DataSource = subjectList;
        ddlDepartment.DataBind();
        ddlDepartment.Items.Insert(0, new ListItem("--Select--", "0", true));
    }

    public string subjectName(long subjectId)
    {
        PASSIS.LIB.Subject reqSubject = context.Subjects.FirstOrDefault(c => c.Id == subjectId);
        return reqSubject.Name;
    }
    public string deptName(long deptId)
    {
        SubjectDepartment reqDept = context.SubjectDepartments.FirstOrDefault(c => c.Id == deptId && c.SchoolId == logonUser.SchoolId);
        if (reqDept != null)
        {
            return reqDept.DepartmentName;
        }
        else
        {
            return string.Empty;
        }
    }
    protected void btnAddDepartment_Click(object sender, EventArgs e)
    {
        if (ddlDepartment.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select the department";
            lblErrorMsg.Visible = true;
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            return;
        }
        if (ddlYear.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select the class";
            lblErrorMsg.Visible = true;
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            return;
        }
        foreach (GridViewRow row in gdvAllSubject.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                bool isChecked = row.Cells[2].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                if (isChecked)
                {
                    string subjectId = row.Cells[3].Controls.OfType<Label>().FirstOrDefault().Text;

                    //PASSIS.LIB.Subject subject = context.Subjects.FirstOrDefault(x => x.Id == Convert.ToInt16(subjectId));
                    //if (subject != null)
                    //{
                    //    subject.DepartmentId = Convert.ToInt64(ddlDepartment.SelectedValue);
                    //    context.SubmitChanges();
                    //}
                    PASSIS.LIB.SubjectsInSchool subjectInSchool = context.SubjectsInSchools.FirstOrDefault(x => x.SubjectId == Convert.ToInt16(subjectId) && x.SchoolId == logonUser.SchoolId);
                    if (subjectInSchool != null)
                    {
                        subjectInSchool.DepartmentId = Convert.ToInt64(ddlDepartment.SelectedValue);
                        context.SubmitChanges();
                    }
                }
                var cb = row.FindControl("CheckBox1") as CheckBox;
                if (cb != null)
                    cb.Checked = false;
            }
        }
        lblErrorMsg.Text = "Department added successfully";
        lblErrorMsg.Visible = true;
        lblErrorMsg.ForeColor = System.Drawing.Color.Green;
    }
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

    public long ProcessMyDataItem(object myValue)
    {
        if (myValue == null)
        {
            return 0;
        }

        return Convert.ToInt64(myValue);
    }
}