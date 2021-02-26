using System;
using System.IO;
using System.Linq;
using CCDPlanetHelper.Database;
using CCDPlanetHelper.Models;
using Fooxboy.NucleusBot;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Commands
{
    public class TargetCommand : INucleusCommand

    {
        public string Command => "target";
        public string[] Aliases => new[] {"цель", "цели"};
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            bool isChat = msg.ChatId != msg.MessageVK.FromId.Value;
            using (var db = new BotData())
            {
                var target = db.Targets.SingleOrDefault(t => t.UserId == msg.MessageVK.FromId);
                if (target is null)
                {
                    var kb1 = new KeyboardBuilder(bot);
                    kb1.AddButton("🔙 В меню", "menu");
                    kb1.SetOneTime();

                    if (isChat)
                    {
                        sender.Text("✈ Вы пока что не установили цель", msg.ChatId);

                    }else sender.Text("✈ Вы пока что не установили цель", msg.ChatId, kb1.Build());
                    return;
                }

                var car = db.Cars.SingleOrDefault(c => c.CarId == target.CarId);

                long priceRub = 0;

                long priceDoll = 0;
                long priceEuro = 0;
                
                var courseText = File.ReadAllText("CourceConfig.json");
                var course = JsonConvert.DeserializeObject<CourseModel>(courseText);

                if (car.Currency == 1) //руб
                {
                    priceRub = car.Price;
                    priceDoll = Convert.ToInt64(Convert.ToSingle(car.Price) / course.Dollar);
                    priceEuro = Convert.ToInt64(Convert.ToSingle(car.Price) / course.Euro);
                }else if (car.Currency == 2) //долл
                {
                    priceRub = Convert.ToInt64(course.Dollar * Convert.ToSingle(car.Price));
                    priceDoll = car.Price;
                    priceEuro = Convert.ToInt64(Convert.ToSingle(priceRub) / course.Euro);
                }else if (car.Currency == 3) //евро
                {
                    priceRub = Convert.ToInt64(course.Euro * Convert.ToSingle(car.Price));
                    priceEuro = car.Price;
                    priceDoll = Convert.ToInt64(Convert.ToSingle(priceRub) / course.Dollar);
                }

                var kb = new KeyboardBuilder(bot);
                kb.AddButton("➕ Изменить собранную сумму", "targetedit");
                kb.AddLine();
                kb.AddButton("🔙 В меню", "menu");
                kb.SetOneTime();

                var text = $"✈ Ваша цель: " +
                           $"\n 🚗 {car.Model}" +
                           $"\n 💎 Собрано {target.Count.ToString("N1")} из {priceRub.ToString("N1")} ₽ ({target.Count/(priceRub/100)}%)";
                
                if(isChat) sender.Text(text, msg.ChatId);
                else sender.Text(text, msg.ChatId, kb.Build());
                
            }
        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}