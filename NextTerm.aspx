<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="NextTerm.aspx.cs" Inherits="NextTerm" %>

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
                                        <h4>REPORT CARD(SET WHEN NEXT TERM BEGINS)</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" class="text-success ml-2"></asp:Label>
                                          <asp:Label ID="lblResultMsg" runat="server" Text="" class="text-success ml-2"></asp:Label>

                                            
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Select Date:<span class="text-danger ml-2">*</span></label>
                                                   <asp:TextBox ID="txtDateStart" placeholder="MM-dd-yyyy" class="form-control" TabIndex="25"  runat="server" MaxLength="10"></asp:TextBox>
                                               <ajaxToolKit:CalendarExtender ID="myDateFromCal" Format="MM-dd-yyyy"  runat="server"  TargetControlID="txtDateStart" PopupButtonID="ImageButton1" PopupPosition="BottomLeft"></ajaxToolKit:CalendarExtender>
                                                       
                                                </div>
                                                      </div>
                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSaveAcademicSession" OnClick="btnSaveAcademicSession_OnClick" class="btn btn-secondary" runat="server" Text="Save"  />
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
