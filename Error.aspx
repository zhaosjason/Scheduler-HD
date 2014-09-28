<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Scheduler HD</title>

    <link rel="icon" type="image/ico" href="images/calendar_favicon.ico" />
    <link rel="shortcut icon" href="images/calendar_favicon.ico" />
    <link href="css/reset.css" rel="stylesheet" type="text/css" />
    <link href="css/error.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <p id="no">:(</p>
            <p id="errorText">
                Oh no! There has been an error.
                <br /><br />
                Feel free to contact us with our <asp:Button CssClass="redirectButton" ID="btnContact" runat="server" Text="contact page" 
                    onclick="btnContact_Click" />.
                <br /> Your feedback makes this site better!
                <br /><br />
                <asp:Button CssClass="redirectButton" ID="btnReturnHome" runat="server" Text="Return to Home" 
                    onclick="btnReturnHome_Click" />
            </p>
    </div>
    </form>
</body>
</html>
