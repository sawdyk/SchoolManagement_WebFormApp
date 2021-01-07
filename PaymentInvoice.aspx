<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="PaymentInvoice.aspx.cs" Inherits="PaymentInvoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
   
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
                                        <h4>GENERATE INVOICE </h4>&nbsp;&nbsp;&nbsp;&nbsp;
                                      <asp:Label runat="server" Text="(NB:Click on 'Get total' to get total amount to be paid before generating invoice!)" ForeColor="Red"></asp:Label>  
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" Visible="false" class="text-danger ml-2"></asp:Label>
                                           
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Child:<span class="text-danger ml-2">*</span></label>
                                                    <asp:DropDownList ID="ddlWard" AutoPostBack="true" class="custom-select form-control" runat="server" DataTextField="StudentFullName" DataValueField="Id"
                                                            AppendDataBoundItems="true" OnSelectedIndexChanged="ddlWard_SelectedIndexChanged">
                                                            <asp:ListItem Enabled="true" Text="--Select Ward--" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>                                               
                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Class:<span class="text-danger ml-2">*</span></label>
                                                    <asp:DropDownList ID="ddlClass" class="custom-select form-control" runat="server" DataTextField="Name" DataValueField="Id"
                                                                AppendDataBoundItems="true">
                                                                <asp:ListItem Enabled="true" Text="--Select Class--" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>                                                

                                                </div>
                                            </div>
                                                 <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Term:<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList ID="ddlTerm" runat="server" class="custom-select form-control"  DataTextField="AcademicTermName" DataValueField="Id"
                                                    AppendDataBoundItems="true">
                                                    <asp:ListItem Enabled="true" Text="--Select Term--" Value="0"></asp:ListItem>
                                                </asp:DropDownList>                                            
                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Session:<span class="text-danger ml-2">*</span></label>
                                                    <asp:DropDownList ID="ddlSession" class="custom-select form-control" runat="server" DataTextField="SessionName" DataValueField="ID"
                                                        AppendDataBoundItems="true">
                                                        <asp:ListItem Enabled="true" Text="--Select Session--" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>                                              

                                                </div>
                                            </div>

                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnViewPaymentList" class="btn btn-secondary" runat="server" Text="View Payment List"  OnClick="btnViewPaymentList_Click" />
                                                </div>
                                            </div>
                                                    
                                        <br /><br />
                                 <%--<div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>CLASS ARMS</h4>
                                 </div>--%>
                                <asp:Label ID="lblLists" Font-Size="Large" ForeColor="Black" runat="server" Visible="false"></asp:Label>
                                <asp:GridView ID="gdvList" runat="server" ForeColor="Black" GridLines="None" AutoGenerateColumns="false" class="table table-striped" EmptyDataText=" No record Currently exist "
                    AllowPaging="true" Width="100%" PageSize="30" OnRowDataBound="gdvList_RowDataBound">
                    <AlternatingRowStyle BackColor="White" />
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
                        <asp:TemplateField HeaderText="Check All">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkAll" OnCheckedChanged="chkAll_CheckedChanged" AutoPostBack="true" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" AutoPostBack="true" runat="server" OnCheckedChanged="CheckBox1_CheckedChanged"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" S/N">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="FeeName" HeaderText="Fee" Visible="true" />
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblFeeId" runat="server" Text='<%# Eval("Id")  %>'></asp:Label>
<%--                                <asp:Label ID="lblFeeTypeId" runat="server" Text='<%# Eval("FeeTypeId")  %>'></asp:Label>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Amount" HeaderText="Amount" Visible="true" />
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id")  %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblMandatory" runat="server" Text='<%# Eval("Mandatory")  %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                    <PagerSettings FirstPageText="&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                        LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                        Mode="NextPreviousFirstLast" 
                        NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" 
                        PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />                  

                                </asp:GridView>
                                             <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                          <asp:label runat="server" ID="lblUpdatedTotalSelectedAmount" class="form-control-label">Total Selected Amount(N)::<span class="text-danger ml-2">*</span></asp:label>
                                <asp:Label ID="lblTotalInvoicePayable" ForeColor="Black" runat="server" ></asp:Label>
                                     
                                                </div>
                                                <div class="col-xl-6">
                          <asp:label runat="server" ID="lbltotalAmtAfterDiscount" class="form-control-label">Total Amount After Discount Applied(N):<span class="text-danger ml-2">*</span></asp:label>
                                <asp:Label ID="lblTotalAmountAfterDiscount" ForeColor="Black" runat="server"></asp:Label>

                                                </div>
                                            </div>
                                         <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                          <asp:label runat="server" ID="lblTotalDiscount" class="form-control-label">Total Discount Calculated(N):<span class="text-danger ml-2">*</span></asp:label>
                                <asp:Label ID="lbltotalDiscountCalculated" ForeColor="Black" runat="server"></asp:Label>
                                     
                                                </div>
                                               <%-- <div class="col-xl-6">
                                                    <label class="form-control-label">Class:<span class="text-danger ml-2">*</span></label>
                                <asp:Label ID="Label4" ForeColor="Black" runat="server" Visible="false" Text="LIST OF FEES"></asp:Label>

                                                </div>--%>
                                            </div>
                                                <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                  <asp:Button ID="btnGenerateInvoice" class="btn btn-secondary" runat="server" Text="Generate Invoice"  OnClick="btnGenerateInvoice_Click" />
                                     
                                                </div>
                                                <div class="col-xl-6">
                 <asp:Button ID="btnReset" class="btn btn-secondary" runat="server" Text="Reset"  OnClick="btnReset_Click" />   &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btnTotal" class="btn btn-secondary" runat="server" Text="Get Total"  OnClick="btnTotal_Click" />

                                                </div>
                                            </div>

                                            </div>
                                            </div>
                                            </form>
                                                </div>
                                            </div>                                        
                                        </form>

                                 </div>
                                </div>    
                                </asp:Content>
