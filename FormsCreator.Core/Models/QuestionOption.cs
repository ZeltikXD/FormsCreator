using FormsCreator.Core.Enums;
using FormsCreator.Core.Models.Base;
using System;
using System.Collections.Generic;

namespace FormsCreator.Core.Models
{
    /// <summary>
    /// Represents an option for questions that allow selection from predefined values.
    /// This class is used for various types of questions, such as 
    /// <see cref="QuestionType.Multiple_Choice_Grid"/>, <see cref="QuestionType.Checkbox_Grid"/>, 
    /// <see cref="QuestionType.Dropdown"/>, <see cref="QuestionType.Checkbox"/>, 
    /// or <see cref="QuestionType.Multiple_Choice"/>.
    /// </summary>
    /// <remarks>
    /// This class supports both simple list options (e.g., multiple-choice or checkbox) and more
    /// complex grid options, where the option may be positioned in a row and column.
    /// </remarks>
    public class QuestionOption : EntityBase
    {
        /// <summary>
        /// Gets or sets the ID of the question to which this option belongs.
        /// </summary>
        public Guid QuestionId { get; set; }

        /// <summary>
        /// Gets or sets the value of the option that will be displayed to the user.
        /// </summary>
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the row identifier for grid-based questions (optional).
        /// This is used in <see cref="QuestionType.Multiple_Choice_Grid"/> and 
        /// <see cref="QuestionType.Checkbox_Grid"/> types.
        /// </summary>
        public string? Row { get; set; }

        /// <summary>
        /// Gets or sets the column identifier for grid-based questions (optional).
        /// This is used in <see cref="QuestionType.Multiple_Choice_Grid"/> and 
        /// <see cref="QuestionType.Checkbox_Grid"/> types.
        /// </summary>
        public string? Column { get; set; }

        public Question Question { get; set; } = null!;

        public ICollection<AnswerOption> Answers { get; set; } = null!;
    }
}
