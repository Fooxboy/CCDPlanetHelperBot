using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;

namespace CCDPlanetHelper.Commands
{
    public class TestPhoto:INucleusCommand
    {
        public string Command => "testphoto";
        public string[] Aliases => new string[0];
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            sender.TextImage("bruh", msg.ChatId, "D:\\Pictures\\a.png");
        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}