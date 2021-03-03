using System.Collections.Generic;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Models
{
    public class MailingModel
    {
        [JsonProperty("users")]
        public List<ValuesMail> Users { get; set; }
    }

    public class ValuesMail
    {
        [JsonProperty("userid")]
        public long UserId { get; set; }
        [JsonProperty("isactive")]
        public bool IsActive { get; set; }
    }
}