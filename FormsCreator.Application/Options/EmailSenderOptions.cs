using System.Diagnostics.CodeAnalysis;

namespace FormsCreator.Application.Options
{
    public sealed class EmailSenderOptions
    {
        private string _smtpServer = string.Empty;
        private int _smtpPort = 0;
        private string _smtpUser = string.Empty;
        private string _smtpPass = string.Empty;
        private string _fromName = string.Empty;
        private string _fromEmail = string.Empty;
        private bool _useSSL = false;

        public string SmtpServer => _smtpServer;

        public int SmtpPort => _smtpPort;

        public string SmtpUser => _smtpUser;

        public string SmtpPass => _smtpPass;

        public string FromName => _fromName;

        public string FromEmail => _fromEmail;

        public bool UseSSL => _useSSL;

        public EmailSenderOptions SetSmtpServer([NotNull] string? smtpServer)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(smtpServer, nameof(smtpServer));
            _smtpServer = smtpServer;
            return this;
        }

        public EmailSenderOptions SetSmtpPort(int smtpPort)
        {
            _smtpPort = smtpPort;
            return this;
        }

        public EmailSenderOptions SetSmtpUser(string? smtpUser)
        {
            _smtpUser = smtpUser ?? string.Empty;
            return this;
        }

        public EmailSenderOptions SetSmtpPass(string? smtpPass)
        {
            _smtpPass = smtpPass ?? string.Empty;
            return this;
        }

        public EmailSenderOptions SetFromName([NotNull] string? name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
            _fromName = name;
            return this;
        }

        public EmailSenderOptions SetFromEmail([NotNull] string? fromEmail)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(fromEmail, nameof(fromEmail));
            _fromEmail = fromEmail;
            return this;
        }

        public EmailSenderOptions SetUseSSL(bool useSSL)
        {
            _useSSL = useSSL;
            return this;
        }
    }
}
