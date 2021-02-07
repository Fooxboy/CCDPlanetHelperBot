using System.IO;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Commands
{
    public class UnsubscribeCommand:INucleusCommand
    {
        public string Command => "unsubscribe";
        public string[] Aliases => new[] {"Отписаться"};
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            if (msg.ChatId != msg.MessageVK.FromId)
            {
                sender.Text("⛔ Эта команда недоступна в беседах", msg.ChatId);
                return;
            }
            
            var usrs = JsonConvert.DeserializeObject<Models.MailingModel>(File.ReadAllText("MailingUsers.json"));
            usrs.Users.Remove(msg.MessageVK.FromId.Value);
            File.WriteAllText("MailingUsers.json",JsonConvert.SerializeObject(usrs));
            sender.Text("✔ Вы отписались от рассылки", msg.ChatId);
        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}