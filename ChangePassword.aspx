<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="ChangePassword.aspx.cs" Inherits="ChangePassword" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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
                                        <h4>CHANGE/RESET PASSWORD</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                          <asp:Label ID="lblResultMsg" runat="server" Text="" ></asp:Label>

                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Old Password:<span class="text-danger ml-2">*</span></label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ForeColor="Red" ControlToValidate="txtOldPassword" runat="server" ErrorMessage="This field is required"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtOldPassword" class="form-control" TextMode="Password" runat="server"></asp:TextBox>

                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">New Password:<span class="text-danger ml-2">*</span></label>
                               <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" runat="server" ForeColor="Red" ControlToValidate="txtNewPassword" ErrorMessage="This field is required"></asp:RequiredFieldValidator>
                            <asp:TextBox runat="server" class="form-control" placeholder="minimum of six characters" TextMode="Password" ID="txtNewPassword"> </asp:TextBox>

                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Confirm Password:<span class="text-danger ml-2">*</span></label>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Display="Dynamic" runat="server" ForeColor="Red" ControlToValidate="txtConNewPassword" ErrorMessage="This field is required"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" Display="Dynamic" ForeColor="Red" ControlToValidate="txtConNewPassword" ControlToCompare="txtNewPassword" ErrorMessage="Password doesn't match"></asp:CompareValidator>
                            <asp:TextBox runat="server" class="form-control" placeholder="minimum of six characters" TextMode="Password" ID="txtConNewPassword"> </asp:TextBox>
                                                       
                                                </div>
                                               
                                               <div class="col-xl-6">
                                                      <br />
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                   <asp:Button runat="server" class="btn btn-secondary" ID="btnSubmit" OnClick="btnSubmit_Click" Text="Change Password"/>                                                

                                                     </div>
                                                      </div>
                                               <%-- <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSaveAcademicSession" OnClick="btnSaveAcademicSession_OnClick" class="btn btn-secondary" runat="server" Text="Save Academic Session"  />
                                                </div>
                                            </div>--%>
                                                </form>    
                                
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>                        
                                </asp:Content>
