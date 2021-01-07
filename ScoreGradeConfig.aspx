<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="ScoreGradeConfig.aspx.cs" Inherits="ScoreGradeConfig" %>

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
                                        <h4>CREATE SCORE GRADES</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Class:<span class="text-danger ml-2">*</span></label>
                                                    <asp:DropDownList ID="ddlClass" DataTextField="Name" DataValueField="Id" class="custom-select form-control" runat="server"></asp:DropDownList>

                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Lowest Range:<span class="text-danger ml-2">*</span></label>
                                                    <asp:TextBox ID="txtLowestRange" placeholder="e.g 50" class="form-control" runat="server"></asp:TextBox>

                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Highest Range:<span class="text-danger ml-2">*</span></label>
                                                   <asp:TextBox ID="txtHighestRange" placeholder="e.g 70" class="form-control" runat="server"></asp:TextBox>

                                                </div>
                                               <div class="col-xl-6">
                                                 <label class="form-control-label">Grade:<span class="text-danger ml-2">*</span></label>
                                                 <asp:TextBox ID="txtGrade" placeholder="e.g A" class="form-control" runat="server"></asp:TextBox>
                                                     </div>
                                                  <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Remark:<span class="text-danger ml-2">*</span></label>
                                                   <asp:TextBox ID="txtRemark" placeholder="e.g Excellent" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                                      </div>
                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSave" class="btn btn-secondary"  OnClick="btnSave_Click" runat="server" Text="Save Score Grade"  />
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
                               <asp:Label ID="lblAcademic" Font-Size="Large" runat="server" Visible="true" ForeColor="Black" Text="LIST OF SCORE GRADES"></asp:Label>

                                        <asp:Panel ID="Panel2" Width="900px" class="table table-striped" ScrollBars="Vertical" runat="server">
      <asp:GridView ID="gvScoreGrades" EmptyDataText="No Score Grade" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" class="table table-striped"
     CellPadding="2" ForeColor="#333333"  GridLines="None" OnRowCancelingEdit="gvScoreGrades_RowCancelingEdit" PageSize="20" OnPageIndexChanging="gvScoreGrades_PageIndexChanging"
      OnRowUpdating="gvScoreGrades_RowUpdating" OnRowEditing="gvScoreGrades_RowEditing">
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
                                                    <asp:TemplateField HeaderText=" S/N">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Lowest Range">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtLowestRange" class="form-control" Text='<%# Eval("LowestRange")  %>' runat="server"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLowestRange" Text='<%# Eval("LowestRange")  %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Highest Range">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtHighestRange" class="form-control" Text='<%# Eval("HighestRange")  %>' runat="server"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHighestRange" Text='<%# Eval("HighestRange")  %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Grade">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtGrade" class="form-control" Text='<%# Eval("Grade")  %>' runat="server"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGrade" Text='<%# Eval("Grade")  %>' runat="server"></asp:Label>
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
                                                    <asp:TemplateField HeaderText="Class">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtClass" class="form-control" Text='<%# new ClassGradeLIB().RetrieveClassGrade((Int64)(Eval("ClassId"))).Name %>' runat="server" ReadOnly></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="IblClass" Text='<%# new ClassGradeLIB().RetrieveClassGrade((Int64)(Eval("ClassId"))).Name %>' runat="server"></asp:Label>
                                                            <%--<asp:Label ID="IblClass" Text='<%# Eval("ClassId") %>' runat="server"></asp:Label>--%>
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
