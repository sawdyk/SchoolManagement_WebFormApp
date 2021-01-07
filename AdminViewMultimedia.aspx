<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="AdminViewMultimedia.aspx.cs" Inherits="AdminViewMultimedia" %>

<%@ Import Namespace="PASSIS.DAO" %>
<%@ Import Namespace="PASSIS.LIB" %>
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
    <div class="content-inner">
                    <div class="container-fluid">
                        <div class="row flex-row" style="width:auto;">
                            <div class="col-12">
                                <!-- Form -->
                                <div class="widget has-shadow">
                                    <div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>EXTERNAL RESOURCE MULTIMEDIA</h4>
                                    </div>
                                   <div class="widget-body"> 
                                        <form class="form-horizontal">
                                    <h4>DESCRIPTION: </h4>  <asp:Label ID="lblDescription" Font-Size="Large"  runat="server" Visible="true" ForeColor="Black"></asp:Label>
                                          <div id="containerDiv" runat="server"></div>
                                            
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                                                 
                                </asp:Content>

