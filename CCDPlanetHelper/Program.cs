using System;
using System.Collections.Generic;
using Fooxboy.NucleusBot;
using Fooxboy.NucleusBot.Enums;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Fooxboy.NucleusBot.Services;

namespace CCDPlanetHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            var botSettings = new BotSettings();
            botSettings.GroupId = 0;
            botSettings.Messenger = MessengerPlatform.Vkontakte;
            botSettings.VKToken = "null";

            var command = new Commands.UnknownCommand();

            var bot = new Bot(botSettings, command);
        }
    }
}
