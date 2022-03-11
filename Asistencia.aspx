<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Asistencia.aspx.cs" Inherits="Asistencia" %>

<!DOCTYPE html>
<%--https://blog.jscrambler.com/a-momentjs-in-time/--%>
<%--https://sweetalert2.github.io/--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="icon" type="image/ico" href="imagenes/Sistema.ico" />
    <title>Asistencia</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link href="https://fonts.googleapis.com/css?family=Inconsolata" rel="stylesheet" />
    <script src="moment/moment.min.js"></script>
    <script src="moment/countdown.min.js" type="text/javascript"></script>
    <script src="moment/locale/es.js"></script>

    <!-- Bootstrap -->
    <link href="bootstrap-4.5.0/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Iconos -->
    <link href="bootstrap-4.5.0/icons/css/all.min.css" rel="stylesheet" type="text/css" />
    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <%--<script src="bootstrap-4.5.0/js/jquery-3.5.1.slim.min.js"></script>--%><!-- NO funciona capturar ip -->
    <script src="bootstrap-4.5.0/js/jquery-3.5.1.min.js"></script>
    <!-- SI funciona capturar ip -->
    <script src="bootstrap-4.5.0/js/popper.min.js"></script>
    <script src="bootstrap-4.5.0/js/bootstrap.min.js"></script>
    <%--<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>--%>

    <!-- Capturar Navegador -->
    <script src="Otros_css_js/bowser.min.js"></script>

    <!-- PARA BUSCAR EN DROPDOWN LIST -->
    <script src="bootstrap-4.5.0/js/bootstrap-select.js"></script>
    <link href="bootstrap-4.5.0/css/bootstrap-select.min.css" rel="stylesheet" />

    <!-- MOSTRAR MENSAJE DE CARGA -->
    <link href="Otros_css_js/CargandoDatos.css" rel="stylesheet" />

    <!-- MENSAJE DEL SISTEMA -->
    <script src="Otros_css_js/sweetalert2.all.min.js"></script>
    <link href="Otros_css_js/sweetalert2.min.css" rel="stylesheet" />

    <script type="text/javascript">
        function Confirmar(men) {
            if (!confirm(men))
                return false;
        }

        function displayTime() {
            moment.locale('es');
            var time = moment().format('HH:mm:ss');
            var time2 = moment().format('MMMM Do YYYY, h:mm:ss a')
            var time22 = moment().format('h:mm:ss a')
            var time3 = moment().format('LLLL');
            var time4 = moment().format();
            var time5 = moment().add(7, 'days').subtract(1, 'months').calendar();
            var time6 = moment().add(7, 'days').subtract(1, 'months').year(2009).hours(0).minutes(0).seconds(0).fromNow();
            $('#clock').html(time);
            $('#clock2').html(time22);
            $('#clock3').html(time3);
            $('#clock4').html(time4);
            $('#clock5').html(time5);
            $('#clock6').html(time6);
            setTimeout(displayTime, 1000);
        }

        $(document).ready(function () {
            displayTime();
        });
    </script>



    <%
        string uAg = Request.ServerVariables["HTTP_USER_AGENT"];
        Regex regEx = new Regex(@"android|iphone|ipad|ipod|blackberry|symbianos", RegexOptions.IgnoreCase);
        bool isMobile = regEx.IsMatch(uAg);
        if (isMobile)
        {
            Response.Write("<html><body style='background-color: black; color: white; font-weight: bold;'></body></html>");
            Response.Write("Solo puede registrar asistencia desde una computadora. ");
            Response.Write("Usted esta navegando desde un dispositivo móvil.");
            DdlNroDni.Visible = false;
            btnRegistrar.Visible = false;
            rbPreference.Visible = false;
            rvDdlNroDni.Visible = false;
            rfvDdlNroDni.Visible = false;
            RfvrbPreference.Visible = false;
        }
    %>


    <style>
        .countdown, #clock, #clock2, #clock3, #clock4, #clock5, #clock6 {
            display: flex;
            font-size: 36px;
            color: black;
            text-shadow: 2px 2px 5px darkslategrey;
            font-family: 'Inconsolata';
            text-align: center;
            justify-content: center;
            position: relative;
        }

        .countdown, #FechaActual {
            display: flex;
            font-size: 31px;
            color: black;
            text-shadow: 2px 2px 5px darkslategrey;
            font-family: 'Inconsolata';
            text-align: center;
            justify-content: center;
            position: relative;
        }

        .hacked {
            display: flex;
            opacity: 0;
            margin-left: auto;
            margin-right: auto;
            width: 40%;
            animation-name: pwned;
            animation-duration: 4s;
            animation-timing-function: ease, cubic-bezier(0.1, 0.7, 1.0, 0.1);
            animation-delay: 112s;
            animation-iteration-count: 32;
            animation-direction: normal;
            animation-fill-mode: backwards;
            animation-play-state: running;
        }

        @keyframes pwned {
            0% {
                opacity: 0;
                width: 7px;
                height: 10px;
                top: 50%;
            }

            50% {
                opacity: 1;
                width: 100%;
                height: 100%;
                top: 50%;
            }

            100% {
                opacity: 0;
                top: 50%;
                display: none;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Scripts>
                <%--<asp:ScriptReference Path="~/Otros_css_js/sweetalert2.all.min.js" />--%>
            </Scripts>
        </asp:ScriptManager>
        <div class="container" style="margin-top: 15px">
            <div class="row">
                <div class="col-6 col-md-3"></div>
                <div class="col-md-6 ">
                    <!-- CARD -->
                    <div class="card border-dark">
                        <div class="card-header bg-dark text-white">
                            Asistencia
                        </div>
                        <!-- CARD BODY -->
                        <div class="card-body">
                            <!-- BUSQUEDA -->
                            <div class="form-row">
                                <div class="form-group col-md-12">
                                    <label for="name">
                                        Trabajador:
                                         <asp:RangeValidator ID="rvDdlNroDni" runat="server" BackColor="Yellow"
                                             ControlToValidate="DdlNroDni" ErrorMessage="Busque y Seleccione" ForeColor="Red" MaximumValue="99999"
                                             MinimumValue="1" SetFocusOnError="True" Type="Integer"></asp:RangeValidator>
                                        <asp:RequiredFieldValidator ID="rfvDdlNroDni" runat="server" BackColor="Yellow"
                                            ControlToValidate="DdlNroDni" ErrorMessage="**" ForeColor="Red"
                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:DropDownList ID="DdlNroDni" runat="server"
                                        class="form-control form-control-sm selectpicker"
                                        data-live-search="true" data-container="body">
                                    </asp:DropDownList>
                                </div>


                                <div class="form-group col-md-12">
                                    <label for="name">
                                        Movimiento:
                                        <asp:RequiredFieldValidator ID="RfvrbPreference" runat="server" ControlToValidate="rbPreference"
                                            BackColor="Yellow" ForeColor="Red" ErrorMessage="Seleccione Entrada o Salida">  
                                        </asp:RequiredFieldValidator>
                                    </label>
                                    <asp:RadioButtonList ID="rbPreference" runat="server" AutoPostBack="False"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Text="&nbsp;Entrada &nbsp;&nbsp;" Value="ENTRADA">
                                        </asp:ListItem>
                                        <asp:ListItem Text="&nbsp;Salida" Value="SALIDA"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>

                            <asp:UpdatePanel ID="upresultados" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <!-- MOSTRAR MENSAJE CUANDO HAY UPDATEPANEL -->
                                    <script type="text/javascript">
                                        //Sys.Application.add_load(MostrarMensajeError);
                                        //Sys.Application.add_load(MostrarMensajeExito);
                                        function MostrarMensajeError() {
                                            var mensaje = document.getElementById("__mensaje").value;
                                            if (mensaje != "") {
                                                //Swal.fire('Any fool can use a computer')
                                                Swal.fire({
                                                    title: "Mensaje del Sistema",
                                                    text: mensaje,
                                                    type: 'error',
                                                    showCancelButton: false,
                                                    confirmButtonText: "Aceptar",
                                                }).then(function () {
                                                    if (document.getElementById("__pagina").value != "")
                                                        window.location.href = document.getElementById("__pagina").value;
                                                });
                                            }
                                        }

                                        function MostrarMensajeExito() {
                                            var mensaje = document.getElementById("__mensaje").value;
                                            if (mensaje != "") {
                                                Swal.fire({
                                                    title: "Mensaje del Sistema",
                                                    text: mensaje,
                                                    type: 'success',
                                                    showCancelButton: false,
                                                    confirmButtonText: "Aceptar",
                                                }).then(function () {
                                                    if (document.getElementById("__pagina").value != "")
                                                        window.location.href = document.getElementById("__pagina").value;
                                                });
                                            }
                                        }

                                        //function window_load() {
                                        //    MostrarMensaje()
                                        //}
                                    </script>
                                    <!-- FIN DE MOSTRAR MENSAJE CUANDO HAY UPDATEPANEL -->

                                    <div class="form-group row">
                                        <div class="col-sm-12 text-center">
                                            <div class="btn-group">
                                                <asp:LinkButton ID="btnRegistrar" runat="server" class="btn btn-primary"
                                                    OnClientClick="return Confirmar('¿Desea Registrar Asistencia?');"
                                                    Style="font-family: Calibri; font-size: medium"
                                                    UseSubmitBehavior="false" CausesValidation="true"
                                                    Text="Registrar" OnClick="btnRegistrar_Click" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <div class="col-sm-12 text-center">
                                            <div class="text">
                                                <asp:Label ID="FechaActual" runat="server" Text="Label"></asp:Label>
                                                <div class=".countdown" id="clock2"></div>
                                            </div>
                                            <%--<asp:Label ID="FechaActual" runat="server" Text="Label" Font-Bold="True" Font-Names="Courier New" Font-Size="X-Large"></asp:Label>--%>
                                        </div>
                                    </div>

                                    <div>
                                        <asp:HiddenField ID="__mensaje" runat="server" />
                                        <asp:HiddenField ID="__pagina" runat="server" />
                                        <asp:HiddenField ID="Id_" runat="server" Value="0" Visible="False" />                                       
                                        
                                        <asp:HiddenField ID="Hf_Ip" runat="server" />
                                        
                                        <%--<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>--%>
                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <%--<asp:label id="lbNavegador" runat="server" visible="true"></asp:label>
        <asp:label id="lbVersionNavegador" runat="server" visible="true"></asp:label>--%>
        <asp:HiddenField ID="hfNavegador" runat="server" />
        <asp:HiddenField ID="hfNavegadorVersion" runat="server" />
        <%-- <asp:TextBox ID="Txt_Ip" runat="server"  Enabled="False"></asp:TextBox>--%>
        <%--<asp:Label ID="Label1" runat="server" style="color:white" Text=""></asp:Label>--%>

        <script>
            $.ajax({
                //url: "https://geoip-db.com/jsonp",
                url: "https://geolocation-db.com/jsonp",
                jsonpCallback: "callback",
                dataType: "jsonp",
                success: function (location) {
                    //$('#Label1').html(location.IPv4);
                    $('#<%=Hf_Ip.ClientID%>').val(location.IPv4);
                   <%-- $('#<%=TextBox1.ClientID%>').val(location.IPv4);--%>
                }
            });

            $('#<%=hfNavegador.ClientID%>').val(bowser.name);
            $('#<%=hfNavegadorVersion.ClientID%>').val(bowser.version);
            //$('#lbNavegador').text(bowser.name);
            //$('#lbVersionNavegador').text(bowser.version);
            //document.getElementById("Hf_Navegador").value = navigator.appCodeName;
        </script>

        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="">
            <ProgressTemplate>
                <div class="FondoCargando">
                    <div class="block">
                        <div class="block-in">
                            <div class="clock2"></div>
                            <div class="text-center text-primary">Registrando...</div>
                        </div>
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

    </form>
</body>
</html>
