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


public partial class NewsLetter : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        BindGrid2();
    }

   

    
    protected void gdvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvList.PageIndex = e.NewPageIndex;
        BindGrid2();
    }
    protected void BindGrid2()
    {
       long ?termId = new PASSIS.LIB.AcademicSessionLIB().GetCurrentTermId(logonUser.SchoolId);
        long sessionId = new PASSIS.LIB.AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);
        var newsLetter = from s in context.NewsLetters
                          where s.UploadedById == logonUser.Id && s.TermId == termId && s.SessionId == sessionId
                          && s.SchoolId == logonUser.SchoolId && s.CampusId == logonUser.SchoolCampusId
                          select s;

        gdvList.DataSource = newsLetter;
        gdvList.DataBind();

    }
    protected void gdvList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "download")
        {
            string filename = e.CommandArgument.ToString();
            string path = MapPath("~/docs/NewsLetter/" + filename);
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
        if (e.CommandName == "remove")
        {
            Int64 Id = Convert.ToInt64(e.CommandArgument);
            PASSISLIBDataContext ct = new PASSISLIBDataContext();
            PASSIS.LIB.NewsLetter news = new PASSIS.LIB.NewsLetter();
            news = ct.NewsLetters.FirstOrDefault(s => s.Id == Id);
            ct.NewsLetters.DeleteOnSubmit(news);
            ct.SubmitChanges();
            BindGrid2();
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "News Letter removed successfully";
            lblErrorMsg.ForeColor = System.Drawing.Color.Green;

        }
    }
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
      
        try
        {
           
                if (documentUpload.HasFile)
                {
                    string originalFileName = Path.GetFileName(documentUpload.PostedFile.FileName);
                    string modifiedFileName = string.Format("{0}{1}", DateTime.Now.ToString("MMddHmmss"), originalFileName);
                    string fileExtension = System.IO.Path.GetExtension(documentUpload.PostedFile.FileName);
                    string fileLocation = Server.MapPath("~/docs/NewsLetter/") + modifiedFileName;
                    documentUpload.SaveAs(fileLocation);
              
                    if (!(fileExtension.Contains(".doc")) && !(fileExtension.Contains(".pdf")))
                    {
                        lblErrorMsg.Text = string.Format("Upload not succesful. The file format is not supported!!!");
                        lblErrorMsg.Visible = true;
                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                        return;
                    }

                    PASSIS.LIB.NewsLetter newssletter = new PASSIS.LIB.NewsLetter();
                    newssletter.SchoolId = logonUser.SchoolId;
                    newssletter.CampusId = logonUser.SchoolCampusId;
                    newssletter.TermId = new PASSIS.LIB.AcademicSessionLIB().GetCurrentTermId(logonUser.SchoolId);
                    newssletter.SessionId  = new PASSIS.LIB.AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);
                    newssletter.UploadedById = logonUser.Id;
                    newssletter.Description = txtDesc.Text.Trim();
                    newssletter.FileName = modifiedFileName;
                    newssletter.DateUploaded = DateTime.Now;

                    context.NewsLetters.InsertOnSubmit(newssletter);
                    context.SubmitChanges();
                    BindGrid2();
                    lblErrorMsg.Text = "NewsLetter Uploaded Successfully";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                    lblErrorMsg.Visible = true;

                }
                else
                {
                    lblErrorMsg.Text = string.Format("File upload is required.");
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
}