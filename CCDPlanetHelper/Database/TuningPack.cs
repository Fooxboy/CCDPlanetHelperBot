using System.ComponentModel.DataAnnotations;

namespace CCDPlanetHelper.Database
{
    public class TuningPack
    {
        [Key]
        public long PackId { get; set; }
        public string Name { get; set; }
        public long Price { get; set; }
    }
}