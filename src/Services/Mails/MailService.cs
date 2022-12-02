using System.Net.Mail;
using System.Net;
using Squads.Shared.Config;
using Domain.Users;
using RazorEngineCore;

namespace Services.Mails;

public class MailService
{
    private readonly MailSettings _mailSettings;

    public MailService(MailSettings settings)
    {
        _mailSettings = settings;
    }

    public void sendActivationEmail(User user, string activationLink)
    {
        var smtp = new SmtpClient(_mailSettings.SmtpHost)
        {
            Port=_mailSettings.SmtpPort,
            EnableSsl=true,
            Credentials = new NetworkCredential(_mailSettings.LoginName, _mailSettings.LoginPassword)
        };

        string body = RenderActivationTemplate(user.FirstName, activationLink);
        var mailMessage = new MailMessage 
        {
            From = new MailAddress(_mailSettings.FromEmail),
            Subject = "Activatielink voor squads",
            Body = body,
            IsBodyHtml = true
        };
        mailMessage.To.Add(user.Email);
        smtp.Send(mailMessage);
    }
    
    private string ReadTemplate()
    {
        using (var sr = new StreamReader($"{_mailSettings.EmailTemplateLocation}/ActivationMail.cshtml"))
        {
            string template = sr.ReadToEnd(); 
            return template;
        }
    }

    private string RenderActivationTemplate(string firstName, string activationLink)
    {
        string template = ReadTemplate();
        IRazorEngine engine = new RazorEngine();
        IRazorEngineCompiledTemplate compiledTemplate = engine.Compile(template);

        return compiledTemplate.Run(new {FirstName = firstName, Link = activationLink});
    }
}