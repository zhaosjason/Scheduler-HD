using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class Error : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnReturnHome_Click(object sender, EventArgs e)
    {
        Response.Redirect(ConfigurationManager.AppSettings["SiteURL"] + "/Home.aspx");
    }

    protected void btnContact_Click(object sender, EventArgs e)
    {
        Response.Redirect(ConfigurationManager.AppSettings["SiteURL"] + "/Contact.aspx");
    }
}