using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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

        public static void SendMail(string subject, string body)
        {
            try
            {
                using (SmtpClient smtpClient = new SmtpClient())
                using (MailMessage message = new MailMessage())
                {
                    //NetworkCredential basicCredential = new NetworkCredential(Username, Password, domain);
                    MailAddress fromAddress = new MailAddress(MailFrom);

                    // setup up the host, increase the timeout to 5 minutes
                    smtpClient.Host = SmtpServer;
                    smtpClient.UseDefaultCredentials = true;
                    //smtpClient.Credentials = basicCredential;
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
                }
            }
            catch (Exception e)
            {

            }
        }
    }

}