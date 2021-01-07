<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="SupAdminResourceUpload.aspx.cs" Inherits="SupAdminResourceUpload" %>


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
                                        <h4>UPLOAD RESOURCE</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblResponse" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Description:<span class="text-danger ml-2">*</span></label>
                            <asp:TextBox runat="server" class="form-control" ID="txtDesc"></asp:TextBox>
                                                </div>
                                                <div class="col-xl-6">
                                     <label class="form-control-label">Upload File:<span class="text-danger ml-2">*</span></label>
                            <asp:FileUpload ID="documentUpload" class="form-control" runat="server" />

                                                </div>
                                            </div>
                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSave" class="btn btn-secondary"  OnClick="btnSave_Click" runat="server" Text="Upload Resource"  />
                                                </div>
                                            </div>
                                                </form>    
                                        <br />
                               <asp:Label ID="lblList" Font-Size="Large" ForeColor="Black" runat="server" Visible="true" Text="LIST OF UPLOADED RESOURCES"></asp:Label>

                                        <asp:GridView ID="gdvList" GridLines="None" class="table table-striped" ForeColor="#333333" OnRowCommand="gdvList_RowCommand" EmptyDataText=" No Document Type has been created" runat="server" AutoGenerateColumns="False">
                                        <AlternatingRowStyle BackColor="White" />
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
                        <asp:TemplateField HeaderText=" Description ">
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("FileDescription") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="false">
                            <ItemTemplate>
                                    <asp:LinkButton ID="lnkDownload" runat="server" class="btn btn-success"  CommandArgument='<%# Eval("ID") %>' CommandName='cmd' SkinID="lnkGreen"> Download </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblExtension" runat="server" Text='<%#  Eval("DocumentType")  %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                                    </asp:GridView>

                                        
                                            
                                            </div>                                        

                                 </div>
                                </div>
                          
                                </asp:Content>



