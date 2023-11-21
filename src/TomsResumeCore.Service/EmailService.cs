using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TomsResumeCore.DomainModels;

namespace TomsResumeCore.Service
{
    public class EmailService : IEmailService
    {        
        private readonly IGoogleRecaptchaService _recaptchaService;
        private readonly IConfiguration _config;
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IGoogleRecaptchaService recaptchaService, IConfiguration config)
        {
            _recaptchaService = recaptchaService;
            _config = config;
            _smtpSettings = new SmtpSettings() {
                Host = _config["smtp:Host"],
                Port = Convert.ToInt32(_config["smtp:Port"]),
                IsSSL = Convert.ToBoolean(_config["smtp:IsSSL"]),
                SenderEmail = _config["smtp:SenderEmail"],
                SenderName = _config["smtp:SenderName"],
                Password = _config["GmailPassword"]
            };
        }

        public async Task SendContactMessage(string name, string email, string message, string recaptcha)
        {
            var RecaptchaValid = await _recaptchaService.IsReCaptchaPassed(recaptcha);

            if (!RecaptchaValid)
                return;

            if (!IsValidEmail(email))
                return;

            if (String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(message))
                return;

            var ContactFormEmail = _config["ContactFormEmail"];

            if (ContactFormEmail == null)
                throw new ArgumentNullException("The ContactFormEmail param is missing from appsettings.json");

            var to = new List<string>();
            to.Add(ContactFormEmail);
            
            await SendEmail("Contact Form Email", string.Concat("Message from '" + name + "(" + email + ")'  : ", message), to, null, null);
        }

        public async Task SendEmail(string subject, string message, List<string> to, List<string> cc, List<string> bcc)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
            email.To.AddRange(to.Select(x => new MailboxAddress("",x)));

            if (cc != null)
            {
                email.Cc.AddRange(cc.Select(x => new MailboxAddress("", x)));
            }

            if (bcc != null)
            {
                email.Bcc.AddRange(bcc.Select(x => new MailboxAddress("", x)));
            }

            email.Subject = subject;

            email.Body = new TextPart("plain")
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                if (!_smtpSettings.IsSSL)
                    client.Connect(_smtpSettings.Host, _smtpSettings.Port, false);
                else
                    client.Connect(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.Auto);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(_smtpSettings.SenderEmail, _smtpSettings.Password);

                await client.SendAsync(email);
                client.Disconnect(true);
            }
        }

        //Code found at https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
