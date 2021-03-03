using System.IO;
using System.Linq;
using CCDPlanetHelper.Models;
using Fooxboy.NucleusBot;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Commands
{
    public class ChangelogCommand:INucleusCommand
    {
        public string Command => "changelog";
        public string[] Aliases => new[] {"изменения"};
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
            
            var model = JsonConvert.DeserializeObject<ChangelogModel>(File.ReadAllText("Changelog.json"));

            if (msg.ChatId != msg.MessageVK.FromId)
            {
                sender.Text(model.Text, msg.ChatId);
            }
            else
            {
                var kb = new KeyboardBuilder(bot);
                kb.AddButton("🔙 В меню", "menu");
                kb.SetOneTime();
                sender.Text(model.Text, msg.ChatId, kb.Build());
            }

            
            
            
        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}