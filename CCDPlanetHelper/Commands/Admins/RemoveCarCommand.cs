using System.IO;
using System.Linq;
using CCDPlanetHelper.Database;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Commands.Admins
{
    public class RemoveCarCommand:INucleusCommand
    {
        public string Command => "removecar";
        public string[] Aliases => new[] {"carremove", "удалитьавто"};
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

            var carId = long.Parse(msg.Text.Split(" ")[1]);

            using (var db = new BotData())
            {
                var car = db.Cars.SingleOrDefault(c => c.CarId == carId);
                if (car != null)
                {
                    db.Cars.Remove(car);
                }

                else
                {
                    sender.Text("⛔ Авто с таким ID не найдено.", msg.ChatId);
                    return;
                }

                db.SaveChanges();
            }
            
            sender.Text("✔ Машина была удалена из базы данынх", msg.ChatId);
        }

        public void Init(IBot bot, ILoggerService logger)
        {
            
        }
    }
}