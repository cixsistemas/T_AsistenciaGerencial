<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Prueba.aspx.cs" Inherits="Prueba" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>CSS3 Digital Clock with jQuery</title>



    <link rel="stylesheet" type="text/css" href="Reloj/clock.css" />
    <script type="text/javascript" src="Reloj/jquery-1.6.4.min.js"></script>
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
    <%--<link rel="canonical" href="http://www.alessioatzeni.com/wp-content/tutorials/jquery/CSS3-digital-clock/index.html" />--%>
</head>
<body>
    <%--<h1>CSS3 Digital Clock with jQuery<small>Tutorial by Alessio Atzeni | <a href="http://www.alessioatzeni.com/blog/css3-digital-clock-with-jquery/">View Tutorial</a></small></h1>--%>
    <div class="container">
        <div class="clock">
            <div id="Date"></div>

            <ul>
                <li id="hours"></li>
                <li id="point">:</li>
                <li id="min"></li>
                <li id="point">:</li>
                <li id="sec"></li>
            </ul>

        </div>
    </div>


</body>
</html>

<%--<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>--%>
