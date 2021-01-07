<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="ReportCardComment.aspx.cs" Inherits="ReportCardComment" %>

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
                                        <h4>ADD COMMENT AND REMARK TO STUDENT REPORT CARD</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Year:<span class="text-danger ml-2">*</span></label>
                                                       <asp:DropDownList runat="server" AutoPostBack="true" class="custom-select form-control" ID="ddlYear" AppendDataBoundItems="true"
                                        DataTextField="Name" DataValueField="Id" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                        <asp:ListItem Enabled="true" Selected="True" Text="--Select--" Value="0"></asp:ListItem>
                                    </asp:DropDownList>                                          

                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Grade:<span class="text-danger ml-2">*</span></label>
                                 <asp:DropDownList runat="server" AutoPostBack="true" class="custom-select form-control" ID="ddlGrade" DataTextField="GradeName" DataValueField="Id" OnSelectedIndexChanged="ddlGrade_SelectedIndexChanged">
                                 </asp:DropDownList>

                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Session:<span class="text-danger ml-2">*</span></label>
                                      <asp:DropDownList runat="server" class="custom-select form-control" DataTextField="SessionName" DataValueField="ID" ID="ddlSession">
                                      </asp:DropDownList>
                                                   </div>

                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Term:<span class="text-danger ml-2">*</span></label>
                            <asp:DropDownList runat="server" ID="ddlTerm" class="custom-select form-control" DataTextField="AcademicTermName" DataValueField="Id">
                            </asp:DropDownList>
                                                  </div>
                                                  <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Comments For:<span class="text-danger ml-2">*</span></label>
                                 <asp:DropDownList runat="server" class="custom-select form-control" ID="ddlCommentFor" DataTextField="CommentBy" DataValueField="ID">
                                 </asp:DropDownList>
                                       
                                                   </div>
                                               </div>  
                                            

                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSearch" class="btn btn-secondary"  OnClick="btnSearch_Click" runat="server" Text="View"  />
                                                </div>
                                            </div>
                                                </form>    
                                        <br /><br />
                               
  <asp:Label ID="lblAcademic" Font-Size="Large" runat="server" Visible="false" ForeColor="Black" Text="LIST OF STUDENTS"></asp:Label>             
<asp:GridView ID="gvComment" ForeColor="Black" class="table table-striped" GridLines="None" AutoGenerateColumns="false" runat="server" OnRowDataBound="gvComment_RowDataBound">
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
                    <asp:TemplateField HeaderText="S/N">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fullname">
                        <ItemTemplate>
                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("User.StudentFullName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Admission No">
                        <ItemTemplate>
                            <asp:Label ID="lblAdmNo" runat="server" Text='<%# Eval("User.AdmissionNumber") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("User.Id") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Comment">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlComment" Width="100%" OnSelectedIndexChanged="ddlComment_SelectedIndexChanged" AutoPostBack="true"  AppendDataBoundItems="true" class="custom-select form-control" runat="server">
<%--                                               <asp:ListItem Text="--Select Comment--" Value="0"></asp:ListItem>--%>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Enter Comment">
                        <ItemTemplate>
                            <asp:TextBox ID="txtComment" Visible="True" class="form-control" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Remark">
                        <ItemTemplate>
                            <asp:TextBox ID="txtRemark" class="form-control" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
                                          
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSaveComment" class="btn btn-secondary"  Visible="false" OnClick="btnSaveComment_Click" runat="server" Text="Save"  />
                                                </div>
                                            </div>
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
                         
                                </asp:Content>

