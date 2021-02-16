using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using CCDPlanetHelper.Database;
using CCDPlanetHelper.Services;
using Fooxboy.NucleusBot;
using Fooxboy.NucleusBot.Interfaces;
using Newtonsoft.Json;
using VkNet;
using VkNet.Model;
using Message = Fooxboy.NucleusBot.Models.Message;

namespace CCDPlanetHelper.Commands
{
    public class AdvertisementCommand:INucleusCommand
    {
        public string Command => "ads";
        public string[] Aliases => new[] {"Объявления", "объявление"};
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            var file = File.ReadAllText("AdminsConfig.json");
            var admins = JsonConvert.DeserializeObject<Models.AdminsModels>(file);

            bool isAdmin = admins.Users.Any(u => u == msg.MessageVK.FromId);

            var vkNet = new VkApi();
            vkNet.Authorize(new ApiAuthParams()
            {
                AccessToken = "e7980081cccad8d0df1ce342355da76e6e0d8de37509d76fb08ff1f065823dc19e5f4ed7f4578314cc772"
            });

            var ids = new List<long>();
            
            using (var db = new BotData())
            {
                string s = string.Empty;
                var stringText = string.Empty;
                
                if (db.Ads.Count() != 0)
                {
                    foreach (var ad in db.Ads)
                    {
                        var id = isAdmin ? $"| ID:{ad.AdId}" : "";
                        ids.Add(ad.Owner);
                        s += $"💎 -{ad.Owner}- сервер - {ad.Server} {id} : {ad.Text} \n";
                    }

                    var usrs = vkNet.Users.Get(ids);
                    
                    foreach (var usr in usrs)
                    { 
                        stringText = s.Replace($"-{usr.Id}-", $"[id{usr.Id}|{usr.FirstName} {usr.LastName}]");
                    }
                }
                else
                {
                    stringText += "Объявлений нет.";
                }




                if (msg.ChatId != msg.MessageVK.FromId)
                {
                    sender.Text($"🎫 Объявления: \n {stringText}", msg.ChatId);

                }
                else
                {
                    var kb = new KeyboardBuilder(bot);
                    kb.AddButton("1", "adsFilter", new List<string>() {"1"});
                    kb.AddButton("2", "adsFilter", new List<string>() {"2"});
                    kb.AddButton("3", "adsFilter", new List<string>() {"3"});
                    kb.AddLine();
                    kb.AddButton("4", "adsFilter", new List<string>() {"4"});
                    kb.AddButton("5", "adsFilter", new List<string>() {"5"});
                    kb.AddButton("6", "adsFilter", new List<string>() {"6"});
                    kb.AddLine();
                    kb.AddButton("🔙 В меню объявлений", "adsmenu");
                    sender.Text($"🎫 Объявления: \n {stringText}", msg.ChatId, kb.Build());

                }

            }
        }

        public void Init(IBot bot, ILoggerService logger)
        {
            var adsService = new RemoveAdsService();
            var thread = new Thread(() =>
            {
                adsService.Start(logger);
            });
            logger.Info($"Запуск потока Ads Service#{thread.ManagedThreadId}");
            thread.Start();

        }
    }
}