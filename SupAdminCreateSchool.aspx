<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="SupAdminCreateSchool.aspx.cs" Inherits="SupAdminCreateSchool" %>


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
                                        <h4>CREATE NEW SCHOOL</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblResponse" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">School Name:<span class="text-danger ml-2">*</span></label>
                                                 <asp:TextBox runat="server" class="form-control"  ID="txtName"></asp:TextBox>                                          
                                                </div>
                                                <div class="col-xl-6">
                                     <label class="form-control-label">School Code: (Should be a three-letter word.):<span class="text-danger ml-2">*</span></label>
                                         <asp:TextBox runat="server" class="form-control" MaxLength="3" ID="txtCode"></asp:TextBox>                                          

                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                       <label class="form-control-label">School Type:<span class="text-danger ml-2">*</span></label>
                                            <asp:DropDownList ID="ddlSchoolType" DataTextField="SchoolTypeName" class="custom-select form-control" DataValueField="Id" runat="server"></asp:DropDownList>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">School Address:<span class="text-danger ml-2">*</span></label>
                                            <asp:TextBox runat="server"  class="form-control" ID="txtAddress"></asp:TextBox>
                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">School Logo (jpg, png):<span class="text-danger ml-2">*</span></label>
                                            <input type="file" id="slogo" class="form-control" runat="server" accept="image/png" title="Upload School Logo (jpg, png)"/> 
                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">School Website(URL):<span class="text-danger ml-2">*</span></label>
                                            <asp:TextBox runat="server" class="form-control" ID="txtUrl"></asp:TextBox>

                                                </div>
                                            </div>
                                           <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="Button2" class="btn btn-secondary" runat="server" Text="Save"  OnClick="btnSave_OnClick" />
                                                </div>
                                            </div>
                                                </form>    
                                        <br />
                                <asp:Label ID="lblSchool" ForeColor="Black" runat="server" Visible="true" Text="SCHOOL LIST"></asp:Label>

                            <asp:GridView ID="gdvList" runat="server" class="table table-striped" AutoGenerateColumns="false" GridLines="None" EmptyDataText=" No School has been created"
                                AllowPaging="true" Width = "100%" ForeColor="Black" AutoGenerateDeleteButton="false" AutoGenerateEditButton="false" OnPageIndexChanging="gdvList_PageIndexChanging" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowEditing="GridView1_RowEditing" OnRowUpdating="gdvList_RowUpdating">
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
                                    <asp:TemplateField HeaderText="School ID ">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSId" runat="server" Text='<%# Eval("Id")  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=" Name ">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtGetSchoolName" class="form-control" Text='<%# Eval("Name")  %>' runat="server"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name")  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=" Code ">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCode" runat="server" Text='<%#  Eval("Code") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText=" Address ">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtGetSchoolAddress" class="form-control" Text='<%# Eval("Address")  %>' runat="server"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAddress" runat="server" Text='<%#  Eval("Address") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                                    <asp:TemplateField ShowHeader="False">
                                                        <EditItemTemplate>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Update" ToolTip="Update"><i class="la la-floppy-o la-2x"></i></asp:LinkButton>
                                                            &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" ToolTip="Cancel"><i class="la la-remove la-2x"></i></asp:LinkButton>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" ToolTip="Edit"><i class="la la-edit la-2x"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
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
                                </div>
                         
                                </asp:Content>

