using System;
using System.IO;
using System.Linq;
using System.Net;
using CCDPlanetHelper.Database;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;
using VkNet.Model.Attachments;

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
                sender.Text("⛔ Вы не указали модель", msg.ChatId);
                return;
            }

            if (model[^1] == ' ')
            {
                model = model.Remove(model.Length - 1);
            } 

            if (msg.MessageVK.Attachments.Count() == 0)
            {
                sender.Text("⛔ Вы не прикрепили фотографию авто", msg.ChatId);
                return;
            }

            var photo = msg.MessageVK.Attachments[0].Instance as Photo;
            if (photo is null)
            {
                sender.Text("⛔ Вы не прикрепили фотографию авто", msg.ChatId);
                return;
            }

            var link = photo.Sizes[^1].Url;
            if (!Directory.Exists("Cars Photos"))
            {
                Directory.CreateDirectory("Cars Photos");
            }

            var client = new WebClient();
            var id = new Random().Next(1, 999999999);
            client.DownloadFile(link, $"Cars Photos\\{id}.jpg");

            using (var db = new BotData())
            {
                
                db.Cars.Add(new CarInfo()
                {
                    CarId = id,
                    Image = $"Cars Photos\\{id}.jpg",
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
            sender.Text("✔ Машина зарезерирована. Теперь укажите цену, цену за донат валюту, максимальную скорость и номер автосалона. В таком формате: \n" +
                        "<Цена с салона>-<Валюта> <Цена за донат валюту> <Максимальная скорость> <автосалон> \n Например: 100-р 5 200 1", msg.ChatId);
            
        }


        public static void AddCarInfo(IMessageSenderService service, long owner, string msg)
        {
            var words = msg.Split(" ");
            string priceText = words[0];
            long price = long.Parse(priceText.Split("-")[0]);
            int currency = 0;
            string ctx = (priceText.Split("-")[1]).ToLower();
            
            //обработка валюты.

            if (ctx == "р")
            {
                currency = 1;
            }else if (ctx == "д")
            {
                currency = 2;
            }else if (ctx == "е")
            {
                currency = 3;
            }
            
            long donatePrice = long.Parse(words[1]);
            long speed = long.Parse(words[2]);
            int showroom = int.Parse(words[3]);

            var carId = StaticContent.AddCarInfo.SingleOrDefault(i => i.Key == owner);
            
            using (var db = new BotData())
            {
                var car = db.Cars.SingleOrDefault(c => c.CarId == carId.Value);
                car.Price = price;
                car.Currency = currency;
                car.PriceDonate = donatePrice; 
                car.MaxSpeed = speed;
                car.IsPublic = true;
                car.Showroom = showroom;

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