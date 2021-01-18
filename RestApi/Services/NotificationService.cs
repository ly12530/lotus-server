using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;
using Core.DomainServices;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace RestApi.Services
{
    public class NotificationService
    {
        private readonly IConfiguration _config;

        public NotificationService(IConfiguration config)
        {
            _config = config;
        }
        
        public async Task<bool> SendShowInterestsNotification(User sender, IEnumerable<User> receivers, string title, string body)
        {
            if (sender == null && receivers == null || receivers == null || sender == null)
                throw new NullReferenceException();

            foreach (var receiver in receivers)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(sender.UserName, _config["Mail:Email"]));
                message.To.Add(new MailboxAddress(receiver.UserName, receiver.EmailAddress));
                message.Subject = title;

                message.Body = new TextPart("plain")
                {
                    Text = $@"{body}"

                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_config["Mail:Server"], Int32.Parse(_config["Mail:Port"]), SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_config["Mail:Email"], _config["Mail:Password"]);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }    
            }
            
            return true;
        }

        public async Task<bool> SendNewRequestNotification(Request openedRequest, List<User> bettingCoordinators)
        {
            if (openedRequest == null && bettingCoordinators == null || openedRequest == null ||
                bettingCoordinators == null) throw new NullReferenceException();

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Lotus Notifier", _config["Mail:Email"]));
            foreach (var bettingCoordinator in bettingCoordinators)
            {
                message.To.Add(new MailboxAddress(bettingCoordinator.UserName, bettingCoordinator.EmailAddress));
            }

            message.Subject = $"{openedRequest.Customer.Name} heeft een nieuwe aanvraag toegevoegd";
            message.Body = new TextPart("plain")
            {
                Text = $@"Geachte inzetcoördinator,

{openedRequest.Customer.Name} heeft een nieuwe aanvraag {openedRequest.Title} aangemaakt.

-- Dit is een automatische notificatie"
                
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_config["Mail:Server"], Int32.Parse(_config["Mail:Port"]), SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_config["Mail:Email"], _config["Mail:Password"]);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            
            return true;
        }
    }
}