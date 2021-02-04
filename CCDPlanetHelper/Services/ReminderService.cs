using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CCDPlanetHelper.Database;
using Fooxboy.NucleusBot.Interfaces;

namespace CCDPlanetHelper.Services
{
    public class ReminderService
    {
        public void Start(ILoggerService logger, IMessageSenderService sender)
        {
            logger.Trace("Запуск сервиса ReminderService...");

            while (true)
            {
                var time = DateTime.Now;
                var day = time.Day;
                var mouth = time.Month;
                using (var db = new BotData())
                {
                    logger.Trace("Проверка неотправленных напоминаний...");
                    var reminders = db.Reminders.Where(r => r.Day == day && r.Mouth == mouth && r.Sent == false);
                    var list = reminders.ToList();
                    if (list.Count != 0)
                    {
                        logger.Trace($"Начата рассылка {list.Count} напоминаний...");
                        foreach (var reminder in list)
                        {
                            try
                            {
                                sender.Text($"⌛ Ваше напоминание: \n {reminder.Text}", reminder.UserId);

                                var reminderDb = db.Reminders.Single(r => r.ReminderId == reminder.ReminderId);
                                reminderDb.Sent = true;
                                db.SaveChanges();
                                Thread.Sleep(1000);
                            }
                            catch (Exception e)
                            {
                                logger.Error($"Бот не смог отправить напоминание пользователю с Id {reminder.UserId} \n EXCEPTION: \n {e.Message}");
                            }
                           
                        }
                    }
                }
                
                Thread.Sleep(18000000);
            }
        }
    }
}