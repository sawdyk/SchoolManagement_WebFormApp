<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="ViewChildDetails.aspx.cs" Inherits="ViewChildDetails" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>--%>

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
                                        <h4>CHILD/CHILDREN</h4>&nbsp;&nbsp;&nbsp;&nbsp;
<%--                                      <asp:Label runat="server" Text="(NB:Click on 'Get total' to get total amount to be paid before generating invoice!)" ForeColor="Red"></asp:Label>  --%>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" Visible="false" class="text-danger ml-2"></asp:Label>
                                             <asp:Label ID="lblSuccessMsg" runat="server" Text="" Visible="false" class="text-success ml-2"></asp:Label>

                                           
                                <asp:Label ID="lblLists" ForeColor="Black" Text="PAYMENTS LIST" runat="server" Visible="false"></asp:Label>
                                <asp:GridView ID="gdvList" Width="100%" EmptyDataText="Empty List" class="table table-striped" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CssClass="auto-style4"
                                    CellPadding="4" ForeColor="#333333" GridLines="None" OnPageIndexChanging="gdvList_PageIndexChanging" >
                                    <AlternatingRowStyle BackColor="White" />
                                   <Columns>
                                    <asp:TemplateField HeaderText=" S/N">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=" Username ">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Username")  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=" Firstname ">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCode" runat="server" Text='<%#  Eval("Firstname") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=" Lastname">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCode" runat="server" Text='<%# Eval("Lastname") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=" Email Address">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCode" runat="server" Text='<%# Eval("EmailAddress") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Class">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClass" runat="server" Text='<%# stdClass(Convert.ToInt64(Eval("Id"))) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Grade">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGrade" runat="server" Text='<%# stdGrade(Convert.ToInt64(Eval("Id"))) %>'></asp:Label>
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

                <PagerSettings FirstPageText="&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                        LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                        Mode="NextPreviousFirstLast" 
                        NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" 
                        PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />                 
                                </asp:GridView>
                                               

                                            </div>
                                            </form>
                                                </div>
                                            </div>                                        
                                        </form>

                                 </div>
                                </div>    
                                </asp:Content>


