<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Admin.aspx.cs" Inherits="Admin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Scheduler HD</title>
    <link rel="icon" type="image/ico" href="images/admin_favicon.ico" />
    <link rel="shortcut icon" href="images/admin_favicon.ico" />
    <link href="css/admin.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="wrapper">
        <div id="admin-toolbar"><asp:Button ID="btnGoHome" CssClass="admin-button" runat="server" Text="Go Home"
         PostBackUrl="http://schedulerhd.com/Home.aspx" /></div>
        <div class="admin-title">Administrative Data</div>
            <div class="admin-subtitle">
            Users</div>
        <div id="users-gridview-wrapper" class="gridview-wrapper">
            <asp:GridView ID="UsersGridView" class="GridView" runat="server" AllowSorting="True" 
                AutoGenerateColumns="False" DataKeyNames="ID" 
                DataSourceID="UsersSqlDataSource">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True"
                        SortExpression="ID" />
                    <asp:BoundField DataField="First_Name" HeaderText="First Name" SortExpression="First_Name" />
                    <asp:BoundField DataField="Last_Name" HeaderText="Last Name" SortExpression="Last_Name" />
                    <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" />
                    <asp:BoundField DataField="Pass" HeaderText="Pass" SortExpression="Pass" />
                    <asp:BoundField DataField="Schedule" HeaderText="Schedule" SortExpression="Schedule" />
                    <asp:BoundField DataField="Friends" HeaderText="Friends" SortExpression="Friends" />
                    <asp:BoundField DataField="Admin" HeaderText="Admin" SortExpression="Admin" />
                    <asp:BoundField DataField="Logins" HeaderText="Logins" SortExpression="Logins" />
                </Columns>
            </asp:GridView>
            
            <asp:SqlDataSource ID="UsersSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:sqlConnStr %>"
                DeleteCommand="DELETE FROM [Users] WHERE [ID] = ?" 
                InsertCommand="INSERT INTO [Users] ([ID], [First_Name], [Last_Name], [Username], [Pass], [Schedule], [Friends], [Admin]) VALUES (?, ?, ?, ?, ?, ?, ?, ?)"
                SelectCommand="SELECT * FROM [Users]" 
                UpdateCommand="UPDATE [Users] SET [First_Name] = ?, [Last_Name] = ?, [Username] = ?, [Pass] = ?, [Schedule] = ?, [Friends] = ?, [Admin] = ? WHERE [ID] = ?">
                <DeleteParameters>
                    <asp:Parameter Name="ID" Type="Int32" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="ID" Type="Int32" />
                    <asp:Parameter Name="First_Name" Type="String" />
                    <asp:Parameter Name="Last_Name" Type="String" />
                    <asp:Parameter Name="Username" Type="String" />
                    <asp:Parameter Name="Pass" Type="String" />
                    <asp:Parameter Name="Schedule" Type="String" />
                    <asp:Parameter Name="Friends" Type="String" />
                    <asp:Parameter Name="Admin" Type="Int32" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="First_Name" Type="String" />
                    <asp:Parameter Name="Last_Name" Type="String" />
                    <asp:Parameter Name="Username" Type="String" />
                    <asp:Parameter Name="Pass" Type="String" />
                    <asp:Parameter Name="Schedule" Type="String" />
                    <asp:Parameter Name="Friends" Type="String" />
                    <asp:Parameter Name="Admin" Type="Int32" />
                    <asp:Parameter Name="ID" Type="Int32" />
                </UpdateParameters>
            </asp:SqlDataSource>
        </div>

        <div class="admin-subtitle">
            Application Log</div>
        <div id="log-gridview-wrapper" class="gridview-wrapper">
        <asp:GridView ID="LogGridView" class="GridView" runat="server" AllowSorting="True" 
                AutoGenerateColumns="False" DataKeyNames="ID" 
                DataSourceID="LogSqlDataSource">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True"
                        SortExpression="ID" />
                    <asp:BoundField DataField="Kind" HeaderText="Type" SortExpression="Kind" />
                    <asp:BoundField DataField="Content" HeaderText="Content" SortExpression="Content" />
                    <asp:BoundField DataField="Stamp" HeaderText="Time" SortExpression="Stamp" />
                </Columns>
            </asp:GridView>
            
            <asp:SqlDataSource ID="LogSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:sqlConnStr %>"
                DeleteCommand="DELETE FROM [Log] WHERE [ID] = ?" 
                InsertCommand="INSERT INTO [Log] ([ID], [Kind], [Content], [Stamp]) VALUES (?, ?, ?, ?)"
                SelectCommand="SELECT * FROM [Log] ORDER BY ID DESC" 
                UpdateCommand="UPDATE [Log] SET [Kind] = ?, [Content] = ?, [Stamp] = ? WHERE [ID] = ?">
                <DeleteParameters>
                    <asp:Parameter Name="ID" Type="Int32" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="ID" Type="Int32" />
                    <asp:Parameter Name="Kind" Type="Int32" />
                    <asp:Parameter Name="Content" Type="String" />
                    <asp:Parameter Name="Stamp" Type="DateTime" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="ID" Type="Int32" />
                    <asp:Parameter Name="Kind" Type="Int32" />
                    <asp:Parameter Name="Content" Type="String" />
                    <asp:Parameter Name="Stamp" Type="DateTime" />
                </UpdateParameters>
            </asp:SqlDataSource>
        </div>

        <asp:Button ID="btnEdit" CssClass="admin-button" runat="server" Text="Edit the Database" 
        OnClientClick="window.document.forms[0].target='_blank';" 
        PostBackUrl="https://p3nmssqladmin.secureserver.net/2012/7/scripts/framespage.aspx" />
    </div>
    </form>
</body>
</html>
