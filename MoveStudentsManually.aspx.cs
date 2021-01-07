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
public partial class MoveStudentsManually : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    //#region view state prop
    //private IList<pdc.MoveStudent> GradeMovementList_VS
    //{
    //    get
    //    {
    //        return ViewState[":::GradeMovementList_VS:::"] as IList<pdc.MoveStudent>;
    //    }
    //    set
    //    {
    //        ViewState[":::GradeMovementList_VS:::"] = value;
    //    }
    //}

    //private IList<Int64> GridGradeIds_VS
    //{
    //    get
    //    {
    //        return ViewState[":::GridGradeIds_VS:::"] as IList<Int64>;
    //    }
    //    set
    //    {
    //        ViewState[":::GridGradeIds_VS:::"] = value;
    //    }
    //}
    //#endregion

    //#region
    protected void Page_Load(object sender, EventArgs e)
    {

        lblErrorMsg.Text = "";

        //clsMyDB mdb = new clsMyDB();
        //mdb.connct();
        //string query = "SELECT CurriculumId FROM Schools WHERE id=" + logonUser.SchoolId;
        //SqlDataReader reader = mdb.fetch(query);
        //int CurriculumId = 0;
        //while (reader.Read())
        //{
        //    int.TryParse(reader[0].ToString(), out CurriculumId);
        //}
        //reader.Close();
        //mdb.closeConnct();

        if (!IsPostBack)
        {

            PASSIS.LIB.User currentUser = logonUser;
            long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
            long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));

            if (curriculumId == (long)CurriculumType.British)
            {
                ddlOldYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, 0);
                ddlNewYear.Items.Add(new ListItem("Alumni", "1", true));
                ddlNewYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, 0);
            }
            else if (curriculumId == (long)CurriculumType.Nigerian)
            {
                ddlOldYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
                ddlNewYear.Items.Add(new ListItem("Alumni", "1", true));
                ddlNewYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
            }
            ddlOldYear.DataBind();
            ddlNewYear.DataBind();


            //ddlNewYear.DataBind();

            //GradeMovementList_VS = new List<pdc.MoveStudent>();

        }

    }

    public enum CurriculumType
    {
        British = 1,
        Nigerian = 2
    }




    protected void ddlOldYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        //populate the grade list,
        Int64 selectedYearId = 0L;
        selectedYearId = Convert.ToInt64(ddlOldYear.SelectedValue);
        //  ddlOldGrade.Items.Clear();
        var availableGrades = new ClassGradeLIB().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, selectedYearId);
        //ddlOldGrade.Items.Add(new ListItem("---Select--- ", "0", true));
        ddlOldGrade.DataSource = availableGrades;
        ddlOldGrade.DataBind();
        ddlOldGrade.Items.Insert(0, new ListItem("---Select---", "0", true));



    }
    protected void btnAddToGrid_Click(object sender, EventArgs e)
    {

        if (ddlNewYear.SelectedIndex > 1 && ddlNewSession.SelectedIndex == 0) //if selected dropdown is not alumni and Session is not selected
        {
            lblErrorMsg.Text = "Session is Required!";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
        if (ddlOldYear.SelectedIndex == 0 || ddlOldGrade.SelectedIndex == 0 || ddlOldSession.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "All fields are required!";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
        }
        
        else
        {

            if (ddlNewYear.SelectedIndex > 1) //if selected dropdown is not alumni
            {
                if (ddlNewGrade.SelectedIndex > 0)
                {

                    //PASSIS.LIB.GradeStudent studentObject = context.GradeStudents.FirstOrDefault(x => x.StudentId == Convert.ToInt64(ddlStudentList.SelectedValue) &&
                    //    x.ClassId == Convert.ToInt64(ddlOldYear.SelectedValue) && x.GradeId == Convert.ToInt64(ddlOldGrade.SelectedValue));
                    PASSIS.LIB.Grade getTeacher = context.Grades.FirstOrDefault(x => x.Id == Convert.ToInt64(ddlNewGrade.SelectedValue));
                    PASSIS.LIB.AcademicSession getSession = context.AcademicSessions.FirstOrDefault(x => x.SchoolId == logonUser.SchoolId && x.IsCurrent == true);

                    foreach (GridViewRow row in gdvAllStudent.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            bool isChecked = row.Cells[2].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                            if (isChecked)
                            {
                                long studentId = Convert.ToInt64(row.Cells[3].Controls.OfType<Label>().FirstOrDefault().Text.ToString().Trim());
                        //        PASSIS.LIB.GradeStudent studentObject = context.GradeStudents.FirstOrDefault(x => x.StudentId == studentId &&
                        //x.ClassId == Convert.ToInt64(ddlOldYear.SelectedValue) && x.GradeId == Convert.ToInt64(ddlOldGrade.SelectedValue));

                                //studentObject.ClassId = Convert.ToInt64(ddlNewYear.SelectedValue);
                                //studentObject.GradeId = Convert.ToInt64(ddlNewGrade.SelectedValue);
                                //studentObject.GradeTeacherId = getTeacher.GradeTeacherId;
                                //studentObject.GradeTeacherId = context.Grades.FirstOrDefault(x => x.Id == Convert.ToInt64(ddlNewGrade.SelectedValue)).GradeTeacherId;
                                PASSIS.LIB.GradeStudent studentExists = context.GradeStudents.FirstOrDefault(x => x.StudentId == studentId && x.AcademicSessionId == Convert.ToInt64(ddlNewSession.SelectedValue));

                                if (studentExists != null )
                                {
                                    lblErrorMsg.Text = "One or more student(s) selection exists for this session!";
                                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                                    lblErrorMsg.Visible = true;
                                    return;
                                }
                              
                                PASSIS.LIB.GradeStudent newStdGrade = new PASSIS.LIB.GradeStudent();
                                newStdGrade.ClassId = Convert.ToInt64(ddlNewYear.SelectedValue);
                                newStdGrade.GradeId = Convert.ToInt64(ddlNewGrade.SelectedValue);
                                newStdGrade.StudentId = studentId;
                                newStdGrade.SchoolId = logonUser.SchoolId;
                                newStdGrade.SchoolCampusId = logonUser.SchoolCampusId;
                                newStdGrade.GradeTeacherId = getTeacher.GradeTeacherId;
                                newStdGrade.GradeTeacherId = context.Grades.FirstOrDefault(x => x.Id == Convert.ToInt64(ddlNewGrade.SelectedValue)).GradeTeacherId;
                                newStdGrade.AcademicSessionId = Convert.ToInt64(ddlNewSession.SelectedValue);
                                //newStdGrade.AcademicSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);
                                newStdGrade.DateCreated = DateTime.Now;
                                context.GradeStudents.InsertOnSubmit(newStdGrade);
                                context.SubmitChanges();


                            }
                        }
                    }

                    //studentObject.ClassId = Convert.ToInt64(ddlNewYear.SelectedValue);
                    //studentObject.GradeId = Convert.ToInt64(ddlNewGrade.SelectedValue);
                    //studentObject.GradeTeacherId = getTeacher.GradeTeacherId;
                    //studentObject.GradeTeacherId = context.Grades.FirstOrDefault(x => x.Id == Convert.ToInt64(ddlNewGrade.SelectedValue)).GradeTeacherId;

                    
                    lblErrorMsg.Text = "Student(s) Moved to a new Class and Grade successfully.";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                    lblErrorMsg.Visible = true;
                }
                else
                {
                    lblErrorMsg.Text = "Select the New Grade";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                    BindStudent();
                }
            }
            else
            {
                foreach (GridViewRow row in gdvAllStudent.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        bool isChecked = row.Cells[2].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                        if (isChecked)
                        {
                            long studentId = Convert.ToInt64(row.Cells[3].Controls.OfType<Label>().FirstOrDefault().Text.ToString().Trim());
                            PASSIS.LIB.GradeStudent studentObject = context.GradeStudents.FirstOrDefault(x => x.StudentId == studentId &&
                    x.ClassId == Convert.ToInt64(ddlOldYear.SelectedValue) && x.GradeId == Convert.ToInt64(ddlOldGrade.SelectedValue));
                            PASSIS.LIB.Alumni getAlumni = context.Alumnis.FirstOrDefault(x => x.StudentId == Convert.ToInt64(studentId));
                            if (getAlumni == null)
                            {
                                PASSIS.LIB.Alumni alumniObj = new Alumni();
                                alumniObj.StudentId = studentId;
                                alumniObj.SchoolId = logonUser.SchoolId;
                                alumniObj.SchoolCampusId = logonUser.SchoolCampusId;
                                alumniObj.AcademicSessionId = studentObject.AcademicSessionId;
                                alumniObj.ClassId = Convert.ToInt64(ddlOldYear.SelectedValue);
                                alumniObj.GradeId = Convert.ToInt64(ddlOldGrade.SelectedValue);
                                alumniObj.GradeTeacherId = studentObject.GradeTeacherId;
                                alumniObj.StudentStatusId = 4;
                                alumniObj.DateGraduated = DateTime.Now;

                                context.Alumnis.InsertOnSubmit(alumniObj);
                                context.SubmitChanges();
                            }
                            else
                            {
                                getAlumni.StudentId = studentId;
                                getAlumni.SchoolId = logonUser.SchoolId;
                                getAlumni.SchoolCampusId = logonUser.SchoolCampusId;
                                getAlumni.AcademicSessionId = studentObject.AcademicSessionId;
                                getAlumni.ClassId = Convert.ToInt64(ddlOldYear.SelectedValue);
                                getAlumni.GradeId = Convert.ToInt64(ddlOldGrade.SelectedValue);
                                getAlumni.GradeTeacherId = studentObject.GradeTeacherId;
                                getAlumni.StudentStatusId = 4;
                                getAlumni.DateGraduated = DateTime.Now;

                                context.SubmitChanges();
                            }

                            studentObject.HasGraduated = true;
                            studentObject.StudentStatusId = 4;

                            PASSIS.LIB.User userObj = context.Users.FirstOrDefault(x => x.Id == studentId);
                            userObj.StudentStatus = false;

                            context.SubmitChanges();

                        }
                    }
                }
                lblErrorMsg.Text = "Student(s) Graduated and Moved to Alumni Succesfully!";
                lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                lblErrorMsg.Visible = true;
                BindStudent();
            }
        }
    }

    //protected void btnMoveStudent_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //quick dirty work 
    //        PASSISDataContext pcon = new PASSISDataContext();
    //        foreach (pdc.MoveStudent gdm in GradeMovementList_VS)
    //        {
    //            //get old id and replace w
    //            //delete one students in the new classes move old student into the new classes
    //            PASSIS.DAO.Grade oldGs = new PASSIS.DAO.Grade();
    //            PASSIS.DAO.Grade NewGs = new PASSIS.DAO.Grade();
    //            //foreach (PASSIS.DAO.GradeStudent g in pcon.GradeStudents.Where(s => s.StudentId == gdm.StudentId))
    //            //{
    //            //    pcon.GradeStudents.DeleteOnSubmit(g);
    //            //    pcon.SubmitChanges();
    //            //}
    //            PASSIS.DAO.Grade gradeObj = pcon.Grades.FirstOrDefault(o => o.Id == gdm.NewGradeId);
    //            //foreach (PASSIS.DAO.GradeStudent g in pcon.GradeStudents.Where(s => s.GradeId == gdm.NewGradeId))
    //            //{
    //            foreach (PASSIS.DAO.GradeStudent g in pcon.GradeStudents.Where(s => s.GradeId == gdm.OldGradeId && s.StudentId == gdm.StudentId))
    //            {
    //                g.ClassId = gradeObj.ClassId;
    //                g.GradeId = gdm.NewGradeId;
    //                g.StudentId = gdm.StudentId;
    //                g.SchoolCampusId = gradeObj.SchoolCampusId;
    //                g.SchoolId = gradeObj.SchoolId;

    //                pcon.SubmitChanges();
    //            }
    //            //PASSIS.DAO.GradeStudent gs = pcon.GradeStudents.FirstOrDefault(s => s.GradeId == gdm.NewGradeId);
    //            //if (gs.GradeId == gdm.NewGradeId)
    //            //    {
    //            //        gs.ClassId = gradeObj.ClassId;
    //            //        gs.GradeId = gdm.NewGradeId;
    //            //        gs.StudentId = gdm.StudentId;
    //            //        gs.SchoolCampusId = gradeObj.SchoolCampusId;
    //            //        gs.SchoolId = gradeObj.SchoolId;

    //            //    }

    //            //pcon.SubmitChanges();


    //            //}
    //            GradeMovementList_VS = new List<pdc.MoveStudent>();
    //            gdvListMoveClasses.DataSource = GradeMovementList_VS;
    //            gdvListMoveClasses.DataBind();

    //        }
    //        lblErrorMsg.Text = "Movement successful.";
    //        lblErrorMsg.ForeColor = System.Drawing.Color.Green;
    //        lblErrorMsg.Visible = true;
    //    }
    //    catch (Exception exp) { throw exp; }
    //}



    //protected void ddlStudentList_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Int64 selectedYearId = 0L;
    //    //Int64 selectedStudent = 0L;
    //    selectedYearId = Convert.ToInt64(ddlOldYear.SelectedValue);
    //    //selectedStudent = Convert.ToInt64(ddlStudentList.SelectedValue);
    //    // ddlOldYear.Items.Clear();
    //    var availableGrades = new ClassGradeDAL().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, selectedYearId);

    //    //ddlNewGrade.DataSource = availableGrades;
    //    //ddlNewGrade.DataBind();
    //}

    protected void ddlOldGrade_SelectedIndexChanged(object sender, EventArgs e)
    {

        var academicSession = (from c in context.AcademicSessions where c.SchoolId == (long)logonUser.SchoolId select c.AcademicSessionName).Distinct();
        ddlOldSession.DataSource = academicSession.ToList();
        ddlOldSession.DataBind();
        ddlOldSession.Items.Insert(0, new ListItem("---Select---", "0", true));

        //BindStudent();
        //lblStudents.Visible = true;
    }

    protected void ddlNewGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        var academicSession = from c in context.AcademicSessions
                              where c.SchoolId == (long)logonUser.SchoolId && c.IsCurrent == true
                              select c.AcademicSessionName;
        ddlNewSession.DataSource = academicSession.ToList();
        ddlNewSession.DataBind();
        ddlNewSession.Items.Insert(0, new ListItem("---Select---", "0", true));


    }
    protected void ddlOldSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindStudent();
        lblStudents.Visible = true;
    }

    public void BindStudent()
    {
        Int64 selectedYearId = 0L;
        Int64 selectedGradeId = 0L;
        Int64 selectedSessionId = 0L;
        selectedYearId = Convert.ToInt64(ddlOldYear.SelectedValue);
        selectedGradeId = Convert.ToInt64(ddlOldGrade.SelectedValue);
        selectedSessionId = Convert.ToInt64(ddlOldSession.SelectedValue);
        //ddlStudentList.Items.Clear();
        var availableGrades = new UsersLIB().RetrieveStudents((long)logonUser.SchoolId, logonUser.SchoolCampusId, selectedYearId, selectedGradeId, selectedSessionId); //edit this code

        ////var gradeStdts = from grd in context.GradeStudents.Where(x => x.HasGraduated == null || x.HasGraduated == false && x.User.SchoolId == logonUser.SchoolId && x.User.SchoolCampusId == logonUser.SchoolCampusId) select grd;

        //var gradeStdts = from grd in context.GradeStudents.Where(x => x.HasGraduated == null || x.HasGraduated == false) select grd;

        //gradeStdts = gradeStdts.Where(c => c.User.SchoolCampusId == (logonUser.SchoolCampusId));

        //gradeStdts = gradeStdts.Where(s => s.ClassId == (selectedYearId));

        //gradeStdts = gradeStdts.Where(s => s.AcademicSessionId == (selectedSessionId));

        //gradeStdts = gradeStdts.Where(s => s.GradeId == (selectedGradeId));
        //var result = from res in gradeStdts select res.User;


        var NewavailableGrades = new ClassGradeLIB().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, selectedYearId);
        gdvAllStudent.DataSource = availableGrades;
        gdvAllStudent.DataBind();
    }
    //#endregion
    protected void ddlNewYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        //populate the grade list,
        Int64 selectedYearId = 0L;
        selectedYearId = Convert.ToInt64(ddlNewYear.SelectedValue);
        //  ddlOldGrade.Items.Clear();
        var availableGrades = new ClassGradeLIB().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, selectedYearId);
        //ddlNewGrade.Items.Add(new ListItem("---Select--- ", "0", true));
        ddlNewGrade.DataSource = availableGrades;
        ddlNewGrade.DataBind();
        ddlNewGrade.Items.Insert(0, new ListItem("---Select---", "0", true));
    }

   


   


    public string getGender(long genderId)
    {
        string gender = "";
        if (genderId == 1) { gender = "Male"; }
        else if (genderId == 2) { gender = "Female"; }
        return gender;
    }

    public string getStudentName(long studentId)
    {
        PASSIS.LIB.User objUser = context.Users.FirstOrDefault(x => x.Id == studentId);
        return objUser.StudentFullName;
    }
    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chk = (sender as CheckBox);
            if (chk.ID == "chkAll")
            {
                foreach (GridViewRow row in gdvAllStudent.Rows)
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