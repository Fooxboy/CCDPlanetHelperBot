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
    public class SearchCommand:INucleusCommand
    {
        public string Command => "search";
        public string[] Aliases => new[] {"поиск", "найти"};
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
            
            var kb = new KeyboardBuilder(bot);
            kb.AddButton("🔙 Назад", "searchmenu");
            
            StaticContent.UsersCommand.Add(msg.ChatId, "search");
            sender.Text("🔍 Напишите название автомобиля", msg.ChatId, kb.Build());
        }

        public static void Search(Message msg, IMessageSenderService sender, IBot bot)
        {
            StaticContent.UsersCommand.Remove(msg.MessageVK.FromId.Value);
            var search = msg.Text;

            using (var db = new BotData())
            {
                var cars = db.Cars.Where(c => c.Model.ToLower() == search.ToLower());

                if (!cars.Any())
                {
                    StaticContent.UsersCommand.Add(msg.ChatId, "search");
                    var kb = new KeyboardBuilder(bot);
                    kb.AddButton("🔙 Назад", "searchmenu");
                    sender.Text("🔍 Автомобиль не найден. Попробуйте ещё раз", msg.ChatId, kb.Build());
                    return;
                }

                var car = cars.ToList()[0];

                var kb1 = new KeyboardBuilder(bot);
                kb1.AddButton("🚗 Открыть информацию", "carinfo", new List<string>() {car.CarId.ToString()});
                kb1.AddLine();
                kb1.AddButton("🔙 Назад", "searchmenu");
                kb1.AddLine();
                kb1.AddButton("🔍 Найти другой", "search");
                
                sender.Text($"🚗 {car.Model} найден! Нажмите на кнопку ниже, чтобы открыть информацию об авто.", msg.ChatId, kb1.Build());

            }
        }

        public void Init(IBot bot, ILoggerService logger)
        {
            
        }
    }
}