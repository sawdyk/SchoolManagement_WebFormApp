using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;
using PASSIS.LIB.Utility;
using PASSIS.DAO.CustomClasses;
using System.Configuration;
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

public partial class UploadStudentImage : PASSIS.LIB.Utility.BasePage
{

    PASSISLIBDataContext dbcontext = new PASSISLIBDataContext();
    private static int maximumFileSize = 4194304; //4MB

    long CurriculumID;
    long SchoolID;
    public static string SchoolName = "";
    public static string SchoolLogo = "";
    public static string SchoolAddress = "";
    public static string SchoolCurriculumId = "";
    public static string SchoolUrl = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {

            //code to fetch the Class based on their curriculum
            long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
            long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
            ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
            ddlYear.DataBind();
            ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Year--", "0"));
            ddlStudent.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Student--", "0"));
            ddlClass.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Class--", "0"));

            //code to fetch the Academic Sessions
            //var session = (from c in dbcontext.AcademicSessions
            //               where c.SchoolId == logonUser.SchoolId
            //               orderby c.IsCurrent descending
            //               select c.AcademicSessionName).Distinct();

            //ddlSession.DataSource = session;
            //ddlSession.DataTextField = "SessionName";
            //ddlSession.DataValueField = "ID";
            //ddlSession.DataBind();
            //ddlSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0", true));

            //code to fetch the Academic Terms
            //var term = from c in dbcontext.AcademicTerm1s
            //           select c;
            //ddlTerm.DataSource = term;
            //ddlTerm.DataTextField = "AcademicTermName";
            //ddlTerm.DataValueField = "Id";
            //ddlTerm.DataBind();
            //ddlTerm.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0", true));

            // To get the school detail
            clsMyDB mdb = new clsMyDB();
            mdb.connct();
            string query = "SELECT * FROM Schools WHERE Id=" + logonUser.SchoolId;
            SqlDataReader reader = mdb.fetch(query);
            while (reader.Read())
            {
                SchoolName = reader[1].ToString();
                SchoolLogo = reader[5].ToString();
                SchoolAddress = reader[4].ToString();
                SchoolCurriculumId = reader[7].ToString();
                SchoolUrl = reader[6].ToString();
            }
            if (SchoolLogo == "") SchoolLogo = "~/Images/Schools_Logo/passis_logoB.png";
            if (SchoolCurriculumId == "") SchoolCurriculumId = "0";
        }

    }


    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int64 selectedYearId = 0L;
        selectedYearId = Convert.ToInt64(ddlYear.SelectedValue);
        ddlClass.Items.Clear();
        var availableGrades = new ClassGradeDAL().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, selectedYearId);
        ddlClass.DataSource = availableGrades;
        ddlClass.DataBind();
        ddlClass.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Class--", "0"));


    }


    protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlStudent.Items.Clear();
        BindGrid(Convert.ToInt64(ddlYear.SelectedValue), Convert.ToInt64(ddlClass.SelectedValue));

    }

    protected void BindGrid(long yearId, long gradeId)
    {
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

        long campusId, schoolId;
        campusId = logonUser.SchoolCampusId;
        schoolId = (long)logonUser.SchoolId;
        IList<PASSIS.LIB.GradeStudent> classList = new ClassGradeLIB().RetrieveGradeStudents(schoolId, campusId, yearId, gradeId, curSessionId);
        ddlStudent.Items.Add(new System.Web.UI.WebControls.ListItem("--Select Student--", "0"));
        foreach (PASSIS.LIB.GradeStudent studentList in classList)
        {
            PASSIS.LIB.User stdList = dbcontext.Users.FirstOrDefault(x => x.Id == studentList.StudentId);
            ddlStudent.Items.Add(new System.Web.UI.WebControls.ListItem(stdList.StudentFullName, stdList.Id.ToString()));
        }
    }


    protected void btnImageUpload_Click(object sender, EventArgs e)
    {
        try
        {

            if (ddlYear.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Select the Year";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }

            if (ddlClass.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Select the Class";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            if (ddlStudent.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Select the Student Name";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }

            if (ImageFile.HasFile)
            {
                string originalFileName = Path.GetFileName(ImageFile.PostedFile.FileName);
                string modifiedFileName = string.Format("{0}{1}", DateTime.Now.ToString("MMddHmmss"), originalFileName);
                string fileExtensionLetter = Path.GetExtension(ImageFile.PostedFile.FileName);
                int fileSizeLetter = ImageFile.PostedFile.ContentLength;
                string fileLocation = Server.MapPath("~/Passports/") + originalFileName;


                //Check the file extension

                if (!fileExtensionLetter.ToLower().Equals(".jpg") && !fileExtensionLetter.ToLower().Equals(".png") && !fileExtensionLetter.ToLower().Equals(".gif") && !fileExtensionLetter.ToLower().Equals(".jpeg") && !fileExtensionLetter.ToLower().Equals(".tiff"))
                {
                    lblErrorMsg.Text = "Invalid file format for " + originalFileName + " letter";
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                else
                {
                    if (fileSizeLetter > maximumFileSize)
                    {
                        lblErrorMsg.Text = "4MB file size exceeded for attached documentation";
                        lblErrorMsg.Visible = true;
                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                }

                //    PASSIS.LIB.School school = dbcontext.Schools.FirstOrDefault(x => x.Id == logonUser.SchoolId);
                //    ImageFile.SaveAs(fileLocation);
                //    school.Logo = "~/SchoolLogo/" + originalFileName;


                ////Check the file extension;
                //dbcontext.SubmitChanges();

                PASSIS.LIB.User studentPassport = dbcontext.Users.FirstOrDefault(x => x.Id == Convert.ToInt64(ddlStudent.SelectedValue));
                ImageFile.SaveAs(fileLocation);
                //studentPassport.PassportFileName = "~/Passport/" + originalFileName;
                studentPassport.PassportFileName = "~/Passports/" + originalFileName;
                dbcontext.SubmitChanges();


                //lblResult.Visible = true;
                lblErrorMsg.Text = string.Format("Uploaded Successfully.");
                lblErrorMsg.Visible = true;
                lblErrorMsg.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblErrorMsg.Text = "Kindly select the Student image";
                lblErrorMsg.Visible = true;
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "An Error occurred, Make Sure All Fields are Selected!";
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }
  }
