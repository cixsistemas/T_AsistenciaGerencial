using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

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
    public void Search_DropDownList(DataTable Tabla, System.Web.UI.WebControls.DropDownList control, String strcampo)
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

    public void ShowMessage(System.Web.UI.WebControls.HiddenField __mensaje, System.Web.UI.WebControls.HiddenField __pagina, string msg, string paginaweb)
    {
        __mensaje.Value = msg;
        __pagina.Value = paginaweb;
    }
}