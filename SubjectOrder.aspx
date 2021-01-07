<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Site.master" CodeFile="SubjectOrder.aspx.cs" Inherits="SubjectOrder" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<%@ Import Namespace="PASSIS.DAO" %>
<%@ Import Namespace="PASSIS.LIB" %>
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
                                    <h4>CREATE/SET ORDER OF SUBJECTS</h4>
                                </div>
                                <div class="widget-body">
                                    <form class="form-horizontal">

                                        <asp:Label ID="lblErrorMsg" runat="server" Text="" class="text-success ml-2"></asp:Label>
                                        <asp:Label ID="lblSubjectSelectionError" runat="server" Text="" class="text-danger ml-2"></asp:Label>
       

                                        <div class="form-group row mb-3">
                                            <div class="col-xl-6 mb-3">
                                                <label class="form-control-label">Year:<span class="text-danger ml-2">*</span></label>
                                                    <asp:DropDownList runat="server" class="custom-select form-control" ID="ddlYear" AppendDataBoundItems="true"
                                                                        DataTextField="Name" DataValueField="Id">
                                                                        <asp:ListItem Text="-- Select --" Value="0"></asp:ListItem>
                                                                    </asp:DropDownList>                                            
                                            </div>
                                        </div>
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSearch" class="btn btn-secondary"  OnClick="btnSearch_Click" Visible="true" runat="server" Text="View"  />
                                                </div>
                                            </div>

                                            <br />
                                            <%--<div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>CLASS ARMS</h4>
                                 </div>--%>
                                    <asp:Label ID="lblSubjects" Font-Size="Large" runat="server" Visible="false" ForeColor="Black" Text="LIST OF SUBJECTS"></asp:Label>             
         
                                        <asp:GridView ID="gvSubject" ForeColor="Black" GridLines="None" class="table table-striped" AutoGenerateColumns="false" runat="server">
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
                    <asp:TemplateField HeaderText="S/N">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Subject">
                        <ItemTemplate>
                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Position">
                        <ItemTemplate>
                            <asp:TextBox ID="txtPosition" class="form-control" onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

          <div class="row">
          <div class="col-xl-6">
            <asp:Button ID="btnSaveOrder" class="btn btn-secondary" Visible="false" OnClick="btnSaveOrder_Click" runat="server" Text="Save"  />
         </div>
         </div>
                  
                                        </div>
                                </div>
                              
                            </div>
                        </div>
                        </form>
            </div>
     
</asp:Content>



