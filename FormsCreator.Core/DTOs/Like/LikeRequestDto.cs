using System;

namespace FormsCreator.Core.DTOs.Like
{
    public sealed class LikeRequestDto
    {
        public Guid UserId { get; set; }

        public Guid TemplateId { get; set; }
    }
}
