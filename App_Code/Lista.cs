using System;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
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

    //// Clave de 256 bits (32 bytes)
    //private static readonly byte[] key = Encoding.UTF8.GetBytes("thisisaverysecurekey123456789012"); // 32 caracteres


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

  
    public string Encrypt(string clearText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }

    public string Decrypt(string cipherText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        cipherText = cipherText.Replace(" ", "+");
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }


}