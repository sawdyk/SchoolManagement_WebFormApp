<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="TeacherViewStudents.aspx.cs" Inherits="TeacherViewStudents" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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
                                        <h4>VIEW STUDENTS ASSIGNED</h4>&nbsp;&nbsp;&nbsp;&nbsp;
<%--                                      <asp:Label runat="server" Text="(NB:Click on 'Get total' to get total amount to be paid before generating invoice!)" ForeColor="Red"></asp:Label>  --%>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" Visible="false" class="text-danger ml-2"></asp:Label>
                                             <asp:Label ID="lblSuccessMsg" runat="server" Text="" Visible="false" class="text-success ml-2"></asp:Label>

                                           
                                <asp:Label ID="lblLists" Font-Size="Large" ForeColor="Black" Text="LISTS OF ASSIGNED STUDENTS" runat="server" Visible="true"></asp:Label>
                                 <asp:Panel runat = "server" ID = "pnlusers" ScrollBars = "Horizontal">
                    <asp:GridView ID="gdvLists" class="table table-striped" ForeColor="Black" runat="server" GridLines="None" AutoGenerateColumns="false" EmptyDataText=" No student has been assigned to you. "
                        AllowPaging="true" Width = "100%" PageSize="50" OnPageIndexChanging="gdvLists_PageIndexChanging">
                                   <Columns>
                            <asp:TemplateField HeaderText=" S/N">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" Pulpil FullName ">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lblName" runat="server" Text='<%#  String.Format("{0} {1} {2}",Eval("User.LastName"),Eval("User.FirstName") ,Eval("User.MiddleName")   )  %>' ForeColor="#0066ff" NavigateUrl='<%# Eval("StudentId", "StudentDetailClassTeacher.aspx?mode=view&id={0}") %>'></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" Admission Number ">
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("User.AdmissionNumber")  %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                          <asp:TemplateField HeaderText=" Gender ">
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%# (PASSIS.DAO.Utility.Gender) Eval("User.Gender")  %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField HeaderText=" Username ">
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%#   Eval("User.Username")  %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
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

                <PagerSettings FirstPageText="&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                        LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                        Mode="NextPreviousFirstLast" 
                        NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" 
                        PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />                 
                                </asp:GridView>
                                       </asp:Panel>        

                                            </div>
                                            </form>
                                                </div>
                                            </div>                                        
                                        </form>

                                 </div>
                                </div>    
                                </asp:Content>



