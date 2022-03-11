<%@ Page Language="C#" UnobtrusiveValidationMode="None" AutoEventWireup="true" CodeFile="_Asistencia.aspx.cs" Inherits="_Asistencia" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="icon" type="image/ico" href="/imagenes/Sistema.ico" />
    <title>Asistencia</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->


    <!-- Bootstrap -->
    <link href="bootstrap/css/bootstrap.min.css" rel="stylesheet" />

    <%-- PARA BUSCAR EN DROPDOWN LIST --%>
    <link rel="stylesheet" href="Otros_css_js/chosen.css" />

    <script>
        function muestraReloj() {
            // Compruebo si se puede ejecutar el script en el navegador del usuario
            if (!document.layers && !document.all && !document.getElementById) return;
            // Obtengo la hora actual y la divido en sus partes
            var fechacompleta = new Date();
            var horas = fechacompleta.getHours();
            var minutos = fechacompleta.getMinutes();
            var segundos = fechacompleta.getSeconds();
            var mt = "AM";
            // Pongo el formato 12 horas
            if (horas > 12) {
                mt = "PM";
                horas = horas - 12;
            }
            if (horas == 0) horas = 12;
            // Pongo minutos y segundos con dos dígitos
            if (minutos <= 9) minutos = "0" + minutos;
            if (segundos <= 9) segundos = "0" + segundos;
            // En la variable ‘cadenareloj’ puedes cambiar los colores y el tipo de fuente
            cadenareloj = "<font size='18' face='Courier New' ><b>" + horas + ":" + minutos + ":" + segundos + " " + mt + "</b></font>";
            // Escribo el reloj de una manera u otra, según el navegador del usuario
            if (document.layers) {
                document.layers.spanreloj.document.write(cadenareloj);
                document.layers.spanreloj.document.close();
            }
            else if (document.all) spanreloj.innerHTML = cadenareloj;
            else if (document.getElementById) document.getElementById("spanreloj").innerHTML = cadenareloj;
            // Ejecuto la función con un intervalo de un segundo
            setTimeout("muestraReloj()", 1000);
        }
        // Fin del script –>
    </script>

    <script src="Otros_css_js/jquery-1.6.2.min.js" type="text/javascript"></script>
    <%--<script>
    $(document).ready(parpa);
    function parpa() {
        $('#FechaActual').fadeIn(500).delay(250).fadeOut(500, parpa)
        setTimeout("$('#FechaActual').stop(true,true).css('opacity', 1)", 5000);
    }
</script>--%>
    <script>
        $(document).ready(parpa);
        function parpa() {
            $('#spanreloj').fadeIn(500).delay(250).fadeOut(500, parpa)
            setTimeout("$('#spanreloj').stop(true,true).css('opacity', 1)", 5000);
        }
    </script>

    <%-- PARA SABER SI SE NAVEGA EN UN DISPOSITIVO MOVIL --%>
    <%--<script type="text/javascript">
        var uAg = navigator.userAgent.toLowerCase();
        var isMobile = !!uAg.match(/android|iphone|ipad|ipod|blackberry|symbianos/i);
        if (isMobile) {
            document.write("Esta Navegando desde un dispositivo móvil");
        }        
</script>--%>

    <%
        string uAg = Request.ServerVariables["HTTP_USER_AGENT"];
        Regex regEx = new Regex(@"android|iphone|ipad|ipod|blackberry|symbianos", RegexOptions.IgnoreCase);
        bool isMobile = regEx.IsMatch(uAg);
        if (isMobile)
        {
            Response.Write("<html><body style='background-color: black; color: white; font-weight: bold;'></body></html>");
            Response.Write("Solo puede registrar asistencia desde una computadora. ");
            Response.Write("Usted esta navegando desde un dispositivo móvil.");
            //DdlNroDni.Enabled = false;
            //btnRegistrar.Enabled = false;
            DdlNroDni.Visible = false;
            btnRegistrar.Visible = false;
            Label1.Visible = false;
            Label2.Visible = false;
            LbAsistencia.Visible = false;
            rbPreference.Visible = false;
            rvDdlNroDni.Visible = false;
            rfvDdlNroDni.Visible = false;
            RfvrbPreference.Visible = false;
        }
    %>

    <%-- RELOJ --%>
    <%--<link rel="stylesheet" type="text/css" href= "Reloj/clock.css" />--%>
    <%--<script type="text/javascript" src="Reloj/jquery-1.6.4.min.js"></script>--%>

    <script type="text/javascript">
        $(document).ready(function () {
            // Create two variable with the names of the months and days in an array
            var monthNames = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembrer", "Diciembre"];
            var dayNames = ["Domingo", "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado"]

            // Create a newDate() object
            var newDate = new Date();
            // Extract the current date from Date object
            newDate.setDate(newDate.getDate());
            // Output the day, date, month and year    
            $('#Date').html(dayNames[newDate.getDay()] + " " + newDate.getDate() + ' ' + monthNames[newDate.getMonth()] + ' ' + newDate.getFullYear());

            setInterval(function () {
                // Create a newDate() object and extract the seconds of the current time on the visitor's
                var seconds = new Date().getSeconds();
                // Add a leading zero to seconds value
                $("#sec").html((seconds < 10 ? "0" : "") + seconds);
            }, 1000);

            setInterval(function () {
                // Create a newDate() object and extract the minutes of the current time on the visitor's
                var minutes = new Date().getMinutes();
                // Add a leading zero to the minutes value
                $("#min").html((minutes < 10 ? "0" : "") + minutes);
            }, 1000);

            setInterval(function () {
                // Create a newDate() object and extract the hours of the current time on the visitor's
                var hours = new Date().getHours();
                // Add a leading zero to the hours value
                $("#hours").html((hours < 10 ? "0" : "") + hours);
            }, 1000);

        });
    </script>

    <script type="text/javascript">
        function MostrarMensaje() {
            var mensaje = document.getElementById("__mensaje").value;
            if (mensaje != "") {
                alert(mensaje);
                if (document.getElementById("__pagina").value != "")
                    location.href = document.getElementById("__pagina").value;
            }
        }

        function window_load() {
            MostrarMensaje()
        }

        function Confirmar(men) {
            if (!confirm(men))
                return false;
        }

        function BloqueaIngresoDatos() {
            event.returnValue = false;
        }
    </script>

</head>

<%-- INICIOOOOOOOO ............... MENUUUUUUUUUUUUU --%>
<body onload="MostrarMensaje(); muestraReloj();">
    <form id="form1" runat="server">
        <%--      <iframe class="menu" src="Prueba.aspx"></iframe>
  <iframe class="mainContent" src="Prueba2.aspx"></iframe>--%>
        <div class="col-md-6 col-md-offset-3">
            <table class="table table-condensed">
                <thead>
                    <tr>
                        <td colspan="3"
                            style="text-align: center; color: #FFFFFF; background-color: #000000; font-weight: bold;">
                            <asp:Label ID="LbAsistencia" runat="server" Font-Bold="True" Text="ASISTENCIA"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td
                            style="text-align: center; color: #FFFFFF; background-color: #000000;">
                            <asp:Label ID="Label1" runat="server" Text="NRO. DNI:"></asp:Label>
                            <br />
                            <asp:RangeValidator ID="rvDdlNroDni" runat="server" BackColor="Yellow"
                                ControlToValidate="DdlNroDni" ErrorMessage="Busque y Seleccione DNI" ForeColor="Red" MaximumValue="99999"
                                MinimumValue="1" SetFocusOnError="True" Type="Integer"></asp:RangeValidator>
                            &nbsp;&nbsp;
            <asp:RequiredFieldValidator ID="rfvDdlNroDni" runat="server" BackColor="Yellow"
                ControlToValidate="DdlNroDni" ErrorMessage="*..." ForeColor="Red"
                SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </td>

                        <td style="text-align: Left;">
                            <asp:DropDownList ID="DdlNroDni" runat="server"
                                class="chzn-select form-control input-sm">
                            </asp:DropDownList>
                        </td>

                    </tr>
                    <tr>
                        <td
                            style="text-align: center; color: #FFFFFF; background-color: #000000;">
                            <asp:Label ID="Label2" runat="server" Text="ENTRADA - SALIDA"></asp:Label>

                            <br />
                            <asp:RequiredFieldValidator ID="RfvrbPreference" runat="server" ControlToValidate="rbPreference"
                                BackColor="Yellow" ForeColor="Red" ErrorMessage="Seleccione Entrada o Salida">  
                            </asp:RequiredFieldValidator>
                        </td>
                        <td style="text-align: left;">
                            <asp:RadioButtonList ID="rbPreference" runat="server" AutoPostBack="False"
                                RepeatDirection="Horizontal" OnSelectedIndexChanged="rbPreference_SelectedIndexChanged">
                                <asp:ListItem Text="&nbsp;Entrada &nbsp;&nbsp;" Value="ENTRADA">
                                </asp:ListItem>
                                <asp:ListItem Text="&nbsp;Salida" Value="SALIDA"></asp:ListItem>
                            </asp:RadioButtonList>
                            <%--<asp:RadioButton ID="RbEntrada" runat="server" Text="Entrada" Value = "Entrada"/>
                    &nbsp;&nbsp;
                    <asp:RadioButton ID="RbSalida" runat="server" Text="Salida" Value = "Salida"/>--%>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="3" style="text-align: center;">
                            <asp:Button ID="btnRegistrar" runat="server" class="btn btn-primary"
                                OnClientClick="return Confirmar('¿Desea Registrar Asistencia?');"
                                Style="font-family: Calibri; font-size: medium"
                                Text="Registrar" OnClick="btnRegistrar_Click" />
                            &nbsp;&nbsp;
                    <asp:Button ID="btnModificar" runat="server" class="btn btn-primary"
                        OnClientClick="return Confirmar('¿Desea Modificar Asistencia?');"
                        Style="font-family: Calibri; font-size: medium"
                        Text="Modificar" Visible="False"
                        OnClick="btnModificar_Click" />
                            &nbsp;&nbsp;
                    <asp:Button ID="btnEliminar" runat="server" class="btn btn-danger"
                        OnClientClick="return Confirmar('¿Desea Eliminar Asistencia?');"
                        Style="font-family: Calibri; font-size: medium" Text="Eliminar"
                        Visible="False" OnClick="btnEliminar_Click" />
                            &nbsp;&nbsp;
                    <asp:Button ID="btnCancelar" runat="server" class="btn btn-primary"
                        Style="font-family: Calibri; font-size: medium"
                        Text="Cancelar" Visible="False"
                        OnClick="btnCancelar_Click" PostBackUrl="~/_Asistencia.aspx?Codigo=0" />
                        </td>
                    </tr>


                </thead>
                <tbody>

                    <tr>
                        <td colspan="2">
                            <asp:Table ID="TableTipoproduccion" runat="server" BackColor="White"
                                BorderColor="White" CellPadding="2" CellSpacing="0" Font-Size="Small"
                                GridLines="Both" Style="text-align: left" Width="100%">
                                <asp:TableRow ID="TableRow1" runat="server">
                                    <asp:TableCell ID="TableCell1" runat="server" BackColor="Black" BorderColor="Black"
                                        ForeColor="White" Visible="false">FECHA</asp:TableCell>
                                    <asp:TableCell ID="TableCell2" runat="server" BackColor="Black" BorderColor="Black"
                                        ForeColor="White" Visible="false">NOMBRE</asp:TableCell>
                                    <asp:TableCell ID="TableCell3" runat="server" BackColor="Black" BorderColor="Black"
                                        ForeColor="White" Visible="false">DNI</asp:TableCell>
                                    <asp:TableCell ID="TableCell6" runat="server" BackColor="Black" BorderColor="Black"
                                        ForeColor="White" Visible="false">VALIDO</asp:TableCell>
                                    <asp:TableCell ID="TableCell5" runat="server" BackColor="Black" BorderColor="Black"
                                        ForeColor="White" Visible="false">DNI_NOMBRE</asp:TableCell>
                                    <asp:TableCell ID="TableCell7" runat="server" BackColor="Black" BorderColor="Black"
                                        ForeColor="White" Visible="false">ENTRADA_SALIDA</asp:TableCell>
                                    <asp:TableCell ID="TableCell4" runat="server" BackColor="Black" BorderColor="Black"
                                        ForeColor="White" HorizontalAlign="Center" Visible="false">SELECCIONAR</asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </td>
                    </tr>
                </tbody>


                <tr>
                    <td colspan="3"
                        style="text-align: center; color: #FFFFFF; background-color: #000000;">
                        <asp:Label ID="FechaActual" runat="server" Text="Label" Font-Bold="True" Font-Names="Courier New" Font-Size="X-Large"></asp:Label>
                        <br />
                        <span id="spanreloj" style="font-size: small"></span>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right" colspan="2">
                        <asp:HiddenField ID="__mensaje" runat="server" />
                        <asp:TextBox ID="TxtHostName" runat="server" Visible="False"></asp:TextBox>
                        <asp:HiddenField ID="__pagina" runat="server" />
                        <asp:HiddenField ID="Id_" runat="server" Value="0" Visible="False" />
                    </td>
                </tr>
            </table>
        </div>
    </form>

    <%-- PARA DROPDOWN LIST --%>
    <script src="Otros_css_js/jquery.min.js" type="text/javascript"></script>
    <script src="Otros_css_js/chosen.jquery.js" type="text/javascript"></script>
    <script type="text/javascript"> $(".chzn-select").chosen(); $(".chzn-select-deselect").chosen({ allow_single_deselect: true }); </script>


    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="bootstrap/js/jquery-1.12.4.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="bootstrap/js/bootstrap.min.js"></script>
</body>
</html>
