using System.Collections.Generic;
using System.Linq;
using CCDPlanetHelper.Database;
using Fooxboy.NucleusBot;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;

namespace CCDPlanetHelper.Commands
{
    public class SearchCommand:INucleusCommand
    {
        public string Command => "search";
        public string[] Aliases => new[] {"поиск", "найти"};
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            var kb = new KeyboardBuilder(bot);
            kb.AddButton("🔙 Назад", "searchmenu");
            
            StaticContent.UsersCommand.Add(msg.ChatId, "search");
            sender.Text("🔍 Напишите название автомобиля", msg.ChatId, kb.Build());
        }

        public static void Search(Message msg, IMessageSenderService sender, IBot bot)
        {
            StaticContent.UsersCommand.Remove(msg.MessageVK.FromId.Value);
            var search = msg.Text;

            using (var db = new BotData())
            {
                var car = db.Cars.SingleOrDefault(c => c.Model.ToLower() == search.ToLower());

                if (car is null)
                {
                    StaticContent.UsersCommand.Add(msg.ChatId, "search");
                    var kb = new KeyboardBuilder(bot);
                    kb.AddButton("🔙 Назад", "searchmenu");
                    sender.Text("🔍 Автомобиль не найден. Попробуйте ещё раз", msg.ChatId, kb.Build());
                    return;
                }

                var kb1 = new KeyboardBuilder(bot);
                kb1.AddButton("🚗 Открыть информацию", "carinfo", new List<string>() {car.CarId.ToString()});
                kb1.AddLine();
                kb1.AddButton("🔙 Назад", "searchmenu");
                kb1.AddLine();
                kb1.AddButton("🔍 Найти другой", "search");
                
                sender.Text($"🚗 {car.Model} найден! Нажмите на кнопку ниже, чтобы открыть информацию об авто.", msg.ChatId, kb1.Build());

            }
        }

        public void Init(IBot bot, ILoggerService logger)
        {
            
        }
    }
}