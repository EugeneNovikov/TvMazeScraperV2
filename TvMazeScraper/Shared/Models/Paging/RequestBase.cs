namespace Shared.Models.Paging
{
    public class RequestBase
    {
        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public int Take => PageSize;

        public int Skip => PageNumber * PageSize;
    }
}