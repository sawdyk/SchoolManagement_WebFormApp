﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="FinanceDiscounts.aspx.cs" Inherits="FinanceDiscounts" %>


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
                                        <h4>ADD DISCOUNTS</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Class:<span class="text-danger ml-2">*</span></label>
                                  <asp:DropDownList runat="server" ID="ddlYear" DataTextField="Name" class="custom-select form-control" DataValueField="id" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" AutoPostBack="true"    >
                            </asp:DropDownList>                                            
                                                </div>
                                                <div class="col-xl-6">
                                     <label class="form-control-label">Grade:<span class="text-danger ml-2">*</span></label>
                                 <asp:DropDownList ID="ddlClass" DataTextField="GradeName" class="custom-select form-control" DataValueField="Id"  runat="server"></asp:DropDownList>                       
                                                
                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                       <label class="form-control-label">Term:<span class="text-danger ml-2">*</span></label>
                                             <asp:DropDownList runat="server" ID="ddlterm" class="custom-select form-control" DataTextField="AcademicTermName" DataValueField="Id"></asp:DropDownList>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Session:<span class="text-danger ml-2">*</span></label>
                                    <asp:DropDownList runat="server" class="custom-select form-control" ID="ddlsession"></asp:DropDownList>
                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Fee Category:<span class="text-danger ml-2">*</span></label>
                           <asp:DropDownList ID="ddlFeeCategory" class="custom-select form-control" OnSelectedIndexChanged="ddlFeeCategory_SelectedIndexChanged" AutoPostBack="true"     runat="server"></asp:DropDownList>  
                                           
                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Fee Type:<span class="text-danger ml-2">*</span></label>
                            <asp:DropDownList ID="ddlFeeTypee" class="custom-select form-control" OnSelectedIndexChanged="ddlFeeTypee_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>                        

                                                </div>
                                            </div>
                                           <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Fee Amount:<span class="text-danger ml-2">*</span></label>
                            <asp:DropDownList ID="ddlGetFeeAmountId" class="custom-select form-control"  runat="server"></asp:DropDownList>                        
                                              
                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Discount(%):<span class="text-danger ml-2">*</span></label>                                                  
                                                <asp:TextBox ID="txtDiscount" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                         <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Status:<span class="text-danger ml-2">*</span></label>
                            <asp:DropDownList ID="ddlStatus" class="custom-select form-control"  runat="server"></asp:DropDownList>                        
                                                
                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Number of Child/Ward:<span class="text-danger ml-2">*</span></label>
                            <asp:TextBox ID="txtNumChild" class="form-control"  Visible="True" runat="server"></asp:TextBox>                        

                                                </div>
                                            </div>
                                            <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnAdd" class="btn btn-secondary"  OnClick="btnAdd_OnClick" runat="server" Text="Add Discount"  />
                                                </div>
                                            </div>
                                                
                                                </form>    
                                        <br />
                                <%-- <div class="widget-header bordered no-actions d-flex align-items-center">
                                       <asp:Label ID="lblsubjectResp" ForeColor="Black" runat="server" Visible="false" Text="LIST OF SUBJECTS"></asp:Label>
                                 </div>--%>
                            <%--<asp:GridView ID="gvdAcademicSession" runat="server" AutoGenerateColumns="False" EmptyDataText=" No Calendar year currently exist" AllowPaging="true" Width="100%" ShowFooter="True" PageSize="20" GridLines="Both" OnRowCommand="gvdAcademicSession_RowCommand">       
                            <PagerSettings FirstPageText="&lt;i class=&quot;fa  fa-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-left&quot;&gt;&lt;/i&gt;"
                                LastPageText="&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;" Mode="NextPreviousFirstLast" 
                                NextPageText="&lt;i class=&quot;fa fa-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;fa fa-arrow-left&quot;&gt;&lt;/i&gt;" />--%>
                       <asp:Label ID="lblDiscounts" Font-Size="Large" ForeColor="Black" runat="server" Visible="true" Text="LIST OF DISCOUNTS"></asp:Label>
                                        <asp:GridView ID="gdvLists" runat="server" AllowPaging="True"  class="table table-striped" AllowSorting="false"  
                                               GridLines="None" CellPadding="4" Width="1000px" ForeColor="#333333"  PageSize="10" OnPageIndexChanging="gdvLists_PageIndexChanging"  
                                               AutoGenerateColumns="false">
                                                <EditRowStyle BackColor="#2461BF" />
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#3366ff" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EFF3FB" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                                <Columns>
                                                     <asp:TemplateField HeaderText=" S/N">
                                                    <ItemTemplate>
                                                         <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                     </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Fee Type">
                                                 <ItemTemplate>
                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("FeeName")  %>'></asp:Label>                
                                                 </ItemTemplate>
                                                   </asp:TemplateField>  
                                                     <asp:TemplateField HeaderText="Class">
                                                 <ItemTemplate>
                                                     <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name")  %>'></asp:Label>
                                                 </ItemTemplate>
                                                   </asp:TemplateField>  
                                                     <asp:TemplateField HeaderText="Grade">
                                                 <ItemTemplate>
                                                     <asp:Label ID="lblName" runat="server" Text='<%# Eval("GradeName")  %>'></asp:Label>
                                                 </ItemTemplate>
                                                   </asp:TemplateField>  
                                                     <asp:TemplateField HeaderText="Session">
                                                 <ItemTemplate>
                                                     <asp:Label ID="lblName" runat="server" Text='<%# Eval("SessionName")  %>'></asp:Label>
                                                 </ItemTemplate>
                                                   </asp:TemplateField>  
                                                    <asp:TemplateField HeaderText="Term">
                                                 <ItemTemplate>
                                                     <asp:Label ID="lblName" runat="server" Text='<%# Eval("AcademicTermName")  %>'></asp:Label>
                                                 </ItemTemplate>
                                                   </asp:TemplateField>  
                                                    <asp:TemplateField HeaderText="Discount">
                                                 <ItemTemplate>
                                                     <asp:Label ID="lblName" runat="server" Text='<%# Eval("Discount")  %>'></asp:Label>
                                                 </ItemTemplate>
                                                   </asp:TemplateField>  
                                                    <asp:TemplateField HeaderText="Number of Child">
                                                 <ItemTemplate>
                                                     <asp:Label ID="lblName" runat="server" Text='<%# Eval("NumChild")  %>'></asp:Label>
                                                 </ItemTemplate>
                                                   </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Status">
                                                 <ItemTemplate>
                                                     <asp:Label ID="lblName" runat="server" Text='<%# Eval("StatusName")  %>'></asp:Label>
                                                 </ItemTemplate>
                                                   </asp:TemplateField>  
                                                     <asp:TemplateField HeaderText="Date">
                                                 <ItemTemplate>
                                                     <asp:Label ID="lblName" runat="server" Text='<%# Eval("Date")  %>'></asp:Label>
                                                 </ItemTemplate>
                                                   </asp:TemplateField>    

                                                </Columns>
                    <PagerSettings FirstPageText="&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                        LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                        Mode="NextPreviousFirstLast" 
                        NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" 
                        PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />                  </asp:GridView>
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
                         
                                </asp:Content>
