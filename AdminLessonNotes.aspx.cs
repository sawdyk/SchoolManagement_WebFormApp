using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO;
using PASSIS.DAO.Utility;
using System.Data.SqlTypes;
using PASSIS.LIB;
using System.Data.SqlClient;

public partial class AdminLessonNotes : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    long campusId = 0;
    long yearId = 0;
    long sessionId = 0;
    long termId = 0;
    long schoolId = 0;
    string lNStatus = "";
   
    protected void Page_Load(object sender, EventArgs e)
    {
        //schoolId = (long)logonUser.SchoolId;
        if (!IsPostBack)
        {



            int SessionId = 0;
            clsMyDB mdb = new clsMyDB();
            mdb.connct();
            string query = "SELECT DISTINCT AcademicSessionId FROM AcademicSession WHERE SchoolId=" + logonUser.SchoolId;
            SqlDataReader reader = mdb.fetch(query);
            while (reader.Read())
            {
                ddlSession.DataSource = from s in context.AcademicSessionNames
                                        where s.ID == Convert.ToInt64(reader["AcademicSessionId"].ToString())
                                        select s;
                ddlSession.DataBind();

            }
            ddlSession.Items.Insert(0, new ListItem("--Select--", "0", true));
            reader.Close();
            mdb.closeConnct();

            long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
            long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
            ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
            ddlYear.DataBind();
            ddlYear.Items.Insert(0, new ListItem("--Select--", "0", true));

            ddlTerm.DataSource = new AcademicTermLIB().RetrieveAcademicTerm();
            ddlTerm.DataBind();
            ddlTerm.Items.Insert(0, new ListItem("--Select--", "0", true));


            ddlCampus.DataSource = new AcademicSessionDAL().RetrieveCampusInSchool((long)logonUser.SchoolId);
            ddlCampus.DataBind();
            ddlCampus.Items.Insert(0, new ListItem("--Select--", "0", true));
            //if (logonUserRole.Id == (long)roles.teacher)
            //{
            //    pnlCreateUser.Visible = false;
            //}
            schoolId = Convert.ToInt64(logonUser.SchoolId);
            BindGrid(new PASSIS.LIB.LessonNoteLIB().RetrieveLessonNote(schoolId, campusId, yearId, sessionId, termId));
        }
        campusId = Convert.ToInt64(ddlCampus.SelectedValue);
        yearId = Convert.ToInt64(ddlYear.SelectedValue);
        sessionId = Convert.ToInt64(ddlSession.SelectedValue);
        termId = Convert.ToInt64(ddlTerm.SelectedValue);
    }
    protected void objRole_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["parentId"] = 2;
    }
    protected void btnSearchStaff_OnClick(object sender, EventArgs e)
    {
        //BindGrid(txtUsername.Text.Trim());
    }

    public string getSchoolName(object objSchlId)
    {
        Int64 schId = 0;
        string schName = "";
        try
        {
            schId = Convert.ToInt32(objSchlId);
        }
        catch { }
        if (schId != 0)
        {
            schName = new PASSIS.DAO.SchoolConfigDAL().RetrieveSchool(schId).Name;
        }
        return schName;

    }
    public string getCampusName(object objSchlId)
    {
        Int64 schId = 0;
        string schName = "";
        try
        {
            schId = Convert.ToInt32(objSchlId);
        }
        catch { }
        if (schId != 0)
        {
            schName = new PASSIS.DAO.AcademicSessionDAL().RetrieveSchoolCampus(schId).Name;
        }
        return schName;

    }
    protected void gdvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvList.PageIndex = e.NewPageIndex;
        BindGrid(new PASSIS.LIB.LessonNoteLIB().RetrieveLessonNote(schoolId, campusId, yearId, sessionId, termId));
    }
    protected void gdvList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "download")
        {
            string filename = e.CommandArgument.ToString();
            string path = MapPath("~/docs/LessonNote/" + filename);
            byte[] bts = System.IO.File.ReadAllBytes(path);
            Response.Clear();
            Response.ClearHeaders();
            Response.AddHeader("Content-Type", "Application/msword");
            Response.AddHeader("Content-Length", bts.Length.ToString());
            Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
            Response.BinaryWrite(bts);
            Response.Flush();
            Response.End();
        }
        //if (e.CommandName == "remove")
        //{
        //    Int64 Id = Convert.ToInt64(e.CommandArgument);
        //    PASSISLIBDataContext ct = new PASSISLIBDataContext();
        //    PASSIS.LIB.LessonNote lnote = new PASSIS.LIB.LessonNote();
        //    lnote = ct.LessonNotes.FirstOrDefault(s => s.Id == Id);
        //    ct.LessonNotes.DeleteOnSubmit(lnote);
        //    ct.SubmitChanges();
        //    BindGrid(new PASSIS.LIB.LessonNoteLIB().RetrieveLessonNote(schoolId, campusId, yearId, sessionId, termId));
        //    lblErrorMsg.Text = "Lesson Note removed successfully";
        //    lblErrorMsg.Visible = true;
        //    lblErrorMsg.ForeColor = System.Drawing.Color.Green;

        //}
        if (e.CommandName == "approve")
        {
            Int64 Id = Convert.ToInt64(e.CommandArgument);
            PASSISLIBDataContext ct = new PASSISLIBDataContext();
            PASSIS.LIB.LessonNote lnote = new PASSIS.LIB.LessonNote();
            lnote = ct.LessonNotes.FirstOrDefault(s => s.Id == Id);
            lnote.Status = 1;
            //ct.LessonNotes.InsertOnSubmit(lnote);
            ct.SubmitChanges();
            lblErrorMsg.Text = "Lesson Note approved successfully";
            lblErrorMsg.Visible = true;
            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
            schoolId = Convert.ToInt64(logonUser.SchoolId);
            BindGrid(new PASSIS.LIB.LessonNoteLIB().RetrieveLessonNote(schoolId, campusId, yearId, sessionId, termId));

        }
        if (e.CommandName == "decline")
        {
            Int64 Id = Convert.ToInt64(e.CommandArgument);
            PASSISLIBDataContext ct = new PASSISLIBDataContext();
            PASSIS.LIB.LessonNote lnote = new PASSIS.LIB.LessonNote();
            lnote = ct.LessonNotes.FirstOrDefault(s => s.Id == Id);
            lnote.Status = 2;
            //ct.LessonNotes.InsertOnSubmit(lnote);
            ct.SubmitChanges();
            lblErrorMsg.Text = "Lesson Note declined successfully";
            lblErrorMsg.Visible = true;
            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
            schoolId = Convert.ToInt64(logonUser.SchoolId);
            BindGrid(new PASSIS.LIB.LessonNoteLIB().RetrieveLessonNote(schoolId, campusId, yearId, sessionId, termId));
        }
    }

    protected void BindGrid(IList<PASSIS.LIB.LessonNote> lessonNotes)
    {
        gdvList.DataSource = lessonNotes;
        gdvList.DataBind();

    }

    public PASSIS.LIB.Class_Grade getYear(Int64 yearId)
    {
        PASSIS.LIB.Class_Grade year = context.Class_Grades.FirstOrDefault(x => x.Id == yearId);
        return year;
    }
    public string getGrade(Int64 gradeId)
    {
        string gradeName = string.Empty;
        PASSIS.LIB.Grade grade = context.Grades.FirstOrDefault(x => x.Id == gradeId);

        if (grade != null)
        {
            gradeName = grade.GradeName;
        }
        return gradeName;
    }

    public PASSIS.LIB.AcademicSessionName getSession(Int64 sessionId)
    {
        PASSIS.LIB.AcademicSessionName session = context.AcademicSessionNames.FirstOrDefault(x => x.ID == sessionId);
        return session;
    }

    public PASSIS.LIB.AcademicTerm1 getTerm(Int64 termId)
    {
        PASSIS.LIB.AcademicTerm1 term = context.AcademicTerm1s.FirstOrDefault(x => x.Id == termId);
        return term;
    }

    public string getLNStatus(long status)
    {

        if (status == 0)
        {
            lNStatus = "Awaiting Approval";
        }
        else if (status == 1)
        {
            lNStatus = "Approved";
        }
        else if (status == 2)
        {
            lNStatus = "Declined";
        }


        return lNStatus;
    }

    public PASSIS.LIB.User getTeacher(Int64 teacherId)
    {
        PASSIS.LIB.User teacher = context.Users.FirstOrDefault(x => x.Id == teacherId);
        return teacher;
    }
    //protected void btnSave_OnClick(object sender, EventArgs e)
    //{
    //    //validate the value and save
    //    try
    //    {
    //        string username = txtUsername.Text.Trim().ToLower();
    //        if (string.IsNullOrEmpty(username))
    //        {
    //            lblErrorMsg.Text = string.Format("Username field cannot be null");
    //            lblErrorMsg.Visible = true;
    //            return;
    //        }
    //        if (new UsersDAL().RetrieveUser(username) != null)
    //        {
    //            lblErrorMsg.Text = string.Format("Username already exist. Try a more unique ");
    //            lblErrorMsg.Visible = true;
    //            return;
    //        }
    //        if (ddlRole.SelectedValue == "0")
    //        {
    //            lblErrorMsg.Text = string.Format("Select a role for the new user. ");
    //            lblErrorMsg.Visible = true;
    //            return;
    //        }
    //        Int32 selectedRole = 0;

    //        selectedRole = Convert.ToInt32(ddlRole.SelectedValue);
    //        UsersDAL usrDal = new UsersDAL();

    //        PASSIS.DAO.User usr = new PASSIS.DAO.User();
    //        usr.EmailAddress = txtEmailAddress.Text.Trim();
    //        usr.FirstName = txtFirstname.Text.Trim();
    //        usr.LastLoginDate = DateTime.Now;
    //        usr.LastName = txtLastname.Text.Trim();
    //        usr.DateOfBirth = (DateTime)SqlDateTime.MinValue;
    //        usr.Password = "password";
    //        usr.PhoneNumber = txtPhoneNumber.Text.Trim();
    //        //usr.SchoolId = Convert.ToInt64(ddlSchool.SelectedValue.Trim());
    //        usr.SchoolId = 3;//readjust latter
    //        usr.Username = username;
    //        usrDal.SaveUser(usr);
    //        usr.SchoolCampusId = Convert.ToInt64(ddlCampus.SelectedValue);
    //        //save user role
    //        UserRole usrRl = new UserRole();
    //        usrRl.RoleId = selectedRole;
    //        usrRl.UserId = usr.Id;
    //        usrDal.SaveUserRole(usrRl);



    //        //bind the data to a grid 
    //        BindGrid();
    //        ddlRole.SelectedValue = "0";
    //        txtEmailAddress.Text = txtFirstname.Text = txtLastname.Text = txtPhoneNumber.Text = txtUsername.Text = string.Empty;
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrorMsg.Text = ex.Message;
    //        lblErrorMsg.Visible = true;
    //        throw ex;
    //    }
    //}
    protected void btnSearchLesson_Click(object sender, EventArgs e)
    {
        schoolId = Convert.ToInt64(logonUser.SchoolId);
        campusId = Convert.ToInt64(ddlCampus.SelectedValue);
        yearId = Convert.ToInt64(ddlYear.SelectedValue);
        sessionId = Convert.ToInt64(ddlSession.SelectedValue);
        termId = Convert.ToInt64(ddlTerm.SelectedValue);
        BindGrid(new PASSIS.LIB.LessonNoteLIB().RetrieveLessonNote(schoolId, campusId, yearId, sessionId, termId));
    }
    protected void gdvList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton approve = e.Row.FindControl("lnkApprove") as LinkButton;
            LinkButton decline = e.Row.FindControl("lnkDecline") as LinkButton;
            if (lNStatus == "Awaiting Approval")
            {
                approve.Visible = true;
                decline.Visible = true;
            }
            else
            {
                approve.Visible = false;
                decline.Visible = false;
            }
        }
    }
}