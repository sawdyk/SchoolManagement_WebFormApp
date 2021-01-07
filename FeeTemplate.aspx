<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="FeeTemplate.aspx.cs" Inherits="FeeTemplate" %>


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
            <div class="row flex-row" style="width: auto;">
                <div class="col-12">
                    <!-- Form -->
                    <div class="widget has-shadow">
                        <div class="widget-header bordered no-actions d-flex align-items-center">
                            <h4>CREATE FEE TEMPLATE</h4>
                        </div>
                        <div class="widget-body">
                            <form class="form-horizontal">

                                <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                <div class="form-group row mb-3">
                                    <div class="col-xl-6 mb-3">
                                        <label class="form-control-label">Template Name:<span class="text-danger ml-2">*</span></label>
                                        <asp:TextBox ID="txtTemplateName" class="form-control" runat="server"></asp:TextBox>

                                    </div>
                                </div>


                            </form>
                          
                             <div class="widget-header bordered no-actions d-flex align-items-center">
                                       <asp:Label ID="lblsubjectResp" Font-Size="Large" ForeColor="Black" runat="server" Visible="false" Text="FEE TEMPLATE LIST"></asp:Label>
                                 </div>
                         
                            <asp:GridView ID="GvFeeTemplate" class="table table-striped" Width="100%" EmptyDataText="No Fee Type" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CssClass="auto-style4"
                                CellPadding="4" ForeColor="#333333" GridLines="None" OnPageIndexChanging="gvScore_PageIndexChanging" OnRowCancelingEdit="gvCampus_RowCancelingEdit"
                                OnRowUpdating="gvCampus_RowUpdating" OnRowEditing="gvCampus_RowEditing" OnRowDataBound="GvFeeTemplate_RowDataBound">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Fee Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFeeName" Text='<%# Eval("FeeName")  %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount NGN">
                                        <ItemTemplate>
                                            <asp:TextBox ID="gvtxtAmount" runat="server" class="form-control" placeholder="0.00" onkeydown="return (!(event.keyCode>=65) && event.keyCode!=32);" DataFormatString="{0:0,0.00}"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mandatory">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlMand" class="custom-select form-control" runat="server">
                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" Text='<%# Eval("Id")  %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
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
                     <SortedDescendingHeaderStyle BackColor="#4870BE" /> 
                            <PagerSettings FirstPageText="&lt;i class=&quot;la  la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                                    LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                                    Mode="NextPreviousFirstLast" NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" 
                                    PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />                                                    

                            </asp:GridView>


                             <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSave" class="btn btn-secondary"  OnClick="btnSave_OnClick" runat="server" Text="Save"  />
                                                </div>
                                            </div>
                                 <br />
                                 <div class="widget-header bordered no-actions d-flex align-items-center">
                                       <asp:Label ID="lblfeetemplateEdit" ForeColor="Black" runat="server" Font-Size="Large" Visible="false" Text="EDIT FEE TEMPLATE NAME"></asp:Label>
                                 </div>
                                 <asp:GridView ID="gvFeeType" Width="100%" class="table table-striped" EmptyDataText="No Fee Type" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CssClass="auto-style4"
                                CellPadding="3" ForeColor="#333333" GridLines="None" OnPageIndexChanging="gvScore_PageIndexChanging" OnRowCancelingEdit="gvCampus_RowCancelingEdit"
                                OnRowUpdating="gvCampus_RowUpdating" OnRowEditing="gvCampus_RowEditing" OnRowDataBound="gvFeeType_RowDataBound">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Fee Template">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="gvtxtTemplateName" class="form-control" Text='<%# Eval("TemplateName")  %>' runat="server"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label4" Text='<%# Eval("TemplateName")  %>' runat="server"></asp:Label>
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
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div class="modal fade" id='<%# "mymodal" + Eval("Id") %>'>
                                                <div class="modal-dialog" style="width: 60%">
                                                    <div class="modal-content">
                                                        <div class="modal-header">
                                                            <button type="button" class="close" data-dismiss="modal">
                                                                <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                                            <h4 class="modal-title">Template Details</h4>
                                                        </div>
                                                        <div class="modal-body">
                                                                        <asp:GridView ID="GridView2" runat="server"
                                                                                        AutoGenerateColumns="false" ShowFooter="true" ForeColor="black" class="table table-striped" PageSize="100000" GridLines="None" DataSource='<%# Eval("templateList") %>'>
                                                                                        <PagerStyle HorizontalAlign="Right" />
                                                                                        <Columns>

                                                                                            <asp:TemplateField HeaderText="Fee Type">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="Label1" runat="server" Text='<%# FeeType(Convert.ToInt16(Eval("FeeTypeId")))%>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Amount">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="Label12" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Mandatory">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="Label13" runat="server" Text='<%# Mandatory(Convert.ToInt16(Eval("Mandatory"))) %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                           
                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                        </div>

                                                                </div>
                                                                <!-- /.modal-content -->
                                                            </div>
                                                            <!-- /.modal-dialog -->
                                                        </div>
                                                                        <asp:LinkButton ID="targetbtn" runat="server" ToolTip="View Details" data-toggle="modal" data-target='<%# "#mymodal" + Eval("ID") %>' CssClass="input-group input-group-lg"><i class="la la-eye"></i></asp:LinkButton>
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
                                                <SortedDescendingHeaderStyle BackColor="#4870BE" /> 
                            <PagerSettings FirstPageText="&lt;i class=&quot;la  la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                                    LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                                    Mode="NextPreviousFirstLast" NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" 
                                    PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />                                                                    

                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
</div>
                                </div>
                         
                                </asp:Content>


