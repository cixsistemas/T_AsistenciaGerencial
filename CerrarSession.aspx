<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CerrarSession.aspx.cs" Inherits="CerrarSession" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
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

     function SoloNumeros() {
         if ((event.keyCode < 48) || (event.keyCode > 57)) {
             event.returnValue = false;
         }

     }

     function SoloLetras(e) {
         if (!(event.keyCode >= 65 && event.keyCode <= 90 || event.keyCode == 32 || event.keyCode == 193 || event.keyCode == 201 || event.keyCode == 205 || event.keyCode == 209 || event.keyCode == 211 || event.keyCode == 218 || event.keyCode == 220)) {
             event.keyCode = 0;
         }
     }

     function LetrasNumeros(e) {
         if (!(event.keyCode >= 48 && event.keyCode <= 57 || event.keyCode >= 65 && event.keyCode <= 90 || event.keyCode == 32 || event.keyCode == 193 || event.keyCode == 201 || event.keyCode == 205 || event.keyCode == 209 || event.keyCode == 211 || event.keyCode == 218 || event.keyCode == 220)) {
             event.keyCode = 0;
         }
     }

     /*
     Función que permite al usuario ingrese sólo números enteros o reales.
     Parametros: e    , permite obtener el valor de la tecla prsionada.
     Punto, false sólo números enteros;true sólo números reales.
     Texto, texto que contiene el valor numerico. 
     */
     function SoloDecimales(e, Punto, Texto) {
         if (event.keyCode == 8)
             ;
         else if (event.keyCode == 46) {
             if (Punto == false)
                 event.keyCode = 0;
             else {
                 for (i = 0; i < Texto.value.length; i++) {
                     if (Texto.value.charCodeAt(i) == 46) {
                         event.keyCode = 0;
                     }
                 }
             }
         }
         else if (!(event.keyCode >= 48 && event.keyCode <= 57)) {
             event.keyCode = 0;
         }
     }

     function LimpiaImagen() {


     }


     function CambiaLetraMayuscula(Caja) {
         document.getElementById(Caja).value = document.getElementById(Caja).value.toUpperCase();
     }
    
    </script>
 
    
</head>
<body onload ="MostrarMensaje()" 
    >
    <form id="form1" runat="server">
                    <asp:HiddenField ID="__mensaje" runat="server" />
                    <asp:HiddenField ID="__pagina" runat="server" />
                    </form>
    </body>
</html>

