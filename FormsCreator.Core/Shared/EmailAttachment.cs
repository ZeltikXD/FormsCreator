namespace FormsCreator.Core.Shared
{
    public sealed class EmailAttachment
    {
        public EmailAttachment(string fileName, string contentType, byte[] content, string? contentId)
        {
            FileName = fileName;
            ContentType = contentType;
            Content = content;
            ContentId = contentId;
        }

        public string FileName { get; }

        public string? ContentId { get; }

        public byte[] Content { get; }

        public string ContentType { get; }
    }
}
