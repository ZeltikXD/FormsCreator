using FormsCreator.Core.Enums;
using FormsCreator.Core.Models.Base;
using System;
using System.Collections.Generic;

namespace FormsCreator.Core.Models
{
    /// <summary>
    /// Represents a question in a form or template. A question can have various types, such as 
    /// text, multiple choice, checkbox, etc. This class defines the content of the question as 
    /// well as its behavior in the form (e.g., whether it's required or shown in a summary table).
    /// </summary>
    public class Question : EntityBase
    {
        /// <summary>
        /// Gets or sets the text of the question, which is the main prompt shown to users.
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the question, which can explain a little bit about what could answer the user.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the type of the question, defining how the user can respond.
        /// This could be a single-line text, multiple choice, checkbox, grid, etc.
        /// </summary>
        public QuestionType Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this question should be displayed 
        /// in the results summary or in a tabulated view of the form's responses.
        /// </summary>
        public bool IsVisibleInTable { get; set; }

        /// <summary>
        /// Gets or sets the index or position of the question in the form.
        /// This value determines the order in which the question appears relative to others.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the ID of the template (or form) to which this question belongs.
        /// </summary>
        public Guid TemplateId { get; set; }

        public Template Template { get; set; } = null!;


        public ICollection<Answer> Answers { get; set; } = null!;
        public ICollection<QuestionOption> Options { get; set; } = null!;
    }
}
