using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO;
using PASSIS.DAO.Utility;
using System.IO;
using PASSIS.LIB;
using System.Text;

public partial class TeacherViewAssignments : PASSIS.LIB.Utility.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid();

        }
        //BindGrid();
    }

    protected string getClassOrGroupName(object gradeId, object groupId)
    {
        if (gradeId == null)
        {
            return new GroupingDAL().RetrieveGrouping(Convert.ToInt64(groupId)).GroupName;
        }
        else
        {
            return new ClassGradeDAL().RetrieveGrade(Convert.ToInt64(gradeId)).GradeName;
        }
    }
    protected void gdvList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "cmd")
        {
            string filename = e.CommandArgument.ToString();
            string path = MapPath("~/docs/SubmittedAssignment/" + filename);
            byte[] bts = System.IO.File.ReadAllBytes(path);
            Response.Clear();
            Response.ClearHeaders();
            Response.AddHeader("Content-Type", "Application/msword");
            Response.AddHeader("Content-Length", bts.Length.ToString());
            Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
            Response.BinaryWrite(bts);
            Response.Flush();
            Response.End();
        }
    }


    protected void BindGrid()
    {
        gdvList.DataSource = new AssignmentDAL().getSubmittedAssignment((long)logonUser.Id);
        gdvList.DataBind();

    }


    //protected void btnSave_OnClick(object sender, EventArgs e)
    //{
    //    //validate the value and save
    //    //impose restriction on file sixe
    //    Assignment asgnmt = new Assignment();
    //    try
    //    {
    //        if (string.IsNullOrEmpty(txtDesc.Text))
    //        {
    //            lblErrorMsg.Text = string.Format("A short document description is required.");
    //            lblErrorMsg.Visible = true;
    //            return;
    //        }


    //        if (documentUpload.HasFile)
    //        {
    //            string originalFileName = Path.GetFileName(documentUpload.PostedFile.FileName);
    //            string modifiedFileName = string.Format("{0}{1}", DateTime.Now.ToString("MMddHmmss"), originalFileName);
    //            string fileExtension = Path.GetExtension(documentUpload.PostedFile.FileName);
    //            string fileLocation = Server.MapPath("~/docs/Assignment/") + modifiedFileName;
    //            documentUpload.SaveAs(fileLocation);

    //            //Check whether file extension is xls or xslx
    //            //if (!(fileExtension.Contains(".doc") || fileExtension.Contains(".docx") || fileExtension.Contains(".xls") || fileExtension.Contains(".xlsx")))
    //            if (!(fileExtension.Contains(".doc")))
    //            {
    //                lblErrorMsg.Text = string.Format("Upload not succesful. The file format is not supported!!!");
    //                lblErrorMsg.Visible = true;
    //                return;
    //            }
    //            asgnmt.DateUploaded = DateTime.Now;
    //            asgnmt.Description = txtDesc.Text.Trim();
    //            asgnmt.FileName = modifiedFileName;
    //            //asgnmt.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
    //            asgnmt.GradeId = new ClassGradeDAL().RetrieveTeacherGrade(logonUser.Id).GradeId;
    //            asgnmt.MaximumObtainableScore = Convert.ToDecimal(txtScore.Text);
    //            asgnmt.SessionId = CurrentAcademicSession.Id;
    //            asgnmt.SubjectId = Convert.ToInt64(ddlSubject.SelectedValue);
    //            asgnmt.TeacherId = logonUser.Id;
    //            new AssignmentDAL().SaveAssignments(asgnmt);
    //            BindGrid(new AssignmentDAL().RetrieveAssignment(logonUser.Id, 0, 0, string.Empty));


    //        }
    //        else
    //        {
    //            lblErrorMsg.Text = string.Format("File upload is required.");
    //            lblErrorMsg.Visible = true;
    //            return;
    //        }

    //        //Response.Redirect("~/rescList.aspx");



    //    }
    //    catch (Exception ex)
    //    {

    //        throw ex;
    //    }
    //}

    protected bool checkIfMarked(object obj)
    {
        Int32 markId = 0;
        Int32.TryParse(obj.ToString(), out markId);
        PASSIS.DAO.Utility.MarkStatus markStatus = (PASSIS.DAO.Utility.MarkStatus)markId;
        return markStatus == PASSIS.DAO.Utility.MarkStatus.Marked ? true : false;
    }
    protected void gdvList_RowEditing1(object sender, GridViewEditEventArgs e)
    {
        gdvList.EditIndex = e.NewEditIndex;
        BindGrid();
    }
    protected void gdvList_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        System.Web.UI.WebControls.Label lblAssignmentId = (System.Web.UI.WebControls.Label)gdvList.Rows[e.RowIndex].FindControl("lblAssignmentId");
        System.Web.UI.WebControls.Label lblStudentUserId = (System.Web.UI.WebControls.Label)gdvList.Rows[e.RowIndex].FindControl("lblStudentUserId");

        System.Web.UI.WebControls.FileUpload fileUpload = gdvList.Rows[e.RowIndex].FindControl("FileUpload1") as System.Web.UI.WebControls.FileUpload;
        if (fileUpload.HasFile)
        {
            fileUpload.SaveAs(System.IO.Path.Combine(Server.MapPath("Images"), fileUpload.FileName));
            string originalFileName = System.IO.Path.GetFileName(fileUpload.PostedFile.FileName);
            string modifiedFileName = string.Format("{0}{1}{2}", "Assessed", DateTime.Now.ToString("MMddHmmss"), originalFileName);
            string fileExtension = System.IO.Path.GetExtension(fileUpload.PostedFile.FileName);
            string fileLocation = Server.MapPath("~/docs/AssessedAssignment/") + modifiedFileName;
            if (!(fileExtension.ToLower().Contains(".doc")))
            {
                lblErrorMsg.Text = string.Format("Upload not succesful. The file format is not supported!");
                lblErrorMsg.Visible = true;
                return;
            }
            fileUpload.SaveAs(fileLocation);

            //Check this code for modification....................................................

            //Int64 assignmentId = Convert.ToInt64(lblAssignmentId.Text);
            //PASSIS.DAO.AssignmentsGraded agrd = new PASSIS.DAO.AssignmentsGraded();
            //agrd.SubmittedAssignmentId = assignmentId;
            //agrd.DateUploaded = DateTime.Now;
            //agrd.GradedAssignmentFileName = modifiedFileName;
            //AssignmentDAL asDAL = new AssignmentDAL();
            //asDAL.SaveGradedAssignments(agrd);

            ////update submitted assignment 
            //Int64 studentId = Convert.ToInt64(lblStudentUserId.Text);
            //PASSIS.DAO.AssignmentSubmitted submitdAsgn = asDAL.GetSubmittedAssignment(assignmentId, studentId);
            //submitdAsgn.MarkStatus = (Int32)PASSIS.DAO.Utility.MarkStatus.Marked;
            //asDAL.UpdateSubmittedAssignment(submitdAsgn);
            //// send mail to parent
            //PASSIS.DAO.ParentStudentMap psm = new ParentStudentMapDAL().RetrieveStudentParent(studentId);
            //if (psm != null)
            //{
            //    Extension.Mailer(psm.ParentDetail.FathersEmail, psm.ParentDetail.MothersEmail, "ASSIGNMENT", getMailBody(psm.ParentDetail.FathersName, studentId));
            //}
        }

        gdvList.EditIndex = -1;
        BindGrid();
    }
    public string getMailBody(String parentName, Int64 studentId)
    {
        PASSIS.DAO.User std = new PASSIS.DAO.UsersDAL().RetrieveUser(studentId);
        StringBuilder MailBody = new StringBuilder();
        MailBody.Append("<html><head></head><body> \n");
        MailBody.AppendFormat("<span style=\"font-size: 11 px; font-family:Verdana,Helvetica, sans-serif\">Dear {0},<br><br>The assignment of your child, <b> {1} </b>  with admission number {2}  has been marked! </span><br><br> \n", parentName.Replace(",", ""), std.FullName, std.AdmissionNumber);

        MailBody.AppendFormat("<span style=\"font-size: 11 px; font-family:Verdana,Helvetica, sans-serif\"> <br><br>log on to the PASSIS portal to view.<br><br> Yours Faithfully, <br> Learning Space.</span> \n");

        MailBody.Append("</body></html>");
        return MailBody.ToString();
    }
    protected void gdvList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gdvList.EditIndex = -1;
        BindGrid();
    }
    protected void gdvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvList.PageIndex = e.NewPageIndex;
        BindGrid();
    }
}