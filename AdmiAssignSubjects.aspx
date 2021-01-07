<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="AdmiAssignSubjects.aspx.cs" Inherits="AdmiAssignSubjects" %>


<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<%@ Import Namespace="PASSIS.DAO" %>
<%@ Import Namespace="PASSIS.LIB" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:FormView ID="fvwUser" runat="server" DataSourceID="objStaff" DataKeyNames="Id" data OnDataBound="fvwUser_DataBound"
        Width="100%">
        <ItemTemplate>
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
                                    <h4>ASSIGN SUBJECT(S)</h4>
                                </div>
                                <div class="widget-body">
                                    <form class="form-horizontal">

                                        <asp:Label ID="lblErrorMsg" runat="server" Text="" class="text-success ml-2"></asp:Label>
                                        <asp:Label ID="lblSubjectSelectionError" runat="server" Text="" class="text-danger ml-2"></asp:Label>
       

                                        <div class="form-group row mb-3">
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
                                        </div>
                                       <%-- <div class="form-group row mb-3">
                                            <div class="col-xl-6 mb-3">
                                                <label class="form-control-label">Campus:<span class="text-danger ml-2">*</span></label>
                                                <asp:DropDownList ID="ddlCampus" DataTextField="Name" DataValueField="Id" class="custom-select form-control" runat="server"></asp:DropDownList>
                                            </div>
                                        </div>--%>
                                        <div class="widget-body">
                                            <%--<div class="row">
                                                <div class="col-xl-6">
                                                    <asp:Button ID="Button2" class="btn btn-secondary" runat="server" Text="Assign Subject(s)" OnClick="btnAddSubject_OnClick" />
                                                </div>
                                            </div>--%>

                                            <br />
                                            <br />
                                            <%--<div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>CLASS ARMS</h4>
                                 </div>--%>
                         <asp:Label ID="lblunAssigned" runat="server" Visible="false" ForeColor="Blue" Text="List of UnAssigned Subjects" class="text-success ml-2"></asp:Label>

                                            <asp:GridView ID="gdvAllSubject" runat="server" ForeColor="Black" class="table table-striped" GridLines="None" AutoGenerateColumns="false" EmptyDataText=" No result."
                                                AllowPaging="false" Width="100%"
                                                OnRowCommand="gdvAllSubject_RowCommand" Visible="false">
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
                                                    <asp:TemplateField HeaderText="Check All">
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkAll" OnCheckedChanged="chkAll_CheckedChanged" AutoPostBack="true" runat="server" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CheckBox1" AutoPostBack="false" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnRemoveSubject" Visible="false" runat="server" class="btn btn-danger" CausesValidation="False" Text="Remove"
                                                                CommandName="Remove" CommandArgument='<%# Eval("Id") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>

                                            <div class="row">
                                                <div class="col-xl-6">
                                                    <asp:Button ID="Button2" class="btn btn-secondary" runat="server" Text="Assign Subject(s)" OnClick="btnAddSubject_OnClick" />
                                                </div>
                                            </div>
                                            <br /><br />
                                             <asp:Label runat="server" ID="Label9" Text="Campus : " class="text-success ml-2"> </asp:Label>
     <asp:Label runat="server" ID="Label5" Text='<%# getCampus() %>'> </asp:Label> 
                                           <br /><br />
                <asp:Label ID="Label1" runat="server" ForeColor="Blue" Text="List of Assigned Subjects" class="text-success ml-2"></asp:Label>
                                            <br />  <br /> 
                                            <asp:Panel ID="Panel2" Width="100%"  class="table table-striped" ScrollBars="Vertical" Height="300px" runat="server">
                                                <asp:GridView ID="gdvList" runat="server" AutoGenerateColumns="false" EmptyDataText=" No subject has been assigned to you"
                                                    AllowPaging="true" Width="100%"  PageSize="20" GridLines="None" ForeColor="Black" class="table table-striped" OnPageIndexChanging="gdvList_PageIndexChanging"
                                                    OnRowCommand="gdvList_RowCommand">
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
                                                    <Columns>
                                                        <asp:TemplateField HeaderText=" S/N">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText=" Subject Name ">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="lblCode" runat="server" ForeColor="#0066ff" Text='<%# Eval("Subject.Name")  %>'></asp:HyperLink>
                                                                <%--<asp:Label ID="lblCode" runat="server" Text='<%# Eval("Subject.Name") %>'></asp:Label>--%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText=" Year ">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblName" runat="server" Text='<%#  new ClassGradeLIB().RetrieveGrade(Convert.ToInt64(Eval("GradeId"))).Class_Grade.Name %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText=" Grade ">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblName3432423" runat="server" Text='<%# new ClassGradeDAL().RetrieveGrade(Convert.ToInt64(Eval("GradeId"))).GradeName %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGradeId" runat="server" Text='<%# Eval("GradeId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSubjectId" runat="server" Text='<%#  Eval("SubjectId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ShowHeader="False">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnRemoveRole" runat="server" class="btn btn-danger" CausesValidation="False" Text="Remove"
                                                                    CommandName="Remove" CommandArgument='<%# Eval("Id") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                      <PagerSettings FirstPageText="&lt;i class=&quot;la  la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;"
                                LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" Mode="NextPreviousFirstLast" 
                                NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />
                                                            </asp:GridView>
                                            </asp:Panel>

                                        </div>
                                </div>
                            </div>
                        </div>
                        </form>
                   
        </ItemTemplate>
    </asp:FormView>
     <asp:ObjectDataSource ID="objStaff" runat="server" DataObjectTypeName="PASSIS.DAO.User"
        SelectMethod="RetrieveUser" TypeName="PASSIS.DAO.UsersDAL">
        <SelectParameters>
            <asp:QueryStringParameter DefaultValue="0" Name="id" QueryStringField="Id" Type="Int64" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

