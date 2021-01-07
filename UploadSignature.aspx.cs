using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.LIB;

public partial class UploadSignature : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    private static int maximumFileSize = 4194304; //4MB
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //ddlUploadType.Items.Add(new System.Web.UI.WebControls.ListItem("School Logo", "1"));
            //ddlUploadType.Items.Add(new System.Web.UI.WebControls.ListItem("Student Passport", "2"));
        }
    }
    protected void btnImageUpload_Click(object sender, EventArgs e)
    {
        //if (ddlUploadType.SelectedValue == "1")
        //{
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

        if (ImageFile.HasFile)
            {
                string originalFileName = Path.GetFileName(ImageFile.PostedFile.FileName);
                string modifiedFileName = string.Format("{0}{1}", DateTime.Now.ToString("MMddHmmss"), originalFileName);
                string fileExtensionLetter = Path.GetExtension(ImageFile.PostedFile.FileName);
                int fileSizeLetter = ImageFile.PostedFile.ContentLength;
                string fileLocation = Server.MapPath("~/Images/SchoolSignature/") + originalFileName;


                //Check the file extension

                if (!fileExtensionLetter.ToLower().Equals(".jpg") && !fileExtensionLetter.ToLower().Equals(".png") && !fileExtensionLetter.ToLower().Equals(".gif") && !fileExtensionLetter.ToLower().Equals(".jpeg") && !fileExtensionLetter.ToLower().Equals(".tiff"))
                {
                    lblErrorMsg.Text = "Invalid file format for " + originalFileName + " letter";
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                else
                {
                    if (fileSizeLetter > maximumFileSize)
                    {
                        lblErrorMsg.Text = "4MB file size exceeded for attached documentation";
                        lblErrorMsg.Visible = true;
                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                }

                PASSIS.LIB.School school = context.Schools.FirstOrDefault(x => x.Id == logonUser.SchoolId);
                ImageFile.SaveAs(fileLocation);
                school.Signature = "~/Images/SchoolSignature/" + originalFileName;


                //Check the file extension;
                context.SubmitChanges();

                lblResult.Visible = true;
                lblErrorMsg.Text = string.Format("Uploaded Successfully.");
                lblErrorMsg.Visible = true;
                lblErrorMsg.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblErrorMsg.Text = "Kindly select the product image";
                lblErrorMsg.Visible = true;
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                return;
            }
        }
    
}