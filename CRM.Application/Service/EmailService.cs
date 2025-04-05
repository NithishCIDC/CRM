using CRM.Application.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Connections;
using MimeKit;
using MimeKit.Text;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client;
using CRM.Application.DTO;


namespace CRM.Application.Service
{
    public class EmailService : IEmailService
    {
        public void Email(string email, string subject, string bodyContent)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var notification = new NotificationRequest
            {
                To = email,
                Subject = subject,
                Message = bodyContent,
                Type = "email"
            };

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(notification));
            channel.BasicPublish(exchange: "", routingKey: "notification-queue", basicProperties: null, body: body);
            Console.WriteLine("Message published.");

            //var message = new MimeMessage();
            //message.From.Add(new MailboxAddress("CRM", "nithish1206official@gmail.com"));
            //message.To.Add(new MailboxAddress("", email));
            //message.Subject = subject;

            //message.Body = new TextPart(TextFormat.Plain) { Text = bodyContent };

            //using var client = new SmtpClient();
            //client.Connect("smtp.gmail.com", 587, false);
            //client.Authenticate("Theboyscidc@gmail.com", "qordywyabamdbwav");
            //client.Send(message);
            //client.Disconnect(true);
        }
    }
}
