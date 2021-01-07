using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PASSIS.LIB;
using PASSIS.LIB.Utility;
using System.Data.SqlClient;

public partial class _Default : PASSIS.LIB.Utility.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            long x;
            long.TryParse(logonUser.UserRoles[0].RoleId.ToString(), out x);
            switch ((int)(x - (long)1))
            {
                case 0:
                    {
                        //this.Page.MasterPageFile = "~/Site.master";
                        this.containerDiv.InnerHtml = this.Dashboard_SuperAdmin();
                        break;
                    }
                case 1:
                    {
                        //this.Page.MasterPageFile = "~/localAdmin.master";
                        this.containerDiv.InnerHtml = this.Dashboard_SystemAdmin();
                        break;
                    }
                case 2:
                    {
                        //this.Page.MasterPageFile = "~/teacher.master";
                        this.containerDiv.InnerHtml = this.Dashboard_Teacher();
                        break;
                    }
                case 3:
                    {
                        //this.Page.MasterPageFile = "~/vprincipal.master";
                        this.containerDiv.InnerHtml = this.Dashboard_Teacher();
                        break;
                    }
                case 4:
                    {
                        //this.Page.MasterPageFile = "~/parent.master";
                        this.containerDiv.InnerHtml = this.Dashboard_Parent();
                        break;
                    }
                case 5:
                    {
                        //this.Page.MasterPageFile = "~/student.master";
                        this.containerDiv.InnerHtml = this.Dashboard_Student();
                        break;
                    }
                case 6:
                    {
                        //this.Page.MasterPageFile = "~/finance.master";
                        this.containerDiv.InnerHtml = this.Dashboard_Finance();
                        break;
                    }
                case 7:
                    {
                        //this.Page.MasterPageFile = "~/localAdmin.master";
                        this.containerDiv.InnerHtml = this.Dashboard_SystemAdmin();
                        break;
                    }
                case 8:
                    {
                        //this.Page.MasterPageFile = "~/subjectTeacher.master";
                        this.containerDiv.InnerHtml = this.Dashboard_Teacher();

                        break;
                    }
                case 9:
                    {
                        //this.Page.MasterPageFile = "~/principal.master";
                        this.containerDiv.InnerHtml = this.Dashboard_Principal();

                        break;
                    }
                case 10:
                    {

                        break;
                    }
                case 11:
                    {

                        break;
                    }
                case 12:
                    {

                        //this.Page.MasterPageFile = "~/AdmissionOfficer.master";
                        //this.containerDiv.InnerHtml = this.Dashboard_AdmissionOfficer();
                        break;
                    }
                case 13:
                    {

                        break;
                    }
                case 14:
                    {

                        //this.Page.MasterPageFile = "~/localAdmin.master";
                        this.containerDiv.InnerHtml = this.Dashboard_SystemAdmin();
                        break;
                    }
                case 15:
                    {

                        //this.Page.MasterPageFile = "~/localAdmin.master";
                        this.containerDiv.InnerHtml = this.Dashboard_SystemAdmin();
                        break;
                    }
                case 16:
                    {

                        //this.Page.MasterPageFile = "~/localAdmin.master";
                        this.containerDiv.InnerHtml = this.Dashboard_SystemAdmin();
                        break;
                    }
                case 17:
                    {

                        //this.Page.MasterPageFile = "~/localAdmin.master";
                        this.containerDiv.InnerHtml = this.Dashboard_SchOwner();
                        break;
                    }
                case 18:
                    {

                        ////this.Page.MasterPageFile = "~/localAdmin.master";
                        //this.containerDiv.InnerHtml = this.Dashboard_SchOwner();
                        break;
                    }
                default:
                    {
                        return;
                    }
            }
            //}
        }
        catch (Exception exception)
        {
            Exception ex = exception;
            PASSIS.LIB.PAConfig.LogAudit(PASSIS.LIB.PAConfig.AuditAction.Login, Request.Url.AbsoluteUri, ex.Message.ToString(), logonUser.Id);
            base.Response.Redirect("~/Login.aspx");
            throw ex;
        }
    }

    public string Dashboard_SuperAdmin()
    {
        //Schools
        IList<School> school = new PASSIS.LIB.SchoolConfigLIB().getAllSchools();

        string ret = @"<div class='row'>
                            <div class='page-header'>
	                            <div class='d-flex align-items-center'>
	                                <h2 class='page-header-title'>SUPER ADMINISTRATOR DASHBOARD</h2>
	                              
	                            </div>
                            </div>
                        </div>
                    <div class='row flex-row'>
                                <div class='col-xl-4 col-md-6 col-sm-6'>
                                <div class='widget widget-12 has-shadow'>
                                    <div class='widget-body'>
                                        <div class='media'>
                                            <div class='align-self-center ml-5 mr-5'>
                                                <i class='la la-university'></i>
                                            </div>
                                            <div class='media-body align-self-center'>
                                                <div class='title'>SCHOOLS</div>
                                                <div class='number'>" + school.Count + @"</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                             </div>
                        </div>";

         
        return ret;
    }
    public string Dashboard_SystemAdmin()
    {
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);
        //Students tSe
        IList<User> students = new UsersLIB().RetrieveStudents((long)logonUser.SchoolId, logonUser.SchoolCampusId, 0, 0, curSessionId);
        //staff
        IList<User> staffs = new PASSIS.LIB.UsersLIB().RetrieveUsersBelowRole(2, (long)logonUser.SchoolId, 5, logonUser.SchoolCampusId);
        string ret = @"<div class='row'>
                            <div class='page-header'>
	                            <div class='d-flex align-items-center'>
	                                <h2 class='page-header-title'>ADMINISTRATOR DASHBOARD</h2>
	                              
	                            </div>
                            </div>
                        </div>
                    <div class='row flex-row'>
                        <div class='col-xl-4 col-md-6 col-sm-6'>
                                <div class='widget widget-12 has-shadow'>
                                    <div class='widget-body'>
                                        <div class='media'>
                                            <div class='align-self-center ml-5 mr-5'>
                                                <i class='ion-ios-people'></i>
                                            </div>
                                            <div class='media-body align-self-center'>
                                                <div class='title'>STUDENT</div>
                                                <div class='number'>" + students.Count + @"</div>
                                            </div>
                                        </div>
                                    </div>
                                </div></div>
                          
                            
                            <div class='col-xl-4 col-md-6 col-sm-6'>
                                <div class='widget widget-12 has-shadow'>
                                    <div class='widget-body'>
                                        <div class='media'>
                                            <div class='align-self-center ml-5 mr-5'>
                                                <i class='ion-person'></i>
                                            </div>
                                            <div class='media-body align-self-center'>
                                                <div class='title'>STAFF</div>
                                                <div class='number'>" + staffs.Count + @"</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                           
                            <div class='col-xl-4 col-md-6 col-sm-6'>
                                <div class='widget widget-12 has-shadow'>
                                    <div class='widget-body'>
                                        <div class='media'>
                                            <div class='align-self-center ml-5 mr-5'>
                                                <i class='ion-clipboard'></i>
                                            </div>
                                            <div class='media-body align-self-center'>
                                                <div class='title'>ADMISSION</div>
                                                <div class='number'>   0   </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>";

        return ret;
    }


    public string Dashboard_SchOwner()
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);
        //Students tSe
        IList<User> students = new UsersLIB().RetrieveStudents((long)logonUser.SchoolId, logonUser.SchoolCampusId, 0, 0, curSessionId);
        //staff
        IList<User> staffs = new PASSIS.LIB.UsersLIB().RetrieveUsersBelowRole(2, (long)logonUser.SchoolId, 5, logonUser.SchoolCampusId);

        //Total Payments
        var total = from s in context.PaymentPermanents where s.SchoolId == logonUser.SchoolId select s;

        decimal totalAmountPaid = 0;
        decimal totalOutstanding = 0;
        decimal totalAmountGenerated = 0;

        foreach (var totalAmountPaids in total)
        {
            totalAmountPaid = (decimal)totalAmountPaids.AmountPaid + totalAmountPaid;
            totalOutstanding = (decimal)totalAmountPaids.Balance + totalOutstanding;
            totalAmountGenerated = (decimal)totalAmountPaids.AmountGenerated + totalAmountGenerated;

        }


        string ret = @"<div class='row'>
                            <div class='page-header'>
	                            <div class='d-flex align-items-center'>
	                                <h2 class='page-header-title'>SCHOOL OWNER DASHBOARD</h2>
	                              
	                            </div>
                            </div>
                        </div>
                    <div class='row flex-row'>
                        <div class='col-xl-4 col-md-6 col-sm-6'>
                                <div class='widget widget-12 has-shadow'>
                                    <div class='widget-body'>
                                        <div class='media'>
                                            <div class='align-self-center ml-5 mr-5'>
                                                <i class='ion-ios-people'></i>
                                            </div>
                                            <div class='media-body align-self-center'>
                                                <div class='title'>STUDENT</div>
                                                <div class='number'>" + students.Count + @"</div>
                                            </div>
                                        </div>
                                    </div>
                                </div></div>
                       
                            
                            <div class='col-xl-4 col-md-6 col-sm-6'>
                                <div class='widget widget-12 has-shadow'>
                                    <div class='widget-body'>
                                        <div class='media'>
                                            <div class='align-self-center ml-5 mr-5'>
                                                <i class='ion-person'></i>
                                            </div>
                                            <div class='media-body align-self-center'>
                                                <div class='title'>STAFF</div>
                                                <div class='number'>" + staffs.Count + @"</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class='col-xl-4 col-md-6 col-sm-6'>
                                <div class='widget widget-12 has-shadow'>
                                    <div class='widget-body'>
                                        <div class='media'>
                                            <div class='align-self-center ml-5 mr-5'>
                                                <i class='ion-clipboard'></i>
                                            </div>
                                            <div class='media-body align-self-center'>
                                                <div class='title'>TOTAL ADMISSION</div>
                                                <div class='number'>0</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        <div class='col-xl-4 col-md-6 col-sm-6'>
                                <div class='widget widget-12 has-shadow'>
                                    <div class='widget-body'>
                                        <div class='media'>
                                            <div class='align-self-center ml-5 mr-5'>
                                                <i class='la la-money'></i>
                                            </div>
                                            <div class='media-body align-self-center'>
                                                <div class='title'>AMOUNT GENERATED</div>
                                                <div class='number'>" + totalAmountGenerated.ToString() + @"</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class='col-xl-4 col-md-6 col-sm-6'>
                                <div class='widget widget-12 has-shadow'>
                                    <div class='widget-body'>
                                        <div class='media'>
                                            <div class='align-self-center ml-5 mr-5'>
                                                <i class='la la-money'></i>
                                            </div>
                                            <div class='media-body align-self-center'>
                                                <div class='title'>AMOUNT PAID</div>
                                                <div class='number'>" + totalAmountPaid.ToString() + @"</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class='col-xl-4 col-md-6 col-sm-6'>
                                <div class='widget widget-12 has-shadow'>
                                    <div class='widget-body'>
                                        <div class='media'>
                                            <div class='align-self-center ml-5 mr-5'>
                                                <i class='la la-money'></i>
                                            </div>
                                            <div class='media-body align-self-center'>
                                                <div class='title'>OUTSTANDING</div>
                                                <div class='number'>" + totalOutstanding.ToString() + @"</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
 
                           
                        </div>";

        return ret;
    }


    public string Dashboard_Principal()
    {
        PASSISLIBDataContext context = new PASSISLIBDataContext();
        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);
        //Students tSe
        IList<User> students = new UsersLIB().RetrieveStudents((long)logonUser.SchoolId, logonUser.SchoolCampusId, 0, 0, curSessionId);
        //staff
        IList<User> staffs = new PASSIS.LIB.UsersLIB().RetrieveUsersBelowRole(2, (long)logonUser.SchoolId, 5, logonUser.SchoolCampusId);

        //Total Payments
        var total = from s in context.PaymentPermanents where s.SchoolId == logonUser.SchoolId select s;

        decimal totalAmountPaid = 0;
        decimal totalOutstanding = 0;
        decimal  totalAmountGenerated= 0;

        foreach (var totalAmountPaids in total)
        {
            totalAmountPaid = (decimal)totalAmountPaids.AmountPaid + totalAmountPaid;
            totalOutstanding = (decimal)totalAmountPaids.Balance + totalOutstanding;
            totalAmountGenerated = (decimal)totalAmountPaids.AmountGenerated + totalAmountGenerated;

        }


        string ret = @"<div class='row'>
                            <div class='page-header'>
	                            <div class='d-flex align-items-center'>
	                                <h2 class='page-header-title'>PRINCIPAL DASHBOARD</h2>
	                              
	                            </div>
                            </div>
                        </div>
                    <div class='row flex-row'>
                        <div class='col-xl-4 col-md-6 col-sm-6'>
                                <div class='widget widget-12 has-shadow'>
                                    <div class='widget-body'>
                                        <div class='media'>
                                            <div class='align-self-center ml-5 mr-5'>
                                                <i class='ion-ios-people'></i>
                                            </div>
                                            <div class='media-body align-self-center'>
                                                <div class='title'>STUDENT</div>
                                                <div class='number'>" + students.Count + @"</div>
                                            </div>
                                        </div>
                                    </div>
                                </div></div>
                       
                            
                            <div class='col-xl-4 col-md-6 col-sm-6'>
                                <div class='widget widget-12 has-shadow'>
                                    <div class='widget-body'>
                                        <div class='media'>
                                            <div class='align-self-center ml-5 mr-5'>
                                                <i class='ion-person'></i>
                                            </div>
                                            <div class='media-body align-self-center'>
                                                <div class='title'>STAFF</div>
                                                <div class='number'>" + staffs.Count + @"</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class='col-xl-4 col-md-6 col-sm-6'>
                                <div class='widget widget-12 has-shadow'>
                                    <div class='widget-body'>
                                        <div class='media'>
                                            <div class='align-self-center ml-5 mr-5'>
                                                <i class='ion-clipboard'></i>
                                            </div>
                                            <div class='media-body align-self-center'>
                                                <div class='title'>TOTAL ADMISSION</div>
                                                <div class='number'>0</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                         <div class='col-xl-4 col-md-6 col-sm-6'>
                                <div class='widget widget-12 has-shadow'>
                                    <div class='widget-body'>
                                        <div class='media'>
                                            <div class='align-self-center ml-5 mr-5'>
                                                <i class='la la-money'></i>
                                            </div>
                                            <div class='media-body align-self-center'>
                                                <div class='title'>AMOUNT GENERATED</div>
                                                <div class='number'>" + totalAmountGenerated.ToString() + @"</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class='col-xl-4 col-md-6 col-sm-6'>
                                <div class='widget widget-12 has-shadow'>
                                    <div class='widget-body'>
                                        <div class='media'>
                                            <div class='align-self-center ml-5 mr-5'>
                                                <i class='la la-money'></i>
                                            </div>
                                            <div class='media-body align-self-center'>
                                                <div class='title'>AMOUNT PAID</div>
                                                <div class='number'>" + totalAmountPaid.ToString() + @"</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class='col-xl-4 col-md-6 col-sm-6'>
                                <div class='widget widget-12 has-shadow'>
                                    <div class='widget-body'>
                                        <div class='media'>
                                            <div class='align-self-center ml-5 mr-5'>
                                                <i class='la la-money'></i>
                                            </div>
                                            <div class='media-body align-self-center'>
                                                <div class='title'>OUTSTANDING</div>
                                                <div class='number'>" + totalOutstanding.ToString() + @"</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
 
                           
                        </div>";

        return ret;
    }

    public string Dashboard_Teacher()
    {
        //Students

        if (!isUserClassTeacher)
        {

            IList<AssignmentSubmitted> assignments = new AssignmentLIB().getSubmittedAssignment((long)logonUser.Id);
            //Groups
            IList<Grouping> groups = new GroupingLIB().RetrieveTeacherGrouping(logonUser.Id);

            string ret = @"<div class='row'>
                            <div class='page-header'>
	                            <div class='d-flex align-items-center'>
	                                <h2 class='page-header-title'>SUBJECT TEACHER DASHBOARD</h2>
	                              
	                            </div>
                            </div>
                        </div>
                   
                            <div class='col-xl-4 col-md-6 col-sm-6'>
                                <div class='widget widget-12 has-shadow'>
                                    <div class='widget-body'>
                                        <div class='media'>
                                            <div class='align-self-center ml-5 mr-5'>
                                                <i class='la la-book'></i>
                                            </div>
                                            <div class='media-body align-self-center'>
                                                <div class='title'>ASSIGNMENTS</div>
                                                <div class='number'>" + assignments.Count + @"</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>";
            return ret;
        }

        else
        {
            long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);
            //Students tSe
            IList<GradeStudent> students = new PASSIS.LIB.ClassGradeLIB().getAllGradeStudents(logonUser.Id, (long)logonUser.SchoolId, logonUser.SchoolCampusId, curSessionId);
            //Assignments
            IList<AssignmentSubmitted> assignments = new AssignmentLIB().getSubmittedAssignment((long)logonUser.Id);
            //Groups
            IList<Grouping> groups = new GroupingLIB().RetrieveTeacherGrouping(logonUser.Id);

            string ret = @"<div class='row'>
                            <div class='page-header'>
	                            <div class='d-flex align-items-center'>
	                                <h2 class='page-header-title'>CLASS TEACHER DASHBOARD</h2>
	                              
	                            </div>
                            </div>
                        </div>
                   <div class='row flex-row'>
                        <div class='col-xl-4 col-md-6 col-sm-6'>
                                <div class='widget widget-12 has-shadow'>
                                    <div class='widget-body'>
                                        <div class='media'>
                                            <div class='align-self-center ml-5 mr-5'>
                                                <i class='ion-ios-people'></i>
                                            </div>
                                            <div class='media-body align-self-center'>
                                                <div class='title'>STUDENT</div>
                                                <div class='number'>" + students.Count + @"</div>
                                            </div>
                                        </div>
                                    </div>
                                </div></div>
                          
                            
                            <div class='col-xl-4 col-md-6 col-sm-6'>
                                <div class='widget widget-12 has-shadow'>
                                    <div class='widget-body'>
                                        <div class='media'>
                                            <div class='align-self-center ml-5 mr-5'>
                                                <i class='la la-book'></i>
                                            </div>
                                            <div class='media-body align-self-center'>
                                                <div class='title'>ASSIGNMENTS</div>
                                                <div class='number'>" + assignments.Count + @"</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                           
                            
                        </div>";
            return ret;
        }
        return "";
    }


    public string Dashboard_Finance()
    {
        int fees = 0;
        clsMyDB mdb = new clsMyDB();
        mdb.connct();
        string query = "SELECT * FROM Fee WHERE SchoolId=" + logonUser.SchoolId;
        SqlDataReader reader = mdb.fetch(query);
        while (reader.Read())
        {
            fees++;
        }
        reader.Close();
        mdb.closeConnct();
        PASSISLIBDataContext context = new PASSISLIBDataContext();

        var listFee = from s in context.PaymentFeeTypes where s.SchoolId == logonUser.SchoolId select s;
        var feeTemplates = from s in context.PaymentFeeTemplates where s.SchoolId == logonUser.SchoolId select s;

        string ret = @"<div class='row'>
                            <div class='page-header'>
	                            <div class='d-flex align-items-center'>
	                                <h2 class='page-header-title'>ACCOUNT/FINANCE OFFICER DASHBOARD</h2>
	                              
	                            </div>
                            </div>
                        </div>
                     <div class='row flex-row'>
                        <div class='col-xl-4 col-md-6 col-sm-6'>
                                <div class='widget widget-12 has-shadow'>
                                    <div class='widget-body'>
                                        <div class='media'>
                                            <div class='align-self-center ml-5 mr-5'>
                                                <i class='la la-money'></i>
                                            </div>
                                            <div class='media-body align-self-center'>
                                                <div class='title'>FEE TYPES</div>
                                                <div class='number'>" + listFee.Count() + @"</div>
                                            </div>
                                        </div>
                                    </div>
                                </div> </div>
                          
                            
                            <div class='col-xl-4 col-md-6 col-sm-6'>
                                <div class='widget widget-12 has-shadow'>
                                    <div class='widget-body'>
                                        <div class='media'>
                                            <div class='align-self-center ml-5 mr-5'>
                                                <i class='la la-money'></i>
                                            </div>
                                            <div class='media-body align-self-center'>
                                                <div class='title'>TEMPLATES</div>
                                                <div class='number'> " + feeTemplates.Count() + @"   </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                           
                            
                        </div>";

        return ret;
    }


    public string Dashboard_Parent()
    {
        //Wards
        IList<PASSIS.LIB.User> wards = new PASSIS.LIB.UsersLIB().RetrieveParentsChildren(logonUser.Id);



        string ret = @"<div class='row'>
                            <div class='page-header'>
	                            <div class='d-flex align-items-center'>
	                                <h2 class='page-header-title'>PARENT DASHBOARD</h2>
	                              
	                            </div>
                            </div>
                        </div>
                   <div class='row flex-row'>
                        <div class='col-xl-4 col-md-6 col-sm-6'>
                                <div class='widget widget-12 has-shadow'>
                                    <div class='widget-body'>
                                        <div class='media'>
                                            <div class='align-self-center ml-5 mr-5'>
                                                <i class='ion-ios-people'></i>
                                            </div>
                                            <div class='media-body align-self-center'>
                                                <div class='title'>CHILD/CHILDREN</div>
                                                <div class='number'>" + wards.Count + @"</div>
                                            </div>
                                        </div>
                                    </div>
                                </div></div>
                                                     
                            <div class='col-xl-4 col-md-6 col-sm-6'>
                                <div class='widget widget-12 has-shadow'>
                                    <div class='widget-body'>
                                        <div class='media'>
                                            <div class='align-self-center ml-5 mr-5'>
                                                <i class='la la-money'></i>
                                            </div>
                                            <div class='media-body align-self-center'>
                                                <div class='title'>FEES</div>
                                                <div class='number'>     </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                           
                            
                        </div> ";

        return ret;
    }

    public string Dashboard_Student()
    {
        //Wards
        IList<PASSIS.LIB.User> wards = new PASSIS.LIB.UsersLIB().RetrieveParentsChildren(logonUser.Id);

        PASSISLIBDataContext context = new PASSISLIBDataContext();
        Int64 userId = logonUser.Id;
        Int64 StudentGradeId = 0L;
        Int64 StudentClassId = 0L;

        long curSessionId = new AcademicSessionLIB().GetCurrentSessionId(logonUser.SchoolId);

        try
        {
            StudentGradeId = new ClassGradeLIB().RetrieveStudentGrade(userId, curSessionId).GradeId;
            StudentClassId = new ClassGradeLIB().RetrieveStudentGrade(userId, curSessionId).ClassId;
        }
        catch { }
       
        PASSIS.LIB.Grade gradeName = context.Grades.FirstOrDefault(x => x.Id == StudentGradeId);
        PASSIS.LIB.Class_Grade clsGrade = context.Class_Grades.FirstOrDefault(x => x.Id == StudentClassId);

        var getStdAssignment = from s in context.Assignments where s.GradeId == StudentGradeId select s;


        string ret = @"<div class='row'>
                            <div class='page-header'>
	                            <div class='d-flex align-items-center'>
	                                <h2 class='page-header-title'>STUDENT DASHBOARD</h2>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                  <h3 class='page-header-title'>CLASS:&nbsp;" + clsGrade.Name.ToString()  + @"&nbsp;&nbsp; GRADE/ARM:&nbsp;" + gradeName.GradeName.ToString()  + @"</h3>
	                              
	                            </div>
                            </div>
                        </div>
                   <div class='row flex-row'>
                        <div class='col-xl-4 col-md-6 col-sm-6'>
                                <div class='widget widget-12 has-shadow'>
                                    <div class='widget-body'>
                                        <div class='media'>
                                            <div class='align-self-center ml-5 mr-5'>
                                                <i class='ion-clipboard'></i>
                                            </div>
                                            <div class='media-body align-self-center'>
                                                <div class='title'>ASSIGNMENT</div>
                                                <div class='number'>" + getStdAssignment.Count() + @"</div>
                                            </div>
                                        </div>
                                    </div>
                                </div></div>

                          <div class='col-xl-4 col-md-6 col-sm-6'>
                                <div class='widget widget-12 has-shadow'>
                                    <div class='widget-body'>
                                        <div class='media'>
                                            <div class='align-self-center ml-5 mr-5'>
                                                <i class='ion-clipboard'></i>
                                            </div>
                                            <div class='media-body align-self-center'>
                                                <div class='title'>SUBMMITTED ASSIGNMENT</div>
                                                <div class='number'>  </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                           
                            
                        </div> ";

        return ret;
    }



}

