<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="DeleteInvoice.aspx.cs" Inherits="DeleteInvoice" %>

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
                                        <h4>DELETE GENERATED INVOICES</h4>&nbsp;&nbsp;&nbsp;&nbsp;
<%--                                      <asp:Label runat="server" Text="(NB:You can't Generate an Invoice if payment has been made previously on a generated Invoice!)" ForeColor="Red"></asp:Label>  --%>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" class="text-success ml-2"></asp:Label>
                                           
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Invoice:<span class="text-danger ml-2">*</span></label>
                                                <asp:DropDownList ID="ddlInvoiceCode" AutoPostBack="true" class="custom-select form-control"  runat="server" DataTextField="InvoiceCode" DataValueField="Id"
                                                                    AppendDataBoundItems="true" OnSelectedIndexChanged="ddlInvoiceCode_SelectedIndexChanged">
                                                                    <asp:ListItem Enabled="true" Text="--Select Invoice Code--" Value="0"></asp:ListItem>
                                                                </asp:DropDownList>                                                
                                                     </div>
                                               <%-- <div class="col-xl-6">
                                                    <label class="form-control-label">Teacher:<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList ID="ddlTeacher" DataTextField="LastName" DataValueField="Id" class="custom-select form-control" runat="server"></asp:DropDownList>
                                                </div>--%>
                                            </div>
                                               <%--<div class="col-xl-6">
                                                 <label class="form-control-label">Class<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList ID="ddlClass" OnSelectedIndexChanged="ddlClass_SelectedIndexChanged" AutoPostBack="true" DataTextField="Name" DataValueField="Id" class="custom-select form-control" runat="server"></asp:DropDownList>
                                                   </div>--%>
                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnDeleteInvoice" OnClientClick="javascript:return confirm('Are you sure you want to delete this invoice?');" class="btn btn-secondary" runat="server" Text="Delete Invoice"  OnClick="btnDeleteInvoice_Click" />
                                                </div>
                                            </div>
                                                    
                                        <br /><br />
                                 <%--<div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>CLASS ARMS</h4>
                                 </div>--%>
                                      <asp:Panel ID="Panel2" Width="1000px" class="table table-striped" ScrollBars="Vertical" runat="server">
                                 <asp:Label ID="lblSummary" ForeColor="Black" runat="server" Visible="false" Text="PAYMENT SUMMARY"></asp:Label>
                            <asp:GridView ID="gvOutStandPaySummary" Width="1000px" class="table table-striped" GridLines="None" runat="server" AllowPaging="True" AllowSorting="false" CellPadding="4" ForeColor="#333333" 
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
                                          </asp:Panel>
                                        <br /><br />
                    <asp:Panel ID="Panel1" Width="1000px" class="table table-striped" ScrollBars="Vertical" runat="server">
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
                        </asp:Panel>
                                            </div>
                                            </div>
                                            </form>
                                                </div>
                                            </div>                                        
                                        </form>

                                 </div>
                                </div>
                        
                                </asp:Content>

