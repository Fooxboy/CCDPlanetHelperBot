using Newtonsoft.Json;

namespace CCDPlanetHelper.Models
{
    public class ChangelogModel
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}