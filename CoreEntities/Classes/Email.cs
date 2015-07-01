using CoreEntities.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreEntities.Classes
{
    public class Email
    {

        public static void SendEmail(string from, string[] to, string[] CC, string[] BCC, string subject, string body,
                                        Dictionary<string, AddAttachment> attachments = null)
        {
            Thread email = new Thread(delegate()
            {
                //if (config == null)
                    SendAsyncEmail(from, to, CC, BCC, subject, body, attachments/*, config*/);
                //else
                //    SendEmailViaAgent(from, to, CC, BCC, subject, body, attachments, config);
            });
            email.IsBackground = true;
            email.Start();
        }

        private static void SendAsyncEmail(string from, string[] to, string[] CC, string[] BCC, string subject,
            string body, Dictionary<string, AddAttachment> attachments)
        {
            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                //if (config != null)
                //{
                //    client.Credentials = new System.Net.NetworkCredential(config.EmailID + "@gmail.com", config.Password);
                //}
                //// Read smtp from config table
                //using (var context = new ADVANTAFLdbmlDataContext(AdvantaConfig.GetADVConnectionString()))
                //{
                //    Mail objMail = MailConfiguration.getEmailConfig();
                //    client.Host = objMail.MAILSERVER;
                //    client.Port = Convert.ToInt32(objMail.MAILPORT);
                //    client.EnableSsl = false;
                //    client.Credentials = new NetworkCredential(objMail.MAILUSER, objMail.MAILPASS);
                //}
                message.From = new System.Net.Mail.MailAddress(from);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                foreach (string t in to)
                {
                    if (!message.To.Contains(new System.Net.Mail.MailAddress(t)))
                        message.To.Add(new System.Net.Mail.MailAddress(t));
                }

                if (CC != null)
                    foreach (string c in CC)
                    {
                        if (!message.CC.Contains(new System.Net.Mail.MailAddress(c)))
                            message.CC.Add(new System.Net.Mail.MailAddress(c));
                    }

                if (BCC != null)
                    foreach (string b in BCC)
                    {
                        if (!message.Bcc.Contains(new System.Net.Mail.MailAddress(b)))
                            message.Bcc.Add(new System.Net.Mail.MailAddress(b));
                    }

                if (attachments != null && attachments.Count() > 0)
                {
                    foreach (var item in attachments)
                    {
                        var stream = new MemoryStream(item.Value.Stream);
                        var attachment = new System.Net.Mail.Attachment(stream, item.Key, item.Value.MediaType);
                        message.Attachments.Add(attachment);
                    }
                }
                client.Send(message);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
