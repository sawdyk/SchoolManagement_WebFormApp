<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="SupAdminViewSubscription.aspx.cs" Inherits="SupAdminViewSubscription" %>

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
                                        <h4>VIEW SUBSCRIPTIONS</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblReport" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>

                                               <asp:Label ID="lblUsers" Font-Size="Large" ForeColor="Black" runat="server" Visible="true" Text="ALL SUBSCRIPTIONS"></asp:Label>
                                        <br /><br />

                                           <asp:GridView ID="gdvSubList" runat="server" AutoGenerateColumns="false" GridLines="None" EmptyDataText=" No record Currently exist "
                    AllowPaging="true" Width="100%" class="table table-striped"   CellPadding="4" ForeColor="#333333"  OnPageIndexChanging="gdvSubList_PageIndexChanging" >
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
                        <asp:TemplateField HeaderText=" S/N">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="School Name">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("SchoolName")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount Paid">
                            <ItemTemplate>
                                <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Amount")  %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount Used">
                            <ItemTemplate>
                                <asp:Label ID="lblAmountUsed" runat="server" Text='<%# Eval("AmountUsed")  %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Payment Date">
                            <ItemTemplate>
                                <asp:Label ID="lblPaymentDate" runat="server" Text='<%# Eval("PaymentDate","{0:dd, MMMM yyyy}")  %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <div class="modal fade" id='<%# "mymodal" + Eval("ID") %>'>
                                    <div class="modal-dialog" style="width: 60%">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" class="close" data-dismiss="modal">
                                                    <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                                <h4 class="modal-title">Subscription Details</h4>
                                            </div>
                                            <div class="modal-body">
                                                <table class="table table-striped table-hover">
                                                    <tr>
                                                        <td><strong>School Name:</strong></td>
                                                        <td><%# Eval("SchoolName") %></td>
                                                    </tr>
                                                    <tr>
                                                        <td><strong>Campus Name:</strong></td>
                                                        <td><%# Eval("CampusName") %></td>
                                                    </tr>
                                                    <tr>
                                                        <td><strong>Academic Session:</strong></td>
                                                        <td><%# Eval("SessionName") %></td>
                                                    </tr>
                                                    <tr>
                                                        <td><strong>Academic Term:</strong></td>
                                                        <td><%# Eval("TermName") %></td>
                                                    </tr>
                                                    <tr>
                                                        <td><strong>Amount paid:</strong></td>
                                                        <td><%# Eval("Amount") %></td>
                                                    </tr>
                                                    <tr>
                                                        <td><strong>Amount Used:</strong></td>
                                                        <td><%# Eval("AmountUsed") %></td>
                                                    </tr>
                                                    <tr>
                                                        <td><strong>Payment Date:</strong></td>
                                                        <td><%# Eval("PaymentDate","{0:dd, MMMM yyyy}") %></td>
                                                    </tr>
                                                    <tr>
                                                        <td><strong>Has Fully Utilized:</strong></td>
                                                        <td><%# Eval("FullyUtilized") %></td>
                                                    </tr>
                                                </table>
                                            </div>

                                        </div>
                                        <!-- /.modal-content -->
                                    </div>
                                    <!-- /.modal-dialog -->
                                </div>
                                <!-- /.modal -->

                                <asp:LinkButton ID="targetbtn" runat="server" ToolTip="View Application Details" data-toggle="modal" data-target='<%# "#mymodal" + Eval("Id") %>' CssClass="input-group input-group-lg"><i class="la la-search"></i></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id")  %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                       <PagerSettings FirstPageText="&lt;i class=&quot;la  la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;"
                                LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" Mode="NextPreviousFirstLast" 
                                NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />
                                                            </asp:GridView>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
                         
                                </asp:Content>



