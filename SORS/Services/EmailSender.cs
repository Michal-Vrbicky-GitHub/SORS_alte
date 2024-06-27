using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;

namespace SORS.Services
{
    public class EmailSender : IEmailSender
    {
        ILogger<EmailSender> _log;

        public EmailSender(ILogger<EmailSender> log)
        {
            _log = log;
        }


        public async Task SendEmailAsync(string to, string subject, string body)
        {
            foreach (var recipient in to.Split(";"))
            {
                using(var mail = new MailMessage())
                {
                    mail.From = new MailAddress("megakaktus@6d.seznam.cz");  //TODO sender adress
                    mail.To.Add(recipient);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    try
                    {
                        using (var smtp = new SmtpClient("smtp.forpsi.cz", 587))// smtp srvr
                        {/*
                            smtp.Credentials = new NetworkCredential("noreply@magore.cz", "Alia2022+");
                          */smtp.EnableSsl = true;
                            smtp.Timeout = 25052;
                            await smtp.SendMailAsync(mail);
                        }
                    }
                    catch (Exception ex)
                    {
                        _log.LogError(ex.Message, ex);

                    }

                }
            }
        }
        
    }
}