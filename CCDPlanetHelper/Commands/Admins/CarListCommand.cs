using System.IO;
using System.Linq;
using CCDPlanetHelper.Database;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Commands.Admins
{
    public class CarListCommand:INucleusCommand
    {
        public string Command => "carlist";
        public string[] Aliases => new string[0];
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            var file = File.ReadAllText("AdminsConfig.json");
            var admins = JsonConvert.DeserializeObject<Models.AdminsModels>(file);

            if (!(admins.Users.Any(u => u == msg.MessageVK.FromId)))
            {
                sender.Text("🛑 У вас нет прав администратора для вызова этой команды.", msg.ChatId);
                return;
            }

            var carsText = string.Empty;
            using (var db = new BotData())
            {
                foreach (var car in db.Cars)
                {
                    carsText +=
                        $"ID: {car.CarId}, {car.Model},Price:{car.Price}, speed:{car.MaxSpeed}, priceDonate:{car.PriceDonate},tuning: {car.TuningPacks}\n";
                }
            }
            
            sender.Text($"cars: {carsText}", msg.ChatId);

        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}