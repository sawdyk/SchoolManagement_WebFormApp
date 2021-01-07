<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="MakePayment.aspx.cs" Inherits="MakePayment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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
                                        <h4>MAKE PAYMENT</h4>&nbsp;&nbsp;&nbsp;&nbsp;
<%--                                      <asp:Label runat="server" Text="(NB:Click on 'Get total' to get total amount to be paid before generating invoice!)" ForeColor="Red"></asp:Label>  --%>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" Visible="false" class="text-danger ml-2"></asp:Label>
                                             <asp:Label ID="lblSuccessMsg" runat="server" Text="" Visible="false" class="text-success ml-2"></asp:Label>

                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Select Invoice Code:<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList ID="ddlInvoiceCode" class="custom-select form-control" runat="server" DataTextField="InvoiceCode" DataValueField="Id"
                                                    AppendDataBoundItems="true">
                                                    <asp:ListItem Enabled="true" Text="--Select Invoice Code--" Value="0"></asp:ListItem>
                                                </asp:DropDownList>                                            
                                                </div>
                                                <div class="col-xl-6">
                                              <label class="form-control-label">Payment Method:<span class="text-danger ml-2">*</span></label>
                                                    <asp:DropDownList ID="ddlPaymentMethod" AutoPostBack="true" class="custom-select form-control" OnSelectedIndexChanged="ddlPaymentMethod_SelectedIndexChanged" runat="server" AppendDataBoundItems="true">
                                                    <asp:ListItem Enabled="true" Text="--Select payment Method --" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Card Payment(MasterCard,Verve or Visa)" Value="1" />
                                                    <asp:ListItem Text="Bank Deposit" Value="2" />
                                                    <asp:ListItem Text="Bank Online Transfer" Value="3" />
                                                </asp:DropDownList>                                            

                                                </div>
                                            </div>

                                                 <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <asp:label runat="server" ID="lblAmounttoPay" visible="false" Text="Amount(N):" class="form-control-label"><span class="text-danger ml-2">Amount(N):*</span></asp:label>
                                                    <asp:TextBox ID="txtAmounttoPay" visible="false" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-xl-6">
                                                    <asp:label runat="server" ID="lblInvoiceRef" visible="false" Text="Reference No:" class="form-control-label"><span class="text-danger ml-2">Reference No:*</span></asp:label>
                                                    <asp:TextBox ID="txtRefNo" visible="false" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                             <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <asp:label runat="server" ID="lblBankName" visible="false" Text="Bank Name:" class="form-control-label"><span class="text-danger ml-2">Bank Name:*</span></asp:label>
                                                    <asp:TextBox ID="txtBankName" visible="false" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-xl-6">
                                                    <asp:label runat="server" ID="lblBankRef" visible="false" Text="Depositor/Account Name:" class="form-control-label"><span class="text-danger ml-2">Depositor/Account Name:*</span></asp:label>
                                                    <asp:TextBox ID="txtDepAcctName" visible="false" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                             <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <asp:label runat="server" ID="lblPaymentDate" visible="false" Text="Payment Date." class="form-control-label"><span class="text-danger ml-2">Payment Date:*</span></asp:label>
                                                <asp:TextBox ID="txtBankPaymentDate" class="form-control" visible="false" placeholder="MM-dd-yyyy" TabIndex="25"  runat="server" MaxLength="10"></asp:TextBox>
                                                     <ajaxToolKit:CalendarExtender ID="CalendarExtender1" Format="MM-dd-yyyy"  runat="server"  TargetControlID="txtBankPaymentDate" PopupButtonID="ImageButton1" PopupPosition="BottomLeft">
                                                                            </ajaxToolKit:CalendarExtender>   

                                                </div>
                                                <%--<div class="col-xl-6">
                                                    <asp:label runat="server" ID="Label3" visible="false" Text="Depositor/Account Name:" class="form-control-label"><span class="text-danger ml-2">*</span></asp:label>
                                                    <asp:TextBox ID="TextBox2" visible="false" class="form-control" runat="server"></asp:TextBox>
                                                </div>--%>
                                            </div>
                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnMakePayment" class="btn btn-secondary" runat="server" Text="Make Payment"  OnClick="btnMakePayment_Click" />
                                                </div>
                                            </div>
                                                    
                                        <br /><br />
                                 <%--<div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>CLASS ARMS</h4>
                                 </div>--%>
                                <asp:Label ID="lblLists" Font-Size="Large" ForeColor="Black" Text="PAYMENTS LIST" runat="server" Visible="false"></asp:Label>
                                                     <br /><br />
                                <asp:GridView ID="gvPayment" Width="100%" CellPadding="4" EmptyDataText="Empty List" class="table table-striped" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CssClass="auto-style4"
               ForeColor="#333333" GridLines="None" OnPageIndexChanging="gvPayment_PageIndexChanging" >
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="InvoiceCode" HeaderText="Invoice Code" />
                    <asp:BoundField DataField="BankName" HeaderText="Bank Name" />
                    <asp:BoundField DataField="DepositorAccountName" HeaderText="Depositor/Account Name" />
                    <asp:BoundField DataField="ReferenceCode" HeaderText="Reference Code" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount(N)" />
                    <asp:BoundField DataField="Date" HeaderText="Date" />
                    <%--<asp:BoundField DataField="Status" HeaderText="Approval Status" />--%>
                    <asp:TemplateField HeaderText="Approval Status">
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" runat="server" CausesValidation="False" Text='<%# Eval("Status") %>' CssClass="input-group input-group-lg"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="lblId" runat="server" CausesValidation="False" Text='<%# Eval("Id") %>' CssClass="input-group input-group-lg" Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField ShowHeader="False">
                        <EditItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("Id")  %>' CausesValidation="True" CommandName="Update" Text="Update" ToolTip="Update"><i class="fa fa-floppy-o fa-2x"></i></asp:LinkButton>
                            &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CommandArgument='<%# Eval("Id")  %>' CausesValidation="False" CommandName="Cancel" Text="Cancel" ToolTip="Cancel"><i class="fa fa-remove fa-2x"></i></asp:LinkButton>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" CommandArgument='<%# Eval("Id")  %>' runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" ToolTip="Edit"><i class="fa fa-edit fa-2x"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
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

                <PagerSettings FirstPageText="&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                        LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                        Mode="NextPreviousFirstLast" 
                        NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" 
                        PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />                 
                                </asp:GridView>
                                               

                                            </div>
                                            </div>
                                            </form>
                                                </div>
                                            </div>                                        
                                        </form>

                                 </div>
                                </div>    
                                </asp:Content>

