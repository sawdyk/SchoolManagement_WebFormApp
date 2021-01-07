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
//using Microsoft.Reporting.WebForms;

public partial class ParentsChildTopNPerformance : PASSIS.LIB.Utility.BasePage
{
    //public string IPSITE = ConfigurationManager.AppSettings["LOCALSITE"];
    //PASSISLIBDataContext context = new PASSISLIBDataContext();
    //public static string SchoolName = "";
    //public static string SchoolLogo = "";
    //public static string SchoolAddress = "";
    //public static string SchoolCurriculumId = "";
    //public static string SchoolUrl = "";
    //protected void Page_Load(object sender, EventArgs e)
    //{



    //    if (!IsPostBack)
    //    {
    //        BindDropDown();

    //        clsMyDB mdb = new clsMyDB();
    //        mdb.connct();
    //        string query = "SELECT * FROM Schools WHERE Id=" + logonUser.SchoolId;
    //        SqlDataReader reader = mdb.fetch(query);
    //        while (reader.Read())
    //        {
    //            SchoolName = reader[1].ToString();
    //            SchoolLogo = reader[5].ToString();
    //            SchoolAddress = reader[4].ToString();
    //            SchoolCurriculumId = reader[7].ToString();
    //            SchoolUrl = reader[6].ToString();
    //        }
    //        if (SchoolLogo == "") SchoolLogo = "~/Images/Schools_Logo/passis_logoB.png";
    //        if (SchoolCurriculumId == "") SchoolCurriculumId = "0";
    //    }
    //}
    //protected void ddlWard_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    ddlClass.Items.Clear();
    //    PASSIS.LIB.GradeStudent stdGrade = context.GradeStudents.FirstOrDefault(x => x.StudentId == Convert.ToInt64(ddlWard.SelectedValue));
    //    if (stdGrade != null)
    //    {
    //        var stdClass = from s in context.Class_Grades where s.Id == stdGrade.ClassId select s;
    //        ddlClass.DataSource = stdClass;
    //        ddlClass.DataBind();
    //        ddlClass.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select Class---", "0", true));
    //    }

    //}

    //protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //    Int64 selectedYearId = 0L;
    //    selectedYearId = Convert.ToInt64(ddlClass.SelectedValue);
    //    var availableGrades = new ClassGradeDAL().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, selectedYearId);
    //    ddlGrade.DataSource = availableGrades;
    //    ddlGrade.DataBind();
    //}

    //protected void BindDropDown()
    //{
    //    ddlWard.DataSource = new UsersLIB().RetrieveParentsChildren(logonUser.Id);
    //    ddlWard.DataBind();

    //    clsMyDB mdb = new clsMyDB();
    //    mdb.connct();

    //    string querySession = "SELECT DISTINCT AcademicSessionId FROM AcademicSession WHERE SchoolId=" + logonUser.SchoolId;
    //    SqlDataReader readerSession = mdb.fetch(querySession);
    //    while (readerSession.Read())
    //    {
    //        ddlSession.DataSource = from s in context.AcademicSessionNames
    //                                where s.ID == Convert.ToInt64(readerSession["AcademicSessionId"].ToString())
    //                                select s;
    //        ddlSession.DataBind();
    //    }

    //    readerSession.Close();
    //    mdb.closeConnct();

    //    ddlTerm.DataSource = new AcademicTermLIB().RetrieveAcademicTerm();
    //    ddlTerm.DataBind();
    //}

    //protected void btnViewReport_OnClick(object sender, EventArgs e)
    //{

    //    //try
    //    //{
    //    if (ddlWard.SelectedIndex == 0)
    //    {
    //        lblErrorMsg.Text = "Kindly select the Ward/Child";
    //        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
    //        lblErrorMsg.Visible = true;
    //        return;
    //    }

    //    if (ddlClass.SelectedIndex == 0)
    //    {
    //        lblErrorMsg.Text = "Kindly select class";
    //        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
    //        lblErrorMsg.Visible = true;
    //        return;
    //    }

    //    if (ddlSession.SelectedIndex == 0)
    //    {
    //        lblErrorMsg.Text = "Kindly select the session";
    //        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
    //        lblErrorMsg.Visible = true;
    //        return;
    //    }

    //    if (ddlTerm.SelectedIndex == 0)
    //    {
    //        lblErrorMsg.Text = "Kindly select the term";
    //        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
    //        lblErrorMsg.Visible = true;
    //        return;
    //    }


    //    long wardId = Convert.ToInt64(ddlWard.SelectedValue);
    //    long classId = Convert.ToInt64(ddlClass.SelectedValue);
    //    long sessionId = Convert.ToInt64(ddlSession.SelectedValue);
    //    long termId = Convert.ToInt64(ddlTerm.SelectedValue);
    //    long schoolId = (long)logonUser.SchoolId;
    //    long campusId = logonUser.SchoolCampusId;

    //    if (ddlSession.SelectedIndex != 0 && ddlClass.SelectedIndex != 0 && ddlWard.SelectedIndex != 0 && ddlTerm.SelectedIndex != 0)
    //    {
    //        long UserId = logonUser.Id; // The user(SubjectTeacherId/AdminId etc) ID

    //        long SchoolId = (long)logonUser.SchoolId; //The School ID

    //        long SchoolCampusId = logonUser.SchoolCampusId; //The School Campus ID

    //        //long termId = 1;

    //        //long sessionId = 15;

    //        //long classId = 30;
    //        //long yearId = 135;

    //        //long studentId = 5031;

    //        Response.Write(ddlWard.SelectedValue);
    //        Response.Write(SchoolId);
    //        Response.Write(SchoolCampusId);
    //        Response.Write(ddlTerm.SelectedValue);

    //        Response.Write(ddlSession.SelectedValue);
    //        Response.Write(ddlClass.SelectedValue);

    //        Response.Write(ddlGrade.SelectedValue);




    //        //ErrorLabel.Text = "";
    //        RVReport.Visible = true;
    //        //ShowData();
    //        //Response.Write("THIS IS WORKING FINE.");
    //        RVReport.Reset();
    //        ReportParameter[] param = new ReportParameter[8];
    //        //Response.Write( + "  " +  + " " +  + " " + ddlAcademicSession.SelectedItem.Value + " " +  + " " +  + " " + );
    //        param[0] = new ReportParameter("StudentID", ddlWard.SelectedValue.ToString());
    //        param[1] = new ReportParameter("SchoolID", SchoolId.ToString());
    //        param[2] = new ReportParameter("CampusID", SchoolCampusId.ToString());
    //        param[3] = new ReportParameter("TermID", ddlTerm.SelectedValue.ToString());
    //        param[4] = new ReportParameter("SessionID", ddlSession.SelectedValue.ToString());
    //        param[5] = new ReportParameter("ClassID", ddlClass.SelectedValue.ToString());
    //        param[6] = new ReportParameter("GradeID", ddlGrade.SelectedValue.ToString());
    //        param[7] = new ReportParameter("N", txtTopNSubject.Text.ToString());
    //        RVReport.ProcessingMode = ProcessingMode.Remote;
    //        RVReport.ServerReport.ReportServerUrl = new Uri(IPSITE);
    //        RVReport.ServerReport.ReportPath = "/parentsreport/TopNPerformance";
    //        RVReport.ServerReport.SetParameters(param);
    //        RVReport.ServerReport.Refresh();
    //        RVReport.ShowParameterPrompts = false;
    //        RVReport.ShowPrintButton = true;
    //    }
    //    else
    //    {
    //        //ErrorLabel.Text = "Please Ensure All Field are Selected Properly";
    //    }
    //    if (ddlTerm.SelectedIndex == 0)
    //    {
    //        //    ErrorLabel.Text = "Please Ensure All Field are Selected Properly";
    //    }
    //    else
    //    {
    //        //    ErrorLabel.Text = "";
    //    }
    //}



}