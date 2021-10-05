using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace WebBalanceTracker
{
    public class EmailHelper
    {
        private static string MailFrom
        {
            get
            {
                return ConfigurationManager.AppSettings["mailFrom"];
            }
        }

        private static string MailFromPass
        {
            get
            {
                var base64EncodedBytes = System.Convert.FromBase64String(ConfigurationManager.AppSettings["mailFromPassword"]);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
        }

        private static string MailTo
        {
            get
            {
                return ConfigurationManager.AppSettings["mailTo"];
            }
        }

        private static string SmtpServer
        {
            get
            {
                return ConfigurationManager.AppSettings["SmtpServer"];
            }
        }

        public static bool SendMail(string subject, string body)
        {
            try
            {
                using (SmtpClient smtpClient = new SmtpClient())
                using (MailMessage message = new MailMessage())
                {
#if  DEBUG
#else
                    MailAddress fromAddress = new MailAddress(MailFrom);

                    // setup up the host, increase the timeout to 5 minutes
                    smtpClient.Host = SmtpServer;
                    smtpClient.Port = 587;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential(MailFrom,MailFromPass);
                    
                    smtpClient.Timeout = 300000;

                    message.From = fromAddress;
                    message.Subject = subject;
                    message.IsBodyHtml = true;
                    message.Body = body.Replace("\r\n", "<br>");
                    string[] recipientList = MailTo.Split(';');
                    foreach (string rec in recipientList)
                    {
                        if (rec.Length > 0)
                        {
                            message.To.Add(rec);
                        }
                    }
                    smtpClient.Send(message);

# endif

                    return true;
                }
            }
            catch (Exception e)
            {
                var m = e.Message;
            }

            return false;
        }
    }

}