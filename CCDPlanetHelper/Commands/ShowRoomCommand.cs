using System.Collections.Generic;
using System.IO;
using System.Linq;
using CCDPlanetHelper.Database;
using CCDPlanetHelper.Models;
using Fooxboy.NucleusBot;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Commands
{
    public class ShowRoomCommand:INucleusCommand
    {
        public string Command => "showroom";
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
            
            var showroom = int.Parse(msg.Payload.Arguments[0]);

            var offset = int.Parse(msg.Payload.Arguments[1]);
            string text = "🚗 Автомобили в этом автосалоне: \n";
            var kb = new KeyboardBuilder(bot);

            using (var db = new BotData())
            {
                var autos = db.Cars.Where(c => c.Showroom == showroom).ToList();
                if (autos.Count() == 0)
                {
                    text = "🔍 Автомобилей в этом автосалоне пока что нет.";
                    
                }
                else
                {
                    int counter = 0;
                    bool isEnd = false;
                    for (int i = offset; i < offset + 6; i++)
                    {
                        try
                        {
                            var car = autos[i];
                            kb.AddButton($"{i}", "carinfo", new List<string>() {$"{car.CarId}"});
                            text += $"🚘 [{i}] - {car.Model}\n";
                            counter++;
                            if (counter == 3)
                            {
                                kb.AddLine();
                            }

                        }
                        catch
                        {
                            isEnd = true;
                        }
                    }

                    if (!isEnd)
                    {
                        kb.AddLine();
                        kb.AddButton("⏭ Следующая страница", "showroom", new List<string>() {$"{showroom}", $"{offset + 6}"});
                    }
                }
                
            }

            kb.AddLine();
            kb.AddButton("🔙 К автосалонам", "catalog");
            
            sender.Text(text, msg.ChatId, kb.Build());
            
        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}