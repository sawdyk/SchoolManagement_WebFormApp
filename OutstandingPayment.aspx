<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="OutstandingPayment.aspx.cs" Inherits="OutstandingPayment" %>


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
                                        <h4>OUTSTANDING PAYMENTS</h4>
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
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                       <label class="form-control-label">Term:<span class="text-danger ml-2">*</span></label>
                                             <asp:DropDownList runat="server" ID="ddlTerm" class="custom-select form-control" DataTextField="AcademicTermName" DataValueField="Id"></asp:DropDownList>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Session:<span class="text-danger ml-2">*</span></label>
                                    <asp:DropDownList runat="server" DataTextField="SessionName" class="custom-select form-control" DataValueField="ID" ID="ddlSession"></asp:DropDownList>
                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Student:<span class="text-danger ml-2">*</span></label>
                             <asp:DropDownList ID="ddlStudent" DataTextField="Name" DataValueField="Id" class="custom-select form-control"  runat="server"></asp:DropDownList>                                                                  
                                                </div>
                                                <div class="col-xl-6">
                                                    <asp:label ID="lblInvoiceCode" runat="server" Visible="false"  Text="Invoice_Code" class="form-control-label"></asp:label>
                           <asp:DropDownList ID="ddlInvoiceCode" class="custom-select form-control" Visible="false"     runat="server"></asp:DropDownList>  

                                                </div>
                                            </div>
                                           <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <asp:label ID="lblBalanceToPay" runat="server" Visible="false"  Text="Amount:" class="form-control-label"></asp:label>
                                        <asp:Textbox ID="txtBalanceToPay" class="form-control" Visible="false" runat="server"></asp:Textbox>                        
                                              
                                                </div>
                                                
                                            </div>
                                        

                                           <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                     
                                                    <asp:Button ID="Button1" class="btn btn-secondary"  OnClick="btnView_OnClick" runat="server" Text="View Payment"  />
                                            </div>                                    
                                                <div class="col-xl-6">
                                                    
                                                    <asp:Button ID="btnSaveBal" class="btn btn-secondary" visible="false"  OnClick="btnSaveBal_OnClick" runat="server" Text="Make_Payment"  />
                                            </div>                           
                                               
                                            </div>
                                                
                                                </form>    
                                        <br />
                                       <asp:Label ID="lblSummary" ForeColor="Black" runat="server" Visible="false" Text="PAYMENT SUMMARY"></asp:Label>
                            <%--<asp:GridView ID="gvdAcademicSession" runat="server" AutoGenerateColumns="False" EmptyDataText=" No Calendar year currently exist" AllowPaging="true" Width="100%" ShowFooter="True" PageSize="20" GridLines="Both" OnRowCommand="gvdAcademicSession_RowCommand">       
                            <PagerSettings FirstPageText="&lt;i class=&quot;fa  fa-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-left&quot;&gt;&lt;/i&gt;"
                                LastPageText="&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;" Mode="NextPreviousFirstLast" 
                                NextPageText="&lt;i class=&quot;fa fa-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;fa fa-arrow-left&quot;&gt;&lt;/i&gt;" />--%>

                               <asp:GridView ID="gvOutStandPaySummary"  class="table table-striped" GridLines="None" runat="server" AllowPaging="True" AllowSorting="false" CellPadding="4" ForeColor="#333333" 
                                                OnPageIndexChanging="gvOutStandPaySummary_PageIndexChanging" PageSize="20">
                                                <Columns>
                                                    <%--<asp:CommandField ShowDeleteButton="True" />--%>
                                                </Columns>
                                                <AlternatingRowStyle BackColor="White" />
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <HeaderStyle BackColor="#3366ff" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                    <RowStyle BackColor="#EFF3FB" />
                                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                <PagerSettings FirstPageText="&lt;i class=&quot;la  la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                                    LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                                    Mode="NextPreviousFirstLast" NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" 
                                    PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />                                                    
                               </asp:GridView>  

                                        <br /><br />
                                       <asp:Label ID="lblList" ForeColor="Black" runat="server" Visible="false" Text="PAYMENT LIST"></asp:Label>
                                        <asp:GridView ID="gvOutStandPayList" class="table table-striped" GridLines="None"  runat="server" AllowPaging="True" AllowSorting="False"  CellPadding="4" ForeColor="#333333" 
                                                 OnPageIndexChanging="gvOutStandPayList_PageIndexChanging"  PageSize="20">
                                               <%-- <Columns>
                                                  
                                                </Columns>--%>
                                                <AlternatingRowStyle BackColor="White" />
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <HeaderStyle BackColor="#3366ff" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                    <RowStyle BackColor="#EFF3FB" />
                                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                    <PagerSettings FirstPageText="&lt;i class=&quot;la  la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                                    LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                                    Mode="NextPreviousFirstLast" NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" 
                                    PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />                                            

                                        </asp:GridView>  
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                         
                                </asp:Content>

