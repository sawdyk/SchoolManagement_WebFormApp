using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO;
using PASSIS.DAO.Utility;
using System.IO;
using PASSIS.LIB;


public partial class TeacherAssignmentUpload : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    //protected void Page_PreInit(object sender, EventArgs e)
    //{
    //    base.Page_PreInit(sender, e);
    //    this.MasterPageFile = "~/TeachersAssignments.master";
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid2();
            if (isUserClassTeacher)
            {
                try
                {
                    ddlYear.DataSource = new PASSIS.LIB.TeacherLIB().getTeacherAllClass_Grade(logonUser.Id);
                    ddlYear.DataBind();

                    ddlGrade.DataTextField = "GradeName";
                    ddlGrade.DataValueField = "Id";
                    //ddlGrade.DataSource = new ClassGradeDAL().getAllClassGrade((long)logonUser.SchoolId);
                    //ddlGrade.DataBind();
                    Int64 userId = logonUser.Id;
                    Int64 assignedGradeId = 0L;
                    //assignedGradeId = new ClassGradeDAL().RetrieveTeacherGrade(userId).GradeId;
                    //ddlGrade.SelectedValue = assignedGradeId.ToString();
                    //ddlGrade.Enabled = false;
                    //should d selected garde

                   // BindGrid(new PASSIS.LIB.AssignmentLIB().RetrieveAssignment(userId, -1, 0, string.Empty));
                    BindGrid2();
                }
                catch
                {
                }
            }
            else
            {
                long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
                long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
                ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
                ddlYear.DataBind();

                //IList<PASSIS.LIB.SubjectsInSchool> test = new PASSIS.LIB.SubjectTeachersLIB().getAllSubjects((long)logonUser.SchoolId);
                //foreach (PASSIS.LIB.SubjectsInSchool subjId in test)
                //{
                //    PASSIS.LIB.Subject reqSubjects = context.Subjects.FirstOrDefault(c => c.Id == subjId.SubjectId);
                //    ddlSubject.Items.Add(new ListItem(reqSubjects.Name, reqSubjects.Id.ToString()));
                //}
                //ddlSubject.Items.Insert(0, new ListItem("--Select--", "0", true));
             
            }
        }
        //BindGrid();

    }

    protected void ddlAssignmentRecipient_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAssignmentRecipient.SelectedValue == "1")// recipient is meant to be a class 
        {
            ddlGrade.Visible = true;
            ddlGrade.Enabled = true;
            // ddlGroup.Enabled = false;
        }
        else
        {
            ddlGrade.Visible = false;
            ddlGrade.Enabled = false;
            // ddlGroup.Enabled = true;
        }

    }

    protected void ddlYear_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //populate the grade list,
        Int64 selectedYearId = 0L;
        selectedYearId = Convert.ToInt64(ddlYear.SelectedValue);
        ddlGrade.Items.Clear();// ddlGroup.Items.Clear();
        var availableGrades = new ClassGradeLIB().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, selectedYearId);
        ////removes the grades that are already on the grid 
        //var d = from s in availableGrades where !GridGradeIds_VS.Contains(s.Id) select s;
        var logonTeacherGroupsByYear = new GroupingLIB().RetrieveTeacherGrouping(logonUser.Id, selectedYearId);
        ddlGrade.DataSource = availableGrades;
        ddlGrade.DataBind();

        //ddlGroup.DataSource = logonTeacherGroupsByYear;
        //ddlGroup.DataBind();
        //ddlGrade.Items.Add(new ListItem("--Select All--", "0", true));
        ddlGrade.Items.Insert(0, new ListItem("--Select--", "0", true));
        // ddlGroup.Items.Insert(0, new ListItem("--Select--", "0", true));
    }

    protected string getClassOrGroupName(object gradeId, object groupId)
    {
        string result = string.Empty;
        try
        {
            if (gradeId == null)
            {
                result = new GroupingLIB().RetrieveGrouping(Convert.ToInt64(groupId)).GroupName;
            }
            else
            {
                result = new ClassGradeLIB().RetrieveGrade(Convert.ToInt64(gradeId)).GradeName;
            }
        }
        catch { }
        return result;
    }
    protected string getYear(object gradeId, object groupId)
    {
        string result = string.Empty;
        if (gradeId == null)
        {
            result = new GroupingLIB().RetrieveGroupingYear(Convert.ToInt64(groupId)).Name;
        }
        else
        {
            try
            {
                result = new ClassGradeLIB().RetrieveGrade(Convert.ToInt64(gradeId)).Class_Grade.Name;
            }
            catch { }
        }
        return result;
    }


    protected string getSubjectName(long subjectId)
    {
        string subj = string.Empty;
        PASSIS.LIB.Subject subjName = context.Subjects.FirstOrDefault(sub => sub.Id == subjectId);
        if (subjName !=null)
        {
            return subj = subjName.Name;
        }

        return subj;
    }

    protected string getTeacherName(long teacherId)
    {
        string tName = string.Empty;
        

        PASSIS.LIB.User teacherName = context.Users.FirstOrDefault(tch => tch.Id == teacherId);
        if (teacherName != null)
        {
            return tName = string.Format("{0}{1}{2}", teacherName.FirstName," ", teacherName.LastName); ;
        }

        return tName;
    }

    //protected void BindGrid(IList<PASSIS.LIB.Assignment> assignments)
    //{
    //    gdvList.DataSource = assignments;
    //    gdvList.DataBind();

    //}
    protected void gdvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvList.PageIndex = e.NewPageIndex;
        BindGrid2();
    }
    protected void BindGrid2()
    {
        var assignments = from s in context.Assignments
                          where s.TeacherId == logonUser.Id
                          select s;

        gdvList.DataSource = assignments;
        gdvList.DataBind();

    }
    protected void gdvList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "download")
        {
            string filename = e.CommandArgument.ToString();
            string path = MapPath("~/docs/Assignment/" + filename);
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
            PASSIS.LIB.Assignment ass = new PASSIS.LIB.Assignment();
            ass = ct.Assignments.FirstOrDefault(s => s.Id == Id);
            ct.Assignments.DeleteOnSubmit(ass);
            ct.SubmitChanges();
            //BindGrid(new PASSIS.LIB.AssignmentLIB().RetrieveAssignment((long)logonUser.Id, -1, 0, string.Empty));
            BindGrid2();
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "Assignment removed successfully";
            lblErrorMsg.ForeColor = System.Drawing.Color.Green;

        }
    }
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        //validate the value and save
        //impose restriction on file sixe
        PASSIS.LIB.Assignment asgnmt = new PASSIS.LIB.Assignment();
        try
        {
            if (ddlYear.SelectedValue == "0")
            {
                lblErrorMsg.Text = string.Format("Select a valid year.");
                lblErrorMsg.Visible = true;
                return;
            }
            if (ddlGrade.SelectedValue == "0" && ddlAssignmentRecipient.SelectedValue == "1")
            {
                lblErrorMsg.Text = string.Format("Select a valid Grade.");
                lblErrorMsg.Visible = true;
                return;
            }
            //if (ddlGroup.SelectedValue == "0" && ddlAssignmentRecipient.SelectedValue == "2")
            //{
            //    lblErrorMsg.Text = string.Format("Select a valid group/set.");
            //    lblErrorMsg.Visible = true;
            //    return;
            //}
            if (string.IsNullOrEmpty(txtDesc.Text))
            {
                lblErrorMsg.Text = string.Format("A short document description is required.");
                lblErrorMsg.Visible = true;
                return;
            }
            if (string.IsNullOrEmpty(txtScore.Text))
            {
                lblErrorMsg.Text = string.Format("Assignment score is required.");
                lblErrorMsg.Visible = true;
                return;
            }

            string inputDate = txtAssignmentDueDate.Text;
            int currentDate = DateTime.Compare(Convert.ToDateTime(inputDate), DateTime.Now);

            if (currentDate < 0)
            {
                lblErrorMsg.Text = "You cannot pick a date that is earlier";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
            }
            else
            {

                if (documentUpload.HasFile)
                {
                    //log.InfoFormat("....1");
                    string originalFileName = Path.GetFileName(documentUpload.PostedFile.FileName);
                    string modifiedFileName = string.Format("{0}{1}", DateTime.Now.ToString("MMddHmmss"), originalFileName);
                    string fileExtension = System.IO.Path.GetExtension(documentUpload.PostedFile.FileName);
                    //log.InfoFormat("....2");
                    string fileLocation = Server.MapPath("~/docs/Assignment/") + modifiedFileName;
                    //log.InfoFormat("....3");
                    documentUpload.SaveAs(fileLocation);
                    //log.InfoFormat("....4");
                    //log.InfoFormat(" original filename:= {0};;;;; modified filename:= {1} ;;;;;filextension:=  {2};;;;; filelocation:=={3}", originalFileName, modifiedFileName, fileExtension, fileLocation);
                    //Check whether file extension is xls or xslx
                    //if (!(fileExtension.Contains(".doc") || fileExtension.Contains(".docx") || fileExtension.Contains(".xls") || fileExtension.Contains(".xlsx")))
                    if (!(fileExtension.Contains(".doc")) && !(fileExtension.Contains(".pdf")))
                    {
                        lblErrorMsg.Text = string.Format("Upload not succesful. The file format is not supported!!!");
                        lblErrorMsg.Visible = true;
                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                        return;
                    }

                    asgnmt.DateUploaded = DateTime.Now;
                    asgnmt.Description = txtDesc.Text.Trim();
                    asgnmt.FileName = modifiedFileName;
                    asgnmt.DueDate = Convert.ToDateTime(txtAssignmentDueDate.Text);
                    asgnmt.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                    //log.InfoFormat("....before logon user");
                    try
                    {
                        if (ddlAssignmentRecipient.SelectedValue == "1")// class is selected 
                        {
                            asgnmt.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                            asgnmt.GroupId = null;
                        }
                        else
                        {
                            // asgnmt.GroupId = Convert.ToInt64(ddlGroup.SelectedValue);
                            asgnmt.GradeId = null;
                        }



                        //asgnmt.GradeId = new ClassGradeDAL().RetrieveGradeClassOfTeacher(logonUser.Id).Id;
                    }
                    catch
                    {
                        //might not be necessary again. Thanks joor. kayode 23.09.2013
                        asgnmt.GradeId = 0L;
                        //log.InfoFormat("....This teacher must be a subject teacher,  user id {0}", logonUser.Id);
                    }
                    //log.InfoFormat("....after logon users");
                    asgnmt.MaximumObtainableScore = Convert.ToDecimal(txtScore.Text);
                    asgnmt.SessionId = new PASSIS.LIB.AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);
                    asgnmt.SubjectId = Convert.ToInt64(ddlSubject.SelectedValue);
                    asgnmt.TeacherId = logonUser.Id;

                    context.Assignments.InsertOnSubmit(asgnmt);
                    context.SubmitChanges();
                    //new PASSIS.LIB.AssignmentLIB().SaveAssignments(asgnmt);
                    //log.InfoFormat("....7");
                    //BindGrid(new PASSIS.LIB.AssignmentLIB().RetrieveAssignment(logonUser.Id, -1, 0, string.Empty));
                    BindGrid2();
                    //log.InfoFormat("....8");
                    lblErrorMsg.Text = "Assignment Uploaded Successfully";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                    lblErrorMsg.Visible = true;

                    clearAssignmentUploadFields();

                }
                else
                {
                    lblErrorMsg.Text = string.Format("File upload is required.");
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                //Response.Redirect("~/rescList.aspx");
            }

        }
        catch (Exception ex)
        {
            //log.InfoFormat("....exception {0}", ex.Message);
            throw ex;

        }
    }
    protected void ddlGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!isUserClassTeacher)
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
        else
        {
            //ddlSubject.DataSource = new TeacherLIB().getTeacherSubjects(logonUser.Id, Convert.ToInt32(ddlGrade.SelectedValue));
            //ddlSubject.DataBind();

            IList<PASSIS.LIB.SubjectTeacher> getId = new TeacherLIB().getTeacherSubjects(logonUser.Id, Convert.ToInt32(ddlGrade.SelectedValue));
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            ddlSubject.Items.Clear();
            foreach (PASSIS.LIB.SubjectTeacher subjId in getId)
            {
                PASSIS.LIB.Subject reqSubject = context.Subjects.FirstOrDefault(c => c.Id == subjId.SubjectId);
                ddlSubject.Items.Add(new ListItem(reqSubject.Name, reqSubject.Id.ToString()));
            }
            ddlSubject.Items.Insert(0, new ListItem("--Select--", "0", true));
        }

    }
    public void clearAssignmentUploadFields()
    {
        txtDesc.Text = "";
        txtScore.Text = "";
        txtAssignmentDueDate.Text = "";
        ddlAssignmentRecipient.SelectedIndex = 0;
        ddlGrade.SelectedIndex = 0;
        //  ddlGroup.SelectedIndex = 0;
        ddlSubject.SelectedIndex = 0;
        ddlYear.SelectedIndex = 0;
    }
}