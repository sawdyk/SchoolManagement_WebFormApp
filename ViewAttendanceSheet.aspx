<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="ViewAttendanceSheet.aspx.cs" Inherits="ViewAttendanceSheet" %>

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
                                        <h4>VIEW STUDENTS ATTENDANCE RECORD</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Select Date:<span class="text-danger ml-2">*</span></label>
                                                    <asp:TextBox ID="txtAttendanceDate" class="form-control" placeholder="MM-dd-yyyy"   TabIndex="25"  runat="server" MaxLength="10"></asp:TextBox>
<%--                                                    <asp:Image ID="ImageButton1" runat="server" ImageUrl="Calendar_scheduleHS.png"/>--%>
                                                    <ajaxToolkit:CalendarExtender 
                                                     ID="myDateFromCal" Format="yyyy-MM-dd"  runat="server" TargetControlID="txtAttendanceDate" PopupButtonID="ImageButton1" >
                                                      </ajaxToolkit:CalendarExtender>
                                                </div>
                                                
                                            </div>
                                                     <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnViewAttendance" Visible="true"  class="btn btn-secondary"  OnClick="btnViewAttendance_Click" runat="server" Text="View Attendance"  />
                                                </div>
                                            </div>
                                            </div>                           
                                                </form>    
                                        <br /><br />
                                 <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Selected Date:<span class="text-danger ml-2">*</span></label>
                                                 <asp:label class="form-control-label" ID="lblDate" runat="server" Text="" ></asp:label>
                                                </div>
                                                  <div class="col-xl-6">
                                                    <label class="form-control-label">Total Selected Records:<span class="text-danger ml-2">*</span></label>
                                                        <asp:label class="form-control-label" ID="lblNoOfRecordsSelected" runat="server" Text="" ></asp:label>
                                                </div>
                                            </div>
                                        <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">No of Present:<span class="text-danger ml-2">*</span></label>
                                                 <asp:label class="form-control-label" ID="lblNoOfPresent" runat="server" Text="" ></asp:label>
                                                </div>
                                                  <div class="col-xl-6">
                                                    <label class="form-control-label">No of Absent:<span class="text-danger ml-2">*</span></label>
                                                        <asp:label class="form-control-label" ID="lblNoOfAbsent" runat="server" Text="" ></asp:label>
                                                </div>
                                            </div>
                            <%--<asp:GridView ID="gvdAcademicSession" runat="server" AutoGenerateColumns="False" EmptyDataText=" No Calendar year currently exist" AllowPaging="true" Width="100%" ShowFooter="True" PageSize="20" GridLines="Both" OnRowCommand="gvdAcademicSession_RowCommand">       
                            <PagerSettings FirstPageText="&lt;i class=&quot;fa  fa-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-left&quot;&gt;&lt;/i&gt;"
                                LastPageText="&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;" Mode="NextPreviousFirstLast" 
                                NextPageText="&lt;i class=&quot;fa fa-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;fa fa-arrow-left&quot;&gt;&lt;/i&gt;" />--%>
                                          <asp:Label ID="lblStudents" Font-Size="Large" runat="server" Visible="false" ForeColor="Black" Text="LIST OF STUDENTS ATTENDANCE"></asp:Label>             
 
                                        <asp:Panel runat="server" class="table table-striped" ID="pnlMarkAttendance" Visible="false" ScrollBars="Horizontal">
                             <asp:GridView ID="gdvList" runat="server"  ForeColor="#333333" AutoGenerateColumns="false" EmptyDataText=" No Student on the list"
                                AllowPaging="false" Width="100%" class="table table-striped"  GridLines="None">
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
                                            <asp:Label ID="lblName" runat="server" Text='<%#  Eval("Fullname")  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=" Admission Number ">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAdmission" runat="server" Text='<%# Eval("AdmissionNo")  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Morning">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAttendanceStatusMorning" runat="server" Text='<%# getAttendanceStatus(Convert.ToInt16(Eval("AttendancePeriodIDMorning"))) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Afternoon">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAttendanceStatusAfternoon" runat="server" Text='<%# getAttendanceStatus(Convert.ToInt16(Eval("AttendancePeriodIDAfternoon"))) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Approval Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAttendanceStatus" runat="server" Text='<%# Eval("SupervisorApproval") %>'></asp:Label>
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

                                       
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
            </asp:Content>


