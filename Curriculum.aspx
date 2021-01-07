<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="Curriculum.aspx.cs" Inherits="Curriculum" %>

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
                                    <div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>VIEW CURRICULUM</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">
                                            <asp:SqlDataSource ID="dsSubjects" runat="server" ConnectionString="<%$ ConnectionStrings:PASSISConnectionString %>" SelectCommand="" ></asp:SqlDataSource>
                                            <asp:SqlDataSource ID="dsClass" runat="server" ConnectionString="<%$ ConnectionStrings:PASSISConnectionString %>" SelectCommand=""></asp:SqlDataSource>
                                            <asp:SqlDataSource ID="dsSubjectTheme" runat="server" ConnectionString="<%$ ConnectionStrings:PASSISConnectionString %>" SelectCommand="SELECT * FROM [Subject_Theme]"></asp:SqlDataSource>

                                            <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Grade(Class):<span class="text-danger ml-2">*</span></label>
                                    <asp:DropDownList runat="server" ID="ddlClass" class="custom-select form-control"  DataTextField="Name" DataValueField="Id" OnSelectedIndexChanged="ddlClass_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList> *
                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Subject:<span class="text-danger ml-2">*</span></label>
                                    <asp:DropDownList runat="server" ID="ddlSubject" class="custom-select form-control" DataSourceID="dsSubjects" DataTextField="Name" DataValueField="Id" OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList> *
                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Subject Theme:<span class="text-danger ml-2">*</span></label>
                                    <asp:DropDownList runat="server" ID="ddlTheme" class="custom-select form-control" DataSourceID="dsSubjectTheme" DataTextField="Title" DataValueField="Id" AppendDataBoundItems="true" ></asp:DropDownList> *
                                                   </div>

                                               </div>                                         
                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSearch" class="btn btn-secondary"  OnClick="btnSearch_Click" runat="server" Text="Search"  />
                                                </div>
                                            </div>
                                                </form>    
                                        <br /><br />
                                         <h3 id="searchTitle" runat="server"></h3>
                                 <%--<div class="widget-header bordered no-actions d-flex align-items-center">
                                        <h4>CLASS ARMS</h4>
                                 </div>--%>
                            <%--<asp:GridView ID="gvdAcademicSession" runat="server" AutoGenerateColumns="False" EmptyDataText=" No Calendar year currently exist" AllowPaging="true" Width="100%" ShowFooter="True" PageSize="20" GridLines="Both" OnRowCommand="gvdAcademicSession_RowCommand">       
                            <PagerSettings FirstPageText="&lt;i class=&quot;fa  fa-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-left&quot;&gt;&lt;/i&gt;"
                                LastPageText="&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;fa fa-chevron-right&quot;&gt;&lt;/i&gt;" Mode="NextPreviousFirstLast" 
                                NextPageText="&lt;i class=&quot;fa fa-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;fa fa-arrow-left&quot;&gt;&lt;/i&gt;" />--%>
                         <table class="table table-striped">
                            <tr>
                                <td align="center" colspan="5" >
                                    <h3 id="H1" runat="server"></h3>
                                </td>
                            </tr>
                            <tr>
                                <td align="right"  colspan = "5">
                        
                                        <table class="table table-striped">
                                        <thead>
                                            <tr>
                                                <th>Subject</th>
                                                <th>Theme</th>
                                                <th>Theme Topics</th>
                                                <%--<th>Topic Details</th>--%>
                                            </tr>
                                        </thead>
                                        <tbody runat="server" id="tbdCurriculum" >
                                            <tr>
                                                <td id="tdSubject" runat="server"></td>
                                                <td id="tdTheme" runat="server"></td>
                                                <td id="tdThemeTopic" runat="server"></td>
                                                <%--<td id="tdTopicDetails" runat="server"></td>--%>
                                            </tr>
                                        </tbody>
                                    </table>
                                    </td>
                                </tr>
                             </table>


                                            </div>
                                            </div>
                                                </div>
                                            </div>                                        

                                 </div>
                                </div>
        
                                </asp:Content>


