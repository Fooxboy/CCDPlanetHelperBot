using System;
using System.Globalization;
using System.Linq;
using CCDPlanetHelper.Database;
using Fooxboy.NucleusBot;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using VkNet.Enums.SafetyEnums;

namespace CCDPlanetHelper.Commands
{
    public class TargetEditCommand:INucleusCommand
    {
        public string Command => "targetedit";
        public string[] Aliases => new string[0];
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            StaticContent.UsersCommand.Add(msg.ChatId, "target");
            sender.Text("❓ Укажите новую сумму:", msg.ChatId);
        }


        public static void Edited(Message msg, IMessageSenderService sender, IBot bot)
        {
            using (var db = new BotData())
            {
                var target = db.Targets.SingleOrDefault(t=> t.UserId == msg.ChatId);
                long count = 0;

                try
                {
                    count = long.Parse(msg.Text);
                }
                catch (Exception e)
                {
                    StaticContent.UsersCommand.Add(msg.ChatId, "target");
                    var kb = new KeyboardBuilder(bot);
                    kb.AddButton("🛑 Отмена", "target", color: KeyboardButtonColor.Negative);
                    sender.Text("⛔ Вы указали неверное число, попробуйте ещё раз", msg.ChatId, kb.Build());
                    return;
                }

                target.Count = count;
                var kb1 = new KeyboardBuilder(bot);
                kb1.AddButton("🔙 Назад", "target", color: KeyboardButtonColor.Positive);
                db.SaveChanges();
                sender.Text("✔ Данные были обновлены", msg.ChatId, kb1.Build());
            }
        }
        public void Init(IBot bot, ILoggerService logger)
        {
            
        }
    }
}