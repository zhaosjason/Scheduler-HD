using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Configuration;
using System.Web.Mail;

public partial class Contact : System.Web.UI.Page
{
    OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConnStr"].ConnectionString);

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Request.Browser.Type.ToUpper().Contains("IE"))
            Response.Redirect("IE.aspx");
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
    protected void btnSend_Click(object sender, EventArgs e)
    {
        string messageSubject = "Message: " + this.tbName.Text;
        string messageBody = "From: " + this.tbName.Text + "<br />" + "Email: " + this.tbEmail.Text +
           "<br />" + "Phone: " + this.tbPhone.Text + "<br />" + "Message: " + this.tbMessage.Text;

        MailMessage oMail = new System.Web.Mail.MailMessage();
        oMail.From = this.tbEmail.Text;
        oMail.To = "schedulerhd@gmail.com";
        oMail.Subject = messageSubject;
        oMail.Body = messageBody;
        oMail.BodyFormat = MailFormat.Html;
        SmtpMail.SmtpServer = "relay-hosting.secureserver.net";
        SmtpMail.Send(oMail);

        logContact();
        Response.Redirect("Home.aspx");
    }

    private void logContact()
    {
        string contactString = "Account with ID " + Session["UserID"] + " sent a message";

        string insertContactCmdStr = "INSERT INTO Log (Kind, Content, Stamp) VALUES (?, ?, ?)";
        System.Data.OleDb.OleDbCommand insertContactCmd = new System.Data.OleDb.OleDbCommand(insertContactCmdStr, conn);
        insertContactCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@Kind", "3"));
        insertContactCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@Content", contactString));
        insertContactCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@Stamp", DateTime.Now.AddHours(3)));

        conn.Open();
        insertContactCmd.ExecuteNonQuery();
        conn.Close();
    }
}