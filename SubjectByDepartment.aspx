<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="SubjectByDepartment.aspx.cs" Inherits="SubjectByDepartment" %>

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
                                        <h4>CREATE SUBJECTS BY DEPARTMENT</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label runat="server" class="form-control-label">New Department:<span class="text-danger ml-2">*</span></label>
                                               <asp:TextBox runat="server" placeholder="Department Name" class="form-control" ID="txtNewDepartment"></asp:TextBox>                                 
                                                </div>
                                              
                                                <div class="col-xl-6">
                                                      <br />
                                                    <asp:Button ID="AddDepartment" class="btn btn-secondary"  OnClick="AddDepartment_Click" runat="server" Text="Create Department"  />
                                                </div>  
                                            </div>

                                              <div class="form-group row mb-3">
                                                  <div class="col-xl-6 mb-3">
                                                    <label runat="server" class="form-control-label">Class:<span class="text-danger ml-2">*</span></label>
                                                     <asp:DropDownList runat="server" ID="ddlYear" class="custom-select form-control" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-xl-6">
                                                    <label runat="server" class="form-control-label">Department:<span class="text-danger ml-2">*</span></label>
                                                     <asp:DropDownList runat="server" class="custom-select form-control" ID="ddlDepartment" AppendDataBoundItems="true" DataValueField="Id" DataTextField="DepartmentName" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                </div>  
                                               </div>

                                           
                                         
                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnAddDepartment" class="btn btn-secondary"  OnClick="btnAddDepartment_Click" runat="server" Text="Add Department"  />
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
                                
                     <asp:Label ID="lblSubjects" Font-Size="Large" runat="server" Visible="false" ForeColor="Black" Text="LIST OF SUBJECTS"></asp:Label>             
          
                                  <asp:Panel ID="Panel2" Width="1000px" class="table table-striped" ScrollBars="Vertical" runat="server">
                                    <asp:GridView ID="gdvAllSubject" runat="server" ForeColor="Black" GridLines="None" class="table table-striped" AutoGenerateColumns="false" EmptyDataText=" No Result."
                                    AllowPaging="false" Width="100%" Visible="false">
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
                                                <asp:Label ID="lblName32434" runat="server" Text='<%# subjectName((long)Eval("SubjectId")) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText=" Department ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDept" runat="server" Text='<%# deptName((long)ProcessMyDataItem(Eval("DepartmentId"))) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
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
                                  <%--  <PagerSettings FirstPageText="&lt;i class=&quot;la  la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                                        LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                                        Mode="NextPreviousFirstLast" NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />--%>
                                </asp:GridView>
                                        </asp:Panel>
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
                         
                                </asp:Content>


