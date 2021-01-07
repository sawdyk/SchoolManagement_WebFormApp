<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="MoveStudentsManually.aspx.cs" Inherits="MoveStudentsManually" %>

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
                                        <h4>MOVE STUDENTS TO NEW CLASS/GRADE(ARMS)/ALUMNI</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" ></asp:Label>
                                           <br />
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Old Class:<span class="text-danger ml-2">*</span></label>
                                                    <asp:DropDownList ID="ddlOldYear" DataTextField="Name" DataValueField="Id" class="custom-select form-control" AutoPostBack="true" AppendDataBoundItems="true"
                                                        OnSelectedIndexChanged="ddlOldYear_SelectedIndexChanged" runat="server">
                                                       <asp:ListItem Enabled="true" Selected="True" Text="--  Select --" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>

                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">New Class:<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList ID="ddlNewYear" DataTextField="Name" DataValueField="Id" class="custom-select form-control" AutoPostBack="true" AppendDataBoundItems="true"
                                                        OnSelectedIndexChanged="ddlNewYear_SelectedIndexChanged" runat="server">
                                                       <asp:ListItem Enabled="true" Selected="True" Text="--  Select --" Value="0"></asp:ListItem>
                                                   </asp:DropDownList>

                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Old Grade:<span class="text-danger ml-2">*</span></label>
                                               <asp:DropDownList ID="ddlOldGrade" DataTextField="GradeName" DataValueField="Id" runat="server" class="custom-select form-control"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlOldGrade_SelectedIndexChanged">
                                                <asp:ListItem Enabled="true" Selected="True" Text="--  Select --" Value="0"></asp:ListItem>

                                               </asp:DropDownList>

                                                </div>
                                               <div class="col-xl-6">
                                                 <label class="form-control-label">New Grade:<span class="text-danger ml-2">*</span></label>
                                            <asp:DropDownList ID="ddlNewGrade" DataTextField="GradeName" DataValueField="Id" runat="server" class="custom-select form-control"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlNewGrade_SelectedIndexChanged">
                                             <asp:ListItem Enabled="true" Selected="True" Text="--  Select Grade--" Value="0"></asp:ListItem>
                                            </asp:DropDownList>

                                                     </div>
                                                      </div>
                                             <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Old Session:<span class="text-danger ml-2">*</span></label>
                                               <asp:DropDownList ID="ddlOldSession" DataTextField="SessionName" DataValueField="ID" runat="server" class="custom-select form-control"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlOldSession_SelectedIndexChanged">
                                                <asp:ListItem Enabled="true" Selected="True" Text="--  Select --" Value="0"></asp:ListItem>

                                               </asp:DropDownList>

                                                </div>
                                               <div class="col-xl-6">
                                                 <label class="form-control-label">New Session:<span class="text-danger ml-2">*</span></label>
                                            <asp:DropDownList ID="ddlNewSession" DataTextField="SessionName" DataValueField="ID" runat="server" class="custom-select form-control"
                                                AutoPostBack="true">
                                             <asp:ListItem Enabled="true" Selected="True" Text="--  Select --" Value="0"></asp:ListItem>
                                            </asp:DropDownList>

                                                     </div>
                                                      </div>
                                                <div class="widget-body">
                                                <%--<div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="Button2" class="btn btn-secondary" runat="server" Text="Save Academic Session"  />
                                                </div>--%>
                                            </div>
                                                </form>    
                                       <br />
                                 <%--<div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>CLASS ARMS</h4>
                                 </div>--%>
                                          <asp:Label ID="lblStudents" Font-Size="Large" ForeColor="Black" runat="server" Visible="false" Text="LIST OF STUDENTS"></asp:Label>
                                                    <br />
                               <asp:GridView ID="gdvAllStudent" runat="server" GridLines="None" ForeColor="Black" class="table table-striped" AutoGenerateColumns="false" EmptyDataText=" No result."
                                    AllowPaging="false" Width="100%" Visible="true">
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
                                        <asp:TemplateField HeaderText=" Name ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblName32434" runat="server" Text='<%# Eval("StudentFullName") %>'></asp:Label>
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
                                         <br />
                        <div class="col-xl-6">
                            <asp:Button ID="Button1" class="btn btn-secondary" runat="server" Text="Move Selected Student"  OnClick="btnAddToGrid_Click" />
                         </div>
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>                        
                                </asp:Content>
