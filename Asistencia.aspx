<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Asistencia.aspx.cs" Inherits="Asistencia" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="icon" type="image/ico" href="Imagenes/Sistema.ico" />
    <title>Asistencia</title>
    <script src="bootstrap-4.5.0/js/jquery-3.5.1.min.js"></script>
 

    <script type="text/javascript">
        function getGeolocation() {
            $.ajax({
                url: "https://geolocation-db.com/jsonp",
                jsonpCallback: "callback",
                dataType: "jsonp",
                success: function (location) {
                    handleIpSuccess(location.IPv4);
                },
                error: function (xhr, status, error) {
                    console.error("Error al obtener la IP desde geolocation-db:", error);
                    // Intentar obtener la IP desde la segunda fuente
                    getIpAddress();
                }
            });
        }

        function getIpAddress() {
            fetch('https://api.ipify.org?format=json')
                .then(response => response.json())
                .then(data => {
                    handleIpSuccess(data.ip);
                })
                .catch(error => {
                    console.error('Error fetching IP address from ipify:', error);
                    // Intentar obtener la IP desde la tercera fuente
                    getAlternativeIpAddress();
                });
        }

        function getAlternativeIpAddress() {
            fetch('https://jsonip.com')
                .then(response => response.json())
                .then(data => {
                    handleIpSuccess(data.ip);
                })
                .catch(error => {
                    console.error('Error fetching IP address from jsonip:', error);
                    // Manejo de error si todas las fuentes fallan
                    alert('No se pudo obtener la dirección IP.');
                });
        }

        function handleIpSuccess(ip) {
            // Obtener el elemento oculto por su ID
            var hiddenIpAddress = document.getElementById('<%= Hf_Ip.ClientID %>');

            // Asignar la dirección IP al valor del campo oculto
            hiddenIpAddress.value = ip;

            // Enviar el parámetro al servidor para almacenarlo en la sesión
            $.ajax({
                type: "POST",
                url: "StoreInSession.aspx", // Asegúrate de que esta URL sea correcta
                data: { param: ip },
                success: function () {
                    // Redirigir a la página después de 1 segundo sin el parámetro en la URL
                    setTimeout(function () {
                        window.location.href = 'AsistenciaReg.aspx';
                    }, 1000);
                },
                error: function (xhr, status, error) {
                    console.error("Error al almacenar el parámetro en la sesión:", error);
                }
            });
        }

        // Ejecutar la función getGeolocation al cargar la página
        $(document).ready(function () {
            getGeolocation();
        });
    </script>


</head>
<body>
    <form id="form1" runat="server"> 
        <div>
            <asp:HiddenField ID="Hf_Ip" runat="server" />
        </div>
        <%--<button onclick="generateFile()">Descargar Archivo TXT</button>--%>
        <%--<script>
        function generateFile() {
            var content = "Este es el contenido del archivo de texto generado.";
            var blob = new Blob([content], { type: 'text/plain' });
            var url = URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.href = url;
            a.download = 'sample.txt';
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
            URL.revokeObjectURL(url);
        } </script>--%>
    </form>
</body>
</html>
