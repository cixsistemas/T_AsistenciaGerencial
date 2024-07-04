<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ErrorPage.aspx.cs" Inherits="ErrorPage" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="icon" type="image/ico" href="Imagenes/accesoDenegado.ico" />
    <title>Acceso Denegado</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            margin: 0;
        }

        .error-container {
            background-color: #fff;
            padding: 30px;
            border: 1px solid #ccc;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            text-align: center;
        }

            .error-container h1 {
                font-size: 24px;
                margin-bottom: 20px;
                color: red; /* Cambiar color del texto a rojo */
            }

            .error-container p {
                font-size: 16px;
                color: #333;
            }
    </style>
</head>
<body>
    <div class="error-container">
        <h1>Acceso Denegado</h1>
        <p>No tiene acceso a esta página web.</p>
        <p>Por favor, consulte con el administrador del sistema.</p>
    </div>
</body>
</html>
