using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;

namespace CCDPlanetHelper.Commands
{
    public class StartCommand:INucleusCommand
    {
        public string Command => "start";
        public string[] Aliases => new [] {"старт", "начать", "привет"};
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            sender.Text("Вы нажали на кнопку старт!", msg.ChatId);
        }

        public void Init(IBot bot, ILoggerService logger)
        {
            
        }
    }
}