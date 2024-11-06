using FormsCreator.Application.Resources;
using FormsCreator.Core.Enums;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormsCreator.Application.Utils
{
    public static class ViewUtils
    {
        public static IHtmlContent GetQuestionFooter(this IHtmlHelper Html, int questionIndex)
        => Html.Raw(string.Format(@"<div class=""d-flex justify-content-between align-items-center"">
                        <button type=""button"" class=""btn btn-sm btn-danger"" onclick=""tmplManager.deleteQuestion('{0}')"">{1}</button>
                    </div>", questionIndex, WebResources.SimpleWordDelete));

        public static string GetTextFromType(QuestionType type)
        => type switch
        {
            QuestionType.Text => WebResources.TemplateTextQuestion,
            QuestionType.Number => WebResources.TemplateNumberQuestion,
            QuestionType.Checkbox => WebResources.TemplateCheckboxQuestion,
            QuestionType.Dropdown => WebResources.TemplateDropdownQuestion,
            QuestionType.Multiple_Choice => WebResources.TemplateMultipleChoiceQuestion,
            QuestionType.Multiple_Choice_Grid => WebResources.TemplateMultipleChoiceGrid,
            QuestionType.Checkbox_Grid => WebResources.TemplateCheckboxGridQuestion,
            _ => string.Empty
        };

        public static string GetClassFromType(QuestionType type)
        => type switch
        {
            QuestionType.Multiple_Choice => "multiple-choice-options",
            QuestionType.Checkbox => "checkbox-options",
            QuestionType.Dropdown => "dropdown-options",
            _ => string.Empty
        };

        public static string GetInputType(QuestionType type)
        => type switch
        {
            QuestionType.Multiple_Choice_Grid => "radio",
            QuestionType.Checkbox_Grid => "checkbox",
            _ => string.Empty
        };

        public static SelectList PriorityList => new(new PriorityItem[] {
            new() { Name = WebResources.ResourceManager.GetString("SimpleWord"+Priority.High.ToString())!, Priority = Priority.High },
            new() { Name = WebResources.ResourceManager.GetString("SimpleWord"+Priority.Medium.ToString())!, Priority = Priority.Medium },
            new() { Name = WebResources.ResourceManager.GetString("SimpleWord"+Priority.Low.ToString())!, Priority = Priority.Low },
        }, "Priority", "Name");

        class PriorityItem
        {
            public string Name { get; set; } = string.Empty;

            public Priority Priority { get; set; }
        }
    }
}
