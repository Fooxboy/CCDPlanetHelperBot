using System;
using System.Collections.Generic;
using System.Linq;
using CCDPlanetHelper.Database;
using Fooxboy.NucleusBot;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using VkNet.Enums.SafetyEnums;

namespace CCDPlanetHelper.Commands
{
    public class AdvertisementAddCommand:INucleusCommand
    {
        public string Command => "AddAds";
        public string[] Aliases => new[] {"Добавить", "addads", "adsadd"};
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            if (msg.Text.Split(" ")[1].ToLower() != "объявление")
            {
                return;
            }

            using (var db = new BotData())
            {
                var adsFromUser = db.Ads.Where(ad => ad.Owner == msg.MessageVK.FromId);
                if (adsFromUser.Count() == 3)
                {
                    sender.Text("⛔ Вы уже создали 3 объявления.", msg.ChatId);
                    return;
                }
            }

            var kb = new KeyboardBuilder(bot);
            kb.AddButton("1", "excecuteCommand", new List<string>(){"adsAdd", "1"});
            kb.AddButton("2", "excecuteCommand", new List<string>(){"adsAdd", "2"});
            kb.AddButton("3", "excecuteCommand", new List<string>(){"adsAdd", "3"});
            kb.AddLine();
            kb.AddButton("4", "excecuteCommand", new List<string>(){"adsAdd", "4"});
            kb.AddButton("5", "excecuteCommand", new List<string>(){"adsAdd", "5"});
            kb.AddButton("6", "excecuteCommand", new List<string>(){"adsAdd", "6"});
            kb.SetOneTime();

            sender.Text("✔ Выберите сервер", msg.ChatId, kb.Build());
        }

        public static void AddPartOne(long userId, long chatId, int server, IMessageSenderService sender)
        {
            StaticContent.UsersCommand.Add(userId, "addAdsPartTwo");
            StaticContent.SelectUserServer.Add(userId, server);
            sender.Text("Напишите текст объявления. Не больше 300х символов.", chatId);
        }

        public static void AddPartTwo(long userId, long chatId, IMessageSenderService sender, string text, IBot bot)
        {
            if (text.Length > 300)
            {
                var kb = new KeyboardBuilder(bot);
                kb.AddButton("Попробовать ещё раз", "AddAds", color: KeyboardButtonColor.Positive);
                kb.SetOneTime();
                sender.Text($"⛔ Объявление не может быть больше 300х символов. В Вашем объявлении {text.Length} символов.", chatId, kb.Build());
            }
            
            using (var db = new BotData())
            {
                var server = StaticContent.SelectUserServer.SingleOrDefault(u => u.Value == userId);
                db.Ads.Add(new Ad()
                {
                    AdId =  db.Ads.Count() + 1,
                    Owner = userId,
                    DateCreate =  3,
                    Server =  server.Value,
                    Text = text
                });

                db.SaveChanges();

                StaticContent.SelectUserServer.Remove(userId);
            }
            
            sender.Text("✔ Ваше объявление добавлено.", chatId);
        }

        public void Init(IBot bot, ILoggerService logger)
        {
           
        }
    }
}