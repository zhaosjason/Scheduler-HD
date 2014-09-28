<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Schedule.aspx.cs" Inherits="Schedule" %>

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
            <asp:Literal ID="ltrClasses" runat="server" EnableViewState="False"></asp:Literal>
            <asp:Literal ID="ltrTeachers" runat="server" EnableViewState="False"></asp:Literal>
            <asp:Panel ID="scheduleFriendsInstructions" runat="server">Hover over course names to see who's in your classes</asp:Panel>
            <div id="edit-schedule">
                <div id="edit-schedule-header">
                    Edit Schedule</div>
                <hr class="edit-schedule-hr" />
                <div class="edit-schedule-sub-header">
                    Edit Classes</div>
                <asp:TextBox ID="txtScheduleInput" CssClass="user-data-input-field" runat="server"
                    TextMode="MultiLine" placeholder="Paste Schedule Text Here"></asp:TextBox>
                <asp:Button ID="btnApplySchedule" runat="server" Text="Apply Schedule" OnClick="btnApplySchedule_Click" />
                <asp:Button ID="btnHowTo" runat="server" Text="How To" OnClick="btnHowTo_Click" />
                <hr class="edit-schedule-hr" />
                <div class="edit-schedule-sub-header">
                    Edit Teachers</div>
                <asp:Panel ID="pnlTeacherDropdowns" runat="server">
                </asp:Panel>
                <asp:Button ID="btnApplyTeachers" runat="server" Text="Apply Teachers" OnClick="btnApplyTeachers_Click" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
