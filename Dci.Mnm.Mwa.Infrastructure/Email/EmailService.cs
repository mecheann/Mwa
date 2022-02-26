using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dci.Mnm.Mwa.Core;
using Dci.Mnm.Mwa.Domain;
using Dci.Mnm.Mwa.Infrastructure.Core.Email;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;

namespace Dci.Mnm.Mwa.Infrastructure.Email
{
    public class EmailService : IEmailService
    {

        private readonly AppConfig appConfig;
        private readonly EmailConfig emailConfig;
        private readonly ILogger<EmailService> logger;

        public EmailService(AppConfig appConfig, ILogger<EmailService> logger)
        {
            this.appConfig = appConfig;
            this.emailConfig = appConfig.Email;
            this.logger = logger;
        }

        public async Task SendEmail(EmailMessage emailMessage)
        {
            logger.
                LogInformation("Sending mail for @{mail_sender}, to @{mail_recipients} subject: {mail_subject}",
                emailMessage.FromAddresses, emailMessage.ToAddresses, emailMessage.Subject);

            var message = new MimeMessage();
            message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

            message.Subject = emailMessage.Subject;

            //We will say we are sending HTML. But there are options for plaintext etc. 
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = emailMessage.Content
            };

            if (emailMessage.EmailAttachments.Any())
            {
                logger.LogInformation("has attachements: {mail_attachement_count}", emailMessage.EmailAttachments.Count());
                AddAttachments(emailMessage, message);
            }

            await SendMailViaMailKit(message);
        }

        private async Task SendMailViaMailKit(MimeMessage message)
        {
            //Be careful that the SmtpClient class is the one from Mailkit not the framework!
            using (var emailClient = new SmtpClient())
            {
                if (!emailConfig.CheckServerCertificate)
                {
                    emailClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
                }

                await emailClient.ConnectAsync(emailConfig.SmtpServer, emailConfig.SmtpPort, emailConfig.CheckServerCertificate);
                if (!String.IsNullOrEmpty(emailConfig.SmtpUsername))
                {
                    await emailClient.AuthenticateAsync(emailConfig.SmtpUsername, emailConfig.SmtpPassword);
                }

                emailClient.MessageSent += (sender, messageArgs) =>
                {
                    logger.LogInformation("Response from mail client: {response}", messageArgs.Response);
                };

                await emailClient.SendAsync(message);
                await emailClient.DisconnectAsync(true);
            }
        }

        private static void AddAttachments(EmailMessage emailMessage, MimeMessage message)
        {
            //get list of email attachments (as MimePart)
            var attachments = new List<MimePart>();
            foreach (var emailAttachment in emailMessage.EmailAttachments)
            {
                // Create attachments
                var attachment = new MimePart()
                {
                    Content = new MimeContent(emailAttachment.Content, ContentEncoding.Default),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Default,
                    FileName = emailAttachment.FileName
                };
                attachments.Add(attachment);
            }

            // changing body type to mulitpart
            var multipart = new Multipart("mixed"); // TODO: check if mixed is need
            var newBody = message.Body;

            multipart.Add(newBody);

            foreach (var item in attachments)
            {
                multipart.Add(item);
            }
            message.Body = multipart;
        }

        public EmailMessage SetUpEmail(string name, string address, string compiledTemplate, string subject)
        {
            logger.LogInformation("setting up email.");
            var toAddress = new EmailAddress { Name = name, Address = address };
            var fromAddress = new EmailAddress { Name = appConfig.Email.SenderEmailName, Address = appConfig.Email.SenderEmailAddress };
            var emailMessage = new EmailMessage { Content = "", Subject = subject };
            emailMessage.ToAddresses.Add(toAddress);
            emailMessage.FromAddresses.Add(fromAddress);
            emailMessage.Content = compiledTemplate;
            logger.LogInformation("completed email set up.");

            return emailMessage;
        }
    }
}









