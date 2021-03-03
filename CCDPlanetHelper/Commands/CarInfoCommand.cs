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

                string priceStr = "";
                
                var courseText = File.ReadAllText("CourseConfig.json");
                var course = JsonConvert.DeserializeObject<CourseModel>(courseText);

                if (car.Currency == 1) //руб
                {
                    priceRub = car.Price;
                    priceDoll = Convert.ToInt64(Convert.ToSingle(car.Price) / course.Dollar);
                    priceEuro = Convert.ToInt64(Convert.ToSingle(car.Price) / course.Euro);
                    priceStr = $"{priceRub.ToString("N1").Split(",")[0]}₽";
                }else if (car.Currency == 2) //долл
                {
                    priceRub = Convert.ToInt64(course.Dollar * Convert.ToSingle(car.Price));
                    priceDoll = car.Price;
                    priceEuro = Convert.ToInt64(Convert.ToSingle(priceRub) / course.Euro);
                    priceStr = $"{priceRub.ToString("N1").Split(",")[0]}₽ (${priceDoll.ToString("N1").Split(",")[0]})";
                }else if (car.Currency == 3) //евро
                {
                    priceRub = Convert.ToInt64(course.Euro * Convert.ToSingle(car.Price));
                    priceEuro = car.Price;
                    priceDoll = Convert.ToInt64(Convert.ToSingle(priceRub) / course.Dollar);
                    priceStr = $"{priceRub.ToString("N1").Split(",")[0]}₽ ({priceEuro.ToString("N1").Split(",")[0]}€)";
                }

                var price1 = Convert.ToDecimal(priceRub);

                var price2 = Convert.ToInt64(Decimal.Multiply(price1, decimal.Parse("0,84")));
                var price3 = Convert.ToInt64(Decimal.Multiply(price1, decimal.Parse("0,7")));


                var showroomStr = "";

                if (car.Showroom == 1)
                {
                    showroomStr = "Европа";
                }else if (car.Showroom == 2)
                {
                    showroomStr = "Япония";
                }else if (car.Showroom == 3)
                {
                    showroomStr = "Toyota";
                }else if (car.Showroom == 4)
                {
                    showroomStr = "Mercedes-Benz";
                }else if (car.Showroom == 5)
                {
                    showroomStr = "BMW";
                }else if (car.Showroom == 6)
                {
                    showroomStr = "Лада";
                }else if (car.Showroom == 7)
                {
                    showroomStr = "Яхты";
                }else if (car.Showroom == 8)
                {
                    showroomStr = "Вертолеты";
                }else if (car.Showroom == 9)
                {
                    showroomStr = "Америка";
                }else if (car.Showroom == 10)
                {
                    showroomStr = "Коммерческий";
                }

                var text = $"🚘 Модель: {car.Model}" +
                           $"\n 💵 Цена с салона: {priceStr}" +
                           $"\n 💰 Цена с б/у: {price2.ToString("N1").Split(",")[0]}₽" +
                           $"\n 💳 Слив с б/у: {price3.ToString("N1").Split(",")[0]}₽" +
                           $"\n 💎 Цена за донат-валюту: {car.PriceDonate.ToString("N1").Split(",")[0]}" +
                           $"\n 🚗 Автосалон: {showroomStr}" +
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