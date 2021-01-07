<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="AdmissionConfig.aspx.cs" Inherits="AdmissionConfig" %>


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
                                        <h4>ADMISSION APPLICATION CONFIGURATION</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblResponse" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblResponseMessage" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Do you want to process your admission on Passis?:<span class="text-danger ml-2">*</span></label>
                                    <asp:DropDownList runat="server" ID="ddlProcessAdmission" class="custom-select form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlProcessAdmission_SelectedIndexChanged"></asp:DropDownList>                                           
                                                </div>
                                                
                                                <div class="col-xl-6">
                                     <asp:label runat="server" ID="lblSelecteFormFee" Visible="false" class="form-control-label">Form Fee?:<span class="text-danger ml-2">*</span></asp:label>
                    <asp:DropDownList runat="server" ID="ddlFormFee" AutoPostBack="true" Visible="false" class="custom-select form-control" OnSelectedIndexChanged="ddlFormFee_SelectedIndexChanged"></asp:DropDownList>
                                                
                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                       <asp:label runat="server" Visible="false" ID="lblRedirectToSite" class="form-control-label">Do you want to redirect to your site?:<span class="text-danger ml-2">*</span></asp:label>
                    <asp:DropDownList runat="server" ID="ddlSiteRedirect" Visible="false" class="custom-select form-control" OnSelectedIndexChanged="ddlSiteRedirect_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <asp:label runat="server" Visible="false" ID="lblWebsiteUrl" class="form-control-label">Website Url (e.g http://www.google.com):<span class="text-danger ml-2">*</span></asp:label>
                    <asp:TextBox ID="txtWebsiteUrl" class="form-control"  Visible="false" placeholder="e.g http://www.google.com" runat="server"></asp:TextBox>
                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <asp:label runat="server" Visible="false" ID="lblFormFee" class="form-control-label">Form Fee:<span class="text-danger ml-2">*</span></asp:label>
                      <asp:TextBox ID="txtFormFee" class="form-control" Visible="false" runat="server"></asp:TextBox>
                                           
                                                </div>
                                                <div class="col-xl-6">
                                                    <asp:label runat="server" Visible="false" ID="lblAccountDetails" class="form-control-label">Account Details:<span class="text-danger ml-2">*</span></asp:label>
                    <asp:TextBox ID="txtAccount" runat="server" Visible="false" class="form-control" placeholder="BankName-AccountName-AccountNumber"></asp:TextBox>

                                                </div>
                                            </div>
                                           <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <asp:label runat="server" Visible="false" ID="lblAdmissionMode" class="form-control-label">Admission Mode:<span class="text-danger ml-2">*</span></asp:label>
                    <asp:DropDownList runat="server" ID="ddlAdmissionMode" Visible="false" class="custom-select form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlAdmissionMode_SelectedIndexChanged"></asp:DropDownList>
                                              
                                                </div>
                                                <div class="col-xl-6">
                                                    <asp:label runat="server" ID="lblMinimiumTestScore" Visible="false" class="form-control-label">Minimium Test Score:<span class="text-danger ml-2">*</span></asp:label>                                                  
                    <asp:TextBox ID="txtMinimiumTestScore" class="form-control" Visible="false" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                         <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <asp:label runat="server" ID="lblMinimiumInterviewScore" Visible="false" class="form-control-label">Minimium Interview Score:<span class="text-danger ml-2">*</span></asp:label>
                    <asp:TextBox ID="txtMinimiumInterviewScore" Visible="false" class="form-control" runat="server"></asp:TextBox>
                                                
                                                </div>
                                                <div class="col-xl-6">
                                                    <br />
                                         <asp:Button ID="btnSaveConfig" Visible="false" class="btn btn-secondary" OnClick="btnSaveConfig_Click" Text="Save Configuration" runat="server" />

                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <asp:label runat="server" ID="lblTextRequirement" Visible="false" class="form-control-label">Test Requirements:<span class="text-danger ml-2">*</span></asp:label>
                    <asp:TextBox ID="txtTestRequirement" runat="server" Visible="false" class="form-control" placeholder="Biro,Pencil,Passport"></asp:TextBox>
                                              
                                                </div>
                                                <div class="col-xl-6">
                                                    <br />
                     <asp:Button ID="btnSaveTestRequirement" class="btn btn-secondary" Visible="false"  OnClick ="btnSaveTextRequirement_Click" Text="Save Test Requirements" runat="server" />

                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <asp:label runat="server" ID="lblInterviewRequirement" Visible="false" class="form-control-label">Interview Requirements:<span class="text-danger ml-2">*</span></asp:label>
                    <asp:TextBox ID="txtInterviewRequirement" runat="server" Visible="false" class="form-control" placeholder="Biro,Pencil,Passport"></asp:TextBox>
                                              
                                                </div>
                                                <div class="col-xl-6">
                                              <br />
                    <asp:Button ID="btnSaveInterviewRequirement" class="btn btn-secondary" Visible="false" OnClick="btnSaveInterviewRequirement_Click" Text="Save Interview Requirements" runat="server" />
                                                </div>
                                            </div>

                                            <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">

                                                </div>
                                            </div>
                                                
                                                </form>    
                                        <br />
                               
            <asp:label runat="server" Text="Configuration List" ForeColor="Black" Font-Size="Large" Visible="true"></asp:label>
               <asp:GridView ID="gdvConfig" runat="server" ForeColor="Black" class="table table-striped" Width="100%" GridLines="None" OnRowCancelingEdit="gdvConfig_RowCancelingEdit" OnRowEditing="gdvConfig_RowEditing" OnRowUpdating="gdvConfig_RowUpdating" AllowPaging="false" EmptyDataText="No configuration found" AllowSorting="True" AutoGenerateColumns="False">
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
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Process Admission On Passis">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlGdvProcessAdmission" runat="server">
                                        <asp:ListItem Text="YES" Value="YES"></asp:ListItem>
                                        <asp:ListItem Text="NO" Value="NO"></asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblIsProcessAdmissionOnPassis" runat="server" Text='<%# Eval("IsProcessAdmissionOnPassis") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Redirect">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlGdvRedirect" DataTextField='<%# Eval("IsRedirect") %>' runat="server">
                                        <asp:ListItem Text="NO" Value="NO"></asp:ListItem>
                                        <asp:ListItem Text="YES" Value="YES"></asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblIsRedirect" runat="server" Text='<%# Eval("IsRedirect") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mode">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlGdvAdmissionMode" DataTextField='<%# Eval("AdmissionMode") %>' runat="server">
                                        <asp:ListItem Text="Form Only" Value="Form Only"></asp:ListItem>
                                        <asp:ListItem Text="Form/Test" Value="Form/Test"></asp:ListItem>
                                        <asp:ListItem Text="Form/Test/Interview" Value="Form/Test/Interview"></asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAdmissionMode" runat="server" Text='<%# Eval("AdmissionMode") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Website Url">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtGdvWebsiteUrl" runat="server" Text='<%# Eval("WebsiteUrl") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblWebsiteUrl" runat="server" Text='<%# Eval("WebsiteUrl") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Select Form Fee">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlGdvSelectFormFee" DataTextField='<%# Eval("SelectFormFee") %>' runat="server">
                                        <asp:ListItem Text="NO" Value="NO"></asp:ListItem>
                                        <asp:ListItem Text="YES" Value="YES"></asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSelectFormFee" runat="server" Text='<%# Eval("SelectFormFee") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Form Fee">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtGdvFormFee" runat="server" Text='<%# Eval("FormFee") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFormFee" runat="server" Text='<%# Eval("FormFee") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Account Details">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtGdvAccount" runat="server" Text='<%# Eval("AccountDetail") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAccount" runat="server" Text='<%# Eval("AccountDetail") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Minimium Test Score">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtGdvMinimiumTestScore" runat="server" Text='<%# Eval("MinimiumTestScore") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblMinimiumTestScore" runat="server" Text='<%# Eval("MinimiumTestScore") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Minimium Interview Score">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtGdvMinimiumInterviewScore" runat="server" Text='<%# Eval("MinimiumInterviewScore") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblMinimiumInterviewScore" runat="server" Text='<%# Eval("MinimiumInterviewScore") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Update" ToolTip="Update"><i class="fa fa-floppy-o fa-2x"></i></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" ToolTip="Cancel"><i class="fa fa-remove fa-2x"></i></asp:LinkButton>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" ToolTip="Edit"><i class="fa fa-edit fa-2x"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>

            <tr>
                <td colspan="2">
                    <asp:GridView ID="gdvTestRequirements" runat="server" ForeColor="Black" class="table table-striped" Width="100%"  GridLines="None" OnRowCommand="gdvTestRequirements_RowCommand" PageSize ="10" AllowPaging="false" EmptyDataText="No configuration found" AllowSorting="True" AutoGenerateColumns="False">
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
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Test Requirements">
                                <ItemTemplate>
                                    <asp:Label ID="lblFormFee" runat="server" Text='<%# Eval("Item") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:Button ID="btnRemoveRole" class="btn btn-danger" runat="server" CausesValidation="False" Text="Remove"
                                            CommandName="Remove" CommandArgument='<%# Eval("Id") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                        </Columns>
  <PagerSettings FirstPageText="&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                        LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                        Mode="NextPreviousFirstLast" 
                        NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" 
                        PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />                     </asp:GridView>
                </td>
            </tr>

            <tr>
                <td colspan="2">
                    <asp:GridView ID="gdvInterviewRequirements" runat="server" ForeColor="Black" class="table table-striped" Width="100%" GridLines="None" OnRowCommand="gdvInterviewRequirements_RowCommand" PageSize="10" AllowPaging="false" EmptyDataText="No configuration found" AllowSorting="True" AutoGenerateColumns="False">
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
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Interview Requirements">
                                <ItemTemplate>
                                    <asp:Label ID="lblFormFee" runat="server" Text='<%# Eval("Item") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:Button ID="btnRemoveRole" class="btn btn-danger" runat="server" CausesValidation="False" Text="Remove"
                                            CommandName="Remove" CommandArgument='<%# Eval("Id") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                        </Columns>
                    <PagerSettings FirstPageText="&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                        LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                        Mode="NextPreviousFirstLast" 
                        NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" 
                        PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />                    

                    </asp:GridView>

                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
                         
                                </asp:Content>
