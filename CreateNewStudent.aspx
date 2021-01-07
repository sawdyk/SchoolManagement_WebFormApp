<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="CreateNewStudent.aspx.cs" Inherits="CreateNewStudent" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl" TagPrefix="rjs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="PASSIS.LIB" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .style1 {
            width: 255px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">          
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
                                  <%--  <div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>NEW STUDENT</h4>
                                    </div>--%>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">
                                           <asp:Label ID="lblErrorPassportMsg" runat="server" Text="" Visible="false"></asp:Label>
                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" Visible="false"></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text="" Visible="false"></asp:Label>
                                                 <asp:Panel runat="server" ID="pnlAll" Width="100%">
                            <asp:Wizard ID="wzdTaxPayment" runat="server" ActiveStepIndex="0" OnFinishButtonClick="wzdTaxPayment_FinishButtonClick"
                                Width="100%" DisplaySideBar="False" OnNextButtonClick="wzdTaxPayment_NextButtonClick">
                                <StartNextButtonStyle CssClass="button" />
                                <FinishCompleteButtonStyle CssClass="button" />
                                <FinishPreviousButtonStyle CssClass="button" />
                                <FinishNavigationTemplate>
                                    <asp:Button ID="FinishPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious"
                                        class="btn btn-secondary" OnClick="FinishPreviousButton_Click" TabIndex="2" Text="Previous" />
                                    <asp:Button ID="FinishButton" runat="server" CommandName="MoveComplete"  class="btn btn-secondary"
                                        Text=" Save Student Details" Visible="<%# FinishCompleteButtonVisibility %>" />
                                </FinishNavigationTemplate>
                                <StepPreviousButtonStyle CssClass="button" />
                                <StartNavigationTemplate>
                                    <asp:Button ID="StartNextButton" runat="server" CommandName="MoveNext" class="btn btn-secondary"
                                        Text="Next" />
                                </StartNavigationTemplate>
                                <StepNavigationTemplate>
                                    <asp:Button ID="StepPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious"
                                        class="btn btn-secondary" meta:resourcekey="StepPreviousButtonResource1" TabIndex="3"
                                        Text="Previous" Visible="<%# StepPreviousButtonVisibility %>" />
                                    <asp:Button ID="StepNextButton" runat="server" CommandName="MoveNext" TabIndex="2"
                                        Text="Next" class="btn btn-secondary" ValidationGroup="simple" Visible="<%# StepNextButtonVisibility %>" />
                                </StepNavigationTemplate>
                                <WizardSteps>
                                    <asp:WizardStep ID="WizardStep1" runat="server" Title="Step 1">
                                         <div class="widget has-shadow">
                                                <div class="widget-header bordered no-actions d-flex align-items-center">
                                                 <h4>Admission Form: Students Biodata (1 of 3)</h4>
                                                </div>
                                             </div>
                                        </div>
                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Choose Campus:<span class="text-danger ml-2">*</span></label>
                                                    <asp:DropDownList ID="ddlCampus" DataTextField="Name" DataValueField="Id" class="custom-select form-control" runat="server"></asp:DropDownList>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Choose Accomodation Type:<span class="text-danger ml-2">*</span></label>
                                                <asp:DropDownList runat="server" class="custom-select form-control" ID="ddlAccomodation" AppendDataBoundItems="true"
                                                DataTextField="CategoryName" DataValueField="CategoryName">  
                                                    <asp:ListItem Text="Day Student" Value="Day Student"></asp:ListItem>
                                                    <asp:ListItem Text="Boarder" Value="Boarder"></asp:ListItem>
                                                </asp:DropDownList>                                                   

                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">FirstName:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtFirstName" class="form-control" placeholder="FirstName"></asp:TextBox>
                                             <asp:RequiredFieldValidator SetFocusOnError="true" ControlToValidate="txtFirstName"
                                              ID="RequiredFieldValidator1" runat="server" Text="Enter FirstName">*</asp:RequiredFieldValidator>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label"> LastName:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtLastName" class="form-control" placeholder="LastName"></asp:TextBox>
                                                       <asp:RequiredFieldValidator SetFocusOnError="true" ControlToValidate="txtLastName"
                                                         ID="RequiredFieldValidator2" runat="server" Text="Enter LastName">*</asp:RequiredFieldValidator>
                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">MiddleName:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtMiddleName" class="form-control" placeholder="MiddleName"></asp:TextBox>
                                                </div>
                                                <div class="col-xl-6">
                                                  <label class="form-control-label">Gender:<span class="text-danger ml-2">*</span></label>
                                                 <asp:DropDownList runat="server" class="custom-select form-control" ID="ddlGender">
                                                    <asp:ListItem Selected="True" Text=" --Select--" Value="0"></asp:ListItem>
                                                     <asp:ListItem Text=" Male " Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Female " Value="2"></asp:ListItem>
                                                     </asp:DropDownList>         
                                                    </div>
                                                    </div>
                                          <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Learning Support:<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList ID="ddlLearningSupport" class="custom-select form-control" runat="server"></asp:DropDownList>
                                                </div>
                                                <div class="col-xl-6">
                                                  <label class="form-control-label">Date of Birth:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtDateOfBirth"  class="form-control" placeholder="Date of Birth"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender 
                                                     ID="myDateFromCal" Format="yyyy-MM-dd"  runat="server" TargetControlID="txtDateOfBirth" PopupButtonID="ImageButton1" >
                                                      </ajaxToolkit:CalendarExtender>
                                                     <asp:RequiredFieldValidator SetFocusOnError="true" ControlToValidate="txtDateOfBirth"
                                                         ID="RequiredFieldValidator3" runat="server" class="form-control" Text="Enter Date of Birth">*</asp:RequiredFieldValidator>
                                                    </div>
                                                    </div>
                                          <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Street Address:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtStreetAddress" class="form-control" placeholder="Street Address"></asp:TextBox>
                                                </div>
                                                <div class="col-xl-6">
                                                  <label class="form-control-label">City:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtCity" class="form-control" placeholder="City"></asp:TextBox>
         
                                                    </div>
                                                    </div>
                                          <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Country:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtCountry" class="form-control" placeholder="Country"></asp:TextBox>
                                                </div>
                                                <div class="col-xl-6">
                                                  <label class="form-control-label">Alergies:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="TextBox9" class="form-control" placeholder="Alergies"></asp:TextBox>
         
                                                    </div>
                                                    </div>
                                          <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Bus Route:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="TextBox6" class="form-control" placeholder="Bus Route"></asp:TextBox>
                                                </div>
                                                <div class="col-xl-6">
                                                  <label class="form-control-label">Siblings Presently in the school(if any):<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtSiblings" class="form-control" placeholder=""></asp:TextBox>
         
                                                    </div>
                                             
                                              <div class="col-xl-6 mb-3">
                                                   <br /><br /><br />
                                                   <div style="color: #006600; font-family: tahoma; font-size: 11px;">
                                                                    If the child has siblings already registered, enter the father's phone number and
                                                            click next to retreive the previously stored parents details.
                                                                </div>
                                                   <label class="form-control-label">Check if parents Exists:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtFathersMobileToBeChecked" class="form-control" placeholder="Fathers' Mobile No :"></asp:TextBox>
                                                </div>
                                                    </div>
                                         
                                                <%--<div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSave" class="btn btn-secondary"   runat="server" Text="Save Configuration"  />
                                                </div>
                                            </div>--%>
                                                </form>    
                                        <br /><br />
                                         </asp:WizardStep>
                                    <asp:WizardStep ID="WizardStep2" runat="server" Title="Step 2">
                                         <div class="widget has-shadow">
                                                <div class="widget-header bordered no-actions d-flex align-items-center">
                                                 <h4>Admission Form: Father's Details (2 of 3)</h4>
                                                </div>
                                             </div>
                                        </div>
                                        <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Fathers's Name:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtFathersName" class="form-control" placeholder="Father's Name"></asp:TextBox>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Nationality:<span class="text-danger ml-2">*</span></label>
                                                 <asp:TextBox runat="server" ID="txtFathersNationality" class="form-control" placeholder="Nationality"></asp:TextBox>
                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Occupation:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtFathersOccupation" class="form-control" placeholder="Occupation"></asp:TextBox>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label"> Office Address:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtFathersOfficeAdrress" class="form-control" placeholder="Office Address"></asp:TextBox>

                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Email Address:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtFathersEmailAddress" class="form-control" placeholder="Youremail@gmail.com"></asp:TextBox>
                                                </div>
                                                <div class="col-xl-6">
                                                  <label class="form-control-label">Telephone:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtFathersTelephone" class="form-control" placeholder="Telephone"></asp:TextBox>
         
                                                    </div>
                                                    </div>

                                                 <br /><br /><br />
                                              <div class="widget has-shadow">
                                                <div class="widget-header bordered no-actions d-flex align-items-center">
                                                 <h4>Admission Form: Mother's Details</h4>
                                             </div>
                                                  </div>
                                                 <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Mother's Name:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtMothersName" class="form-control" placeholder="Mother's Name"></asp:TextBox>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Nationality:<span class="text-danger ml-2">*</span></label>
                                                 <asp:TextBox runat="server" ID="txtMotherNationality" class="form-control" placeholder="Nationality"></asp:TextBox>
                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Occupation:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtMotherOccupation" class="form-control" placeholder="Occupation"></asp:TextBox>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label"> Office Address:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtMothersOfficeAddress" class="form-control" placeholder="Office Address"></asp:TextBox>

                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Email Address:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtMotherEmailAddress" class="form-control" placeholder="Youremail@gmail.com"></asp:TextBox>
                                                </div>
                                                <div class="col-xl-6">
                                                  <label class="form-control-label">Telephone:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtMotherTelephone" class="form-control" placeholder="Telephone"></asp:TextBox>
         
                                                    </div>
                                                    </div>

                                     </asp:WizardStep>
                                    <asp:WizardStep ID="WizardStep3" runat="server" Title="Step 3">
                                          <div class="widget has-shadow">
                                                <div class="widget-header bordered no-actions d-flex align-items-center">
                                                 <h4>Admission Form: Guardian's Details  (3 of 3)</h4>
                                                    <br /><br />
                                                 <asp:Label ID="lblResponse" runat="server" Text="" Visible="false"></asp:Label>
                                                </div>
                                             </div>
                                        </div>
                                        <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label"> Name:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtGuardianName" class="form-control" placeholder="Name"></asp:TextBox>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Home Address:<span class="text-danger ml-2">*</span></label>
                                                 <asp:TextBox runat="server" ID="TextBox24" class="form-control" placeholder="Home Address"></asp:TextBox>
                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Email Address:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtGuardianEmailAddress" class="form-control" placeholder="Email Address"></asp:TextBox>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label"> Telephone:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtGuardianTelephone" class="form-control" placeholder="Telephone"></asp:TextBox>

                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Relationship To Student:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtGuardianRelationshipWtStd" class="form-control" placeholder="Brother,Sister,Uncle..."></asp:TextBox>
                                                </div>
                                                    </div>

                                                 <br /><br /><br />
                                              <div class="widget has-shadow">
                                                <div class="widget-header bordered no-actions d-flex align-items-center">
                                                 <h4>Admission Form: Last School Attended</h4>
                                                </div>
                                             </div>
                                                 <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">School Name:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtSchoolName" class="form-control" placeholder="School Name"></asp:TextBox>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Year:<span class="text-danger ml-2">*</span></label>
                                                 <asp:TextBox runat="server" ID="TextBox30" class="form-control" placeholder="Year"></asp:TextBox>
                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">City:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="TextBox31" class="form-control" placeholder="City"></asp:TextBox>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Class:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="TextBox32"  class="form-control" placeholder="Class"></asp:TextBox>

                                                  </div>
                                                <div class="col-xl-6">
                                                 <label class="form-control-label">Upload Passport:<span class="text-danger ml-2">*</span></label>
                                             <asp:FileUpload ID="documentUpload" runat="server" class="form-control" />

                                             <asp:Checkbox runat="server" Text="Confirm Upload" ForeColor="#006600" Font-Names="tahoma" Font-Size="11px" ID="chkConfirmUpload" class="form-control"/>

                                                  </div>
                                               </div>
                                            
                                   </asp:WizardStep>
                                </WizardSteps>
                            </asp:Wizard>

                            <asp:CustomValidator ID="cumError" runat="server" ValidationGroup="simple" Visible="False"
                                meta:resourcekey="cumErrorResource1"></asp:CustomValidator>
                        </asp:Panel>
                                 <%--<div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>CLASS ARMS</h4>
                                 </div>--%>
                            <%--<asp:GridView ID="gvdAcademicSession" runat="server" AutoGenerateColumns="False" EmptyDataText=" No Calendar year currently exist" AllowPaging="true" Width="100%" ShowFooter="True" PageSize="20" GridLines="Both" OnRowCommand="gvdAcademicSession_RowCommand">       
                            <PagerSettings FirstPageText="&lt;i class=&quot;fa  fa-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-left&quot;&gt;&lt;/i&gt;"
                                LastPageText="&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;" Mode="NextPreviousFirstLast" 
                                NextPageText="&lt;i class=&quot;fa fa-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;fa fa-arrow-left&quot;&gt;&lt;/i&gt;" />--%>
    
     
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
                         
                                </asp:Content>
