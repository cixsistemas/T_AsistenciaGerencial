using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;

public partial class _Asistencia : System.Web.UI.Page
{
	private string Ruta = ConfigurationManager.AppSettings.Get("CadenaConeccion");
	policia.clsaccesodatos servidor = new policia.clsaccesodatos();
	TableRow tRow;
	Lista _Lista = new Lista();
	private void ShowMessage(string msg, string paginaweb)
	{
		__mensaje.Value = msg;
		__pagina.Value = paginaweb;
	}
	private void Listar(string criterio, string desde, string Opcion)
	{
		__mensaje.Value = "";
		__pagina.Value = "";

		for (int i = 1; i < TableTipoproduccion.Rows.Count; i++)
		{
			TableTipoproduccion.Rows[i].Cells.Clear();
		}

		try
		{
			policia.clsaccesodatos servidor = new policia.clsaccesodatos();
			servidor.cadenaconexion = Ruta;
			if (servidor.abrirconexion() == true)
			{
				DataTable dt = servidor.consultar("[dbo].[_pa_Listar_Asistencia]", criterio, desde, Opcion).Tables[0];
				if (dt.Rows.Count == 0)
				{
					servidor.cerrarconexion();
					__mensaje.Value = "No hay Datos para mostrar.";
					__pagina.Value = "";
				}
				else
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						tRow = new TableRow();
						for (int j = 0; j < 7; j++)//Cabecera de la tabla
						{
							TableCell tCell = new TableCell();
							tCell.BorderColor = System.Drawing.Color.Black;
							switch (j)
							{
								case 0:
									tCell.Text = dt.Rows[i]["FECHA_HORA"].ToString();
									tCell.HorizontalAlign = HorizontalAlign.Left;
									tCell.Visible = false;
									tRow.Cells.Add(tCell);
									break;

								case 1:
									tCell.Text = dt.Rows[i]["NOMBRE"].ToString();
									tCell.HorizontalAlign = HorizontalAlign.Left;
									tCell.Visible = false;
									tRow.Cells.Add(tCell);
									break;

								case 2:
									tCell.Text = dt.Rows[i]["NRO DOCUMENTO"].ToString();
									tCell.HorizontalAlign = HorizontalAlign.Left;
									tCell.Visible = false;
									tRow.Cells.Add(tCell);
									break;

								case 3:
									tCell.Text = dt.Rows[i]["VALIDO"].ToString();
									tCell.HorizontalAlign = HorizontalAlign.Left;
									tCell.Visible = false;
									tRow.Cells.Add(tCell);
									break;

								case 4:
									tCell.Text = dt.Rows[i]["DNI_NOMBRE"].ToString();
									tCell.HorizontalAlign = HorizontalAlign.Left;
									tCell.Visible = false;
									tRow.Cells.Add(tCell);
									break;

								case 5:
									tCell.Text = dt.Rows[i]["ENTRADA_SALIDA"].ToString();
									tCell.HorizontalAlign = HorizontalAlign.Left;
									tCell.Visible = false;
									tRow.Cells.Add(tCell);
									break;

								case 6:
									Button b = new Button();
									b.Text = "EDITAR";
									b.ToolTip = "Seleccione registro para modificarlo o eliminarlo.";
									b.BorderStyle = BorderStyle.None;
									b.CausesValidation = false;
									b.UseSubmitBehavior = true;
									b.PostBackUrl = "_Asistencia.aspx?Codigo=" + dt.Rows[i]["ID"].ToString();
									b.Click += new EventHandler(visualiza_tipo_produccion);
									tCell.Visible = false;
									tCell.HorizontalAlign = HorizontalAlign.Center;
									tCell.Controls.Add(b);
									tRow.Cells.Add(tCell);
									break;

							}
						}

						TableTipoproduccion.Rows.Add(tRow);
					}
					servidor.cerrarconexion();
				}
			}
			else
			{
				servidor.cerrarconexion();
				__mensaje.Value = servidor.getMensageError();
				__pagina.Value = "CerrarSession.aspx";
			}
		}
		catch (Exception)
		{
			//throw;
			__mensaje.Value = "Error inesperado al intentar conectarnos con el servidor.";
			__pagina.Value = "CerrarSession.aspx";
		}
	}
	protected void visualiza_tipo_produccion(object sender, EventArgs e)
	{
		int cod_fundo = Convert.ToInt32(Request.QueryString.Get("Codigo").Trim());
		Id_.Value = cod_fundo.ToString();
		Consultar_(cod_fundo);
		btnRegistrar.Visible = false;
		oculta(true);
	}

	private void oculta(bool ok)
	{
		btnModificar.Visible = ok;
		btnEliminar.Visible = ok;
		btnCancelar.Visible = ok;
	}
	private void Matenimiento_(int Id_Asistencia, DateTime fecha, string valido, string Ent_Sal, string Desde,
    string Observacion, string Host, DateTime FechaModi, int Id_Trabajador, int Id_Usuario
        , int Id_Asist_Entrada, int Id_Autoriza, string operacion)
	{
		try
		{
			policia.clsaccesodatos servidor = new policia.clsaccesodatos();
			servidor.cadenaconexion = Ruta;
			if (servidor.abrirconexiontrans() == true)
			{
				servidor.ejecutar("[dbo].[_pa_mantenimiento_Asistencia]",
									false,
									Id_Asistencia,
									fecha,
									valido,
									Ent_Sal,
									Desde,
                                    Observacion,
                                    Host,
									FechaModi,
									Id_Trabajador,
									Id_Usuario,
									Id_Asist_Entrada,
									Id_Autoriza,
									operacion,
									0, "");
				if (servidor.getRespuesta() == 1)
				{
					servidor.cerrarconexiontrans();
                    btnRegistrar.Visible = false;
                    __mensaje.Value = servidor.getMensaje();
					__pagina.Value = "_Asistencia.aspx";
				}
				else
				{
					servidor.cancelarconexiontrans();
					__mensaje.Value = servidor.getMensaje();
					__pagina.Value = "";
					//this.__pagina.Value = "_Asistencia.aspx";
				}
			}
			else
			{
				servidor.cancelarconexiontrans();
				__mensaje.Value = servidor.getMensageError();
				__pagina.Value = "CerrarSession.aspx";
			}

		}
		catch (Exception)
		{
			__mensaje.Value = "Verifique ";
			__pagina.Value = "";
		}
	}

	private void Consultar_(int idTipo_Caja)
	{
		try
		{
			policia.clsaccesodatos servidor = new policia.clsaccesodatos();
			servidor.cadenaconexion = Ruta;
			if (servidor.abrirconexiontrans() == true)
			{
				DataTable dt = servidor.consultar("[dbo].[_Pr_Consultar_Asistencia]",
									idTipo_Caja).Tables[0];
				if (dt.Rows.Count == 0)
				{
					servidor.cerrarconexion();
					__mensaje.Value = "Tipo de Produccion no tiene datos.";
					__pagina.Value = "CerrarSession.aspx";
				}
				else
				{
					Id_.Value = dt.Rows[0]["ID"].ToString();
					_Lista.Search_DropDownList(dt, DdlNroDni, "DNI_NOMBRE");
					//this.nombretipoproduccion.Text = dt.Rows[0]["TIPO DE PRODUCCION"].ToString().Trim();
				}
			}
			else
			{
				servidor.cerrarconexion();
				__mensaje.Value = servidor.getMensageError();
				__pagina.Value = "CerrarSession.aspx";
			}
		}
		catch (Exception)
		{
			__mensaje.Value = "Error inesperado al intentar conectarnos con el servidor.";
			__pagina.Value = "CerrarSession.aspx";
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{

		//Label1.Text = Request.ServerVariables.Item("REMOTE_ADDR");
		//Label2.Text = Request.ServerVariables.Item("REMOTE_HOST");
		GetClientHostIP();
		//System.Net.Dns.BeginGetHostByName(Request.UserHostAddress, new AsyncCallback(GetHostNameCallBack), Request.UserHostAddress);
	}


	public string GetClientHostIP()
	{
		string clientIP = string.Empty;
		string hostName = string.Empty;
		try
		{
			clientIP = Request.UserHostAddress;
			hostName = System.Net.Dns.GetHostEntry(Request.UserHostAddress).HostName;
		}
		catch (Exception)
		{
			//Ip solo para que no de error sino esta registrada la pc en el DNS
			clientIP = "0.0.0.0";
			hostName = "PCError";
		}
		return TxtHostName.Text = hostName;// +" - " + clientIP;
	}
	protected void Page_init(object sender, EventArgs e)
	{
		__mensaje.Value = "";
		__pagina.Value = "";
		//this.nombretipoproduccion.Focus();
		Listar("", "", "1");
		Obtener_Trabajador("18");
		DdlNroDni.Focus();
		FechaActual.Text = DateTime.Now.ToLongDateString();
	}


	protected void btnRegistrar_Click(object sender, EventArgs e)
	{
		Boolean ok;
		ok = rvDdlNroDni.IsValid;
		ok = ok && rfvDdlNroDni.IsValid;
		ok = ok && RfvrbPreference.IsValid;

		if (ok == false)
		{
			return;
		}

		DateTime Hoy = DateTime.Today;
		string fecha_actual = Hoy.ToString("dd-MM-yyyy");
		//TxtFechaActual.Text = fecha_actual;
		Matenimiento_(Convert.ToInt32(Id_.Value.Trim()),
		Convert.ToDateTime(fecha_actual),
		"SI",
		//".",
		rbPreference.SelectedValue,
		"WEB",
        "",
		TxtHostName.Text.Trim(), //HOSTMODIFICACION
		Convert.ToDateTime(fecha_actual),//FECHA_MODIFICACION
		Convert.ToInt32(DdlNroDni.SelectedValue),
		10,//ID_USUARIO DE ASISTENCIA
		0,//ID_ASISTENCIA_ENTRADA
		318,//TRABAJADOR_AUTORIZA
		"N");

		//this.nombretipoproduccion.Focus();
	}
	protected void btnModificar_Click(object sender, EventArgs e)
	{
		Boolean ok;

		ok = rvDdlNroDni.IsValid;
		ok = ok && rfvDdlNroDni.IsValid;

		if (ok == false)
		{
			return;
		}

		DateTime Hoy = DateTime.Today;
		string fecha_actual = Hoy.ToString("dd-MM-yyyy");
		//TxtFechaActual.Text = fecha_actual;
		Matenimiento_(Convert.ToInt32(Id_.Value.Trim()),
		Convert.ToDateTime(fecha_actual),
		"SI",
		rbPreference.SelectedValue,
		"WEB",
        "",
		TxtHostName.Text.Trim(), //HOSTMODIFICACION
		Convert.ToDateTime(fecha_actual),//FECHA_MODIFICACION
		Convert.ToInt32(DdlNroDni.SelectedValue),
		0,//ID_ASISTENCIA_ENTRADA
		318,//TRABAJADOR_AUTORIZA
		0,
		"M");

		btnRegistrar.Visible = true;

		oculta(false);
	}
	protected void btnEliminar_Click(object sender, EventArgs e)
	{
		Boolean ok;

		ok = rvDdlNroDni.IsValid;
		ok = ok && rfvDdlNroDni.IsValid;

		if (ok == false)
		{
			return;
		}

		//Matenimiento_Tipo_produccion(Convert.ToInt32(Id_.Value.Trim()),
		//  "",
		//  Convert.ToInt32(this.DdlNroDni.SelectedValue),
		//    "E");

		btnRegistrar.Visible = true;

		oculta(false);
	}
	protected void btnCancelar_Click(object sender, EventArgs e)
	{

		btnRegistrar.Visible = true;
		oculta(false);
	}

	private void Obtener_Trabajador(string Opcion)
	{
		try
		{
			servidor.cadenaconexion = Ruta;
			if (servidor.abrirconexion() == true)
			{
				System.Data.DataTable dt = servidor.consultar("[dbo].[_pr_Obtener_Varios]", Opcion).Tables[0];
				if (dt.Rows.Count == 0)
				{
					servidor.cerrarconexion();
					_Lista.ShowMessage(__mensaje, __pagina, "Error, al intentar recuperar datos.", "CerrarSession.aspx");
				}
				else
				{
					DdlNroDni.DataSource = dt;
					DdlNroDni.DataTextField = "NOMBRE";
					DdlNroDni.DataValueField = "CODIGO";
					DdlNroDni.DataBind();
					servidor.cerrarconexion();
				}
			}
			else
			{
				servidor.cerrarconexion();
				_Lista.ShowMessage(__mensaje, __pagina, servidor.getMensageError(), "CerrarSession.aspx");
			}
		}
		catch (Exception)
		{
			servidor.cerrarconexion();
			_Lista.ShowMessage(__mensaje, __pagina, "Error inesperado al intentar conectarnos con el servidor.", "CerrarSession.aspx");
		}
	}
	protected void Buscar_Click(object sender, EventArgs e)
	{

	}
	protected void rbPreference_SelectedIndexChanged(object sender, EventArgs e)
	{

	}
}