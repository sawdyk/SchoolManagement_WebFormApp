<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="StudentBulkUpload.aspx.cs" Inherits="StudentBulkUpload" %>

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
                                        <h4>UPLOAD STUDENTS INFORMATION</h4> 
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" class="text-danger ml-2"></asp:Label>
                                       <asp:Label ID="lblResult" runat="server" Text="" class="text-success ml-2"></asp:Label>

                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Campus:<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList ID="ddlCampus"  DataTextField="Name" DataValueField="Id" class="custom-select form-control" runat="server"></asp:DropDownList>
                                                </div>
                                                <div class="col-xl-6">
                                                 <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Upload File:<span class="text-danger ml-2">*</span></label>
                                                    <asp:FileUpload ID="bulkUploadFile" class="btn btn-secondary"  runat="server" />
                                                </div>
                                                    </div>
                                            </div>
                                             
                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnBulkUpload" class="btn btn-secondary" runat="server" OnClick="btnBulkUpload_OnClick" Text="Upload File"  />
                                                </div>
                                            </div>
                                                    <br />
                                   <%-- <h4>SELECTED CAMPUS:&nbsp;&nbsp;&nbsp;&nbsp; <asp:Label runat="server" ID="lblResultSchoolCampus" ForeColor="Green"> </asp:Label> </h4>
                                  <h4>TOTAL NUMBER OF STUDENTS: <asp:Label runat="server" ID="lblTotalNumberofStudents" ForeColor="Green">> </asp:Label></h4>--%>
                                  
                               <h6>Successful:</h6>
                                                <asp:GridView ID="GridView1" EmptyDataText="No record on the list" Width="100%" runat="server" AutoGenerateColumns="false">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="S/N">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="New Name" HeaderText="New Name" />
                                                        <asp:BoundField DataField="Existing Name(s)" HeaderText="Existing Name(s)" />
                                                        <asp:BoundField DataField="Reason" HeaderText="Reason" />
                                                    </Columns>
                                                </asp:GridView>
                                            <%--</div>
                                        </section>--%>
                                        <%--<section class="row" style="margin-bottom: 20px">
                                            <div class="col-xs-12 col-sm-12">--%>
                                                <h6>Unsuccessful:</h6>
                                                <asp:GridView ID="GridView2" EmptyDataText="No record on the list" Width="100%" runat="server" AutoGenerateColumns="false">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="S/N">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="New Name" HeaderText="New Name" />
                                                        <asp:BoundField DataField="Existing Name(s)" HeaderText="Existing Name(s)" />
                                                        <asp:BoundField DataField="Reason" HeaderText="Reason" />
                                                    </Columns>
                                                </asp:GridView>
                      
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        
                                        </form>

                                 </div>
                                </div>
                                </asp:Content>

