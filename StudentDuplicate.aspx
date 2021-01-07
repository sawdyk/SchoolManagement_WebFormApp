<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="StudentDuplicate.aspx.cs" Inherits="StudentDuplicate" %>


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
                                        <h4>MANAGE STUDENTS DUPLICATE</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblResult" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Campus:<span class="text-danger ml-2">*</span></label>
                                <asp:DropDownList runat="server" ID="ddlCampus" class="custom-select form-control" OnSelectedIndexChanged="ddlCampus_SelectedIndexChanged" DataTextField="Name" DataValueField="Id" AutoPostBack="true">
                                    </asp:DropDownList>                                                

                                                </div>
                                                <div class="col-xl-6">
                                     <label class="form-control-label">New Students:<span class="text-danger ml-2">*</span></label>
                                        <asp:DropDownList runat="server" ID="ddlStudent" class="custom-select form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlStudent_SelectedIndexChanged" DataTextField="NewStudent" DataValueField="NewStudent">
                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                       <label class="form-control-label">Year:<span class="text-danger ml-2">*</span></label>
                                            <asp:DropDownList runat="server" ID="ddlYear" class="custom-select form-control" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                                    </asp:DropDownList>                                                

                                                   </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Grade:<span class="text-danger ml-2">*</span></label>
                                        <asp:DropDownList runat="server" class="custom-select form-control" ID="ddlGrade" DataTextField="GradeName" DataValueField="Id" AppendDataBoundItems="true" AutoPostBack="true">
                                        </asp:DropDownList>                                                  

                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Session:<span class="text-danger ml-2">*</span></label>
                                            <asp:DropDownList runat="server" class="custom-select form-control" ID="ddlSession" DataTextField="SessionName" DataValueField="Id" AppendDataBoundItems="true" AutoPostBack="true">
                                            </asp:DropDownList>                                                

                                                </div>
                                            </div>
                                         
                                                </form>    
                                        <br />
                                <asp:Label ID="lblStudents" ForeColor="Black" runat="server" Visible="false" Text="EXISTING STUDENTS"></asp:Label>

                            <asp:GridView ID="GridView1" EmptyDataText="No record on the list" Width="100%" runat="server" AutoGenerateColumns="false" class="table table-striped" GridLines="None"
                                AllowPaging="true" ForeColor="Black" AutoGenerateDeleteButton="false" AutoGenerateEditButton="false">
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
                                <%--<asp:BoundField DataField="New Name" HeaderText="New Name" />--%>
                                <asp:BoundField DataField="User.LastName" HeaderText="LastName" />
                                <asp:BoundField DataField="User.FirstName" HeaderText="FirstName" />
                                <asp:BoundField DataField="User.Username" HeaderText="UserName" />
                                <asp:BoundField DataField="User.AdmissionNumber" HeaderText="Admission Number" />
                                <asp:BoundField DataField="User.StudentStatus" HeaderText="Status" />
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblId" AutoPostBack="false" runat="server" Text='<%# Eval("User.Id") %>'/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Check One">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" AutoPostBack="false" runat="server"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                               <%-- <PagerSettings FirstPageText="&lt;i class=&quot;la  la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                                    LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                                    Mode="NextPreviousFirstLast" NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" 
                                    PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />                   --%>                                 
                               </asp:GridView>  

                                         <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="Button2" class="btn btn-secondary" runat="server" Text="Update"  OnClick="btnUpdate_Click" />
                                                </div>
                                            </div>
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
                         
                                </asp:Content>


