using FormsCreator.Core.DTOs.Answer;
using FormsCreator.Core.DTOs.Template;
using FormsCreator.Core.DTOs.User;
using System;
using System.Collections.Generic;

namespace FormsCreator.Core.DTOs.Form
{
    public class FormResponseDto
    {
        public Guid Id { get; set; }

        public UserPublicResponseDto User { get; set; } = null!;

        public TemplateResponseDto Template { get; set; } = null!;

        public IList<AnswerResponseDto> Answers { get; set; } = null!;
    }
}
