<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="PaymentLog.aspx.cs" Inherits="PaymentLog" %>

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
                                        <h4>PAYMENT LOG</h4>&nbsp;&nbsp;&nbsp;&nbsp;
<%--                                      <asp:Label runat="server" Text="(NB:Click on 'Get total' to get total amount to be paid before generating invoice!)" ForeColor="Red"></asp:Label>  --%>
                                    </div>
                                    <div class="widget-body">
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" Visible="false" class="text-danger ml-2"></asp:Label>
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
                                                    <label class="form-control-label">Child:<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList ID="ddlWard" runat="server"  class="custom-select form-control" DataTextField="FullName" DataValueField="Id"
                                                    AppendDataBoundItems="true" AutoPostBack="false">
                                                    <asp:ListItem Enabled="true" Text="--Select ward--" Value="0"></asp:ListItem>
                                                </asp:DropDownList>                                         
                                                </div>
                                            </div>
                                                
                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnViewPaymentLog" class="btn btn-secondary" runat="server" Text="View Payments"  OnClick="btnViewPaymentLog_Click" />
                                                </div>
                                            </div>
                                                    
                                        <br /><br />
                                 <%--<div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>CLASS ARMS</h4>
                                 </div>--%>
                                <asp:Label ID="lblSummary" Font-Size="Large" ForeColor="Black" runat="server" Visible="false" Text="PAYMENT LOG"></asp:Label>

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
                                            
                                            </div>
                                            </div>
                                            </form>
                                                </div>
                                            </div>                                        
                                        </form>

                                 </div>
                                </div>    
                                </asp:Content>

