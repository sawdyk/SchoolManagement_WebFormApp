using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;

public partial class ExportStudents : PASSIS.LIB.Utility.BasePage
{

    long CampusID, yearId, lSupport = 0L;
    string studentName, fatherName, matricNo, AdminNo = string.Empty;
    long schoolId, gradeId;
    PASSIS.LIB.PASSISLIBDataContext context = new PASSIS.LIB.PASSISLIBDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        schoolId = (long)logonUser.SchoolId;
        CampusID = logonUser.SchoolCampusId;

        if (!IsPostBack)
        {
            PASSIS.LIB.PASSISLIBDataContext context = new PASSIS.LIB.PASSISLIBDataContext();
            var schlTypeId = context.Schools.FirstOrDefault(x => x.Id == (long)logonUser.SchoolId).SchoolTypeId;
            ddlYear.DataTextField = "Name";
            ddlYear.DataValueField = "Id";

            ddlGrade.Items.Insert(0, new ListItem("--Select--", "0", true));
            ddlYear.Items.Add(new ListItem(" All ", "0", true));
            ddlCampus.DataTextField = "Name";
            ddlCampus.DataValueField = "Id";
            ddlYear.DataSource = new PASSIS.LIB.ClassGradeLIB().getClassGrade(2, (long)schlTypeId);
            ddlYear.DataBind();

            ddlCampus.DataSource = new PASSIS.LIB.AcademicSessionLIB().RetrieveUserCampus((long)logonUser.SchoolCampusId);
            ddlCampus.DataBind();
            //Util.BindToEnum(typeof(LearningSupport), ddlLearningSupport);
            BindGrid(schoolId, CampusID, yearId, gradeId, studentName, fatherName, matricNo, AdminNo, lSupport);
            lblResultSchoolCampus.Text = ddlCampus.SelectedItem.Text.Trim();

        }
    }
    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        CampusID = Convert.ToInt64(ddlCampus.SelectedValue);
        yearId = Convert.ToInt64(ddlYear.SelectedValue);
        Int64.TryParse(ddlGrade.SelectedValue, out gradeId);
        //gradeId = Convert.ToInt64(ddlGrade.SelectedValue);
        //lSupport = Convert.ToInt64(ddlLearningSupport.SelectedValue);
        //studentName = txtName.Text;
        //fatherName = txtFathersName.Text;
        //matricNo = txtMatricNumber.Text;
        //AdminNo = txtAdmNumber.Text;

        BindGrid(schoolId, CampusID, yearId, gradeId, studentName, fatherName, matricNo, AdminNo, lSupport);
        lblResultSchoolCampus.Text = ddlCampus.SelectedItem.Text.Trim();
        //lblTotalNumberofStudents.Text = students.Count.ToString();

        //usersFound = new UsersDAL().RetrieveStudents((long)logonUser.SchoolId, CampusID, yearId, (long)1.0);
        //usersFound = new UsersDAL().RetrieveStudents(

        //Response.Write(ddlYear.SelectedItem.Text);
    }

    protected void gdvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvList.PageIndex = e.NewPageIndex;
        BindGrid(schoolId, CampusID, yearId, gradeId, studentName, fatherName, matricNo, AdminNo, lSupport);
    }
    protected void BindGrid(long schoolId, long campusId, long yearId, long gradeId, string studentName, string fatherName, string matNo, string admNo, long learningSupport)
    {
        CampusID = Convert.ToInt64(ddlCampus.SelectedValue);
        yearId = Convert.ToInt64(ddlYear.SelectedValue);
        Int64.TryParse(ddlGrade.SelectedValue, out gradeId);
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

        //lSupport = Convert.ToInt64(ddlLearningSupport.SelectedValue);
        //studentName = txtName.Text;
        //fatherName = txtFathersName.Text;
        //matricNo = txtMatricNumber.Text;
        //AdminNo = txtAdmNumber.Text;

        //IList<User> students = new UsersDAL().RetrieveStudents((long)logonUser.SchoolId, logonUser.SchoolCampusId, 0, 0);
        IList<PASSIS.LIB.User> students = new PASSIS.LIB.UsersLIB().RetrieveStudents(schoolId, campusId, yearId, gradeId, studentName, fatherName, matNo, admNo, learningSupport, curSessionId);
        gdvList.DataSource = students;
        gdvList.DataBind();
        lblTotalNumberofStudents.Text = students.Count.ToString();
    }

    //protected void btnPrint_Click(object sender, EventArgs e)
    //{
    //    Response.ClearContent();
    //    Response.AppendHeader("content-disposition", "attachment; filename=" + ddlYear.SelectedItem.Text + ".xls");
    //    Response.ContentType = "application/vnd.ms-excel";
    //    System.IO.StringWriter stringWriter = new System.IO.StringWriter();
    //    HtmlTextWriter htw = new HtmlTextWriter(stringWriter);
    //    gdvList.AllowPaging = false;
    //    this.BindGrid(schoolId, CampusID, yearId, gradeId, studentName, fatherName, matricNo, AdminNo, lSupport);
    //    gdvList.RenderControl(htw);
    //    Response.Write(stringWriter.ToString());
    //    Response.End();
    //}

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            // check if any row is selected
            bool isSelected = false;
            foreach (GridViewRow i in gdvList.Rows)
            {
                CheckBox cb = (CheckBox)i.FindControl("chk");
                if (cb != null && cb.Checked == true)
                {
                    isSelected = true;
                    break;
                }
            }

            if (isSelected == true)
            {
                GridView gv = gdvList;
                //this below line is not to export the checkbox to excel
                gv.Columns[6].Visible = false;
                foreach (GridViewRow row in gdvList.Rows)
                {
                    gv.Rows[row.RowIndex].Visible = false;
                    CheckBox cb = (CheckBox)row.FindControl("chk");
                    if (cb != null && cb.Checked == true)
                    {
                        gv.Rows[row.RowIndex].Visible = true;
                    }
                }

                Response.Clear();
                Response.Buffer = true;
                Response.AppendHeader("content-disposition", "attachment; filename=" + ddlYear.SelectedItem.Text + ".xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                System.IO.StringWriter stringWriter = new System.IO.StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(stringWriter);
                gv.RenderControl(htw);
                Response.Output.Write(stringWriter.ToString());
                Response.End();
            }
        }



        catch (Exception ex)
        {

        }

    }

    public string getClassName(long studentId)
    {
        var className = context.GradeStudents.FirstOrDefault(x => x.StudentId == studentId).Class_Grade.Name;
        return className;
    }
    public string getGradeName(long studentId)
    {
        var classGrade = context.GradeStudents.FirstOrDefault(x => x.StudentId == studentId).Grade.GradeName;
        return classGrade;
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        //populate the grade list,
        Int64 selectedYearId = 0L;
        selectedYearId = Convert.ToInt64(ddlYear.SelectedValue);
        ddlGrade.Items.Clear();
        var availableGrades = new ClassGradeLIB().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, selectedYearId);
        ddlGrade.DataSource = availableGrades;
        ddlGrade.DataBind();
        ddlGrade.Items.Insert(0, new ListItem("--Select--", "0", true));

    }



    protected void OnCheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (sender as CheckBox);
        if (chk.ID == "chkAll")
        {
            foreach (GridViewRow row in gdvList.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    row.Cells[6].Controls.OfType<CheckBox>().FirstOrDefault().Checked = chk.Checked;
                }
            }
        }
        CheckBox chkAll = (gdvList.HeaderRow.FindControl("chkAll") as CheckBox);
        chkAll.Checked = true;
        foreach (GridViewRow row in gdvList.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                bool isChecked = row.Cells[6].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
              
                if (!isChecked)
                {
                    chkAll.Checked = false;
                }
            }
        }
    }


}