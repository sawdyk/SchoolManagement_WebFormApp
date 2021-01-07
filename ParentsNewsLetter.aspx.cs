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


public partial class ParentsNewsLetter : PASSIS.LIB.Utility.BasePage
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
        long? termId = new PASSIS.LIB.AcademicSessionLIB().GetCurrentTermId(logonUser.SchoolId);
        long sessionId = new PASSIS.LIB.AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);
        var newsLetter = from s in context.NewsLetters
                         where s.TermId == termId && s.SessionId == sessionId
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
        
    }
  
}