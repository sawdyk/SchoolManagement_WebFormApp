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

public partial class SupAdminResourceUpload : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        Bindgrid();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //validate the value and save
        //impose restriction on file sixe
        GeneralResourceCenter rs = new GeneralResourceCenter();
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


                rs.FileDescription = txtDesc.Text.Trim();
                rs.FileName = modifiedFileName.Replace(" ", "");
                rs.DocumentType = fileExtension;
                context.GeneralResourceCenters.InsertOnSubmit(rs);
                context.SubmitChanges();
                documentUpload.SaveAs(fileLocation);
                lblErrorMsg.Text = string.Format("Upload Succesful.");
                lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                lblErrorMsg.Visible = true;
            }

            Bindgrid();

        }
        catch (Exception ex)
        {

        }
    }

    private void Bindgrid()
    {
        gdvList.DataSource = context.GeneralResourceCenters;
        gdvList.DataBind();
    }
    protected void gdvList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "cmd":
                    GeneralResourceCenter objResource = new GeneralResourceCenter();
                    int id = Convert.ToInt32(e.CommandArgument);
                    objResource = context.GeneralResourceCenters.First(x => x.ID == id);
                    if (objResource.DocumentType.Equals(".xls"))
                    {
                        string path = MapPath("~/docs/ResourceCenter/" + objResource.FileName);
                        byte[] bts = System.IO.File.ReadAllBytes(path);
                        Response.Clear();
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Type", "Application/ms-excel");
                        Response.AddHeader("Content-Length", bts.Length.ToString());
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + objResource.FileName);
                        Response.BinaryWrite(bts);
                        Response.Flush();
                        Response.End();
                    }
                    else if (objResource.DocumentType.Equals(".xlsx"))
                    {
                        string path = MapPath("~/docs/ResourceCenter/" + objResource.FileName);
                        byte[] bts = System.IO.File.ReadAllBytes(path);
                        Response.Clear();
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Type", "Application/ms-excel");
                        Response.AddHeader("Content-Length", bts.Length.ToString());
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + objResource.FileName);
                        Response.BinaryWrite(bts);
                        Response.Flush();
                        Response.End();
                    }
                    else if (objResource.DocumentType.Equals(".pdf"))
                    {
                        string path = MapPath("~/docs/ResourceCenter/" + objResource.FileName);
                        byte[] bts = System.IO.File.ReadAllBytes(path);
                        Response.Clear();
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Type", "Application/pdf");
                        Response.AddHeader("Content-Length", bts.Length.ToString());
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + objResource.FileName);
                        Response.BinaryWrite(bts);
                        Response.Flush();
                        Response.End();
                    }
                    else if (objResource.DocumentType.Equals(".doc"))
                    {
                        string path = MapPath("~/docs/ResourceCenter/" + objResource.FileName);
                        byte[] bts = System.IO.File.ReadAllBytes(path);
                        Response.Clear();
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Type", "Application/msword");
                        Response.AddHeader("Content-Length", bts.Length.ToString());
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + objResource.FileName);
                        Response.BinaryWrite(bts);
                        Response.Flush();
                        Response.End();
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "Error occured, kindly contact the administrator";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
        }
    }

}