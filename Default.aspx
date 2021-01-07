<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        #containerDiv
        {
	        background:#808080;
	        margin:	10px;
	        padding:	10px;
	        overflow:	auto;
	        width:	100%;
        }
        .col-sm-3
        {
            border: solid 1px #cccccc;
        }
        .col-sm-3:hover
        {
            box-shadow: 0px 0px 2px #808080;
            cursor: pointer;
        }
    </style>
    
    <!-- Begin Page Content -->
            <%--<div class="page-content d-flex align-items-stretch">
                <div class="default-sidebar">
                    <!-- Begin Side Navbar -->
                    <nav class="side-navbar box-scroll sidebar-scroll">
                        <!-- Begin Main Navigation -->
                        <ul class="list-unstyled">
                            <li class=""><a href="#dropdown-db" aria-expanded="true" data-toggle="collapse"><i class="la la-columns"></i><span> Home </span></a>
                            </li>
                        </ul>
                            <!-- <li>
                                <a href="#dropdown-tables" aria-expanded="false"><i class="la la-street-view"></i><span>Smart Learn</span>
                                </a>
                            </li>
                            <li>
                                <a href="#dropdown-tables" aria-expanded="false"><i class="la la-bar-chart"></i><span> Reports </span>
                                </a>
                            </li>
                            <li>
                                <a href="#dropdown-tables" aria-expanded="false"><i class="la la-graduation-cap"></i><span> Alumni</span>
                                </a>
                            </li>
                        </ul>  -->

                        <ul class="list-unstyled">
                            <li><a href="#dropdown-ui" aria-expanded="false" data-toggle="collapse"><i class="la la-sitemap"></i><span> Administration </span></a>
                                <ul id="dropdown-ui" class="collapse list-unstyled pt-0">
                                    <li><a href="#"> Student</a></li>
                                    <li><a href="#"> Classes</a></li>
                                    <li><a href="#"> Staff</a></li>
                                    <li><a href="#"> School</a></li>
                                </ul>
                            </li>
                            <li><a href="#dropdown-icons" aria-expanded="false" data-toggle="collapse"><i class="la la-institution"></i><span>Admission</span></a>
                                <ul id="dropdown-icons" class="collapse list-unstyled pt-0">
                                    <li><a href="#"> School Process </a></li>
                                    <li><a href="#"> Manage Application </a></li>
                                    <li><a href="#"> Scheduling </a></li>
                                    <li><a href="#">Enrolments </a></li>
                                    <li><a href="#">  Directory</a></li>
                                    <!-- <li><a href="#">Contribution Transactions</a></li> -->
                                </ul>
                            </li>

                            <li><a href="#dropdown-app" aria-expanded="false" data-toggle="collapse"><i class="la la-book"></i><span> Academics </span></a>
                                <ul id="dropdown-app" class="collapse list-unstyled pt-0">
                                    
                                    <li><a href="#"> Grading </a></li>
                                    <li><a href="#"> Syllabus and Notes</a></li>
                                    <li><a href="#"> Timetable </a></li>
                                    <li><a href="#"> Assignments  </a></li>
                                </ul>
                            </li>

                                <li><a href="#dropdown-authentication" aria-expanded="false" data-toggle="collapse"><i class="la la-money"></i><span>Finance and Fees</span></a>
                                    <ul id="dropdown-authentication" class="collapse list-unstyled pt-0">
                                        <li><a href="#"> Fees </a></li>
                                        <li><a href="#">Payments </a></li>
                                        <li><a href="#">Accounts</a></li>
                                        <li><a href="#">Reconcilliations</a></li>
                                        <!-- <li><a href="#">Accounts</a></li> -->
                                    </ul>
                                </li>

                            <li><a href="#dropdown-forms" aria-expanded="false" data-toggle="collapse"><i class="la la-smile-o"></i><span>Socials Collaborations </span></a>
                                <ul id="dropdown-forms" class="collapse list-unstyled pt-0">
                                    <li><a href="#">Forums</a></li>
                                    <li><a href="#">Chats</a></li>
                                    <li><a href="#">PTA</a></li>
                                    <li><a href="#">Notice Board</a></li>
                                    <li><a href="#">Bulletin</a></li>
                                </ul>
                            </li>
                            <li><a href="#dropdown-tables" aria-expanded="false" data-toggle="collapse"><i class="la la-th-large"></i><span>Services</span></a>
                                <ul id="dropdown-tables" class="collapse list-unstyled pt-0">
                                  <li><a href="#">Transport</a></li>
                                  <li><a href="#">Hostel</a></li>
                                  <li><a href="#">Feeding</a></li>
                                  <li><a href="#">Coaching</a></li>
                                </ul>
                            </li>
                            <li><a href="#dropdown-new" aria-expanded="false" data-toggle="collapse"><i class="la la-street-view"></i><span>Smart Learn</span></a>
                                <ul id="dropdown-new" class="collapse list-unstyled pt-0">
                                    <li><a href="#"> School Process </a></li>
                                    <li><a href="#"> Manage Application </a></li>
                                    <li><a href="#"> Scheduling </a></li>
                                    <li><a href="#"> Enrolments </a></li>
                                    <li><a href="#">  Directory</a></li>
                                    <!-- <li><a href="#">Contribution Transactions</a></li> -->
                                </ul>
                            </li>
                            <li><a href="#dropdown-new1" aria-expanded="false" data-toggle="collapse"><i class="la la-bar-chart"></i><span>Reports</span></a>
                                <ul id="dropdown-new1" class="collapse list-unstyled pt-0">
                                    <li><a href="#"> School Process </a></li>
                                    <li><a href="#"> Manage Application </a></li>
                                    <li><a href="#"> Scheduling </a></li>
                                    <li><a href="#"> Enrolments </a></li>
                                    <li><a href="#">  Directory</a></li>
                                    <!-- <li><a href="#">Contribution Transactions</a></li> -->
                                </ul>
                            </li>
                            <li><a href="#dropdown-new2" aria-expanded="false" data-toggle="collapse"><i class="la la-graduation-cap"></i><span>Alumni</span></a>
                                <ul id="dropdown-new2" class="collapse list-unstyled pt-0">
                                    <li><a href="#"> School Process </a></li>
                                    <li><a href="#"> Manage Application </a></li>
                                    <li><a href="#"> Scheduling </a></li>
                                    <li><a href="#"> Enrolments </a></li>
                                    <li><a href="#">  Directory</a></li>
                                    <!-- <li><a href="#">Contribution Transactions</a></li> -->
                                </ul>
                            </li>
                        </ul>
                        <!-- End Main Navigation -->
                    </nav>
                    <!-- End Side Navbar -->
                </div>--%>
                <!-- End Left Sidebar -->

                <!-- End Left Sidebar -->
                <div class="content-inner">
                    <div class="container-fluid">
                        <!-- Begin Page Header-->

                        <div id="containerDiv" runat="server" class="f"></div>

                        <!-- End Page Header -->
                        <!-- Begin Row -->
                        

                        
                          

                        <!-- End Row -->


</div>
</div>
</asp:Content>
