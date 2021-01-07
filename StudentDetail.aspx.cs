using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using RJS.Web.WebControl;
using System.IO;
using PASSIS.LIB;

public partial class StudentDetail : PASSIS.LIB.Utility.BasePage
{

    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected string modifiedFileName
    {
        get
        {

            return ViewState["::modifiedFileName::"].ToString();
        }
        set
        {
            ViewState["::modifiedFileName::"] = value;
        }
    }
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
        else { Response.Redirect("~/AdminViewStudents.aspx"); }
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
        string fileLocation = string.Empty;
        CheckBox chkConfirmUpload = (CheckBox)fvwUser.FindControl("chkConfirmUpload");
        FileUpload documentUpload = (FileUpload)fvwUser.FindControl("documentUpload");
        Label lblErrorPassportMsg = (Label)fvwUser.FindControl("lblErrorPassportMsg");
        if (chkConfirmUpload.Checked)
        {
            if (documentUpload.HasFile)
            {

                if (documentUpload.PostedFile.ContentType != "image/jpeg")
                {
                    lblErrorPassportMsg.Text = string.Format("Upload Status: only jpg files are accepted.");
                    lblErrorPassportMsg.Visible = true;
                    return;
                }
                if (documentUpload.PostedFile.ContentLength > 50720)
                {
                    lblErrorPassportMsg.Text = string.Format("Upload Status: The file has to be less than 30 kb!");
                    lblErrorPassportMsg.Visible = true;
                    return;
                }
                string originalFileName = Path.GetFileName(documentUpload.PostedFile.FileName).Replace(" ", "");
                modifiedFileName = string.Format("{0}{1}", DateTime.Now.ToString("MMddHmmss"), originalFileName);
                string fileExtension = Path.GetExtension(documentUpload.PostedFile.FileName);
                string fileWithoutExt = Path.GetFileNameWithoutExtension(documentUpload.PostedFile.FileName);
                //fileLocation = Server.MapPath("~/Passports/") + originalFileName;
                fileLocation = Server.MapPath("~/Passports/") + modifiedFileName;
                if (string.IsNullOrEmpty(fileLocation))
                {
                    lblErrorPassportMsg.Text = string.Format("Retry passport upload! ");
                    lblErrorPassportMsg.Visible = true;
                    return;
                }
            }
            else
            {
                lblErrorPassportMsg.Text = string.Format("Upload Status: A passport upload is required!");
                lblErrorPassportMsg.Visible = true;
                return;
            }

        }

        PASSIS.LIB.ParentStudentMap psm = new ParentStudentMapLIB().RetrieveStudentParent(userId);
        PASSIS.LIB.ParentDetail pd = new UsersLIB().RetrieveParent((long)psm.ParentId);
        PASSIS.LIB.User parentUSer = new UsersLIB().RetrieveUser((long)psm.ParentUserId);
        PASSIS.LIB.User usr = new UsersLIB().RetrieveUser(userId);
        // PASSIS.LIB.User newuser = new UsersLIB().RetrieveUser(userId);
        DropDownList ddlCampus = (DropDownList)fvwUser.FindControl("ddlCampus");
        TextBox txtFirstName = (TextBox)fvwUser.FindControl("txtFirstName");
        TextBox txtLastName = (TextBox)fvwUser.FindControl("txtLastName");
        TextBox txtMiddleName = (TextBox)fvwUser.FindControl("txtMiddleName");
        DropDownList ddlGender = (DropDownList)fvwUser.FindControl("ddlGender");
        DropDownList ddlAccomodationType = (DropDownList)fvwUser.FindControl("ddlAccomodationType");
        DropDownList ddlLearningSupport = (DropDownList)fvwUser.FindControl("ddlLearningSupport");
        TextBox txtDateOfBirth = (TextBox)fvwUser.FindControl("txtDateOfBirth");
        TextBox txtStreetAddress = (TextBox)fvwUser.FindControl("txtStreetAddress");
        TextBox txtCity = (TextBox)fvwUser.FindControl("txtCity");
        TextBox txtCountry = (TextBox)fvwUser.FindControl("txtCountry");
        TextBox txtSchoolHouse = (TextBox)fvwUser.FindControl("txtSchoolHouse");
        TextBox txtSchoolHouseParentName = (TextBox)fvwUser.FindControl("txtSchoolHouseParentName");
        TextBox txtBusRoute = (TextBox)fvwUser.FindControl("txtBusRoute");
        TextBox txtStatus = (TextBox)fvwUser.FindControl("txtStatus");
        TextBox txtAdmissionNumber = (TextBox)fvwUser.FindControl("txtAdmissionNumber");
        DropDownList ddlStatus1 = (DropDownList)fvwUser.FindControl("ddlStatus1");
        DropDownList ddlStatus2 = (DropDownList)fvwUser.FindControl("ddlStatus2");
        Label lblError = (Label)fvwUser.FindControl("lblError");

       
        if (Convert.ToInt64(ddlStatus1.SelectedValue) < 1 || Convert.ToInt64(ddlStatus2.SelectedValue) < 1)
        {
            lblError.Text = "Select the Status";
            lblError.ForeColor = System.Drawing.Color.Red;
            lblError.Visible = true;
            return;
        }
        //RJS.Web.WebControl. PopCalendar popCalendar = 
        //PopCalendar myDateToCal = (PopCalendar)fvwUser.FindControl("myDateToCal");
        usr.AccomodationType = ddlAccomodationType.SelectedValue;
        usr.AdmissionNumber = txtAdmissionNumber.Text;
        usr.FirstName = txtFirstName.Text.Trim();
        usr.LastName = txtLastName.Text.Trim();
        usr.MiddleName = txtMiddleName.Text.Trim();
        usr.Gender = Convert.ToInt32(ddlGender.SelectedValue);
        usr.StudentFullName = txtFirstName.Text + " " + txtMiddleName.Text + " " + txtLastName.Text;
        usr.DateOfBirth = Convert.ToDateTime(txtDateOfBirth.Text);
        usr.StreetAddress = txtStreetAddress.Text.Trim();
        usr.City = txtCity.Text.Trim();
        usr.Country = txtCountry.Text.Trim();
        //usr.SchoolHouse = txtSchoolHouse.Text.Trim();
        //usr.SchoolHouseParentName = txtSchoolHouseParentName.Text.Trim();
        usr.BusRoute = txtBusRoute.Text.Trim();
        if (ddlStatus1.SelectedValue == "1")
        {
            usr.StudentStatus = true;
        }
        else if (ddlStatus1.SelectedValue == "2")
        {
            usr.StudentStatus = false;
        }
        usr.IsLearningSupport = Convert.ToInt32(ddlLearningSupport.SelectedValue);
        usr.LastSchoolAttended = (fvwUser.FindControl("txtLastSchoolAttended") as TextBox).Text;
        usr.LastSchoolAttendedYear = (fvwUser.FindControl("txtLastSchoolAttendedYear") as TextBox).Text;
        usr.SchoolCampusId = Convert.ToInt64(ddlCampus.SelectedValue);
        usr.LastSchoolAttendedCityCountry = (fvwUser.FindControl("txtLastSchoolAttendedCityCountry") as TextBox).Text;
        usr.LastSchoolAttendedClass = (fvwUser.FindControl("txtLastSchoolAttendedClass") as TextBox).Text;
        if (chkConfirmUpload.Checked)
            usr.PassportFileName = modifiedFileName;

        PASSIS.LIB.GradeStudent stdObj = context.GradeStudents.FirstOrDefault(x => x.StudentId == usr.Id);
        stdObj.StudentStatusId = Convert.ToInt64(ddlStatus2.SelectedValue);
        if (ddlStatus1.SelectedValue == "1")
        {
            stdObj.HasGraduated = false;
        }
        else if (ddlStatus1.SelectedValue == "2")
        {
            stdObj.HasGraduated = true;
        }
        context.SubmitChanges();

        //paren'ts detail 

        pd.FathersName = (fvwUser.FindControl("txtFathersName") as TextBox).Text;
        parentUSer.FirstName = (fvwUser.FindControl("txtFathersName") as TextBox).Text;
        pd.FathersPhoneNumber = (fvwUser.FindControl("txtFatherPhoneNumber") as TextBox).Text;
        parentUSer.PhoneNumber = (fvwUser.FindControl("txtFatherPhoneNumber") as TextBox).Text;
        parentUSer.Username = (fvwUser.FindControl("txtFatherPhoneNumber") as TextBox).Text; // Awaits Modification
        pd.FathersNationality = (fvwUser.FindControl("txtFathersNationality") as TextBox).Text;
        parentUSer.FathersNationality = (fvwUser.FindControl("txtFathersNationality") as TextBox).Text;
        pd.FathersEmail = (fvwUser.FindControl("txtFathersEmail") as TextBox).Text;
        parentUSer.EmailAddress = (fvwUser.FindControl("txtFathersEmail") as TextBox).Text;

        pd.FathersWorkAddress = (fvwUser.FindControl("txtFathersWorkAddress") as TextBox).Text;
        pd.MothersName = (fvwUser.FindControl("txtMothersName") as TextBox).Text;
        pd.MothersOtherName = (fvwUser.FindControl("txtMothersOtherName") as TextBox).Text;
        pd.MothersPhoneNumber = (fvwUser.FindControl("txtMothersPhoneNumber") as TextBox).Text;
        pd.MothersNationality = (fvwUser.FindControl("txtMothersNationality") as TextBox).Text;

        pd.MothersEmail = (fvwUser.FindControl("txtMothersEmail") as TextBox).Text;
        pd.MothersWorkAddress = (fvwUser.FindControl("txtMothersWorkAddress") as TextBox).Text;
        pd.GuardianDetails = (fvwUser.FindControl("txtGuardianDetails") as TextBox).Text;
        pd.GuardianPhoneNumber = (fvwUser.FindControl("txtGuardianPhoneNumber") as TextBox).Text;

        pd.GuardianRelationship = (fvwUser.FindControl("txtGuardianRelationship") as TextBox).Text;
        pd.GuardianEmail = (fvwUser.FindControl("txtGuardianEmail") as TextBox).Text;
        pd.Siblings = (fvwUser.FindControl("txtSiblings") as TextBox).Text;
        UsersLIB userDal = new UsersLIB();
        userDal.UpdateUser(usr);
        userDal.UpdateUser(parentUSer);
        userDal.UpdateParentDetail(pd);

        try
        {
            documentUpload.SaveAs(fileLocation);
        }
        catch (Exception ex) { }
        string url = ViewState["cancelEditUrl"].ToString();
        if (!string.IsNullOrEmpty(url))
        {
            Response.Redirect(url.Replace("edit", "view"));
        }
        else { Response.Redirect("~/Students.aspx"); }




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
                fvwUser.DataSource = new ParentStudentMapLIB().GetParentStudentMap(userId);
                fvwUser.DataBind();

                DropDownList ddlLearningSupport = (DropDownList)fvwUser.FindControl("ddlLearningSupport");
                Util.BindToEnum(typeof(LearningSupport), ddlLearningSupport);


                //ddlLearningSupport.SelectedValue = 
                //new ParentStudentMap().ParentDetail.
            }
            if (fvwUser.CurrentMode == FormViewMode.ReadOnly)
            {

                //fvwUser.DataSource = new UsersDAL().RetrieveUserToList(Convert.ToInt64(Request.QueryString["id"]));
                //fvwUser.DataBind();
                userId = Convert.ToInt64(Request.QueryString["id"]);
                //ViewState["cancelEditUrl"] = referrerUrl_VS;

                fvwUser.DataSource = new ParentStudentMapLIB().GetParentStudentMap(userId);
                fvwUser.DataBind();

            }

        }
    }
    protected string getLearningSupportSelectedValue(object learningSupportObj)
    {
        return learningSupportObj.ToString();
    }

    
    protected IList<PASSIS.LIB.SchoolCampus> SchoolCampusList()
    {
        return new AcademicSessionLIB().GetAllSchoolCampus((long)logonUser.SchoolId);
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
    protected void fvwUser_PreRender(object sender, EventArgs e)
    {
        if (fvwUser.CurrentMode == FormViewMode.Edit)
        {
            DropDownList ddlLearningSupport = (DropDownList)fvwUser.FindControl("ddlLearningSupport");
            ddlLearningSupport.SelectedValue = new UsersLIB().RetrieveUser(userId).IsLearningSupport.ToString();
        }
    }

    public string StudentStatus(long StudentId)
    {
        string status = "";
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        PASSIS.LIB.GradeStudent student = context.GradeStudents.FirstOrDefault(x => x.StudentId == StudentId);
        if (student.StudentStatusId != null)
        {
            long? statusId = student.StudentStatusId;
            StudentStatus statusObj = context.StudentStatus.FirstOrDefault(x => x.Id == statusId);
            status = statusObj.Status.ToString();
        }
        return status;
    }
    protected void ddlStatus1_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlStatus1 = (DropDownList)fvwUser.FindControl("ddlStatus1");
        DropDownList ddlStatus2 = (DropDownList)fvwUser.FindControl("ddlStatus2");
        ddlStatus2.Items.Clear();
        if (ddlStatus1.SelectedValue == "1")
        {
            var activeList = from s in context.StudentStatus where s.Category == 1 select s;
            ddlStatus2.DataSource = activeList.ToList<StudentStatus>();
            ddlStatus2.DataTextField = "Status";
            ddlStatus2.DataValueField = "Id";
            ddlStatus2.DataBind();
            ddlStatus2.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        else if (ddlStatus1.SelectedValue == "2")
        {
            var activeList = from s in context.StudentStatus where s.Category == 2 select s;
            ddlStatus2.DataSource = activeList.ToList<StudentStatus>();
            ddlStatus2.DataTextField = "Status";
            ddlStatus2.DataValueField = "Id";
            ddlStatus2.DataBind();
            ddlStatus2.Items.Insert(0, new ListItem("--Select--", "0"));
        }
    }
}