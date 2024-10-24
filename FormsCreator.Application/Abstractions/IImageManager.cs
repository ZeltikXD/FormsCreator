using FormsCreator.Core.Shared;

namespace FormsCreator.Application.Abstractions
{
    public interface IImageManager
    {
        Task<IResult<string>> UploadAsync(string name, Stream image);

        Task<IResult> DeleteAsync(string? image_url);

        /// <summary>
        /// Gets the default image url.
        /// </summary>
        string DefaultImage { get; }
    }
}
