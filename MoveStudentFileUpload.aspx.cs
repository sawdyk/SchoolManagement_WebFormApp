using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using System.IO;
using System.Data.OleDb;
using System.Data;
using PASSIS.LIB;


public partial class MoveStudentFileUpload : PASSIS.LIB.Utility.BasePage
{
    //protected void Page_PreInit(object sender, EventArgs e )
    //{
    //    base.Page_PreInit(sender,e);
    //    this.MasterPageFile = "~/SchlAdminStds.master";
    //}

    PASSISLIBDataContext pcon = new PASSISLIBDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        //ddlMove.Items.Clear();
        if (!IsPostBack)
        {
            ddlMove.Items.Insert(0, new ListItem("--Select--", "0", true));
            ddlMove.Items.Insert(1, new ListItem("To Next Class", "1", true));
            ddlMove.Items.Insert(2, new ListItem("To Alumni", "2", true));
        }
    }

    protected void btnUpload_OnClick(object sender, EventArgs e)
    {
        //validate the value and save
        //impose restriction on file sixe 
        //try
        //{
        if (uploadFile.HasFile)
        {
            string originalFileName = Path.GetFileName(uploadFile.PostedFile.FileName);
            string modifiedFileName = string.Format("{0}{1}", DateTime.Now.ToString("MMddHmmss"), originalFileName);
            string fileExtension = Path.GetExtension(uploadFile.PostedFile.FileName);
            string fileLocation = Server.MapPath("~/docs/") + modifiedFileName;


            //Check whether file extension is xls or xslx

            if (!fileExtension.Contains(".xls"))
            {
                lblErrorMsg.Text = string.Format("Upload not successful. The file format is not supported!!!");
                lblErrorMsg.Visible = true;
                return;
            }
            else
            {
                uploadFile.SaveAs(fileLocation);
                IList<MoveStudentClass> msList = new List<MoveStudentClass>();
                Net.SourceForge.Koogra.Excel.Workbook wkbk = new Net.SourceForge.Koogra.Excel.Workbook(fileLocation);
                Net.SourceForge.Koogra.Excel.Worksheet workSheet = wkbk.Sheets[0];
                for (uint rowCount = workSheet.Rows.MinRow; rowCount <= workSheet.Rows.MaxRow; ++rowCount)
                {
                    try
                    {
                        if (rowCount != workSheet.Rows.MinRow)// jump the first row
                        {
                            MoveStudentClass msClass = new MoveStudentClass();
                            //Net.SourceForge.Koogra.Excel.Row row = new Net.SourceForge.Koogra.Excel.Row();
                            Net.SourceForge.Koogra.Excel.Row row = workSheet.Rows[rowCount];
                            //do validation when u hv time 
                            try
                            { msClass.serialNumber = row.Cells[0].Value.ToString(); }
                            catch { }
                            try { msClass.pupilFullname = row.Cells[1].Value.ToString(); }
                            catch { }
                            try
                            { msClass.admissionNumber = row.Cells[2].Value.ToString(); }
                            catch { }
                            try { msClass.oldClass = row.Cells[3].Value.ToString(); }
                            catch { }
                            try
                            { msClass.oldGrade = row.Cells[4].Value.ToString(); }
                            catch { }
                            try { msClass.gender = row.Cells[5].Value.ToString(); }
                            catch { }
                            try
                            { msClass.newClass = row.Cells[6].Value.ToString(); }
                            catch { }
                            try { msClass.newGrade = row.Cells[7].Value.ToString(); }
                            catch { }
                            msList.Add(msClass);
                        }
                    }
                    catch (Exception ex) { throw ex; }

                }
                gdvList.DataSource = msList;
                gdvList.DataBind();
                //    uploadFile.SaveAs(fileLocation);
                //    OleDbConnection conn = new OleDbConnection();
                //    String con = "";


                //if (fileExtension == ".xls"){

                //    //con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=Excel 8.0;HDR=YES;IMEX=1");
                //    conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" + fileLocation + "; Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\";");
                //    //con = @"Provider=Microsoft.Jet.OLEDB.4.0; data source=" + fileLocation + "; Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\";";


                //    //con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=YES\"");

                //}else if (fileExtension == ".xlsx"){
                //    conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;ReadOnly=False;HDR=YES\"");
                //    //con = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;ReadOnly=False;HDR=YES\"";

                //}
                ////conn.ConnectionString = con;
                //conn.Open();
                //DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                //string getSheetName = txtSheetName.Text;
                //if (getSheetName != "" || getSheetName.Contains("Sheet"))
                //{
                //    OleDbCommand excelCommand = new OleDbCommand(@"select * from [" + getSheetName + "$]", conn);
                //    OleDbDataAdapter excelAdapter = new OleDbDataAdapter(excelCommand);
                //    DataSet excelDataSet = new DataSet();
                //    excelAdapter.Fill(excelDataSet);
                //    gdvList.DataSource = excelDataSet;
                //    gdvList.DataBind();
                //    lblErrorMsg.Text = string.Format("Upload successful.");
                //    lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                //    lblErrorMsg.Visible = true;
                //    return;
                //}
                //else {
                //    lblErrorMsg.Text = string.Format("Invalid Sheet Name!!!");
                //    lblErrorMsg.Visible = true;
                //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //    return;
                //}
            }

            //        IList<PASSIS.LIB.User> users = RetrieveStudentsFromExcelFile(fileLocation, (long)logonUser.SchoolId, logonUser.SchoolCampusId);
            //        string log = ProcessRetrievedStudentInfo(users, (long)logonUser.SchoolId, logonUser.SchoolCampusId, BasePage.log, logonUser.School.Code);
            //        migrateParents(BasePage.log, users);
            //        lblResult.Text = log;
            //        lblResult.ForeColor = System.Drawing.Color.Green;



            //    }

            //    if (uploadFile.HasFile == false)
            //    {
            //        lblErrorMsg.Text = string.Format("Please specify the file to be uploaded.");
            //        lblErrorMsg.Visible = true;
            //        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            //        return;
            //    }

            //    else if (Page.ClientScript.IsStartupScriptRegistered("Duplicate"))
            //    {
            //        DuplicateReturn();
            //    }

            //    else if (BasePage.log.Logger.Equals("0 Students have been created."))
            //    {
            //        lblErrorMsg.Text = string.Format("Duplicate Found.");
            //        lblErrorMsg.Visible = true;
            //        lblErrorMsg.ForeColor = System.Drawing.Color.Green;
            //    }

            //    else

            //    //Response.Redirect("~/Students.aspx");
            //    {
            //        lblResult.Visible = true;
            //        lblErrorMsg.Text = string.Format("Uploaded Successfully.");
            //        lblErrorMsg.Visible = true;
            //        lblErrorMsg.ForeColor = System.Drawing.Color.Green;

            //    }

            //    lblResult.Visible = true;
            //    lblErrorMsg.Text = string.Format("Uploaded Successfully.");
            //    lblErrorMsg.Visible = true;
            //    lblErrorMsg.ForeColor = System.Drawing.Color.Green;


        }
        else
        {
            lblErrorMsg.Text = string.Format("Please specify the file to be uploaded.");
            lblErrorMsg.Visible = true;
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            return;

        }

        //}
        //catch (Exception ex)
        //{
        //    PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
        //    lblErrorMsg.Text = string.Format(ex.ToString() + "Error occur, kindly contact your administrator.");
        //    lblErrorMsg.Visible = true;
        //}
    }
    protected void gdvList_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void gdvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    protected void btnMoveSelectedClasses_Click(object sender, EventArgs e)
    {
        //try
        //{
        //quick dirty work 



        if (Convert.ToInt64(ddlMove.SelectedValue) == 1)
        {
            foreach (GridViewRow row in gdvList.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    string OldClass = row.Cells[3].Text.Trim();
                    string OldGrade = row.Cells[4].Text.Trim();
                    string NewClass = row.Cells[6].Text.Trim();
                    string NewGrade = row.Cells[7].Text.Trim();
                    string _StudentAdmissionNumber = row.Cells[2].Text.Trim();
                    PASSIS.LIB.User getUserobj = pcon.Users.FirstOrDefault(x => x.AdmissionNumber == _StudentAdmissionNumber);
                    PASSIS.LIB.Grade oldGradeObj = pcon.Grades.FirstOrDefault(o => o.GradeName == OldGrade);
                    PASSIS.LIB.Class_Grade oldClassObj = pcon.Class_Grades.FirstOrDefault(c => c.Name == OldClass);
                    PASSIS.LIB.Grade newGradeObj = pcon.Grades.FirstOrDefault(o => o.GradeName == NewGrade);
                    PASSIS.LIB.Class_Grade newClassObj = pcon.Class_Grades.FirstOrDefault(x => x.Name == NewClass);
                    //PASSIS.LIB.GradeStudent newgradeStudent = pcon.GradeStudents.FirstOrDefault(x => x.GradeId == newGradeObj.Id);
                    if (getUserobj != null && oldGradeObj != null && oldClassObj != null && newGradeObj != null && newClassObj != null)
                    {
                        //get old id and replace w
                        //delete all students in the new classes move old student into the new classes
                        PASSIS.DAO.Grade oldGs = new PASSIS.DAO.Grade();
                        PASSIS.DAO.Grade NewGs = new PASSIS.DAO.Grade();
                        //foreach (PASSIS.LIB.GradeStudent g in pcon.GradeStudents.Where(s => s.GradeId == gradeObj.Id && s.StudentId == gradeStudent.StudentId))
                        //{
                        //    pcon.GradeStudents.DeleteOnSubmit(g);
                        //    pcon.SubmitChanges();
                        //}


                        //PASSIS.LIB.Class_Grade getGradeId = pcon.Class_Grades.FirstOrDefault(x => x.Id == getGradeObj.Id);
                        //Int64 gradeId = Convert.ToInt16(newClassObj.Id);

                        PASSIS.LIB.GradeStudent oldGradeStudent = pcon.GradeStudents.FirstOrDefault(x => x.GradeId == oldGradeObj.Id && x.ClassId == oldClassObj.Id && x.StudentId == getUserobj.Id);
                        //PASSIS.LIB.GradeStudent getGradeStudent = pcon.GradeStudents.FirstOrDefault(s => s.GradeId == oldGradeObj.Id && s.ClassId == oldClassObj.Id && s.StudentId == oldGradeStudent.StudentId);
                        if (oldGradeStudent != null)
                        {
                            //foreach (PASSIS.LIB.GradeStudent g in getGradeStudent)
                            //{
                            PASSIS.LIB.Grade getTeacher = pcon.Grades.FirstOrDefault(x => x.Id == newGradeObj.Id);

                            oldGradeStudent.ClassId = Convert.ToInt64(newClassObj.Id);
                            oldGradeStudent.GradeId = Convert.ToInt64(newGradeObj.Id);
                            oldGradeStudent.StudentId = oldGradeStudent.StudentId;
                            oldGradeStudent.GradeTeacherId = getTeacher.GradeTeacherId;
                            oldGradeStudent.SchoolCampusId = oldGradeStudent.SchoolCampusId;
                            oldGradeStudent.SchoolId = oldGradeStudent.SchoolId;

                            pcon.SubmitChanges();
                            //}
                            lblErrorMsg.Text = "Movement successful.";
                            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                            gdvList.Visible = false;
                            lblErrorMsg.Visible = true;
                        }
                        else
                        {
                            lblErrorMsg.Text = "Error in Getting GradeStudent";
                            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                            gdvList.Visible = false;
                            lblErrorMsg.Visible = true;
                            return;
                        }
                    }
                    else
                    {
                        if (getUserobj == null)
                        {
                            lblErrorMsg.Text = "This Admission Number: " + _StudentAdmissionNumber + " Doesn't exist";
                            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                            gdvList.Visible = false;
                            lblErrorMsg.Visible = true;
                            return;
                        }
                        else
                        {
                            lblErrorMsg.Text = "Error in Class or Grade Selection for" + " " + getUserobj.StudentFullName + " " + "with Admission Number:" + " " + getUserobj.AdmissionNumber;
                            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                            gdvList.Visible = false;
                            lblErrorMsg.Visible = true;
                            return;
                        }
                    }

                }

            }


        }
        else if (Convert.ToInt64(ddlMove.SelectedValue) == 2)
        {
            foreach (GridViewRow row in gdvList.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    string OldClass = row.Cells[3].Text.Trim();
                    string OldGrade = row.Cells[4].Text.Trim();
                    string NewClass = row.Cells[6].Text.Trim();
                    string NewGrade = row.Cells[7].Text.Trim();
                    string _StudentAdmissionNumber = row.Cells[2].Text.Trim();
                    PASSIS.LIB.User getUserobj = pcon.Users.FirstOrDefault(x => x.AdmissionNumber == _StudentAdmissionNumber);
                    PASSIS.LIB.Grade oldGradeObj = pcon.Grades.FirstOrDefault(o => o.GradeName == OldGrade);
                    PASSIS.LIB.Class_Grade oldClassObj = pcon.Class_Grades.FirstOrDefault(c => c.Name == OldClass);
                    PASSIS.LIB.Grade newGradeObj = pcon.Grades.FirstOrDefault(o => o.GradeName == NewGrade);
                    PASSIS.LIB.Class_Grade newClassObj = pcon.Class_Grades.FirstOrDefault(x => x.Name == NewClass);
                    if (getUserobj != null && oldGradeObj != null && oldClassObj != null && newGradeObj == null && newClassObj == null)
                    {
                        PASSIS.LIB.GradeStudent oldGradeStudent = pcon.GradeStudents.FirstOrDefault(x => x.GradeId == oldGradeObj.Id && x.ClassId == oldClassObj.Id && x.StudentId == getUserobj.Id);
                        PASSIS.LIB.Alumni getAlumni = pcon.Alumnis.FirstOrDefault(x => x.StudentId == getUserobj.Id);
                        //PASSIS.LIB.GradeStudent getGradeStudent = pcon.GradeStudents.FirstOrDefault(s => s.GradeId == oldGradeObj.Id && s.ClassId == oldClassObj.Id && s.StudentId == oldGradeStudent.StudentId);
                        if (oldGradeStudent != null)
                        {
                            PASSIS.LIB.Grade getTeacher = pcon.Grades.FirstOrDefault(x => x.Id == oldGradeObj.Id);

                            oldGradeStudent.HasGraduated = true;
                            oldGradeStudent.StudentStatusId = 4;
                            pcon.SubmitChanges();
                            if (getAlumni == null)
                            {
                                PASSIS.LIB.Alumni alumniObj = new Alumni();
                                alumniObj.StudentId = oldGradeStudent.StudentId;
                                alumniObj.SchoolId = logonUser.SchoolId;
                                alumniObj.SchoolCampusId = logonUser.SchoolCampusId;
                                alumniObj.AcademicSessionId = oldGradeStudent.AcademicSessionId;
                                alumniObj.ClassId = oldGradeStudent.ClassId;
                                alumniObj.GradeId = oldGradeStudent.GradeId;
                                alumniObj.GradeTeacherId = oldGradeStudent.GradeTeacherId;
                                alumniObj.StudentStatusId = 4;
                                alumniObj.DateGraduated = DateTime.Now;

                                pcon.Alumnis.InsertOnSubmit(alumniObj);
                                pcon.SubmitChanges();
                            }
                            else
                            {
                                getAlumni.StudentId = oldGradeStudent.StudentId;
                                getAlumni.SchoolId = logonUser.SchoolId;
                                getAlumni.SchoolCampusId = logonUser.SchoolCampusId;
                                getAlumni.AcademicSessionId = oldGradeStudent.AcademicSessionId;
                                getAlumni.ClassId = oldGradeStudent.ClassId; ;
                                getAlumni.GradeId = oldGradeStudent.GradeId;
                                getAlumni.GradeTeacherId = oldGradeStudent.GradeTeacherId;
                                getAlumni.StudentStatusId = 4;
                                getAlumni.DateGraduated = DateTime.Now;

                                pcon.SubmitChanges();
                            }


                            PASSIS.LIB.User userObj = pcon.Users.FirstOrDefault(x => x.Id == oldGradeStudent.StudentId);
                            userObj.StudentStatus = false;
                            pcon.SubmitChanges();
                            //}
                            lblErrorMsg.Text = "Graduated successfully.";
                            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                            gdvList.Visible = false;
                            lblErrorMsg.Visible = true;
                        }
                        else
                        {
                            lblErrorMsg.Text = "Error in Getting GradeStudent with Admission Number: " + _StudentAdmissionNumber + " in this class";
                            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                            gdvList.Visible = false;
                            lblErrorMsg.Visible = true;
                            return;
                        }
                    }
                    else
                    {
                        if (getUserobj == null)
                        {
                            lblErrorMsg.Text = "This Admission Number: " + _StudentAdmissionNumber + " Doesn't exist";
                            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                            gdvList.Visible = false;
                            lblErrorMsg.Visible = true;
                            return;
                        }
                        else
                        {
                            lblErrorMsg.Text = "Error in Class or Grade Selection for" + " " + getUserobj.StudentFullName + " " + "with Admission Number:" + " " + getUserobj.AdmissionNumber;
                            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                            gdvList.Visible = false;
                            lblErrorMsg.Visible = true;
                            return;
                        }
                    }
                }
            }
        }

        //else if (Convert.ToInt64(ddlMove.SelectedValue) == 1)
        //{
        //    foreach (GridViewRow row in gdvMovetoAlumni.Rows)
        //    {
        //        if (row.RowType == DataControlRowType.DataRow)
        //        {
        //            string OldGrade = row.Cells[2].Text;
        //            string _StudentFullName = row.Cells[3].Text;
        //            PASSIS.LIB.Grade oldgradeObj = pcon.Grades.FirstOrDefault(o => o.GradeName == OldGrade);
        //            PASSIS.LIB.User getUserobj = pcon.Users.FirstOrDefault(x => x.StudentFullName == _StudentFullName);


        //            foreach (PASSIS.LIB.GradeStudent g in pcon.GradeStudents.Where(s => s.GradeId == oldgradeObj.Id && s.StudentId == getUserobj.Id))
        //            {
        //                PASSIS.LIB.Alumni logAlumi = new Alumni();
        //                logAlumi.ClassId = g.ClassId;
        //                logAlumi.GradeId = g.GradeId;
        //                logAlumi.StudentId = g.StudentId;
        //                logAlumi.GradeTeacherId = g.GradeTeacherId;
        //                logAlumi.AcademicSessionId = g.AcademicSessionId;
        //                logAlumi.SchoolId = g.SchoolId;
        //                logAlumi.SchoolCampusId = g.SchoolCampusId;
        //                logAlumi.DateGraduated = DateTime.Now;

        //                pcon.Alumnis.InsertOnSubmit(logAlumi);
        //                pcon.SubmitChanges();
        //            }
        //            foreach (PASSIS.LIB.GradeStudent g in pcon.GradeStudents.Where(s => s.GradeId == oldgradeObj.Id && s.StudentId == getUserobj.Id))
        //            {
        //                pcon.GradeStudents.DeleteOnSubmit(g);
        //                pcon.SubmitChanges();
        //            }




        //        }
        //    }

        //    lblErrorMsg.Text = "Student Successfully Graduated";
        //    lblErrorMsg.ForeColor = System.Drawing.Color.Green;
        //    gdvMovetoAlumni.Visible = false;
        //    lblErrorMsg.Visible = true;
        //}

        //}
        //catch (Exception exp) { throw exp; }
    }

    protected void ddlMove_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMove.SelectedIndex == 2)
        {
            btnMoveSelectedClasses.Text = "Graduate Selected Student";
        }
        else if (ddlMove.SelectedIndex == 1)
        {
            btnMoveSelectedClasses.Text = "Move Selected Classes";
        }
    }

    public class MoveStudentClass
    {
        public string serialNumber { get; set; }
        public string pupilFullname { get; set; }
        public string admissionNumber { get; set; }
        public string oldClass { get; set; }
        public string oldGrade { get; set; }
        public string gender { get; set; }
        public string newClass { get; set; }
        public string newGrade { get; set; }
    }
}