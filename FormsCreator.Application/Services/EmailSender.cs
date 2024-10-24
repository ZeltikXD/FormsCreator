using FormsCreator.Application.Options;
using FormsCreator.Core.Interfaces.Services;
using FormsCreator.Core.Shared;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace FormsCreator.Application.Services
{
    internal sealed class EmailSender(IOptions<EmailSenderOptions> opts) : IEmailSender
    {
        private readonly EmailSenderOptions _options = opts.Value;

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            await SendEmailAsync([to], subject, body, false);
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml)
        {
            await SendEmailAsync([to], subject, body, isHtml);
        }

        public async Task SendEmailAsync(string[] to, string subject, string body)
        {
            await SendEmailAsync(to, subject, body, false);
        }

        public async Task SendEmailAsync(string[] to, string subject, string body, bool isHtml)
        {
            using var client = PrepareClient();

            using var mailMessage = PrepareMessage(subject, body, isHtml, out _);

            foreach (var recipient in to)
            {
                mailMessage.To.Add(new MailboxAddress(recipient, recipient));
            }

            await client.SendAsync(mailMessage);
            await client.DisconnectAsync(true);
        }

        public async Task SendEmailWithAttachmentsAsync(string to, string subject, string body, IEnumerable<EmailAttachment> attachments)
        {
            await SendEmailWithAttachmentsAsync([to], subject, body, attachments);
        }

        public async Task SendEmailWithAttachmentsAsync(string[] to, string subject, string body, IEnumerable<EmailAttachment> attachments)
        {
            using var client = PrepareClient();

            using var mailMessage = PrepareMessage(subject, body, true, out var multipart);

            foreach (var recipient in to)
            {
                mailMessage.To.Add(new MailboxAddress(recipient, recipient));
            }

            foreach (var attachment in attachments)
            {
                var stream = new MemoryStream(attachment.Content)
                {
                    Position = 0
                };
                var attchPart = new MimePart(attachment.ContentType)
                {
                    Content = new MimeContent(stream),
                    ContentId = attachment.ContentId ?? attachment.FileName,
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = attachment.FileName,
                };
                multipart.Add(attchPart);
            }

            try
            {
                await client.SendAsync(mailMessage);
            }
            finally
            {
                foreach (var attachment in mailMessage.Attachments)
                {
                    attachment.Dispose();
                }
            }
        }

        private SmtpClient PrepareClient()
        {
            var client = new SmtpClient();
            if (_options.UseSSL)
                client.Connect(_options.SmtpServer, _options.SmtpPort, _options.UseSSL);
            else
                client.Connect(_options.SmtpServer, _options.SmtpPort, MailKit.Security.SecureSocketOptions.None);
            client.Authenticate(_options.SmtpUser, _options.SmtpPass);
            return client;
        }

        private MimeMessage PrepareMessage(string subject, string body, bool isHtml, out Multipart resBody)
        {
            var msg = new MimeMessage
            {
                Subject = subject,
                Priority = MessagePriority.Normal
            };

            msg.From.Add(new MailboxAddress(_options.FromName, _options.FromEmail));
            resBody = new Multipart("mixed")
            {
                new TextPart(isHtml ? TextFormat.Html : TextFormat.Text)
                {
                    Text = body,
                    ContentTransferEncoding = ContentEncoding.Base64
                }
            };
            msg.Body = resBody;
            return msg;
        }
    }
}
