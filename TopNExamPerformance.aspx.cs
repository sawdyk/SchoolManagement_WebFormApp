using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;
using PASSIS.LIB.Utility;
using System.Configuration;
using Microsoft.Reporting.WebForms;

public partial class TopNExamPErformance : PASSIS.LIB.Utility.BasePage
{
    public string IPSITE = ConfigurationManager.AppSettings["LOCALSITE"];
    PASSISLIBDataContext db = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {


        long UserId = logonUser.Id; // The user(SubjectTeacherId/AdminId etc) ID

        long SchoolId = (long)logonUser.SchoolId; //The School ID

        long SchoolCampusId = logonUser.SchoolCampusId; //The School Campus ID



        if (!IsPostBack)
        {
            //LoadCampus();
            //code to fetch the Class based on their curriculum
            long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
            long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
            ddlClass.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("--Select Year--", "0"));
            ddlWard.Items.Insert(0, new ListItem("--Select Student--", "0"));
            ddlGrade.Items.Insert(0, new ListItem("--Select Grade--", "0"));

            //code to fetch campus
            Int64 AkokaCampus = 93;
            Int64 IdiArabaCampus = 156;



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


    protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int64 selectedYearId = 0L;
        selectedYearId = Convert.ToInt64(ddlClass.SelectedValue);
        ddlGrade.Items.Clear();
        var availableGrades = new ClassGradeDAL().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, selectedYearId);
        ddlGrade.DataSource = availableGrades;
        ddlGrade.DataBind();
        ddlGrade.Items.Insert(0, new ListItem("--Select Class--", "0"));

    }

    protected void ddlGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlWard.Items.Clear();
        BindGrid(Convert.ToInt64(ddlClass.SelectedValue), Convert.ToInt64(ddlGrade.SelectedValue));

    }



    protected void BindGrid(long yearId, long gradeId)
    {
        long campusId, schoolId;
        campusId = logonUser.SchoolCampusId;
        schoolId = (long)logonUser.SchoolId;
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

        IList<PASSIS.LIB.GradeStudent> classList = new ClassGradeLIB().RetrieveGradeStudents(schoolId, campusId, yearId, gradeId, curSessionId);
        ddlWard.Items.Add(new ListItem("--Select Student--", "0"));
        foreach (PASSIS.LIB.GradeStudent studentList in classList)
        {
            PASSIS.LIB.User stdList = db.Users.FirstOrDefault(x => x.Id == studentList.StudentId);
            ddlWard.Items.Add(new ListItem(stdList.StudentFullName, stdList.Id.ToString()));
        }
    }



    protected void ViewReports_Click(object sender, EventArgs e)
    {
        if (ddlsession.SelectedIndex != 0 && ddlGrade.SelectedIndex != 0 && ddlClass.SelectedIndex != 0 && ddlWard.SelectedIndex != 0 && ddlterm.SelectedIndex != 0)
        {
            long UserId = logonUser.Id; // The user(SubjectTeacherId/AdminId etc) ID

            //long SchoolId = (long)logonUser.SchoolId; //The School ID

            //long SchoolCampusId = logonUser.SchoolCampusId; //The School Campus ID



            long SchoolId = (long)logonUser.SchoolId; //The School ID

            long SchoolCampusId = logonUser.SchoolCampusId; //The School Campus ID

           // long termId = 2;

           // long sessionId = 17;

           // long classId = 30;
           //long yearId = 301;

           // long studentId = 11886;
           // long topN = 3;
            //ErrorLabel.Text = "";
            RVReport.Visible = true;
            //ShowData();
            //Response.Write("THIS IS WORKING FINE.");
            RVReport.Reset();
            ReportParameter[] param = new ReportParameter[8];
            //Response.Write( + "  " +  + " " +  + " " + ddlAcademicSession.SelectedItem.Value + " " +  + " " +  + " " + );
            param[0] = new ReportParameter("StudentID", ddlWard.SelectedValue.ToString());
            param[1] = new ReportParameter("SchoolID", SchoolId.ToString());
            param[2] = new ReportParameter("CampusID", SchoolCampusId.ToString());
            param[3] = new ReportParameter("TermID", ddlterm.SelectedValue.ToString());
            param[4] = new ReportParameter("SessionID", ddlsession.SelectedValue.ToString());
            param[5] = new ReportParameter("ClassID", ddlClass.SelectedValue.ToString());
            param[6] = new ReportParameter("GradeID", ddlGrade.SelectedValue.ToString());
            param[7] = new ReportParameter("N", txtTopNSubject.Text.ToString());
            RVReport.ProcessingMode = ProcessingMode.Remote;
            RVReport.ServerReport.ReportServerUrl = new Uri(IPSITE);
            RVReport.ServerReport.ReportPath = "/parentsreport/TopNPerformance";
            RVReport.ServerReport.SetParameters(param);
            RVReport.ServerReport.Refresh();
            RVReport.ShowParameterPrompts = false;
            RVReport.ShowPrintButton = true;
        }
        else
        {
            //ErrorLabel.Text = "Please Ensure All Field are Selected Properly";
        }
        if (ddlterm.SelectedIndex == 0)
        {
            //ErrorLabel.Text = "Please Ensure All Field are Selected Properly";
        }
        else
        {
            //ErrorLabel.Text = "";
        }
    }






}