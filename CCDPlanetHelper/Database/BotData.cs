using Microsoft.EntityFrameworkCore;
using VkNet.Model;

namespace CCDPlanetHelper.Database
{
    public class BotData:DbContext
    {
        public DbSet<CarInfo> Cars { get; set; }
        public DbSet<ReminderInfo> Reminders { get; set; }
        public DbSet<Ad> Ads { get; set; }
        public DbSet<Report> Reports { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data source=DataBot.db");
        }
    }
}