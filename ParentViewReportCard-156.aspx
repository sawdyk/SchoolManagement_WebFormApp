<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="ParentViewReportCard-156.aspx.cs" Inherits="ParentViewReportCard_156" %>

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
                                        <h4>DOWNLOAD REPORT CARD</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Select Ward:<span class="text-danger ml-2">*</span></label>
                                        <asp:DropDownList runat="server" ID="ddlWard" class="custom-select form-control" DataTextField="StudentFullName" DataValueField="Id" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlWard_SelectedIndexChanged"></asp:DropDownList>

                                                </div>
                                                <div class="col-xl-6">
                                                   <asp:label runat="server" class="form-control-label">Session:<span class="text-danger ml-2">*</span></asp:label>
                                       <asp:DropDownList runat="server" AutoPostBack="true" class="custom-select form-control" ID="ddlAcademicSession" OnSelectedIndexChanged="ddlAcademicSession_SelectedIndexChanged">
                                            <asp:ListItem Enabled="true" Selected="True" Text="--  Select All --" Value="0"></asp:ListItem>
                                        </asp:DropDownList>                                              
                                                </div>
                                            </div>
                                            
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                      <label class="form-control-label">Term:<span class="text-danger ml-2">*</span></label>
                                    <asp:DropDownList runat="server" ID="ddlAcademicTerm" class="custom-select form-control" OnSelectedIndexChanged="ddlAcademicTerm_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Enabled="true" Selected="True" Text="--  Select All --" Value="0"></asp:ListItem>
                                        </asp:DropDownList>                                                    

                                                </div>
                                                    <div class="col-xl-6">
                                                    <label class="form-control-label">Year:<span class="text-danger ml-2">*</span></label>
                                        <asp:DropDownList runat="server" AutoPostBack="true" class="custom-select form-control" ID="ddlYear" AppendDataBoundItems="true"
                                            DataTextField="Name" DataValueField="Id" OnSelectedIndexChanged="ddlYear_OnSelectedIndexChanged">
                                            <asp:ListItem Enabled="true" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>                                               

                                                    </div>                                               
                                                    </div>
                                              <div class="form-group row mb-3">
                                                        <div class="col-xl-6 mb-3">
                                                 <label class="form-control-label">Grade:<span class="text-danger ml-2">*</span></label>
                        <asp:DropDownList runat="server" ID="ddlGrade" class="custom-select form-control" OnSelectedIndexChanged="ddlGrade_SelectedIndexChanged" DataTextField="GradeName" DataValueField="Id"
                                            AutoPostBack="true" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                                  </div>
                                                     </div>

                                                <div class="widget-body">
                                                <div class="row">
                                           <%-- <div class="col-xl-6">
                                                    <asp:Button ID="btnPrintBehavior" class="btn btn-secondary"  OnClick="btnPrintBehavior_Click" runat="server" Text="Download Behavioral Report"  />
                                                </div>--%>
                                                    <div class="col-xl-6">
                            <asp:Button runat="server" ID="btnPrintAll" class="btn btn-secondary" Text=" Download Report Card " OnClick="btnPrintAll_OnClick" />
                                                </div>
                                            </div>
                                                </form>    
                                        <br /><br />
                               
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
                         
                                </asp:Content>



