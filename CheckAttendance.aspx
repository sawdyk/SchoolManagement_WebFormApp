<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="CheckAttendance.aspx.cs" Inherits="CheckAttendance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
   
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
                                        <h4>VIEW STUDENT'S ATTENDANCE RECORD</h4>&nbsp;&nbsp;&nbsp;&nbsp;
<%--                                      <asp:Label runat="server" Text="(NB:Click on 'Get total' to get total amount to be paid before generating invoice!)" ForeColor="Red"></asp:Label>  --%>
                                    </div>
                                    <div class="widget-body">
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" Visible="false" class="text-danger ml-2"></asp:Label>
                                           
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Child:<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList ID="ddlWard" runat="server"  class="custom-select form-control" DataTextField="FullName" DataValueField="AdmissionNumber"
                                                    AppendDataBoundItems="true" AutoPostBack="false">
                                                    <asp:ListItem Enabled="true" Text="--Select ward--" Value="0"></asp:ListItem>
                                                </asp:DropDownList>                                         
                                                </div>
                                                
                                            </div>
                                                 <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <asp:label runat="server" ID="lblAdmissionNo" Text="" class="form-control-label"><span class="text-danger ml-2"></span></asp:label>
                                                </div>
                                                <div class="col-xl-6">
                                                    <asp:label runat="server" ID="lblNoOfPresent"  Text="" class="form-control-label"><span class="text-danger ml-2"></span></asp:label>
                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                          <asp:label runat="server" ID="lblNoOfRecordsSelected"  Text="" class="form-control-label"><span class="text-danger ml-2"></span></asp:label>
                                                </div>
                                                <div class="col-xl-6">                                          
                                        <asp:label runat="server" ID="lblNoOfAbsent"  Text="" class="form-control-label"><span class="text-danger ml-2"></span></asp:label>
                                                </div>
                                            </div>
                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnViewAttendance" class="btn btn-secondary" runat="server" Text="View Attendance Record"  OnClick="btnViewAttendance_Click" />
                                                </div>
                                            </div>
                                                    
                                        <br /><br />
                                 <%--<div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>CLASS ARMS</h4>
                                 </div>--%>
                                               <asp:Label ID="lblStudents" Font-Size="Large" runat="server" Visible="false" ForeColor="Black" Text="STUDENT'S ATTENDANCE RECORD"></asp:Label>                    
                                                    <asp:Label ID="lblLists" ForeColor="Black" runat="server" Visible="false"></asp:Label>
                       <asp:Panel runat="server" ID="pnlMarkAttendance" Visible="true" ScrollBars="Horizontal">
                                <asp:GridView ID="gdvList" runat="server" ForeColor="Black" GridLines="None" AutoGenerateColumns="false" class="table table-striped" EmptyDataText=" No record Currently exist "
                    AllowPaging="true" Width="100%" PageSize="30">
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
                                            <asp:TemplateField HeaderText="Date(mm/dd/yyyy)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAttendanceDate" runat="server" Text='<%# Eval("AttendanceDate")  %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                                                                        
                                            <%--<asp:TemplateField HeaderText="Present/Absent">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAttendanceStatus" runat="server" Text='<%# Eval("AttendanceStatus")  %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Morning">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAttendanceMorning" runat="server" Text='<%# getAttendanceStatus(Convert.ToInt16(Eval("AttendancePeriodIDMorning")))  %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Afternoon">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAttendanceAfternoon" runat="server" Text='<%# getAttendanceStatus(Convert.ToInt16(Eval("AttendancePeriodIDAfternoon")))  %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                                                                        
                                            <asp:TemplateField ShowHeader="false" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblId" runat="server" Text='' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                    <PagerSettings FirstPageText="&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                        LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                        Mode="NextPreviousFirstLast" 
                        NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" 
                        PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />                  

                                </asp:GridView>
                           </asp:Panel>
                                            
                                            </div>
                                            </div>
                                            </form>
                                                </div>
                                            </div>                                        
                                        </form>

                                 </div>
                                </div>    
                                </asp:Content>
