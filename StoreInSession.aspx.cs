using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class StoreInSession : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.HttpMethod == "POST")
        {
            string param = Request.Form["param"];
            if (!string.IsNullOrEmpty(param))
            {
                Session["param"] = param;
                Response.StatusCode = 200;
            }
            else
            {
                Response.StatusCode = 400;
                Response.Write("Parameter 'param' is missing or empty.");
            }
            Response.End();
        }
        else
        {
            Response.StatusCode = 405; // Method Not Allowed
            Response.End();
        }
    }
}