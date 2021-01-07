<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="UploadLessonNote.aspx.cs" Inherits="UploadLessonNote" %>

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
                                        <h4>UPLOAD LESSON NOTE</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Year:<span class="text-danger ml-2">*</span></label>
                                                        <asp:DropDownList runat="server" class="custom-select form-control" AutoPostBack ="true" ID="ddlYear" DataTextField="Name" DataValueField="Id" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                                        </asp:DropDownList>                                             

                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Grade:<span class="text-danger ml-2">*</span></label>
                                            <asp:DropDownList runat="server" AutoPostBack ="true" class="custom-select form-control" ID="ddlGrade" DataTextField="GradeName" DataValueField="Id" OnSelectedIndexChanged="ddlGrade_SelectedIndexChanged">
                                            </asp:DropDownList>
                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Session:<span class="text-danger ml-2">*</span></label>
                                                <asp:DropDownList runat="server" AutoPostBack="false" class="custom-select form-control" ID="ddlSession" 
                                                    DataTextField="SessionName" DataValueField="Id" >
                                                </asp:DropDownList>                                               
                                                   </div>

                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Term:<span class="text-danger ml-2">*</span></label>
                                                <asp:DropDownList runat="server" ID="ddlTerm" class="custom-select form-control" DataTextField="AcademicTermName" DataValueField="Id"  AppendDataBoundItems = "true">
                                         </asp:DropDownList>                                                   
                                                  </div>
                                                  <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Subject:<span class="text-danger ml-2">*</span></label>
                                                <asp:DropDownList runat="server" class="custom-select form-control" AutoPostBack="true" ID="ddlSubject" DataTextField="Name" DataValueField="Id">
                                                 </asp:DropDownList>                                        
                                                   </div>
                                                   <div class="col-xl-6">
                                                 <label class="form-control-label">Description:<span class="text-danger ml-2">*</span></label>
                                        <asp:TextBox ID="txtDescription" class="form-control" runat="server"></asp:TextBox>
                                             
                                                  </div>
                                                  <br /> <br />
                                                   <div class="col-xl-6">
                                                 <label class="form-control-label">Upload File:<span class="text-danger ml-2">*</span></label>
                                                  <asp:FileUpload class="btn btn-primary" ID="LessonNoteFile" runat="server" />
                                                 
                                                  </div>
                                                 
                                               </div>  
                                            

                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnLessonUpload" class="btn btn-secondary"  OnClick="btnLessonUpload_Click" runat="server" Text="Upload"  />
                                                </div>
                                            </div>
                                                </form>    
                                        <br /><br />
                                 <%--<div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>CLASS ARMS</h4>
                                 </div>--%>
                            <%--<asp:GridView ID="gvdAcademicSession" runat="server" AutoGenerateColumns="False" EmptyDataText=" No Calendar year currently exist" AllowPaging="true" Width="100%" ShowFooter="True" PageSize="20" GridLines="Both" OnRowCommand="gvdAcademicSession_RowCommand">       
                            <PagerSettings FirstPageText="&lt;i class=&quot;fa  fa-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-left&quot;&gt;&lt;/i&gt;"
                                LastPageText="&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;" Mode="NextPreviousFirstLast" 
                                NextPageText="&lt;i class=&quot;fa fa-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;fa fa-arrow-left&quot;&gt;&lt;/i&gt;" />--%>
                  <asp:Label ID="lblStudents" Font-Size="Large" runat="server" Visible="true" ForeColor="Black" Text="LIST OF UPLOADED LESSON NOTES"></asp:Label>                    
                                 
                                        <asp:Panel runat="server"  ID="pnlAssignmentUpload" ScrollBars="Horizontal">
                                <asp:GridView ID="gdvList" runat="server" class="table table-striped"  AutoGenerateColumns="false" EmptyDataText=" No Lesson Note has been uploaded"
                                    AllowPaging="true" Width="1000px" ForeColor="#333333" GridLines="None" OnRowCommand="gdvList_RowCommand">
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
                                        <asp:TemplateField HeaderText=" Description ">
                                            <HeaderStyle Width="100px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Description")  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Subject ">
                                            <HeaderStyle Width="100px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblCod3e" runat="server" Text='<%# new SchoolLIB().RetrieveSubjectInSchoolById(Convert.ToInt64(Eval("SubjectId"))).Name  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Year ">
                                            <HeaderStyle Width="100px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblCode" runat="server" Text='<%# getYear(Convert.ToInt64(Eval("ClassId"))).Name  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Grade ">
                                            <HeaderStyle Width="100px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblGradeName" runat="server" Text='<%# getGrade(Convert.ToInt64(Eval("GradeId"))).GradeName  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Session ">
                                            <HeaderStyle Width="100px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblCode333" runat="server" Text='<%# getSession(Convert.ToInt64(Eval("SessionId"))).SessionName  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Term ">
                                            <HeaderStyle Width="50px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblCode4342" runat="server" Text='<%#  getTerm(Convert.ToInt64(Eval("TermId"))).AcademicTermName %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Status ">
                                            <HeaderStyle Width="50px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%#  getLNStatus(Convert.ToInt64(Eval("Status"))) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       <%-- <asp:TemplateField HeaderText=" Date Uploaded ">
                                            <HeaderStyle Width="100px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblCode3232" runat="server" Text='<%#  Eval("DateUploaded") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Due Date">
                                            <HeaderStyle Width="100px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblDueDate" runat="server" Text='<%#  Eval("DueDate") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Uploaded By">
                                            <HeaderStyle Width="100px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblCodegfdgf" runat="server" Text='<%# new UsersDAL().RetrieveUser(Convert.ToInt64(Eval("TeacherId"))).FullName  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField ShowHeader="false">
                                            <ItemTemplate>
                                                <headerstyle width="100px" />
                                                <asp:LinkButton ID="lnkDownload" runat="server" CommandArgument='<%# Eval("FileName") %>'
                                                    CommandName="download" SkinID="lnkGreen" ForeColor="White" class="btn btn-success"> Download </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false">
                                            <ItemTemplate>
                                                <headerstyle width="100px" />
                                                <asp:LinkButton ID="lnkDownload342342" OnClientClick="javascript:return confirm('Are you sure you want to remove this lesson note?');" runat="server" CommandArgument='<%# Eval("Id") %>'
                                                    CommandName="remove" SkinID="lnkGreen" ForeColor="White" class="btn btn-danger"> Remove </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIdfdfsdfs" runat="server" Text='<%# Eval("Id") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                        <PagerSettings FirstPageText="&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                        LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                        Mode="NextPreviousFirstLast" 
                        NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" 
                        PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />                               

                                </asp:GridView>
                            </asp:Panel>
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
                         
                                </asp:Content>
