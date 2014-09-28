using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Configuration;
using System.IO;

public partial class Home : System.Web.UI.Page
{
    OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConnStr"].ConnectionString);

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Request.Browser.Type.ToUpper().Contains("IE"))
            Response.Redirect("IE.aspx");
        else
        {
            object userID = Session["UserID"];
            if (userID == null || userID.ToString() == "0")
                Response.Redirect("Default.aspx");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string selectFriendsStringCmdStr = "SELECT Friends FROM Users WHERE ID = ?";
        OleDbCommand selectFriendsStringCmd = new OleDbCommand(selectFriendsStringCmdStr, conn);
        selectFriendsStringCmd.Parameters.Add(new OleDbParameter("@ID", Session["UserID"]));

        conn.Open();
        OleDbDataReader drFriends = selectFriendsStringCmd.ExecuteReader();
        string friendsString = "";
        while (drFriends.Read())
            friendsString = (string)drFriends["Friends"];
        drFriends.Close();
        conn.Close();

        if (friendsString.Length > 0)
        {
            string[] friendIDStrings = friendsString.Split(' ');

            string selectExistingFriendsCmdStr = "SELECT ID FROM Users WHERE ";
            foreach (string friendID in friendIDStrings)
                selectExistingFriendsCmdStr += "ID = " + friendID + " OR ";
            selectExistingFriendsCmdStr = selectExistingFriendsCmdStr.Substring(0, selectExistingFriendsCmdStr.Length - 4);
            OleDbCommand selectExistingFriendsCmd = new OleDbCommand(selectExistingFriendsCmdStr, conn);

            conn.Open();
            OleDbDataReader drExistingFriends = selectExistingFriendsCmd.ExecuteReader();
            string existingFriendsString = "";
            while (drExistingFriends.Read())
                existingFriendsString += drExistingFriends["ID"].ToString() + " ";
            drExistingFriends.Close();
            conn.Close();

            if (existingFriendsString.Length > 0)
            {
                existingFriendsString = existingFriendsString.Substring(0, existingFriendsString.Length - 1);

                string updateFriendsCmdStr = "UPDATE Users SET Friends = ? WHERE ID = ?";
                OleDbCommand updateFriendsCmd = new OleDbCommand(updateFriendsCmdStr, conn);
                updateFriendsCmd.Parameters.Add(new OleDbParameter("@Friends", existingFriendsString));
                updateFriendsCmd.Parameters.Add(new OleDbParameter("@ID", Session["UserID"]));

                conn.Open();
                updateFriendsCmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        
        /*
        string selectUsersCmdStr = "SELECT First_Name, Last_Name, Username FROM Users";
        OleDbCommand selectUsersCmd = new OleDbCommand(selectUsersCmdStr, conn);

        conn.Open();
        OleDbDataReader drUsers = selectUsersCmd.ExecuteReader();
        string usersStr = "";
        while (drUsers.Read())
        {
            usersStr = usersStr + "'" + drUsers["Username"].ToString() + "': '" + drUsers["First_Name"].ToString() + " " +
                drUsers["Last_Name"].ToString() + "',";
        }
        drUsers.Close();
        conn.Close();

        usersStr = usersStr.Substring(0, usersStr.Length - 1);
        StreamWriter writer = new StreamWriter("services/ajax-searchbar-data.txt");
        writer.WriteLine(usersStr);
        */
    }

    protected void menuBtn_Click(object sender, EventArgs e)
    {
        Button senderButton = (Button)sender;
        string buttonID = senderButton.ID;

        if (buttonID.Equals("btnHome"))
            Response.Redirect("Home.aspx");
        else if (buttonID.Equals("btnSchedule"))
            Response.Redirect("Schedule.aspx");
        else if (buttonID.Equals("btnContact"))
            Response.Redirect("Contact.aspx");
        else if (buttonID.Equals("btnSettings"))
            Response.Redirect("Settings.aspx");
        else if (buttonID.Equals("btnFriends"))
            Response.Redirect("Friends.aspx");
        else if (buttonID.Equals("btnLogout"))
        {
            Session["UserID"] = 0;
            Response.Redirect("Default.aspx");
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (this.tbSearchData.Text != "")
        {
            checkAsUsername(this.tbSearchData.Text);
        }
        else
        {
            checkAsUsername();
            checkAsName();
        }
    }

    private void checkAsUsername()
    {
        string search = this.tbSearch.Text;

        if (!search.Contains(" "))
        {
            string selectFriendUsernameCmdStr = "SELECT Username, First_Name, Last_Name FROM Users";
            OleDbCommand selectFriendUsernameCmd = new OleDbCommand(selectFriendUsernameCmdStr, conn);

            conn.Open();
            OleDbDataReader drUsername = selectFriendUsernameCmd.ExecuteReader();
            string username = "";
            while (drUsername.Read())
            {
                username = (string)drUsername["Username"];
                if (search.ToLower().Equals(username.ToLower()))
                    Response.Redirect("User.aspx?u=" + username);
            }
            drUsername.Close();
            conn.Close();
        }
    }

    private void checkAsUsername(string temp)
    {
        string search = temp;

        if (!search.Contains(" "))
        {
            string selectFriendUsernameCmdStr = "SELECT Username, First_Name, Last_Name FROM Users";
            OleDbCommand selectFriendUsernameCmd = new OleDbCommand(selectFriendUsernameCmdStr, conn);

            conn.Open();
            OleDbDataReader drUsername = selectFriendUsernameCmd.ExecuteReader();
            string username = "";
            while (drUsername.Read())
            {
                username = (string)drUsername["Username"];
                if (search.ToLower().Equals(username.ToLower()))
                    Response.Redirect("User.aspx?u=" + username);
            }
            drUsername.Close();
            conn.Close();
        }
    }

    private void checkAsName()
    {
        string search = this.tbSearch.Text;

        int spaceCounter = 0;
        foreach (char c in search.ToCharArray())
            if (c == ' ')
                spaceCounter++;

        if (spaceCounter == 1)
        {
            string[] searchParts = search.Split(' ');

            string selectFriendNameCmdStr = "SELECT Username, First_Name, Last_Name FROM Users";
            OleDbCommand selectFriendNameCmd = new OleDbCommand(selectFriendNameCmdStr, conn);

            conn.Open();
            OleDbDataReader drUsername = selectFriendNameCmd.ExecuteReader();
            string username = "";
            int hitCounter = 0;
            while (drUsername.Read())
            {
                if ((drUsername["First_Name"] != null && ((string)drUsername["First_Name"]).ToLower().Equals(searchParts[0].ToLower())) &&
                    (drUsername["Last_Name"] != null && ((string)drUsername["Last_Name"]).ToLower().Equals(searchParts[1].ToLower())))
                {
                    hitCounter++;
                    username = (string)drUsername["Username"];
                }
            }
            drUsername.Close();
            conn.Close();
            
            if(hitCounter == 1)
                Response.Redirect("User.aspx?u=" + username);
        }
    }

    /*

    private void checkAsUsername()
    {
        string search = this.tbSearch.Text;

        string selectFriendUsernameCmdStr = "SELECT Username, First_Name, Last_Name FROM Users";
        OleDbCommand selectFriendUsernameCmd = new OleDbCommand(selectFriendUsernameCmdStr, conn);

        conn.Open();
        OleDbDataReader drUsername = selectFriendUsernameCmd.ExecuteReader();
        string username = "";
        while (drUsername.Read())
        {
            username = (string)drUsername["Username"];
            if (search.ToLower().Equals(username.ToLower()))
                Response.Redirect("User.aspx?u=" + username);
        }
        drUsername.Close();
        conn.Close();
    }

    private void checkAsName()
    {
        string search = this.tbSearch.Text;

        string selectFriendUsernameCmdStr = "SELECT Username FROM Users WHERE First_Name = ? AND Last_Name = ?";
        OleDbCommand selectFriendUsernameCmd = new OleDbCommand(selectFriendUsernameCmdStr, conn);
        if (search.ToLower().IndexOf(' ') != -1)
        {
            selectFriendUsernameCmd.Parameters.Add(new OleDbParameter("@First_Name", search.ToLower().Split(' ')[0]));
            selectFriendUsernameCmd.Parameters.Add(new OleDbParameter("@Last_Name", search.ToLower().Split(' ')[1]));
        }
        else
            Response.Redirect("Home.aspx");

        conn.Open();
        OleDbDataReader drUsername = selectFriendUsernameCmd.ExecuteReader();
        while (drUsername.Read())
        {
            Response.Redirect("User.aspx?u=" + drUsername["Username"].ToString());
        }
        drUsername.Close();
        conn.Close();
    }
     */
}