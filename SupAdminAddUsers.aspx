<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="SupAdminAddUsers.aspx.cs" Inherits="SupAdminAddUsers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .style1 {
            width: 255px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">   
    
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
                        <div class="row flex-row">
                            <div class="col-12">
                                <!-- Form -->
                                <div class="widget has-shadow">
                                    <div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>CREATE NEW USERS/STAFFS</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" class="text-danger ml-2"></asp:Label>
                                          <asp:Label ID="lblResultMsg" runat="server" Text="" class="text-success ml-2"></asp:Label>

                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">First Name:<span class="text-danger ml-2">*</span></label>
                                <asp:TextBox runat="server" class="form-control" ID="txtFirstname"></asp:TextBox>

                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Last Name:<span class="text-danger ml-2">*</span></label>
                                <asp:TextBox runat="server" class="form-control" ID="txtLastname"></asp:TextBox>

                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Email Address:<span class="text-danger ml-2">*</span></label>
                                <asp:TextBox runat="server" class="form-control" ID="txtEmailAddress"></asp:TextBox>
                                                       
                                                </div>
                                               <div class="col-xl-6">
                                                 <label class="form-control-label">Phone Number:<span class="text-danger ml-2">*</span></label>
                                <asp:TextBox runat="server" class="form-control"  ID="txtPhoneNumber"></asp:TextBox>

                                                     </div>
                                                      </div>
                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">UserName:<span class="text-danger ml-2">*</span></label>
                                <asp:TextBox runat="server" class="form-control" ID="txtUsername"></asp:TextBox>
                              <asp:Label ID="lblUsernameError" runat="server" SkinID="LabelError" Visible="false"> </asp:Label>
                                                </div>
                                               <div class="col-xl-6">
                                                 <label class="form-control-label">Role:<span class="text-danger ml-2">*</span></label>
                                <asp:DropDownList runat = "server" ID = "ddlRole" class="custom-select form-control" AppendDataBoundItems = "true" 
                                  DataTextField = "RoleName" DataValueField = "Id" DataSourceID = "objRole"                                                                                                                                                                                                                                                                                          >
                                  <asp:ListItem Enabled = "true" Selected = "True" Value = "0"> --Select Role--</asp:ListItem>
                                  </asp:DropDownList>
                                                     </div>
                                                      </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                  <label class="form-control-label">School:<span class="text-danger ml-2">*</span></label>
                             <asp:DropDownList runat = "server" ID = "ddlSchool" AppendDataBoundItems = "true"  class="custom-select form-control" OnSelectedIndexChanged="ddlSchool_SelectedIndexChanged" AutoPostBack="true" 
                                  DataTextField = "Name" DataValueField = "Id" DataSourceID = "objSchool"                                                                                                                                                                                                                                                                                          >
                                  <asp:ListItem Enabled = "true" Selected = "True" Value = "0"> --Select School--</asp:ListItem>
                                  </asp:DropDownList>
                                                </div>
                                                   <div class="col-xl-6">
                                                  <label class="form-control-label">Campus:<span class="text-danger ml-2">*</span></label>
                               <asp:DropDownList runat = "server" class="custom-select form-control" ID = "ddlCampus" AppendDataBoundItems ="true" 
                                  DataTextField = "Name" DataValueField = "Id"></asp:DropDownList>
                                                     </div>
                                                  </div>



                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSave" OnClick="btnSave_OnClick" class="btn btn-secondary" runat="server" Text="Add New User"  />
                                                </div>
                                            </div>
                                                </form>    
                                        <br /><br />
                           <asp:Label ID="lblUsers" Font-Size="Large" ForeColor="Black" runat="server" Visible="true" Text="ALL USERS"></asp:Label>
                                        <br /><br />
                                 <asp:GridView ID="gdvList" EmptyDataText="No user(s) has been created" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" ShowFooter="True" class="table table-striped"
                                                 CellPadding="4" ForeColor="#333333" GridLines="None" OnPageIndexChanging="gdvList_PageIndexChanging"  >
                                                 <AlternatingRowStyle BackColor="White" />
                               <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#3366ff" Font-Bold="True" ForeColor="White" />
                                   <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#EFF3FB" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                     <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                       <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                      <SortedDescendingHeaderStyle BackColor="#4870BE" />      
                                                                <Columns>
                            <asp:TemplateField HeaderText=" S/N">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" Username ">
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Username")  %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" Firstname ">
                                <ItemTemplate>
                                    <asp:Label ID="lblCode" runat="server" Text='<%#  Eval("Firstname") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText=" Lastname">
                                <ItemTemplate>
                                    <asp:Label ID="lblCode" runat="server" Text='<%# Eval("Lastname") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText=" Email Address">
                                <ItemTemplate>
                                    <asp:Label ID="lblCode" runat="server" Text='<%# Eval("EmailAddress") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText=" Role ">
                                <ItemTemplate>
                                    <asp:Label ID="lblCode" runat="server" Text='<%# getRoleFromUserRole(Eval("Id")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText=" School ">
                                <ItemTemplate>
                                    <asp:Label ID="lblCode" runat="server" Text='<%# getSchoolName(Eval("SchoolId")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                                     <PagerSettings FirstPageText="&lt;i class=&quot;la  la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;"
                                LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" Mode="NextPreviousFirstLast" 
                                NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />
                                                            </asp:GridView>

                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
                        <asp:ObjectDataSource ID="objSchool" runat="server" SelectMethod="getAllSchools"
    TypeName="PASSIS.DAO.SchoolConfigDAL"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="objRole" runat="server" SelectMethod="getAllRoles"
    TypeName="PASSIS.DAO.RoleDAL" OnSelecting="objRole_Selecting"
>
    
       <SelectParameters>
        <asp:Parameter Name="parentId" Type="Int32"  />
    </SelectParameters>
    </asp:ObjectDataSource>  
                                </asp:Content>
