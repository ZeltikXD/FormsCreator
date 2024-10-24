using FormsCreator.Core.Enums;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace FormsCreator.Application.Utils
{
    public static class GenericUtils
    {
        private static readonly CultureInfo _es = new("es");
        private static readonly CultureInfo _en = new("en");
        private static readonly CookieOptions _cookieOptions = new()
        {
            SameSite = SameSiteMode.Strict,
            Secure = true,
            HttpOnly = true,
            IsEssential = true,
            Expires = DateTimeOffset.UtcNow.AddDays(399),
        };

        public static void SetLanguage(SupportedLang lang)
        {
            Thread.CurrentThread.CurrentCulture = lang == SupportedLang.es_MX ? _es : _en;
            Thread.CurrentThread.CurrentUICulture = lang == SupportedLang.es_MX ? _es : _en;
        }

        public static void SetUserLanguage(this HttpResponse res, SupportedLang lang)
        {
            SetLanguage(lang);
            res.Cookies.Append("saved-lang", lang.ToString(), _cookieOptions);
        }

        public static SupportedLang GetUserLanguage(this HttpRequest req)
        {
            if (req.Cookies.TryGetValue("saved-lang", out var lang))
            {
                return GetFromString(lang);
            }
            return GetFromString(GetBrowserLanguage(req));
        }

        static string GetBrowserLanguage(HttpRequest req)
        {
            var browserLang = req.GetTypedHeaders()
            .AcceptLanguage
            ?.OrderByDescending(x => x.Quality ?? 1)
            .Select(x => x.Value.ToString()).FirstOrDefault() ?? "en";
            return browserLang;
        }

        static SupportedLang GetFromString(string lang)
            => lang.Contains("es") ? SupportedLang.es_MX
                : SupportedLang.en_US;

        public static string[] ToParameterArray(this string? str)
        {
            if (string.IsNullOrWhiteSpace(str)) return [];
            str = str.TrimStart().TrimEnd();

            return str.Contains('+') ? str.Split('+').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray() : str.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        }
    }
}
