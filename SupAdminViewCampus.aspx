<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="SupAdminViewCampus.aspx.cs" Inherits="SupAdminViewCampus" %>


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
                                        <h4>SCHOOL CAMPUS</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblResponse" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Select School:<span class="text-danger ml-2">*</span></label>
                                    <asp:DropDownList ID="ddlSchoolList" class="custom-select form-control" DataTextField="Name" DataValueField="Id" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSchoolList_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                         
                                                </form>    
                                        <br />
                                <asp:Label ID="lblSchool" ForeColor="Black" runat="server" Visible="false" Text="SCHOOL CAMPUS LIST"></asp:Label>

                            <asp:GridView ID="gvCampus" EmptyDataText="No campus" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" class="table table-striped"
                                                 CellPadding="4" ForeColor="#333333" GridLines="None" OnRowCancelingEdit="gvCampus_RowCancelingEdit" OnRowUpdating="gvCampus_RowUpdating" OnRowEditing="gvCampus_RowEditing">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Campus Name">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtCampusName" class="form-control" Text='<%# Eval("Name")  %>' runat="server"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label4"  Text='<%# Eval("Name")  %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Campus Code">
        <%--                                                <EditItemTemplate>
                                                            <asp:TextBox ID="txtCampusCode" Text='<%# Eval("Code")  %>' runat="server"></asp:TextBox>
                                                        </EditItemTemplate>--%>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label3" Text='<%# Eval("Code")  %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Campus Address">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtCampusAddress" class="form-control" Text='<%# Eval("CampusAddress")  %>' runat="server"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label2" Text='<%# Eval("CampusAddress")  %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ID" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblId" Text='<%# Eval("Id")  %>' runat="server"></asp:Label>
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
                                                </Columns>
        <%--                                        <EditRowStyle BackColor="#2461BF" />--%>
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
                                            </asp:GridView>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
                         
                                </asp:Content>

