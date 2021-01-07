<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="ViewScores.aspx.cs" Inherits="ViewScores" %>


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
                                        <h4>VIEW UPLOADED SCORES (Single Entry)</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                             <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                       <label class="form-control-label">Term:<span class="text-danger ml-2">*</span></label>
                                             <asp:DropDownList runat="server" ID="ddlTerm" class="custom-select form-control" DataTextField="AcademicTermName" DataValueField="Id"></asp:DropDownList>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Session:<span class="text-danger ml-2">*</span></label>
                                    <asp:DropDownList runat="server" DataTextField="SessionName" class="custom-select form-control" DataValueField="ID" ID="ddlSession"></asp:DropDownList>
                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Class/Year:<span class="text-danger ml-2">*</span></label>
                                    <asp:DropDownList runat="server" class="custom-select form-control"  AutoPostBack="true" ID="ddlYear" AppendDataBoundItems="true"
                                        DataTextField="Name" DataValueField="Id" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                        <asp:ListItem Enabled="true" Selected="True" Text="--Select--" Value="0"></asp:ListItem>
                                    </asp:DropDownList>                                              
                                                </div>
                                                <div class="col-xl-6">
                                     <label class="form-control-label">Grade:<span class="text-danger ml-2">*</span></label>
                                     <asp:DropDownList runat="server" class="custom-select form-control"  AutoPostBack="true" ID="ddlGrade" OnSelectedIndexChanged="ddlGrade_SelectedIndexChanged" DataTextField="GradeName" DataValueField="Id">
                                    </asp:DropDownList>                                                  

                                                </div>
                                            </div>
                                             
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Subject:<span class="text-danger ml-2">*</span></label>
                                 <asp:DropDownList runat="server" class="custom-select form-control" ID="ddlClassSubject"></asp:DropDownList>
                                           
                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Select Category:<span class="text-danger ml-2">*</span></label>
                             <asp:DropDownList runat="server" AutoPostBack="true" class="custom-select form-control" DataTextField="Category" DataValueField="ID" ID="ddlCategory" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"></asp:DropDownList>

                                                </div>
                                            </div>
                                           <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Sub Category:<span class="text-danger ml-2">*</span></label>
                                     <asp:DropDownList runat="server" ID="ddlSubCategory" class="custom-select form-control" DataTextField="SubCategory" DataValueField="Id"></asp:DropDownList>
                                              
                                                </div>
                                            </div>
                                            <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSearch" class="btn btn-secondary"  OnClick="btnSearch_Click" runat="server" Text="View Scores"  />
                                                </div>
                                            </div>
                                                
                                                </form>    
                                        <br />
                                <%-- <div class="widget-header bordered no-actions d-flex align-items-center">
                                       <asp:Label ID="lblsubjectResp" ForeColor="Black" runat="server" Visible="false" Text="LIST OF SUBJECTS"></asp:Label>
                                 </div>--%>
                            <%--<asp:GridView ID="gvdAcademicSession" runat="server" AutoGenerateColumns="False" EmptyDataText=" No Calendar year currently exist" AllowPaging="true" Width="100%" ShowFooter="True" PageSize="20" GridLines="Both" OnRowCommand="gvdAcademicSession_RowCommand">       
                            <PagerSettings FirstPageText="&lt;i class=&quot;fa  fa-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-left&quot;&gt;&lt;/i&gt;"
                                LastPageText="&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;" Mode="NextPreviousFirstLast" 
                                NextPageText="&lt;i class=&quot;fa fa-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;fa fa-arrow-left&quot;&gt;&lt;/i&gt;" />--%>
             <asp:Label ID="lblStudents" Font-Size="Large" runat="server" Visible="false" ForeColor="Black" Text="LIST OF STUDENTS SCORES"></asp:Label>                   
                                        <asp:MultiView ID="MultiView1" runat="server">
                <asp:View ID="ViewTestAssignment" runat="server">
                    <asp:GridView ID="gvTestAssigment" class="table table-striped"  ForeColor="#333333" GridLines="None" AutoGenerateColumns="false" runat="server">
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
                            <asp:TemplateField HeaderText="S/N">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fullname">
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("StudentFullName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Admission No">
                                <ItemTemplate>
                                    <asp:Label ID="lblAdmNo" runat="server" Text='<%# Eval("AdmissionNumber") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
<%--                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("User.Id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Score Obtainable">
                                <ItemTemplate>
                                    <asp:Label ID="lblScore" runat="server" Text='<%# Eval("MarkObtainable") %>'>></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Score Obtained">
                                <ItemTemplate>
                                    <asp:Label ID="lblScore" runat="server" Text='<%# Eval("MarkObtained") %>'>></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Percentage Score">
                                <ItemTemplate>
                                    <asp:Label ID="lblScore" runat="server" Text='<%# Eval("PercentageScore") %>'>></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CA Percentage Score">
                                <ItemTemplate>
                                    <asp:Label ID="lblScore" runat="server" Text='<%# Eval("CAPercentageScore") %>'>></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CA Final Score">
                                <ItemTemplate>
                                    <asp:Label ID="lblScore" runat="server" Text='<%# Eval("FinalScore") %>'>></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remark">
                                <ItemTemplate>
                                    <asp:Label ID="lblRemark" runat="server" Text='<%# Eval("Remark") %>'>></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:View>
                <asp:View ID="ViewExam" runat="server">
                    <asp:GridView ID="gvExam" class="table table-striped"  ForeColor="#333333" GridLines="None" AutoGenerateColumns="false" runat="server">
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
                            <asp:TemplateField HeaderText="S/N">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fullname">
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("StudentFullName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Admission No">
                                <ItemTemplate>
                                    <asp:Label ID="lblAdmNo" runat="server" Text='<%# Eval("AdmissionNumber") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
<%--                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("User.Id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Score Obtainable">
                                <ItemTemplate>
                                    <asp:Label ID="lblScore" runat="server" Text='<%# Eval("ExamScoreObtainable") %>'>></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Score Obtained">
                                <ItemTemplate>
                                    <asp:Label ID="lblScore" runat="server" Text='<%# Eval("ExamScore") %>'>></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Percentage Score">
                                <ItemTemplate>
                                    <asp:Label ID="lblScore" runat="server" Text='<%# Eval("PercentageScore") %>'>></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Exam Percentage Score">
                                <ItemTemplate>
                                    <asp:Label ID="lblScore" runat="server" Text='<%# Eval("ExamPercentageScore") %>'>></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Exam Final Score">
                                <ItemTemplate>
                                    <asp:Label ID="lblScore" runat="server" Text='<%# Eval("FinalScore") %>'>></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remark">
                                <ItemTemplate>
                                    <asp:Label ID="lblRemark" runat="server" Text='<%# Eval("Remark") %>'>></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:View>
                <asp:View ID="ViewBehavioural" runat="server">
                    <asp:GridView ID="gvBehavioural" class="table table-striped"  GridLines="None" ForeColor="#333333" AutoGenerateColumns="false" runat="server">
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
                            <asp:TemplateField HeaderText="S/N">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fullname">
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("StudentFullName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Admission No">
                                <ItemTemplate>
                                    <asp:Label ID="lblAdmNo" runat="server" Text='<%# Eval("AdmissionNumber") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
<%--                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("User.Id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Score Obtainable">
                                <ItemTemplate>
                                    <asp:Label ID="lblScore" runat="server" Text='<%# Eval("MarkObtainable") %>'>></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Score Obtained">
                                <ItemTemplate>
                                    <asp:Label ID="lblScore" runat="server" Text='<%# Eval("MarkObtained") %>'>></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Percentage Score">
                                <ItemTemplate>
                                    <asp:Label ID="lblScore" runat="server" Text='<%# Eval("PercentageScore") %>'>></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CA Percentage Score">
                                <ItemTemplate>
                                    <asp:Label ID="lblScore" runat="server" Text='<%# Eval("CAPercentageScore") %>'>></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remark">
                                <ItemTemplate>
                                    <asp:Label ID="lblRemark" runat="server" Text='<%# Eval("Remark") %>'>></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:View>
                <asp:View ID="ViewExtraCurricular" runat="server">
                    <asp:GridView ID="gvExtraCurricular" class="table table-striped" GridLines="None" ForeColor="#333333" AutoGenerateColumns="false" runat="server">
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
                            <asp:TemplateField HeaderText="S/N">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fullname">
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("StudentFullName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Admission No">
                                <ItemTemplate>
                                    <asp:Label ID="lblAdmNo" runat="server" Text='<%# Eval("AdmissionNumber") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
<%--                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("User.Id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Score Obtainable">
                                <ItemTemplate>
                                    <asp:Label ID="lblScore" runat="server" Text='<%# Eval("MarkObtainable") %>'>></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Score Obtained">
                                <ItemTemplate>
                                    <asp:Label ID="lblScore" runat="server" Text='<%# Eval("MarkObtained") %>'>></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Percentage Score">
                                <ItemTemplate>
                                    <asp:Label ID="lblScore" runat="server" Text='<%# Eval("PercentageScore") %>'>></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CA Percentage Score">
                                <ItemTemplate>
                                    <asp:Label ID="lblScore" runat="server" Text='<%# Eval("CAPercentageScore") %>'>></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remark">
                                <ItemTemplate>
                                    <asp:Label ID="lblRemark" runat="server" Text='<%# Eval("Remark") %>'>></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:View>
            </asp:MultiView>

                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
                         
                                </asp:Content>
