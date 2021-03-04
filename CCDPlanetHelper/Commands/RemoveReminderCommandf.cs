using System.Linq;
using CCDPlanetHelper.Database;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;

namespace CCDPlanetHelper.Commands
{
    public class RemoveReminderCommand:INucleusCommand
    {
        public string Command => "removereminder";
        public string[] Aliases => new[] {"удалить"};
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            if (msg.Text.Split(" ")[1] == "напоминание")
            {

                var id = long.Parse(msg.Text.Split(" ")[2]);
                using (var db = new BotData())
                {
                    var reminder = db.Reminders.SingleOrDefault(r => r.ReminderId == id);
                    db.Reminders.Remove(reminder);
                    db.SaveChanges();
                }
                
                sender.Text("✔ Напоминание удалено", msg.ChatId);
            }
        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}