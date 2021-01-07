using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.LIB;
using PASSIS.LIB.Utility;
using System.IO;

public partial class AdminExternalResources : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    private static int maximumFileSize = 10485760;  //4MB 4194304 //10MB 10485760

    protected void Page_Load(object sender, EventArgs e)
    {
        BindgridLink();
        BindgridDoc();
        BindgridMultimedia();
        //this.containerDiv.InnerHtml = this.VideoAudio();

        if (!IsPostBack)
        {

            ddlAcademicSession.DataSource = new AdminExternalResources().schSession().Distinct();
            ddlAcademicSession.DataTextField = "SessionName";
            ddlAcademicSession.DataValueField = "ID";
            ddlAcademicSession.DataBind();
            ddlAcademicSession.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Session--", "0", true));

            long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
            long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
            ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
            ddlYear.DataBind();
            ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Class--", "0", true));
            ddlYear.Items.Insert(1, new System.Web.UI.WebControls.ListItem("All Classes", "1", true));
           
            ddlAcademicTerm.DataSource = new AcademicTermLIB().RetrieveAcademicTerm();
            ddlAcademicTerm.DataTextField = "AcademicTermName";
            ddlAcademicTerm.DataValueField = "Id";
            ddlAcademicTerm.DataBind();
            ddlAcademicTerm.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Term--", "0", true));

            ddlResourceType.DataSource = new AdminExternalResources().resourceType();
            ddlResourceType.DataTextField = "Name";
            ddlResourceType.DataValueField = "Id";
            ddlResourceType.DataBind();
            ddlResourceType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Resource--", "0", true));

            ddlMediaType.DataSource = new AdminExternalResources().mediaType();
            ddlMediaType.DataTextField = "Name";
            ddlMediaType.DataValueField = "Id";
            ddlMediaType.DataBind();
            ddlMediaType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Media Type--", "0", true));
        }
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

    protected void ddlResourceType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlResourceType.SelectedIndex == 1) //if link is selected
        {
            lblLink.Visible = true;
            txtLink.Visible = true;
            lblDocument.Visible = false;
            resourceDocument.Visible = false;
            lblMultimedia.Visible = false;
            lblMultimediaUpload.Visible = false;
            multimedia.Visible = false;
            ddlMediaType.Visible = false;

        }

        if (ddlResourceType.SelectedIndex == 2) //if document is selected
        {
            lblDocument.Visible = true;
            resourceDocument.Visible = true;
            lblLink.Visible = false;
            txtLink.Visible = false;
            lblMultimedia.Visible = false;
            lblMultimediaUpload.Visible = false;
            multimedia.Visible = false;
            ddlMediaType.Visible = false;

        }

        if (ddlResourceType.SelectedIndex == 3) //if document is selected
        {
            lblMultimedia.Visible = true;
            multimedia.Visible = true;
            ddlMediaType.Visible = true;
            lblMultimediaUpload.Visible = true;
            lblDocument.Visible = false;
            resourceDocument.Visible = false;
            lblLink.Visible = false;
            txtLink.Visible = false;
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

    public IList<ExternalResourceMediaType> mediaType()
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        var mediaType = from c in context.ExternalResourceMediaTypes select c;
        return mediaType.ToList<ExternalResourceMediaType>();
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
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

        if (ddlYear.SelectedIndex > 1) //if All Classes is not selected
        {
            if (ddlGrade.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select Grade";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }
        }

        if (ddlResourceType.SelectedIndex == 0)
        {
            lblErrorMsg.Text = "Kindly select Resource Type";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }

        if (txtDescription.Text == "")
        {
            lblErrorMsg.Text = "Kindly enter Resource description";
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            lblErrorMsg.Visible = true;
            return;
        }


        if (ddlResourceType.SelectedIndex == 1) //if selected resource type is Link
        {
            if (txtLink.Text == "")
            {
                lblErrorMsg.Text = "Kindly enter Link/Url";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }

            if (ddlYear.SelectedIndex == 1) //All Classes
            {

                var grades = from c in context.Grades
                             where c.SchoolId == logonUser.SchoolId && c.SchoolCampusId == logonUser.SchoolCampusId
                             select c;
                IList<PASSIS.LIB.Grade> allGrades = grades.ToList<PASSIS.LIB.Grade>();

                foreach (PASSIS.LIB.Grade grd in allGrades)
                {
                    ExternalResourceLink res = new ExternalResourceLink();
                    res.SchoolId = logonUser.SchoolId;
                    res.CampusId = logonUser.SchoolCampusId;
                    res.SessionId = Convert.ToInt64(ddlAcademicSession.SelectedValue);
                    res.TermId = Convert.ToInt64(ddlAcademicTerm.SelectedValue);
                    res.ClassId = grd.ClassId;
                    res.GradeId = grd.Id;
                    res.Link = txtLink.Text;
                    res.Description = txtDescription.Text;
                    res.dateCreated = DateTime.Now;

                    context.ExternalResourceLinks.InsertOnSubmit(res);
                    context.SubmitChanges();
                }

                lblErrorMsg.Text = "Resource Upload was Successful!";
                lblErrorMsg.Visible = true;
                lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                BindgridLink();

            }
            else
            {
                ExternalResourceLink res = new ExternalResourceLink();
                res.SchoolId = logonUser.SchoolId;
                res.CampusId = logonUser.SchoolCampusId;
                res.SessionId = Convert.ToInt64(ddlAcademicSession.SelectedValue);
                res.TermId = Convert.ToInt64(ddlAcademicTerm.SelectedValue);
                res.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
                res.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                res.Link = txtLink.Text;
                res.Description = txtDescription.Text;
                res.dateCreated = DateTime.Now;

                context.ExternalResourceLinks.InsertOnSubmit(res);
                context.SubmitChanges();

                lblErrorMsg.Text = "Resource Upload was Successful!";
                lblErrorMsg.Visible = true;
                lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                BindgridLink();

            }
        }

        if (ddlResourceType.SelectedIndex == 2) //if selected resource type is Document
        {

            if (ddlYear.SelectedIndex == 1) //All Classes
            {
                if (resourceDocument.HasFile)
                {
                   

                    

                    var grades = from c in context.Grades
                                 where c.SchoolId == logonUser.SchoolId && c.SchoolCampusId == logonUser.SchoolCampusId
                                 select c;
                    IList<PASSIS.LIB.Grade> allGrades = grades.ToList<PASSIS.LIB.Grade>();

                    foreach (PASSIS.LIB.Grade grd in allGrades)
                    {
                        string originalFileName = Path.GetFileName(resourceDocument.PostedFile.FileName);
                        string modifiedFileName = string.Format("{0}{1}", DateTime.Now.ToString("MMddHmmss"), originalFileName);
                        string fileExtensionLetter = Path.GetExtension(resourceDocument.PostedFile.FileName);
                        int fileSizeLetter = resourceDocument.PostedFile.ContentLength;
                        string fileLocation = Server.MapPath("~/ExternalResource/") + grd.ClassId.ToString() + grd.Id.ToString() + originalFileName;

                        //Check the file extension

                        if (fileExtensionLetter.ToLower().Equals(".jpg") || fileExtensionLetter.ToLower().Equals(".png") || fileExtensionLetter.ToLower().Equals(".gif") || fileExtensionLetter.ToLower().Equals(".jpeg") || fileExtensionLetter.ToLower().Equals(".tiff"))
                        {
                            lblErrorMsg.Text = "Invalid file format";
                            lblErrorMsg.Visible = true;
                            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                            return;
                        }

                        if (fileSizeLetter > maximumFileSize)
                        {
                            lblErrorMsg.Text = "10MB file size exceeded for attached file";
                            lblErrorMsg.Visible = true;
                            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                            return;
                        }

                        ExternalResourceDocument res = new ExternalResourceDocument();
                        res.SchoolId = logonUser.SchoolId;
                        res.CampusId = logonUser.SchoolCampusId;
                        res.SessionId = Convert.ToInt64(ddlAcademicSession.SelectedValue);
                        res.TermId = Convert.ToInt64(ddlAcademicTerm.SelectedValue);
                        res.ClassId = grd.ClassId;
                        res.GradeId = grd.Id;
                        res.DocumentName = grd.ClassId.ToString() + grd.Id.ToString() + originalFileName;
                        res.Description = txtDescription.Text;
                        res.dateCreated = DateTime.Now;

                        context.ExternalResourceDocuments.InsertOnSubmit(res);
                        context.SubmitChanges();
                        resourceDocument.SaveAs(fileLocation);

                    }

                    lblErrorMsg.Text = "Resource Upload was Successful!";
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                    BindgridDoc();
                }

                else
                {
                    lblErrorMsg.Text = "Kindly select the Document";
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }
            }
            else
            {
                if (resourceDocument.HasFile)
                {
                    long classId = Convert.ToInt64(ddlYear.SelectedValue);
                    long gradeId = Convert.ToInt64(ddlGrade.SelectedValue);

                    string originalFileName = Path.GetFileName(resourceDocument.PostedFile.FileName);
                    string modifiedFileName = string.Format("{0}{1}", DateTime.Now.ToString("MMddHmmss"), originalFileName);
                    string fileExtensionLetter = Path.GetExtension(resourceDocument.PostedFile.FileName);
                    int fileSizeLetter = resourceDocument.PostedFile.ContentLength;
                    string fileLocation = Server.MapPath("~/ExternalResource/") + classId.ToString() + gradeId.ToString() + originalFileName;

                    //Check the file extension

                    if (fileExtensionLetter.ToLower().Equals(".jpg") && fileExtensionLetter.ToLower().Equals(".png") && fileExtensionLetter.ToLower().Equals(".gif") && fileExtensionLetter.ToLower().Equals(".jpeg") && fileExtensionLetter.ToLower().Equals(".tiff"))
                    {
                        lblErrorMsg.Text = "Invalid file format";
                        lblErrorMsg.Visible = true;
                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                        return;
                    }

                    if (fileSizeLetter > maximumFileSize)
                    {
                        lblErrorMsg.Text = "10MB file size exceeded for attached file";
                        lblErrorMsg.Visible = true;
                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    ExternalResourceDocument res = new ExternalResourceDocument();
                    res.SchoolId = logonUser.SchoolId;
                    res.CampusId = logonUser.SchoolCampusId;
                    res.SessionId = Convert.ToInt64(ddlAcademicSession.SelectedValue);
                    res.TermId = Convert.ToInt64(ddlAcademicTerm.SelectedValue);
                    res.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
                    res.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                    res.DocumentName = classId.ToString() + gradeId.ToString() + originalFileName;
                    res.Description = txtDescription.Text;
                    res.dateCreated = DateTime.Now;

                    context.ExternalResourceDocuments.InsertOnSubmit(res);
                    context.SubmitChanges();

                    resourceDocument.SaveAs(fileLocation);
                    lblErrorMsg.Text = "Resource Upload was Successful!";
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                    BindgridDoc();

                }
                else
                {
                    lblErrorMsg.Text = "Kindly select the Document";
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }
            }
        }




        if (ddlResourceType.SelectedIndex == 3) //if selected resource type is Multimedia
        {
            if (ddlMediaType.SelectedIndex == 0)
            {
                lblErrorMsg.Text = "Kindly select Media Type";
                lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                lblErrorMsg.Visible = true;
                return;
            }

            if (ddlYear.SelectedIndex == 1) //All Classes
            {
                if (multimedia.HasFile)
                {
                    
                    //if (fileSizeLetter > maximumFileSize)
                    //{
                    //    lblErrorMsg.Text = "4MB file size exceeded for attached documentation";
                    //    lblErrorMsg.Visible = true;
                    //    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    //    return;
                    //}

                    var grades = from c in context.Grades
                                 where c.SchoolId == logonUser.SchoolId && c.SchoolCampusId == logonUser.SchoolCampusId
                                 select c;
                    IList<PASSIS.LIB.Grade> allGrades = grades.ToList<PASSIS.LIB.Grade>();

                    foreach (PASSIS.LIB.Grade grd in allGrades)
                    {
                        string originalFileName = Path.GetFileName(multimedia.PostedFile.FileName);
                        string modifiedFileName = string.Format("{0}{1}", DateTime.Now.ToString("MMddHmmss"), originalFileName);
                        string fileExtensionLetter = Path.GetExtension(multimedia.PostedFile.FileName);
                        int fileSizeLetter = multimedia.PostedFile.ContentLength;
                        string fileLocation = Server.MapPath("~/ExternalResourceMultimedia/") + grd.ClassId.ToString() + grd.Id.ToString() + originalFileName;

                        //Check the file extension

                        if (fileExtensionLetter.ToLower().Equals(".pdf") || fileExtensionLetter.ToLower().Equals(".doc") || fileExtensionLetter.ToLower().Equals(".docx") || fileExtensionLetter.ToLower().Equals(".jpg") || fileExtensionLetter.ToLower().Equals(".png") || fileExtensionLetter.ToLower().Equals(".gif") || fileExtensionLetter.ToLower().Equals(".jpeg") || fileExtensionLetter.ToLower().Equals(".tiff"))
                        {
                            lblErrorMsg.Text = "Invalid file format";
                            lblErrorMsg.Visible = true;
                            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                            return;
                        }

                        if (fileSizeLetter > maximumFileSize)
                        {
                            lblErrorMsg.Text = "10MB file size exceeded for attached file";
                            lblErrorMsg.Visible = true;
                            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                            return;
                        }

                        ExternalResourceMultimedia res = new ExternalResourceMultimedia();
                        res.SchoolId = logonUser.SchoolId;
                        res.CampusId = logonUser.SchoolCampusId;
                        res.SessionId = Convert.ToInt64(ddlAcademicSession.SelectedValue);
                        res.TermId = Convert.ToInt64(ddlAcademicTerm.SelectedValue);
                        res.ClassId = grd.ClassId;
                        res.GradeId = grd.Id;
                        res.MediaTypeId = Convert.ToInt64(ddlMediaType.SelectedValue);
                        res.MediaName = grd.ClassId.ToString() + grd.Id.ToString() + originalFileName;
                        res.Description = txtDescription.Text;
                        res.dateCreated = DateTime.Now;

                        context.ExternalResourceMultimedias.InsertOnSubmit(res);
                        context.SubmitChanges();
                        multimedia.SaveAs(fileLocation);

                    }

                    lblErrorMsg.Text = "Resource Upload was Successful!";
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                    BindgridMultimedia();
                }

                else
                {
                    lblErrorMsg.Text = "Kindly select the File";
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }
            }
            else
            {
                if (multimedia.HasFile)
                {
                    long classId = Convert.ToInt64(ddlYear.SelectedValue);
                    long gradeId = Convert.ToInt64(ddlGrade.SelectedValue);

                    string originalFileName = Path.GetFileName(multimedia.PostedFile.FileName);
                    string modifiedFileName = string.Format("{0}{1}", DateTime.Now.ToString("MMddHmmss"), originalFileName);
                    string fileExtensionLetter = Path.GetExtension(multimedia.PostedFile.FileName);
                    int fileSizeLetter = multimedia.PostedFile.ContentLength;
                    string fileLocation = Server.MapPath("~/ExternalResourceMultimedia/") + classId.ToString() + gradeId.ToString() + originalFileName;

                    //Check the file extension

                    if (fileExtensionLetter.ToLower().Equals(".pdf") || fileExtensionLetter.ToLower().Equals(".doc") || fileExtensionLetter.ToLower().Equals(".docx") || fileExtensionLetter.ToLower().Equals(".jpg") || fileExtensionLetter.ToLower().Equals(".png") || fileExtensionLetter.ToLower().Equals(".gif") || fileExtensionLetter.ToLower().Equals(".jpeg") || fileExtensionLetter.ToLower().Equals(".tiff"))
                    {
                        lblErrorMsg.Text = "Invalid file format";
                        lblErrorMsg.Visible = true;
                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                        return;
                    }

                    if (fileSizeLetter > maximumFileSize)
                    {
                        lblErrorMsg.Text = "10MB file size exceeded for attached file";
                        lblErrorMsg.Visible = true;
                        lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                        return;
                    }

                    ExternalResourceMultimedia res = new ExternalResourceMultimedia();
                    res.SchoolId = logonUser.SchoolId;
                    res.CampusId = logonUser.SchoolCampusId;
                    res.SessionId = Convert.ToInt64(ddlAcademicSession.SelectedValue);
                    res.TermId = Convert.ToInt64(ddlAcademicTerm.SelectedValue);
                    res.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
                    res.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                    res.MediaTypeId = Convert.ToInt64(ddlMediaType.SelectedValue);
                    res.MediaName = classId.ToString() + gradeId.ToString() + originalFileName;
                    res.Description = txtDescription.Text;
                    res.dateCreated = DateTime.Now;

                    context.ExternalResourceMultimedias.InsertOnSubmit(res);
                    context.SubmitChanges();

                    multimedia.SaveAs(fileLocation);
                    lblErrorMsg.Text = "Resource Upload was Successful!";
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.ForeColor = System.Drawing.Color.Green;
                    BindgridMultimedia();

                }
                else
                {
                    lblErrorMsg.Text = "Kindly select the File";
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }
            }
        }






    }


    private void BindgridLink()
    {
        var getResourceLink = from list in context.ExternalResourceLinks
                          where list.SchoolId == logonUser.SchoolId && list.CampusId == logonUser.SchoolCampusId
                          select list;
        gdvList.DataSource = getResourceLink.ToList();
        gdvList.DataBind();
    }

    private void BindgridDoc()
    {
        var getResourceDoc = from list in context.ExternalResourceDocuments
                              where list.SchoolId == logonUser.SchoolId && list.CampusId == logonUser.SchoolCampusId
                              select list;
        gdvListDoc.DataSource = getResourceDoc.ToList();
        gdvListDoc.DataBind();
    }

    private void BindgridMultimedia()
    {
        var getMultimedias = from list in context.ExternalResourceMultimedias
                              where list.SchoolId == logonUser.SchoolId && list.CampusId == logonUser.SchoolCampusId
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
        
        PASSIS.LIB.Class_Grade clsName = context.Class_Grades.FirstOrDefault(c=>c.Id == classId);
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

//    public string VideoAudio()
//    {
       

////        string ret = @"<video width='320' height='240' controls>
////  <source src='movie.mp4' type='video/mp4'>
////</video>
////</br>
////<video width='320' height='240' controls>
////  <source src='moviiie.mp4' type='video/mp4'>
////</video>";

//        string ret = @"<audio controls>
//  <source src='Wasiu-Ayinde-Message.mp3' type='audio/mp3'>
//</audio>
//<br/>
//<video width='320' height='240' controls>
//<source src='movie.mp4' type='video/mp4'>
//</video>
//";


//        return ret;
//    }

}