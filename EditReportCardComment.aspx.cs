using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;

public partial class EditReportCardComment : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //populate ddlsession
            ddlSession.DataSource = schSession().Distinct();
            ddlSession.DataTextField = "SessionName";
            ddlSession.DataValueField = "ID";
            ddlSession.DataBind();
            ddlSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Session--", "0", true));
            //populate ddlterm
            ddlTerm.DataSource = new AcademicTermLIB().RetrieveAcademicTerm();
            ddlTerm.DataBind();
            ddlTerm.Items.Insert(0, new ListItem("--Select--", "0", true));
            //populate year
            long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
            long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
            ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
            ddlYear.DataBind();
            //populate comment for
            PASSIS.LIB.UserRole userRole = context.UserRoles.FirstOrDefault(x => x.UserId == logonUser.Id);
            if (userRole != null)
            {
                if (userRole.RoleId == 5)
                {
                    ddlCommentFor.DataSource = from s in context.ReportCardCommentConfigs where s.ID == 4 select s;
                    ddlCommentFor.DataBind();
                    ddlCommentFor.Items.Insert(0, new ListItem("--Select--", "0", true));
                }
                else
                {
                    ddlCommentFor.DataSource = from s in context.ReportCardCommentConfigs where s.ID != 4 select s;
                    ddlCommentFor.DataBind();
                    ddlCommentFor.Items.Insert(0, new ListItem("--Select--", "0", true));
                }
            }
        }
    }
    // Method for current session
    public IList<AcademicSessionName> schSession()
    {

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
        var availableGrades = new ClassGradeDAL().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, yearId);
        ddlGrade.DataSource = availableGrades;
        ddlGrade.DataBind();
        ddlGrade.Items.Insert(0, new ListItem("--Select--", "0", true));
    }
    protected void ddlGrade_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        lblAcademic.Visible = true;
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

        try
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
            if (ddlCommentFor.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select Comment For";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }


            Int64 yearId = 0L, gradeId = 0L, schoolId = 0L, campusId = 0L, configId = 0L, classId = 0L, sessionId = 0L, termId = 0L, userId = 0L;
            yearId = Convert.ToInt64(ddlYear.SelectedValue);
            gradeId = Convert.ToInt64(ddlGrade.SelectedValue);
            termId = Convert.ToInt64(ddlTerm.SelectedValue);
            classId = Convert.ToInt64(ddlYear.SelectedValue);
            sessionId = Convert.ToInt64(ddlSession.SelectedValue);
            configId = Convert.ToInt64(ddlCommentFor.SelectedValue);

            schoolId = (long)logonUser.SchoolId;
            campusId = logonUser.SchoolCampusId;
            userId = logonUser.Id;
            PASSIS.LIB.UserRole userRole = context.UserRoles.FirstOrDefault(x => x.UserId == logonUser.Id);
            if (userRole != null)
            {
                BindComment();
            }
            lblErrorMsg.Text = "Kindly edit the comments in the table below";
            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
            lblErrorMsg.Visible = true;

            //btnSaveComment.Visible = true;
        }

        catch (Exception ex)
        {
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
            lblErrorMsg.Text = "Error occured, kindly contact your administrator";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
        }
    }
    protected void btnSaveComment_Click(object sender, EventArgs e)
    {
        Int64 yearId = Convert.ToInt64(ddlYear.SelectedValue);
        Int64 gradeId = Convert.ToInt64(ddlGrade.SelectedValue);
        Int64 uploadedById = logonUser.Id;
        //Int64 departmentId = 0;
        Int64 sessionId = Convert.ToInt64(ddlSession.SelectedValue);
        Int64 termId = Convert.ToInt64(ddlTerm.SelectedValue);


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
        if (ddlCommentFor.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select Comment For";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }

        //    foreach (GridViewRow row in gvComment.Rows)
        //    {
        //        if (row.RowType == DataControlRowType.DataRow)
        //        {


        //            DropDownList ddlComment = (DropDownList)gvComment.Rows[row.RowIndex].FindControl("ddlCommenttt");

        //            Label lblAdmNo = (Label)gvComment.Rows[row.RowIndex].FindControl("lblAdmNo");
        //            Label lblId = (Label)gvComment.Rows[row.RowIndex].FindControl("lblId");
        //            TextBox txtComment = (TextBox)gvComment.Rows[row.RowIndex].FindControl("txtComment");
        //            TextBox txtRemark = (TextBox)gvComment.Rows[row.RowIndex].FindControl("txtRemark");
        //            string comment = txtComment.Text.Trim();
        //            long studentId = Convert.ToInt64(lblId.Text.ToString().Trim());
        //            if (comment != "")
        //            {
        //                PASSIS.LIB.ReportCardComment objComment = context.ReportCardComments.FirstOrDefault(x => x.StudentId == studentId && x.AcademicSessionID == sessionId && x.TermId == termId && x.CommentConfigId == Convert.ToInt64(ddlCommentFor.SelectedValue));
        //                if (objComment == null)
        //                {
        //                    PASSIS.LIB.ReportCardComment newObjComment = new PASSIS.LIB.ReportCardComment();
        //                    newObjComment.AdmissionNumber = lblAdmNo.Text;
        //                    newObjComment.StudentId = Convert.ToInt64(lblId.Text.ToString().Trim());
        //                    newObjComment.Comment = comment;
        //                    newObjComment.CommentConfigId = Convert.ToInt64(ddlCommentFor.SelectedValue);
        //                    newObjComment.TermId = termId;
        //                    newObjComment.AcademicSessionID = sessionId;
        //                    newObjComment.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
        //                    newObjComment.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
        //                    newObjComment.CampusId = logonUser.SchoolCampusId;
        //                    newObjComment.SchoolId = logonUser.SchoolId;
        //                    newObjComment.UploadedById = logonUser.Id;
        //                    newObjComment.Remark = txtRemark.Text.Trim();
        //                    newObjComment.Date = DateTime.Now;
        //                    context.ReportCardComments.InsertOnSubmit(newObjComment);
        //                    context.SubmitChanges();
        //                }
        //                else
        //                {
        //                    objComment.AdmissionNumber = lblAdmNo.Text;
        //                    objComment.StudentId = Convert.ToInt64(lblId.Text.ToString().Trim());
        //                    objComment.Comment = comment;
        //                    objComment.CommentConfigId = Convert.ToInt64(ddlCommentFor.SelectedValue);
        //                    objComment.TermId = termId;
        //                    objComment.AcademicSessionID = sessionId;
        //                    objComment.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
        //                    objComment.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
        //                    objComment.CampusId = logonUser.SchoolCampusId;
        //                    objComment.SchoolId = logonUser.SchoolId;
        //                    objComment.UploadedById = logonUser.Id;
        //                    objComment.Remark = txtRemark.Text.Trim();
        //                    objComment.Date = DateTime.Now;
        //                    context.SubmitChanges();
        //                }
        //            }
        //        }
        //    }
        //    lblErrorMsg.Text = "Uploaded Successfully";
        //    lblErrorMsg.ForeColor = System.Drawing.Color.Green;
        //    lblErrorMsg.Visible = true;
        //}

    }

    public void BindComment()
    {
        Int64 yearId = 0L, gradeId = 0L, schoolId = 0L, campusId = 0L, configId = 0L, classId = 0L, sessionId = 0L, termId = 0L, userId = 0L;
        yearId = Convert.ToInt64(ddlYear.SelectedValue);
        gradeId = Convert.ToInt64(ddlGrade.SelectedValue);
        termId = Convert.ToInt64(ddlTerm.SelectedValue);
        classId = Convert.ToInt64(ddlYear.SelectedValue);
        sessionId = Convert.ToInt64(ddlSession.SelectedValue);
        configId = Convert.ToInt64(ddlCommentFor.SelectedValue);

        schoolId = (long)logonUser.SchoolId;
        campusId = logonUser.SchoolCampusId;
        userId = logonUser.Id;
        var getComment = from s in context.ReportCardComments
        where s.UploadedById == userId && s.TermId == termId && s.CommentConfigId == configId
        && s.AcademicSessionID == sessionId && s.GradeId == gradeId && s.ClassId == yearId && s.SchoolId == schoolId && s.CampusId == campusId
        select s;
        gvComment.DataSource = getComment.ToList();
        gvComment.DataBind();
    }


    protected void gvComment_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            Label lblId = (Label)gvComment.Rows[e.RowIndex].FindControl("lblId");
            TextBox txtComment = (TextBox)gvComment.Rows[e.RowIndex].FindControl("txtComment");
            TextBox txtRemark = (TextBox)gvComment.Rows[e.RowIndex].FindControl("txtRemark");

            PASSISLIBDataContext context = new PASSISLIBDataContext();
            ReportCardComment editComm = context.ReportCardComments.FirstOrDefault(x => x.Id == Convert.ToInt64(lblId.Text));
            string comment = txtComment.Text;
            string remark = txtRemark.Text;

            editComm.Comment = comment;
            editComm.Remark = remark;

            context.SubmitChanges();
            gvComment.EditIndex = -1;
            BindComment();

            lblMessage.Text = "Updated Successfully";
            lblMessage.ForeColor = System.Drawing.Color.Green;



        }

        catch (Exception ex)
        {
            lblMessage.Text = "Error occurred, try again";
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }

    public string studentFullName(long studentId)
    {
        string fullName = string.Empty;
        var name = context.Users.FirstOrDefault(u => u.Id == studentId);
        fullName = name.StudentFullName;
        return fullName;
    }
    protected void gvComment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lblErrorMsg.Visible = false;
        gvComment.EditIndex = -1;
        BindComment();
    }

    protected void gvComment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvComment.PageIndex = e.NewPageIndex;
        BindComment();
    }
    protected void gvComment_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvComment.EditIndex = e.NewEditIndex;
        BindComment();
    }


    //    protected void gvComment_RowDataBound(object sender, GridViewRowEventArgs e)
    //    {
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            //if ((e.Row.RowState) > 0)
    //            //{
    //            DropDownList ddlComment = (e.Row.FindControl("ddlComment") as DropDownList);
    //            var comm = from c in context.RpComments
    //                       where c.SchoolId == logonUser.SchoolId && c.CampusId == logonUser.SchoolCampusId
    //&& c.UploadedById == logonUser.Id
    //                       select c;
    //            ddlComment.DataSource = comm.ToList();
    //            ddlComment.DataTextField = "Comment";
    //            ddlComment.DataValueField = "Id";
    //            ddlComment.DataBind();
    //            ddlComment.Items.Insert(0, new ListItem("--Select Comment--", "0", true));
    //            //}
    //        }
    //    }

    //protected void ddlComment_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    DropDownList ddl = (DropDownList)sender;
    //    GridViewRow row = (GridViewRow)ddl.NamingContainer;
    //    TextBox txtCom = (TextBox)row.FindControl("txtComment");

    //    if (ddl.SelectedIndex != 0)
    //    {
    //        txtCom.Text = ddl.SelectedItem.Text;
    //        txtCom.Enabled = false;
    //    }
    //    else { txtCom.Text = ""; txtCom.Enabled = true; }
    //}
}