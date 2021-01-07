<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="VerifyPayments.aspx.cs" Inherits="VerifyPayments" %>

    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .style1 {
            width: 255px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">   
    
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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
                                        <h4>VERIFY AND APPROVE/DECLINE PAYMENTS</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" class="text-danger ml-2"></asp:Label>
                                          <asp:Label ID="lblResultMsg" runat="server" Text="" class="text-success ml-2"></asp:Label>

                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Class:<span class="text-danger ml-2">*</span></label>
                                         <asp:DropDownList runat="server" ID="ddlYear" class="custom-select form-control" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                            </asp:DropDownList>
                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Grade:<span class="text-danger ml-2">*</span></label>
                                                    <asp:DropDownList runat="server" class="custom-select form-control" ID="ddlGrade" DataTextField="GradeName" DataValueField="Id" AppendDataBoundItems="true" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Session:<span class="text-danger ml-2">*</span></label>
                                            <asp:DropDownList runat="server" class="custom-select form-control" ID="ddlSession">
                                             </asp:DropDownList>                                                      
                                                </div>
                                               <div class="col-xl-6">
                                                 <label class="form-control-label">Term:<span class="text-danger ml-2">*</span></label>
                                            <asp:DropDownList runat="server" class="custom-select form-control" ID="ddlTerm">
                                             </asp:DropDownList>                                                        
                                                     </div>
                                                      </div>
                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSave" class="btn btn-secondary" OnClick="btnSave_Click"  runat="server" Text="View Payment List"  />
                                                </div>
                                            </div>
                                                </form>    
                                        <br /><br />
                                 <%--<div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>CLASS ARMS</h4>
                                 </div>--%>
                            <%--<asp:GridView ID="gvdAcademicSession" runat="server" AutoGenerateColumns="False" EmptyDataText=" No Calendar year currently exist" AllowPaging="true" Width="100%" ShowFooter="True" PageSize="20" GridLines="Both" OnRowCommand="gvdAcademicSession_RowCommand">       
                            <PagerSettings FirstPageText="&lt;i class=&quot;fa  fa-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-left&quot;&gt;&lt;/i&gt;"
                                LastPageText="&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;" Mode="NextPreviousFirstLast" 
                                NextPageText="&lt;i class=&quot;fa fa-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;fa fa-arrow-left&quot;&gt;&lt;/i&gt;" />--%>
                                <asp:Label ID="lblList" Font-Size="Large" ForeColor="Black" runat="server" Visible="false" Text="PAYMENTS LIST"></asp:Label>
                                        <asp:Panel runat="server"  ID="pnlAssignmentUpload" ScrollBars="Horizontal">
                                        <asp:GridView ID="gvPayment" EmptyDataText="Empty List" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" class="table table-striped"
                            CellPadding="4" ForeColor="#333333"  Width="1000px" GridLines="None" OnPageIndexChanging="gvPayment_PageIndexChanging" OnRowCommand="gvPayment_RowCommand" OnRowDataBound="gvPayment_RowDataBound">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="InvoiceCode" HeaderText="Invoice Code" />
                                <asp:BoundField DataField="BankName" HeaderText="Bank Name" />
                                <asp:BoundField DataField="DepositorAccountName" HeaderText="Depositor/Account Name" />
                                <asp:BoundField DataField="ReferenceCode" HeaderText="Reference Code" />
                                <asp:BoundField DataField="Amount" HeaderText="Amount(N)" />
                                <asp:BoundField DataField="Date" HeaderText="Date" />
                                <asp:TemplateField HeaderText="Approval Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" CausesValidation="False" Text='<%# Eval("Status") %>' CssClass="input-group input-group-lg"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="false">
                                    <ItemTemplate>
                                        <headerstyle width="100px" />
                                        <asp:LinkButton ID="lnkApprove" runat="server" CommandArgument='<%# Eval("Id") %>'
                                            CommandName="approve" SkinID="lnkGreen" ForeColor="White" class="btn btn-success" BackColor="Green" OnClientClick="javascript:return confirm('Are you sure you want to approve this payment?');"> Approve </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="false">
                                    <ItemTemplate>
                                        <headerstyle width="100px" />
                                        <asp:LinkButton ID="lnkDecline" runat="server" CommandArgument='<%# Eval("Id") %>'
                                            CommandName="decline" SkinID="lnkGreen" ForeColor="White" class="btn btn-danger" BackColor="Red" OnClientClick="javascript:return confirm('Are you sure you want to decline this payment?');"> Decline </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblId" runat="server" CausesValidation="False" Text='<%# Eval("Id") %>' CssClass="input-group input-group-lg" Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
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
                                LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" Mode="NextPreviousFirstLast" 
                                NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />
                                        </asp:GridView>
                                    </asp:Panel>

                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
                        
                                </asp:Content>

