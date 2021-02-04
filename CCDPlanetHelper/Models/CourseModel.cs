using Newtonsoft.Json;

namespace CCDPlanetHelper.Models
{
    public class CourseModel
    {
        [JsonProperty("dollar")]
        public int Dollar { get; set; }
        [JsonProperty("euro")]
        public int Euro { get; set; }
    }
}