using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace KindleMail
{
    public class SendMain
    {
        private readonly EmailConfiguration _emailConfiguration;

        public SendMain(EmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        public void Send(EmailMessage emailMessage, params string[] attachments)
        {
            var message = new MimeMessage();
            message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            message.Subject = emailMessage.Subject;

            var builder = new BodyBuilder();
            // Set the plain-text version of the message text
            builder.TextBody = @"Hey Alice,
                What are you up to this weekend? Monica is throwing one of her parties on
                Saturday and I was hoping you could make it.
                Will you be my +1?
                -- Joey
                ";

            // see:
            // https://github.com/jstedfast/MailKit/blob/master/FAQ.md#CreateAttachments
            foreach (string path in attachments)
            {
                if (File.Exists(path))
                    builder.Attachments.Add(path);
            }

            // Now we just need to set the message body and we're done
            message.Body = builder.ToMessageBody();

            //Be careful that the SmtpClient class is the one from Mailkit not the framework!
            using (var emailClient = new SmtpClient())
            {
                //The last parameter here is to use SSL (Which you should!)
                emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, true);

                //Remove any OAuth functionality as we won't be using it. 
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

                emailClient.Send(message);

                emailClient.Disconnect(true);
            }
        }
    }
}
