using System.IO;
using System.Linq;
using CCDPlanetHelper.Database;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Commands.Admins
{
    public class TuningSetCommand:INucleusCommand
    {
        public string Command => "tuningset";
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

            var array = msg.Text.Split(" ");
            var idCar = long.Parse(array[1]);
            var idTuning = long.Parse(array[2]);


            using (var db = new BotData())
            {
                var car = db.Cars.SingleOrDefault(c => c.CarId == idCar);
                car.TuningPacks += $"{idTuning},";
                db.SaveChanges();
            }
            
            sender.Text("✔ Тюнинг добавлен в автомобиль.", msg.ChatId);
        }

        public void Init(IBot bot, ILoggerService logger)
        {
            
        }
    }
}