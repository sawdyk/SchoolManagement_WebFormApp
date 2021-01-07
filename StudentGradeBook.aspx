<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="StudentGradeBook.aspx.cs" Inherits="StudentGradeBook" %>

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
                                        <h4>GRADE BOOK</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                           
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                         <label class="form-control-label">Select CA/EXAM:<span class="text-danger ml-2">*</span></label>
                                            <asp:DropDownList runat="server" class="custom-select form-control" DataTextField="Category" DataValueField="ID" ID="ddlCategory">
                                            </asp:DropDownList>

                                                </div>
                                            </div>
                                           
                                                
                                        <br />
                               
              <asp:Label ID="lblSubjetcs" ForeColor="Black" Font-Size="Large" runat="server" Visible="True" Text="ALL SUBJECTS"></asp:Label>
                 <asp:GridView ID="gdvAllSubj" runat="server" class="table table-striped"  ForeColor="#333333"  GridLines="None" AutoGenerateColumns="false" EmptyDataText=" No result."
                        AllowPaging="false" Width="100%"
                        OnRowCommand="gdvAllSubject_RowCommand">
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
                            <asp:TemplateField HeaderText="ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                       <div class="form-group row mb-3">
                        <div class="col-xl-6 mb-3">
                           <asp:Button ID="btnViewScores" class="btn btn-secondary"  OnClick="btnViewScores_Click" runat="server" Text="View Scores"  />
                        </div>                                                                                    
                        </div>  
                              </form>              
                       <asp:Label ID="lblUploaded" ForeColor="Black" Font-Size="Large" runat="server" Visible="false"></asp:Label>           
                    <asp:MultiView ID="MultiView1" runat="server">
                            <asp:View ID="ViewTestAssignment" runat="server">
                                <asp:GridView ID="gdvTestScores" runat="server" class="table table-striped"  ForeColor="#333333" AutoGenerateColumns="false" EmptyDataText=" No scores available" 
                                    Width="100%"  PageSize="30"  AllowPaging="true" GridLines="None"  Visible="false">
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
                                        <asp:TemplateField HeaderText="Fullname">
                                            <ItemTemplate>
                                                <asp:Label ID="lblName" runat="server" Text='<%# new SchoolLIB().GetUserDetails(Eval("AdmissionNo").ToString()).StudentFullName  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Admission No">
                                            <%-- <EditItemTemplate>
                                           <asp:TextBox ID="txtAdmissionNo" Text='<%# Eval("AdmissionNo")  %>' runat="server"></asp:TextBox>

                                            </EditItemTemplate>--%>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAdmNo" runat="server" Text='<%# new SchoolLIB().GetUserDetails(Eval("AdmissionNo").ToString()).AdmissionNumber  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--                                        <asp:TemplateField HeaderText="Template Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCode" runat="server" Text='<%# Eval("TemplateCode")  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                          <asp:TemplateField HeaderText="Subject">
                                            <ItemTemplate>
                                                <asp:Label ID="SubjectName" runat="server" Text='<%# SubjectName(Convert.ToInt64( Eval("SubjectId")))  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <%-- <asp:TemplateField HeaderText="Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCode" runat="server" Text='<%# Eval("Code")  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                       <%-- <asp:TemplateField HeaderText="Category">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCategory" runat="server" Text='<%# CategoryName(Convert.ToInt64(Eval("CategoryId")))  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Sub Category">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubCategory" runat="server" Text='<%# SubCategoryName(Convert.ToInt64(Eval("SubCategoryId")))  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                       
                                        <asp:TemplateField HeaderText="Mark Obtainable">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExamScoreObtainable" runat="server" Text='<%# Eval("MarkObtainable")  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Mark Obtained">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExamScore" runat="server" Text='<%# Eval("MarkObtained")  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Percentage Score">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExamScore" runat="server" Text='<%# Eval("PercentageScore")  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Percenatage Score">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExamScore" runat="server" Text='<%# Eval("CAPercentageScore")  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Final Score">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExamScore" runat="server" Text='<%# Eval("FinalScore")  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" Text='<%# Eval("StudentId")  %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings FirstPageText="&lt;i class=&quot;la  la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                                    LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                                    Mode="NextPreviousFirstLast" NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" 
                                    PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />
                                </asp:GridView>
                                </asp:View>

                                <asp:View ID="ViewExam" runat="server">
                                <asp:GridView ID="gdvExamScores" runat="server" ForeColor="#333333" AutoGenerateColumns="false" EmptyDataText=" No scores available" class="table table-striped" 
                                    Width="100%" PageSize="30"  AllowPaging="true" GridLines="None" Visible="false">
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
                                        <asp:TemplateField HeaderText="Fullname">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# new SchoolLIB().GetUserDetails(Eval("AdmissionNumber").ToString()).StudentFullName  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Admission No">
                                            <%-- <EditItemTemplate>
                                           <asp:TextBox ID="txtAdmissionNo" Text='<%# Eval("AdmissionNo")  %>' runat="server"></asp:TextBox>

                                            </EditItemTemplate>--%>
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# new SchoolLIB().GetUserDetails(Eval("AdmissionNumber").ToString()).AdmissionNumber  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Subject">
                                            <ItemTemplate>
                                                <asp:Label ID="Label3" runat="server" Text='<%# SubjectName(Convert.ToInt64( Eval("SubjectId")))  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <%-- <asp:TemplateField HeaderText="Code">
                                            <ItemTemplate>
                                                <asp:Label ID="Label4" runat="server" Text='<%# Eval("Code")  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <%-- <asp:TemplateField HeaderText="Category">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCategory" runat="server" Text='<%# CategoryName(Convert.ToInt64(Eval("CategoryId")))  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Sub Category">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubCategory" runat="server" Text='<%# SubCategoryName(Convert.ToInt64(Eval("SubCategoryId")))  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                       
                                        <asp:TemplateField HeaderText="Exam Score Obtainable">
                                            <ItemTemplate>
                                                <asp:Label ID="Label6" runat="server" Text='<%# Eval("ExamScoreObtainable")  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Exam Score">
                                            <ItemTemplate>
                                                <asp:Label ID="Label5" runat="server" Text='<%# Eval("ExamScore")  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Percentage Score">
                                            <ItemTemplate>
                                                <asp:Label ID="Label5" runat="server" Text='<%# Eval("PercentageScore")  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Exam Percentage Score">
                                            <ItemTemplate>
                                                <asp:Label ID="Label5" runat="server" Text='<%# Eval("ExamPercentageScore")  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Final Score">
                                            <ItemTemplate>
                                                <asp:Label ID="Label5" runat="server" Text='<%# Eval("FinalScore")  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="Label7" Text='<%# Eval("StudentId")  %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                            <PagerSettings FirstPageText="&lt;i class=&quot;la  la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                                    LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                                    Mode="NextPreviousFirstLast" NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" 
                                    PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />
                                </asp:GridView>
                            </asp:View>
                        </asp:MultiView>



                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>                         
                                </asp:Content>
