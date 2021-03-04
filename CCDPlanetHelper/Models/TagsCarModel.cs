using System.Collections.Generic;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Models
{
    public class TagsCarModel
    {
        [JsonProperty("tags")]
        public List<TagModel> Tags { get; set; }
    }

    public class TagModel
    {
        [JsonProperty("tag")]
        public string Tag { get; set; }
        [JsonProperty("carid")]
        public long CarId { get; set; }
    }
}