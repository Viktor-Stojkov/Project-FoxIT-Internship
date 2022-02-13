using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Postal;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WorkOrders.Shared
{
    public class MessageModel : Postal.Email
    {
        protected MessageModel() : base()
        {

        }

        public MessageModel(string viewName)
            : base(viewName)
        {

        }

        public MessageModel(string viewName, string areaName)
            : base(viewName, areaName, new EmptyModelMetadataProvider())
        {

        }

        public string To
        {
            get
            {
                return (string)ViewData["to"];
            }
            set
            {
                ViewData["to"] = value;
            }
        }
        public string Cc
        {
            get
            {
                return (string)ViewData["cc"];
            }
            set
            {
                ViewData["cc"] = value;
            }
        }
        public string Bcc
        {
            get
            {
                return (string)ViewData["bcc"];
            }
            set
            {
                ViewData["bcc"] = value;
            }
        }
        public string Subject
        {
            get
            {
                return (string)ViewData["Subject"];
            }
            set
            {
                ViewData["Subject"] = value;
            }
        }
        public string DisplayName
        {
            get
            {
                return (string)ViewData["DisplayName"];
            }
            set
            {
                ViewData["DisplayName"] = value;
            }
        }
    }

    public class EmailSenderOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; } 
        public string FromAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public interface IEmailSenderEnhance : IEmailSender
    {
        Task SendEmailAsync(MailMessage mailMessage);
        Task SendEmailAsync(Postal.Email emailData);
    }

    

    public class EmailSender : IEmailSenderEnhance
    {
        // Our private configuration variables
        private readonly EmailSenderOptions _emailOptions;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _env;

        // Get our parameterized configuration
        public EmailSender(IEmailService emailService,
            IEmailViewRender emailViewRenderer,
            IWebHostEnvironment env,
            IOptions<EmailSenderOptions> emailOptions)
        {
            _emailOptions = emailOptions.Value;
            _emailService = emailService;
            _env = env;
        }

        private SmtpClient CreateSmtpClient()
        {
            var client = new SmtpClient(_emailOptions.Host, _emailOptions.Port)
            {
                EnableSsl = _emailOptions.EnableSSL,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailOptions.Username, _emailOptions.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            return client;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using (var client = CreateSmtpClient())
            {
                await client.SendMailAsync(
                    new MailMessage(_emailOptions.FromAddress, email, subject, htmlMessage) { IsBodyHtml = true }
                );
            }
        }

        public async Task SendEmailAsync(MailMessage mailMessage)
        {
            mailMessage.From = new MailAddress(_emailOptions.FromAddress, "Test Foxit");
            using (var client = CreateSmtpClient())
            {
                await client.SendMailAsync(mailMessage);
            }
        }

        public async Task SendEmailAsync(Postal.Email emailData)
        {
            MailMessage message = await _emailService.CreateMailMessageAsync(emailData);
            message.From = new MailAddress(_emailOptions.FromAddress);
            await SendEmailAsync(message);
        }
    }
}
