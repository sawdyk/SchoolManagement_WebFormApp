<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="TestPerformance.aspx.cs" Inherits="TestPerformance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

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
                                        <h4>TEST PERFORMANCE</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Visible="false" Text="" class="text-danger ml-2"></asp:Label>
                                          <asp:Label ID="lblResultMsg" runat="server" Visible="false" Text="" class="text-success ml-2"></asp:Label>

                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Year:<span class="text-danger ml-2">*</span></label>
                            <asp:DropDownList runat="server" ID="ddlYear" DataTextField="Name" DataValueField="id" class="custom-select form-control" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" AutoPostBack="true"    >
                            </asp:DropDownList>
                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Grade:<span class="text-danger ml-2">*</span></label>
                            <asp:DropDownList ID="ddlClass" OnSelectedIndexChanged="ddlClass_SelectedIndexChanged" class="custom-select form-control" DataTextField="GradeName" DataValueField="Id" AutoPostBack="true"    runat="server"></asp:DropDownList>                       

                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Session:<span class="text-danger ml-2">*</span></label>
                                                <asp:DropDownList ID="ddlsession" class="custom-select form-control" runat="server">
                                                </asp:DropDownList>                                                        
                                                </div>
                                               <div class="col-xl-6">
                                                 <label class="form-control-label">Term:<span class="text-danger ml-2">*</span></label>
                            <asp:DropDownList ID="ddlterm" class="custom-select form-control" runat="server"></asp:DropDownList>                       

                                                     </div>
                                                      </div>
                                             <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Student:<span class="text-danger ml-2">*</span></label>
                            <asp:DropDownList ID="ddlWard" DataTextField="GradeName" class="custom-select form-control" DataValueField="Id" AutoPostBack="true"    runat="server"></asp:DropDownList>                       
                                                       
                                                </div>
                                                      </div>
                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                             <asp:Button ID="btnViewTopN"  class="btn btn-secondary" OnClick="ViewReports_Click" runat="server" Text="View Report" />
                                                </div>
                                            </div>
                                                </form>    
                                        <br /><br />
                                                        <asp:Label ID="Label1" runat="server" ForeColor="Black" Text="TOP TEST PERFORMANCE"></asp:Label>

                              <%--<asp:Panel ID="Panel2" Width="1300px"  class="table table-striped" ScrollBars="Horizontal" runat="server">
                              <asp:GridView ID="GVReport" runat="server"></asp:GridView>--%>

                    <rsweb:ReportViewer ID="RVReport" runat="server" Width="920px" Height="700px"
                        ShowBackButton="True" ShowFindControls="false" ShowPrintButton="True"
                        ShowRefreshButton="True" ShowExportControls="True">
                    </rsweb:ReportViewer>
                                     <%--</asp:Panel>--%>




                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
                        
                                </asp:Content>



