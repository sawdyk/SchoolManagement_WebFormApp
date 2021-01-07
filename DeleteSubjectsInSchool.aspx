<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="DeleteSubjectsInSchool.aspx.cs" Inherits="DeleteSubjectsInSchool" %>


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
                                    <h4>DELETE SUBJECT(S)</h4>
                                </div>
                                <div class="widget-body">
                                    <form class="form-horizontal">

                                        <asp:Label ID="lblErrorMsg" runat="server" Text="" class="text-danger ml-2"></asp:Label>
                                        <asp:Label ID="lblSubjectSelectionError" runat="server" Text="" class="text-danger ml-2"></asp:Label>
       

                                       <%-- <div class="form-group row mb-3">
                                            <div class="col-xl-6 mb-3">
                                                <label class="form-control-label">Year:<span class="text-danger ml-2">*</span></label>
                                                    <asp:DropDownList runat="server" class="custom-select form-control" AutoPostBack="true" ID="ddlYear" AppendDataBoundItems="true"
                                                                        DataTextField="Name" DataValueField="Id"
                                                                        OnSelectedIndexChanged="ddlYear_OnSelectedIndexChanged">
                                                                        <asp:ListItem Text="-- Select --" Value="0"></asp:ListItem>
                                                                    </asp:DropDownList>                                            
                                            </div>
                                            <div class="col-xl-6">
                                                <label class="form-control-label">Grade:<span class="text-danger ml-2">*</span></label>
                                                <asp:DropDownList runat="server" class="custom-select form-control" ID="ddlGrade" DataTextField="GradeName" DataValueField="Id">
                                                                        <asp:ListItem Text="-- Select --" Value="0"></asp:ListItem>
                                                                    </asp:DropDownList>                                            
                                            </div>
                                        </div>--%>
                                    

                                            <br />
                                           
                         <asp:Label ID="lblunAssigned" runat="server" Visible="true" ForeColor="Black" Text="LIST OF SUBJECT"></asp:Label>

                                            <asp:GridView ID="gdvAllSubject" runat="server" ForeColor="Black" PageSize="20" class="table table-striped" GridLines="None" AutoGenerateColumns="false" EmptyDataText=" No result."
                                                AllowPaging="true" Width="100%" Visible="true" OnPageIndexChanging="gdvAllSubject_PageIndexChanging">
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
                                                            <asp:Label ID="lblName32434" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText=" Class ">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblClass" runat="server" Text='<%# Eval("Class") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Check All">
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkAll" OnCheckedChanged="chkAll_CheckedChanged" AutoPostBack="true" runat="server" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CheckBox1" AutoPostBack="false" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("SubjectId") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                             <PagerSettings FirstPageText="&lt;i class=&quot;la  la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;"
                                LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" Mode="NextPreviousFirstLast" 
                                NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />
                                                            </asp:GridView>


                                             <div class="widget-body">
                                            <div class="row">
                                                <div class="col-xl-6">
                                                    <asp:Button ID="btnDelete" class="btn btn-secondary" runat="server" Text="Delete Subject(s)" OnClick="btnDeleteSubject_OnClick" />
                                                </div>
                                            </div>

                                        </div>
                                </div>
                            </div>
                        </div> 
                         </div>
                     </div>
                        </form>
                   
      
     <asp:ObjectDataSource ID="objStaff" runat="server" DataObjectTypeName="PASSIS.DAO.User"
        SelectMethod="RetrieveUser" TypeName="PASSIS.DAO.UsersDAL">
        <SelectParameters>
            <asp:QueryStringParameter DefaultValue="0" Name="id" QueryStringField="Id" Type="Int64" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

