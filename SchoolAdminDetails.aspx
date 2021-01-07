<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="SchoolAdminDetails.aspx.cs" Inherits="SchoolAdminDetails" %>

<%--<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>--%>
<%@ Import Namespace="PASSIS.DAO" %>
<%@ Import Namespace="PASSIS.LIB" %>
  <script runat="server">
    public string getHomeRoom(object IdObj)
    {
        string homeRoom = string.Empty;
        try
        {
            PASSIS.LIB.PASSISLIBDataContext cn = new PASSIS.LIB.PASSISLIBDataContext();
            PASSIS.LIB.GradeStudent gd = cn.GradeStudents.FirstOrDefault(g => g.StudentId == Convert.ToInt64(IdObj));
            PASSIS.LIB.Class_Grade cl = cn.Class_Grades.FirstOrDefault(c => c.Id == gd.ClassId);
            PASSIS.LIB.Grade gr = cn.Grades.FirstOrDefault(r => r.Id == gd.GradeId);
            homeRoom = string.Format("{0} {1}", cl.Name, gr.GradeName);
        }
        catch { }
        return homeRoom;
    }
</script>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="Server">  
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
<asp:FormView ID="fvwUser" runat="server"   DataKeyNames="Id" Width="100%" >        
        <EditItemTemplate>
                    <div class="content-inner">
                    <div class="container-fluid">
                        <!-- Begin Page Header-->
                      <%--  <div class="row">
                            <div class="page-header">
	                            <div class="d-flex align-items-center">
	                                <h2 class="page-header-title">PASSIS</h2>
	                                <div>
	                                <!-- <div class="page-header-tools">
	                                    <a class="btn btn-gradient-01" href="#">Login</a>
	                                </div> -->
	                                </div>
	                            </div>
                            </div>
                        </div>--%>
                        <!-- End Page Header -->
                        <!-- Begin Row -->
                        <div class="row flex-row" style="width:auto;">
                            <div class="col-12">
                                <!-- Form -->
                                <div class="widget has-shadow">
                                  <%--  <div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>NEW STUDENT</h4>
                                    </div>--%>
                                    <div class="widget-body"> 
                                        <form  class="form-horizontal">   
                                            <asp:Label ID="lblErrorPassportMsg" runat="server" Text="" Visible="false"></asp:Label>
                                            <asp:Label ID="lblError" runat="server" Text="" Visible="false"></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text="" Visible="false"></asp:Label>
                                            <div class="widget has-shadow">
                                                <div class="widget-header bordered no-actions d-flex align-items-center">
                                                 <h4>EDIT SCHOOL ADMINISTRATOR DETAILS</h4>
                                                </div>
                                             </div>
                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Campus:<span class="text-danger ml-2">*</span></label>
                                                <asp:DropDownList runat="server" ID="ddlCampus" class="custom-select form-control" DataTextField="Name" DataValueField="Id"
                                                    DataSource='<%#  SchoolCampusList()  %>' SelectedValue = '<%# Eval("SchoolCampusId") %>'>
                                                </asp:DropDownList>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">First Name:<span class="text-danger ml-2">*</span></label>
                                                <asp:TextBox runat="server" ID="txtFirstName" class="form-control" Text='<%# Bind("FirstName")  %>'> </asp:TextBox>
                                  
                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Last Name:<span class="text-danger ml-2">*</span></label>
                                                <asp:TextBox runat="server" ID="txtLastName" class="form-control" Text='<%# Bind("LastName")  %>'> </asp:TextBox>
                                            
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label"> Email Address::<span class="text-danger ml-2">*</span></label>
                                                <asp:TextBox runat="server" class="form-control" ID="txtEmailAddress" Text='<%# Bind("EmailAddress") %>'> </asp:TextBox>
                                                       
                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Gender:<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList runat="server" class="custom-select form-control" ID="ddlGender">
                                                    <asp:ListItem Text=" Male " Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Female " Value="2"></asp:ListItem>
                                                </asp:DropDownList>                                                
                                                </div>

                                                <div class="col-xl-6">
                                                  <label class="form-control-label">Phone Number::<span class="text-danger ml-2">*</span></label>
                                                <asp:TextBox runat="server" ID="txtPhoneNumber" class="form-control" Text='<%# Bind("PhoneNumber")  %>'> </asp:TextBox>
         
                                                    </div>
                                                    </div>
                                          <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">UserName:<span class="text-danger ml-2">*</span></label>
                                                 <asp:TextBox runat="server" class="form-control" ID="txtUsername" Text='<%# Bind("Username") %>'> </asp:TextBox>
   
                                                </div>
                                                <div class="col-xl-6">
                                                  <label class="form-control-label">Date Of Birth::<span class="text-danger ml-2">*</span></label>
                                    <asp:TextBox runat="server" class="form-control" ID="txtDateOfBirth" Text='<%# Eval("DateOfBirth", "{0:dd-MMM-yyyy }") %>'> </asp:TextBox>
                                <ajaxToolKit:CalendarExtender ID="myDateFromCal" Format="dd-MM-yyyy"  runat="server"  TargetControlID="txtDateOfBirth" PopupButtonID="ImageButton1" PopupPosition="BottomLeft"></ajaxToolKit:CalendarExtender>

                                                    </div>
                                                    </div>
                                        
                                          <br />
                                   
                                    <asp:Button ID="btnUpdate" class="btn btn-secondary" runat="server" Text="Update Details"  OnClick="btnUpdate_Click" />
                                   <asp:Button  ID="btnCancel" class="btn btn-secondary" runat="server" Text="Cancel"   OnClick="btnCancel_Click" />
                                                    </div>
                                          
                                                    </div>
                                
                                                 </div>
                                            </div>
                                                </div>
                                            </div>                                        
                                 </div>
                                </div>
            </form>
                                     </EditItemTemplate>



             <ItemTemplate>
                 <div class="content-inner">
                    <div class="container-fluid">
                        <!-- Begin Page Header-->
                      <%--  <div class="row">
                            <div class="page-header">
	                            <div class="d-flex align-items-center">
	                                <h2 class="page-header-title">PASSIS</h2>
	                                <div>
	                                <!-- <div class="page-header-tools">
	                                    <a class="btn btn-gradient-01" href="#">Login</a>
	                                </div> -->
	                                </div>
	                            </div>
                            </div>
                        </div>--%>
                        <!-- End Page Header -->
                        <!-- Begin Row -->
                        <div class="row flex-row" style="width:auto;">
                            <div class="col-12">
                                <!-- Form -->
                                <div class="widget has-shadow">
                                  <%--  <div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>NEW STUDENT</h4>
                                    </div>--%>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">
                                              <div class="widget has-shadow">
                                                <div class="widget-header bordered no-actions d-flex align-items-center">
                                                 <h4>SCHOOL ADMINISTRATOR DETAILS</h4>
                        &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:HyperLink ID="hypLnkReturnToList" runat="server" NavigateUrl="~/SupAdminSchoolAdmin.aspx"><i class="la la-mail-reply"></i> Return to List</asp:HyperLink>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:HyperLink ID="hypLnkEdit" runat="server" SkinID="HyperLinkb" NavigateUrl='<%# Eval("Id", "SchoolAdminDetails.aspx?mode=edit&id={0}") %>'><i class="la la-pencil"></i> Edit</asp:HyperLink>
                                                </div>
                                             </div>
                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">School Campus:<span class="text-danger ml-2">*</span></label>&nbsp;&nbsp;&nbsp;
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" Text='<%#  new AcademicSessionLIB().RetrieveSchoolCampus((Int64)Eval("SchoolCampusId")).Name   %>' class="form-control-label"></asp:Label>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">UserName:<span class="text-danger ml-2">*</span></label>
                                                  <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="lblUserName" class="form-control-label" Text='<%# Bind("UserName") %>'> </asp:Label>

                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">FirstName:<span class="text-danger ml-2">*</span></label>
                                               <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="lblFirstName" Text='<%# Bind("FirstName")  %>' class="form-control-label"></asp:Label>

                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label"> LastName:<span class="text-danger ml-2">*</span></label>
                                                 <asp:Label runat="server"  ForeColor="Blue" Font-Bold="true" ID="lblFamilyName" class="form-control-label" Text='<%# Bind("LastName")  %>'> </asp:Label>

                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Phone Number:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="lblsjhsd" Text='<%# Bind("PhoneNumber") %>' class="form-control-label"></asp:Label>
                                                </div>
                                                <div class="col-xl-6">
                                                  <label class="form-control-label">Gender:<span class="text-danger ml-2">*</span></label>
                                                   <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="Label18" Text='<%#  (PASSIS.DAO.Utility.Gender) Eval("Gender") %>' class="form-control-label"></asp:Label>
  
                                                    </div>
                                                    </div>
                                          <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Email Address:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="Label38" Text='<%#  Eval("EmailAddress") %>' class="form-control-label"></asp:Label>
                                                </div>
                                                <div class="col-xl-6">
                                                  <label class="form-control-label">Date Of Birth:<span class="text-danger ml-2">*</span></label>
                                            <asp:Label runat="server" ID="Label19" ForeColor="Blue" Font-Bold="true" Text='<%# Eval("DateOfBirth", "{0:dd-MMM-yyyy }") %>' class="form-control-label"></asp:Label>

                                                    </div>
                                                    </div>
                                              
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        
                                        </form>

                                 </div>
                                </div>
                                </ItemTemplate>
                                 </asp:FormView>
   <asp:ObjectDataSource ID="objStudent" runat="server" DataObjectTypeName="PASSIS.DAO.User"
        SelectMethod="RetrieveUser" TypeName="PASSIS.DAO.UsersDAL">
        <SelectParameters>
            <asp:QueryStringParameter DefaultValue="0" Name="id" QueryStringField="Id" Type="Int64" />
        </SelectParameters>
    </asp:ObjectDataSource>
           </asp:Content>
