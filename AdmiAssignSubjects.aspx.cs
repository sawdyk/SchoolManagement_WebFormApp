using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using pdc = PASSIS.DAO.CustomClasses;
using PASSIS.LIB;
using PASSIS.LIB.Utility;

public partial class AdmiAssignSubjects : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();

    protected IList<pdc.SubjectTeachers> subjTeachers_VS
    {
        get
        {
            return ViewState["::subjTeachers_VS::"] as List<pdc.SubjectTeachers>;
        }
        set
        {
            ViewState["::subjTeachers_VS::"] = value;
        }
    }
    protected IList<Int64> gridGradeIds
    {
        get
        {
            //if (ViewState["::referrerUrl_VS::"] != null)
            return ViewState["::gridGradeIds::"] as List<Int64>;
        }
        set
        {
            ViewState["::gridGradeIds::"] = value;
        }
    }
    protected string getCampus()
    {
        return new AcademicSessionDAL().RetrieveSchoolCampus(logonUser.SchoolCampusId).Name;
    }
    protected string referrerUrl_VS
    {
        get
        {
            //if (ViewState["::referrerUrl_VS::"] != null)
            return ViewState["::referrerUrl_VS::"].ToString();
        }
        set
        {
            ViewState["::referrerUrl_VS::"] = value;
        }
    }
    protected Int64 itemId_VS
    {
        get
        {

            return Convert.ToInt64(ViewState["::itemId_VS::"]);
        }
        set
        {
            ViewState["::itemId_VS::"] = value;
        }
    }
    protected Int64 contextUserSchoolId_VS
    {
        get
        {

            return Convert.ToInt64(ViewState["::contextUserSchoolId_VS::"]);
        }
        set
        {
            ViewState["::contextUserSchoolId_VS::"] = value;
        }
    }
    protected Int64 contextUserSchoolCampusId_VS
    {
        get
        {

            return Convert.ToInt64(ViewState["::contextUserSchoolCampusId_VS::"]);
        }
        set
        {
            ViewState["::contextUserSchoolCampusId_VS::"] = value;
        }
    }
    protected string getUrl()
    {
        return referrerUrl_VS;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

          
            //Panel pnlAddSubjects = (Panel)fvwUser.FindControl("pnlAddSubjects");
            //if (logonUserRole.Id == (long)PASSIS.LIB.Utility.roles.teacher)
            //{
            //    pnlAddSubjects.Visible = false;
            //}
            //else { pnlAddSubjects.Visible = true; }
            string mode = Request.QueryString["Mode"].ToString();
            SetFormViewMode(fvwUser, mode);
            referrerUrl_VS = Request.UrlReferrer.ToString();
            BindGridYear();
            try
            {
                itemId_VS = Convert.ToInt64(Request.QueryString["id"]);
                PASSIS.DAO.User usr = new UsersDAL().RetrieveUser(itemId_VS);
                contextUserSchoolCampusId_VS = usr.SchoolCampusId;
                contextUserSchoolId_VS = (long)usr.SchoolId;

            }
            catch (Exception ex)
            {

            }
            switch (PASSIS.DAO.Utility.Util.getFormMode(mode))
            {
                case PASSIS.DAO.Utility.FormMode.Edit:
                    try
                    {
                        //DropDownList ddlUserStatus = (DropDownList)fvwUser.FindControl("ddlUserStatus");
                        //ddlUserStatus.DataSource = Enumeration.GetAll<UserStatus>();
                        //ddlUserStatus.DataBind();

                        //itemId_VS = Convert.ToInt64(Request.QueryString["id"]);
                        //check if id is 0, and do d needful
                    }
                    catch { }



                    break;
                case PASSIS.DAO.Utility.FormMode.Insert:
                    break;
                case PASSIS.DAO.Utility.FormMode.View:
                    //BindGrid();
                    try
                    {
                        //itemId_VS = Convert.ToInt64(Request.QueryString["id"]);
                        //check if id is 0, and do d needful
                    }
                    catch { }
                    BindGridFormViewReadOnlyFormView();

                    break;

            }
        }
    }
    public List<PASSIS.LIB.SchoolCampus> schCampus
    {
        get
        {
            var getSchCampus = from campus in context.SchoolCampus where campus.SchoolId == logonUser.SchoolId select campus;
            return getSchCampus.ToList<PASSIS.LIB.SchoolCampus>();
        }
    }
    public List<PASSIS.LIB.Role> schRole
    {
        get
        {
            RoleLIB rolLib = new RoleLIB();
            IList<PASSIS.LIB.Role> role = rolLib.getAllRolesSpecial();
            return role.ToList<PASSIS.LIB.Role>();
        }
    }
    protected IDictionary<int, string> UserStatusEnumeration
    {
        get
        {
            return Enumeration.GetAll<PASSIS.LIB.Utility.UserStatus>();
        }
    }
    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        Response.Redirect(getUrl());
    }
    //protected void btnUpdate_OnClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Label lblErrorMsg = (Label)fvwUser.FindControl("lblErrorMsg");
    //        TextBox txtUsername = (TextBox)fvwUser.FindControl("txtUsername");
    //        TextBox txtEmailAddress = (TextBox)fvwUser.FindControl("txtEmailAddress");
    //        TextBox txtPhoneNumber = (TextBox)fvwUser.FindControl("txtPhoneNumber");
    //        TextBox txtFirstName = (TextBox)fvwUser.FindControl("txtFirstName");
    //        TextBox txtLastName = (TextBox)fvwUser.FindControl("txtLastName");
    //        TextBox txtMiddleName = (TextBox)fvwUser.FindControl("txtMiddleName");
    //        DropDownList ddlUserStatus = (DropDownList)fvwUser.FindControl("ddlUserStatus");
    //        DropDownList ddlCampus = (DropDownList)fvwUser.FindControl("ddlCampus");
    //        DropDownList ddlRole = (DropDownList)fvwUser.FindControl("ddlRole");

    //        DropDownList ddlGender = (DropDownList)fvwUser.FindControl("ddlGender");
    //        string username, emailaddress, firstname, lastname, middlename;
    //        username = txtUsername.Text.Trim();
    //        emailaddress = txtEmailAddress.Text.Trim();

    //        if (string.IsNullOrEmpty(username))
    //        {
    //            lblErrorMsg.Text = string.Format("Username field cannot be null");
    //            lblErrorMsg.Visible = true;
    //            return;
    //        }
    //        //if (string.IsNullOrEmpty(emailaddress))
    //        //{
    //        //    lblErrorMsg.Text = string.Format("Email Address field cannot be null");
    //        //    lblErrorMsg.Visible = true;
    //        //    return;
    //        //}
    //        if (new UsersDAL().usernameExist(username, itemId_VS, (Int64)PASSIS.DAO.Utility.roles.teacher))
    //        {
    //            lblErrorMsg.Text = string.Format("Username already exist. Try a more unique username.");
    //            lblErrorMsg.Visible = true;
    //            return;
    //        }
    //        //if (new UsersDAL().RetrieveUserByMailAddress(emailaddress, itemId_VS) != null)
    //        //{
    //        //    lblErrorMsg.Text = string.Format("Email address already exist.");
    //        //    lblErrorMsg.Visible = true;
    //        //    return;
    //        //}

    //        PASSIS.DAO.User usr = new UsersDAL().RetrieveUser(itemId_VS);

    //        usr.EmailAddress = txtEmailAddress.Text.Trim();
    //        usr.PhoneNumber = txtPhoneNumber.Text.Trim();
    //        usr.FirstName = txtFirstName.Text.Trim();
    //        usr.LastLoginDate = DateTime.Now;
    //        usr.LastName = txtLastName.Text.Trim();
    //        usr.MiddleName = txtMiddleName.Text.Trim();
    //        usr.Username = username;
    //        DropDownList ddlGend = (DropDownList)fvwUser.FindControl("ddlGender");
    //        usr.Gender = Convert.ToInt32(ddlGend.SelectedValue);
    //        usr.UserStatus = Convert.ToInt32(ddlUserStatus.SelectedValue);
    //        usr.SchoolCampusId = Convert.ToInt64(ddlCampus.SelectedValue);
    //        new UsersDAL().UpdateUser(usr);


    //        PASSIS.LIB.UserRole userRole = context.UserRoles.FirstOrDefault(x => x.UserId == usr.Id);
    //        userRole.RoleId = Convert.ToInt64(ddlRole.SelectedValue);
    //        context.SubmitChanges();
    //        Response.Redirect(getUrl());

    //    }
    //    catch (Exception ex)
    //    {

    //    }

    //    Response.Redirect(getUrl());
    //}
    protected void populateSubjetsDropDownList(Int64 Id)
    {
        //DropDownList ddlSubjects = (DropDownList)fvwUser.FindControl("ddlSubjects");
        //ddlSubjects.Items.Clear();
        //ddlSubjects.Items.Add(new ListItem("-- Select --", "0", true));
        //ddlSubjects.DataSource = getSubjectDataSource();
        //ddlSubjects.DataBind();

        CheckBoxList ddlSubjects = (CheckBoxList)fvwUser.FindControl("ddlSubjects");
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        IList<PASSIS.LIB.Subject> getId = new SubjectTeachersLIB().getAllSubjectsInSchool(Id);
        ddlSubjects.Items.Clear();
        foreach (PASSIS.LIB.Subject subjId in getId)
        {
            PASSIS.LIB.Subject reqSubject = context.Subjects.FirstOrDefault(c => c.Id == subjId.Id);
            ddlSubjects.Items.Add(new ListItem(reqSubject.Name, reqSubject.Id.ToString()));
        }

    }
    protected void SetFormViewMode(FormView frmView, string mode)
    {
        switch (PASSIS.DAO.Utility.Util.getFormMode(mode))
        {
            case PASSIS.DAO.Utility.FormMode.Edit:
                frmView.ChangeMode(FormViewMode.Edit);
                break;
            case PASSIS.DAO.Utility.FormMode.Insert:
                frmView.ChangeMode(FormViewMode.Insert);
                break;
            case PASSIS.DAO.Utility.FormMode.View:
                frmView.ChangeMode(FormViewMode.ReadOnly);
                break;
        }
    }
    protected IList<PASSIS.LIB.Grade> getGradeDataSource()
    {
        IList<PASSIS.LIB.Grade> grades = new List<PASSIS.LIB.Grade>();


        return grades;
    }
    protected IList<PASSIS.DAO.Class_Grade> getYearDataSource()
    {
        IList<PASSIS.DAO.Class_Grade> years = new List<PASSIS.DAO.Class_Grade>();
        years = new PASSIS.DAO.SchoolConfigDAL().getAllClass_Grade();

        return years;
    }
    protected IList<PASSIS.DAO.SubjectInSchool> getSubjectDataSource()
    {
        IList<PASSIS.DAO.SubjectInSchool> subjects = new List<PASSIS.DAO.SubjectInSchool>();
        subjects = new SchoolConfigDAL().RetrieveSubjectsInSchool((long)new UsersDAL().RetrieveUser(itemId_VS).SchoolId);
        return subjects;
    }
    protected void BindGridFormViewReadOnlyFormView()
    {
        GridView gdvList = (GridView)fvwUser.FindControl("gdvList");

        gdvList.DataSource = new SubjectTeachersLIB().getAllTeacherSubject(itemId_VS);
        gdvList.DataBind();
    }
    public string RoleName(long UserId)
    {
        string roleName = "";
        PASSIS.LIB.UserRole userRole = context.UserRoles.FirstOrDefault(x => x.Id == UserId);
        if (userRole != null)
        {
            long? roleId = userRole.RoleId;
            PASSIS.LIB.Role role = context.Roles.FirstOrDefault(x => x.Id == roleId);
            if (role != null)
            {
                roleName = role.RoleName;
            }
        }
        return roleName;
    }
    protected void BindGrid()
    {
        GridView gdvList = (GridView)fvwUser.FindControl("gdvList");
        IList<PASSIS.LIB.SubjectTeacher> sbTchrs = new SubjectTeachersLIB().getAllTeacherSubject(itemId_VS);
        gdvList.DataSource = sbTchrs;
        gdvList.DataBind();
    }
    protected void btnAddSubject_OnClick(object sender, EventArgs e)
    {
        try
        {

            PASSISLIBDataContext context = new PASSISLIBDataContext();
            Label lblSubjectSelectionError = (Label)fvwUser.FindControl("lblSubjectSelectionError");
            lblSubjectSelectionError.Visible = false;
            //if (ddlTeachers.SelectedValue == "0")
            //{
            //    lblErrorMsg.Text = string.Format("Select a valid teacher. ");
            //    lblErrorMsg.Visible = true;
            //    return;
            //}
            PASSIS.LIB.User userInContext = new PASSIS.LIB.User();
            userInContext = new UsersLIB().RetrieveUser(itemId_VS);
            Int64 teacherId, schoolId, subjectId, yearId, gradeId;
            DropDownList ddlYear = (DropDownList)fvwUser.FindControl("ddlYear");
            DropDownList ddlGrade = (DropDownList)fvwUser.FindControl("ddlGrade");
            GridView gdvAllSubject = (GridView)fvwUser.FindControl("gdvAllSubject");
            DropDownList ddlSubjects = (DropDownList)fvwUser.FindControl("ddlSubjects");
            //check the dropdownlists
            //PASSIS.LIB.SubjectTeacher sbT = new PASSIS.LIB.SubjectTeacher();
            //sbT.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
            //sbT.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
            //sbT.SchoolId = (long)userInContext.SchoolId;
            long count = 0;
            long ifcheck = 0;
            foreach (GridViewRow row in gdvAllSubject.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    bool isChecked = row.Cells[2].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    if (isChecked)
                    {
                        PASSIS.LIB.SubjectTeacher sbT = new PASSIS.LIB.SubjectTeacher();
                        sbT.ClassId = Convert.ToInt64(ddlYear.SelectedValue);
                        sbT.GradeId = Convert.ToInt64(ddlGrade.SelectedValue);
                        sbT.SchoolId = (long)userInContext.SchoolId;
                        string lblId = row.Cells[4].Controls.OfType<Label>().FirstOrDefault().Text;
                        long gvSubjectId = Convert.ToInt64(lblId);
                        sbT.SubjectId = Convert.ToInt32(gvSubjectId);
                        sbT.TeacherId = (long)userInContext.Id;

                        if (checkIfSelectedTeacherSubjectExist((long)(sbT.ClassId), Convert.ToInt32(sbT.GradeId), Convert.ToInt32(sbT.SubjectId), (long)(sbT.TeacherId)))
                        {
                            lblSubjectSelectionError.Visible = true;
                            lblSubjectSelectionError.ForeColor = System.Drawing.Color.Red;
                            lblSubjectSelectionError.Text = "Your selection already exist for this teacher. Try another.";
                            return;
                        }


                        context.SubjectTeachers.InsertOnSubmit(sbT);
                        context.SubmitChanges();
                        ifcheck++;
                    }
                }
                count++;
            }


            lblSubjectSelectionError.Visible = true;
            lblSubjectSelectionError.ForeColor = System.Drawing.Color.Green;
            lblSubjectSelectionError.Text = "Your selection have been Added and Assigned Successfully";
            //sbT.SubjectId = Int32.Parse(ddlSubjects.SelectedValue);
            //sbT.TeacherId = (long)userInContext.Id;
            //new SubjectTeachersLIB().SaveSubjectTeacher(sbT);
            //bind grid, make changes in the ddl items 
            BindGrid();


        }
        catch (Exception ex)
        {
            throw ex;
        }


    }
    /// <summary>
    /// Dirty but quick solution: this should be moved away from presentation layer
    /// </summary>
    /// <param name="gradeId"></param>
    /// <param name="subjectId"></param>
    /// <param name="teacherId"></param>
    /// <returns></returns>
    protected bool checkIfSelectedTeacherSubjectExist(long ClassId, int GradeId, int subjectId, long teacherId)
    {
        bool existAlready = false;
        PASSIS.LIB.PASSISLIBDataContext cn = new PASSIS.LIB.PASSISLIBDataContext();
        PASSIS.LIB.SubjectTeacher sb = cn.SubjectTeachers.FirstOrDefault(s => s.ClassId == ClassId && s.GradeId == GradeId && s.SubjectId == subjectId && s.TeacherId == teacherId);
        existAlready = sb == null ? false : true;
        return existAlready;
    }
    protected void gdvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView gdvList = fvwUser.FindControl("gdvList") as GridView;
        gdvList.PageIndex = e.NewPageIndex;
        BindGrid();
    }

    protected void PopulateallSubject()
    {
        PASSISLIBDataContext db = new PASSISLIBDataContext();
        DropDownList ddlYear = (DropDownList)fvwUser.FindControl("ddlYear");
        GridView gdvAllSubject = (GridView)fvwUser.FindControl("gdvAllSubject");
        PASSIS.LIB.Class_Grade classgrade = db.Class_Grades.FirstOrDefault(x => x.Name == ddlYear.SelectedItem.Text);
        gdvAllSubject.DataSource = new TeacherLIB().GetAllSubjectInClass(Convert.ToInt32(classgrade.CurriculumId), classgrade.Id, Convert.ToInt64(logonUser.SchoolId));
        gdvAllSubject.DataBind();
        gdvAllSubject.Visible = true;
    }
    protected void ddlYear_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //get the
        Int64 selectedYearId = 0L;
        long selectedSubjId = 0L;
        DropDownList ddlYear = (DropDownList)fvwUser.FindControl("ddlYear");
        DropDownList ddlSubjects = (DropDownList)fvwUser.FindControl("ddlSubjects");
        DropDownList ddlGrade = (DropDownList)fvwUser.FindControl("ddlGrade");
        //Int64.TryParse(ddlSubjects.SelectedValue, out selectedSubjId);
        //var idsOfSubjectGradeOnGrid = (System.Collections.Generic.IEnumerable<long>)null;
        //if (subjTeachers_VS != null && selectedSubjId > 0)
        //    idsOfSubjectGradeOnGrid = from s in subjTeachers_VS where s.SubjectId == selectedSubjId select s.GradeId;
        //populate subject dropdown

        //selectedYearId = Convert.ToInt64(ddlYear.SelectedValue);
        //populateSubjetsDropDownList(selectedYearId);
        //ddlGrade.Items.Clear();
        IList<PASSIS.DAO.Grade> gradeList = new ClassGradeDAL().getAllGradesByYear(Convert.ToInt64(logonUser.SchoolId), logonUser.SchoolCampusId, Convert.ToInt64(ddlYear.SelectedValue));
        //var gradesLeft = from g in gradeList where !gridGradeIds.Contains(g.Id) select g;
        //var gradesLeft = (System.Collections.Generic.IEnumerable<PASSIS.DAO.Grade>)null;
        //try
        //{
        //    gradesLeft = from g in gradeList where !idsOfSubjectGradeOnGrid.ToList<Int64>().Contains(g.Id) select g;
        //    ddlGrade.DataSource = gradesLeft.ToList<PASSIS.DAO.Grade>();
        //}
        //catch
        //{
        //    gradesLeft = from g in gradeList select g;
        //    ddlGrade.DataSource = gradesLeft.ToList<PASSIS.DAO.Grade>();
        //}
        ddlGrade.DataSource = gradeList;
        ddlGrade.DataBind();

        PopulateallSubject();
        Label lblunAssigned = (Label)fvwUser.FindControl("lblunAssigned");
        lblunAssigned.Visible = true;

    }
    protected void gdvList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        // long rowItemId = long.Parse(e.ToString());
        Label lblSubjectSelectionError = (Label)fvwUser.FindControl("lblSubjectSelectionError");

        switch (e.CommandName)
        {
            case "Remove":
                //AvailableRole_VS.Add(retrieveSelectedRole(SelectedRoles_VS, rowItemId));
                //SelectedRoles_VS = SelectedRoles_VS.Where(r => r.Id != rowItemId).ToList<pdc.Role>();
                //BindGrid(SelectedRoles_VS);
                //PopulateDropDownList(AvailableRole_VS);
                //SubjectTeacher sb = new SubjectTeachersDAL().RetrieveSubjectTeacher(rowItemId);
                //sb.SubjectStatus = "0";
                //new SubjectTeachersDAL().UpdateSubjectTeacher(sb);
                new SubjectTeachersLIB().DeleteSubjectTeacher(Convert.ToInt64(e.CommandArgument.ToString()));
                lblSubjectSelectionError.Visible = true;
                lblSubjectSelectionError.ForeColor = System.Drawing.Color.Red;
                lblSubjectSelectionError.Text = "Subject Assigned Deleted Successfully";
                BindGrid();

                break;
        }



    }
    protected void PopulateDropDown()
    {
        //if (!IsPostBack)
        //{
        //    ddlSubjects.DataSource = new SchoolConfigDAL().RetrieveSubjectsInSchool((long)logonUser.SchoolId);
        //    ddlSubjects.DataBind();


        //}

        //ddlTeachers.DataSource = getAvailableTeachers();
        //ddlTeachers.DataBind();


    }
    protected void BindGridYear()
    {
        DropDownList ddlYear = (DropDownList)fvwUser.FindControl("ddlYear");
        long curriculumId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolCurriculumId(logonUser.SchoolId));
        long schoolTypeId = Convert.ToInt32(new PASSIS.LIB.SchoolLIB().GetSchoolTypeId(logonUser.SchoolId));
        ddlYear.DataSource = new PASSIS.LIB.SchoolLIB().getAllClass_Grade(curriculumId, schoolTypeId);
        ddlYear.DataBind();

    }


    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        GridView gdvAllSubject = (GridView)fvwUser.FindControl("gdvAllSubject");
        Label lblSubjectSelectionError = (Label)fvwUser.FindControl("lblSubjectSelectionError");

        try
        {
            CheckBox chk = (sender as CheckBox);
            if (chk.ID == "chkAll")
            {
                foreach (GridViewRow row in gdvAllSubject.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        row.Cells[2].Controls.OfType<CheckBox>().FirstOrDefault().Checked = chk.Checked;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
            lblSubjectSelectionError.Text = "Error occured, kindly contact your administrator";
            lblSubjectSelectionError.ForeColor = System.Drawing.Color.Red;
            lblSubjectSelectionError.Visible = true;
        }
    }

    protected void gdvAllSubject_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        Label lblSubjectSelectionError = (Label)fvwUser.FindControl("lblSubjectSelectionError");
        DropDownList ddlYear = (DropDownList)fvwUser.FindControl("ddlYear");

        switch (e.CommandName)
        {
            case "Remove":
                //AvailableRole_VS.Add(retrieveSelectedRole(SelectedRoles_VS, rowItemId));
                //SelectedRoles_VS = SelectedRoles_VS.Where(r => r.Id != rowItemId).ToList<pdc.Role>();
                //BindGrid(SelectedRoles_VS);
                //PopulateDropDownList(AvailableRole_VS);
                //SubjectTeacher sb = new SubjectTeachersDAL().RetrieveSubjectTeacher(rowItemId);
                //sb.SubjectStatus = "0";
                //new SubjectTeachersDAL().UpdateSubjectTeacher(sb);
                new SubjectTeachersLIB().DeleteSubjectInSchool(Convert.ToInt64(e.CommandArgument.ToString()), Convert.ToInt64(ddlYear.SelectedValue));
                lblSubjectSelectionError.Visible = true;
                lblSubjectSelectionError.ForeColor = System.Drawing.Color.Red;
                lblSubjectSelectionError.Text = "Deleted Successfully";
                //BindGrid();
                PopulateallSubject();
                break;
        }
    }
    protected void fvwUser_DataBound(object sender, EventArgs e)
    {
        DropDownList ddlCampus = (DropDownList)fvwUser.FindControl("ddlCampus");
        DropDownList ddlRole = (DropDownList)fvwUser.FindControl("ddlRole");
        Label lblCampus = (Label)fvwUser.FindControl("lblCampus");
        Label lblRole = (Label)fvwUser.FindControl("lblRole");

        //ddlCampus.Text = lblCampus.Text;
        //ddlRole.Text = lblRole.Text;
    }

    //public string RoleS 
    //{
    //    get
    //    {
    //        Label lblRole = (Label)fvwUser.FindControl("lblRole");
    //        return lblRole.Text;
    //    }
    //}
}