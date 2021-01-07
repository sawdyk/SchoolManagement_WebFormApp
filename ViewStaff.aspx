<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="ViewStaff.aspx.cs" Inherits="ViewStaff" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
   <%--  <script type="text/javascript">
        $(function () {
            $('[id*=gdvList]').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                "responsive": true,
                "sPaginationType": "full_numbers"
            });
        });
    </script>--%>

          <div class="content-inner">
                    <div class="container-fluid">
                       
                        <div class="row flex-row" >
                            <div class="col-12">
                                <!-- Form -->
                                <div class="widget has-shadow">
                                    <div class="widget-header bordered no-actions d-flex align-items-center" >
                                        <h4>VIEW STAFF INFORMATION</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                            <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Username:<span class="text-danger ml-2">*</span></label>
                         <asp:TextBox runat="server"  ID="txtUsername"  placeholder="Enter Username to search for a specific Staff's information" class="form-control"> </asp:TextBox>
                                                </div>
                                              </div>
                                            

                                                <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSearchStaff" class="btn btn-secondary"  OnClick="btnSearchStaff_OnClick" runat="server" Text="Search"  />
                                                </div>
                                            </div>
                                                </form>    
                                        <br /><br />
      <table cellpadding="0" cellspacing="0" width="100%">
       <tr>
            <td>
                
            </td>
        </tr>
        <tr>
            <td align="left">
                                          <asp:Label ID="lblStaffs" ForeColor="Black" runat="server" Visible="true" Text="LIST OF STAFF"></asp:Label>
                                             <asp:Panel ID="Panel2" Width="100%" ScrollBars="Vertical" Height="500px" runat="server">
                               <asp:GridView ID="gdvList" EmptyDataText="NO STAFF" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" class="table table-striped"
                                                 CellPadding="4" ForeColor="#333333" GridLines="None"  Width="100%" Height="200px"  PageSize="20" OnPageIndexChanging="gdvList_PageIndexChanging" OnRowCommand="gdvList_RowCommand" >
                                   <%--OnRowCancelingEdit="gvCampus_RowCancelingEdit" 
                                                OnRowUpdating="gvCampus_RowUpdating" OnRowEditing="gvCampus_RowEditing"--%>
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                     <asp:TemplateField HeaderText=" S/N">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="USERNAME">
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="lblName" runat="server" ForeColor="#0066ff" Text='<%# Eval("Username")  %>' NavigateUrl='<%# Eval("Id", "StaffDetail.aspx?mode=view&id={0}") %>'></asp:HyperLink>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="FIRSTNAME">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtExamScore" Text='<%# Eval("Firstname")  %>' runat="server"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label2" Text='<%# Eval("Firstname")  %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="LASTNAME">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtClass" Text='<%# Eval("Lastname")  %>' runat="server" Enabled="false"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="IblClass" Text='<%# Eval("Lastname") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="EMAIL ADDRESS">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtClass" Text='<%# Eval("EmailAddress")  %>' runat="server" Enabled="false"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmail" Text='<%# Eval("EmailAddress") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="ROLE">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtClass" runat="server" Text='<%# getRoleFromUserRole(Eval("Id")) %>' Enabled="false"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRole" runat="server" Text='<%# getRoleFromUserRole(Eval("Id")) %>' ></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="CAMPUS">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtClass" Text='<%# getCampusName(Eval("SchoolCampusId")) %>' runat="server" Enabled="false"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCampus" Text='<%# getCampusName(Eval("SchoolCampusId")) %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ID" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblId" Text='<%# Eval("Id")  %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ShowHeader="False">
                                                        <EditItemTemplate>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("Id")  %>' CausesValidation="True" CommandName="Update" Text="Update" ToolTip="Update"><i class="fa fa-floppy-o fa-2x"></i></asp:LinkButton>
                                                            &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CommandArgument='<%# Eval("Id")  %>' CausesValidation="False" CommandName="Cancel" Text="Cancel" ToolTip="Cancel"><i class="fa fa-remove fa-2x"></i></asp:LinkButton>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButton1" CommandArgument='<%# Eval("Id")  %>' runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" ToolTip="Edit"><i class="fa fa-edit fa-2x"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                       <asp:TemplateField ShowHeader="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload342342" OnClientClick="javascript:return confirm('Are you sure you want to delete?');" runat="server" CommandArgument='<%# Eval("Id") %>'
                                                    CommandName="remove" class="btn btn-danger"> Delete </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                                </Columns>
        <%--                                        <EditRowStyle BackColor="#2461BF" />--%>
                                                <AlternatingRowStyle BackColor="White" />
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle BackColor="#3366ff" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <RowStyle BackColor="#EFF3FB" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                                <SortedDescendingHeaderStyle BackColor="#4870BE" /> 
                    <PagerSettings FirstPageText="&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                        LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                        Mode="NextPreviousFirstLast" 
                        NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" 
                        PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />
                                            </asp:GridView>
                                                 </asp:Panel>
                 </td>
    </tr>
    </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                     </div>
                                            
     <asp:ObjectDataSource ID="objSchool" runat="server" SelectMethod="getAllSchools"
        TypeName="PASSIS.DAO.SchoolConfigDAL"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="objRole" runat="server" SelectMethod="getAllRolesSpecial" TypeName="PASSIS.DAO.RoleDAL"
        OnSelecting="objRole_Selecting">
        <SelectParameters>
            <asp:Parameter Name="parentId" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>     
   
</asp:Content>