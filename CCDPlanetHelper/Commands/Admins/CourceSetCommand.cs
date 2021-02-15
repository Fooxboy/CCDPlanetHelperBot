using System;
using System.IO;
using System.Linq;
using Fooxboy.NucleusBot;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Commands.Admins
{
    public class CourceSetCommand:INucleusCommand
    {
        public string Command => "courceset";
        public string[] Aliases => new string[0];
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            var bott = bot as Bot;
            var logger = bott.GetLogger();
            var file = File.ReadAllText("AdminsConfig.json");
            var admins = JsonConvert.DeserializeObject<Models.AdminsModels>(file);

            if (!(admins.Users.Any(u => u == msg.MessageVK.FromId)))
            {
                sender.Text("🛑 У вас нет прав администратора для вызова этой команды.", msg.ChatId);
                return;
            }

            var words = msg.Text.Split(" ");

            try
            {
                var dollar = float.Parse(words[1]);
                var euro = float.Parse(words[2]);
                
                var cource = new Models.CourseModel();
                cource.Dollar = dollar;
                cource.Euro = euro;
                var text = JsonConvert.SerializeObject(cource);
                File.WriteAllText("CourceConfig.json", text);
                sender.Text($"✔ Новый курс установлен. Доллар - {dollar}, Евро - {euro}", msg.ChatId);
                return;
            }
            catch (Exception e)
            {
             logger.Error(e.Message);   
                sender.Text(
                    "⛔ Произошла ошибка. Возможно, вы ввели данные не в том формате. Пример: \n courceset 10 20 \n Где 10 - доллар, 20 - евро.",
                    msg.ChatId);
                return;
            }
            
            
            
        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}