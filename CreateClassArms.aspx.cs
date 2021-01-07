using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;


public partial class CreateClassArms : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblErrorMsg.Text = "";
        if (!IsPostBack)
        {
            long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
            long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
            ddlClass.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("--Select Class--", "0"));

            foreach (PASSIS.LIB.User user in LoadTeachers())
            {
                ListItem lii = new ListItem(user.LastName + " " + user.FirstName, "" + user.Id);
                ddlTeacher.Items.Add(lii);
            }
            ddlTeacher.DataBind();
            ddlTeacher.Items.Insert(0, new ListItem("---Select Teacher---", "0", true));
        }

    }

    public IList<PASSIS.LIB.User> LoadTeachers()
    {
        var Ids = from g in context.Grades select (long?)g.GradeTeacherId;
        List<Int64?> gradeTeachersId = Ids.ToList<Int64?>();
        IList<PASSIS.LIB.User> unAssignedTeachers = new List<PASSIS.LIB.User>();
        var allTeachers = new UsersLIB().RetrieveUsersInRoleByCampus((long)PASSIS.DAO.Utility.roles.teacher, (long)logonUser.SchoolId, (long)logonUser.SchoolCampusId).OrderBy(x => x.LastName);

        //var unAllocatedTeachers = from u in dc.Users 
        foreach (PASSIS.LIB.User u in allTeachers)
        {
            if (!gradeTeachersId.Contains(u.Id))
                unAssignedTeachers.Add(u);
        }

        return unAssignedTeachers;
    }

    public string getTeacherName(object teacherIdObj)
    {
        string name = string.Empty;
        try
        {
            PASSIS.LIB.User usr = new UsersLIB().RetrieveUser(Convert.ToInt64(teacherIdObj));
            name = string.Format("{0} {1} {2}", usr.FirstName, usr.LastName, usr.MiddleName);
        }
        catch { }
        return name;
    }

    public string getSupervisorName(object supervisorIdObj)
    {
        string name = string.Empty;
        try
        {
            PASSIS.LIB.User usr = new UsersLIB().RetrieveUser(Convert.ToInt64(supervisorIdObj));
            name = string.Format("{0} {1} {2}", usr.FirstName, usr.LastName, usr.MiddleName);
        }
        catch { }
        return name;
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        if (ddlTeacher.SelectedValue == "0" || ddlTeacher.SelectedValue == "")
        {
            lblErrorMsg.Text = "Kindly select a teacher ";
            lblErrorMsg.Visible = true;
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            return;
        }
        if (string.IsNullOrEmpty(txtGradeName.Text))
        {
            lblErrorMsg.Text = " Name cannot be null or empty. ";
            lblErrorMsg.Visible = true;
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            return;
        }
        
        PASSIS.LIB.Grade gd = new PASSIS.LIB.Grade();
        gd.GradeName = txtGradeName.Text.Trim();
        gd.SchoolCampusId = logonUser.SchoolCampusId;
        gd.SchoolId = (long)logonUser.SchoolId;
        gd.ClassId = Convert.ToInt64(ddlClass.SelectedValue);
        gd.GradeTeacherId = Convert.ToInt64(ddlTeacher.SelectedValue);
        gd.DateCreated = DateTime.Now;

        new ClassGradeLIB().SaveGrade(gd);
        PASSIS.LIB.UserRole teacherRole = context.UserRoles.FirstOrDefault(x => x.UserId == Convert.ToInt64(ddlTeacher.SelectedValue));
        teacherRole.RoleId = 3;
        context.SubmitChanges();

        lblErrorMsg.Text = "Class Arm Successfully Created!";
        lblErrorMsg.ForeColor = System.Drawing.Color.Green;
        lblErrorMsg.Visible = true;
        //BindGrid();
        // ddlTeacher.Text = string.Empty;

    }

    protected void BindGrid()
    {
        gdvList.DataSource = new ClassGradeLIB().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, Convert.ToInt64(ddlClass.SelectedValue));
        gdvList.DataBind();
    }
    protected void gdvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvList.PageIndex = e.NewPageIndex;
        BindGrid();
    }

    protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
        lblLists.Visible = true;
    }

    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gdvList.EditIndex = -1;
        //showgrid();
        BindGrid();

    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gdvList.EditIndex = e.NewEditIndex;
        BindGrid();
    }
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        Label lblId = (Label)gdvList.Rows[e.RowIndex].FindControl("lblId");
        DropDownList ddlTeachers = (DropDownList)gdvList.Rows[e.RowIndex].FindControl("ddlTeachers");
        DropDownList ddlSupervisors = (DropDownList)gdvList.Rows[e.RowIndex].FindControl("ddlSupervisors");
        TextBox txtGradeName = (TextBox)gdvList.Rows[e.RowIndex].FindControl("txtGradeName");
        if (string.IsNullOrEmpty(txtGradeName.Text))
            e.Cancel = true;

        Int64 gId = Convert.ToInt64(lblId.Text);
        //using (PASSISDataContext ct = new PASSISDataContext()){
        //PASSIS.DAO.Grade g = ct.Grades.FirstOrDefault(d => d.Id == gId);
        //g.GradeName = txtGradeName.Text.Trim();
        //g.GradeTeacherId = Convert.ToInt64(ddlTeachers.SelectedValue);

        //ct.SubmitChanges();
        //}

        using (PASSISLIBDataContext ct = new PASSISLIBDataContext())
        {
            PASSIS.LIB.Grade g = ct.Grades.FirstOrDefault(d => d.Id == gId);
            g.GradeName = txtGradeName.Text.Trim();
            g.GradeTeacherId = Convert.ToInt64(ddlTeachers.SelectedValue);
            g.GradeSupervisorId = Convert.ToInt64(ddlSupervisors.SelectedValue);

            PASSIS.LIB.UserRole objTeacher = ct.UserRoles.FirstOrDefault(x => x.UserId == Convert.ToInt64(ddlTeachers.SelectedValue));
            objTeacher.RoleId = 3;

            ct.SubmitChanges();
            var getGrade = ct.Grades.FirstOrDefault(s => s.GradeTeacherId == Convert.ToInt64(ddlTeachers.SelectedValue));
            var gsStudent = from s in ct.GradeStudents where s.GradeId == getGrade.Id select s;
            IList<PASSIS.LIB.GradeStudent> stdList = gsStudent.ToList();
            if (gsStudent != null)
            {
                foreach (PASSIS.LIB.GradeStudent s in stdList)
                {
                    s.GradeTeacherId = getGrade.GradeTeacherId;
                    ct.SubmitChanges();
                }
            }
            else
            {
                Label lblError2 = gdvList.FindControl("lblError2") as Label;
                lblError2.Text = "No Student in this class";
                lblError2.Visible = true;
                lblError2.ForeColor = System.Drawing.Color.Red;
            }
        }

        gdvList.EditIndex = -1;
        BindGrid();

    }


    public IList<PASSIS.DAO.User> LoadTeachers(long excemptUserId)
    {
        PASSISDataContext dc = new PASSISDataContext();
        var Ids = from g in dc.Grades where g.GradeTeacherId != excemptUserId select (long?)g.GradeTeacherId;
        List<Int64?> gradeTeachersId = Ids.ToList<Int64?>();
        IList<PASSIS.DAO.User> unAssignedTeachers = new List<PASSIS.DAO.User>();
        IList<PASSIS.DAO.User> allTeachers = new UsersDAL().RetrieveUsersInRole((long)PASSIS.DAO.Utility.roles.teacher, (long)logonUser.SchoolId);
        IList<PASSIS.DAO.User> allClassTeachers = new UsersDAL().RetrieveUsersInRole(3, (long)logonUser.SchoolId);
        if (excemptUserId > 0)
        {
            allTeachers.Add(new UsersDAL().RetrieveUser(excemptUserId));
        }

        foreach (PASSIS.DAO.User u in allTeachers)
        {
            if (!gradeTeachersId.Contains(u.Id))
                unAssignedTeachers.Add(u);
        }

        foreach (PASSIS.DAO.User u in allClassTeachers)
        {
            //if (!gradeTeachersId.Contains(u.Id))
            if (!unAssignedTeachers.Contains(u))
            {
                unAssignedTeachers.Add(u);
            }
        }
        return unAssignedTeachers.OrderBy(x => x.FullName).ToList<PASSIS.DAO.User>();

    }


    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //DataRowView drv = e.Row.DataItem as DataRowView;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList ddlTeachers = (DropDownList)e.Row.FindControl("ddlTeachers");
                DropDownList ddlSupervisor = (DropDownList)e.Row.FindControl("ddlSupervisor");
                Label lblId = (Label)e.Row.FindControl("lblId");
                Label lblGradeTeacherId = (Label)e.Row.FindControl("lblGradeTeacherId");

                ddlTeachers.DataTextField = "FullName";
                ddlTeachers.DataValueField = "Id";
                long exceptUserId = 0L;
                try { exceptUserId = Convert.ToInt64(lblGradeTeacherId.Text); }
                catch { }
                ddlTeachers.DataSource = LoadTeachers(exceptUserId);
                ddlTeachers.DataBind();
                ddlTeachers.SelectedValue = lblGradeTeacherId.Text;
               
            }

        }
    }
}
