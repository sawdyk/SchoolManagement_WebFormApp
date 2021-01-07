using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using System.IO;
using PASSIS.LIB;


public partial class AdminResourceUpload : PASSIS.LIB.Utility.BasePage
{
    
    PASSISLIBDataContext context = new PASSISLIBDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        //Bindgrid();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        //validate the value and save
        //impose restriction on file sixe
        PASSIS.LIB.ResourceCenter objResourceCenter = new PASSIS.LIB.ResourceCenter();
        try
        {
            if (string.IsNullOrEmpty(txtDesc.Text))
            {
                lblErrorMsg.Text = string.Format("A short document description is required.");
                lblErrorMsg.Visible = true;
                return;
            }


            if (documentUpload.HasFile)
            {
                string originalFileName = Path.GetFileName(documentUpload.PostedFile.FileName);
                string modifiedFileName = string.Format("{0}{1}", DateTime.Now.ToString("MMddHmmss"), originalFileName).Replace(" ", "");
                string fileExtension = Path.GetExtension(documentUpload.PostedFile.FileName);
                string fileLocation = Server.MapPath("~/docs/ResourceCenter/") + modifiedFileName;

                //Check whether file extension is xls or xslx

                if (!fileExtension.Contains(".pdf") && !fileExtension.Contains(".xls") && !fileExtension.Contains(".doc"))
                {
                    lblErrorMsg.Text = string.Format("Upload not succesful. The file format is not supported!!!");
                    lblErrorMsg.Visible = true;
                    return;
                }

                long getExtension = 0L;

                if (fileExtension.Contains(".xls"))
                {
                    getExtension = (long)FileExtension.MicrosoftExcel;
                }
                else if (fileExtension.Contains(".pdf"))
                {
                    getExtension = (long)FileExtension.PdfDocument;
                }
                else if (fileExtension.Contains(".doc"))
                {
                    getExtension = (long)FileExtension.MicrosoftWord;
                }

                objResourceCenter.FileDesc = txtDesc.Text.Trim();
                objResourceCenter.FileName = modifiedFileName.Replace(" ", "");
                objResourceCenter.SchoolId = logonUser.SchoolId;
                objResourceCenter.CampusId = logonUser.SchoolCampusId;
                objResourceCenter.DocumentType = getExtension;
                context.ResourceCenters.InsertOnSubmit(objResourceCenter);
                context.SubmitChanges();
                documentUpload.SaveAs(fileLocation);
                lblErrorMsg.Text = string.Format("Uploaded Succesfully.");
                lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                lblErrorMsg.Visible = true;
            }

            //Bindgrid();

        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = "Error occurred, kindly contact the administrator";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
        }
    }

    public enum FileExtension
    {
        MicrosoftExcel = 1,
        PdfDocument = 2,
        MicrosoftWord = 3
    }


    protected void btnSaveLink_Click(object sender, EventArgs e)
    {
        try
        {
            string ResourceLinkDescription = txtLinkDesc.Text.Trim();
            string ResourceLink = txtLink.Text.Trim();
            if (ResourceLinkDescription.Equals(""))
            {
                lblErrorMsg.Text = "Link resource description is required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
            if (ResourceLink.Equals(""))
            {
                lblErrorMsg.Text = "Link resource is required";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }

            ExternalResourceCenter objExternalResourceCenter = new ExternalResourceCenter();
            objExternalResourceCenter.CampusId = logonUser.SchoolCampusId;
            objExternalResourceCenter.Description = ResourceLinkDescription;
            objExternalResourceCenter.ExternalLink = ResourceLink;
            context.ExternalResourceCenters.InsertOnSubmit(objExternalResourceCenter);
            context.SubmitChanges();
            lblErrorMsg.Text = "Link saved successfully";
            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
            lblErrorMsg.Visible = true;
            txtLink.Text = "";
            txtLinkDesc.Text = "";
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = "Error occurred, kindly contact the administrator" + ex.Message + " " + ex.StackTrace;
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
        }
    }
}