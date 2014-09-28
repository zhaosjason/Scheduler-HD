using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.OleDb;

public partial class Friends : System.Web.UI.Page
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
            int[] friendIDs = new int[friendIDStrings.Length];
            for (int i = 0; i < friendIDStrings.Length; i++)
                friendIDs[i] = Convert.ToInt32(friendIDStrings[i]);

            string selectFriendsInfoCmdStr = "SELECT ID, First_Name, Last_Name, Username FROM Users WHERE";
            foreach (string friendID in friendIDStrings)
                selectFriendsInfoCmdStr += " ID = " + friendID + " OR";
            selectFriendsInfoCmdStr = selectFriendsInfoCmdStr.Substring(0, selectFriendsInfoCmdStr.Length - 3);
            OleDbCommand selectFriendsInfoCmd = new OleDbCommand(selectFriendsInfoCmdStr, conn);

            List<string> friendNameStrings = new List<string>();
            conn.Open();
            OleDbDataReader drFriendsInfo = selectFriendsInfoCmd.ExecuteReader();
            while (drFriendsInfo.Read())
            {
                Boolean hasFirstName = true;
                Boolean hasLastName = true;
                string firstName = "";
                string lastName = "";
                if (drFriendsInfo["First_Name"].ToString().Trim() == "NULL" || drFriendsInfo["First_Name"].ToString().Trim().Equals(""))
                    hasFirstName = false;
                else
                    firstName = drFriendsInfo["First_Name"].ToString();
                if (drFriendsInfo["Last_Name"].ToString().Trim() == "NULL" || drFriendsInfo["Last_Name"].ToString().Trim().Equals(""))
                    hasLastName = false;
                else
                    lastName = drFriendsInfo["Last_Name"].ToString();

                string friendPageLink = "User.aspx?u=" + drFriendsInfo["Username"];
                if (hasFirstName || hasLastName)
                    friendNameStrings.Add("<!--" + firstName + lastName + drFriendsInfo["Username"] + "-->" + "<a href='" + friendPageLink + "' class='friend-link'>" + firstName + " " + lastName+ "</a> (" + drFriendsInfo["Username"] + ")");
                else
                    friendNameStrings.Add("<!--" + drFriendsInfo["Username"] + "-->" + "<a href='" + friendPageLink + "' class='friend-link'>" + drFriendsInfo["Username"] + "</a>");
            }
            drFriendsInfo.Close();
            conn.Close();

            friendNameStrings.Sort();
            foreach (string friendNameString in friendNameStrings)
                this.lblFriends.Text += friendNameString + " <br /><br />";
        }
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
}