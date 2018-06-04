﻿using System;
using System.Net;
using System.Net.Mail;

namespace Model
{
    public class TextEmailAlertService : IEmailAlertService
    {
        private EmailAlert _config = null;

        public void SetSettings(Alert alert)
        {
            _config = alert.email;
        }

        public void SendAlert(string header, string text)
        {
            SendEmail(header, text);
        }

        public void SendAlertRecover(string header, string text)
        {
            SendEmail(header, text);
        }

        private void SendEmail(string subject, string body)
        {
            if (_config == null) throw new NullReferenceException(
                "EmailAlertService.SendEmail: Settings was not set. Cannot send letter." +
                "\n\nSubject:\n" + subject + "\n\nBody:\n" + body);
            try
            {
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(_config.emailFrom),
                    Subject = subject,
                    Body = body,
                };
                mail.To.Add(new MailAddress(_config.emailTo));
                //if (!string.IsNullOrEmpty(attachFile)) mail.Attachments.Add(new Attachment(attachFile));
                SmtpClient client = new SmtpClient()
                {
                    Host = _config.serverUrl,
                    Port = _config.serverPort,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_config.authUser, _config.authPassword),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                };
                client.Send(mail);
                mail.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception("EmailAlertService.SendEmail:\n" + e.GetType() + "\n" + e.Message +
                    "\n\nSubject:\n" + subject + "\n\nBody:\n" + body);
            }
        }


    }
}
