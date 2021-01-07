<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="ExtendedSheet.aspx.cs" Inherits="ExtendedSheet" %>


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
                                        <h4>GENERATE EXTENDED SHEET</h4>
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
                                                    <label class="form-control-label">Class:<span class="text-danger ml-2">*</span></label>
                                                <asp:DropDownList runat="server" AutoPostBack="true" class="custom-select form-control" ID="ddlGrade" OnSelectedIndexChanged="ddlGrade_SelectedIndexChanged" DataTextField="GradeName" DataValueField="Id">
                                                </asp:DropDownList>                                                    

                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                       <label class="form-control-label">Select Template:<span class="text-danger ml-2">*</span></label>
                                              <asp:DropDownList runat = "server" class="custom-select form-control" ID = "ddlTemplate" OnSelectedIndexChanged="ddlTemplate_SelectedIndexChanged" ></asp:DropDownList>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Description:<span class="text-danger ml-2">*</span></label>
                                                 <asp:TextBox ID="txtDescription" class="form-control" runat="server"></asp:TextBox>
                                                  </div>
                                               </div>

                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                       <label class="form-control-label">All/Single student:<span class="text-danger ml-2">*</span></label>
                                              <asp:DropDownList runat = "server" class="custom-select form-control" AutoPostBack="true" ID = "ddlAllSingle" OnSelectedIndexChanged="ddlAllSingle_SelectedIndexChanged" ></asp:DropDownList>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <asp:label runat="server" ID="lblStudentName" Visible="false" class="form-control-label">Student Name:<span class="text-danger ml-2">*</span></asp:label>
                                              <asp:DropDownList runat = "server" class="custom-select form-control" Visible="false" ID = "ddlStudent" DataTextField="StudentFullName" DataValueField="Id"></asp:DropDownList>
                                                  </div>
                                               </div>
                                         
                                                
                                                </form>    
                                        <br />
                            <%--<asp:GridView ID="gvdAcademicSession" runat="server" AutoGenerateColumns="False" EmptyDataText=" No Calendar year currently exist" AllowPaging="true" Width="100%" ShowFooter="True" PageSize="20" GridLines="Both" OnRowCommand="gvdAcademicSession_RowCommand">       
                            <PagerSettings FirstPageText="&lt;i class=&quot;fa  fa-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-left&quot;&gt;&lt;/i&gt;"
                                LastPageText="&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;" Mode="NextPreviousFirstLast" 
                                NextPageText="&lt;i class=&quot;fa fa-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;fa fa-arrow-left&quot;&gt;&lt;/i&gt;" />--%>
                                       <asp:Label ID="lblsubjectResp" Font-Size="Large" ForeColor="Black" runat="server" Visible="false" Text="LIST OF SUBJECTS"></asp:Label>

                                        <asp:GridView ID="gdvAllSubject" class="table table-striped" runat="server" GridLines="None" AutoGenerateColumns="false" EmptyDataText=" No result."
                                    AllowPaging="false" Width="100%"  ForeColor="#333333"
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
                                        <asp:TemplateField HeaderText="ID" Visible="false" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>



                                    <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSearch" class="btn btn-secondary"  OnClick="btnSearch_Click" runat="server" Text="View BroadSheet"  />
                                                </div>
                                            </div>
                                        <br /> <br />
                                       <asp:Label ID="lblresltt" Font-Size="Large"  ForeColor="Black" runat="server" Visible="false" Text="EXTENDED SHEET GENERATED"></asp:Label>
   <asp:Panel runat="server" class="table table-striped" ID="pnlMarkAttendance" Visible="true" ScrollBars="Horizontal">
                                         <asp:MultiView ID="MultiView1" runat="server">
                            <asp:View ID="ViewTestAssignment" runat="server">                      
                                <asp:GridView ID="gvTestAssigment"  class="table table-striped" ForeColor="#333333"  AutoGenerateColumns="true" runat="server" >
                                   <Columns>
                                        <asp:TemplateField HeaderText="S/N">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       <%--<asp:TemplateField HeaderText="Fullname">
                                            <ItemTemplate>
                                                <asp:Label ID="Label5" runat="server" Text='<%# Eval("User.StudentFullName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                       <asp:TemplateField HeaderText="Admission No">
                                            <ItemTemplate>
                                                <asp:Label ID="Label5" runat="server" Text='<%# Eval("User.AdmissionNumber") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        
                                       
                                      
                                    </Columns>
                                </asp:GridView>                                
                            </asp:View>
                            <asp:View ID="ViewExam" runat="server">
                                <asp:GridView ID="gvExam" class="table table-striped"  ForeColor="#333333" AutoGenerateColumns="true" runat="server">
                                    <Columns>
                                     <asp:TemplateField HeaderText="S/N">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        </Columns>
                                    
                                </asp:GridView>                                
                            </asp:View>
                        </asp:MultiView>
                        </asp:Panel>

                                        <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnExportToexcel" class="btn btn-secondary"  OnClick="btnExportToexcel_Click" Visible="false" runat="server" Text="Save & Export"  />
                                                </div>
                                            </div>



                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
                         
                                </asp:Content>
