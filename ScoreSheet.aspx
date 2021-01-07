<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master"  CodeFile="ScoreSheet.aspx.cs" Inherits="BroadSheet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
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
                                        <h4>GENERATE/DOWNLOAD SCORESHEET</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" class="text-success ml-2"></asp:Label>
                                           
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Year:<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList ID="ddlYear" OnSelectedIndexChanged="ddlYear_OnSelectedIndexChanged" AutoPostBack="true"  
                                                       DataTextField="Name" DataValueField="Id" class="custom-select form-control" runat="server">
                                                   </asp:DropDownList>
                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Grade(Arms):<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList ID="ddlGrade" DataTextField="GradeName" DataValueField="Id" class="custom-select form-control" runat="server"></asp:DropDownList>
                                                </div>
                                            </div>
                                              
                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="Button2" class="btn btn-secondary" runat="server" Text="Display"  OnClick="btnSearch_Click" />
                                                </div>
                                            </div>
                                                    
                                        <br /><br />
                                 <%--<div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>CLASS ARMS</h4>
                                 </div>--%>
<asp:Label ID="lblAcademic" Font-Size="Large" runat="server" Visible="false" ForeColor="Black" Text="LIST OF STUDENTS"></asp:Label>             
                                       <asp:GridView ID="GridView1" ForeColor="Black" class="table table-striped" AutoGenerateColumns="false" runat="server">
                    <Columns>
                        <asp:TemplateField HeaderText="S/N">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fullname">
                            <ItemTemplate>
                                <asp:Label ID="Label5" runat="server" Text='<%# Eval("User.StudentFullName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Admission No">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("User.AdmissionNumber") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Exam">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>                 
                                             <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnExportToexcel" class="btn btn-secondary" Visible="false" runat="server" Text="Download ScoreSheet"  OnClick="btnExportToexcel_Click" />
                                                </div>
                                            </div>
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        
                                        </form>

                                 </div>
                                </div>
                        
                                </asp:Content>

