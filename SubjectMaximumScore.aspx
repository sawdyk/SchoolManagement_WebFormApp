<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="SubjectMaximumScore.aspx.cs" Inherits="SubjectMaximumScore" %>

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
                                    <h4>EDIT SUBJECT MAXIMUM SCORE</h4>
                                </div>
                                <div class="widget-body">
                                    <form class="form-horizontal">

                                        <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                        <asp:Label ID="lblSubjectSelectionError" runat="server" Text="" class="text-danger ml-2"></asp:Label>
       

                                        <div class="form-group row mb-3">
                                            <div class="col-xl-6 mb-3">
                                                <label class="form-control-label">Year:<span class="text-danger ml-2">*</span></label>
                                                 <asp:DropDownList runat="server" class="custom-select form-control" AutoPostBack="true" ID="ddlYear" AppendDataBoundItems="true"
                                        DataTextField="Name" DataValueField="Id" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                        <asp:ListItem Enabled="true" Selected="True" Text="--Select--" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="widget-body">
                                            <br />
                                            <%--<div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>CLASS ARMS</h4>
                                 </div>--%>
                                             <asp:Label runat="server" ID="lblSelected" Text="List of Subjects For : " Visible="false" ForeColor="Blue" class="text-success ml-2"> </asp:Label>
                                <asp:Label runat="server" ID="lblCampusSelected" Text="" ForeColor="Black" Visible="false"> </asp:Label> 
                                           <br /><br />
                         <asp:Panel ID="Panel2" Width="100%" class="table table-striped" ScrollBars="Vertical" runat="server">
                                           <asp:GridView ID="gdvLists" runat="server" GridLines="None" AutoGenerateColumns="false" EmptyDataText=" No subject has been assigned to you. "
                                AllowPaging="true" Width="100%" PageSize="50" ForeColor="#333333" class="table table-striped" OnRowEditing="gdvLists_RowEditing" OnRowCancelingEdit="gdvLists_RowCancelingEdit" OnRowUpdating="gdvLists_RowUpdating">
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
                                    <asp:TemplateField HeaderText=" Subject ">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lblSubject" runat="server" Text='<%# Eval("Name")  %>' ForeColor="#0066ff"></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=" Class ">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClass" runat="server" Text='<%# Eval("className")  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=" Maximum Mark ">
                                        <EditItemTemplate>
                                            <asp:TextBox runat="server" ID="txtScore" class="form-control" Text='<%# Eval("MaximumScore")  %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblScore" runat="server" Text='<%# Eval("MaximumScore")  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="CA">
                                         <EditItemTemplate>
                                            <asp:DropDownList ID="ddlCA" class="custom-select form-control" runat="server">
                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                            <ItemTemplate>
                                            <asp:Label ID="lblCA" runat="server" Text='<%# checkTorF(Convert.ToString(Eval("CA"))) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="EXAM">
                                         <EditItemTemplate>
                                            <asp:DropDownList ID="ddlExam" class="custom-select form-control" runat="server">
                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                           <ItemTemplate>
                                            <asp:Label ID="lblExam" runat="server" Text='<%# checkTorF(Convert.ToString(Eval("Exam")))  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" Text='<%# Eval("Id")  %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("Id")  %>' CausesValidation="True" CommandName="Update" Text="Update" ToolTip="Update"><i class="la la-floppy-o la-2x"></i></asp:LinkButton>
                                            &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CommandArgument='<%# Eval("Id")  %>' CausesValidation="False" CommandName="Cancel" Text="Cancel" ToolTip="Cancel"><i class="la la-remove la-2x"></i></asp:LinkButton>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" CommandArgument='<%# Eval("Id")  %>' runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" ToolTip="Edit"><i class="la la-edit la-2x"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            <PagerSettings FirstPageText="&lt;i class=&quot;la  la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                                    LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                                    Mode="NextPreviousFirstLast" NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" 
                                    PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />                            </asp:GridView>
                                            
                                            </asp:Panel>

                                        </div>
                                </div>
                            </div>
                        </div>
                        </form>

                    </div>
                </div>
        </ItemTemplate>
    </asp:FormView>
     
</asp:Content>


