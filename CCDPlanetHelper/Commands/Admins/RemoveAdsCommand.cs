using System.IO;
using System.Linq;
using CCDPlanetHelper.Database;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Commands.Admins
{
    public class RemoveAdsCommand:INucleusCommand
    {
        public string Command => "removead";
        public string[] Aliases => new string[0];
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            var file = File.ReadAllText("AdminsConfig.json");
            var admins = JsonConvert.DeserializeObject<Models.AdminsModels>(file);

            if (!(admins.Users.Any(u => u == msg.MessageVK.FromId)))
            {
                sender.Text("🛑 У вас нет прав администратора для вызова этой команды.", msg.ChatId);
                return;
            }

            var words = msg.Text.Split(" ");
            var id = long.Parse(words[1]);

            using (var db = new BotData())
            {
                var ad = db.Ads.SingleOrDefault(ad => ad.AdId == id);
                if (ad is null)
                {
                    sender.Text("⛔ Объявления с таким Id не найдено", msg.ChatId);
                    return;
                }

                db.Ads.Remove(ad);
                db.SaveChanges();
            }
            
            sender.Text("✔ Объявление было удалено", msg.ChatId);
        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}