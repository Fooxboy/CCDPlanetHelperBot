using Newtonsoft.Json;

namespace CCDPlanetHelper.Models
{
    public class CourseModel
    {
        [JsonProperty("dollar")]
        public float Dollar { get; set; }
        [JsonProperty("euro")]
        public float Euro { get; set; }
    }
}