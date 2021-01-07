<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Site.master" CodeFile="ViewScoresExtended.aspx.cs" Inherits="ViewScoresExtended" %>


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
                                        <h4>VIEW UPLOADED SCORES</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                       <label class="form-control-label">Term:<span class="text-danger ml-2">*</span></label>
                                            <asp:DropDownList runat="server" ID="ddlTerm" class="custom-select form-control"  DataTextField="AcademicTermName" DataValueField="Id"></asp:DropDownList>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Session:<span class="text-danger ml-2">*</span></label>
                                            <asp:DropDownList runat="server" DataTextField="SessionName" class="custom-select form-control" DataValueField="ID" ID="ddlSession"></asp:DropDownList>
                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Class/Year:<span class="text-danger ml-2">*</span></label>
                                   <asp:DropDownList runat="server" AutoPostBack="true" class="custom-select form-control" ID="ddlYear" AppendDataBoundItems="true"
                                                DataTextField="Name" DataValueField="Id" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                                <asp:ListItem Enabled="true" Selected="True" Text="--Select--" Value="0"></asp:ListItem>
                                            </asp:DropDownList>                                         
                                                </div>
                                                <div class="col-xl-6">
                                     <label class="form-control-label">Grade:<span class="text-danger ml-2">*</span></label>
                                   <asp:DropDownList runat="server" AutoPostBack="true" class="custom-select form-control" ID="ddlGrade" OnSelectedIndexChanged="ddlGrade_SelectedIndexChanged" DataTextField="GradeName" DataValueField="Id">
                                            </asp:DropDownList>                                             

                                                </div>
                                            </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Category:<span class="text-danger ml-2">*</span></label>
                        <asp:DropDownList runat="server" class="custom-select form-control" DataTextField="Category" DataValueField="ID" ID="ddlCategory">
                                            </asp:DropDownList>                                         
                                                </div>
                                                </div>
                                                </form>    
                 <asp:Label ID="lblStudents" Font-Size="Large" runat="server" Visible="false" ForeColor="Black" Text="LIST OF SUBJECTS"></asp:Label>                    

                 <asp:GridView ID="gdvAllSubject" runat="server" class="table table-striped"  ForeColor="#333333"  GridLines="None" AutoGenerateColumns="false" EmptyDataText=" No result."
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
                            <asp:TemplateField HeaderText="ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                                        <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnViewExtendedScores" class="btn btn-secondary"  OnClick="btnViewExtendedScores_Click" runat="server" Text="View Scores"  />
                                                </div>
                                            </div>
                                        <br />
              <asp:Label ID="Label1" Font-Size="Large" runat="server" Visible="false" ForeColor="Black" Text="STUDENT SCORES"></asp:Label>                    
                                        <asp:Panel ID="Panel2" Width="100%" ScrollBars="Horizontal" runat="server">
          <asp:GridView ID="gdvViewExtendedScores" runat="server" class="table table-striped"  ForeColor="#333333"  AutoGenerateColumns="true" EmptyDataText=" No scores available"
                            Width="100%" BorderStyle="None" GridLines="None" OnRowUpdating="gdvViewExtendedScores_RowUpdating" OnRowCancelingEdit="gdvViewExtendedScores_RowCancelingEdit" OnRowEditing="gdvViewExtendedScores_RowEditing">
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
                                <%--<asp:TemplateField HeaderText="Fullname">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# new SchoolLIB().GetUserDetails(Eval("AdmissionNo").ToString()).StudentFullName  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <%--<asp:TemplateField HeaderText=" Admission No">--%>
                                    <%-- <EditItemTemplate>
                                           <asp:TextBox ID="txtAdmissionNo" Text='<%# Eval("AdmissionNo")  %>' runat="server"></asp:TextBox>

                                            </EditItemTemplate>--%>
                                   <%-- <ItemTemplate>
                                        <asp:Label ID="lblCod3e" runat="server" Text='<%# new SchoolLIB().GetUserDetails(Eval("AdmissionNo").ToString()).AdmissionNumber  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <%--                                        <asp:TemplateField HeaderText="Template Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCode" runat="server" Text='<%# Eval("TemplateCode")  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                <%--<asp:TemplateField HeaderText="Exam Score">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtExamScore" Text='<%# Eval("ExamScore")  %>' runat="server"></asp:TextBox>

                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCode331" runat="server" Text='<%# Eval("ExamScore")  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                <%--<asp:TemplateField HeaderText="ID" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblId" Text='<%# Eval("Id")  %>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <%--<asp:TemplateField ShowHeader="False">
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Update" ToolTip="Update"><i class="fa fa-floppy-o fa-2x"></i></asp:LinkButton>
                                        &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" ToolTip="Cancel"><i class="fa fa-remove fa-2x"></i></asp:LinkButton>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" ToolTip="Edit"><i class="fa fa-edit fa-2x"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                            </Columns>
                            </asp:GridView>
                        </asp:Panel>
                                        <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnExportToexcel" class="btn btn-secondary"  OnClick="btnExportToexcel_Click" Visible="false" runat="server" />
                                                </div>
                                            </div>

                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
                         
                                </asp:Content>


