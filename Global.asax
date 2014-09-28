<%@ Application Language="C#" %>

<script runat="server">
    
    void Application_Error(object sender, EventArgs e)
    {
        System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection(ConfigurationManager.ConnectionStrings["dbConnStr"].ConnectionString);

        Exception exc = Server.GetLastError().GetBaseException();

        if (exc.GetType() == typeof(HttpException))
        {
            if (exc.Message.Contains("NoCatch") || exc.Message.Contains("maxUrlLength"))
                return;
            Response.Redirect("Error.aspx");
        }
        else
        {
            string errorMessage = "";
            if (HttpContext.Current.Session != null)
                errorMessage = "User with ID: " + Session["UserID"] + " got an error: " + exc.Message;
            else
                errorMessage = "Unknown user recieved error: " + exc.Message;

            string insertErrorCmdStr = "INSERT INTO Log (Kind, Content, Stamp) VALUES (?, ?, ?)";
            System.Data.OleDb.OleDbCommand insertErrorCmd = new System.Data.OleDb.OleDbCommand(insertErrorCmdStr, conn);
            insertErrorCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@Kind", "0"));
            insertErrorCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@Content", errorMessage));
            insertErrorCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("@Content", DateTime.Now.AddHours(3)));

            conn.Open();
            insertErrorCmd.ExecuteNonQuery();
            conn.Close();
        }
    }

    void Application_Start(object sender, EventArgs e) { }

    void Application_End(object sender, EventArgs e) { }

    void Session_Start(object sender, EventArgs e)
    {
        //Application.Lock();
        
        //Application.UnLock();
    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
    }
       
</script>
