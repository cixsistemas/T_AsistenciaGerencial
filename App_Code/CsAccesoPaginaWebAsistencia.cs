using System.Configuration;
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

    public void SaveGuidToDatabase(string ip, string ciudad, string guidContent, string permiso)
    {
        using (SqlConnection connection = new SqlConnection(Ruta))
        {
            connection.Open();
            string query = "INSERT INTO [dbo].[TbAccesoPaginaWebAsistencia] ([Ip], [ciudad], [Guid], [Permiso]) VALUES (@Ip, @ciudad, @Guid, @Permiso)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Ip", ip);
                command.Parameters.AddWithValue("@ciudad", ciudad);
                command.Parameters.AddWithValue("@Guid", guidContent);
                command.Parameters.AddWithValue("@Permiso", permiso);
                command.ExecuteNonQuery();
            }
        }
    }

    public bool VerifyGuidInDatabase(string guidContent)
    {
        using (SqlConnection connection = new SqlConnection(Ruta))
        {
            connection.Open();
            string query = "SELECT COUNT(*) FROM [dbo].[TbAccesoPaginaWebAsistencia] WHERE [Guid] = @Guid";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Guid", guidContent);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
    }
}