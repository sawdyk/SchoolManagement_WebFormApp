<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="StudentDetail.aspx.cs" Inherits="StudentDetail" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar" Namespace="RJS.Web.WebControl" TagPrefix="rjs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="PASSIS.LIB" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .style1 {
            width: 255px;
        }
    </style>
    <script runat="server">
    public string getHomeRoom(object IdObj)
    {
        string homeRoom = string.Empty;
        try
        {
            PASSIS.LIB.PASSISLIBDataContext cn = new PASSIS.LIB.PASSISLIBDataContext();
            PASSIS.LIB.GradeStudent gd = cn.GradeStudents.FirstOrDefault(g => g.StudentId == Convert.ToInt64(IdObj));
            PASSIS.LIB.Class_Grade cl = cn.Class_Grades.FirstOrDefault(c => c.Id == gd.ClassId);
            PASSIS.LIB.Grade gr = cn.Grades.FirstOrDefault(r => r.Id == gd.GradeId);
            homeRoom = string.Format("{0} {1}", cl.Name, gr.GradeName);
        }
        catch { }
        return homeRoom;
    }
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">  
    
    <asp:FormView ID="fvwUser" runat="server"   DataKeyNames="Id"
        Width="100%" onprerender="fvwUser_PreRender">
                <EditItemTemplate>
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
                                            <asp:Label ID="lblError" runat="server" Text="" Visible="false"></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text="" Visible="false"></asp:Label>
                                            <div class="widget has-shadow">
                                                <div class="widget-header bordered no-actions d-flex align-items-center">
                                                 <h4>EDIT STUDENT DETAILS</h4>
                                                </div>
                                             </div>
                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Choose Campus:<span class="text-danger ml-2">*</span></label>
                                                    <asp:DropDownList ID="ddlCampus" DataTextField="Name" DataValueField="Id" class="custom-select form-control"
                                                        DataSource='<%#  SchoolCampusList()  %>' SelectedValue = '<%# Eval("User.SchoolCampusId") %>' runat="server">
                                                    </asp:DropDownList>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Choose Accomodation Type:<span class="text-danger ml-2">*</span></label>
                                                <asp:DropDownList runat="server" class="custom-select form-control" ID="ddlAccomodationType" SelectedValue = '<%# Eval("User.AccomodationType") %>'>  
                                                    <asp:ListItem Text="Day Student" Value="Day Student"></asp:ListItem>
                                                    <asp:ListItem Text="Boarder " Value="Boarder"></asp:ListItem>
                                                </asp:DropDownList>                                                   

                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">FirstName:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtFirstName" Text='<%# Bind("User.FirstName")  %>' class="form-control" placeholder="FirstName"></asp:TextBox>
                                            
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label"> LastName:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtLastName" Text='<%# Bind("User.LastName")  %>' class="form-control" placeholder="LastName"></asp:TextBox>
                                                       
                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">MiddleName:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtMiddleName" Text='<%# Bind("User.MiddleName") %>' class="form-control" placeholder="MiddleName"></asp:TextBox>
                                                </div>
                                                <div class="col-xl-6">
                                                  <label class="form-control-label">Gender:<span class="text-danger ml-2">*</span></label>
                                                 <asp:DropDownList runat="server" class="custom-select form-control" ID="ddlGender">
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
                                             <asp:TextBox runat="server" ID="txtDateOfBirth" Text='<%# Eval("User.DateOfBirth", "{0:yyyy-MM-dd }") %>' class="form-control"  Columns="10" placeholder="Date of Birth"></asp:TextBox>
                                        <ajaxToolKit:CalendarExtender ID="myDateFromCal" Format="yyyy-MM-dd"  runat="server"  TargetControlID="txtDateOfBirth" PopupButtonID="ImageButton1" PopupPosition="BottomLeft"></ajaxToolKit:CalendarExtender>
                                                    </div>
                                                    </div>
                                          <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Street Address:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtStreetAddress" Text='<%# Bind("User.StreetAddress") %>' class="form-control" placeholder="Street Address"></asp:TextBox>
                                                </div>
                                                <div class="col-xl-6">
                                                  <label class="form-control-label">City:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtCity" Text='<%# String.Format("{0}  {1}",  Eval("User.City"), Eval("User.State"))  %>' class="form-control" placeholder="City"></asp:TextBox>
         
                                                    </div>
                                                    </div>
                                          <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Country:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtCountry" Text='<%# Bind("User.Country") %>' class="form-control" placeholder="Country"></asp:TextBox>
                                                </div>
                                                <div class="col-xl-6">
                                                  <label class="form-control-label">Alergies:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="TextBox9" class="form-control" placeholder="Alergies"></asp:TextBox>
         
                                                    </div>
                                                    </div>
                                          <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Bus Route:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtBusRoute" Text='<%# Bind("User.BusRoute") %>' class="form-control" placeholder="Bus Route"></asp:TextBox>
                                                </div>
                                                <div class="col-xl-6">
                                                  <label class="form-control-label">Status:<span class="text-danger ml-2">*</span></label>
                                                    <asp:DropDownList runat="server" class="custom-select form-control" AutoPostBack="true" ID="ddlStatus1" OnSelectedIndexChanged="ddlStatus1_SelectedIndexChanged">
                                                     <asp:ListItem Text=" ...Select... " Value="0"></asp:ListItem>
                                                    <asp:ListItem Text=" Active " Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="InActive " Value="2"></asp:ListItem>
                                                </asp:DropDownList>         
                                                    </div>
                                              <div class="col-xl-6 mb-3">
                                          <label class="form-control-label">Status(Category):<span class="text-danger ml-2">*</span></label>
                                                  <label class="form-control-label"><span class="text-danger ml-2">*</span></label>
                                                    <asp:DropDownList runat="server" class="custom-select form-control" ID="ddlStatus2">
                                                     <asp:ListItem Text=" ...Select... " Value="0"></asp:ListItem>
                                                </asp:DropDownList>        
                                                </div>
                                              <div class="col-xl-6">
                                               <label class="form-control-label">Admission Number:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtAdmissionNumber" Text='<%# Bind("User.AdmissionNumber") %>' class="form-control" placeholder="AdmissionNumber"></asp:TextBox>
                                                    </div>
                                              </div>
                                         
                                             
                                         <div class="widget has-shadow">
                                                <div class="widget-header bordered no-actions d-flex align-items-center">
                                                 <h4>EDIT FATHER DETAILS</h4>
                                                </div>
                                             </div>
                                        <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Fathers's Name:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtFathersName" Text='<%# Bind("ParentDetail.FathersName")  %>' class="form-control" placeholder="Father's Name"></asp:TextBox>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Nationality:<span class="text-danger ml-2">*</span></label>
                                                 <asp:TextBox runat="server" Text='<%# Bind("ParentDetail.FathersNationality")  %>' ID="txtFathersNationality" class="form-control" placeholder="Nationality"></asp:TextBox>
                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Phone Number:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtFatherPhoneNumber" Text='<%# Bind("ParentDetail.FathersPhoneNumber")  %>' class="form-control" placeholder="Occupation"></asp:TextBox>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label"> Office Address:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtFathersWorkAddress"  Text='<%# Bind("ParentDetail.FathersWorkAddress")  %>' class="form-control" placeholder="Office Address"></asp:TextBox>

                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Email Address:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtFathersEmail" Text='<%# Bind("ParentDetail.FathersEmail")  %>' class="form-control" placeholder="Youremail@gmail.com"></asp:TextBox>
                                                </div>
                                                <div class="col-xl-6">
                                                  <label class="form-control-label">Occupation:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtFathersTelephone" class="form-control" placeholder="Telephone"></asp:TextBox>
         
                                                    </div>
                                                    </div>

                                                 <br />
                                              <div class="widget has-shadow">
                                                <div class="widget-header bordered no-actions d-flex align-items-center">
                                                 <h4>EDIT MOTHER DETAILS</h4>
                                             </div>
                                                  </div>
                                                 <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Mother's Name:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtMothersName" Text='<%# Bind("ParentDetail.MothersName") %>' class="form-control" placeholder="Mother's Name"></asp:TextBox>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Mothers OtherName:<span class="text-danger ml-2">*</span></label>
                                                 <asp:TextBox runat="server" ID="txtMothersOtherName" Text='<%# Bind("ParentDetail.MothersNationality")  %>' class="form-control" placeholder="Nationality"></asp:TextBox>
                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Nationality:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtMothersNationality" class="form-control" placeholder="Occupation"></asp:TextBox>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label"> Office Address:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtMothersWorkAddress" Text='<%# Bind("ParentDetail.MothersWorkAddress")  %>' class="form-control" placeholder="Office Address"></asp:TextBox>

                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Email Address:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtMothersEmail" Text='<%# Bind("ParentDetail.MothersEmail")  %>' class="form-control" placeholder="Youremail@gmail.com"></asp:TextBox>
                                                </div>
                                                <div class="col-xl-6">
                                                  <label class="form-control-label">Telephone:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtMothersPhoneNumber" Text='<%# Bind("ParentDetail.MothersPhoneNumber")  %>' class="form-control" placeholder="Telephone"></asp:TextBox>
         
                                                    </div>
                                                    </div>

                                          <div class="widget has-shadow">
                                                <div class="widget-header bordered no-actions d-flex align-items-center">
                                                 <h4>EDIT OTHER INFORMATION</h4>
                                                    <br /><br />
                                                 <asp:Label ID="lblResponse" runat="server" Text="" Visible="false"></asp:Label>
                                                </div>
                                             </div>
                                        <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Guardian Name:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtGuardianDetails" Text='<%# Bind("ParentDetail.GuardianDetails") %>' class="form-control" placeholder="Name"></asp:TextBox>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Siblings:<span class="text-danger ml-2">*</span></label>
                                                 <asp:TextBox runat="server" ID="txtSiblings" Text='<%# Bind("ParentDetail.Siblings")  %>' class="form-control" placeholder="Home Address"></asp:TextBox>
                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Email Address:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtGuardianEmail" Text='<%# Bind("ParentDetail.GuardianEmail")  %>'  class="form-control" placeholder="Email Address"></asp:TextBox>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label"> Guardian's Phone Number:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtGuardianPhoneNumber" Text='<%# Bind("ParentDetail.GuardianPhoneNumber")  %>' class="form-control" placeholder="Telephone"></asp:TextBox>

                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Relationship To Student:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtGuardianRelationship" Text='<%# Bind("ParentDetail.GuardianRelationship")  %>' class="form-control" placeholder="Brother,Sister,Uncle..."></asp:TextBox>
                                                </div>
                                                    </div>

                                                 <br />
                                              <div class="widget has-shadow">
                                                <div class="widget-header bordered no-actions d-flex align-items-center">
                                                 <h4>EDIT LAST SCHOOL ATTENDED</h4>
                                                </div>
                                             </div>
                                                 <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">School Attended:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtLastSchoolAttended" Text='<%# Bind("User.LastSchoolAttended") %>' class="form-control" placeholder="School Name"></asp:TextBox>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Year:<span class="text-danger ml-2">*</span></label>
                                                 <asp:TextBox runat="server" class="form-control" ID="txtLastSchoolAttendedYear" Text='<%# Bind("User.LastSchoolAttendedYear") %>'> class="form-control" placeholder="Year"></asp:TextBox>
                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">City:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtLastSchoolAttendedCityCountry" Text='<%# Bind("User.LastSchoolAttendedCityCountry") %>' class="form-control" placeholder="City"></asp:TextBox>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Class:<span class="text-danger ml-2">*</span></label>
                                             <asp:TextBox runat="server" ID="txtLastSchoolAttendedClass" Text='<%# Bind("User.LastSchoolAttendedClass") %>' class="form-control" placeholder="Class"></asp:TextBox>

                                                  </div>
                                                </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                            <label class="form-control-label">Upload Passport:<span class="text-danger ml-2">*</span></label>
                                             <asp:FileUpload ID="documentUpload" runat="server" class="form-control" />
                                                       </div>
                                                   <div class="col-xl-6">
                                                                                                                   
                                              <label class="form-control-label">Maximum file dimension: 140 X 160; Maximum file size : 30Kb ; file type: .jpg<span class="text-danger ml-2">*</span></label>
                                             <asp:Checkbox runat="server" Text="Confirm Upload" ForeColor="#006600" Font-Names="tahoma" Font-Size="11px" ID="chkConfirmUpload" class="form-control"/>
                                                    <div style="color: #006600; font-family: tahoma; font-size: 11px;">
                                                      </div>
                                                       </div>
                                                    <br />
                                                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                   <asp:Button ID="Button2" class="btn btn-secondary" runat="server" Text="Update Details"  OnClick="btnUpdate_Click" />
                                                  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                   <asp:Button ID="Button1" class="btn btn-secondary" runat="server" Text="Cancel"  OnClick="btnCancel_Click" />


                                                    </div>
                                                 </div>
                                            </div>
                                                </div>
                                            </div>                                        
                                 </div>
                                </div>
                                     </EditItemTemplate>



             <ItemTemplate>
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
                                              <div class="widget has-shadow">
                                                <div class="widget-header bordered no-actions d-flex align-items-center">
                                                 <h4>STUDENT DETAILS</h4>
                        &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                       <asp:HyperLink ID="hypLnkReturnToList" runat="server" NavigateUrl="~/AdminViewStudents.aspx"><i class="la la-mail-reply"></i> Return to List </asp:HyperLink>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:HyperLink runat="server" ID="hypLnkEdit" NavigateUrl='<%# Eval("StudentId", "StudentDetail.aspx?mode=edit&id={0}") %>'><i class="la la-pencil"></i> Edit</asp:HyperLink>
                                                </div>
                                             </div>
                <asp:Image ID="Image1" Style="float: right" runat="server" ImageUrl='<%# String.Format("~/Passports/{0}", Eval("User.PassportFileName")) %>' />

                                            <asp:Label runat="server" ID="lbldjd" ForeColor="Blue" Font-Size="Large" Font-Bold="true" Text='<%#  Bind("User.StudentFullName") %>' class="form-control-label"></asp:Label>
                                            <br /><br />
                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">School Campus:<span class="text-danger ml-2">*</span></label>&nbsp;&nbsp;&nbsp;
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" Text='<%#  new AcademicSessionLIB().RetrieveSchoolCampus((Int64)Eval("User.SchoolCampusId")).Name   %>' class="form-control-label"></asp:Label>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Accomodation Type:<span class="text-danger ml-2">*</span></label>
                                                  <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="lblAccomodationtype" class="form-control-label" Text='<%# Bind("User.AccomodationType") %>'> </asp:Label>

                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">FirstName:<span class="text-danger ml-2">*</span></label>
                                               <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="lblFirstName" Text='<%# Bind("User.FirstName")  %>' class="form-control-label"></asp:Label>

                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label"> LastName:<span class="text-danger ml-2">*</span></label>
                                                 <asp:Label runat="server"  ForeColor="Blue" Font-Bold="true" ID="lblFamilyName" class="form-control-label" Text='<%# Bind("User.LastName")  %>'> </asp:Label>

                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">MiddleName:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="lblsjhsd" Text='<%# Bind("User.MiddleName") %>' class="form-control-label"></asp:Label>
                                                </div>
                                                <div class="col-xl-6">
                                                  <label class="form-control-label">Gender:<span class="text-danger ml-2">*</span></label>
                                                   <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="Label18" Text='<%#  (PASSIS.DAO.Utility.Gender) Eval("User.Gender") %>' class="form-control-label"></asp:Label>
  
                                                    </div>
                                                    </div>
                                          <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">AdmissionNumber:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="Label38" Text='<%#  Eval("User.AdmissionNumber") %>' class="form-control-label"></asp:Label>
                                                </div>
                                                <div class="col-xl-6">
                                                  <label class="form-control-label">Date of Birth:<span class="text-danger ml-2">*</span></label>
                                            <asp:Label runat="server" ID="Label19" ForeColor="Blue" Font-Bold="true" Text='<%# Eval("User.DateOfBirth", "{0:dd-MMM-yyyy }") %>' class="form-control-label"></asp:Label>

                                                    </div>
                                                    </div>
                                          <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Street Address:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ID="Label39" ForeColor="Blue" Font-Bold="true" Text='<%# Bind("User.StreetAddress") %>' class="form-control-label"></asp:Label>
                                                </div>
                                                <div class="col-xl-6">
                                                  <label class="form-control-label">City/State:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ID="Label41" ForeColor="Blue" Font-Bold="true" Text='<%# Eval("User.City") + "   " +   Eval("User.State") %>' class="form-control-label"></asp:Label>
         
                                                    </div>
                                                    </div>
                                          <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Country:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="Label43" Text='<%# Bind("User.Country") %>' class="form-control-label"></asp:Label>
                                                </div>
                                                <div class="col-xl-6">
                                                  <label class="form-control-label">UserName:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="Label63237" Text='<%# Eval("User.Username") %>' class="form-control-label"></asp:Label>         
                                                    </div>
                                                    </div>
                                          <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Year:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="Label61" Text='<%# getHomeRoom(Eval("User.Id")) %>' class="form-control-label"></asp:Label>
                                                </div>
                                              <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Learning Support:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="Label64" Text='<%# (PASSIS.DAO.Utility.LearningSupport) Eval("User.IsLearningSupport") %>' class="form-control-label"></asp:Label>
                                                </div>
                                              <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Bus Route:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="txtBusRoute" Text='<%# Bind("User.BusRoute") %>' class="form-control-label"></asp:Label>
                                                </div>
                                              <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Status:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="txtStatus" Text='<%# StudentStatus(Convert.ToInt64(Eval("User.Id"))) %>' class="form-control-label"></asp:Label>
                                                </div>
                                                    </div>
                                            <div class="widget has-shadow">
                                                <div class="widget-header bordered no-actions d-flex align-items-center">
                                                 <h4>Father's Details</h4>
                                                </div>
                                             </div>
                                        <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Fathers's Name:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="lblghere" Text='<%# Bind("ParentDetail.FathersName")  %>' class="form-control-label"></asp:Label>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Nationality:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="Label24" Text='<%# Bind("ParentDetail.FathersNationality")  %>' class="form-control-label"></asp:Label>
                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Occupation:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="lbl232" Text='<%# Bind("ParentDetail.FathersWorkAddress")  %>' class="form-control-label"></asp:Label>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label"> Office Address:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="Label21" Text='<%# Bind("ParentDetail.FathersWorkAddress")  %>' class="form-control-label"></asp:Label>

                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Email Address:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="Label22" Text='<%# Bind("ParentDetail.FathersEmail")  %>' class="form-control-label"></asp:Label>
                                                </div>
                                                <div class="col-xl-6">
                                                  <label class="form-control-label">Phone Number:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="Label20" Text='<%# Bind("ParentDetail.FathersPhoneNumber")  %>' class="form-control-label"></asp:Label>
         
                                                    </div>
                                                    </div>

                                              <div class="widget has-shadow">
                                                <div class="widget-header bordered no-actions d-flex align-items-center">
                                                 <h4>Mother's Details</h4>
                                                </div>
                                             </div>
                                                 <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Mother's Name:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="Label26"  Text='<%# Bind("ParentDetail.MothersName")  %>' class="form-control-label"></asp:Label>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Nationality:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="Label34"  Text='<%# Bind("ParentDetail.MothersNationality")  %>' class="form-control-label"></asp:Label>
                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Occupation:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="Label2121" Text='<%# Bind("ParentDetail.MothersWorkAddress")  %>' class="form-control-label"></asp:Label>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label"> Office Address:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="Label30" Text='<%# Bind("ParentDetail.MothersWorkAddress")  %>' class="form-control-label"></asp:Label>

                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Email Address:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="Label32" Text='<%# Bind("ParentDetail.MothersEmail")  %>' class="form-control-label"></asp:Label>
                                                </div>
                                                <div class="col-xl-6">
                                                  <label class="form-control-label">Phone Number:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="Label68" Text='<%# Bind("ParentDetail.MothersPhoneNumber")  %>' class="form-control-label"></asp:Label>
         
                                                    </div>
                                                    </div>
                                              <div class="widget has-shadow">
                                                <div class="widget-header bordered no-actions d-flex align-items-center">
                                                 <h4>Other Information</h4>
                                                </div>
                                             </div>
                                        <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label"> Guardian's Name:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="Label49" Text='<%# Bind("ParentDetail.GuardianDetails")   %>' class="form-control-label"></asp:Label>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Home Address:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="Label5222" Text='<%# Bind("ParentDetail.GuardianEmail")  %>' class="form-control-label"></asp:Label>
                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Email Address:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="Label55" Text='<%# Bind("ParentDetail.GuardianEmail")  %>' class="form-control-label"></asp:Label>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label"> Telephone:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="Label51" Text='<%# Bind("ParentDetail.GuardianPhoneNumber")  %>' class="form-control-label"></asp:Label>

                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                   <label class="form-control-label">Relationship To Student:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="Label53" Text='<%# Bind("ParentDetail.GuardianRelationship")  %>' class="form-control-label"></asp:Label>
                                                </div>
                                                    </div>

                                              <div class="widget has-shadow">
                                                <div class="widget-header bordered no-actions d-flex align-items-center">
                                                 <h4>Last School Attended</h4>
                                                </div>
                                             </div>
                                                 <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">School Name:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="lbl3344" Text='<%# Bind("User.LastSchoolAttended") %>' class="form-control-label"></asp:Label>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Year:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="Label1764" Text='<%# Bind("User.LastSchoolAttendedYear") %>' class="form-control-label"></asp:Label>
                                                  </div>
                                               </div>
                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">City:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="Label17" Text='<%# Bind("User.LastSchoolAttendedCityCountry") %>' class="form-control-label"></asp:Label>
                                                </div>
                                                  <div class="col-xl-6">
                                                 <label class="form-control-label">Class:<span class="text-danger ml-2">*</span></label>
                                                       <asp:Label runat="server" ForeColor="Blue" Font-Bold="true" ID="lblb434" Text='<%# Bind("User.LastSchoolAttendedClass") %>' class="form-control-label"></asp:Label>

                                                  </div>
                                                
                                  
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
                                        </form>

                                 </div>
                                </div>
                     </div>
                                </ItemTemplate>
                                 </asp:FormView>
     <asp:ObjectDataSource ID="objStudent" runat="server" DataObjectTypeName="PASSIS.DAO.User"
        SelectMethod="RetrieveUser" TypeName="PASSIS.DAO.UsersDAL">
        <SelectParameters>
            <asp:QueryStringParameter DefaultValue="0" Name="id" QueryStringField="Id" Type="Int64" />
        </SelectParameters>
    </asp:ObjectDataSource>
                                </asp:Content>
