﻿using CRM.Application.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;


namespace CRM.Application.Service
{
    public class EmailService : IEmailService
    {
        public void Email(string email, string subject, string bodyContent)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("CRM", "nithish1206official@gmail.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = subject;

            message.Body = new TextPart(TextFormat.Plain) { Text = bodyContent };

            using var client = new SmtpClient();
            client.Connect("smtp.gmail.com", 587, false);
            client.Authenticate("Theboyscidc@gmail.com", "qordywyabamdbwav");
            client.Send(message);
            client.Disconnect(true);
        }
    }
}
