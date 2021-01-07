<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Site.master" CodeFile="SupAdminAddThemeTopic.aspx.cs" Inherits="SupAdminAddThemeTopic" %>

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
                                        <h4>CREATE SUBJECT'S THEME TOPIC</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblReport" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Curriculum:<span class="text-danger ml-2">*</span></label>
                                                    <asp:DropDownList ID="ddlCurriculum" DataTextField="CurriculumName" DataValueField="Id" class="custom-select form-control" runat="server" 
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlCurriculum_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-xl-6">
                                                   <asp:label runat="server"  ID="lblSchoolType" Visible="false" class="form-control-label">SchoolType:<span class="text-danger ml-2">*</span></asp:label>
                                             <asp:DropDownList runat="server" Visible="false" class="custom-select form-control" ID="ddlSchoolType" DataTextField="SchoolTypeName" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="ddlSchoolType_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                            
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                      <label class="form-control-label">Grade(Class):<span class="text-danger ml-2">*</span></label>
                                        <asp:DropDownList runat="server" ID="ddlClass" class="custom-select form-control" OnSelectedIndexChanged="ddlClass_SelectedIndexChanged" AutoPostBack="true" DataTextField="Name" DataValueField="Id"></asp:DropDownList> 
                                                     </div>
                                                    <div class="col-xl-6">
                                                    <label class="form-control-label">Subject:<span class="text-danger ml-2">*</span></label>
                                        <asp:DropDownList runat="server" ID="ddlSubject" AutoPostBack="true" class="custom-select form-control" DataTextField="Name" DataValueField="Id"></asp:DropDownList> 
                                                  </div>                                               
                                                    </div>
                                              <div class="form-group row mb-3">
                                                        <div class="col-xl-6 mb-3">
                                                 <label class="form-control-label">Theme Title:<span class="text-danger ml-2">*</span></label>
                                        <asp:DropDownList runat="server" ID="ddlThemeTitle" class="custom-select form-control" DataTextField="Title" DataValueField="Id" OnSelectedIndexChanged="ddlThemeTitle_SelectedIndexChanged"></asp:DropDownList>                                    

                                                  </div>
                                                   <div class="col-xl-6">
                                                      <label class="form-control-label">Theme Topic:<span class="text-danger ml-2">*</span></label>
                                        <asp:TextBox runat="server" ID="txaTopic" class="form-control" Columns="90" Rows="3" TextMode="MultiLine"></asp:TextBox> 
                                                     </div>
                                                 
                                               </div>

                                         
                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSave" class="btn btn-secondary"  OnClick="btnSave_Click" runat="server" Text="Add Topic"  />
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


