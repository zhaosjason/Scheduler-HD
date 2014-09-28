using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Configuration;
using System.Text.RegularExpressions;

public partial class Schedule : System.Web.UI.Page
{
    OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConnStr"].ConnectionString);
    private string teacherDropdownDefaultText = "Select Your Teacher";

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

    protected HDSchedule getSchedule(int id)
    {
        string selectScheduleDataCmdStr = "SELECT Schedule FROM Users WHERE ID = ?";
        OleDbCommand selectScheduleDataCmd = new OleDbCommand(selectScheduleDataCmdStr, conn);
        selectScheduleDataCmd.Parameters.Add(new OleDbParameter("@ID", id + ""));

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
        HDSchedule schedule = getSchedule(Convert.ToInt32(Session["UserID"]));

        Boolean scheduleHasClasses = schedule.hasClasses();
        Boolean scheduleHasTeachers = schedule.hasTeachers();

        if (scheduleHasClasses)
        {
            ltrClasses.Text = "";
            ltrClasses.Text += "<table id='classes-table'><tr><th id='classes-table-header' colspan='6'>Classes</th></tr>";
            for (int y = 0; y < HDSchedule.DEFAULT_DAY_LENGTH; y++)
            {
                ltrClasses.Text += "<tr class='classes-row'>";
                for (int x = 0; x < HDSchedule.DEFAULT_WEEK_LENGTH; x++)
                {
                    ltrClasses.Text += "<td class='classes-cell'>";
                    string className = schedule.classes[x, y];
                    ltrClasses.Text += className;

                    ltrClasses.Text += "<div class='roster-tooltip'>";
                    ltrClasses.Text += "<div class='roster-tooltip-header'>Friends in " + className + "</div>";

                    ltrClasses.Text += "<div class='roster-tooltip-text'>";

                    List<string> friendsInClass = new List<string>();

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

                        string selectFriendsDataCmdStr = "SELECT First_Name, Last_Name, Username, Schedule FROM Users WHERE";
                        foreach (string friendID in friendIDStrings)
                            selectFriendsDataCmdStr += " ID = " + friendID + " OR";
                        selectFriendsDataCmdStr = selectFriendsDataCmdStr.Substring(0, selectFriendsDataCmdStr.Length - 3);
                        selectFriendsDataCmdStr += " ORDER BY Username";
                        OleDbCommand selectFriendsDataCmd = new OleDbCommand(selectFriendsDataCmdStr, conn);

                        conn.Open();
                        OleDbDataReader drFriendsData = selectFriendsDataCmd.ExecuteReader();
                        while (drFriendsData.Read())
                        {
                            HDSchedule friendSchedule = new HDSchedule();
                            friendSchedule.setFromDatabaseString((string)drFriendsData["Schedule"]);
                            Comparator compare = new Comparator(schedule, friendSchedule);

                            Boolean[,] commonPeriods = compare.getCommonPeriods();
                            if (commonPeriods[x, y] == true)
                                friendsInClass.Add((string)drFriendsData["Username"]);
                        }
                        drFriendsData.Close();
                        conn.Close();
                    }

                    if (friendsInClass.Count() == 0)
                        ltrClasses.Text += "None";
                    else
                        foreach (string friendInClassName in friendsInClass)
                            ltrClasses.Text += "<a class='friend-in-class-name-item' href='User.aspx?u=" + friendInClassName + "'>" + friendInClassName + "</a><br />";

                    ltrClasses.Text += "</div>";
                    ltrClasses.Text += "</div>";

                    ltrClasses.Text += "</td>";
                }
                ltrClasses.Text += "</tr>";
            }

            ltrClasses.Text += "</table>";
        }

        if (scheduleHasTeachers)
        {
            this.scheduleFriendsInstructions.Style.Add("display", "block");
            ltrTeachers.Text = "";
            ltrTeachers.Text += "<table id='teachers-table'><tr><th id='teachers-table-header' colspan='2'>Teachers</th></tr>";

            foreach (string className in schedule.teachers.Keys)
            {
                ltrTeachers.Text += "<tr class='teachers-row'>";

                ltrTeachers.Text += "<td class='teachers-cell' onclick='#teachers-cell'>";
                ltrTeachers.Text += className;

                ltrTeachers.Text += "<div class='roster-tooltip'>";
                ltrTeachers.Text += "<div class='roster-tooltip-header'>Friends in " + className + "</div>";

                ltrTeachers.Text += "<div class='roster-tooltip-text'>";

                List<string> friendsInClass = new List<string>();

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

                    string selectFriendsDataCmdStr = "SELECT First_Name, Last_Name, Username, Schedule FROM Users WHERE";
                    foreach (string friendID in friendIDStrings)
                        selectFriendsDataCmdStr += " ID = " + friendID + " OR";
                    selectFriendsDataCmdStr = selectFriendsDataCmdStr.Substring(0, selectFriendsDataCmdStr.Length - 3);
                    selectFriendsDataCmdStr += " ORDER BY Username";
                    OleDbCommand selectFriendsDataCmd = new OleDbCommand(selectFriendsDataCmdStr, conn);

                    conn.Open();
                    OleDbDataReader drFriendsData = selectFriendsDataCmd.ExecuteReader();
                    while (drFriendsData.Read())
                    {
                        HDSchedule friendSchedule = new HDSchedule();
                        friendSchedule.setFromDatabaseString((string)drFriendsData["Schedule"]);
                        Comparator compare = new Comparator(schedule, friendSchedule);

                        string[] commonClasses = compare.getCommonClasses();
                        if (commonClasses.Contains(className))
                        {
                            friendsInClass.Add((string)drFriendsData["Username"]);
                        }
                    }
                    drFriendsData.Close();
                    conn.Close();
                }

                if (friendsInClass.Count() == 0)
                    ltrTeachers.Text += "None";
                else
                    foreach (string friendInClassName in friendsInClass)
                        ltrTeachers.Text += "<a class='friend-in-class-name-item' href='User.aspx?u=" + friendInClassName + "'>" + friendInClassName + "</a><br />";

                ltrTeachers.Text += "</div>";
                ltrTeachers.Text += "</div>";

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

        if (!scheduleHasClasses && !scheduleHasTeachers)
        {
            this.btnApplyTeachers.Enabled = false;
            this.btnApplyTeachers.Style.Add("background", "rgba(100, 100, 100, .1)");
            this.btnApplyTeachers.Style.Add("color", "rgba(30, 30, 30, .3)");
            this.btnApplyTeachers.Style.Add("border", "1px solid rgba(40, 40, 40, .1)");
            this.scheduleFriendsInstructions.Style.Add("display", "none");
        }
        else if (scheduleHasClasses && !scheduleHasTeachers)
        {
            string[] classNames = schedule.getAllClassNames();
            DropDownList[] dropDowns = new DropDownList[classNames.Length];
            for (int i = 0; i < classNames.Length; i++)
            {
                dropDowns[i] = new DropDownList();
                dropDowns[i].CssClass = "teachers-drop-down";
                dropDowns[i].Items.Add(new ListItem(teacherDropdownDefaultText));

                string selectTeacherNamesCmdStr = "SELECT First_Name, Last_Name FROM Teachers  ORDER BY Last_Name ASC";
                OleDbCommand selectTeacherNamesCmd = new OleDbCommand(selectTeacherNamesCmdStr, conn);

                conn.Open();
                OleDbDataReader drTeacherNames = selectTeacherNamesCmd.ExecuteReader();
                while (drTeacherNames.Read())
                    dropDowns[i].Items.Add(new ListItem(drTeacherNames["Last_Name"] + ", " + drTeacherNames["First_Name"]));
                drTeacherNames.Close();
                conn.Close();

                dropDowns[i].Items.FindByText(teacherDropdownDefaultText).Selected = true;
            }
            for (int i = 0; i < classNames.Length; i++)
            {
                string classNameText = "<div class='teachers-class-name'>" + classNames[i] + "</div>";
                LiteralControl classNameCtrl = new LiteralControl(classNameText);
                Panel pnlClassNameAndDropdown = new Panel();
                pnlClassNameAndDropdown.CssClass = "pnlClassNameWithDropdown";
                pnlClassNameAndDropdown.Controls.Add(classNameCtrl);
                pnlClassNameAndDropdown.Controls.Add(dropDowns[i]);
                this.pnlTeacherDropdowns.Controls.Add(pnlClassNameAndDropdown);
            }
            this.scheduleFriendsInstructions.Style.Add("display", "none");
        }
        else if (scheduleHasClasses && scheduleHasTeachers)
        {
            string[] classNames = schedule.getAllClassNames();
            DropDownList[] dropDowns = new DropDownList[classNames.Length];
            for (int i = 0; i < classNames.Length; i++)
            {
                dropDowns[i] = new DropDownList();
                dropDowns[i].CssClass = "teachers-drop-down";

                string selectTeacherNamesCmdStr = "SELECT First_Name, Last_Name FROM Teachers ORDER BY Last_Name ASC";
                OleDbCommand selectTeacherNamesCmd = new OleDbCommand(selectTeacherNamesCmdStr, conn);

                conn.Open();
                OleDbDataReader drTeacherNames = selectTeacherNamesCmd.ExecuteReader();
                while (drTeacherNames.Read())
                    dropDowns[i].Items.Add(new ListItem(drTeacherNames["Last_Name"] + ", " + drTeacherNames["First_Name"]));
                drTeacherNames.Close();
                conn.Close();
            }
            for (int i = 0; i < classNames.Length; i++)
            {
                string classNameText = "<div class='teachers-class-name'>" + classNames[i] + "</div>";
                LiteralControl classNameCtrl = new LiteralControl(classNameText);
                Panel pnlClassNameAndDropdown = new Panel();
                pnlClassNameAndDropdown.CssClass = "pnlClassNameWithDropdown";
                pnlClassNameAndDropdown.Controls.Add(classNameCtrl);
                pnlClassNameAndDropdown.Controls.Add(dropDowns[i]);
                this.pnlTeacherDropdowns.Controls.Add(pnlClassNameAndDropdown);

                string teacherName = "";
                schedule.teachers.TryGetValue(classNames[i], out teacherName);
                dropDowns[i].Items.FindByText(teacherName).Selected = true;
            }
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

    protected void btnHowTo_Click(object sender, EventArgs e)
    {
        Response.Redirect("HowTo.aspx");
    }

    protected void btnApplySchedule_Click(object sender, EventArgs e)
    {
        HDSchedule schedule = getSchedule(Convert.ToInt32(Session["UserID"]));
        schedule.clearSchedule();

        schedule.setFromSiteString(this.txtScheduleInput.Text);

        if (!schedule.hasClasses())
            logScheduleError();

        string newDatabaseString = schedule.getDatabaseString();

        string updateScheduleCmdStr = "UPDATE Users SET Schedule = ? WHERE ID = ?";
        OleDbCommand updateScheduleCmd = new OleDbCommand(updateScheduleCmdStr, conn);
        updateScheduleCmd.Parameters.Add(new OleDbParameter("@Schedule", newDatabaseString));
        updateScheduleCmd.Parameters.Add(new OleDbParameter("@ID", Session["UserID"]));

        conn.Open();
        updateScheduleCmd.ExecuteNonQuery();
        conn.Close();

        Response.Redirect("Schedule.aspx");
    }

    private void logScheduleError()
    {
        string errorMessage = "";
        if (HttpContext.Current.Session != null)
            errorMessage = "User with ID: " + Session["UserID"] + " couldn't add their schedule: [" + this.txtScheduleInput.Text + "]";
        else
            errorMessage = "Unknown user couldn't add their schedule: [" + this.txtScheduleInput.Text + "]";

        string insertScheduleInLogCmdStr = "INSERT INTO Log (Kind, Content, Stamp) VALUES (?, ?, ?)";
        System.Data.OleDb.OleDbCommand insertScheduleInLogCmd = new System.Data.OleDb.OleDbCommand(insertScheduleInLogCmdStr, conn);
        insertScheduleInLogCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@Kind", "4"));
        insertScheduleInLogCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@Content", errorMessage));
        insertScheduleInLogCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@Stamp", DateTime.Now.AddHours(3)));

        conn.Open();
        insertScheduleInLogCmd.ExecuteNonQuery();
        conn.Close();
    }

    protected void btnApplyTeachers_Click(object sender, EventArgs e)
    {
        HDSchedule schedule = getSchedule(Convert.ToInt32(Session["UserID"]));
        schedule.teachers.Clear();

        int controlCounter = 2;
        foreach (Control c1 in this.pnlTeacherDropdowns.Controls)
        {
            if (controlCounter != 2)
            {
                string[] classTeacherPair = new string[2];
                foreach (Control c2 in c1.Controls)
                {
                    if (controlCounter % 2 == 0)
                    {
                        LiteralControl ltrClass = (LiteralControl)c2;
                        string literalText = ltrClass.Text;
                        literalText = Regex.Replace(literalText, "<div class='teachers-class-name'>", "");
                        literalText = Regex.Replace(literalText, "</div>", "");
                        classTeacherPair[0] = literalText;
                    }
                    if (controlCounter % 2 == 1)
                    {
                        DropDownList ddlTeachers = (DropDownList)c2;
                        classTeacherPair[1] = ddlTeachers.SelectedValue;
                        if (ddlTeachers.SelectedValue.Equals(teacherDropdownDefaultText))
                        {
                            schedule.teachers.Clear();
                            return;
                        }
                    }
                    controlCounter++;
                }
                schedule.teachers.Add(classTeacherPair[0], classTeacherPair[1]);
            }
            else
                controlCounter += 2;
        }

        string newScheduleString = schedule.getDatabaseString();

        string updateScheduleDataCmdStr = "UPDATE Users SET Schedule = ? WHERE ID = ?";
        OleDbCommand updateScheduleDataCmd = new OleDbCommand(updateScheduleDataCmdStr, conn);
        updateScheduleDataCmd.Parameters.Add(new OleDbParameter("@Schedule", newScheduleString));
        updateScheduleDataCmd.Parameters.Add(new OleDbParameter("@ID", Session["UserID"]));

        conn.Open();
        updateScheduleDataCmd.ExecuteNonQuery();
        conn.Close();

        Response.Redirect("Schedule.aspx");
    }
}