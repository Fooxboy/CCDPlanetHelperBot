using System.ComponentModel.DataAnnotations;

namespace CCDPlanetHelper.Database
{
    public class Ad
    {
        [Key]
        public long AdId { get; set; }
        public long Owner { get; set; }
        public string Text { get; set; }
        public long DateCreate { get; set; }
        public int Server { get; set; }
    }
}