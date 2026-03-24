using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using System.Net.Mail;
using System.Net;
using System.IO;
using System.Configuration;
using System.Diagnostics;

namespace CapaNegocio
{
    public class CN_Recursos
    {
        public static string GenerarClave() { 
            string clave = Guid.NewGuid().ToString("N").Substring(0,6);
            return clave;
        }
        public static string ConvertirSha256(string texto)
        {
            // Crear objeto SHA256
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convertir texto a bytes
                byte[] bytes = Encoding.UTF8.GetBytes(texto);

                // Calcular hash
                byte[] hash = sha256.ComputeHash(bytes);

                // Convertir hash a string hexadecimal
                StringBuilder resultado = new StringBuilder();
                foreach (byte b in hash)
                {
                    resultado.Append(b.ToString("x2")); // formato hexadecimal
                }

                return resultado.ToString();
            }
        }

        public static bool EnviarCorreo(string correo,string asunto,string mensaje) {
            bool resultado = false;

            try
            {
                string smtpHost = ConfigurationManager.AppSettings["SmtpHost"] ?? "smtp.gmail.com";
                int smtpPort = int.TryParse(ConfigurationManager.AppSettings["SmtpPort"], out var p) ? p : 587;
                bool enableSsl = bool.TryParse(ConfigurationManager.AppSettings["SmtpEnableSsl"], out var s) ? s : true;
                string smtpUser = ConfigurationManager.AppSettings["SmtpUser"];
                string smtpPass = ConfigurationManager.AppSettings["SmtpPass"];
                string fromAddress = ConfigurationManager.AppSettings["SmtpFrom"] ?? smtpUser;
                string fromDisplay = ConfigurationManager.AppSettings["SmtpFromDisplay"] ?? "Tienda Online";

                if (string.IsNullOrWhiteSpace(smtpUser) || string.IsNullOrWhiteSpace(smtpPass) || string.IsNullOrWhiteSpace(fromAddress))
                {
                    Trace.WriteLine("EnviarCorreo: configuración SMTP incompleta. Configure SmtpUser/SmtpPass/SmtpFrom en Web.config.");
                    return false;
                }

                using (var mail = new MailMessage())
                {
                    mail.To.Add(correo);
                    mail.From = new MailAddress(fromAddress, fromDisplay);
                    mail.Subject = asunto;
                    mail.Body = mensaje;
                    mail.IsBodyHtml = true;

                    using (var smtp = new SmtpClient(smtpHost, smtpPort))
                    {
                        smtp.Credentials = new NetworkCredential(smtpUser, smtpPass);
                        smtp.EnableSsl = enableSsl;
                        smtp.UseDefaultCredentials = false;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.Timeout = 20000;

                        smtp.Send(mail);
                        resultado = true;
                    }
                }
            }
            catch (Exception ex) {
                Trace.WriteLine("Error EnviarCorreo: " + ex.ToString());
                resultado = false;
            }
            return (resultado);
        }
    }
}
