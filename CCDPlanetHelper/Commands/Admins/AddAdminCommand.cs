using System;
using System.IO;
using System.Linq;
using Fooxboy.NucleusBot;
using Fooxboy.NucleusBot.Interfaces;
using Newtonsoft.Json;
using VkNet;
using VkNet.Model;
using Message = Fooxboy.NucleusBot.Models.Message;

namespace CCDPlanetHelper.Commands.Admins
{
    public class AddAdminCommand:INucleusCommand
    {
        public string Command => "adminadd";
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

            if (words.Length == 1)
            {
                sender.Text("⛔ Вы не указали пользователя", msg.ChatId);
                return;
            }
            
            
            var adminText = words[1];

            long userId = 0;

            if (adminText[0] == '[')
            {
                var adminTextPartOne = adminText.Split("|")[0];
                var id = string.Empty;
                for (int i = 3; i < adminTextPartOne.Length; i++)
                {
                    id += adminTextPartOne[i];
                }

                userId = long.Parse(id);
            }else if (adminText[0] == 'h')
            {
                var wordsAdmin = adminText.Split("/");

                var url = wordsAdmin[^1];

                var vkNet = new VkApi();
                vkNet.Authorize(new ApiAuthParams()
                {
                    AccessToken = "e7980081cccad8d0df1ce342355da76e6e0d8de37509d76fb08ff1f065823dc19e5f4ed7f4578314cc772"
                });

                userId = vkNet.Utils.ResolveScreenName(url).Id.Value;
                
            }else if (adminText[0] == 'v')
            {
                var wordsAdmin = adminText.Split("/");

                var url = wordsAdmin[^1];

                var vkNet = new VkApi();
                vkNet.Authorize(new ApiAuthParams()
                {
                    AccessToken = "e7980081cccad8d0df1ce342355da76e6e0d8de37509d76fb08ff1f065823dc19e5f4ed7f4578314cc772"
                });

                userId = vkNet.Utils.ResolveScreenName(url).Id.Value;
            }
            else
            {
                try
                {
                    userId = long.Parse(adminText);
                }
                catch (Exception e)
                {
                    sender.Text("⛔ Неверный формат Id  пользователя. Попробуйте ещё раз", msg.ChatId);
                    return;
                }
            }
            
            admins.Users.Add(userId);

            var text = JsonConvert.SerializeObject(admins);
            File.WriteAllText("AdminsConfig.json", text);
            sender.Text("✔ Новый администратор добавлен", msg.ChatId);
            logger.War($"ДОБАВЛЕН НОВЫЙ АДМИНИСТРАТОР С ID: {userId}");
        }

        public void Init(IBot bot, ILoggerService logger)
        {

        }
    }
}