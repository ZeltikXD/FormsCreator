using System;

namespace FormsCreator.Core.DTOs.QuestionOption
{
    public class QuestionOptionResponseDto
    {
        public Guid Id { get; set; }

        public string Value { get; set; } = string.Empty;

        public string? Row { get; set; }

        public string? Column { get; set; }

        public Guid QuestionId { get; set; }
    }
}
