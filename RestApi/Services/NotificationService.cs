using System;
using System.Text;
using System.Threading.Tasks;
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
        private readonly IUserRepository _userRepository;

        public NotificationService(IConfiguration config, IUserRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }
        
        public async Task<bool> SendRequestNotification(int senderId, int receiverId)
        {
            var sender = await _userRepository.GetUserById(senderId);
            var receiver = await _userRepository.GetUserById(receiverId);

            if (sender == null && receiver == null || receiver == null || sender == null)
                throw new NullReferenceException();
            
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(sender.UserName, _config["Mail:Email"]));
            message.To.Add(new MailboxAddress(receiver.UserName, receiver.EmailAddress));
            message.Subject = $"{sender.UserName} heeft voor uw interesse gevraagd";

            message.Body = new TextPart("plain")
            {
                Text = @$"Geachte {receiver.UserName},

{sender.UserName} wilt dat je interesse toont in de openstaande aanvragen

-- Send using MailKit via Outlook SMTP"
                
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_config["Mail:Server"], 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_config["Mail:Email"], _config["Mail:Password"]);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            return true;
        }
    }
}