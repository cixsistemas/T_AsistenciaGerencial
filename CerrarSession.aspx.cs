using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CerrarSession : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Abandon();
        //FormsAuthentication.SignOut();
        Response.Redirect("acceso.aspx");
    }
    //public CerrarSession()
    //{
    //    Load += Page_Load;
    //}
}
