<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Site.master" CodeFile="SupAdminCreateCampus.aspx.cs" Inherits="SupAdminCreateCampus" %>


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
                                        <h4>CREATE NEW SCHOOL CAMPUS</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblResponse" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">School:<span class="text-danger ml-2">*</span></label>
                                    <asp:DropDownList ID="ddlSchool" DataTextField="Name" class="custom-select form-control" DataValueField="Id" runat="server"></asp:DropDownList>
                                                </div>
                                                <div class="col-xl-6">
                                     <label class="form-control-label">Name:<span class="text-danger ml-2">*</span></label>
                                         <asp:TextBox runat="server" class="form-control" ID="txtCampusName"></asp:TextBox>                                          

                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                       <label class="form-control-label">Campus Code:<span class="text-danger ml-2">*</span></label>
                                                <asp:TextBox runat="server" class="form-control" ID="txtCampusCode"></asp:TextBox> 
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Address:<span class="text-danger ml-2">*</span></label>
                                            <asp:TextBox runat="server"  class="form-control" ID="txtCampusAddress"></asp:TextBox>
                                                  </div>
                                               </div>
                                           <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSave" class="btn btn-secondary" runat="server" Text="Save"  OnClick="btnSave_Click" />
                                                </div>
                                            </div>
                                                </form>    
                                        <br />
                               
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
                         
                                </asp:Content>


