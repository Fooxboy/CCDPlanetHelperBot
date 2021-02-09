using System.IO;
using System.Linq;
using CCDPlanetHelper.Database;
using Fooxboy.NucleusBot.Interfaces;
using Newtonsoft.Json;
using VkNet.Model;
using Message = Fooxboy.NucleusBot.Models.Message;

namespace CCDPlanetHelper.Commands.Admins
{
    public class ReportListCommand:INucleusCommand
    {
        public string Command => "reportlist";
        public string[] Aliases => new[] {"репорты", "репортысписок"};
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            var file = File.ReadAllText("AdminsConfig.json");
            var admins = JsonConvert.DeserializeObject<Models.AdminsModels>(file);

            if (!(admins.Users.Any(u => u == msg.MessageVK.FromId)))
            {
                sender.Text("🛑 У вас нет прав администратора для вызова этой команды.", msg.ChatId);
                return;
            }

            using (var db = new BotData())
            {
                string reports = string.Empty;

                var repostsDb = db.Reports.Where(r => r.IsAnswered == false);

                if (repostsDb.Count() != 0)
                {
                    foreach (var report in repostsDb)
                    {
                        reports += $"ID: {report.ReportId} | Написал: [id{report.OwnerId}| это человек]\n Текст: {report.Message} \n";
                    }
                }
                else
                {
                    reports = "Репортов нет.";
                }
                
                
                sender.Text($"Неотвеченные репорты: \n {reports}", msg.ChatId);
            }
            
        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}