using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;

namespace Pharmacie2.utils
{
    public class GMailer
    {
        public static string GmailUsername { get; set; }
        public static string GmailPassword { get; set; }
        public static string GmailHost { get; set; }
        public static int GmailPort { get; set; }
        public static bool GmailSSL { get; set; }

        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }

        static GMailer()
        {
            GmailHost = "smtp.gmail.com";
            GmailPort = 587;
            GmailSSL = true;
        }
        public void Send()
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = GmailHost;
            smtp.Port = GmailPort;
            smtp.EnableSsl = GmailSSL;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(GmailUsername, GmailPassword);

            try
            {
                using (var message = new MailMessage(GmailUsername, ToEmail))
                {
                    message.Subject = Subject;
                    message.Body = Body;
                    message.IsBodyHtml = IsHtml;
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                ///to do: stockage dans une table pour un envoi antérieur
            }
        }
        public static void senMail(string destinataire, string subjet, string body)
        {
            try
            {
                string email = ConfigurationManager.AppSettings["Email"];
                string emailPassword = ConfigurationManager.AppSettings["EmailPassword"];

                // Ajout des identifiants manquants
                GMailer.GmailUsername = email;
                GMailer.GmailPassword = emailPassword;

                GMailer mailer = new GMailer
                {
                    ToEmail = destinataire,
                    Subject = subjet,
                    Body = body,
                    IsHtml = true
                };

                mailer.Send();
            }
            catch (Exception ex)
            {
               
                Console.WriteLine("Erreur lors de l'envoi de l'email : " + ex.Message);
            }
        }

    }
}
