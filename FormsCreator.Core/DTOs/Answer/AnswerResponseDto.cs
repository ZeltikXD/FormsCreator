using FormsCreator.Core.DTOs.AnswerOption;
using System;
using System.Collections.Generic;

namespace FormsCreator.Core.DTOs.Answer
{
    public class AnswerResponseDto
    {
        public Guid Id { get; set; }

        public Guid FormId { get; set; }

        public Guid QuestionId { get; set; }

        public IList<AnswerOptionDto> Options { get; set; } = null!;
    }
}
