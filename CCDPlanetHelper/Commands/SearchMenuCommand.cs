using Fooxboy.NucleusBot;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;

namespace CCDPlanetHelper.Commands
{
    public class SearchMenuCommand:INucleusCommand
    {
        public string Command => "searchmenu";
        public string[] Aliases => new string[0];
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {

            if (msg.ChatId != msg.MessageVK.FromId)
            {
                sender.Text("⛔ Эту команду нельзя вызывать в беседах. Чтобы узнать список всех команд - напишите \"команды\" ", msg.ChatId);
                return;
            }

            var kb = new KeyboardBuilder(bot);
            kb.AddButton("🚗 Каталог машин", "catalog");
            kb.AddLine();
            kb.AddButton("🔍 Поиск по названию", "search");
            kb.SetOneTime();
            kb.AddLine();
            kb.AddButton("🔙 В меню", "menu");
            
            sender.Text("🚗 Выберите необходимый пункт: ", msg.ChatId, kb.Build());


        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}