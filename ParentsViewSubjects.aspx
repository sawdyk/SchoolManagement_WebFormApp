<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Site.master" CodeFile="ParentsViewSubjects.aspx.cs" Inherits="ParentsViewSubjects" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

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
                                        <h4>VIEW SUBJECTS</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Visible="false" Text="" class="text-danger ml-2"></asp:Label>
                                          <asp:Label ID="lblResultMsg" runat="server" Visible="false" Text="" class="text-success ml-2"></asp:Label>

                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Select Child:<span class="text-danger ml-2">*</span></label>
                                        <asp:DropDownList ID="ddlWard" AutoPostBack="true" runat="server" class="custom-select form-control" DataTextField="StudentFullName" DataValueField="Id"
                                AppendDataBoundItems="true" OnSelectedIndexChanged="ddlWard_SelectedIndexChanged">
                                <asp:ListItem Enabled="true" Text="--Select Child--" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Class:<span class="text-danger ml-2">*</span></label>
                                        <asp:DropDownList ID="ddlClass"  AutoPostBack="true" OnSelectedIndexChanged="ddlClass_OnSelectedIndexChanged" class="custom-select form-control" runat="server" DataTextField="Name" DataValueField="Id" >
                                            <asp:ListItem Enabled="true" Text="--Select Class--" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                                </div>
                                            </div>
                                                                                              
                                                </form>    
                                        <br /><br />
                                                        <asp:Label ID="Label1" runat="server" Visible="false" ForeColor="Black" Text="LIST OF SUBJECTS"></asp:Label>
                         <asp:Panel ID="Panel2" Width="100%" class="table table-striped" ScrollBars="Vertical" runat="server">
                                            <asp:GridView ID="gdvListSubject" GridLines="None" runat="server" AutoGenerateColumns="false" EmptyDataText=" No result."
                                AllowPaging="true" Width="100%" PageSize ="20" ForeColor="Black" class="table table-striped" OnPageIndexChanging="gdvListSubject_PageIndexChanging">
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
                                    <asp:TemplateField HeaderText=" Subject Name  ">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubject" runat="server" Text='<%# Bind("Subject.Name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                      
                                    <%--<asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:Button ID="btnRemoveRole" runat="server" CausesValidation="False" Text="Remove"
                                            CommandName="Remove" CommandArgument='<%# Eval("Id") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField> --%>                      
                                </Columns>
                                <PagerSettings FirstPageText="&lt;i class=&quot;la  la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                                    LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                                    Mode="NextPreviousFirstLast" NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" 
                                    PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />
                            </asp:GridView>

                                            
                                 </asp:Panel>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
                        
                                </asp:Content>





