using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

/// <summary>
/// Descripción breve de IpAccessRepository
/// </summary>
public class IpAccessRepository
{
    private readonly string connectionString;
    public IpAccessRepository()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
        connectionString = ConfigurationManager.AppSettings.Get("CadenaConeccion");
    }

    public List<IpAccess> GetAllIpAccess()
    {
        List<IpAccess> ipAccessList = new List<IpAccess>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT IdIp, Ip, Permiso FROM TbIpAsistenciaWeb";
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                IpAccess ipAccess = new IpAccess
                {
                    IdIp = Convert.ToInt32(reader["IdIp"]),
                    Ip = reader["Ip"].ToString(),
                    Permiso = reader["Permiso"].ToString()
                };
                ipAccessList.Add(ipAccess);
            }
        }

        return ipAccessList;
    }

    public bool IsIpAllowed(string ip)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT COUNT(*) FROM TbIpAsistenciaWeb WHERE Ip = @Ip";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Ip", ip);
            connection.Open();
            int count = (int)command.ExecuteScalar();
            return count > 0;
        }
    }

    public void AddIpAccess(IpAccess ipAccess)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "INSERT INTO TbIpAsistenciaWeb (Ip, Permiso) VALUES (@Ip, @Permiso)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Ip", ipAccess.Ip);
            command.Parameters.AddWithValue("@Permiso", ipAccess.Permiso);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }



    public class IpAccess
    {
        public int IdIp { get; set; }
        public string Ip { get; set; }
        public string Permiso { get; set; }
    }
}