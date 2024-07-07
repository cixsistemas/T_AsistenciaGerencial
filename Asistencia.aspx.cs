using System;
using System.IO;
using System.Net;
using System.Web.Services;
using System.Web.UI;

public partial class Asistencia :Page
{
    Lista _Lista = new Lista();
    protected void Page_Load(object sender, EventArgs e)
    {
        ////if (!IsPostBack)
        ////{
        ////    // Llamar a la función para obtener la geolocalización
        ////    GetGeolocation();
        ////}

        //string filePath = Server.MapPath("~/App_Data/sample.txt");

        //// Crear el contenido del archivo
        //string content = "Este es el contenido del archivo de texto generado.";

        //// Escribir el contenido en el archivo
        //File.WriteAllText(filePath, content);

        //// Configurar la respuesta para descargar el archivo
        //Response.Clear();
        //Response.ContentType = "text/plain";
        //Response.AddHeader("Content-Disposition", "attachment; filename=sample.txt");
        //Response.WriteFile(filePath);
        //Response.End();
    }


    private void GetGeolocation()
    {
        try
        {
            // URL del servicio de geolocalización JSONP
            string geoLocationUrl = "https://geolocation-db.com/jsonp";

            // Crear solicitud HTTP
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(geoLocationUrl);
            request.Method = "GET";

            // Obtener la respuesta
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                // Leer la respuesta
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string jsonResponse = reader.ReadToEnd();

                    // Eliminar el padding de la función de callback
                    string json = ExtractJsonFromJsonp(jsonResponse);

                    // Procesar el JSON
                    dynamic location = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                    // Obtener IPv4 si está presente
                    string ipv4 = null;
                    if (location != null && location.IPv4 != null)
                    {
                        ipv4 = location.IPv4.ToString();
                    }


                    if (!string.IsNullOrEmpty(ipv4))
                    {
                        string ipEnviar = _Lista.Encrypt(ipv4);
                        // Redirigir a la página después de 1 segundo con el parámetro IPv4
                        string redirectUrl = "AsistenciaReg.aspx?param=" + ipEnviar;
                        ClientScript.RegisterStartupScript(this.GetType(), "redirect", "setTimeout(function() { window.location.href = '" + redirectUrl + "'; }, 1000);", true);
                    }
                    else
                    {
                        // Manejar caso donde no se pudo obtener la IP
                        Response.Write("No se pudo obtener la dirección IP del servicio de geolocalización.");
                    }
                }
            }
        }
        catch (WebException webEx)
        {
            // Capturar excepciones de solicitud HTTP
            Console.WriteLine("Error de solicitud HTTP: " + webEx.Message);
            Response.Write("Error de solicitud HTTP al obtener la IP.");
        }
        catch (Exception ex)
        {
            // Capturar cualquier otra excepción
            Console.WriteLine("Error general: " + ex.Message);
            Response.Write("Error general al obtener la IP: " + ex.Message);
        }
    }

    // Función para extraer el JSON del JSONP
    private string ExtractJsonFromJsonp(string jsonp)
    {
        int startIndex = jsonp.IndexOf('(');
        int endIndex = jsonp.LastIndexOf(')');
        if (startIndex != -1 && endIndex != -1)
        {
            return jsonp.Substring(startIndex + 1, endIndex - startIndex - 1);
        }
        return jsonp;
    }

    
}