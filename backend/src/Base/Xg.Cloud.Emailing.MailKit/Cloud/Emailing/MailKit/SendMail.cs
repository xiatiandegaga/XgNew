using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace Cloud.Emailing.MailKit
{
    public class SendMail : ISendMail
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SendMail> _logger;
        public SendMail(IConfiguration configuration, ILogger<SendMail> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendAsync(string to, string subject, string body, bool isBodyHtml = false)
        {
            try
            {
                MimeMessage message = new MimeMessage();
                //发件人
                message.From.Add(new MailboxAddress(_configuration["Mail:fromAddress"], _configuration["Mail:fromAddress"]));
                var listAddress = new InternetAddressList();
                //收件人
                string toAdressArray;
                if (to == default)
                    toAdressArray = _configuration["Mail:toAddress"];
                else
                    toAdressArray = to;
                foreach (var address in toAdressArray.Split(','))
                {
                    listAddress.Add(new MailboxAddress(address, address));
                }
                message.To.AddRange(listAddress);
                //标题
                message.Subject = subject;
                var bodyBuilder = new BodyBuilder();
                if (isBodyHtml)
                    bodyBuilder.HtmlBody = body;
                else
                    bodyBuilder.TextBody = body;
                message.Body = bodyBuilder.ToMessageBody();
                using var client = new SmtpClient
                {
                    ServerCertificateValidationCallback = (s, c, h, e) => true,
                    CheckCertificateRevocation = false
                };
                await client.ConnectAsync(_configuration["Mail:host"], Convert.ToInt32(_configuration["Mail:port"]), SecureSocketOptions.Auto);
                await client.AuthenticateAsync(_configuration["Mail:userName"], _configuration["Mail:fromToken"]);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendMail");
            }
        }

        public async Task SendWithFromAsync(string from, string to, string subject, string body, bool isBodyHtml = false)
        {
            try
            {
                MimeMessage message = new MimeMessage();
                //发件人
                message.From.Add(new MailboxAddress(from, from));
                var listAddress = new InternetAddressList();
                //收件人
                string toAdressArray;
                if (to == default)
                    toAdressArray = _configuration["Mail:toAddress"];
                else
                    toAdressArray = to;
                foreach (var address in toAdressArray.Split(','))
                {
                    listAddress.Add(new MailboxAddress(address, address));
                }
                message.To.AddRange(listAddress);
                //标题
                message.Subject = subject;
                var bodyBuilder = new BodyBuilder();
                if (isBodyHtml)
                    bodyBuilder.HtmlBody = body;
                else
                    bodyBuilder.TextBody = body;
                message.Body = bodyBuilder.ToMessageBody();
                using var client = new SmtpClient
                {
                    ServerCertificateValidationCallback = (s, c, h, e) => true,
                    CheckCertificateRevocation = false
                };
                await client.ConnectAsync(_configuration["Mail:host"], Convert.ToInt32(_configuration["Mail:port"]), SecureSocketOptions.Auto);
                await client.AuthenticateAsync(_configuration["Mail:userName"], _configuration["Mail:fromToken"]);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendMail");
            }
        }
    }
}
