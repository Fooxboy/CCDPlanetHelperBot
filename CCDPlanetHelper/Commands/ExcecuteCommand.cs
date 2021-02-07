using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;

namespace CCDPlanetHelper.Commands
{
    public class ExcecuteCommand:INucleusCommand
    {
        public string Command => "excecuteCommand";
        public string[] Aliases => new[] {"выполнить"};
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            if (msg.Payload.Arguments[0] == "adsAdd")
            {
                AdvertisementAddCommand.AddPartOne(msg.MessageVK.FromId.Value, msg.ChatId, int.Parse(msg.Payload.Arguments[1]), sender);
            }
        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}