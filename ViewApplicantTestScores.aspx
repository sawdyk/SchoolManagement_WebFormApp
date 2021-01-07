<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="ViewApplicantTestScores.aspx.cs" Inherits="ViewApplicantTestScores" %>

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
                                        <h4>ADMISSION APPLICANTS TEST SCORE LIST</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">
                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" ></asp:Label>
                                          <asp:Label ID="lblResultMsg" runat="server" Text=""></asp:Label>

                                           
               <asp:GridView ID="gdvList" runat="server" ForeColor="Black" class="table table-striped" AutoGenerateColumns="false" GridLines="None" EmptyDataText=" No record Currently exist "
                            AllowPaging="true" Width="100%" OnPageIndexChanging="gdvList_PageIndexChanging" PageSize="30">
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
                                <asp:TemplateField HeaderText=" Pupil FullName ">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# string.Concat( Eval("AdmissionApp.FirstName")," ", Eval("AdmissionApp.LastName"))%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Application Number">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("ApplicantId")  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Test Score">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("TestScore")  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText=" Gender ">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("AdmissionApp.Gender")  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <div class="modal fade" id='<%# "mymodal" + Eval("Id") %>'>
                                             <div class="modal-dialog" style="width:100%">
                                                <div class="modal-content" style="width:1000px" >
                                                    <div class="modal-header">
                                                        <button type="button" class="close" data-dismiss="modal">
                                                            <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                                        <h4 class="modal-title">Application Details</h4>
                                                    </div>
                                                    <div class="modal-body">
                                                        <table class="table table-striped table-hover" style="color:black">
<%--                                                            <tr>
                                                                <td colspan="4" align="right">
                                                                    <asp:Image Width="25%" Height="25%" ImageUrl='<%# Eval("AdmissionApp.PassportFileName") %>' runat="server"></asp:Image>
                                                                </td>
                                                            </tr>--%>
                                                            <tr>
                                                                <td><strong>Firstname:</strong></td>
                                                                <td><%# Eval("AdmissionApp.FirstName") %></td>
                                                                <td><strong>Lastname:</strong></td>
                                                                <td><%# Eval("AdmissionApp.Lastname") %></td>
                                                            </tr>
                                                            <tr>
                                                                <td><strong>Middle Name:</strong></td>
                                                                <td><%# Eval("AdmissionApp.MiddleName") %></td>
                                                                <td><strong>Gender:</strong></td>
                                                                <td><%# Eval("AdmissionApp.Gender") %></td>
                                                            </tr>
                                                            <tr>
                                                                <td><strong>Admission Session:</strong></td>
                                                                <td>&nbsp;</td>
                                                                <td><strong>Date of Birth:</strong></td>
                                                                <td><%# Eval("AdmissionApp.DateOfBirth","{0:dd, MMMM yyyy}") %></td>
                                                            </tr>
                                                            <tr>
                                                                <td><strong>Last School Attended:</strong></td>
                                                                <td><%# Eval("AdmissionApp.LastSchoolAttended") %></td>
                                                                <td><strong>Last School Address:</strong></td>
                                                                <td><%# Eval("AdmissionApp.LastSchoolAttendedAddress") %></td>
                                                            </tr>
                                                            <tr>
                                                                <td><strong>Last School Class:</strong></td>
                                                                <td><%# Eval("AdmissionApp.LastSchoolAttendedClass") %></td>
                                                                <td><strong>Last School Year:</strong></td>
                                                                <td><%# Eval("AdmissionApp.LastSchoolAttendedYear") %></td>
                                                            </tr>
                                                            <tr>
                                                                <td><strong>Any Disability:</strong></td>
                                                                <td><%# Eval("AdmissionApp.Disability") %></td>
                                                                <td><strong></strong></td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td><strong>Guardian's Name:</strong></td>
                                                                <td><%# Eval("AdmissionApp.GuardianName") %></td>
                                                                <td><strong>Guardian's Address:</strong></td>
                                                                <td><%# Eval("AdmissionApp.GuardianAddress") %></td>
                                                            </tr>
                                                            <tr>
                                                                <td><strong>Guardian's Phone:</strong></td>
                                                                <td><%# Eval("AdmissionApp.GuardianPhoneNumber") %></td>
                                                                <td><strong>Guardian's Email:</strong></td>
                                                                <td><%# Eval("AdmissionApp.GuardianEmail") %></td>
                                                            </tr>
                                                            <tr>
                                                                <td><strong>Relationship:</strong></td>
                                                                <td><%# Eval("AdmissionApp.GuardianRelationship") %></td>
                                                                <td><strong>Occupation:</strong></td>
                                                                <td><%# Eval("AdmissionApp.Occupation") %></td>
                                                            </tr>
                                                            <tr>
                                                                <td><strong>Place of Work:</strong></td>
                                                                <td><%# Eval("AdmissionApp.PlaceOfWork") %></td>
                                                                <td><strong>Test Score:</strong></td>
                                                                <td><%# Eval("TestScore") %></td>
                                                            </tr>
                                                            <tr>
                                                                <td><strong>Admission Status:</strong></td>
                                                                <td><%# Eval("AdmissionStatus") %></td>
                                                                <td><strong>Application Date:</strong></td>
                                                                <td><%# Eval("DateApplied","{0:dd, MMMM yyyy}") %></td>
                                                            </tr>
<%--                                                            <tr>
                                                                <td><strong>Payment Status:</strong></td>
                                                                <td><%# Eval("PaymentStatus") %></td>
                                                                <td><strong>Payment Date:</strong></td>
                                                                <td><%# Eval("PaymentDate") %></td>
                                                            </tr>--%>
                                                        </table>
                                                    </div>

                                                </div>
                                                <!-- /.modal-content -->
                                            </div>
                                            <!-- /.modal-dialog -->
                                        </div>
                                        <!-- /.modal -->

                                        <asp:LinkButton ID="targetbtn" runat="server" ToolTip="View Application Details" data-toggle="modal" data-target='<%# "#mymodal" + Eval("Id") %>' CssClass="input-group input-group-lg"><i class="la la-search"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id")  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                      <PagerSettings FirstPageText="&lt;i class=&quot;la  la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" Mode="NextPreviousFirstLast" NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />
                        </asp:GridView>
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        
                                </asp:Content>


