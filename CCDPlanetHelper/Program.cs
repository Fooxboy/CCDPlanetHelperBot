using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CCDPlanetHelper.Commands;
using CCDPlanetHelper.Commands.Admins;
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
            Console.WriteLine("Запуск бота...");
            
            var botSettings = new BotSettings();
            botSettings.GroupId = 202349916;
            botSettings.Messenger = MessengerPlatform.Vkontakte;
            botSettings.VKToken = "e7980081cccad8d0df1ce342355da76e6e0d8de37509d76fb08ff1f065823dc19e5f4ed7f4578314cc772";

            var command = new Commands.UnknownCommand();

            var bot = new Bot(botSettings, command);
            
            bot.SetCommands(new StartCommand(), new ReminderCommand(), new CalcCommand(), 
                new CourseCommand(), new CourceSetCommand(), new AddAdminCommand(),
                new RemoveAdminCommand(), new ListAdminsCommand(), new AdvertisementCommand(), 
                new ExcecuteCommand(), new AdvertisementAddCommand(), new RemoveAdsCommand(),
                new MailingCommand(), new UnsubscribeCommand(), new SubscribeCommand(),
                new ReportCommand(), new ReportReply(), new ReportListCommand(), 
                new AddCarCommand(), new CarListCommand(), new RemoveCarCommand(),
                new HelpCommand(), new ChangelogCommand(), new ChangelogEditCommand(), 
                new BindsCommand(), new BindsSetCommand() , new MenuCommand(), new SettingsCommand());
            

            var logger = bot.GetLogger();
            logger.Trace("Инициализация статик контента...");
            StaticContent.UsersCommand = new Dictionary<long, string>();
            StaticContent.SelectUserServer = new Dictionary<long, int>();
            StaticContent.AddCarInfo = new Dictionary<long, long>();
            bot.Start();
            Console.ReadLine();
        }
    }
}
