using System.IO;
using System.Linq;
using CCDPlanetHelper.Database;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Commands.Admins
{
    public class AddTuningCommand:INucleusCommand
    {
        public string Command => "addtuning";
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

            var price = array[1];


            var name = string.Empty;

            for (int i = 2; i < array.Length; i++) name += array[i] + " ";

            long packId = 0;
            using (var db = new BotData())
            {
                var tuningPack = new TuningPack();
                tuningPack.Name = name;
                tuningPack.Price = long.Parse(price);
                tuningPack.PackId = db.TuningPacks.Count() + 1;
                packId = tuningPack.PackId;

                db.TuningPacks.Add(tuningPack);
                db.SaveChanges();
            }
            
            
            sender.Text($"✔ Новый тюнинг пак добавлен. ID: {packId} Чтобы добавить его к авто - напишите: tuningset <ID АВТО> <ID тюнинга>.", msg.ChatId);

        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}