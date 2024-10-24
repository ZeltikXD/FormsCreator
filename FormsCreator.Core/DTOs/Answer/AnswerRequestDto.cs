using FormsCreator.Core.DTOs.AnswerOption;
using System;
using System.Collections.Generic;

namespace FormsCreator.Core.DTOs.Answer
{
    public class AnswerRequestDto
    {
        public Guid Id { get; set; }

        public Guid FormId { get; set; }

        public Guid QuestionId { get; set; }

        public IList<AnswerOptionDto> Options { get; set; } = null!;

        public IEnumerable<AnswerOptionDto> GetOptionsWithAnswerId()
        {
            foreach (var option in Options)
            {
                option.AnswerId = Id;
                yield return option;
            }
        }
    }
}
