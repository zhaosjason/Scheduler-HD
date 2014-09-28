using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Configuration;
using System.Data;

public partial class Admin : System.Web.UI.Page
{
    OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["dbConnStr"].ConnectionString);

    protected void Page_Init(object sender, EventArgs e)
    {
        object userID = Session["UserID"];
        if (userID == null || userID.ToString() == "0")
            Response.Redirect("Default.aspx");
        else
        {
            string selectIsAdminCmdStr = "SELECT Admin FROM Users WHERE ID = ?";
            OleDbCommand selectIsAdminCmd = new OleDbCommand(selectIsAdminCmdStr, conn);
            selectIsAdminCmd.Parameters.Add(new OleDbParameter("@ID", Session["UserID"]));

            conn.Open();
            OleDbDataReader drAdminStatus = selectIsAdminCmd.ExecuteReader();
            int isAdmin = 0;
            while (drAdminStatus.Read())
                isAdmin = (int)drAdminStatus["Admin"];
            drAdminStatus.Close();
            conn.Close();

            if (isAdmin == 0)
                Response.Redirect("Home.aspx");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
}