using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HowTo : System.Web.UI.Page
{
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

    protected void btnGotIt_Click(object sender, EventArgs e)
    {
        Response.Redirect("Schedule.aspx");
    }
}