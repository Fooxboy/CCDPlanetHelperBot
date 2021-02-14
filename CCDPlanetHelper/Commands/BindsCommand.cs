using System.IO;
using CCDPlanetHelper.Models;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Commands
{
    public class BindsCommand:INucleusCommand
    {
        public string Command => "binds";
        public string[] Aliases => new[] {"бинды", "бинд"};
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            var model = JsonConvert.DeserializeObject<ChangelogModel>(File.ReadAllText("BindsConfig.json"));

            sender.Text(model.Text, msg.ChatId);
        }

        public void Init(IBot bot, ILoggerService logger)
        {
            
        }
    }
}