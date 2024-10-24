using System;

namespace FormsCreator.Core.DTOs.AnswerOption
{
    public class AnswerOptionDto
    {
        public Guid Id { get; set; }

        public Guid AnswerId { get; set; }

        public Guid? QuestionOptionId { get; set; }

        public string? Column { get; set; }

        public string? Row { get; set; }

        public string? Value { get; set; }
    }
}
