namespace Shared.Models.Paging
{
    public class TvShowRequest : RequestBase
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }
}
