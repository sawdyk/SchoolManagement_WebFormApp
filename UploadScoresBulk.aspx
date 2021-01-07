<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="UploadScoresBulk.aspx.cs" Inherits="UploadScoresBulk" %>


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
                                        <h4>UPLOAD SCORES(BULK)</h4>
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
                                                    <label class="form-control-label">Upload File:<span class="text-danger ml-2">*</span></label>
                                            <asp:FileUpload ID="documentUpload" class="btn btn-primary" runat="server" />
                                           
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
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Description:<span class="text-danger ml-2">*</span></label>
                                            <asp:DropDownList runat="server" ID="ddlDescription" class="custom-select form-control"></asp:DropDownList>
                                                  

                                                </div>
                                            </div>
                                         <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Mark Obtainable:<span class="text-danger ml-2">*</span></label>
                                      <asp:TextBox ID="txtTotalMark" runat="server" class="form-control" onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);"></asp:TextBox>
                                                
                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Code:<span class="text-danger ml-2">*</span></label>
                                    <asp:TextBox ID="txtCode" runat="server" class="form-control" onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);"></asp:TextBox>
                                     

                                                </div>
                                            </div>
                                         
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <asp:Button ID="btnSaveScore" class="btn btn-secondary" Visible="false" OnClientClick="javascript:return confirm('Are you sure you want to save this Record?');"  OnClick="btnSaveScore_Click" runat="server" Text="Save Scores"  />
                                                </div>
                                                 <div class="col-xl-6">
                                                    <asp:Button ID="btnUploadScores" class="btn btn-secondary"  OnClick="btnUploadScores_Click" runat="server" Text="Upload Scores"  /> &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnViewScores" Visible="false" class="btn btn-secondary"  OnClick="btnViewScores_Click" runat="server" Text="View Scores"  />
                                                     <asp:Button ID="btnCancel" Visible="false" class="btn btn-secondary" OnClientClick="javascript:return confirm('Are you sure you want to cancel this Record?');"  OnClick="btnCancel_Click" runat="server" Text="Cancel"  />

                                                </div>
                                            </div>
                                                
                                                </form>    
                                        <br />
                         <asp:Label ID="lblresltt" Font-Size="Large"  ForeColor="Black" runat="server" Visible="false" Text="UPLOADED SCORES"></asp:Label>

                                <asp:Panel runat="server" class="table table-striped" ID="pnl"  ScrollBars="Both">
                                     <%-- <asp:MultiView ID="MultiView1" runat="server">
                            <asp:View ID="ViewExam" runat="server">--%>
                                <asp:GridView ID="gdvViewExtendedScores"   CellPadding="4" Width="100%"  ForeColor="Black" GridLines="None" class="table table-striped" 
                                    AutoGenerateColumns="false" runat="server" >
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
                           <%-- <asp:TemplateField HeaderText="Fullname">
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("User.StudentFullName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                           
                            <asp:TemplateField HeaderText="StudentFulllName">
                                <ItemTemplate>
                                    <asp:Label ID="lblStudentName" runat="server" Text='<%# getStudentFullName(Convert.ToInt64( Eval("StudentId"))) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Admission No">
                                <ItemTemplate>
                                    <asp:Label ID="lblAdmNo" runat="server" Text='<%# Eval("AdmissionNumber") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Admission No">
                                <ItemTemplate>
                                    <asp:Label ID="lblSubjectName" runat="server" Text='<%# getSubjectName(Convert.ToInt64( Eval("SubjectId"))) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("StudentId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblSubjectId" runat="server" Text='<%# Eval("SubjectId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblCategoryId" runat="server" Text='<%# Eval("CategoryId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblSubCategoryId" runat="server" Text='<%# Eval("SubCategoryId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblTermId" runat="server" Text='<%# Eval("TermId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblAcademicSessionID" runat="server" Text='<%# Eval("AcademicSessionID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblClassId" runat="server" Text='<%# Eval("ClassId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblGradeId" runat="server" Text='<%# Eval("GradeId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Exam Score Obtainable">
                                <ItemTemplate>
                                    <asp:Label ID="lblExamScoreObtainable" runat="server" Text='<%# Eval("ExamScoreObtainable") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Exam Score">
                                <ItemTemplate>
                                    <asp:Label ID="lblExamScore" runat="server" Text='<%# Eval("ExamScore") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblTemplateId" runat="server" Text='<%# Eval("BroadSheetTemplateID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblCode" runat="server" Text='<%# Eval("Code") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblUploadedById" runat="server" Text='<%# Eval("UploadedById") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblScoreId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                          <PagerSettings FirstPageText="&lt;i class=&quot;la  la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                                    LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                                    Mode="NextPreviousFirstLast" NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" 
                                    PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />            
                    </asp:GridView>
            
                              </asp:Panel>
                                        
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                         
                                </asp:Content>
