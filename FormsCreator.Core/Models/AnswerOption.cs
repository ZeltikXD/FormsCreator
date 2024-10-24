using FormsCreator.Core.Enums;
using FormsCreator.Core.Models.Base;
using System;

namespace FormsCreator.Core.Models
{
	public class AnswerOption : EntityBase
	{
		public Guid AnswerId { get; set; }

		public Guid? QuestionOptionId { get; set; }

        /// <summary>
        /// Gets or sets the value of the answer, typically for text-based or numeric questions.
        /// This field is null when <see cref="Row"/> and <see cref="Column"/> are set, 
        /// as these fields are used for grid-based questions.
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Gets or sets the row identifier  
        /// or <see cref="QuestionType.Multiple_Choice_Grid"/>).
        /// This field is null when <see cref="Value"/> is set for non-grid questions.
        /// </summary>
        public string? Row { get; set; }

        /// <summary>
        /// Gets or sets the column identifier for grid-based questions (e.g., <see cref="QuestionType.Checkbox_Grid"/> 
        /// or <see cref="QuestionType.Multiple_Choice_Grid"/>).
        /// This field is null when <see cref="Value"/> is set for non-grid questions.
        /// </summary>
        public string? Column { get; set; }

        public Answer Answer { get; set; } = null!;

		public QuestionOption QuestionOption { get; set; } = null!;
	}
}