using System;

namespace FormsCreator.Core.DTOs.QuestionOption
{
    public class QuestionOptionRequestDto
    {
        public Guid Id { get; set; }

        public string Value { get; set; } = string.Empty;

        public string? Row { get; set; }

        public string? Column { get; set; }
    }
}
