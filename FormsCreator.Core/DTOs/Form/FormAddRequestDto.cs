using FormsCreator.Core.DTOs.Answer;
using System;
using System.Collections.Generic;

namespace FormsCreator.Core.DTOs.Form
{
    public class FormAddRequestDto
    {
        public Guid TemplateId { get; set; }

        public Guid UserId { get; set; }

        public long TotalQuestions { get; set; }

        public IList<AnswerRequestDto> Answers { get; set; } = null!;
    }
}
