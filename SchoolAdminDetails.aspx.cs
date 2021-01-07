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
using RJS.Web.WebControl;

public partial class SchoolAdminDetails : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();

    protected string referrerUrl_VS
    {
        get
        {
            //if (ViewState["::referrerUrl_VS::"] != null)
            return ViewState["::referrerUrl_VS::"].ToString();
        }
        set
        {
            ViewState["::referrerUrl_VS::"] = value;
        }
    }

    protected string getUrl()
    {
        return referrerUrl_VS;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        string url = ViewState["cancelEditUrl"].ToString();
        if (!string.IsNullOrEmpty(url))
        {
            Response.Redirect(url.Replace("edit", "view"));
        }
        else { Response.Redirect("~/UpdateUsers.aspx"); }
    }
    protected Int64 userId
    {
        get
        {
            if (Session["userId"] == null)
                return 0L;
            else
                return Convert.ToInt64(Session["userId"]);
        }
        set
        {
            Session["userId"] = value;
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {

        try
        {
            Label lblError = (Label)fvwUser.FindControl("lblError");
            //DropDownList ddlStatus1 = (DropDownList)fvwUser.FindControl("ddlStatus1");
            //if (Convert.ToInt64(ddlStatus1.SelectedValue) < 1)
            //{
            //    lblError.Text = "Select the Status";
            //    lblError.ForeColor = System.Drawing.Color.Red;
            //    lblError.Visible = true;
            //    return;
            //}

            PASSIS.LIB.User usr = new UsersLIB().RetrieveUser(userId);
            DropDownList ddlCampus = (DropDownList)fvwUser.FindControl("ddlCampus");
            TextBox txtFirstName = (TextBox)fvwUser.FindControl("txtFirstName");
            TextBox txtLastName = (TextBox)fvwUser.FindControl("txtLastName");
            TextBox txtEmailAddress = (TextBox)fvwUser.FindControl("txtEmailAddress");
            DropDownList ddlGender = (DropDownList)fvwUser.FindControl("ddlGender");
            TextBox txtPhoneNumber = (TextBox)fvwUser.FindControl("txtPhoneNumber");
            TextBox txtDateOfBirth = (TextBox)fvwUser.FindControl("txtDateOfBirth");
            TextBox txtUserName = (TextBox)fvwUser.FindControl("txtUserName");

            string username, emailaddress;
            username = txtUserName.Text.Trim();
            emailaddress = txtEmailAddress.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                lblError.Text = string.Format("Username field cannot be null");
                lblError.ForeColor = System.Drawing.Color.Red;
                lblError.Visible = true;
                return;
            }
            if (string.IsNullOrEmpty(emailaddress))
            {
                lblError.Text = string.Format("Email Address field cannot be null");
                lblError.ForeColor = System.Drawing.Color.Red;
                lblError.Visible = true;
                return;
            }
            if (new UsersDAL().RetrieveUserByMailAddress(emailaddress, userId) != null)
            {
                lblError.Text = string.Format("Email address already exist.");
                lblError.ForeColor = System.Drawing.Color.Red;
                lblError.Visible = true;
                return;
            }

            if (new UsersDAL().usernameExist(username, userId, (Int64)PASSIS.DAO.Utility.roles.schooladmin))
            {
                lblError.Text = string.Format("Username already exist. Try a more unique username.");
                lblError.ForeColor = System.Drawing.Color.Red;
                lblError.Visible = true;
                return;
            }

            //RJS.Web.WebControl. PopCalendar popCalendar = 
            //PopCalendar myDateToCal = (PopCalendar)fvwUser.FindControl("myDateToCal");
            usr.SchoolCampusId = Convert.ToInt64(ddlCampus.SelectedValue);
            usr.FirstName = txtFirstName.Text.Trim();
            usr.LastName = txtLastName.Text.Trim();
            usr.EmailAddress = txtEmailAddress.Text.Trim();
            usr.Gender = Convert.ToInt32(ddlGender.SelectedValue);
            usr.PhoneNumber = txtPhoneNumber.Text.Trim();
            usr.DateOfBirth = Convert.ToDateTime(txtDateOfBirth.Text);
            usr.Username = txtUserName.Text.Trim();

            //if (ddlStatus1.SelectedValue == "1")
            //{
            //    usr.UserStatus = 1;
            //}
            //else if (ddlStatus1.SelectedValue == "2")
            //{
            //    usr.UserStatus = 2;
            //}
            new UsersLIB().UpdateUser(usr);

            string url = ViewState["cancelEditUrl"].ToString();
            if (!string.IsNullOrEmpty(url))
            {
                Response.Redirect(url.Replace("edit", "view"));
            }
            else { Response.Redirect("~/UpdateUsers.aspx"); }

        }
        catch (Exception ex)
        {

        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //ParentStudentMap psm = new ParentStudentMap();
        //psm.
        //PASSIS.DAO.User 
        //Request.QueryString["mode"];
        if (!IsPostBack)
        {
            referrerUrl_VS = Request.UrlReferrer.ToString();
            //Int64 userId = Convert.ToInt64(Request.QueryString["Id"].ToString());
            string mode = Request.QueryString["mode"].ToString();
            SetFormViewMode(fvwUser, mode);
            //fvwUser.DefaultMode = FormViewMode.ReadOnly;
            if (fvwUser.CurrentMode == FormViewMode.Edit)
            {
                userId = Convert.ToInt64(Request.QueryString["id"]);
                ViewState["cancelEditUrl"] = referrerUrl_VS;
                fvwUser.DataSource = new UsersLIB().RetrieveSingleUserList(userId);
                fvwUser.DataBind();
            }
            if (fvwUser.CurrentMode == FormViewMode.ReadOnly)
            {
                userId = Convert.ToInt64(Request.QueryString["id"]);
                fvwUser.DataSource = new UsersLIB().RetrieveSingleUserList(userId);
                fvwUser.DataBind();

            }

        }
    }

    public enum AdminStatus
    {
        Active = 1,
        Inactive = 2,
    }

    public long? getUserSchoolId() //Retrieve Users SchoolId
    {
        PASSISLIBDataContext dataContext = new PASSISLIBDataContext();
        PASSIS.LIB.User user = new PASSIS.LIB.User();
        return user.SchoolId = dataContext.Users.First(a => a.Id == userId).SchoolId;
    }

    protected IList<PASSIS.LIB.SchoolCampus> SchoolCampusList()
    {
        long SchoolId = (long)getUserSchoolId();
        return new AcademicSessionLIB().GetAllSchoolCampus(SchoolId);
    }
    protected void SetFormViewMode(FormView frmView, string mode)
    {
        switch (PASSIS.LIB.Utility.Utili.getFormMode(mode))
        {
            case PASSIS.LIB.Utility.FormMode.Edit:
                frmView.ChangeMode(FormViewMode.Edit);
                break;
            case PASSIS.LIB.Utility.FormMode.Insert:
                frmView.ChangeMode(FormViewMode.Insert);
                break;
            case PASSIS.LIB.Utility.FormMode.View:
                frmView.ChangeMode(FormViewMode.ReadOnly);
                break;
        }
    }
    //protected void fvwUser_PreRender(object sender, EventArgs e)
    //{
    //    if (fvwUser.CurrentMode == FormViewMode.Edit)
    //    {
    //        DropDownList ddlLearningSupport = (DropDownList)fvwUser.FindControl("ddlLearningSupport");
    //        ddlLearningSupport.SelectedValue = new UsersDAL().RetrieveUser(userId).IsLearningSupport.ToString();
    //    }
    //}

    //    public string StudentStatus(long StudentId) 
    //    {
    //        string status = "";
    //        PASSISLIBDataContext context = new PASSISLIBDataContext();
    //        PASSIS.LIB.GradeStudent student = context.GradeStudents.FirstOrDefault(x => x.StudentId == StudentId);
    //        if (student.StudentStatusId != null)
    //        {
    //            long? statusId = student.StudentStatusId;
    //            StudentStatus statusObj = context.StudentStatus.FirstOrDefault(x => x.Id == statusId);
    //            status = statusObj.Status.ToString();
    //        }
    //        return status;
    //    }
    //protected void ddlStatus1_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    DropDownList ddlStatus1 = (DropDownList)fvwUser.FindControl("ddlStatus1");
    //    DropDownList ddlStatus2 = (DropDownList)fvwUser.FindControl("ddlStatus2");
    //    ddlStatus2.Items.Clear();
    //    if (ddlStatus1.SelectedValue == "1")
    //    {
    //        var activeList = from s in context.StudentStatus where s.Category == 1 select s;
    //        ddlStatus2.DataSource = activeList.ToList<StudentStatus>();
    //        ddlStatus2.DataTextField = "Status";
    //        ddlStatus2.DataValueField = "Id";
    //        ddlStatus2.DataBind();
    //        ddlStatus2.Items.Insert(0, new ListItem("--Select--", "0"));
    //    }
    //    else if (ddlStatus1.SelectedValue == "2")
    //    {
    //        var activeList = from s in context.StudentStatus where s.Category == 2 select s;
    //        ddlStatus2.DataSource = activeList.ToList<StudentStatus>();
    //        ddlStatus2.DataTextField = "Status";
    //        ddlStatus2.DataValueField = "Id";
    //        ddlStatus2.DataBind();
    //        ddlStatus2.Items.Insert(0, new ListItem("--Select--", "0"));
    //    }
    //}
    //}
}