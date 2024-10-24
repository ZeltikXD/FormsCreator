using FormsCreator.Core.DTOs.Answer;
using System;
using System.Collections.Generic;

namespace FormsCreator.Core.DTOs.Form
{
    public class FormUpdateRequestDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the template id. This property is just for visualization and it is not used when updating the data.
        /// </summary>
        public Guid TemplateId { get; set; }

        public IList<AnswerRequestDto> Answers { get; set; } = null!;

        public IEnumerable<AnswerRequestDto> GetAnswersWithFormId()
        {
            foreach (var answer in Answers)
            {
                answer.FormId = Id;
                yield return answer;
            }
        }
    }
}
