using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class CarInfoCommand:INucleusCommand
    {
        public string Command => "carinfo";
        public string[] Aliases => new string[0];
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            var carid = long.Parse(msg.Payload.Arguments[0]);
            
            var file = File.ReadAllText("AdminsConfig.json");
            var admins = JsonConvert.DeserializeObject<Models.AdminsModels>(file);

            bool isAdmin = admins.Users.Any(u => u == msg.MessageVK.FromId);

            using (var db = new BotData())
            {
                var car = db.Cars.SingleOrDefault(c => c.CarId == carid);

                


                var adminText = string.Empty;

                if (isAdmin) adminText = $"\n ⚙ ID автомобиля: {carid}";


                var tuningText = string.Empty;

                var tunings = car.TuningPacks.Split(",");
                var ids = new List<long>();
                foreach (var tuning in tunings)
                {
                    if (tuning != "")
                    {
                        ids.Add(long.Parse(tuning));
                    }
                }

                var tuningsT = new List<TuningPack>();

                foreach (var id in ids)
                {
                    var t = db.TuningPacks.SingleOrDefault(t => t.PackId == id);
                    tuningsT.Add(t);
                }


                foreach (var pack in tuningsT)
                {
                    tuningText += $"🔶 {pack.Name} - {pack.Price.ToString("N1")} ₽ \n";
                }

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

                var price1 = Convert.ToDecimal(priceRub);

                var price2 = Convert.ToInt64(Decimal.Multiply(price1, decimal.Parse("0,84")));
                var price3 = Convert.ToInt64(Decimal.Multiply(price1, decimal.Parse("0,7")));

                var text = $"🚘 Модель: {car.Model}" +
                           $"\n 💵 Цена с салона: {priceRub.ToString("N1")}₽ (${priceDoll.ToString("N1")} или {priceEuro.ToString("N1")}€)" +
                           $"\n 💰 Цена с б/у: {price2.ToString("N1")}₽" +
                           $"\n 💳 Слив с б/у: {price3.ToString("N1")}₽" +
                           $"\n 💎 Цена за донат-валюту: {car.PriceDonate.ToString("N1")}" +
                           $"\n 🚗 Автосалон: {car.Showroom}" +
                           $"\n ⚙ Максимальная скорость: {car.MaxSpeed}" +
                           $"{adminText}" +
                           $"\n " +
                           $"\n 🔧 Комплекты тюнинга:" +
                           $"\n {tuningText}";

                var kb = new KeyboardBuilder(bot);
                kb.AddButton("✔ Добавить в цель", "addtarget", new List<string>() {car.CarId.ToString()});
                kb.AddLine();
                kb.AddButton("🔙 В меню", "menu");
                kb.SetOneTime();
                sender.TextImage(text, msg.ChatId, car.Image, kb.Build());
            }
            
            
        }
        

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}