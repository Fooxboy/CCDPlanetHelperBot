using System.IO;
using System.Linq;
using CCDPlanetHelper.Models;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Commands.Admins
{
    public class ChangelogEditCommand : INucleusCommand

    {
        public string Command => "changelogedit";
        public string[] Aliases => new string[0];
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            var file = File.ReadAllText("AdminsConfig.json");
            var admins = JsonConvert.DeserializeObject<Models.AdminsModels>(file);

            if (!(admins.Users.Any(u => u == msg.MessageVK.FromId)))
            {
                sender.Text("🛑 У вас нет прав администратора для вызова этой команды.", msg.ChatId);
                return;
            }
            
            var text = string.Empty;
            var words = msg.Text.Split(" ");

            for (int i = 1; i < words.Length; i++)
            {
                text += words[i] + " ";
            }

            var model = new ChangelogModel();
            model.Text = text;

            File.WriteAllText("Changelog.json", JsonConvert.SerializeObject(model));
            
            sender.Text("✔ Новый changelog установлен", msg.ChatId);
        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}