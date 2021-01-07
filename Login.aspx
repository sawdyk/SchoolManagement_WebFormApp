<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8">
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <title>Passis</title>
        <meta name="description" content="Cooprative is a user friendly application which can help members contribute ">
        <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
        <!-- Google Fonts -->
        <script src="https://ajax.googleapis.com/ajax/libs/webfont/1.6.26/webfont.js"></script>
        <script>
          WebFont.load({
            google: {"families":["Montserrat:400,500,600,700","Noto+Sans:400,700"]},
            active: function() {
                sessionStorage.fonts = true;
            }
          });
        </script>
        <!-- Favicon -->
        <link rel="apple-touch-icon" sizes="180x180" href="assets/img/apple-touch-icon.png">
        <link rel="icon" type="image/png" sizes="32x32" href="assets/img/passis_logoB_2_32x32.png">
        <link rel="icon" type="image/png" sizes="16x16" href="assets/img/passis_logoB_1_16x16.png">
        <!-- Stylesheet -->
        <link rel="stylesheet" href="assets/vendors/css/base/bootstrap.min.css">
        <link rel="stylesheet" href="assets/vendors/css/base/elisyam-1.5.min.css">
        <!-- Tweaks for older IEs--><!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
        <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script><![endif]-->
        <style type="text/css" media="screen">
             #headline1 {
            background-image: url(assets/img/passisbrgd1.jpg);
            
        }
        .stretch {
            width:100%;
            height:100%;
        }
        </style>
    </head>
    <body >
        <div id="headline1" class="stretch">
        <!-- Begin Preloader -->
        <div id="preloader">
            <div class="canvas">
                <img src="assets/img/passis_logoB.png" alt="logo" class="loader-logo">
                <div class="spinner"></div>   
            </div>
        </div>
        <!-- End Preloader -->
        <!-- Begin Section -->
        <div class="container-fluid h-100 overflow-y">
            <div class="row flex-row h-100">
                <div class="col-12 my-auto">
                    <div class="lock-form mx-auto">
                        <div class="">
                            <img src="assets/img/avatar/passis_logoB.png" alt="...">
                        </div>
                        <h3>Sign In</h3>    
                      <form name="form" runat="server">
                            <div class="group material-input">
          <asp:TextBox ID="txtUsername" runat="server" required="true" class="md-input" ng-model="user.email"></asp:TextBox>
<%--							    <input type="email" required>--%>
							    <span class="highlight"></span>
							    <span class="bar"></span>
							    <label>Username</label>
                            </div>
                            <div class="group material-input">
          <asp:TextBox TextMode="Password" ID="txtPassword" runat="server" required="true" ng-model="user.password"></asp:TextBox>
                                <span class="highlight"></span>
                                <span class="bar"></span>
                                <label>Password</label>
                            </div>
                            
                        
                        <div class="button text-center">
<asp:Button type="submit" id="btnSubmit" runat="server" class="btn btn-lg btn-outline-success mr-1 mb-2" Text="Login" OnClick="btnSubmit_OnClick"></asp:Button>
<%--                            <a href="signup.html" class="btn btn-lg btn-outline-success mr-1 mb-2" >--%>
<%--                               Login--%>
                            </a>
                        </div>
         <asp:Label ID="lblErrorMsg" runat="server" Text="" ForeColor="red" style="text-align: justify"></asp:Label>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="button text-center">
                                    <a href="Self_Setup/Default.aspx" class="btn btn-sm btn-outline-info mr-1 mb-2"> Set Up a School </a>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="button text-center">
                                    <a href="ForgotPassword.aspx" class="btn btn-sm btn-outline-danger mr-1 mb-2"> Forgot Password </a>
                                </div>
                                </form>
                            </div>
                        </div>
                    </div>      
                </div>
            </div>
            <!-- End Container -->
        </div>  
        <!-- End Section -->  
        <!-- Begin Vendor Js -->
        <script src="assets/vendors/js/base/jquery.min.js"></script>
        <script src="assets/vendors/js/base/core.min.js"></script>
        <!-- End Vendor Js -->
        <!-- Begin Page Vendor Js -->
        <script src="assets/vendors/js/app/app.min.js"></script>
        <!-- End Page Vendor Js -->
        </div>
    </body>
</html>