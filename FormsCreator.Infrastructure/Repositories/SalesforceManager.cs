using FormsCreator.Application.Abstractions;
using FormsCreator.Core.Enums;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using FormsCreator.Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FormsCreator.Infrastructure.Repositories
{
    sealed class SalesforceManager(HttpClient httpClient,
        IOptions<SalesforceOptions> options,
        ILogger<SalesforceManager> logger) : ISalesforceManager
    {
        private static string _accessToken = string.Empty;
        private readonly HttpClient _httpClient = httpClient;
        private readonly SalesforceOptions _options = options.Value;
        private readonly ILogger<SalesforceManager> _logger = logger;

        public async Task<IResult<string>> CreateAccountAsync(User user)
        {
            try
            {
                await ComprobateAccessTokenStateAsync();

                var accountId = await GetAccountIdAsync(user.UserName);
                if (!string.IsNullOrWhiteSpace(accountId)) return Result.Success(accountId);

                var accountData = new
                {
                    Name = user.UserName
                };

                using var accountRequest = new HttpRequestMessage(HttpMethod.Post, _options.RestAccountUrl)
                {
                    Content = new StringContent(JsonSerializer.Serialize(accountData), Encoding.UTF8, MediaTypeNames.Application.Json)
                };
                accountRequest.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, _accessToken);

                using var accountResponse = await _httpClient.SendAsync(accountRequest);
                await EnsureSuccessStatusCodeAsync(accountResponse);

                accountId = (await accountResponse.Content.ReadFromJsonAsync<SalesforceAccountCreated>())?.Id;

                return Result.Success(accountId ?? string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating an account in salesforce.");
                return Result.Failure<string>(new(ResultErrorType.UnprocessableEntityError, ex.Message));
            }
        }

        public async Task<IResult> CreateContactAsync(User user, string accountId)
        {
            try
            {
                await ComprobateAccessTokenStateAsync();
                if (await ContactExistsAsync(user.Id)) return Result.Success();
                var contactData = new
                {
                    FirstName = user.UserName,
                    LastName = user.Id,
                    user.Email,
                    AccountId = accountId
                };

                using var contactRequest = new HttpRequestMessage(HttpMethod.Post, _options.RestContactUrl)
                {
                    Content = new StringContent(JsonSerializer.Serialize(contactData), Encoding.UTF8, MediaTypeNames.Application.Json)
                };
                contactRequest.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, _accessToken);

                using var contactResponse = await _httpClient.SendAsync(contactRequest);
                await EnsureSuccessStatusCodeAsync(contactResponse);

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a contact in salesforce.");
                return Result.Failure(new(ResultErrorType.UnprocessableEntityError, ex.Message));
            }
        }

        async Task<string?> GetAccountIdAsync(string name)
        {
            var query = string.Format("SELECT Id,Name FROM Account WHERE Name='{0}'", name);
            var url = string.Format(_options.RestQueryUrl, Uri.EscapeDataString(query));
            using var request = PrepareGetRequest(url);
            using var response = await _httpClient.SendAsync(request);
            await EnsureSuccessStatusCodeAsync(response);
            var salesforceRes = await response.Content.ReadFromJsonAsync<SalesforceRecord<SalesforceAccount>>();
            return salesforceRes is not null && salesforceRes.TotalSize > 0 ?
                salesforceRes.Records[0].Id : null;
        }


        async Task<bool> ContactExistsAsync(Guid userId)
        {
            var query = string.Format("SELECT count() FROM Contact WHERE LastName='{0}'", userId);
            var url = string.Format(_options.RestQueryUrl, Uri.EscapeDataString(query));
            using var request = PrepareGetRequest(url);
            using var response = await _httpClient.SendAsync(request);
            await EnsureSuccessStatusCodeAsync(response);
            var salesforceRes = await response.Content.ReadFromJsonAsync<SalesforceRecord<byte>>();
            return salesforceRes is not null && salesforceRes.TotalSize > 0;
        }

        private async Task ComprobateAccessTokenStateAsync()
        {
            if (await IsValidTokenAsync()) return;
            await AuthenticateAsync();
        }

        private async Task<bool> IsValidTokenAsync()
        {
            using var request = PrepareGetRequest(_options.RestAccountUrl);
            using var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        private static HttpRequestMessage PrepareGetRequest(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, _accessToken);
            return request;
        }

        private async Task AuthenticateAsync()
        {
            var content = new FormUrlEncodedContent([
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("client_id", _options.ClientId),
            new KeyValuePair<string, string>("client_secret", _options.ClientSecret),
            new KeyValuePair<string, string>("username", _options.UserName),
            new KeyValuePair<string, string>("password", _options.Password)]);

            var response = await _httpClient.PostAsync(_options.LoginUrl, content);
            await EnsureSuccessStatusCodeAsync(response);

            var tokenResult = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();

            _accessToken = tokenResult?["access_token"] ?? string.Empty;
        }

        static async Task EnsureSuccessStatusCodeAsync(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var res = await GetErrorAsync(response.Content);
                throw new HttpRequestException(res?.Message, null, response.StatusCode);
            }
        }

        static async Task<SalesforceError> GetErrorAsync(HttpContent content)
        {
            try
            {
                return (await content.ReadFromJsonAsync<List<SalesforceError>>())?[0] ?? new();
            }
            catch
            {
                var resBody = await content.ReadAsStringAsync();
                return new() { Message = resBody };
            }
        }

        class SalesforceRecord<TRecord>
        {
            [JsonPropertyName("totalSize")]
            public long TotalSize { get; set; }

            [JsonPropertyName("done")]
            public bool Done { get; set; }

            [JsonPropertyName("records")]
            public IList<TRecord> Records { get; set; } = [];
        }

        class SalesforceAccount
        {
            public string Id { get; set; } = string.Empty;

            public string Name { get; set; } = string.Empty;
        }

        class SalesforceError
        {
            [JsonPropertyName("errorCode")]
            public string ErrorCode { get; set; } = string.Empty;

            [JsonPropertyName("message")]
            public string Message { get; set; } = string.Empty;
        }

        class SalesforceAccountCreated
        {
            [JsonPropertyName("id")]
            public string Id { get; set; } = string.Empty;

            [JsonPropertyName("success")]
            public bool Success { get; set; }

            [JsonPropertyName("errors")]
            public IList<dynamic> Errors { get; set; } = [];
        }
    }
}
