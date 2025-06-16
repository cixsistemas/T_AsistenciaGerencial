/**
 * Función que obtiene información de la terminal del cliente desde un servicio local
 * que corre en http://localhost:9100/. El valor de la terminal se asigna a un campo
 * oculto en la página (hfTerminal). Si no se puede obtener, se asigna "NO".
 */
function obtenerTerminalCliente(idCampoDestino) {
    // Crea una nueva instancia del objeto XMLHttpRequest para hacer la llamada AJAX
    var xhr = new XMLHttpRequest();

    // Configura la solicitud GET hacia el servicio local en el puerto 9100
    xhr.open("GET", "http://localhost:9100/", true); // true = asincrónica

    // Define la función que se ejecutará cada vez que cambie el estado de la solicitud
    xhr.onreadystatechange = function () {
        // Si la solicitud ha finalizado y fue exitosa (HTTP 200)
        if (xhr.readyState === 4 && xhr.status === 200) {
            try {
                if (!xhr.responseText || !xhr.responseText.trim().startsWith("[")) {
                    throw new Error("Respuesta inválida o vacía.");
                }

                // Parsea la respuesta JSON recibida
                var data = JSON.parse(xhr.responseText);

                // Valor por defecto si no se encuentra la terminal o viene vacía
                var terminal = "NO";

                // Itera sobre los elementos del JSON buscando el campo "terminal"
                for (var i = 0; i < data.length; i++) {
                    if (data[i].hasOwnProperty("terminal")) {
                        terminal = data[i].terminal;

                        // Validación adicional: si el valor está vacío o solo contiene espacios, usar "NO"
                        if (!terminal || terminal.trim() === "") {
                            terminal = "NO";
                        }

                        // Se encontró la terminal, se puede salir del bucle
                        break;
                    }
                }

                // Asigna el valor de la terminal al campo oculto del formulario (ASP.NET)
                document.getElementById(idCampoDestino).value = terminal;

                // Imprime en consola para fines de depuración
                console.log("Terminal detectado:", terminal);

            } catch (e) {
                // En caso de error al parsear la respuesta, asigna "NO" al campo
                document.getElementById(idCampoDestino).value = "NO";

                // Imprime el error en consola para depuración
                console.error("Error al detectar terminal:", e);
            }
        }
    };

    // Envía la solicitud al servidor local
    xhr.send();
}
