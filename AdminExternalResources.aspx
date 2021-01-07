<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Site.master" CodeFile="AdminExternalResources.aspx.cs" Inherits="AdminExternalResources" %>

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
                                        <h4>EXTERNAL RESOURCE UPLOAD</h4>
                                    </div>
                                   <div class="widget-body"> 
                                        <form class="form-horizontal">
                                 <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Session:<span class="text-danger ml-2">*</span></label>
                                            <asp:DropDownList runat="server" AutoPostBack="true" class="custom-select form-control" ID="ddlAcademicSession">
                                        </asp:DropDownList>
                                                </div>
                                                <div class="col-xl-6">
                                                    <label class="form-control-label">Term:<span class="text-danger ml-2">*</span></label>
                                        <asp:DropDownList runat="server" ID="ddlAcademicTerm" class="custom-select form-control" AutoPostBack="true">
                                        </asp:DropDownList>
                                                </div>
                                            </div>
                                              <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Class:<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlYear" AppendDataBoundItems="true"
                                            DataTextField="Name" DataValueField="Id" class="custom-select form-control" OnSelectedIndexChanged="ddlYear_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                                </div>
                                               <div class="col-xl-6">
                                                 <label class="form-control-label">Grade:<span class="text-danger ml-2">*</span></label>
                                                     <asp:DropDownList runat="server" ID="ddlGrade" class="custom-select form-control" DataTextField="GradeName" DataValueField="Id"
                                            AutoPostBack="true" AppendDataBoundItems="true">
                                        </asp:DropDownList>         
                                            </div>
                                            </div>
                                 <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Resource Type:<span class="text-danger ml-2">*</span></label>
                                                   <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlResourceType" AppendDataBoundItems="true"
                                            DataTextField="Name" DataValueField="Id" class="custom-select form-control" OnSelectedIndexChanged="ddlResourceType_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                                </div>
                                               <div class="col-xl-6">
                                       <label class="form-control-label">Description:<span class="text-danger ml-2">*</span></label>
                                      <asp:TextBox ID="txtDescription" Visible="true" class="form-control" runat="server"></asp:TextBox>
                                             </div>
                                            </div>
                                            <div class="form-group row mb-3">
                                                   <div class="col-xl-6 mb-3">
                                                        <asp:Label runat="server" Visible="false" ID ="lblLink" class="form-control-label">Link/Url:<span class="text-danger ml-2">*</span></asp:Label>
                                      <asp:TextBox ID="txtLink" Visible="false" class="form-control" runat="server"></asp:TextBox>
                                                  
                                       <asp:Label runat="server" Visible="false" ID="lblDocument" class="form-control-label">Upload Document:<span class="text-danger ml-2">*</span></asp:Label>
                                                  <asp:FileUpload class="btn btn-secondary" Visible="false" ID="resourceDocument" runat="server" />

                                         <asp:Label runat="server" Visible="false" ID="lblMultimediaUpload" class="form-control-label">Upload Audio/Video:<span class="text-danger ml-2">*</span></asp:Label>
                                          <asp:FileUpload class="btn btn-secondary" Visible="false" ID="multimedia" runat="server" />
                                                   
                                                </div>
                                              <div class="col-xl-6">
                                       <asp:Label runat="server" Visible="false" ID="lblMultimedia" class="form-control-label">Media Type:<span class="text-danger ml-2">*</span></asp:label>
                                        <asp:DropDownList runat="server" Visible="false" AutoPostBack="true" ID="ddlMediaType" AppendDataBoundItems="true"
                                            DataTextField="Name" DataValueField="Id" class="custom-select form-control">
                                        </asp:DropDownList>                                            

                                              </div>
                                            </div>
                                             <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSave" OnClick="btnSave_OnClick" class="btn btn-secondary" runat="server" Text="Save Resource"  />
                                                </div>
                                            </div>
                                           </form>
                                        <br />
                               <asp:Label ID="lblAcademic" Font-Size="Large" runat="server" Visible="true" ForeColor="Black" Text="EXTERNAL RESOURCES (LINK)"></asp:Label>
                              <asp:Panel runat="server" ID="pnlAssignmentUpload" ScrollBars="Vertical">
                                        <asp:GridView ID="gdvList" ForeColor="#333333" runat="server" PageSize="20" class="table table-striped" GridLines="None" AutoGenerateColumns="false" EmptyDataText="No Resource Created Yet!"
                    AllowPaging="true" Width="100%" OnRowCommand="gdvList_RowCommand" OnPageIndexChanging="gdvList_PageIndexChanging">
                    <AlternatingRowStyle BackColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#3366ff" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EFF3FB" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                    <Columns>
                        <asp:TemplateField HeaderText=" S/N">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Link Description ">
                            <ItemTemplate>
                                <asp:Label ID="lblLinkDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                           <asp:TemplateField HeaderText="Class">
                               <ItemTemplate>
                           <asp:Label ID="lblClass" runat="server" Text='<%# getClassName(Convert.ToInt64(Eval("ClassId")))  %>'></asp:Label>
                             </ItemTemplate>
                             </asp:TemplateField>
                           <asp:TemplateField HeaderText="Grade">
                              <ItemTemplate>
                           <asp:Label ID="lblGrade" runat="server" Text='<%# getGradeName(Convert.ToInt64(Eval("GradeId")))  %>'></asp:Label>
                           </ItemTemplate>
                          </asp:TemplateField>  
                        <asp:TemplateField HeaderText="Link">
                            <ItemTemplate>
                                <asp:HyperLink ID="lblLink" Target="_blank" ForeColor="#0066ff" runat="server" Text='<%# Eval("Link")  %>' NavigateUrl='<%# Eval("Link", "{0}") %>'></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="false">
                            <ItemTemplate>
                      <asp:LinkButton ID="lnkDownload342342" OnClientClick="javascript:return confirm('Are you sure you want to Delete this Resource?');" runat="server" CommandArgument='<%# Eval("Id") %>'
                         CommandName="remove" class="btn btn-danger"> Remove </asp:LinkButton>
                       </ItemTemplate>
                        </asp:TemplateField>                                       
                            </Columns>
                        <PagerSettings FirstPageText="&lt;i class=&quot;la  la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;"
                                LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" Mode="NextPreviousFirstLast" 
                                NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />                
                                      </asp:GridView>
                            </asp:Panel>

                                         <br />
                               <asp:Label ID="Label1" Font-Size="Large" runat="server" Visible="true" ForeColor="Black" Text="EXTERNAL RESOURCES (DOCUMENT)"></asp:Label>
                              <asp:Panel runat="server" ID="Panel1" ScrollBars="Vertical">
                                        <asp:GridView ID="gdvListDoc" ForeColor="#333333" runat="server" PageSize="20" class="table table-striped" GridLines="None" AutoGenerateColumns="false" EmptyDataText="No Resource Created Yet!"
                                        AllowPaging="true" Width="100%" OnRowCommand="gdvListDoc_RowCommand" OnPageIndexChanging="gdvListDoc_PageIndexChanging">
                                        <AlternatingRowStyle BackColor="White" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#3366ff" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#EFF3FB" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                       <Columns>
                                        <asp:TemplateField HeaderText=" S/N">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Description")  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Class">
                                            <ItemTemplate>
                                                <asp:Label ID="lblClass" runat="server" Text='<%# getClassName(Convert.ToInt64(Eval("ClassId")))  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Grade">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrade" runat="server" Text='<%# getGradeName(Convert.ToInt64(Eval("GradeId")))  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField ShowHeader="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload" runat="server" CommandArgument='<%# Eval("DocumentName") %>'
                                                    CommandName="download" class="btn btn-success"> Download </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload342342" OnClientClick="javascript:return confirm('Are you sure you want to Delete this Resource?');" runat="server" CommandArgument='<%# Eval("Id") %>'
                                                    CommandName="remove" class="btn btn-danger"> Remove </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIdfdfsdfs" runat="server" Text='<%# Eval("Id") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                     <PagerSettings FirstPageText="&lt;i class=&quot;la  la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;"
                                LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" Mode="NextPreviousFirstLast" 
                                NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />                
                                      </asp:GridView>
                            </asp:Panel>
                                        

                                       <br />
                               <asp:Label ID="Label2" Font-Size="Large" runat="server" Visible="true" ForeColor="Black" Text="EXTERNAL RESOURCES (MULTIMEDIA)"></asp:Label>
                              <asp:Panel runat="server" ID="pnlMultimedia" ScrollBars="Vertical">
                                        <asp:GridView ID="gdvMultimedia" ForeColor="#333333" runat="server" PageSize="20" class="table table-striped" GridLines="None" AutoGenerateColumns="false" EmptyDataText="No Resource Created Yet!"
                                        AllowPaging="true" Width="100%" OnRowCommand="gdvMultimedia_RowCommand" OnPageIndexChanging="gdvMultimedia_PageIndexChanging">
                                        <AlternatingRowStyle BackColor="White" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#3366ff" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#EFF3FB" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                       <Columns>
                                        <asp:TemplateField HeaderText=" S/N">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           
                                        <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description")  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Multimedia Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblName" runat="server" Text='<%# mediaType(Convert.ToInt64(Eval("MediaTypeId")))  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Class">
                                            <ItemTemplate>
                                                <asp:Label ID="lblClass" runat="server" Text='<%# getClassName(Convert.ToInt64(Eval("ClassId")))  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Grade">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGrade" runat="server" Text='<%# getGradeName(Convert.ToInt64(Eval("GradeId")))  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="View">
                                            <ItemTemplate>
                                        <asp:HyperLink ID="lblLink" ForeColor="#0066ff" runat="server" Text='View' NavigateUrl='<%# Eval("Id", "AdminViewMultimedia.aspx?id={0}") %>'></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload" runat="server" CommandArgument='<%# Eval("MediaName") %>'
                                                    CommandName="download" class="btn btn-success"> Download </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload342342" OnClientClick="javascript:return confirm('Are you sure you want to Delete this Resource?');" runat="server" CommandArgument='<%# Eval("Id") %>'
                                                    CommandName="remove" class="btn btn-danger"> Remove </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIdfdfsdfs" runat="server" Text='<%# Eval("Id") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                     <PagerSettings FirstPageText="&lt;i class=&quot;la  la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;"
                                LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" Mode="NextPreviousFirstLast" 
                                NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />                
                                      </asp:GridView>
                            </asp:Panel>


                                       <br />
                                            </div>
                                            </div>
                                                </div>
                                            </div>                                                                 
                                </asp:Content>
