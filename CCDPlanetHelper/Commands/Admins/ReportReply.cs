using System.IO;
using System.Linq;
using CCDPlanetHelper.Database;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Commands.Admins
{
    public class ReportReply:INucleusCommand
    {
        public string Command => "reportrp";
        public string[] Aliases => new[] {"ответитьрп ", "рпответ"};
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            var file = File.ReadAllText("AdminsConfig.json");
            var admins = JsonConvert.DeserializeObject<Models.AdminsModels>(file);

            if (!(admins.Users.Any(u => u == msg.MessageVK.FromId)))
            {
                sender.Text("🛑 У вас нет прав администратора для вызова этой команды.", msg.ChatId);
                return;
            }
            
            var words = msg.Text.Split(" ");
            var message = string.Empty;
            var reportId = long.Parse(words[1]);

            for (int i = 2; i < words.Length; i++)
            {
                message += words[i] + " ";
            }

            using (var db = new BotData())
            {
                var report = db.Reports.SingleOrDefault(r => r.ReportId == reportId);

                if (report is null)
                {
                    sender.Text("⛔ Репорта с таким ID не найдено", msg.ChatId);
                    return;
                }

                report.IsAnswered = true;
                report.ModeratorId = msg.ChatId;
                report.ModeratorReply = message;

                db.SaveChanges();
                
                sender.Text($"📤 Ответ на ваш репорт с ID: {reportId}: \n {message}", report.OwnerId);

            }


            sender.Text("✔ Ответ на репорт отправлен", msg.ChatId);
            
        }

        public void Init(IBot bot, ILoggerService logger)
        {
            
        }
    }
}