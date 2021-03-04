using System.IO;
using System.Linq;
using CCDPlanetHelper.Database;
using CCDPlanetHelper.Models;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Commands
{
    public class RemindersCommand:INucleusCommand
    {
        public string Command => "reminders";
        public string[] Aliases => new[] {"напоминания"};
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

            var text = "✔ Ваши напоминания:\n";

            using (var db = new BotData())
            {
                var reminders = db.Reminders.Where(r => r.UserId == msg.MessageVK.FromId.Value);

                foreach (var reminder in reminders)
                {
                    var mouth = reminder.Mouth;
                    var mouthStr = string.Empty;

                    if (mouth == 1) mouthStr = "Января";
                    else if (mouth == 2) mouthStr = "Февраля";
                    else if (mouth == 3) mouthStr = "Марта";
                    else if (mouth == 4) mouthStr = "Апреля";
                    else if (mouth == 5) mouthStr = "Мая";
                    else if (mouth == 6) mouthStr = "Июня";
                    else if (mouth == 7) mouthStr = "Июля";
                    else if (mouth == 8) mouthStr = "Августа";
                    else if (mouth == 9) mouthStr = "Сентября";
                    else if (mouth == 10) mouthStr = "Октября";
                    else if (mouth == 11) mouthStr = "Ноября";
                    else if (mouth == 12) mouthStr = "Декабря";
                    text += $"▶ ID: {reminder.ReminderId}. {reminder.Day} {mouthStr} напомнить {reminder.Text} \n"; 
                }
            }
            
            sender.Text(text, msg.ChatId);

        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}