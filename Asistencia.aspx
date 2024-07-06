<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Asistencia.aspx.cs" Inherits="Asistencia" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="icon" type="image/ico" href="Imagenes/Sistema.ico" />
    <title>Asistencia</title>
    <script src="bootstrap-4.5.0/js/jquery-3.5.1.min.js"></script>
    <%--<script src="https://cdnjs.cloudflare.com/ajax/libs/crypto-js/4.1.1/crypto-js.min.js"></script>--%>


    <script type="text/javascript">
        function getGeolocation() {
            $.ajax({
                url: "https://geolocation-db.com/jsonp",
                jsonpCallback: "callback",
                dataType: "jsonp",
                success: function (location) {
                    $('#<%=Hf_Ip.ClientID%>').val(location.IPv4);
                    var param = $('#<%=Hf_Ip.ClientID%>').val();

                    // Enviar el parámetro al servidor para almacenarlo en la sesión
                    $.ajax({
                        type: "POST",
                        url: "StoreInSession.aspx", // Asegúrate de que esta URL sea correcta
                        data: { param: param },
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
                },
                error: function (xhr, status, error) {
                    console.error("Error al obtener la IP:", error);
                }
            });
        }

        // Ejecutar la función getGeolocation al cargar la página
        $(document).ready(function () {
            getGeolocation();
        });

        <%--function getGeolocation() {
            $.ajax({
                url: "https://geolocation-db.com/jsonp",
                jsonpCallback: "callback",
                dataType: "jsonp",
                success: function (location) {
                    $('#<%=Hf_Ip.ClientID%>').val(location.IPv4);
                    var param = $('#<%=Hf_Ip.ClientID%>').val();

                    // Redirigir a la página después de 1 segundo
                    setTimeout(function () {
                        window.location.href = 'AsistenciaReg.aspx?param=' + param;
                    }, 1000);
                },
                error: function (xhr, status, error) {
                    console.error("Error al obtener la IP:", error);
                }
            });
        }

        // Ejecutar la función getGeolocation al cargar la página
        $(document).ready(function () {
            getGeolocation();
        });--%>
    </script>


</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
        <div>
            <asp:HiddenField ID="Hf_Ip" runat="server" />
        </div>
    </form>
</body>
</html>
