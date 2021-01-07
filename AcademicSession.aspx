<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="AcademicSession.aspx.cs" Inherits="AcademicSession" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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
                        <div class="row flex-row">
                            <div class="col-12">
                                <!-- Form -->
                                <div class="widget has-shadow">
                                    <div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>CREATE AND SET ACADEMIC SESSION/TERM</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" class="text-danger ml-2"></asp:Label>
                                          <asp:Label ID="lblResultMsg" runat="server" Text="" class="text-success ml-2"></asp:Label>

                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Academic Session:<span class="text-danger ml-2">*</span></label>
                                                    <asp:DropDownList ID="ddlAcademicSession" DataTextField="SessionName" DataValueField="ID" class="custom-select form-control" runat="server"></asp:DropDownList>

                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Academic Term:<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList ID="ddlAcademicTerm" DataTextField="AcademicTermName" DataValueField="id" class="custom-select form-control" runat="server"></asp:DropDownList>

                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Starting Date:<span class="text-danger ml-2">*</span></label>
                                                   <asp:TextBox ID="txtDateStart" placeholder="MM-dd-yyyy" class="form-control" TabIndex="25"  runat="server" MaxLength="10"></asp:TextBox>
                                               <ajaxToolKit:CalendarExtender ID="myDateFromCal" Format="MM-dd-yyyy"  runat="server"  TargetControlID="txtDateStart" PopupButtonID="ImageButton1" PopupPosition="BottomLeft"></ajaxToolKit:CalendarExtender>
                                                       
                                                </div>
                                               <div class="col-xl-6">
                                                 <label class="form-control-label">Ending Date:<span class="text-danger ml-2">*</span></label>
                                                    <asp:TextBox ID="txtDateEnd" placeholder="MM-dd-yyyy"  class="form-control" TabIndex="25"  runat="server" MaxLength="10"></asp:TextBox>
                                                   <ajaxToolKit:CalendarExtender ID="CalendarExtender1" Format="MM-dd-yyyy"  runat="server"  TargetControlID="txtDateEnd" PopupButtonID="ImageButton1" PopupPosition="BottomLeft">
                                                                            </ajaxToolKit:CalendarExtender>                                                   

                                                     </div>
                                                      </div>
                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSaveAcademicSession" OnClick="btnSaveAcademicSession_OnClick" class="btn btn-secondary" runat="server" Text="Save Academic Session"  />
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
                                                          <asp:Label ID="lblAcademic" Font-Size="Large" runat="server" Visible="true" ForeColor="Black" Text="List of Academic Session/Term"></asp:Label>
                                        <asp:GridView ID="gvdAcademicSession" EmptyDataText="No Calendar year currently exist" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" ShowFooter="True" class="table table-striped"
                                                 CellPadding="4" ForeColor="#333333" GridLines="None" PageSize="10" OnRowCommand="gvdAcademicSession_RowCommand" OnPageIndexChanging="gvdAcademicSession_PageIndexChanging" >
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
                                                                    <asp:TemplateField HeaderText="Academic Session">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("AcademicSessionName.SessionName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="AcademicTermName" HeaderText="Academic Term" />
                                                                    <asp:BoundField DataField="DateStart" HeaderText="Starting Date" />
                                                                    <asp:BoundField DataField="DateEnd" HeaderText="Ending Date" />
<%--                                                                    <asp:TemplateField HeaderText=" Academic Session ">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("AcademicSessionName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText=" Academic Term ">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("AcademicTermName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText=" Starting Date ">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCode" runat="server" Text='<%# Eval("DateStart" , "{0:dd-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText=" Ending Date ">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCode" runat="server" Text='<%# Eval("DateEnd" , "{0:dd-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>--%>
                                                                    
                                                                    <asp:TemplateField HeaderText="Set Current Term" ShowHeader="false">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="btnSetCurrentTerm" runat="server" Text="Set as Current" CommandName="CURRENTTERM" ToolTip="Set as Current Term" CommandArgument='<%# Eval("id") %>'><i class="ti-check-box"></i></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                      <asp:TemplateField HeaderText="Open Term" ShowHeader="false">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="btnOpenTerm" runat="server" Text="Open Term"  OnClientClick="javascript:return confirm('Are you sure you want to Open this Term?');" CommandName="OPENTERM" ToolTip="Open Term" CommandArgument='<%# Eval("id") %>'><i class="ti-check-box"></i></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                      <asp:TemplateField HeaderText="Lock Term" ShowHeader="false">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="btnLockTerm" runat="server" Text="Lock Term" OnClientClick="javascript:return confirm('Are you sure you want to Lock this Term?');" CommandName="LOCKTERM" ToolTip="Lock Term" CommandArgument='<%# Eval("id") %>'><i class="ti-check-box"></i></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                     <asp:TemplateField HeaderText="Close Term" ShowHeader="false">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="btnCloseTerm" runat="server" Text="Close Term" OnClientClick="javascript:return confirm('Are you sure you want to Close this Term?');" CommandName="CLOSETERM" ToolTip="Close Term" CommandArgument='<%# Eval("id") %>'><i class="ti-check-box"></i></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                     <PagerSettings FirstPageText="&lt;i class=&quot;la  la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;"
                                LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" Mode="NextPreviousFirstLast" 
                                NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />
                                                            </asp:GridView>

                                        <br /><br />

                                         <asp:Label ID="Label1" Font-Size="Large" runat="server" Visible="true" ForeColor="Black" Text="Academic Session/Term Status"></asp:Label>
<%--                                            <asp:Panel ID="Panel2" Width="100%" class="table table-striped" ScrollBars="Vertical" runat="server">--%>
                                        <asp:GridView ID="gvdAcademicSession2" EmptyDataText="No Calendar year currently exist" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" ShowFooter="True" class="table table-striped"
                                                 CellPadding="4" ForeColor="#333333" GridLines="None" PageSize="10" OnPageIndexChanging="gvdAcademicSession2_PageIndexChanging" >
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
                                                                    <asp:TemplateField HeaderText=" Academic Session ">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("SessionName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText=" Academic Term ">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTermName" runat="server" Text='<%# Eval("AcademicTermName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText=" Starting Date ">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblStart" runat="server" Text='<%# Eval("DateStart" , "{0:dd-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText=" Ending Date ">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEnd" runat="server" Text='<%# Eval("DateEnd" , "{0:dd-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Current Term">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCurent" runat="server" Text='<%#checkTorF(Convert.ToString(Eval("IsCurrent"))) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Opened">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblOpened" runat="server" Text='<%# checkTorF(Convert.ToString(Eval("IsOpened"))) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Locked">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLocked" runat="server" Text='<%# checkTorF(Convert.ToString(Eval("IsLocked"))) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                     <asp:TemplateField HeaderText="Closed">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblClosed" runat="server" Text='<%# checkTorF(Convert.ToString(Eval("IsClosed"))) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                     <PagerSettings FirstPageText="&lt;i class=&quot;la  la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;"
                                LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" Mode="NextPreviousFirstLast" 
                                NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />
                                                            </asp:GridView>
<%--                                                </asp:Panel>--%>
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
                        
                                </asp:Content>
