<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="AdminResourceUpload.aspx.cs" Inherits="AdminResourceUpload" %>

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
                                        <h4>RESOURCE UPLOAD</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text="" class="text-danger ml-2"></asp:Label>
                                          <asp:Label ID="lblResultMsg" runat="server" Text="" class="text-success ml-2"></asp:Label>
                              <asp:Label ID="Label1" Font-Size="Large" runat="server" Visible="true" ForeColor="Black" Text="INTERNAL RESOURCE CENTER"></asp:Label>

                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Short File Description:<span class="text-danger ml-2">*</span></label>
                                                    <asp:TextBox ID="txtDesc"  class="form-control" TabIndex="25"  runat="server" MaxLength="10"></asp:TextBox>

                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Upload File:<span class="text-danger ml-2">*</span></label>
                                             <asp:FileUpload ID="documentUpload" class="form-control" runat="server" />

                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <asp:Button ID="btnSave" OnClick="btnSave_Click" class="btn btn-secondary" runat="server" Text="Save"  />
                                                       
                                                </div>
                                                      </div>
                               <asp:Label ID="lblDisplay" Font-Size="Large" runat="server" Visible="true" ForeColor="Black" Text="EXTERNAL RESOURCE CENTER"></asp:Label>

                                                     <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Link Description:<span class="text-danger ml-2">*</span></label>
                                                    <asp:TextBox placeholder="Chemical Reaction" ID="txtLinkDesc"  class="form-control" runat="server" ></asp:TextBox>

                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Link:<span class="text-danger ml-2">*</span></label>
                                                    <asp:TextBox placeholder="http://www.learnchemistry.com" ID="txtLink"  class="form-control" runat="server"></asp:TextBox>

                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                  <asp:Button ID="btnSaveLink" OnClick="btnSaveLink_Click" class="btn btn-secondary" runat="server" Text="Save"  />

                                                </div>
                                                      </div>

                                                </form>    
                                        <br /><br />
                              
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
                        
                                </asp:Content>

