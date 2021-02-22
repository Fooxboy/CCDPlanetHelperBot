using System;
using System.Collections.Generic;
using System.Linq;
using CCDPlanetHelper.Database;
using Fooxboy.NucleusBot;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;

namespace CCDPlanetHelper.Commands
{
    public class CarInfoCommand:INucleusCommand
    {
        public string Command => "carinfo";
        public string[] Aliases => new string[0];
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            var carid = long.Parse(msg.Payload.Arguments[0]);

            using (var db = new BotData())
            {
                var car = db.Cars.SingleOrDefault(c => c.CarId == carid);

                var price1 = Convert.ToDecimal(car.Price);

                var price2 = Convert.ToInt64(Decimal.Multiply(price1, decimal.Parse("0,84")));
                var price3 = Convert.ToInt64(Decimal.Multiply(price1, decimal.Parse("0,7")));
                
                var text = $"🚘 Модель: {car.Model}" +
                           $"\n 💵 Цена с салона: {car.Price}" +
                           $"\n 💰 Цена с б/у: {price2}" +
                           $"\n 💳 Слив с б/у: {price3}" +
                           $"\n 💎 Цена за донат-валюту: {car.PriceDonate}" +
                           $"\n 🚗 Автосалон: {car.Showroom}" +
                           $"\n ⚙ Максимальная скорость: {car.MaxSpeed}" +
                           $"\n " +
                           $"\n 🔧 Комплекты тюнинга:";

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