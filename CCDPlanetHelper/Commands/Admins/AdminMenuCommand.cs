using System.IO;
using System.Linq;
using Fooxboy.NucleusBot;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Commands.Admins
{
    public class AdminMenuCommand:INucleusCommand
    {
        public string Command => "adminmenu";
        public string[] Aliases => new [] {"админка"};
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            if (msg.ChatId != msg.MessageVK.FromId)
            {
                sender.Text("⛔ Эту команду нельзя вызывать в беседах. Чтобы узнать список всех команд - напишите \"команды\" ", msg.ChatId);
                return;
            }
            var file = File.ReadAllText("AdminsConfig.json");
            var admins = JsonConvert.DeserializeObject<Models.AdminsModels>(file);

            if (!(admins.Users.Any(u => u == msg.MessageVK.FromId)))
            {
                sender.Text("🛑 У вас нет прав администратора для вызова этой команды.", msg.ChatId);
                return;
            }

            var kb = new KeyboardBuilder(bot);
            kb.AddButton("Репорты", "reportlist");
            kb.AddLine();
            kb.AddButton("Список администраторов", "adminlist");
            kb.AddLine();
            kb.AddButton("🔙 В меню", "menu");
            kb.SetOneTime();
        }

        public void Init(IBot bot, ILoggerService logger)
        {
           
        }
    }
}