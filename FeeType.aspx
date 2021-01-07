<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="FeeType.aspx.cs" Inherits="FeeType" %>


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
                                        <h4>CREATE FEE TYPE</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Fee Category:<span class="text-danger ml-2">*</span></label>
                                    <asp:DropDownList runat="server" ID="ddlCategory"  class="custom-select form-control">
                                    </asp:DropDownList>                                              
                                                </div>
                                                <div class="col-xl-6">
                                     <label class="form-control-label">Fee Name:<span class="text-danger ml-2">*</span></label>
                                     <asp:TextBox ID="txtFeeName" class="form-control" runat="server"></asp:TextBox>

                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                       <label class="form-control-label">Fee Code:<span class="text-danger ml-2">*</span></label>
                                         <asp:TextBox ID="txtFeeCode" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                                  
                                               </div>
                                            
                                            <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSave" class="btn btn-secondary"  OnClick="btnSave_OnClick" runat="server" Text="Save"  />
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
                                    <asp:Label ID="lblSummary" Font-Size="Large" ForeColor="Black" runat="server" Visible="true" Text="LIST OF FEE TYPES"></asp:Label>
          
                                        <asp:Panel ID="Panel2" Width="1000%" class="table table-striped" runat="server">
                                        <asp:GridView ID="gvFeeType" EmptyDataText="No Fee Type"  Width="100%" class="table table-striped"  ForeColor="#333333" GridLines="None" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CssClass="auto-style4"
                                                 CellPadding="3" OnPageIndexChanging="gvScore_PageIndexChanging" OnRowCancelingEdit="gvCampus_RowCancelingEdit" 
                                                OnRowUpdating="gvCampus_RowUpdating" OnRowEditing="gvCampus_RowEditing" OnRowDataBound="gvFeeType_RowDataBound">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Fee Name">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="gvtxtFeeName"  class="form-control" Text='<%# Eval("FeeName")  %>' runat="server"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label4" Text='<%# Eval("FeeName")  %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Fee Category">
                                                        <EditItemTemplate>
                                                            <asp:DropDownList ID="gvddllFeeCategory" class="custom-select form-control" runat="server"></asp:DropDownList>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label2" Text='<%# Eval("CategoryName")  %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Fee Code">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="gvtxtFeeCode" class="form-control"  Text= '<%# Eval("FeeCode")  %>' runat="server"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="IblClass" Text='<%# Eval("FeeCode")  %>' runat="server"></asp:Label>
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
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("Id")  %>' CausesValidation="True" CommandName="Update" Text="Update" ToolTip="Update"><i class="la la-floppy-o la-2x"></i></asp:LinkButton>
                                                            &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CommandArgument='<%# Eval("Id")  %>' CausesValidation="False" CommandName="Cancel" Text="Cancel" ToolTip="Cancel"><i class="la la-remove la-2x"></i></asp:LinkButton>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButton1" CommandArgument='<%# Eval("Id")  %>' runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" ToolTip="Edit"><i class="la la-edit la-2x"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
        <%--                                        <EditRowStyle BackColor="#2461BF" />--%>
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

