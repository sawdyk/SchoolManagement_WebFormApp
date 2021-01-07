<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master"  CodeFile="ComputeResult2.aspx.cs" Inherits="ComputeResult2" %>

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
                                        <h4>COMPUTE RESULT</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">
                                 <asp:Label ID="lblErrorMsg" runat="server" Text="" class="text-danger ml-2"></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Year:<span class="text-danger ml-2">*</span></label>
                                            <asp:DropDownList runat="server" AutoPostBack="True" ID="ddlYear" AppendDataBoundItems="True"
                                                DataTextField="Name" DataValueField="Id" class="custom-select form-control" OnSelectedIndexChanged="ddlYear_OnSelectedIndexChanged">
                                                <asp:ListItem Enabled="true" Selected="True" Text="--  Select All --" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Grade:<span class="text-danger ml-2">*</span></label>
                                            <asp:DropDownList runat="server" ID="ddlGrade" AutoPostBack="true" class="custom-select form-control" OnSelectedIndexChanged="ddlGrade_SelectedIndexChanged" DataTextField="GradeName" DataValueField="Id"
                                                AppendDataBoundItems="true">
                                            </asp:DropDownList>
                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Session:<span class="text-danger ml-2">*</span></label>
                                                    <asp:DropDownList runat="server" AutoPostBack="false" class="custom-select form-control" ID="ddlAcademicSession" AppendDataBoundItems="true"
                                                DataTextField="SessionName" DataValueField="Id">
                                                <asp:ListItem Enabled="true" Selected="True" Text="--  Select --" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                                </div>
                                               <div class="col-xl-6">
                                                 <label class="form-control-label">Term:<span class="text-danger ml-2">*</span></label>
                                                      <asp:DropDownList runat="server" class="custom-select form-control" ID="ddlAcademicTerm" DataTextField="AcademicTermName" DataValueField="Id" AppendDataBoundItems="true">
                                                <asp:ListItem Enabled="true" Selected="True" Text="--  Select --" Value="0"></asp:ListItem>
                                            </asp:DropDownList>     
                                          
                                             </form>
                                            </div>
                                            </div>
                                        
                    <asp:Label ID="lblMsgDisplay" Font-Size="Large" runat="server" Text="LIST OF SUBJECTS" Visible="false" ForeColor="Black" ></asp:Label>
                                <asp:GridView ID="gdvAllSubject" runat="server" ForeColor="Black" GridLines="None" class="table table-striped" AutoGenerateColumns="false" EmptyDataText=" No result."
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
                                                <asp:Label ID="lblName32434" runat="server" Text='<%# subjectName(Convert.ToInt32(Eval("SubjectId"))) %>'></asp:Label>
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
                                </asp:GridView>
                                <br />
                                   <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                  <asp:Button ID="btnCompute" class="btn btn-secondary" runat="server" Text="Compute"  OnClick="btnCompute_Click" />
                                     
                                                </div>
                                            </div>
<%-- <div id="containerDiv" runat="server" class="f"></div>--%>

                                            </div>
                                         </div>   
                        
</asp:Content>
