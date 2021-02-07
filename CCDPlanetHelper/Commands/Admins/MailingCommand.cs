using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CCDPlanetHelper.Models;
using Fooxboy.NucleusBot;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Commands.Admins
{
    public class MailingCommand:INucleusCommand
    {
        public string Command => "mailing";
        public string[] Aliases => new[] {"рассылка"};
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            var file = File.ReadAllText("AdminsConfig.json");
            var admins = JsonConvert.DeserializeObject<Models.AdminsModels>(file);

            if (!(admins.Users.Any(u => u == msg.MessageVK.FromId)))
            {
                sender.Text("🛑 У вас нет прав администратора для вызова этой команды.", msg.ChatId);
                return;
            }

            var users = JsonConvert.DeserializeObject<MailingModel>(File.ReadAllText("MailingUsers.json")).Users;

            var text = string.Empty;
            var words = msg.Text.Split(" ");

            for (int i = 1; i < words.Length; i++)
            {
                text += words[i] + " ";
            }

            sender.Text("⌛ Начата рассылка сообщений...", msg.ChatId);
            int sended = 0;
            foreach (var userId in users)
            {
                Thread.Sleep(1000);
                try
                {
                    sender.Text($"📤 Рассылка:\n {text} \n ❓Чтобы отписаться от рассылки, напишите \"отписаться\"", userId);
                    sended++;
                }
                catch (Exception e)
                {
                    var bott = bot as Bot;
                    bott.GetLogger().Error("Ошибка при отправке сообщения пользователю..");
                } 
            }
            
            sender.Text($"✔ Рассылка завершена. Отправлено {sended} пользователям из {users.Count()}", msg.ChatId);
            

        }

        public void Init(IBot bot, ILoggerService logger)
        {
            
        }
    }
}