using System.Collections.Generic;
using System.IO;
using System.Linq;
using CCDPlanetHelper.Database;
using CCDPlanetHelper.Models;
using Fooxboy.NucleusBot;
using Fooxboy.NucleusBot.Interfaces;
using Newtonsoft.Json;
using VkNet;
using VkNet.Model;
using Message = Fooxboy.NucleusBot.Models.Message;

namespace CCDPlanetHelper.Commands
{
    public class AdsFilterCommand:INucleusCommand
    {
        public string Command => "adsFilter";
        public string[] Aliases => new string[0];
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
            
            var file = File.ReadAllText("AdminsConfig.json");
            var admins = JsonConvert.DeserializeObject<Models.AdminsModels>(file);

            bool isAdmin = admins.Users.Any(u => u == msg.MessageVK.FromId);

            

            int offset, server = 0;
            
            if (msg.Text.Split("-")[0] == "com")
            {
                offset = (int.Parse(msg.Text.Split("-")[1] )- 1) * 10 ;
                server = int.Parse(msg.Text.Split("-")[2]);
            }
            else
            {
                var argument = msg.Payload.Arguments[0];
                 offset = int.Parse(msg.Payload.Arguments[1]);
                 server = int.Parse(argument);
            }
            
            
            var vkNet = new VkApi();
            vkNet.Authorize(new ApiAuthParams()
            {
                AccessToken = "e7980081cccad8d0df1ce342355da76e6e0d8de37509d76fb08ff1f065823dc19e5f4ed7f4578314cc772"
            });

            var ids = new List<long>();
            var kb = new KeyboardBuilder(bot);

            using (var db = new BotData())
            {
                var ads = db.Ads.Where(a => a.Server == server);
                
                string s = string.Empty;
                var stringText = string.Empty;
                
                if (ads.Count() != 0)
                {
                    bool isAddNextCommand = true;
                    int counter = 0;
                    for (int i = offset; i < offset + 10; i++)
                    {
                        try
                        {
                            var ad = ads.OrderByDescending(a=> a.DateCreate).ToList()[i];
                            var id = $"| ID:{ad.AdId}";
                            ids.Add(ad.Owner);
                            stringText += $"💎[{i}] -{ad.Owner}- сервер - {ad.Server} {id} : {ad.Text} \n";
                            kb.AddButton(i.ToString(), "showad", new List<string>() {ad.AdId.ToString()});

                            if (counter == 4)
                            {
                                kb.AddLine();
                                counter = 0;
                            }
                        }
                        catch
                        {
                            isAddNextCommand = false;
                        }
                    }
                    
                    kb.AddLine();
                    kb.SetOneTime();

                    
                    if (offset > 0)
                    {
                        kb.AddButton("⏮ Назад", "adsFilter", new List<string>() {server.ToString(), $"{offset-10}"});
                    }
                   if(isAddNextCommand) kb.AddButton("⏭ Вперед", "adsFilter", new List<string>() {server.ToString(), $"{offset + 10}"});
                    

                    var usrs = vkNet.Users.Get(ids);
                    
                    foreach (var usr in usrs)
                    { 
                        stringText = stringText.Replace($"-{usr.Id}-", $"{usr.FirstName} {usr.LastName}");
                    }
                }
                else
                {
                    stringText += "Объявлений нет.";
                }

                kb.AddLine();
                kb.AddButton("🔙 Назад  в меню", "ads");
                
                if (msg.ChatId != msg.MessageVK.FromId)
                {
                    sender.Text($"🎫 Объявления: \n {stringText}", msg.ChatId);

                }
                else
                {
                    sender.Text($"🎫 Объявления: \n {stringText}", msg.ChatId, kb.Build());

                }

            }
        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}