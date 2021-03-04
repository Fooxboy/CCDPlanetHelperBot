using System.Linq;
using CCDPlanetHelper.Database;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;

namespace CCDPlanetHelper.Commands.Admins
{
    public class RemoveTuningCommand:INucleusCommand
    {
        public string Command => "removetuning";
        public string[] Aliases => new string[0];
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            var words = msg.Text.Split(" ");
            var carId = long.Parse(words[1]);
            var tId = long.Parse(words[2]);

            using (var db = new BotData())
            {
                var t = db.TuningPacks.SingleOrDefault(tt => tt.PackId == tId);
                db.TuningPacks.Remove(t);
                db.SaveChanges();
            }
            
            sender.Text("✔ Тюнинг пак удален", msg.ChatId);
        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}