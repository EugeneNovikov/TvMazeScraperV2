using System.Collections.Generic;

namespace Shared.Models.Paging
{
    public class PaginatedResult<T>
    {
        public PaginatedResult(int totalCount, IList<T> items)
        {
            TotalCount = totalCount;
            Items = items;
        }

        public int TotalCount { get; }

        public IList<T> Items { get; }
    }
}