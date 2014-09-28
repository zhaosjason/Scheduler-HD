using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Configuration;
using System.Text;
using System.Security.Cryptography;

public partial class _Default : System.Web.UI.Page
{
    OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConnStr"].ConnectionString);

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Request.Browser.Type.ToUpper().Contains("IE"))
            Response.Redirect("IE.aspx");
        else
        {
            object userID = Session["UserID"];
            if (userID != null && userID.ToString() != "0")
                Response.Redirect("Home.aspx");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string selectUserCmdStr = "SELECT ID, Username, Pass FROM Users WHERE Username = ?";
        OleDbCommand selectUserCmd = new OleDbCommand(selectUserCmdStr, conn);
        selectUserCmd.Parameters.Add(new OleDbParameter("@Username", this.tbLoginUsername.Text.ToLower()));

        conn.Open();
        OleDbDataReader drUser = selectUserCmd.ExecuteReader();

        int ID = 0;
        string password = "";
        Boolean userExists = false;
        while (drUser.Read())
        {
            userExists = true;
            ID = (int)drUser["ID"];
            password = (string)drUser["Pass"];
        }
        drUser.Close();
        conn.Close();

        if (userExists && VerifyHash(this.tbLoginPassword.Text, "SHA512", password))
        {
            Session["UserID"] = ID;
            incrementLogins();
            logLogin();
            Response.Redirect("Home.aspx");
        }
        else if (userExists && !VerifyHash(this.tbLoginPassword.Text, "SHA512", password))
        {
            //Incorrect password
        }
        else if (!userExists)
        {

        }
    }

    private void logLogin()
    {
        if (this.tbLoginUsername.Text.ToLower().Equals("cgregory"))
            return;
        string loginString = this.tbLoginUsername.Text.ToLower() + " logged in";

        string insertLoginCmdStr = "INSERT INTO Log (Kind, Content, Stamp) VALUES (?, ?, ?)";
        System.Data.OleDb.OleDbCommand insertLoginCmd = new System.Data.OleDb.OleDbCommand(insertLoginCmdStr, conn);
        insertLoginCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@Kind", "2"));
        insertLoginCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@Content", loginString));
        insertLoginCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@Stamp", DateTime.Now.AddHours(3)));

        conn.Open();
        insertLoginCmd.ExecuteNonQuery();
        conn.Close();
    }

    private void incrementLogins()
    {
    	if (this.tbLoginUsername.Text.ToLower().Equals("cgregory"))
            return;
    	
        string updateLoginsCmdStr = "UPDATE Users SET Logins = Logins + 1 WHERE ID = ?";
        OleDbCommand updateLoginsCmd = new OleDbCommand(updateLoginsCmdStr, conn);
        updateLoginsCmd.Parameters.Add(new OleDbParameter("@ID", Session["UserID"]));

        conn.Open();
        updateLoginsCmd.ExecuteNonQuery();
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