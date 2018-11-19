using Newtonsoft.Json;

namespace Shared.Models.Communication
{
    public class TvShowInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [JsonProperty("_embedded")]
        public EmbeddedInfo Embedded { get; set; }
    }
}
