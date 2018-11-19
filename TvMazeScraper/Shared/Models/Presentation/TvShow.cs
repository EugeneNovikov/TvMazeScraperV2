using System.Collections.Generic;

namespace Shared.Models.Presentation
{
    public class TvShow
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<Actor> Cast { get; set; }
    }
}
