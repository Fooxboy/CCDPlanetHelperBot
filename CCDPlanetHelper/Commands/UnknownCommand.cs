using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;

namespace CCDPlanetHelper.Commands
{
    public class UnknownCommand :INucleusCommand
    {
        public string Command => "inknown";
        public string[] Aliases => new string[0];
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            if (StaticContent.UsersCommand.Any(u => u.Key == msg.MessageVK.FromId))
            {
                var command = StaticContent.UsersCommand.SingleOrDefault(u => u.Key == msg.MessageVK.FromId);
                if (command.Value == "addAdsPartTwo")
                {
                    AdvertisementAddCommand.AddPartTwo(msg.MessageVK.FromId.Value, msg.ChatId, sender, msg.Text, bot);
                }

                StaticContent.UsersCommand.Remove(msg.MessageVK.FromId.Value);
            }
            else
            {
                sender.Text("⛔ Неизвестная команда", msg.ChatId);
            }
        }

        public void Init(IBot bot, ILoggerService logger)
        {
            //throw new NotImplementedException();
        }
    }
}
