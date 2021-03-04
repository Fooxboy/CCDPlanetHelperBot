using System.IO;
using CCDPlanetHelper.Models;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Commands.Admins
{
    public class AddTagCommand:INucleusCommand
    {
        public string Command => "addtag";
        public string[] Aliases => new string[0];
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            msg.Text = msg.Text.Replace("addtag ", "");
            
            var words = msg.Text.Split("-");
            var carId = long.Parse(words[0]);
            var tags = words[1].Split(",");


            var tagsModel = JsonConvert.DeserializeObject<TagsCarModel>(File.ReadAllText("Tags.json"));

            foreach (var tag in tags)
            {
                tagsModel.Tags.Add(new TagModel() { CarId = carId, Tag = tag});
            }
            
            File.WriteAllText("Tags.json", JsonConvert.SerializeObject(tagsModel));
            
            sender.Text("✔ Теги добавлены", msg.ChatId);
        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}