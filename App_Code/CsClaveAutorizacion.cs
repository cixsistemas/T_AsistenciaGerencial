using System;
using System.Configuration;
using System.Data;

/// <summary>
/// Descripción breve de CsClaveAutorizacion
/// </summary>
public class CsClaveAutorizacion
{
    private readonly string Ruta = ConfigurationManager.AppSettings.Get("CadenaConeccion");
    CsConexion servidor = new CsConexion();
    private string Mensaje;
    private string Pagina;
    private int Respuesta;
    public int ObtenerRespuesta()
    { return Respuesta; }
    public string ObtenerMensaje()
    { return Mensaje; }
    public CsClaveAutorizacion()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }


    public DataTable Listar(string opcion, string Descripcion)
    {
        DataTable dt = new DataTable();    
        try
        {
            servidor.cadenaconexion = Ruta;
            if (servidor.abrirconexion() == true)
            {
                dt = servidor.consultar("[dbo].[PaListarTbClavesAutorizacion]"
                    , opcion, Descripcion).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    servidor.cerrarconexion();
                    Mensaje = "No hay registros para mostrar";
                }
                else
                {
                    servidor.cerrarconexion();
                }
            }
            else
            {
                servidor.cerrarconexion();
                Mensaje = servidor.getMensageError() + " :-(";
            }
        }
        catch (Exception e)
        {
            servidor.cerrarconexion();
            Mensaje = servidor.getMensageError() + (e.Message.ToString()) + " :-((";
        }
        return (dt);
    }

}