using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;
using System.Configuration;
using System.Web.Configuration;
using PASSIS.LIB.Utility;
using PASSIS.DAO.CustomClasses;
//using Microsoft.Reporting.WebForms;

public partial class TeacherExamPerformance : PASSIS.LIB.Utility.BasePage
{
    //public string IPSITE = ConfigurationManager.AppSettings["LOCALSITE"];
    //PASSISLIBDataContext db = new PASSISLIBDataContext();
    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    if (!IsPostBack)
    //    {
    //        //LoadCampus();
    //        //code to fetch the Class based on their curriculum
    //        long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
    //        long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
    //        ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
    //        ddlYear.DataBind();
    //        ddlYear.Items.Insert(0, new ListItem("--Select Year--", "0"));
    //        ddlClass.Items.Insert(0, new ListItem("--Select Class--", "0"));

    //        //code to fetch the Academic Sessions
    //        var session = (from c in db.AcademicSessions
    //                       where c.SchoolId == logonUser.SchoolId
    //                       orderby c.IsCurrent descending
    //                       select c.AcademicSessionName).Distinct();

    //        ddlsession.DataSource = session;
    //        ddlsession.DataTextField = "SessionName";
    //        ddlsession.DataValueField = "ID";
    //        ddlsession.DataBind();
    //        ddlsession.Items.Insert(0, new ListItem("---Select---", "0", true));

    //        //code to fetch the Academic Terms
    //        var term = from c in db.AcademicTerm1s
    //                   select c;
    //        ddlterm.DataSource = term;
    //        ddlterm.DataTextField = "AcademicTermName";
    //        ddlterm.DataValueField = "Id";
    //        ddlterm.DataBind();
    //        ddlterm.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0", true));

    //    }
    //}


    //protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Int64 selectedYearId = 0L;
    //    selectedYearId = Convert.ToInt64(ddlYear.SelectedValue);
    //    ddlClass.Items.Clear();
    //    var availableGrades = new ClassGradeDAL().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, selectedYearId);
    //    ddlClass.DataSource = availableGrades;
    //    ddlClass.DataBind();
    //    ddlClass.Items.Insert(0, new ListItem("--Select Class--", "0"));

    //}

   

    //protected void ViewReports_Click(object sender, EventArgs e)
    //{
    //    if (ddlsession.SelectedIndex != 0 && ddlClass.SelectedIndex != 0 && ddlYear.SelectedIndex != 0 &&  ddlterm.SelectedIndex != 0)
    //    {
    //        long UserId = logonUser.Id; // The Teacher ID

    //        long SchoolId = (long)logonUser.SchoolId; //The School ID

    //        long SchoolCampusId = logonUser.SchoolCampusId; //The School Campus ID

          
    //        RVReport.Visible = true;
    //        RVReport.Reset();
    //        ReportParameter[] param = new ReportParameter[6];
    //        param[0] = new ReportParameter("SchoolID", SchoolId.ToString());
    //        param[1] = new ReportParameter("CampusID", SchoolCampusId.ToString());
    //        param[2] = new ReportParameter("TermID", ddlterm.SelectedValue.ToString());
    //        param[3] = new ReportParameter("SessionID", ddlsession.SelectedValue.ToString());
    //        param[4] = new ReportParameter("ClassID", ddlClass.SelectedValue.ToString());
    //        param[5] = new ReportParameter("YearID", ddlYear.SelectedValue.ToString());
    //        RVReport.ProcessingMode = ProcessingMode.Remote;
    //        RVReport.ServerReport.ReportServerUrl = new Uri(IPSITE);
    //        RVReport.ServerReport.ReportPath = "/adminreport/MAXComparismTestPerformance";
    //        RVReport.ServerReport.SetParameters(param);
    //        RVReport.ServerReport.Refresh();
    //        RVReport.ShowParameterPrompts = false;
    //        RVReport.ShowPrintButton = true;
    //    }
    //    else
    //    {
    //        //ErrorLabel.Text = "Please Ensure All Field are Selected Properly";
    //    }
    //    if (ddlterm.SelectedIndex == 0)
    //    {
    //        //ErrorLabel.Text = "Please Ensure All Field are Selected Properly";
    //    }
    //    else
    //    {
    //        //ErrorLabel.Text = "";
    //    }
    //}






}