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
        public long UserId { get; set; }
        public bool IsActive { get; set; }
    }
}