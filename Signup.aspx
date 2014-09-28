<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Signup.aspx.cs" Inherits="Signup" %>

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
<body id="page-body">
    <form id="form1" runat="server">
    <div id="wrapper">
        <div id="main-header-ribbon">
            <a id="main-header-title-link" href="Home.aspx">
                <div id="main-header-text">
                    Scheduler HD</div>
            </a>
            <div id="main-header-menu-div">
            </div>
        </div>
        <div id="page-content">
            <div id="signup-div">
                <div id="signup-header">Sign Up</div>
                <hr class="signup-hr" />
                <table id="signup-table">
                    <tr>
                        <td><asp:TextBox ID="tbSignupFName" CssClass="signup-login-field" runat="server" placeholder="First Name (optional)">
                        </asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:TextBox ID="tbSignupLName" CssClass="signup-login-field" runat="server" placeholder="Last Name (optional)">
                        </asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:TextBox ID="tbSignupUsername" CssClass="signup-login-field" runat="server" placeholder="Username"
                         required="required"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:TextBox ID="tbSignupPassword" CssClass="signup-login-field" runat="server" placeholder="Password"
                         required="required" TextMode="Password"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:TextBox ID="tbSignupPasswordConfirm" CssClass="signup-login-field" runat="server" placeholder="Confirm Password"
                         required="required" TextMode="Password"></asp:TextBox></td>
                    </tr>
                </table>
                <asp:CheckBox ID="chkAgree" runat="server" /><span id="terms-text">I agree to the <a id="terms-link" href="legal/tos.txt">Terms and Conditions</a></span>
                <asp:Button ID="btnSignup" runat="server" Text="Sign Up" onclick="btnSignup_Click" />
                <asp:Label ID="lblPasswordConfirm" runat="server"></asp:Label>
                <br clear="all" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
