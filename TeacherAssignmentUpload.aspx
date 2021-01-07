<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="TeacherAssignmentUpload.aspx.cs" Inherits="TeacherAssignmentUpload" %>

<%@ Import Namespace="PASSIS.DAO" %>
<%@ Import Namespace="PASSIS.LIB" %>
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
                                        <h4>DOWNLOAD/UPLOAD ASSIGNMENTS</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Assignment Title:<span class="text-danger ml-2">*</span></label>
                                      <asp:TextBox runat="server" class="form-control" ID="txtDesc"></asp:TextBox>
                                                </div>
                                                <div class="col-xl-6">
                                     <label class="form-control-label">Alloted Score:<span class="text-danger ml-2">*</span></label>
                                   <asp:TextBox runat="server" class="form-control" ID="txtScore"></asp:TextBox>
                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                       <label class="form-control-label">Assignment Recipients:<span class="text-danger ml-2">*</span></label>
                            <asp:DropDownList runat="server" class="custom-select form-control" AutoPostBack="true" ID="ddlAssignmentRecipient"
                                OnSelectedIndexChanged="ddlAssignmentRecipient_OnSelectedIndexChanged">
                                <asp:ListItem Selected="True" Text="  Class  " Value="1"></asp:ListItem>
                                <asp:ListItem Text= "Set" Value="2"></asp:ListItem>
                            </asp:DropDownList>                                                

                                                   </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Year:<span class="text-danger ml-2">*</span></label>
                         <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlYear" class="custom-select form-control" AppendDataBoundItems="true"
                                DataTextField="Name" DataValueField="Id" OnSelectedIndexChanged="ddlYear_OnSelectedIndexChanged">
                                <asp:ListItem Enabled="true" Selected="True" Text="--  Select  --" Value="0"></asp:ListItem>
                            </asp:DropDownList>                                                  

                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Class:<span class="text-danger ml-2">*</span></label>
                            <asp:DropDownList runat="server" AutoPostBack="true" class="custom-select form-control" OnSelectedIndexChanged="ddlGrade_SelectedIndexChanged" ID="ddlGrade" DataTextField="GradeName" DataValueField="Id">
                            </asp:DropDownList>                                           
                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Subject:<span class="text-danger ml-2">*</span></label>
                            <asp:DropDownList runat="server" ID="ddlSubject" class="custom-select form-control" AppendDataBoundItems="true">
                            </asp:DropDownList>
                                                </div>
                                            </div>
                                           <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Uplaod File:<span class="text-danger ml-2">*</span></label>
                            <asp:FileUpload ID="documentUpload" class="btn btn-primary" runat="server" />
                                              
                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Assignment Due Date:<span class="text-danger ml-2">*</span></label>
                                <asp:TextBox runat="server" ID="txtAssignmentDueDate" class="form-control"  Columns="10" placeholder="Due Date"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender 
                                                     ID="myDateFromCal" Format="yyyy-MM-dd"  runat="server" TargetControlID="txtAssignmentDueDate" PopupButtonID="ImageButton1" >
                                                      </ajaxToolkit:CalendarExtender>                                               

                                                </div>
                                            </div>
                                       
                                            <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSave" class="btn btn-secondary"  OnClick="btnSave_OnClick" runat="server" Text="Save&Upload"  />
                                                </div>
                                            </div>
                                                
                                                </form>    
                                        <br />
                             
                                         <asp:Label ID="lblUpload" Font-Size="Large" runat="server" Visible="true" ForeColor="Black" Text="LIST OF UPLOADED ASSIGNMENTS"></asp:Label>
                              <asp:Panel runat="server" ID="pnlAssignmentUpload" ScrollBars="Horizontal">
                                <asp:GridView ID="gdvList" runat="server" PageSize="20" AutoGenerateColumns="false" class="table table-striped"  EmptyDataText=" No Assignment has been uploaded"
                                    AllowPaging="true" Width="100%"  GridLines="None" ForeColor="#333333"  OnRowCommand="gdvList_RowCommand" OnPageIndexChanging="gdvList_PageIndexChanging">
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
                                        <asp:TemplateField HeaderText=" Title ">
                                            <HeaderStyle Width="100px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Description")  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Subject ">
                                            <HeaderStyle Width="100px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblCod3e" runat="server" Text='<%# getSubjectName(Convert.ToInt64(Eval("SubjectId"))) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Year ">
                                            <HeaderStyle Width="100px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblCode" runat="server" Text='<%# getYear(Eval("GradeId"), Eval("GroupId"))  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Class/ Group ">
                                            <HeaderStyle Width="100px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblCode333" runat="server" Text='<%# getClassOrGroupName(Eval("GradeId"), Eval("GroupId"))  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Maximum Score ">
                                            <HeaderStyle Width="50px" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblCode4342" runat="server" Text='<%#  Eval("MaximumObtainableScore") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Date Uploaded ">
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
                                                <asp:Label ID="lblCodegfdgf" runat="server" Text='<%# getTeacherName(Convert.ToInt64(Eval("TeacherId")))%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false">
                                            <ItemTemplate>
                                                <headerstyle width="100px" />
                                                <asp:LinkButton ID="lnkDownload" runat="server" CommandArgument='<%# Eval("FileName") %>'
                                                    CommandName="download" class="btn btn-success"> Download </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false">
                                            <ItemTemplate>
                                                <headerstyle width="100px" />
                                                <asp:LinkButton ID="lnkDownload342342" OnClientClick="javascript:return confirm('Are you sure you want to remove this assignment?');" runat="server" CommandArgument='<%# Eval("Id") %>'
                                                    CommandName="remove" class="btn btn-danger"> Remove </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIdfdfsdfs" runat="server" Text='<%# Eval("Id") %>' />
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
                                </div>
                         
                                </asp:Content>
