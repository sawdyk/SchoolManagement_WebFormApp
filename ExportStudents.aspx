<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="ExportStudents.aspx.cs" Inherits="ExportStudents" %>

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
                                        <h4>EXPORT STUDENTS DATA</h4> 
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" class="text-success ml-2"></asp:Label>
                                           
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Year:<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList ID="ddlYear" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" AutoPostBack="true"  DataTextField="Name" DataValueField="Id" class="custom-select form-control" runat="server"></asp:DropDownList>
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
                                                   </div>
                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="Button2" class="btn btn-secondary" runat="server" Text="View Students"  OnClick="btnSearch_OnClick" />
                                                </div>
                                            </div>
                                                    <br />
                                    <h4>SELECTED CAMPUS:&nbsp;&nbsp;&nbsp;&nbsp; <asp:Label runat="server" ID="lblResultSchoolCampus" ForeColor="Green"> </asp:Label> </h4>
                                  <h4>TOTAL NUMBER OF STUDENTS: <asp:Label runat="server" ID="lblTotalNumberofStudents" ForeColor="Green">> </asp:Label></h4>
                                  
                                   <asp:GridView ID="gdvList" class="table table-striped" runat="server" AutoGenerateColumns="false" GridLines="None" EmptyDataText=" No record Currently exist "
                    AllowPaging="true" Width="100%" OnPageIndexChanging="gdvList_PageIndexChanging" ForeColor="Black" PageSize="30">
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
                        <asp:TemplateField HeaderText=" Pupil FullName ">
                            <ItemTemplate>
                                <asp:HyperLink ID="lblName" ForeColor="#0066ff" runat="server" Text='<%# Eval("StudentFullName")  %>'></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Admission Number">
                            <ItemTemplate>
                                <asp:Label ID="lblAdmNo" runat="server" Text='<%# Eval("AdmissionNumber")  %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Class">
                            <ItemTemplate>
                                <asp:Label ID="lblClass" runat="server" Text='<%# getClassName(Convert.ToInt64(Eval("Id"))) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Grade">
                            <ItemTemplate>
                                <asp:Label ID="lblGrade" runat="server" Text='<%# getGradeName(Convert.ToInt64( Eval("Id")))  %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" Gender ">
                            <ItemTemplate>
                                <asp:Label ID="lblGender" runat="server" Text='<%# (PASSIS.DAO.Utility.Gender) Eval("Gender")  %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Check">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkAll" OnCheckedChanged="OnCheckedChanged" AutoPostBack="true" runat="server" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk" AutoPostBack="true" OnCheckedChanged="OnCheckedChanged" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id")  %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- <asp:TemplateField HeaderText= " View ">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtn"  runat="server" Text = "Details" OnClick="lnkbtn_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>
 <PagerSettings FirstPageText="&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                        LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                        Mode="NextPreviousFirstLast" 
                        NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" 
                        PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />                
                                    </asp:GridView>
                            <br />
                        <div class="col-xl-6">
                            <asp:Button ID="Button1" class="btn btn-secondary" runat="server" Text="Export Student's Data"  OnClick="btnExport_Click" />
                         </div>
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        
                                        </form>

                                 </div>
                                </div>
                                </asp:Content>

