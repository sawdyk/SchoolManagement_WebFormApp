<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="EditReportCardComment.aspx.cs" Inherits="EditReportCardComment" %>

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
                                        <h4>EDIT COMMENT AND REMARK ON STUDENT REPORT CARD</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Year:<span class="text-danger ml-2">*</span></label>
                                                       <asp:DropDownList runat="server" AutoPostBack="true" class="custom-select form-control" ID="ddlYear" AppendDataBoundItems="true"
                                        DataTextField="Name" DataValueField="Id" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                        <asp:ListItem Enabled="true" Selected="True" Text="--Select--" Value="0"></asp:ListItem>
                                    </asp:DropDownList>                                          

                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Grade:<span class="text-danger ml-2">*</span></label>
                                 <asp:DropDownList runat="server" AutoPostBack="true" class="custom-select form-control" ID="ddlGrade" DataTextField="GradeName" DataValueField="Id" OnSelectedIndexChanged="ddlGrade_SelectedIndexChanged">
                                 </asp:DropDownList>

                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Session:<span class="text-danger ml-2">*</span></label>
                                      <asp:DropDownList runat="server" class="custom-select form-control" DataTextField="SessionName" DataValueField="ID" ID="ddlSession">
                                      </asp:DropDownList>
                                                   </div>

                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Term:<span class="text-danger ml-2">*</span></label>
                            <asp:DropDownList runat="server" ID="ddlTerm" class="custom-select form-control" DataTextField="AcademicTermName" DataValueField="Id">
                            </asp:DropDownList>
                                                  </div>
                                                  <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Comments For:<span class="text-danger ml-2">*</span></label>
                                 <asp:DropDownList runat="server" class="custom-select form-control" ID="ddlCommentFor" DataTextField="CommentBy" DataValueField="ID">
                                 </asp:DropDownList>
                                       
                                                   </div>
                                               </div>  
                                            

                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSearch" class="btn btn-secondary"  OnClick="btnSearch_Click" runat="server" Text="View Added Comments"  />
                                                </div>
                                            </div>
                                                </form>    
                                        <br /><br />
                               
  <asp:Label ID="lblAcademic" Font-Size="Large" runat="server" Visible="false" ForeColor="Black" Text="LIST OF STUDENTS REPORT CARD COMMENTS"></asp:Label>             
    <asp:GridView ID="gvComment" ForeColor="Black" class="table table-striped" GridLines="None" AutoGenerateColumns="false" runat="server" OnRowCancelingEdit="gvComment_RowCancelingEdit" PageSize="20" OnPageIndexChanging="gvComment_PageIndexChanging"
      OnRowUpdating="gvComment_RowUpdating" OnRowEditing="gvComment_RowEditing">
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
                    <asp:TemplateField HeaderText="Fullname">
                        <ItemTemplate>
                            <asp:Label ID="lblName" runat="server" Text='<%# studentFullName((long)Eval("StudentId")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Admission No">
                        <ItemTemplate>
                            <asp:Label ID="lblAdmNo" runat="server" Text='<%# Eval("AdmissionNumber") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                  
                    <asp:TemplateField HeaderText="Comment">
                           <EditItemTemplate>
                        <asp:TextBox ID="txtComment" class="form-control" Text='<%# Eval("Comment")  %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblComment" Text='<%# Eval("Comment")  %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Remark">
                           <EditItemTemplate>
                        <asp:TextBox ID="txtRemark" class="form-control" Text='<%# Eval("Remark")  %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblRemark" Text='<%# Eval("Remark")  %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="ID" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblId" Text='<%# Eval("Id")  %>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        <asp:LinkButton ID="lnkUpdate" runat="server" CommandArgument='<%# Eval("Id")  %>' CausesValidation="true" CommandName="Update" Text="Update" ToolTip="Update"><i class="la la-floppy-o la-2x"></i></asp:LinkButton>
                        &nbsp;<asp:LinkButton ID="lnkCancel" runat="server" CommandArgument='<%# Eval("Id")  %>' CausesValidation="False" CommandName="Cancel" Text="Cancel" ToolTip="Cancel"><i class="la la-remove la-2x"></i></asp:LinkButton>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEdit" CommandArgument='<%# Eval("Id") %>' runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" ToolTip="Edit"><i class="la la-edit la-2x"></i></asp:LinkButton>`
                    </ItemTemplate>
                </asp:TemplateField>                        
                </Columns>
            </asp:GridView>
                                          
                                               <%-- <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSaveComment" class="btn btn-secondary"  Visible="false" OnClick="btnSaveComment_Click" runat="server" Text="Save"  />
                                                </div>--%>
                                            </div>
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                         
                                </asp:Content>

