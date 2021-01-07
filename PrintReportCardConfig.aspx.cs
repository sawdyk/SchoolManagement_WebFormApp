using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.LIB.Utility;
using PASSIS.LIB;
using PASSIS.DAO;

public partial class PrintReportCardConfig : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            var academicSessions = (from c in context.AcademicSessions
                                    where c.SchoolId == logonUser.SchoolId
                                    orderby c.IsCurrent descending
                                    select new
                                    {
                                        c.AcademicSessionName.SessionName

                                    }).Distinct();

            ddlAcademicSession.DataSource = new PrintReportCardConfig().schSession().Distinct();
            ddlAcademicSession.DataTextField = "SessionName";
            ddlAcademicSession.DataValueField = "ID";
            ddlAcademicSession.DataBind();
            ddlAcademicSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Session--", "0", true));

            PASSIS.LIB.User currentUser = logonUser;
            long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
            long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
            if (curriculumId == (long)CurriculumType.British)
            {
                ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, 0);
                //ddlCurrentYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, 0);
            }
            else if (curriculumId == (long)CurriculumType.Nigerian)
            {
                ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
                //ddlCurrentYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
            }
            ddlYear.DataBind();

            var academicTermCurrent = (from c in context.AcademicSessions
                                       where c.IsCurrent == true
                                       select new
                                       {
                                           c.AcademicTerm1

                                       }).Distinct();


            ddlAcademicTerm.DataSource = new AcademicTermLIB().RetrieveAcademicTerm();
            ddlAcademicTerm.DataTextField = "AcademicTermName";
            ddlAcademicTerm.DataValueField = "Id";
            ddlAcademicTerm.DataBind();
            ddlAcademicTerm.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Term--", "0", true));

            ddlCategory.Items.Add(new ListItem("Select", "0"));
            ddlCategory.Items.Add(new ListItem("CA", "1"));
            ddlCategory.Items.Add(new ListItem("Exam", "2"));
            ddlCategory.Items.Add(new ListItem("Both", "3"));

        }


    }
    public enum CurriculumType
    {
        British = 1,
        Nigerian = 2
    }

    protected void ddlYear_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Int64 selectedYearId = 0L;
        selectedYearId = Convert.ToInt64(ddlYear.SelectedValue);
        ddlGrade.Items.Clear();
        var availableGrades = new ClassGradeDAL().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, selectedYearId);
        ddlGrade.DataSource = availableGrades;
        ddlGrade.DataBind();
        ddlGrade.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Grade--", "0", true));
    }

    public IList<AcademicSessionName> schSession()
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        var session = from c in context.AcademicSessions
                      where c.SchoolId == logonUser.SchoolId
                      orderby c.IsCurrent descending
                      select c.AcademicSessionName;
        return session.ToList<AcademicSessionName>();
    }

    public IList<AcademicTerm1> schTerm()
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        var session = from c in context.AcademicSessions
                      where c.SchoolId == logonUser.SchoolId
                      select c.AcademicTerm1;
        return session.ToList<AcademicTerm1>();
    }

    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chk = (sender as CheckBox);
            if (chk.ID == "chkAll")
            {
                foreach (GridViewRow row in gdvAllSubject.Rows)
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

    protected void ddlGrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddlSubject.Items.Clear();
        lblMsgDisplay.Visible = true;
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        IList<SubjectsInSchool> getId = new SubjectTeachersLIB().getAllSubjectsForClass((long)logonUser.SchoolId, Convert.ToInt64(ddlYear.SelectedValue));
        foreach (SubjectsInSchool subjId in getId)
        {
            PASSIS.LIB.Subject reqSubject = context.Subjects.FirstOrDefault(c => c.Id == subjId.SubjectId);
            //ddlSubject.Items.Add(new System.Web.UI.WebControls.ListItem(reqSubject.Name, reqSubject.Id.ToString()));

        }
        gdvAllSubject.DataSource = getId;
        gdvAllSubject.DataBind();
        gdvAllSubject.Visible = true;
    }

    public string subjectName(int subjectId)
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();

        PASSIS.LIB.Subject reqSubject = context.Subjects.FirstOrDefault(c => c.Id == subjectId);
        return reqSubject.Name;
    }

    protected void btnSaveConfig_OnClick(object sender, EventArgs e)
    {

        PASSIS.LIB.AcademicSession academicSession = context.AcademicSessions.FirstOrDefault(x => x.AcademicSessionId == Convert.ToInt64(ddlAcademicSession.SelectedValue) && x.AcademicTermId == Convert.ToInt64(ddlAcademicTerm.SelectedValue)
        && x.SchoolId == logonUser.SchoolId);

        if (academicSession != null && academicSession.IsClosed == true)
        {
            lblErrorMsg.Text = "";
            lblErrorMsg.Text = "This term has been closed for this session, Kindly contact Administrator!";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            return;
        }

        if (academicSession != null && academicSession.IsLocked == true)
        {
            lblErrorMsg.Text = "";
            lblErrorMsg.Text = "This term has been locked for this session, Kindly contact Administrator!";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            return;
        }


        if (ddlYear.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select year";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
        if (ddlGrade.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select class";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }

        if (ddlAcademicSession.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select session";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }

        if (ddlAcademicTerm.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select term";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }

        if (ddlCategory.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select Category";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }

        foreach (GridViewRow row in gdvAllSubject.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                bool isChecked = row.Cells[2].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                if (isChecked)
                {
                    string subjectId = row.Cells[3].Controls.OfType<Label>().FirstOrDefault().Text;

                    try
                    {
                        ReportCardPrintConfig checkExist = context.ReportCardPrintConfigs.FirstOrDefault(x => x.SchoolId == logonUser.SchoolId && x.CampusId == logonUser.SchoolCampusId
                        && x.ClassId == Convert.ToInt64(ddlYear.SelectedValue) && x.GradeId == Convert.ToInt64(ddlGrade.SelectedValue) && x.SessionId == Convert.ToInt64(ddlAcademicSession.SelectedValue)
                        && x.TermId == Convert.ToInt64(ddlAcademicTerm.SelectedValue) && x.SubjectId == Convert.ToInt64(subjectId));

                        if (checkExist != null)
                        {
                            //lblErrorMsg.Text = "Configuration Already exists, use panel below to edit!";
                            //lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                            //lblErrorMsg.Visible = true;
                            //return;

                            checkExist.SchoolId = logonUser.SchoolId;
                            checkExist.CampusId = logonUser.SchoolCampusId;
                            checkExist.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
                            checkExist.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                            checkExist.SessionId = Convert.ToInt64(ddlAcademicSession.SelectedValue);

                            if (ddlCategory.SelectedItem.Text == "CA")
                            {
                                checkExist.CA = true;
                                checkExist.Exam = false;
                            }
                            else if (ddlCategory.SelectedItem.Text == "Exam")
                            {
                                checkExist.CA = false;
                                checkExist.Exam = true;
                            }
                            else if (ddlCategory.SelectedItem.Text == "Both")
                            {
                                checkExist.CA = true;
                                checkExist.Exam = true;
                            }
                            else
                            {
                                checkExist.CA = true;
                                checkExist.Exam = true;
                            }
                            checkExist.SubjectId = Convert.ToInt64(subjectId);
                            checkExist.TermId = Convert.ToInt64(ddlAcademicTerm.SelectedValue);
                            checkExist.DateCreated = DateTime.Now;
                            context.SubmitChanges();

                            lblErrorMsg.Text = "Saved Successfully!";
                            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                            lblErrorMsg.Visible = true;

                        }
                        else
                        {
                            ReportCardPrintConfig rptConfig = new ReportCardPrintConfig();
                            rptConfig.SchoolId = logonUser.SchoolId;
                            rptConfig.CampusId = logonUser.SchoolCampusId;
                            rptConfig.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
                            rptConfig.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                            rptConfig.SessionId = Convert.ToInt64(ddlAcademicSession.SelectedValue);

                            if (ddlCategory.SelectedItem.Text == "CA")
                            {
                                rptConfig.CA = true;
                                rptConfig.Exam = false;
                            }
                            else if (ddlCategory.SelectedItem.Text == "Exam")
                            {
                                rptConfig.CA = false;
                                rptConfig.Exam = true;
                            }
                            else if (ddlCategory.SelectedItem.Text == "Both")
                            {
                                rptConfig.CA = true;
                                rptConfig.Exam = true;
                            }
                            else
                            {
                                rptConfig.CA = true;
                                rptConfig.Exam = true;
                            }
                            rptConfig.SubjectId = Convert.ToInt64(subjectId);
                            rptConfig.TermId = Convert.ToInt64(ddlAcademicTerm.SelectedValue);
                            rptConfig.DateCreated = DateTime.Now;

                            context.ReportCardPrintConfigs.InsertOnSubmit(rptConfig);
                            context.SubmitChanges();

                            lblErrorMsg.Text = "Saved Successfully!";
                            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                            lblErrorMsg.Visible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
            }
        }








        //PASSISLIBDataContext context = new PASSISLIBDataContext();

        //try
        //{
        //    ReportCardPrintConfig checkExist = context.ReportCardPrintConfigs.FirstOrDefault(x=>x.SchoolId == logonUser.SchoolId && x.CampusId == logonUser.SchoolCampusId 
        //    && x.ClassId == Convert.ToInt64(ddlYear.SelectedValue) && x.GradeId == Convert.ToInt64(ddlGrade.SelectedValue) && x.SessionId == Convert.ToInt64(ddlAcademicSession.SelectedValue)
        //    && x.TermId == Convert.ToInt64(ddlAcademicTerm.SelectedValue));

        //    if (checkExist != null)
        //    {
        //        lblErrorMsg.Text = "Configuration Already exists, use panel below to edit!";
        //        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
        //        lblErrorMsg.Visible = true;
        //        return;
        //    }
        //    else
        //    {
        //        ReportCardPrintConfig rptConfig = new ReportCardPrintConfig();
        //        rptConfig.SchoolId = logonUser.SchoolId;
        //        rptConfig.CampusId = logonUser.SchoolCampusId;
        //        rptConfig.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
        //        rptConfig.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
        //        rptConfig.SessionId = Convert.ToInt64(ddlAcademicSession.SelectedValue);

        //        if (ddlCategory.SelectedItem.Text == "CA")
        //        {
        //            rptConfig.CA = true;
        //            rptConfig.Exam = false;
        //        }
        //        else if (ddlCategory.SelectedItem.Text == "Exam")
        //        {
        //            rptConfig.CA = false;
        //            rptConfig.Exam = true;
        //        }
        //        else if (ddlCategory.SelectedItem.Text == "Both")
        //        {
        //            rptConfig.CA = true;
        //            rptConfig.Exam = true;
        //        }
        //        else
        //        {
        //            rptConfig.CA = true;
        //            rptConfig.Exam = true;
        //        }
        //        rptConfig.TermId = Convert.ToInt64(ddlAcademicTerm.SelectedValue);
        //        rptConfig.UploadedById = logonUser.Id;
        //        rptConfig.DateCreated = DateTime.Now;

        //        context.ReportCardPrintConfigs.InsertOnSubmit(rptConfig);
        //        context.SubmitChanges();

        //        lblErrorMsg.Text = "Saved Successfully!";
        //        lblErrorMsg.ForeColor = System.Drawing.Color.Green;
        //        lblErrorMsg.Visible = true;
        //    }
        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}
    }
}