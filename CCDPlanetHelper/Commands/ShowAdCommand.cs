using System.Linq;
using CCDPlanetHelper.Database;
using Fooxboy.NucleusBot;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;

namespace CCDPlanetHelper.Commands
{
    public class ShowAdCommand:INucleusCommand
    {
        public string Command => "showad";
        public string[] Aliases => new string[0];
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            long ad = 0;
            if (msg.Text.Split("-")[0] == "id")
            {
                ad = long.Parse(msg.Text.Split("-")[1]);
            }
            else
            {
                ad = long.Parse(msg.Payload.Arguments[0]);
            }
            
            using (var db = new BotData())
            {
                var add = db.Ads.SingleOrDefault(a => a.AdId == ad);
                var text = $"🔍 Информация об объявлении" +
                           $"\n ▶ ID:{add.AdId}" +
                           $"\n ▶ Текст: {add.Text}" +
                           $"\n ▶ Страница автора: https://vk.com/id{add.Owner}" +
                           $"\n ▶ Сервер {add.Server}";


                var kb = new KeyboardBuilder(bot);
                kb.AddButton("🔙 В меню объявлений", "ads");


                if (msg.ChatId != msg.MessageVK.FromId)
                {
                    sender.Text(text, msg.ChatId);
                }
                else
                {
                    sender.Text(text, msg.ChatId, kb.Build());

                }
                
            }
        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}