using System.IO;
using CCDPlanetHelper.Models;
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

            sender.Text(model.Text, msg.ChatId);
        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}