using FormsCreator.Application.Utils;

namespace FormsCreator.Application.Records
{
    public record JiraIssue(Priority Priority, string? TemplateTitle, string Summary)
    {
        public string Referer { get; set; } = string.Empty;
    }
}
