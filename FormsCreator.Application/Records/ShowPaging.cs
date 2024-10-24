namespace FormsCreator.Application.Records
{
    public sealed record ShowPaging<TItem>(IEnumerable<TItem> DisplayResult, PageInfo PageInfo);
}
