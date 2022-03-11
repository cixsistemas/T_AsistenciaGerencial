<%@ Application Language="C#" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="System.Threading" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Código que se ejecuta al iniciarse la aplicación

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Código que se ejecuta al cerrarse la aplicación

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Código que se ejecuta cuando se produce un error sin procesar

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Código que se ejecuta al iniciarse una nueva sesión

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Código que se ejecuta cuando finaliza una sesión. 
        // Nota: el evento Session_End se produce solamente con el modo sessionstate
        // se establece como InProc en el archivo Web.config. Si el modo de sesión se establece como StateServer
        // o SQLServer, el evento no se produce.

    }

    protected void Application_BeginRequest(object sender, EventArgs e)
    {
        // **************** FUNCIONA DE MODO LOCAL Y EN AZURE ************************
        CultureInfo cInfo = new CultureInfo("es-PE");
        cInfo.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
        cInfo.DateTimeFormat.DateSeparator = "/";
        Thread.CurrentThread.CurrentCulture = cInfo;
        Thread.CurrentThread.CurrentUICulture = cInfo;
    }
       
</script>
