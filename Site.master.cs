using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.DAO.Utility;
using System.Data.SqlClient;
using PASSIS.LIB;

public partial class SiteMaster : MasterPage
{
    PASSISLIBDataContext context = new PASSISLIBDataContext();
    string logo = "";
    string url = "";
    string name = "";

    Int64 StudentGradeId = 0L; 
    Int64 StudentClassId = 0L;
    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {
            PASSIS.LIB.Utility.BasePage bp = new PASSIS.LIB.Utility.BasePage();
            PASSIS.LIB.User logonUser = bp.logonUser;

            long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);


            try
            {
                StudentGradeId = new ClassGradeLIB().RetrieveStudentGrade(logonUser.Id, curSessionId).GradeId;
                StudentClassId = new ClassGradeLIB().RetrieveStudentGrade(logonUser.Id, curSessionId).ClassId;
            }
            catch { }

            Grade gradedId = context.Grades.FirstOrDefault(s => s.Id == StudentGradeId);
            Class_Grade classId = context.Class_Grades.FirstOrDefault(s => s.Id == StudentClassId);
            //Response.Write(logonUser.);
            try
            {
                lblName.Text = string.Format(" {0}, {1} ", logonUser.FirstName.ToUpper(), logonUser.LastName);

                //this is use to assign current session to a label
                lblSession.Text = new AcademicSessionLIB().GetCurrentSession(logonUser.SchoolId);


                //using System.Data.SqlClient;
                clsMyDB db = new clsMyDB();
                db.connct();
                string que = "SELECT Logo, Url, Name FROM Schools WHERE Id='" + logonUser.SchoolId + "'";
                SqlDataReader dat = db.fetch(que);
                while (dat.Read())
                {
                    logo = dat[0].ToString();
                    url = dat[1].ToString();
                    name = dat[2].ToString();
                    lblSchoolName.Text = name.ToUpper();
                }

                db.closeConnct();
                if (logo != "")
                {
                    Img3.Src = logo;
                    Img1.Src = logo;
                    Img2.Src = logo;
                    //this.slogo.Src = logo;
                    //this.slogo.Visible = true;
                }
                //if (url != "")
                //{
                //    this.sUrl.HRef = url;
                //}


            }
            catch
            {
                lblName.Text = string.Format("Welcome {0}", logonUser.Username);
            }

            try
            {
                // left side Menu

                long x;
                long.TryParse(logonUser.UserRoles[0].RoleId.ToString(), out x);
                switch ((int)(x - (long)1))
                {
                    case 0:
                        {
                            //this.Page.MasterPageFile = "~/Site.master";
                            this.containerDivs.InnerHtml = this.SuperAdminSideMenu();
                            break;
                        }
                    case 1:
                        {
                            //this.Page.MasterPageFile = "~/localAdmin.master";
                            this.containerDivs.InnerHtml = this.AdminSideMenu();
                            break;
                        }
                    case 2:
                        {
                            //this.Page.MasterPageFile = "~/teacher.master";
                            this.containerDivs.InnerHtml = this.ClassTeacherSideMenu();
                            break;
                        }
                    case 3:
                        {
                            //this.Page.MasterPageFile = "~/vprincipal.master";
                            this.containerDivs.InnerHtml = this.ClassTeacherSideMenu();
                            break;
                        }
                    case 4:
                        {
                            //this.Page.MasterPageFile = "~/parent.master";
                            this.containerDivs.InnerHtml = this.ParentSideMenu();
                            break;
                        }
                    case 5:
                        {
                            //this.Page.MasterPageFile = "~/student.master";
                            this.containerDivs.InnerHtml = this.StudentSideMenu();
                            break;
                        }
                    case 6:
                        {
                            //this.Page.MasterPageFile = "~/finance.master";
                            this.containerDivs.InnerHtml = this.FinanceOfficerSideMenu();
                            break;
                        }
                    case 7:
                        {
                            //this.Page.MasterPageFile = "~/localAdmin.master";
                            this.containerDivs.InnerHtml = this.AdminSideMenu();
                            break;
                        }
                    case 8:
                        {
                            //this.Page.MasterPageFile = "~/subjectTeacher.master";
                            this.containerDivs.InnerHtml = this.SubjectTeacherSideMenu();

                            break;
                        }
                    case 9:
                        {
                            //this.Page.MasterPageFile = "~/principal.master";
                            this.containerDivs.InnerHtml = this.PrincipalSideMenu();

                            break;
                        }
                    //case 10:
                    //    {

                    //        break;
                    //    }
                    //case 11:
                    //    {

                    //        break;
                    //    }
                    //case 12:
                    //    {

                    //        //this.Page.MasterPageFile = "~/AdmissionOfficer.master";
                    //        //this.containerDiv.InnerHtml = this.Dashboard_AdmissionOfficer();
                    //        break;
                    //    }
                    //case 13:
                    //    {

                    //        break;
                    //    }
                    case 14:
                        {

                            //this.Page.MasterPageFile = "~/localAdmin.master";
                            this.containerDivs.InnerHtml = this.AdminSideMenu();
                            break;
                        }
                    case 15:
                        {

                            //this.Page.MasterPageFile = "~/localAdmin.master";
                            this.containerDivs.InnerHtml = this.AdminSideMenu();
                            break;
                        }
                    case 16:
                        {

                            //this.Page.MasterPageFile = "~/localAdmin.master";
                            this.containerDivs.InnerHtml = this.AdminSideMenu();
                            break;
                        }
                    case 17:
                        {

                            //this.Page.MasterPageFile = "~/localAdmin.master";
                            this.containerDivs.InnerHtml = this.SchoolOwnerSideMenu();
                            break;
                        }
                    case 18:
                        {

                            ////this.Page.MasterPageFile = "~/localAdmin.master";
                            this.containerDivs.InnerHtml = this.SchoolOwnerSideMenu();
                            break;
                        }
                    default:
                        {
                            return;
                        }
                }
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                PASSIS.LIB.PAConfig.LogAudit(PASSIS.LIB.PAConfig.AuditAction.Login, Request.Url.AbsoluteUri, ex.Message.ToString(), logonUser.Id);
                base.Response.Redirect("~/Login.aspx");
                throw ex;
            }
        }
    }
    public string SuperAdminSideMenu()
    {

        string ret = @"<div class='page-content d-flex align-items-stretch'>
                        <div class='default-sidebar'>
                    <nav class='side-navbar box-scroll sidebar-scroll'>
                              <!--Begin Main Navigation -->
                              <ul class='list-unstyled'>
                            <li class=''><a href ='Default.aspx' 'aria-expanded='false'><i class='la la-columns'></i><span> Home </span></a>
                            </li>
                        </ul>
                        <ul class='list-unstyled'>
                            <li><a href ='#dropdown-ui' aria-expanded='false' data-toggle='collapse'><i class='la la-sitemap'></i><span> Administration</span></a>
                                <ul id='dropdown-ui' class='collapse list-unstyled pt-0'>
                                    <li><a href='BasePage.aspx?ID=1'> Student </a></li>
                                    <li><a href='BasePage.aspx?ID=2'> Classes</a></li>
                                    <li><a href='BasePage.aspx?ID=3' > Staff </a></li>
                                    <li><a href='BasePage.aspx?ID=4'> School</a></li>
                                </ul>
                            </li>
                            <li><a href='#dropdown-icons' aria-expanded='false' data-toggle='collapse'><i class='la la-institution'></i><span>Admission</span></a>
                                <ul id='dropdown-icons' class='collapse list-unstyled pt-0'>
                                    <li><a href ='#'> Configuration / Application </a></li>
                                    <li><a href='#'>Payments</a></li>
                                    <li><a href ='#'> Test </a></li>
                                    <li><a href='#'>Interview</a></li>
                                    <li><a href ='#'> Admissions </a></li>
                                </ul>
                            </li>
                            <li><a href='#dropdown-app' aria-expanded='false' data-toggle='collapse'><i class='la la-book'></i><span> Academics</span></a>
                                <ul id ='dropdown-app' class='collapse list-unstyled pt-0'>
                                    <li><a href ='BasePage.aspx?ID=5'> Grading and Attendance</a><li>
                                    <li><a href ='BasePage.aspx?ID=6'> Syllabus and Notes</a></li>
                                    <li><a href ='BasePage.aspx?ID=7'> Subjects </a></li>
                                    <li><a href='BasePage.aspx?ID=10'> Assignments </a></li>
                                    </ul>
                                </li>
                             <li><a href='#dropdown-authentication' aria-expanded= 'false' data-toggle='collapse'><i class='la la-money'></i><span>Finance and Fees</span></a>
                                    <ul id='dropdown-authentication' class='collapse list-unstyled pt-0'>
                                        <li><a href='BasePage.aspx?ID=8'> Fees </a></li>
                                        <li><a href='BasePage.aspx?ID=9'>Payments</a></li>
                                    </ul>
                                </li>
                            <li><a href='#dropdown-forms' aria-expanded='false' data-toggle='collapse'><i class='la la-smile-o'></i><span>Socials Collaborations</span></a>
                                <ul id='dropdown-forms' class='collapse list-unstyled pt-0'>
                                    <li><a href='#'> Forums </a></li>
                                    <li><a href='#'>Chats</a></li>
                                    <li><a href ='#'> PTA </a></li>
                                    <li><a href='BasePage.aspx?ID=13'>Notice and Bulletin</a></li>
                                </ul>
                            </li>
                            <li><a href='#dropdown-tables' aria-expanded='false' data-toggle='collapse'><i class='la la-th-large'></i><span>Services</span></a>
                                <ul id ='dropdown-tables' class='collapse list-unstyled pt-0'>
                                  <li><a href = '#'> Transport </a></li>
                                  <li><a href='#'>Hostel</a></li>
                                  <li><a href = '#'> Feeding </a></li>
                                  <li><a href='#'>Coaching</a></li>
                                </ul>
                            </li>
                            <li><a href = '#dropdown-new' aria-expanded='false' data-toggle='collapse'><i class='la la-street-view'></i><span>Smart Learn</span></a>
                                <ul id = 'dropdown-new' class='collapse list-unstyled pt-0'>
                                    <li><a href ='BasePage.aspx?ID=12' > Resources </a></li>
                                    <div id='containerURL' runat='server' class='f'> </div>
                                </ul>
                            </li>
                            <li><a href='#dropdown-new1' aria-expanded= 'false' data-toggle= 'collapse' ><i class='la la-bar-chart'></i><span>Reports</span></a>
                                <ul id ='dropdown-new1' class='collapse list-unstyled pt-0'>
                                    <li><a href = '#'> Administration </a></li>
                                    <li><a href='#'>Admissions</a></li>
                                    <li><a href ='BasePage.aspx?ID=11'> Academics </a></li>
                                    <li><a href='#'>Finance</a></li>
                                </ul>
                            </li>
                            <li><a href ='#dropdown-new2' aria-expanded='false' data-toggle='collapse'><i class='la la-graduation-cap'></i><span>Alumni</span></a>
                                <ul id = 'dropdown-new2' class='collapse list-unstyled pt-0'>
                                    <li><a href = '#'> Graduated Students</a></li>
                                </ul>
                            </li>
                            <li class=''><a href ='logout.aspx'' aria-expanded='false'><i class='la la-recycle'></i><span>Logout</span></a>
                        </ul>
                        <!--End Main Navigation-->
                    </nav>
                </div></div>";

        return ret;

    }


    public string AdminSideMenu()
    {
        PASSIS.LIB.Utility.BasePage bp = new PASSIS.LIB.Utility.BasePage();
        PASSIS.LIB.User logonUser = bp.logonUser;

        string ret = @"<div class='page-content d-flex align-items-stretch'>
                        <div class='default-sidebar'>
                    <nav class='side-navbar box-scroll sidebar-scroll'>
                              <!--Begin Main Navigation -->
                              <ul class='list-unstyled'>
                            <li class=''><a href ='Default.aspx' 'aria-expanded='false'><i class='la la-columns'></i><span> Home </span></a>
                            </li>
                        </ul>
                        <ul class='list-unstyled'>
                            <li><a href ='#dropdown-ui' aria-expanded='false' data-toggle='collapse'><i class='la la-sitemap'></i><span> Administration</span></a>
                                <ul id='dropdown-ui' class='collapse list-unstyled pt-0'>
                                    <li><a href='BasePage.aspx?ID=1'> Student </a></li>
                                    <li><a href='BasePage.aspx?ID=2'> Classes</a></li>
                                    <li><a href='BasePage.aspx?ID=3' > Staff </a></li>
                                    <li><a href='BasePage.aspx?ID=4'> School</a></li>
                                </ul>
                            </li>
                            <li><a href='#dropdown-icons' aria-expanded='false' data-toggle='collapse'><i class='la la-institution'></i><span>Admission</span></a>
                                <ul id='dropdown-icons' class='collapse list-unstyled pt-0'>
                                    <li><a href ='BasePage.aspx?ID=14'>Configuration/Application</a></li>
                                    <li><a href='BasePage.aspx?ID=15'>Payments</a></li>
                                    <li><a href ='BasePage.aspx?ID=16'>Test</a></li>
                                    <li><a href='BasePage.aspx?ID=17'>Interview</a></li>
                                    <li><a href ='BasePage.aspx?ID=18'> Admissions </a></li>
                                </ul>
                            </li>
                            <li><a href='#dropdown-app' aria-expanded='false' data-toggle='collapse'><i class='la la-book'></i><span> Academics</span></a>
                                <ul id ='dropdown-app' class='collapse list-unstyled pt-0'>
                                    <li><a href ='BasePage.aspx?ID=5'> Grading and Attendance</a><li>
                                    <li><a href ='BasePage.aspx?ID=6'> Syllabus and Notes</a></li>
                                    <li><a href ='BasePage.aspx?ID=7'> Subjects </a></li>
                                    <li><a href='BasePage.aspx?ID=10'> Assignments </a></li>
                                    </ul>
                                </li>
                              <!--<li><a href='#dropdown-authentication' aria-expanded= 'false' data-toggle='collapse'><i class='la la-money'></i><span>Finance and Fees</span></a>
                                    <ul id='dropdown-authentication' class='collapse list-unstyled pt-0'>
                                        <li><a href='BasePage.aspx?ID=8'> Fees </a></li>
                                        <li><a href='BasePage.aspx?ID=9'>Payments</a></li>
                                    </ul> 
                                </li>-->
                            <li><a href='#dropdown-forms' aria-expanded='false' data-toggle='collapse'><i class='la la-smile-o'></i><span>Socials Collaborations</span></a>
                                <ul id='dropdown-forms' class='collapse list-unstyled pt-0'>
                                    <!-- <li><a href='#'> Forums </a></li>
                                    <li><a href='#'>Chats</a></li>
                                    <li><a href ='#'> PTA </a></li>-->
                                    <li><a href='BasePage.aspx?ID=13'>Notice and Bulletin</a></li>
                                </ul>
                            </li>
                            <!-- <li><a href='#dropdown-tables' aria-expanded='false' data-toggle='collapse'><i class='la la-th-large'></i><span>Services</span></a>
                                <ul id ='dropdown-tables' class='collapse list-unstyled pt-0'>
                                  <li><a href = '#'> Transport </a></li>
                                  <li><a href='#'>Hostel</a></li>
                                  <li><a href = '#'> Feeding </a></li>
                                  <li><a href='#'>Coaching</a></li>
                                </ul>-->
                            </li>
                            <li><a href = '#dropdown-new' aria-expanded='false' data-toggle='collapse'><i class='la la-street-view'></i><span>Smart Learn</span></a>
                                <ul id = 'dropdown-new' class='collapse list-unstyled pt-0'>
                                    <li><a href ='BasePage.aspx?ID=12'> Resources </a></li>
                                    <li><a href='http://localhost/websites/PassisCBT/?schoolId=" + logonUser.SchoolId + @"&campusId=" + logonUser.SchoolCampusId + @"&userId=" + logonUser.Id + @"&roleType=Admin'> CBT </a></li>
                                    <!--<li><a href='http://passiscbt.telnetapps.com/?schoolId=" + logonUser.SchoolId + @"&campusId=" + logonUser.SchoolCampusId + @"&userId=" + logonUser.Id + @"&roleType=Admin'> CBT </a></li>-->
                                    <div id='containerURL' runat='server' class='f'> </div>
                                </ul>
                            </li>
                            <li><a href='#dropdown-new1' aria-expanded= 'false' data-toggle= 'collapse' ><i class='la la-bar-chart'></i><span>Reports</span></a>
                                <ul id ='dropdown-new1' class='collapse list-unstyled pt-0'>
                                    <li><a href = '#'> Administration </a></li>
                                    <li><a href='#'>Admissions</a></li>
                                    <li><a href='#'>Finance</a></li>
                                    <li><a href ='BasePage.aspx?ID=11'> Academics </a></li>
                                </ul>
                            </li>
                           <!--<li><a href ='#dropdown-new2' aria-expanded='false' data-toggle='collapse'><i class='la la-graduation-cap'></i><span>Alumni</span></a>
                                <ul id = 'dropdown-new2' class='collapse list-unstyled pt-0'>
                                    <li><a href = '#'> Graduated Students</a></li>
                                </ul>
                            </li>-->
                            <li class=''><a href ='logout.aspx'' aria-expanded='false'><i class='la la-recycle'></i><span>Logout</span></a>
                        </ul>
                        <!--End Main Navigation-->
                    </nav>
                </div></div>";



        return ret;
    }

    public string ClassTeacherSideMenu()
    {
        PASSIS.LIB.Utility.BasePage bp = new PASSIS.LIB.Utility.BasePage();
        PASSIS.LIB.User logonUser = bp.logonUser;

        string ret = @"<div class='page-content d-flex align-items-stretch'>
                        <div class='default-sidebar'>
                    <nav class='side-navbar box-scroll sidebar-scroll'>
                              <!--Begin Main Navigation -->
                              <ul class='list-unstyled'>
                            <li class=''><a href ='Default.aspx' 'aria-expanded='false'><i class='la la-columns'></i><span> Home </span></a>
                            </li>
                        </ul>
                        <ul class='list-unstyled'>
                            <li><a href ='#dropdown-ui' aria-expanded='false' data-toggle='collapse'><i class='la la-sitemap'></i><span> Administration</span></a>
                                <ul id='dropdown-ui' class='collapse list-unstyled pt-0'>
                                    <li><a href='BasePage.aspx?ID=1'> Student </a></li>
                                  <!--<li><a href='BasePage.aspx?ID=2'> Classes</a></li>
                                    <li><a href='BasePage.aspx?ID=3' > Staff </a></li>
                                    <li><a href='BasePage.aspx?ID=4'> School</a></li>-->
                                </ul>
                            </li>
                          <!--<li><a href='#dropdown-icons' aria-expanded='false' data-toggle='collapse'><i class='la la-institution'></i><span>Admission</span></a>
                                <ul id='dropdown-icons' class='collapse list-unstyled pt-0'>
                                    <li><a href ='#'> Configuration / Application </a></li>
                                    <li><a href='#'>Payments</a></li>
                                    <li><a href ='#'> Test </a></li>
                                    <li><a href='#'>Interview</a></li>
                                    <li><a href ='#'> Admissions </a></li>
                                </ul>
                            </li>-->
                            <li><a href='#dropdown-app' aria-expanded='false' data-toggle='collapse'><i class='la la-book'></i><span> Academics</span></a>
                                <ul id ='dropdown-app' class='collapse list-unstyled pt-0'>
                                    <li><a href ='BasePage.aspx?ID=5'> Grading and Attendance</a><li>
                                    <li><a href ='BasePage.aspx?ID=6'> Syllabus and Notes</a></li>
                                    <li><a href ='BasePage.aspx?ID=7'> Subjects </a></li>
                                    <li><a href='BasePage.aspx?ID=10'> Assignments </a></li>
                                    </ul>
                                </li>
                        <!--<li><a href='#dropdown-authentication' aria-expanded= 'false' data-toggle='collapse'><i class='la la-money'></i><span>Finance and Fees</span></a>
                                    <ul id='dropdown-authentication' class='collapse list-unstyled pt-0'>
                                        <li><a href='BasePage.aspx?ID=8'> Fees </a></li>
                                        <li><a href='BasePage.aspx?ID=9'>Payments</a></li>
                                    </ul>
                                </li>-->
                            <!--<li><a href='#dropdown-forms' aria-expanded='false' data-toggle='collapse'><i class='la la-smile-o'></i><span>Socials Collaborations</span></a>
                                <ul id='dropdown-forms' class='collapse list-unstyled pt-0'>
                                    <li><a href='#'> Forums </a></li>
                                    <li><a href='#'>Chats</a></li>
                                    <li><a href ='#'> PTA </a></li>
                                    <li><a href='BasePage.aspx?ID=13'>Notice and Bulletin</a></li>
                                </ul>
                            </li>-->
                        <!--<li><a href='#dropdown-tables' aria-expanded='false' data-toggle='collapse'><i class='la la-th-large'></i><span>Services</span></a>
                                <ul id ='dropdown-tables' class='collapse list-unstyled pt-0'>
                                  <li><a href = '#'> Transport </a></li>
                                  <li><a href='#'>Hostel</a></li>
                                  <li><a href = '#'> Feeding </a></li>
                                  <li><a href='#'>Coaching</a></li>
                                </ul>
                            </li>-->
                           <li><a href = '#dropdown-new' aria-expanded='false' data-toggle='collapse'><i class='la la-street-view'></i><span>Smart Learn</span></a>
                                <ul id = 'dropdown-new' class='collapse list-unstyled pt-0'>
                                    <li><a href='http://passiscbt.telnetapps.com/?schoolId=" + logonUser.SchoolId + @"&campusId=" + logonUser.SchoolCampusId + @"&userId=" + logonUser.Id + @"&roleType=Admin'> CBT </a></li>
                                    <div id='containerURL' runat='server' class='f'> </div>
                                </ul>
                            </li>
                            <li><a href='#dropdown-new1' aria-expanded= 'false' data-toggle= 'collapse' ><i class='la la-bar-chart'></i><span>Reports</span></a>
                                <ul id ='dropdown-new1' class='collapse list-unstyled pt-0'>
                                   <!--<li><a href = '#'> Administration </a></li>
                                    <li><a href='#'>Admissions</a></li>
                                    <li><a href='#'>Finance</a></li>-->
                                    <li><a href ='BasePage.aspx?ID=11'> Academics </a></li>
                                </ul>
                            </li>
                         <!--<li><a href ='#dropdown-new2' aria-expanded='false' data-toggle='collapse'><i class='la la-graduation-cap'></i><span>Alumni</span></a>
                                <ul id = 'dropdown-new2' class='collapse list-unstyled pt-0'>
                                    <li><a href = '#'> Graduated Students</a></li>
                                </ul>
                            </li>-->
                            <li class=''><a href ='logout.aspx'' aria-expanded='false'><i class='la la-recycle'></i><span>Logout</span></a>
                        </ul>
                        <!--End Main Navigation-->
                    </nav>
                </div></div>";

        return ret;

    }


    public string SubjectTeacherSideMenu()
    {
        PASSIS.LIB.Utility.BasePage bp = new PASSIS.LIB.Utility.BasePage();
        PASSIS.LIB.User logonUser = bp.logonUser;

        string ret = @"<div class='page-content d-flex align-items-stretch'>
                        <div class='default-sidebar'>
                    <nav class='side-navbar box-scroll sidebar-scroll'>
                              <!--Begin Main Navigation -->
                              <ul class='list-unstyled'>
                            <li class=''><a href ='Default.aspx' 'aria-expanded='false'><i class='la la-columns'></i><span> Home </span></a>
                            </li>
                        </ul>
                        <ul class='list-unstyled'>
                            
                          <!--<li><a href='#dropdown-icons' aria-expanded='false' data-toggle='collapse'><i class='la la-institution'></i><span>Admission</span></a>
                                <ul id='dropdown-icons' class='collapse list-unstyled pt-0'>
                                    <li><a href ='#'> Configuration / Application </a></li>
                                    <li><a href='#'>Payments</a></li>
                                    <li><a href ='#'> Test </a></li>
                                    <li><a href='#'>Interview</a></li>
                                    <li><a href ='#'> Admissions </a></li>
                                </ul>
                            </li>-->
                            <li><a href='#dropdown-app' aria-expanded='false' data-toggle='collapse'><i class='la la-book'></i><span> Academics</span></a>
                                <ul id ='dropdown-app' class='collapse list-unstyled pt-0'>
                                    <!--<li><a href ='BasePage.aspx?ID=5'> Grading and Attendance</a><li>
                                    <li><a href ='BasePage.aspx?ID=6'> Syllabus and Notes</a></li>-->
                                    <li><a href ='BasePage.aspx?ID=7'> Subjects </a></li>
                                    <li><a href='BasePage.aspx?ID=10'> Assignments </a></li>
                                    </ul>
                                </li>
                        <!--<li><a href='#dropdown-authentication' aria-expanded= 'false' data-toggle='collapse'><i class='la la-money'></i><span>Finance and Fees</span></a>
                                    <ul id='dropdown-authentication' class='collapse list-unstyled pt-0'>
                                        <li><a href='BasePage.aspx?ID=8'> Fees </a></li>
                                        <li><a href='BasePage.aspx?ID=9'>Payments</a></li>
                                    </ul>
                                </li>-->
                            <!--<li><a href='#dropdown-forms' aria-expanded='false' data-toggle='collapse'><i class='la la-smile-o'></i><span>Socials Collaborations</span></a>
                                <ul id='dropdown-forms' class='collapse list-unstyled pt-0'>
                                    <li><a href='#'> Forums </a></li>
                                    <li><a href='#'>Chats</a></li>
                                    <li><a href ='#'> PTA </a></li>
                                    <li><a href='BasePage.aspx?ID=13'>Notice and Bulletin</a></li>
                                </ul>
                            </li>-->
                        <!--<li><a href='#dropdown-tables' aria-expanded='false' data-toggle='collapse'><i class='la la-th-large'></i><span>Services</span></a>
                                <ul id ='dropdown-tables' class='collapse list-unstyled pt-0'>
                                  <li><a href = '#'> Transport </a></li>
                                  <li><a href='#'>Hostel</a></li>
                                  <li><a href = '#'> Feeding </a></li>
                                  <li><a href='#'>Coaching</a></li>
                                </ul>
                            </li>-->
                            <li><a href = '#dropdown-new' aria-expanded='false' data-toggle='collapse'><i class='la la-street-view'></i><span>Smart Learn</span></a>
                                <ul id = 'dropdown-new' class='collapse list-unstyled pt-0'>
                                    <li><a href='http://passiscbt.telnetapps.com/?schoolId=" + logonUser.SchoolId + @"&campusId=" + logonUser.SchoolCampusId + @"&userId=" + logonUser.Id + @"&roleType=Admin'> CBT </a></li>
                                    <div id='containerURL' runat='server' class='f'> </div>
                                </ul>
                            </li>
                            <!--<li><a href='#dropdown-new1' aria-expanded= 'false' data-toggle= 'collapse' ><i class='la la-bar-chart'></i><span>Reports</span></a>
                                <ul id ='dropdown-new1' class='collapse list-unstyled pt-0'>
                                    <li><a href = '#'> Administration </a></li>
                                    <li><a href='#'>Admissions</a></li>
                                    <li><a href ='BasePage.aspx?ID=11'> Academics </a></li>
                                    <li><a href='#'>Finance</a></li>
                                </ul>
                            </li>-->
                            <!--<li><a href ='#dropdown-new2' aria-expanded='false' data-toggle='collapse'><i class='la la-graduation-cap'></i><span>Alumni</span></a>
                                <ul id = 'dropdown-new2' class='collapse list-unstyled pt-0'>
                                    <li><a href = '#'> Graduated Students</a></li>
                                </ul>
                            </li>-->
                            <li class=''><a href ='logout.aspx'' aria-expanded='false'><i class='la la-recycle'></i><span>Logout</span></a>
                        </ul>
                        <!--End Main Navigation-->
                    </nav>
                </div></div>";

        return ret;

    }

    public string PrincipalSideMenu()
    {
        string ret = @"<div class='page-content d-flex align-items-stretch'>
                        <div class='default-sidebar'>
                    <nav class='side-navbar box-scroll sidebar-scroll'>
                              <!--Begin Main Navigation -->
                              <ul class='list-unstyled'>
                            <li class=''><a href ='Default.aspx' 'aria-expanded='false'><i class='la la-columns'></i><span> Home </span></a>
                            </li>
                        </ul>
                        <ul class='list-unstyled'>
                            <li><a href ='#dropdown-ui' aria-expanded='false' data-toggle='collapse'><i class='la la-sitemap'></i><span> Administration</span></a>
                                <ul id='dropdown-ui' class='collapse list-unstyled pt-0'>
                                    <li><a href='BasePage.aspx?ID=1'> Student </a></li>
                                    <li><a href='BasePage.aspx?ID=2'> Classes</a></li>
                                    <li><a href='BasePage.aspx?ID=3' > Staff </a></li>
                                    <li><a href='BasePage.aspx?ID=4'> School</a></li>
                                </ul>
                            </li>
                            <li><a href='#dropdown-icons' aria-expanded='false' data-toggle='collapse'><i class='la la-institution'></i><span>Admission</span></a>
                                <ul id='dropdown-icons' class='collapse list-unstyled pt-0'>
                                    <li><a href ='#'> Configuration / Application </a></li>
                                    <li><a href='#'>Payments</a></li>
                                    <li><a href ='#'> Test </a></li>
                                    <li><a href='#'>Interview</a></li>
                                    <li><a href ='#'> Admissions </a></li>
                                </ul>
                            </li>
                            <li><a href='#dropdown-app' aria-expanded='false' data-toggle='collapse'><i class='la la-book'></i><span> Academics</span></a>
                                <ul id ='dropdown-app' class='collapse list-unstyled pt-0'>
                                    <li><a href ='BasePage.aspx?ID=5'> Grading and Attendance</a><li>
                                    <li><a href ='BasePage.aspx?ID=6'> Syllabus and Notes</a></li>
                                    <li><a href ='BasePage.aspx?ID=7'> Subjects </a></li>
                                    <li><a href='BasePage.aspx?ID=10'> Assignments </a></li>
                                    </ul>
                                </li>
                             <li><a href='#dropdown-authentication' aria-expanded= 'false' data-toggle='collapse'><i class='la la-money'></i><span>Finance and Fees</span></a>
                                    <ul id='dropdown-authentication' class='collapse list-unstyled pt-0'>
                                        <li><a href='BasePage.aspx?ID=8'> Fees </a></li>
                                        <li><a href='BasePage.aspx?ID=9'>Payments</a></li>
                                    </ul>
                                </li>
                            <li><a href='#dropdown-forms' aria-expanded='false' data-toggle='collapse'><i class='la la-smile-o'></i><span>Socials Collaborations</span></a>
                                <ul id='dropdown-forms' class='collapse list-unstyled pt-0'>
                                    <li><a href='#'> Forums </a></li>
                                    <li><a href='#'>Chats</a></li>
                                    <li><a href ='#'> PTA </a></li>
                                    <li><a href='BasePage.aspx?ID=13'>Notice and Bulletin</a></li>
                                </ul>
                            </li>
                            <li><a href='#dropdown-tables' aria-expanded='false' data-toggle='collapse'><i class='la la-th-large'></i><span>Services</span></a>
                                <ul id ='dropdown-tables' class='collapse list-unstyled pt-0'>
                                  <li><a href = '#'> Transport </a></li>
                                  <li><a href='#'>Hostel</a></li>
                                  <li><a href = '#'> Feeding </a></li>
                                  <li><a href='#'>Coaching</a></li>
                                </ul>
                            </li>
                            <li><a href = '#dropdown-new' aria-expanded='false' data-toggle='collapse'><i class='la la-street-view'></i><span>Smart Learn</span></a>
                                <ul id = 'dropdown-new' class='collapse list-unstyled pt-0'>
                                    <li><a href ='BasePage.aspx?ID=12' > Resources </a></li>
                                    <div id='containerURL' runat='server' class='f'> </div>
                                </ul>
                            </li>
                            <li><a href='#dropdown-new1' aria-expanded= 'false' data-toggle= 'collapse' ><i class='la la-bar-chart'></i><span>Reports</span></a>
                                <ul id ='dropdown-new1' class='collapse list-unstyled pt-0'>
                                    <li><a href = '#'> Administration </a></li>
                                    <li><a href='#'>Admissions</a></li>
                                    <li><a href ='BasePage.aspx?ID=11'> Academics </a></li>
                                    <li><a href='#'>Finance</a></li>
                                </ul>
                            </li>
                            <li><a href ='#dropdown-new2' aria-expanded='false' data-toggle='collapse'><i class='la la-graduation-cap'></i><span>Alumni</span></a>
                                <ul id = 'dropdown-new2' class='collapse list-unstyled pt-0'>
                                    <li><a href = '#'> Graduated Students</a></li>
                                </ul>
                            </li>
                            <li class=''><a href ='logout.aspx'' aria-expanded='false'><i class='la la-recycle'></i><span>Logout</span></a>
                        </ul>
                        <!--End Main Navigation-->
                    </nav>
                </div></div>";



        return ret;
    }


    public string StudentSideMenu()
    {
        PASSIS.LIB.Utility.BasePage bp = new PASSIS.LIB.Utility.BasePage();
        PASSIS.LIB.User logonUser = bp.logonUser;
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

        try
        {
            StudentGradeId = new ClassGradeLIB().RetrieveStudentGrade(logonUser.Id, curSessionId).GradeId;
            StudentClassId = new ClassGradeLIB().RetrieveStudentGrade(logonUser.Id, curSessionId).ClassId;
        }
        catch { }

        Grade gradedId = context.Grades.FirstOrDefault(s => s.Id == StudentGradeId);
        Class_Grade classId = context.Class_Grades.FirstOrDefault(s => s.Id == StudentClassId);

        string ret = @"<div class='page-content d-flex align-items-stretch'>
                        <div class='default-sidebar'>
                    <nav class='side-navbar box-scroll sidebar-scroll'>
                              <!--Begin Main Navigation -->
                              <ul class='list-unstyled'>
                            <li class=''><a href ='Default.aspx' 'aria-expanded='false'><i class='la la-columns'></i><span> Home </span></a>
                            </li>
                        </ul>
                        <ul class='list-unstyled'>
                            <!--<li><a href ='#dropdown-ui' aria-expanded='false' data-toggle='collapse'><i class='la la-sitemap'></i><span> Administration</span></a>
                                <ul id='dropdown-ui' class='collapse list-unstyled pt-0'>
                                    <li><a href='BasePage.aspx?ID=1'> Student </a></li>
                                    <li><a href='BasePage.aspx?ID=2'> Classes</a></li>
                                    <li><a href='BasePage.aspx?ID=3' > Staff </a></li>
                                    <li><a href='BasePage.aspx?ID=4'> School</a></li>
                                </ul>
                            </li>-->
                           <!-- <li><a href='#dropdown-icons' aria-expanded='false' data-toggle='collapse'><i class='la la-institution'></i><span>Admission</span></a>
                                <ul id='dropdown-icons' class='collapse list-unstyled pt-0'>
                                    <li><a href ='#'> Configuration / Application </a></li>
                                    <li><a href='#'>Payments</a></li>
                                    <li><a href ='#'> Test </a></li>
                                    <li><a href='#'>Interview</a></li>
                                    <li><a href ='#'> Admissions </a></li>
                                </ul>
                            </li>-->
                            <li><a href='#dropdown-app' aria-expanded='false' data-toggle='collapse'><i class='la la-book'></i><span> Academics</span></a>
                                <ul id ='dropdown-app' class='collapse list-unstyled pt-0'>
                                    <li><a href ='BasePage.aspx?ID=5'> Grading and Attendance</a><li>
                                    <li><a href ='BasePage.aspx?ID=6'> Syllabus and Notes</a></li>
                                    <li><a href ='BasePage.aspx?ID=7'> Subjects </a></li>
                                    <li><a href='BasePage.aspx?ID=10'> Assignments </a></li>
                                    </ul>
                                </li>
                            <!-- <li><a href='#dropdown-authentication' aria-expanded= 'false' data-toggle='collapse'><i class='la la-money'></i><span>Finance and Fees</span></a>
                                    <ul id='dropdown-authentication' class='collapse list-unstyled pt-0'>
                                        <li><a href='BasePage.aspx?ID=8'> Fees </a></li>
                                        <li><a href='BasePage.aspx?ID=9'>Payments</a></li>
                                    </ul>
                                </li>-->
                            <li><a href='#dropdown-forms' aria-expanded='false' data-toggle='collapse'><i class='la la-smile-o'></i><span>Socials Collaborations</span></a>
                                <ul id='dropdown-forms' class='collapse list-unstyled pt-0'>
                                    <li><a href='#'> Forums </a></li>
                                    <li><a href='#'>Chats</a></li>
                                    <li><a href ='#'> PTA </a></li>
                                    <li><a href='BasePage.aspx?ID=13'>Notice and Bulletin</a></li>
                                </ul>
                            </li>
                           <!-- <li><a href='#dropdown-tables' aria-expanded='false' data-toggle='collapse'><i class='la la-th-large'></i><span>Services</span></a>
                                <ul id ='dropdown-tables' class='collapse list-unstyled pt-0'>
                                  <li><a href = '#'> Transport </a></li>
                                  <li><a href='#'>Hostel</a></li>
                                  <li><a href = '#'> Feeding </a></li>
                                  <li><a href='#'>Coaching</a></li>
                                </ul>
                            </li>-->
                            <li><a href = '#dropdown-new' aria-expanded='false' data-toggle='collapse'><i class='la la-street-view'></i><span>Smart Learn</span></a>
                                <ul id = 'dropdown-new' class='collapse list-unstyled pt-0'>
                                    <li><a href ='BasePage.aspx?ID=12' > Resources </a></li>
                                    <li><a href='http://localhost/websites/PassisCBT/?schoolId=" + logonUser.SchoolId + @"&campusId=" + logonUser.SchoolCampusId + @"&classId=" + classId.Id + @"&gradeId=" + gradedId.Id + @"&userId=" + logonUser.Id + @"&roleType=Student'> CBT </a></li>
                                    <!--<li><a href='http://passiscbt.telnetapps.com/?schoolId=" + logonUser.SchoolId + @"&campusId=" + logonUser.SchoolCampusId + @"&classId=" + classId.Id + @"&gradeId=" + gradedId.Id + @"&userId=" + logonUser.Id + @"&roleType=Student'> CBT </a></li>-->
                                    <div id='containerURL' runat='server' class='f'> </div>
                                </ul>
                            </li>
                            <!--<li><a href='#dropdown-new1' aria-expanded= 'false' data-toggle= 'collapse' ><i class='la la-bar-chart'></i><span>Reports</span></a>
                                <ul id ='dropdown-new1' class='collapse list-unstyled pt-0'>
                                    <li><a href = '#'> Administration </a></li>
                                    <li><a href='#'>Admissions</a></li>
                                    <li><a href ='BasePage.aspx?ID=11'> Academics </a></li>
                                    <li><a href='#'>Finance</a></li>
                                </ul>
                            </li>-->
                           <!--<li><a href ='#dropdown-new2' aria-expanded='false' data-toggle='collapse'><i class='la la-graduation-cap'></i><span>Alumni</span></a>
                                <ul id = 'dropdown-new2' class='collapse list-unstyled pt-0'>
                                    <li><a href = '#'> Graduated Students</a></li>
                                </ul>
                            </li>-->
                            <li class=''><a href ='logout.aspx'' aria-expanded='false'><i class='la la-recycle'></i><span>Logout</span></a>
                        </ul>
                        <!--End Main Navigation-->
                    </nav>
                </div></div>";



        return ret;
    }


    public string ParentSideMenu()
    {
        string ret = @"<div class='page-content d-flex align-items-stretch'>
                        <div class='default-sidebar'>
                    <nav class='side-navbar box-scroll sidebar-scroll'>
                              <!--Begin Main Navigation -->
                              <ul class='list-unstyled'>
                            <li class=''><a href ='Default.aspx' 'aria-expanded='false'><i class='la la-columns'></i><span> Home </span></a>
                            </li>
                        </ul>
                        <ul class='list-unstyled'>
                           <li><a href ='#dropdown-ui' aria-expanded='false' data-toggle='collapse'><i class='la la-user'></i><span>Child/Children</span></a>
                                <ul id='dropdown-ui' class='collapse list-unstyled pt-0'>
                                    <li><a href='BasePage.aspx?ID=1'>Child/Children</a></li>
                                </ul>
                            </li>
                           <!-- <li><a href='#dropdown-icons' aria-expanded='false' data-toggle='collapse'><i class='la la-institution'></i><span>Admission</span></a>
                                <ul id='dropdown-icons' class='collapse list-unstyled pt-0'>
                                    <li><a href ='#'> Configuration / Application </a></li>
                                    <li><a href='#'>Payments</a></li>
                                    <li><a href ='#'> Test </a></li>
                                    <li><a href='#'>Interview</a></li>
                                    <li><a href ='#'> Admissions </a></li>
                                </ul>
                            </li>-->
                            <li><a href='#dropdown-app' aria-expanded='false' data-toggle='collapse'><i class='la la-book'></i><span> Academics</span></a>
                                <ul id ='dropdown-app' class='collapse list-unstyled pt-0'>
                                    <li><a href ='BasePage.aspx?ID=5'> Grading and Attendance</a><li>
                                    <li><a href ='BasePage.aspx?ID=6'> Syllabus and Notes</a></li>
                                    <li><a href ='BasePage.aspx?ID=7'> Subjects </a></li>
                                    <li><a href='BasePage.aspx?ID=10'> Assignments </a></li>
                                    </ul>
                                </li>
                            <li><a href='#dropdown-authentication' aria-expanded= 'false' data-toggle='collapse'><i class='la la-money'></i><span>Fees and Payments</span></a>
                                    <ul id='dropdown-authentication' class='collapse list-unstyled pt-0'>
                                   <!-- <li><a href='BasePage.aspx?ID=8'> Fees </a></li>-->
                                        <li><a href='BasePage.aspx?ID=9'>Payments</a></li>
                                    </ul>
                                </li>
                            <li><a href='#dropdown-forms' aria-expanded='false' data-toggle='collapse'><i class='la la-smile-o'></i><span>Socials Collaborations</span></a>
                                <ul id='dropdown-forms' class='collapse list-unstyled pt-0'>
                                    <li><a href='#'> Forums </a></li>
                                    <li><a href='#'>Chats</a></li>
                                    <li><a href ='#'> PTA </a></li>
                                    <li><a href='BasePage.aspx?ID=13'>Notice and Bulletin</a></li>
                                </ul>
                            </li>
                           <li><a href='#dropdown-tables' aria-expanded='false' data-toggle='collapse'><i class='la la-th-large'></i><span>Services</span></a>
                                <ul id ='dropdown-tables' class='collapse list-unstyled pt-0'>
                                  <li><a href = '#'> Transport </a></li>
                                  <li><a href='#'>Hostel</a></li>
                                  <li><a href = '#'> Feeding </a></li>
                                  <li><a href='#'>Coaching</a></li>
                                </ul>
                            </li>
                            <!--<li><a href = '#dropdown-new' aria-expanded='false' data-toggle='collapse'><i class='la la-street-view'></i><span>Smart Learn</span></a>
                                <ul id = 'dropdown-new' class='collapse list-unstyled pt-0'>
                                    <li><a href ='BasePage.aspx?ID=12' > Resources </a></li>
                                    <div id='containerURL' runat='server' class='f'> </div>
                                </ul>
                            </li>-->
                            <li><a href='#dropdown-new1' aria-expanded= 'false' data-toggle= 'collapse' ><i class='la la-bar-chart'></i><span>Reports</span></a>
                                <ul id ='dropdown-new1' class='collapse list-unstyled pt-0'>
                                    <!--<li><a href = '#'> Administration </a></li>
                                    <li><a href='#'>Admissions</a></li>-->
                                    <li><a href ='BasePage.aspx?ID=11'>Academics</a></li>
                                    <li><a href='#'>Fees and Payments</a></li>
                                </ul>
                            </li>
                           <!--<li><a href ='#dropdown-new2' aria-expanded='false' data-toggle='collapse'><i class='la la-graduation-cap'></i><span>Alumni</span></a>
                                <ul id = 'dropdown-new2' class='collapse list-unstyled pt-0'>
                                    <li><a href = '#'> Graduated Students</a></li>
                                </ul>
                            </li>-->
                            <li class=''><a href ='logout.aspx'' aria-expanded='false'><i class='la la-recycle'></i><span>Logout</span></a>
                        </ul>
                        <!--End Main Navigation-->
                    </nav>
                </div></div>";



        return ret;
    }


    public string SchoolOwnerSideMenu()
    {

        string ret = @"<div class='page-content d-flex align-items-stretch'>
                        <div class='default-sidebar'>
                    <nav class='side-navbar box-scroll sidebar-scroll'>
                              <!--Begin Main Navigation -->
                              <ul class='list-unstyled'>
                            <li class=''><a href ='Default.aspx' 'aria-expanded='false'><i class='la la-columns'></i><span> Home </span></a>
                            </li>
                        </ul>
                        <ul class='list-unstyled'>
                            <li><a href ='#dropdown-ui' aria-expanded='false' data-toggle='collapse'><i class='la la-sitemap'></i><span> Administration</span></a>
                                <ul id='dropdown-ui' class='collapse list-unstyled pt-0'>
                                    <li><a href='BasePage.aspx?ID=1'> Student </a></li>
                                    <li><a href='BasePage.aspx?ID=2'> Classes</a></li>
                                    <li><a href='BasePage.aspx?ID=3' > Staff </a></li>
                                    <li><a href='BasePage.aspx?ID=4'> School</a></li>
                                </ul>
                            </li>
                            <li><a href='#dropdown-icons' aria-expanded='false' data-toggle='collapse'><i class='la la-institution'></i><span>Admission</span></a>
                                <ul id='dropdown-icons' class='collapse list-unstyled pt-0'>
                                    <li><a href ='#'> Configuration / Application </a></li>
                                    <li><a href='#'>Payments</a></li>
                                    <li><a href ='#'> Test </a></li>
                                    <li><a href='#'>Interview</a></li>
                                    <li><a href ='#'> Admissions </a></li>
                                </ul>
                            </li>
                            <li><a href='#dropdown-app' aria-expanded='false' data-toggle='collapse'><i class='la la-book'></i><span> Academics</span></a>
                                <ul id ='dropdown-app' class='collapse list-unstyled pt-0'>
                                    <li><a href ='BasePage.aspx?ID=5'> Grading and Attendance</a><li>
                                    <li><a href ='BasePage.aspx?ID=6'> Syllabus and Notes</a></li>
                                    <li><a href ='BasePage.aspx?ID=7'> Subjects </a></li>
                                    <li><a href='BasePage.aspx?ID=10'> Assignments </a></li>
                                    </ul>
                                </li>
                             <li><a href='#dropdown-authentication' aria-expanded= 'false' data-toggle='collapse'><i class='la la-money'></i><span>Finance and Fees</span></a>
                                    <ul id='dropdown-authentication' class='collapse list-unstyled pt-0'>
                                        <li><a href='BasePage.aspx?ID=8'> Fees </a></li>
                                        <li><a href='BasePage.aspx?ID=9'>Payments</a></li>
                                    </ul>
                                </li>
                            <li><a href='#dropdown-forms' aria-expanded='false' data-toggle='collapse'><i class='la la-smile-o'></i><span>Socials Collaborations</span></a>
                                <ul id='dropdown-forms' class='collapse list-unstyled pt-0'>
                                    <li><a href='#'> Forums </a></li>
                                    <li><a href='#'>Chats</a></li>
                                    <li><a href ='#'> PTA </a></li>
                                    <li><a href='BasePage.aspx?ID=13'>Notice and Bulletin</a></li>
                                </ul>
                            </li>
                            <li><a href='#dropdown-tables' aria-expanded='false' data-toggle='collapse'><i class='la la-th-large'></i><span>Services</span></a>
                                <ul id ='dropdown-tables' class='collapse list-unstyled pt-0'>
                                  <li><a href = '#'> Transport </a></li>
                                  <li><a href='#'>Hostel</a></li>
                                  <li><a href = '#'> Feeding </a></li>
                                  <li><a href='#'>Coaching</a></li>
                                </ul>
                            </li>
                            <li><a href = '#dropdown-new' aria-expanded='false' data-toggle='collapse'><i class='la la-street-view'></i><span>Smart Learn</span></a>
                                <ul id = 'dropdown-new' class='collapse list-unstyled pt-0'>
                                    <li><a href ='BasePage.aspx?ID=12' > Resources </a></li>
                                    <div id='containerURL' runat='server' class='f'> </div>
                                </ul>
                            </li>
                            <li><a href='#dropdown-new1' aria-expanded= 'false' data-toggle= 'collapse' ><i class='la la-bar-chart'></i><span>Reports</span></a>
                                <ul id ='dropdown-new1' class='collapse list-unstyled pt-0'>
                                    <li><a href = '#'> Administration </a></li>
                                    <li><a href='#'>Admissions</a></li>
                                    <li><a href ='BasePage.aspx?ID=11'> Academics </a></li>
                                    <li><a href='#'>Finance</a></li>
                                </ul>
                            </li>
                            <li><a href ='#dropdown-new2' aria-expanded='false' data-toggle='collapse'><i class='la la-graduation-cap'></i><span>Alumni</span></a>
                                <ul id = 'dropdown-new2' class='collapse list-unstyled pt-0'>
                                    <li><a href = '#'> Graduated Students</a></li>
                                </ul>
                            </li>
                            <li class=''><a href ='logout.aspx'' aria-expanded='false'><i class='la la-recycle'></i><span>Logout</span></a>
                        </ul>
                        <!--End Main Navigation-->
                    </nav>
                </div></div>";

        return ret;

    }


    public string FinanceOfficerSideMenu()
    {

        string ret = @"<div class='page-content d-flex align-items-stretch'>
                        <div class='default-sidebar'>
                    <nav class='side-navbar box-scroll sidebar-scroll'>
                              <!--Begin Main Navigation -->
                              <ul class='list-unstyled'>
                            <li class=''><a href ='Default.aspx' 'aria-expanded='false'><i class='la la-columns'></i><span> Home </span></a>
                            </li>
                        </ul>
                        <ul class='list-unstyled'>
                           <!--<li><a href ='#dropdown-ui' aria-expanded='false' data-toggle='collapse'><i class='la la-sitemap'></i><span> Administration</span></a>
                                <ul id='dropdown-ui' class='collapse list-unstyled pt-0'>
                                    <li><a href='BasePage.aspx?ID=1'> Student </a></li>
                                  <li><a href='BasePage.aspx?ID=2'> Classes</a></li>
                                    <li><a href='BasePage.aspx?ID=3' > Staff </a></li>
                                    <li><a href='BasePage.aspx?ID=4'> School</a></li>
                                </ul>
                            </li>-->
                          <!--<li><a href='#dropdown-icons' aria-expanded='false' data-toggle='collapse'><i class='la la-institution'></i><span>Admission</span></a>
                                <ul id='dropdown-icons' class='collapse list-unstyled pt-0'>
                                    <li><a href ='#'> Configuration / Application </a></li>
                                    <li><a href='#'>Payments</a></li>
                                    <li><a href ='#'> Test </a></li>
                                    <li><a href='#'>Interview</a></li>
                                    <li><a href ='#'> Admissions </a></li>
                                </ul>
                            </li>-->
                            <!--<li><a href='#dropdown-app' aria-expanded='false' data-toggle='collapse'><i class='la la-book'></i><span> Academics</span></a>
                                <ul id ='dropdown-app' class='collapse list-unstyled pt-0'>
                                    <li><a href ='BasePage.aspx?ID=5'> Grading and Attendance</a><li>
                                    <li><a href ='BasePage.aspx?ID=6'> Syllabus and Notes</a></li>
                                    <li><a href ='BasePage.aspx?ID=7'> Subjects </a></li>
                                    <li><a href='BasePage.aspx?ID=10'> Assignments </a></li>
                                    </ul>
                                </li>-->
                            <li><a href='#dropdown-authentication' aria-expanded= 'false' data-toggle='collapse'><i class='la la-money'></i><span>Finance and Fees</span></a>
                                    <ul id='dropdown-authentication' class='collapse list-unstyled pt-0'>
                                        <li><a href='BasePage.aspx?ID=8'> Fees </a></li>
                                        <li><a href='BasePage.aspx?ID=9'>Payments</a></li>
                                    </ul>
                                </li>
                            <!--<li><a href='#dropdown-forms' aria-expanded='false' data-toggle='collapse'><i class='la la-smile-o'></i><span>Socials Collaborations</span></a>
                                <ul id='dropdown-forms' class='collapse list-unstyled pt-0'>
                                    <li><a href='#'> Forums </a></li>
                                    <li><a href='#'>Chats</a></li>
                                    <li><a href ='#'> PTA </a></li>
                                    <li><a href='BasePage.aspx?ID=13'>Notice and Bulletin</a></li>
                                </ul>
                            </li>-->
                        <!--<li><a href='#dropdown-tables' aria-expanded='false' data-toggle='collapse'><i class='la la-th-large'></i><span>Services</span></a>
                                <ul id ='dropdown-tables' class='collapse list-unstyled pt-0'>
                                  <li><a href = '#'> Transport </a></li>
                                  <li><a href='#'>Hostel</a></li>
                                  <li><a href = '#'> Feeding </a></li>
                                  <li><a href='#'>Coaching</a></li>
                                </ul>
                            </li>-->
                            <!--<li><a href = '#dropdown-new' aria-expanded='false' data-toggle='collapse'><i class='la la-street-view'></i><span>Smart Learn</span></a>
                                <ul id = 'dropdown-new' class='collapse list-unstyled pt-0'>
                                    <li><a href ='BasePage.aspx?ID=12' > Resources </a></li>
                                    <div id='containerURL' runat='server' class='f'> </div>
                                </ul>
                            </li>-->
                            <li><a href='#dropdown-new1' aria-expanded= 'false' data-toggle= 'collapse' ><i class='la la-bar-chart'></i><span>Reports</span></a>
                                <ul id ='dropdown-new1' class='collapse list-unstyled pt-0'>
                                    <!--<li><a href = '#'> Administration </a></li>
                                    <li><a href='#'>Admissions</a></li>
                                    <li><a href ='BasePage.aspx?ID=11'> Academics </a></li>-->
                                    <li><a href='#'>Finance</a></li>
                                </ul>
                            </li>
                            <!--<li><a href ='#dropdown-new2' aria-expanded='false' data-toggle='collapse'><i class='la la-graduation-cap'></i><span>Alumni</span></a>
                                <ul id = 'dropdown-new2' class='collapse list-unstyled pt-0'>
                                    <li><a href = '#'> Graduated Students</a></li>
                                </ul>
                            </li>-->
                            <li class=''><a href ='logout.aspx'' aria-expanded='false'><i class='la la-recycle'></i><span>Logout</span></a>
                        </ul>
                        <!--End Main Navigation-->
                    </nav>
                </div></div>";

        return ret;

    }

}



// links to PASSIS CBT 
//long x;
//long.TryParse(logonUser.UserRoles[0].RoleId.ToString(), out x);
//switch ((int)(x - (long)1))
//{
//    case 0:
//        {
//            ////this.Page.MasterPageFile = "~/Site.master";
//            //this.containerURL.InnerHtml = this.Dashboard_SuperAdmin();
//            break;
//        }
//    case 1:
//        {
//            //this.Page.MasterPageFile = "~/localAdmin.master";
//            this.containerURL.InnerHtml = @"<li><a href='http://localhost:8080/web/PHP/PassisCBT/Admin?schoolId=" + logonUser.SchoolId + @"&campusId=" + logonUser.SchoolCampusId + @"&userId=" + logonUser.Id + @"&roleType=Admin' target='_blank'>Computer Based Test</a></li>";
//            break;
//        }
//    case 2:
//        {
//            //this.Page.MasterPageFile = "~/teacher.master";
//            this.containerURL.InnerHtml = @"<li><a href='http://localhost:8080/web/PHP/PassisCBT/Admin?schoolId=" + logonUser.SchoolId + @"&campusId=" + logonUser.SchoolCampusId + @"&userId=" + logonUser.Id + @"&roleType=Admin' target='_blank'>Computer Based Test</a></li>";
//            break;
//        }
//    case 3:
//        {
//            //this.Page.MasterPageFile = "~/vprincipal.master";
//            this.containerURL.InnerHtml = @"<li><a href='http://localhost:8080/web/PHP/PassisCBT/Admin?schoolId=" + logonUser.SchoolId + @"&campusId=" + logonUser.SchoolCampusId + @"&userId=" + logonUser.Id + @"&roleType=Admin' target='_blank'>Computer Based Test</a></li>";
//            break;
//        }
//    case 4:
//        {
//            ////this.Page.MasterPageFile = "~/parent.master";
//            //this.containerURL.InnerHtml = this.Dashboard_Parent();
//            break;
//        }
//    case 5:
//        {
//            //this.Page.MasterPageFile = "~/student.master";
//            this.containerURL.InnerHtml = @"<li><a href='http://localhost:8080/web/PHP/PassisCBT/Student?schoolId=" + logonUser.SchoolId + @"&campusId=" + logonUser.SchoolCampusId + @"&userId=" + logonUser.Id + @"&classId=" + classId.Id + @"&gradeId=" + gradedId.Id + @"&roleType=Student' target='_blank'>Computer Based Test</a></li>";
//            break;
//        }
//    case 6:
//        {
//            ////this.Page.MasterPageFile = "~/finance.master";
//            //this.containerURL.InnerHtml = this.Dashboard_Finance();
//            break;
//        }
//    case 7:
//        {
//            //this.Page.MasterPageFile = "~/localAdmin.master";
//            this.containerURL.InnerHtml = @"<li><a href='http://localhost:8080/web/PHP/PassisCBT/Admin?schoolId=" + logonUser.SchoolId + @"&campusId=" + logonUser.SchoolCampusId + @"&userId=" + logonUser.Id + @"&roleType=Admin' target='_blank'>Computer Based Test</a></li>";
//            break;
//        }
//    case 8:
//        {
//            //this.Page.MasterPageFile = "~/subjectTeacher.master";
//            this.containerURL.InnerHtml = @"<li><a href='http://localhost:8080/web/PHP/PassisCBT/Admin?schoolId=" + logonUser.SchoolId + @"&campusId=" + logonUser.SchoolCampusId + @"&userId=" + logonUser.Id + @"&roleType=Admin' target='_blank'>Computer Based Test</a></li>";

//            break;
//        }
//    case 9:
//        {

//            break;
//        }
//    case 10:
//        {

//            break;
//        }
//    case 11:
//        {

//            break;
//        }
//    case 12:
//        {

//            break;
//        }
//    case 13:
//        {

//            break;
//        }
//    case 14:
//        {

//            //this.Page.MasterPageFile = "~/localAdmin.master";
//            this.containerURL.InnerHtml = @"<li><a href='http://localhost:8080/web/PHP/PassisCBT/Admin?schoolId=" + logonUser.SchoolId + @"&campusId=" + logonUser.SchoolCampusId + @"&userId=" + logonUser.Id + @"&roleType=Admin' target='_blank'>Computer Based Test</a></li>";
//            break;
//        }
//    case 15:
//        {

//            //this.Page.MasterPageFile = "~/localAdmin.master";
//            this.containerURL.InnerHtml = @"<li><a href='http://localhost:8080/web/PHP/PassisCBT/Admin?schoolId=" + logonUser.SchoolId + @"&campusId=" + logonUser.SchoolCampusId + @"&userId=" + logonUser.Id + @"&roleType=Admin' target='_blank'>Computer Based Test</a></li>";
//            break;
//        }
//    case 16:
//        {

//            //this.Page.MasterPageFile = "~/localAdmin.master";
//            this.containerURL.InnerHtml = @"<li><a href='http://localhost:8080/web/PHP/PassisCBT/Admin?schoolId=" + logonUser.SchoolId + @"&campusId=" + logonUser.SchoolCampusId + @"&userId=" + logonUser.Id + @"&roleType=Admin' target='_blank'>Computer Based Test</a></li>";
//            break;
//        }
//    default:
//        {
//            return;
//        }
//}

