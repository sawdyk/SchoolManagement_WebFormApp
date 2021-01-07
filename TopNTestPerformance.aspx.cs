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

public partial class TopNTestPerformance : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext db = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {


        long UserId = logonUser.Id; // The user(SubjectTeacherId/AdminId etc) ID

        long SchoolId = (long)logonUser.SchoolId; //The School ID

        long SchoolCampusId = logonUser.SchoolCampusId; //The School Campus ID



        if (!IsPostBack)
        {

            //code to fetch the Class based on their curriculum
            long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
            long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
            ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
            ddlYear.DataBind();
            ddlYear.Items.Insert(0, new ListItem("--Select Year--", "0"));
            ddlWard.Items.Insert(0, new ListItem("--Select Student--", "0"));
            ddlClass.Items.Insert(0, new ListItem("--Select Class--", "0"));


            //code to fetch the Academic Sessions
            var session = (from c in db.AcademicSessions
                           where c.SchoolId == logonUser.SchoolId
                           orderby c.IsCurrent descending
                           select c.AcademicSessionName).Distinct();

            ddlsession.DataSource = session;
            ddlsession.DataTextField = "SessionName";
            ddlsession.DataValueField = "ID";
            ddlsession.DataBind();
            ddlsession.Items.Insert(0, new ListItem("---Select---", "0", true));

            //code to fetch the Academic Terms
            var term = from c in db.AcademicTerm1s
                       select c;
            ddlterm.DataSource = term;
            ddlterm.DataTextField = "AcademicTermName";
            ddlterm.DataValueField = "Id";
            ddlterm.DataBind();
            ddlterm.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0", true));

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
        ddlClass.Items.Insert(0, new ListItem("--Select Class--", "0"));

    }

    protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlWard.Items.Clear();
        BindGrid(Convert.ToInt64(ddlYear.SelectedValue), Convert.ToInt64(ddlClass.SelectedValue));

    }


    protected void BindGrid(long yearId, long gradeId)
    {
        long campusId, schoolId;
        campusId = logonUser.SchoolCampusId;
        schoolId = (long)logonUser.SchoolId;
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

        IList<PASSIS.LIB.GradeStudent> classList = new ClassGradeLIB().RetrieveGradeStudents(schoolId, campusId, yearId, gradeId,curSessionId);
        ddlWard.Items.Add(new ListItem("--Select Student--", "0"));
        foreach (PASSIS.LIB.GradeStudent studentList in classList)
        {
            PASSIS.LIB.User stdList = db.Users.FirstOrDefault(x => x.Id == studentList.StudentId);
            ddlWard.Items.Add(new ListItem(stdList.StudentFullName, stdList.Id.ToString()));
        }
    }



    protected void btnViewTopN_OnClick(object sender, EventArgs e)
    {


        int parsedValue;
        if (!int.TryParse(txtTopNExam.Text, out parsedValue))
        {
            lblErrorMsg.Text = "TOP(N) Field Must Contain Numbers Only!";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
        }

        if (txtTopNExam.Text == " ")
        {
            lblErrorMsg.Text = "TOP(N) Field is Required";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }





        //------------------------------------------------YOUR CODE TO VIEW REPORT GOES HERE--------------------------------------------//





    }




}