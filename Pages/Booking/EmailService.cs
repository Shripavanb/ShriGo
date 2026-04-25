using MailKit.Net.Smtp;
using MimeKit;
using System.Net.Mail;

namespace ShriGo.Pages.Booking
{
    public class EmailService
    {
        private IConfiguration _config;
        public EmailService(IConfiguration config)=> _config= config;

        public async Task sendEmailAsync(string toEmail, string subject, string body)
        {

            try { 
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_config["MailSettings:SenderName"], _config["MailSettings:SenderEmail"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart("html") { Text = body };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(_config["MailSettings:Server"], int.Parse(_config["MailSettings:Port"]));
            await smtp.AuthenticateAsync(_config["MailSettings:SenderEmail"], _config["MailSettings:Password"]);
            await smtp.SendAsync(email);
            Console.WriteLine("Email sent successfully.");
            await smtp.DisconnectAsync(true);
          
            }
            catch (MailKit.Security.AuthenticationException authEx)
            {
                Console.WriteLine($"Authentication Error:{authEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error:{ex.Message}");
                throw;
            }

        }

    }
}
