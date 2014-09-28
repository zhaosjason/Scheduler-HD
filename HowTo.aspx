<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HowTo.aspx.cs" Inherits="HowTo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
                    <asp:Button CssClass="main-menu-button" ID="btnHome" formnovalidate="formnovalidate"
                        runat="server" Text="Home" OnClick="menuBtn_Click" />
                    <asp:Button CssClass="main-menu-button" ID="btnSchedule" formnovalidate="formnovalidate"
                        runat="server" Text="Schedule" OnClick="menuBtn_Click" />
                    <asp:Button CssClass="main-menu-button" ID="btnFriends" formnovalidate="formnovalidate"
                        runat="server" Text="Friends" OnClick="menuBtn_Click" />
                    <asp:Button CssClass="main-menu-button" ID="btnSettings" formnovalidate="formnovalidate"
                        runat="server" Text="Settings" OnClick="menuBtn_Click" />
                    <asp:Button CssClass="main-menu-button" ID="btnContact" formnovalidate="formnovalidate"
                        runat="server" Text="Contact" OnClick="menuBtn_Click" />
                    <asp:Button CssClass="main-menu-button" ID="btnLogout" formnovalidate="formnovalidate"
                        runat="server" Text="Logout" OnClick="menuBtn_Click" />
                </div>
            </div>
        </div>
        <div id="page-content">
            <div id="how-to">
                <div id="how-to-header">
                    Adding Your Schedule
                </div>
                <ol id="how-to-instructions">
                    <li class="how-to-item">1) Go to your schedule on MyBackpack
                        <div id="how-to-image-1" class="how-to-image" />
                    </li>
                    <li class="how-to-item">2) Uncheck the "Show Times" checkbox
                        <div id="how-to-image-2" class="how-to-image" />
                    </li>
                    <li class="how-to-item">3) Copy and paste the schedule into the "Edit Classes" section
                        <div id="how-to-image-3" class="how-to-image" />
                    </li>
                </ol>
                <asp:Button ID="btnGotIt" runat="server" Text="Got It" OnClick="btnGotIt_Click" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
