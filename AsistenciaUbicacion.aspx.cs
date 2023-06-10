using System;
using System.Configuration;
using System.Data;
using System.Web.UI;

public partial class AsistenciaUbicacion : Page
{
    private string Ruta = ConfigurationManager.AppSettings.Get("CadenaConeccion");
    policia.clsaccesodatos servidor = new policia.clsaccesodatos();
    //string IP;
    string Ubicacion;
    protected void Page_Load(object sender, EventArgs e)
    {
        ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        //IP = Label1.Text.Trim();
        //Title = IP;
        //Title = Label1.Text;

    }

    protected void Page_init(object sender, EventArgs e)
    {
        __mensaje.Value = "";
        __pagina.Value = "";
        //this.nombretipoproduccion.Focus();
        //Listar("", "", "1");
        Obtener_Trabajador("1");
        DdlNroDni.Focus();
        FechaActual.Text = DateTime.Now.ToLongDateString();
        Ubicacion = Request.QueryString["Ubicacion"].Trim();
    }

    private void Obtener_Trabajador(string Opcion)
    {
        try
        {
            servidor.cadenaconexion = Ruta;
            if (servidor.abrirconexion() == true)
            {
                DataTable dt = servidor.consultar("[dbo].[PaListarTrabajadoresAsistenciaWeb]", Opcion).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    servidor.cerrarconexion();
                    __mensaje.Value = "Error, al intentar recuperar datos.";
                    __pagina.Value = "CerrarSession.aspx";
                    //_Lista.ShowMessage(__mensaje, __pagina, "Error, al intentar recuperar datos.", "CerrarSession.aspx");
                }
                else
                {
                    DdlNroDni.DataSource = dt;
                    DdlNroDni.DataTextField = "NOMBRE";
                    DdlNroDni.DataValueField = "CODIGO";
                    DdlNroDni.DataBind();
                    servidor.cerrarconexion();
                }
            }
            else
            {
                servidor.cerrarconexion();
                __mensaje.Value = "servidor.getMensageError()";
                __pagina.Value = "CerrarSession.aspx";
                //_Lista.ShowMessage(__mensaje, __pagina, servidor.getMensageError(), "CerrarSession.aspx");
            }
        }
        catch (Exception)
        {
            servidor.cerrarconexion();
            __mensaje.Value = "Error inesperado al intentar conectarnos con el servidor.";
            __pagina.Value = "CerrarSession.aspx";
            //_Lista.ShowMessage(__mensaje, __pagina, "Error inesperado al intentar conectarnos con el servidor.", "CerrarSession.aspx");
        }
    }

    private void Matenimiento_(int Id_Asistencia, DateTime fecha, string valido, string Ent_Sal, string Desde
    , string paubicacion, string Observacion, string Host, DateTime FechaModi, int Id_Trabajador
    , int Id_Usuario, int Id_Asist_Entrada, int Id_Autoriza, string operacion)
    {
        try
        {
            policia.clsaccesodatos servidor = new policia.clsaccesodatos();
            servidor.cadenaconexion = Ruta;
            if (servidor.abrirconexiontrans() == true)
            {
                servidor.ejecutar("[dbo].[_pa_mantenimiento_Asistencia]",
                                    false,
                                    Id_Asistencia,
                                    fecha,
                                    valido,
                                    Ent_Sal,
                                    Desde,
                                    paubicacion,
                                    Observacion,
                                    Host,
                                    FechaModi,
                                    Id_Trabajador,
                                    Id_Usuario,
                                    Id_Asist_Entrada,
                                    Id_Autoriza,
                                    operacion,
                                    0, "");
                if (servidor.getRespuesta() == 1)
                {
                    servidor.cerrarconexiontrans();
                    btnRegistrar.Visible = false;
                    __mensaje.Value = servidor.getMensaje();
                    __pagina.Value = "AsistenciaUbicacion.aspx?Ubicacion=" + Ubicacion;
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "MostrarMensajeExito();", true);
                }
                else
                {
                    servidor.cancelarconexiontrans();
                    __mensaje.Value = servidor.getMensaje();
                    __pagina.Value = "";
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "MostrarMensajeError();", true);
                    //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert();", true);
                    //this.__pagina.Value = "_Asistencia.aspx";
                }
            }
            else
            {
                servidor.cancelarconexiontrans();
                __mensaje.Value = servidor.getMensageError();
                __pagina.Value = "CerrarSession.aspx";
            }

        }
        catch (Exception)
        {
            __mensaje.Value = "Verifique ";
            __pagina.Value = "";
        }
    }
    protected void btnRegistrar_Click(object sender, EventArgs e)
    {
        __mensaje.Value = "";
        __pagina.Value = "";
        //Title= HiddenField1.Value;
        bool ok;
        ok = rvDdlNroDni.IsValid;
        ok = ok && rfvDdlNroDni.IsValid;
        ok = ok && RfvrbPreference.IsValid;

        if (ok == false)
        {
            return;
        }

        if (Ubicacion == "")
        {
            __mensaje.Value = "Se debe especificar la ubicación desde donde se registra marcación";
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "MostrarMensajeError();", true);
            return;
        }

        DateTime Hoy = DateTime.Today;
        string fecha_actual = Hoy.ToString("dd-MM-yyyy");
        ////TxtFechaActual.Text = fecha_actual;

        Matenimiento_(Convert.ToInt32(Id_.Value.Trim()),
        Convert.ToDateTime(fecha_actual),
        "SI",//VALIDO
        rbPreference.SelectedValue,//ENTRADA o SALIDA
        "WEB",//DESDE
        Ubicacion.ToUpper().Trim(),//UBICACION
        "",//OBSERVACION
        "Ip:" + Hf_Ip.Value + " - Navegador:" + hfNavegador.Value.Trim() + "(" + hfNavegadorVersion.Value.Trim() + ")",//HOSTMODIFICACION
                                                                                                                       //"Ip:" + Hf_Ip.Value,
        Convert.ToDateTime(fecha_actual),//FECHA_MODIFICACION
        Convert.ToInt32(DdlNroDni.SelectedValue),
        10,//ID_USUARIO DE ASISTENCIA
        0,//ID_ASISTENCIA_ENTRADA
        318,//TRABAJADOR_AUTORIZA (SISTEMAS)
        "N");
    }

    //protected void Button1_Click(object sender, EventArgs e)
    //{
    //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert();", true);
    //}
}