<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="RegisterSuccessfulStudent.aspx.cs" Inherits="RegisterSuccessfulStudent" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
 
        <div class="content-inner">
                    <div class="container-fluid">
                      
                        <div class="row flex-row" style="width:auto;">
                            <div class="col-12">
                                <!-- Form -->
                                <div class="widget has-shadow">
                                  <%--  <div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>NEW STUDENT</h4>
                                    </div>--%>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">
                                           <asp:Label ID="Label32" runat="server" Text="" Visible="false"></asp:Label>
                                            <asp:Label ID="lblResponse" runat="server" Text="" Visible="false"></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text="" Visible="false"></asp:Label>


                        <asp:Panel runat="server" ID="pnlAll" Width="100%">
                            <asp:Wizard ID="wzdTaxPayment" runat="server" ActiveStepIndex="0" OnFinishButtonClick="wzdTaxPayment_FinishButtonClick"
                                Width="100%" DisplaySideBar="False" OnNextButtonClick="wzdTaxPayment_NextButtonClick">
                                <StartNextButtonStyle CssClass="button" />
                                <FinishCompleteButtonStyle CssClass="button" />
                                <FinishPreviousButtonStyle CssClass="button" />
                                <FinishNavigationTemplate>
                                    <asp:Button ID="FinishPreviousButton" class="btn btn-secondary" runat="server" CausesValidation="False" CommandName="MovePrevious"
                                         OnClick="FinishPreviousButton_Click" TabIndex="2" Text="Previous" />
                                    <asp:Button ID="FinishButton" class="btn btn-secondary" runat="server" CommandName="MoveComplete" 
                                        Text=" Save Student Details" Visible="<%# FinishCompleteButtonVisibility %>" />
                                </FinishNavigationTemplate>
                                <StepPreviousButtonStyle CssClass="button" />
                                <StartNavigationTemplate>
                                    <asp:Button ID="StartNextButton" class="btn btn-secondary" runat="server" CommandName="MoveNext" 
                                        Text="Next" />
                                </StartNavigationTemplate>
                                <StepNavigationTemplate>
                                    <asp:Button ID="StepPreviousButton" class="btn btn-secondary" runat="server" CausesValidation="False" CommandName="MovePrevious"
                                        meta:resourcekey="StepPreviousButtonResource1" TabIndex="3"
                                        Text="Previous" Visible="<%# StepPreviousButtonVisibility %>" />
                                    <asp:Button ID="StepNextButton" class="btn btn-secondary" runat="server" CommandName="MoveNext" TabIndex="2"
                                        Text="Next" ValidationGroup="simple" Visible="<%# StepNextButtonVisibility %>" />
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
              <asp:Label ID="lblMainErrorMsg" runat="server" Visible="false" SkinID="LabelError"></asp:Label>
                                                    <label class="form-control-label">Choose a Campus:<span class="text-danger ml-2">*</span></label>
                                                    <asp:DropDownList ID="ddlCampus" DataTextField="Name" DataValueField="Id" class="custom-select form-control" runat="server"></asp:DropDownList>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Choose Accomodation Type:<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList runat="server" class="custom-select form-control" ID="ddlAccomodation">
                                                    <asp:ListItem Text="Day Student" Value="Day Student"></asp:ListItem>
                                                    <asp:ListItem Text="Boarder" Value="Boarder"></asp:ListItem>
                                                </asp:DropDownList>                                                   

                                                  </div>
                                               </div>

                                      <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Application ID:<span class="text-danger ml-2">*</span></label>
                                                        <asp:DropDownList AutoPostBack="true" class="custom-select form-control" DataTextField="ApplicantId" runat="server" ID="ddlApplicationID" OnSelectedIndexChanged="ddlApplicationID_SelectedIndexChanged">
                                                                                        </asp:DropDownList>                                               
                                                   </div>
                                               </div>
   
                                             <div class="widget has-shadow">
                                                <div class="widget-header bordered no-actions d-flex align-items-center">
                                                 <h4>Students Biodata</h4>
                                                </div>
                                             </div>
                                        </div>
                                                
                                          <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">First Name:<span class="text-danger ml-2">*</span></label>
                                                     <asp:Label runat="server" class="form-control-label" ID="lblFirstName"></asp:Label>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Last Name:<span class="text-danger ml-2">*</span></label>
                                                     <asp:Label runat="server" class="form-control-label" ID="IblLastName"></asp:Label>

                                                  </div>
                                               </div>
                                <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Middle Name:<span class="text-danger ml-2">*</span></label>
                                                     <asp:Label runat="server" class="form-control-label" ID="lblMiddleName"></asp:Label>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Gender:<span class="text-danger ml-2">*</span></label>
                                                     <asp:Label runat="server" class="form-control-label" ID="IblGender"></asp:Label>

                                                  </div>
                                               </div>

                                <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                     <label class="form-control-label">Street Address:<span class="text-danger ml-2">*</span></label>
                                                     <asp:Label runat="server" class="form-control-label" ID="lblStreetAddress"></asp:Label>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Date of Birth:<span class="text-danger ml-2">*</span></label>
                                                     <asp:Label runat="server" class="form-control-label" ID="lblDateOfBirth"></asp:Label>

                                                  </div>
                                               </div>

                                 <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                       <label class="form-control-label"> Learning Support ?:<span class="text-danger ml-2">*</span></label>
                                                 <asp:DropDownList runat="server" class="custom-select form-control" ID="ddlLearningSupport"></asp:DropDownList>
                                                   
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">City:<span class="text-danger ml-2">*</span></label>
                                           <asp:TextBox runat="server" class="form-control" ID="txtCity"></asp:TextBox>

                                                  </div>
                                               </div>
                                <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Country:<span class="text-danger ml-2">*</span></label>
                                           <asp:TextBox runat="server" class="form-control" ID="txtCountry"></asp:TextBox>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Alergies:<span class="text-danger ml-2">*</span></label>
                                           <asp:TextBox runat="server" class="form-control" ID="txtAlergies"></asp:TextBox>

                                                  </div>
                                               </div>
                                 <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Bus Route:<span class="text-danger ml-2">*</span></label>
                                           <asp:TextBox runat="server" class="form-control" ID="txtRoute"></asp:TextBox>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Siblings Presently In The School (if any ):<span class="text-danger ml-2">*</span></label>
                                           <asp:TextBox runat="server" class="form-control" ID="txtSiblings"></asp:TextBox>

                                                  </div>
                                               </div>
                                                  <div class="widget has-shadow">
                                                <div class="widget-header bordered no-actions d-flex align-items-center">
                                                 <h4>Check if parents already exist</h4>
                                                </div>
                                             </div>
                                        </div>
                                             <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label"><span class="text-danger ml-2">*</span></label>
                                                       <div style="color: #006600; font-family: tahoma; font-size: 11px;">
                                                                    If the child has siblings already registered, enter the father's email address and
                                                            click next to retreive the previously stored parents details.
                                                                </div>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Fathers' Mobile No:<span class="text-danger ml-2">*</span></label>
                                           <asp:TextBox runat="server" class="form-control" ID="txtFathersMobileToBeChecked"></asp:TextBox>

                                                  </div>
                                               </div>
                                               
                                    </asp:WizardStep>
                                    <asp:WizardStep ID="WizardStep2" runat="server" Title="Step 2">
                                         <div class="widget has-shadow">
                                                <div class="widget-header bordered no-actions d-flex align-items-center">
                                                 <h4>Admission Form : Father's Details (2 of 3)</h4>
                                                </div>
                                             </div>
                                        </div>

                                    <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Father's Name:<span class="text-danger ml-2">*</span></label>
                                           <asp:TextBox runat="server" class="form-control" ID="txtFathersName"></asp:TextBox>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Nationality:<span class="text-danger ml-2">*</span></label>
                                           <asp:TextBox runat="server" class="form-control" ID="txtFathersNationality"></asp:TextBox>

                                                  </div>
                                               </div>
                        
                                    <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Occupation:<span class="text-danger ml-2">*</span></label>
                                           <asp:TextBox runat="server" class="form-control" ID="txtFathersOccupation"></asp:TextBox>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Office Address:<span class="text-danger ml-2">*</span></label>
                                           <asp:TextBox runat="server" class="form-control" ID="txtFathersOfficeAdrress"></asp:TextBox>

                                                  </div>
                                               </div>

                                    <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Email Address:<span class="text-danger ml-2">*</span></label>
                                           <asp:TextBox runat="server" class="form-control" ID="txtFathersEmailAddress"></asp:TextBox>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Telephone:<span class="text-danger ml-2">*</span></label>
                                           <asp:TextBox runat="server" class="form-control" ID="txtFathersTelephone"></asp:TextBox>

                                                  </div>
                                               </div>

                                             <div class="widget has-shadow">
                                                <div class="widget-header bordered no-actions d-flex align-items-center">
                                                 <h4>Admission Form : Mother's Details</h4>
                                                </div>
                                             </div>
                                        </div>

                                          <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Mother's Name:<span class="text-danger ml-2">*</span></label>
                                           <asp:TextBox runat="server" class="form-control" ID="txtMothersName"></asp:TextBox>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Nationality:<span class="text-danger ml-2">*</span></label>
                                           <asp:TextBox runat="server" class="form-control" ID="txtMotherNationality"></asp:TextBox>

                                                  </div>
                                               </div>     

                                    <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Occupation:<span class="text-danger ml-2">*</span></label>
                                           <asp:TextBox runat="server" class="form-control" ID="txtMotherOccupation"></asp:TextBox>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Office Address:<span class="text-danger ml-2">*</span></label>
                                           <asp:TextBox runat="server" class="form-control" ID="txtMothersOfficeAddress"></asp:TextBox>

                                                  </div>
                                               </div>   
            
                                    <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Email Address:<span class="text-danger ml-2">*</span></label>
                                           <asp:TextBox runat="server" class="form-control" ID="txtMotherEmailAddress"></asp:TextBox>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Telephone:<span class="text-danger ml-2">*</span></label>
                                           <asp:TextBox runat="server" class="form-control" ID="txtMotherTelephone"></asp:TextBox>

                                                  </div>
                                               </div>   

                                    </asp:WizardStep>
                                    <asp:WizardStep ID="WizardStep3" runat="server" Title="Step 3">
                                        
                                             <div class="widget has-shadow">
                                                <div class="widget-header bordered no-actions d-flex align-items-center">
                                                 <h4>Admission Form : Last School Attended (3 of 3)</h4>
                                                </div>
                                             </div>
                                        </div>
                                               <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">School Name:<span class="text-danger ml-2">*</span></label>
                                                     <asp:Label runat="server" class="form-control-label" ID="lblSchoolName"></asp:Label>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Year:<span class="text-danger ml-2">*</span></label>
                                                     <asp:Label runat="server" class="form-control-label" ID="lblLastSchoolAttendedYear"></asp:Label>

                                                  </div>
                                               </div> 

                                        <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">City:<span class="text-danger ml-2">*</span></label>
                                           <asp:TextBox runat="server" class="form-control" ID="txtLastSchoolAttendedCity"></asp:TextBox>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Class:<span class="text-danger ml-2">*</span></label>
                                                     <asp:Label runat="server" class="form-control-label" ID="lblLastSchoolAttendedClass"></asp:Label>

                                                  </div>
                                               </div>


                                             <div class="widget has-shadow">
                                                <div class="widget-header bordered no-actions d-flex align-items-center">
                                                 <h4>Admission Form : Guardian's Details</h4>
                                                </div>
                                             </div>
                                        </div>

                                             <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Name:<span class="text-danger ml-2">*</span></label>
                                           <asp:TextBox runat="server" class="form-control" ID="txtGuardianName"></asp:TextBox>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Home Address:<span class="text-danger ml-2">*</span></label>
                                           <asp:TextBox runat="server" class="form-control" ID="txtGuardianHomeAddress"></asp:TextBox>

                                                  </div>
                                               </div>  

                                        <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Email Address:<span class="text-danger ml-2">*</span></label>
                                           <asp:TextBox runat="server" class="form-control" ID="txtGuardianEmailAddress"></asp:TextBox>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Telephone:<span class="text-danger ml-2">*</span></label>
                                           <asp:TextBox runat="server" class="form-control" ID="txtGuardianTelephone"></asp:TextBox>

                                                  </div>
                                               </div>  
                                        <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Relationship to student:<span class="text-danger ml-2">*</span></label>
                                           <asp:TextBox runat="server" class="form-control" ID="txtGuardianRelationshipWtStd"></asp:TextBox>
                                                </div>
                                               </div>  

                                            <div class="widget has-shadow">
                                                <div class="widget-header bordered no-actions d-flex align-items-center">
                                                 <h4>Upload Passport</h4>
                                                </div>
                                             </div>
                                        </div>
                   <asp:Label ID="lblErrorPassportMsg" runat="server" Visible="true" SkinID="LabelError"></asp:Label>
                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Upload passport:<span class="text-danger ml-2">*</span></label>
                                                                <asp:FileUpload ID="documentUpload"  class="form-control" runat="server" />
                                                </div>
                                            <div class="col-xl-6">
                                                 <label class="form-control-label"><span class="text-danger ml-2">*</span></label>
                             <asp:CheckBox ID="chkConfirmUpload" runat="server" class="form-control" Text="Confirm Upload" ForeColor="#006600" Font-Names="tahoma" Font-Size="11px" />    
                                                 <div style="color: #006600; font-family: tahoma; font-size: 11px;">
                                                                    Maximum file dimension: 140 X 160; Maximum file size : 30Kb ; file type: .jpg
                                                                </div>
                                            </div>
                                               </div>  
                                    </asp:WizardStep>
                                </WizardSteps>
                            </asp:Wizard>

                            <asp:CustomValidator ID="cumError" runat="server" ValidationGroup="simple" Visible="False"
                                meta:resourcekey="cumErrorResource1"></asp:CustomValidator>
                        </asp:Panel>
    </div>
    </div>
</asp:Content>


