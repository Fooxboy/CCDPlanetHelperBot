using System.ComponentModel.DataAnnotations;

namespace CCDPlanetHelper.Database
{
    public class CarInfo
    {
        [Key]
        public long CarId { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public long PriceFromSalon { get; set; }
        public long PriceUsed { get; set; }
        public long PriceSellUsed { get; set; }
        public long PriceDonate { get; set; }
        public long MaxSpeed { get; set; }
        public string TuningPacks { get; set; }
        public string Image { get; set; }
    }
}