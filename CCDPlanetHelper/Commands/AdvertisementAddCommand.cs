using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CCDPlanetHelper.Database;
using CCDPlanetHelper.Models;
using Fooxboy.NucleusBot;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;
using VkNet.Enums.SafetyEnums;

namespace CCDPlanetHelper.Commands
{
    public class AdvertisementAddCommand:INucleusCommand
    {
        public string Command => "AddAds";
        public string[] Aliases => new[] {"Добавить", "addads", "adsadd"};
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            //проверка и подписка на рассылку, если пользователь пользуется ботом первый раз.
            var usrs1 = JsonConvert.DeserializeObject<MailingModel>(File.ReadAllText("MailingUsers.json"));
            if (usrs1.Users.All(u => u.UserId != msg.MessageVK.FromId.Value))
            {
                usrs1.Users.Add(new ValuesMail()
                {
                    IsActive = true,
                    UserId = msg.MessageVK.FromId.Value
                });
                
                File.WriteAllText("MailingUsers.json", JsonConvert.SerializeObject(usrs1));
            }
            
            if (msg.Text.Split(" ")[1].ToLower() != "объявление" && msg.Payload == null)
            {
                return;
            }


            if (msg.ChatId != msg.MessageVK.FromId)
            {
                sender.Text("⛔ Эта команда недоступна в беседе", msg.ChatId);
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
            try
            {
                StaticContent.UsersCommand.Add(userId, "addAdsPartTwo");

            }
            catch
            {
                StaticContent.UsersCommand.Remove(userId);
                StaticContent.UsersCommand.Add(userId, "addAdsPartTwo");

            }
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
                return;
            }
            
            using (var db = new BotData())
            {
                var server = StaticContent.SelectUserServer.SingleOrDefault(u => u.Key == userId);
                db.Ads.Add(new Ad()
                {
                    AdId =  new Random().Next(0, 99999999),
                    Owner = userId,
                    DateCreate = Convert.ToInt64(DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds),
                    Server =  server.Value,
                    Time = 3,
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