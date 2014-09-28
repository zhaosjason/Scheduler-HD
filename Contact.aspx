<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Contact.aspx.cs" Inherits="Contact" %>

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
                <div id="main-header-menu-icon" onclick="#main-menu">
                </div>
                <div id="main-menu">
                    <asp:Button CssClass="main-menu-button" ID="btnHome" formnovalidate="formnovalidate" runat="server" Text="Home" OnClick="menuBtn_Click" />
                    <asp:Button CssClass="main-menu-button" ID="btnSchedule" formnovalidate="formnovalidate" runat="server" Text="Schedule"
                        OnClick="menuBtn_Click" />
                    <asp:Button CssClass="main-menu-button" ID="btnFriends" formnovalidate="formnovalidate" runat="server" Text="Friends"
                        OnClick="menuBtn_Click" />
                    <asp:Button CssClass="main-menu-button" ID="btnSettings" formnovalidate="formnovalidate" runat="server" Text="Settings"
                        OnClick="menuBtn_Click" />
                    <asp:Button CssClass="main-menu-button" ID="btnContact" formnovalidate="formnovalidate" runat="server" Text="Contact"
                        OnClick="menuBtn_Click" />
                    <asp:Button CssClass="main-menu-button" ID="btnLogout" formnovalidate="formnovalidate" runat="server" Text="Logout"
                        OnClick="menuBtn_Click" />
                </div>
            </div>
        </div>
        <div id="page-content">
            <br />
            <div id="contact-forms">
                <div id="contact-forms-header">
                    Contact Us
                </div>
            <asp:TextBox ID="tbName" CssClass="contact-field" runat="server" placeholder="Name"
                required="required"></asp:TextBox>
            <asp:TextBox ID="tbEmail" CssClass="contact-field" runat="server" placeholder="Email"
                required="required"></asp:TextBox>
            <asp:TextBox ID="tbPhone" CssClass="contact-field" runat="server" placeholder="Phone (optional)"></asp:TextBox>
            <asp:TextBox ID="tbMessage" CssClass="contact-field" runat="server" placeholder="Message"
                required="required" TextMode="MultiLine"></asp:TextBox>
            <asp:Button ID="btnSend" runat="server" Text="Send" OnClick="btnSend_Click" />
        </div>
    </div>
    </div>
    </form>
</body>
</html>
