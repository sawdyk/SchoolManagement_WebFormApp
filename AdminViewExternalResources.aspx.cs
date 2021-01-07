using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.LIB;
using PASSIS.LIB.Utility;
using System.IO;

public partial class AdminViewExternalResources : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        //BindgridLink();
        //BindgridDoc();

        if (!IsPostBack)
        {

            ddlAcademicSession.DataSource = new AdminViewExternalResources().schSession().Distinct();
            ddlAcademicSession.DataTextField = "SessionName";
            ddlAcademicSession.DataValueField = "ID";
            ddlAcademicSession.DataBind();
            ddlAcademicSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Session--", "0", true));

            long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
            long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
            ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
            ddlYear.DataBind();
            ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Class--", "0", true));

            ddlAcademicTerm.DataSource = new AcademicTermLIB().RetrieveAcademicTerm();
            ddlAcademicTerm.DataTextField = "AcademicTermName";
            ddlAcademicTerm.DataValueField = "Id";
            ddlAcademicTerm.DataBind();
            ddlAcademicTerm.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Term--", "0", true));

            ddlResourceType.DataSource = new AdminViewExternalResources().resourceType();
            ddlResourceType.DataTextField = "Name";
            ddlResourceType.DataValueField = "Id";
            ddlResourceType.DataBind();
            ddlResourceType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Resource--", "0", true));
        }
    }

    public IList<AcademicSessionName> schSession()
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        var session = from c in context.AcademicSessions
                      where c.SchoolId == logonUser.SchoolId && c.IsCurrent == true
                      orderby c.IsCurrent descending
                      select c.AcademicSessionName;
        return session.ToList<AcademicSessionName>();
    }

    public IList<ExternalResourceType> resourceType()
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        var resourceTypes = from c in context.ExternalResourceTypes select c;
        return resourceTypes.ToList<ExternalResourceType>();
    }

    protected void ddlYear_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Int64 selectedYearId = 0L;
        selectedYearId = Convert.ToInt64(ddlYear.SelectedValue);
        ddlGrade.Items.Clear();
        //var availableGrades = new ClassGradeDAL().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, selectedYearId);
        var availableGrades = new ClassGradeLIB().getAllGradesByYear((long)logonUser.SchoolId, logonUser.SchoolCampusId, selectedYearId);
        ddlGrade.DataSource = availableGrades;
        ddlGrade.DataBind();
        ddlGrade.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Grade--", "0", true));
    }

    protected void btnView_OnClick(object sender, EventArgs e)
    {
        if (ddlAcademicSession.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select Session";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }

        if (ddlAcademicTerm.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select Term";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }

        if (ddlYear.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select Class";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }

        if (ddlGrade.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select Grade";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }
            
        if (ddlResourceType.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select Resource Type";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }


        if (ddlResourceType.SelectedIndex == 1) //if selected resource is link
        {
            BindgridLink();
        }

        if (ddlResourceType.SelectedIndex == 2) //if selected resource is Document
        {
            BindgridDoc();
        }

        if (ddlResourceType.SelectedIndex == 3) //if selected resource is Multimedia
        {
            BindgridMultimedia();
        }
        

    }

    private void BindgridLink()
    {
        var getResourceLink = from list in context.ExternalResourceLinks
                              where list.SchoolId == logonUser.SchoolId && list.CampusId == logonUser.SchoolCampusId && list.SessionId == Convert.ToInt64(ddlAcademicSession.SelectedValue)
                              && list.TermId == Convert.ToInt64(ddlAcademicTerm.SelectedValue) && list.ClassId == Convert.ToInt64(ddlYear.SelectedValue) && list.GradeId == Convert.ToInt64(ddlGrade.SelectedValue)
                              select list;
        gdvList.DataSource = getResourceLink.ToList();
        gdvList.DataBind();

        gdvList.Visible = true;
        lblList.Visible = true;
        pnlList.Visible = true;

        gdvListDoc.Visible = false;
        lblDoc.Visible = false;
        pnlDoc.Visible = false;

        gdvMultimedia.Visible = false;
        lblMultimedia.Visible = false;
        pnlMultimedia.Visible = false;
    }

    private void BindgridDoc()
    {
        var getResourceDoc = from list in context.ExternalResourceDocuments
                             where list.SchoolId == logonUser.SchoolId && list.CampusId == logonUser.SchoolCampusId && list.SessionId == Convert.ToInt64(ddlAcademicSession.SelectedValue)
                              && list.TermId == Convert.ToInt64(ddlAcademicTerm.SelectedValue) && list.ClassId == Convert.ToInt64(ddlYear.SelectedValue) && list.GradeId == Convert.ToInt64(ddlGrade.SelectedValue)
                             select list;
        gdvListDoc.DataSource = getResourceDoc.ToList();
        gdvListDoc.DataBind();

        gdvListDoc.Visible = true;
        lblDoc.Visible = true;
        pnlDoc.Visible = true;

        gdvList.Visible = false;
        lblList.Visible = false;
        pnlList.Visible = false;

        gdvMultimedia.Visible = false;
        lblMultimedia.Visible = false;
        pnlMultimedia.Visible = false;
    }

    private void BindgridMultimedia()
    {
        var getMultimedias = from list in context.ExternalResourceMultimedias
                             where list.SchoolId == logonUser.SchoolId && list.CampusId == logonUser.SchoolCampusId && list.SessionId == Convert.ToInt64(ddlAcademicSession.SelectedValue)
                              && list.TermId == Convert.ToInt64(ddlAcademicTerm.SelectedValue) && list.ClassId == Convert.ToInt64(ddlYear.SelectedValue) && list.GradeId == Convert.ToInt64(ddlGrade.SelectedValue)
                             select list;
        gdvMultimedia.DataSource = getMultimedias.ToList();
        gdvMultimedia.DataBind();

        gdvMultimedia.Visible = true;
        lblMultimedia.Visible = true;
        pnlMultimedia.Visible = true;

        gdvListDoc.Visible = false;
        lblDoc.Visible = false;
        pnlDoc.Visible = false;

        gdvList.Visible = false;
        lblList.Visible = false;
        pnlList.Visible = false;
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

    protected void gdvList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "remove")
        {
            Int64 Id = Convert.ToInt64(e.CommandArgument);
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            PASSIS.LIB.ExternalResourceLink link = new PASSIS.LIB.ExternalResourceLink();
            link = context.ExternalResourceLinks.FirstOrDefault(s => s.Id == Id);
            context.ExternalResourceLinks.DeleteOnSubmit(link);
            context.SubmitChanges();
            BindgridLink();
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "Resource Deleted successfully";
            lblErrorMsg.Visible = true;
            lblErrorMsg.ForeColor = System.Drawing.Color.Green;

        }
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
        if (e.CommandName == "remove")
        {
            Int64 Id = Convert.ToInt64(e.CommandArgument);
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            PASSIS.LIB.ExternalResourceDocument doc = new PASSIS.LIB.ExternalResourceDocument();
            doc = context.ExternalResourceDocuments.FirstOrDefault(s => s.Id == Id);
            context.ExternalResourceDocuments.DeleteOnSubmit(doc);
            context.SubmitChanges();

            string path = MapPath("~/ExternalResource/" + doc.DocumentName); //Code to delete the file from the folder
            if ((System.IO.File.Exists(path)))
            {
                System.IO.File.Delete(path);
            }

            BindgridDoc();
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "Resource Deleted successfully";
            lblErrorMsg.Visible = true;
            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
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

        if (e.CommandName == "remove")
        {
            Int64 Id = Convert.ToInt64(e.CommandArgument);
            PASSISLIBDataContext context = new PASSISLIBDataContext();
            PASSIS.LIB.ExternalResourceMultimedia mul = new PASSIS.LIB.ExternalResourceMultimedia();
            mul = context.ExternalResourceMultimedias.FirstOrDefault(s => s.Id == Id);
            context.ExternalResourceMultimedias.DeleteOnSubmit(mul);
            context.SubmitChanges();

            string path = MapPath("~/ExternalResourceMultimedia/" + mul.MediaName); //Code to delete the file from the folder
            if ((System.IO.File.Exists(path)))
            {
                System.IO.File.Delete(path);
            }

            BindgridMultimedia();
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = "Resource Deleted successfully";
            lblErrorMsg.Visible = true;
            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
        }
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
}