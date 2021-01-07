<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="CashPayment.aspx.cs" Inherits="CashPayment" %>


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
                                        <h4>MAKE CASH PAYMENT</h4>
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
                                             <asp:DropDownList runat="server" ID="ddlterm" class="custom-select form-control" DataTextField="AcademicTermName" DataValueField="Id"></asp:DropDownList>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Session:<span class="text-danger ml-2">*</span></label>
                                    <asp:DropDownList runat="server" DataTextField="SessionName" class="custom-select form-control" DataValueField="ID" ID="ddlsession"></asp:DropDownList>
                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Student:<span class="text-danger ml-2">*</span></label>
                                        <asp:DropDownList ID="ddlWard"  class="custom-select form-control"  runat="server"></asp:DropDownList>                                                                  
                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Fee Category:<span class="text-danger ml-2">*</span></label>
                           <asp:DropDownList ID="ddlfeeType" OnSelectedIndexChanged="ddlfeeType_SelectedIndexChanged" class="custom-select form-control" AutoPostBack="true"     runat="server"></asp:DropDownList>  

                                                </div>
                                            </div>
                                           <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Fee Type:<span class="text-danger ml-2">*</span></label>
                            <asp:DropDownList ID="ddlCashCategoryType" class="custom-select form-control" OnSelectedIndexChanged="ddlCashCategoryType_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>                        
                                              
                                                </div>
                                                <div class="col-xl-6">
                                                    <asp:label ID="lblAmtToPay" runat="server" Visible="false"  Text="Fee Amount" class="form-control-label"></asp:label>
                                             <asp:DropDownList ID="ddlAmount"  class="custom-select form-control"  Enabled="false" runat="server"></asp:DropDownList>
                                                </div>

                                            </div>
                                         <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Amount To Pay:<span class="text-danger ml-2">*</span></label>
                                                <asp:TextBox ID="txtAmountToPay" class="form-control" Visible="false" runat="server"></asp:TextBox>
                                                
                                                </div>
                                                <div class="col-xl-6">
                                                <label class="form-control-label">Total Amount Generated:<span class="text-danger ml-2">*</span></label>
                                                    <asp:label ID="lblAmount" runat="server" Visible="false" Font-Bold="true" Font-Size="Large" class="form-control-label"></asp:label>
                                                 <asp:Label ID="lblAmountGen" runat="server" Visible="true" Font-Bold="true" Font-Size="Large" Text=""></asp:Label>

                                                </div>
                                            </div>

                                           <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                     
                                                    <asp:Button ID="btnAdd" class="btn btn-secondary"  OnClick="btnAdd_OnClick" runat="server" Text="Add to List"  />&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;   
                                                    <asp:Button ID="btnClear" class="btn btn-secondary" visible="false" OnClick="btnClear_OnClick" runat="server" Text="Clear_Entries"  />

                                            </div>                                    
                                                <div class="col-xl-6">
                                                    <asp:Button ID="btnSave" class="btn btn-secondary" visible="false"  OnClick="btnSave_OnClick" runat="server" Text="Make_Payment"  />
                                            </div>                           
                                            </div>
                                                
                                                </form>    
                                        <br />
                                 <div class="widget-header bordered no-actions d-flex align-items-center">
                                       <asp:Label ID="lblFee" ForeColor="Black" runat="server" Visible="false" Text="SELECTED FEE TYPE"></asp:Label>
                                 </div>
                            <%--<asp:GridView ID="gvdAcademicSession" runat="server" AutoGenerateColumns="False" EmptyDataText=" No Calendar year currently exist" AllowPaging="true" Width="100%" ShowFooter="True" PageSize="20" GridLines="Both" OnRowCommand="gvdAcademicSession_RowCommand">       
                            <PagerSettings FirstPageText="&lt;i class=&quot;fa  fa-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-left&quot;&gt;&lt;/i&gt;"
                                LastPageText="&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;" Mode="NextPreviousFirstLast" 
                                NextPageText="&lt;i class=&quot;fa fa-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;fa fa-arrow-left&quot;&gt;&lt;/i&gt;" />--%>
                              
                                        <asp:GridView ID="gvdAddCashPayments" class="table table-striped"  runat="server" AllowPaging="True" AllowSorting="false"  CellPadding="4" ForeColor="#333333"  PageSize="10" OnPageIndexChanging="gvdAddCashPayments_PageIndexChanging"  
                                                OnRowDeleting="gvdAddCashPayments_RowDeleting" GridLines="None" AutoGenerateColumns="false" >
                                                <Columns>
                                                     <asp:TemplateField HeaderText=" S/N">
                                                    <ItemTemplate>
                                                         <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                     </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fee_Id" Visible="false">
                                                 <ItemTemplate>
                                                       <asp:label ID="Fee_Id" Enabled="false"  runat="server" Text='<%# Eval("Id") %>'></asp:label>                    
                                                 </ItemTemplate>
                                                   </asp:TemplateField>  
                                                     <asp:TemplateField HeaderText="Invoice_Code">
                                                 <ItemTemplate>
                                                      <asp:label ID="InvoiceCode"  class="form-control-label" Enabled="false" runat="server" Text='<%# Eval("InvoiceCode") %>'></asp:label>                      
                                                 </ItemTemplate>
                                                   </asp:TemplateField>  
                                                     <asp:TemplateField HeaderText="Fee_Name">
                                                 <ItemTemplate>
                                                     <asp:label ID="Fee_Name" class="form-control-label" Enabled="false" runat="server" Text='<%# Eval("Fee_Name") %>'></asp:label>                   
                                                 </ItemTemplate>
                                                   </asp:TemplateField>  
                                                     <asp:TemplateField HeaderText="Fee_Amount">
                                                 <ItemTemplate>
                                                      <asp:label ID="Fee_Amount"  class="form-control-label"  Enabled="false" runat="server" Text='<%# Eval("Fee_Amount") %>'></asp:label>                       
                                                 </ItemTemplate>
                                                   </asp:TemplateField>  
                                                    <asp:CommandField ShowDeleteButton="True" ControlStyle-CssClass="btn btn-danger"/>
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
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
                         
                                </asp:Content>
