using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using System.IO;
using System.Data.SqlClient;
using PASSIS.LIB;

public partial class RegisterSuccessfulStudent : PASSIS.LIB.Utility.BasePage
{
    #region properties
    public bool StepNextButtonVisibility
    {
        get
        {
            if (ViewState[":::Step_Next_Button_Visibility:::"] == null)
            {
                return true;
            }
            return Convert.ToBoolean(ViewState[":::Step_Next_Button_Visibility:::"]);
        }
        set
        {
            ViewState[":::Step_Next_Button_Visibility:::"] = value;
        }
    }

    /// <summary>
    /// Gets/Sets the visibility of the previous button 
    /// </summary>
    public bool StepPreviousButtonVisibility
    {
        get
        {
            if (ViewState[":::Step_Previous_Button_Visibility:::"] == null)
            {
                return true;
            }
            return Convert.ToBoolean(ViewState[":::Step_Previous_Button_Visibility:::"]);
        }
        set
        {
            ViewState[":::Step_Previous_Button_Visibility:::"] = value;
        }
    }

    /// <summary>
    /// Gets/Sets the visibility of the complete button 
    /// </summary>
    public bool FinishCompleteButtonVisibility
    {
        get
        {
            if (ViewState[":::Finish_Complete_Button_Visibility:::"] == null)
            {
                return true;
            }
            return Convert.ToBoolean(ViewState[":::Finish_Complete_Button_Visibility:::"]);
        }
        set
        {
            ViewState[":::Finish_Complete_Button_Visibility:::"] = value;
        }
    }

    /// <summary>
    /// Gets/Sets the visibility of the finish previous button 
    /// </summary>
    public bool FinishPreviousButtonVisibility
    {
        get
        {
            if (ViewState[":::Finish_Previous_Button_Visibility:::"] == null)
            {
                return true;
            }
            return Convert.ToBoolean(ViewState[":::Finish_Previous_Button_Visibility:::"]);
        }
        set
        {
            ViewState[":::Finish_Previous_Button_Visibility:::"] = value;
        }
    }



    /// <summary>
    /// Gets/Sets the visibility of the finish previous button 
    /// </summary>
    public string FinishPreviousButtonText
    {
        get
        {
            if (ViewState[":::Finish_Previous_Button_Text:::"] == null)
            {
                return "Previous";
            }
            return Convert.ToString(ViewState[":::Finish_Previous_Button_Text:::"]);
        }
        set
        {
            ViewState[":::Finish_Previous_Button_Text:::"] = value;
        }
    }

    /// <summary>
    /// Gets/Sets the visibility of the finish complete button 
    /// </summary>
    public string FinishCompleteButtonText
    {
        get
        {
            if (ViewState[":::Finish_Complete_Button_Text:::"] == null)
            {
                return "Pay";
            }
            return Convert.ToString(ViewState[":::Finish_Complete_Button_Text:::"]);
        }
        set
        {
            ViewState[":::Finish_Complete_Button_Text:::"] = value;
        }
    }
    public string fileLocation
    {
        get
        {
            if (ViewState[":::fileLocation:::"] == null)
            {
                return "";
            }
            return Convert.ToString(ViewState[":::fileLocation:::"]);
        }
        set
        {
            ViewState[":::fileLocation:::"] = value;
        }
    }
    public decimal AmountToPay_Vs
    {
        get
        {
            if (ViewState[":::AmountToPay:::"] == null)
            {
                return 0;
            }
            return Convert.ToDecimal(ViewState[":::AmountToPay:::"]);
        }
        set
        {
            ViewState[":::AmountToPay:::"] = value;
        }
    }
    #endregion

    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        clsMyDB mdb = new clsMyDB();
        mdb.connct();
        string query = "SELECT CurriculumId FROM Schools WHERE id=" + logonUser.SchoolId;
        SqlDataReader reader = mdb.fetch(query);
        int CurriculumId = 0;
        while (reader.Read())
        {
            int.TryParse(reader[0].ToString(), out CurriculumId);
        }
        reader.Close();
        mdb.closeConnct();
        if (!IsPostBack)
        {
            ddlCampus.DataSource = new AcademicSessionDAL().RetrieveCampusInSchool((long)logonUser.SchoolId);
            ddlCampus.DataBind();
            //var _getListofStudentAdmitted = context.AdmissionApplicationLists.Where(getList => getList.ProcessingLevel == 5 && getList.SchoolId == logonUser.SchoolId && getList.AdmissionApprovalStatus==1 && getList.HasApplicantRegistered == false);
            var _getListofStudentAdmitted = from getList in context.AdmissionApplicationLists
                                            where getList.ProcessingLevel == 5
                                                && getList.SchoolId == (long)logonUser.SchoolId && getList.AdmissionApprovalStatus == 1
                                                && getList.HasApplicantRegistered == null
                                            select getList;
            ddlApplicationID.DataSource = _getListofStudentAdmitted.ToList();
            //ddlApplicationID.DataBind();
            //ddlApplicationID.DataTextField = "ApplicantId";
            ddlApplicationID.DataBind();
            ddlApplicationID.Items.Insert(0, new ListItem("-- Select Applicant ID --", "0"));
            ddlApplicationID.SelectedIndex = 0;
            //long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
            //long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
            //ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
            //ddlYear.DataBind();
            Util.BindToEnum(typeof(LearningSupport), ddlLearningSupport);
            ddlLearningSupport.SelectedValue = "0";
        }
    }

    protected IList<PASSIS.LIB.Class_Grade> getYearDataSource()
    {
        IList<PASSIS.LIB.Class_Grade> years = new List<PASSIS.LIB.Class_Grade>();
        years = new PASSIS.LIB.SchoolLIB().getAllClass_Grade();
        return years;
    }
    #region WizardEvents
    protected void FinishPreviousButton_Click(object sender, EventArgs e)
    {
        //if ((sender as Button).Text == "New Payment")
        //{
        //    //Response.Redirect("");
        //}
    }
    protected void wzdTaxPayment_ActiveStepChanged(object sender, EventArgs e)
    {
        switch (wzdTaxPayment.ActiveStepIndex)
        {
            case 0:
                break;
            case 1:
                StepPreviousButtonVisibility = true;

                // wzdTaxPayment.DataBind();
                break;
            case 2:
                StepPreviousButtonVisibility = true;
                break;
        }
    }
    protected string CurrencyCodeSelector(string selectedindex)
    {
        return selectedindex == "1" ? " NGN " : " USD ";
    }
    public string modifiedFileName
    {
        get
        {
            if (ViewState[":::modifiedFileName:::"] == null)
            {
                return "";
            }
            return Convert.ToString(ViewState[":::modifiedFileName:::"]);
        }
        set
        {
            ViewState[":::modifiedFileName:::"] = value;
        }
    }
    protected void wzdTaxPayment_NextButtonClick(object sender, WizardNavigationEventArgs e)
    {

        switch (e.CurrentStepIndex)
        {
            case 0:

                //check if parents exist 
                if (!string.IsNullOrEmpty(txtFathersMobileToBeChecked.Text))
                {
                    PASSIS.LIB.ParentDetail pd = new PASSIS.LIB.UsersLIB().RetrieveParentDetail(txtFathersMobileToBeChecked.Text.Trim());
                    if (pd != null)
                    {
                        //display a message that 
                        txtFathersName.Text = pd.FathersName; txtFathersName.Enabled = false;
                        txtFathersNationality.Text = pd.FathersNationality; txtFathersNationality.Enabled = false;
                        txtFathersOccupation.Text = pd.FathersOccupation; txtFathersOccupation.Enabled = false;
                        txtFathersOfficeAdrress.Text = pd.FathersWorkAddress; txtFathersOfficeAdrress.Enabled = false;
                        txtFathersEmailAddress.Text = pd.FathersEmail; txtFathersEmailAddress.Enabled = false;
                        txtFathersTelephone.Text = pd.FathersPhoneNumber; txtFathersTelephone.Enabled = false;

                        txtMothersName.Text = pd.MothersName; txtMothersName.Enabled = false;
                        txtMotherEmailAddress.Text = pd.MothersEmail; txtMotherEmailAddress.Enabled = false;
                        txtMotherNationality.Text = pd.MothersNationality; txtMotherNationality.Enabled = false;
                        txtMotherOccupation.Text = pd.MothersOccupation; txtMotherOccupation.Enabled = false;
                        txtMothersOfficeAddress.Text = pd.MothersWorkAddress; txtMothersOfficeAddress.Enabled = false;
                        txtMotherTelephone.Text = pd.MothersPhoneNumber; txtMotherTelephone.Enabled = false;
                        txtGuardianEmailAddress.Text = pd.GuardianEmail; txtGuardianEmailAddress.Enabled = false;
                        txtGuardianName.Text = pd.GuardianDetails; txtGuardianName.Enabled = false;
                        txtGuardianRelationshipWtStd.Text = pd.GuardianRelationship; txtGuardianRelationshipWtStd.Enabled = false;
                        txtGuardianTelephone.Text = pd.GuardianPhoneNumber; txtGuardianTelephone.Enabled = false;

                        ViewState["parentId"] = pd.Id;


                        //lblMainErrorMsg.Text = string.Format("Admission number already exist!");
                        //lblMainErrorMsg.Visible = true;

                    }
                }
                break;

            case 1:
                //if (Page.IsValid)
                //{


                //}
                //else
                //{
                //    e.Cancel = true;
                //}
                break;
        }
    }
    protected void wzdTaxPayment_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
    {
        switch (e.CurrentStepIndex)
        {
            case 0:

                break;
            case 1:

                break;
        }
    }
    protected void wzdTaxPayment_FinishButtonClick(object sender, WizardNavigationEventArgs e)
    {
        /*approve registration 
         assign a matric number 
         * assign a class grade 
         * 
         */

        if (chkConfirmUpload.Checked)
        {
            if (documentUpload.HasFile)
            {

                if (documentUpload.PostedFile.ContentType != "image/jpeg")
                {
                    lblErrorPassportMsg.Text = string.Format("Upload Status: only jpg files are accepted.");
                    lblErrorPassportMsg.Visible = true;
                    e.Cancel = true;
                    //return;
                }
                if (documentUpload.PostedFile.ContentLength > 30720)
                {
                    lblErrorPassportMsg.Text = string.Format("Upload Status: The file has to be less than 30 kb!");
                    lblErrorPassportMsg.Visible = true;
                    e.Cancel = true;
                }
                string originalFileName = Path.GetFileName(documentUpload.PostedFile.FileName);
                modifiedFileName = string.Format("{0}{1}", DateTime.Now.ToString("MMddHmmss"), originalFileName);
                string fileExtension = Path.GetExtension(documentUpload.PostedFile.FileName);
                string fileWithoutExt = Path.GetFileNameWithoutExtension(documentUpload.PostedFile.FileName);
                //fileLocation = Server.MapPath("~/Passports/") + originalFileName;
                fileLocation = Server.MapPath("~/Passports/") + modifiedFileName;
                if (string.IsNullOrEmpty(fileLocation))
                {
                    lblErrorPassportMsg.Text = string.Format("Retry passport upload! ");
                    lblErrorPassportMsg.Visible = true;
                    e.Cancel = true;
                }
            }
            else
            {
                lblErrorPassportMsg.Text = string.Format("Upload Status: A passport upload is required!");
                lblErrorPassportMsg.Visible = true;
                e.Cancel = true;
            }
        }
        PASSIS.LIB.User ns = new PASSIS.LIB.User();
        PASSIS.LIB.ParentDetail pd = new PASSIS.LIB.ParentDetail();
        ns.BusRoute = "";
        ns.City = txtCity.Text.Trim();
        ns.Country = txtCountry.Text.Trim();
        ns.DateOfBirth = Convert.ToDateTime(lblDateOfBirth.Text);
        if (IblGender.Text == "Male")
        {
            ns.Gender = Convert.ToInt32(1);
        }
        if (IblGender.Text == "Female")
        {
            ns.Gender = Convert.ToInt32(2);

        }
        //ns.HomeRoomTutor = "";
        ns.LastLoginDate = DateTime.Now;
        ns.LastName = IblLastName.Text.Trim();
        ns.LastSchoolAttended = lblSchoolName.Text;
        try
        {
            Int64 schoolId = (Int64)logonUser.SchoolId;
            SchoolLIB schLib = new SchoolLIB();
            var schoolCode = schLib.RetrieveSchool(schoolId).Code;
            //ns.AdmissionNumber = ns.Username = PASSIS.LIB.Utility.Utili.GenerateAdmissionNumber(logonUser.School.Code, Convert.ToInt64(logonUser.SchoolId));
            ns.AdmissionNumber = ns.Username = PASSIS.LIB.Utility.Utili.GenerateAdmissionNumber(schoolCode, Convert.ToInt64(logonUser.SchoolId));
        }
        catch { }
        ns.MiddleName = lblMiddleName.Text; //ns.EmailAddress = txt;
        ns.FirstName = lblFirstName.Text;
        if (ViewState["parentId"] == null)//no parent was found
        {
            pd.FathersEmail = ns.FathersEmail = txtFathersEmailAddress.Text;
            pd.FathersName = ns.FathersName = txtFathersName.Text;
            pd.FathersNationality = ns.FathersNationality = txtFathersNationality.Text;
            pd.FathersPhoneNumber = ns.FathersPhoneNumber = txtFathersTelephone.Text;
            pd.FathersWorkAddress = ns.FathersWorkAddress = txtFathersOfficeAdrress.Text;

            pd.GuardianDetails = ns.GuardianDetails = txtGuardianName.Text.Trim();
            pd.GuardianEmail = ns.GuardianEmail = txtGuardianEmailAddress.Text.Trim();
            pd.GuardianPhoneNumber = ns.GuardianPhoneNumber = txtGuardianTelephone.Text.Trim();
            pd.GuardianRelationship = ns.GuardianRelationship = txtGuardianRelationshipWtStd.Text.Trim();

            pd.MothersEmail = ns.MothersEmail = txtMotherEmailAddress.Text;
            pd.MothersName = ns.MothersName = txtMothersName.Text;
            pd.MothersNationality = ns.MothersNationality = txtMotherNationality.Text;
            pd.MothersOtherName = ns.MothersOtherName = "";
            pd.MothersPhoneNumber = ns.MothersPhoneNumber = txtMotherTelephone.Text;
            pd.MothersWorkAddress = ns.MothersWorkAddress = txtMothersOfficeAddress.Text;
            new PASSIS.LIB.UsersLIB().SaveParentDetail(pd);
        }
        else
        {
            ns.FathersEmail = txtFathersEmailAddress.Text;
            ns.FathersName = txtFathersName.Text;
            ns.FathersNationality = txtFathersNationality.Text;
            ns.FathersPhoneNumber = txtFathersTelephone.Text;
            ns.FathersWorkAddress = txtFathersOfficeAdrress.Text;

            ns.GuardianDetails = txtGuardianName.Text.Trim();
            ns.GuardianEmail = txtGuardianEmailAddress.Text.Trim();
            ns.GuardianPhoneNumber = txtGuardianTelephone.Text.Trim();
            ns.GuardianRelationship = txtGuardianRelationshipWtStd.Text.Trim();

            ns.MothersEmail = txtMotherEmailAddress.Text;
            ns.MothersName = txtMothersName.Text;
            ns.MothersNationality = txtMotherNationality.Text;
            ns.MothersOtherName = "";
            ns.MothersPhoneNumber = txtMotherTelephone.Text;
            ns.MothersWorkAddress = txtMothersOfficeAddress.Text;
        }
        ns.Password = Util.getDefaultPassword();
        ns.SchoolCampusId = Convert.ToInt64(ddlCampus.SelectedValue);
        ns.SchoolHouse = "";
        ns.SchoolHouseParentName = "";
        ns.SchoolId = (long)logonUser.SchoolId;
        ns.Siblings = txtSiblings.Text.Trim();
        ns.SpecialNoteAlert = "";
        ns.State = "";
        ns.StreetAddress = lblStreetAddress.Text.Trim();
        ns.StudentFullName = lblFirstName.Text + " " + lblMiddleName.Text + " " + IblLastName.Text;
        ns.PassportFileName = modifiedFileName;
        ns.AccomodationType = ddlAccomodation.SelectedValue.Trim();
        ns.IsLearningSupport = Convert.ToInt32(ddlLearningSupport.SelectedValue);
        ns.DateCreated = DateTime.Now;
        ns.UserStatus = (Int32)UserStatus.Active;
        ns.CreatedBy = logonUser.Id;
        new PASSIS.LIB.UsersLIB().SaveUser(ns);
        if (chkConfirmUpload.Checked)
        {
            try
            {
                documentUpload.SaveAs(fileLocation);
            }
            catch (Exception ex) { }
        }
        //map student to parent, create parent as user if he doesv nt 
        if (ViewState["parentId"] == null)//no parent was found
        {
            PASSIS.LIB.User parentUser = new PASSIS.LIB.User();
            parentUser.Username = pd.FathersPhoneNumber;
            parentUser.LastLoginDate = DateTime.Now;
            parentUser.DateCreated = parentUser.LastLoginDate;
            parentUser.Password = Util.getDefaultPassword();
            parentUser.Gender = 1;
            parentUser.DateOfBirth = DateTime.Now;
            parentUser.SchoolCampusId = ns.SchoolCampusId;
            parentUser.SchoolId = ns.SchoolId;
            parentUser.FirstName = pd.FathersName;
            parentUser.EmailAddress = ns.FathersEmail;
            new PASSIS.LIB.UsersLIB().SaveUser(parentUser);

            PASSIS.LIB.UserRole pUser = new PASSIS.LIB.UserRole();
            pUser.RoleId = (Int64)roles.parent;
            pUser.UserId = parentUser.Id;
            new PASSIS.LIB.UsersLIB().SaveUserRole(pUser);
            PASSIS.LIB.ParentStudentMap psm = new PASSIS.LIB.ParentStudentMap();
            psm.StudentId = ns.Id;
            psm.ParentId = pd.Id;
            psm.ParentUserId = parentUser.Id;
            new PASSIS.LIB.ParentStudentMapLIB().SaveParentStudentMap(psm);
        }
        else
        { //parent already exist 
            PASSIS.LIB.ParentStudentMap psm = new PASSIS.LIB.ParentStudentMap();
            psm.StudentId = ns.Id;
            psm.ParentId = Convert.ToInt64(ViewState["parentId"]);
            psm.ParentUserId = new ParentStudentMapDAL().RetrieveParentStudentMap((long)psm.ParentId).ParentUserId;
            new PASSIS.LIB.ParentStudentMapLIB().SaveParentStudentMap(psm);
        }
        //redirect tto a new page
        PASSIS.LIB.UserRole usrRol = new PASSIS.LIB.UserRole();
        usrRol.UserId = ns.Id;
        usrRol.RoleId = (Int64)roles.student;
        new PASSIS.LIB.UsersLIB().SaveUserRole(usrRol);

        lblResponse.Text = "Created Successfully";
        lblResponse.ForeColor = System.Drawing.Color.Green;
        using (PASSISLIBDataContext dbcontext = new PASSISLIBDataContext())
        {
            AdmissionApplicationList admissionappuser = dbcontext.AdmissionApplicationLists.FirstOrDefault(s => s.ApplicantId == ddlApplicationID.SelectedValue);
            admissionappuser.HasApplicantRegistered = true;
            dbcontext.SubmitChanges();

        }

        //Response.Redirect("~/Students.aspx");

        //newStudent.DateOfBirth = ;

        //    }
        //    else
        //    {

        //        wzdTaxPayment.DataBind();
        //    }
        //}
    }
    #endregion
    protected void ddlApplicationID_SelectedIndexChanged(object sender, EventArgs e)
    {
        AdmissionApp getstddetail = context.AdmissionApps.FirstOrDefault(getstd => getstd.ApplicationID == ddlApplicationID.SelectedValue);

        if (getstddetail != null)
        {
            lblFirstName.Text = getstddetail.FirstName;
            IblLastName.Text = getstddetail.LastName;
            lblMiddleName.Text = getstddetail.MiddleName;
            IblGender.Text = getstddetail.Gender;
            lblDateOfBirth.Text = getstddetail.DateOfBirth;
            lblStreetAddress.Text = getstddetail.GuardianAddress;
            lblSchoolName.Text = getstddetail.LastSchoolAttended;
            lblLastSchoolAttendedYear.Text = getstddetail.LastSchoolAttendedYear;
            lblLastSchoolAttendedClass.Text = getstddetail.LastSchoolAttendedClass;
            lblMainErrorMsg.Visible = false;
        }
        else
        {
            lblMainErrorMsg.Text = "Please Select an Application ID";
            lblMainErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblMainErrorMsg.Visible = true;
            lblFirstName.Text = string.Empty;
            IblLastName.Text = string.Empty;
            lblMiddleName.Text = string.Empty;
            IblGender.Text = string.Empty;
            lblDateOfBirth.Text = string.Empty;
            lblSchoolName.Text = string.Empty;
            lblLastSchoolAttendedYear.Text = string.Empty;
            lblLastSchoolAttendedClass.Text = string.Empty;

        }

    }

}