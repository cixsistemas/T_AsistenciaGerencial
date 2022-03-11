using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;


/// <summary>
/// Descripción breve de CsConexion
/// </summary>
public class CsConexion
{
    private SqlConnection cn;
    private SqlTransaction tsql;
    private bool transaccion;
    private string MensageError;
    public int MensajeNroError;
    private int Respuesta;
    private string Mensaje;
    public string cadenaconexion;

    public CsConexion()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
        this.cn = new SqlConnection();
    }

    public int getRespuesta()
    {
        return this.Respuesta;
    }

    public string getMensaje()
    {
        return this.Mensaje;
    }

    public string getMensageError()
    {
        switch (this.MensajeNroError)
        {
            case 53:
                this.MensageError = "No se encontro el servidor o esta inaccesible";
                break;
            case 2812:
                this.MensageError = "Procedimiento almacenado no existe";
                break;
            case 4060:
                this.MensageError = "No se puede abrir la base de datos solicitada para el inicio de sesión";
                break;
        }
        return this.MensageError;
    }

    public bool abrirconexion()
    {
        bool flag = true;
        try
        {
            this.cn.ConnectionString = this.cadenaconexion;
            this.cn.Open();
            this.transaccion = false;
        }
        catch (SqlException ex)
        {
            //ProjectData.SetProjectError((Exception)ex);
            SqlException sqlException = ex;
            flag = false;
            this.MensajeNroError = sqlException.Number;
            //ProjectData.ClearProjectError();
        }
        catch (Exception ex)
        {
            //ProjectData.SetProjectError(ex);
            Exception exception = ex;
            flag = false;
            this.MensageError = exception.Message;
            //ProjectData.ClearProjectError();
        }
        return flag;
    }

    public void cerrarconexion()
    {
        this.transaccion = false;
        this.cn.Close();
        this.cn.Dispose();
    }

    public bool abrirconexiontrans()
    {
        bool flag = true;
        try
        {
            this.cn.ConnectionString = this.cadenaconexion;
            this.cn.Open();
            this.tsql = this.cn.BeginTransaction();
            this.transaccion = true;
        }
        catch (SqlException ex)
        {
            //ProjectData.SetProjectError((Exception)ex);
            SqlException sqlException = ex;
            flag = false;
            this.MensajeNroError = sqlException.Number;
            //ProjectData.ClearProjectError();
        }
        catch (Exception ex)
        {
            //ProjectData.SetProjectError(ex);
            Exception exception = ex;
            flag = false;
            this.MensageError = exception.Message;
            //ProjectData.ClearProjectError();
        }
        return flag;
    }

    public void cancelarconexiontrans()
    {
        this.transaccion = false;
        if (this.transaccion)
            this.tsql.Rollback();
        if (this.cn.State != ConnectionState.Open)
            return;
        this.cn.Close();
    }

    public void cerrarconexiontrans()
    {
        try
        {
            this.transaccion = false;
            this.tsql.Commit();
            this.cn.Close();
        }
        catch (Exception)
        {
            throw;
            //ProjectData.SetProjectError(ex);
            //ProjectData.ClearProjectError();
        }
    }

    public DataSet consultar(string procedimiento, params object[] x)
    {
        SqlCommand command = new SqlCommand();
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
        DataSet dataSet = new DataSet();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = Convert.ToString(procedimiento).Trim();
        command.Connection = this.cn;
        if (this.transaccion)
            command.Transaction = this.tsql;
        SqlCommandBuilder.DeriveParameters(command);
        //*** AGREGADO EL DIA 25-03-2021
        command.CommandTimeout = 300;
        //=====================================
        int index = 0;
        IEnumerator enumerator;
        try
        {
            enumerator = command.Parameters.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SqlParameter current = (SqlParameter)enumerator.Current;
                //if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString
                //    (current.ParameterName, "@RETURN_VALUE", false) != 0)
                if ((current.ParameterName != "@RETURN_VALUE"))
                {
                    current.Value = RuntimeHelpers.GetObjectValue(x[index]);
                    checked { ++index; }
                }
            }
        }
        finally
        {
            //if (enumerator is IDisposable)
            //    (enumerator as IDisposable).Dispose();
        }
        sqlDataAdapter.SelectCommand = command;
        sqlDataAdapter.Fill(dataSet, "Consulta");
        return dataSet;
    }

    public DataSet consultarx(string procedimiento, params object[] x)
    {
        DataSet dataSet1;
        try
        {
            SqlCommand command = new SqlCommand();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataSet dataSet2 = new DataSet();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = Convert.ToString(procedimiento).Trim();
            command.Connection = this.cn;
            if (this.transaccion)
                command.Transaction = this.tsql;
            SqlCommandBuilder.DeriveParameters(command);
            int index = 0;
            IEnumerator enumerator;
            try
            {
                enumerator = command.Parameters.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    SqlParameter current = (SqlParameter)enumerator.Current;
                    if ((current.ParameterName != "@RETURN_VALUE"))
                    //if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString
                    //    (current.ParameterName, "@RETURN_VALUE", false) != 0)
                    {
                        current.Value = RuntimeHelpers.GetObjectValue(x[index]);
                        checked { ++index; }
                    }
                }
            }
            finally
            {
                //if (enumerator is IDisposable)
                //    (enumerator as IDisposable).Dispose();
            }
            sqlDataAdapter.SelectCommand = command;
            sqlDataAdapter.Fill(dataSet2, "Consulta");
            dataSet1 = dataSet2;
        }
        catch (Exception)
        {
            //ProjectData.SetProjectError(ex);
            throw;
        }
        return dataSet1;
    }

    public object ejecutar(string procedimiento, bool devuelve_valor, params object[] x)
    {
        IEnumerator enumerator1;
        IEnumerator enumerator2;
        SqlCommand command = new SqlCommand();
        DataSet dataSet = new DataSet();
        int index = 0;
        command.CommandType = CommandType.StoredProcedure;
        command.Connection = this.cn;
        command.CommandText = procedimiento;
        if (this.transaccion)
            command.Transaction = this.tsql;
        SqlParameter sqlParameter1 = new SqlParameter();
        SqlParameter sqlParameter2 = new SqlParameter();
        SqlCommandBuilder.DeriveParameters(command);


        enumerator1 = command.Parameters.GetEnumerator();
        try
        {

            while (enumerator1.MoveNext())
            {
                SqlParameter current = (SqlParameter)enumerator1.Current;
                //if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString
                //    (current.ParameterName, "@RETURN_VALUE", false) != 0)
                if ((current.ParameterName != "@RETURN_VALUE"))
                {
                    current.Value = RuntimeHelpers.GetObjectValue(x[index]);
                    checked { ++index; }
                }
            }
        }
        finally
        {
            if (enumerator1 is IDisposable)
                (enumerator1 as IDisposable).Dispose();
        }
        command.ExecuteNonQuery();

        //=======================================================================

        enumerator2 = command.Parameters.GetEnumerator();
        try
        {

            while (enumerator2.MoveNext())
            {
                SqlParameter current = (SqlParameter)enumerator2.Current;
                string parameterName = current.ParameterName;
                if (parameterName == "@pRpta")
                {
                    Respuesta = (int)Convert.ToInt64(current.Value);
                }
                if (parameterName == "@pMensaje")
                {
                    Mensaje = Convert.ToString(current.Value);
                }
                //if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(parameterName, "@pRpta", false) == 0)
                //    this.Respuesta = Conversions.ToInteger(current.Value);
                //else if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(parameterName, "@pMensaje", false) == 0)
                //    this.Mensaje = Conversions.ToString(current.Value);
            }
        }
        finally
        {
            if (enumerator2 is IDisposable)
                (enumerator2 as IDisposable).Dispose();
        }


        object objectValue = new object();
        if (devuelve_valor)
            objectValue = RuntimeHelpers.GetObjectValue(command.Parameters[checked(command.Parameters.Count - 1)].Value);
        return objectValue;
    }


    //public object ejecutar2(string procedimiento, bool devuelve_valor, params object[]x {
    //    SqlParameter current;
    //    IEnumerator enumerator;
    //    IEnumerator enumerator2;
    //    SqlCommand command = new SqlCommand();
    //    DataSet DataSet1 = new DataSet();
    //    int index = 0;
    //    command.CommandType = CommandType.StoredProcedure;
    //    command.Connection = this.cn;
    //    command.CommandText = procedimiento;
    //    if (this.transaccion)
    //    {
    //        command.Transaction = this.tsql;
    //    }
    //    SqlParameter parameter2 = new SqlParameter();
    //    SqlParameter parameter = new SqlParameter();
    //    SqlCommandBuilder.DeriveParameters(command);
    //    try
    //    {
    //        enumerator = command.Parameters.GetEnumerator();
    //        while (true)
    //        {
    //            if (!enumerator.MoveNext)
    //            {
    //                break;
    //            }
    //            current = ((SqlParameter)enumerator.Current);
    //            if ((current.ParameterName != "@RETURN_VALUE"))
    //            {
    //                current.Value = x(index);
    //                index += 1;
    //            }
    //        }
    //    }
    //    finally
    //    {
    //        if (!Object.ReferenceEquals(((IDisposable)enumerator), null))
    //        {
    //            ((IDisposable)enumerator).Dispose();
    //        }
    //    }
    //    command.ExecuteNonQuery();
    //    try
    //    {
    //        enumerator2 = command.Parameters.GetEnumerator();
    //        while (true)
    //        {
    //            if (!enumerator2.MoveNext)
    //            {
    //                break;
    //            }
    //            current = ((SqlParameter)enumerator2.Current);
    //            string parameterName = current.ParameterName;
    //            if ((parameterName == "@pRpta"))
    //            {
    //                this.Respuesta = Conversions.ToInteger(current.Value);
    //                continue do
    //                {
    //                }
    //            if ((parameterName == "@pMensaje"))
    //                {
    //                    this.Mensaje = Conversions.ToString(current.Value);
    //                }
    //            }
    //        }finally
    //    {
    //        //If Not Object.ReferenceEquals(TryCast(enumerator2, IDisposable), Nothing) Then
    //        //    TryCast(enumerator2, IDisposable).Dispose()
    //        //End If
    //    }
    //    if (devuelve_valor)
    //    {
    //    }
    //    return command.Parameters((command.Parameters.Count - 1)).Value;
    //}
}
