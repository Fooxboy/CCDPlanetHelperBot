using System.IO;
using System.Linq;
using Fooxboy.NucleusBot.Interfaces;
using Fooxboy.NucleusBot.Models;
using Newtonsoft.Json;

namespace CCDPlanetHelper.Commands
{
    public class HelpCommand:INucleusCommand
    {
        public string Command => "help";
        public string[] Aliases => new[] {"помощь", "помоги", "команды", "возможности"};
        public void Execute(Message msg, IMessageSenderService sender, IBot bot)
        {

            string helpText = "❓Список всех доступных команд:" +
                              "\n ▶ Объявления. Показывает список актуальных объявлений пользователей." +
                              "\n ▶▶ Использование: написать \"Объявления\" \n" +
                              "\n ▶ Добавить объявление. Добавляет новое объявление в список объявлений." +
                              "\n ▶▶ Использование: написать \"Добавить объявление\" \n" +
                              "\n ▶ Калькулятор. Считает пример, который вы напишете." +
                              "\n ▶▶ Использование: написать \"Посчитай <пример>\"" +
                              "\n ▶▶ Пример: посчитай 10*5\n" +
                              "\n ▶ Курс валют. Пишет вам актуальный курс валют" +
                              "\n ▶▶ Использование: написать \"курс\"\n" +
                              "\n ▶ Напоминания. Бот вам напомнит о нужном событии, которые вы напишете." +
                              "\n ▶▶ Использование: написать \"напомни <день>.<месяц> <текст напомнинания>\"" +
                              "\n ▶▶ Пример: напомни 25.1 поздравить моего брата с Днем рождения.\n" +
                              "\n ▶ Репорт. Сообщение, которое получит администратор и сможет ответить на него." +
                              "\n ▶▶ Использование: написать \"репорт <сообщение>\"" +
                              "\n ▶▶ Пример: репорт я не могу найти список всех команд\n" +
                              "\n ▶ Подписка. Подписаться на рассылку уведомлений от администрации." +
                              "\n ▶▶ Использование: написать \"подписаться\"\n" +
                              "\n ▶ Бинды. ?????" +
                              "\n ▶▶ Использование: Бинды \n" +
                              "\n ▶ Changelog. " +
                              "\n ▶▶ Использование: changelog" +
                              "\n ▶ Отписка. Отписаться от рассылку уведомлений от администрации." +
                              "\n ▶▶ Использование: написать \"опписаться\"\n\n";
            
            
            var file = File.ReadAllText("AdminsConfig.json");
            var admins = JsonConvert.DeserializeObject<Models.AdminsModels>(file);

            var isAdmin = admins.Users.Any(u => u == msg.MessageVK.FromId);

            if (!isAdmin) sender.Text(helpText, msg.ChatId);
            else
            {
                var adminText = "🤴 Админ-команды: " +
                                "\n ▶ Добавить администратора: adminadd <id или упоминание>" +
                                "\n ▶ Удалить администратора: adminremove <id или упоминание>" +
                                "\n ▶ Добавить автомобиль: addcar <модель>" +
                                "\n ▶ Установка курса валют: courceset <доллар> <евро>" +
                                "\n ▶ Список администраторов: adminlist" +
                                "\n ▶ Рассылка информационных сообщений: рассылка <сообщение>" +
                                "\n ▶ Удалить объявление: removead <ID объявления>" +
                                "\n ▶ Удалить автомобиль: removecar <ID автомобиля>" +
                                "\n ▶ Список неотвеченных репортов: репорты" +
                                "\n ▶ Ответить на репорт: reportrp <ID репорта> <сообщение>" +
                                "\n ▶ Установить бинды: bindsedit <текст>" +
                                "\n ▶ Установить changelog: changelogedit <текст>";

                var text = helpText + adminText;
                sender.Text(text, msg.ChatId);
            }
        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}