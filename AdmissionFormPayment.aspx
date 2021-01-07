<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="AdmissionFormPayment.aspx.cs" Inherits="AdmissionFormPayment" %>

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
                                        <h4>ADMISSION PAYMENT</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">
                                            <asp:Label ID="lblReport" runat="server" Visible="false"></asp:Label>
                                            <asp:label runat="server" Text="Kindly select one of the payment method below to make payment" ForeColor="Black" Font-Size="Large" Visible="true"></asp:label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <asp:label runat="server" ID="lblApplicationNo" class="form-control-label">Application No:<span class="text-danger ml-2">*</span></asp:label>
                                                </div>
                                                 <div class="col-xl-6">
                                                    <asp:label runat="server" ID="Label1" class="form-control-label">Admission Form Fee:<span class="text-danger ml-2">*</span></asp:label>
                                                </div>
                                            </div>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <asp:label runat="server" ID="lblFormFee" class="form-control-label"><span class="text-danger ml-2">*</span></asp:label>
                                                </div>
                                                 <div class="col-xl-6">
                                               <asp:label ID="Label2" runat="server" class="form-control-label">Payment Channel:<span class="text-danger ml-2">*</span></asp:label>
                                <asp:DropDownList ID="ddlPaymentChannel"  class="custom-select form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPaymentChannel_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                                </div>
                                            </div>
                                           <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                           <asp:label runat="server" Visible="true" ID="lblRefNo" class="form-control-label">Reference No:<span class="text-danger ml-2">*</span></asp:label>
                                         <asp:TextBox ID="txtBankRef"   Visible="true" class="form-control" runat="server"></asp:TextBox>

                                                </div>
                                                 <div class="col-xl-6">
                                               <asp:label ID="lblPayDate" runat="server"  Visible="true" class="form-control-label">Payment Date:<span class="text-danger ml-2">*</span></asp:label>
                                                <asp:TextBox ID="txtPaymentDate"  Visible="true" placeholder="MM-dd-yyyy"  class="form-control"  runat="server" MaxLength="10"></asp:TextBox>
                                                   <ajaxToolKit:CalendarExtender ID="CalendarExtender1" Format="MM-dd-yyyy"  runat="server"  TargetControlID="txtPaymentDate" PopupButtonID="ImageButton1" PopupPosition="BottomLeft">
                                                                            </ajaxToolKit:CalendarExtender>
                                                </div>
                                            </div>
                                              <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnPay" OnClick="btnPay_Click" class="btn btn-secondary" runat="server" Text="Make Payment"  />
                                                </div>
                                            </div>
                                                </form>

                                         </div>
                                            </div>
                                            </div>
                                          </div>
                                            </div>                                        
                                </asp:Content>
                        
