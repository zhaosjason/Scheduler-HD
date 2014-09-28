using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.OleDb;
using System.Text;
using System.Security.Cryptography;

public partial class Settings : System.Web.UI.Page
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

    protected void btnUpdateAccount_Click(object sender, EventArgs e)
    {
        if(!this.tbOldPassword.Text.Equals("") || !this.tbNewPassword.Text.Equals("") || !this.tbPasswordConfirm.Text.Equals(""))
        {
            if (this.tbNewPassword.Text.Equals(this.tbPasswordConfirm.Text))
            {
                if (this.tbNewPassword.Text.Contains(" "))
                {
                    // GIVE ERROR FOR BAD PASSWORD
                }
                else if (this.tbNewPassword.Text.Length > 20 || this.tbNewPassword.Text.Length < 6)
                {
                    // GIVE ERROR FOR BAD PASSWORD
                }
                else
                {
                    string selectPasswordCmdStr = "SELECT Pass FROM Users WHERE ID = ?";
                    OleDbCommand selectPasswordCmd = new OleDbCommand(selectPasswordCmdStr, conn);
                    selectPasswordCmd.Parameters.Add(new OleDbParameter("@ID", Session["UserID"]));

                    conn.Open();
                    OleDbDataReader drPassword = selectPasswordCmd.ExecuteReader();

                    string password = "";
                    while (drPassword.Read())
                        password = (string)drPassword["Pass"];
                    drPassword.Close();
                    conn.Close();

                    if (VerifyHash(this.tbOldPassword.Text, "SHA512", password))
                    {
                        string updatePasswordCmdStr = "UPDATE Users SET Pass = ? WHERE ID = ?";
                        OleDbCommand updatePasswordCmd = new OleDbCommand(updatePasswordCmdStr, conn);
                        updatePasswordCmd.Parameters.Add(new OleDbParameter("@Pass", ComputeHash(this.tbNewPassword.Text, "SHA512", null)));
                        updatePasswordCmd.Parameters.Add(new OleDbParameter("@ID", Session["UserID"]));
                        conn.Open();
                        updatePasswordCmd.ExecuteNonQuery();
                        conn.Close();
                        Response.Redirect("Home.aspx");
                    }
                }
            }
        }

        if (!this.tbFirstName.Text.Equals("") || !this.tbLastName.Text.Equals("") || !this.tbPasswordForName.Text.Equals(""))
        {
            string selectPasswordCmdStr = "SELECT Pass FROM Users WHERE ID = ?";
            OleDbCommand selectPasswordCmd = new OleDbCommand(selectPasswordCmdStr, conn);
            selectPasswordCmd.Parameters.Add(new OleDbParameter("@ID", Session["UserID"]));

            conn.Open();
            OleDbDataReader drUser = selectPasswordCmd.ExecuteReader();

            string password = "";
            while (drUser.Read())
                password = (string)drUser["Pass"];
            drUser.Close();
            conn.Close();

            if (VerifyHash(this.tbPasswordForName.Text, "SHA512", password))
            {
                string formattedFirstName = "";
                string formattedLastName = "";
                if (!string.IsNullOrEmpty(this.tbFirstName.Text))
                    formattedFirstName = char.ToUpper(this.tbFirstName.Text[0]) + this.tbFirstName.Text.Substring(1).ToLower();
                if (!string.IsNullOrEmpty(this.tbLastName.Text))
                    formattedLastName = char.ToUpper(this.tbLastName.Text[0]) + this.tbLastName.Text.Substring(1).ToLower();

                string updateNameCmdStr = "UPDATE Users SET First_Name = ?, Last_Name = ? WHERE ID = ?";
                OleDbCommand updateNameCmd = new OleDbCommand(updateNameCmdStr, conn);
                updateNameCmd.Parameters.Add(new OleDbParameter("@First_Name", formattedFirstName));
                updateNameCmd.Parameters.Add(new OleDbParameter("@Last_Name", formattedLastName));
                updateNameCmd.Parameters.Add(new OleDbParameter("@ID", Session["UserID"]));
                conn.Open();
                updateNameCmd.ExecuteNonQuery();
                conn.Close();
                Response.Redirect("Home.aspx");
            }
        }
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

    public static bool VerifyHash(string plainText, string hashAlgorithm, string hashValue)
    {
        byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);

        int hashSizeInBits, hashSizeInBytes;

        if (hashAlgorithm == null)
            hashAlgorithm = "";

        switch (hashAlgorithm.ToUpper())
        {
            case "SHA1":
                hashSizeInBits = 160;
                break;

            case "SHA256":
                hashSizeInBits = 256;
                break;

            case "SHA384":
                hashSizeInBits = 384;
                break;

            case "SHA512":
                hashSizeInBits = 512;
                break;

            default: // Must be MD5
                hashSizeInBits = 128;
                break;
        }

        hashSizeInBytes = hashSizeInBits / 8;

        if (hashWithSaltBytes.Length < hashSizeInBytes)
            return false;

        byte[] saltBytes = new byte[hashWithSaltBytes.Length - hashSizeInBytes];

        for (int i = 0; i < saltBytes.Length; i++)
            saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

        string expectedHashString = ComputeHash(plainText, hashAlgorithm, saltBytes);

        return (hashValue == expectedHashString);
    }
}