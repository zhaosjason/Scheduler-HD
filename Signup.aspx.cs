using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;

public partial class Signup : System.Web.UI.Page
{
    OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConnStr"].ConnectionString);

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Request.Browser.Type.ToUpper().Contains("IE"))
            Response.Redirect("IE.aspx");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.lblPasswordConfirm.Visible = false;
    }

    protected void btnSignup_Click(object sender, EventArgs e)
    {
        string selectUsernamesCmdStr = "SELECT Username FROM Users";
        OleDbCommand selectUsernamesCmd = new OleDbCommand(selectUsernamesCmdStr, conn);

        conn.Open();
        OleDbDataReader drUsernames = selectUsernamesCmd.ExecuteReader();
        Boolean usernameAlreadyExists = false;
        while (drUsernames.Read())
        {
            if (drUsernames["Username"].Equals(this.tbSignupUsername.Text.ToLower()))
            {
                usernameAlreadyExists = true;
                break;
            }
        }
        drUsernames.Close();
        conn.Close();

        this.lblPasswordConfirm.Text = "";
        string errorsString = "";

        if (usernameAlreadyExists)
            errorsString += "* That username is taken <br /><br />";
        if (this.tbSignupUsername.Text.IndexOf(' ') != -1)
            errorsString += "* Usernames may not have spaces <br /><br />";
        if (this.tbSignupUsername.Text.Length > 15 || this.tbSignupUsername.Text.Length < 3)
            errorsString += "* Usernames must be between 3 and 15 characters <br /><br />";
        if (!this.tbSignupPassword.Text.Equals(this.tbSignupPasswordConfirm.Text))
             errorsString += "* Passwords do not match <br /><br />";
        if (this.tbSignupPassword.Text.Contains(" "))
            errorsString += "* Passwords must not contain spaces  <br /><br />";
        if (this.tbSignupPassword.Text.Length > 20 || this.tbSignupPassword.Text.Length < 6)
            errorsString += "* Passwords must be between 6 and 20 characters  <br /><br />";
        if (!this.chkAgree.Checked)
            errorsString += "* You must agree to the Terms and Conditions <br /><br />";
        
        if(errorsString.Length < 5)
        {
            string insertUserCmdStr = "INSERT INTO Users(First_Name, Last_Name, Username, Pass, Schedule, Friends, Admin) Values (?, ?, ?, ?, ?, ?, ?)";
            OleDbCommand insertUserCmd = new OleDbCommand(insertUserCmdStr, conn);
            insertUserCmd.Parameters.Add(new OleDbParameter("@First_Name", this.tbSignupFName.Text));
            insertUserCmd.Parameters.Add(new OleDbParameter("@Last_Name", this.tbSignupLName.Text));
            insertUserCmd.Parameters.Add(new OleDbParameter("@Username", this.tbSignupUsername.Text.ToLower()));
            string saltedHashbrowns = ComputeHash(this.tbSignupPassword.Text, "SHA512", null);
            insertUserCmd.Parameters.Add(new OleDbParameter("@Pass", saltedHashbrowns));
            insertUserCmd.Parameters.Add(new OleDbParameter("@Schedule", ""));
            insertUserCmd.Parameters.Add(new OleDbParameter("@Friends", ""));
            insertUserCmd.Parameters.Add(new OleDbParameter("@Admin", "0"));

            conn.Open();
            insertUserCmd.ExecuteNonQuery();
            conn.Close();

            string selectIDCmdStr = "SELECT ID FROM Users WHERE Username = ?";
            OleDbCommand selectIDCmd = new OleDbCommand(selectIDCmdStr, conn);
            selectIDCmd.Parameters.Add(new OleDbParameter("@Username", this.tbSignupUsername.Text.ToLower()));

            conn.Open();
            OleDbDataReader drUserID = selectIDCmd.ExecuteReader();
            int ID = 0;
            while (drUserID.Read())
                ID = (int)drUserID["ID"];
            drUserID.Close();
            conn.Close();

            logSignup();
            Session["UserID"] = ID;
            Response.Redirect("Home.aspx");
        }

        this.lblPasswordConfirm.Visible = true;
        this.lblPasswordConfirm.Text = errorsString;
    }

    private void logSignup()
    {
        string signupString = this.tbSignupUsername.Text.ToLower() + " signed up";

        string insertSignupCmdStr = "INSERT INTO Log (Kind, Content, Stamp) VALUES (?, ?, ?)";
        System.Data.OleDb.OleDbCommand insertSignupCmd = new System.Data.OleDb.OleDbCommand(insertSignupCmdStr, conn);
        insertSignupCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@Kind", "1"));
        insertSignupCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@Content", signupString));
        insertSignupCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@Stamp", DateTime.Now.AddHours(3)));

        conn.Open();
        insertSignupCmd.ExecuteNonQuery();
        conn.Close();
    }

    public static string ComputeHash(string plainText, string hashAlgorithm, byte[] saltBytes)
    {
        if (saltBytes == null)
        {
            int minSaltSize = 4;
            int maxSaltSize = 8;

            Random random = new Random();
            int saltSize = random.Next(minSaltSize, maxSaltSize);

            saltBytes = new byte[saltSize];

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            rng.GetNonZeroBytes(saltBytes);
        }

        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

        byte[] plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];

        for (int i = 0; i < plainTextBytes.Length; i++)
            plainTextWithSaltBytes[i] = plainTextBytes[i];

        for (int i = 0; i < saltBytes.Length; i++)
            plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

        HashAlgorithm hash;

        if (hashAlgorithm == null)
            hashAlgorithm = "";

        switch (hashAlgorithm.ToUpper())
        {
            case "SHA1":
                hash = new SHA1Managed();
                break;

            case "SHA256":
                hash = new SHA256Managed();
                break;

            case "SHA384":
                hash = new SHA384Managed();
                break;

            case "SHA512":
                hash = new SHA512Managed();
                break;

            default:
                hash = new MD5CryptoServiceProvider();
                break;
        }

        byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

        byte[] hashWithSaltBytes = new byte[hashBytes.Length + saltBytes.Length];

        for (int i = 0; i < hashBytes.Length; i++)
            hashWithSaltBytes[i] = hashBytes[i];

        for (int i = 0; i < saltBytes.Length; i++)
            hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

        string hashValue = Convert.ToBase64String(hashWithSaltBytes);

        return hashValue;
    }
}