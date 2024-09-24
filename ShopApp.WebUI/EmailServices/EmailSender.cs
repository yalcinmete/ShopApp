using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ShopApp.WebUI.EmailServices
{
    public class EmailSender : IEmailSender
    {
        //Apı key
        private const string SendGridKey = " ";
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            
            var apiKey = SendGridKey;
            var client = new SendGridClient(SendGridKey);
            var from = new EmailAddress("yalcinmete.g4@gmail.com", "Shop App");
            var subject2 = subject;
            var to = new EmailAddress("yalcinmete.g4@gmail.com", "Yalcin Mete");
            var plainTextContent = "SendGrid ile test maili gönderildi.";
            var htmlContent = htmlMessage;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);

            //return Task.FromResult(response);
        }
    }
}
