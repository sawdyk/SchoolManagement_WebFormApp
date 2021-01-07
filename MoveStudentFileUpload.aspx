<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="MoveStudentFileUpload.aspx.cs" Inherits="MoveStudentFileUpload" %>


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
                                        <h4>MOVE STUDENT (FILE UPLOAD)</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblReport" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Upload File:<span class="text-danger ml-2">*</span></label>
                                    <asp:FileUpload ID="uploadFile" class="form-control" runat="server" />

                                                </div>
                                                </div>
                                       
                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnUpload" class="btn btn-secondary"  OnClick="btnUpload_OnClick" runat="server" Text="Save"  />
                                                </div>
                                            </div>
                                                </form>    
                                        <br /><br />
                               <%--  <asp:Label ID="lblStudents" Font-Size="Large" ForeColor="Black" runat="server" Visible="true" Text="LIST OF STUDENTS"></asp:Label>
                                                    <br />         --%>                         
                        <asp:GridView ID="gdvList" runat="server" GridLines="None" class="table table-striped" AutoGenerateColumns="true" EmptyDataText=" No result."
                            AllowPaging="true" Width="100%" ForeColor="#333333" OnPageIndexChanging="gdvList_PageIndexChanging"
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
                                <%--<asp:TemplateField HeaderText=" S/N">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                            </Columns>
                              <PagerSettings FirstPageText="&lt;i class=&quot;la  la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                                    LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                                    Mode="NextPreviousFirstLast" NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" 
                                    PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />                                                                    

                            </asp:GridView>
                                    <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Move To:<span class="text-danger ml-2">*</span></label>
                                    <asp:DropDownList ID="ddlMove" AutoPostBack="true" class="custom-select form-control" runat="server" OnSelectedIndexChanged="ddlMove_SelectedIndexChanged"></asp:DropDownList>

                                                </div>
                                                </div>
                                        <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnMoveSelectedClasses" class="btn btn-secondary"  OnClick="btnMoveSelectedClasses_Click" runat="server" Text="Move Selected Classes"  />
                                                </div>
                                            </div>
                                 </div>
                                </div>
                         
                                            </div>
                                            </div>
                                                </div>
                                            </div>      
                                </asp:Content>


