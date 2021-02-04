using System;
using System.Collections.Generic;
using CCDPlanetHelper.Commands;
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
            botSettings.GroupId = 202349916;
            botSettings.Messenger = MessengerPlatform.Vkontakte;
            botSettings.VKToken = "e7980081cccad8d0df1ce342355da76e6e0d8de37509d76fb08ff1f065823dc19e5f4ed7f4578314cc772";

            var command = new Commands.UnknownCommand();

            var bot = new Bot(botSettings, command);
            
            bot.SetCommands(new StartCommand(), new ReminderCommand(), new CalcCommand(), new CourseCommand());
            bot.Start();
            Console.ReadLine();
        }
    }
}
