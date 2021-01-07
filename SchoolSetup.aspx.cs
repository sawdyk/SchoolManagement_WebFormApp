using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using PASSIS.LIB;
using PASSIS.LIB.Utility;
using System.Text;
using System.Net.Mail;

public partial class SchoolSetup : System.Web.UI.Page
{
    //public static PASSIS.DAO.Utility.BasePage bp = new PASSIS.DAO.Utility.BasePage();

    protected void Page_PreInit(object sender, EventArgs e)
    {
        //base.Page_PreInit(sender, e);
        //this.MasterPageFile = "~/Self_Setup/setup.master";
    }

    public SchoolSetup()
    {
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                ddlPositioninSchool.DataSource = new SchoolLIB().PositionInSchool();
                ddlPositioninSchool.DataBind();
                ddlPositioninSchool.Items.Insert(0, new ListItem("--Select--", "0", true));
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error occurred, kindly contact the administrator";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }


            MultiView1.SetActiveView(ViewSchoolRegistration);
            ddlSchoolType.DataSource = new SchoolLIB().SchoolType();
            ddlSchoolType.DataBind();
            ddlCurriculum.DataSource = new SchoolLIB().SchoolCurriculum();
            ddlCurriculum.DataBind();
            ddlCurriculum.Items.Insert(0, new ListItem("--Select Curriculum--", "0", true));
        }
    }
    protected void ddlCurriculum_SelectedIndexChanged(object sender, EventArgs e)
    {
        long schoolTypeId = Convert.ToInt64(Session["schooltype"]); //new SchoolLIB().GetSchoolTypeId(Convert.ToInt64(txtSchoolId.Text.Trim()));
        if (ddlCurriculum.SelectedIndex == 1)
        {
            schoolTypeId = 0; //new SchoolLIB().GetSchoolTypeId(Convert.ToInt64(txtSchoolId.Text.Trim()));
            gdvCurriulumSubjectsList.DataSource = new SchoolLIB().MasterSubjects(Convert.ToInt64(ddlCurriculum.SelectedValue), schoolTypeId);
            gdvCurriulumSubjectsList.DataBind();
            btnSaveSchoolSubjects.Visible = true;
            lblSubjectSetup.Text = "Kindly select all the subjects your school is offering from the list below";
        }
        else if (ddlCurriculum.SelectedIndex == 2)
        {
            if (Session["schooltype"] != null)
            {
                gdvCurriulumSubjectsList.DataSource = new SchoolLIB().MasterSubjects(Convert.ToInt64(ddlCurriculum.SelectedValue), schoolTypeId);
                gdvCurriulumSubjectsList.DataBind();
                btnSaveSchoolSubjects.Visible = true;
                lblSubjectSetup.Text = "Kindly select all the subjects your school is offering from the list below";
            }
        }


        if (ddlCurriculum.SelectedIndex == 1)
        {
            //If british is selected, then pick nigeria subjects as optional
            gdvOptionalSubjects.DataSource = new SchoolLIB().OptionalSubjects(Convert.ToInt64(2), Convert.ToInt64(2));
            gdvOptionalSubjects.DataBind();
            lblOptionalSubjects.Text = "Are you offering part of Nigeria curriculum?";
        }
        else if (ddlCurriculum.SelectedIndex == 2)
        {
            gdvOptionalSubjects.DataSource = new SchoolLIB().OptionalSubjects(Convert.ToInt64(1), Convert.ToInt64(0));
            gdvOptionalSubjects.DataBind();
            lblOptionalSubjects.Text = "Are you offering part of British curriculum?";
        }

    }
    protected void btnSaveSchoolSubjects_Click(object sender, EventArgs e)
    {
        try
        {
            //Save contact details
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            SelfSetupDetail newUserSetup = new SelfSetupDetail();
            newUserSetup.Email = txtEmail.Text.Trim();
            newUserSetup.Firstname = txtFirstname.Text.Trim();
            newUserSetup.Lastname = txtLastname.Text.Trim();
            newUserSetup.PhoneNo = txtPhoneNo.Text.Trim();
            newUserSetup.PositionInSchoolId = Convert.ToInt64(ddlPositioninSchool.SelectedValue);
            newUserSetup.SubmissionDate = DateTime.Now;
            new SchoolLIB().SaveNewSetupRequestDetails(newUserSetup);


            //Save school
            School saveSchool = new School();
            saveSchool.Name = txtSchoolName.Text.Trim();
            saveSchool.Code = txtSchoolCode.Text.Trim();
            saveSchool.SchoolTypeId = Convert.ToInt32(ddlSchoolType.SelectedValue);
            new SchoolLIB().SaveNewSchool(saveSchool);


            //Save campus
            SchoolCampus saveCampus = new SchoolCampus();
            saveCampus.Name = txtCampusName.Text.Trim();
            saveCampus.CampusAddress = txtCampusAddress.Text.Trim();
            saveCampus.SchoolId = saveSchool.Id;
            new CampusLIB().SaveCampus(saveCampus);

            //Save user
            PASSIS.LIB.User usr = new PASSIS.LIB.User();
            PASSIS.LIB.UsersLIB userLIB = new PASSIS.LIB.UsersLIB();
            usr.EmailAddress = Session["email"].ToString();
            usr.FirstName = Session["firstname"].ToString();
            usr.LastName = Session["lastname"].ToString();
            usr.PhoneNumber = Session["phoneno"].ToString();
            usr.Password = "P" + usr.PhoneNumber.Substring(7, 4) + DateTime.Now.Year;
            usr.LastLoginDate = DateTime.Now;
            usr.DateOfBirth = DateTime.Now;
            usr.DateCreated = DateTime.Now;
            usr.SchoolId = saveSchool.Id;
            usr.Username = usr.EmailAddress;
            usr.SchoolCampusId = saveCampus.Id;
            userLIB.SaveUser(usr);

            //save user role
            UserRole usrRl = new UserRole();
            usrRl.RoleId = Convert.ToInt64(2);
            usrRl.UserId = usr.Id;
            userLIB.SaveUserRole(usrRl);


            //Save subjects
            Boolean isChecked = false;
            foreach (GridViewRow row in gdvCurriulumSubjectsList.Rows)
            {
                CheckBox chkSelectSubject = row.FindControl("SubjectCheckbox") as CheckBox;
                Label lblSubjectId = row.FindControl("lblId") as Label;
                Int64 Id = Convert.ToInt64(lblSubjectId.Text.Trim());

                if (chkSelectSubject.Checked)
                {
                    isChecked = true;
                    long id = Id;
                    SubjectsInSchool newSubject = new SubjectsInSchool();
                    newSubject.SubjectId = Convert.ToInt32(Id);
                    newSubject.SchoolId = saveSchool.Id;
                    newSubject.SchoolPickedSubject = 1; //One is use to denote all the subjects under the selected curriculum a user picked
                    if (new SchoolLIB().subjectInSchoolExist(Convert.ToInt32(Id), saveSchool.Id))
                    {
                        //Subject exist, don't save
                    }
                    else
                    {
                        new SchoolLIB().SaveSchoolCurriculum(newSubject);
                        chkSelectSubject.Checked = false;
                    }
                }

            }

            if (isChecked == true)
            {
                School objSchool = context.Schools.FirstOrDefault(x => x.Id == saveSchool.Id);
                objSchool.CurriculumId = Convert.ToInt32(ddlCurriculum.SelectedValue);
                context.SubmitChanges();
            }
            else
            {
                MultiView1.SetActiveView(ViewSetupCurriculum);
                lblSubjectSetupResponse.Text = "Kindly select the subjects for each class before moving to the next stage";
                lblSubjectSetupResponse.ForeColor = System.Drawing.Color.Red;
                return;
            }

            foreach (GridViewRow row in gdvOptionalSubjects.Rows)
            {
                CheckBox chkSelectSubject = row.FindControl("SubjectCheckboxOptional") as CheckBox;
                Label lblSubjectId = row.FindControl("lblIdOptional") as Label;
                Int64 Id = Convert.ToInt64(lblSubjectId.Text.Trim());

                if (chkSelectSubject.Checked)
                {
                    long id = Id;
                    SubjectsInSchool newSubject = new SubjectsInSchool();
                    newSubject.SubjectId = Convert.ToInt32(Id);
                    newSubject.SchoolId = saveSchool.Id;
                    newSubject.SchoolPickedSubject = 0; //Zero is use to denote optional subjects in the database
                    new SchoolLIB().SaveSchoolCurriculum(newSubject);
                    chkSelectSubject.Checked = false;
                }

            }

            //Mail Code

            //SmtpClient client = new SmtpClient();
            //client.Port = 587;
            //client.Host = "smtp.gmail.com";
            //client.EnableSsl = true;
            //client.Timeout = 10000;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = false;
            //client.Credentials = new System.Net.NetworkCredential("passisng@gmail.com", "5pecialprojects");

            //MailMessage mm = new MailMessage("donotreply@domain.com", txtEmail.Text.Trim(), "Passis Login Details", "Congratulations, Thanks for registering on Passis.<br /> Username: " + userDetail.Username + "<br />Password: " + userDetail.Password);
            //mm.BodyEncoding = UTF8Encoding.UTF8;
            //mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            //client.Send(mm);

            var getUserLogin = from user in context.Users
                               where user.SchoolId == Convert.ToInt64(saveSchool.Id)
                               select user;
            gdvLoginDetails.DataSource = getUserLogin;
            gdvLoginDetails.DataBind();
            MultiView1.SetActiveView(ViewSummary);
            //try
            //{
            //    StringBuilder sb = new StringBuilder();
            //    sb.Append("<div class=col-md-8 col-sm-6 col-xs-12>");
            //    sb.Append("<div style='margin-left:0px; margin-top:10px'>");
            //    sb.Append("Hello " + Session["firstname"].ToString() + ",<br/><br/>");
            //    sb.Append("Thank you for choosing PASSIS.<br/><br/>");
            //    sb.Append(string.Format("Your school {0} has been setup successfully.", Session["schoolname"].ToString()) + "<br/><br/>");
            //    sb.Append("Kindly use the details below to login as school administrator.<br/><br/>");
            //    sb.Append("<table border=0>");
            //    foreach (User userDetail in getUserLogin)
            //    {
            //        sb.Append("<tr>");
            //        sb.Append("<td>");
            //        sb.Append(string.Format("Username: {0}", userDetail.Username + ","));
            //        sb.Append("<td>");
            //        sb.Append("<td>");
            //        sb.Append(string.Format("Password: {0}", userDetail.Password));
            //        sb.Append("<td>");
            //        sb.Append("</tr>");
            //        sb.Append("</table><br/><br/>");
            //        sb.Append("Thank you.");
            //        sb.Append("</div>");
            //        sb.Append("</div>");
            //        new PendingEmailLIB().SendMail(Session["email"].ToString(), sb.ToString(), userDetail);

            //        //Mail Continuation
            //        MailMessage mm = new MailMessage("donotreply@domain.com", txtEmail.Text.Trim(), "Passis Login Details", "Congratulations, Thanks for registering on Passis. Username: " + userDetail.Username + " Password: " + userDetail.Password);
            //        mm.BodyEncoding = UTF8Encoding.UTF8;
            //        mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            //        client.Send(mm);
            //    }
            //    //new PendingEmailLIB().SendMail(Session["email"].ToString(), sb.ToString(), "School Setup Notification");
            //}
            //catch (Exception ex)
            //{

            //}
            Session["email"] = null;

        }
        catch (Exception ex)
        {

        }

    }
    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chk = (sender as CheckBox);
            if (chk.ID == "chkAll")
            {
                foreach (GridViewRow row in gdvCurriulumSubjectsList.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        row.Cells[1].Controls.OfType<CheckBox>().FirstOrDefault().Checked = chk.Checked;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
            //lblMessage.Text = "Error occured, kindly contact your administrator";
            //lblMessage.ForeColor = System.Drawing.Color.Red;
            //lblMessage.Visible = true;
        }
    }
    protected void chkAll_CheckedChangedOptional(object sender, EventArgs e)
    {
        try
        {
            CheckBox chk = (sender as CheckBox);
            if (chk.ID == "chkAllOptional")
            {
                foreach (GridViewRow row in gdvOptionalSubjects.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        row.Cells[1].Controls.OfType<CheckBox>().FirstOrDefault().Checked = chk.Checked;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
            //lblMessage.Text = "Error occured, kindly contact your administrator";
            //lblMessage.ForeColor = System.Drawing.Color.Red;
            //lblMessage.Visible = true;
        }
    }
    protected void btnContinue_Click(object sender, EventArgs e)
    {
        if (new SchoolLIB().selfSetupEmailExist(txtEmail.Text.Trim()))
        {
            lblMessage.Text = "Email already exist";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        if (new SchoolLIB().schoolNameExist(txtSchoolName.Text.Trim()))
        {
            lblMessage.Text = "School name already exist";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        if (new CampusLIB().CampusExist(txtCampusName.Text.Trim()))
        {
            lblMessage.Text = "Campus name already exist!";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        Session["email"] = txtEmail.Text.Trim();
        Session["firstname"] = txtFirstname.Text.Trim();
        Session["lastname"] = txtLastname.Text.Trim();
        Session["phoneno"] = txtPhoneNo.Text.Trim();
        Session["PositionInSchool"] = Convert.ToInt64(ddlPositioninSchool.SelectedValue);

        Session["schoolname"] = txtSchoolName.Text.Trim();
        Session["schooltype"] = Convert.ToInt64(ddlSchoolType.SelectedValue);
        Session["campusname"] = txtCampusName.Text.Trim();
        Session["campusaddress"] = txtCampusAddress.Text.Trim();

        MultiView1.SetActiveView(ViewSetupCurriculum);

    }
}