using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO;
using PASSIS.DAO.Utility;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using PASSIS.LIB;
using System.IO;

public partial class AdmissionApplication : PASSIS.LIB.Utility.BasePage
{
    //protected void Page_PreInit(object sender, EventArgs e)
    //{
    //    base.Page_PreInit(sender, e);
    //    this.MasterPageFile = "~/SchlAdminAdmission.master";
    //}
    PASSISLIBDataContext context = new PASSISLIBDataContext();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlGender.Items.Add(new ListItem("Male", "1"));
            ddlGender.Items.Add(new ListItem("Female", "2"));

            //ddlYear.Items.Add(new ListItem("2010", "1"));
            //ddlYear.Items.Add(new ListItem("2011", "2"));
            //ddlYear.Items.Add(new ListItem("2012", "3"));
            //ddlYear.Items.Insert(0, new ListItem("None", "0"));

            ddlDisability.Items.Add(new ListItem("No", "1"));
            ddlDisability.Items.Add(new ListItem("Yes", "2"));

            ddlIllness.Items.Add(new ListItem("No", "1"));
            ddlIllness.Items.Add(new ListItem("Yes", "2"));

            ddlInoculations.Items.Add(new ListItem("No", "1"));
            ddlInoculations.Items.Add(new ListItem("Yes", "2"));

            ddlAdmissionSession.DataSource = from ses in context.AcademicSessionNames
                                             select ses;
            ddlAdmissionSession.DataBind();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string fileLocation = string.Empty;
            if (documentUpload.HasFile)
            {

                if (documentUpload.PostedFile.ContentType != "image/jpeg" && documentUpload.PostedFile.ContentType != "image/png")
                {
                    lblReport.Text = string.Format("Upload Status: only jpg and png files are accepted.");
                    lblReport.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                if (documentUpload.PostedFile.ContentLength > 30720)
                {
                    lblReport.Text = string.Format("Upload Status: The file has to be less than 30 kb!");
                    lblReport.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                AdmissionApp newAdmissionObj = new AdmissionApp();

                long applicationId = 0;
                var checkIsTableEmpty = from appId in context.AdmissionApps
                                        select appId;
                if (checkIsTableEmpty.Count() > 0)
                {
                    var maxValue = context.AdmissionApps.Max(x => x.Id);
                    applicationId = maxValue + 1;
                }
                else
                {
                    applicationId = 1;
                }

                newAdmissionObj.ApplicationID = "PA0" + applicationId + DateTime.Now.ToString("yyyy");
                newAdmissionObj.LastName = txtLastname.Text.Trim();
                newAdmissionObj.FirstName = txtFirstname.Text.Trim();
                newAdmissionObj.Gender = ddlGender.SelectedItem.Text;
                newAdmissionObj.MiddleName = txtMiddlename.Text.Trim();
                newAdmissionObj.AdmissionSessionId = Convert.ToInt64(ddlAdmissionSession.SelectedValue);
                newAdmissionObj.DateOfBirth = txtDateOfBirth.Text.Trim();
                newAdmissionObj.LastSchoolAttended = txtLastSchoolAttended.Text.Trim();
                newAdmissionObj.LastSchoolAttendedAddress = txtLastSchoolAddress.Text.Trim();
                newAdmissionObj.LastSchoolAttendedClass = txtClass.Text.Trim();
                newAdmissionObj.LastSchoolAttendedYear = txtYear.Text;
                newAdmissionObj.Disability = ddlDisability.SelectedItem.Text;
                newAdmissionObj.GuardianName = txtGuardianName.Text.Trim();
                newAdmissionObj.GuardianEmail = txtGuardianEmail.Text.Trim();
                newAdmissionObj.GuardianAddress = txtGuardianAddress.Text.Trim();
                newAdmissionObj.GuardianPhoneNumber = txtGuardianPhoneNo.Text.Trim();
                newAdmissionObj.GuardianRelationship = txtRelationship.Text.Trim();
                newAdmissionObj.Occupation = txtOccupation.Text.Trim();
                newAdmissionObj.PlaceOfWork = txtPlaceofWork.Text.Trim();
                newAdmissionObj.LastLoginDate = DateTime.Now;
                newAdmissionObj.DateCreated = DateTime.Now;
                context.AdmissionApps.InsertOnSubmit(newAdmissionObj);
                context.SubmitChanges();

                PASSIS.LIB.School mySchool = context.Schools.FirstOrDefault(x => x.Id == logonUser.SchoolId);
                AdmissionApp admObj = context.AdmissionApps.FirstOrDefault(x => x.Id == newAdmissionObj.Id);
                string originalFileName = Path.GetFileName(documentUpload.PostedFile.FileName);
                string modifiedFileName = string.Format("{0}", mySchool.Code + "00" + newAdmissionObj.Id);
                string fileExtension = Path.GetExtension(documentUpload.PostedFile.FileName);
                string fileWithoutExt = Path.GetFileNameWithoutExtension(documentUpload.PostedFile.FileName);
                fileLocation = "~/images/admission/" + modifiedFileName + fileExtension.ToLower();
                if (string.IsNullOrEmpty(fileLocation))
                {
                    lblReport.Text = string.Format("Retry passport upload! ");
                    lblReport.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    documentUpload.SaveAs(Server.MapPath(fileLocation));
                    admObj.PassportFileName = fileLocation;
                    context.SubmitChanges();

                    var getConfigureAdminssionMode = context.AdmissionConfigurations.FirstOrDefault(s => s.SchoolId == logonUser.SchoolId);
                    AdmissionApplicationList objAdminList = new AdmissionApplicationList();
                    objAdminList.SchoolId = (long)logonUser.SchoolId;
                    objAdminList.ApplicantId = newAdmissionObj.ApplicationID;
                    objAdminList.DateApplied = DateTime.Now;
                    if (getConfigureAdminssionMode.AdmissionMode == "Form Only" && getConfigureAdminssionMode.SelectFormFee == "NO")
                    {
                        objAdminList.ProcessingLevel = 4;
                    }
                    else if (getConfigureAdminssionMode.AdmissionMode != "Form Only" && getConfigureAdminssionMode.SelectFormFee == "NO")
                    {
                        objAdminList.ProcessingLevel = 2;
                    }
                    else
                    { objAdminList.ProcessingLevel = 1; }
                    objAdminList.Admissionstatus = "In Processing";
                    context.AdmissionApplicationLists.InsertOnSubmit(objAdminList);
                    context.SubmitChanges();
                    Session["ApplicationNo"] = newAdmissionObj.ApplicationID;
                    Response.Redirect("~/AdmissionFormPayment.aspx");
                    //lblReport.Text = "Saved successfully";
                    //lblReport.ForeColor = System.Drawing.Color.Green;
                }

            }
            else
            {
                lblReport.Text = string.Format("Upload Status: A passport upload is required!");
                lblReport.ForeColor = System.Drawing.Color.Red;
            }



        }
        catch (Exception ex)
        {
        }
    }
}