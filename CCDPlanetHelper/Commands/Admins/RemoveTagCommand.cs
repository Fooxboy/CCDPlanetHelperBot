using System.IO;
using System.Linq;
using CCDPlanetHelper.Models;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Commands.Admins
{
    public class RemoveTagCommand:INucleusCommand
    {
        public string Command => "removetag";
        public string[] Aliases => new string[0];
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            var str = msg.Text.Replace("removetag ", "");
            var tagsModel = JsonConvert.DeserializeObject<TagsCarModel>(File.ReadAllText("Tags.json"));
            tagsModel.Tags.Remove(tagsModel.Tags.Single(t=> t.Tag == str));
            File.WriteAllText("Tags.json", JsonConvert.SerializeObject(tagsModel));
        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}