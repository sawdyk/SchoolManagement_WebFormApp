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

public partial class StudentUploadAssignments : PASSIS.LIB.Utility.BasePage
{
  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Int64 Id = 0L; Int64.TryParse(Request.QueryString["id"], out Id);
            ViewState["Id"] = Id;
            if (Id > 0)
            {

                PASSIS.LIB.AssignmentLIB assLIB = new AssignmentLIB();

                Int64 userId = logonUser.Id;
                Int64 StudentGradeId = 0L;
                long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

                if (logonUserRole.Id == (long)roles.student)
                {
                    StudentGradeId = assLIB.RetrieveStudentGrade(userId, curSessionId).GradeId;
                    ViewState["StudentGradeId"] = StudentGradeId;
                    BindGrid(assLIB.RetrieveAssignmentDetails((long)ViewState["Id"], (long)ViewState["StudentGradeId"]));
                }
                //else { pnlStudentOnly.Visible = false; }


                BindMarkedStudentGrid(assLIB.RetrieveGradedAssignment(userId));

                if (StudentGradeId != 0)
                {
                    //BindGrid(new AssignmentDAL().RetrieveAssignment(0, StudentGradeId, 0, string.Empty));
                }
                else
                {

                    //lblErrorMsg.Text = "You have not been assigned to a class. Contact the administrator.";
                    //return;
                }
            }
        }

    }
    protected string getClassOrGroupName(object gradeId, object groupId)
    {
        if (gradeId == null)
        {
            return new PASSIS.DAO.GroupingDAL().RetrieveGrouping(Convert.ToInt64(groupId)).GroupName;
        }
        else
        {
            return new ClassGradeDAL().RetrieveGrade(Convert.ToInt64(gradeId)).GradeName;
        }
    }
    protected void BindGrid(IList<PASSIS.LIB.Assignment> assignments)
    {
        gdvList.DataSource = assignments;
        gdvList.DataBind();
    }
    protected void gdvListMarked_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
       
        //BindGrid();
    }

    protected void gdvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvList.PageIndex = e.NewPageIndex;

        PASSIS.LIB.AssignmentLIB assLIB = new AssignmentLIB();
        Int64 userId = logonUser.Id;
        Int64 StudentGradeId = 0L;
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

        StudentGradeId = assLIB.RetrieveStudentGrade(userId, curSessionId).GradeId;
        BindGrid(assLIB.RetrieveAssignmentDetails((long)ViewState["Id"], (long)ViewState["StudentGradeId"]));
    }
    protected void BindMarkedStudentGrid(IList<PASSIS.LIB.AssignmentsGraded> list)
    {
        //gdvListMarked.DataSource = list;
        //gdvListMarked.DataBind();
    }

    protected void displaySuccessMessage(string msg)
    {
        string jScript;
        jScript = "<script>alert (' " + msg + "')</script>";
        //ClientScript.RegisterClientScriptBlock("keyClientBlock", jScript);
    }
    protected void gdvListMarked_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "cmd")
            {
                string filename = e.CommandArgument.ToString();
                string path = MapPath("~/docs/AssessedAssignment/" + filename);
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
        catch (Exception ex)
        {
            IblMessage.Text = "File Not Found";
            IblMessage.ForeColor = System.Drawing.Color.Red;
            IblMessage.Visible = true;

        }
    }
    protected void gdvList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "cmd")
            {
                string filename = e.CommandArgument.ToString();
                string path = MapPath("~/docs/Assignment/" + filename);
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
        catch (Exception ex)
        {
            IblMessage.Text = "File Not Found";
            IblMessage.ForeColor = System.Drawing.Color.Red;
            IblMessage.Visible = true;
        }
    }

    protected void btnUpload_OnClick(object sender, EventArgs e)
    {
        //validate the value and save
        //impose restriction on file sixe
        PASSIS.DAO.AssignmentSubmitted asgnmt = new PASSIS.DAO.AssignmentSubmitted();
        try
        {
            if (!documentUpload.HasFile)
            {
                lblErrorMsg.Text = string.Format("Select file to upload.");
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            else
            {
                string checkSelectedAssignment = Convert.ToInt32(getSelectedAssignmentId()).ToString();

                if (checkSelectedAssignment.Equals("0"))
                {
                    lblErrorMsg.Text = string.Format("Select one from the list of assignments displayed below.");
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    lblErrorMsg.Visible = true;
                    return;
                }
                else
                {
                    PASSISLIBDataContext context = new PASSISLIBDataContext();
                    //var getDueDate = from getDate in context.Assignments where getDate.Id == Convert.ToInt32(checkSelectedAssignment) select getDate.DueDate;
                    PASSIS.LIB.Assignment objAssignment = context.Assignments.FirstOrDefault(x => x.Id == Convert.ToInt32(checkSelectedAssignment));

                    string inputDate = objAssignment.DueDate.ToString();
                    int currentDate = DateTime.Compare(Convert.ToDateTime(inputDate), DateTime.Now);
                    if (currentDate < 0)
                    {
                        lblErrorMsg.Text = "Due date passed, you can no longer submit this assignment!";
                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                        lblErrorMsg.Visible = true;
                    }
                    else
                    {
                        string originalFileName = Path.GetFileName(documentUpload.PostedFile.FileName);
                        string modifiedFileName = string.Format("{0}{1}", logonUser.Id, objAssignment.FileName);
                        string fileExtension = Path.GetExtension(documentUpload.PostedFile.FileName);
                        string fileLocation = Server.MapPath("~/docs/SubmittedAssignment/") + modifiedFileName;

                        //Check whether file extension is xls or xslx
                        //if (!(fileExtension.Contains(".doc") || fileExtension.Contains(".docx") || fileExtension.Contains(".xls") || fileExtension.Contains(".xlsx")))
                        if (!(fileExtension.Contains(".doc") || fileExtension.Contains(".pdf")))
                        {
                            lblErrorMsg.Text = string.Format("Upload not succesful. The file format is not supported!!!");
                            lblErrorMsg.Visible = true;
                            return;
                        }

                        var HasPreviouslySubmitted = from getAssignment in context.AssignmentSubmitteds where getAssignment.AssignmentId == getSelectedAssignmentId() && getAssignment.StudentId == logonUser.Id select getAssignment.Id;

                        if (HasPreviouslySubmitted.Count() > 0)
                        {
                            documentUpload.SaveAs(fileLocation);
                            PASSIS.LIB.AssignmentSubmitted objSubmitted = context.AssignmentSubmitteds.FirstOrDefault(x => x.Id == Convert.ToInt64(HasPreviouslySubmitted.FirstOrDefault().ToString()));
                            objSubmitted.DateSubmitted = DateTime.Now;
                            context.SubmitChanges();
                        }
                        else
                        {
                            documentUpload.SaveAs(fileLocation);
                            asgnmt.AssignmentId = getSelectedAssignmentId();
                            asgnmt.DateSubmitted = DateTime.Now;
                            asgnmt.MarkStatus = (int)PASSIS.DAO.Utility.MarkStatus.Submitted;
                            asgnmt.StudentFileName = modifiedFileName;
                            asgnmt.StudentId = logonUser.Id;
                            asgnmt.GradeId = new AssignmentDAL().RetrieveAssignmentById(asgnmt.AssignmentId).GradeId;
                            new AssignmentDAL().SaveSubmittedAssignments(asgnmt);
                        }
                        PASSIS.LIB.AssignmentLIB assLIB = new AssignmentLIB();
                        Int64 userId = logonUser.Id;
                        BindGrid(assLIB.RetrieveAssignmentDetails((long)ViewState["Id"], (long)ViewState["StudentGradeId"]));
                        lblErrorMsg.Text = string.Format("Assignment Successfully submitted.");
                        lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                        lblErrorMsg.Visible = true;
                    }
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected Int64 getSelectedAssignmentId()
    {
        Int64 selectedAssignmentId = 0L;
        int count = gdvList.Rows.Count;

        for (int i = 0; i < count; i++)
        {
            GridViewRow row = gdvList.Rows[i];
            CheckBox cb = row.FindControl("SubmitCheckbox") as CheckBox;

            if (cb != null && cb.Checked)
            {
                try
                {
                    Label lblId = row.FindControl("lblId") as Label;
                    selectedAssignmentId = Convert.ToInt64(lblId.Text.Trim());
                }
                catch { }

            }
        }
        return selectedAssignmentId;
    }
}