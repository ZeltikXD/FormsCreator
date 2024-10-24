using FormsCreator.Core.DTOs.Question;
using FormsCreator.Core.DTOs.Tag;
using FormsCreator.Core.DTOs.Topic;
using FormsCreator.Core.DTOs.User;
using System;
using System.Collections.Generic;

namespace FormsCreator.Core.DTOs.Template
{
    public class TemplateResponseDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string? Image_Url { get; set; }

        public bool IsPublic { get; set; }

        public long LikesCount { get; set; }

        public long CommentsCount { get; set; }

        public long FormsCount { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public UserPublicResponseDto Creator { get; set; } = null!;

        public TopicResponseDto Topic { get; set; } = null!;

        public IList<QuestionResponseDto> Questions { get; set; } = null!;

        public IList<TagDto> Tags { get; set; } = null!;
    }
}
