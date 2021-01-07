<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Self_Setup/Self_setup.master" CodeFile="Default.aspx.cs" Inherits="Self_Setup_schools" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="PASSIS.LIB" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1 {
            width: 255px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="ViewSchoolRegistration" runat="server">
    <div class="content-inner">
                    <div class="container-fluid">
                      
                        <!-- Begin Row -->
                        <div class="row flex-row" style="width:auto;margin-left:-210px">
                            <div class="col-12">
                                <!-- Form -->
                                <div class="widget has-shadow">
                                    <div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>CONTACT PERSON DETAILS</h4>
                                    </div>
                                    <div class="widget-body"> 
                                       <%-- <form class="form-horizontal">--%>

                                            <asp:Label ID="lblReport" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Firstname:<span class="text-danger ml-2">*</span></label>
                                      <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Firstname is required" ForeColor="Red" ControlToValidate="txtFirstname"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtFirstname" placeholder="Enter firstname" class="form-control" runat="server"></asp:TextBox>    
                                                </div>
                                                <div class="col-xl-6 mb-3">
                                                   <asp:label runat="server" class="form-control-label">Lastname:<span class="text-danger ml-2">*</span></asp:label>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Lastname is required" ForeColor="Red" ControlToValidate="txtLastname"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtLastname" placeholder="Enter lastname" class="form-control" runat="server"></asp:TextBox>    
                                                </div>
                                            </div>
                                            
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                      <label class="form-control-label">Position in School:<span class="text-danger ml-2">*</span></label>
                                    <asp:DropDownList ID="ddlPositioninSchool" class="custom-select form-control" DataTextField="PositionName" DataValueField="ID" runat="server"></asp:DropDownList>
                                                     </div>
                                                       <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Email Address:<span class="text-danger ml-2">*</span></label>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" Display="Dynamic" runat="server" ErrorMessage="Email is required" ForeColor="Red" ControlToValidate="txtEmail"></asp:RequiredFieldValidator> 
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic" ErrorMessage="Invalid Email" ForeColor="Red" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>                                   
                                    <asp:TextBox runat="server" class="form-control" placeholder="Enter email" ID="txtEmail"></asp:TextBox>                                                   

                                                       </div>                                               
                                                    </div>
                                              <div class="form-group row mb-3">
                                                        <div class="col-xl-6 mb-3">
                                                 <label class="form-control-label">Phone Number:<span class="text-danger ml-2">*</span></label>
                                    <asp:TextBox runat="server" class="form-control" placeholder="Enter phone no" ID="txtPhoneNo"></asp:TextBox>

                                                  </div>
                                                     </div>


                                            <div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>SCHOOL DETAILS</h4>
                                    </div>
                                    <div class="widget-body"> 
<%--                                        <form class="form-horizontal">--%>

                                            <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">School Name:<span class="text-danger ml-2">*</span></label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="School name is required" ForeColor="Red" ControlToValidate="txtSchoolName"></asp:RequiredFieldValidator>
                                    <asp:TextBox runat="server" placeholder="Enter school name" class="form-control" ID="txtSchoolName"></asp:TextBox>
                                                </div>
                                                <div class="col-xl-6">
                                                   <asp:label runat="server" class="form-control-label">School Code:<span class="text-danger ml-2">*</span></asp:label>
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="School code is required" ForeColor="Red" ControlToValidate="txtSchoolCode"></asp:RequiredFieldValidator>
                                    <asp:TextBox runat="server" class="form-control" placeholder="Enter school code e.g ECD" MaxLength="3" ID="txtSchoolCode"></asp:TextBox>                                                </div>
                                            </div>
                                            
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                      <label class="form-control-label">School Type:<span class="text-danger ml-2">*</span></label>
                                    <asp:DropDownList ID="ddlSchoolType" class="custom-select form-control" DataTextField="SchoolTypeName" DataValueField="Id" runat="server"></asp:DropDownList>
                                                     </div>
                                                    <%--<div class="col-xl-6">
                                                    <label class="form-control-label">Term:<span class="text-danger ml-2">*</span></label>
                                    <asp:DropDownList ID="ddlTerm" DataTextField="AcademicTermName" class="custom-select form-control" DataValueField="Id" runat="server"></asp:DropDownList>
                                                  </div> --%>                                              
                                                    </div>
                                              
                                            </div>

                                            <div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>CAMPUS DETAILS</h4>
                                    </div>
                                    <div class="widget-body"> 
<%--                                        <form class="form-horizontal">--%>

                                            <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="Label4" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Name:<span class="text-danger ml-2">*</span></label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Campus name is required" ForeColor="Red" ControlToValidate="txtCampusName"></asp:RequiredFieldValidator>
                                    <asp:TextBox runat="server" placeholder="Enter campus name" class="form-control" ID="txtCampusName"></asp:TextBox>
                                                </div>
                                                <div class="col-xl-6">
                                                   <asp:label runat="server" class="form-control-label">Address:<span class="text-danger ml-2">*</span></asp:label>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Campus address is required" ForeColor="Red" ControlToValidate="txtCampusAddress"></asp:RequiredFieldValidator>
                                    <asp:TextBox runat="server" placeholder="Enter campus address" class="form-control" ID="txtCampusAddress"></asp:TextBox>                                                </div>
                                            </div>
                                            
                                           
                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnContinue" OnClick="btnContinue_Click" class="btn btn-secondary"   runat="server" Text="Continue"  />
                                                </div>
                                            </div>
<%--                                                </form>    --%>
                                        <br /><br />
                                        </div>
                                            </div>
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
                            </asp:View>
                               <asp:View ID="ViewSetupCurriculum" runat="server">
                                    <div class="content-inner">
                    <div class="container-fluid">
                   
                        <!-- Begin Row -->
                        <div class="row flex-row" style="width:auto;margin-left:-210px">
                            <div class="col-12">
                                <!-- Form -->
                                <div class="widget has-shadow">
                                    <div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>CURRICULUM SETUP</h4>
                                    </div>
                                    <div class="widget-body"> 
<%--                                        <form class="form-horizontal">--%>

                                            <asp:Label ID="lblSubjectSetupResponse" ForeColor="Black" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="Label6" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Curriculum Type:<span class="text-danger ml-2">*</span></label>
                           <asp:DropDownList ID="ddlCurriculum" DataTextField="CurriculumName" class="custom-select form-control" DataValueField="Id" runat="server" OnSelectedIndexChanged="ddlCurriculum_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>

                                                </div>
                                            </div>                                            
<%--                                                </form>    --%>
                                           <asp:Label ID="lblSubjectSetup" ForeColor="Black" runat="server" Text=""></asp:Label>
                                        <br />    <br />
                                        <asp:Panel runat="server" ID="pnlCurriculum" ScrollBars="Auto" Height="400">
                                        <asp:GridView ID="gdvCurriulumSubjectsList" GridLines="None" runat="server"  Width="1000%" ForeColor="Black" class="table table-striped" EmptyDataText="Empty Subjects List" AutoGenerateColumns="False">
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
                                                        <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <HeaderStyle Width="50px" />
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkAll" OnCheckedChanged="chkAll_CheckedChanged" AutoPostBack="true" runat="server" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="SubjectCheckbox" runat="server" />
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
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>

                                        <br /><br />
                        <div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>CURRICULUM OPTIONAL SUBJECTS:</h4>
                                    </div>
                                    <div class="widget-body"> 
<%--                                        <form class="form-horizontal">--%>

                                            <asp:Label ID="lll" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="Label8" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                        <asp:Label ID="lblOptionalSubjects" ForeColor="Black" runat="server" Text=""></asp:Label>
                                                </div>
                                            </div>
                                            
                                           
                                            </div>
                                        <asp:Panel runat="server" ID="Panel1" ScrollBars="Auto" Height="400">
                                        <asp:GridView ID="gdvOptionalSubjects" GridLines="None" runat="server" Width="1000%" ForeColor="Black" class="table table-striped" EmptyDataText="Empty Subjects List" AutoGenerateColumns="False">
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
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkAllOptional" OnCheckedChanged="chkAll_CheckedChangedOptional" AutoPostBack="true" runat="server" />
                                                    </HeaderTemplate>
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
                                    </asp:Panel>

                                           <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSaveSchoolSubjects" class="btn btn-secondary" OnClick="btnSaveSchoolSubjects_Click"  runat="server" Visible="false" Text="Save"   />
                                                </div>
                                            </div>



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
                   
                        <!-- Begin Row -->
                        <div class="row flex-row" style="width:auto;margin-left:-210px">
                            <div class="col-12">
                                <!-- Form -->
                                <div class="widget has-shadow">
                                    <div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>SCHOOL SETUP SUMMARY</h4>
                                    </div>
                                    <div class="widget-body"> 
                                    <div class="widget-body"> 
<%--                                        <form  class="form-horizontal">--%>

                                            <asp:Label ID="Label9" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="Label10" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                   <asp:Label ID="Label11" runat="server" Text="Congratulations!, you have successfully setup your school. You can login with the password below to access you account" ForeColor="Green"></asp:Label><br />
                                    <asp:HyperLink ID="HyperLink1" NavigateUrl="~/Login.aspx" runat="server">Continue to login page</asp:HyperLink>
                                                </div>
                                            </div>
                                            
                                           
                                            </div>
                                       <asp:GridView ID="gdvLoginDetails" runat="server" CellPadding="4"  GridLines="None" ForeColor="Black" class="table table-striped" AutoGenerateColumns="False">
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
                                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("Id") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText=" Username ">
                                                <HeaderStyle Width="100px" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUsername" runat="server" Text='<%# Eval("Username")  %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText=" Password">
                                                <HeaderStyle Width="100px" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPassword" runat="server" Text='<%# Eval("Password")  %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
              
                                      
                                             </div>
                                            </div>
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                            </asp:View>

                         </asp:MultiView>
                                </asp:Content>



