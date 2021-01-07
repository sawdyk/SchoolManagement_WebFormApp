<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="AdmissionApplication.aspx.cs" Inherits="AdmissionApplication" %>

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
                      
                        <div class="row flex-row">
                            <div class="col-12">
                                <!-- Form -->
                                <div class="widget has-shadow">
                                    <div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>NEW APPLICATION</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblReport" runat="server" Text="" Visible="false"></asp:Label>
                                          <asp:Label ID="lblResultMsg" runat="server" Text="" class="text-success ml-2"></asp:Label>
                                            <fieldset><legend>Personal's Details</legend>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">LastName:<span class="text-danger ml-2">*</span></label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ForeColor="Red" runat="server" ErrorMessage="This field is required" ControlToValidate="txtLastname"></asp:RequiredFieldValidator>
                                                   <asp:TextBox ID="txtLastname" class="form-control"  runat="server"></asp:TextBox>

                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">FirstName: <span class="text-danger ml-2">*</span></label>
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ForeColor="Red" runat="server" ErrorMessage="This field is required" ControlToValidate="txtFirstname"></asp:RequiredFieldValidator>
                                                   <asp:TextBox ID="txtFirstname" class="form-control" runat="server"></asp:TextBox>

                                                </div>
                                            </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Middlename:<span class="text-danger ml-2">*</span></label>
                                                   <asp:TextBox ID="txtMiddlename"  class="form-control" runat="server" ></asp:TextBox>

                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Admission Session:<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList ID="ddlAdmissionSession" DataTextField="SessionName" DataValueField="ID" class="custom-select form-control" runat="server"></asp:DropDownList>

                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Gender:<span class="text-danger ml-2">*</span></label>
                                                  <asp:DropDownList ID="ddlGender" class="custom-select form-control" runat="server"></asp:DropDownList>
                                                </div>
                                               <div class="col-xl-6">
                                                 <label class="form-control-label">Date of Birth:<span class="text-danger ml-2">*</span></label>
                                                    <asp:TextBox ID="txtDateOfBirth" placeholder="dd-MM-yyyy" class="form-control" TabIndex="25"  runat="server" MaxLength="10"></asp:TextBox>
                                                   <ajaxToolKit:CalendarExtender ID="CalendarExtender1" Format="dd-MM-yyyy"  runat="server"  TargetControlID="txtDateOfBirth" PopupButtonID="ImageButton1" PopupPosition="BottomLeft">
                                                                            </ajaxToolKit:CalendarExtender>                                                   

                                                     </div>
                                                      </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Last School Attended:<span class="text-danger ml-2">*</span></label>
                                                   <asp:TextBox ID="txtLastSchoolAttended"  class="form-control" runat="server" ></asp:TextBox>

                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Last School Address:<span class="text-danger ml-2">*</span></label>
                                                   <asp:TextBox ID="txtLastSchoolAddress"  class="form-control" runat="server" ></asp:TextBox>

                                                </div>
                                            </div>
                                             <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Last School Attended Class:<span class="text-danger ml-2">*</span></label>
                                                   <asp:TextBox ID="txtClass"  class="form-control" runat="server" ></asp:TextBox>

                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Last School Attended Year:<span class="text-danger ml-2">*</span></label>
                                                   <asp:TextBox ID="txtYear"  class="form-control" runat="server" ></asp:TextBox>

                                                </div>
                                            </div>

                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Passport:<span class="text-danger ml-2">*</span></label>
                                                    <asp:FileUpload ID="documentUpload" class="form-control" runat="server" />
                                                </div>
                                            </div>
                                                </fieldset>
                                            <fieldset><legend>Medical Details</legend>

                                                 <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Any Disability:<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList ID="ddlDisability" class="custom-select form-control" runat="server"></asp:DropDownList>

                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Illness:<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList ID="ddlIllness" class="custom-select form-control" runat="server"></asp:DropDownList>

                                                </div>
                                            </div>
                                                 <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Inoculations:<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList ID="ddlInoculations" class="custom-select form-control" runat="server"></asp:DropDownList>

                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Others:<span class="text-danger ml-2">*</span></label>
                                                <asp:TextBox ID="txtOthers" class="form-control" runat="server"></asp:TextBox>

                                                </div>
                                            </div>
                                                </fieldset>
                                              <fieldset><legend>Guardian's Details</legend>
                                                   <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Fullname:<span class="text-danger ml-2">*</span></label>
                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Display="Dynamic" ForeColor="Red" runat="server" ErrorMessage="This field is required" ControlToValidate="txtGuardianName"></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="txtGuardianName" class="form-control" runat="server"></asp:TextBox>

                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Address:<span class="text-danger ml-2">*</span></label>
                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator4" Display="Dynamic" ForeColor="Red" runat="server" ErrorMessage="This field is required" ControlToValidate="txtGuardianAddress"></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="txtGuardianAddress" class="form-control" runat="server"></asp:TextBox>

                                                </div>
                                            </div>
                                                   <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Phone No:<span class="text-danger ml-2">*</span></label>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" Display="Dynamic" ForeColor="Red" runat="server" ErrorMessage="This field is required" ControlToValidate="txtGuardianPhoneNo"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtGuardianPhoneNo" class="form-control"  runat="server"></asp:TextBox>

                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Email:<span class="text-danger ml-2">*</span></label>
                                                <asp:TextBox ID="txtGuardianEmail" class="form-control" runat="server"></asp:TextBox>

                                                </div>
                                            </div>
                                                   <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Relationship:<span class="text-danger ml-2">*</span></label>
                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator6" Display="Dynamic" ForeColor="Red" runat="server" ErrorMessage="This field is required" ControlToValidate="txtRelationship"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtRelationship" class="form-control" runat="server"></asp:TextBox>

                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Occupation:<span class="text-danger ml-2">*</span></label>
                                                <asp:TextBox ID="txtOccupation" class="form-control" runat="server"></asp:TextBox>

                                                </div>
                                            </div>
                                                   <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Place of Work:<span class="text-danger ml-2">*</span></label>
                                                <asp:TextBox ID="txtPlaceofWork" class="form-control" runat="server"></asp:TextBox>

                                                </div>
                                            </div>
                                                  </fieldset>
                                                    <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                     <asp:Button ID="btnSave" OnClick="btnSave_Click" class="btn btn-secondary" runat="server" Text="Save"  />
                                                    </div>
                                           <%-- <div class="col-xl-6">
                    <asp:Button ID="btnReset" OnClick="btnReset_Click" class="btn btn-secondary" runat="server" Text="Reset"  />

                                                </div>--%>
                                                </div>
                                                </form>    
                                            </div>
                                            </div>

                                 </div>
                                </div>
                                </asp:Content>
                        
