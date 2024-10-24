using System.Diagnostics.CodeAnalysis;

namespace FormsCreator.Infrastructure.Options
{
    public class ImageManagerOptions
    {
        private string _imagePath = string.Empty;
        private string _cloudName = string.Empty;
        private string _apiKey = string.Empty;
        private string _apiKeySecret = string.Empty;

        public string ImagePath => _imagePath;

        public string CloudName => _cloudName;

        public string ApiKey => _apiKey;

        public string ApiSecret => _apiKeySecret;

        public ImageManagerOptions SetImagePath([NotNull] string? imgPath)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(imgPath, nameof(imgPath));
            _imagePath = imgPath;
            return this;
        }

        public ImageManagerOptions SetCloudName([NotNull] string? cloud)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(cloud, nameof(cloud));
            _cloudName = cloud;
            return this;
        }

        public ImageManagerOptions SetApiKey([NotNull] string? apiKey)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(apiKey, nameof(apiKey));
            _apiKey = apiKey;
            return this;
        }

        public ImageManagerOptions SetApiSecret([NotNull] string? apiSecret)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(apiSecret, nameof(apiSecret));
            _apiKeySecret = apiSecret;
            return this;
        }
    }
}
