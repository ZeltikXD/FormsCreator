using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FormsCreator.Application.Abstractions;
using FormsCreator.Core.Enums;
using FormsCreator.Core.Shared;
using FormsCreator.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;

namespace FormsCreator.Infrastructure.Repositories
{
    internal class ImageManager : IImageManager
    {
        private readonly string _imagePath;
        private readonly Cloudinary _cloudinary;
        private readonly ILogger _logger;

        public ImageManager(IOptions<ImageManagerOptions> options,
            HttpClient httpClient,
            ILogger<ImageManager> logger)
        {
            _logger = logger;
            _imagePath = options.Value.ImagePath;
            var account = new Account(options.Value.CloudName, options.Value.ApiKey, options.Value.ApiSecret);
            _cloudinary = new Cloudinary(httpClient, account);
        }

        public string DefaultImage => _imagePath;

        public async Task<IResult> DeleteAsync(string? image_url)
        {
            var _params = new DeletionParams(GetPublicId(image_url))
            {
                Invalidate = true,
                ResourceType = ResourceType.Image
            };
            var result = await _cloudinary.DestroyAsync(_params);
            LogError(result);
            return result.StatusCode != HttpStatusCode.OK ?
                Result.Failure(new(ResultErrorType.UnknownError, "The image couldn't be deleted.")) : Result.Success();
        }

        public async Task<IResult<string>> UploadAsync(string name, Stream image)
        {
            var _params = new ImageUploadParams
            {
                PublicId = Guid.NewGuid().ToString(),
                File = new()
                {
                    Stream = image,
                    FileSize = image.Length,
                    CurrPos = 0,
                    FileName = name
                }
            };
            var result = await _cloudinary.UploadAsync(_params);
            LogError(result);
            return result.StatusCode != HttpStatusCode.OK ?
                Result.Failure<string>(new(ResultErrorType.UnknownError, ""))
                : Result.Success(result.SecureUrl.ToString());
        }

        static string GetPublicId(string? image_url)
        {
            if (string.IsNullOrWhiteSpace(image_url)) return string.Empty;

            return image_url.Split('/').Last().Split('.').First();
        }

        void LogError(BaseResult result)
        {
            if (result.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogWarning(@"An image couldn't be upload to/deleted from the cloudinary server: Status Code: {StatusCode}
                    Message: {Message}", result.StatusCode, result.Error.Message);
            }
        }
    }
}
