<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home" %>

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
    <link href="css/ajax-searchbar-styles.css" rel="stylesheet" type="text/css" />
    <link href="css/main.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="http://code.jquery.com/jquery-1.10.1.min.js"></script>
    <script type="text/javascript" src="js/jquery.autocomplete.min.js"></script>
    <script type="text/javascript" src="js/ajax-searchbar.js"></script>
</head>
<body id="home-page-body">
    <form id="form1" runat="server">
    <div id="wrapper">
        <div id="main-header-ribbon">
            <a id="main-header-title-link" href="Home.aspx">
                <div id="main-header-text">
                    Scheduler HD</div>
            </a>
            <div id="main-header-menu-div">
                <div id="main-header-menu-icon" onclick="#main-menu"></div>
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
            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSearch">
            <asp:TextBox ID="tbSearch" runat="server" autofocus="autofocus" autocomplete="off" 
                AutoCompleteType="Disabled"></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="" onclick="btnSearch_Click" />
            </asp:Panel>
            <div id="searchText">
                Search for friends by username or name
               </div>
            <asp:Label ID="tbSearchData" runat="server" Text="" Visible="false"></asp:Label>
        </div>
    </div>
    </form>
</body>
</html>
