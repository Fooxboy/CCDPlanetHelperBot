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
            sender.Text("Каталог в данный момент не работает.", msg.ChatId);
        }

        public void Init(IBot bot, ILoggerService logger)
        {
            
        }
    }
}