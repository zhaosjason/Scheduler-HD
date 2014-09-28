using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.OleDb;

public partial class User : System.Web.UI.Page
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
            if (Request.QueryString["u"] == null)
                Response.Redirect("Home.aspx");

            string selectPageUserIDCmdStr = "SELECT ID FROM Users WHERE Username = ?";
            OleDbCommand selectPageUserIDCmd = new OleDbCommand(selectPageUserIDCmdStr, conn);
            selectPageUserIDCmd.Parameters.Add(new OleDbParameter("@Username", Request.QueryString["u"]));

            conn.Open();
            OleDbDataReader drPageUserID = selectPageUserIDCmd.ExecuteReader();
            int pageUserID = 0;
            while (drPageUserID.Read())
                pageUserID = (int)drPageUserID["ID"];
            drPageUserID.Close();
            conn.Close();

            if (pageUserID == 0)
                Response.Redirect("Home.aspx");
            if (pageUserID == Convert.ToInt32(Session["UserID"]))
                Response.Redirect("Schedule.aspx");
        }
    }

    protected HDSchedule getSchedule(string username)
    {
        string selectScheduleDataCmdStr = "SELECT Schedule FROM Users WHERE Username = ?";
        OleDbCommand selectScheduleDataCmd = new OleDbCommand(selectScheduleDataCmdStr, conn);
        selectScheduleDataCmd.Parameters.Add(new OleDbParameter("@Username", username));

        conn.Open();
        OleDbDataReader drSchedule = selectScheduleDataCmd.ExecuteReader();
        string databaseScheduleString = "";
        while (drSchedule.Read())
            if (!drSchedule["Schedule"].GetType().FullName.Equals("System.DBNull") && drSchedule["Schedule"] != null && !drSchedule["Schedule"].Equals(""))
                databaseScheduleString = (string)drSchedule["Schedule"];
        drSchedule.Close();
        conn.Close();

        HDSchedule schedule = new HDSchedule();
        schedule.setFromDatabaseString(databaseScheduleString);
        return schedule;
    }

    protected HDSchedule getSchedule(int id)
    {
        string selectScheduleDataCmdStr = "SELECT Schedule FROM Users WHERE ID = ?";
        OleDbCommand selectScheduleDataCmd = new OleDbCommand(selectScheduleDataCmdStr, conn);
        selectScheduleDataCmd.Parameters.Add(new OleDbParameter("@ID", id));

        conn.Open();
        OleDbDataReader drSchedule = selectScheduleDataCmd.ExecuteReader();
        string databaseScheduleString = "";
        while (drSchedule.Read())
            if (!drSchedule["Schedule"].GetType().FullName.Equals("System.DBNull") && drSchedule["Schedule"] != null && !drSchedule["Schedule"].Equals(""))
                databaseScheduleString = (string)drSchedule["Schedule"];
        drSchedule.Close();
        conn.Close();

        HDSchedule schedule = new HDSchedule();
        schedule.setFromDatabaseString(databaseScheduleString);
        return schedule;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string selectPageUserDataCmdStr = "SELECT ID, First_Name, Last_Name, Username, Schedule FROM Users WHERE Username = ?";
        OleDbCommand selectPageUserDataCmd = new OleDbCommand(selectPageUserDataCmdStr, conn);
        selectPageUserDataCmd.Parameters.Add(new OleDbParameter("@Username", Request.QueryString["u"]));

        conn.Open();
        OleDbDataReader drPageUserData = selectPageUserDataCmd.ExecuteReader();
        int pageUserID = 0;
        string pageUserName = "";
        string pageUserScheduleString = "";
        while (drPageUserData.Read())
        {
            Boolean hasFirstName = true;
            Boolean hasLastName = true;
            string firstName = "";
            string lastName = "";
            if (drPageUserData["First_Name"].ToString().Trim() == "NULL" || drPageUserData["First_Name"].ToString().Trim().Equals(""))
                hasFirstName = false;
            else
                firstName = drPageUserData["First_Name"].ToString();
            if (drPageUserData["Last_Name"].ToString().Trim() == "NULL" || drPageUserData["Last_Name"].ToString().Trim().Equals(""))
                hasLastName = false;
            else
                lastName = drPageUserData["Last_Name"].ToString();

            pageUserID = (int)drPageUserData["ID"];
            if (drPageUserData["Schedule"] != null && !drPageUserData["Schedule"].ToString().Trim().Equals(""))
                pageUserScheduleString = (string)drPageUserData["Schedule"];

            if (hasFirstName || hasLastName)
                pageUserName = firstName + " " + lastName + " (" + Request.QueryString["u"] + ")";
            else
                pageUserName = Request.QueryString["u"];
        }
        drPageUserData.Close();
        conn.Close();

        this.lblName.Text = pageUserName;

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

        Boolean isFriend = false;

        if (friendsString.Length != 0)
        {
            string[] friendIDStrings = friendsString.Split(' ');
            int[] friendIDs = new int[friendIDStrings.Length];
            for (int i = 0; i < friendIDStrings.Length; i++)
                friendIDs[i] = Convert.ToInt32(friendIDStrings[i]);

            foreach (int ifriendID in friendIDs)
                if (pageUserID == ifriendID)
                    isFriend = true;
        }

        if (isFriend)
            this.btnFriendAddRemove.Text = "Remove from Friends";
        else
            this.btnFriendAddRemove.Text = "Add to Friends";

        HDSchedule schedule = getSchedule(Request.QueryString["u"]);

        HDSchedule currentUserSchedule = getSchedule(Convert.ToInt32(Session["UserID"]));
        Comparator compare = new Comparator(schedule, currentUserSchedule);

        Boolean scheduleHasClasses = schedule.hasClasses();
        Boolean scheduleHasTeachers = schedule.hasTeachers();

        if (scheduleHasClasses)
        {
            ltrClasses.Text = "";
            ltrClasses.Text += "<table id='classes-user-table'><tr><th id='classes-table-header' colspan='6'>Classes</th></tr>";
            Boolean[,] simClasses = compare.getCommonPeriods();
            for (int y = 0; y < HDSchedule.DEFAULT_DAY_LENGTH; y++)
            {
                ltrClasses.Text += "<tr class='classes-row'>";
                for (int x = 0; x < HDSchedule.DEFAULT_WEEK_LENGTH; x++)
                {
                    if (simClasses[x, y] == true)
                    {
                        ltrClasses.Text += "<td class='classes-cell classes-cell-highlight'>";
                        ltrClasses.Text += schedule.classes[x, y];
                        ltrClasses.Text += "</td>";
                    }
                    else
                    {
                        ltrClasses.Text += "<td class='classes-cell'>";
                        ltrClasses.Text += schedule.classes[x, y];
                        ltrClasses.Text += "</td>";
                    }
                }

                ltrClasses.Text += "</tr>";
            }

            ltrClasses.Text += "</table>";
        }

        if (scheduleHasTeachers)
        {
            ltrTeachers.Text = "";
            ltrTeachers.Text += "<table id='teachers-user-table'><tr><th id='teachers-table-header' colspan='2'>Teachers</th></tr>";

            foreach (string className in schedule.teachers.Keys)
            {
                ltrTeachers.Text += "<tr class='teachers-row'>";

                ltrTeachers.Text += "<td class='teachers-cell'>";
                ltrTeachers.Text += className;
                ltrTeachers.Text += "</td>";

                ltrTeachers.Text += "<td class='teachers-cell'>";
                string teacherName = "";
                schedule.teachers.TryGetValue(className, out teacherName);
                ltrTeachers.Text += teacherName;
                ltrTeachers.Text += "</td>";

                ltrTeachers.Text += "</tr>";
            }

            ltrTeachers.Text += "</table>";
        }

        ltrComparison.Text = "";

        ltrComparison.Text += "<div id='user-comparison-text'>";
        if (!currentUserSchedule.hasTeachers())
            ltrComparison.Text += "<span class='user-comparison-text-header'>You have not added your schedule yet</span>";
        else if (!schedule.hasClasses() && !schedule.hasTeachers())
            ltrComparison.Text += "<span class='user-comparison-text-header'>This user has not added their schedule yet</span>";
        else if (schedule.hasClasses() && !schedule.hasTeachers())
            ltrComparison.Text += "<span class='user-comparison-text-header'>This user has not added their teachers yet</span>";
        else
        {
            ltrComparison.Text += "<span class='user-comparison-text-header'>Common Classes:</span> ";
            foreach (string className in compare.getCommonClasses())
                ltrComparison.Text += "<br />" + className;
            ltrComparison.Text += "<br /><br /><span class='user-comparison-text-header'>Common Frees:</span> ";
            foreach (string freePeriod in compare.getCommonFrees())
                ltrComparison.Text += "<br />" + freePeriod;
            ltrComparison.Text += "<br /><br /><span class='user-comparison-text-header'>Common Lunches:</span> <br />";
            foreach (string lunchPeriod in compare.getCommonLunches())
                ltrComparison.Text += lunchPeriod + " ";
        }
        ltrComparison.Text += "</div>";
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

    protected void btnFriendAddRemove_Click(object sender, EventArgs e)
    {
        string selectPageUserIDCmdStr = "SELECT ID FROM Users WHERE Username = ?";
        OleDbCommand selectPageUserIDCmd = new OleDbCommand(selectPageUserIDCmdStr, conn);
        selectPageUserIDCmd.Parameters.Add(new OleDbParameter("@Username", Request.QueryString["u"]));

        conn.Open();
        OleDbDataReader drPageUserID = selectPageUserIDCmd.ExecuteReader();
        int pageUserID = 0;
        while (drPageUserID.Read())
            pageUserID = (int)drPageUserID["ID"];
        drPageUserID.Close();
        conn.Close();

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

        string[] friendIDStrings = null;
        int[] friendIDs = null;
        if (friendsString.Length > 0)
        {
            friendIDStrings = friendsString.Split(' ');
            friendIDs = new int[friendIDStrings.Length];
            for (int i = 0; i < friendIDStrings.Length; i++)
                friendIDs[i] = Convert.ToInt32(friendIDStrings[i]);
        }

        if (this.btnFriendAddRemove.Text.Contains("Add"))
        {
            string newFriendsStringAdd = "";

            if (friendsString.Length == 0)
                newFriendsStringAdd = pageUserID + "";
            else if (Convert.ToInt32(pageUserID) < friendIDs[0])
                newFriendsStringAdd = pageUserID + " " + friendsString;
            else
            {
                Boolean added = false;
                for (int i = friendIDs.Count() - 1; i >= 0; i--)
                {
                    if (added == false && Convert.ToInt32(pageUserID) > friendIDs[i])
                    {
                        newFriendsStringAdd = friendIDs[i] + " " + pageUserID + " " + newFriendsStringAdd;
                        added = true;
                    }
                    else
                        newFriendsStringAdd = friendIDs[i] + " " + newFriendsStringAdd;
                }
                newFriendsStringAdd = newFriendsStringAdd.Substring(0, newFriendsStringAdd.Length - 1);
            }

            string updateFriendsAddCmdStr = "UPDATE Users SET Friends = ? WHERE ID = ?";
            OleDbCommand updateFriendsAddCmd = new OleDbCommand(updateFriendsAddCmdStr, conn);
            updateFriendsAddCmd.Parameters.Add(new OleDbParameter("@Friends", newFriendsStringAdd));
            updateFriendsAddCmd.Parameters.Add(new OleDbParameter("@ID", Session["UserID"]));

            conn.Open();
            updateFriendsAddCmd.ExecuteNonQuery();
            conn.Close();

            Response.Redirect("User.aspx?u=" + Request.QueryString["u"]);
        }
        else if (this.btnFriendAddRemove.Text.Contains("Remove"))
        {
            string newFriendsStringRemove = "";
            if (friendIDs.Length > 1)
            {
                for (int i = 0; i < friendIDs.Count(); i++)
                    if (Convert.ToInt32(pageUserID) != friendIDs[i])
                        newFriendsStringRemove += friendIDs[i] + " ";
                newFriendsStringRemove = newFriendsStringRemove.Substring(0, newFriendsStringRemove.Length - 1);
            }

            string updateFriendsRemoveCmdStr = "UPDATE Users SET Friends = ? WHERE ID = ?";
            OleDbCommand updateFriendsRemoveCmd = new OleDbCommand(updateFriendsRemoveCmdStr, conn);
            updateFriendsRemoveCmd.Parameters.Add(new OleDbParameter("@Friends", newFriendsStringRemove));
            updateFriendsRemoveCmd.Parameters.Add(new OleDbParameter("@ID", Session["UserID"]));

            conn.Open();
            updateFriendsRemoveCmd.ExecuteNonQuery();
            conn.Close();

            Response.Redirect("User.aspx?u=" + Request.QueryString["u"]);
        }
    }
}