using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using PASSIS.LIB;

public partial class AdminInternalResource : PASSIS.LIB.Utility.BasePage
{
    
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bindgrid();
        }
    }
    protected void gdvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvList.PageIndex = e.NewPageIndex;
        gdvList.DataBind();

    }

    private void Bindgrid()
    {
        var getResource = from list in context.ResourceCenters
                          where list.SchoolId == logonUser.SchoolId
                          select list;
        gdvList.DataSource = getResource.ToList();
        gdvList.DataBind();
    }
    protected void gdvList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "cmdDownload":
                    PASSIS.LIB.ResourceCenter objResource = new PASSIS.LIB.ResourceCenter();
                    int id = Convert.ToInt32(e.CommandArgument);
                    objResource = context.ResourceCenters.First(x => x.Id == id);
                    if (objResource.DocumentType == (long)FileExtension.MicrosoftExcel)
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
                    else if (objResource.DocumentType == (long)FileExtension.PdfDocument)
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
                    else if (objResource.DocumentType == (long)FileExtension.MicrosoftWord)
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
            lblErrorMsg.Text = "Error occured, kindly contact the administrator";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
        }
    }

    public enum FileExtension
    {
        MicrosoftExcel = 1,
        PdfDocument = 2,
        MicrosoftWord = 3
    }

}