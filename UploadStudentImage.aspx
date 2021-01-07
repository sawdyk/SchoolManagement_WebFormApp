<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="UploadStudentImage.aspx.cs" Inherits="UploadStudentImage" %>


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
                                        <h4>UPLOAD/UPDATE STUDENT PASSPORT</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Year:<span class="text-danger ml-2">*</span></label>
                                    <asp:DropDownList runat="server" class="custom-select form-control"  AutoPostBack="true" ID="ddlYear" 
                                        DataTextField="Name" DataValueField="Id" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                    </asp:DropDownList>                                              
                                                </div>
                                                <div class="col-xl-6">
                                     <label class="form-control-label">Grade:<span class="text-danger ml-2">*</span></label>
                                     <asp:DropDownList runat="server" class="custom-select form-control"  AutoPostBack="true" ID="ddlClass" OnSelectedIndexChanged="ddlClass_SelectedIndexChanged" DataTextField="GradeName" DataValueField="Id">
                                    </asp:DropDownList>                                                  

                                                </div>
                                            </div>
                                             <%-- <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                       <label class="form-control-label">Term:<span class="text-danger ml-2">*</span></label>
                                             <asp:DropDownList runat="server" ID="ddlTerm" class="custom-select form-control" DataTextField="AcademicTermName" DataValueField="Id"></asp:DropDownList>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Session:<span class="text-danger ml-2">*</span></label>
                                    <asp:DropDownList runat="server" DataTextField="SessionName" class="custom-select form-control" DataValueField="ID" ID="ddlSession"></asp:DropDownList>
                                                  </div>
                                               </div>--%>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Student:<span class="text-danger ml-2">*</span></label>
                             <asp:DropDownList ID="ddlStudent" DataTextField="Name" DataValueField="Id" class="custom-select form-control"  runat="server"></asp:DropDownList>                                                                  
                                                </div>
                                                 <div class="col-xl-6">
                                               <br />
                                                    <label class="form-control-label">Upload File:<span class="text-danger ml-2">*</span></label>
                                                    <asp:FileUpload ID="ImageFile" class="btn btn-secondary"  runat="server" />
                                                </div>
                                            </div>
                                                <div class="col-xl-6">
                                                    <asp:Button ID="btnUpload" class="btn btn-secondary" visible="true"  OnClick="btnImageUpload_Click" runat="server" Text="Upload Passport"  />
                                            </div>                           
                                               
                                            </div>
                                                
                                                </form>    
                                        <br />
                                      
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                                                 
                                </asp:Content>

