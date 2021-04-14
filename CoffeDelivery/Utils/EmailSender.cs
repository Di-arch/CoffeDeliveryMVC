using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CoffeDelivery.Utils
{
    public class EmailSender:IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string host;
        private int port;
        private bool enableSSL;
        private string senderEmail;
        private string password;

        public Task SendEmailAsync(string email, string subject, string message)
        {
            host = _configuration["SmtpClient:Host"];
            port = int.Parse(_configuration["SmtpClient:Port"]);
            enableSSL = bool.Parse(_configuration["SmtpClient:EnableSsl"]);
            senderEmail = _configuration["SmtpClient:Sender"];
            password = _configuration["SmtpClient:Password"];

            var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(senderEmail, password),
                EnableSsl = enableSSL
            };
            return client.SendMailAsync(
                new MailMessage(senderEmail, email, subject, message) { IsBodyHtml = true }
            );
        }



    }
}
