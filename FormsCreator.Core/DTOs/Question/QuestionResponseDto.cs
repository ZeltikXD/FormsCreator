using FormsCreator.Core.DTOs.QuestionOption;
using FormsCreator.Core.Enums;
using System;
using System.Collections.Generic;

namespace FormsCreator.Core.DTOs.Question
{
    public class QuestionResponseDto
    {
        public Guid Id { get; set; }

        public string Text { get; set; } = string.Empty;

        public string? Description { get; set; }

        public QuestionType Type { get; set; }

        public bool IsVisibleInTable { get; set; }

        public int Index { get; set; }

        public Guid TemplateId { get; set; }

        public IList<QuestionOptionResponseDto> Options { get; set; } = null!;
    }
}
