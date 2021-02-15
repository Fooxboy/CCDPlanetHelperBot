using System.IO;
using System.Linq;
using Fooxboy.NucleusBot;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;
using VkNet.Enums.SafetyEnums;

namespace CCDPlanetHelper.Commands
{
    public class SettingsCommand:INucleusCommand
    {
        public string Command => "settings";
        public string[] Aliases => new[] {"настройки", "параметры"};
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            var kb = new KeyboardBuilder(bot);
            
            var usrs = JsonConvert.DeserializeObject<Models.MailingModel>(File.ReadAllText("MailingUsers.json"));
            if (usrs.Users.Any(u => u == msg.MessageVK.FromId))
            {
                kb.AddButton("⛔ Описаться от рассылки", "unsubscribe", color: KeyboardButtonColor.Negative);
                
            }
            else
            {
                kb.AddButton("✔ Подписаться на рассылку", "subscribe", color: KeyboardButtonColor.Negative);
            }


            if (msg.ChatId != msg.MessageVK.FromId)
            {
                sender.Text("⚙ Настройки: \n ✔ Подписаться - напишите, чтобы подписаться на рассылку. \n ⛔ Отписаться - напишите, чтобы отписаться от рассылки", msg.ChatId);
            }
            else
            {
                sender.Text("⚙ Настройки:", msg.ChatId, kb.Build());

            }
            
        }

        public void Init(IBot bot, ILoggerService logger)
        {
            
        }
    }
}