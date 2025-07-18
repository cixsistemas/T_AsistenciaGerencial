﻿using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;

public partial class Asistencia : Page
{
    private string Ruta = ConfigurationManager.AppSettings.Get("CadenaConeccion");
    CsConexion servidor = new CsConexion();
    Lista _Lista = new Lista();
    CsClaveAutorizacion _CsClaveAutorizacion = new CsClaveAutorizacion();
    CsAccesoPaginaWebAsistencia _CsAccesoAsistenciaWeb = new CsAccesoPaginaWebAsistencia();
    string pagina = "AsistenciaReg.aspx";
    string ubicacion = "";
    string observacion = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
    }

    protected void Page_init(object sender, EventArgs e)
    {
        __mensaje.Value = "";
        __pagina.Value = "";
        Obtener_Trabajador("1");
        DdlNroDni.Focus();
        FechaActual.Text = DateTime.Now.ToLongDateString();
        ////=================================================================
        // *** QUITAR EL COMENTARIO cuando se solucione el problema cuando cambia el ip de forma constante
        //en san ignacio
        //// Obtener el parámetro 'param' de la sesión
        //string param = Session["param"] as string;
        //if (!string.IsNullOrEmpty(param))
        //{
        //    Hf_Ip.Value = param;
        //    Inicializar();
        //}
        ////=================================================================


        //=================================================================
        //*** OJO ==> comentar cuando se solucione el problema cuando cambia el ip de forma constante
        //en san ignacio
        string Opcion = Request.QueryString["Op"];
        //se agrego este if para evitar que pida contraseña, este link AsistenciaReg.aspx?Op=
        //sera usado solo para san ignacio
        if (Opcion == null) //momentaneo
        {
            // Manejo del caso cuando "Op" no está presente en la QueryString
            // Por ejemplo, podrías asignar un valor por defecto o lanzar una excepción personalizada.
            //=================================================================
            // Obtener el parámetro 'param' de la sesión
            string param = Session["param"] as string;
            if (!string.IsNullOrEmpty(param))
            {
                Hf_Ip.Value = param;
                Inicializar();
            }
            pagina = "AsistenciaReg.aspx";
            ubicacion = "";
            observacion = "";
        }
        else
        {
            Hf_Ip.Value = "255.255.255.254";
            pagina = "AsistenciaReg.aspx?Op=";
            ubicacion = "";
            observacion = "TERMINAL DE EMBARQUE";
            hfCiudad.Value = "San Ignacio";
        }
        //=================================================================
    }

    //============================================================================
    #region registrar
    private void Matenimiento_(int Id_Asistencia, DateTime fecha, string valido, string Ent_Sal, string Desde
   , string ubicacion, string Observacion, string Host, DateTime FechaModi, int Id_Trabajador
   , int Id_Usuario, int Id_Asist_Entrada, int Id_Autoriza, string operacion)
    {
        try
        {
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
                                    ubicacion,
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
                    MostrarMensaje(servidor.getMensaje(), pagina, "correcto");
                }
                else
                {
                    servidor.cancelarconexiontrans();
                    MostrarMensaje(servidor.getMensaje(), "", "error");
                }
            }
            else
            {
                servidor.cancelarconexiontrans();
                MostrarMensaje(servidor.getMensageError(), "", "error");
            }

        }
        catch (Exception ex)
        {
            MostrarMensaje("Error: " + ex.Message.ToString(), "", "error");
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

        if (!ok)
        {
            return;
        }

        if (Hf_Ip.Value == null || Hf_Ip.Value == "")
        {
            MostrarMensaje("Error al cargar el IP", "Asistencia.aspx", "error");
            return;
        }

        DateTime Hoy = DateTime.Today;
        string fecha_actual = Hoy.ToString("dd-MM-yyyy");
        string hostModificacion = "Ip:" + Hf_Ip.Value + " - Ciudad: " + hfCiudad.Value.Trim()
        + " - Navegador: " + hfNavegador.Value.Trim() + "(" + hfNavegadorVersion.Value.Trim() + ")";
        ////TxtFechaActual.Text = fecha_actual;

        Matenimiento_(Convert.ToInt32(Id_.Value.Trim()),
        Convert.ToDateTime(fecha_actual),
        "SI",//VALIDO
        rbPreference.SelectedValue,//ENTRADA o SALIDA
        "WEB",//DESDE
        ubicacion,//UBICACION
        observacion,//OBSERVACION
        hostModificacion,//HOSTMODIFICACION
        //"Ip:" + Hf_Ip.Value,
        Convert.ToDateTime(fecha_actual),//FECHA_MODIFICACION
        Convert.ToInt32(DdlNroDni.SelectedValue),
        10,//ID_USUARIO DE ASISTENCIA
        0,//ID_ASISTENCIA_ENTRADA
        318,//TRABAJADOR_AUTORIZA (SISTEMAS)
        "N");
    }
    #endregion
    //============================================================================

    //============================================================================
    #region validar clave
    private void Inicializar()
    {
        if (!_CsAccesoAsistenciaWeb.VerifyGuidInDatabase(Hf_Ip.Value, ""))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "$('#myModalClave').modal();"
  + "setTimeout(function() { $('#TxtClave').focus(); }, 500);", true);
            //MostrarMensaje("Clave no coincide con las registradas en la base de datos.", "", "error");
        }
    }

    private void Inicializar2()
    {
        //=============================================================================================
        //ConsultarClavesAutorizacion();
        if (Verificar())
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "$('#myModalClave').modal();"
              + "setTimeout(function() { $('#TxtClave').focus(); }, 500);", true);
        }
        else
        {
            string pathC = @"C:\MyAppData\unique_01.txt";
            if (File.Exists(pathC))
            {
                string encryptedContent = File.ReadAllText(pathC);
                try
                {
                    string decryptedContent = _Lista.Decrypt(encryptedContent);

                    if (!_CsAccesoAsistenciaWeb.VerifyGuidInDatabase(Hf_Ip.Value, decryptedContent))
                    {
                        File.Delete(pathC); // Eliminar el archivo
                        MostrarMensaje("Clave no coincide, con ninguna de la base de datos.", "", "error");
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "$('#myModalClave').modal();"
              + "setTimeout(function() { $('#TxtClave').focus(); }, 500);", true);
                    }
                }
                catch (Exception ex)
                {
                    MostrarMensaje("[Txt modificado] ==> " + ex.Message.ToString(), "ErrorPage.aspx", "error");
                }
            }
            else
            {
                MostrarMensaje("Txt no existe.", "ErrorPage.aspx", "error");
            }
        }
    }
    protected void BtnClave_Click(object sender, EventArgs e)
    {
        // Obtener la fecha actual
        DateTime today = DateTime.Now;

        // Extraer el día, mes y año
        string day = today.Day.ToString("D2");
        string month = today.Month.ToString("D2");
        string year = today.Year.ToString("D4");

        string claveBD = ConsultarClavesAutorizacion() + day + month + year;
        string claveDigitar = TxtClave.Text.Trim();

        if (claveBD.Trim() == claveDigitar)
        {
            //__mensaje.Value = "Contraseña correcta.";
            //__pagina.Value = pagina;
            //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "MostrarMensajeExito();", true);
            //////ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "MostrarMensajeExito();", true);
            MostrarMensaje("Contraseña correcta.", pagina, "correcto");
            _CsAccesoAsistenciaWeb.SaveGuidToDatabase(Hf_Ip.Value.Trim(), hfCiudad.Value, "", "", "AUTORIZADO", "");
            //CrearTxt(); //se comento el 07-07-2024
        }
        else
        {
            MostrarMensaje("Contraseña incorrecta. Verifique.", "", "error");
        }
    }

    private string ConsultarClavesAutorizacion()
    {
        string clave = "";
        string nombreAcceso = "INGRESAR A PAGINA WEB DE ASISTENCIA";
        DataTable T_ClaveManual = _CsClaveAutorizacion.Listar("1", nombreAcceso.Trim());
        if (T_ClaveManual.Rows.Count > 0)
        {
            clave = T_ClaveManual.Rows[0]["Clave"].ToString();
        }
        else
        {
            MostrarMensaje("No hay clave de autorización para mostrar. Consulte con el administrador.", "ErrorPage.aspx", "error");
        }
        return clave;
    }
    #endregion
    //============================================================================

    //============================================================================
    #region OBTENER DATOS
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
                }
                else
                {
                    //guardar en una sesion el datatable
                    Session["Trabajadores"] = dt;

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
            }
        }
        catch (Exception)
        {
            servidor.cerrarconexion();
            __mensaje.Value = "Error inesperado al intentar conectarnos con el servidor.";
            __pagina.Value = "CerrarSession.aspx";
        }
    }

    #endregion
    //============================================================================

    //============================================================================
    #region generar txt
    public bool Verificar()
    {
        bool solicitarContraseña = false;
        string pathC = @"C:\MyAppData";

        EnsureDirectoryExists(pathC);

        var filesC = Directory.GetFiles(pathC);

        // Caso 1: No hay archivos
        if (filesC.Length == 0)
        {
            solicitarContraseña = true;
        }
        // Caso 2: Hay más de un archivo
        else if (filesC.Length > 1)
        {
            solicitarContraseña = true;
        }

        // No necesitamos comparar con archivos en App_Data ya que se ha eliminado esa lógica
        return solicitarContraseña;
    }

    public void CrearTxt()
    {
        string pathC = @"C:\MyAppData";

        EnsureDirectoryExists(pathC);

        var filesC = Directory.GetFiles(pathC);

        if (filesC.Length == 0)
        {
            string newFile = CreateNewFile(pathC, "unique_01.txt");
        }
        else if (filesC.Length > 1)
        {
            DeleteAllFiles(pathC);
            string newFile = CreateNewFile(pathC, GenerateNextFileName(filesC));
        }

        // Verificar archivos en C:\MyAppData y asegurar que sólo haya un archivo
        if (filesC.Length == 1)
        {
            // Implementar lógica adicional si es necesario
        }
    }

    // Asegurar que el directorio exista, si no, crearlo
    private void EnsureDirectoryExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    // Crear un nuevo archivo con un nombre específico y contenido de GUID
    public string CreateNewFile(string directory, string fileName)
    {
        string filePath = Path.Combine(directory, fileName);
        string guidContent = Guid.NewGuid().ToString();

        _CsAccesoAsistenciaWeb.SaveGuidToDatabase(Hf_Ip.Value.Trim(), hfCiudad.Value, guidContent, "", "AUTORIZADO", "");

        string encryptedContent = _Lista.Encrypt(guidContent);
        File.WriteAllText(filePath, encryptedContent);

        return filePath;
    }

    // Eliminar todos los archivos en un directorio
    private void DeleteAllFiles(string directory)
    {
        foreach (var file in Directory.GetFiles(directory))
        {
            File.Delete(file);
        }
    }

    // Generar el siguiente nombre de archivo basado en los archivos existentes
    private string GenerateNextFileName(string[] existingFiles)
    {
        int maxIndex = existingFiles
            .Select(file => Path.GetFileNameWithoutExtension(file))
            .Where(name => name.StartsWith("unique_"))
            .Select(name =>
            {
                int number;
                if (int.TryParse(name.Substring(7), out number))
                {
                    return number;
                }
                return 0;
            })
            .DefaultIfEmpty(0)
            .Max();

        return string.Format("unique_{0:D2}.txt", maxIndex + 1);
    }
    #endregion
    //============================================================================

    //============================================================================
    #region varios
    private void MostrarMensaje(string mensaje, string pagina, string tipo)
    {
        //if (tipo == "correcto")
        //{
        //    __mensaje.Value = mensaje;
        //    __pagina.Value = pagina;
        //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "MostrarMensajeExito();", true);
        //}
        //else if (tipo == "error")
        //{
        //    __mensaje.Value = mensaje;
        //    __pagina.Value = pagina;
        //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "MostrarMensajeError();", true);
        //}

        __mensaje.Value = mensaje;
        __pagina.Value = pagina;
        string scriptFunction = (tipo == "error") ? "MostrarMensajeError();" : "MostrarMensajeExito();";
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", scriptFunction, true);
    }
    #endregion
    //============================================================================

    //============================================================================
    #region buscar trabajador con el lector de codigo de barras
    [WebMethod]
    public static string ValidarTrabajador(string dni)
    {
        try
        {
            DataTable pasajeros = (DataTable)HttpContext.Current.Session["Trabajadores"];

            if (pasajeros == null)
                return "ERROR: La sesión ha expirado";

            DataRow[] encontrados = pasajeros.Select("DNI = '" + dni + "'");

            if (encontrados.Length > 0)
            {
                return "OK|" + encontrados[0]["CODIGO"].ToString().Trim();
            }
            else
            {
                return "NO_ENCONTRADO";
            }
        }
        catch (Exception ex)
        {
            return "ERROR_GENERAL: " + ex.Message;
        }
    }

    #endregion
    //============================================================================


}