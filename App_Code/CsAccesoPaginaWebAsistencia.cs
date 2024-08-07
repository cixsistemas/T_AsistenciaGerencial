﻿using System.Configuration;
using System.Data.SqlClient;

/// <summary>
/// Descripción breve de CsAccesoAsistenciaWeb
/// </summary>
public class CsAccesoPaginaWebAsistencia
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

    public CsAccesoPaginaWebAsistencia()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public void SaveGuidToDatabase(string ip, string ciudad, string guidContent, string mac, string permiso
        ,string observacion)
    {
        using (SqlConnection connection = new SqlConnection(Ruta))
        {
            connection.Open();
            string query 
                = "INSERT INTO [dbo].[TbAccesoPaginaWebAsistencia] ([Ip], [ciudad], [Guid], [Mac], [Permiso], [Observacion]) " +
                "VALUES (@Ip, @ciudad, @Guid, @Mac, @Permiso, @Observacion)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Ip", ip);
                command.Parameters.AddWithValue("@ciudad", ciudad);
                command.Parameters.AddWithValue("@Guid", guidContent);
                command.Parameters.AddWithValue("@Mac", mac);
                command.Parameters.AddWithValue("@Permiso", permiso);
                command.Parameters.AddWithValue("@Observacion", observacion);
                command.ExecuteNonQuery();
            }
        }
    }

    public bool VerifyGuidInDatabase(string ip, string guidContent)//variable guidContent ya no se usa
    {
        using (SqlConnection connection = new SqlConnection(Ruta))
        {
            connection.Open();
            //string query = "SELECT COUNT(*) FROM [dbo].[TbAccesoPaginaWebAsistencia] WHERE ([Ip] = @ip OR @ip = '') AND ([Guid] = @Guid)";
            string query = "SELECT COUNT(*) FROM [dbo].[TbAccesoPaginaWebAsistencia] WHERE ([Ip] = @ip)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ip", ip);
                //command.Parameters.AddWithValue("@Guid", guidContent);               
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
    }
}