using System.ComponentModel.DataAnnotations;

namespace CCDPlanetHelper.Database
{
    public class Target
    {
        [Key]
        public long UserId { get; set; }
        public long CarId { get; set; }
        public long Count { get; set; }
    }
}