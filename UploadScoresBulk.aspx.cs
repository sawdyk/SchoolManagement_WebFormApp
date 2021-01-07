﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO;
using PASSIS.DAO.Utility;
using System.IO;
using PASSIS.LIB;
using System.Data.SqlClient;
using System.Data;


public partial class UploadScoresBulk : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        //BindExcelScores();
        if (!IsPostBack)
        {
            if (!isUserClassTeacher)
            {
                PASSIS.LIB.Grade grd = getLogonTeacherGrade;
                //if (grd != null)
                //{
                //}
                int SessionId = 0;
                clsMyDB mdb = new clsMyDB();
                mdb.connct();

                ddlSession.DataSource = new UploadScoresBulk().schSession().Distinct();
                ddlSession.DataTextField = "SessionName";
                ddlSession.DataValueField = "ID";
                ddlSession.DataBind();
                ddlSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Session--", "0", true));

                long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
                long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
                ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
                ddlYear.DataBind();


                var getTestTemplate = from t in context.TestAssigenmentBroadSheetTemplates
                                      where t.TeacherId == logonUser.Id
                                      select new
                                      {
                                          t.BroadSheetDescriptionCode.DescriptionName
                                      };
                ddlDescription.DataSource = getTestTemplate;
                ddlDescription.DataTextField = "DescriptionName";
                ddlDescription.DataBind();
                ddlDescription.Items.Insert(0, new ListItem("--Select--", "0", true));

                ddlTerm.DataSource = new AcademicTermLIB().RetrieveAcademicTerm();
                ddlTerm.DataBind();
                ddlTerm.Items.Insert(0, new ListItem("--Select--", "0", true));
            }
            else
            {
                int SessionId = 0;
                clsMyDB mdb = new clsMyDB();
                mdb.connct();
                //string query = "SELECT DISTINCT AcademicSessionId FROM AcademicSession WHERE SchoolId=" + logonUser.SchoolId;
                //SqlDataReader reader = mdb.fetch(query);
                //while (reader.Read())
                //{
                //    ddlSession.DataSource = from s in context.AcademicSessionNames
                //                            where s.ID == Convert.ToInt64(reader["AcademicSessionId"].ToString())
                //                            select s;
                //    ddlSession.DataBind();
                //    ddlSession.Items.Insert(0, new ListItem("--Select--", "0", true));
                //}
                //reader.Close();
                //mdb.closeConnct();

                ddlSession.DataSource = new UploadScoresBulk().schSession().Distinct();
                ddlSession.DataTextField = "SessionName";
                ddlSession.DataValueField = "ID";
                ddlSession.DataBind();
                ddlSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Session--", "0", true));

                long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
                long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
                ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
                ddlYear.DataBind();


                var getTestTemplate = from t in context.TestAssigenmentBroadSheetTemplates
                                      where t.TeacherId == logonUser.Id
                                      select new
                                      {
                                          t.BroadSheetDescriptionCode.DescriptionName
                                      };
                ddlDescription.DataSource = getTestTemplate;
                ddlDescription.DataTextField = "DescriptionName";
                ddlDescription.DataBind();
                ddlDescription.Items.Insert(0, new ListItem("--Select--", "0", true));

                ddlTerm.DataSource = new AcademicTermLIB().RetrieveAcademicTerm();
                ddlTerm.DataBind();
                ddlTerm.Items.Insert(0, new ListItem("--Select--", "0", true));


                ////send a message that the logon user will not be able to view certain section of this page bcos he's not a class teacher 
            }
        }
    }
    public IList<AcademicSessionName> schSession()
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        var session = from c in context.AcademicSessions
                      where c.SchoolId == logonUser.SchoolId && c.IsCurrent == true
                      orderby c.IsCurrent descending
                      select c.AcademicSessionName;
        return session.ToList<AcademicSessionName>();
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        long yearId = Convert.ToInt64(ddlYear.SelectedValue);
        long sessionId = Convert.ToInt64(ddlSession.SelectedValue);
        long termId = Convert.ToInt64(ddlTerm.SelectedValue);
        ddlGrade.Items.Clear();
        var availableGrades = new ClassGradeLIB().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, yearId);
        ddlGrade.DataSource = availableGrades;
        ddlGrade.DataBind();
        ddlGrade.Items.Insert(0, new ListItem("--Select--", "0", true));

        //Populate Category
        ddlCategory.Items.Clear();
        var categoryList = from s in context.ScoreCategoryConfigurations where s.ClassId == yearId && s.SessionId == sessionId && s.TermId == termId && s.SchoolId == logonUser.SchoolId && s.CampusId == logonUser.SchoolCampusId select s;
        ddlCategory.DataSource = categoryList;
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, new ListItem("--Select--", "0", true));
    }
    protected void ddlGrade_SelectedIndexChanged(object sender, EventArgs e)
    {

    }


    public string getStudentFullName(long studentId)
    {
        PASSIS.LIB.User fullName = context.Users.FirstOrDefault(x=>x.Id == studentId);
        return fullName.StudentFullName;
    }

    public string getSubjectName(long subjectId)
    {
        PASSIS.LIB.Subject subjName = context.Subjects.FirstOrDefault(x => x.Id == subjectId);
        return subjName.Name;
    }

    protected void BindExcelScores()
    {
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

        if (ddlCategory.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select category";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
        if (ddlSubCategory.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select sub category";
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

        if (ddlTerm.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select term";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
        if (ddlDescription.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Description Template  is required";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }


        TestAssigenmentBroadSheetTemplate broadsheettemplate = context.TestAssigenmentBroadSheetTemplates.FirstOrDefault(x => x.BroadSheetDescriptionCode.DescriptionName == ddlDescription.SelectedValue);

        IList<StudentScoreTemporary> tempScore = (from sc in context.StudentScoreTemporaries
                                                  where sc.ClassId == Convert.ToInt64(ddlYear.SelectedValue) && sc.GradeId == Convert.ToInt64(ddlGrade.SelectedValue)
                                                  && sc.TermId == Convert.ToInt64(ddlTerm.SelectedValue) && sc.AcademicSessionID == Convert.ToInt64(ddlSession.SelectedValue)
                                                  && sc.BroadSheetTemplateID == broadsheettemplate.ID && sc.SubCategoryId == Convert.ToInt64(ddlSubCategory.SelectedValue)
                                                  && sc.CategoryId == Convert.ToInt64(ddlCategory.SelectedValue)
                                                  && sc.UploadedById == logonUser.Id && sc.SchoolId == logonUser.SchoolId && sc.CampusId == logonUser.SchoolCampusId
                                                  select sc).ToList();

        if (tempScore.Count > 0)
        {
            lblresltt.Visible = true;
            gdvViewExtendedScores.DataSource = tempScore;
            gdvViewExtendedScores.DataBind();
        }

        else
        {
            lblErrorMsg.Text = "No Scores Uploaded!";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;

        }
    }

    protected void btnViewScores_Click(object sender, EventArgs e)
    {

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

        if (ddlCategory.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select category";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
        if (ddlSubCategory.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select sub category";
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

        if (ddlTerm.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select term";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
        if (ddlDescription.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Description Template  is required";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }

        TestAssigenmentBroadSheetTemplate broadsheettemplate = context.TestAssigenmentBroadSheetTemplates.FirstOrDefault(x => x.BroadSheetDescriptionCode.DescriptionName == ddlDescription.SelectedValue);

        IList<StudentScoreTemporary> tempScore = (from sc in context.StudentScoreTemporaries
                                                  where sc.ClassId == Convert.ToInt64(ddlYear.SelectedValue) && sc.GradeId == Convert.ToInt64(ddlGrade.SelectedValue)
                                                  && sc.TermId == Convert.ToInt64(ddlTerm.SelectedValue) && sc.AcademicSessionID == Convert.ToInt64(ddlSession.SelectedValue)
                                                  && sc.BroadSheetTemplateID == broadsheettemplate.ID && sc.SubCategoryId == Convert.ToInt64(ddlSubCategory.SelectedValue)
                                                  && sc.CategoryId == Convert.ToInt64(ddlCategory.SelectedValue)
                                                  && sc.UploadedById == logonUser.Id && sc.SchoolId == logonUser.SchoolId && sc.CampusId == logonUser.SchoolCampusId
                                                  select sc).ToList();

        if (tempScore.Count > 0)
        {
            btnSaveScore.Visible = true;
        }
        else
        {
            btnSaveScore.Visible = false;
        }                                                                                                                                                                                                                                                                                                                                                
        BindExcelScores();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        btnUploadScores.Visible = true;
        btnViewScores.Visible = false;
        btnCancel.Visible = false;
        btnSaveScore.Visible = false;
        lblresltt.Visible = false;
        IList<StudentScoreTemporary> tempScore = (from sc in context.StudentScoreTemporaries
                                                  where sc.UploadedById == logonUser.Id && sc.SchoolId == logonUser.SchoolId && sc.CampusId == logonUser.SchoolCampusId
                                                  select sc).ToList();

        
            foreach (StudentScoreTemporary score in tempScore)
            {

                PASSIS.LIB.StudentScoreTemporary temppScore = context.StudentScoreTemporaries.FirstOrDefault(x => x.UploadedById == score.UploadedById && x.SchoolId == score.SchoolId && x.CampusId == score.CampusId);
            if (temppScore != null)
            {
                context.StudentScoreTemporaries.DeleteOnSubmit(temppScore);
                context.SubmitChanges();
            }
                lblErrorMsg.Text = "Scores Uploaded cancelled Successfully!";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
            }

        //BindExcelScores();
        gdvViewExtendedScores.DataBind();
    }

    protected void gdvViewExtendedScores_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvViewExtendedScores.PageIndex = e.NewPageIndex;
        BindExcelScores();
    }


    protected void btnUploadScores_Click(object sender, EventArgs e)
    {

        try
        {         
              //btnSaveScore.Visible = true;
              PASSIS.LIB.AcademicSession academicSession = context.AcademicSessions.FirstOrDefault(x => x.AcademicSessionId == Convert.ToInt64(ddlSession.SelectedValue) && x.AcademicTermId == Convert.ToInt64(ddlTerm.SelectedValue)
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

            if (ddlCategory.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select category";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            if (ddlSubCategory.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select sub category";
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
            if (txtTotalMark.Text.Trim() == "")
            {
                lblErrorMsg.Text = "Mark Obtainable is Missing";
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
            if (ddlDescription.SelectedIndex == -1)
            {
                lblErrorMsg.Text = "Description Template  is required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }

            Int64 yearId = Convert.ToInt64(ddlYear.SelectedValue);
            Int64 gradeId = Convert.ToInt64(ddlGrade.SelectedValue);
            Int64 UploadedById = logonUser.Id;
            //Int64 departmentId = 0;
            Int64 sessionId = Convert.ToInt64(ddlSession.SelectedValue);
            Int64 termId = Convert.ToInt64(ddlTerm.SelectedValue);

            ScoreCategoryConfiguration scoreCategory = context.ScoreCategoryConfigurations.FirstOrDefault(x => x.ClassId == Convert.ToInt64(ddlYear.SelectedValue) && x.Category == ddlCategory.SelectedItem.Text && x.SessionId == sessionId && x.TermId == termId && x.SchoolId == logonUser.SchoolId && x.CampusId == logonUser.SchoolCampusId);
            if (scoreCategory == null)
            {
                lblErrorMsg.Text = "Kindly set the score category";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            PASSISLIBDataContext db = new PASSISLIBDataContext();

            if (ddlCategory.SelectedItem.Text == "Exam")
            {
                ScoreSubCategoryConfiguration subCategory = context.ScoreSubCategoryConfigurations.FirstOrDefault(x => x.Id == Convert.ToInt64(ddlSubCategory.SelectedValue));
                if (subCategory == null)
                {
                    lblErrorMsg.Text = "Kindly set the score sub category";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                    return;
                }
                if (txtCode.Text == "")
                {
                    lblErrorMsg.Text = "Kindly enter the code";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                    return;
                }




                string TemplateCode = ddlDescription.SelectedItem.Text.ToString();

                //confirm if file to be uploaded has been selected else notify the user to select file to upload
                if (documentUpload.HasFile)
                {

                    string originalFileName = Path.GetFileName(documentUpload.PostedFile.FileName);
                    string fileExtension = Path.GetExtension(documentUpload.PostedFile.FileName);

                    //This is use to confirm if user is uploading against the previously generated template
                    TestAssigenmentBroadSheetTemplate broadsheettemplate = db.TestAssigenmentBroadSheetTemplates.FirstOrDefault(x => x.BroadSheetDescriptionCode.DescriptionName == TemplateCode);

                    //If template exist
                    if (broadsheettemplate != null)
                    {
                        //Confirm if the result has been uploaded
                        if (broadsheettemplate.HasSubmitted == false)
                        {
                            string[] subjectId = broadsheettemplate.SubjectId.Split(',');


                            string fileLocation = Server.MapPath("~/docs/ScoreSheets/") + originalFileName;
                            documentUpload.SaveAs(fileLocation);
                            IList<PASSIS.LIB.StudentScore> scoresFound = new List<PASSIS.LIB.StudentScore>();
                            Net.SourceForge.Koogra.Excel.Workbook wkbk = new Net.SourceForge.Koogra.Excel.Workbook(fileLocation);
                            Net.SourceForge.Koogra.Excel.Worksheet workSheet = wkbk.Sheets[0];

                            // long count = 0;
                            for (uint rowCount = workSheet.Rows.MinRow; rowCount <= workSheet.Rows.MaxRow; ++rowCount)
                            {
                                try
                                {


                                    if (rowCount != workSheet.Rows.MinRow)// jump the first row
                                    {

                                        //broadsheettemplate.TotalNumberofSubjectInserted = count;
                                        uint count = 0;
                                        uint newunit = 3;
                                        uint countnum;
                                        foreach (string subId in subjectId)
                                        {
                                            PASSIS.LIB.SubjectTeacher subTeacher = context.SubjectTeachers.FirstOrDefault(x => x.SubjectId == Convert.ToInt64(subId));
                                            PASSIS.LIB.Subject subject = context.Subjects.FirstOrDefault(x => x.Id == Convert.ToInt64(subId));
                                            PASSIS.LIB.SubjectsInSchool subSch = context.SubjectsInSchools.FirstOrDefault(x => x.SubjectId == subject.Id && x.SchoolId == logonUser.SchoolId);
                                            //for (uint count = 0; count <= broadsheettemplate.TotalNumberofSubjectInserted; count++ )
                                            //{
                                            // countnum = count + newunit ;
                                            if (count <= broadsheettemplate.TotalNumberofSubjectInserted)
                                            {
                                                Net.SourceForge.Koogra.Excel.Row row = workSheet.Rows[rowCount];
                                                if (row.Cells[count + 3] != null)
                                                {
                                                    PASSIS.LIB.User student = db.Users.FirstOrDefault(x => x.AdmissionNumber == row.Cells[2].Value.ToString().Trim() && x.SchoolId == logonUser.SchoolId && x.SchoolCampusId == logonUser.SchoolCampusId);

                                                    StudentScoreTransaction trans = context.StudentScoreTransactions.FirstOrDefault(x => x.BroadSheetTemplateID == broadsheettemplate.ID && x.Code == Convert.ToInt16(txtCode.Text) && x.StudentId == student.Id && x.SubjectId == Convert.ToInt64(subId)
                                                        && x.CategoryId == Convert.ToInt64(ddlCategory.SelectedValue) && x.SubCategoryId == Convert.ToInt64(ddlSubCategory.SelectedValue)
                                                        && x.TermId == termId && x.AcademicSessionID == sessionId && x.ClassId == yearId && x.GradeId == gradeId);
                                                    if (Convert.ToInt16(txtCode.Text) < 1)
                                                    {
                                                        lblErrorMsg.Text = "Code should not be lesser than 1";
                                                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                                                        lblErrorMsg.Visible = true;
                                                        return;
                                                    }

                                                    if (trans != null)
                                                    {
                                                        lblErrorMsg.Text = "Code has been used for this sub category";
                                                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                                                        lblErrorMsg.Visible = true;
                                                        continue;
                                                    }
                                                    else
                                                    {
                                                        StudentScoreTransaction transs = context.StudentScoreTransactions.FirstOrDefault(x => x.BroadSheetTemplateID == broadsheettemplate.ID && x.Code == Convert.ToInt16(txtCode.Text) - 1 && x.StudentId == student.Id && x.SubjectId == Convert.ToInt64(subId)
                                                            && x.CategoryId == Convert.ToInt64(ddlCategory.SelectedValue) && x.SubCategoryId == Convert.ToInt64(ddlSubCategory.SelectedValue) && x.TermId == termId && x.AcademicSessionID == sessionId && x.ClassId == yearId && x.GradeId == gradeId);
                                                        if (transs == null && Convert.ToInt16(txtCode.Text) - 1 != 0)
                                                        {
                                                            lblErrorMsg.Text = "Enter lesser number for the code";
                                                            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                                                            lblErrorMsg.Visible = true;
                                                            return;
                                                        }
                                                    }

                                                    //PASSIS.LIB.StudentScoreTransaction score = new PASSIS.LIB.StudentScoreTransaction();
                                                    //PASSIS.LIB.StudentScore scores = new PASSIS.LIB.StudentScore();
                                                    PASSIS.LIB.StudentScoreTemporary scoresTemp = new PASSIS.LIB.StudentScoreTemporary();
                                                    //Net.SourceForge.Koogra.Excel.Row row = new Net.SourceForge.Koogra.Excel.Row();
                                                    //Net.SourceForge.Koogra.Excel.Row row = workSheet.Rows[rowCount];
                                                    //do validation when u hv time 
                                                    //if (row.Cells[count + 3].Value.ToString() != "")

                                                    StudentScoreTemporary chekTemp = context.StudentScoreTemporaries.FirstOrDefault(x => x.BroadSheetTemplateID == broadsheettemplate.ID && x.Code == Convert.ToInt16(txtCode.Text) && x.StudentId == student.Id && x.SubjectId == Convert.ToInt64(subId)
                                                    && x.CategoryId == Convert.ToInt64(ddlCategory.SelectedValue) && x.SubCategoryId == Convert.ToInt64(ddlSubCategory.SelectedValue) && x.TermId == termId && x.AcademicSessionID == sessionId && x.ClassId == yearId && x.GradeId == gradeId 
                                                    && x.UploadedById == logonUser.Id && x.SchoolId == logonUser.SchoolId && x.CampusId == logonUser.SchoolCampusId);

                                                    if (chekTemp == null)
                                                    {

                                                        try
                                                        { scoresTemp.AdmissionNumber = row.Cells[2].Value.ToString(); }
                                                        catch { }
                                                        scoresTemp.StudentId = student.Id;
                                                        try { scoresTemp.ExamScore = Convert.ToInt64(row.Cells[count + 3].Value); }
                                                        catch { }
                                                        scoresTemp.ExamScoreObtainable = Convert.ToInt16(txtTotalMark.Text);
                                                        scoresTemp.CategoryId = Convert.ToInt64(ddlCategory.SelectedValue);
                                                        scoresTemp.SubCategoryId = Convert.ToInt64(ddlSubCategory.SelectedValue);
                                                        scoresTemp.TermId = termId;
                                                        scoresTemp.AcademicSessionID = sessionId;
                                                        scoresTemp.SubjectId = Convert.ToInt32(subId);
                                                        if (subSch.DepartmentId != null)
                                                        {
                                                            scoresTemp.DepartmentId = Convert.ToInt64(subSch.DepartmentId);
                                                        }
                                                        scoresTemp.CampusId = logonUser.SchoolCampusId;
                                                        scoresTemp.SchoolId = logonUser.SchoolId;
                                                        if (subTeacher != null)
                                                        {
                                                            scoresTemp.SubjectTeacherId = Convert.ToInt64(subTeacher.TeacherId);
                                                        }
                                                        else { scoresTemp.SubjectTeacherId = logonUser.Id; }
                                                        scoresTemp.UploadedById = UploadedById;
                                                        scoresTemp.BroadSheetTemplateID = broadsheettemplate.ID;
                                                        scoresTemp.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
                                                        scoresTemp.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                                                        scoresTemp.Description = ddlDescription.SelectedItem.Text;
                                                        scoresTemp.Code = Convert.ToInt16(txtCode.Text);
                                                        scoresTemp.Date = DateTime.Now;
                                                        scoresTemp.StatusCode = "I";
                                                        scoresTemp.IsCancelled = false;
                                                        context.StudentScoreTemporaries.InsertOnSubmit(scoresTemp);
                                                        context.SubmitChanges();

                                                        btnCancel.Visible = true;
                                                        btnViewScores.Visible = true;
                                                        btnUploadScores.Visible = false;
                                                    }
                                                    else
                                                    {
                                                        btnCancel.Visible = true;
                                                        btnViewScores.Visible = true;
                                                        btnUploadScores.Visible = false;
                                                        lblErrorMsg.Text = "Kindly View scores before saving or click on cancel to cancel upload!";
                                                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                                                        lblErrorMsg.Visible = true;
                                                        return;
                                                    }

                                                }
                                            }

                                            count++;
                                        }

                                    }
                                }
                                catch (Exception ex) { throw ex; }


                            }

                            lblErrorMsg.Text = "Kindly Click on View Scores to view Scores Uploaded before saving!";
                            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                            lblErrorMsg.Visible = true;
                            ClearContent();
                        }
                        else
                        {
                            lblErrorMsg.Text = "Exam has been submitted or wrong code selected";
                            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                            lblErrorMsg.Visible = true;
                        }
                    }

                    else
                    {
                        lblErrorMsg.Text = "Kindly confirm you are uploading against previously generated template";
                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                        lblErrorMsg.Visible = true;
                    }


                }

                else
                {
                    lblErrorMsg.Text = "Kindly select the excel file to upload";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                }

                //BindExcelScores();

            }

            if (ddlCategory.SelectedItem.Text == "CA")
            {
                ScoreSubCategoryConfiguration subCategory = context.ScoreSubCategoryConfigurations.FirstOrDefault(x => x.Id == Convert.ToInt64(ddlSubCategory.SelectedValue));
                if (subCategory == null)
                {
                    lblErrorMsg.Text = "Kindly set the score sub category";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                    return;
                }
                if (txtCode.Text == "")
                {
                    lblErrorMsg.Text = "Kindly enter the code";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                    return;
                }

                string TemplateCode = ddlDescription.SelectedItem.Text.ToString();

                //confirm if file to be uploaded has been selected else notify the user to select file to upload
                if (documentUpload.HasFile)
                {

                    string originalFileName = Path.GetFileName(documentUpload.PostedFile.FileName);
                    string fileExtension = Path.GetExtension(documentUpload.PostedFile.FileName);

                    //This is use to confirm if user is uploading against the previously generated template
                    TestAssigenmentBroadSheetTemplate broadsheettemplate = db.TestAssigenmentBroadSheetTemplates.FirstOrDefault(x => x.BroadSheetDescriptionCode.DescriptionName == TemplateCode);

                    //If template exist
                    if (broadsheettemplate != null)
                    {
                        //Confirm if the result has been uploaded
                        if (broadsheettemplate.HasSubmitted == false)
                        {
                            string[] subjectId = broadsheettemplate.SubjectId.Split(',');

                            string fileLocation = Server.MapPath("~/docs/ScoreSheets/") + originalFileName;
                            documentUpload.SaveAs(fileLocation);
                            IList<PASSIS.LIB.StudentScore> scoresFound = new List<PASSIS.LIB.StudentScore>();
                            Net.SourceForge.Koogra.Excel.Workbook wkbk = new Net.SourceForge.Koogra.Excel.Workbook(fileLocation);
                            Net.SourceForge.Koogra.Excel.Worksheet workSheet = wkbk.Sheets[0];
                            // long count = 0;
                            for (uint rowCount = workSheet.Rows.MinRow; rowCount <= workSheet.Rows.MaxRow; ++rowCount)
                            {
                                try
                                {

                                    if (rowCount != workSheet.Rows.MinRow)// jump the first row
                                    {

                                        //broadsheettemplate.TotalNumberofSubjectInserted = count;
                                        uint count = 0;
                                        uint newunit = 3;
                                        uint countnum;
                                        foreach (string subId in subjectId)
                                        {
                                            PASSIS.LIB.SubjectTeacher subTeacher = context.SubjectTeachers.FirstOrDefault(x => x.SubjectId == Convert.ToInt64(subId));
                                            PASSIS.LIB.Subject subject = context.Subjects.FirstOrDefault(x => x.Id == Convert.ToInt64(subId));
                                            PASSIS.LIB.SubjectsInSchool subSch = context.SubjectsInSchools.FirstOrDefault(x => x.SubjectId == subject.Id && x.SchoolId == logonUser.SchoolId);
                                            //for (uint count = 0; count <= broadsheettemplate.TotalNumberofSubjectInserted; count++ )
                                            //{
                                            // countnum = count + newunit ;
                                            if (count <= broadsheettemplate.TotalNumberofSubjectInserted)
                                            {
                                                Net.SourceForge.Koogra.Excel.Row row = workSheet.Rows[rowCount];
                                                if (row.Cells[count + 3] != null)
                                                {
                                                    PASSIS.LIB.User student = db.Users.FirstOrDefault(x => x.AdmissionNumber == row.Cells[2].Value.ToString().Trim() && x.SchoolId == logonUser.SchoolId && x.SchoolCampusId == logonUser.SchoolCampusId);

                                                    StudentScoreRepositoryTransaction trans = context.StudentScoreRepositoryTransactions.FirstOrDefault(x => x.TestAssigenmentBroadSheetTemplateID == broadsheettemplate.ID
                                                        && x.Code == Convert.ToInt16(txtCode.Text) && x.StudentId == student.Id && x.SubjectId == Convert.ToInt64(subId)
                                                        && x.CategoryId == Convert.ToInt64(ddlCategory.SelectedValue) && x.SubCategoryId == Convert.ToInt64(ddlSubCategory.SelectedValue)
                                                        && x.TermId == termId && x.SessionId == sessionId && x.ClassId == yearId && x.GradeId == gradeId);
                                                    if (Convert.ToInt16(txtCode.Text) < 1)
                                                    {
                                                        lblErrorMsg.Text = "Code should not be lesser than 1";
                                                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                                                        lblErrorMsg.Visible = true;
                                                        return;
                                                    }

                                                    if (trans != null)
                                                    {
                                                        lblErrorMsg.Text = "Code has been used for this sub category";
                                                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                                                        lblErrorMsg.Visible = true;
                                                        continue;
                                                    }
                                                    else
                                                    {
                                                        StudentScoreRepositoryTransaction transs = context.StudentScoreRepositoryTransactions.FirstOrDefault(x => x.TestAssigenmentBroadSheetTemplateID == broadsheettemplate.ID && x.Code == Convert.ToInt16(txtCode.Text) - 1 && x.StudentId == student.Id && x.SubjectId == Convert.ToInt64(subId)
                                                            && x.CategoryId == Convert.ToInt64(ddlCategory.SelectedValue) && x.SubCategoryId == Convert.ToInt64(ddlSubCategory.SelectedValue)
                                                            && x.TermId == termId && x.SessionId == sessionId && x.ClassId == yearId && x.GradeId == gradeId);
                                                        if (transs == null && Convert.ToInt16(txtCode.Text) - 1 != 0)
                                                        {
                                                            lblErrorMsg.Text = "Enter lesser number for the code";
                                                            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                                                            lblErrorMsg.Visible = true;
                                                            return;
                                                        }
                                                    }

                                                    //PASSIS.LIB.StudentScoreRepositoryTransaction score = new PASSIS.LIB.StudentScoreRepositoryTransaction();
                                                    //PASSIS.LIB.StudentScoreRepository scores = new StudentScoreRepository();
                                                    //PASSIS.LIB.StudentScoreTransaction score = new PASSIS.LIB.StudentScoreTransaction();
                                                    //PASSIS.LIB.StudentScore scores = new PASSIS.LIB.StudentScore();
                                                    PASSIS.LIB.StudentScoreTemporary scoresTemp = new PASSIS.LIB.StudentScoreTemporary();
                                                    //Net.SourceForge.Koogra.Excel.Row row = new Net.SourceForge.Koogra.Excel.Row();
                                                    //Net.SourceForge.Koogra.Excel.Row row = workSheet.Rows[rowCount];
                                                    //do validation when u hv time 
                                                    //if (row.Cells[count + 3].Value.ToString() != "")
                                                    StudentScoreTemporary chekTemp = context.StudentScoreTemporaries.FirstOrDefault(x => x.BroadSheetTemplateID == broadsheettemplate.ID && x.Code == Convert.ToInt16(txtCode.Text) && x.StudentId == student.Id && x.SubjectId == Convert.ToInt64(subId)
                                                   && x.CategoryId == Convert.ToInt64(ddlCategory.SelectedValue) && x.SubCategoryId == Convert.ToInt64(ddlSubCategory.SelectedValue) && x.TermId == termId && x.AcademicSessionID == sessionId && x.ClassId == yearId && x.GradeId == gradeId
                                                   && x.UploadedById == logonUser.Id && x.SchoolId == logonUser.SchoolId && x.CampusId == logonUser.SchoolCampusId);

                                                    if (chekTemp == null)
                                                    {
                                                        try
                                                        { scoresTemp.AdmissionNumber = row.Cells[2].Value.ToString(); }
                                                        catch { }
                                                        scoresTemp.StudentId = student.Id;
                                                        try { scoresTemp.ExamScore = Convert.ToInt64(row.Cells[count + 3].Value); }
                                                        catch { }
                                                        scoresTemp.ExamScoreObtainable = Convert.ToInt16(txtTotalMark.Text);
                                                        scoresTemp.CategoryId = Convert.ToInt64(ddlCategory.SelectedValue);
                                                        scoresTemp.SubCategoryId = Convert.ToInt64(ddlSubCategory.SelectedValue);
                                                        scoresTemp.TermId = termId;
                                                        scoresTemp.AcademicSessionID = sessionId;
                                                        scoresTemp.SubjectId = Convert.ToInt32(subId);
                                                        if (subSch.DepartmentId != null)
                                                        {
                                                            scoresTemp.DepartmentId = Convert.ToInt64(subSch.DepartmentId);
                                                        }
                                                        scoresTemp.CampusId = logonUser.SchoolCampusId;
                                                        scoresTemp.SchoolId = logonUser.SchoolId;
                                                        if (subTeacher != null)
                                                        {
                                                            scoresTemp.SubjectTeacherId = Convert.ToInt64(subTeacher.TeacherId);
                                                        }
                                                        else { scoresTemp.SubjectTeacherId = logonUser.Id; }
                                                        scoresTemp.UploadedById = UploadedById;
                                                        scoresTemp.BroadSheetTemplateID = broadsheettemplate.ID;
                                                        scoresTemp.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
                                                        scoresTemp.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                                                        scoresTemp.Description = ddlDescription.SelectedItem.Text;
                                                        scoresTemp.Code = Convert.ToInt16(txtCode.Text);
                                                        scoresTemp.Date = DateTime.Now;
                                                        scoresTemp.StatusCode = "I";
                                                        scoresTemp.IsCancelled = false;
                                                        context.StudentScoreTemporaries.InsertOnSubmit(scoresTemp);
                                                        context.SubmitChanges();

                                                        btnCancel.Visible = true;
                                                        btnViewScores.Visible = true;
                                                        btnUploadScores.Visible = false;
                                                    }
                                                    else
                                                    {
                                                        btnCancel.Visible = true;
                                                        btnViewScores.Visible = true;
                                                        btnUploadScores.Visible = false;
                                                        lblErrorMsg.Text = "Kindly View scores before saving or click on cancel to cancel upload!";
                                                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                                                        lblErrorMsg.Visible = true;
                                                        return;
                                                    }
                                                }
                                            }

                                            count++;
                                        }

                                    }
                                }
                                catch (Exception ex) { throw ex; }


                            }

                            lblErrorMsg.Text = "Kindly click on View Scores to view Scores uploaded before saving!";
                            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                            lblErrorMsg.Visible = true;
                            ClearContent();

                        }
                        else
                        {
                            lblErrorMsg.Text = "Exam has been submitted or wrong code selected";
                            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                            lblErrorMsg.Visible = true;
                        }
                    }

                    else
                    {
                        lblErrorMsg.Text = "Kindly confirm you are uploading against previously generated template";
                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                        lblErrorMsg.Visible = true;
                    }


                }

                else
                {
                    lblErrorMsg.Text = "Kindly select the excel file to upload";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                }

                //  BindExcelScores();

            }
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = "Error occurred, kindly contact your administrator";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
        }

    }


    protected void btnSaveScore_Click(object sender, EventArgs e)
    {
        try
        {
            //check if all the required fields on the form has been selected
            //Int64 yearId = Convert.ToInt64(ddlYear.SelectedValue);
            //Int64 gradeId = Convert.ToInt64(ddlGrade.SelectedValue);
            //Int64 UploadedById = logonUser.Id;
            ////Int64 departmentId = 0;
            //Int64 sessionId = Convert.ToInt64(ddlSession.SelectedValue);
            //Int64 termId = Convert.ToInt64(ddlTerm.SelectedValue);
            //if (ddlYear.SelectedIndex == 0)
            //{
            //    lblErrorMsg.Text = "Kindly select year";
            //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            //    lblErrorMsg.Visible = true;
            //    return;
            //}
            //if (ddlGrade.SelectedIndex == 0)
            //{
            //    lblErrorMsg.Text = "Kindly select class";
            //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            //    lblErrorMsg.Visible = true;
            //    return;
            //}

            //if (ddlCategory.SelectedIndex == 0)
            //{
            //    lblErrorMsg.Text = "Kindly select category";
            //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            //    lblErrorMsg.Visible = true;
            //    return;
            //}
            //if (ddlSubCategory.SelectedIndex == 0)
            //{
            //    lblErrorMsg.Text = "Kindly select sub category";
            //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            //    lblErrorMsg.Visible = true;
            //    return;
            //}
            //if (ddlSession.SelectedIndex == 0)
            //{
            //    lblErrorMsg.Text = "Kindly select session";
            //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            //    lblErrorMsg.Visible = true;
            //    return;
            //}
            //if (txtTotalMark.Text.Trim() == "")
            //{
            //    lblErrorMsg.Text = "Mark Obtainable is Missing";
            //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            //    lblErrorMsg.Visible = true;
            //    return;
            //}
            //if (ddlTerm.SelectedIndex == 0)
            //{
            //    lblErrorMsg.Text = "Kindly select term";
            //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            //    lblErrorMsg.Visible = true;
            //    return;
            //}
            //if (ddlDescription.SelectedIndex == -1)
            //{
            //    lblErrorMsg.Text = "Description Template  is required";
            //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            //    lblErrorMsg.Visible = true;
            //    return;
            //}
            //ScoreCategoryConfiguration scoreCategory = context.ScoreCategoryConfigurations.FirstOrDefault(x => x.ClassId == Convert.ToInt64(ddlYear.SelectedValue) && x.Category == ddlCategory.SelectedItem.Text && x.SessionId == sessionId && x.TermId == termId && x.SchoolId == logonUser.SchoolId && x.CampusId == logonUser.SchoolCampusId);
            //if (scoreCategory == null)
            //{
            //    lblErrorMsg.Text = "Kindly set the score category";
            //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            //    lblErrorMsg.Visible = true;
            //    return;
            //}
            PASSISLIBDataContext db = new PASSISLIBDataContext();

            if (ddlCategory.SelectedItem.Text == "Exam")
            {
                //ScoreSubCategoryConfiguration subCategory = context.ScoreSubCategoryConfigurations.FirstOrDefault(x => x.Id == Convert.ToInt64(ddlSubCategory.SelectedValue));
                //if (subCategory == null)
                //{
                //    lblErrorMsg.Text = "Kindly set the score sub category";
                //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //    lblErrorMsg.Visible = true;
                //    return;
                //}
                //if (txtCode.Text == "")
                //{
                //    lblErrorMsg.Text = "Kindly enter the code";
                //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //    lblErrorMsg.Visible = true;
                //    return;
                //}




                string TemplateCode = ddlDescription.SelectedItem.Text.ToString();

                //confirm if file to be uploaded has been selected else notify the user to select file to upload
                //if (documentUpload.HasFile)
                //{

                //    string originalFileName = Path.GetFileName(documentUpload.PostedFile.FileName);
                //    string fileExtension = Path.GetExtension(documentUpload.PostedFile.FileName);

                //This is use to confirm if user is uploading against the previously generated template
                TestAssigenmentBroadSheetTemplate broadsheettemplate = db.TestAssigenmentBroadSheetTemplates.FirstOrDefault(x => x.BroadSheetDescriptionCode.DescriptionName == TemplateCode);

                //If template exist
                if (broadsheettemplate != null)
                {
                    //Confirm if the result has been uploaded
                    if (broadsheettemplate.HasSubmitted == false)
                    {
                        //string[] subjectId = broadsheettemplate.SubjectId.Split(',');


                        //string fileLocation = Server.MapPath("~/docs/ScoreSheets/") + originalFileName;
                        //documentUpload.SaveAs(fileLocation);
                        //IList<PASSIS.LIB.StudentScore> scoresFound = new List<PASSIS.LIB.StudentScore>();
                        //Net.SourceForge.Koogra.Excel.Workbook wkbk = new Net.SourceForge.Koogra.Excel.Workbook(fileLocation);
                        //Net.SourceForge.Koogra.Excel.Worksheet workSheet = wkbk.Sheets[0];

                        //// long count = 0;
                        //for (uint rowCount = workSheet.Rows.MinRow; rowCount <= workSheet.Rows.MaxRow; ++rowCount)
                        //{
                        //    try
                        //    {


                        //        if (rowCount != workSheet.Rows.MinRow)// jump the first row
                        //        {

                        //            //broadsheettemplate.TotalNumberofSubjectInserted = count;
                        //            uint count = 0;
                        //            uint newunit = 3;
                        //            uint countnum;
                        //            foreach (string subId in subjectId)
                        //            {
                        //PASSIS.LIB.SubjectTeacher subTeacher = context.SubjectTeachers.FirstOrDefault(x => x.SubjectId == Convert.ToInt64(subId));
                        //PASSIS.LIB.Subject subject = context.Subjects.FirstOrDefault(x => x.Id == Convert.ToInt64(subId));
                        //PASSIS.LIB.SubjectsInSchool subSch = context.SubjectsInSchools.FirstOrDefault(x => x.SubjectId == subject.Id && x.SchoolId == logonUser.SchoolId);
                        //for (uint count = 0; count <= broadsheettemplate.TotalNumberofSubjectInserted; count++ )
                        //{
                        // countnum = count + newunit ;
                        //if (count <= broadsheettemplate.TotalNumberofSubjectInserted)
                        //{
                        //    Net.SourceForge.Koogra.Excel.Row row = workSheet.Rows[rowCount];
                        //    if (row.Cells[count + 3] != null)
                        //    {




                        var getTempScores = from scoreTemp in context.StudentScoreTemporaries
                                            where scoreTemp.BroadSheetTemplateID == broadsheettemplate.ID && scoreTemp.Code == Convert.ToInt64(txtCode.Text)
                                             && scoreTemp.IsCancelled == false && scoreTemp.AcademicSessionID == Convert.ToInt64(ddlSession.SelectedValue) && scoreTemp.TermId == Convert.ToInt64(ddlTerm.SelectedValue)
                                             && scoreTemp.ClassId == Convert.ToInt64(ddlYear.SelectedValue) && scoreTemp.GradeId == Convert.ToInt64(ddlGrade.SelectedValue)
                                             && scoreTemp.CategoryId == Convert.ToInt64(ddlCategory.SelectedValue) && scoreTemp.SubCategoryId == Convert.ToInt64(ddlSubCategory.SelectedValue) && scoreTemp.UploadedById == logonUser.Id
                                            select scoreTemp;
                        IList<StudentScoreTemporary> stdTmpScore = getTempScores.ToList<StudentScoreTemporary>();

                        if (stdTmpScore.Count > 0)
                        {

                            foreach (StudentScoreTemporary scoreTmp in stdTmpScore)
                            {
                                PASSIS.LIB.SubjectTeacher subTeacher = context.SubjectTeachers.FirstOrDefault(x => x.SubjectId == Convert.ToInt64(scoreTmp.SubjectId));
                                PASSIS.LIB.Subject subject = context.Subjects.FirstOrDefault(x => x.Id == Convert.ToInt64(scoreTmp.SubjectId));
                                PASSIS.LIB.SubjectsInSchool subSch = context.SubjectsInSchools.FirstOrDefault(x => x.SubjectId == subject.Id && x.SchoolId == logonUser.SchoolId);


                                //PASSIS.LIB.User student = db.Users.FirstOrDefault(x => x.AdmissionNumber == lblAdmNo.Text.Trim() && x.SchoolId == logonUser.SchoolId && x.SchoolCampusId == logonUser.SchoolCampusId);
                                PASSIS.LIB.User student = db.Users.FirstOrDefault(x => x.AdmissionNumber == scoreTmp.AdmissionNumber && x.SchoolId == logonUser.SchoolId && x.SchoolCampusId == logonUser.SchoolCampusId);

                                ScoreSubCategoryConfiguration subCategory = context.ScoreSubCategoryConfigurations.FirstOrDefault(x => x.Id == Convert.ToInt64(scoreTmp.SubCategoryId));
                                ScoreCategoryConfiguration scoreCategory = context.ScoreCategoryConfigurations.FirstOrDefault(x => x.ClassId == Convert.ToInt64(scoreTmp.ClassId) && x.Category == ddlCategory.SelectedItem.Text && x.SessionId == scoreTmp.AcademicSessionID && x.TermId == scoreTmp.TermId && x.SchoolId == logonUser.SchoolId && x.CampusId == logonUser.SchoolCampusId);

                                //StudentScoreTemporary tempTrans = context.StudentScoreTemporaries.FirstOrDefault(x => x.BroadSheetTemplateID == broadsheettemplate.ID && x.Code == Convert.ToInt16(code) && x.StudentId == student.Id && x.SubjectId == Convert.ToInt64(subId)
                                //                       && x.CategoryId == Convert.ToInt64(categoryId) && x.SubCategoryId == Convert.ToInt64(subCategoryId)
                                //                       && x.TermId == termId && x.AcademicSessionID == academicSessionId && x.ClassId == classId && x.GradeId == gradeId);

                                //StudentScoreTransaction trans = context.StudentScoreTransactions.FirstOrDefault(x => x.BroadSheetTemplateID == broadsheettemplate.ID && x.Code == Convert.ToInt16(txtCode.Text) && x.StudentId == student.Id && x.SubjectId == Convert.ToInt64(subId)
                                //                       && x.CategoryId == Convert.ToInt64(ddlCategory.SelectedValue) && x.SubCategoryId == Convert.ToInt64(ddlSubCategory.SelectedValue)
                                //                       && x.TermId == termId && x.AcademicSessionID == sessionId && x.ClassId == yearId && x.GradeId == gradeId);
                                //if (Convert.ToInt16(txtCode.Text) < 1)
                                //{
                                //    lblErrorMsg.Text = "Code should not be lesser than 1";
                                //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                                //    lblErrorMsg.Visible = true;
                                //    return;
                                //}

                                //if (trans != null)
                                //{
                                //    lblErrorMsg.Text = "Code has been used for this sub category";
                                //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                                //    lblErrorMsg.Visible = true;
                                //    continue;
                                //}
                                //else
                                //{
                                //    StudentScoreTransaction transs = context.StudentScoreTransactions.FirstOrDefault(x => x.BroadSheetTemplateID == broadsheettemplate.ID && x.Code == Convert.ToInt16(txtCode.Text) - 1 && x.StudentId == student.Id && x.SubjectId == Convert.ToInt64(subId)
                                //        && x.CategoryId == Convert.ToInt64(ddlCategory.SelectedValue) && x.SubCategoryId == Convert.ToInt64(ddlSubCategory.SelectedValue) && x.TermId == termId && x.AcademicSessionID == sessionId && x.ClassId == yearId && x.GradeId == gradeId);
                                //    if (transs == null && Convert.ToInt16(txtCode.Text) - 1 != 0)
                                //    {
                                //        lblErrorMsg.Text = "Enter lesser number for the code";
                                //        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                                //        lblErrorMsg.Visible = true;
                                //        return;
                                //    }
                                //}


                                PASSIS.LIB.StudentScoreTransaction score = new PASSIS.LIB.StudentScoreTransaction();
                                PASSIS.LIB.StudentScore scores = new PASSIS.LIB.StudentScore();
                                //Net.SourceForge.Koogra.Excel.Row row = new Net.SourceForge.Koogra.Excel.Row();
                                //Net.SourceForge.Koogra.Excel.Row row = workSheet.Rows[rowCount];
                                //do validation when u hv time 
                                //if (row.Cells[count + 3].Value.ToString() != "")
                                decimal examScores = Convert.ToDecimal(scoreTmp.ExamScore);
                                try
                                { score.AdmissionNumber = scoreTmp.AdmissionNumber; }
                                catch { }
                                score.StudentId = student.Id;
                                try { score.ExamScore = Convert.ToInt64(examScores); }
                                catch { }
                                score.ExamScoreObtainable = Convert.ToInt16(scoreTmp.ExamScoreObtainable);
                                score.CategoryId = Convert.ToInt64(scoreTmp.CategoryId);
                                score.SubCategoryId = Convert.ToInt64(scoreTmp.SubCategoryId);
                                score.TermId = scoreTmp.TermId;
                                score.AcademicSessionID = scoreTmp.AcademicSessionID;
                                score.SubjectId = Convert.ToInt32(scoreTmp.SubjectId);
                                if (subSch.DepartmentId != null)
                                {
                                    score.DepartmentId = Convert.ToInt64(subSch.DepartmentId);
                                }
                                score.CampusId = logonUser.SchoolCampusId;
                                score.SchoolId = logonUser.SchoolId;
                                if (subTeacher != null)
                                {
                                    score.SubjectTeacherId = Convert.ToInt64(subTeacher.TeacherId);
                                }
                                else { score.SubjectTeacherId = logonUser.Id; }
                                score.UploadedById = scoreTmp.UploadedById;
                                score.BroadSheetTemplateID = scoreTmp.BroadSheetTemplateID;
                                score.ClassId = Convert.ToInt64(scoreTmp.ClassId);
                                score.GradeId = Convert.ToInt64(scoreTmp.GradeId);
                                score.Description = scoreTmp.Description;
                                score.Code = Convert.ToInt16(scoreTmp.Code);
                                score.Date = DateTime.Now;
                                score.StatusCode = "I";
                                score.IsCancelled = false;
                                context.StudentScoreTransactions.InsertOnSubmit(score);
                                context.SubmitChanges();

                                PASSIS.LIB.StudentScore getScoreObj = context.StudentScores.FirstOrDefault(x => x.AdmissionNumber == scoreTmp.AdmissionNumber && x.StudentId == student.Id && x.BroadSheetTemplateID == scoreTmp.BroadSheetTemplateID && x.SubjectId == Convert.ToInt64(scoreTmp.SubjectId)
                                    && x.CategoryId == Convert.ToInt64(scoreTmp.CategoryId) && x.SubCategoryId == Convert.ToInt64(scoreTmp.SubCategoryId) && x.TermId == scoreTmp.TermId && x.AcademicSessionID == scoreTmp.AcademicSessionID && x.ClassId == scoreTmp.ClassId && x.GradeId == scoreTmp.GradeId);
                                if (getScoreObj == null)
                                {
                                    //Calculating the percentage for the first entry

                                    decimal totalScore = Convert.ToDecimal(scoreTmp.ExamScoreObtainable); // score obtainable
                                    decimal tsScore = Convert.ToInt64(examScores) / totalScore; //obtained divided obtainable
                                    int testPercentage = Convert.ToInt16(subCategory.Percentage); // percentage given to subcategory 
                                    decimal percentageScore = tsScore * testPercentage; // total score obtained multiplied by subcategory percentage
                                    decimal ExamPercentageScore = Convert.ToDecimal((percentageScore / 100) * scoreCategory.Percentage); // percentage score divided by 100, and multiplied by category percentage for CA/EXAM(40/60)
                                    decimal? finalScore = (ExamPercentageScore * subSch.MaximumScore) / 100;

                                    scores.AdmissionNumber = scoreTmp.AdmissionNumber;
                                    scores.StudentId = student.Id;
                                    scores.ExamScoreObtainable = Convert.ToInt16(scoreTmp.ExamScoreObtainable);
                                    scores.ExamScore = Convert.ToDecimal(examScores);
                                    scores.TermId = scoreTmp.TermId;
                                    scores.AcademicSessionID = scoreTmp.AcademicSessionID;
                                    scores.ClassId = Convert.ToInt64(scoreTmp.ClassId);
                                    scores.GradeId = Convert.ToInt64(scoreTmp.GradeId);
                                    scores.SubjectId = Convert.ToInt16(scoreTmp.SubjectId);
                                    if (subSch.DepartmentId != null)
                                    {
                                        scores.DepartmentId = Convert.ToInt64(subSch.DepartmentId);
                                    }
                                    scores.Percentage = subCategory.Percentage;
                                    scores.PercentageScore = percentageScore;
                                    scores.ExamPercentage = scoreCategory.Percentage;
                                    scores.ExamPercentageScore = ExamPercentageScore;
                                    scores.SubjectMaxScore = subSch.MaximumScore;
                                    scores.FinalScore = finalScore;
                                    scores.CategoryId = scoreCategory.Id;
                                    scores.SubCategoryId = subCategory.Id;
                                    //score.DepartmentId = departmentId;
                                    scores.CampusId = logonUser.SchoolCampusId;
                                    scores.SchoolId = logonUser.SchoolId;
                                    if (subTeacher != null)
                                    {
                                        scores.SubjectTeacherId = Convert.ToInt64(subTeacher.TeacherId);
                                    }
                                    else { scores.SubjectTeacherId = logonUser.Id; }
                                    scores.UploadedById = logonUser.Id;
                                    scores.BroadSheetTemplateID = scoreTmp.BroadSheetTemplateID;
                                    //scores.Remark = txtRemark.Text.Trim();
                                    scores.Count = 1;
                                    //score.Description = txtDescription.Text.Trim();
                                    scores.Date = DateTime.Now;
                                    //score.TestAssigenmentBroadSheetTemplateID = TestAssigenmentBroadSheetTemplateID;
                                    context.StudentScores.InsertOnSubmit(scores);
                                    context.SubmitChanges();
                                    //scoreList.Add(score);
                                    //new ScoresheetLIB().SaveStudentTestAssignmentScore(scoreList);
                                }
                                else
                                {
                                    //decimal totalScore = Convert.ToDecimal(txtTotalMark.Text.Trim());
                                    decimal totalScore = Convert.ToDecimal(scoreTmp.ExamScoreObtainable) + Convert.ToDecimal(getScoreObj.ExamScoreObtainable);
                                    decimal newScore = Convert.ToInt64(examScores) + Convert.ToDecimal(getScoreObj.ExamScore);
                                    decimal tsScore = newScore / totalScore;
                                    int examPercentage = Convert.ToInt16(subCategory.Percentage);
                                    decimal percentageScore = tsScore * examPercentage;
                                    decimal ExamPercentageScore = Convert.ToDecimal((percentageScore / 100) * scoreCategory.Percentage);
                                    decimal? finalScore = (ExamPercentageScore * subSch.MaximumScore) / 100;

                                    getScoreObj.AdmissionNumber = scoreTmp.AdmissionNumber;
                                    getScoreObj.StudentId = student.Id;
                                    getScoreObj.ExamScoreObtainable = Convert.ToInt16(totalScore);
                                    getScoreObj.ExamScore = Convert.ToDecimal(newScore);
                                    getScoreObj.TermId = scoreTmp.TermId;
                                    getScoreObj.AcademicSessionID = scoreTmp.AcademicSessionID;
                                    getScoreObj.ClassId = Convert.ToInt64(scoreTmp.ClassId);
                                    getScoreObj.GradeId = Convert.ToInt64(scoreTmp.GradeId);
                                    getScoreObj.SubjectId = Convert.ToInt16(scoreTmp.SubjectId);
                                    if (subSch.DepartmentId != null)
                                    {
                                        getScoreObj.DepartmentId = Convert.ToInt64(subSch.DepartmentId);
                                    }
                                    getScoreObj.Percentage = subCategory.Percentage;
                                    getScoreObj.PercentageScore = percentageScore;
                                    getScoreObj.ExamPercentage = scoreCategory.Percentage;
                                    getScoreObj.ExamPercentageScore = ExamPercentageScore;
                                    getScoreObj.SubjectMaxScore = subSch.MaximumScore;
                                    getScoreObj.FinalScore = finalScore;
                                    getScoreObj.CategoryId = scoreCategory.Id;
                                    getScoreObj.SubCategoryId = subCategory.Id;
                                    getScoreObj.CampusId = logonUser.SchoolCampusId;
                                    getScoreObj.SchoolId = logonUser.SchoolId;
                                    if (subTeacher != null)
                                    {
                                        getScoreObj.SubjectTeacherId = Convert.ToInt64(subTeacher.TeacherId);
                                    }
                                    getScoreObj.UploadedById = logonUser.Id;
                                    getScoreObj.BroadSheetTemplateID = scoreTmp.BroadSheetTemplateID;
                                    //getScoreObj.Remark = txtRemark.Text.Trim();
                                    //getScoreObj.Description = txtDescription.Text.Trim();
                                    getScoreObj.Count = getScoreObj.Count + 1;
                                    getScoreObj.Date = DateTime.Now;
                                    //score.TestAssigenmentBroadSheetTemplateID = TestAssigenmentBroadSheetTemplateID;
                                    //scoreList.Add(getScoreObj);
                                    context.SubmitChanges();
                                }
                                score.StatusCode = "C";
                                context.SubmitChanges();

                                //StudentScoreTemporary scoreTemp = context.StudentScoreTemporaries.FirstOrDefault(sc => sc.UploadedById == logonUser.Id && sc.BroadSheetTemplateID == tempalateId && sc.SchoolId == logonUser.SchoolId && sc.CampusId == logonUser.SchoolCampusId);
                                StudentScoreTemporary scoreTemp = context.StudentScoreTemporaries.FirstOrDefault(sc => sc.AdmissionNumber == scoreTmp.AdmissionNumber && sc.CategoryId == scoreTmp.CategoryId && sc.SubCategoryId == scoreTmp.SubCategoryId
                                && sc.SubjectId == scoreTmp.SubjectId && sc.TermId == scoreTmp.TermId && sc.AcademicSessionID == scoreTmp.AcademicSessionID && sc.BroadSheetTemplateID == scoreTmp.BroadSheetTemplateID
                                && sc.ClassId == scoreTmp.ClassId && sc.GradeId == scoreTmp.GradeId && sc.UploadedById == scoreTmp.UploadedById && sc.SchoolId == logonUser.SchoolId && sc.CampusId == logonUser.SchoolCampusId);

                                if (scoreTemp != null)
                                {
                                    context.StudentScoreTemporaries.DeleteOnSubmit(scoreTemp);
                                    context.SubmitChanges();
                                }

                            }
                        }
                        else
                        {
                            lblErrorMsg.Text = "No Scores has been Uploaded!";
                            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                            lblErrorMsg.Visible = true;
                        }




                        //foreach (GridViewRow row in gdvViewExtendedScores.Rows)
                        //{
                        //    if (row.RowType == DataControlRowType.DataRow)
                        //    {

                        //        Label lblAdmNo = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblAdmNo");
                        //        Label lblId = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblId");
                        //        Label subjId = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblSubjectId");
                        //        Label lblCategoryId = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblCategoryId");
                        //        Label lblSubCategoryId = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblSubCategoryId");
                        //        Label lblTermId = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblTermId");
                        //        Label lblAcademicSessionID = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblAcademicSessionID");
                        //        Label lblClassId = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblClassId");
                        //        Label lblGradeId = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblGradeId");
                        //        Label lblExamScoreObtainable = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblExamScoreObtainable");
                        //        Label lblExamScore = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblExamScore");
                        //        Label lblTemplateId = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblTemplateId");
                        //        Label lblCode = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblCode");
                        //        Label lblUploadedById = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblUploadedById");
                        //        Label lblDescription = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblDescription");


                        //        long? studentId = Convert.ToInt64(lblId.Text.ToString().Trim());
                        //        long? subId = Convert.ToInt64(subjId.Text.Trim());
                        //        long? categoryId = Convert.ToInt64(lblCategoryId.Text.Trim());
                        //        long? subCategoryId = Convert.ToInt64(lblSubCategoryId.Text.Trim());
                        //        long? termId = Convert.ToInt64(lblTermId.Text.Trim());
                        //        long? academicSessionId = Convert.ToInt64(lblAcademicSessionID.Text.Trim());
                        //        long? classId = Convert.ToInt64(lblClassId.Text.Trim());
                        //        long? gradeId = Convert.ToInt64(lblGradeId.Text.Trim());
                        //        long? examScoreObtainable = Convert.ToInt64(lblExamScoreObtainable.Text.Trim());
                        //        string examScore = lblExamScore.Text.Trim();
                        //        long? tempalateId = Convert.ToInt64(lblTemplateId.Text.Trim());
                        //        long? code = Convert.ToInt64(lblCode.Text.Trim());
                        //        long? uploadedById = Convert.ToInt64(lblUploadedById.Text.Trim());
                        //        string description = lblDescription.Text.Trim();

                        //        PASSIS.LIB.SubjectTeacher subTeacher = context.SubjectTeachers.FirstOrDefault(x => x.SubjectId == Convert.ToInt64(subId));
                        //        PASSIS.LIB.Subject subject = context.Subjects.FirstOrDefault(x => x.Id == Convert.ToInt64(subId));
                        //        PASSIS.LIB.SubjectsInSchool subSch = context.SubjectsInSchools.FirstOrDefault(x => x.SubjectId == subject.Id && x.SchoolId == logonUser.SchoolId);


                        //        PASSIS.LIB.User student = db.Users.FirstOrDefault(x => x.AdmissionNumber == lblAdmNo.Text.Trim() && x.SchoolId == logonUser.SchoolId && x.SchoolCampusId == logonUser.SchoolCampusId);

                        //        ScoreSubCategoryConfiguration subCategory = context.ScoreSubCategoryConfigurations.FirstOrDefault(x => x.Id == Convert.ToInt64(subCategoryId));
                        //        ScoreCategoryConfiguration scoreCategory = context.ScoreCategoryConfigurations.FirstOrDefault(x => x.ClassId == Convert.ToInt64(classId) && x.Category == ddlCategory.SelectedItem.Text && x.SessionId == academicSessionId && x.TermId == termId && x.SchoolId == logonUser.SchoolId && x.CampusId == logonUser.SchoolCampusId);

                        //        //StudentScoreTemporary tempTrans = context.StudentScoreTemporaries.FirstOrDefault(x => x.BroadSheetTemplateID == broadsheettemplate.ID && x.Code == Convert.ToInt16(code) && x.StudentId == student.Id && x.SubjectId == Convert.ToInt64(subId)
                        //        //                       && x.CategoryId == Convert.ToInt64(categoryId) && x.SubCategoryId == Convert.ToInt64(subCategoryId)
                        //        //                       && x.TermId == termId && x.AcademicSessionID == academicSessionId && x.ClassId == classId && x.GradeId == gradeId);

                        //        //StudentScoreTransaction trans = context.StudentScoreTransactions.FirstOrDefault(x => x.BroadSheetTemplateID == broadsheettemplate.ID && x.Code == Convert.ToInt16(txtCode.Text) && x.StudentId == student.Id && x.SubjectId == Convert.ToInt64(subId)
                        //        //                       && x.CategoryId == Convert.ToInt64(ddlCategory.SelectedValue) && x.SubCategoryId == Convert.ToInt64(ddlSubCategory.SelectedValue)
                        //        //                       && x.TermId == termId && x.AcademicSessionID == sessionId && x.ClassId == yearId && x.GradeId == gradeId);
                        //        //if (Convert.ToInt16(txtCode.Text) < 1)
                        //        //{
                        //        //    lblErrorMsg.Text = "Code should not be lesser than 1";
                        //        //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                        //        //    lblErrorMsg.Visible = true;
                        //        //    return;
                        //        //}

                        //        //if (trans != null)
                        //        //{
                        //        //    lblErrorMsg.Text = "Code has been used for this sub category";
                        //        //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                        //        //    lblErrorMsg.Visible = true;
                        //        //    continue;
                        //        //}
                        //        //else
                        //        //{
                        //        //    StudentScoreTransaction transs = context.StudentScoreTransactions.FirstOrDefault(x => x.BroadSheetTemplateID == broadsheettemplate.ID && x.Code == Convert.ToInt16(txtCode.Text) - 1 && x.StudentId == student.Id && x.SubjectId == Convert.ToInt64(subId)
                        //        //        && x.CategoryId == Convert.ToInt64(ddlCategory.SelectedValue) && x.SubCategoryId == Convert.ToInt64(ddlSubCategory.SelectedValue) && x.TermId == termId && x.AcademicSessionID == sessionId && x.ClassId == yearId && x.GradeId == gradeId);
                        //        //    if (transs == null && Convert.ToInt16(txtCode.Text) - 1 != 0)
                        //        //    {
                        //        //        lblErrorMsg.Text = "Enter lesser number for the code";
                        //        //        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                        //        //        lblErrorMsg.Visible = true;
                        //        //        return;
                        //        //    }
                        //        //}


                        //        PASSIS.LIB.StudentScoreTransaction score = new PASSIS.LIB.StudentScoreTransaction();
                        //        PASSIS.LIB.StudentScore scores = new PASSIS.LIB.StudentScore();
                        //        //Net.SourceForge.Koogra.Excel.Row row = new Net.SourceForge.Koogra.Excel.Row();
                        //        //Net.SourceForge.Koogra.Excel.Row row = workSheet.Rows[rowCount];
                        //        //do validation when u hv time 
                        //        //if (row.Cells[count + 3].Value.ToString() != "")
                        //        decimal examScores = Convert.ToDecimal(examScore);
                        //        try
                        //        { score.AdmissionNumber = lblAdmNo.Text; }
                        //        catch { }
                        //        score.StudentId = student.Id;
                        //        try { score.ExamScore = Convert.ToInt64(examScores); }
                        //        catch { }
                        //        score.ExamScoreObtainable = Convert.ToInt16(examScoreObtainable);
                        //        score.CategoryId = Convert.ToInt64(categoryId);
                        //        score.SubCategoryId = Convert.ToInt64(subCategoryId);
                        //        score.TermId = termId;
                        //        score.AcademicSessionID = academicSessionId;
                        //        score.SubjectId = Convert.ToInt32(subId);
                        //        if (subSch.DepartmentId != null)
                        //        {
                        //            score.DepartmentId = Convert.ToInt64(subSch.DepartmentId);
                        //        }
                        //        score.CampusId = logonUser.SchoolCampusId;
                        //        score.SchoolId = logonUser.SchoolId;
                        //        if (subTeacher != null)
                        //        {
                        //            score.SubjectTeacherId = Convert.ToInt64(subTeacher.TeacherId);
                        //        }
                        //        else { score.SubjectTeacherId = logonUser.Id; }
                        //        score.UploadedById = uploadedById;
                        //        score.BroadSheetTemplateID = tempalateId;
                        //        score.ClassId = Convert.ToInt64(classId);
                        //        score.GradeId = Convert.ToInt64(gradeId);
                        //        score.Description = description;
                        //        score.Code = Convert.ToInt16(code);
                        //        score.Date = DateTime.Now;
                        //        score.StatusCode = "I";
                        //        score.IsCancelled = false;
                        //        context.StudentScoreTransactions.InsertOnSubmit(score);
                        //        context.SubmitChanges();

                        //        PASSIS.LIB.StudentScore getScoreObj = context.StudentScores.FirstOrDefault(x => x.AdmissionNumber == lblAdmNo.Text.ToString() && x.StudentId == student.Id && x.BroadSheetTemplateID == tempalateId && x.SubjectId == Convert.ToInt64(subId)
                        //            && x.CategoryId == Convert.ToInt64(categoryId) && x.SubCategoryId == Convert.ToInt64(subCategoryId) && x.TermId == termId && x.AcademicSessionID == academicSessionId && x.ClassId == classId && x.GradeId == gradeId);
                        //        if (getScoreObj == null)
                        //        {
                        //            //Calculating the percentage for the first entry

                        //            decimal totalScore = Convert.ToDecimal(examScoreObtainable); // score obtainable
                        //            decimal tsScore = Convert.ToInt64(examScores) / totalScore; //obtained divided obtainable
                        //            int testPercentage = Convert.ToInt16(subCategory.Percentage); // percentage given to subcategory 
                        //            decimal percentageScore = tsScore * testPercentage; // total score obtained multiplied by subcategory percentage
                        //            decimal ExamPercentageScore = Convert.ToDecimal((percentageScore / 100) * scoreCategory.Percentage); // percentage score divided by 100, and multiplied by category percentage for CA/EXAM(40/60)
                        //            decimal? finalScore = (ExamPercentageScore * subSch.MaximumScore) / 100; 

                        //            scores.AdmissionNumber = lblAdmNo.Text.ToString();
                        //            scores.StudentId = student.Id;
                        //            scores.ExamScoreObtainable = Convert.ToInt16(examScoreObtainable);
                        //            scores.ExamScore = Convert.ToDecimal(examScores);
                        //            scores.TermId = termId;
                        //            scores.AcademicSessionID = academicSessionId;
                        //            scores.ClassId = Convert.ToInt64(classId);
                        //            scores.GradeId = Convert.ToInt64(gradeId);
                        //            scores.SubjectId = Convert.ToInt16(subId);
                        //            if (subSch.DepartmentId != null)
                        //            {
                        //                scores.DepartmentId = Convert.ToInt64(subSch.DepartmentId);
                        //            }
                        //            scores.Percentage = subCategory.Percentage;
                        //            scores.PercentageScore = percentageScore;
                        //            scores.ExamPercentage = scoreCategory.Percentage;
                        //            scores.ExamPercentageScore = ExamPercentageScore;
                        //            scores.SubjectMaxScore = subSch.MaximumScore;
                        //            scores.FinalScore = finalScore;
                        //            scores.CategoryId = scoreCategory.Id;
                        //            scores.SubCategoryId = subCategory.Id;
                        //            //score.DepartmentId = departmentId;
                        //            scores.CampusId = logonUser.SchoolCampusId;
                        //            scores.SchoolId = logonUser.SchoolId;
                        //            if (subTeacher != null)
                        //            {
                        //                scores.SubjectTeacherId = Convert.ToInt64(subTeacher.TeacherId);
                        //            }
                        //            else { scores.SubjectTeacherId = logonUser.Id; }
                        //            scores.UploadedById = logonUser.Id;
                        //            scores.BroadSheetTemplateID = tempalateId;
                        //            //scores.Remark = txtRemark.Text.Trim();
                        //            scores.Count = 1;
                        //            //score.Description = txtDescription.Text.Trim();
                        //            scores.Date = DateTime.Now;
                        //            //score.TestAssigenmentBroadSheetTemplateID = TestAssigenmentBroadSheetTemplateID;
                        //            context.StudentScores.InsertOnSubmit(scores);
                        //            context.SubmitChanges();
                        //            //scoreList.Add(score);
                        //            //new ScoresheetLIB().SaveStudentTestAssignmentScore(scoreList);
                        //        }
                        //        else
                        //        {
                        //            //decimal totalScore = Convert.ToDecimal(txtTotalMark.Text.Trim());
                        //            decimal totalScore = Convert.ToDecimal(examScoreObtainable) + Convert.ToDecimal(getScoreObj.ExamScoreObtainable);
                        //            decimal newScore = Convert.ToInt64(examScores) + Convert.ToDecimal(getScoreObj.ExamScore);
                        //            decimal tsScore = newScore / totalScore;
                        //            int examPercentage = Convert.ToInt16(subCategory.Percentage);
                        //            decimal percentageScore = tsScore * examPercentage;
                        //            decimal ExamPercentageScore = Convert.ToDecimal((percentageScore / 100) * scoreCategory.Percentage);
                        //            decimal? finalScore = (ExamPercentageScore * subSch.MaximumScore) / 100;

                        //            getScoreObj.AdmissionNumber = lblAdmNo.Text.ToString();
                        //            getScoreObj.StudentId = student.Id;
                        //            getScoreObj.ExamScoreObtainable = Convert.ToInt16(totalScore);
                        //            getScoreObj.ExamScore = Convert.ToDecimal(newScore);
                        //            getScoreObj.TermId = termId;
                        //            getScoreObj.AcademicSessionID = academicSessionId;
                        //            getScoreObj.ClassId = Convert.ToInt64(classId);
                        //            getScoreObj.GradeId = Convert.ToInt64(gradeId);
                        //            getScoreObj.SubjectId = Convert.ToInt16(subId);
                        //            if (subSch.DepartmentId != null)
                        //            {
                        //                getScoreObj.DepartmentId = Convert.ToInt64(subSch.DepartmentId);
                        //            }
                        //            getScoreObj.Percentage = subCategory.Percentage;
                        //            getScoreObj.PercentageScore = percentageScore;
                        //            getScoreObj.ExamPercentage = scoreCategory.Percentage;
                        //            getScoreObj.ExamPercentageScore = ExamPercentageScore;
                        //            getScoreObj.SubjectMaxScore = subSch.MaximumScore;
                        //            getScoreObj.FinalScore = finalScore;
                        //            getScoreObj.CategoryId = scoreCategory.Id;
                        //            getScoreObj.SubCategoryId = subCategory.Id;
                        //            getScoreObj.CampusId = logonUser.SchoolCampusId;
                        //            getScoreObj.SchoolId = logonUser.SchoolId;
                        //            if (subTeacher != null)
                        //            {
                        //                getScoreObj.SubjectTeacherId = Convert.ToInt64(subTeacher.TeacherId);
                        //            }
                        //            getScoreObj.UploadedById = logonUser.Id;
                        //            getScoreObj.BroadSheetTemplateID = tempalateId;
                        //            //getScoreObj.Remark = txtRemark.Text.Trim();
                        //            //getScoreObj.Description = txtDescription.Text.Trim();
                        //            getScoreObj.Count = getScoreObj.Count + 1;
                        //            getScoreObj.Date = DateTime.Now;
                        //            //score.TestAssigenmentBroadSheetTemplateID = TestAssigenmentBroadSheetTemplateID;
                        //            //scoreList.Add(getScoreObj);
                        //            context.SubmitChanges();
                        //        }
                        //        score.StatusCode = "C";
                        //        context.SubmitChanges();



                        //        StudentScoreTemporary scoreTemp = context.StudentScoreTemporaries.FirstOrDefault(sc => sc.UploadedById == logonUser.Id && sc.BroadSheetTemplateID == tempalateId && sc.SchoolId == logonUser.SchoolId && sc.CampusId == logonUser.SchoolCampusId);
                        //        //StudentScoreTemporary scoreTemp = context.StudentScoreTemporaries.FirstOrDefault(sc => sc.AdmissionNumber == lblAdmNo.Text.ToString() && sc.CategoryId == categoryId && sc.SubCategoryId == subCategoryId
                        //        //&& sc.SubjectId == subId && sc.TermId == termId && sc.AcademicSessionID == academicSessionId && sc.BroadSheetTemplateID == tempalateId
                        //        //&& sc.ClassId == classId && sc.GradeId == gradeId && sc.UploadedById == uploadedById && sc.SchoolId == logonUser.SchoolId && sc.CampusId == logonUser.SchoolCampusId);

                        //        if (scoreTemp != null)
                        //        {
                        //            context.StudentScoreTemporaries.DeleteOnSubmit(scoreTemp);
                        //            context.SubmitChanges();
                        //        }

                        //    }
                        //}

                    }
                    else
                    {
                        lblErrorMsg.Text = "Exam has been submitted or wrong code selected";
                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                        lblErrorMsg.Visible = true;
                    }

                }
                else
                {
                    lblErrorMsg.Text = "Kindly confirm you are uploading against previously generated template";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                }


                lblErrorMsg.Text = "Scores Uploaded Successfully";
                lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                lblErrorMsg.Visible = true;
                ClearContent();
                TestAssigenmentBroadSheetTemplate objTemplate = context.TestAssigenmentBroadSheetTemplates.FirstOrDefault(x => x.ID == broadsheettemplate.ID);
                objTemplate.HasSubmitted = true;
                context.SubmitChanges();//update result has been submitted

                btnCancel.Visible = false;
                btnSaveScore.Visible = false;
                gdvViewExtendedScores.DataBind();
                lblresltt.Visible = false;
                //BindExcelScores();

            }


            //catch (Exception ex) { throw ex; }

            // IList<StudentScoreRepository> scoreList = PASSIS.LIB.Utility.Utili.RetrieveStudentTestAssignmentScoresFromScoresheetBroad(fileLocation, termId, sessionId, logonUser.Id, Convert.ToInt32(subId), (long)logonUser.SchoolId, logonUser.SchoolCampusId, markObtainable, broadsheettemplate.ID);

            if (ddlCategory.SelectedItem.Text == "CA")
            {
                //ScoreSubCategoryConfiguration subCategory = context.ScoreSubCategoryConfigurations.FirstOrDefault(x => x.Id == Convert.ToInt64(ddlSubCategory.SelectedValue));
                //if (subCategory == null)
                //{
                //    lblErrorMsg.Text = "Kindly set the score sub category";
                //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //    lblErrorMsg.Visible = true;
                //    return;
                //}
                //if (txtCode.Text == "")
                //{
                //    lblErrorMsg.Text = "Kindly enter the code";
                //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                //    lblErrorMsg.Visible = true;
                //    return;
                //}




                string TemplateCode = ddlDescription.SelectedItem.Text.ToString();

                //confirm if file to be uploaded has been selected else notify the user to select file to upload
                //if (documentUpload.HasFile)
                //{

                //    string originalFileName = Path.GetFileName(documentUpload.PostedFile.FileName);
                //    string fileExtension = Path.GetExtension(documentUpload.PostedFile.FileName);

                //    //This is use to confirm if user is uploading against the previously generated template
                TestAssigenmentBroadSheetTemplate broadsheettemplate = db.TestAssigenmentBroadSheetTemplates.FirstOrDefault(x => x.BroadSheetDescriptionCode.DescriptionName == TemplateCode);

                //    //If template exist
                if (broadsheettemplate != null)
                {
                    //Confirm if the result has been uploaded
                    if (broadsheettemplate.HasSubmitted == false)
                    {
                        //            string[] subjectId = broadsheettemplate.SubjectId.Split(',');


                        //            string fileLocation = Server.MapPath("~/docs/ScoreSheets/") + originalFileName;
                        //            documentUpload.SaveAs(fileLocation);
                        //            IList<PASSIS.LIB.StudentScore> scoresFound = new List<PASSIS.LIB.StudentScore>();
                        //            Net.SourceForge.Koogra.Excel.Workbook wkbk = new Net.SourceForge.Koogra.Excel.Workbook(fileLocation);
                        //            Net.SourceForge.Koogra.Excel.Worksheet workSheet = wkbk.Sheets[0];
                        //            // long count = 0;
                        //            for (uint rowCount = workSheet.Rows.MinRow; rowCount <= workSheet.Rows.MaxRow; ++rowCount)
                        //            {
                        //                try
                        //                {


                        //                    if (rowCount != workSheet.Rows.MinRow)// jump the first row
                        //                    {

                        //                        //broadsheettemplate.TotalNumberofSubjectInserted = count;
                        //                        uint count = 0;
                        //                        uint newunit = 3;
                        //                        uint countnum;
                        //                        foreach (string subId in subjectId)
                        //                        {

                        var getTempScores = from scoreTemp in context.StudentScoreTemporaries
                                            where scoreTemp.BroadSheetTemplateID == broadsheettemplate.ID && scoreTemp.Code == Convert.ToInt64(txtCode.Text)
                                             && scoreTemp.IsCancelled == false && scoreTemp.AcademicSessionID == Convert.ToInt64(ddlSession.SelectedValue) && scoreTemp.TermId == Convert.ToInt64(ddlTerm.SelectedValue)
                                             && scoreTemp.ClassId == Convert.ToInt64(ddlYear.SelectedValue) && scoreTemp.GradeId == Convert.ToInt64(ddlGrade.SelectedValue)
                                             && scoreTemp.CategoryId == Convert.ToInt64(ddlCategory.SelectedValue) && scoreTemp.SubCategoryId == Convert.ToInt64(ddlSubCategory.SelectedValue) && scoreTemp.UploadedById == logonUser.Id
                                            select scoreTemp;


                        IList<StudentScoreTemporary> stdTmpScore = getTempScores.ToList<StudentScoreTemporary>();

                        foreach (StudentScoreTemporary scoreTmp in stdTmpScore)
                        {
                            PASSIS.LIB.SubjectTeacher subTeacher = context.SubjectTeachers.FirstOrDefault(x => x.SubjectId == Convert.ToInt64(scoreTmp.SubjectId));
                            PASSIS.LIB.Subject subject = context.Subjects.FirstOrDefault(x => x.Id == Convert.ToInt64(scoreTmp.SubjectId));
                            PASSIS.LIB.SubjectsInSchool subSch = context.SubjectsInSchools.FirstOrDefault(x => x.SubjectId == subject.Id && x.SchoolId == logonUser.SchoolId);


                            //PASSIS.LIB.User student = db.Users.FirstOrDefault(x => x.AdmissionNumber == lblAdmNo.Text.Trim() && x.SchoolId == logonUser.SchoolId && x.SchoolCampusId == logonUser.SchoolCampusId);
                            PASSIS.LIB.User student = db.Users.FirstOrDefault(x => x.AdmissionNumber == scoreTmp.AdmissionNumber && x.SchoolId == logonUser.SchoolId && x.SchoolCampusId == logonUser.SchoolCampusId);

                            ScoreSubCategoryConfiguration subCategory = context.ScoreSubCategoryConfigurations.FirstOrDefault(x => x.Id == Convert.ToInt64(scoreTmp.SubCategoryId));
                            ScoreCategoryConfiguration scoreCategory = context.ScoreCategoryConfigurations.FirstOrDefault(x => x.ClassId == Convert.ToInt64(scoreTmp.ClassId) && x.Category == ddlCategory.SelectedItem.Text && x.SessionId == scoreTmp.AcademicSessionID && x.TermId == scoreTmp.TermId && x.SchoolId == logonUser.SchoolId && x.CampusId == logonUser.SchoolCampusId);

                            PASSIS.LIB.StudentScoreRepositoryTransaction score = new PASSIS.LIB.StudentScoreRepositoryTransaction();
                            PASSIS.LIB.StudentScoreRepository scores = new StudentScoreRepository();
                            //Net.SourceForge.Koogra.Excel.Row row = new Net.SourceForge.Koogra.Excel.Row();
                            //Net.SourceForge.Koogra.Excel.Row row = workSheet.Rows[rowCount];
                            //do validation when u hv time 
                            //if (row.Cells[count + 3].Value.ToString() != "")
                            decimal CAScore = Convert.ToDecimal(scoreTmp.ExamScore);

                            try
                            { score.AdmissionNo = scoreTmp.AdmissionNumber; }
                            catch { }
                            score.StudentId = student.Id;
                            try { score.MarkObtained = Convert.ToInt64(CAScore); }
                            catch { }
                            score.MarkObtainable = Convert.ToInt16(scoreTmp.ExamScoreObtainable);
                            score.CategoryId = Convert.ToInt64(scoreTmp.CategoryId);
                            score.SubCategoryId = Convert.ToInt64(scoreTmp.SubCategoryId);
                            score.TermId = scoreTmp.TermId;
                            score.SessionId = scoreTmp.AcademicSessionID;
                            score.SubjectId = Convert.ToInt32(scoreTmp.SubjectId);
                            if (subSch.DepartmentId != null)
                            {
                                score.DepartmentId = Convert.ToInt64(subSch.DepartmentId);
                            }
                            score.CampusId = logonUser.SchoolCampusId;
                            score.SchoolId = logonUser.SchoolId;
                            if (subTeacher != null)
                            {
                                score.SubjectTeacherId = Convert.ToInt64(subTeacher.TeacherId);
                            }
                            score.UploadedById = scoreTmp.UploadedById;
                            score.TestAssigenmentBroadSheetTemplateID = scoreTmp.BroadSheetTemplateID;
                            score.ClassId = Convert.ToInt64(scoreTmp.ClassId);
                            score.GradeId = Convert.ToInt64(scoreTmp.GradeId);
                            score.Description = scoreTmp.Description;
                            score.Code = Convert.ToInt16(scoreTmp.Code);
                            score.Date = DateTime.Now;
                            score.StatusCode = "I";
                            score.IsCancelled = false;
                            context.StudentScoreRepositoryTransactions.InsertOnSubmit(score);
                            context.SubmitChanges();

                            StudentScoreRepository getScoreObj = context.StudentScoreRepositories.FirstOrDefault(x => x.AdmissionNo == scoreTmp.AdmissionNumber && x.StudentId == student.Id && x.TestAssigenmentBroadSheetTemplateID == scoreTmp.BroadSheetTemplateID && x.SubjectId == Convert.ToInt64(scoreTmp.SubjectId)
                            && x.CategoryId == Convert.ToInt64(scoreTmp.CategoryId) && x.SubCategoryId == Convert.ToInt64(scoreTmp.SubCategoryId) && x.TermId == scoreTmp.TermId && x.SessionId == scoreTmp.AcademicSessionID && x.ClassId == scoreTmp.ClassId && x.GradeId == scoreTmp.GradeId);
                            if (getScoreObj == null)
                            {
                                //Calculating the percentage for the first entry

                                decimal totalScore = Convert.ToDecimal(scoreTmp.ExamScoreObtainable);
                                decimal tsScore = Convert.ToInt64(CAScore) / totalScore;
                                int testPercentage = Convert.ToInt16(subCategory.Percentage);
                                decimal percentageScore = tsScore * testPercentage;
                                decimal CAPercentageScore = Convert.ToDecimal((percentageScore / 100) * scoreCategory.Percentage);
                                decimal? finalScore = (CAPercentageScore * subSch.MaximumScore) / 100;

                                scores.AdmissionNo = scoreTmp.AdmissionNumber;
                                scores.StudentId = student.Id;
                                scores.MarkObtainable = Convert.ToInt16(scoreTmp.ExamScoreObtainable);
                                scores.MarkObtained = Convert.ToDecimal(CAScore);
                                scores.TermId = scoreTmp.TermId;
                                scores.SessionId = scoreTmp.AcademicSessionID;
                                scores.ClassId = Convert.ToInt64(scoreTmp.ClassId);
                                scores.GradeId = Convert.ToInt64(scoreTmp.GradeId);
                                scores.SubjectId = Convert.ToInt16(scoreTmp.SubjectId);
                                if (subSch.DepartmentId != null)
                                {
                                    scores.DepartmentId = Convert.ToInt64(subSch.DepartmentId);
                                }
                                scores.Percentage = subCategory.Percentage;
                                scores.PercentageScore = percentageScore;
                                scores.CAPercentage = scoreCategory.Percentage;
                                scores.CAPercentageScore = CAPercentageScore;
                                scores.SubjectMaxScore = subSch.MaximumScore;
                                scores.FinalScore = finalScore;
                                scores.CategoryId = scoreCategory.Id;
                                scores.SubCategoryId = subCategory.Id;
                                //score.DepartmentId = departmentId;
                                scores.CampusId = logonUser.SchoolCampusId;
                                scores.SchoolId = logonUser.SchoolId;
                                if (subTeacher != null)
                                {
                                    scores.SubjectTeacherId = Convert.ToInt64(subTeacher.TeacherId);
                                }
                                scores.UploadedById = logonUser.Id;
                                scores.TestAssigenmentBroadSheetTemplateID = scoreTmp.BroadSheetTemplateID;
                                //scores.Remark = txtRemark.Text.Trim();
                                scores.Count = 1;
                                //score.Description = txtDescription.Text.Trim();
                                scores.Date = DateTime.Now;
                                //score.TestAssigenmentBroadSheetTemplateID = TestAssigenmentBroadSheetTemplateID;
                                context.StudentScoreRepositories.InsertOnSubmit(scores);
                                context.SubmitChanges();
                                //scoreList.Add(score);
                                //new ScoresheetLIB().SaveStudentTestAssignmentScore(scoreList);
                            }
                            else
                            {
                                //decimal totalScore = Convert.ToDecimal(txtTotalMark.Text.Trim());
                                decimal totalScore = Convert.ToDecimal(scoreTmp.ExamScoreObtainable) + Convert.ToDecimal(getScoreObj.MarkObtainable);
                                decimal newScore = Convert.ToInt64(CAScore) + Convert.ToDecimal(getScoreObj.MarkObtained);
                                decimal tsScore = newScore / totalScore;
                                int testPercentage = Convert.ToInt16(subCategory.Percentage);
                                decimal percentageScore = tsScore * testPercentage;
                                decimal CAPercentageScore = Convert.ToDecimal((percentageScore / 100) * scoreCategory.Percentage);
                                decimal? finalScore = (CAPercentageScore * subSch.MaximumScore) / 100;

                                getScoreObj.AdmissionNo = scoreTmp.AdmissionNumber;
                                getScoreObj.StudentId = student.Id;
                                getScoreObj.MarkObtainable = Convert.ToInt16(totalScore);
                                getScoreObj.MarkObtained = Convert.ToDecimal(newScore);
                                getScoreObj.TermId = scoreTmp.TermId;
                                getScoreObj.SessionId = scoreTmp.AcademicSessionID;
                                getScoreObj.ClassId = Convert.ToInt64(scoreTmp.ClassId);
                                getScoreObj.GradeId = Convert.ToInt64(scoreTmp.GradeId);
                                getScoreObj.SubjectId = Convert.ToInt16(scoreTmp.SubjectId);
                                if (subSch.DepartmentId != null)
                                {
                                    getScoreObj.DepartmentId = Convert.ToInt64(subSch.DepartmentId);
                                }
                                getScoreObj.Percentage = subCategory.Percentage;
                                getScoreObj.PercentageScore = percentageScore;
                                getScoreObj.CAPercentage = scoreCategory.Percentage;
                                getScoreObj.CAPercentageScore = CAPercentageScore;
                                getScoreObj.SubjectMaxScore = subSch.MaximumScore;
                                getScoreObj.FinalScore = finalScore;
                                getScoreObj.CategoryId = scoreCategory.Id;
                                getScoreObj.SubCategoryId = subCategory.Id;
                                getScoreObj.CampusId = logonUser.SchoolCampusId;
                                getScoreObj.SchoolId = logonUser.SchoolId;
                                if (subTeacher != null)
                                {
                                    getScoreObj.SubjectTeacherId = Convert.ToInt64(subTeacher.TeacherId);
                                }
                                getScoreObj.UploadedById = logonUser.Id;
                                getScoreObj.TestAssigenmentBroadSheetTemplateID = scoreTmp.BroadSheetTemplateID;
                                //getScoreObj.Remark = txtRemark.Text.Trim();
                                //getScoreObj.Description = txtDescription.Text.Trim();
                                getScoreObj.Count = getScoreObj.Count + 1;
                                getScoreObj.Date = DateTime.Now;
                                //score.TestAssigenmentBroadSheetTemplateID = TestAssigenmentBroadSheetTemplateID;
                                //scoreList.Add(getScoreObj);
                                context.SubmitChanges();
                            }

                            score.StatusCode = "C";
                            context.SubmitChanges();

                            //StudentScoreTemporary scoreTemp = context.StudentScoreTemporaries.FirstOrDefault(sc => sc.UploadedById == logonUser.Id && sc.BroadSheetTemplateID == tempalateId && sc.SchoolId == logonUser.SchoolId && sc.CampusId == logonUser.SchoolCampusId);
                            StudentScoreTemporary scoreTemp = context.StudentScoreTemporaries.FirstOrDefault(sc => sc.AdmissionNumber == scoreTmp.AdmissionNumber && sc.CategoryId == scoreTmp.CategoryId && sc.SubCategoryId == scoreTmp.SubCategoryId
                            && sc.SubjectId == scoreTmp.SubjectId && sc.TermId == scoreTmp.TermId && sc.AcademicSessionID == scoreTmp.AcademicSessionID && sc.BroadSheetTemplateID == scoreTmp.BroadSheetTemplateID
                            && sc.ClassId == scoreTmp.ClassId && sc.GradeId == scoreTmp.GradeId && sc.UploadedById == scoreTmp.UploadedById && sc.SchoolId == logonUser.SchoolId && sc.CampusId == logonUser.SchoolCampusId);

                            if (scoreTemp != null)
                            {
                                context.StudentScoreTemporaries.DeleteOnSubmit(scoreTemp);
                                context.SubmitChanges();
                            }

                        }









                        //foreach (GridViewRow row in gdvViewExtendedScores.Rows)
                        //{
                        //    if (row.RowType == DataControlRowType.DataRow)
                        //    {

                        //        Label lblAdmNo = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblAdmNo");
                        //        Label lblId = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblId");
                        //        Label subjId = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblSubjectId");
                        //        Label lblCategoryId = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblCategoryId");
                        //        Label lblSubCategoryId = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblSubCategoryId");
                        //        Label lblTermId = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblTermId");
                        //        Label lblAcademicSessionID = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblAcademicSessionID");
                        //        Label lblClassId = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblClassId");
                        //        Label lblGradeId = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblGradeId");
                        //        Label lblExamScoreObtainable = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblExamScoreObtainable");
                        //        Label lblExamScore = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblExamScore");
                        //        Label lblTemplateId = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblTemplateId");
                        //        Label lblCode = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblCode");
                        //        Label lblUploadedById = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblUploadedById");
                        //        Label lblDescription = (Label)gdvViewExtendedScores.Rows[row.RowIndex].FindControl("lblDescription");


                        //        long? studentId = Convert.ToInt64(lblId.Text.ToString().Trim());
                        //        long? subId = Convert.ToInt64(subjId.Text.Trim());
                        //        long? categoryId = Convert.ToInt64(lblCategoryId.Text.Trim());
                        //        long? subCategoryId = Convert.ToInt64(lblSubCategoryId.Text.Trim());
                        //        long? termId = Convert.ToInt64(lblTermId.Text.Trim());
                        //        long? academicSessionId = Convert.ToInt64(lblAcademicSessionID.Text.Trim());
                        //        long? classId = Convert.ToInt64(lblClassId.Text.Trim());
                        //        long? gradeId = Convert.ToInt64(lblGradeId.Text.Trim());
                        //        long? examScoreObtainable = Convert.ToInt64(lblExamScoreObtainable.Text.Trim());
                        //        string examScore = lblExamScore.Text.Trim();
                        //        long? tempalateId = Convert.ToInt64(lblTemplateId.Text.Trim());
                        //        long? code = Convert.ToInt64(lblCode.Text.Trim());
                        //        long? uploadedById = Convert.ToInt64(lblUploadedById.Text.Trim());
                        //        string description = lblDescription.Text.Trim();



                        //        PASSIS.LIB.SubjectTeacher subTeacher = context.SubjectTeachers.FirstOrDefault(x => x.SubjectId == Convert.ToInt64(subId));
                        //        PASSIS.LIB.Subject subject = context.Subjects.FirstOrDefault(x => x.Id == Convert.ToInt64(subId));
                        //        PASSIS.LIB.SubjectsInSchool subSch = context.SubjectsInSchools.FirstOrDefault(x => x.SubjectId == subject.Id && x.SchoolId == logonUser.SchoolId);
                        //        //for (uint count = 0; count <= broadsheettemplate.TotalNumberofSubjectInserted; count++ )
                        //        //{
                        //        //// countnum = count + newunit ;
                        //        //if (count <= broadsheettemplate.TotalNumberofSubjectInserted)
                        //        //{
                        //        //    Net.SourceForge.Koogra.Excel.Row row = workSheet.Rows[rowCount];
                        //        //    if (row.Cells[count + 3] != null)
                        //        //    {
                        //        PASSIS.LIB.User student = db.Users.FirstOrDefault(x => x.AdmissionNumber == lblAdmNo.Text.ToString().Trim() && x.SchoolId == logonUser.SchoolId && x.SchoolCampusId == logonUser.SchoolCampusId);

                        //        //StudentScoreRepositoryTransaction trans = context.StudentScoreRepositoryTransactions.FirstOrDefault(x => x.TestAssigenmentBroadSheetTemplateID == broadsheettemplate.ID
                        //        //    && x.Code == Convert.ToInt16(txtCode.Text) && x.StudentId == student.Id && x.SubjectId == Convert.ToInt64(subId)
                        //        //    && x.CategoryId == Convert.ToInt64(ddlCategory.SelectedValue) && x.SubCategoryId == Convert.ToInt64(ddlSubCategory.SelectedValue)
                        //        //    && x.TermId == termId && x.SessionId == sessionId && x.ClassId == yearId && x.GradeId == gradeId);
                        //        //if (Convert.ToInt16(txtCode.Text) < 1)
                        //        //{
                        //        //    lblErrorMsg.Text = "Code should not be lesser than 1";
                        //        //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                        //        //    lblErrorMsg.Visible = true;
                        //        //    return;
                        //        //}

                        //        //if (trans != null)
                        //        //{
                        //        //    lblErrorMsg.Text = "Code has been used for this sub category";
                        //        //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                        //        //    lblErrorMsg.Visible = true;
                        //        //    continue;
                        //        //}
                        //        //else
                        //        //{
                        //        //    StudentScoreRepositoryTransaction transs = context.StudentScoreRepositoryTransactions.FirstOrDefault(x => x.TestAssigenmentBroadSheetTemplateID == broadsheettemplate.ID && x.Code == Convert.ToInt16(txtCode.Text) - 1 && x.StudentId == student.Id && x.SubjectId == Convert.ToInt64(subId)
                        //        //        && x.CategoryId == Convert.ToInt64(ddlCategory.SelectedValue) && x.SubCategoryId == Convert.ToInt64(ddlSubCategory.SelectedValue)
                        //        //        && x.TermId == termId && x.SessionId == sessionId && x.ClassId == yearId && x.GradeId == gradeId);
                        //        //    if (transs == null && Convert.ToInt16(txtCode.Text) - 1 != 0)
                        //        //    {
                        //        //        lblErrorMsg.Text = "Enter lesser number for the code";
                        //        //        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                        //        //        lblErrorMsg.Visible = true;
                        //        //        return;
                        //        //    }
                        //        //}
                        //        ScoreSubCategoryConfiguration subCategory = context.ScoreSubCategoryConfigurations.FirstOrDefault(x => x.Id == Convert.ToInt64(subCategoryId));
                        //        ScoreCategoryConfiguration scoreCategory = context.ScoreCategoryConfigurations.FirstOrDefault(x => x.ClassId == Convert.ToInt64(classId) && x.Category == ddlCategory.SelectedItem.Text && x.SessionId == academicSessionId && x.TermId == termId && x.SchoolId == logonUser.SchoolId && x.CampusId == logonUser.SchoolCampusId);

                        //        PASSIS.LIB.StudentScoreRepositoryTransaction score = new PASSIS.LIB.StudentScoreRepositoryTransaction();
                        //        PASSIS.LIB.StudentScoreRepository scores = new StudentScoreRepository();
                        //        //Net.SourceForge.Koogra.Excel.Row row = new Net.SourceForge.Koogra.Excel.Row();
                        //        //Net.SourceForge.Koogra.Excel.Row row = workSheet.Rows[rowCount];
                        //        //do validation when u hv time 
                        //        //if (row.Cells[count + 3].Value.ToString() != "")
                        //        decimal CAScore = Convert.ToDecimal(examScore);

                        //        try
                        //        { score.AdmissionNo = lblAdmNo.Text.ToString(); }
                        //        catch { }
                        //        score.StudentId = student.Id;
                        //        try { score.MarkObtained = Convert.ToInt64(CAScore); }
                        //        catch { }
                        //        score.MarkObtainable = Convert.ToInt16(examScoreObtainable);
                        //        score.CategoryId = Convert.ToInt64(categoryId);
                        //        score.SubCategoryId = Convert.ToInt64(subCategoryId);
                        //        score.TermId = termId;
                        //        score.SessionId = academicSessionId;
                        //        score.SubjectId = Convert.ToInt32(subId);
                        //        if (subSch.DepartmentId != null)
                        //        {
                        //            score.DepartmentId = Convert.ToInt64(subSch.DepartmentId);
                        //        }
                        //        score.CampusId = logonUser.SchoolCampusId;
                        //        score.SchoolId = logonUser.SchoolId;
                        //        if (subTeacher != null)
                        //        {
                        //            score.SubjectTeacherId = Convert.ToInt64(subTeacher.TeacherId);
                        //        }
                        //        score.UploadedById = uploadedById;
                        //        score.TestAssigenmentBroadSheetTemplateID = tempalateId;
                        //        score.ClassId = Convert.ToInt64(classId);
                        //        score.GradeId = Convert.ToInt64(gradeId);
                        //        score.Description = description;
                        //        score.Code = Convert.ToInt16(code);
                        //        score.Date = DateTime.Now;
                        //        score.StatusCode = "I";
                        //        score.IsCancelled = false;
                        //        context.StudentScoreRepositoryTransactions.InsertOnSubmit(score);
                        //        context.SubmitChanges();

                        //        StudentScoreRepository getScoreObj = context.StudentScoreRepositories.FirstOrDefault(x => x.AdmissionNo == lblAdmNo.Text.ToString() && x.StudentId == student.Id && x.TestAssigenmentBroadSheetTemplateID == tempalateId && x.SubjectId == Convert.ToInt64(subId)
                        //            && x.CategoryId == Convert.ToInt64(categoryId) && x.SubCategoryId == Convert.ToInt64(subCategoryId) && x.TermId == termId && x.SessionId == academicSessionId && x.ClassId == classId && x.GradeId == gradeId);
                        //        if (getScoreObj == null)
                        //        {
                        //            //Calculating the percentage for the first entry

                        //            decimal totalScore = Convert.ToDecimal(examScoreObtainable);
                        //            decimal tsScore = Convert.ToInt64(CAScore) / totalScore;
                        //            int testPercentage = Convert.ToInt16(subCategory.Percentage);
                        //            decimal percentageScore = tsScore * testPercentage;
                        //            decimal CAPercentageScore = Convert.ToDecimal((percentageScore / 100) * scoreCategory.Percentage);
                        //            decimal? finalScore = (CAPercentageScore * subSch.MaximumScore) / 100;

                        //            scores.AdmissionNo = lblAdmNo.Text.ToString();
                        //            scores.StudentId = student.Id;
                        //            scores.MarkObtainable = Convert.ToInt16(examScoreObtainable);
                        //            scores.MarkObtained = Convert.ToDecimal(CAScore);
                        //            scores.TermId = termId;
                        //            scores.SessionId = academicSessionId;
                        //            scores.ClassId = Convert.ToInt64(classId);
                        //            scores.GradeId = Convert.ToInt64(gradeId);
                        //            scores.SubjectId = Convert.ToInt16(subId);
                        //            if (subSch.DepartmentId != null)
                        //            {
                        //                scores.DepartmentId = Convert.ToInt64(subSch.DepartmentId);
                        //            }
                        //            scores.Percentage = subCategory.Percentage;
                        //            scores.PercentageScore = percentageScore;
                        //            scores.CAPercentage = scoreCategory.Percentage;
                        //            scores.CAPercentageScore = CAPercentageScore;
                        //            scores.SubjectMaxScore = subSch.MaximumScore;
                        //            scores.FinalScore = finalScore;
                        //            scores.CategoryId = scoreCategory.Id;
                        //            scores.SubCategoryId = subCategory.Id;
                        //            //score.DepartmentId = departmentId;
                        //            scores.CampusId = logonUser.SchoolCampusId;
                        //            scores.SchoolId = logonUser.SchoolId;
                        //            if (subTeacher != null)
                        //            {
                        //                scores.SubjectTeacherId = Convert.ToInt64(subTeacher.TeacherId);
                        //            }
                        //            scores.UploadedById = logonUser.Id;
                        //            scores.TestAssigenmentBroadSheetTemplateID = tempalateId;
                        //            //scores.Remark = txtRemark.Text.Trim();
                        //            scores.Count = 1;
                        //            //score.Description = txtDescription.Text.Trim();
                        //            scores.Date = DateTime.Now;
                        //            //score.TestAssigenmentBroadSheetTemplateID = TestAssigenmentBroadSheetTemplateID;
                        //            context.StudentScoreRepositories.InsertOnSubmit(scores);
                        //            context.SubmitChanges();
                        //            //scoreList.Add(score);
                        //            //new ScoresheetLIB().SaveStudentTestAssignmentScore(scoreList);
                        //        }
                        //        else
                        //        {
                        //            //decimal totalScore = Convert.ToDecimal(txtTotalMark.Text.Trim());
                        //            decimal totalScore = Convert.ToDecimal(examScoreObtainable) + Convert.ToDecimal(getScoreObj.MarkObtainable);
                        //            decimal newScore = Convert.ToInt64(CAScore) + Convert.ToDecimal(getScoreObj.MarkObtained);
                        //            decimal tsScore = newScore / totalScore;
                        //            int testPercentage = Convert.ToInt16(subCategory.Percentage);
                        //            decimal percentageScore = tsScore * testPercentage;
                        //            decimal CAPercentageScore = Convert.ToDecimal((percentageScore / 100) * scoreCategory.Percentage);
                        //            decimal? finalScore = (CAPercentageScore * subSch.MaximumScore) / 100;

                        //            getScoreObj.AdmissionNo = lblAdmNo.Text.ToString();
                        //            getScoreObj.StudentId = student.Id;
                        //            getScoreObj.MarkObtainable = Convert.ToInt16(totalScore);
                        //            getScoreObj.MarkObtained = Convert.ToDecimal(newScore);
                        //            getScoreObj.TermId = termId;
                        //            getScoreObj.SessionId = academicSessionId;
                        //            getScoreObj.ClassId = Convert.ToInt64(classId);
                        //            getScoreObj.GradeId = Convert.ToInt64(gradeId);
                        //            getScoreObj.SubjectId = Convert.ToInt16(subId);
                        //            if (subSch.DepartmentId != null)
                        //            {
                        //                getScoreObj.DepartmentId = Convert.ToInt64(subSch.DepartmentId);
                        //            }
                        //            getScoreObj.Percentage = subCategory.Percentage;
                        //            getScoreObj.PercentageScore = percentageScore;
                        //            getScoreObj.CAPercentage = scoreCategory.Percentage;
                        //            getScoreObj.CAPercentageScore = CAPercentageScore;
                        //            getScoreObj.SubjectMaxScore = subSch.MaximumScore;
                        //            getScoreObj.FinalScore = finalScore;
                        //            getScoreObj.CategoryId = scoreCategory.Id;
                        //            getScoreObj.SubCategoryId = subCategory.Id;
                        //            getScoreObj.CampusId = logonUser.SchoolCampusId;
                        //            getScoreObj.SchoolId = logonUser.SchoolId;
                        //            if (subTeacher != null)
                        //            {
                        //                getScoreObj.SubjectTeacherId = Convert.ToInt64(subTeacher.TeacherId);
                        //            }
                        //            getScoreObj.UploadedById = logonUser.Id;
                        //            getScoreObj.TestAssigenmentBroadSheetTemplateID = tempalateId;
                        //            //getScoreObj.Remark = txtRemark.Text.Trim();
                        //            //getScoreObj.Description = txtDescription.Text.Trim();
                        //            getScoreObj.Count = getScoreObj.Count + 1;
                        //            getScoreObj.Date = DateTime.Now;
                        //            //score.TestAssigenmentBroadSheetTemplateID = TestAssigenmentBroadSheetTemplateID;
                        //            //scoreList.Add(getScoreObj);
                        //            context.SubmitChanges();
                        //        }

                        //        score.StatusCode = "C";
                        //        context.SubmitChanges();

                        //        StudentScoreTemporary scoreTemp = context.StudentScoreTemporaries.FirstOrDefault(sc => sc.UploadedById == logonUser.Id && sc.BroadSheetTemplateID == tempalateId && sc.SchoolId == logonUser.SchoolId && sc.CampusId == logonUser.SchoolCampusId);
                        //        //StudentScoreTemporary scoreTemp = context.StudentScoreTemporaries.FirstOrDefault(sc => sc.AdmissionNumber == lblAdmNo.Text.ToString() && sc.CategoryId == categoryId && sc.SubCategoryId == subCategoryId
                        //        //&& sc.SubjectId == subId && sc.TermId == termId && sc.AcademicSessionID == academicSessionId && sc.BroadSheetTemplateID == tempalateId
                        //        //&& sc.ClassId == classId && sc.GradeId == gradeId && sc.UploadedById == uploadedById && sc.SchoolId == logonUser.SchoolId && sc.CampusId == logonUser.SchoolCampusId);

                        //        if (scoreTemp != null)
                        //        {
                        //            context.StudentScoreTemporaries.DeleteOnSubmit(scoreTemp);
                        //            context.SubmitChanges();
                        //        }

                        //    }
                        //}
                    }

                    else
                    {
                        lblErrorMsg.Text = "Exam has been submitted or wrong code selected";
                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                        lblErrorMsg.Visible = true;
                    }
                }
                else
                {
                    lblErrorMsg.Text = "Kindly confirm you are uploading against previously generated template";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                }

                lblErrorMsg.Text = "Scores Uploaded Successfully";
                lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                lblErrorMsg.Visible = true;
                ClearContent();
                TestAssigenmentBroadSheetTemplate objTemplate = context.TestAssigenmentBroadSheetTemplates.FirstOrDefault(x => x.ID == broadsheettemplate.ID);
                objTemplate.HasSubmitted = true;
                context.SubmitChanges();//update result has been submitted

                btnCancel.Visible = false;
                btnSaveScore.Visible = false;
                gdvViewExtendedScores.DataBind();
                lblresltt.Visible = false;
               // BindExcelScores();


            }

        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = "Error occurred, kindly contact your administrator";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
        }
    }

    private void ClearContent()
    {

        //ddlDescription.SelectedIndex = 0;
        //ddlGrade.SelectedIndex = 0;
        //ddlSession.SelectedIndex = 0;
        //ddlTerm.SelectedIndex = 0;
        //ddlYear.SelectedIndex = 0;

    }
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Populate Sub Category
        ddlSubCategory.Items.Clear();
        var categoryList = from s in context.ScoreSubCategoryConfigurations where s.CategoryId == Convert.ToInt64(ddlCategory.SelectedValue) select s;
        ddlSubCategory.DataSource = categoryList;
        ddlSubCategory.DataBind();
        ddlSubCategory.Items.Insert(0, new ListItem("--Select--", "0", true));
    }
}