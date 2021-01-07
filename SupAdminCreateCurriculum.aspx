<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="SupAdminCreateCurriculum.aspx.cs" Inherits="SupAdminCreateCurriculum" %>


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
     <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="ViewSetupCurriculum" runat="server">
    <div class="content-inner">
                    <div class="container-fluid">
                    
                        <div class="row flex-row" style="width:auto;">
                            <div class="col-12">
                                <!-- Form -->
                                <div class="widget has-shadow">
                                    <div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>CREATE CURRICULUM</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblResponse" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">School:<span class="text-danger ml-2">*</span></label>
                                    <asp:DropDownList ID="ddlSchool" DataTextField="Name" class="custom-select form-control" DataValueField="Id" runat="server"></asp:DropDownList>
                                                </div>
                                                <div class="col-xl-6">
                                     <label class="form-control-label">Curriculum Type:<span class="text-danger ml-2">*</span></label>
                                    <asp:DropDownList ID="ddlCurriculum" class="custom-select form-control" DataTextField="CurriculumName" DataValueField="Id" runat="server" OnSelectedIndexChanged="ddlCurriculum_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>

                                                </div>
                                            </div>
                                              
                                                </form>    
                                        <br />
                                                            <asp:Label ID="lblList" Font-Size="Large" ForeColor="Black" runat="server" Visible="false" Text="LIST OF SUBJECTS"></asp:Label>

                                        <asp:GridView ID="gdvCurriulumSubjectsList" GridLines="None" class="table table-striped" ForeColor="#333333" runat="server" EmptyDataText="Empty Subjects List" AutoGenerateColumns="False">
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
                                                 <HeaderStyle Width="100px" />
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>                              
                                            <asp:TemplateField HeaderText=" Subject Name ">
                                                <HeaderStyle Width="100px" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name")  %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText=" Subject Code">
                                                <HeaderStyle Width="100px" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCode" runat="server" Text='<%# Eval("Code")  %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText=" Class/Year">
                                                <HeaderStyle Width="100px" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblYear" runat="server" Text='<%# Eval("Class_Grade.Name")  %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="check Subject">
                                                <HeaderStyle Width="100px" />
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkAll" OnCheckedChanged="chkAll_CheckedChanged" AutoPostBack="true" runat="server" />
                                            </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="SubjectCheckbox" AutoPostBack="false" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="false" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             
                                        </Columns>
                                    </asp:GridView>

                                         <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSaveSchoolSubjects" class="btn btn-secondary" runat="server" Text="Save"  OnClick="btnSaveSchoolSubjects_Click" />
                                                </div>
                                            </div>
                                               </div>
                                            </div>
                                      </div>
                                                                       </asp:View>
      
                                               <asp:View ID="ViewOptionalCurriculum" runat="server">
                   <div class="content-inner">
                    <div class="container-fluid">
                    
                        <div class="row flex-row" style="width:auto;">
                            <div class="col-12">
                                <!-- Form -->
                                <div class="widget has-shadow">
                                    <div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4></h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">
                                                    <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <asp:label ID="lblOptionalSubjects" runat="server" Text="" class="form-control-label"></asp:label>
                                    <asp:DropDownList ID="ddlOptionalSubjects" runat="server" class="custom-select form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlOptionalSubjects_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                    <asp:GridView ID="gdvOptionalSubjects" GridLines="None" class="table table-striped" ForeColor="#333333" Visible="false" runat="server" EmptyDataText="Empty Subjects List" AutoGenerateColumns="False" CssClass="auto-style4">
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
                                            <asp:TemplateField ShowHeader="false" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIdOptional" runat="server" Text='<%# Eval("Id") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <HeaderStyle Width="50px" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="SubjectCheckboxOptional" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText=" Subject Name ">
                                                <HeaderStyle Width="100px" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNameOptional" runat="server" Text='<%# Eval("Name")  %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText=" Subject Code">
                                                <HeaderStyle Width="100px" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCodeOptional" runat="server" Text='<%# Eval("Code")  %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText=" Class/Year">
                                                <HeaderStyle Width="100px" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblYearOptional" runat="server" Text='<%# Eval("Class_Grade.Name")  %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>

                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSaveOptionalSubjects"  Visible="false" class="btn btn-secondary" runat="server" Text="Save"  OnClick="btnSaveOptionalSubjects_Click" />
                                                </div>
                                            </div>
                                                    </div>
                                            </div>
                                               </div>
                                            </div>
                                      </div>
                                                   </asp:View>
                                              <asp:View ID="ViewSummary" runat="server">
                                                  <div class="content-inner">
                    <div class="container-fluid">
                    
                        <div class="row flex-row" style="width:auto;">
                            <div class="col-12">
                                <!-- Form -->
                                <div class="widget has-shadow">
                                    <div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4></h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">
                                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label><br />
                                            </div>
                                                    </div>
                                            </div>
                                               </div>
                                            </div>
                                                  </asp:View>
                                            
                                 </asp:MultiView>  
                                </asp:Content>



