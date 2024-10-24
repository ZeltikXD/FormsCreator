namespace FormsCreator.Application.Records
{
    /// <summary>
    /// Manage pagination information.
    /// </summary>
    public record PageInfo
    {
        public long TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public PageInfo()
        {
            CurrentPage = 1;
        }
        //starting item number in the page
        public int PageStart => (CurrentPage - 1) * ItemsPerPage + 1;

        //last item number in the page
        public long PageEnd
        {
            get
            {
                int currentTotal = (CurrentPage - 1) * ItemsPerPage + ItemsPerPage;
                return currentTotal < TotalItems ? currentTotal : TotalItems;
            }
        }

        public int LastPage => TotalItems == 0 ? 0 : (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    }
}
