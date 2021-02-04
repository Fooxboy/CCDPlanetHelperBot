using System;
using System.Linq;
using System.Threading;
using CCDPlanetHelper.Database;
using CCDPlanetHelper.Services;
using Fooxboy.NucleusBot.Interfaces;
using VkNet.Model;
using Message = Fooxboy.NucleusBot.Models.Message;

namespace CCDPlanetHelper.Commands
{
    public class ReminderCommand:INucleusCommand
    {
        public string Command => "reminder";
        public string[] Aliases => new [] {"напомнить", "Напоминание", "напомни"};
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {
            var text = msg.Text;
            var wordsText = text.Split(" ");
            var date = wordsText[1];

            var wordsDate = date.Split(".");

            if (wordsDate.Length == 1)
            {
                sender.Text("⛔ Вы неверно указали дату. Пример: \n Напомни 25.1 поздравить Никиту с Днем рождения", msg.ChatId);
                return;
            }

            int day = 0;
            int mouth = 0;
            
            try
            {
                 day = int.Parse(wordsDate[0]);
                 mouth = int.Parse(wordsDate[1]);
            }
            catch (Exception e)
            {
                sender.Text("⛔ Вы неверно указали дату. Пример: \n Напомни 25.1 поздравить Никиту с Днем рождения", msg.ChatId);
                return;
            }
           

            if (day > 31|| day < 1)
            {
                sender.Text($"⛔ День не может быть больше 31 или меньше 1, вы указали - {day}", msg.ChatId);
                return;
            }

            if (mouth > 12|| mouth < 1)
            {
                sender.Text($"⛔ Месяц не может быть больше 12 или меньше 1, вы указали - {mouth}", msg.ChatId);
                return;
            }
            
            

            string textReminder = string.Empty;

            if (wordsText.Length < 3)
            {
                sender.Text("🎈 Вы не написали напоминание. Пример: \n Напомни 25.1 поздравить Никиту с Днем рождения",
                    msg.ChatId);
                return;
            }
            for (int i = 2; i < wordsText.Length; i++)
            {
                textReminder += wordsText[i] + " ";
            }
            
            using (var db = new BotData())
            {
                db.Reminders.Add(new ReminderInfo()
                {
                    Day = day,
                    Mouth = mouth,
                    ReminderId = db.Reminders.Count() + 1,
                    UserId = msg.ChatId,
                    Sent = false,
                    Text =  textReminder
                });

                db.SaveChanges();
            }
            sender.Text($"✔ Ваше упоминание установлено на {day}.{mouth}", msg.ChatId);
        }

        public void Init(IBot bot, ILoggerService logger)
        {
            
            
            var reminderSerivce = new ReminderService();

            var thread = new Thread(() =>
            {
                reminderSerivce.Start(logger, bot.SenderServices[0]);
            });
            
            logger.Trace($"Запуск потока ReminderServiceThread#{thread.ManagedThreadId}");
            thread.Start();
        }
    }
}