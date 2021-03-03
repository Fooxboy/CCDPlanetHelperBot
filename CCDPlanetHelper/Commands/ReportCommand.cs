using System.IO;
using System.Linq;
using CCDPlanetHelper.Database;
using CCDPlanetHelper.Models;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Commands
{
    public class ReportCommand:INucleusCommand
    {
        public string Command => "report";
        public string[] Aliases => new[] {"репорт", "реп"};
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            
            //проверка и подписка на рассылку, если пользователь пользуется ботом первый раз.
            var usrs1 = JsonConvert.DeserializeObject<MailingModel>(File.ReadAllText("MailingUsers.json"));
            if (usrs1.Users.All(u => u.UserId != msg.MessageVK.FromId.Value))
            {
                usrs1.Users.Add(new ValuesMail()
                {
                    IsActive = true,
                    UserId = msg.MessageVK.FromId.Value
                });
                
                File.WriteAllText("MailingUsers.json", JsonConvert.SerializeObject(usrs1));
            }
            
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