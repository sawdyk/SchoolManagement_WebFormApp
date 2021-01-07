<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="StaffDetail.aspx.cs" Inherits="StaffDetail" %>

<%--<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>--%>
<%@ Import Namespace="PASSIS.DAO" %>
<%@ Import Namespace="PASSIS.LIB" %>
  
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="Server">  
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
     <asp:FormView ID="fvwUser" runat="server" DataSourceID="objStaff" DataKeyNames="Id" dataOnDataBound="fvwUser_DataBound"
        Width="100%">
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
                                                 <h4>EDIT STAFF DETAILS</h4>
                                                </div>
                                             </div>
                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">UserName:<span class="text-danger ml-2">*</span></label>
                                                  <asp:TextBox runat="server" Text='<%# Bind("Username") %>' ID="txtUsername" class="form-control"> </asp:TextBox>

                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Email:<span class="text-danger ml-2">*</span></label>
                                                <asp:TextBox runat="server" ID="txtEmailAddress" Text='<%# Bind("EmailAddress")  %>' class="form-control"> </asp:TextBox>
                                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmailAddress"
                                        ErrorMessage="Email entry is incorrect." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>                                       

                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">PhoneNumber:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtPhoneNumber" Text='<%# Bind("PhoneNumber")  %>' class="form-control" placeholder="FirstName"></asp:TextBox>
                                            
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label"> FirstName:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtFirstName" Text='<%# Bind("FirstName")  %>' class="form-control" placeholder="LastName"></asp:TextBox>
                                                       
                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">LastName:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtLastName" Text='<%# Bind("LastName") %>' class="form-control" placeholder="LastName"></asp:TextBox>
                                                </div>
                                                <div class="col-xl-6">
                                                  <label class="form-control-label">MiddleName:<span class="text-danger ml-2">*</span></label>
                                               <asp:TextBox runat="server" ID="txtMiddleName" Text='<%# Bind("MiddleName") %>' class="form-control" placeholder="MiddleName"></asp:TextBox>
         
                                                    </div>
                                                    </div>
                                          <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Gender:<span class="text-danger ml-2">*</span></label>
                                                 <asp:DropDownList runat="server" class="custom-select form-control" ID="ddlGender" SelectedValue='<%# (int)Eval("Gender")  %>'>
                                                     <asp:ListItem Text=" Male " Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Female " Value="2"></asp:ListItem>
                                                     </asp:DropDownList>    
                                                </div>
                                                <div class="col-xl-6">
                                                  <label class="form-control-label">School Campus:<span class="text-danger ml-2">*</span></label>
                                             <asp:DropDownList ID="ddlCampus" runat="server" AutoPostBack="true" AppendDataBoundItems="true"
                                        DataTextField="Name" DataValueField="Id" class="custom-select form-control" DataSource="<%# schCampus %>">
                                    </asp:DropDownList>
                                                    </div>
                                                    </div>
                                          <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Status:<span class="text-danger ml-2">*</span></label>
                                <asp:DropDownList runat="server" DataTextField="Value" class="custom-select form-control" DataValueField="Key" ID="ddlUserStatus" AppendDataBoundItems="true" DataSource="<%# UserStatusEnumeration %>">
                                    </asp:DropDownList>                                                
                                                </div>
                                                <div class="col-xl-6">
                                                  <label class="form-control-label">Role:<span class="text-danger ml-2">*</span></label>
                                <asp:DropDownList ID="ddlRole" runat="server" AutoPostBack="true" AppendDataBoundItems="true"
                                        DataTextField="RoleName" class="custom-select form-control" DataValueField="Id" DataSource="<%# schRole %>">
                                    </asp:DropDownList>         
                                                    </div>
                                                    </div>
                                          <br />
                                   
                                    <asp:Button ID="btnUpdate" class="btn btn-secondary" runat="server" Text="Update Details"  OnClick="btnUpdate_OnClick" />
                                   <asp:Button  ID="btnCancel" class="btn btn-secondary" runat="server" Text="Cancel"   OnClick="btnCancel_OnClick" />
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
                                                 <h4>STAFF DETAILS</h4>
                        &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/ViewStaff.aspx"><i class="fa fa-mail-reply"></i> Return to List</asp:HyperLink>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:HyperLink ID="hypLnk" runat="server" SkinID="HyperLinkb" NavigateUrl='<%# Eval("Id", "StaffDetail.aspx?mode=edit&id={0}") %>'><i class="la la-pencil"></i> Edit</asp:HyperLink>
                                                </div>
                                             </div>
                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">School Campus:<span class="text-danger ml-2">*</span></label>&nbsp;&nbsp;&nbsp;
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" Text='<%#  new AcademicSessionLIB().RetrieveSchoolCampus((Int64)Eval("SchoolCampusId")).Name   %>' class="form-control-label"></asp:Label>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">UserName:<span class="text-danger ml-2">*</span></label>
                                                  <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="lblUserName" class="form-control-label" Text='<%# Bind("Username") %>'> </asp:Label>

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
                                                   <label class="form-control-label">MiddleName:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="lblsjhsd" Text='<%# Bind("MiddleName") %>' class="form-control-label"></asp:Label>
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
                                                  <label class="form-control-label">Phone Number:<span class="text-danger ml-2">*</span></label>
                                            <asp:Label runat="server" ID="Label19" ForeColor="Blue" Font-Bold="true" Text='<%# Eval("PhoneNumber") %>' class="form-control-label"></asp:Label>

                                                    </div>
                                                    </div>
                                          <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Status:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ID="Label39" ForeColor="Blue" Font-Bold="true" Text='<%# (PASSIS.LIB.Utility.UserStatus) Eval("UserStatus") %>' class="form-control-label"></asp:Label>
                                                </div>
                                                <div class="col-xl-6">
                                                  <label class="form-control-label">Role:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ID="lblRo" ForeColor="Blue" Font-Bold="true" Text='<%# RoleName((long)Eval("Id")) %>' class="form-control-label"></asp:Label>
         
                                                    </div>
                                                    </div>
                                          
                                                
                                  
                                 <%--<div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>CLASS ARMS</h4>
                                 </div>--%>
                            <%--<asp:GridView ID="gvdAcademicSession" runat="server" AutoGenerateColumns="False" EmptyDataText=" No Calendar year currently exist" AllowPaging="true" Width="100%" ShowFooter="True" PageSize="20" GridLines="Both" OnRowCommand="gvdAcademicSession_RowCommand">       
                            <PagerSettings FirstPageText="&lt;i class=&quot;fa  fa-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-left&quot;&gt;&lt;/i&gt;"
                                LastPageText="&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;" Mode="NextPreviousFirstLast" 
                                NextPageText="&lt;i class=&quot;fa fa-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;fa fa-arrow-left&quot;&gt;&lt;/i&gt;" />--%>
    
     
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        
                                        </form>

                                 </div>
                                </div>
                                </ItemTemplate>
                                 </asp:FormView>
     <asp:ObjectDataSource ID="objStaff" runat="server" DataObjectTypeName="PASSIS.DAO.User"
        SelectMethod="RetrieveUser" TypeName="PASSIS.DAO.UsersDAL">
        <SelectParameters>
            <asp:QueryStringParameter DefaultValue="0" Name="id" QueryStringField="Id" Type="Int64" />
        </SelectParameters>
    </asp:ObjectDataSource>
                                </asp:Content>
