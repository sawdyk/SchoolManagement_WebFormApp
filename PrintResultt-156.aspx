<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="PrintResultt-156.aspx.cs" Inherits="PrintResultt_156" %>


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
                                        <h4>PRINT RESULT</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" class="text-danger ml-2"></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Session:<span class="text-danger ml-2">*</span></label>
                                            <asp:DropDownList runat="server" AutoPostBack="true" class="custom-select form-control" ID="ddlAcademicSession" OnSelectedIndexChanged="ddlSession_SelectedIndexChanged">
                                            <asp:ListItem Enabled="true" Selected="True"  Text="--  Select All --" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Term:<span class="text-danger ml-2">*</span></label>
                                        <asp:DropDownList runat="server" ID="ddlAcademicTerm" class="custom-select form-control" OnSelectedIndexChanged="ddlAcademicTerm_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Enabled="true" Selected="True" Text="--  Select All --" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Year:<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlYear" AppendDataBoundItems="true"
                                            DataTextField="Name" DataValueField="Id" class="custom-select form-control" OnSelectedIndexChanged="ddlYear_OnSelectedIndexChanged">
                                            <asp:ListItem Enabled="true" Selected="True" Text="--  Select All --" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                                </div>
                                               <div class="col-xl-6">
                                                 <label class="form-control-label">Grade:<span class="text-danger ml-2">*</span></label>
                                                     <asp:DropDownList runat="server" ID="ddlGrade" class="custom-select form-control" OnSelectedIndexChanged="ddlGrade_SelectedIndexChanged" DataTextField="GradeName" DataValueField="Id"
                                            AutoPostBack="true" AppendDataBoundItems="true">
                                        </asp:DropDownList>         
                                          
                                             </form>
                                            </div>
                                            </div>
                                 <%--<div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Current Session:<span class="text-danger ml-2">*</span></label>
                                                  <asp:DropDownList runat="server" AutoPostBack="true" class="custom-select form-control" ID="ddlCurrentSession">
                                            <asp:ListItem Enabled="true" Selected="True" Text="--  Select All --" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                                </div>
                                               <div class="col-xl-6">
                                           <label class="form-control-label">Current Year:<span class="text-danger ml-2">*</span></label>
                                               <asp:DropDownList runat="server" AutoPostBack="true" class="custom-select form-control" ID="ddlCurrentYear" AppendDataBoundItems="true"
                                            DataTextField="Name" DataValueField="Id" OnSelectedIndexChanged="ddlCurrentYear_SelectedIndexChanged">
                                            <asp:ListItem Enabled="true" Selected="True" Text="--  Select All --" Value="0"></asp:ListItem>
                                        </asp:DropDownList>   
                                            </div>
                                            </div>
                                  <div class="form-group row mb-3">
                                 <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Current Grade:<span class="text-danger ml-2">*</span></label>
                                                 <asp:DropDownList runat="server" ID="ddlCurrentGrade" class="custom-select form-control" OnSelectedIndexChanged="ddlCurrentGrade_SelectedIndexChanged" DataTextField="GradeName" DataValueField="Id"
                                            AutoPostBack="true" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                                </div>
                                </div>--%>
                              <asp:Label ID="lblMsgDisplay" Font-Size="Large" runat="server" Visible="false" ForeColor="Black" ></asp:Label>
                                            <asp:GridView ID="gdvList" runat="server" Width="100%" ForeColor="Black" GridLines="None"  class="table table-striped" AutoGenerateColumns="false" EmptyDataText=" No result."
                                AllowPaging="true" OnPageIndexChanging="gdvList_PageIndexChanging"
                                OnRowCommand="gdvList_RowCommand">
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
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText=" Year  ">
                                <ItemTemplate>
                                    <asp:Label ID="lblCode" runat="server" Text='<%#  new ClassGradeDAL().RetrieveGrade(Convert.ToInt64(Eval("GradeId"))).Class_Grade.Name %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" Grade ">
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%# new ClassGradeDAL().RetrieveGrade(Convert.ToInt64(Eval("GradeId"))).GradeName %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" Student's FullName ">
                                <ItemTemplate>
                                    <asp:Label ID="lblName324o9934" runat="server" Text='<%# String.Format("{0} {1} {2} ",Eval("User.LastName"),Eval("User.FirstName"),Eval("User.MiddleName")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText=" Student's FullName ">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName324o9934" runat="server" Text='<%# String.Format("{0} {1} {2} ",Eval("LastName"),Eval("FirstName"),Eval("MiddleName")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--                            <asp:TemplateField ShowHeader="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkView" runat="server" CommandArgument='<%# Eval("User.AdmissionNumber") %>'
                                        CommandName="ViewReport" SkinID="lnkGreen"> View Report Card </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                                    <asp:TemplateField ShowHeader="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDownload" runat="server" CommandArgument='<%# Eval("Id") %>'
                                                CommandName="report" class="btn btn-success" SkinID="lnkGreen"> Print Report Card </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStudentId" runat="server" Text='<%#  Eval("Id")  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
 <PagerSettings FirstPageText="&lt;i class=&quot;la  la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;"
                                LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" Mode="NextPreviousFirstLast" 
                                NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />       

                                            </asp:GridView>
                            <%--Commented by Olayemi, It works--%>
                            <%--<asp:GridView ID="gdvList" runat="server" AutoGenerateColumns="false" EmptyDataText=" No result."
                        AllowPaging="true" Width="100%" OnPageIndexChanging="gdvList_PageIndexChanging"
                        OnRowCommand="gdvList_RowCommand">
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
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" Year  ">
                                <ItemTemplate>
                                    <asp:Label ID="lblCode" runat="server" Text='<%#  new ClassGradeDAL().RetrieveGrade(Convert.ToInt64(Eval("GradeId"))).Class_Grade.Name %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" Grade ">
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%# new ClassGradeDAL().RetrieveGrade(Convert.ToInt64(Eval("GradeId"))).GradeName %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText=" Student's FullName ">
                                <ItemTemplate>
                                    <asp:Label ID="lblName324o9934" runat="server" Text='<%# String.Format("{0} {1} {2} ",Eval("User.LastName"),Eval("User.FirstName"),Eval("User.MiddleName")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
<%--                            <asp:TemplateField ShowHeader="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkView" runat="server" CommandArgument='<%# Eval("User.AdmissionNumber") %>'
                                        CommandName="ViewReport" SkinID="lnkGreen"> View Report Card </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>-%>
                            <asp:TemplateField ShowHeader="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDownload" runat="server" CommandArgument='<%# Eval("User.Id") %>'
                                        CommandName="report" SkinID="lnkGreen"> Print Report Card </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblStudentId" runat="server" Text='<%#  Eval("User.Id")  %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings FirstPageText="&lt;i class=&quot;fa  fa-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-left&quot;&gt;&lt;/i&gt;" LastPageText="&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;" Mode="NextPreviousFirstLast" NextPageText="&lt;i class=&quot;fa fa-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;fa fa-arrow-left&quot;&gt;&lt;/i&gt;" />
                    </asp:GridView>--%>




                                   <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                  <asp:Button ID="btnPrintAll" class="btn btn-secondary" runat="server" Text="Print All"  OnClick="btnPrintAll_OnClick" />
                                     
                                                </div>
                                                <div class="col-xl-6">
                 <asp:Button ID="btnApprove" class="btn btn-secondary" runat="server" Text="Print Selection"  OnClick="btnPrintSelection_OnClick" />   &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;

                                                </div>
                                            </div>

                                            </div>
                                         </div>                         
</asp:Content>
