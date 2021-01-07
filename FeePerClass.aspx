<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="FeePerClass.aspx.cs" Inherits="FeePerClass" %>

    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .style1 {
            width: 255px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">   
    
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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
                        <div class="row flex-row">
                            <div class="col-12">
                                <!-- Form -->
                                <div class="widget has-shadow">
                                    <div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>CREATE FEE PER CLASS,SESSION AND TERM</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" class="text-danger ml-2"></asp:Label>
                                          <asp:Label ID="lblResultMsg" runat="server" Text="" class="text-success ml-2"></asp:Label>

                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Class:<span class="text-danger ml-2">*</span></label>
                                            <asp:DropDownList runat="server" ID="ddlClass" class="custom-select form-control" DataTextField="Name" DataValueField="Id" AutoPostBack="true">
                                            </asp:DropDownList>
                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Template:<span class="text-danger ml-2">*</span></label>
                                                    <asp:DropDownList runat="server" DataTextField="TemplateName" class="custom-select form-control" DataValueField="Id" AutoPostBack="true" ID="ddlTemplate" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlTemplate_SelectedIndexChanged">
                                                     </asp:DropDownList>
                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Session:<span class="text-danger ml-2">*</span></label>
                                            <asp:DropDownList runat="server" class="custom-select form-control" ID="ddlSession">
                                             </asp:DropDownList>                                                      
                                                </div>
                                               <div class="col-xl-6">
                                                 <label class="form-control-label">Term:<span class="text-danger ml-2">*</span></label>
                                            <asp:DropDownList runat="server" class="custom-select form-control" ID="ddlTerm">
                                             </asp:DropDownList>                                                        
                                                     </div>
                                                      </div>
                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSave" OnClick="btnSave_OnClick" class="btn btn-secondary" runat="server" Text="Save"  />
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
                                                                     <asp:Label ID="lblfeetemplateEdit" ForeColor="Black" runat="server" Font-Size="Large" Visible="false" Text="FEE TEMPLATE LIST"></asp:Label>
                                        <asp:GridView ID="gvFeeTemplate" EmptyDataText="No Fee Type" class="table table-striped" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" 
                                                    CellPadding="4" ForeColor="#333333" GridLines="None" OnPageIndexChanging="gvScore_PageIndexChanging" OnRowCancelingEdit="gvCampus_RowCancelingEdit"
                                                    OnRowUpdating="gvCampus_RowUpdating" OnRowEditing="gvCampus_RowEditing" OnRowDataBound="gvFeeType_RowDataBound">
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <Columns>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFeeTypeId" Text='<%# Eval("FeeTypeId")  %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Fee Type">
                                                            <EditItemTemplate>
                                                                <asp:DropDownList ID="gvddlFeeType1" class="custom-select form-control" runat="server"></asp:DropDownList>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFeeType" Text='<%# Eval("feeType")%>' runat="server"></asp:Label>
                                                                <%--<asp:Label ID="Label2" Text='<%# Eval("FeeName")  %>' runat="server"></asp:Label>--%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblMand" Text='<%# Eval("mandatory")  %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mandatory">
                                                            <EditItemTemplate>
                                                                <asp:DropDownList ID="ddlMand1" runat="server"></asp:DropDownList>

                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlMand" class="custom-select form-control" runat="server">
                                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <%--<asp:Label ID="gvlblMand" Text= '<%#Mandatory(Convert.ToInt16( Eval("Mandatory") )) %>' runat="server"></asp:Label>--%>
                                                                <%--<asp:Label ID="IblClass" Text='<%# Eval("ClassId") %>' runat="server"></asp:Label>--%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Amount">
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="gvtxtAmount1"  runat="server" Text='<%# Eval("Amount") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="gvtxtAmount" class="form-control" runat="server" Text='<%# Eval("Amount") %>'></asp:TextBox>
                                                                <%--<asp:Label ID="Label22" Text='<%# Eval("Amount")  %>' runat="server"></asp:Label>--%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ID" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblId" Text='<%# Eval("Id")  %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="TemplateID" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTemplateId" Text='<%# Eval("TemplateId")  %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ShowHeader="False">
                                                            <EditItemTemplate>
                                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("Id")  %>' CausesValidation="True" CommandName="Update" Text="Update" ToolTip="Update"><i class="la la-floppy-o la-2x"></i></asp:LinkButton>
                                                                &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CommandArgument='<%# Eval("Id")  %>' CausesValidation="False" CommandName="Cancel" Text="Cancel" ToolTip="Cancel"><i class="la la-remove la-2x"></i></asp:LinkButton>
                                                            </EditItemTemplate>
                                                            <%--<ItemTemplate>
                                                                <asp:LinkButton ID="LinkButton1" CommandArgument='<%# Eval("Id")  %>' runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" ToolTip="Edit"><i class="fa fa-edit fa-2x"></i></asp:LinkButton>
                                                            </ItemTemplate>--%>
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
                                LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" Mode="NextPreviousFirstLast" 
                                NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />
                                                            </asp:GridView>

                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
                        
                                </asp:Content>

