using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO;
using PASSIS.LIB;
using PASSIS.DAO.Utility;

public partial class CreateSupervisor : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected string classSubjectStrings_VS
    {
        get
        {
            if (ViewState["::classSubjectStrings_VS::"] == null)
                return string.Empty;
            else
                return ViewState["::classSubjectStrings_VS::"] as string;
        }
        set
        {
            ViewState["::classSubjectStrings_VS::"] = value;
        }
    }
    protected string classSubjectStringsTemp_VS
    {
        get
        {
            if (ViewState["::classSubjectStringsTemp_VS::"] == null)
                return string.Empty;
            else
                return ViewState["::classSubjectStringsTemp_VS::"] as string;
        }
        set
        {
            ViewState["::classSubjectStringsTemp_VS::"] = value;
        }
    }
    protected Int64 itemId
    {
        get
        {
            return Convert.ToInt64(ViewState["::itemId::"]);
        }
        set
        {
            ViewState["::itemId::"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
       // BindGrid2();
        //lblErrorMsg.Text = "";
        if (!IsPostBack)
        {
                      
            long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
            long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));

            //DropDownList ddlClass = (DropDownList)fvwUser.FindControl("ddlClass");
            ddlClass.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("--Select Class--", "0"));

           // DropDownList ddlSupervisor = (DropDownList)fvwUser.FindControl("ddlSupervisor");
            ddlSupervisor.DataTextField = "LastName";
            ddlSupervisor.DataValueField = "Id";
            foreach (PASSIS.LIB.User user in loadSupervisor())
            {
                ListItem lii = new ListItem(user.LastName + " " + user.FirstName, "" + user.Id);
                ddlSupervisor.Items.Add(lii);
            }
            ddlSupervisor.DataBind();
            ddlSupervisor.Items.Insert(0, new ListItem("---Select---", "0", true));

        }
    }
    public string getTeacherName(object teacherIdObj)
    {
        string name = string.Empty;
        try
        {
            PASSIS.DAO.User usr = new UsersDAL().RetrieveUser(Convert.ToInt64(teacherIdObj));
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
            PASSIS.DAO.User usr = new UsersDAL().RetrieveUser(Convert.ToInt64(supervisorIdObj));
            name = string.Format("{0} {1} {2}", usr.FirstName, usr.LastName, usr.MiddleName);
        }
        catch { }
        return name;
    }
    protected void btnAddSupervisor_Click(object sender, EventArgs e)
    {
        //DropDownList ddlClass = (DropDownList)fvwUser.FindControl("ddlClass");
        //DropDownList ddlGrade = (DropDownList)fvwUser.FindControl("ddlGrade");
        //DropDownList ddlSupervisor = (DropDownList)fvwUser.FindControl("ddlSupervisor");
        //Label lblErrorMsg = (Label)fvwUser.FindControl("lblErrorMsg");

        if (ddlSupervisor.SelectedValue == "0" || ddlSupervisor.SelectedValue == "")
        {
            lblErrorMsg.Text = "Kindly select a Supervisor";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
        if (ddlClass.SelectedValue == "0" || ddlClass.SelectedValue == "")
        {
            lblErrorMsg.Text = "Kindly Select a Class";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
        if (ddlGrade.SelectedValue == "0" || ddlGrade.SelectedValue == "")
        {
            lblErrorMsg.Text = "Kindly Select a Grade";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }

        //PASSIS.LIB.Grade CheckSupervisorExists = context.Grades.FirstOrDefault(x => x.Id == Convert.ToInt64(ddlGrade.SelectedValue) && x.GradeSupervisorId != null);
        //if (CheckSupervisorExists != null)
        //{
        //    lblErrorMsg.Text = "A Supervisor has been Assigned to this Class";
        //    lblErrorMsg.Visible = true;
        //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
        //    return;
        //}

        PASSIS.LIB.Grade grade = context.Grades.FirstOrDefault(x => x.Id == Convert.ToInt64(ddlGrade.SelectedValue));
        grade.GradeSupervisorId = Convert.ToInt64(ddlSupervisor.SelectedValue);
        context.SubmitChanges();
        lblErrorMsg.Text = "Supervisor Assigned Successfully";
        lblErrorMsg.Visible = true;
        lblErrorMsg.ForeColor = System.Drawing.Color.Green;
    }

    public IList<PASSIS.LIB.User> loadSupervisor()
    {
        var allSupervisor = new UsersLIB().RetrieveUsersInRoleByCampus(17, (long)logonUser.SchoolId, (long)logonUser.SchoolCampusId).OrderBy(x => x.LastName);
        return allSupervisor.ToList<PASSIS.LIB.User>();
    }
    public void BindGrid()
    {
        //DropDownList ddlClass = (DropDownList)fvwUser.FindControl("ddlClass");
        //DropDownList ddlGrade = (DropDownList)fvwUser.FindControl("ddlGrade");

        var getGrade = from s in context.Grades where s.ClassId == Convert.ToInt64(ddlClass.SelectedValue) && s.SchoolId == logonUser.SchoolId && s.SchoolCampusId == logonUser.SchoolCampusId select s;
        ddlGrade.DataSource = getGrade.ToList();
        ddlGrade.DataTextField = "GradeName";
        ddlGrade.DataValueField = "Id";
        ddlGrade.DataBind();
        ddlGrade.Items.Insert(0, new ListItem("---Select Grade---", "0", true));
    }
    protected void gdvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //GridView gdvList = (GridView)fvwUser.FindControl("gdvList");
        gdvList.PageIndex = e.NewPageIndex;
        BindGrid2();
    }
    protected void BindGrid2()
    {
        //GridView gdvList = (GridView)fvwUser.FindControl("gdvList");
        //DropDownList ddlClass = (DropDownList)fvwUser.FindControl("ddlClass");
        gdvList.DataSource = new ClassGradeLIB().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, Convert.ToInt64(ddlClass.SelectedValue));
        gdvList.DataBind();

    }
    protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
        BindGrid2();
        lblLists.Visible = true;
    }


    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        //GridView gdvList = (GridView)fvwUser.FindControl("gdvList");
        gdvList.EditIndex = -1;
        //showgrid();
        BindGrid();

    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        //GridView gdvList = (GridView)fvwUser.FindControl("gdvList");
        gdvList.EditIndex = e.NewEditIndex;
        BindGrid();
    }
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //GridView gdvList = (GridView)fvwUser.FindControl("gdvList");
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
                //Label lblError2 = fvwUser.FindControl("lblError2") as Label;
                lblErrorMsg.Text = "No Student in this class";
                lblErrorMsg.Visible = true;
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            }
        }




        gdvList.EditIndex = -1;
        BindGrid();



    }
    public IList<PASSIS.LIB.User> LoadTeachers()
    {
        PASSISDataContext dc = new PASSISDataContext();
        var Ids = from g in dc.Grades select (long?)g.GradeTeacherId;
        List<Int64?> gradeTeachersId = Ids.ToList<Int64?>();
        IList<PASSIS.LIB.User> unAssignedTeachers = new List<PASSIS.LIB.User>();
        var allTeachers = new UsersLIB().RetrieveUsersInRoleByCampus((long)PASSIS.DAO.Utility.roles.teacher, (long)logonUser.SchoolId, (long)logonUser.SchoolCampusId).OrderBy(x => x.LastName);

        //var unAllocatedTeachers = from u in dc.Users 
        foreach (PASSIS.LIB.User u in allTeachers)
        {
            // before by ola if (!gradeTeachersId.Contains(u.Id))
            if (!gradeTeachersId.Contains(u.Id))
                unAssignedTeachers.Add(u);
        }
        //allTeachers.Where(u => !gradeTeachersId.Contains(u.Id));

        return unAssignedTeachers;

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