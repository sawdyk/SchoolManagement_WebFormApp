using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.LIB;
using PASSIS.DAO.Utility;

public partial class AdmissionConfig : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlProcessAdmission.Items.Add(new ListItem("YES", "1"));
            ddlProcessAdmission.Items.Add(new ListItem("NO", "2"));
            ddlProcessAdmission.Items.Insert(0, new ListItem("--Select--", "0"));
            ddlFormFee.Items.Add(new ListItem("YES", "1"));
            ddlFormFee.Items.Add(new ListItem("NO", "2"));
            ddlFormFee.Items.Insert(0, new ListItem("--Select--", "0"));
            ddlSiteRedirect.Items.Add(new ListItem("YES", "1"));
            ddlSiteRedirect.Items.Add(new ListItem("NO", "2"));
            ddlSiteRedirect.Items.Insert(0, new ListItem("--Select--", "0"));
            ddlAdmissionMode.Items.Add(new ListItem("Form Only", "1"));
            ddlAdmissionMode.Items.Add(new ListItem("Form/Test", "2"));
            ddlAdmissionMode.Items.Add(new ListItem("Form/Test/Interview", "3"));
            ddlAdmissionMode.Items.Insert(0, new ListItem("--Select--", "0"));

            BindGrid();
            BindText();
            BindInterview();
        }

    }
    protected void ddlProcessAdmission_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtFormFee.Text = "";
        txtAccount.Text = "";
        txtWebsiteUrl.Text = "";
        txtMinimiumTestScore.Text = "";
        txtMinimiumInterviewScore.Text = "";
        if (ddlProcessAdmission.SelectedIndex == 1)
        {
            lblAdmissionMode.Visible = true;
            ddlAdmissionMode.Visible = true;

            lblSelecteFormFee.Visible = true;
            ddlFormFee.Visible = true;

            lblFormFee.Visible = false;
            txtFormFee.Visible = false;

            lblAccountDetails.Visible = false;
            txtAccount.Visible = false;

            btnSaveConfig.Visible = true;

            lblRedirectToSite.Visible = false;
            ddlSiteRedirect.Visible = false;

            lblWebsiteUrl.Visible = false;
            txtWebsiteUrl.Visible = false;

            lblTextRequirement.Visible = false;
            txtTestRequirement.Visible = false;

            lblInterviewRequirement.Visible = false;
            txtInterviewRequirement.Visible = false;

            lblMinimiumInterviewScore.Visible = false;
            txtMinimiumInterviewScore.Visible = false;

            lblMinimiumTestScore.Visible = false;
            txtMinimiumTestScore.Visible = false;
        }
        else if (ddlProcessAdmission.SelectedIndex == 2)
        {
            lblSelecteFormFee.Visible = false;
            ddlFormFee.Visible = false;

            lblRedirectToSite.Visible = true;
            ddlSiteRedirect.Visible = true;

            lblFormFee.Visible = false;
            txtFormFee.Visible = false;

            lblAccountDetails.Visible = false;
            txtAccount.Visible = false;
            btnSaveConfig.Visible = false;

            lblTextRequirement.Visible = false;
            txtTestRequirement.Visible = false;

            lblInterviewRequirement.Visible = false;
            txtInterviewRequirement.Visible = false;

            lblMinimiumInterviewScore.Visible = false;
            txtMinimiumInterviewScore.Visible = false;

            lblMinimiumTestScore.Visible = false;
            txtMinimiumTestScore.Visible = false;
            lblAdmissionMode.Visible = true;
            ddlAdmissionMode.Visible = true;
        }
    }
    protected void ddlSiteRedirect_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtFormFee.Text = "";
        txtWebsiteUrl.Text = "";
        txtMinimiumTestScore.Text = "";
        txtMinimiumInterviewScore.Text = "";
        if (ddlSiteRedirect.SelectedIndex == 1)
        {
            lblFormFee.Visible = false;
            txtFormFee.Visible = false;

            lblAccountDetails.Visible = false;
            txtAccount.Visible = false;

            btnSaveConfig.Visible = true;
            lblWebsiteUrl.Visible = true;
            txtWebsiteUrl.Visible = true;

            lblMinimiumInterviewScore.Visible = false;
            txtMinimiumInterviewScore.Visible = false;

            lblMinimiumTestScore.Visible = false;
            txtMinimiumTestScore.Visible = false;
        }
        else if (ddlSiteRedirect.SelectedIndex == 2)
        {
            lblFormFee.Visible = false;
            txtFormFee.Visible = false;

            lblAccountDetails.Visible = false;
            txtAccount.Visible = false;
            lblMinimiumInterviewScore.Visible = false;
            txtMinimiumInterviewScore.Visible = false;

            lblMinimiumTestScore.Visible = false;
            txtMinimiumTestScore.Visible = false;
            btnSaveConfig.Visible = true;

            lblWebsiteUrl.Visible = false;
            txtWebsiteUrl.Visible = false;
        }
    }
    protected void btnSaveConfig_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlProcessAdmission.SelectedIndex == 0)
            {
                lblResponse.Text = "Kindly select option for processing your admission on Passis";
                lblResponse.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                var getConfig = (from configList in context.AdmissionConfigurations
                                 where configList.SchoolId == (long)logonUser.SchoolId
                                 select configList).ToList();
                if (getConfig.Count > 0)
                {
                    lblResponse.Text = "Configuration already exist, kindly use panel below to edit";
                    lblResponse.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                else
                {
                    AdmissionConfiguration objConfig = new AdmissionConfiguration();
                    objConfig.SchoolId = (long)logonUser.SchoolId;
                    if (ddlProcessAdmission.SelectedIndex == 1)
                    {
                        objConfig.IsProcessAdmissionOnPassis = ddlProcessAdmission.SelectedItem.Text;
                        objConfig.IsRedirect = "";
                    }
                    else if (ddlProcessAdmission.SelectedIndex == 2)
                    {
                        objConfig.IsProcessAdmissionOnPassis = ddlProcessAdmission.SelectedItem.Text;
                        objConfig.IsRedirect = ddlSiteRedirect.SelectedItem.Text;
                    }
                    objConfig.WebsiteUrl = txtWebsiteUrl.Text.Trim();
                    objConfig.SelectFormFee = ddlFormFee.SelectedItem.Text;
                    objConfig.FormFee = txtFormFee.Text.Trim();
                    objConfig.AccountDetail = txtAccount.Text.Trim();
                    objConfig.AdmissionMode = ddlAdmissionMode.SelectedItem.Text;
                    objConfig.MinimiumTestScore = txtMinimiumTestScore.Text.Trim();
                    objConfig.MinimiumInterviewScore = txtMinimiumInterviewScore.Text.Trim();
                    context.AdmissionConfigurations.InsertOnSubmit(objConfig);
                    context.SubmitChanges();
                    lblResponse.Text = "Configuration saved successfully";
                    BindGrid();
                    lblResponse.ForeColor = System.Drawing.Color.Green;

                }

            }
        }
        catch (Exception ex)
        {

        }
    }

    private void BindGrid()
    {

        gdvConfig.DataSource = from configList in context.AdmissionConfigurations
                               where configList.SchoolId == logonUser.SchoolId
                               select configList;
        gdvConfig.DataBind();
    }
    private void BindText()
    {
        gdvTestRequirements.DataSource = from s in context.AdmissionTestRequirements
                                         where s.SchoolId == logonUser.SchoolId
                                         select s;
        gdvTestRequirements.DataBind();
    }

    private void BindInterview()
    {
        gdvInterviewRequirements.DataSource = from s in context.AdmissionInterviewRequirements
                                              where s.SchoolId == logonUser.SchoolId
                                              select s;
        gdvInterviewRequirements.DataBind();
    }
    protected void gdvConfig_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gdvConfig.EditIndex = -1;
        BindGrid();
        lblResponseMessage.Text = "";
    }
    protected void gdvConfig_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gdvConfig.EditIndex = e.NewEditIndex;
        BindGrid();
        lblResponseMessage.Text = "";
    }
    protected void gdvConfig_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            Label lblId = (Label)gdvConfig.Rows[e.RowIndex].FindControl("lblId");
            DropDownList IsprocessedAdmission = (DropDownList)gdvConfig.Rows[e.RowIndex].FindControl("ddlGdvProcessAdmission");
            DropDownList AdmissionMode = (DropDownList)gdvConfig.Rows[e.RowIndex].FindControl("ddlGdvAdmissionMode");
            DropDownList IsRedirect = (DropDownList)gdvConfig.Rows[e.RowIndex].FindControl("ddlGdvRedirect");
            TextBox WebsiteUrl = (TextBox)gdvConfig.Rows[e.RowIndex].FindControl("txtGdvWebsiteUrl");
            DropDownList SelectFormFee = (DropDownList)gdvConfig.Rows[e.RowIndex].FindControl("ddlGdvSelectFormFee");
            TextBox FormFee = (TextBox)gdvConfig.Rows[e.RowIndex].FindControl("txtGdvFormFee");
            TextBox AccountDetail = (TextBox)gdvConfig.Rows[e.RowIndex].FindControl("txtGdvAccount");
            TextBox MinimiumTestScore = (TextBox)gdvConfig.Rows[e.RowIndex].FindControl("txtGdvMinimiumTestScore");
            TextBox MinimiumInterviewScore = (TextBox)gdvConfig.Rows[e.RowIndex].FindControl("txtGdvMinimiumInterviewScore");

            //Check if a school want to process admission on passis
            if (IsprocessedAdmission.SelectedValue == "YES")
            {
                //If the school want to process the admission on passis, then check if the admission form fee is empty
                //if (FormFee.Text.Equals(""))
                //{
                //    lblResponseMessage.Text = "Kindly type the admission form fee";
                //    lblResponseMessage.ForeColor = System.Drawing.Color.Red;
                //    return;
                //}
                //if (MinimiumTestInterviewScore.Text.Equals(""))
                //{

                //    lblResponseMessage.Text = "Kindly type the Minimium Test or Interview Score for Admission Process";
                //    lblResponseMessage.ForeColor = System.Drawing.Color.Red;
                //    return;
                //}
            }
            else
            {
                if (IsRedirect.SelectedValue == "YES")
                {
                    if (WebsiteUrl.Text.Equals(""))
                    {
                        lblResponseMessage.Text = "Kindly type your redirect website url";
                        lblResponseMessage.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                }
            }
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            PASSIS.LIB.AdmissionConfiguration objConfig = context.AdmissionConfigurations.FirstOrDefault(x => x.ID == Convert.ToInt64(lblId.Text));
            objConfig.IsProcessAdmissionOnPassis = IsprocessedAdmission.SelectedValue;
            objConfig.AdmissionMode = AdmissionMode.SelectedValue;
            objConfig.IsRedirect = IsRedirect.SelectedValue;
            objConfig.WebsiteUrl = WebsiteUrl.Text;
            objConfig.AccountDetail = AccountDetail.Text;
            objConfig.SelectFormFee = SelectFormFee.SelectedValue;
            objConfig.FormFee = FormFee.Text;
            objConfig.MinimiumTestScore = MinimiumTestScore.Text;
            objConfig.MinimiumInterviewScore = MinimiumInterviewScore.Text;
            context.SubmitChanges();
            gdvConfig.EditIndex = -1;
            BindGrid();
            lblResponseMessage.Text = "Saved Successfully";
            lblResponseMessage.ForeColor = System.Drawing.Color.Green;
            txtFormFee.Text = "";
            txtAccount.Text = "";
            txtWebsiteUrl.Text = "";
            txtMinimiumTestScore.Text = "";
            txtMinimiumInterviewScore.Text = "";
        }
        catch (Exception ex)
        {
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
            lblResponseMessage.Text = "Error occurred, kindly contact the administrator";
            lblResponseMessage.ForeColor = System.Drawing.Color.Red;
        }
    }
    protected void ddlFormFee_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtFormFee.Text = "";
        if (ddlFormFee.SelectedIndex == 1)
        {

            lblFormFee.Visible = true;
            txtFormFee.Visible = true;

            lblAccountDetails.Visible = true;
            txtAccount.Visible = true;

        }
        else if (ddlFormFee.SelectedIndex == 2)
        {

            lblFormFee.Visible = false;
            txtFormFee.Visible = false;

            lblAccountDetails.Visible = false;
            txtAccount.Visible = false;
        }
    }
    protected void btnSaveInterviewRequirement_Click(object sender, EventArgs e)
    {
        AdmissionInterviewRequirement intReq = new AdmissionInterviewRequirement();
        intReq.SchoolId = logonUser.SchoolId;
        intReq.Item = txtInterviewRequirement.Text.ToString();
        context.AdmissionInterviewRequirements.InsertOnSubmit(intReq);
        context.SubmitChanges();
        BindInterview();
        lblResponseMessage.Text = "Saved Successfully";
        lblResponseMessage.ForeColor = System.Drawing.Color.Green;
        txtInterviewRequirement.Text = "";
    }
    protected void btnSaveTextRequirement_Click(object sender, EventArgs e)
    {
        AdmissionTestRequirement tstReq = new AdmissionTestRequirement();
        tstReq.SchoolId = logonUser.SchoolId;
        tstReq.Item = txtTestRequirement.Text.ToString();
        context.AdmissionTestRequirements.InsertOnSubmit(tstReq);
        context.SubmitChanges();
        BindText();
        lblResponseMessage.Text = "Saved Successfully";
        lblResponseMessage.ForeColor = System.Drawing.Color.Green;
        txtTestRequirement.Text = "";
    }
    protected void gdvInterviewRequirements_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //Label lblSubjectSelectionError = (Label)fvwUser.FindControl("lblSubjectSelectionError");

        switch (e.CommandName)
        {

            case "Remove":
                //new SubjectTeachersLIB().DeleteSubjectTeacher(Convert.ToInt64(e.CommandArgument.ToString()));
                AdmissionInterviewRequirement inter = context.AdmissionInterviewRequirements.FirstOrDefault(s => s.Id == Convert.ToInt64(e.CommandArgument.ToString()));
                context.AdmissionInterviewRequirements.DeleteOnSubmit(inter);
                context.SubmitChanges();
                lblResponseMessage.ForeColor = System.Drawing.Color.Red;
                lblResponseMessage.Text = "Deleted Successfully";
                BindInterview();

                break;
        }
    }
    protected void gdvTestRequirements_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Remove":
                //new SubjectTeachersLIB().DeleteSubjectTeacher(Convert.ToInt64(e.CommandArgument.ToString()));
                AdmissionTestRequirement test = context.AdmissionTestRequirements.FirstOrDefault(s => s.Id == Convert.ToInt64(e.CommandArgument.ToString()));
                context.AdmissionTestRequirements.DeleteOnSubmit(test);
                context.SubmitChanges();
                lblResponseMessage.ForeColor = System.Drawing.Color.Red;
                lblResponseMessage.Text = "Deleted Successfully";
                BindText();

                break;
        }
    }
    protected void ddlAdmissionMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        //txtFormFee.Text = "";
        //txtAccount.Text = "";
        //txtWebsiteUrl.Text = "";
        txtMinimiumTestScore.Text = "";
        txtMinimiumInterviewScore.Text = "";
        if (ddlAdmissionMode.SelectedIndex == 1)
        {
            //AdmissionMode.Visible = true;
            //ddlFormFee.Visible = true;
            //FormFee.Visible = false;
            //AccountDetails.Visible = false;
            //btnSaveConfig.Visible = true;
            //RedirectToSite.Visible = false;
            //WebsiteUrl.Visible = false;
            lblTextRequirement.Visible = false;
            txtTestRequirement.Visible = false;

            lblInterviewRequirement.Visible = false;
            txtInterviewRequirement.Visible = false;

            lblMinimiumInterviewScore.Visible = false;
            txtMinimiumInterviewScore.Visible = false;

            lblMinimiumTestScore.Visible = false;
            txtMinimiumTestScore.Visible = false;
            btnSaveTestRequirement.Visible = false;
            btnSaveInterviewRequirement.Visible = false;


        }
        else if (ddlAdmissionMode.SelectedIndex == 2)
        {

            lblTextRequirement.Visible = true;
            txtTestRequirement.Visible = true;

            lblInterviewRequirement.Visible = false;
            txtInterviewRequirement.Visible = false;

            lblMinimiumInterviewScore.Visible = false;
            txtMinimiumInterviewScore.Visible = false;

            lblMinimiumTestScore.Visible = true;
            txtMinimiumTestScore.Visible = true;
            btnSaveTestRequirement.Visible = true;
            btnSaveInterviewRequirement.Visible = false;


        }
        else if (ddlAdmissionMode.SelectedIndex == 3)
        {


            lblTextRequirement.Visible = true;
            txtTestRequirement.Visible = true;

            lblInterviewRequirement.Visible = true;
            txtInterviewRequirement.Visible = true;

            lblMinimiumInterviewScore.Visible = true;
            txtMinimiumInterviewScore.Visible = true;

            lblMinimiumTestScore.Visible = true;
            txtMinimiumTestScore.Visible = true;
            btnSaveInterviewRequirement.Visible = true;
            btnSaveTestRequirement.Visible = true;



        }
    }
}