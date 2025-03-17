using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Email.Commands
{
    public interface IEmailSender
    {
        public interface IEmailSender
        {
            Task SendEmailAsync(string email, string subject, string message);
        }

        public class EmailSender : IEmailSender
        {
            public Task SendEmailAsync(string email, string subject, string message)
            {
                // In real application, use an email service to send an email.
                Console.WriteLine($"Email to: {email}, Subject: {subject}, Message: {message}");
                return Task.CompletedTask;
            }
        }
    }
}

