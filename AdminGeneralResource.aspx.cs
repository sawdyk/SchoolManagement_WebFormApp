using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using System.Data.SqlClient;
using PASSIS.LIB;

public partial class AdminGeneralResource : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        Bindgrid();
    }

    private void Bindgrid()
    {
        var getResource = from list in context.GeneralResourceCenters
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