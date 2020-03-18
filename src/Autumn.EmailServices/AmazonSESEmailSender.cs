using Abp.Runtime.Security;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Autumn.AwsEmail;
using Autumn.Configuration;
using EASendMail;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Autumn.EmailServices
{
    public class AmazonSESEmailSender : IAwsEmailSender
    { 
        public async Task AmazonEmailSendAsync(string userToaddress, string subject, string body)
        {
            try
            {
                var builder = new BodyBuilder();
                string EmailConfirmationCode = RandomString(10, false);
                builder.HtmlBody = body;
                var oMessage = new MimeMessage();

                oMessage.From.Add(new MailboxAddress("Autumn", ""));
                oMessage.To.Add(new MailboxAddress(userToaddress));
                oMessage.Subject = subject;
                oMessage.Body = builder.ToMessageBody();

                var stream = new MemoryStream();
                oMessage.WriteTo(stream);
                var request = new SendRawEmailRequest
                {
                    RawMessage = new RawMessage { Data = stream },
                    Source = ""
                };

                using (var client = new AmazonSimpleEmailServiceClient())
                {
                    var response = await client.SendRawEmailAsync(request);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task OutlookEmailSendAsync(string userToaddress, string subject, string body, IConfigurationRoot configuration)
        {
            try
            {
                SmtpMail oMail = new SmtpMail("TryIt");
                SmtpClient oSmtp = new SmtpClient();

                // Your Hotmail email address
                oMail.From = configuration["OutlookMail:UserID"];

                // Set recipient email address
                oMail.To = userToaddress;
                //oMail.Mailer = "info@routereporter.com";

                // Set email subject
                oMail.Subject = subject;

                // Set email body

                //oMail.HtmlBody = @_emailTemplate.GetDefaultTemplate(null).Replace("{EMAIL_TITLE}", sendemail.EmailTitle)
                //    .Replace("{EMAIL_SUB_TITLE}", sendemail.EmailSubTitle)
                //    .Replace("{EMAIL_BODY}", sendemail.EmailBody);
                oMail.HtmlBody = body;

                // Hotmail SMTP server address
                SmtpServer oServer = new SmtpServer("smtp.office365.com");

                // Hotmail user authentication should use your
                // email address as the user name.
                oServer.User = configuration["OutlookMail:UserID"];
                oServer.Password = configuration["OutlookMail:Password"];

                // Set 587 port, if you want to use 25 port, please change 587 to 25
                oServer.Port = Convert.ToInt32(configuration["OutlookMail:Port"]);
                // detect SSL/TLS connection automatically
                oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;

                oSmtp.SendMail(oServer, oMail);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string EncryptQueryParameters(string link, string encrptedParameterName = "c")
        {
            if (!link.Contains("?"))
            {
                return link;
            }

            var uri = new Uri(link);
            var basePath = link.Substring(0, link.IndexOf('?'));
            var query = uri.Query.TrimStart('?');

            return basePath + "?" + encrptedParameterName + "=" + HttpUtility.UrlEncode(SimpleStringCipher.Instance.Encrypt(query));
        }

        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
    }
}
