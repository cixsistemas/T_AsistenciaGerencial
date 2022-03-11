var nav4 = window.Event ? true : false;
function SoloNumeros(evt) {
    // NOTE: Backspace = 8, Enter = 13, '0' = 48, '9' = 57
    var key = nav4 ? evt.which : evt.keyCode;
    return (key <= 13 || (key >= 48 && key <= 57));
}

//ACEPTA SOLO NUMEROS (MENOS Ñ Y TILDES)
//FUNCIONA EN FIREFOX Y CHROME
//AGREGADO EL 31-05-2018
function SoloNumeros_(e) {
    key = e.keyCode || e.which;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = " 1234567890";
    especiales = [8, 9, 37, 39, 46];
    tecla_especial = false
    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_especial = true;
            break;
        }
    }
    if (letras.indexOf(tecla) == -1 && !tecla_especial) {
        return false;
    }
}

//ACEPTA SOLO LETRAS (MENOS Ñ Y TILDES)
//FUNCIONA EN FIREFOX Y CHROME
//AGREGADO EL 31-05-2018
function soloLetras3(e) {
    key = e.keyCode || e.which;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = " abcdefghijklmnñopqrstuvwxyz()";
    especiales = [8, 9, 37, 39, 46];

    tecla_especial = false
    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial)
        return false;
}

//function soloLetras3(e) {
//    key = e.keyCode || e.which;
//    tecla = String.fromCharCode(key).toLowerCase();
//    letras = " abcdefghijklmnopqrstuvwxyz";
//    especiales = "8-37-38-39-46";
//    tecla_especial = false
//    for (var i in especiales) {
//        if (key == especiales[i]) {
//            tecla_especial = true;
//            break;
//        }
//    }
//    if (letras.indexOf(tecla) == -1 && !tecla_especial) {
//        return false;
//    }
//}

//function SoloLetras_1(e) {
//    if (!(event.keyCode >= 65 && event.keyCode <= 90 || event.keyCode == 32 || event.keyCode == 193 || event.keyCode == 201 || event.keyCode == 205 || event.keyCode == 209 || event.keyCode == 211 || event.keyCode == 218 || event.keyCode == 220)) {
//        event.keyCode = 0;
//    }
//}

//ACEPTA SOLO LETRAS Y NUMEROS (MENOS Ñ Y TILDES)
//FUNCIONA EN FIREFOX Y CHROME
//AGREGADO EL 31-05-2018
function soloLetras2(e) {
    key = e.keyCode || e.which;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = " abcdefghijklmnopqrstuvwxyz1234567890.,:-()+-/¿?@";
    especiales = [8, 9, 37, 39, 46];
    tecla_especial = false
    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_especial = true;
            break;
        }
    }
    if (letras.indexOf(tecla) == -1 && !tecla_especial) {
        return false;
    }
}

//Solo numeros con 1 punto y maximo dos decimales
function filterFloat(evt, input) {
    // Backspace = 8, Enter = 13, ‘0′ = 48, ‘9′ = 57, ‘.’ = 46, ‘-’ = 43
    var key = window.Event ? evt.which : evt.keyCode;
    var chark = String.fromCharCode(key);
    var tempValue = input.value + chark;
    if (key >= 48 && key <= 57) {
        if (filter(tempValue) === false) {
            return false;
        } else {
            return true;
        }
    } else {
        if (key == 8 || key == 13 || key == 0) {
            return true;
        } else if (key == 46) {
            if (filter(tempValue) === false) {
                return false;
            } else {
                return true;
            }
        } else {
            return false;
        }
    }
}
function filter(__val__) {
    var preg = /^[0-9]+([.])?([0-9]+)?$/;
    //var preg = /^([0-9]+\.?[0-9]{0,2})$/;
    if (preg.test(__val__) === true) {
        return true;
    } else {
        return false;
    }

}

//var nav4 = window.Event ? true : false;
//function SoloNumeros(evt) {
//    // NOTE: Backspace = 8, Enter = 13, '0' = 48, '9' = 57
//    var key = nav4 ? evt.which : evt.keyCode;
//    return (key <= 13 || (key >= 48 && key <= 57 || key == 46));
//}