<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="BasePage.aspx.cs" Inherits="BasePage" %>

<asp:Content ID="Content1" MasterPageFile="~/Site.master" ContentPlaceHolderID="MainContent" runat="Server">



    <style type="text/css">


        .sizeof10f3{float:left; width:32%; height:20%;}


    </style>



    <div class="content-inner">
        <div class="container-fluid">
            <!-- Begin Page Header-->
            <div class="row">
                <div class="page-header">
                    <div class="d-flex align-items-center">
                        <h2 class="page-header-title">My Dashboard</h2>
                        <div>
                            <!-- <div class="page-header-tools">
	                                    <a class="btn btn-gradient-01" href="#">Login</a>
	                                </div> -->
                        </div>
                    </div>
                </div>
            </div>
            <!-- End Page Header -->
            <!-- Begin Row -->
            <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div>
                            <asp:Repeater runat="server" ID="Repeater1">
                                <HeaderTemplate>
                                    <ul>
                                </HeaderTemplate>
                                <ItemTemplate>--%>

           <%-- <div class="row flex-row">
                <!-- Begin Contribution -->
                <div class="col-xl-4 col-md-6 col-sm-6">
                    <div class="widget widget-12 has-shadow">
                        <div class="widget-body">
                            <div class="media">
                                <div class="align-self-center ml-5 mr-5">
                                    <i class="ion-ios-people"></i>
                                </div>
                                <div class="media-body align-self-center">
                                    <div class="title">Students</div>
                                    <div class="number">60</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>


               </div>
            </div>--%>
                   <%-- </ItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>--%>
            <div id="containerDivvvs" runat="server" class="f">
                  

                        </div>
                            </div>

                <!-- End Contribution -->
                <!-- Begin Member -->
                <%--<div class="col-xl-4 col-md-6 col-sm-6">
                    <div class="widget widget-12 has-shadow">
                        <div class="widget-body">
                            <div class="media">
                                <div class="align-self-center ml-5 mr-5">
                                    <i class="ion-person"></i>
                                </div>
                                <div class="media-body align-self-center">
                                    <div class="title">Staff</div>
                                    <div class="number">10</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- End Member -->
                <!-- Begin Amount -->
                <div class="col-xl-4 col-md-6 col-sm-6">
                    <div class="widget widget-12 has-shadow">
                        <div class="widget-body">
                            <div class="media">
                                <div class="align-self-center ml-5 mr-5">
                                    <i class="ion-clipboard"></i>
                                </div>
                                <div class="media-body align-self-center">
                                    <div class="title">Admission</div>
                                    <div class="number">3</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>--%>
                <!-- End Amount -->
            </div>
     </div>
    <!-- End Row -->
</asp:Content>

