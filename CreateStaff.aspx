<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="CreateStaff.aspx.cs" Inherits="CreateStaff" %>

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
                                        <h4>CREATE NEW STAFF</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                        <%--    <div class="form-group row">
                                                <label class="col-lg-3 form-control-label">Child</label>
                                                <div class="col-sm-9">
                                                    <div class="row">
                                                        <div class="col-lg-4">
                                                            <input type="text" placeholder="Class" class="form-control">
                                                        </div>
                                                        <div class="col-lg-4">
                                                            <input type="text" placeholder="Grade" class="form-control">
                                                        </div>
                                                        <div class="col-lg-4">
                                                            <input type="text" placeholder="Year" class="form-control">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="form-group row">
                                                <label class="col-lg-3 form-control-label">Teacher </label>
                                                <div class="col-sm-9">
                                                    <div class="row">
                                                        <div class="col-lg-4">
                                                            <input type="text" placeholder="Class" class="form-control">
                                                        </div>
                                                        <div class="col-lg-4">
                                                            <input type="text" placeholder="Grade" class="form-control">
                                                        </div>
                                                        <div class="col-lg-4">
                                                            <input type="text" placeholder="Year" class="form-control">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="form-group row">
                                                <label class="col-lg-3 form-control-label">Parent</label>
                                                <div class="col-sm-9">
                                                    <div class="row">
                                                        <div class="col-lg-4">
                                                            <input type="text" placeholder="Class" class="form-control">
                                                        </div>
                                                        <div class="col-lg-4">
                                                            <input type="text" placeholder="Grade" class="form-control">
                                                        </div>
                                                        <div class="col-lg-4">
                                                            <input type="text" placeholder="Year" class="form-control">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>--%>



                                           <%-- <div class="section-title mt-5 mb-5">
                                                <h4>Input Field</h4>
                                            </div>--%>
                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" class="text-danger ml-2"></asp:Label>
                                           
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">FirstName<span class="text-danger ml-2">*</span></label>
                                                    <asp:TextBox ID="txtFirstname" Placeholder="FirstName" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">LastName<span class="text-danger ml-2">*</span></label>
                                                    <asp:TextBox ID="txtLastname" Placeholder="LastName" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group row mb-5">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Phone Number<span class="text-danger ml-2">*</span></label>
                                                    <div class="input-group">
                                                        <span class="input-group-addon addon-secondary">
                                                            <i class="la la-phone"></i>
                                                        </span>
                                                       <asp:TextBox ID="txtPhoneNumber" Placeholder="Phone Number" class="form-control" runat="server"></asp:TextBox>
                                                         <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtPhoneNumber"
                                            ErrorMessage="Phone Number entry is incorrect." ValidationExpression="[0|1|2|3|4|5|6|7|8|9|+|-]+"
                                            ValidationGroup="validateUser">*</asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">UserName<span class="text-danger ml-2">*</span></label>
                                                    <asp:TextBox ID="txtUsername" Placeholder="UserName" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                                 <div class="col-xl-6">
                                                 <label class="form-control-label">Email Address<span class="text-danger ml-2">*</span></label>
                                                    <asp:TextBox ID="txtEmailAddress" Placeholder="Email Address" class="form-control" runat="server"></asp:TextBox>
                                                      <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmailAddress"
                                            ErrorMessage="Email is incorrect." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>
                                                     </div>
                                               <div class="col-xl-6">
                                                 <label class="form-control-label">Gender<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList ID="ddlGender" class="custom-select form-control" runat="server">
                                            <asp:ListItem Value="1"> Male </asp:ListItem>
                                            <asp:ListItem Value="2"> Female </asp:ListItem>
                                                </asp:DropDownList>
                                                   </div>

                                                <div class="col-xl-6">
                                                 <label class="form-control-label">Campus<span class="text-danger ml-2">*</span></label>
                                                    <asp:DropDownList ID="ddlCampus" class="custom-select form-control" runat="server"></asp:DropDownList>
                                                    <%--<select name="country" class="custom-select form-control">
                                                        <option value="">Select</option>
                                                        <option value="AF">Afghanistan</option>
                                                        <option value="ZW">Zimbabwe</option>
                                                    </select>--%>
                                                     </div>
                                               <div class="col-xl-6">
                                                 <label class="form-control-label">Role<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList ID="ddlRole" class="custom-select form-control" runat="server"></asp:DropDownList>
                                                   <%-- <select name="country" class="custom-select form-control">
                                                        <option value="">Select</option>
                                                        <option value="AF">Afghanistan</option>
                                                        <option value="ZW">Zimbabwe</option>
                                                    </select>--%>
                                                   </div>
                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="Button2" class="btn btn-secondary" runat="server" Text="Create Staff"  OnClick="btnSave_OnClick" />
                                                </div>
                                            </div>


                                                   
                                            </div>
                                            </div>
                                            </form>
                                            <%--<div class="section-title mt-5 mb-5">
                                                <h4>Text goes here </h4>
                                            </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-4">
                                                    <label class="form-control-label">Dropdown<span class="text-danger ml-2">*</span></label>
                                                    <select name="country" class="custom-select form-control">
                                                        <option value="">Select</option>
                                                        <option value="AF">Afghanistan</option>
                                                        <option value="ZW">Zimbabwe</option>
                                                    </select>
                                                </div>
                                                <div class="col-xl-4">
                                                    <label class="form-control-label">Dropdown<span class="text-danger ml-2">*</span></label>
                                                    <select name="country" class="custom-select form-control">
                                                        <option value="">Select</option>
                                                        <option value="AF">Afghanistan</option>
                                                        <option value="ZW">Zimbabwe</option>
                                                    </select>
                                                </div>
                                                <div class="col-xl-4">
                                                    <label class="form-control-label">Dropdown<span class="text-danger ml-2">*</span></label>
                                                    <select name="country" class="custom-select form-control">
                                                        <option value="">Select</option>
                                                        <option value="AF">Afghanistan</option>
                                                        <option value="ZW">Zimbabwe</option>
                                                    </select>
                                                </div>
                                            </div>

                                            <div class="section-title mt-5 mb-5">
                                                <h4>Text goes here </h4>
                                            </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-4">
                                                    <label class="form-control-label">Dropdown<span class="text-danger ml-2">*</span></label>
                                                    <select name="country" class="custom-select form-control">
                                                        <option value="">Select</option>
                                                        <option value="AF">Afghanistan</option>
                                                        <option value="ZW">Zimbabwe</option>
                                                    </select>
                                                </div>
                                                <div class="col-xl-4">
                                                    <label class="form-control-label">Dropdown<span class="text-danger ml-2">*</span></label>
                                                    <select name="country" class="custom-select form-control">
                                                        <option value="">Select</option>
                                                        <option value="AF">Afghanistan</option>
                                                        <option value="ZW">Zimbabwe</option>
                                                    </select>
                                                </div>
                                                <div class="col-xl-4">
                                                    <label class="form-control-label">Dropdown<span class="text-danger ml-2">*</span></label>
                                                    <select name="country" class="custom-select form-control">
                                                        <option value="">Select</option>
                                                        <option value="AF">Afghanistan</option>
                                                        <option value="ZW">Zimbabwe</option>
                                                    </select>
                                                </div>
                                            </div>


                                            <div class="section-title mt-5 mb-5">
                                                <h4>Text goes here </h4>
                                            </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-4">
                                                    <label class="form-control-label">Dropdown<span class="text-danger ml-2">*</span></label>
                                                    <select name="country" class="custom-select form-control">
                                                        <option value="">Select</option>
                                                        <option value="AF">Afghanistan</option>
                                                        <option value="ZW">Zimbabwe</option>
                                                    </select>
                                                </div>
                                                <div class="col-xl-4">
                                                    <label class="form-control-label">Dropdown<span class="text-danger ml-2">*</span></label>
                                                    <select name="country" class="custom-select form-control">
                                                        <option value="">Select</option>
                                                        <option value="AF">Afghanistan</option>
                                                        <option value="ZW">Zimbabwe</option>
                                                    </select>
                                                </div>
                                                <div class="col-xl-4">
                                                    <label class="form-control-label">Dropdown<span class="text-danger ml-2">*</span></label>
                                                    <select name="country" class="custom-select form-control">
                                                        <option value="">Select</option>
                                                        <option value="AF">Afghanistan</option>
                                                        <option value="ZW">Zimbabwe</option>
                                                    </select>
                                                </div>
                                            </div>




                                            <div class="form-group row mb-3">
                                                <div class="col-xl-4 mb-3">
                                                    <label class="form-control-label">Text Goes Here<span class="text-danger ml-2">*</span></label>
                                                    <input type="text" value="" class="form-control" placeholder="see it">
                                                </div>
                                                <div class="col-xl-4 mb-5">
                                                    <label class="form-control-label">State<span class="text-danger ml-2">*</span></label>
                                                    <input type="email" value="CA" class="form-control" placeholder="input here">
                                                </div>
                                                <div class="col-xl-4">
                                                    <label class="form-control-label">Zip<span class="text-danger ml-2">*</span></label>
                                                    <input type="email" value="90045" class="form-control" placeholder="input">
                                                </div>
                                            </div>

                                            <div class="form-group row mb-5">
                                                <label class="col-lg-4 form-control-label d-flex justify-content-lg-end">Checkbox *</label>
                                                <div class="col-lg-5">
                                                    <div class="custom-control custom-checkbox styled-checkbox">
                                                        <input class="custom-control-input" type="checkbox" name="checkbox" id="check-1" required>
                                                        <label class="custom-control-descfeedback" for="check-1">Tick me</label>
                                                        <div class="invalid-feedback">
                                                            Tick me please!
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group row mb-5">
                                                <label class="col-lg-4 form-control-label d-flex justify-content-lg-end">Radios *</label>
                                                <div class="col-lg-1">
                                                    <div class="custom-control custom-radio styled-radio mb-3">
                                                        <input class="custom-control-input" type="radio" name="options" id="opt-01" required>
                                                        <label class="custom-control-descfeedback" for="opt-01">Option 1</label>
                                                        <div class="invalid-feedback">
                                                            Toggle this custom radio
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-lg-1">
                                                    <div class="custom-control custom-radio styled-radio mb-3">
                                                        <input class="custom-control-input" type="radio" name="options" id="opt-02" required>
                                                        <label class="custom-control-descfeedback" for="opt-02">Option 2</label>
                                                        <div class="invalid-feedback">
                                                            Or toggle this other custom radio
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="form-group row d-flex align-items-center mb-5">
                                                <label class="col-lg-3 form-control-label">Single Date</label>
                                                <div class="col-lg-4">
                                                    <div class="form-group">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">
                                                                <i class="la la-calendar"></i>
                                                            </span>
                                                            <input type="text" class="form-control" id="date" placeholder="Select value">
                                                        </div>
                                                    </div>
                                                </div>

                                                
                                            </div>--%>

                                            <%--<div class="form-group row d-flex align-items-center mb-5">
                                                <label class="col-lg-3 form-control-label">Single Date</label>
                                                <div class="col-lg-4">
                                                    <div class="form-group">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">
                                                                <i class="la la-calendar"></i>
                                                            </span>
                                                            <input type="text" class="form-control" id="date" placeholder="Select value">
                                                        </div>
                                                    </div>--%>
                                                </div>

                                                
                                            </div>

                                        
                                        </form>
                          
                            
                        </div>
</div>
                         
</asp:Content>