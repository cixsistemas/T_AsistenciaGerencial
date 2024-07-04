using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;

public partial class Asistencia : Page
{
    private string Ruta = ConfigurationManager.AppSettings.Get("CadenaConeccion");
    CsConexion servidor = new CsConexion();
    Lista _Lista = new Lista();
    CsClaveAutorizacion _CsClaveAutorizacion = new CsClaveAutorizacion();
    string idClaveAutorizacion = "16";//INGRESAR A WEB DE ASISTENCIA

    protected void Page_Load(object sender, EventArgs e)
    {
        ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        ////agregado el 03-07-2024
        //if (!IsPostBack)
        //{
        //    //string clientIp = Request.ServerVariables["REMOTE_ADDR"];
        //    Title = TxtIp.Text.Trim();
        //    //VerificarIP();
        //}
    }

    protected void Page_init(object sender, EventArgs e)
    {
        __mensaje.Value = "";
        __pagina.Value = "";
        Obtener_Trabajador("1");
        DdlNroDni.Focus();
        FechaActual.Text = DateTime.Now.ToLongDateString();
        //Title = TxtIp.Text.Trim();
        string ipValor = Hf_Ip.Value.Trim();
        VerificarAcceso(ipValor.Trim());

        if (Verificar())
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "$('#myModalClave').modal();"
              + "setTimeout(function() { $('#TxtClave').focus(); }, 500);", true);
        }

        //CrearTxt();
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
                    __mensaje.Value = servidor.getMensaje();
                    __pagina.Value = "Asistencia.aspx";
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
        catch (Exception ex)
        {
            __mensaje.Value = "Verifique " + ex.Message.ToString();
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

        if (!ok)
        {
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
        "",//UBICACION
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
    #endregion
    //============================================================================

    //============================================================================
    #region validar clave
    protected void BtnClave_Click(object sender, EventArgs e)
    {
        if (true)
        {

        }
    }

    private void ConsultarClavesAutorizacion()
    {
        DataTable T_ClaveManual = _CsClaveAutorizacion.Listar("1", "INGRESAR A WEB DE ASISTENCIA");
        //hfClaveAutorizacion.Value = T_ClaveManual.Rows[0]["Clave"].ToString();
        hfClaveAutorizacion.Value = T_ClaveManual.Rows[0]["Clave"].ToString();
    }
    #endregion
    //============================================================================

    //============================================================================
    #region validar ip 
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
    private void LogUnsuccessfulAccessAttempt(string ip)
    {
        string logDirectory = Server.MapPath("~/Archivos");
        string logFilePath = Path.Combine(logDirectory, "AccessAttempts.txt");

        string logEntry = string.Format("IP: {0}, Fecha y Hora: {1}\n", ip, DateTime.Now);

        try
        {
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            File.AppendAllText(logFilePath, logEntry);
        }
        catch (Exception ex)
        {
            // Manejar cualquier error de IO aquí
            // Puedes registrar el error o notificar al administrador
        }
    }

    private void VerificarAcceso(string clientIp)
    {
        //string clientIp = Hf_Ip.Value;
        string direccionIP = _Lista.LimpiarDireccionIP(clientIp);
        IpAccessRepository repository = new IpAccessRepository();

        if (!repository.IsIpAllowed(clientIp))
        {
            //Title = clientIp;
            LogUnsuccessfulAccessAttempt(clientIp);
            //Response.Redirect("ErrorPage.aspx?Ip=" + clientIp);
        }
        else
        {
            // Procesar la página normalmente
            var ipAccessList = repository.GetAllIpAccess();
            foreach (var ipAccess in ipAccessList)
            {
                //// Procesar la lista de IPs
                //Response.Write($"ID: {ipAccess.IdIp}, IP: {ipAccess.Ip}, Permiso: {ipAccess.Permiso}<br/>");
            }

            //// Agregar una nueva IP (ejemplo)
            //IpAccess newIpAccess = new IpAccess
            //{
            //    Ip = "192.168.1.100",
            //    Permiso = "Allow"
            //};
            //repository.AddIpAccess(newIpAccess);
        }
    }

    public bool Verificar()
    {
        bool solicitarContraseña = false;
        string pathC = @"C:\MyAppData";
        string pathAppData = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");

        EnsureDirectoryExists(pathC);
        EnsureDirectoryExists(pathAppData);

        var filesC = Directory.GetFiles(pathC);
        var filesAppData = Directory.GetFiles(pathAppData);

        // Caso 1 y 2 combinados
        if (filesC.Length == 0)
        {
            solicitarContraseña = true;
        }
        // Caso 3
        else if (filesC.Length > 1)
        {
            solicitarContraseña = true;
        }
        // Caso 4: Comparar archivos individuales
        else if (filesC.Length == 1 && filesAppData.Length == 1)
        {
            if (Path.GetFileName(filesC[0]) != Path.GetFileName(filesAppData[0]))
            {
                solicitarContraseña = true;
            }
        }

        // Verificar archivos en C:\MyAppData no presentes en ~/App_Data/
        if (!solicitarContraseña)
        {
            foreach (var file in filesC)
            {
                string fileName = Path.GetFileName(file);
                if (!filesAppData.Any(f => Path.GetFileName(f) == fileName))
                {
                    solicitarContraseña = true;
                    break;
                }
            }
        }

        return solicitarContraseña;
    }
    //public void CrearTxt()
    //{

    //    // Definir las rutas de los directorios
    //    string pathC = @"C:\MyAppData";
    //    string pathAppData = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");

    //    // Asegurar que ambos directorios existan
    //    EnsureDirectoryExists(pathC);
    //    EnsureDirectoryExists(pathAppData);

    //    // Obtener todos los archivos en ambos directorios
    //    var filesC = Directory.GetFiles(pathC);
    //    var filesAppData = Directory.GetFiles(pathAppData);

    //    //Caso 1: Si ambos directorios están vacíos, crear un archivo en cada uno
    //    if (filesC.Length == 0 && filesAppData.Length == 0)
    //    {
    //        string newFileC = CreateNewFile(pathC, "unique_01.txt");
    //        CreateNewFile(pathAppData, "unique_01.txt");
    //    }
    //    // Caso 2: Si C:\MyAppData está vacío y ~/App_Data/ tiene archivos, crear un nuevo archivo 
    //    // en ~/App_Data/ y copiarlo a C:\MyAppData
    //    else if (filesC.Length == 0 && filesAppData.Length > 0)
    //    {
    //        string newFileAppData = CreateNewFile(pathAppData, GenerateNextFileName(filesAppData));
    //        CopyFile(newFileAppData, pathC);
    //    }
    //    // Caso 3: Si C:\MyAppData tiene más de un archivo, eliminar todos los archivos y crear uno nuevo 
    //    //en ~/App_Data/ y copiarlo a C:\MyAppData
    //    else if (filesC.Length > 1)
    //    {
    //        DeleteAllFiles(pathC);
    //        string newFileAppData = CreateNewFile(pathAppData, GenerateNextFileName(filesAppData));
    //        CopyFile(newFileAppData, pathC);
    //    }
    //    // Caso 4: Si ambos directorios tienen un archivo, comparar nombres de archivos y actuar en consecuencia
    //    else if (filesC.Length == 1 && filesAppData.Length == 1)
    //    {
    //        string fileC = Path.GetFileName(filesC[0]);
    //        string fileAppData = Path.GetFileName(filesAppData[0]);

    //        if (fileC != fileAppData)
    //        {
    //            DeleteAllFiles(pathC);
    //            string newFileAppData = CreateNewFile(pathAppData, GenerateNextFileName(filesAppData));
    //            CopyFile(newFileAppData, pathC);
    //        }
    //    }

    //    // Verificar si hay algún archivo en C:\MyAppData que no exista en ~/App_Data/
    //    foreach (var file in filesC)
    //    {
    //        string fileName = Path.GetFileName(file);
    //        if (!filesAppData.Any(f => Path.GetFileName(f) == fileName))
    //        {
    //            File.Delete(file);
    //            string newFileAppData = CreateNewFile(pathAppData, GenerateNextFileName(filesAppData));
    //            CopyFile(newFileAppData, pathC);
    //        }
    //    }


    //}

    public void CrearTxt()
    {
        string pathC = @"C:\MyAppData";
        string pathAppData = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");

        EnsureDirectoryExists(pathC);
        EnsureDirectoryExists(pathAppData);

        var filesC = Directory.GetFiles(pathC);
        var filesAppData = Directory.GetFiles(pathAppData);

        string newFileAppData;
        if (filesC.Length == 0 && filesAppData.Length == 0)
        {
            CreateNewFile(pathC, "unique_01.txt");
            CreateNewFile(pathAppData, "unique_01.txt");
        }
        else if (filesC.Length == 0 && filesAppData.Length > 0)
        {
            newFileAppData = CreateNewFile(pathAppData, GenerateNextFileName(filesAppData));
            CopyFile(newFileAppData, pathC);
        }
        else if (filesC.Length > 1)
        {
            DeleteAllFiles(pathC);
            newFileAppData = CreateNewFile(pathAppData, GenerateNextFileName(filesAppData));
            CopyFile(newFileAppData, pathC);
        }
        else if (filesC.Length == 1 && filesAppData.Length == 1)
        {
            if (Path.GetFileName(filesC[0]) != Path.GetFileName(filesAppData[0]))
            {
                DeleteAllFiles(pathC);
                newFileAppData = CreateNewFile(pathAppData, GenerateNextFileName(filesAppData));
                CopyFile(newFileAppData, pathC);
            }
        }

        // Verificar archivos en C:\MyAppData no presentes en ~/App_Data/
        foreach (var file in filesC)
        {
            string fileName = Path.GetFileName(file);
            if (!filesAppData.Any(f => Path.GetFileName(f) == fileName))
            {
                File.Delete(file);
                newFileAppData = CreateNewFile(pathAppData, GenerateNextFileName(filesAppData));
                CopyFile(newFileAppData, pathC);
            }
        }
    }

    // Asegurar que el directorio exista, si no, crearlo
    static void EnsureDirectoryExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    // Crear un nuevo archivo con un nombre específico y contenido de GUID
    static string CreateNewFile(string directory, string fileName)
    {
        string filePath = Path.Combine(directory, fileName);
        string fileContent = Guid.NewGuid().ToString();
        File.WriteAllText(filePath, fileContent);
        return filePath;
    }

    // Copiar un archivo de una ubicación a otra
    static string CopyFile(string sourceFile, string destinationDirectory)
    {
        string fileName = Path.GetFileName(sourceFile);
        string destinationFile = Path.Combine(destinationDirectory, fileName);
        File.Copy(sourceFile, destinationFile, true);
        return destinationFile;
    }

    // Eliminar todos los archivos en un directorio
    static void DeleteAllFiles(string directory)
    {
        foreach (var file in Directory.GetFiles(directory))
        {
            File.Delete(file);
        }
    }

    // Generar el siguiente nombre de archivo basado en los archivos existentes
    static string GenerateNextFileName(string[] existingFiles)
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


}