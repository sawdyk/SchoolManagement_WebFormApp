<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="CreateClassArms.aspx.cs" Inherits="CreateClassArms" %>

<%@ Import Namespace="PASSIS.DAO" %>

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
                                        <h4>CREATE CLASS ARMS AND ASSIGN TEACHERS</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" class="text-success ml-2"></asp:Label>
                                           
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Name:<span class="text-danger ml-2">*</span></label>
                                                    <asp:TextBox ID="txtGradeName" placeholder="Arm Name" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Teacher:<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList ID="ddlTeacher" DataTextField="LastName" DataValueField="Id" class="custom-select form-control" runat="server"></asp:DropDownList>
                                                </div>
                                            </div>
                                               <div class="col-xl-6">
                                                 <label class="form-control-label">Class<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList ID="ddlClass" OnSelectedIndexChanged="ddlClass_SelectedIndexChanged" AutoPostBack="true" DataTextField="Name" DataValueField="Id" class="custom-select form-control" runat="server"></asp:DropDownList>
                                                   </div>
                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="Button2" class="btn btn-secondary" runat="server" Text="Save New Arm"  OnClick="btnSave_OnClick" />
                                                </div>
                                            </div>
                                               </div>     
                                        <br /><br />
                                 <%--<div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>CLASS ARMS</h4>
                                 </div>--%>
  <asp:Label ID="lblLists" ForeColor="Black" runat="server" Visible="false" Text="LIST OF CLASSES AND ASSIGNED TEACHER/SUPERVISOR"></asp:Label>
                            <asp:GridView ID="gdvList" runat="server" GridLines="None" AutoGenerateColumns="false" class="table table-striped" EmptyDataText="No Result" 
                                AllowPaging="false" ForeColor="#333333"  Width="100%" Height="150px"  PageSize="20" OnPageIndexChanging="gdvList_PageIndexChanging"
                                OnRowCancelingEdit="GridView1_RowCancelingEdit"
                                OnRowEditing="GridView1_RowEditing" ShowFooter="false" OnRowUpdating="GridView1_RowUpdating" OnRowDataBound="GridView1_RowDataBound">
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
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText=" S/N">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=" Grade Name  ">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGradeName" runat="server" Text='<%#  Eval("GradeName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtGradeName" class="form-control" runat="server" Text='<%# Eval("GradeName")%>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText=" Grade Code  ">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("GradeCode")  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText=" Campus ">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName34445" runat="server" Text='<%# new AcademicSessionDAL().RetrieveSchoolCampus(Convert.ToInt64(Eval("SchoolCampusId"))).Name %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGradeTeacherId" runat="server" Text='<%# Eval("GradeTeacherId")  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGradeSupervisorId" runat="server" Text='<%# Eval("GradeSupervisorId")  %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=" Class Teacher ">
                                        <ItemTemplate>
                                            <asp:Label ID="lblhomeroomTeacher" runat="server" Text='<%# getTeacherName(Eval("GradeTeacherId")) %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlTeachers" class="custom-select form-control" runat="server">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=" Class Supervisor ">
                                        <ItemTemplate>
                                            <asp:Label ID="lblClassSupervisor" runat="server" Text='<%# getSupervisorName(Eval("GradeSupervisorId")) %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlSupervisors" class="custom-select form-control" runat="server">
                                                <asp:ListItem  Selected =  "True" Text = " ---Select--- " Value = "0" ></asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit" ShowHeader="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnedit" runat="server" CommandName="Edit" Text="Edit" CommandArgument='<%# Eval("Id") %>'
                                                SkinID="lnkGreen"></asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="btnupdate" runat="server" CommandName="Update" Text="Update"
                                                SkinID="lnkGreen" CommandArgument='<%# Eval("Id") %>'></asp:LinkButton>
                                            <asp:LinkButton ID="btncancel" runat="server" CommandName="Cancel" Text="Cancel"
                                                SkinID="lnkGreen" CommandArgument='<%# Eval("Id") %>'></asp:LinkButton>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:Button ID="btnRemoveRole" runat="server" CausesValidation="False" Text="Remove"
                                                CommandName="Remove"  />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                </Columns>
                        <PagerSettings FirstPageText="&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                        LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                        Mode="NextPreviousFirstLast" 
                        NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" 
                        PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />                            
                            </asp:GridView>

                                            </div>
                                            </div>
                                            </div>                                        
                                        </form>

                                 </div>
                                </div>
                        
                                </asp:Content>
