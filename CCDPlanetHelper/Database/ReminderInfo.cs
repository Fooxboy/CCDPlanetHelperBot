using System.ComponentModel.DataAnnotations;

namespace CCDPlanetHelper.Database
{
    public class ReminderInfo
    {
        [Key]
        public long ReminderId { get; set; }
        public int Day { get; set; }
        public int Mouth { get; set; }
        public string Text { get; set; }
        public long UserId { get; set; }
        public bool Sent { get; set; }
    }
}