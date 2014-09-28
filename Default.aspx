<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Scheduler HD</title>

    <meta name="description" content="Scheduler HD, Your schedule, done right." />
    <meta name="keywords" content="Scheduler,HD,SchedulerHD,Park,Tudor,PT,Schedule,Jason,Chris,Zhao,Gregory,2013,CS,Web,Design" />
    <meta name="author" content="Chris Gregory & Jason Zhao" />

    <link rel="icon" type="image/ico" href="images/calendar_favicon.ico" />
    <link rel="shortcut icon" href="images/calendar_favicon.ico" />
    <link href="css/reset.css" rel="stylesheet" type="text/css" />
    <link href="css/main.css" rel="stylesheet" type="text/css" />
</head>
<body id="welcome-body">
    <form id="form1" runat="server">
    <div id="wrapper">
        <div id="main-header-ribbon">
            <a id="main-header-title-link" href="Home.aspx">
                <div id="main-header-text">Scheduler HD</div>
            </a>
        </div>

        <div id="welcome-content">
            <div id="welcome-about">
                <div id="welcome-about-header">
                    Your schedule, done right.</div>
                <div id="welcome-about-text">
                    Welcome to Scheduler HD. Viewing and sharing schedules has never been
                    easier or quicker.  Sign up now to get in on the action!</div>
            </div>

            <div id="welcome-login">
                <div id="welcome-login-header">Log In</div>
                <asp:TextBox ID="tbLoginUsername" CssClass="login-field" runat="server" placeholder="Username"
                    required="required"></asp:TextBox>
                <asp:TextBox ID="tbLoginPassword" CssClass="login-field" runat="server" placeholder="Password"
                    TextMode="Password" required="required"></asp:TextBox>
                <a id="linkSignUp" href="Signup.aspx">Sign up</a>
                <asp:Button ID="btnLogin" runat="server" Text="Start Sharing" OnClick="btnLogin_Click" />
            </div>

            <div id="copyright-text" title="Copyright 2013 &copy; Chris Gregory & Jason Zhao. All rights reserved.">Scheduler HD &copy; 2013</div>
        </div>
    </div>
    </form>
</body>
</html>
