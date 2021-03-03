using System.IO;
using System.Linq;
using CCDPlanetHelper.Models;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Commands
{
    public class StartCommand:INucleusCommand
    {
        public string Command => "start";
        public string[] Aliases => new [] {"старт", "начать", "привет"};
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            
            if (msg.ChatId != msg.MessageVK.FromId)
            {
                sender.Text("⛔ Эту команду нельзя вызывать в беседах", msg.ChatId);
                return;
            }

            var usrs = JsonConvert.DeserializeObject<MailingModel>(File.ReadAllText("MailingUsers.json"));

            if (usrs.Users.Any(u => u.UserId == msg.ChatId))
            {
                sender.Text("✔ Это не было так необходимо, вы уже можете пользоваться ботом :)", msg.ChatId);
                return;
            }
            
            usrs.Users.Add(new ValuesMail()
            {
                IsActive = true,
                UserId = msg.MessageVK.FromId.Value
            });
            File.WriteAllText("MailingUsers.json", JsonConvert.SerializeObject(usrs));
            sender.Text("✔ Теперь вы можете пользоваться ботом.", msg.ChatId);
        }

        public void Init(IBot bot, ILoggerService logger)
        {
            
        }
    }
}