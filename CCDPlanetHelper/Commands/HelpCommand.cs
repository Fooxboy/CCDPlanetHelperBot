using System.IO;
using System.Linq;
using CCDPlanetHelper.Models;
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
            
            //проверка и подписка на рассылку, если пользователь пользуется ботом первый раз.
            var usrs1 = JsonConvert.DeserializeObject<MailingModel>(File.ReadAllText("MailingUsers.json"));
            if (usrs1.Users.All(u => u.UserId != msg.MessageVK.FromId.Value))
            {
                usrs1.Users.Add(new ValuesMail()
                {
                    IsActive = true,
                    UserId = msg.MessageVK.FromId.Value
                });
                
                File.WriteAllText("MailingUsers.json", JsonConvert.SerializeObject(usrs1));
            }

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
                              "\n ▶ Бинды. Выводит список биндов" +
                              "\n ▶▶ Использование: Бинды \n" +
                              "\n ▶ Changelog. Выводит changelog" +
                              "\n ▶▶ Использование: changelog" +
                              "\n ▶ Отписка. Отписаться от рассылку уведомлений от администрации." +
                              "\n ▶▶ Использование: написать \"опписаться\"\n" +
                              "\n ▶ Объявления <Сервер> <страница> - показывает объявления с сервера <сервер> на странице <страница>" +
                              "\n ▶▶ Использование: Объявления 1 2\n" +
                              "\n ▶ Объявление id-<ID объявления> - показывает объявление по его ID\n" +
                              "\n ▶ CarInfo <ID автомобиля> - показывает информацию об авто по его ID\n" +
                              "\n ▶ Меню. Показывает меню (недоступно в беседах)\n" +
                              "\n ▶ Поиск <название авто> - поиск автомобиля по названию.\n" +
                              "\n ▶ Напомнинания  - выводит список ваших напоминаний" +
                              "\n ▶ Удалить напоминание <ID> - удаляет напоминание";
            
            
            var file = File.ReadAllText("AdminsConfig.json");
            var admins = JsonConvert.DeserializeObject<Models.AdminsModels>(file);

            var isAdmin = admins.Users.Any(u => u == msg.MessageVK.FromId);

            if (!isAdmin) sender.Text(helpText, msg.ChatId);
            else
            {
                var adminText = "\n🤴 Админ-команды: " +
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
                                "\n ▶ Установить changelog: changelogedit <текст>" +
                                "\n ▶ Список авто для отладки: carlist" +
                                "\n ▶ Добавить тег поиска: addtag <ID автомобиля>-<теги через запятую без проблелов>" +
                                "\n ▶ Создать тюнинг: addtuning <Цена> <Название>" +
                                "\n ▶ Админ меню: adminmenu " +
                                "\n ▶ Удалить тег поиска: removetag <текст тега>" +
                                "\n ▶ Удалить тюнинг пак: removetuning <ID тюнинг пака>" +
                                "\n ▶ Установить тюнинг на автомобиль: tuningset <ID автомобиля> <ID тюнинг пака>";

                var text = helpText + adminText;
                sender.Text(text, msg.ChatId);
            }
        }

        public void Init(IBot bot, ILoggerService logger)
        {
        }
    }
}