using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

/// <summary>
/// Descripción breve de Lista
/// </summary>
public class Lista
{
	public Lista()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}
    public void Search_DropDownList(DataTable Tabla, DropDownList control, String strcampo)
    {
        if (Tabla.Rows.Count == 0) return;
        for (int i = 0; i < control.Items.Count; i++)
        {
            if (control.Items[i].Text.Trim().Equals(Tabla.Rows[0][strcampo].ToString()))
            {
                control.SelectedIndex = i;
            }
        }
    }

    public void ShowMessage(HiddenField __mensaje, HiddenField __pagina, string msg, string paginaweb)
    {
        __mensaje.Value = msg;
        __pagina.Value = paginaweb;
    }

    public string LimpiarDireccionIP(string valor)
    {
        // Utilizar una expresión regular para extraer la dirección IP
        // Considerando que la dirección IP contiene solo números y puntos.
        string patron = @"[\d.]+";
        Regex regex = new Regex(patron);
        Match match = regex.Match(valor);

        // Devolver la parte de la dirección IP si se encuentra
        return match.Success ? match.Value : string.Empty;
    }
}