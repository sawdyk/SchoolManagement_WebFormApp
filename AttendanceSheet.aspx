<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master"  CodeFile="AttendanceSheet.aspx.cs" Inherits="AttendanceSheet" %>

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
                                        <h4>TAKE ATTENDANCE</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Select Date:<span class="text-danger ml-2">*</span></label>
                                                     <asp:TextBox ID="txtdate" class="form-control" placeholder="MM-dd-yyyy"   TabIndex="25"  runat="server" MaxLength="10"></asp:TextBox>
<%--                                                    <asp:Image ID="ImageButton1" runat="server" ImageUrl="Calendar_scheduleHS.png"/>--%>
                                                    <ajaxToolkit:CalendarExtender 
                                                     ID="myDateFromCal" Format="yyyy-MM-dd"  runat="server" TargetControlID="txtdate" PopupButtonID="ImageButton1" >
                                                      </ajaxToolkit:CalendarExtender>
                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Period:<span class="text-danger ml-2">*</span></label>
                                                    <asp:DropDownList ID="ddlAttendancePeriod" OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged" class="custom-select form-control"  AutoPostBack="true" runat="server"></asp:DropDownList>

                                                </div>
                                            </div>
                                                                              
                                                </form>    
                                        <br /><br />
                                 <div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>Note:Duplicate attendance will be skipped</h4>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblPeriod" Visible="false" runat="server" Text=""></asp:Label>
                                 </div>
                            <%--<asp:GridView ID="gvdAcademicSession" runat="server" AutoGenerateColumns="False" EmptyDataText=" No Calendar year currently exist" AllowPaging="true" Width="100%" ShowFooter="True" PageSize="20" GridLines="Both" OnRowCommand="gvdAcademicSession_RowCommand">       
                            <PagerSettings FirstPageText="&lt;i class=&quot;fa  fa-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-left&quot;&gt;&lt;/i&gt;"
                                LastPageText="&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;" Mode="NextPreviousFirstLast" 
                                NextPageText="&lt;i class=&quot;fa fa-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;fa fa-arrow-left&quot;&gt;&lt;/i&gt;" />--%>
                  <asp:Panel runat="server" class="table table-striped" ID="pnlMarkAttendance" Visible="false" ScrollBars="Horizontal">
                            <asp:GridView ID="gdvList" runat="server" ForeColor="#333333" class="table table-striped" AutoGenerateColumns="false" 
                                AllowPaging="false" Width="100%" GridLines="None">
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
                                    <asp:TemplateField HeaderText=" Pulpil FullName ">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%#  String.Format("{0} {1} {2}",Eval("User.LastName"),Eval("User.FirstName") ,Eval("User.MiddleName")   )  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=" Admission Number ">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAdmission" runat="server" Text='<%# Eval("User.AdmissionNumber")  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Present/Absent">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAll" OnCheckedChanged="chkAll_CheckedChanged" AutoPostBack="true" runat="server" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBox1" AutoPostBack="false" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField ShowHeader="false" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" runat="server" Text='' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

<%-- Second Gridview for both Morning and Afternoon --%>


                             <asp:GridView ID="gdvList2" runat="server" class="table table-striped" AutoGenerateColumns="false" 
                                AllowPaging="false" Width="100%" ForeColor="#333333"  GridLines="None">
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
                                    <asp:TemplateField HeaderText=" Pulpil FullName ">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%#  String.Format("{0} {1} {2}",Eval("User.LastName"),Eval("User.FirstName") ,Eval("User.MiddleName")   )  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=" Admission Number ">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAdmission" runat="server" Text='<%# Eval("User.AdmissionNumber")  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Present/Absent">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAll1" Text="Morning" OnCheckedChanged="chkAll1_CheckedChanged" AutoPostBack="true" runat="server" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBox" AutoPostBack="false" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Present/Absent">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAll2"  Text="Afternoon" OnCheckedChanged="chkAll2_CheckedChanged" AutoPostBack="true" runat="server" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBox1" AutoPostBack="false" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="false" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" runat="server" Text='' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>   
                      </asp:Panel>

                                         <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSaveAttendance" Visible="false"  class="btn btn-secondary"  OnClick="btnSaveAttendance_Click" runat="server" Text="Take Attendance"  />
                                                </div>
                                            </div>
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
            </asp:Content>


