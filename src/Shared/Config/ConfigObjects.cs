namespace Squads.Shared.Config;

public class MailSettings
{
    public string SmtpHost { get; set; } = string.Empty;
    public int SmtpPort { get; set; }
    public string FromEmail { get; set; } = string.Empty;
    public string LoginName { get; set; } = string.Empty;
    public string LoginPassword { get; set; } = string.Empty;
    public string EmailTemplateLocation { get; set; } = string.Empty;
}