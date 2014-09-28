using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class IE : System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        if (!Request.Browser.Type.ToUpper().Contains("IE"))
            Response.Redirect("Default.aspx");
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}