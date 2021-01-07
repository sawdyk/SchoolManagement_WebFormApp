<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="SupAdminAddRoles.aspx.cs" Inherits="SupAdminAddRoles" %>


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
                                        <h4>CREATE NEW ROLES</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" class="text-danger ml-2"></asp:Label>
                                          <asp:Label ID="lblResultMsg" runat="server" Text="" class="text-success ml-2"></asp:Label>

                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">First Name:<span class="text-danger ml-2">*</span></label>
                                <asp:TextBox runat="server" class="form-control" ID="txtName"></asp:TextBox>

                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Description:<span class="text-danger ml-2">*</span></label>
                                <asp:TextBox runat="server" class="form-control" ID="txtCode"></asp:TextBox>

                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Select Parent Role:<span class="text-danger ml-2">*</span></label>
                                    <asp:DropDownList runat="server" ID="ddlRole" class="custom-select form-control" AppendDataBoundItems="true" DataTextField="RoleName"
                                        DataValueField="Id" DataSourceID="objRole">
                                        <asp:ListItem Enabled="true" Selected="True" Value="0"> --Self--</asp:ListItem>
                                    </asp:DropDownList>                                                       
                                                </div>
                                             
                                                      </div>
                                            
                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSave" OnClick="btnSave_OnClick" class="btn btn-secondary" runat="server" Text="Add New Role"  />
                                                </div>
                                            </div>
                                                </form>    
                                        <br /><br />
                           <asp:Label ID="lblUsers" Font-Size="Large" ForeColor="Black" runat="server" Visible="true" Text="ALL ROLES"></asp:Label>
                                        <br /><br />
                                 <asp:GridView ID="gdvList" EmptyDataText="No role has been created" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" ShowFooter="True" class="table table-striped"
                                                 CellPadding="4" ForeColor="#333333" GridLines="None" OnPageIndexChanging="gdvList_PageIndexChanging" OnSelectedIndexChanged="gdvList_SelectedIndexChanged" OnRowCommand="gdvList_RowCommand" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" 
                                     OnRowDataBound="GridView1_RowDataBound" OnRowDeleting = "gdvList_RowDeleting" >
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
                        <asp:TemplateField HeaderText=" Name ">
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("RoleName")  %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" Description ">
                            <ItemTemplate>
                                <asp:Label ID="lblCode" runat="server" Text='<%#  Eval("RoleCode") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" Parent Role ">
                            <ItemTemplate>
                                <asp:Label ID="lblParentCode" runat="server" Text='<%# getRoleName(Eval("ParentRoleId")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                                <asp:TemplateField HeaderText="Edit" ShowHeader="false">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnedit" runat="server" CommandName="Edit" Text="Edit" CommandArgument='<%# Eval("Id") %>'
                                            SkinID="lnkGreen" ToolTip="Edit"><i class="la la-edit la-2x"></i></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="btnupdate" runat="server" CommandName="Update" Text="Update"
                                            SkinID="lnkGreen" CommandArgument='<%# Eval("Id") %>' ToolTip="Update"><i class="la la-floppy-o la-2x"></i></asp:LinkButton>  
                                        <asp:LinkButton ID="btncancel" runat="server" CommandName="Cancel" Text="Cancel"
                                            SkinID="lnkGreen" CommandArgument='<%# Eval("Id") %>' ToolTip="Cancel"><i class="la la-remove la-2x"></i></asp:LinkButton>
                                                  <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" Text="Delete"
                                            SkinID="lnkGreen" CommandArgument='<%# Eval("Id") %>' ToolTip="Delete"><i class="la la-trash la-2x"></i> </asp:LinkButton>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
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
<asp:ObjectDataSource ID="objRole" runat="server" SelectMethod="getAllRoles" TypeName="PASSIS.DAO.RoleDAL">
    </asp:ObjectDataSource>
                                </asp:Content>
