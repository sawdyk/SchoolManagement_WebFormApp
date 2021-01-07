using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using NLog;
using System.IO;
using PASSIS.LIB;

namespace PASSIS.LIB.Utility
{
    public class BasePage : System.Web.UI.Page
    {
        public static Logger nlogger = LogManager.GetCurrentClassLogger();
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger("BasePage");
        protected void SetFormViewModeBase(FormView frmView, string mode)
        {
            switch (PASSIS.LIB.Utility.Utili.getFormMode(mode))
            {
                case FormMode.Edit:
                    frmView.ChangeMode(FormViewMode.Edit);
                    break;
                case FormMode.Insert:
                    frmView.ChangeMode(FormViewMode.Insert);
                    break;
                case FormMode.View:
                    frmView.ChangeMode(FormViewMode.ReadOnly);
                    break;
            }
        }
        protected string referrerUrlBase_VS
        {
            get
            {
                //if (ViewState["::referrerUrl_VS::"] != null)
                return ViewState["::referrerUrlBase_VS::"].ToString();
            }
            set
            {
                ViewState["::referrerUrlBase_VS::"] = value;
            }
        }
        protected string getUrlBase()
        {
            return referrerUrlBase_VS;
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            string globalUniqueId = null;
            // if the user is not Admin , redirect to Login Page
            if (Session["UserSession"] != null)
            {

               
                logMessage("Login the init");
                UserSessionData usrSessionData = new UserSessionData();
                usrSessionData = (UserSessionData)Session["UserSession"];

                //try
                //{
                //    Int64 roleId = (long)new UsersLIB().RetrieveUserRole(SessionData.UserId).RoleId;
                //    switch (roleId)
                //    {
                //        case 1: //systemadmin
                //            this.Page.MasterPageFile = "~/Site.master";
                //            break;
                //        case 2://admin
                //            this.Page.MasterPageFile = "~/localAdmin.master";
                //            break;
                //        case 3://teacher
                //            this.Page.MasterPageFile = "~/teacher.master";
                //            break;
                //        case 4://vice principal
                //            this.Page.MasterPageFile = "~/vprincipal.master";
                //            break;
                //        case 5://parent
                //            this.Page.MasterPageFile = "~/parent.master";
                //            break;
                //        case 6:// student
                //            this.Page.MasterPageFile = "~/student.master";
                //            break;
                //        case 7:// finance
                //            this.Page.MasterPageFile = "~/finance.master";
                //            break;
                //        case 8:// finance
                //            this.Page.MasterPageFile = "~/localAdmin.master";
                //            break;
                //        case 9:// finance
                //            this.Page.MasterPageFile = "~/subjectTeacher.master";
                //            break;
                //        case 10:// principal
                //            this.Page.MasterPageFile = "~/principal.master";
                //            break;
                //        case 17:// Server Supervisor
                //            this.Page.MasterPageFile = "~/Supervisor.master";
                //            break;

                //            logMessage("Log in the init");
                //    }
                //}
                //catch (Exception ex)
                //{
                //    //Response.Redirect("~/Login1.aspx");
                //    throw ex.InnerException;// +ex.StackTrace + ex.Message;
                //}

            }
            else
            {
                Response.Redirect("~/Login.aspx");
            }


        }
        public string getStatus
        {
            get
            {
                return "false";

            }
        }
        public string getMessage
        {
            get
            {
                return string.Format("0 unread message(s).");
            }
        }
        protected override void OnInit(EventArgs e)
        {
          
             
            base.OnInit(e);

 
        }
        private const string SESSION_NAME = "UserSession";
        private const string AdmissionSession_NAME = "AdmissionSession";
        public Role logonUserRole 
        {
            get 
            {
                return new UsersLIB().RetrieveUserRole((long)logonUser.Id).Role;
            }
        }
        public Grade getLogonTeacherGrade 
        {
            get 
            {
                return new ClassGradeLIB().RetrieveGradeClassOfTeacher(logonUser.Id);
            }
        }
        public Boolean isUserClassTeacher 
        {
            get
            {
                return new ClassGradeLIB().RetrieveGradeClassOfTeacher(logonUser.Id) == null ? false : true; 
            }
        }
        public User logonUser
        {
            get
            {
                return new UsersLIB().RetrieveUser(SessionData.UserId);
            }
        }
        public AdmissionUser logonAdmissionUser
        {
            get
            {
                return new UsersLIB().RetrieveAdmissionUser(AdmissionSessionData.Id);
            }
        }
        public UserSessionData SessionData
        {
            get
            {
                UserSessionData sessionData = (UserSessionData)Session[SESSION_NAME];
                if (sessionData != null)
                    return sessionData;
                else
                    throw new Exception("Could not retrieve session state");
            }
            set
            {
                Session[SESSION_NAME] = value;
            }
        }
        public UserAdmissionSessionData AdmissionSessionData
        {
            get
            {
                UserAdmissionSessionData AdmissionSessionData = (UserAdmissionSessionData)Session[AdmissionSession_NAME];
                if (AdmissionSessionData != null)
                    return AdmissionSessionData;
                else
                    throw new Exception("Could not retrieve session state");
            }
            set
            {
                Session[AdmissionSession_NAME] = value;
            }
        }
        private AcademicSession _schoolCurrentAcademicSession;
        public AcademicSession CurrentAcademicSession
        {
            get
            {
                if (_schoolCurrentAcademicSession == null)
                    _schoolCurrentAcademicSession = new AcademicSessionLIB().RetrieveAcademicSession(DateTime.Now);
                return _schoolCurrentAcademicSession;
            }

        }
        public static void logMessage(string msg)
        {
            StreamWriter sm = null;
            try
            {
                sm = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "~\\Log\\LogFile.txt", true);
                sm.WriteLine(DateTime.Now.ToString() + " : " + msg);
                sm.Flush();
                sm.Close();
            }
            catch (Exception e)
            {

            }
        }
        //public bool IsAdmin() s
        //{
        //    bool result = false;
        //    try
        //    {
        //        if (logonUser.IsAdmin != null)
        //        {
        //            if (logonUser.IsAdmin == false)
        //                result = false;
        //            else
        //                result = true;
        //        }
        //    }
        //    catch { }
        //    return result; 

        //}



    }
}
