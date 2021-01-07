<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="TestInvitationStatus.aspx.cs" Inherits="TestInvitationStatus" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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
                        <div class="row flex-row">
                            <div class="col-12">
                                <!-- Form -->
                                <div class="widget has-shadow">
                                    <div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>TEST INVITATION STATUS</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">
                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" class="text-danger ml-2"></asp:Label>
                                          <asp:Label ID="lblResultMsg" runat="server" Text="" class="text-success ml-2"></asp:Label>

                                           
                <asp:GridView ID="gdvList" ForeColor="Black" class="table table-striped" runat="server" AutoGenerateColumns="false" GridLines="None" EmptyDataText=" No record Currently exist "
                            AllowPaging="false" Width="100%" PageSize="30">
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
                                <asp:TemplateField HeaderText="Application Number">
                                    <ItemTemplate>
                                        <asp:Label ID="lblApplicantId" runat="server" Text='<%# Eval("ApplicantId")  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fullname">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFullname" runat="server" Text='<%# Eval("Fullname")  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>  
                                <asp:TemplateField HeaderText=" Status ">
                                            <HeaderStyle Width="50px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblTestStatus" runat="server" Text='<%#  getLNStatus(Convert.ToInt64( Eval("TestInvitationStatus"))) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>                                                                                     
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblId" runat="server" Text='<%# Eval("ID")  %>'></asp:Label>
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
                                </asp:Content>

