using System.Collections.Generic;
using Fooxboy.NucleusBot;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;

namespace CCDPlanetHelper.Commands
{
    public class CatalogCommand:INucleusCommand
    {
        public string Command => "catalog";
        public string[] Aliases => new[] {"каталог"};
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {

            var kb = new KeyboardBuilder(bot);
            kb.AddButton("1", "showroom", new List<string>() {"1", "0"});

            kb.AddButton("2", "showroom", new List<string>() {"2", "0"});
            kb.AddButton("3", "showroom", new List<string>() {"3", "0"});
            kb.AddLine();
            kb.AddButton("4", "showroom", new List<string>() {"4", "0"});
            kb.AddButton("5", "showroom", new List<string>() {"5", "0"});
            kb.AddButton("6", "showroom", new List<string>() {"6", "0"});
            kb.AddButton("7", "showroom", new List<string>() {"7", "0"});
            kb.AddLine();
            kb.AddButton("🔙 Назад", "searchmenu");
            kb.SetOneTime();

            sender.Text("Выберите необходимый автосалон.", msg.ChatId, kb.Build());
        }

        public void Init(IBot bot, ILoggerService logger)
        {
            
        }
    }
}