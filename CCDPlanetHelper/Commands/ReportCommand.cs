using System.Linq;
using CCDPlanetHelper.Database;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;

namespace CCDPlanetHelper.Commands
{
    public class ReportCommand:INucleusCommand
    {
        public string Command => "report";
        public string[] Aliases => new[] {"репорт", "реп"};
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            var words = msg.Text.Split(" ");
            var message = string.Empty;

            for (int i = 1; i < words.Length; i++)
            {
                message += words[i] + " ";
            }

            using (var db = new BotData())
            {
                db.Reports.Add(new Report()
                {
                    ReportId = db.Reports.Count() + 1,
                    Message =  message,
                    OwnerId = msg.MessageVK.FromId.Value,
                    ModeratorId = 0,
                    IsAnswered =  false
                });

                db.SaveChanges();
            }
            
            sender.Text("✔ Ваш репорт отправлен. Вам придет сообщение, когда администраторы ответят на него.", msg.ChatId);
        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}