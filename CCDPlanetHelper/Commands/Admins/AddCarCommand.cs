using System;
using System.IO;
using System.Linq;
using CCDPlanetHelper.Database;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Commands.Admins
{
    public class AddCarCommand:INucleusCommand
    {
        public string Command => "addcar";
        public string[] Aliases => new[] {"добавитьмашину"};
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            var file = File.ReadAllText("AdminsConfig.json");
            var admins = JsonConvert.DeserializeObject<Models.AdminsModels>(file);

            if (!(admins.Users.Any(u => u == msg.MessageVK.FromId)))
            {
                sender.Text("🛑 У вас нет прав администратора для вызова этой команды.", msg.ChatId);
                return;
            }

            if (msg.MessageVK.FromId != msg.ChatId)
            {
                sender.Text("⛔ Эта команда недоступна в беседах", msg.ChatId);
                return;
            }

            var words = msg.Text.Split(" ");
            var model = string.Empty;

            for (int i = 1; i < words.Length; i++)
            {
                model += words[i] + " ";
            }

            if (model == string.Empty)
            {
                sender.Text("Вы не указали модель", msg.ChatId);
                return;
            }

            using (var db = new BotData())
            {
                var id = new Random().Next(1, 999999999);
                db.Cars.Add(new CarInfo()
                {
                    CarId = id,
                    Image = "photo",
                    IsPublic = false,
                    MaxSpeed = 0,
                    Model = model,
                    Price = 0,
                    PriceDonate = 0,
                    TuningPacks = ""
                });

                db.SaveChanges();
                StaticContent.AddCarInfo.Add(msg.ChatId, id);
            }
            
            StaticContent.UsersCommand.Add(msg.ChatId, "addcarinfo");
            sender.Text("✔ Машина зарезерирована. Теперь укажите цену, цену за донат валюту, и максимальную скорость. В таком формате: \n" +
                        "<Цена с салона> <Цена за донат валюту> <Максимальная скорость> \n Например: 100 5 200", msg.ChatId);
            
        }


        public static void AddCarInfo(IMessageSenderService service, long owner, string msg)
        {
            var words = msg.Split(" ");
            long price = long.Parse(words[0]);
            long donatePrice = long.Parse(words[1]);
            long speed = long.Parse(words[2]);

            var carId = StaticContent.AddCarInfo.SingleOrDefault(i => i.Key == owner);
            
            using (var db = new BotData())
            {
                var car = db.Cars.SingleOrDefault(c => c.CarId == carId.Value);
                car.Price = price;
                car.PriceDonate = donatePrice; 
                car.MaxSpeed = speed;
                car.IsPublic = true;

                db.SaveChanges();
            }

            StaticContent.AddCarInfo.Remove(owner);
            
            service.Text("✔ Автомобиль успешно добавлен", owner);
        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}