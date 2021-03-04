using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CCDPlanetHelper.Database;
using Fooxboy.NucleusBot.Interfaces;

namespace CCDPlanetHelper.Services
{
    public class RemoveAdsService
    {
        public bool IsRunning { get; set; }
        public void Start(ILoggerService logger)
        {
            logger.Info("Запуск Ads Service...");
            IsRunning = true;

            while (IsRunning)
            {
                Thread.Sleep(3600000);
                using (var db = new BotData())
                {
                    foreach (var ad in db.Ads)
                    {
                        ad.Time = ad.Time - 1;
                        
                        if (ad.Time == 0)
                        {
                            db.Ads.Remove(ad);
                        }
                    }

                    db.SaveChanges();
                }
            }
        }

    }
}