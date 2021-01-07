<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="AdminAddSubjects.aspx.cs" Inherits="AdminAddSubjects" %>

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
                                        <h4>ADD NEW SUBJECTS</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblReport" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Curriculum:<span class="text-danger ml-2">*</span></label>
                                                    <asp:DropDownList ID="ddlCurriculum" DataTextField="CurriculumName" DataValueField="Id" class="custom-select form-control" runat="server" 
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddlCurriculum_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-xl-6">
                                                   <label runat="server" class="form-control-label">SchoolType:<span class="text-danger ml-2">*</span></label>
                                             <asp:DropDownList runat="server" class="custom-select form-control" ID="ddlSchoolType" DataTextField="SchoolTypeName" DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="ddlSchoolType_SelectedIndexChanged"></asp:DropDownList>

                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                  
                                                       <label class="form-control-label">Grade:<span class="text-danger ml-2">*</span></label>
                                                    <asp:DropDownList ID="ddlClassGrade" DataTextField="Name" DataValueField="Id" class="custom-select form-control" runat="server"></asp:DropDownList>
                                                </div>
                                                  <div class="col-xl-6">
                                                        <label class="form-control-label">Subject:<span class="text-danger ml-2">*</span></label>
                                                    <asp:TextBox runat="server" class="form-control" ID="txtSubject"></asp:TextBox>       

                                                  </div>
                                               </div>

                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                       <label class="form-control-label">Subject Code:<span class="text-danger ml-2">*</span></label>
                                                <asp:TextBox runat="server"  class="form-control" ID="txtSubCode"></asp:TextBox>      
                                                </div>
                                                <div class="col-xl-6">
                                                    <asp:Label ID="Label4" class="form-control-label" runat="server" Text="Range" Visible="false">Range:<span class="text-danger ml-2">*</span></asp:Label>
                                                <asp:DropDownList runat="server" class="custom-select form-control" AutoPostBack="true" ID="ddlRange" AppendDataBoundItems="true" Visible="false">
                                                </asp:DropDownList> 
                                                    </div>
                                                    </div>
                                         
                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSave" class="btn btn-secondary"  OnClick="btnSave_Click" runat="server" Text="Save"  />
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
                  <asp:Label ID="lblAcademic" Font-Size="Large" runat="server" Visible="true" ForeColor="Black" Text="LIST OF SUBJECTS"></asp:Label>             

                                        <asp:Panel ID="Panel2" Width="1000px"   class="table table-striped" ScrollBars="Vertical" runat="server">
                                     <asp:GridView ID="gvSubjects" Width="1000px" runat="server" ForeColor="#333333" PageSize="20"  EmptyDataText="No Subjects Available" GridLines="None"  CellPadding="2" AllowPaging="True" AllowSorting="True" class="table table-striped" OnPageIndexChanging="gvSubjects_PageIndexChanging" AutoGenerateColumns="False" CssClass="auto-style4">
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
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="Label5" runat="server" Text='<%# Container.DataItemIndex + 1 %>'>'></asp:Label>                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="SubjectStatus" HeaderText="SubjectStatus" SortExpression="SubjectStatus" />--%>
                                        <asp:TemplateField HeaderText="Subject Id" InsertVisible="False" SortExpression="Id">
                                            <EditItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Subject" SortExpression="Name">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Name") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Code" SortExpression="Code">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Code") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("Code") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Class/Grade">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Class_Grade.Name") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("Class_Grade.Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Curriculum" SortExpression="CurriculumId">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("CurriculumId") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("Curriculum.CurriculumName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                    </Columns>
                                    <PagerSettings FirstPageText="&lt;i class=&quot;la  la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                                        LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                                        Mode="NextPreviousFirstLast" NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />
                                </asp:GridView>
                                        </asp:Panel>
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
                         
                                </asp:Content>

