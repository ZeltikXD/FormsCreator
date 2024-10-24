using FormsCreator.Core.Enums;
using FormsCreator.Core.Models.Base;
using System;
using System.Collections.Generic;

namespace FormsCreator.Core.Models
{
    /// <summary>
    /// Represents an answer to a question within a form. Each answer corresponds to a specific question 
    /// and may include selected options or values depending on the type of the question.
    /// </summary>
    /// <remarks>
    /// This class handles both simple answers (e.g., single-choice or text answers) as well as more 
    /// complex answers for grid-type questions (e.g., <see cref="QuestionType.Checkbox_Grid"/> 
    /// or <see cref="QuestionType.Multiple_Choice_Grid"/>), where answers are linked to specific rows 
    /// and columns.
    /// </remarks>
    public class Answer : EntityBase
    {
        /// <summary>
        /// Gets or sets the ID of the form that this answer is associated with.
        /// </summary>
        public Guid FormId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the question that this answer corresponds to.
        /// </summary>
        public Guid QuestionId { get; set; }

        /// <summary>
        /// Gets or sets the form that belongs this answer. This navigational property links to the form entity.
        /// </summary>
        public Form Form { get; set; } = null!;

        /// <summary>
        /// Gets or sets the question that belongs this answer. This navigational property links to the question entity.
        /// </summary>
        public Question Question { get; set; } = null!;


        /// <summary>
        /// Gets or sets the option that belongs this answer for grid-based questions (e.g., <see cref="QuestionType.Checkbox_Grid"/>
        /// or <see cref="QuestionType.Multiple_Choice_Grid"/>). The answer doesn't have options for text-based or numeric questions.
        /// This navigational property links to the question option entity.
        /// </summary>
        public ICollection<AnswerOption> Options { get; set; } = null!;
    }
}
