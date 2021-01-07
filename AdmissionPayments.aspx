<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="AdmissionPayments.aspx.cs" Inherits="AdmissionPayments" %>

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
                                        <h4>EXISTING APPLICANTS</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">
                                            <asp:Label ID="lblResponse" runat="server" Visible="false"></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <asp:label ID="Label1" Visible="true" runat="server" class="form-control-label">Application No:<span class="text-danger ml-2">*</span></asp:label>
                                                   <asp:TextBox ID="txtApplicationNo" Visible="true" class="form-control"  Width="50%" Height="40" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-xl-6">
                                                     <br />
                <asp:Button ID ="btnContinue" runat="server" Visible="true" class="btn btn-secondary" Text="Continue" OnClick="btnContinue_Click" />
                                                </div>       
                                                </div>
                                                                                     
                                                </form>  
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
    </div>
    
                                </asp:Content>
                        