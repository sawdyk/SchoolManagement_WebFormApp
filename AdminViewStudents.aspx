<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="AdminViewStudents.aspx.cs" Inherits="AdminViewStudents" %>

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
                                        <h4>VIEW STUDENT'S INFORMATION/</h4> 
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" class="text-success ml-2"></asp:Label>
                                           
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Year:<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList ID="ddlYear" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" AutoPostBack="true"  DataTextField="Name" DataValueField="Id" class="custom-select form-control" runat="server">
<%--                                                    <asp:ListItem Enabled="true" Selected="True" Text="--  Select --" Value="0"></asp:ListItem>--%>
                                                   </asp:DropDownList>
                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Grade(Arms):<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList ID="ddlGrade" DataTextField="GradeName" DataValueField="Id" class="custom-select form-control" runat="server"></asp:DropDownList>
                                                </div>
                                            </div>
                                               <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                 <label class="form-control-label">Campus:<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList ID="ddlCampus" DataTextField="Name" DataValueField="Id" class="custom-select form-control" runat="server"></asp:DropDownList>
                                                   </div>
                                                    <div class="col-xl-6">
                                                    <label class="form-control-label">Student Name:<span class="text-danger ml-2">*</span></label>
                                                    <asp:TextBox ID="txtName" runat="server" class="form-control"></asp:TextBox>
                                                </div>
                                                   </div>

                                             <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                 <label class="form-control-label">Father's Name:<span class="text-danger ml-2">*</span></label>
                                                    <asp:TextBox ID="txtFathersName" runat="server" class="form-control"></asp:TextBox>
                                                   </div>
                                                    <div class="col-xl-6">
                                                    <label class="form-control-label">Admission Number:<span class="text-danger ml-2">*</span></label>
                                                    <asp:TextBox ID="txtAdmNumber" runat="server" class="form-control"></asp:TextBox>
                                                </div>
                                                   </div>

                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="Button2" class="btn btn-secondary" runat="server" Text="View Student(s)"  OnClick="btnSearch_OnClick" />
                                                </div>
                                            </div>
                                                    <br />
                                   <asp:Label runat="server" ID="lblResultSchoolCampus" Font-Size="Large" ForeColor="black"> </asp:Label>
                                                    <br />
                                  <asp:Label runat="server" ID="lblTotalInSchool" Font-Size="Large" ForeColor="black"> </asp:Label>
                                                    <br />
                                   <asp:Label runat="server" ID="lblTotalNumberofStudents" Font-Size="Large" ForeColor="black">> </asp:Label>

                                  
                                    <asp:GridView ID="gdvList" runat="server" class="table table-striped" AutoGenerateColumns="false" GridLines="None" EmptyDataText=" No record Currently exist "
                    AllowPaging="true" Width="100%" ForeColor="Black" OnPageIndexChanging="gdvList_PageIndexChanging" PageSize="10">
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
                        <asp:TemplateField HeaderText=" Student FullName ">
                            <ItemTemplate>
                                <asp:HyperLink ID="lblName" ForeColor="#0066ff" runat="server" Text='<%# Eval("StudentFullName")  %>' NavigateUrl='<%# Eval("Id", "StudentDetail.aspx?mode=view&id={0}") %>'></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Admission Number">
                            <ItemTemplate>
                                <asp:Label ID="lblAdmNo" runat="server" Text='<%# Eval("AdmissionNumber")  %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" Gender ">
                            <ItemTemplate>
                                <asp:Label ID="lblGender" runat="server" Text='<%# (PASSIS.DAO.Utility.Gender) Eval("Gender")  %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id")  %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
 <PagerSettings FirstPageText="&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                        LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                        Mode="NextPreviousFirstLast" 
                        NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" 
                        PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />                

                                    </asp:GridView>

                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        
                                        </form>

                                 </div>
                                </div>
                        
                                </asp:Content>

