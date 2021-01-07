using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;

public partial class AddComment : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindComment();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);
        long? curTermId = new AcademicSessionLIB().GetCurrentTermId(logonUser.SchoolId);
        PASSIS.LIB.AcademicSession academicSession = context.AcademicSessions.FirstOrDefault(x => x.AcademicSessionId == curSessionId && x.AcademicTermId == curTermId && x.SchoolId == logonUser.SchoolId);


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

        Int64 schoolId = (long)logonUser.SchoolId;
        Int64 campusId = logonUser.SchoolCampusId;
        Int64 uploadedById = logonUser.Id;
        

        if (txtComment.Text == "")
        {
            lblErrorMsg.Text = "Kindly enter the comment in the textbox below!";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }

        RpComment comm = new RpComment();
        comm.SchoolId = schoolId;
        comm.CampusId = campusId;
        comm.Comment = txtComment.Text;
        comm.UploadedById = uploadedById;
        comm.DateAdded = DateTime.Now;

        context.RpComments.InsertOnSubmit(comm);
        context.SubmitChanges();
        BindComment();
        txtComment.Text = "";
        lblErrorMsg.Text = "Comment Added Successfully";
        lblErrorMsg.ForeColor = System.Drawing.Color.Green;
        lblErrorMsg.Visible = true;
    }


    public void BindComment()
    {
        var getComment = from c in context.RpComments
                         where c.SchoolId == logonUser.SchoolId && c.CampusId == logonUser.SchoolCampusId
                         && c.UploadedById == logonUser.Id
                         select c;
        gvComment.DataSource = getComment.ToList();
        gvComment.DataBind();
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

    protected void gvComment_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            Label lblId = (Label)gvComment.Rows[e.RowIndex].FindControl("lblId");
            TextBox txtComment = (TextBox)gvComment.Rows[e.RowIndex].FindControl("txtComment");

            PASSISLIBDataContext context = new PASSISLIBDataContext();
            PASSIS.LIB.RpComment editComm = context.RpComments.FirstOrDefault(x => x.Id == Convert.ToInt64(lblId.Text));
            string comment = txtComment.Text;
            editComm.Comment = comment;

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


}