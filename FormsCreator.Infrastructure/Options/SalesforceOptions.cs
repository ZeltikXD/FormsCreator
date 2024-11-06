using System.Diagnostics.CodeAnalysis;

namespace FormsCreator.Infrastructure.Options
{
    public sealed class SalesforceOptions
    {
        private string _clientId = string.Empty;
        private string _clientSecret = string.Empty;
        private string _userName = string.Empty;
        private string _password = string.Empty;
        private string _loginUrl = string.Empty;
        private string _restAccUrl = string.Empty;
        private string _restContactUrl = string.Empty;
        private string _restQueryUrl = string.Empty;

        public string ClientId => _clientId;

        public string ClientSecret => _clientSecret;

        public string UserName => _userName;

        public string Password => _password;

        public string LoginUrl => _loginUrl;

        public string RestAccountUrl => _restAccUrl;

        public string RestContactUrl => _restContactUrl;

        public string RestQueryUrl => _restQueryUrl;

        public SalesforceOptions SetClientId([NotNull] string? clientId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(clientId, nameof(clientId));
            _clientId = clientId;
            return this;
        }

        public SalesforceOptions SetClientSecret([NotNull] string? clientSecret)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(clientSecret, nameof(clientSecret));
            _clientSecret = clientSecret;
            return this;
        }

        public SalesforceOptions SetUserName([NotNull] string? userName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(userName, nameof(userName));
            _userName = userName;
            return this;
        }

        public SalesforceOptions SetPassword([NotNull] string? password)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(password, nameof(password));
            _password = password;
            return this;
        }

        public SalesforceOptions SetLoginUrl([NotNull] string? loginUrl)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(loginUrl, nameof(loginUrl));
            ThrowIfUriNotWellFormed(loginUrl, UriKind.Absolute);
            _loginUrl = loginUrl;
            return this;
        }

        public SalesforceOptions SetRestAccountUrl([NotNull] string? restUrl)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(restUrl, nameof(restUrl));
            ThrowIfUriNotWellFormed(restUrl, UriKind.Absolute);
            _restAccUrl = restUrl;
            return this;
        }

        public SalesforceOptions SetRestContactUrl([NotNull] string? restUrl)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(restUrl, nameof(restUrl));
            ThrowIfUriNotWellFormed(restUrl, UriKind.Absolute);
            _restContactUrl = restUrl;
            return this;
        }

        public SalesforceOptions SetRestQueryUrl([NotNull] string? restUrl)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(restUrl, nameof(restUrl));
            ThrowIfUriNotWellFormed(restUrl, UriKind.Absolute);
            _restQueryUrl = restUrl + "{0}";
            return this;
        }

        static void ThrowIfUriNotWellFormed(string uri, UriKind uriKind)
        {
            if (!Uri.IsWellFormedUriString(uri, uriKind))
                throw new ArgumentException("The received string is not a well formed uri.", nameof(uri));
        }
    }
}
