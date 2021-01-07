<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="TeacherViewAssignments.aspx.cs" Inherits="TeacherViewAssignments" %>

<%@ Import Namespace="PASSIS.DAO" %>
<%@ Import Namespace="PASSIS.LIB" %>
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
                                        <h4>SUBMITTED ASSIGNMENTS</h4>
                                    </div>
                                    <div class="widget-body"> 
                                        <form class="form-horizontal">

                                            <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label>
                                             <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                          <%--  <div class="form-group row mb-3">
                                                <div class="col-xl-6 mb-3">
                                                    <label class="form-control-label">Username:<span class="text-danger ml-2">*</span></label>
                         <asp:TextBox runat="server"  ID="txtUsername" class="form-control"> </asp:TextBox>
                                                </div>
                                              </div>
                                            --%>

                                               <%-- <div class="widget-body">
                                                <div class="row">
                                            <div class="col-xl-6">
                                                    <asp:Button ID="btnSearchStaff" class="btn btn-secondary"  OnClick="btnSearchStaff_OnClick" runat="server" Text="Search"  />
                                                </div>
                                            </div>--%>
                                                </form>    
                                        <br /><br />
      <table cellpadding="0" cellspacing="0" width="100%">
       <tr>
            <td>
                
            </td>
        </tr>
        <tr>
            <td align="left">
                 <asp:Label ID="lblUpload" Font-Size="Large" runat="server" Visible="true" ForeColor="Black" Text="LIST OF SUBMITTED ASSIGNMENTS"></asp:Label>        
              <asp:Panel ID="Panel2" Width="100%" class="table table-striped"  ScrollBars="Vertical" Height="500px" runat="server">
                                <asp:GridView ID="gdvList" runat="server" ForeColor="#333333"  class="table table-striped"  AutoGenerateColumns="false" GridLines="None" EmptyDataText=" No Assignment has been submitted"
                                    AllowPaging="true" Width="100%" OnRowCommand="gdvList_RowCommand" OnRowCancelingEdit="gdvList_RowCancelingEdit"
                                    OnRowEditing="gdvList_RowEditing1" OnRowUpdating="gdvList_RowUpdating" SkinID="gdvOverflow"
                                    OnPageIndexChanging="gdvList_PageIndexChanging">
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
                                        <asp:TemplateField HeaderText=" Student Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCode" runat="server" Text='<%# new UsersDAL().RetrieveUser(Convert.ToInt64(Eval("StudentId"))).FullName  %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownloaduouo" runat="server" CommandArgument='<%# Eval("StudentFileName") %>'
                                                    CommandName="cmd" SkinID="lnkGreen" class="btn btn-success"> View Submission </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false" HeaderText="Upload Assessment">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload" runat="server" CommandArgument='<%# Eval("StudentFileName") %>'
                                                    CommandName="cmdViewAssessment" SkinID="lnkGreen" Visible='<%# checkIfMarked(Eval("MarkStatus"))  %>'> View Assessment  </asp:LinkButton>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:FileUpload ID="FileUpload1" runat="server" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit" ShowHeader="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnedit" runat="server" CommandName="Edit" Text="Upload Assessed Script"
                                                    CommandArgument='<%# Eval("Id") %>' SkinID="lnkGreen" class="btn btn-success"></asp:LinkButton>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:LinkButton ID="btnupdate" runat="server" CommandName="Update" Text="Update"
                                                    SkinID="lnkGreen" CommandArgument='<%# Eval("Id") %>'></asp:LinkButton>
                                                <asp:LinkButton ID="btncancel" runat="server" CommandName="Cancel" Text="Cancel"
                                                    SkinID="lnkGreen" CommandArgument='<%# Eval("Id") %>'></asp:LinkButton>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Subject ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCodgfgfe" runat="server" Text='<%# new SubjectTeachersLIB().RetrieveSubject((long)new AssignmentLIB().RetrieveAssignmentById(Convert.ToInt64(Eval("AssignmentId")) ).SubjectId).Name %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Class /Group  ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCodefdd" runat="server" Text='<%# getClassOrGroupName(Eval("GradeId"), Eval("GroupId")) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Date Uploaded ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCoddfde" runat="server" Text='<%#  Eval("DateSubmitted") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssignmentId" runat="server" Text='<%# Eval("Id") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStudentUserId" runat="server" Text='<%# Eval("StudentId") %>' />
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
                                            
     <%--<asp:ObjectDataSource ID="objSchool" runat="server" SelectMethod="getAllSchools"
        TypeName="PASSIS.DAO.SchoolConfigDAL"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="objRole" runat="server" SelectMethod="getAllRolesSpecial" TypeName="PASSIS.DAO.RoleDAL"
        OnSelecting="objRole_Selecting">
        <SelectParameters>
            <asp:Parameter Name="parentId" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>     --%>
   
</asp:Content>
