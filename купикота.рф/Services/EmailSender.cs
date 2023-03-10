using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Net;

namespace купикота.рф.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {

            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient()
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential("kupi.cota.company@gmail.com", "fr141195_")
            };
           
            
            var msg = new MailMessage {From = new MailAddress("kupi.cota.company@gmail.com", "Востановление пароля") };
            msg.To.Add(new MailAddress(email));
            msg.Subject = subject;
            msg.Body = message;
            msg.IsBodyHtml = true;


            client.Send(msg);
           
        }
    }
}

