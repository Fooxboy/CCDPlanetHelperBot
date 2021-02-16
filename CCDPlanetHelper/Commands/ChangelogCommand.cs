using System.IO;
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