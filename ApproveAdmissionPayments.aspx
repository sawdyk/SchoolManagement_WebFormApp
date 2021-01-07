<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="ApproveAdmissionPayments.aspx.cs" Inherits="ApproveAdmissionPayments" %>

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
                                        <h4>APROVE TRANSACTIONS</h4>
                                    </div>
                                    <div class="widget-body"> 
                                          <asp:Label ID="lblReport" runat="server" Visible="false"></asp:Label>
                    <asp:label runat="server" Text="Transaction List" ForeColor="Black" Font-Size="Large" Visible="true"></asp:label>
                                      <asp:GridView ID="gdvList" runat="server" ForeColor="Black" class="table table-striped"  AutoGenerateColumns="false" GridLines="None" EmptyDataText=" No record Currently exist "
                            AllowPaging="true" Width="100%" OnPageIndexChanging="gdvList_PageIndexChanging" OnRowCommand="gdvList_RowCommand" PageSize="30">
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
                                <asp:TemplateField HeaderText=" Pupil FullName ">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("Fullname")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Application Number">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName1" runat="server" Text='<%# Eval("ApplicantId")  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reference No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName2" runat="server" Text='<%# Eval("BankReferenceNo")  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName3" runat="server" Text='<%# Eval("AmountPaid")  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <div class="modal fade" id='<%# "mymodal" + Eval("ID") %>'>
                                            <div class="modal-dialog" style="width:100%">
                                                <div class="modal-content" style="width:1000px" >
                                                    <div class="modal-header">
                                                        <button type="button" class="close" data-dismiss="modal">
                                                            <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                                                        <h4 class="modal-title">Application Details</h4>
                                                    </div>
                                                    <div class="modal-body">
                                                        <table class="table table-striped table-hover" style="color:black">
                                                            <tr>
                                                                <td><strong>Fullname:</strong></td>
                                                                <td><%# Eval("Fullname") %></td>
                                                            </tr>
                                                            <tr>
                                                                <td><strong>Applicant Id:</strong></td>
                                                                <td><%# Eval("ApplicantId") %></td>
                                                            </tr>
                                                            <tr>
                                                                <td><strong>Reference No:</strong></td>
                                                                <td><%# Eval("BankReferenceNo") %></td>
                                                            </tr>
                                                            <tr>
                                                                <td><strong>Amount:</strong></td>
                                                                <td><%# Eval("AmountPaid") %></td>
                                                            </tr>
                                                            <tr>
                                                                <td><strong>Payment Channel:</strong></td>
                                                                <td><%# Eval("PaymentChannel") %></td>
                                                            </tr>
                                                            <tr>
                                                                <td><strong>Payment Date:</strong></td>
                                                                <td><%# Eval("PaymentDate") %></td>
                                                            </tr>
                                                            <tr>
                                                                <td><strong>Payment Status:</strong></td>
                                                                <td><%# Eval("PaymentStatus") %></td>
                                                            </tr>
                                                        </table>
                                                    </div>

                                                </div>
                                                <!-- /.modal-content -->
                                            </div>
                                            <!-- /.modal-dialog -->
                                        </div>
                                        <!-- /.modal -->

                                        <asp:LinkButton ID="targetbtn" runat="server" ToolTip="View Payment Details" data-toggle="modal" data-target='<%# "#mymodal" + Eval("ID") %>' CssClass="input-group input-group-lg"><i class="fa fa-search"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnApprove" runat="server" ToolTip="Approve Payment" CommandName="Approve" CommandArgument='<%# Eval("ID") %>'>Approve</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblId" runat="server" Text='<%# Eval("ID")  %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
<PagerSettings FirstPageText="&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-left&quot;&gt;&lt;/i&gt;" 
                        LastPageText="&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;&lt;i class=&quot;la la-chevron-right&quot;&gt;&lt;/i&gt;" 
                        Mode="NextPreviousFirstLast" 
                        NextPageText="&lt;i class=&quot;la la-arrow-right&quot;&gt;&lt;/i&gt;" 
                        PreviousPageText="&lt;i class=&quot;la la-arrow-left&quot;&gt;&lt;/i&gt;" />                          

                                      </asp:GridView>
                                                </div>
                                                </form>    
                                            </div>
                                            </div>

                                 </div>
                                </div>
                                </asp:Content>








