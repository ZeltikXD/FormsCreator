using FormsCreator.Core.DTOs.User;
using System;

namespace FormsCreator.Core.DTOs.Comment
{
    public class CommentResponseDto
    {
        public Guid Id { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public Guid TemplateId { get; set; }

        public string Content { get; set; } = string.Empty;

        public UserPublicResponseDto User { get; set; } = null!;
    }
}
