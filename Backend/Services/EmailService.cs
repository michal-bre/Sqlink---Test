using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var email = _config["EmailSettings:Email"];
                var password = _config["EmailSettings:Password"];
                var host = _config["EmailSettings:Host"];
                var port = int.Parse(_config["EmailSettings:Port"] ?? "587");

                using (var client = new SmtpClient(host, port))
                {
                    client.Credentials = new NetworkCredential(email, password);
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(email!),
                        Subject = subject,
                        Body = body, // כאן נכנס ה-HTML עם הלינק
                        IsBodyHtml = true // מוודא שהמייל יפרש תגיות HTML
                    };
                    mailMessage.To.Add(toEmail);

                    await client.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                // הדפסת השגיאה לחלונית ה-Output במידה והשליחה נכשלה
                System.Diagnostics.Debug.WriteLine($"EMAIL ERROR: {ex.Message}");
                // ניתן גם לזרוק את השגיאה הלאה או לטפל בה בשקט
            }
        }
    }
}