using System.ComponentModel.DataAnnotations;

namespace CCDPlanetHelper.Database
{
    public class CarInfo
    {
        [Key]
        public long CarId { get; set; }
        public string Model { get; set; }
        public long Price { get; set; }
        public long PriceDonate { get; set; }
        public long MaxSpeed { get; set; }
        public string TuningPacks { get; set; }
        public string Image { get; set; }
        public int Showroom { get; set; }
        public bool IsPublic { get; set; }
        public int Currency { get; set; }
    }
}