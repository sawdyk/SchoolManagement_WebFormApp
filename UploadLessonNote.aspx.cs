using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using System.IO;
using PASSIS.LIB;
using System.Text.RegularExpressions;
using System.Text;
using System.Data.SqlClient;

public partial class UploadLessonNote : PASSIS.LIB.Utility.BasePage
{


    PASSISLIBDataContext db = new PASSISLIBDataContext();
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    PASSIS.LIB.ParentDetail parentDetail = new PASSIS.LIB.ParentDetail();
    PASSIS.LIB.User ExistingUsrs = new PASSIS.LIB.User();
    string schoolCode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //lblResult.Visible = false;
            PASSIS.LIB.User currentUser = logonUser;

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
                ddlSession.Items.Insert(0, new ListItem("--Select--", "0", true));
            }
            reader.Close();
            mdb.closeConnct();

            long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
            long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
            //ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);            
            //ddlYear.DataBind();
            ddlYear.DataSource = new PASSIS.LIB.TeacherLIB().getTeacherAllClass_Grade(logonUser.Id);
            ddlYear.DataBind();
            ddlYear.Items.Insert(0, new ListItem("--Select--", "0", true));

            ddlTerm.DataSource = new AcademicTermLIB().RetrieveAcademicTerm();
            ddlTerm.DataBind();
            ddlTerm.Items.Insert(0, new ListItem("--Select--", "0", true));

            getSubject();
            BindGrid(new PASSIS.LIB.LessonNoteLIB().RetrieveLessonNoteByTeacherId(logonUser.Id));

            //ddlCampus.DataSource = new AcademicSessionDAL().GetAllSchoolCampus((long)currentUser.SchoolId);
            //ddlCampus.DataBind();
            //ddlCampus.SelectedValue = currentUser.SchoolCampusId.ToString();

            //ddlCampus.Enabled = false;
            //bp.Visible = false;

            long schoolId = (long)logonUser.SchoolId;
            PASSIS.LIB.SchoolLIB schLib = new PASSIS.LIB.SchoolLIB();
            schoolCode = schLib.RetrieveSchool(schoolId).Code;
        }

    }
    protected void btnLessonUpload_Click(object sender, EventArgs e)
    {
        //validate the value and save
        //impose restriction on file sixe 
        //try
        //{
        if (ddlYear.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select year";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }

        if (ddlSession.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select session";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }

        if (ddlSubject.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select subject";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }

        if (txtDescription.Text.Equals(""))
        {
            lblErrorMsg.Text = "Kindly input the discription";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }

        if (ddlTerm.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select term";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
        PASSIS.LIB.LessonNote lessonNote = new PASSIS.LIB.LessonNote();
        Int64 classId = Convert.ToInt64(ddlYear.SelectedValue);
        Int64 gradeId = Convert.ToInt64(ddlGrade.SelectedValue);
        int subjectId = Convert.ToInt16(ddlSubject.SelectedValue);
        Int64 TeacherId = logonUser.Id;
        long? schoolId = logonUser.SchoolId;
        long campusId = logonUser.SchoolCampusId;
        Int64 termId = Convert.ToInt64(ddlTerm.SelectedValue);
        Int64 sessionId = Convert.ToInt64(ddlSession.SelectedValue);
        string description = txtDescription.Text.Trim();
        //string TemplateCode = ddlCode.SelectedItem.Text.ToString();


        if (LessonNoteFile.HasFile)
        {
            string originalFileName = Path.GetFileName(LessonNoteFile.PostedFile.FileName);
            string modifiedFileName = string.Format("{0}{1}", DateTime.Now.ToString("MMddHmmss"), originalFileName);
            string fileExtension = Path.GetExtension(LessonNoteFile.PostedFile.FileName);
            string fileLocation = Server.MapPath("~/docs/LessonNote/") + modifiedFileName;


            //Check whether file extension is doc or docx

            if (!fileExtension.Contains(".doc"))
            {
                lblErrorMsg.Text = string.Format("Upload not successful. The file format is not supported!!!");
                lblErrorMsg.Visible = true;
                return;
            }

            LessonNoteFile.SaveAs(fileLocation);
            lessonNote.FileName = modifiedFileName;
            lessonNote.SchoolId = Convert.ToInt64(schoolId);
            lessonNote.CampusId = campusId;
            lessonNote.SessionId = sessionId;
            lessonNote.TermId = termId;
            lessonNote.ClassId = classId;
            lessonNote.GradeId = gradeId;
            lessonNote.SubjectId = subjectId;
            lessonNote.Description = description;
            lessonNote.TeacherId = TeacherId;
            lessonNote.Date = DateTime.Now;
            lessonNote.Status = 0;
            new PASSIS.LIB.LessonNoteLIB().SaveLessonNote(lessonNote);
            BindGrid(new PASSIS.LIB.LessonNoteLIB().RetrieveLessonNoteByTeacherId(TeacherId));

            //lblResult.Visible = true;
            lblErrorMsg.Text = string.Format("Uploaded Successfully.");
            lblErrorMsg.Visible = true;
            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
            clearFields();

            //IList<PASSIS.LIB.User> users = RetrieveStudentsFromExcelFile(fileLocation, (long)logonUser.SchoolId, logonUser.SchoolCampusId);
            //string log = ProcessRetrievedStudentInfo(users, (long)logonUser.SchoolId, logonUser.SchoolCampusId, BasePage.log, schoolCode.ToString());
            //migrateParents(BasePage.log, users);
            //lblResult.Text = log;
            //lblResult.ForeColor = System.Drawing.Color.Green;



        }

        if (LessonNoteFile.HasFile == false)
        {
            lblErrorMsg.Text = string.Format("Please specify the file to be uploaded.");
            lblErrorMsg.Visible = true;
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            return;
        }
    }
    protected static string RemoveSpecialCharacters(string str)
    {
        return Regex.Replace(str, "[^a-zA-Z0-9%._]", string.Empty);

    }
    public static string ProcessRetrievedStudentInfo(IList<PASSIS.LIB.User> userList, Int64 schlId, Int64 campsId, log4net.ILog logger, string schoolCode)
    {
        //variables to change 
        string campusCode = new AcademicSessionLIB().RetrieveSchoolCampus(campsId).Code;
        Int64 userIdTemp = 1; // class grade usersiD meant for admin

        StringBuilder log = new StringBuilder();
        log.Append("Total numbers of Students found = ").Append(userList.Count).AppendLine();
        UsersLIB usrDAL = new UsersLIB();
        ClassGradeLIB clsGrdDal = new ClassGradeLIB();
        //matric number processing not yet done 
        logger.InfoFormat("about starting.....{0} users to be processed", userList.Count);
        Int32 studentsCreated = 0;
        foreach (PASSIS.LIB.User usr in userList)
        {  //save the users info
            try
            {

                usr.AdmissionNumber = usr.Username = PASSIS.LIB.Utility.Utili.GenerateAdmissionNumber(schoolCode, Convert.ToInt64(schlId));
                new UsersLIB().SaveUser(usr);
                //save student role
                PASSIS.LIB.UserRole usrRol = new PASSIS.LIB.UserRole();
                usrRol.RoleId = (long)PASSIS.LIB.Utility.roles.student; //student
                usrRol.UserId = usr.Id;
                new UsersLIB().SaveUserRole(usrRol);
                ++studentsCreated;


            }
            catch (Exception ex)
            {
                //logger.InfoFormat("Exception occurrred while processing this  {0} {1} {2}  {3}  {4}   eXCEPTION ::::{5}", usr.FirstName, usr.LastName, usr.GradeString, usr.HomeRoomTutor, usr.FullName, ex.Message);

            }

        }

        logger.InfoFormat("summary {0} ===={1}", log.ToString(), log.ToString());
        return string.Format("{0} Students have been created.", studentsCreated);
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
        if (e.CommandName == "remove")
        {
            Int64 Id = Convert.ToInt64(e.CommandArgument);
            PASSISLIBDataContext ct = new PASSISLIBDataContext();
            PASSIS.LIB.LessonNote lnote = new PASSIS.LIB.LessonNote();
            lnote = ct.LessonNotes.FirstOrDefault(s => s.Id == Id);
            ct.LessonNotes.DeleteOnSubmit(lnote);
            ct.SubmitChanges();
            BindGrid(new PASSIS.LIB.LessonNoteLIB().RetrieveLessonNoteByTeacherId(logonUser.Id));
            lblErrorMsg.Text = "Lesson Note removed successfully";
            lblErrorMsg.Visible = true;
            lblErrorMsg.ForeColor = System.Drawing.Color.Green;

        }
    }
    public static string getRandomUsername()
    {
        return string.Format("Identity{0}", PASSIS.LIB.Utility.Utili.GetUniqueRandomNumber(10));
    }
    public static string GetUniqueRandomNumber(object objlength)
    {
        int length = Convert.ToInt32(objlength);
        var number = rng.NextDouble().ToString("0.000000000000").Substring(2, length);
        return number;
    }
    #region password
    /// <summary>
    /// Under construction 
    /// </summary>
    /// <returns></returns>
    public static string getDefaultPassword()
    {
        return "password";
    }
    #endregion
    private static Random rng = new Random(Environment.TickCount);

    private void getSubject()
    {

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
    public PASSIS.LIB.Grade getGrade(Int64 gradeId)
    {
        PASSIS.LIB.Grade grade = context.Grades.FirstOrDefault(x => x.Id == gradeId);
        return grade;
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
        string lNStatus = "";
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

    public void clearFields()
    {
        ddlSession.SelectedIndex = 0;
        ddlSubject.SelectedIndex = 0;
        ddlTerm.SelectedIndex = 0;
        ddlYear.SelectedIndex = 0;
        txtDescription.Text = "";
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int64 selectedYearId = 0L;
        selectedYearId = Convert.ToInt64(ddlYear.SelectedValue);
        ddlGrade.Items.Clear();
        var availableGrades = new ClassGradeDAL().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, selectedYearId);
        ddlGrade.DataSource = availableGrades;
        ddlGrade.DataBind();
        ddlGrade.Items.Insert(0, new ListItem("--Select--", "0", true));
    }
    protected void ddlGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlSubject.Items.Clear();
        IList<PASSIS.LIB.SubjectTeacher> test = new TeacherLIB().getTeacherSubjects(logonUser.Id, Convert.ToInt32(ddlGrade.SelectedValue));
        foreach (PASSIS.LIB.SubjectTeacher subjId in test)
        {
            PASSIS.LIB.Subject reqSubjects = context.Subjects.FirstOrDefault(c => c.Id == subjId.SubjectId);
            ddlSubject.Items.Add(new ListItem(reqSubjects.Name, reqSubjects.Id.ToString()));
        }
        ddlSubject.Items.Insert(0, new ListItem("--Select--", "0", true));
    }
}