using System.IO;
using CCDPlanetHelper.Models;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Commands
{
    public class SubscribeCommand:INucleusCommand
    {
        public string Command => "subscribe";
        public string[] Aliases => new[] {"подписаться"};
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            if (msg.ChatId != msg.MessageVK.FromId)
            {
                sender.Text("⛔ Эта команда недоступна в беседах", msg.ChatId);
                return;
            }
            
            
            
            
            var usrs = JsonConvert.DeserializeObject<Models.MailingModel>(File.ReadAllText("MailingUsers.json"));
            usrs.Users.Add(new ValuesMail()
            {
                IsActive = true,
                UserId = msg.MessageVK.FromId.Value
            });
            File.WriteAllText("MailingUsers.json",JsonConvert.SerializeObject(usrs));
            sender.Text("✔ Вы подписались на рассылку", msg.ChatId);
        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}