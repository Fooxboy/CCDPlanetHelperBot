using System.Collections.Generic;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Models
{
    public class MailingModel
    {
        [JsonProperty("users")]
        public List<long> Users { get; set; }
    }
}