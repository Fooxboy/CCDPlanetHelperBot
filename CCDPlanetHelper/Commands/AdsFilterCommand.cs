using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;

namespace CCDPlanetHelper.Commands
{
    public class AdsFilterCommand:INucleusCommand
    {
        public string Command => "adsFilter";
        public string[] Aliases => new string[0];
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            
        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}