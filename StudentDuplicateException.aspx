<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="StudentDuplicateException.aspx.cs" Inherits="StudentDuplicateException" %>


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
                                        <h4>DUPLICATE EXCEPTION</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblReport" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                           <%-- <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">School:<span class="text-danger ml-2">*</span></label>
                                              <asp:DropDownList ID="ddlSchool" class="custom-select form-control" DataTextField="Name" AutoPostBack="true" OnSelectedIndexChanged="ddlSchool_SelectedIndexChanged" DataValueField="Id" runat="server"></asp:DropDownList>

                                                </div>
                                                <div class="col-xl-6">
                                                   <asp:label runat="server" class="form-control-label">Campus:<span class="text-danger ml-2">*</span></asp:label>
                                    <asp:DropDownList ID="ddlCampus" class="custom-select form-control" DataTextField="Name" DataValueField="Id" runat="server"></asp:DropDownList>
                                                </div>
                                            </div>
                                            
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                      <label class="form-control-label">Session:<span class="text-danger ml-2">*</span></label>
                                    <asp:DropDownList runat = "server" class="custom-select form-control" DataTextField="SessionName" DataValueField="ID" ID ="ddlSession" ></asp:DropDownList>
                                                     </div>
                                                    <div class="col-xl-6">
                                                    <label class="form-control-label">Term:<span class="text-danger ml-2">*</span></label>
                                    <asp:DropDownList ID="ddlTerm" DataTextField="AcademicTermName" class="custom-select form-control" DataValueField="Id" runat="server"></asp:DropDownList>
                                                  </div>                                               
                                                    </div>
                                              <div class="form-group row mb-3">
                                                        <div class="col-xl-6 mb-3">
                                                 <label class="form-control-label">Amount:<span class="text-danger ml-2">*</span></label>
                                    <asp:TextBox ID="txtAmount" class="form-control" runat="server"></asp:TextBox>    

                                                  </div>
                                                   <div class="col-xl-6">
                                                      <label class="form-control-label">Payment Date:<span class="text-danger ml-2">*</span></label>
                                    <asp:TextBox runat="server" ID="txtPaymentDate" class="form-control" ReadOnly="true"></asp:TextBox>
                                   <ajaxToolKit:CalendarExtender ID="CalendarExtender1" Format="MM-dd-yyyy"  runat="server"  TargetControlID="txtPaymentDate" PopupButtonID="ImageButton1" PopupPosition="BottomLeft">
                                                               </ajaxToolKit:CalendarExtender>   
                                                     </div>
                                                     </div>--%>

                                               <%-- <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSaveSubscriptionFees" class="btn btn-secondary"  OnClick="btnSaveSubscriptionFees_Click" runat="server" Text="Save"  />
                                                </div>
                                            </div>--%>
                                                </form>    
                                        <br /><br />
                               
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
                         
                                </asp:Content>


