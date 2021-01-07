<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="BroadSheet.aspx.cs" Inherits="BroadSheet" %>

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
                                        <h4>GENERATE AND DOWNLOAD ACADEMIC BROADSHEET</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Year:<span class="text-danger ml-2">*</span></label>
                                                        <asp:DropDownList runat="server" AutoPostBack="True" class="custom-select form-control" ID="ddlYear" AppendDataBoundItems="True"
                                                                    DataTextField="Name" DataValueField="Id" OnSelectedIndexChanged="ddlYear_OnSelectedIndexChanged">
                                                                    <asp:ListItem Enabled="true" Selected="True" Text="--  Select --" Value="0"></asp:ListItem>
                                                                </asp:DropDownList>                                               

                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Grade:<span class="text-danger ml-2">*</span></label>
                                                    <asp:DropDownList ID="ddlGrade" DataTextField="GradeName" DataValueField="Id" class="custom-select form-control" runat="server"></asp:DropDownList>

                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Session:<span class="text-danger ml-2">*</span></label>
                                                <asp:DropDownList runat="server" AutoPostBack="false" class="custom-select form-control" ID="ddlAcademicSession" AppendDataBoundItems="true"
                                                    DataTextField="SessionName" DataValueField="Id" >
                                                    <asp:ListItem Enabled="true" Selected="True" Text="--  Select --" Value="0"></asp:ListItem>
                                                </asp:DropDownList>                                               
                                                   </div>

                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Term:<span class="text-danger ml-2">*</span></label>
                                                <asp:DropDownList runat="server" ID="ddlAcademicTerm" class="custom-select form-control" DataTextField="AcademicTermName" DataValueField="Id"  AppendDataBoundItems = "true">
                                    <asp:ListItem Enabled="true" Selected="True" Text="--  Select --" Value="0"></asp:ListItem>
                                         </asp:DropDownList>                                                   

                                                  </div>
                                               </div>                                         
                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnPrintBroadsheet" class="btn btn-secondary"  OnClick="btnPrintBroadsheet_OnClick" runat="server" Text="Download Broadsheet"  />
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
                                   
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
                         
                                </asp:Content>
