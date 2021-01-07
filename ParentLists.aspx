<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="ParentLists.aspx.cs" Inherits="ParentLists" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="PASSIS.LIB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .style1 {
            width: 255px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server"> 
        <asp:Panel runat = "server" Visible = "true" ID = "pnlCreateUser" >
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
                                    <div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>VIEW PARENT'S INFORMATION</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Username:<span class="text-danger ml-2">*</span></label>
                         <asp:TextBox runat="server" placeholder="Enter Username or Firstname to search for a specific parent's details"  ID="txtUsername" class="form-control"> </asp:TextBox>
                                                </div>
                                              </div>
                                            

                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSearchStaff" class="btn btn-secondary"  OnClick="btnSearchStaff_OnClick" runat="server" Text="Search"  />
                                                </div>
                                            </div>
                                                </form>    
                                        <br /><br />
                                 <%--<div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>CLASS ARMS</h4>
                                 </div>--%>
                            <%--<asp:GridView ID="gvdAcademicSession" runat="server" AutoGenerateColumns="False" EmptyDataText=" No Calendar year currently exist" AllowPaging="true" Width="100%" ShowFooter="True" PageSize="20" GridLines="Both" OnRowCommand="gvdAcademicSession_RowCommand">       
                            <PagerSettings FirstPageText="&lt;i class=&quot;fa  fa-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-left&quot;&gt;&lt;/i&gt;"
                                LastPageText="&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;" Mode="NextPreviousFirstLast" 
                                NextPageText="&lt;i class=&quot;fa fa-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;fa fa-arrow-left&quot;&gt;&lt;/i&gt;" />--%>
   
                                                    <asp:Label ID="lblParents" Font-Size="Large" ForeColor="Black" runat="server" Visible="true" Text="LIST OF PARENTS"></asp:Label>
                    <asp:GridView ID="gdvList" runat="server" GridLines="None" AutoGenerateColumns="false" PageSize="30" EmptyDataText="Username Doesn't Exist"
                    AllowPaging="true" Width="100%"  ForeColor="#333333" class="table table-striped" OnPageIndexChanging="gdvList_PageIndexChanging">
                    <EditRowStyle BackColor="#2461BF" />
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
                                <asp:HyperLink ID="lblName" runat="server" ForeColor="#0066ff" Text='<%# Eval("Username")  %>' NavigateUrl='<%# Eval("Id", "ParentDetail.aspx?mode=view&id={0}") %>'></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" Name ">
                            <ItemTemplate>
                                <asp:Label ID="lblCode" runat="server" Text='<%#  Eval("Firstname") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" PhoneNumber">
                            <ItemTemplate>
                                <asp:Label ID="lblCode" runat="server" Text='<%# Eval("PhoneNumber") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" Email Address">
                            <ItemTemplate>
                                <asp:Label ID="lblCode" runat="server" Text='<%# Eval("EmailAddress") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText=" Role ">
                            <ItemTemplate>
                                <asp:Label ID="lblCode" runat="server" Text='<%# getRoleFromUserRole(Eval("Id")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" Campus ">
                            <ItemTemplate>
                                <asp:Label ID="lblCode" runat="server" Text='<%# getCampusName(Eval("SchoolCampusId")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>
                                <PagerSettings FirstPageText="&lt;i class=&quot;la  la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                                    LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                                    Mode="NextPreviousFirstLast" NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" 
                                    PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />                    
                            </asp:GridView>
                                          
                                            </div>
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        
       <asp:ObjectDataSource ID="objSchool" runat="server" SelectMethod="getAllSchools"
        TypeName="PASSIS.DAO.SchoolConfigDAL"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="objRole" runat="server" SelectMethod="getAllRolesSpecial" TypeName="PASSIS.DAO.RoleDAL"
        OnSelecting="objRole_Selecting">
        <SelectParameters>
            <asp:Parameter Name="parentId" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
            </asp:Panel>
                                </asp:Content>

