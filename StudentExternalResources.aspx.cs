using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.LIB;
using PASSIS.LIB.Utility;
using System.IO;

public partial class StudentExternalResources : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        BindgridLink();
        BindgridDoc();
        BindgridMultimedia();
    }
        

    private void BindgridLink()
    {
        Int64 userId = logonUser.Id;
        Int64 StudentGradeId = 0L;
        Int64 StudentClassId = 0L;

        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);
        long? curTermId = new AcademicSessionLIB().GetCurrentTermId(logonUser.SchoolId);


        try
        {
            StudentGradeId = new ClassGradeLIB().RetrieveStudentGrade(userId, curSessionId).GradeId;
            StudentClassId = new ClassGradeLIB().RetrieveStudentGrade(userId, curSessionId).ClassId;
        }
        catch { }

        var getResourceLink = from list in context.ExternalResourceLinks
                              where list.SchoolId == logonUser.SchoolId && list.CampusId == logonUser.SchoolCampusId && list.SessionId == Convert.ToInt64(curSessionId)
                              && list.TermId == Convert.ToInt64(curTermId) && list.ClassId == Convert.ToInt64(StudentClassId) && list.GradeId == Convert.ToInt64(StudentGradeId)
                              select list;
        gdvList.DataSource = getResourceLink.ToList();
        gdvList.DataBind();
    }

    private void BindgridDoc()
    {
         Int64 userId = logonUser.Id;
        Int64 StudentGradeId = 0L;
        Int64 StudentClassId = 0L;

        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);
        long? curTermId = new AcademicSessionLIB().GetCurrentTermId(logonUser.SchoolId);


        try
        {
            StudentGradeId = new ClassGradeLIB().RetrieveStudentGrade(userId, curSessionId).GradeId;
            StudentClassId = new ClassGradeLIB().RetrieveStudentGrade(userId, curSessionId).ClassId;
        }
        catch { }
        var getResourceDoc = from list in context.ExternalResourceDocuments
                             where list.SchoolId == logonUser.SchoolId && list.CampusId == logonUser.SchoolCampusId && list.SessionId == Convert.ToInt64(curSessionId)
                             && list.TermId == Convert.ToInt64(curTermId) && list.ClassId == Convert.ToInt64(StudentClassId) && list.GradeId == Convert.ToInt64(StudentGradeId)
                             select list;
        gdvListDoc.DataSource = getResourceDoc.ToList();
        gdvListDoc.DataBind();
    }

    private void BindgridMultimedia()
    {
        Int64 userId = logonUser.Id;
        Int64 StudentGradeId = 0L;
        Int64 StudentClassId = 0L;

        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);
        long? curTermId = new AcademicSessionLIB().GetCurrentTermId(logonUser.SchoolId);


        try
        {
            StudentGradeId = new ClassGradeLIB().RetrieveStudentGrade(userId, curSessionId).GradeId;
            StudentClassId = new ClassGradeLIB().RetrieveStudentGrade(userId, curSessionId).ClassId;
        }
        catch { }
        var getMultimedias = from list in context.ExternalResourceMultimedias
                             where list.SchoolId == logonUser.SchoolId && list.CampusId == logonUser.SchoolCampusId && list.SessionId == Convert.ToInt64(curSessionId)
                             && list.TermId == Convert.ToInt64(curTermId) && list.ClassId == Convert.ToInt64(StudentClassId) && list.GradeId == Convert.ToInt64(StudentGradeId)
                             select list;
        gdvMultimedia.DataSource = getMultimedias.ToList();
        gdvMultimedia.DataBind();
    }

    protected void gdvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvList.PageIndex = e.NewPageIndex;
        BindgridLink();
    }

    protected void gdvListDoc_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvListDoc.PageIndex = e.NewPageIndex;
        BindgridDoc();
    }

    protected void gdvMultimedia_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvMultimedia.PageIndex = e.NewPageIndex;
        BindgridMultimedia();
    }

    public string getClassName(long classId)
    {
        string className = string.Empty;

        PASSIS.LIB.Class_Grade clsName = context.Class_Grades.FirstOrDefault(c => c.Id == classId);
        if (clsName != null)
        {
            className = clsName.Name;
        }
        return className.ToString();
    }

    public string getGradeName(long gradeId)
    {
        string gradeName = string.Empty;

        PASSIS.LIB.Grade grdName = context.Grades.FirstOrDefault(g => g.Id == gradeId);
        if (grdName != null)
        {
            gradeName = grdName.GradeName;
        }
        return gradeName.ToString();
    }

    public string mediaType(long mediaTypeId)
    {
        string mediaTypeName = string.Empty;

        if (mediaTypeId == 1)
        {
            mediaTypeName = "Audio";
        }
        if (mediaTypeId == 2)
        {
            mediaTypeName = "Video";
        }
        return mediaTypeName.ToString();
    }

    protected void gdvListDoc_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "download")
        {
            string filename = e.CommandArgument.ToString();
            string path = MapPath("~/ExternalResource/" + filename);
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

    protected void gdvMultimedia_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "download")
        {
            string filename = e.CommandArgument.ToString();
            string path = MapPath("~/ExternalResourceMultimedia/" + filename);
            byte[] bts = System.IO.File.ReadAllBytes(path);
            Response.Clear();
            Response.ClearHeaders();
            Response.AddHeader("Content-Type", "octet");
            Response.AddHeader("Content-Length", bts.Length.ToString());
            Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
            Response.BinaryWrite(bts);
            Response.Flush();
            Response.End();
        }
    }


}