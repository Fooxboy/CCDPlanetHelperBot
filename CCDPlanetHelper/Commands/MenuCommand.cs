using System.IO;
using System.Linq;
using CCDPlanetHelper.Models;
using Fooxboy.NucleusBot;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;
using VkNet.Enums.SafetyEnums;

namespace CCDPlanetHelper.Commands
{
    public class MenuCommand:INucleusCommand
    {
        public string Command => "menu";
        public string[] Aliases => new[] {"меню"};
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

            if (msg.ChatId != msg.MessageVK.FromId)
            {
                sender.Text("⛔ Эту команду нельзя вызывать в беседах. Чтобы узнать список всех команд - напишите \"команды\" ", msg.ChatId);
                return;
            }
            
            var file = File.ReadAllText("AdminsConfig.json");
            var admins = JsonConvert.DeserializeObject<Models.AdminsModels>(file);

            var isAdmin = admins.Users.Any(u => u == msg.MessageVK.FromId);
            
            
            var kb = new KeyboardBuilder(bot);
            kb.AddButton("🔍 Поиск машин", "searchMenu", color: KeyboardButtonColor.Primary);
            kb.AddLine();
            kb.AddButton("📮 Объявления", "adsmenu");
            kb.AddButton("🎞 Changelog", "changelog");
            if (isAdmin) kb.AddButton("🤴 Админ-панель", "adminmenu");
            kb.AddLine();
            kb.AddButton("⚙ Настройки", "settings");
            kb.SetOneTime();

            sender.Text("🎈 Главное меню: ", msg.ChatId, kb.Build());
        }

        public void Init(IBot bot, ILoggerService logger)
        {
            
        }
    }
}