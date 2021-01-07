using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using PASSIS.DAO;
using System.Data;
using System.Data.OleDb;
using System.IO;
using PASSIS.LIB;
using System.Text.RegularExpressions;
using System.Text;


public partial class StudentBulkUpload : PASSIS.LIB.Utility.BasePage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    PASSIS.LIB.Utility.BasePage log = new PASSIS.LIB.Utility.BasePage();
    PASSISLIBDataContext db = new PASSISLIBDataContext();
    PASSIS.LIB.ParentDetail parentDetail = new PASSIS.LIB.ParentDetail();
    PASSIS.LIB.User ExistingUsrs = new PASSIS.LIB.User();
    string schoolCode = "";
    int skipDataWithissue = 0;
    string skippedReason = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //lblResult.Visible = false;
            PASSIS.LIB.User currentUser = logonUser;
            ddlCampus.DataSource = new AcademicSessionLIB().GetAllSchoolCampus((long)currentUser.SchoolId);
            ddlCampus.DataBind();
            ddlCampus.SelectedValue = currentUser.SchoolCampusId.ToString();

            //ddlCampus.Enabled = false;
            //bp.Visible = false;

            long schoolId = (long)logonUser.SchoolId;
            PASSIS.LIB.SchoolLIB schLib = new PASSIS.LIB.SchoolLIB();
            schoolCode = schLib.RetrieveSchool(schoolId).Code;
        }

    }

    protected void btnBulkUpload_OnClick(object sender, EventArgs e)
    {
        //validate the value and save
        //impose restriction on file sixe 
        //try
        //{

        if (bulkUploadFile.HasFile)
        {
            string originalFileName = Path.GetFileName(bulkUploadFile.PostedFile.FileName);
            string modifiedFileName = string.Format("{0}{1}", DateTime.Now.ToString("MMddHmmss"), originalFileName);
            string fileExtension = Path.GetExtension(bulkUploadFile.PostedFile.FileName);
            string fileLocation = Server.MapPath("~/docs/") + modifiedFileName;


            //Check whether file extension is xls or xslx

            if (!fileExtension.Contains(".xls"))
            {
                lblErrorMsg.Text = string.Format("Upload not successful. The file format is not supported!!!");
                lblErrorMsg.Visible = true;
                return;
            }

            bulkUploadFile.SaveAs(fileLocation);

            IList<PASSIS.LIB.User> users = RetrieveStudentsFromExcelFile(fileLocation, (long)logonUser.SchoolId, logonUser.SchoolCampusId);
            string log = ProcessRetrievedStudentInfo(users, (long)logonUser.SchoolId, logonUser.SchoolCampusId, PASSIS.LIB.Utility.BasePage.log, schoolCode.ToString());
            migrateParents(PASSIS.LIB.Utility.BasePage.log, users);
            lblResult.Text = log;
            lblResult.ForeColor = System.Drawing.Color.Green;

        }

        if (bulkUploadFile.HasFile == false)
        {
            lblErrorMsg.Text = string.Format("Please specify the file to be uploaded.");
            lblErrorMsg.Visible = true;
            lblErrorMsg.ForeColor = System.Drawing.Color.Red;
            return;
        }

        else if (Page.ClientScript.IsStartupScriptRegistered("Duplicate"))
        {
            DuplicateReturn();
        }

        else if (PASSIS.LIB.Utility.BasePage.log.Logger.Equals("0 Students have been created."))
        {
            lblErrorMsg.Text = string.Format("Duplicate Found.");
            lblErrorMsg.Visible = true;
            lblErrorMsg.ForeColor = System.Drawing.Color.Green;
        }

        else

        //Response.Redirect("~/Students.aspx");
        {
            lblResult.Visible = true;
            lblErrorMsg.Text = string.Format("Uploaded Successfully.");
            lblErrorMsg.Visible = true;
            lblErrorMsg.ForeColor = System.Drawing.Color.Green;

        }

        lblResult.Visible = true;
        lblErrorMsg.Text = string.Format("Uploaded Successfully.");
        lblErrorMsg.Visible = true;
        lblErrorMsg.ForeColor = System.Drawing.Color.Green;




        //}
        //catch (Exception ex)
        //{
        //    PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
        //    lblErrorMsg.Text = string.Format("Error occur, kindly contact your administrator.");
        //    lblErrorMsg.Visible = true;
        //}
    }
    public IList<PASSIS.LIB.User> RetrieveStudentsFromExcelFile(String filePath, long schlId, long campusId)
    {
        DataTable newTable = createUnsuccessfulDataTableTemplate();
        DataRow newRow = null;
        DataTable newSuccessTable = createsuccessfulDataTableTemplate();
        DataRow newSuccessRow = null;
        IList<PASSIS.LIB.User> studentsFound = new List<PASSIS.LIB.User>();
        // IList<PASSIS.LIB.User> DuplicateStudent = new List<PASSIS.LIB.User>();
        int count = 0;

        Net.SourceForge.Koogra.Excel.Workbook wkbk = new Net.SourceForge.Koogra.Excel.Workbook(filePath);
        Net.SourceForge.Koogra.Excel.Worksheet workSheet = wkbk.Sheets[0];

        try
        {
            for (uint rowCount = workSheet.Rows.MinRow; rowCount <= workSheet.Rows.MaxRow; ++rowCount)
            {
                //Net.SourceForge.Koogra.Excel2007.Row row = workSheet.GetRow(rowCount);
                //try
                //{
                if (rowCount != workSheet.Rows.MinRow) //jump the first row
                {
                    count++;
                    Net.SourceForge.Koogra.Excel.Row row = workSheet.Rows[rowCount];


                    PASSIS.LIB.School schObj = context.Schools.FirstOrDefault(x => x.Id == log.logonUser.SchoolId);

                    var getParentIFExist = (from getParentExist in db.Users
                                            where getParentExist.Username == schObj.Code + row.Cells[18].Value.ToString().Trim()
                                            //  && ExistingUsrs.LastName == row.Cells[1].Value.ToString() && ExistingUsrs.FirstName == row.Cells[2].Value.ToString()
                                            select getParentExist);
                    var getStudentIfExist = (from getStudentExist in context.Users
                                             where getStudentExist.LastName == row.Cells[1].Value.ToString()
                                             && getStudentExist.FirstName == row.Cells[2].Value.ToString()
                                             && getStudentExist.SchoolId == log.logonUser.SchoolId

                                             select getStudentExist);

                    //if(getParentIFExist != null && getStudentIfExist != null)

                    if (getStudentIfExist.ToList().Count > 0)
                    {
                        foreach (PASSIS.LIB.User user in getStudentIfExist)
                        {

                            skipDataWithissue = 1;
                            skippedReason = "This student is existing in your school";

                            newRow = newTable.NewRow();
                            newRow["New Name"] = row.Cells[1].Value.ToString() + " " + row.Cells[2].Value.ToString();
                            newRow["Existing Name(s)"] = user.LastName + " " + user.FirstName;
                            newRow["Reason"] = skippedReason;
                            newTable.Rows.Add(newRow);

                            StudentNameDuplicateException objDupStd = context.StudentNameDuplicateExceptions.FirstOrDefault(x => x.NewStudent == row.Cells[1].Value.ToString() + " " + row.Cells[2].Value.ToString()
                                && x.ExistingStudentId == user.Id
                                && x.SchoolId == log.logonUser.SchoolId
                                && x.CampusId == log.logonUser.SchoolCampusId);
                            if (objDupStd == null)
                            {
                                StudentNameDuplicateException objDup = new StudentNameDuplicateException();
                                objDup.NewStudent = row.Cells[1].Value.ToString() + " " + row.Cells[2].Value.ToString();
                                objDup.ExistingStudentId = user.Id;
                                objDup.SchoolId = log.logonUser.SchoolId;
                                objDup.CampusId = log.logonUser.SchoolCampusId;
                                objDup.Date = DateTime.Now;
                                context.StudentNameDuplicateExceptions.InsertOnSubmit(objDup);
                                context.SubmitChanges();
                            }
                            else
                            {
                                objDupStd.NewStudent = row.Cells[1].Value.ToString() + " " + row.Cells[2].Value.ToString();
                                objDupStd.ExistingStudentId = user.Id;
                                objDupStd.SchoolId = log.logonUser.SchoolId;
                                objDupStd.CampusId = log.logonUser.SchoolCampusId;
                                objDupStd.Date = DateTime.Now;
                                context.SubmitChanges();
                            }
                        }
                    }



                    else
                    {
                        PASSIS.LIB.User stdnt = new PASSIS.LIB.User();
                        //Net.SourceForge.Koogra.Excel2007.Row row = workSheet.GetRow(rowCount);
                        try { stdnt.LastName = row.Cells[1].Value.ToString(); }
                        catch { stdnt.LastName = null; }
                        stdnt.Password = "password";
                        try { stdnt.FirstName = row.Cells[2].Value.ToString(); }
                        catch { stdnt.FirstName = null; }
                        try { stdnt.MiddleName = row.Cells[3].Value.ToString(); }
                        catch { stdnt.MiddleName = null; }
                        try { stdnt.Gender = (int)stringToGenderEnum(row.Cells[4].Value.ToString()); }
                        catch { stdnt.Gender = 1; }
                        stdnt.SchoolId = schlId;
                        stdnt.SchoolCampusId = campusId;
                        try { stdnt.BusRoute = row.Cells[5].Value.ToString(); }
                        catch { stdnt.BusRoute = null; }
                        try { stdnt.Status = Convert.ToInt64(row.Cells[6].Value.ToString()); }
                        catch { stdnt.Status = null; }

                        try { stdnt.AccomodationType = PASSIS.LIB.Utility.Utili.getAccommodationType(row.Cells[7].Value.ToString()); }
                        catch { stdnt.AccomodationType = "Day Student"; }
                        try { Double d = Double.Parse(row.Cells[8].Value.ToString()); stdnt.DateOfBirth = DateTime.FromOADate(d); }
                        catch { stdnt.DateOfBirth = DateTime.Now; }
                        try { stdnt.YearofAdmission = row.Cells[9].Value.ToString(); }
                        catch { stdnt.YearofAdmission = null; }
                        try { stdnt.StateofOrigin = row.Cells[10].Value.ToString(); }
                        catch { stdnt.StateofOrigin = null; }
                        try { stdnt.LocalGovt = row.Cells[11].Value.ToString(); }
                        catch { stdnt.LocalGovt = null; }
                        try { stdnt.Religion = row.Cells[12].Value.ToString(); }
                        catch { stdnt.Religion = null; }
                        try { stdnt.StreetAddress = row.Cells[13].Value.ToString(); }
                        catch { stdnt.StreetAddress = null; }
                        try { stdnt.City = row.Cells[14].Value.ToString(); }
                        catch { stdnt.City = null; }
                        try { stdnt.State = row.Cells[15].Value.ToString(); }
                        catch { stdnt.State = null; }
                        try { stdnt.FathersTitle = row.Cells[16].Value.ToString(); }
                        catch { stdnt.FathersTitle = null; }
                        try { stdnt.FathersName = row.Cells[17].Value.ToString(); }
                        catch { stdnt.FathersName = null; }
                        try { stdnt.FathersPhoneNumber = row.Cells[18].Value.ToString(); }
                        catch { stdnt.FathersPhoneNumber = null; }
                        try { stdnt.FathersWorkAddress = row.Cells[19].Value.ToString(); }
                        catch { stdnt.FathersWorkAddress = null; }
                        try { stdnt.FathersEmail = row.Cells[20].Value.ToString(); }
                        catch { stdnt.FathersEmail = null; }
                        try { stdnt.FathersNationality = row.Cells[21].Value.ToString(); }
                        catch { stdnt.FathersNationality = null; }
                        try { stdnt.MothersName = row.Cells[22].Value.ToString(); }
                        catch { stdnt.MothersName = null; }
                        try { stdnt.MothersWorkAddress = row.Cells[23].Value.ToString(); }
                        catch { stdnt.MothersWorkAddress = null; }
                        try { stdnt.MothersPhoneNumber = row.Cells[24].Value.ToString(); }
                        catch { stdnt.MothersPhoneNumber = null; }
                        try { stdnt.MothersEmail = row.Cells[25].Value.ToString(); }
                        catch { stdnt.MothersEmail = null; }
                        try { stdnt.MothersNationality = row.Cells[26].Value.ToString(); }
                        catch { stdnt.MothersNationality = null; }
                        try { stdnt.GuardianDetails = row.Cells[27].Value.ToString(); }
                        catch { stdnt.GuardianDetails = null; }
                        try { stdnt.GuardianPhoneNumber = row.Cells[28].Value.ToString(); }
                        catch { stdnt.GuardianPhoneNumber = null; }
                        try { stdnt.GuardianEmail = row.Cells[29].Value.ToString(); }
                        catch { stdnt.GuardianEmail = null; }
                        try { stdnt.GuardianRelationship = row.Cells[30].Value.ToString(); }
                        catch { stdnt.GuardianRelationship = null; }
                        stdnt.DateCreated = stdnt.LastLoginDate = DateTime.Now;
                        stdnt.StudentFullName = stdnt.FirstName + " " + stdnt.MiddleName + " " + stdnt.LastName;
                        stdnt.UserStatus = (Int32)UserStatus.Active;
                        stdnt.StudentStatus = true;
                        stdnt.IsLearningSupport = 0;//not learning support
                        studentsFound.Add(stdnt);

                        newSuccessRow = newSuccessTable.NewRow();
                        newSuccessRow["New Name"] = row.Cells[1].Value.ToString() + " " + row.Cells[2].Value.ToString();
                        newSuccessRow["Existing Name(s)"] = "";
                        newSuccessRow["Reason"] = "Succesful";
                        newSuccessTable.Rows.Add(newSuccessRow);

                    }
                }
                //}
                //catch (Exception ex) { throw ex; }

                GridView2.DataSource = newTable;
                GridView2.DataBind();
                GridView1.DataSource = newSuccessTable;
                GridView1.DataBind();
            }

        }
        catch (Exception ex)
        {
            PassisUtility.LogErrorMessage(Request.Url.AbsoluteUri, ex.Message, ex.StackTrace);
            lblErrorMsg.Text = string.Format("Error occur, kindly contact your administrator.");
            lblErrorMsg.Visible = true;
        }
        return studentsFound;
    }
    protected static string RemoveSpecialCharacters(string str)
    {
        return Regex.Replace(str, "[^a-zA-Z0-9%._]", string.Empty);

    }
    public static string ProcessRetrievedStudentInfo(IList<PASSIS.LIB.User> userList, Int64 schlId, Int64 campsId, log4net.ILog logger, string schoolCode)
    {
        //variables to change 
        string campusCode = new AcademicSessionLIB().RetrieveSchoolCampus(campsId).Code;
        Int64 userIdTemp = 1; // class grade usersiD meant for admin

        StringBuilder log = new StringBuilder();
        log.Append("Total numbers of Students found = ").Append(userList.Count).AppendLine();
        UsersLIB usrDAL = new UsersLIB();
        ClassGradeLIB clsGrdDal = new ClassGradeLIB();
        //matric number processing not yet done 
        logger.InfoFormat("about starting.....{0} users to be processed", userList.Count);
        Int32 studentsCreated = 0;
        foreach (PASSIS.LIB.User usr in userList)
        {  //save the users info
            try
            {
                PASSISLIBDataContext context = new PASSISLIBDataContext();
                PASSIS.LIB.Utility.BasePage logg = new PASSIS.LIB.Utility.BasePage();
                PASSIS.LIB.School schObj = context.Schools.FirstOrDefault(x => x.Id == logg.logonUser.SchoolId);

                usr.AdmissionNumber = usr.Username = schObj.Code + PASSIS.LIB.Utility.Utili.GenerateAdmissionNumber(schoolCode, Convert.ToInt64(schlId));
                new UsersLIB().SaveUser(usr);
                //save student role
                PASSIS.LIB.UserRole usrRol = new PASSIS.LIB.UserRole();
                usrRol.RoleId = (long)PASSIS.LIB.Utility.roles.student; //student
                usrRol.UserId = usr.Id;
                new UsersLIB().SaveUserRole(usrRol);
                ++studentsCreated;


            }
            catch (Exception ex)
            {
                //logger.InfoFormat("Exception occurrred while processing this  {0} {1} {2}  {3}  {4}   eXCEPTION ::::{5}", usr.FirstName, usr.LastName, usr.GradeString, usr.HomeRoomTutor, usr.FullName, ex.Message);

            }

        }

        logger.InfoFormat("summary {0} ===={1}", log.ToString(), log.ToString());
        return string.Format("{0} Students have been created.", studentsCreated);
    }
    public static void migrateParents(log4net.ILog logger, IList<PASSIS.LIB.User> allStudents)
    {
        /*retrieve all students from userrroles 
        //foreach parents
               move from users tp [parentdetails 
         * check if the fatherseail address exists 
         * if yes
         *     create the parent using emailaddress as its username
         *      update the users parentid 
         *      update the userrole
         *    no 
         *       update the users parentid 
         * 
         * 
         * 
             
         */

        logger.InfoFormat("....found {0} students", allStudents.Count);
        foreach (PASSIS.LIB.User usr in allStudents)
        {
            try
            {
                //check to know whether the fathers phone number is null
                string searchObj = string.Empty;
                searchObj = usr.FathersPhoneNumber.Trim().ToLower();

                if (string.IsNullOrEmpty(searchObj))
                {
                    if (!string.IsNullOrEmpty(usr.MothersPhoneNumber))//look for other string to uniquely identify the father
                        searchObj = usr.MothersPhoneNumber.Trim().ToLower();
                    else
                    {
                        searchObj = getRandomUsername();
                    }
                }

                PASSIS.LIB.ParentDetail father = new UsersLIB().RetrieveParentDetail(searchObj);
                if (father == null)
                {
                    //Father has not been created
                    logger.InfoFormat("parent not yet created ");
                    PASSIS.LIB.ParentDetail pd = new PASSIS.LIB.ParentDetail();
                    pd.FathersEmail = usr.FathersEmail;
                    pd.FathersName = usr.FathersName;
                    pd.FathersNationality = usr.FathersNationality;
                    pd.FathersPhoneNumber = usr.FathersPhoneNumber;
                    pd.FathersTitle = usr.FathersTitle;
                    pd.FathersWorkAddress = usr.FathersWorkAddress;
                    pd.MothersEmail = usr.MothersEmail;
                    pd.MothersName = usr.MothersName;
                    pd.MothersNationality = usr.MothersNationality;
                    pd.MothersOtherName = usr.MothersOtherName;
                    pd.MothersWorkAddress = usr.MothersWorkAddress;
                    pd.Siblings = usr.Siblings;
                    pd.GuardianDetails = usr.GuardianDetails;
                    pd.GuardianEmail = usr.GuardianEmail;
                    pd.GuardianPhoneNumber = usr.GuardianPhoneNumber;
                    pd.GuardianRelationship = usr.GuardianRelationship;
                    new UsersLIB().SaveParentDetail(pd);

                    PASSISLIBDataContext context = new PASSISLIBDataContext();
                    PASSIS.LIB.Utility.BasePage log = new PASSIS.LIB.Utility.BasePage();
                    PASSIS.LIB.School schObj = context.Schools.FirstOrDefault(x => x.Id == log.logonUser.SchoolId);

                    PASSIS.LIB.User newParent = new PASSIS.LIB.User();
                    newParent.Username = schObj.Code + searchObj;/// could be father/mother's email 
                    newParent.DateOfBirth = newParent.LastLoginDate = (DateTime)DateTime.Now;
                    newParent.LastLoginDate = DateTime.Now;
                    newParent.DateCreated = (DateTime)DateTime.Now;
                    newParent.Password = PASSIS.LIB.Utility.Utili.getDefaultPassword();
                    newParent.Gender = usr.FathersEmail == "" ? (Int32)PASSIS.LIB.Utility.Gender.Female : (Int32)PASSIS.LIB.Utility.Gender.Male;
                    newParent.SchoolCampusId = usr.SchoolCampusId;
                    newParent.SchoolId = usr.SchoolId;
                    newParent.FirstName = usr.FathersName;
                    newParent.EmailAddress = usr.FathersEmail;
                    newParent.ParentId = pd.Id;
                    new UsersLIB().SaveUser(newParent);

                    PASSIS.LIB.UserRole usrRol = new PASSIS.LIB.UserRole();
                    usrRol.RoleId = (long)PASSIS.LIB.Utility.roles.parent;
                    usrRol.UserId = newParent.Id;
                    new UsersLIB().SaveUserRole(usrRol);
                    //tell d kid his parent 
                    PASSIS.LIB.ParentStudentMap psm = new PASSIS.LIB.ParentStudentMap();
                    psm.ParentId = pd.Id;
                    psm.ParentUserId = newParent.Id;
                    psm.StudentId = usr.Id;
                    new ParentStudentMapLIB().SaveParentStudentMap(psm);


                    usr.ParentId = pd.Id;
                    new PASSIS.LIB.UsersLIB().UpdateUser(usr);
                    logger.InfoFormat("just created new parent for  student with Id {0}.....fullname {1}", usr.Id, usr.StudentFullName);
                }
                else
                {//
                    logger.InfoFormat("already exist , tell him is parent");
                    usr.ParentId = father.Id;
                    new UsersLIB().UpdateUser(usr);

                    PASSIS.LIB.ParentStudentMap psm = new PASSIS.LIB.ParentStudentMap();
                    psm.ParentId = father.Id;
                    psm.StudentId = usr.Id;
                    psm.ParentUserId = new ParentStudentMapDAL().RetrieveParentStudentMap((long)psm.ParentId).ParentUserId;
                    new ParentStudentMapLIB().SaveParentStudentMap(psm);
                    logger.InfoFormat("has existing parent:::: student with Id {0}.....fullname {1}", usr.Id, usr.StudentFullName);

                }
            }
            catch (Exception ex)
            {
                //throw ex;
                logger.InfoFormat("error occurred on userId = {0} , message = {1}", usr.Id, ex.Message);
            }
        }
    }
    public static string getRandomUsername()
    {
        return string.Format("Identity{0}", PASSIS.LIB.Utility.Utili.GetUniqueRandomNumber(10));
    }
    public static string[] RetrieveEmailAddressesFromAnyString(string text)
    {
        const string MatchEmailPattern =
       @"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
       + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
         + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
       + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})";
        Regex rx = new Regex(MatchEmailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        // Find matches.
        MatchCollection matches = rx.Matches(text);
        // Report the number of matches found.
        int noOfMatches = matches.Count;
        // Report on each match.
        string[] results = new string[noOfMatches];
        int c = 0;
        foreach (Match match in matches)
        {
            results[c] = match.Value.ToString();
            c++;
        }
        return results;
    }
    public static string GetUniqueRandomNumber(object objlength)
    {
        int length = Convert.ToInt32(objlength);
        var number = rng.NextDouble().ToString("0.000000000000").Substring(2, length);
        return number;
    }
    #region password
    /// <summary>
    /// Under construction 
    /// </summary>
    /// <returns></returns>
    public static string getDefaultPassword()
    {
        return "password";
    }
    public static PASSIS.LIB.Utility.Gender stringToGenderEnum(string genderString)
    {
        PASSIS.LIB.Utility.Gender enumObj = PASSIS.LIB.Utility.Gender.Male;
        switch (genderString.Trim().ToLower())
        {
            case "f":
                enumObj = PASSIS.LIB.Utility.Gender.Female;
                break;
            case "m":
                enumObj = PASSIS.LIB.Utility.Gender.Male;
                break;
        }
        return enumObj;
    }
    #endregion
    private static Random rng = new Random(Environment.TickCount);

    public void DuplicateReturn()
    {

        if (!(Page.ClientScript.IsStartupScriptRegistered("Registered Script")))
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "Duplicate Record", "confirmComplete()", true);


        }



    }


    protected void btnContinue_Click(object sender, EventArgs e)
    {
        //btnContinue.Visible = true;
        //if (!(Page.ClientScript.IsStartupScriptRegistered("Registered Script")))
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, GetType(), "Duplicate Record", "confirmComplete()", true);

        //    btnBulkUpload.OnClientClick = "return confirmComplete();";
        //}
    }

    private DataTable createUnsuccessfulDataTableTemplate()
    {
        DataTable table = new DataTable("Table Title");

        DataColumn col1 = new DataColumn("New Name");
        col1.DataType = System.Type.GetType("System.String");

        DataColumn col2 = new DataColumn("Existing Name(s)");
        col1.DataType = System.Type.GetType("System.String");

        DataColumn col3 = new DataColumn("Reason");
        col3.DataType = System.Type.GetType("System.String");


        table.Columns.Add(col1);
        table.Columns.Add(col2);
        table.Columns.Add(col3);
        return table;
    }
    private DataTable createsuccessfulDataTableTemplate()
    {
        DataTable table = new DataTable("Table Title");

        DataColumn col1 = new DataColumn("New Name");
        col1.DataType = System.Type.GetType("System.String");

        DataColumn col2 = new DataColumn("Existing Name(s)");
        col1.DataType = System.Type.GetType("System.String");

        DataColumn col3 = new DataColumn("Reason");
        col3.DataType = System.Type.GetType("System.String");


        table.Columns.Add(col1);
        table.Columns.Add(col2);
        table.Columns.Add(col3);
        return table;
    }
}
