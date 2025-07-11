﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AsistenciaReg.aspx.cs" Inherits="Asistencia" %>

<!DOCTYPE html>
<%--https://blog.jscrambler.com/a-momentjs-in-time/--%>
<%--https://sweetalert2.github.io/--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="icon" type="image/ico" href="Imagenes/Sistema.ico" />
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

    <!-- Capturar Navegador -->
    <script src="Otros_css_js/bowser.min.js"></script>

    <!-- PARA BUSCAR EN DROPDOWN LIST -->
    <script src="bootstrap-4.5.0/js/bootstrap-select.js"></script>
    <link href="bootstrap-4.5.0/css/bootstrap-select.min.css" rel="stylesheet" />

    <!-- MOSTRAR MENSAJE DE CARGA -->
    <link href="Otros_css_js/CargandoDatos.css" rel="stylesheet" />

    <!-- MENSAJE DEL SISTEMA -->
    <script src="Otros_css_js/sweetalert2@11.js"></script>

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

    <script type="text/javascript">
        function redirectOnErrorPage() {
            // Redirigir a la página de error
            window.location.href = 'ErrorPage.aspx';
        }

        function convertirAMayusculas(elemento) {
            elemento.value = elemento.value.toUpperCase();
        }
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            // Cambia el focus según el radio
            $('input[name="<%=RbOpcion.UniqueID%>"]').change(function (e) {
                var opcion = $('input[name="<%=RbOpcion.UniqueID%>"]:checked').val();

                if (opcion == "LECTOR") {
                    // Desactivar el select
                    $('#<%=DdlNroDni.ClientID%>').prop('disabled', true).selectpicker('refresh');
                    // Activar el input
                    $('#txtDni').focus();

                } else {
                    // Activar el select
                    $('#<%=DdlNroDni.ClientID%>').prop('disabled', false).selectpicker('refresh');
                    // Poner el focus en el select
                    $('#<%=DdlNroDni.ClientID%>').focus();
                }
            });

            // Detectar escaneo automáticamente
            $('#txtDni').on('input', function (e) {
                var dni = $(this).val().trim();

                if (dni.length == 8) {
                    escanearCodigoBarras(dni);
                }
            });
        });

        function escanearCodigoBarras(dni) {
            var opcion = $('input[name="<%=RbOpcion.UniqueID%>"]:checked').val();

            if (opcion == "LECTOR") {
                $.ajax({
                    type: "POST",
                    url: "AsistenciaReg.aspx/ValidarTrabajador",
                    data: JSON.stringify({ dni: dni }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var res = msg.d;

                        if (res.startsWith("OK|")) {
                            var id = res.split('|')[1];
                            var ddl = document.getElementById("<%=DdlNroDni.ClientID%>");
                            ddl.value = id;

                            // Refresca el selectpicker
                            $('#<%=DdlNroDni.ClientID%>').selectpicker('refresh');

                                limpiarCampo();
                            } else if (res == "NO_ENCONTRADO") {
                                alert("DNI NO ENCONTRADO.");
                                limpiarCampo();
                            } else {
                                console.error("Error.", res);
                                alert("Error.");
                                limpiarCampo();
                            }
                    },
                    error: function (err) {
                        console.error(err);
                        alert("Error.");
                        limpiarCampo();
                    }
                });
                }
            }

            function limpiarCampo() {
                var opcion = $('input[name="<%=RbOpcion.UniqueID%>"]:checked').val();

                if (opcion == "LECTOR") {
                    $('#txtDni').val('');
                    $('#txtDni').focus();
                } else {
                    $('#<%=DdlNroDni.ClientID%>').focus();
                }
            }
    </script>

    <style>
        #txtDni {
            width: 1px;
            height: 1px;
            border: none;
            padding: 0;
            margin: 0;
            background: transparent;
            color: transparent;
            caret-color: transparent;
            outline: none;
        }
    </style>


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
                                    <label for="name">Opción</label>
                                    <asp:RadioButtonList ID="RbOpcion" runat="server" AutoPostBack="False"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Text="&nbsp;Normal &nbsp;&nbsp;" Value="NORMAL" Selected="True">
                                        </asp:ListItem>
                                        <asp:ListItem Text="&nbsp;Lector" Value="LECTOR"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div class="form-group col-md-12">
                                    <!-- Campo de escaneo (oculto) -->
                                    <label for="<%= DdlNroDni.ClientID %>">
                                        Trabajador
                                         <asp:RangeValidator ID="rvDdlNroDni" runat="server" BackColor="Yellow"
                                             ControlToValidate="DdlNroDni" ErrorMessage="Busque y Seleccione" ForeColor="Red" MaximumValue="99999"
                                             MinimumValue="1" SetFocusOnError="True" Type="Integer"></asp:RangeValidator>
                                        <asp:RequiredFieldValidator ID="rfvDdlNroDni" runat="server" BackColor="Yellow"
                                            ControlToValidate="DdlNroDni" ErrorMessage="**" ForeColor="Red"
                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    </label>
                                    <!-- Input muy pequeño, invisible pero con el focus -->
                                    <input id="txtDni" type="text" />
                                    <asp:DropDownList ID="DdlNroDni" runat="server" CssClass="form-control form-control-sm selectpicker"
                                        data-live-search="true" data-container="body">
                                    </asp:DropDownList>
                                </div>

                                <div class="form-group col-md-12">
                                    <label for="<%= rbPreference.ClientID %>">
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
                                        function MostrarMensaje(tipo) {
                                            var mensaje = document.getElementById("__mensaje").value;
                                            if (mensaje !== "") {
                                                var tipoAlerta = (tipo === 'error') ? 'error' : 'success';
                                                Swal.fire({
                                                    title: "Mensaje del Sistema",
                                                    text: mensaje,
                                                    icon: tipoAlerta,  // Cambié 'type' a 'icon' porque 'type' está deprecado en SweetAlert2
                                                    showCancelButton: false,
                                                    confirmButtonText: "Aceptar",
                                                }).then(function () {
                                                    var pagina = document.getElementById("__pagina").value;
                                                    if (pagina !== "")
                                                        window.location.href = pagina;
                                                });
                                            }
                                        }

                                        function MostrarMensajeError() {
                                            MostrarMensaje('error');
                                        }

                                        function MostrarMensajeExito() {
                                            MostrarMensaje('success');
                                        }
                                    </script>
                                    <!-- FIN DE MOSTRAR MENSAJE CUANDO HAY UPDATEPANEL -->

                                    <div class="form-group row">
                                        <div class="col-sm-12 text-center">
                                            <div class="btn-group">
                                                <asp:LinkButton ID="btnRegistrar" runat="server" class="btn btn-primary"
                                                    OnClientClick="return Confirmar('¿Desea registrar asistencia?');"
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
                                        <asp:HiddenField ID="hfClaveAutorizacion" runat="server" Visible="false" />
                                    </div>

                                    <!-- Modal -->
                                    <div class="modal fade" id="myModalClave" tabindex="-1" role="dialog" aria-labelledby="myModalClaveLabel" aria-hidden="true" data-keyboard="false" data-backdrop="static">
                                        <div class="modal-dialog modal-dialog-centered">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <h5 class="modal-title" id="myModalClaveLabel">INGRESAR A PAGINA WEB DE ASISTENCIA</h5>
                                                </div>
                                                <div class="modal-body">
                                                    <asp:UpdatePanel ID="UpdatePanelModal" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div class="container-fluid">
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <div class="form-group">
                                                                            <label for="TxtClave"><b>Clave:</b></label>
                                                                            <asp:RequiredFieldValidator ID="rfvTxtClave" runat="server" ControlToValidate="TxtClave"
                                                                                ErrorMessage="*" ValidationGroup="ValidarViatico" BackColor="Yellow" ForeColor="Red" SetFocusOnError="True"
                                                                                Display="Dynamic"></asp:RequiredFieldValidator>
                                                                            <asp:TextBox ID="TxtClave" runat="server" CssClass="form-control"
                                                                                Autocomplete="off" TextMode="Password"
                                                                                MaxLength="20" onkeyup="convertirAMayusculas(this)"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="modal-footer">
                                                    <asp:LinkButton ID="BtnClave" runat="server" ValidationGroup="ValidarViatico" OnClick="BtnClave_Click"
                                                        CssClass="btn btn-primary" OnClientClick="return Confirmar('¿Desea generar acceso?');">
                    Aceptar <span class="glyphicon glyphicon-ok"></span>
                                                    </asp:LinkButton>
                                                    <button type="button" class="btn btn-danger" data-dismiss="modal" onclick="redirectOnErrorPage()">
                                                        Cancelar <span class="glyphicon glyphicon-remove"></span>
                                                    </button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- FIN DE VENTANA MODAL -->

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="Hf_Ip" runat="server" />
        <asp:HiddenField ID="hfCiudad" runat="server" />
        <asp:HiddenField ID="hfNavegador" runat="server" />
        <asp:HiddenField ID="hfNavegadorVersion" runat="server" />

        <script>
            // Asignar información del navegador
            $('#<%=hfNavegador.ClientID%>').val(bowser.name);
            $('#<%=hfNavegadorVersion.ClientID%>').val(bowser.version);
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
