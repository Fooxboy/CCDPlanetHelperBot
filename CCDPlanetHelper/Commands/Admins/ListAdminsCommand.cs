using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fooxboy.NucleusBot;
using Fooxboy.NucleusBot.Interfaces;
using Newtonsoft.Json;
using VkNet;
using VkNet.Model;
using Message = Fooxboy.NucleusBot.Models.Message;

namespace CCDPlanetHelper.Commands.Admins
{
    public class ListAdminsCommand:INucleusCommand
    {
        public string Command => "adminlist";
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

            var adminsText = string.Empty;

            var vkNet = new VkApi();
            vkNet.Authorize(new ApiAuthParams()
            {
                AccessToken = "e7980081cccad8d0df1ce342355da76e6e0d8de37509d76fb08ff1f065823dc19e5f4ed7f4578314cc772"
            });
            
            var names = vkNet.Users.Get(admins.Users);
            
            foreach (var admin in names)
            {
                
                adminsText += $"[id{admin.Id}|{admin.FirstName} {admin.LastName}], ";
            }

            sender.Text($"✔ Администраторы: {adminsText}", msg.ChatId);
        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}