using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Extensions.Configuration;

namespace GameOfThronePool.Services
{
    public class AuthMessageSenderOptions
    {
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
    }

    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private IConfiguration _Configuration;
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor, IConfiguration iconfiguration)
        {
            Options = optionsAccessor.Value;
            _Configuration = iconfiguration;
        }
        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager
        
        public string SendGridKey => _Configuration.GetValue<string>("SendGridKey");

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(SendGridKey, subject, message, email); 
            //return Execute(Options.SendGridKey, subject, message, email);
        }

        public string CheckKey()
        {
            return SendGridKey;
        }
        public async Task<Response> EasySendEmailWait(string email, string subject, string message)
        {
            return await EasySendEmail(SendGridKey, subject, message, email);
        }
        static async Task<Response> EasySendEmail(string apiKey, string subject, string message, string email)
        {            
            //var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("Stephen@foxdeploy.com", "Webmaster-GameOfThrones Deathpool");
            //var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress(email, "Game of Thrones DeathPool Contestant");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "Lord ruler,<br><strong>We've received the following message for you.</strong><hr>" + message + "<br> Thank you for your time,<br><br><a href='stephen@foxdeploy.com'>For support, click here to contact Stephen</a> ";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            return response;
        
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("Stephen Owen", "Stephen@FoxDeploy.com"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }

}
